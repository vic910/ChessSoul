using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Groot.Res
{
	class UnitySceneLoadTask : IResourceLoadTask
	{
		private readonly Resource_Assetbundle m_resource;
		private readonly String m_scene_name;
		private readonly Action<Boolean> m_callback;
		private Boolean m_scene_loaded = false;
		private AsyncOperation m_scene_load_opt = null;

		public UnitySceneLoadTask( Resource_Assetbundle _asset_data, String _scene_name, Action<Boolean> _callback )
		{
			m_resource = _asset_data;
			m_scene_name = _scene_name;
			m_callback = _callback;
			if( _asset_data.State == ResourceState.Error )
				throw new Utility.ExceptionEx( "" );
			m_resource.Retain();
		}

		public Boolean CheckCompleted()
		{
			if( ResourceState.Loaded == m_resource.State )
			{
				if( m_scene_loaded )
				{
					m_resource.Release();
					return true;
				}
				if( m_scene_load_opt != null )
					return false;
				ResourceManager.Instance.StartCoroutine( _loadSceneAsync() );
				return false;
			}
			else if( ResourceState.Error == m_resource.State )
			{
				m_callback( false );
				return true;
			}
			return false;
		}

		private IEnumerator _loadSceneAsync()
		{
			m_scene_load_opt = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync( m_scene_name, LoadSceneMode.Additive );
			if( m_scene_load_opt == null )
			{
				m_callback( false );
				yield break;
			}
			yield return m_scene_load_opt;
			m_callback( true );
		}
	}
}