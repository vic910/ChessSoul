
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using Utility.SheetLite;
using Utility.Variant;

namespace Utility.VariantSystem
{
	internal class VariantPairSerializer
	{
		public static Boolean CreateFromSheetLite( SheetLite.SheetRow _row, out String _key, out Variant.Variant _value )
		{
			_value = null;
			_key = _row["Key"];
			if( _key == String.Empty )
			{
				Log.Error( "[VariantPairSerializer.CreateFromSheetLite]: Key不能为空!" );
				_key = null;
				return false;
			}

			eVariantType variant_type;
			return Configuration.ParseEnum<eVariantType>( _row["VariantType"], out variant_type )
				   && _parseVariant( _row["Value"], ref _value, variant_type );
		}

		private static bool _parseVariant( AnyString _raw, ref Variant.Variant _value, eVariantType _variant_type )
		{
			switch( _variant_type )
			{
				case eVariantType.UInt8:
				{
					SByte v = _raw;
					_value = v;
					return true;
				}
				case eVariantType.Int8:
				{
					Byte v = _raw;
					_value = v;
					return true;
				}
				case eVariantType.UInt16:
				{
					UInt16 v = _raw;
					_value = v;
					return true;
				}
				case eVariantType.Int16:
				{
					Int16 v = _raw;
					_value = v;
					return true;
				}
				case eVariantType.UInt32:
				{
					UInt32 v = _raw;
					_value = v;
					return true;
				}
				case eVariantType.Int32:
				{
					Int32 v = _raw;
					_value = v;
					return true;
				}
				case eVariantType.UInt64:
				{
					UInt64 v = _raw;
					_value = v;
					return true;
				}
				case eVariantType.Int64:
				{
					Int64 v = _raw;
					_value = v;
					return true;
				}
				case eVariantType.Single:
				{
					Single v = _raw;
					_value = v;
					return true;
				}
				case eVariantType.Double:
				{
					Double v = _raw;
					_value = v;
					return true;
				}
				case eVariantType.Boolean:
				{
					Boolean v = _raw;
					_value = v;
					return true;
				}
				case eVariantType.String:
				{
					_value = _raw.ToString();
					return true;
				}
				default:
				{
					Log.Error( "[VariantPairSerializer.CreateFromSheetLite]: _variant_type 为 {0} 无法解析!", _raw );
					return false;
				}
			}
		}

		public static Boolean CreateFromStream( Stream _stream, out Variant.Variant _value )
		{
			eVariantType _variant_type = (eVariantType) _stream.ReadByte();


			Byte[] buffer = new Byte[8];

			switch( _variant_type )
			{
				case eVariantType.UInt8:
				{
					Byte v = (Byte)_stream.ReadByte();
					_value = v;
					return true;
				}
				case eVariantType.Int8:
				{
					unchecked
					{
						SByte v = (SByte)_stream.ReadByte();
						_value = v;
					}
					return true;
				}
				case eVariantType.UInt16:
				{
					_stream.Read( buffer, 0, 2 );
					UInt16 v = BitConverter.ToUInt16( buffer, 0 );
					_value = v;
					return true;
				}
				case eVariantType.Int16:
				{
					_stream.Read( buffer, 0, 2 );
					Int16 v = BitConverter.ToInt16( buffer, 0 );
					_value = v;
					return true;
				}
				case eVariantType.UInt32:
				{
					_stream.Read( buffer, 0, 4 );
					UInt32 v = BitConverter.ToUInt32( buffer, 0 );
					_value = v;
					return true;
				}
				case eVariantType.Int32:
				{
					_stream.Read( buffer, 0, 4 );
					Int32 v = BitConverter.ToInt32( buffer, 0 );
					_value = v;
					return true;
				}
				case eVariantType.UInt64:
				{
					_stream.Read( buffer, 0, 8 );
					UInt64 v = BitConverter.ToUInt64( buffer, 0 );
					_value = v;
					return true;
				}
				case eVariantType.Int64:
				{
					_stream.Read( buffer, 0, 8 );
					Int64 v = BitConverter.ToInt64( buffer, 0 );
					_value = v;
					return true;
				}
				case eVariantType.Single:
				{
					_stream.Read( buffer, 0, 4 );
					Single v = BitConverter.ToSingle( buffer, 0 );
					_value = v;
					return true;
				}
				case eVariantType.Double:
				{
					_stream.Read( buffer, 0, 8 );
					Single v = BitConverter.ToSingle( buffer, 0 );
					_value = v;
					return true;
				}
				case eVariantType.Boolean:
				{
					_stream.Read( buffer, 0, 1 );
					Boolean v = BitConverter.ToBoolean( buffer, 0 );
					_value = v;
					return true;
				}
				case eVariantType.String:
				{
					_stream.Read( buffer, 0, 4 );
					Int32 len = BitConverter.ToInt32( buffer, 0 );

					Byte [] str_bytes = new byte[len];
					_stream.Read( str_bytes, 0, len );
					String str = Encoding.Unicode.GetString( str_bytes );
					_value = str;
					return true;
				}
				default:
				{
					Log.Error( "[VariantPairSerializer.CreateFromStream]: _variant_type 为 {0} 无法解析!", _variant_type );
					_value = null;
					return false;
				}
			}
		}
	}
}