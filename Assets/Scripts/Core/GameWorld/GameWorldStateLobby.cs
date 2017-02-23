using System;

using UnityEngine;

using Utility.FSM;
using Weiqi.UI;

namespace Core
{
	internal class GameWorldStateLobby : State<GameWorld>
	{
		public GameWorldStateLobby() { }
		public GameWorldStateLobby( GameWorld _owner, GameWorld _fsm, String _name ) : base( _owner, _fsm, _name ) { }

		public override void OnEnter()
		{
			//UIManager.Instance.ShowUI( "ui_msgbox" );
			UIManager.Instance.ShowUI( "ui_main" );
		}
	}
}