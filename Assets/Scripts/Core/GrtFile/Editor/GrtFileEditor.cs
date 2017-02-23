using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Utility;

namespace Groot.Res
{
	public class GrtFileEditor
	{
		public static void Pack( String _src_path, String _output_root, String _name )
		{
			String output = String.Concat( _output_root, "/", _name );
			if( !Directory.Exists( _src_path ) )
			{
				Log.Error( "[GrtFielEditor]: 打包目录: {0} 不存在！", _src_path );
				return;
			}
			try
			{
				DirectoryInfo di = new DirectoryInfo( _src_path );
				var files = FileHelper.GetAllFileInfo( _src_path );
				List <GrtFile > pack_files = new List<GrtFile>();
				foreach( FileInfo file_info in files )
				{
					GrtFile grt_file = new GrtFile();
					grt_file.Bytes = FileHelper.ReadAllBytesFromFile( file_info.FullName );
					grt_file.Size = grt_file.Bytes.Length;
					grt_file.Name = FileHelper.GetRelativePath( file_info.FullName, di.FullName );
					pack_files.Add( grt_file );
				}

				FileHelper.CreateDirectoryByFilePath( output );
				using( FileStream fs = new FileStream( output, FileMode.Create, FileAccess.Write ) )
				{
					Int64 offset = 0;
					// 压缩，写入文件
					foreach( GrtFile pack_file_info in pack_files )
					{
						Byte[] compressed_bytes;
						pack_file_info.Offset = offset;
						_compress( pack_file_info.Bytes, out compressed_bytes, out pack_file_info.CompressedSize );
						fs.Write( compressed_bytes, 0, (Int32)pack_file_info.CompressedSize );
						offset += pack_file_info.CompressedSize;
					}

					//offset = fs.Length; // 描述段开始的位置

					Byte[] bytes;
					// 写入文件列表
					foreach( GrtFile pack_file_info in pack_files )
					{
						// 压缩前的size
						bytes = BitConverter.GetBytes( pack_file_info.Size );
						if( !BitConverter.IsLittleEndian )
							Array.Reverse( bytes );
						fs.Write( bytes, 0, bytes.Length );

						// 压缩后的size
						bytes = BitConverter.GetBytes( pack_file_info.CompressedSize );
						if( !BitConverter.IsLittleEndian )
							Array.Reverse( bytes );
						fs.Write( bytes, 0, bytes.Length );

						// 写入偏移值
						bytes = BitConverter.GetBytes( pack_file_info.Offset );
						if( !BitConverter.IsLittleEndian )
							Array.Reverse( bytes );
						fs.Write( bytes, 0, bytes.Length );

						// 名字
						bytes = System.Text.Encoding.UTF8.GetBytes( pack_file_info.Name );

						if( bytes.Length > 255 )
							throw new Exception( "文件名称超过255位，不能打包!" );
						fs.WriteByte( (Byte)bytes.Length ); // 名字长度最大不超过256个字节
						fs.Write( bytes, 0, bytes.Length );
					}

					bytes = BitConverter.GetBytes( offset );
					if( !BitConverter.IsLittleEndian )
						Array.Reverse( bytes );

					fs.Write( bytes, 0, bytes.Length );
					fs.Flush();

					StringBuilder sb = new StringBuilder( 1024 );
					sb.AppendFormat( "GrtPackage打包成功： {0}\n", _name );
					foreach( GrtFile pack_file_info in pack_files )
						sb.AppendLine( pack_file_info.Name );
					//Debug.Log( sb );
				}
			}
			catch( Exception e )
			{
				Debug.LogError( e );
			}

		}

		/// <summary>
		/// 压缩一段字节
		/// </summary>
		/// <param name="_in">源数据</param>
		/// <param name="_out">压缩后的数据，注意这里的数据长度通常大于压缩资源本身</param>
		/// <param name="_length">压缩后的实际字节大小, 通常来说 _length 小于 _out.Length </param>
		private static Boolean _compress( Byte[] _in, out Byte[] _out, out Int64 _length )
		{
			try
			{
				SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
				using( MemoryStream out_stream = new MemoryStream() )
				{
					using( MemoryStream in_stream = new MemoryStream( _in ) )
					{
						encoder.WriteCoderProperties( out_stream );
						// 写入源文件长度
						// out_stream.Write( System.BitConverter.GetBytes( _in.Length ), 0, 8 );
						encoder.Code( in_stream, out_stream, in_stream.Length, -1, null );
					}
					_out = out_stream.GetBuffer();
					_length = out_stream.Length;
				}
			}
			catch( Exception ex )
			{
				_out = null;
				_length = 0;
				Debug.Log( ex );
				return false;
			}
			return true;
		}
	}
}
