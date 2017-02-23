using System;
using System.Text;

namespace Utility.Variant
{
	public enum eVariantType : byte
	{
		Empty = 0,  //完全的空类型，变量未初始化
		Int8,       //int8
		UInt8,      //Uint8
		Int16,      //Int16
		UInt16,     //UInt16
		Int32,      //Int32
		UInt32,     //UInt32
		Int64,      //Int64
		UInt64,     //UInt64
		Single,     //单精度浮点 对应float
		Double,     //双精度浮点 对应double
		Boolean,    //bool
		Class,      //类

		String,     //标准字符串
	}

	public class Variant
	{
		private Variant() { Type = eVariantType.Empty; }

		private Variant( Byte _byte )
		{
			Type = eVariantType.UInt8;
			m_object = _byte;
		}
		private Variant( SByte _sbyte )
		{
			Type = eVariantType.Int8;
			m_object = _sbyte;
		}
		private Variant( Int16 _v )
		{
			Type = eVariantType.Int16;
			m_object = _v;
		}
		private Variant( UInt16 _v )
		{
			Type = eVariantType.UInt16;
			m_object = _v;
		}
		private Variant( Int32 _v )
		{
			Type = eVariantType.Int32;
			m_object = _v;
		}
		private Variant( UInt32 _v )
		{
			Type = eVariantType.UInt32;
			m_object = _v;
		}
		private Variant( UInt64 _v )
		{
			Type = eVariantType.UInt64;
			m_object = _v;
		}
		private Variant( Int64 _v )
		{
			Type = eVariantType.Int64;
			m_object = _v;
		}

		private Variant( Single _v )
		{
			Type = eVariantType.Single;
			m_object = _v;
		}
		private Variant( Double _v )
		{
			Type = eVariantType.Double;
			m_object = _v;
		}
		private Variant( Boolean _v )
		{
			Type = eVariantType.Boolean;
			m_object = _v;
		}
		private Variant( String _v )
		{
			Type = eVariantType.String;
			m_object = _v;
		}
		private Variant( Object _v )
		{
			Type = eVariantType.Class;
			m_object = _v;
		}
		public static implicit operator Variant( SByte _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}

		public static implicit operator Variant( Byte _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}
		public static implicit operator Variant( Int16 _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}
		public static implicit operator Variant( UInt16 _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}
		public static implicit operator Variant( Int32 _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}
		public static implicit operator Variant( UInt32 _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}
		public static implicit operator Variant( Int64 _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}
		public static implicit operator Variant( UInt64 _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}
		public static implicit operator Variant( Single _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}
		public static implicit operator Variant( Double _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}
		public static implicit operator Variant( String _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}
		public static implicit operator Variant( Boolean _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}
		public static Variant FromCustom<T>( T _v )
		{
			Variant varinat = new Variant( _v );
			return varinat;
		}

		// Operators

		public static implicit operator SByte( Variant _v )
		{
			if( _v.Type != eVariantType.Int8 )
				throw new ExceptionEx( "Variant is {0} not required Int8", _v.Type );
			return Convert.ToSByte(_v.m_object);
		}

		public static implicit operator Byte( Variant _v )
		{
			if( _v.Type != eVariantType.UInt8 )
				throw new ExceptionEx( "Variant is {0} not required UInt8", _v.Type );
			return Convert.ToByte(_v.m_object);
		}

		public static implicit operator Int16( Variant _v )
		{
			if( _v.Type != eVariantType.Int16
				&& _v.Type != eVariantType.Int8
				&& _v.Type != eVariantType.UInt8 )
				throw new ExceptionEx( "Variant is {0} not required Int16", _v.Type );
			return Convert.ToInt16(_v.m_object);
		}
		public static implicit operator UInt16( Variant _v )
		{
			if( _v.Type != eVariantType.UInt16
				&& _v.Type != eVariantType.UInt8 )
				throw new ExceptionEx( "Variant is {0} not required UInt16", _v.Type );
			return Convert.ToUInt16(_v.m_object);
		}
		public static implicit operator Int32( Variant _v )
		{
			if( _v.Type != eVariantType.Int32
				&& _v.Type != eVariantType.Int16
				&& _v.Type != eVariantType.UInt16
				&& _v.Type != eVariantType.Int8
				&& _v.Type != eVariantType.UInt8
				)
				throw new ExceptionEx( "Variant is {0} not required Int32", _v.Type );
			return Convert.ToInt32(_v.m_object);
		}
		public static implicit operator UInt32( Variant _v )
		{
			if( _v.Type != eVariantType.UInt32
				&& _v.Type != eVariantType.UInt16
				&& _v.Type != eVariantType.UInt8
				)
				throw new ExceptionEx( "Variant is {0} not required UInt32", _v.Type );
			return Convert.ToUInt32(_v.m_object);
		}

		public static implicit operator Int64( Variant _v )
		{
			if( _v.Type != eVariantType.Int64
				&& _v.Type != eVariantType.UInt32
				&& _v.Type != eVariantType.Int32
				&& _v.Type != eVariantType.Int16
				&& _v.Type != eVariantType.UInt16
				&& _v.Type != eVariantType.Int8
				&& _v.Type != eVariantType.UInt8
			)
				throw new ExceptionEx( "Variant is {0} not required Int64", _v.Type );
			return Convert.ToInt64(_v.m_object);
		}
		public static implicit operator UInt64( Variant _v )
		{
			if( _v.Type != eVariantType.UInt64
				&& _v.Type != eVariantType.UInt32
				&& _v.Type != eVariantType.UInt16
				&& _v.Type != eVariantType.UInt8
			)
				throw new ExceptionEx( "Variant is {0} not required UInt64", _v.Type );
			return Convert.ToUInt16(_v.m_object);
		}
		public static implicit operator Boolean( Variant _v )
		{
			if( _v.Type != eVariantType.Boolean )
				throw new ExceptionEx( "Variant is {0} not required Boolean", _v.Type );
			return Convert.ToBoolean(_v.m_object);
		}
		public static implicit operator Single( Variant _v )
		{
			if( _v.Type != eVariantType.Single )
				throw new ExceptionEx( "Variant is {0} not required Single", _v.Type );
			return Convert.ToSingle(_v.m_object);
		}
		public static implicit operator Double( Variant _v )
		{
			if( _v.Type != eVariantType.Double )
				throw new ExceptionEx( "Variant is {0} not required Double", _v.Type );
			return Convert.ToDouble(_v.m_object);
		}
		public static implicit operator String( Variant _v )
		{
			if( _v.Type != eVariantType.String )
				throw new ExceptionEx( "Variant is {0} not required String", _v.Type );
			return Convert.ToString(_v.m_object);
		}

		public T GetValue<T>()
		{
			if( m_object is T )
				return (T)m_object;
			throw new ExceptionEx( "Variant is {0} not required {1}", Type, typeof( T ).Name );
		}

		public Byte[] GetBytes()
		{
			switch( Type )
			{
				case eVariantType.Int8:
				{
					unchecked
					{
						return new[] { (Byte)(SByte)m_object };
					}
				}
				case eVariantType.UInt8:
				{
					return new[] { (Byte)m_object };
				}
				case eVariantType.Int16:
				{
					return BitConverter.GetBytes( (Int16)m_object );
				}
				case eVariantType.UInt16:
				{
					return BitConverter.GetBytes( (UInt16)m_object );
				}
				case eVariantType.Int32:
				{
					return BitConverter.GetBytes( (Int32)m_object );
				}
				case eVariantType.UInt32:
				{
					return BitConverter.GetBytes( (UInt32)m_object );
				}
				case eVariantType.Int64:
				{
					return BitConverter.GetBytes( (Int64)m_object );
				}
				case eVariantType.UInt64:
				{
					return BitConverter.GetBytes( (UInt64)m_object );
				}
				case eVariantType.Single:
				{
					return BitConverter.GetBytes( (Single)m_object );
				}
				case eVariantType.Double:
				{
					return BitConverter.GetBytes( (Double)m_object );
				}
				case eVariantType.Boolean:
				{
					return BitConverter.GetBytes( (Boolean)m_object );
				}
				case eVariantType.String:
				{
					return m_object == null ? null : Encoding.Unicode.GetBytes( (String)m_object );
					//return BitConverter.GetBytes( (Boolean) m_object );
				}
				default:
				{
					throw new ExceptionEx( "Variant.GetBytes not support Non-Primitive Type" );
				}
			}
		}

		public Boolean IsPrimitive()
		{
			return Type != eVariantType.Class && Type != eVariantType.Empty;
		}

		public Object GetObject()
		{
			return m_object;
		}


		public override String ToString()
		{
			return m_object.ToString();
		}

		private Object m_object = null;
		public eVariantType Type { get; private set; }

		public static Variant Empty = new Variant();
	}
}
