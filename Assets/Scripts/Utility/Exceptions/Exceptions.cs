using System;

namespace Utility
{
	/// <summary>
	/// 扩展异常类
	/// </summary>
	public class ExceptionEx : Exception
	{
		public ExceptionEx( String _msg )
			: base( _msg )
		{
			Log.Exception( _msg );
		}

		public ExceptionEx( String _format, params Object[] _arg )
			: base( String.Format( _format, _arg ) )
		{
			Log.Exception( _format, _arg );
		}
	}


	public class VersionException : ExceptionEx
	{
		public VersionException( String _input_version_string )
			: base( "GVersion parse error: {0}",  _input_version_string )
		{
			Log.Exception( "GVersion parse error: {0}", _input_version_string );
		}
	}

	public class ServerInfoException : ExceptionEx
	{
		public enum ErrorType
		{
			/// <summary>
			/// 为初始化
			/// </summary>
			NotInitialized,

			/// <summary>
			/// 编码错误, 请使用utf-8
			/// </summary>
			EncodingError,

			/// <summary>
			/// 没有设置Primary
			/// </summary>
			PrimaryNotSet,

			/// <summary>
			/// Ip解析错误
			/// </summary>
			AssetsUrlError,

			/// <summary>
			/// Ip解析错误
			/// </summary>
			IpParseError,

			/// <summary>
			/// 包版本号错误
			/// </summary>
			PkgVersionError,

			/// <summary>
			/// 平台编号错误
			/// </summary>
			PlatformIdError,

			/// <summary>
			/// Primary所指向的ServerInfo没有找到
			/// </summary>
			ServerInfoNotFound,

			/// <summary>
			/// 日志等级错误
			/// </summary>
			LogLevelError,
		}

		public ErrorType Type { get; private set; }

		public ServerInfoException( ErrorType _type )
			: base( "ServerInfo parse error: {0}", _type.ToString() )
		{
			Type = _type;
			Log.Exception( "ServerInfo parse error: {0}", _type.ToString() );
		}
	}


	public class ConfigFileNotUTF8 : Exception
	{
		public ConfigFileNotUTF8( String _msg )
			: base( _msg )
		{
			Log.Exception( _msg );
		}

		public ConfigFileNotUTF8( String _format, params Object[] _arg )
			: base( String.Format( _format, _arg ) )
		{
			Log.Exception( _format, _arg );
		}
	}

	public class MultiLanguageFileNotExist : Exception
	{
		public MultiLanguageFileNotExist( String _language, String _virtual_path )
			: base( String.Format( " '{0}' version file: '{1}' not exist!", _language, _virtual_path ) )
		{
			Log.Exception( " '{0}' version file: '{1}' not exist!", _language, _virtual_path );
		}
	}


	/// <summary>
	/// 资源没有存在与资源列表中
	/// </summary>
	public class AssetNotExistException : Exception
	{
		public AssetNotExistException( String _asset_name )
			: base( String.Format( " Required Asset: {0} not exist in ResConfigFile", _asset_name ) )
		{
			Log.Exception( " Required Asset: {0} not exist in ResConfigFile", _asset_name );
		}
	}

	public class AssetException : Exception
	{
		public AssetException( String _msg )
			: base( _msg )
		{
			Log.Exception( _msg );
		}

		public AssetException( String _format, params Object[] _arg )
			: base( String.Format( _format, _arg ) )
		{
			Log.Exception( _format, _arg );
		}

	}

	public class ResourceNotExist : Exception
	{
		public ResourceNotExist( String _filename )
			: base( String.Format( "Resource: '{0} not exist! ", _filename ) )
		{
			Log.Exception( "Resource: '{0} not exist! ", _filename );
		}
	}

	class FileMappingNotExist : Exception
	{
		public FileMappingNotExist( String _virtual_path )
			: base( String.Format( "Resource file mapping for '{0}' not exist! ", _virtual_path ) )
		{
			Log.Exception( "Resource file mapping for '{0}' not exist! ", _virtual_path );
		}
	}
	/// <summary>
	/// AnyString转换异常
	/// </summary>
	public class AnyStringConvertException : ExceptionEx
	{
		public AnyStringConvertException( String _virtual_path )
			: base( String.Format( "AnyString can not convert to '{0}' ", _virtual_path ) )
		{
			Log.Exception( "AnyString can not convert to '{0}' ", _virtual_path );
		}
	}
}