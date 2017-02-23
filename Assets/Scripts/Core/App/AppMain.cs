using System;

using UnityEngine;

using Utility.FSM;

namespace Core.App
{
	static class AppStateName
	{
		public const String Startup = "Startup";
		public const String ResCheckAndUpdate = "ResCheckAndUpdate";
		public const String Loading = "Loading";
		public const String Login = "Login";
		public const String Gaming = "Gaming";
	}

	internal class AppMain : StateMachine<AppMain>
	{

		public override AppMain GetOwner()
		{
			return this;
		}

		protected override void InitializeState()
		{
			AddState<AppStateStartup>( AppStateName.Startup, true );
			AddState<AppStateResCheckAndUpdate>( AppStateName.ResCheckAndUpdate, false );
			AddState<AppStateLogin>( AppStateName.Login, false );
			AddState<AppStateGaming>( AppStateName.Gaming, false );
			AddState<AppStateLoading>( AppStateName.Loading, false );
		}
	}
}