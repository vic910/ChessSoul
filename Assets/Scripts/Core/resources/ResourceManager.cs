using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Utility;
using Weiqi.UI;

namespace Groot.Res
{
	partial class ResourceManager : MonoBehaviour
	{
		public static ResourceManager Instance { get; private set; }

		/// <summary>
		/// 由于Res的名称打包时都存储为小写，所以这里做忽略大小写查找，去掉每次Load时候调用ToLower的Alloc
		/// </summary>
		private readonly Dictionary<String, Resource_Base> m_res_dictionary = new Dictionary<String, Resource_Base>( StringComparer.OrdinalIgnoreCase );

		#region Unity API

		private void Awake()
		{
			if( Instance != null )
			{
				enabled = false;
				return;
			}
			Instance = this;
		}


		#endregion

		/// <summary>
		/// 存储的本地存储的res_config
		/// </summary>
		private ResConfigFile mLoadResConfig;

		Resource_Base _createResourceData( ResInfo _info )
		{
			switch( _info.ResType )
			{
			case ResourceType.AssetBundle:
				{
					return new Resource_Assetbundle( _info );
				}
			case ResourceType.GrtPackage:
				{
					return new Resource_GrtPackage( _info );
				}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// 初始化资源管理器
		/// </summary>
		/// <param name="_updateByAppConfig">是否使用app内config更新列表，在更新完成之后重新初始化时不需要更新</param>
		/// <returns></returns>
		public Boolean Initialize( Boolean _updateByAppConfig )
		{
			String appResConfigPath = String.Concat( Application.streamingAssetsPath, "/res/res_config.grt" );
			var assetbundleAppResConfig = AssetBundle.LoadFromFile( appResConfigPath );
			if( assetbundleAppResConfig == null )
			{
				Log.Critical( "[ResourceManager.Initialize]:读取inapp res config 失败! assetbundle 加载失败!" );
				return false;
			}

			var appResConfigBytes = assetbundleAppResConfig.LoadAsset<TextAsset>( "res_config" );
			if( appResConfigBytes == null )
			{
				Log.Critical( "[ResourceManager.Initialize]:读取inapp res config 失败! TextAsset加载失败" );
				assetbundleAppResConfig.Unload( true );
				return false;
			}
			var appResConfig = ResConfigFile.Create( appResConfigBytes.bytes );
			if( appResConfig == null )
			{
				Log.Critical( eLogType.Resources, "[ResourceManager.Initialize]: 读取inapp res config 失败! 解析失败!" );
				assetbundleAppResConfig.Unload( true );
				return false;
			}
			assetbundleAppResConfig.Unload( true );


			mLoadResConfig = ResConfigFile.Create( ResUtility.LocalResConfigPath, true );
			if( mLoadResConfig == null )
			{
				Log.Error( eLogType.Resources, "[ResouceManager]: Local res config file 创建失败!" );
				return false;
			}

			// 首次初始化时执行
			if( _updateByAppConfig )
				mLoadResConfig.UpdateByAppResConfig( appResConfig );


			mLoadResConfig.TraverseAssets( _info => {
				m_res_dictionary.Add( _info.Name, _createResourceData( _info ) );
			} );

			// 读取依赖
			var assetbundleManifest = AssetBundle.LoadFromFile( GetResource( "res.grt" ).ResouceInfo.SystemApiLoadingPath );
			AssetBundleManifest manifest = assetbundleManifest.LoadAsset<AssetBundleManifest>( "AssetBundleManifest" );
			foreach( Resource_Base res in m_res_dictionary.Values )
			{
				if( res.ResouceInfo.ResType != ResourceType.AssetBundle )
					continue;
				Resource_Assetbundle resAssetbundle = res as Resource_Assetbundle;
				var dependenciesName =  manifest.GetDirectDependencies( res.Name );
				for( int i = 0; i < dependenciesName.Length; i++ )
				{
					Resource_Assetbundle dependency = GetResource<Resource_Assetbundle>( dependenciesName[i] );
					if( dependency == null )
					{
						Log.Critical( eLogType.Resources, "[ResourceManager.Initialize]: {0}的依赖资源: {1}没有在ResourceConfig中找到! 请尝试重新打包!", res.Name, dependenciesName[i] );
						return false;
					}
					resAssetbundle.AddDependencies( dependency );
				}
			}
			assetbundleManifest.Unload( true );

			Log.Debug( eLogType.Resources, "ResourceManager Initialized Done!" );
			return true;
		}

		/// <summary>
		/// 异步初始化
		/// </summary>
		/// <param name="_completed_action"></param>
		public void AsyncInitialize( Action<Boolean> _completed_action )
		{
			StartCoroutine( _asyncInitialize( _completed_action ) );
		}

		/// <summary>
		/// 初始化ResourceManager, 加载包内和包外的res_config
		/// </summary>
		/// <param name="_action_initialized"></param>
		/// <returns></returns>
		private IEnumerator _asyncInitialize( Action<Boolean> _action_initialized )
		{
			String app_res_config_path = ResInfo.GetWWWLoadPath( "res_config", AssetLocation.StreamingPath );
			Log.Debug( eLogType.Resources, app_res_config_path );
			using( WWW www = new WWW( app_res_config_path ) )
			{
				yield return www;

				if( !String.IsNullOrEmpty( www.error ) ) // 加载错误
				{
					Log.Error( eLogType.Resources, "[ResourceManager]: In App res_config assetbundle 加载失败!: {0}", www.error );
					_action_initialized( false );
					yield break;
				}


				ResConfigFile app_res_config = ResConfigFile.Create( www.bytes );
				if( app_res_config == null ) // 解析错误
				{
					Log.Error( eLogType.Resources, "[ResourceManager]: In App res_config 解析错误!" );
					_action_initialized( false );
					yield break;
				}

				mLoadResConfig = ResConfigFile.Create( ResUtility.LocalResConfigPath, true );
				if( mLoadResConfig == null ) // 保存用assetsconfig 创建或读取错误
				{
					app_res_config = null;
					Log.Error( eLogType.Resources, "[ResouceManager]: Saved res_config file 创建或解析错误!" );
					_action_initialized( false );
					yield break;
				}
				_action_initialized( true );
			}
		}

		/// <summary>
		/// 获取资源列表
		/// Exception: AssetNotExistException
		/// </summary>
		/// <param name="_names"></param>
		/// <returns></returns>
		public List<Resource_Base> GetResource( IEnumerable<String> _names )
		{
			List<Resource_Base> re = new List<Resource_Base>();
			foreach( String s in _names )
			{
				re.Add( GetResource( s ) );
			}
			return re;
		}


		public T GetResource<T>( String _name ) where T : Resource_Base
		{
			return GetResource( _name ) as T;
		}

		/// <summary>
		/// 获取资源
		/// Exception: AssetNotExistException
		/// </summary>
		/// <param name="_name"></param>
		/// <returns></returns>
		public Resource_Base GetResource( String _name )
		{
			Resource_Base asset_info;
			if( !m_res_dictionary.TryGetValue( _name.ToLower(), out asset_info ) )
				throw new AssetNotExistException( _name );
			return asset_info;
		}

		/// <summary>
		/// 是否存在资源
		/// </summary>
		/// <param name="_name"></param>
		/// <returns></returns>
		public Boolean Contains( String _name )
		{
			return m_res_dictionary.ContainsKey( _name.ToLower() );
		}
	}
}