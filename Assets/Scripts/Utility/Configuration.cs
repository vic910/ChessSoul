using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Utility
{
	public class Configuration
	{
		#region 配置路径相关
		static Configuration()
		{
			s_configuration_path = "data";
		}
		public static String BuildConfigPath( String _filename )
		{
			return String.Format( "{0}/{1}", s_configuration_path, _filename );
			//return Path.Combine( s_configuration_path, _filename );
		}
		private static String s_configuration_path;
		#endregion
		public static readonly Int32 PixelsToUnits = 100;
		#region 配置字符串解析
		public static T ParseEnum<T>( String _value )
		{
			return (T)Enum.Parse( typeof( T ), _value );
		}
		/// <summary>
		/// 解析枚举类型，如果解析失败不会抛出异常
		/// </summary>
		/// <typeparam name="T">要解析的目标枚举类型</typeparam>
		/// <param name="_value">解析字符串</param>
		/// <param name="_enum">输出参数，解析后得到的值</param>
		/// <returns>解析成功返回true，否则返回false</returns>
		public static bool ParseEnum<T>( String _value, out T _enum ) //where T : Enum
		{
			try
			{
				_enum = (T)Enum.Parse( typeof( T ), _value );
			}
			catch( Exception )
			{
				_enum = default( T );
				return false;
			}
			return true;
		}
		public static bool TryParseVector2( String _value, ref Vector2 _vector2 )
		{
			Utility.TokenString token_string = new Utility.TokenString( _value, new Char[] { ',' } );
			if( 2 != token_string.Length )
			{
				_vector2.x = 0;
				_vector2.y = 0;
				return false;
			}
			_vector2.x = token_string[0];
			_vector2.y = token_string[1];
			return true;
		}

		public static bool TryParseVector3( String _value, out Vector3 _vector3 )
		{
			_vector3 = new Vector3();
			String [] parameters = _value.Split( new Char[] { ',' } );
			if( 3 != parameters.Length )
			{
				return false;
			}
			if( !float.TryParse( parameters[0], out _vector3.x ) )
				return false;
			if( !float.TryParse( parameters[1], out _vector3.y ) )
				return false;
			if( !float.TryParse( parameters[2], out _vector3.z ) )
				return false;
			return true;
		}

		#endregion
	}
}
