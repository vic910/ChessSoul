using System;
using Groot;
using Groot.Network;
using UnityEngine;
using Utility;
using Utility.FSM;
using Weiqi;
using Weiqi.UI;

namespace Core.App
{
	internal class AppStateLogin : State<AppMain>
	{
		public AppStateLogin() { }
		public AppStateLogin( AppMain _owner, AppMain _fsm, String _name ) : base( _owner, _fsm, _name ) { }
		public override void OnInit()
		{
			//EnableUpdate = true;
		}
		public override void OnEnter()
		{
			//SignalSystem.Register( SignalId.NetworkState_EnterConnected, _enterNetworkConnectedState );
			SignalSystem.Register( SignalId.Login_Success, _loginSuccess );
			UIManager.Instance.ShowUI( "ui_login" );
			UIManager.Instance.CreateTitle();
			LoginSystem.Instance.Initialize();
			//由于登录成功后服务器马上发其他消息 所以这些消息的注册放到这
			RoomSystem.Instance.Initialize();
		}

		public override void OnExit()
		{
			//SignalSystem.Unregister( SignalId.NetworkState_EnterConnected, _enterNetworkConnectedState );
			SignalSystem.Unregister( SignalId.Login_Success, _loginSuccess );
			LoginSystem.Instance.Uninitialize();
			UIManager.Instance.HideUI( "ui_login" );
		}

		//private void _enterNetworkConnectedState( SignalId _signal_id, SignalParameters _parameters )
		//{
			
		//}

		private void _loginSuccess( SignalId _signal_id, SignalParameters _parameters )
		{
			Fsm.Translate( AppStateName.Gaming );
		}
	}
}