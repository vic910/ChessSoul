using System;

using UnityEngine;

using Utility.FSM;

namespace Core.App
{
	internal class AppStateGaming : State<AppMain>
	{
		private GameWorld m_game_world;


		public AppStateGaming() { }
		public AppStateGaming( AppMain _owner, AppMain _fsm, String _name ) : base( _owner, _fsm, _name ) { }
	
		public override void OnInit()
		{
			EnableUpdate = true;
			m_game_world = new GameWorld();
			m_game_world.Initialize();
		}

		public override void OnEnter()
		{
			m_game_world.Start();
		}

		public override void OnUpdate()
		{
			m_game_world.Update();
		}

		public override void OnExit()
		{
			m_game_world.Stop();
			m_game_world = null;
		}
	}
}