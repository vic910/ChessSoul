using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using UnityEngine;

namespace Utility
{
	public enum eLogLevel
	{
		Verbose,
		Debug,
		Info,
		Warning,
		Error,
		Critical,
		Exception,

		Max,
	}

	public enum eLogType
	{
		/// <summary>
		/// 普通日志
		/// </summary>
		Normal,

		/// <summary>
		/// 资源相关日志
		/// </summary>
		Resources,

		/// <summary>
		/// 网络日志
		/// </summary>
		Net,

		Max
	}

	public class LogData
	{
		private eLogLevel m_log_level;
		private eLogType  m_log_type;
		private String    m_log_string;

		public LogData( eLogLevel _level, eLogType _type, String _string )
		{
			m_log_level = _level;
			m_log_type = _type;
			m_log_string = _string;
		}

		public eLogLevel LogLevel { get { return m_log_level; } set { m_log_level = value; } }
		public eLogType LogType { get { return m_log_type; } set { m_log_type = value; } }
		public String LogString { get { return m_log_string; } set { m_log_string = value; } }
	}

	public class Log //: UnitySingleton<Log>
	{
		private static readonly Log s_instance = new Log();
#if UNITY_EDITOR
		private static readonly String[] s_colors =
		{
			"E8E8E8",
			"008EBB",
			"00DC06",
			"F0F000",
			"FF6B68",
			"FF0000",
			"FF0000"
		};
#endif

		/// <summary>
		/// 发布版最低存储等级
		/// </summary>
		private eLogLevel m_release_log_level = eLogLevel.Verbose;
		private readonly Boolean[] m_log_type_switch = new Boolean[(Int32)eLogType.Max];
		//private readonly List<LogData> m_all_log_data = new List<LogData>();

		private readonly StringBuilder m_string_buider = new StringBuilder( 512 );
		private ILogger m_logger = UnityEngine.Debug.logger;

		private Log()
		{
			//m_release_Log_Level = Application.isEditor ? eLogLevel.Verbose : eLogLevel.Info;
			//GameWorld.Instance.eventDestroy += _save;
			for( Int32 i = 0; i < m_log_type_switch.Length; ++i )
			{
				m_log_type_switch[i] = true;
			}
		}

		/// <summary>
		/// 设置日志现实的最小级别
		/// </summary>
		/// <param name="_level"></param>
		public static void SetLogLevel( eLogLevel _level )
		{
			UnityEngine.Debug.Log( String.Format( "设置日志等级: {0}", _level ) );
			s_instance.m_release_log_level = _level;
		}

		public static void Verbose( String _msg )
		{
			LogInfo( eLogLevel.Verbose, eLogType.Normal, _msg );
		}
		public static void Verbose( eLogType _type, String _msg )
		{
			LogInfo( eLogLevel.Verbose, _type, _msg );
		}
		public static void Verbose( String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Verbose, eLogType.Normal, _format, _args );
		}
		public static void Verbose( eLogType _type, String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Verbose, _type, _format, _args );
		}
		public static void Debug( String _msg )
		{
			LogInfo( eLogLevel.Debug, eLogType.Normal, _msg );
		}
		public static void Debug( eLogType _type, String _msg )
		{
			LogInfo( eLogLevel.Debug, _type, _msg );
		}
		public static void Debug( String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Debug, eLogType.Normal, _format, _args );
		}
		public static void Debug( eLogType _type, String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Debug, _type, _format, _args );
		}

		public static void Info( String _msg )
		{
			LogInfo( eLogLevel.Info, eLogType.Normal, _msg );
		}
		public static void Info( eLogType _type, String _msg )
		{
			LogInfo( eLogLevel.Info, _type, _msg );
		}
		public static void Info( String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Info, eLogType.Normal, _format, _args );
		}
		public static void Info( eLogType _type, String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Info, _type, _format, _args );
		}

		public static void Warning( String _msg )
		{
			LogInfo( eLogLevel.Warning, eLogType.Normal, _msg );
		}
		public static void Warning( eLogType _type, String _msg )
		{
			LogInfo( eLogLevel.Warning, _type, _msg );
		}
		public static void Warning( String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Warning, eLogType.Normal, _format, _args );
		}
		public static void Warning( eLogType _type, String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Warning, _type, _format, _args );
		}

		public static void Error( String _msg )
		{
			LogInfo( eLogLevel.Error, eLogType.Normal, _msg );
		}
		public static void Error( eLogType _type, String _msg )
		{
			LogInfo( eLogLevel.Error, _type, _msg );
		}
		public static void Error( String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Error, eLogType.Normal, _format, _args );
		}
		public static void Error( eLogType _type, String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Error, _type, _format, _args );
		}

		public static void Critical( String _msg )
		{
			LogInfo( eLogLevel.Critical, eLogType.Normal, _msg );
		}
		public static void Critical( eLogType _type, String _msg )
		{
			LogInfo( eLogLevel.Critical, _type, _msg );
		}
		public static void Critical( String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Critical, eLogType.Normal, _format, _args );
		}
		public static void Critical( eLogType _type, String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Critical, _type, _format, _args );
		}

		public static void Exception( String _msg )
		{
			LogInfo( eLogLevel.Exception, eLogType.Normal, _msg );
		}

		public static void Exception( String _format, params System.Object[] _args )
		{
			LogInfo( eLogLevel.Exception, eLogType.Normal, _format, _args );
		}

		public static void Assert( Boolean _condition )
		{
			System.Diagnostics.Debug.Assert( _condition );
		}
		public static void Assert( Boolean _condition, String _message )
		{
			System.Diagnostics.Debug.Assert( _condition, _message );
		}
		public static void Assert( Boolean _condition, String _message, String _details )
		{
			System.Diagnostics.Debug.Assert( _condition, _message, _details );
		}
		public static void Assert( Boolean _condition, String _message, String _details, System.Object[] _args )
		{
			//System.Diagnostics.Debug.Assert( _condition, _message, _details, _args );
		}

		public static void LogInfo( eLogLevel _level, eLogType _type, String _format, params System.Object[] _args )
		{
			// 未开启的不显示
			if( !s_instance.m_log_type_switch[(Int32)_type]
				&& (Int32)eLogLevel.Warning > (Int32)_level )
				return;

			// 未达到等级的不显示
			if( _level < s_instance.m_release_log_level )
				return;

			// 组装日志
			s_instance.m_string_buider.Length = 0;

#if !UNITY_EDITOR
			s_instance.m_string_buider.AppendFormat( "[{0}][{1}]:\t", _level, _type );
#else
			s_instance.m_string_buider.AppendFormat( "<color=#{2}>[{0}][{1}]:\t", _level, _type, s_colors[(Int32)_level] );
#endif

			s_instance.m_string_buider.AppendFormat( _format, _args );

#if UNITY_EDITOR
			s_instance.m_string_buider.Append( "</color>" );
#endif

			//Instance._showLog( _level, _type, Instance.m_string_buider.ToString() );
			switch( _level )
			{
			case eLogLevel.Verbose:
			case eLogLevel.Debug:
			case eLogLevel.Info:
#if UNITY_EDITOR
				s_instance.m_logger.Log( LogType.Log, s_instance.m_string_buider.ToString() );
#else
				s_instance.m_logger.Log( LogType.Log, WeiqiApp.PROJECT_NAME, s_instance.m_string_buider.ToString() );
#endif
				break;
			case eLogLevel.Warning:
#if UNITY_EDITOR
				s_instance.m_logger.Log( LogType.Warning, s_instance.m_string_buider.ToString() );
#else
				s_instance.m_logger.Log( LogType.Warning, WeiqiApp.PROJECT_NAME, s_instance.m_string_buider.ToString() );
#endif
				break;
			case eLogLevel.Error:
			case eLogLevel.Critical:
#if UNITY_EDITOR
				s_instance.m_logger.Log( LogType.Error, s_instance.m_string_buider.ToString() );
#else
				s_instance.m_logger.Log( LogType.Error, WeiqiApp.PROJECT_NAME, s_instance.m_string_buider.ToString() );
#endif
				break;
			}
		}
	}
}