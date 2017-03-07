using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Utility;

namespace Groot.Res
{
	class AssetbundleLoader : MonoBehaviour
	{
		public static AssetbundleLoader Instance { get; private set; }

		private Queue<Resource_Assetbundle> m_pending_asset = new Queue<Resource_Assetbundle>();

		private Resource_Assetbundle m_current_loading;

		public void Awake()
		{
			if( Instance != null )
			{
				enabled = false;
				return;
			}
			Instance = this;
		}

		public void LoadAssetbundle( Resource_Assetbundle _assetbundle )
		{
			// 等待中或者正在加载的被跳过
			if( m_pending_asset.Contains( _assetbundle ) || m_current_loading == _assetbundle )
				return;
			m_pending_asset.Enqueue( _assetbundle );
		}

		public void Update()
		{
			if( m_current_loading != null || m_pending_asset.Count == 0 )
				return;
			m_current_loading = m_pending_asset.Dequeue();
			StartCoroutine( _loadAssetBundle( m_current_loading ) );
		}

		private IEnumerator _loadAssetBundle( Resource_Assetbundle _assetbundle_data )
		{
			AssetBundle loaded_asset_bundle = null;
			//if( Application.platform == RuntimePlatform.Android && _assetbundle_data.ResouceInfo.Location == AssetLocation.StreamingPath )
			//{
			//	using( WWW www = new WWW( _assetbundle_data.ResouceInfo.UnityApiLoadingPath ) )
			//	{
			//		yield return www;

			//		if( www.error == null )
			//			loaded_asset_bundle = www.assetBundle;
			//		else
			//			Log.Error( eLogType.Resources, "Load Fail: {0}! Msg: {1}", _assetbundle_data.ResouceInfo.UnityApiLoadingPath, www.error );
			//	}
			//}
			//else
			//{
			AssetBundleCreateRequest bundle_load_request = AssetBundle.LoadFromFileAsync( _assetbundle_data.ResouceInfo.SystemApiLoadingPath );
			yield return bundle_load_request;
			loaded_asset_bundle = bundle_load_request.assetBundle;
			if( loaded_asset_bundle == null )
				Log.Error( eLogType.Resources, "Load Fail: {0}! Msg: Assetbundle 为空", _assetbundle_data.ResouceInfo.SystemApiLoadingPath );
			//}
			_assetbundle_data.OnLoadFinished( loaded_asset_bundle , loaded_asset_bundle != null );
			m_current_loading = null;
		}
	}
}