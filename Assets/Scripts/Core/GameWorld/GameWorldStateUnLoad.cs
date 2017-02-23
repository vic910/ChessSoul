using System;

using UnityEngine;

using Utility.FSM;

namespace Core
{
	internal class GameWorldStateUnLoad : State<GameWorld>
	{
		public GameWorldStateUnLoad() { }
		public GameWorldStateUnLoad( GameWorld _owner, GameWorld _fsm, String _name ) : base( _owner, _fsm, _name ) { }
	}
}