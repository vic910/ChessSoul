using System;
using System.Collections.Generic;
using System.Collections;
using Groot.Res;
using UnityEngine;
using Utility;
using Weiqi;

namespace Groot.Res
{
	class GrtPackageLoader : UnitySingleton<GrtPackageLoader>
	{
		private Queue<Resource_GrtPackage> m_pending_asset = new Queue<Resource_GrtPackage>();
		private Resource_GrtPackage m_current_loading;

		public void LoadBinaryFile( Resource_GrtPackage _assetbundle )
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
			StartCoroutine( _loadFileBinaryFile( m_current_loading ) );
		}

		private IEnumerator _loadFileBinaryFile( Resource_GrtPackage _assetbundle_data )
		{
			using( WWW www = new WWW( _assetbundle_data.ResouceInfo.UnityApiLoadingPath ) )
			{
				yield return www;
				if( www.error != null )
					Utility.Log.Error( Utility.eLogType.Resources, "Binary file  [{0}] load fail:! Msg: {1}", _assetbundle_data.ResouceInfo.UnityApiLoadingPath, www.error );
				_assetbundle_data.OnLoadFinish( www.error != null ? null : GrtPackge.CreateFromeMemory( www.bytes ) );
			}
			m_current_loading = null;
		}
	}
}