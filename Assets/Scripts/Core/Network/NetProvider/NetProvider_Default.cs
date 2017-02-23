using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

using Utility;
//using Utility.VariantSystem;
using Utility.FSM;
namespace Groot.Network
{
	class NetProvider_Default : NetProvider
	{
		enum NetStreamName
		{
			GameSrv,
		}
		enum NetWorkStateType
		{
			/// <summary>
			/// GameSrv连接中
			/// </summary>
			ConnectingToGameSrv,

			/// <summary>
			/// 连接成功
			/// </summary>
			Connected,

			/// <summary>
			/// 未连接
			/// </summary>
			Idle,

			/// <summary>
			/// 连接
			/// </summary>
			Connecting,
		}


		private NetClientStream m_net_client_gamesrv;
		//private NetClientStream m_net_client_loginsrv;
		/// <summary>
		/// 在多少秒之后重试
		/// </summary>
		private Single m_connect_in = 0f;

		/// <summary>
		/// 状态机
		/// </summary>
		private StateMachine<NetWorkStateType, NetProvider_Default> m_fsm = new StateMachine<NetWorkStateType, NetProvider_Default>();
		/// <summary>
		/// 消息缓存，只在服务器断开并尝试重连时有效
		/// </summary>
		Queue<MessageBase> m_msg_buffer = new Queue<MessageBase>();
		/// <summary>
		/// 初始化函数
		/// </summary>
		/// <returns></returns>
		public override bool Initialize()
		{
			// 网络库初始化
			m_net_client_gamesrv = new NetClientStream();
			m_net_client_gamesrv.Initialize( (Int32)NetStreamName.GameSrv );

			// 初始化状态机
			List<State<NetWorkStateType, NetProvider_Default>> states = new List<State<NetWorkStateType, NetProvider_Default>>();
			foreach( NetWorkStateType t in Enum.GetValues( typeof( NetWorkStateType ) ) )
			{
				states.Add( NetworkState.Instantiate( t, this, m_fsm ) );
			}
			m_fsm.Initialize( this, states );
			m_fsm.ChangeStateTo( NetWorkStateType.Idle );
			//Main.Instance.eventFrameEnter += _onUpdate;
			GameApp.eventUpdate += _onUpdate;
			return true;
		}

		public override void Uninitialize()
		{
			//Main.Instance.eventFrameEnter -= _onUpdate;
			GameApp.eventUpdate -= _onUpdate;
			//m_net_client_gamesrv.Unregister( Msg_gc_Heartbeat.MyMessageId );
			if( m_net_client_gamesrv != null )
			{
				m_net_client_gamesrv.Disconnect();
				m_net_client_gamesrv.Uninitialize();
				m_net_client_gamesrv = null;
			}
		}

		public override void RegisterPacketPreprocessor( Int32 _packet_id, NetPacketPreprocessor _preprocessor )
		{
			m_net_client_gamesrv.Register( _packet_id, _preprocessor );
		}

		public override void UnregisterPacketPreprocessor( Int32 _packet_id )
		{
			m_net_client_gamesrv.Unregister( _packet_id );
		}

		public override void Login()
		{
			// 如果正在尝试连接，则等连接状态
			if( m_fsm.CurrentState.StateType == NetWorkStateType.ConnectingToGameSrv
				|| m_fsm.CurrentState.StateType == NetWorkStateType.Connected )
				return;
			Connect();
		}

		public override bool SendMsg( MessageBase _msg )
		{
			if( m_fsm.CurrentState.StateType != NetWorkStateType.Connected )
			{
				m_msg_buffer.Enqueue( _msg );
				Connect();
				return false;
			}
			return m_net_client_gamesrv.SendMessage( _msg );
		}

		/// <summary>
		/// 开始连接
		/// </summary>
		public void Connect()
		{
			// 如果正在尝试连接，则等连接状态
			if( m_fsm.CurrentState.StateType == NetWorkStateType.ConnectingToGameSrv
				|| m_fsm.CurrentState.StateType == NetWorkStateType.Connected )
				return;

			Disconnect();
			m_connect_in = 0f;
			m_fsm.ChangeStateTo( NetWorkStateType.Connecting );
		}

		public void Disconnect()
		{
			m_net_client_gamesrv.Disconnect();
		}

		private void _onUpdate( float _dt )
		{
			m_fsm.Update( _dt );
		}
		#region 状态机

		class NetworkState : State<NetWorkStateType, NetProvider_Default>
		{
			/// <summary>
			/// 状态工厂类实例
			/// </summary>
			private static Utility.Factory.SimpleFactory<NetWorkStateType, NetworkState> s_factory
			= new Utility.Factory.SimpleFactory<NetWorkStateType, NetworkState>();

			/// <summary>
			/// 静态构造函数，注册相关状态
			/// </summary>
			static NetworkState()
			{
				s_factory.Register<ConnectingToGameSrv>( NetWorkStateType.ConnectingToGameSrv );
				s_factory.Register<Connected>( NetWorkStateType.Connected );
				s_factory.Register<Conneting>( NetWorkStateType.Connecting );
				s_factory.Register<Idle>( NetWorkStateType.Idle );
			}
			/// <summary>
			/// 状态初始化函数
			/// </summary>
			/// <param name="_type"></param>
			/// <param name="_entity"></param>
			/// <param name="_fsm"></param>
			protected sealed override void Initialze( NetWorkStateType _type
				, NetProvider_Default _entity
				, StateMachine<NetWorkStateType, NetProvider_Default> _fsm )
			{

				base.Initialze( _type, _entity, _fsm );
			}
			/// <summary>
			/// 构建状态实例并初始化
			/// </summary>
			/// <param name="_type">状态类型</param>
			/// <param name="_entity">phase</param>
			/// <param name="_fsm">持有该状态的状态机</param>
			/// <returns></returns>
			public static NetworkState Instantiate( NetWorkStateType _type
				, NetProvider_Default _entity
				, StateMachine<NetWorkStateType, NetProvider_Default> _fsm )
			{
				NetworkState state = s_factory.CreateInstance( _type );
				state.Initialze( _type, _entity, _fsm );
				return state;
			}
		}

		private class ConnectingToGameSrv : NetworkState
		{

			private Timer m_time_out;
			private String m_time_out_tip;

			public override void Enter()
			{
				WaitForResponse.Retain();
				NetManager.Instance.Register<ConnectMsg>( _onGameSrvConnected );
				NetManager.Instance.Register<DisconnectMsg>( _onGameSrvDisconnected );
				IPAddress ip;
				if( !IPAddress.TryParse( "192.168.100.152", out ip ) )
					throw new ServerInfoException( ServerInfoException.ErrorType.IpParseError );
				IPEndPoint ip_end = new IPEndPoint( ip, 12345 );
				Entity.m_net_client_gamesrv.Connect( ip_end );

				if( m_time_out == null )
				{
					m_time_out = Timer.CreateTimer( 10000 );
					m_time_out.eventTimesUp += _onTimeOut;
				}
				m_time_out.Start();
				m_time_out_tip = "GameSrvConnectTimeOut";
			}

			public override void Exit()
			{
				NetManager.Instance.Unregister<ConnectMsg>();
				NetManager.Instance.Unregister<DisconnectMsg>();
				m_time_out.Stop();
			}

			private void _onGameSrvConnected( Int32 _stream_id, PacketType _packet_type, ConnectMsg _msg )
			{
				Log.Info( "GameSrv Connected!" );
				FiniteStateMachine.ChangeStateTo( NetWorkStateType.Connected );
			}

			private void _onGameSrvDisconnected( Int32 _stream_id, PacketType _packet_type, DisconnectMsg _msg )
			{
				_onLoginFail( "GameSrv_AbnormalDisconnection" );
			}

			private void _onTimeOut( Timer _timer, DateTime _date_time, long _arg3 )
			{
				Log.Info( "GameSrv 连接超时!" );
				_onLoginFail( m_time_out_tip );
			}

			private void _onLoginFail( String _tip, params System.Object[] _args )
			{
				Log.Info( "GameSrv 连接出错 自动重连!：{0}", _tip );
				m_time_out.Stop();
				Entity.m_connect_in = 10f;
				FiniteStateMachine.ChangeStateTo( NetWorkStateType.Connecting );
				//TODO 登录游戏服报错处理
			}
		}

		private class Connected : NetworkState
		{
			/// <summary>
			/// 用于时间同步的timer
			/// </summary>
			//private Timer m_timer_sync_time;


			public override void Enter()
			{
				WaitForResponse.Release();
				NetManager.Instance.Register<DisconnectMsg>( _onConnectionClosed );
				//Entity.m_net_client_gamesrv.Register<DisconnectMsg>( DisconnectMsg.MyMessageId, _onConnectionClosed );
				//Entity.m_net_client_gamesrv.Register<Msg_gc_ConnectionClosed>( Msg_gc_ConnectionClosed.MyMessageId, _onGameSrvCloseConnection );
				//Entity.m_net_client_gamesrv.Register<Msg_gc_Heartbeat>( Msg_gc_Heartbeat.MyMessageId, _onHeartbeatReceived );
				Utility.Log.Info( "成功登录GameSrv" );

				//Main.Instance.eventOnApplicationPause += _onApplicationPause;
				//_startSyncTime();
				//_sendHeartbeat();
				// 处理之前缓存的消息
				if( Entity.m_msg_buffer.Count == 0 )
					return;
				MessageBase msg = Entity.m_msg_buffer.Dequeue();
				while( true )
				{
					Entity.SendMsg( msg );
					if( Entity.m_msg_buffer.Count == 0 )
						break;
					msg = Entity.m_msg_buffer.Dequeue();
				}

				SignalSystem.FireSignal( SignalId.NetworkState_EnterConnected );
			}
			public override void Exit()
			{
				//if( m_timer_sync_time != null )
				//	m_timer_sync_time.Stop();
				NetManager.Instance.Unregister<DisconnectMsg>();
				//Entity.m_net_client_gamesrv.Unregister( Msg_gc_ConnectionClosed.MyMessageId );
				//Entity.m_net_client_gamesrv.Unregister( Msg_gc_Heartbeat.MyMessageId );

				_stopHeartbeat();
				//_stopSyncTime();
				//GameWorld.Instance.eventOnApplicationPause -= _onApplicationPause;
			}

			private void _onConnectionClosed( Int32 _stream_id, PacketType _packet_type, DisconnectMsg _msg )
			{
				Utility.Log.Warning( "与游戏服务器的连接已经断开" );
				//TODO 链接断开，自动重连
				Entity.m_connect_in = 10f;
				FiniteStateMachine.ChangeStateTo( NetWorkStateType.Connecting );
			}

			//private void _onGameSrvCloseConnection( Int32 _stream_id, PacketType _packet_type, Msg_gc_ConnectionClosed _msg )
			//{
			//	Utility.Log.Warning( "已被GameSrv踢下线: {0}", _msg.Type );
			//	Entity.Disconnect();
			//	FiniteStateMachine.ChangeStateTo( NetWorkStateType.Idle );
			//	// TODO 被踢下线，返回游戏登陆界面
			//}

			#region 时间同步

			//private void _startSyncTime()
			//{
			//	// 发起时间同步
			//	// 十分钟一次时间同步
			//	if( m_timer_sync_time == null )
			//	{
			//		m_timer_sync_time = Timer.CreateAutoResetTimer( 1000 * 60 * 10 );
			//		//m_timer_sync_time = Timer.CreateAutoResetTimer( 10000 );
			//		m_timer_sync_time.eventTimesUp += _onSendSyncTimeRequire;
			//	}
			//	m_timer_sync_time.Start();
			//	//ServerTime.RequireTimeSync();
			//}

			//private void _onSendSyncTimeRequire( Timer _timer, DateTime _date_time, long _arg3 )
			//{
			//	//ServerTime.RequireTimeSync();
			//}

			//private void _stopSyncTime()
			//{
			//	if( m_timer_sync_time != null )
			//		m_timer_sync_time.Stop();
			//}
			#endregion

			#region 心跳包
			/// <summary>
			/// 心跳包超时用
			/// </summary>
			private Timer m_timer_heartbeat_timeout;

			/// <summary>
			/// 心跳包的值，用于做回执验证
			/// </summary>
			//private Int64 m_heartbeat_value;

			/// <summary>
			/// 发送心跳包的计时器
			/// </summary>
			private Timer m_timer_heartbeat;

			/// <summary>
			/// 发送心跳包
			/// </summary>
			private void _sendHeartbeat()
			{
				if( m_timer_heartbeat_timeout == null )
				{
					m_timer_heartbeat_timeout = Timer.CreateTimer( 5000 );
					m_timer_heartbeat_timeout.eventTimesUp += _onHeartbeatTimeOut;
				}
				else
				{
					m_timer_heartbeat_timeout.Interval = 5000;
				}
				//m_timer_heartbeat_timeout.Start();
				//Msg_cg_Heartbeat msg = new Msg_cg_Heartbeat();
				//Entity.SendMsg( msg );
				Log.Debug( "发起心跳包, 超时时间: {0}ms", 5000 );
			}
			//private void _onHeartbeatReceived( Int32 _stream_id, PacketType _packet_type, Msg_gc_Heartbeat _msg )
			//{
			//	Log.Debug( "收到心跳包回应!" );
			//	//// 抛弃
			//	//if( m_heartbeat_value != _msg.Value )
			//	//{
			//	//	Log.Debug( "抛弃心跳包回执!" );
			//	//	return;
			//	//}
			//	//m_heartbeat_value = 0;
			//	m_timer_heartbeat_timeout.Stop();

			//	//_startSyncTime();
			//	_onStartHeartbeatTimer( 20000 );
			//}

			private void _onHeartbeatTimeOut( Timer _timer, DateTime _date_time, long _interval )
			{
				Log.Info( "心跳包超时! 连接断开" );
				// TODO 断开连接或者自动重连
				Entity.Disconnect();
				Entity.m_connect_in = 3f;
				FiniteStateMachine.ChangeStateTo( NetWorkStateType.Connecting );
			}
			private void _onStartHeartbeatTimer( Int32 _interval )
			{
				if( m_timer_heartbeat == null )
				{
					m_timer_heartbeat = Timer.CreateTimer( _interval );
					m_timer_heartbeat.eventTimesUp += _onHeartbeatTimerTriggered;
				}
				m_timer_heartbeat.Start();
			}

			private void _onHeartbeatTimerTriggered( Timer _timer, DateTime _date_time, long _arg3 )
			{
				_sendHeartbeat();
			}

			private void _stopHeartbeat()
			{
				if( m_timer_heartbeat_timeout != null )
					m_timer_heartbeat_timeout.Stop();
				if( m_timer_heartbeat != null )
					m_timer_heartbeat.Stop();
			}

			#endregion

			private void _onApplicationPause( Boolean _pause )
			{
				if( _pause )
				{
					//m_heartbeat_value = 0;
					_stopHeartbeat();
					//_stopSyncTime();
				}
				else
				{
					_sendHeartbeat();
					//_startSyncTime();
				}
			}
		}

		class Conneting : NetworkState
		{
			private Single m_timer;
			private Int32 m_last_show;
			public override void Enter()
			{
				m_timer = Entity.m_connect_in;
				m_last_show = (Int32)m_timer + 1;
			}

			public override void Update( float _dt )
			{
				m_timer -= _dt;
				if( !( m_timer < 0 ) )
				{
					Int32 tmp = (Int32)m_timer;
					if( m_last_show != tmp && tmp != 0 )
						Log.Debug( "将在{0}s后开始连接!", tmp );
					m_last_show = tmp;
					return;
				}

				Utility.Log.Info( "开始尝试重新连接服务器" );
				FiniteStateMachine.ChangeStateTo( NetWorkStateType.ConnectingToGameSrv );
			}
		}

		/// <summary>
		/// Do nothing.... -_-
		/// </summary>
		class Idle : NetworkState { }

		#endregion
	}
}
