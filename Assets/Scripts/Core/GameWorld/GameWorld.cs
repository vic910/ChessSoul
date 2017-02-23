using System;

using UnityEngine;

using Utility.FSM;

namespace Core
{
	internal class GameWorld : StateMachine<GameWorld>
	{
		/// <summary>
		/// 初始化状态
		/// </summary>
		protected override void InitializeState()
		{
			AddState<GameWorldStateLobby>( "Lobby", true );
			AddState<GameWorldStateLoad>( "Load", false );
			AddState<GameWorldStateUnLoad>( "Unload", false );
		}

		public override GameWorld GetOwner()
		{
			return this;
		}
	}
}