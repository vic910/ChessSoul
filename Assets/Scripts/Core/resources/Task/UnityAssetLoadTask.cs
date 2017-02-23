using System;
using System.Collections;

using UnityEngine;

using Utility;

namespace Groot.Res
{
	class UnityAssetLoadTask : IResourceLoadTask
	{
		private readonly Resource_Assetbundle m_res_assetbundle;
		private readonly String m_asset_name;
		private Type m_asset_type;
		private readonly Predicate<Boolean, Resource_Assetbundle, UnityEngine.Object> m_callback;
		private Boolean m_asset_loading = false;
		public UnityAssetLoadTask( Resource_Assetbundle _res_assetbundle, String _asset_name, Type _type, Predicate<Boolean, Resource_Assetbundle, UnityEngine.Object> _callback )
		{
			m_res_assetbundle = _res_assetbundle;
			m_asset_name = _asset_name;
			m_callback = _callback;
			m_asset_type = _type;
			m_res_assetbundle.Retain();
		}

		/// <summary>
		/// Assetbundle加载完成后, 同步加载Asset
		/// </summary>
		/// <returns></returns>
		public Boolean CheckCompleted()
		{
			try
			{
				if( !m_res_assetbundle.CheckLoaded() )
					return true;
				UnityEngine.Object asset = m_res_assetbundle.GetLoadedAsset( m_asset_name );
				if( asset != null )
				{
					//asset.name = asset.name.ToLower();
					if( !m_callback( true, m_res_assetbundle, asset ) )
						return true;
					m_res_assetbundle.Release();
					return true;
				}
				if( !m_asset_loading )
				{
					m_asset_loading = true;
					m_res_assetbundle.LoadAsset( m_asset_name, m_asset_type );
				}
				return false;
			}
			catch( Exception )
			{
				m_callback( false, m_res_assetbundle, null );
				m_res_assetbundle.Release();
				return true;
			}
		}
	}
}