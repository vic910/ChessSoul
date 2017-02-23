using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

using Utility;
namespace Groot.Res
{
	/// <summary>
	/// 资源存储位置
	/// </summary>
	public enum AssetLocation
	{
		/// <summary>
		/// 资源打包时的默认值
		/// </summary>
		Void,

		/// <summary>
		/// apk 或者 ipa 包内部
		/// </summary>
		StreamingPath,

		/// <summary>
		/// 手机的app专用存储空间
		/// </summary>
		PersistentPath,
	}

	public class ResUtility
	{

		/// <summary>
		/// App包内部存储地址
		/// </summary>
		public static String EditorTempPath
		{
			get
			{
				if( Application.platform != RuntimePlatform.OSXEditor && RuntimePlatform.WindowsEditor != Application.platform )
				{
					throw new Exception( "EditorTempPath can only use with Editor!" );
				}

				String path = Application.dataPath.Substring( 0, Application.dataPath.LastIndexOfAny( new Char[] { '/', '\\' } ) );
				path += "/dev_path/temp";
				if( !System.IO.Directory.Exists( path ) )
					System.IO.Directory.CreateDirectory( path );
				return path;
			}
		}

		/// <summary>
		/// persistent地址
		/// </summary>
		public static String PersistentRootPath
		{
			get
			{
				String path;
#if !GROOT_ASSETBUNDLE_SIMULATION && UNITY_EDITOR
				path = Application.dataPath.Substring( 0, Application.dataPath.LastIndexOfAny( new Char[] { '/', '\\' } ) );
				path += "/dev_path/persistent";
#elif UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_WIN
				path = Application.persistentDataPath;
#else
				throw new Exception( String.Format( "PersistentRootPath: platform {0} not support!", Application.platform ) );
#endif
				if( !System.IO.Directory.Exists( path ) )
					System.IO.Directory.CreateDirectory( path );
				return path;
			}
		}

		public static String LocalResConfigPath
		{
			get { return String.Format( "{0}/res_config", PersistentRootPath ); }
		}

		/// <summary>
		/// 服务器端ResConfig下载地址
		/// </summary>
		public static String ResConfigDownloadUrl
		{
			get
			{
				String url = String.Format( "{0}/res_config.grt?p={1}" , WeiqiApp.ResServerUrl, DateTime.Now.Ticks );
				Log.Debug( "ResConfigDownloadUrl: {0}", url );
				return url;
			}
		}
	}
}