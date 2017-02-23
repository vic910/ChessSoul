using System;
using System.Collections;
using System.Collections.Generic;
using Groot.Network;
using UnityEngine;
using Utility;
using Utility.FSM;
using Weiqi.UI;
using Groot;
using Groot.Res;

namespace Core.App
{
	/// <summary>
	/// 游戏的初始状态
	/// 初始化UI系统，加载基础配置和语言包等
	/// </summary>
	internal class AppStateStartup : State<AppMain>
	{
		public AppStateStartup() { }
		public AppStateStartup( AppMain _owner, AppMain _fsm, String _name ) : base( _owner, _fsm, _name ) { }

		public override void OnEnter()
		{
			TimerSystem.Instance.Initialize();
			NetManager.Instance.Initialize();
			// 初始化资源管理系统
			if( !ResourceManager.Instance.Initialize( true ) )
				return;
			Locale.Instance.Language = LanguageName.zh_cn;
#if GROOT_ASSETBUNDLE_SIMULATION
			Log.Info( "开始加载前置语言包" );
			ResourceManager.Instance.LoadAssetbundleAsync( string.Format( "data/language/{0}.cg", Locale.Instance.Language ), _onLanguageLoaded );
#else
			_loadLanguage();
#endif
		}

		private bool _onLanguageLoaded( bool _success, List<Resource_Assetbundle> _bundle )
		{
			_loadLanguage();
			_bundle[0].Unload( true );
			return true;
		}

		private void _loadLanguage()
		{
			if( Utility.SheetLite.Manager.Instance.GetDB( "language" ) == null )
			{
				Log.Error( "没有language数据" );
				return;
			}
			Locale.Instance.ChangeLanguage( Locale.Instance.Language, true );
			Fsm.Translate( AppStateName.ResCheckAndUpdate );
		}
	}
}