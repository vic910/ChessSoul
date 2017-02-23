using System;
using System.IO;
using System.Linq;
using System.Text;


namespace Groot.Network
{

	public class SerializeReader
	{
		MemoryStream m_stream;

		public void Close()
		{
			if( m_stream != null )
			{
				m_stream.Close();
				m_stream = null;
			}
		}
		/// <summary>
		/// 原生类型的buffer
		/// </summary>
		byte[] m_8bytes_buffer = new byte[8];
		byte[] m_4bytes_buffer = new byte[4];
		byte[] m_2bytes_buffer = new byte[2];
		public SerializeReader( Byte[] _bytes )
		{
			this.m_stream = new MemoryStream( _bytes, false );
		}

		void Read8ToBuffer( int offset )
		{
			try
			{
				m_stream.Read( m_8bytes_buffer, offset, 8 );
				//传输数据为小端
				if( !BitConverter.IsLittleEndian )
					m_8bytes_buffer.Reverse();
			}
			catch( Exception e )
			{
				Utility.Log.Exception( e.Message );
				throw e;
			}
		}
		void Read4ToBuffer( int offset )
		{
			try
			{
				m_stream.Read( m_4bytes_buffer, offset, 4 );
				//传输数据为小端
				if( !BitConverter.IsLittleEndian )
					m_4bytes_buffer.Reverse();
			}
			catch( Exception e )
			{
				Utility.Log.Exception( e.Message );
				throw e;
			}
		}
		void Read2ToBuffer( int offset )
		{
			try
			{
				m_stream.Read( m_2bytes_buffer, offset, 2 );
				//传输数据为小端
				if( !BitConverter.IsLittleEndian )
					m_2bytes_buffer.Reverse();
			}
			catch( Exception e )
			{
				Utility.Log.Exception( e.Message );
				throw e;
			}
		}

		public bool ReadBool()
		{
			return ( m_stream.ReadByte() == 1 ) ? true : false;
		}
		public sbyte ReadSByte()
		{
			return (SByte)m_stream.ReadByte();
		}

		public byte ReadByte()
		{
			return (Byte)m_stream.ReadByte();
		}
		public byte[] ReadBytes( int len )
		{
			byte[] bytes = new byte[len];
			for( int i=0; i < len; ++i )
			{
				bytes[i] = (byte)m_stream.ReadByte();
			}
			return bytes;
		}
		public sbyte[] ReadSBytes( int len )
		{
			sbyte[] sbytes = new sbyte[len];
			for( int i=0; i < len; ++i )
			{
				sbytes[i] = (sbyte)m_stream.ReadByte();
			}
			return sbytes;
		}

		public short ReadInt16()
		{
			Read2ToBuffer( 0 );
			return BitConverter.ToInt16( m_2bytes_buffer, 0 );
		}
		public ushort ReadUInt16()
		{
			Read2ToBuffer( 0 );
			return BitConverter.ToUInt16( m_2bytes_buffer, 0 );
		}

		public int ReadInt32()
		{
			Read4ToBuffer( 0 );
			return BitConverter.ToInt32( m_4bytes_buffer, 0 );
		}
		public uint ReadUInt32()
		{
			Read4ToBuffer( 0 );
			return BitConverter.ToUInt32( m_4bytes_buffer, 0 );
		}
		public float ReadSingle()
		{
			Read4ToBuffer( 0 );
			return BitConverter.ToSingle( m_4bytes_buffer, 0 );
		}

		public double ReadDouble()
		{
			Read8ToBuffer( 0 );
			return BitConverter.ToDouble( m_8bytes_buffer, 0 );
		}
		public long ReadInt64()
		{
			Read8ToBuffer( 0 );
			return BitConverter.ToInt64( m_8bytes_buffer, 0 );
		}
		public ulong ReadUInt64()
		{
			Read8ToBuffer( 0 );
			return BitConverter.ToUInt64( m_8bytes_buffer, 0 );
		}

		public string ReadString( int length )
		{
			if( length > ( m_stream.Length - m_stream.Position ) )
				length = (int)( m_stream.Length - m_stream.Position ); // 这里字符数不可能超过int.MaxValue
			byte[] bytes = new byte[length];
			m_stream.Read( bytes, 0, length );
			Int32 zero_idx = 0;
			for( Int32 i = 0; i < length; ++i )
			{
				if( bytes[i] == 0 )
				{
					zero_idx = i;
					break;
				}
			}
			if (zero_idx == 0)
				return string.Empty;
			else
				//return System.Text.Encoding.UTF8.GetString( bytes, 0, zero_idx == length - 1 ? length : zero_idx );
				return System.Text.Encoding.GetEncoding( "GB2312" ).GetString( bytes, 0, zero_idx == length - 1 ? length : zero_idx );
		}
	}
}
