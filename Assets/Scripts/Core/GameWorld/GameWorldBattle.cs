using System;

using UnityEngine;

using Utility.FSM;

namespace Core
{
	internal class GameWorldBattle : State<GameWorld>
	{
		public GameWorldBattle() { }
		public GameWorldBattle( GameWorld _owner, GameWorld _fsm, String _name ) : base( _owner, _fsm, _name ) { }
	}
}