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
			NetManager.Instance.Register<GC_LoginFailedMsg>( _onPacketArrived );
			NetManager.Instance.Register<GC_LoginOK>( _onPacketArrived );
			SignalSystem.Register( SignalId.NetworkState_EnterConnected, _enterNetworkConnectedState );
			NetManager.Instance.Login();
		}

		public override void OnExit()
		{
			NetManager.Instance.Unregister<GC_LoginFailedMsg>();
			NetManager.Instance.Unregister<GC_LoginOK>();
			SignalSystem.Unregister( SignalId.NetworkState_EnterConnected, _enterNetworkConnectedState );
			UIManager.Instance.HideUI( "ui_login" );
		}

		private void _enterNetworkConnectedState( SignalId _signal_id, SignalParameters _parameters )
		{
			UIManager.Instance.CreateTitle();
			UIManager.Instance.ShowUI( "ui_login" );
		}

		private void _onPacketArrived( Int32 _stream_id, PacketType _packet_type, GC_LoginFailedMsg _msg )
		{
			switch( (GC_LoginFailedMsg.ReasonInfo)_msg.Reason )
			{
			case GC_LoginFailedMsg.ReasonInfo.RET_SUCCESS:
			case GC_LoginFailedMsg.ReasonInfo.RET_SUCCESS_WITH_ITEM:
				break;
			default:
					string tip = Locale.Instance[string.Format( "Login@{0}", (GC_LoginFailedMsg.ReasonInfo)( _msg.Reason ) ).ToString()];
					if( tip == string.Empty )
						UI_MessageBox.Show( ( (GC_LoginFailedMsg.ReasonInfo)( _msg.Reason ) ).ToString(), Locale.Instance["Common@Confirm"] );
					else
						UI_MessageBox.Show(  tip, Locale.Instance["Common@Confirm"] );
				break;
			}
		}

		private void _onPacketArrived( Int32 _stream_id, PacketType _packet_type, GC_LoginOK _msg )
		{
			Log.Info( "收到玩家数据" );
			MainPlayer.Instance.InitializePlayerInfo( _msg.PlayerInfo );
			Fsm.Translate( AppStateName.Gaming );
		}
	}
}