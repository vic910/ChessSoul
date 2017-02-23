using System;
using System.Collections.Generic;

using UnityEngine;

using Utility;

namespace Groot.Res
{
	class UnityAssetBundleLoadTask : IResourceLoadTask
	{
		/// <summary>
		/// 单个加载文件任务
		/// </summary>
		/// <param name="_asset"></param>
		/// <param name="_on_complete_action">全部加载完成的回调, arg1: 是否成功加载全部, arg2: 所加载的AssetRuntimeData列表, 无论加载成功或者失败, 返回值: 是否保持Assetbundle引用</param>
		public UnityAssetBundleLoadTask( Resource_Assetbundle _asset, Predicate<Boolean, List<Resource_Assetbundle>> _on_complete_action )
		{
			m_asset_list.Add( _asset );
			m_on_completed_action = _on_complete_action;
			_requireLoad();
		}

		/// <summary>
		/// 多个加载文件任务
		/// </summary>
		/// <param name="_assets"></param>
		/// <param name="_on_complete_action">全部加载完成的回调, arg1: 是否成功加载全部, arg2: 所加载的AssetRuntimeData列表, 无论加载成功或者失败, 返回值: 是否保持Assetbundle引用</param>
		public UnityAssetBundleLoadTask( IEnumerable<Resource_Assetbundle> _assets, Predicate<Boolean, List<Resource_Assetbundle>> _on_complete_action )
		{
			m_asset_list.AddRange( _assets );
			m_on_completed_action = _on_complete_action;
			_requireLoad();
		}

		private void _requireLoad()
		{
			for( int i = 0; i < m_asset_list.Count; i++ )
			{
				//if( m_asset_list[i].State == ResourceState.Loaded
				//	|| m_asset_list[i].State == ResourceState.PendingUnload )
				//{
				//	m_asset_list[i].Retain();
				//	continue;
				//}
				//m_loading_list.AddLast( m_asset_list[i] );
				if( m_asset_list[i].State == ResourceState.Loading || m_asset_list[i].State == ResourceState.Loaded )
					continue;
				m_asset_list[i].Retain();
				m_loading_list.AddLast( m_asset_list[i] );
			}
		}

		public Boolean CheckCompleted()
		{
			var node = m_loading_list.First;
			while( node != null )
			{
				try
				{
					if( node.Value.CheckLoaded() )
					{
						//node.Value.Retain();
						var tmp = node.Next;
						m_loading_list.Remove( node );
						node = tmp;
					}
					else
					{
						node = node.Next;
					}
				}
				catch( Exception )
				{
					goto FAIL;
				}

			}
			if( m_loading_list.Count != 0 )
				return false;

			// 处理回调
			// 如果保持引用，直接返回
			if( m_on_completed_action( true, m_asset_list ) )
				return true;
			for( int i = 0; i < m_asset_list.Count; i++ )
			{
				m_asset_list[i].Release();
			}
			return true;

			// 如果1个失败则整个Task失败，所有Assetbunle调用Release
			FAIL:
			// 处理当前Task所需要加载的资源，全部release掉
			for( int i = 0; i < m_asset_list.Count; i++ )
			{
				m_asset_list[i].Release();
			}
			return true;
		}

		private readonly List<Resource_Assetbundle> m_asset_list = new List<Resource_Assetbundle>();
		private readonly LinkedList<Resource_Assetbundle> m_loading_list = new LinkedList<Resource_Assetbundle>();
		private readonly Predicate<Boolean, List<Resource_Assetbundle>> m_on_completed_action;
	}
}