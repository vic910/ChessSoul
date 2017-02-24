using System;
using System.Collections.Generic;
using System.Net;
using Utility;

namespace Groot.Network
{
	class NetClientStream
	{
		/// <summary>
		/// socket
		/// </summary>
		private Groot.Network.TcpClient m_client = null;
		/// <summary>
		/// 初始化网络模块
		/// </summary>
		/// <param name="_stream_id">stream 编号</param>
		/// <returns></returns>
		public bool Initialize( Int32 _stream_id )
		{
			m_client = new TcpClient( _stream_id );
			m_client.eventPacketArrived += _onPacketArrived;
			return true;
		}
		/// <summary>
		/// 清理网络模块
		/// </summary>
		public void Uninitialize()
		{
			m_client.Close();
			m_client = null;
		}

		/// <summary>
		/// 连接到服务器
		/// </summary>
		/// <param name="_ip_end_point">ip地址和端口</param>
		/// <returns></returns>
		public bool Connect( IPEndPoint _ip_end_point )
		{
			return m_client.Connect( _ip_end_point );
		}
		/// <summary>
		/// 断开连接
		/// </summary>
		public void Disconnect()
		{
			m_client.Disconnect();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_msg"></param>
		/// <returns></returns>
		public bool SendMessage( MessageBase _msg )
		{
			//消息序列化
			Byte[] msg_body = SerializerHelper.SerializeMessage( _msg );
			return m_client.Send( msg_body, msg_body.Length );
		}
		#region Packet dispatch
		/// <summary>
		/// 
		/// </summary>
		private readonly Dictionary<Int32, NetPacketPreprocessor> m_preprocessors = new Dictionary<Int32, NetPacketPreprocessor>();
		/// <summary>
		/// 默认消息预处理器
		/// </summary>
		private NetPacketPreprocessor m_default_preprocessor = null;
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TyMessage"></typeparam>
		/// <param name="_packet_id"></param>
		/// <param name="_handler"></param>
		//public void Register<TyMessage>( Int32 _packet_id, Action<Int32, PacketType, TyMessage> _handler )
		//	where TyMessage : MessageBase, new()
		//{
		//	m_preprocessors.Add( _packet_id, new TNetPacketPreprocessor<TyMessage>( _handler ) );
		//}
		//public void RegisterEx<TyMessage>( Int32 _packet_id, Action<Int32, NetPacket, TyMessage> _handler )
		//	where TyMessage : MessageBase, new()
		//{
		//	m_preprocessors.Add( _packet_id, new TNetPacketPreprocessorEx<TyMessage>( _handler ) );
		//}

		public void Register( Int32 _packet_id, NetPacketPreprocessor _preprocessor )
		{
			m_preprocessors.Add( _packet_id, _preprocessor );
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_packet_id"></param>
		public void Unregister( Int32 _packet_id )
		{
			NetPacketPreprocessor net_packet_preprocessor = null;
			if( !m_preprocessors.TryGetValue( _packet_id, out net_packet_preprocessor ) )
				return;
			net_packet_preprocessor.Invalid();
			m_preprocessors.Remove( _packet_id );
		}
		/// <summary>
		/// 设置消息默认预处理器
		/// </summary>
		/// <typeparam name="TyPacket">消息类型</typeparam>
		/// <param name="_handler">处理器</param>
		public void SetDefaultHandler<TyPacket>( Action<Int32, PacketType, TyPacket> _handler )
			where TyPacket : MessageBase, new()
		{
			m_default_preprocessor = new TNetPacketPreprocessor<TyPacket>( _handler );
		}
		/// <summary>
		/// 清除消息默认预处理器
		/// </summary>
		public void ClearDefaultHandler()
		{
			m_default_preprocessor = null;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_packet_id"></param>
		/// <returns></returns>
		private NetPacketEvent _createEvent( Int32 _packet_id )
		{
			NetPacketPreprocessor preprocessor = null;
			if( m_preprocessors.TryGetValue( _packet_id, out preprocessor ) )
				return preprocessor.CreateInstance();
			if( null == m_default_preprocessor )
				return null;
			return m_default_preprocessor.CreateInstance();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_stream_id"></param>
		/// <param name="_packet"></param>
		private void _onPacketArrived( Int32 _stream_id, NetPacket _packet )
		{
			try
			{
				Int32 packet_id = 0;
				MessageBase msg = null;
				switch( _packet.PacketType )
				{
				case PacketType.Connected:
					//packet_id = ConnectMsg.MyMessageId;
					packet_id = Helper.GenerateInt32( ConnectMsg.Type, ConnectMsg.Dir, ConnectMsg.MessageId );
					break;
				case PacketType.Disconnected:
					//packet_id = DisconnectMsg.MyMessageId;
					packet_id = Helper.GenerateInt32( DisconnectMsg.Type, DisconnectMsg.Dir, DisconnectMsg.MessageId );
					break;
				case PacketType.Message:
					msg = SerializerHelper.Deserializer<MessageBase>( _packet.Data );
					packet_id = Helper.GenerateInt32( msg.MsgType, msg.MsgDirection, msg.MsgId );
					break;
				case PacketType.Error:
					throw new Exception( "Got error packet" );
				default:
					throw new ArgumentOutOfRangeException();
				}
				NetPacketEvent packet_event = _createEvent( packet_id );
				if( null == packet_event )
				{
					Utility.Log.Warning( "Received a packet dir={0} type={1} id={2} that isn't register with any handler.", msg.MsgDirection, msg.MsgType, msg.MsgId );
					return;
				}
				if( !packet_event.Initialize( _stream_id, _packet ) )
				{
					Utility.Log.Error( "Failed to initialize packet {0}.", packet_id );
					return;
				}
				GameApp.Instance.PushEvent( packet_event );
			}
			catch( Exception e )
			{
				Log.Error( "[NetPacketEvent]Failed to process the arrived packet: {0}", e.Message );
			}
		}

		#endregion
	}
}
