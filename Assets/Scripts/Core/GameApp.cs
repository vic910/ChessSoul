using System;
using UnityEngine;
using System.Collections;
using Core.App;
using Groot;
using Groot.Network;
using Groot.Res;
using Slua;
using Utility;
using Weiqi;
using Weiqi.UI;

public class GameApp : UnitySingleton<GameApp>
{
	public static event Action<float> eventUpdate;

	public static event Action eventLateUpdate;

	public static event Action eventFixedUpdate;

	private AppMain m_app;

	private Utility.SynchronizedQueue<GameEvent> m_game_events = new Utility.SynchronizedQueue<GameEvent>();

	protected override void OnAwake()
	{
		m_app = new AppMain();
		m_app.Initialize();
	}

	protected override void OnStart()
	{
		m_app.Start();
	}

	protected override void OnDestory()
	{

	}

	void OnApplicationQuit()
	{
		m_app.Stop();
		_uninitializeSystem();
		_uninitializeConfig();

		UIManager.Instance.Uninitialize();
		NetManager.Instance.Uninitialize();
		TimerSystem.Instance.Uninitialize();
		ResourceManager.Instance.Uninitialize();
		UILuaSvr.Instance.Uninitialize();
	}

	private void _uninitializeSystem()
	{
		RoomSystem.Instance.Uninitialize();
		LobbySystem.Instance.Uninitialize();
		ChatSystem.Instance.Uninitialize();
		PlayerOnlineSystem.Instance.Uninitialize();
		ChatSystem.Instance.Uninitialize();
	}

	private void _uninitializeConfig()
	{
		Locale.Instance.Uninitialize();
		GlobalConfig.Instance.Uninitialize();
		PlayerInfoConfig.Instance.UnInitialize();
	}

	// Update is called once per frame
	void Update()
	{
		m_app.Update();
		if( eventUpdate != null )
			eventUpdate( Time.deltaTime );
		_handleGameEvents();
	}

	void FixedUpdate()
	{
		if( eventFixedUpdate != null )
			eventFixedUpdate();
	}

	void LateUpdate()
	{
		if( eventLateUpdate != null )
			eventLateUpdate();
	}

	public void PushEvent( GameEvent _event )
	{
		m_game_events.Enqueue( _event );
	}

	private void _handleGameEvents()
	{
		GameEvent game_event = null;
		while( m_game_events.Dequeue( out game_event ) )
		{
			game_event.ExecuteEvent();
		}
	}
}