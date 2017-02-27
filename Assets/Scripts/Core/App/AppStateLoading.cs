using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Groot;
using Groot.Res;
using Slua;
using Utility;
using Utility.FSM;
using Weiqi.UI;

namespace Core.App
{
	internal class AppStateLoading : State<AppMain>
	{
		public AppStateLoading() { }
		public AppStateLoading( AppMain _owner, AppMain _fsm, String _name ) : base( _owner, _fsm, _name ) { }


		private static AppStateLoading s_instance;

		public override void OnInit()
		{
			if( s_instance != null )
			{
				Log.Critical( "AppStateLoading被重复创建! 程序退出!" );
				System.Diagnostics.Process.GetCurrentProcess().Kill();
			}
			s_instance = this;
		}

		public override void OnEnter()
		{
			_loadLanguage();
			_loadConfig();
			s_instance.Fsm.Translate( AppStateName.Login );
		}

		private void _loadLanguage()
		{
			if( Utility.SheetLite.Manager.Instance.GetDB( "language" ) == null )
			{
				Log.Error( "没有language数据" );
				return;
			}
			//加载完整语言包
			Locale.Instance.ChangeLanguage( Locale.Instance.Language );
		}

		private void _loadConfig()
		{
			if( Utility.SheetLite.Manager.Instance.GetDB( "data" ) == null )
			{
				Log.Error( "没有data配置数据" );
				return;
			}
			//加载登录时能用到的配置
			GlobalConfig.Instance.Initialize();
		}
	}
}