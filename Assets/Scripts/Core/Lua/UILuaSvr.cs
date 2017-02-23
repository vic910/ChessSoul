using System.IO;
using System.Text;
using Groot.Res;
using SLua;
using Utility;

namespace Slua
{
	using System;
	using System.Threading;
	using System.Collections;
	using System.Collections.Generic;
	using LuaInterface;
	using System.Reflection;
#if !SLUA_STANDALONE
	using UnityEngine;
	using Debug = UnityEngine.Debug;

#endif

	public enum LuaSvrFlag
	{
		LSF_BASIC = 0,
		LSF_DEBUG = 1,
		LSF_EXTLIB = 2,
		LSF_3RDDLL = 4
	};

	public class UILuaSvr : MonoBehaviour
	{
		public static UILuaSvr Instance { get; private set; }

		private LuaState mLuaState;
		private Dictionary<String, LuaTable> mUiScripts = new Dictionary<String, LuaTable>();
		private List<String> mOtherScripts = new List<String>(); 

		private int mErrorReported = 0;

		public bool Inited { get; private set; }

		public bool LuaFileLoaded { get; private set; }


		void Awake()
		{
			Instance = this;
		}

		public void Uninitialize()
		{
			checkTop( mLuaState.L );
			mLuaState.Dispose();
			mLuaState = null;
			mUiScripts.Clear();
			mOtherScripts.Clear();
		}

		public LuaTable GetScript( String _name )
		{
			LuaTable script;
			if( !mUiScripts.TryGetValue( _name, out script ) )
			{
				Log.Error( "尝试获取ui script: {0}失败！", _name );
				return null;
			}
			return script;
		}

#if !UNITY_EDITOR || GROOT_ASSETBUNDLE_SIMULATION
		/// <summary>
		///     初始化并加载lua脚本
		/// </summary>
		/// <param name="_onCompleted">完成回调</param>
		/// <param name="_loadAllScripts">是否加载所有脚本</param>
		public void InitAndLoadLuaFile( Action<Boolean> _onCompleted, Boolean _loadAllScripts )
		{
			mLuaState = new LuaState();
			IntPtr L = mLuaState.L;
			LuaObject.init( L );
			mLoadAllScripts = _loadAllScripts;
			mOnLoadCompleted = _onCompleted;
			ThreadPool.QueueUserWorkItem( doBind, L );
		}

		public void LoadRemainLuaScripts( Action<Boolean> _onCompleted )
		{
			mOnLoadCompleted = _onCompleted;
			mLoadAllScripts = true;
			ThreadPool.QueueUserWorkItem( loadLuaScripts, new object[] {( (Resource_GrtPackage) mResLua ).Package, mLoadAllScripts} );
		}

		private void onBindComplted()
		{
			Debug.LogWarning( "dobind完成!" );
			_doinit( mLuaState.L, LuaSvrFlag.LSF_BASIC );
			checkTop( mLuaState.L );
			ResourceManager.Instance.LoadGrtPackageAsync( "scripts/ui.grt", onScriptPkgLoaded );
		}

		private Boolean onScriptPkgLoaded( Boolean _b, Resource_Base _resourceBase )
		{
			Debug.LogWarning( "onScriptPkgLoaded完成!" );
			mResLua = _resourceBase;
			mResLua.Retain();
			ThreadPool.QueueUserWorkItem( loadLuaScripts, new object[] {( (Resource_GrtPackage) mResLua ).Package, mLoadAllScripts} );
			return true;
		}

		private Resource_Base mResLua;
		private Boolean mLoadAllScripts;
		private Action<Boolean> mOnLoadCompleted;

		/// <summary>
		///     加载脚本
		/// </summary>
		/// <param name="_args"></param>
		private void loadLuaScripts( object _args )
		{
			Debug.LogWarning( "开始加载脚本!" );
			var args = (object[]) _args;
			GrtPackge pkg = (GrtPackge) args[0];
			Boolean loadAll = (Boolean) args[1];
			var cfg = pkg.LoadFile( "lua_config.txt" );
			if( cfg == null )
				Dispatcher.InvokeAsync( onLuaScriptsLoadFail );
			using( MemoryStream sm = new MemoryStream( cfg ) )
			{
				using( StreamReader sr = new StreamReader( sm, Encoding.UTF8 ) )
				{
					while( !sr.EndOfStream )
					{
						String luaName = sr.ReadLine();
						if( String.IsNullOrEmpty( luaName ) )
							continue;
						if( luaName[0] == '/' && luaName[1] == '/' ) //注释符号
							continue;
						if( !loadAll && luaName[luaName.Length - 1] != '#' ) //是否初始必须加载
							continue;
						luaName = luaName.Substring( 0, luaName.Length - ( luaName[luaName.Length - 1] == '#' ? 5 : 4 ) );
						if( mUiScripts.ContainsKey( luaName ) || mOtherScripts.Contains( luaName ) )
							continue;

						//Debug.LogWarningFormat( "加载脚本: {0}", luaName );
						String loadWithExt = luaName + ".lua";
						var bytes = pkg.LoadFile( loadWithExt );

						try
						{
							lock( mLuaState )
							{
								var table = mLuaState.doFile( bytes, luaName );
								if ( table != null )
								{
									lock ( mUiScripts )
										mUiScripts.Add(luaName, (LuaTable) table);
								}
								else
								{
									lock ( mOtherScripts )
										mOtherScripts.Add( luaName );
								}
							}
						}
						catch( Exception e )
						{
							Debug.LogError( String.Concat( "lua文件: ", luaName,"加载失败! 错误: ", e.Message ) );
							Dispatcher.InvokeAsync( onLuaScriptsLoadFail );
							return;
						}
						pkg.UnloadFile( loadWithExt );
					}
				}
			}
			Dispatcher.InvokeAsync( onLuaScriptsLoadSucceed );
		}

		private void onLuaScriptsLoadSucceed()
		{
			Debug.Log( "加载脚本成功!" );
			mOnLoadCompleted( true );
			mOnLoadCompleted = null;
			if( !mLoadAllScripts )
				return;
			mResLua.Release();
			mResLua = null;
		}

		private void onLuaScriptsLoadFail()
		{
			Debug.LogError( "加载脚本失败!" );
			mOnLoadCompleted( false );
			mOnLoadCompleted = null;
			mResLua.Release();
			mResLua = null;
		}
#endif

#if !GROOT_ASSETBUNDLE_SIMULATION && UNITY_EDITOR
		private Action<Boolean> mOnLoadCompleted;
		/// <summary>
		/// 初始化并加载lua脚本
		/// </summary>
		/// <param name="_onCompleted">完成回调</param>
		/// <param name="_loadAllScripts">是否加载所有脚本</param>
		public void InitAndLoadLuaFile( Action<Boolean> _onCompleted, Boolean _loadAllScripts )
		{
			mLuaState = new LuaState();
			IntPtr L = mLuaState.L;
			LuaObject.init( L );
			mOnLoadCompleted = _onCompleted;
			ThreadPool.QueueUserWorkItem( doBind, L );
		}

		public void LoadRemainLuaScripts( Action<Boolean> _onCompleted )
		{
			mOnLoadCompleted = _onCompleted;
			onLuaScriptsLoadSucceed();
		}

		private void onBindComplted()
		{
			Debug.LogWarning( "dobind完成!" );
			_doinit( mLuaState.L, LuaSvrFlag.LSF_BASIC );
			checkTop( mLuaState.L );
			LoadLuaFiles();
			//ResourceManager.Instance.LoadGrtPackageAsync( "scripts/ui.grt", onScriptPkgLoaded );
		}

		public void LoadLuaFiles()
		{
			// 这里要改，需要有依赖
			var files = FileHelper.GetFiles( WeiqiApp.LUA_ROOT , "*.lua" );
			Int32 start_idx = files[0].FullName.Replace( '\\', '/' ).IndexOf( WeiqiApp.LUA_ROOT, StringComparison.OrdinalIgnoreCase );
			foreach( FileInfo file_info in files )
			{
				var bytes = FileHelper.ReadAllBytesFromFile( file_info.FullName );
				var name = file_info.FullName.Substring( start_idx + WeiqiApp.LUA_ROOT.Length + 1, file_info.FullName.Length - start_idx - WeiqiApp.LUA_ROOT.Length - 5 );
				var table = mLuaState.doFile( bytes, name );
				if( table != null )
					mUiScripts.Add( name, (LuaTable)table );
				else
					mOtherScripts.Add( name );
				Log.Info( "Load lua file: {0}", name );
			}
			LuaFileLoaded = true;
			Dispatcher.InvokeAsync( onLuaScriptsLoadSucceed );
		}

		private void onLuaScriptsLoadSucceed()
		{
			Debug.Log( "加载脚本成功!" );
			mOnLoadCompleted( true );
			mOnLoadCompleted = null;
		}
#endif

		public Boolean LoadLuaFile( String _name, Byte[] _bytes )
		{
			if( _bytes == null )
			{
				//Log.Error( "加载lua文件: {0}失败!", _name );
				return false;
			}
			var table = mLuaState.doFile( _bytes, _name );
			if( table != null )
				mUiScripts.Add( _name, (LuaTable) table );
			else
				mOtherScripts.Add( _name );
			//Log.Debug( "Load lua file: {0}", _name );
			return true;
		}

		private void doBind( object _luaState )
		{
			IntPtr lua_state = (IntPtr) _luaState;

			List<Action<IntPtr>> list = new List<Action<IntPtr>>();

#if UNITY_EDITOR
			var assemblyName = "Assembly-CSharp";
			Assembly assembly = Assembly.Load( assemblyName );
			list.AddRange( getBindList( assembly, "SLua.BindUnityEngine" ) );
			list.AddRange( getBindList( assembly, "SLua.BindUnityEngineUI" ) );
			list.AddRange( getBindList( assembly, "SLua.BindDotNET" ) );
			list.AddRange( getBindList( assembly, "SLua.BindCustom" ) );
#else

			list.AddRange( SLua.BindUnityEngine.GetBindList() );
			list.AddRange( SLua.BindUnityEngineUI.GetBindList() );
			list.AddRange( SLua.BindDotNET.GetBindList() );
			list.AddRange( SLua.BindCustom.GetBindList() );
#endif
			// 绑定所有导出类
			int count = list.Count;
			for( int n = 0; n < count; n++ )
			{
				Action<IntPtr> action = list[n];
				action( lua_state );
			}
			Dispatcher.InvokeAsync( () => Instance.onBindComplted() );
		}

		Action<IntPtr>[] getBindList( Assembly _assembly, string _ns )
		{
			Type t = _assembly.GetType( _ns );
			if( t != null )
				return (Action<IntPtr>[]) t.GetMethod( "GetBindList" ).Invoke( null, null );
			return new Action<IntPtr>[0];
		}

		private void _doinit( IntPtr _luaState, LuaSvrFlag _flag )
		{
			//LuaTimer.reg( _luaState );
#if UNITY_EDITOR
			if( UnityEditor.EditorApplication.isPlaying )
#endif
				LuaCoroutine.reg( _luaState, this );
			SLua.Helper.reg( _luaState );
			if( SLuaSetting.Instance.useLuaValueType )
				LuaValueType.reg( _luaState );

			if( ( _flag & LuaSvrFlag.LSF_EXTLIB ) != 0 )
				LuaDLL.luaS_openextlibs( _luaState );
			if( ( _flag & LuaSvrFlag.LSF_3RDDLL ) != 0 )
				Lua3rdDLL.open( _luaState );

			Inited = true;
		}

		private void checkTop( IntPtr _luaState )
		{
			if( LuaDLL.lua_gettop( mLuaState.L ) != mErrorReported )
			{
				Debug.LogWarning( "Some function not remove temp value from lua stack. You should fix it." );
				mErrorReported = LuaDLL.lua_gettop( mLuaState.L );
			}
		}

		void Update()
		{
			if( !Inited )
				return;

			if( LuaDLL.lua_gettop( mLuaState.L ) != mErrorReported )
			{
				mErrorReported = LuaDLL.lua_gettop( mLuaState.L );
				Debug.LogWarning(
					string.Format( "Some function not remove temp value({0}) from lua stack. You should fix it.", LuaDLL.luaL_typename( mLuaState.L, mErrorReported ) ) );
			}

			mLuaState.checkRef();
			//LuaTimer.tick( Time.deltaTime );
		}
	}
}