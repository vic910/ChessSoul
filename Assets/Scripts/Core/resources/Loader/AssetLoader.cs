using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Utility;
using Weiqi;

namespace Groot.Res
{
	class AssetLoader : UnitySingleton<AssetLoader>
	{
		class LoadRequireData
		{
			public Resource_Assetbundle Resource;
			public String AssetName;
			public Type Type;
		}

		/// <summary>
		/// 某个AssetBundle加载完成时事件(无论这个AssetBundle是否成功加载)
		/// 当加载失败时arg2 不为null, 并包含错误信息
		/// </summary>
		//public event Action<AssetData, String> eventOnAnAssetBundleLoaded;


		private Queue<LoadRequireData> m_pending_asset = new Queue<LoadRequireData>();

		private LoadRequireData m_current_loading;

		/// <summary>
		/// 异步加载bundle中的一个资源
		/// </summary>
		/// <param name="_resouce"></param>
		/// <param name="_asset_name"></param>
		/// <param name="_type"></param>
		public void LoadAsset( Resource_Assetbundle _resouce, String _asset_name, Type _type )
		{
			// 等待中或者正在加载的被跳过
			if( m_current_loading != null && m_current_loading.Resource == _resouce && m_current_loading.AssetName == _asset_name )
				return;

			foreach( LoadRequireData load_require_data in m_pending_asset )
			{
				if( load_require_data.Resource == _resouce && _asset_name == load_require_data.AssetName )
					return;
			}

			m_pending_asset.Enqueue( new LoadRequireData() { Resource = _resouce, AssetName = _asset_name, Type = _type } );
		}

		/// <summary>
		/// 同步加载bundle中的一个资源
		/// </summary>
		/// <param name="_resouce"></param>
		/// <param name="_asset_name"></param>
		public UnityEngine.Object LoadAssetSync( Resource_Assetbundle _resouce, String _asset_name, Type _type )
		{
			UnityEngine.Object asset = _resouce.Assetbundle.LoadAsset( _asset_name, _type );
			//_type asset = _resouce.Assetbundle.LoadAsset<_type>( _asset_name );
			_resouce.OnAssetLoaded( asset );
			return asset;
		}

		public void Update()
		{
			if( m_current_loading != null || m_pending_asset.Count == 0 )
				return;
			m_current_loading = m_pending_asset.Dequeue();
			StartCoroutine( _loadAssetBundle( m_current_loading ) );
		}

		private IEnumerator _loadAssetBundle( LoadRequireData _require_data )
		{
			AssetBundleRequest asset_load_request = _require_data.Resource.Assetbundle.LoadAssetAsync( _require_data.AssetName, _require_data.Type );
			yield return asset_load_request;

			if( asset_load_request.asset == null )
			{
				Log.Error( eLogType.Resources, "从Assetbundle: [{0}]中加载Asset: [{1}]失败!", _require_data.Resource.ResouceInfo.Name, _require_data.AssetName );
				_require_data.Resource.OnAssetLoaded( null );
				yield break;
			}

			Log.Debug( eLogType.Resources, "成功从Assetbundle: [{0}]中加载Asset: [{1}]!", _require_data.Resource.ResouceInfo.Name, _require_data.AssetName );
			asset_load_request.asset.name = asset_load_request.asset.name.ToLower();
			_require_data.Resource.OnAssetLoaded( asset_load_request.asset );
			m_current_loading = null;
		}
	}
}