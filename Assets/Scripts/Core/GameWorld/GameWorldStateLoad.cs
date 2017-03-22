using System;
using Groot;
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
			// 加载剩下所有语言配置
			Locale.Instance.LoadAllLanguageConfig();
			PlayerInfoConfig.Instance.Initialize();

			// 初始化系统
			ChatSystem.Instance.Initialize();
			PlayerOnlineSystem.Instance.Initialize();

			// 进入大厅
			Fsm.Translate( "Lobby" );
		}
	}
}