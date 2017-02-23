using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Utility;
using UnityEngine;

namespace Groot.Res
{
	/// <summary>
	/// 资源类型，用于区分加载方式等
	/// </summary>
	public enum ResourceType : byte
	{
		AssetBundle,
		GrtPackage,
	}

	/// <summary>
	/// Asset信息文件, 包含每个asset的详细信息
	/// </summary>
	public class ResInfo
	{
		public ResInfo( String _name, Int64 _size, Int32 _version, String _sha1, String _md5, UInt32 _crc32, AssetLocation _location, ResourceType _resource_type )
		{
			Name = _name;
			Size = _size;
			Version = _version;
			Md5 = _md5;
			Sha1 = _sha1;
			Crc32 = _crc32;
			Location = _location;
			ResType = _resource_type;
		}

		/// <summary>
		/// 给系统接口用的路径获取
		/// </summary>
		/// <param name="_file_relative_path"></param>
		/// <param name="_location"></param>
		/// <param name="_resource_type"></param>
		/// <returns></returns>
		public static String GetSystemApiLoadPath( String _file_relative_path, AssetLocation _location )
		{
			String re;
			switch( _location )
			{
			case AssetLocation.StreamingPath:
				{
					if( Application.isEditor )
						re = String.Format( "Assets/StreamingAssets/res/{0}", _file_relative_path );
					else if( Application.platform == RuntimePlatform.IPhonePlayer )
						re = String.Format( "{0}/res/{1}", Application.streamingAssetsPath, _file_relative_path );
					// 安卓不支持直接读取
					else
						throw new ExceptionEx( "[ResInfo.GetSystemApiLoadPath] Current platform: {0} not support!", Application.platform );
					break;
				}
			case AssetLocation.PersistentPath:
				{
					re = String.Format( "{0}/{1}", ResUtility.PersistentRootPath, _file_relative_path );
					break;
				}
			default:
				throw new ExceptionEx( "[ResInfo.GetWWWLoadPath] File Location: {0} not exist!", _location );

			}
			Log.Debug( eLogType.Resources, "获取{0} 资源地址: {1}", _location, re );
			return re;
		}

		public static String GetWWWLoadPath( String _file_relative_path, AssetLocation _location )
		{
			String re = null;
			switch( _location )
			{
			case AssetLocation.StreamingPath:
				{
					if( Application.isEditor )
						re = String.Format( "file:///{0}/res/{1}", Application.streamingAssetsPath, _file_relative_path );
					else if( Application.platform == RuntimePlatform.IPhonePlayer )
						re = String.Format( "file://{0}/res/{1}", Application.streamingAssetsPath, _file_relative_path );
					else if( Application.platform == RuntimePlatform.Android )
						re = String.Format( "{0}/res/{1}", Application.streamingAssetsPath, _file_relative_path );
					else
						throw new ExceptionEx( "[ResInfo.GetWWWLoadPath] Current platform: {0} not support!", Application.platform );
					break;
				}
			case AssetLocation.PersistentPath:
				{
					if( Application.isEditor )
						re = String.Format( "file:///{0}/{1}", ResUtility.PersistentRootPath, _file_relative_path );
					else if( Application.platform == RuntimePlatform.IPhonePlayer )
						re = String.Format( "file://{0}/{1}", ResUtility.PersistentRootPath, _file_relative_path );
					else if( Application.platform == RuntimePlatform.Android )
						re = String.Format( "file:///{0}/{1}", ResUtility.PersistentRootPath, _file_relative_path );
					else
						throw new ExceptionEx( "[ResInfo.GetWWWLoadPath] Current platform: {0} not support!", Application.platform );

					break;
				}
			default:
				throw new ExceptionEx( "[ResInfo.GetWWWLoadPath] File Location: {0} not exist!", _location );

			}
			Log.Debug( eLogType.Resources, "获取{0} 资源地址: {1}", _location, re );
			return re;
		}

		/// <summary>
		/// 用于WWW类的加载路径
		/// </summary>
		public String UnityApiLoadingPath
		{
			get
			{
				if( m_unity_api_loading_path != null )
					return m_unity_api_loading_path;
				m_unity_api_loading_path = GetWWWLoadPath( Name, Location );
				return m_unity_api_loading_path;
			}
		}
		private String m_unity_api_loading_path;

		/// <summary>
		/// 用于System.IO类的加载使用路径
		/// </summary>
		public String SystemApiLoadingPath
		{
			get
			{
				if( m_system_api_loading_path != null )
					return m_system_api_loading_path;
				m_system_api_loading_path = GetSystemApiLoadPath( Name, Location );
				return m_system_api_loading_path;
			}
		}
		private String m_system_api_loading_path;

		/// <summary>
		/// 文件保存路径(仅用于下载)
		/// </summary>
		public String SavePath
		{
			get
			{
				String path = String.Format( "{0}/{1}", ResUtility.PersistentRootPath, Name );
				return path;
			}
		}

		/// <summary>
		/// 获取该资源的下载地址
		/// </summary>
		public String DownloadUrl
		{
			get
			{
				String url = String.Format( "{0}/{1}/{2}?p={3}", WeiqiApp.ResServerUrl, Version.ToString(), Name, DateTime.Now.Ticks );
				return url;
			}
		}

		/// <summary>
		/// 资源名称(包括相对路劲), 包括扩展名(如果存在)
		/// </summary>
		public String Name { get; private set; }

		/// <summary>
		/// 资源文件大小, 单位 Byte
		/// </summary>
		public Int64 Size { get; private set; }

		/// <summary>
		/// 资源包版本号
		/// </summary>
		public Int32 Version { get; private set; }

		/// <summary>
		/// md5 校验码
		/// </summary>
		public String Md5 { get; private set; }

		/// <summary>
		/// SHA-1校验码
		/// </summary>
		public String Sha1 { get; private set; }

		/// <summary>
		/// Crc 32 校验码 由unity3d 打包assetbundle时生成
		/// </summary>
		public UInt32 Crc32 { get; private set; }


		/// <summary>
		/// 文件存放的位置
		/// </summary>
		public AssetLocation Location { get; set; }

		/// <summary>
		/// 资源类型
		/// </summary>
		public ResourceType ResType { get; set; }

#if UNITY_EDITOR
		/// <summary>
		/// Assetbundle打包后的TypeTreeHash值，
		/// 用来判断这个AB是否需要更新
		/// (在更换机器或者清理Library的情况下，从新打包文件二进制可能会变，但没有改变的Asset的TypeTreeHash不会变)
		/// </summary>
		public String AssetBundleTypeTreeHash;

#endif

		/// <summary>
		/// 检测文件本身是否需要更新
		/// </summary>
		/// <param name="_correct_res_info"></param>
		/// <returns></returns>
		public Boolean NeedUpdate( ResInfo _correct_res_info )
		{
			return Size != _correct_res_info.Size
				|| Crc32 != _correct_res_info.Crc32
				|| Md5 != _correct_res_info.Md5
				|| Sha1 != _correct_res_info.Sha1;
		}

		public ResInfo Clone()
		{
			return new ResInfo( Name, Size, Version, Sha1, Md5, Crc32, Location, ResType );
		}

		public void CopyTo( ResInfo _dest_res_info )
		{
			_dest_res_info.Name = Name;
			_dest_res_info.Size = Size;
			_dest_res_info.Version = Version;
			_dest_res_info.Sha1 = Sha1;
			_dest_res_info.Md5 = Md5;
			_dest_res_info.Crc32 = Crc32;
			_dest_res_info.Location = Location;
			_dest_res_info.ResType = ResType;
		}

	}
}