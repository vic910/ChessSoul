using System;
using System.IO;
using System.Text;
using System.Threading;
using Groot.Res;
using Slua;
using UnityEngine.UI;
using Utility;
using Utility.FSM;
using Weiqi.UI;

namespace Core.App
{
	internal class AppStateResCheckAndUpdate : State<AppMain>
	{
		public AppStateResCheckAndUpdate() { }
		public AppStateResCheckAndUpdate( AppMain _owner, AppMain _fsm, String _name ) : base( _owner, _fsm, _name ) { }

		/// <summary>
		/// 是否更新完成
		/// 检测更新完成 没有更新也算完成
		/// </summary>
		private Boolean mIsUpdated;
		public override void OnEnter()
		{
			// 初始化UI系统 
			UIManager.Instance.Initialize();
#if GROOT_ASSETBUNDLE_SIMULATION
			mIsUpdated = false;
			// 初始化luaSrv
			UILuaSvr.Instance.InitAndLoadLuaFile( onLuaLoaded, false );
#else
			mIsUpdated = true;
			// 初始化luaSrv
			UILuaSvr.Instance.InitAndLoadLuaFile( onLuaLoaded, true );
#endif
		}

		private void onLuaLoaded( Boolean _b )
		{
			//Log.Debug( "lua脚本初始化完成" );
			if( !_b )
			{
				Log.Critical( "lua脚本初始化失败!" );
				return;
			}
			if( mIsUpdated )
			{
				Fsm.Translate( AppStateName.Loading );
			}
			else
				ResUpdateMgr.Instance.UpdateRes( onUpdateComplted );
		}

		private void onUpdateComplted( Boolean _updated )
		{
			mIsUpdated = true;
			Log.Debug( "更新完成, {0}", _updated ? "有更新文件被找到!" : "没有更新文件被找到!" );
			if( _updated )
			{
				// TODO 重新初始化资源管理系统
				// TODo 重新初始化UI
				ResourceManager.Instance.Uninitialize();
				if( !ResourceManager.Instance.Initialize( false ) )
					return;
				UIManager.Instance.Uninitialize();
				UIManager.Instance.Initialize();
				UILuaSvr.Instance.Uninitialize();
				UILuaSvr.Instance.InitAndLoadLuaFile( onLuaLoaded, true );
			}
			else
			{
				UILuaSvr.Instance.LoadRemainLuaScripts( onLuaLoaded );
			}
		}
	}
}