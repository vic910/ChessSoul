using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Text;
using Utility;
using Utility.SheetLite;



namespace Groot
{
	public enum LanguageName
	{
		/// <summary>
		/// 中文简体
		/// </summary>
		zh_cn,

		/// <summary>
		/// 中文台湾
		/// </summary>
		zh_tw,

		/// <summary>
		/// 英文USA
		/// </summary>
		en_us,

		/// <summary>
		/// 英文UK
		/// </summary>
		en_gb,

		/// <summary>
		/// France
		/// </summary>
		fr,

		/// <summary>
		/// Germany
		/// </summary>
		de,

		/// <summary>
		/// Italy
		/// </summary>
		it,

		/// <summary>
		/// Russion
		/// </summary>
		ru,

		/// <summary>
		/// Japanese
		/// </summary>
		jp,

		/// <summary>
		/// Korean
		/// </summary>
		ko,


		max,
	}

	/// <summary>
	/// 本地化管理器
	/// </summary>
	public class Locale
	{
		private String[] s_language_files =
		{
			//"preload",	// 资源更新前用到的语言包	
			//"login",		// 登录时用到的语言包
			"tips",			// 进入大厅后各个游戏系统通用语言包
		};

		/// <summary>
		/// 单键实例
		/// </summary>
		public static Locale Instance = new Locale();
		/// <summary>
		/// 本地化字符串访问器
		/// </summary>
		/// <param name="_key"></param>
		/// <returns></returns>
		public String this[String _key]
		{
			get { return Instance.GetLocaleString( _key ); }
		}

		/// <summary>
		/// 构造函数：防止外部创建对象实例
		/// </summary>
		private Locale()
		{
			Language = LanguageName.max;
		}

		/// <summary>
		/// 当前语言
		/// </summary>
		public LanguageName Language { get; set; }

		/// <summary>
		/// 语言改变时触发
		/// </summary>
		public event Action<LanguageName> eventOnLanguageChanged;

		/// <summary>
		/// 清理函数
		/// </summary>
		public void Uninitialize()
		{
			m_locale.Clear();
		}

		/// <summary>
		/// 获取本地化字符串
		/// </summary>
		/// <param name="_key"></param>
		/// <returns></returns>
		public String GetLocaleString( String _key )
		{
			String locale_string;
			if( m_locale.TryGetValue( _key, out locale_string ) )
				return locale_string;
			Log.Error( "[Locale.GetLocaleString], 没有找到语言包字段: {0} !", _key );
			return String.Empty;
		}

		/// <summary>
		/// 改变系统语言
		/// </summary>
		/// <param name="_lang"></param>
		/// <param name="_preload_only"></param>
		public Boolean ChangeLanguage( LanguageName _lang )
		{
			m_locale.Clear();
			Language = _lang;
			LoadOneLanguageConfig( "preload" );
			LoadOneLanguageConfig( "login" );
			LoadAllLanguageConfig();
			if( null != eventOnLanguageChanged )
				eventOnLanguageChanged( Language );

			Log.Info( "成功切换语言: {0} -->{1}", Language, _lang );
			return true;
		}

		public void LoadOneLanguageConfig( string _config )
		{
			_loadLocaleFile( Language, _config );
		}

		public void LoadAllLanguageConfig()
		{
			for( int i = 0; i < s_language_files.Length; i++ )
			{
				_loadLocaleFile( Language, s_language_files[i] );
			}
		}

		private Boolean _loadLocaleFile( LanguageName _lang, String _file_name )
		{
			//StringBuilder string_builder = new StringBuilder( 128 );
			Utility.SheetLite.SheetReader reader = new Utility.SheetLite.SheetReader();

			if( !reader.OpenSheet( "language", String.Format( "{0}/{1}", _lang, _file_name ) ) )
			{
				Log.Error( "[Locale]语言包文件: {0} 加载失败!", _file_name );
				return false;
			}
			for( Int32 i = 0; i < reader.Count; ++i )
			{
				SheetRow row = reader[i];
				String key = row["Key"];

				String category = row["Category"];
				if( !String.IsNullOrEmpty( category ) )
				{
					key = String.Format( "{0}@{1}", category, key );
				}

				if( m_locale.ContainsKey( key ) )
				{
					Utility.Log.Error( "[Loclae]:语言包中包含重复键值：{0} {1}", _lang, key );
					continue;
				}
				String value = row["Value"];

				// 颜色值转换
				//string_builder.Length = 0;
				//Int32 pos = 0;
				//while( pos < value.Length )
				//{
				//	if( '[' == value[pos] )
				//	{
				//		if( UnityEngine.UI.Text.ProcessColor( value, pos, string_builder, out pos ) )
				//			continue;
				//	}
				//	string_builder.Append( value[pos++] );
				//}

				//value = string_builder.ToString().Trim();
				m_locale.Add( key, value );
			}
			return true;
		}

		/// <summary>
		/// 语言包，通过配置读取
		/// </summary>
		private Dictionary<String,String> m_locale = new Dictionary<string, string>();
	}
}