using System;
using System.Collections.Generic;
using Groot.Res;
using UnityEngine;
using Utility;
using Object = UnityEngine.Object;

namespace Groot.Res
{
	enum ResourceState
	{
		/// <summary>
		/// 没有加载
		/// </summary>
		NotLoaded,

		/// <summary>
		/// 加载中
		/// </summary>
		Loading,

		/// <summary>
		/// 已加载(不包括依赖资源)
		/// </summary>
		Loaded,

		/// <summary>
		/// 等待卸载
		/// </summary>
		PendingUnload,

		/// <summary>
		/// 加载失败状态
		/// </summary>
		Error,
	}

	/// <summary>
	/// 资源的运行时数据
	/// </summary>
	abstract class Resource_Base
	{
		protected Resource_Base( ResInfo _resouce_info )
		{
			ResouceInfo = _resouce_info;
			RefConut = 0;
			DeleteImmediately = _resouce_info.ResType != ResourceType.AssetBundle;
		}

		public String Name { get { return ResouceInfo.Name; } }
		public Int32 RefConut { get; protected set; }

		public ResInfo ResouceInfo { get; protected set; }

		public ResourceState State { get; protected set; }

		public Boolean DeleteImmediately { get; private set; }

		public Single DeleteTimer { get; set; }

		public abstract void LoadResource();
		/// <summary>
		/// 同步加载
		/// </summary>
		public abstract bool LoadResourceSync();

		public virtual void Release()
		{
			RefConut--;
			if( RefConut < 0 )
#if UNITY_EDITOR
				throw new Utility.ExceptionEx( "[Resource_Base.Release]: {0} RefCount is already 0 when release! :", ResouceInfo.Name );
#else
				RefConut = 0;
#endif
			if( RefConut == 0 )
				State = ResourceState.PendingUnload;
		}

		public virtual void Retain()
		{
			RefConut++;
			if( State == ResourceState.PendingUnload )
				State = ResourceState.Loaded;
		}

		public abstract void Unload( bool _unload_all = false );
	}

	class Resource_Assetbundle : Resource_Base
	{
		public Resource_Assetbundle( ResInfo _res_info )
			: base( _res_info )
		{ }

		public AssetBundle Assetbundle { get; private set; }

		private readonly List<UnityEngine.Object> m_loaded_asset = new List<UnityEngine.Object>();
		private readonly List<Resource_Assetbundle>  m_dependencies = new List<Resource_Assetbundle>();

		public override void LoadResource()
		{
			AssetbundleLoader.Instance.LoadAssetbundle( this );
			State = ResourceState.Loading;
		}

		public override bool LoadResourceSync()
		{
			if ( Assetbundle != null )
				return true;
			bool success = true;
			for ( int i = 0; i < m_dependencies.Count; i++ )
			{
				if( m_dependencies[i].State == ResourceState.Loaded )
					continue;
				success &= m_dependencies[i].LoadResourceSync();
			}
			if ( !success )
				return false;
			Assetbundle = AssetBundle.LoadFromFile( ResouceInfo.SystemApiLoadingPath );
			success = Assetbundle != null ? true : false;
			OnLoadFinished( Assetbundle, success );
			return success;
		}

		public GameObject LoadUI( String _ui_name )
		{
			Assetbundle = AssetBundle.LoadFromFile( ResouceInfo.SystemApiLoadingPath );
			return Assetbundle.LoadAsset<GameObject>( _ui_name );
		}

		public void OnLoadFinished( AssetBundle _asset_bundle, Boolean _succeed )
		{
			Assetbundle = _asset_bundle;

			if( _succeed )
			{
				Utility.Log.LogInfo( eLogLevel.Debug, eLogType.Resources, String.Concat( "AssetbundleLoaded: [", Name, "]" ) );
				State = ResourceState.Loaded;
			}
			else
			{
				State = ResourceState.Error;
				Utility.Log.LogInfo( eLogLevel.Debug, eLogType.Resources, String.Concat( "AssetbundleLoaded: [", Name, "]" ) );
			}
		}

		/// <summary>
		/// 检测Assetbundle是否加载完成可使用了(包括依赖资源是否可用)
		/// </summary>
		/// <returns>Loaded为可用， Loading为不可用，其他状态抛出异常</returns>
		public bool CheckLoaded()
		{
			return _checkLoaded( this );
		}

		/// <summary>
		/// 加载完成检测，传入root是为了处理循环依赖
		/// </summary>
		/// <param name="_root"></param>
		/// <returns></returns>
		private bool _checkLoaded( Resource_Assetbundle _root )
		{
			Boolean all_loaded = State == ResourceState.Loaded;
			for( int i = 0; i < m_dependencies.Count; i++ )
			{
				if( m_dependencies[i].State == ResourceState.Loaded )
					continue;

				all_loaded = false;
				if( m_dependencies[i].State != ResourceState.Loading )
					throw new ExceptionAssetBundleDependenciesStateError( m_dependencies[i].State );
				break;
			}
			return all_loaded;
		}

		/// <summary>
		/// Asset加载完成回调
		/// </summary>
		/// <param name="_asset"></param>
		public void OnAssetLoaded( UnityEngine.Object _asset )
		{
			if( _asset != null )
				m_loaded_asset.Add( _asset );
		}

		/// <summary>
		/// 异步加载一个资源
		/// </summary>
		/// <param name="_asset_name"></param>
		/// <param name="_type"></param>
		public void LoadAsset( String _asset_name, Type _type )
		{
			AssetLoader.Instance.LoadAsset( this, _asset_name, _type );
		}

		/// <summary>
		/// 同步加载一个资源
		/// </summary>
		/// <param name="_asset_name"></param>
		/// <param name="_type"></param>
		/// <returns></returns>
		public UnityEngine.Object LoadAssetSync( String _asset_name, Type _type )
		{
			UnityEngine.Object asset = GetLoadedAsset( _asset_name );
			if (asset != null)
				return asset;
			return AssetLoader.Instance.LoadAssetSync( this, _asset_name, _type );
		}

		public UnityEngine.Object GetLoadedAsset( String _name )
		{
			return m_loaded_asset.Find( _o => _name.Equals( _o.name, StringComparison.OrdinalIgnoreCase ) );
		}

		public void AddDependencies( Resource_Assetbundle _res_assetbundle )
		{
			if( m_dependencies.Contains( _res_assetbundle ) )
			{
				Utility.Log.Error( "[Resource_Assetbundle] Dependencies: {0} 已经被添加!", _res_assetbundle.Name );
				return;
			}
			m_dependencies.Add( _res_assetbundle );
		}

		public Resource_Assetbundle[] GetDependencies()
		{
			return m_dependencies.ToArray();
		}


		public override void Release()
		{
			base.Release();
			_release( this );
		}

		/// <summary>
		/// 处理release, 这里把根的对象传进来，避免循环依赖导致的死循环
		/// </summary>
		/// <param name="_root"></param>
		private void _release( Resource_Assetbundle _root )
		{
			for( int i = 0; i < m_dependencies.Count; i++ )
			{
				if( m_dependencies[i] == _root )
					continue;
				m_dependencies[i]._release( _root );
			}
		}


		public override void Retain()
		{
			base.Retain();
			_retain( this );
		}

		/// <summary>
		/// 处理retain, 这里把根的对象传进来，避免循环依赖导致的死循环
		/// </summary>
		/// <param name="_root"></param>
		private void _retain( Resource_Assetbundle _root )
		{
			for( int i = 0; i < m_dependencies.Count; i++ )
			{
				// 断掉循环依赖
				if( m_dependencies[i] == _root )
					continue;
				m_dependencies[i]._retain( _root );
			}

			// 如果未加载，就加载
			if( State != ResourceState.Loaded && State != ResourceState.Loading )
				LoadResource();
		}

		public override void Unload( bool _unload_all = false )
		{
			if( _unload_all )
				m_loaded_asset.Clear();
			if( Assetbundle != null )
				Assetbundle.Unload( _unload_all );
			Assetbundle = null;
			State = ResourceState.NotLoaded;
		}
	}

	class Resource_GrtPackage : Resource_Base
	{
		public Resource_GrtPackage( ResInfo _res_info )
			: base( _res_info )
		{ }
		public GrtPackge Package { get; set; }

		public override void Retain()
		{
			base.Retain();
			if( State != ResourceState.Loaded && State != ResourceState.Loading )
				LoadResource();
		}

		public override void Unload( bool _unload_all = false )
		{
			Package = null;
			State = ResourceState.NotLoaded;
		}

		public override void LoadResource()
		{
			GrtPackageLoader.Instance.LoadBinaryFile( this );
			State = ResourceState.Loading;
		}

		public override bool LoadResourceSync()
		{
			return true;
		}

		public void OnLoadFinish( GrtPackge _package )
		{
			Package = _package;
			State = _package == null ? ResourceState.Error : ResourceState.Loaded;
		}
	}
}