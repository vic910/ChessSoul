using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using System.IO;

namespace Groot.Network
{
	class SerializerHelper
	{
		public static readonly Type s_boolean_type = typeof( Boolean );
		public static readonly Type s_byte_type = typeof( Byte );
		public static readonly Type s_sbyte_type = typeof( SByte );
		public static readonly Type s_int16_type = typeof( Int16 );
		public static readonly Type s_int32_type = typeof( Int32 );
		public static readonly Type s_int64_type = typeof( Int64 );
		public static readonly Type s_uint16_type = typeof( UInt16 );
		public static readonly Type s_uint32_type = typeof( UInt32 );
		public static readonly Type s_uint64_type = typeof( UInt64 );
		public static readonly Type s_single_type = typeof( Single );
		public static readonly Type s_double_type = typeof( Double );
		public static readonly Type s_string_type = typeof( String );
		public static readonly Type s_list_type = typeof( IList );


		#region 序列化
		public static SerializeWriter s_serialize_writer = new SerializeWriter();

		public static Byte[] SerializeMessage( Object _obj )
		{
			_serializeInit();
			_serializer( _obj, _obj.GetType() );
			return s_serialize_writer.Bytes;
		}

		private static void _serializeInit()
		{
			s_serialize_writer.Reset();
		}

		private static Int32 _serializer( Object _obj, Type _type )
		{
			if( !_type.IsClass )
			{
				Utility.Log.Exception( "Serializer can't handle non-class type" );
			}
			Int32 obj_size = 0;

			var fields = _type.GetFields( BindingFlags.Public | BindingFlags.FlattenHierarchy
				| BindingFlags.NonPublic | BindingFlags.Instance );

			List<MessageFieldInfo> fields_info = new List<MessageFieldInfo>( fields.Length );

			for( Int32 i = 0; i < fields.Length; ++i )
			{
				fields_info.Add( new MessageFieldInfo( fields[i], fields[i].GetCustomAttributes( false )[0] as MessageFiled ) );
			}

			Int32 tmp1 = fields_info[fields_info.Count - 1].Attribute.Index + 1;
			Int32 tmp2 = 0;
			for( Int32 i = fields.Length - 1; i >= 0; --i )
			{
				fields_info[i].NewOrder = fields_info[i].Attribute.Index + tmp2;
				if( fields_info[i].Attribute.Index == 0 && i > 0 )
				{
					tmp2 = tmp1;
					tmp1 = fields_info[i - 1].Attribute.Index + 1 + tmp2;
				}
			}

			fields_info.Sort( ( v1, v2 ) =>
			{
				return ( v1.NewOrder == v2.NewOrder ) ? 0 : v1.NewOrder > v2.NewOrder ? 1 : -1;
			} );

			for( Int32 i = 0; i < fields.Length; ++i )
			{
				var field = fields_info[i].Info;
				// 基础类型
				if( field.FieldType.IsPrimitive )
				{
					obj_size += _primitiveSerializer( field.GetValue( _obj ), field.FieldType );
				}
				// 字符串
				else if( field.FieldType == s_string_type )
				{
					var attribute = fields_info[i].Attribute;
					if( attribute.Length > 0 )
						obj_size += s_serialize_writer.Write( (String)field.GetValue( _obj ), attribute.Length );
					else
						Utility.Log.Error( "字符串数组{0}，字符串长度为零!", field.Name );
				}
				// 数组或者IList
				else if( typeof( IList ).IsAssignableFrom( field.FieldType ) )
				{
					obj_size += _arraySerializer( fields_info[i], _obj );

				}
				else if( field.FieldType.IsClass )
				{
					obj_size += _serializer( field.GetValue( _obj ), field.FieldType );
				}

			}
			return obj_size;
		}
		private static Int32 _arraySerializer( MessageFieldInfo fields_info, Object _obj )
		{
			var attribute = fields_info.Attribute;
			FieldInfo field_info = fields_info.Info;
			var array_value = field_info.GetValue( _obj );

			Type item_type;
			// 标准数组
			if( field_info.FieldType.IsArray )
				item_type = field_info.FieldType.GetElementType();
			else
				item_type = field_info.FieldType.GetGenericArguments()[0];
			Int32 item_size = 0;
			Int32 remain_count = attribute.Length;

			if( array_value == null )
				Utility.Log.Exception( "数组{0}({1}, {2})为空，序列化失败！", field_info.Name, field_info.FieldType.Name, item_type.Name );
			foreach( var item in array_value as IList )
			{
				if( remain_count < 0 )
				{
					Utility.Log.Error( "数组{0}({1}, {2})实际长度超过可容纳长度: {2}"
						, field_info.Name, field_info.FieldType.Name, item_type.Name, attribute.Length );
					break;
				}
				else if( item == null )
				{
					continue;
				}
				else if( item_type.IsPrimitive )
				{
					item_size = _primitiveSerializer( item, item_type );
				}
				else if( item_type == s_string_type )
				{
					if( attribute.Length2 == 0 )
					{
						Utility.Log.Error( "字符串数组{0}，字符串长度为零!", field_info.Name );
						item_size = 0;
						break;
					}
					item_size += s_serialize_writer.Write( (String)item, attribute.Length2 );
				}
				else if( item_type.IsClass )
				{
					item_size = _serializer( item, item_type );
				}
				else
				{
					Utility.Log.Exception( "消息内数组: {0} 容纳类型: {1} 为不支持类型", field_info.Name, item_type.Name );
				}
				--remain_count;
			}
			if( remain_count == attribute.Length )
				Utility.Log.Exception( "数组: {0}({1}, {2}), 元素数量为0 或者数组中没有对象实体，序列化失败!", field_info.Name, field_info.FieldType.Name, item_type.Name );

			Int32 array_size = item_size * attribute.Length;
			s_serialize_writer.WriteZero( item_size * remain_count );
			return array_size;
		}
		private static Int32 _primitiveSerializer( Object _obj, Type _type )
		{
			if( _type == s_boolean_type )
				return s_serialize_writer.Write( (Boolean)_obj );

			if( _type == s_sbyte_type )
				return s_serialize_writer.Write( (SByte)_obj );

			else if( _type == s_byte_type )
				return s_serialize_writer.Write( (Byte)_obj );

			else if( _type == s_int16_type )
				return s_serialize_writer.Write( (Int16)_obj );

			else if( _type == s_int32_type )
				return s_serialize_writer.Write( (Int32)_obj );

			else if( _type == s_int64_type )
				return s_serialize_writer.Write( (Int64)_obj );

			else if( _type == s_uint16_type )
				return s_serialize_writer.Write( (UInt16)_obj );

			else if( _type == s_uint32_type )
				return s_serialize_writer.Write( (UInt32)_obj );

			else if( _type == s_uint64_type )
				return s_serialize_writer.Write( (UInt64)_obj );

			else if( _type == s_single_type )
				return s_serialize_writer.Write( (Single)_obj );

			else if( _type == s_double_type )
				return s_serialize_writer.Write( (Double)_obj );

			return 0;
		}
		#endregion


		#region
		public static SerializeReader s_serialize_reader;

		public static T Deserializer<T>( Byte[] _buff )
		{
			if( s_serialize_reader != null )
			{
				s_serialize_reader.Close();
				s_serialize_reader = null;
			}
			s_serialize_reader = new SerializeReader( _buff );
			return (T)_deserializer( typeof( T ) );
		}

		private static Object _deserializer( Type _type )
		{
			if( !_type.IsClass )
			{
				Utility.Log.Exception( "Deserializer can't handle non-class type" );
			}

			Object _obj = Activator.CreateInstance( _type );

			var fields = _type.GetFields( BindingFlags.Public | BindingFlags.FlattenHierarchy
				| BindingFlags.NonPublic | BindingFlags.Instance );

			List<MessageFieldInfo> fields_info = new List<MessageFieldInfo>( fields.Length );

			for( Int32 i = 0; i < fields.Length; ++i )
			{
				fields_info.Add( new MessageFieldInfo( fields[i], fields[i].GetCustomAttributes( false )[0] as MessageFiled ) );
			}

			Int32 tmp1 = fields_info[fields_info.Count - 1].Attribute.Index + 1;
			Int32 tmp2 = 0;
			for( Int32 i = fields.Length - 1; i >= 0; --i )
			{
				fields_info[i].NewOrder = fields_info[i].Attribute.Index + tmp2;
				if( fields_info[i].Attribute.Index == 0 && i > 0 )
				{
					tmp2 = tmp1;
					tmp1 = fields_info[i - 1].Attribute.Index + 1 + tmp2;
				}
			}

			fields_info.Sort( ( v1, v2 ) =>
			{
				return ( v1.NewOrder == v2.NewOrder ) ? 0 : v1.NewOrder > v2.NewOrder ? 1 : -1;
			} );

			for( Int32 i = 0; i < fields.Length; ++i )
			{
				var field = fields_info[i].Info;
				// 基础类型
				if( field.FieldType.IsPrimitive )
				{
					field.SetValue( _obj, _primitiveDeserializer( field.FieldType ) );
				}
				// 字符串
				else if( field.FieldType == s_string_type )
				{
					var attribute = fields_info[i].Attribute;
					if( attribute.Length > 0 )
						field.SetValue( _obj, s_serialize_reader.ReadString( attribute.Length ) );
					else
						Utility.Log.Error( "字符串数组{0}，字符串长度为零!", field.Name );
				}
				// 数组或者IList
				else if( typeof( IList ).IsAssignableFrom( field.FieldType ) )
				{
					field.SetValue( _obj, _arrayDeserializer( fields_info[i], _obj ) );

				}
				else if( field.FieldType.IsClass )
				{
					field.SetValue( _obj, _deserializer( field.FieldType ) );
				}

			}
			return _obj;
		}


		private static Object _arrayDeserializer( MessageFieldInfo fields_info, Object _obj )
		{
			var attribute = fields_info.Attribute;
			FieldInfo field_info = fields_info.Info;
			//var array_value = field_info.GetValue( _obj );
			object array = null;
			Type item_type;
			if( field_info.FieldType.IsArray )
			{
				//array = Activator.CreateInstance( field_info.FieldType, new object[] { attribute.Length } );

				item_type = field_info.FieldType.GetElementType();
				array = Array.CreateInstance( item_type, attribute.Length );
			}
			else
			{
				array = Activator.CreateInstance( field_info.FieldType );
				item_type = field_info.FieldType.GetGenericArguments()[0];
			}

			object [] value = new object[1];
			for( Int32 i = 0; i < attribute.Length; ++i )
			{
				if( item_type.IsPrimitive )
				{
					value[0] = _primitiveDeserializer( item_type );
				}
				else if( item_type == s_string_type )
				{
					if( attribute.Length2 == 0 )
					{
						Utility.Log.Error( "字符串数组{0}，字符串长度为零!", field_info.Name );
						break;
					}
					value[0] = s_serialize_reader.ReadString( attribute.Length2 );
					if( string.Empty == value[0] )
						value[0] = string.Empty;
				}
				else if( item_type.IsClass )
				{
					value[0] = _deserializer( item_type );
				}
				else
				{
					Utility.Log.Exception( "消息内数组: {0} 容纳类型: {1} 为不支持类型", field_info.Name, item_type.Name );
				}
				if( field_info.FieldType.IsArray )
					( (Array)array ).SetValue( value[0], i );
				else
					field_info.FieldType.InvokeMember( "Add", BindingFlags.DeclaredOnly |
						BindingFlags.Public | BindingFlags.NonPublic |
						BindingFlags.Instance | BindingFlags.InvokeMethod, null, array, value );
			}
			return array;
		}
		private static Object _primitiveDeserializer( Type _type )
		{
			if( _type == s_boolean_type )
				return s_serialize_reader.ReadBool();

			else if( _type == s_sbyte_type )
				return s_serialize_reader.ReadSByte();

			else if( _type == s_byte_type )
				return s_serialize_reader.ReadByte();

			else if( _type == s_int16_type )
				return s_serialize_reader.ReadInt16();

			else if( _type == s_int32_type )
				return s_serialize_reader.ReadInt32();

			else if( _type == s_int64_type )
				return s_serialize_reader.ReadInt64();

			else if( _type == s_uint16_type )
				return s_serialize_reader.ReadUInt16();

			else if( _type == s_uint32_type )
				return s_serialize_reader.ReadUInt32();

			else if( _type == s_uint64_type )
				return s_serialize_reader.ReadUInt64();

			else if( _type == s_single_type )
				return s_serialize_reader.ReadSingle();

			else if( _type == s_double_type )
				return s_serialize_reader.ReadDouble();

			Utility.Log.Error( "_primitiveDeserializer Type: {0} 无法处理!", _type.Name );
			return null;
		}
		#endregion
	}
}
