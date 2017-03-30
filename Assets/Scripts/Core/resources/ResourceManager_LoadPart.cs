using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Groot.Res
{
	interface IResourceLoadTask
	{
		Boolean CheckCompleted();
	}
	partial class ResourceManager : MonoBehaviour
	{
		private readonly LinkedList<Resource_Base> m_pending_delete_list = new LinkedList<Resource_Base>();
		private readonly LinkedList<Resource_Base> m_loaded_assets = new LinkedList<Resource_Base>();
		private readonly LinkedList<IResourceLoadTask> m_running_task = new LinkedList<IResourceLoadTask>();

		private void LateUpdate()
		{
			// 处理Task列表
			LinkedListNode<IResourceLoadTask> loading_task_node = m_running_task.First;
			while( loading_task_node != null )
			{
				if( loading_task_node.Value.CheckCompleted() )
				{
					var tmp = loading_task_node.Next;
					m_running_task.Remove( loading_task_node );
					loading_task_node = tmp;
					continue;
				}
				loading_task_node = loading_task_node.Next;
			}

			// 处理Resource定时删除
			LinkedListNode<Resource_Base> loaded_asset_node = m_loaded_assets.First;
			while( loaded_asset_node != null )
			{
				var resource_node = loaded_asset_node;
				loaded_asset_node = loaded_asset_node.Next;

				if( resource_node.Value.RefConut > 0 )
					continue;

				if( resource_node.Value.DeleteImmediately )
				{
					Log.Warning( eLogType.Resources, "清理Resource: {0}", resource_node.Value.Name );
					resource_node.Value.Unload();
				}
				else
				{
					resource_node.Value.DeleteTimer = 5f;
					_addToPendingUnloadResourceList( resource_node.Value );
				}
				m_loaded_assets.Remove( resource_node );
			}

			LinkedListNode<Resource_Base> pending_delete_asset = m_pending_delete_list.First;
			while( pending_delete_asset != null )
			{
				var tmp = pending_delete_asset;
				pending_delete_asset = pending_delete_asset.Next;
				tmp.Value.DeleteTimer -= Time.deltaTime;

				// 复活了的就不砍了, 没活过来的就没法了
				if( tmp.Value.RefConut > 0 )
				{
					_addToLoadedResourceList( tmp.Value );
					continue;
				}

				// 没到行刑时间的不砍~
				if( tmp.Value.DeleteTimer > 0f )
					continue;

				Log.Warning( eLogType.Resources, "清理Resource: {0}", tmp.Value.Name );
				tmp.Value.Unload();
				m_pending_delete_list.Remove( tmp );
				break;
			}
		}

		public void _addToLoadedResourceList( Resource_Base _asset )
		{
			if( m_loaded_assets.Contains( _asset ) )
				return;
			m_loaded_assets.AddLast( _asset );
		}

		public void _addToPendingUnloadResourceList( Resource_Base _resource )
		{
			if( m_pending_delete_list.Contains( _resource ) )
				return;
			m_pending_delete_list.AddLast( _resource );
		}

		public void LoadAssetbundleAsync( String _assetbundle_name, Predicate<Boolean, List<Resource_Assetbundle>> _callback )
		{
			Resource_Assetbundle res_assetbundle = GetResource( _assetbundle_name ) as Resource_Assetbundle;
			if( res_assetbundle == null )
			{
				Log.Error( "[ResouceManager]: res [{0}] 不存在！或类型不为Assetbundle", _assetbundle_name );
				_callback( false, null );
				return;
			}

#if UNITY_EDITOR
			// 其实没有意义的验证, 除非Resource_assetbundle和内部存储的类型不同
			if( res_assetbundle.ResouceInfo.ResType != ResourceType.AssetBundle )
			{
				_callback( false, null );
				Log.Error( "[ResouceManager]: 请求加载的Assetbundle: {0} 非Assetbundle资源! 类型为: {1}", _assetbundle_name, res_assetbundle.ResouceInfo.ResType );
				return;
			}
#endif
			var task = new UnityAssetBundleLoadTask( res_assetbundle, _callback );
			m_running_task.AddLast( task );
		}

		/// <summary>
		/// 同步加载Assetbundle
		/// </summary>
		/// <param name="_name"></param>
		/// <returns></returns>
		public Resource_Assetbundle LoadAssetbundleSync( String _assetbundle_name )
		{
			Resource_Assetbundle res_assetbundle = GetResource( _assetbundle_name ) as Resource_Assetbundle;
			if( res_assetbundle == null )
			{
				Log.Error( "[ResouceManager]: res [{0}] 不存在！或类型不为Assetbundle", _assetbundle_name );
				return null;
			}
			res_assetbundle.LoadResourceSync();
			return res_assetbundle;
		}

		public void LoadSceneAsync( String _assetbundle_name, String _scene_name, Action<Boolean> _callback )
		{
			//_scene_name = _scene_name.ToLower();
#if GROOT_ASSETBUNDLE_SIMULATION || !UNITY_EDITOR
			StartCoroutine( _loadSceneAsync( _scene_name, _callback ) );
#else
			Resource_Base resource_assetbundle = GetResource( _assetbundle_name );
			if( resource_assetbundle == null )
			{
				_callback( false );
				return;
			}
			var task = new UnitySceneLoadTask( resource_assetbundle as Resource_Assetbundle, _scene_name , _callback );
			m_running_task.AddLast( task );
#endif
		}

		private IEnumerator _loadSceneAsync( String _scene_name, Action<Boolean> _callback )
		{
			AsyncOperation opt = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync( _scene_name, LoadSceneMode.Additive );
			if( opt == null )
			{
				_callback( false );
				yield break;
			}
			yield return opt;
			_callback( true );
		}


		/// <summary>
		/// 从Assetbundle中异步加载一个资源，自动处理依赖关系
		/// </summary>
		/// <typeparam name="T">UnityAsset类型</typeparam>
		/// <param name="_assetbundle_name">目标Assetbundle名称</param>
		/// <param name="_asset_name">Asset名称</param>
		/// <param name="_callback">回调</param>
		public void LoadAssetAsync<T>( String _assetbundle_name, String _asset_name, Predicate<Boolean, Resource_Assetbundle, UnityEngine.Object> _callback ) where T : UnityEngine.Object
		{
			Resource_Base res_assetbundle = GetResource( _assetbundle_name ) as Resource_Assetbundle;
			if( res_assetbundle == null )
			{
				_callback( false, null, null );
				Log.Error( "[ResouceManager.LoadAssetAsync<T>]: res [{0}] 不存在！或类型不为Assetbundle", _assetbundle_name );
				return;
			}

#if UNITY_EDITOR
			// 其实没有意义的验证, 除非Resource_assetbundle和内部存储的类型不同
			if( res_assetbundle.ResouceInfo.ResType != ResourceType.AssetBundle )
			{
				_callback( false, null, null );
				Log.Error( "[AssetManager]: 请求加载的Assetbundle: {0} Asset: {1} 为非Assetbundle资源! 类型为: {2}", _assetbundle_name, _asset_name, res_assetbundle.ResouceInfo.ResType );
				return;
			}
#endif

			var task = new UnityAssetLoadTask( res_assetbundle as Resource_Assetbundle, _asset_name, typeof(T), _callback );

			m_running_task.AddLast( task );
		}

		public void LoadGrtPackageAsync( String _name, Predicate<Boolean, Resource_Base> _callback )
		{
			Resource_GrtPackage res = GetResource( _name ) as Resource_GrtPackage;
			if( res == null )
			{
				_callback( false, null );
				Log.Error( "[ResouceManager.LoadGrtPackageAsync]: res [{0}] 不存在或类型不为GrtPackage!", _name );
				return;
			}
			var task = new GrtPackageLoadTask( res, _callback );
			m_running_task.AddLast( task );
		}

		private void _unloadAllAsset()
		{
			m_loaded_assets.Clear();
			m_pending_delete_list.Clear();
			m_res_dictionary.Clear();
			GC.Collect();
			Resources.UnloadUnusedAssets();
		}

		public void Uninitialize()
		{
			mLoadResConfig.Clear();
			m_res_dictionary.Clear();
			m_running_task.Clear();
			_unloadAllAsset();
		}
	}
}