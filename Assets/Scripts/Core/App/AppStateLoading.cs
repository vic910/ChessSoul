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
#if GROOT_ASSETBUNDLE_SIMULATION
			Log.Info( "开始加载完整语言包" );
			ResourceManager.Instance.LoadAssetbundleAsync( string.Format( "data/language/{0}.cg", Locale.Instance.Language ), _onLanguageLoaded );
#else
			_loadLanguage();
			_loadAllConfig();
			s_instance.Fsm.Translate( AppStateName.Login );
#endif
		}

		private bool _onLanguageLoaded( bool _success, List<Resource_Assetbundle> _bundle )
		{
			_loadLanguage();
			_bundle[0].Unload( true );
			Log.Info( "开始加载前置配置包" );
			ResourceManager.Instance.LoadAssetbundleAsync( "data/config.cg", _onConfigLoaded );
			return true;
		}

		private void _loadLanguage()
		{
			if( Utility.SheetLite.Manager.Instance.GetDB( "language" ) == null )
			{
				Log.Error( "没有language数据" );
				return;
			}
			Locale.Instance.ChangeLanguage( Locale.Instance.Language );
		}

		private bool _onConfigLoaded( bool _success, List<Resource_Assetbundle> _bundle )
		{
			//读取配置
			_loadAllConfig();
			_bundle[0].Unload( true );
			s_instance.Fsm.Translate( AppStateName.Login );
			return true;
		}

		private void _loadAllConfig()
		{
			if( Utility.SheetLite.Manager.Instance.GetDB( "data" ) == null )
			{
				Log.Error( "没有data配置数据" );
				return;
			}
			GlobalConfig.Instance.Initialize();
		}
	}
}