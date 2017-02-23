using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Utility;

namespace Groot.Res
{
	class GrtPackageLoadTask : IResourceLoadTask
	{
		private readonly Resource_GrtPackage m_resource;
		private readonly Predicate<Boolean, Resource_Base> m_callback;

		public GrtPackageLoadTask( Resource_GrtPackage _asset_data, Predicate<Boolean, Resource_Base> _callback )
		{
			m_resource = _asset_data;
			m_callback = _callback;
			if( _asset_data.State == ResourceState.Error )
				throw new Utility.ExceptionEx( "" );
			m_resource.Retain();
		}

		public Boolean CheckCompleted()
		{
			if( ResourceState.Loading == m_resource.State )
				return false;

			if( ResourceState.Loaded == m_resource.State )
			{
				Log.Debug( "[GrtPackageLoadTask]: 加载[{0}]成功!", m_resource.Name );
				if( m_callback( true, m_resource ) )
					return true;
				m_resource.Release();
				return true;
			}
			Log.Error( "[GrtPackageLoadTask]: 加载[{0}]失败! State为: {1}", m_resource.Name, m_resource.State.ToString() );
			// 错误处理
			m_resource.Release();
			return true;
		}
	}
}