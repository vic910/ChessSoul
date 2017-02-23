using System;
using Groot;
using Groot.Network;
using UnityEngine;
using Utility;
using Utility.FSM;
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
			SignalSystem.Register( SignalId.NetworkState_EnterConnected, _enterNetworkConnectedState );
			NetManager.Instance.Login();
		}

		public override void OnExit()
		{
			SignalSystem.Unregister( SignalId.NetworkState_EnterConnected, _enterNetworkConnectedState );
		}

		private void _enterNetworkConnectedState( SignalId _signal_id, SignalParameters _parameters )
		{
			UIManager.Instance.CreateTitle();
			UIManager.Instance.ShowUI( "ui_login" );
		}
	}
}