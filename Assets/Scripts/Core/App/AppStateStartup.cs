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
using Weiqi;

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
			_loadPreLanguage();
		}

		private void _loadPreLanguage()
		{
			if( Utility.SheetLite.Manager.Instance.GetDB( "language" ) == null )
			{
				Log.Error( "没有language数据" );
				return;
			}
			Locale.Instance.Language = LanguageName.zh_cn;
			Locale.Instance.LoadOneLanguageConfig( "preload" );
			Fsm.Translate( AppStateName.ResCheckAndUpdate );
		}
	}
}