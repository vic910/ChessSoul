using System;
using System.IO;
using System.Linq;
using System.Text;


namespace Groot.Network
{
	public class SerializeWriter
	{
		MemoryStream m_stream = new MemoryStream( 1024 );

		public byte[] Bytes
		{
			get
			{
				return m_stream.ToArray();
			}
		}
		public void Reset()
		{
			m_stream.Seek( 0, SeekOrigin.Begin );
			m_stream.SetLength(0);
		}
		public Int32 Write( bool _value )
		{
			m_stream.WriteByte( (byte)( _value ? 1 : 0 ) );
			return 1;
		}
		public Int32 WriteZero( Int32 _count )
		{
			// is these a performence issue? 
			for( Int32 i = 0; i < _count; ++i )
				m_stream.WriteByte(0);
			return _count;
		}
		public Int32 Write( byte _value )
		{
			m_stream.WriteByte( _value );
			return 1;
		}
		public Int32 Write( sbyte _value )
		{
			m_stream.WriteByte( (byte)_value );
			return 1;
		}

		public Int32 Write( short _value )
		{
			var bytes = BitConverter.GetBytes( _value );
			if( !BitConverter.IsLittleEndian )
				bytes.Reverse();
			m_stream.Write( bytes, 0, 2 );
			return 2;
		}
		public Int32 Write( ushort _value )
		{
			var bytes = BitConverter.GetBytes( _value );
			if( !BitConverter.IsLittleEndian )
				bytes.Reverse();
			m_stream.Write( bytes, 0, 2 );
			return 2;
		}

		public Int32 Write( int _value )
		{
			var bytes = BitConverter.GetBytes( _value );
			if( !BitConverter.IsLittleEndian )
				bytes.Reverse();
			m_stream.Write( bytes, 0, 4 );
			return 4;
		}
		public Int32 Write( uint _value )
		{
			var bytes = BitConverter.GetBytes( _value );
			if( !BitConverter.IsLittleEndian )
				bytes.Reverse();
			m_stream.Write( bytes, 0, 4 );
			return 4;
		}
		public Int32 Write( Single _value )
		{
			var bytes = BitConverter.GetBytes( _value );
			if( !BitConverter.IsLittleEndian )
				bytes.Reverse();
			m_stream.Write( bytes, 0, 4 );
			return 4;
		}

		public Int32 Write( Double _value )
		{
			var bytes = BitConverter.GetBytes( _value );
			if( !BitConverter.IsLittleEndian )
				bytes.Reverse();
			m_stream.Write( bytes, 0, 8 );
			return 8;
		}
		public Int32 Write( Int64 _value )
		{
			var bytes = BitConverter.GetBytes( _value );
			if( !BitConverter.IsLittleEndian )
				bytes.Reverse();
			m_stream.Write( bytes, 0, 8 );
			return 8;
		}
		public Int32 Write( UInt64 _value )
		{
			var bytes = BitConverter.GetBytes( _value );
			if( !BitConverter.IsLittleEndian )
				bytes.Reverse();
			m_stream.Write( bytes, 0, 8 );
			return 8;
		}
		public Int32 Write( Byte[] _bytes )
		{
			m_stream.Write( _bytes, 0, _bytes.Length );
			return _bytes.Length;
		}

		public Int32 Write( String _value, int _length )
		{
			if( _value == null )
				_value = String.Empty;

			//var bytes = System.Text.Encoding.UTF8.GetBytes( _value );
			var bytes = Encoding.GetEncoding( "GB2312" ).GetBytes( _value );

			if ( bytes.Length > _length )
			{
				Utility.Log.Error( "字符串: {0} 序列化警告，超过长度，要求长度: {1} 实际长度: {2}超过部分将被裁剪",
					_value, _length, bytes.Length );
				m_stream.Write( bytes, 0, _length - 1 );
				m_stream.WriteByte( 0 );
			}
			else
			{
				m_stream.Write( bytes, 0, bytes.Length );
				WriteZero( _length - bytes.Length );
			}
			return _length;
		}
	}
}
