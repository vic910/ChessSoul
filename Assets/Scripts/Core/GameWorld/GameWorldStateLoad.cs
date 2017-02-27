using System;

using UnityEngine;

using Utility.FSM;

namespace Core
{
	internal class GameWorldStateLoad : State<GameWorld>
	{
		public GameWorldStateLoad() { }
		public GameWorldStateLoad( GameWorld _owner, GameWorld _fsm, String _name ) : base( _owner, _fsm, _name ) { }

		public override void OnEnter()
		{
			// 加载所有配置

			// TODO

			// 初始化基础系统



		}
	}
}