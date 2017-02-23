using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using UnityEngine;
using Utility;

namespace Groot.Res
{
	public class GrtFile
	{
		/// <summary>
		/// 原始大小
		/// </summary>
		public Int64 Size;

		/// <summary>
		/// 压缩后的大小
		/// </summary>
		public Int64 CompressedSize;

		/// <summary>
		/// package中的偏移值
		/// </summary>
		public Int64 Offset;

		/// <summary>
		/// 解压后的流
		/// </summary>
		public Byte[] Bytes;

		/// <summary>
		/// 名称
		/// </summary>
		public String Name;
	}

	/*public class StringCompare : IEqualityComparer<String>
	{
		public static readonly StringCompare DefaultInstance = new StringCompare();

		public Boolean Equals( String x, String y )
		{
			return x.Equals( y, StringComparison.Ordinal );
		}

		public Int32 GetHashCode( String obj )
		{
			return obj.GetHashCode();
		}
	}*/

	public class GrtPackge
	{
		private GrtPackge() { }

		private Dictionary<String, GrtFile>  m_files = new Dictionary<String, GrtFile>( StringComparer.Ordinal );
		private String m_package_path;
		private Byte[] m_bytes;

		private static readonly Byte[] s_buffer = new Byte[256];


		public static GrtPackge CreateFromeMemory( Byte[] _bytes )
		{
			try
			{
				GrtPackge package;
				using( MemoryStream ms = new MemoryStream( _bytes ) )
				{
					package = _create( ms );
				}
				package.m_package_path = null;
				package.m_bytes = _bytes;
				return package;
			}
			catch( Exception e )
			{
				Debug.LogError( e );
			}
			return null;
		}

		/// <summary>
		/// 创建Package, 并读取文件列表(但不加载任何文件)
		/// </summary>
		/// <param name="_package_path">路径</param>
		/// <returns></returns>
		public static GrtPackge CreateFromFile( String _package_path )
		{
			try
			{
				GrtPackge package;
				using( FileStream fs = new FileStream( _package_path, FileMode.Open ) )
				{
					package = _create( fs );
				}
				package.m_package_path = _package_path;
				return package;
			}
			catch( FileNotFoundException )
			{
				Debug.LogErrorFormat( "Can't create GrtPackage, file not exist: [{0}]", _package_path );
			}
			catch( Exception e )
			{
				Debug.LogError( e );
			}
			return null;
		}

		private static GrtPackge _create( Stream _stream )
		{
			lock( s_buffer )
			{
				GrtPackge package = new GrtPackge();
				_stream.Seek( -8, SeekOrigin.End );
				_stream.Read( s_buffer, 0, 8 );
				Int64 start_idx = BitConverter.ToInt64( s_buffer, 0 );

				_stream.Seek( start_idx, SeekOrigin.Begin );

				// 读取描述段
				while( _stream.Position < _stream.Length - 8 )
				{
					GrtFile grt_file = new GrtFile();
					_stream.Read( s_buffer, 0, 8 );
					grt_file.Size = BitConverter.ToInt64( s_buffer, 0 );

					_stream.Read( s_buffer, 0, 8 );
					grt_file.CompressedSize = BitConverter.ToInt64( s_buffer, 0 );

					_stream.Read( s_buffer, 0, 8 );
					grt_file.Offset = BitConverter.ToInt64( s_buffer, 0 );

					Byte file_name_length = (Byte)_stream.ReadByte();
					_stream.Read( s_buffer, 0, file_name_length );
					grt_file.Name = System.Text.Encoding.UTF8.GetString( s_buffer, 0, file_name_length );

					package.m_files.Add( grt_file.Name, grt_file );
					Debug.LogFormat( "name: {0}, size: {1} compressed_size: {2}, offset: {3}", grt_file.Name, grt_file.Size, grt_file.CompressedSize, grt_file.Offset );
				}
				return package;
			}
		}

		/// <summary>
		/// 加载一个文件，并且缓存
		/// </summary>
		/// <param name="_name"></param>
		/// <returns></returns>
		public Byte[] LoadFile( String _name )
		{
			try
			{
				Int32 count = m_files.Count;
				GrtFile grt_file = null;
				if( !m_files.TryGetValue( _name, out grt_file ) )
				{
					Debug.LogErrorFormat( "[GrtPackage.LoadFile]: {0} not in this package!", _name );
					return null;
				}

				if( grt_file.Bytes != null )
					return grt_file.Bytes;

				// 如果路径为空，表示需要从内存读取
				if( m_package_path == null )
				{
					using( MemoryStream ms = new MemoryStream( m_bytes ) )
					{
						ms.Seek( grt_file.Offset, SeekOrigin.Begin );
						_decompress( ms, grt_file.CompressedSize, grt_file.Size, out grt_file.Bytes );
					}
				}
				else
				{
					using( FileStream fs = new FileStream( m_package_path, FileMode.Open ) )
					{
						fs.Seek( grt_file.Offset, SeekOrigin.Begin );
						_decompress( fs, grt_file.CompressedSize, grt_file.Size, out grt_file.Bytes );
					}
				}
				return grt_file.Bytes;
			}
			catch( FileNotFoundException )
			{
				Debug.LogErrorFormat( "[GrtPackage.LoadFile]: Can't Load file from GrtPackage, package: {0} no longer exist!", m_package_path );
			}
			catch( Exception e )
			{
				Debug.LogErrorFormat( "[GrtPackage.LoadFile]: " + e );
			}
			return null;
		}

		public void UnloadFile( String _name )
		{
			GrtFile grt_file = null;
			if( !m_files.TryGetValue( _name, out grt_file ) )
				return;
			grt_file.Bytes = null;
		}

#if UNITY_EDITOR
		public void CompressTest( String _output )
		{
			try
			{
				foreach( GrtFile grt_file in m_files.Values )
				{
					Byte[] bytes = LoadFile( grt_file.Name );
					String output_file = _output + grt_file.Name;
					FileHelper.CreateDirectoryByFilePath( output_file );
					using( FileStream output_fs = new FileStream( output_file, FileMode.Create ) )
					{
						output_fs.Write( bytes, 0, bytes.Length );
					}
				}
			}
			catch( Exception e )
			{
				Debug.LogErrorFormat( "[GrtPackage.CompressTest]: " + e );
			}
		}
#endif


		private const Int32 PROPERTIES_SIZE = SevenZip.Compression.LZMA.Encoder.kPropSize;
		private static readonly Byte[] s_properties_buffer = new Byte[PROPERTIES_SIZE];
		private static Boolean _decompress( Stream _in, Int64 _in_size, Int64 _out_size, out Byte[] _out )
		{
			lock( s_properties_buffer )
			{
				try
				{
					_out = new Byte[_out_size];
					using( MemoryStream out_put = new MemoryStream( _out ) )
					{
						// 读取压缩属性
						Int32 readed = 0;
						while( readed < PROPERTIES_SIZE )
						{
							readed += _in.Read( s_properties_buffer, readed, PROPERTIES_SIZE - readed );
						}

						SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
						decoder.SetDecoderProperties( s_properties_buffer );
						decoder.Code( _in, out_put, _in_size, _out_size, null );
						if( out_put.Length != _out_size )
							Debug.LogFormat( "解压缩后大小和和压缩前大小不一致!!" );

						if( out_put.Capacity != out_put.Length )
							Debug.LogFormat( "解压缩缓存大小不够用!!" );
					}
				}
				catch( Exception e )
				{
					_out = null;
					Debug.Log( e );
					return false;
				}
				return true;
			}
		}
	}
}