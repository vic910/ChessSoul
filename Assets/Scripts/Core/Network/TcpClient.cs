using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using Utility;

namespace Groot.Network
{
	public enum PacketType
	{
		Connected,
		Message,
		Disconnected,
		Error,
	}
	public class NetPacket
	{
		public PacketType PacketType = PacketType.Connected;
		public Byte[] Data;
		public Int32 DataSize = 0;
	}
	class ReceivePacketArg
	{
		public SocketAsyncEventArgs AsyncArgs = new SocketAsyncEventArgs();
		public Int32 BytesReceived = 0;
		public Int32 BytesWanted = 0;
	}
	class SendPacketArg
	{
		public SocketAsyncEventArgs AsyncArgs = new SocketAsyncEventArgs();
		public Int32 BytesSent = 0;
		public Int32 BytesWanted = 0;
	}

	class TcpClient
	{
		private Int32 SendMessageHeadSize = 16;
		private Int32 RecvMessageHeadSize = 4;
		/// <summary>
		/// 网络事件到达时调用，不保证从主线程调用，请注意线程安全
		/// </summary>
		public event Action<Int32, NetPacket> eventPacketArrived;
		public TcpClient( Int32 _stream_id )
		{
			StreamId = _stream_id;
			// recive head args
			Byte[] head_buffer = new Byte[RecvMessageHeadSize];
			m_recv_head_args.AsyncArgs.SetBuffer( head_buffer, 0, RecvMessageHeadSize );
			m_recv_head_args.AsyncArgs.Completed += _onReceivePacket;
			m_recv_head_args.AsyncArgs.UserToken = m_recv_head_args;
			// recive body args
			m_recv_body_args.AsyncArgs.Completed += _onReceivePacket;
			m_recv_body_args.AsyncArgs.UserToken = m_recv_body_args;
			// send arg
			m_send_arg.AsyncArgs.Completed += _onSendPacket;
			m_send_arg.AsyncArgs.UserToken = m_send_arg;
		}
		public void Close()
		{
			Disconnect();
			if (null != m_connect_socket)
			{
				m_connect_socket.Close();
			}
		}

		/// <summary>
		/// 连接到服务器
		/// </summary>
		/// <param name="_ip_end_point">ip地址和端口</param>
		public bool Connect( IPEndPoint _ip_end_point )
		{
			try
			{
				if( _ip_end_point == null )
					throw new ExceptionEx( "Groot.NetWork.TcpClient.Connect: Arg mush not be null!" );
				Log.Debug( "[TcpClient]Attempting to connect to server {0}:{1}", _ip_end_point.Address, _ip_end_point.Port );
				Socket socket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
				SocketAsyncEventArgs args = new SocketAsyncEventArgs();
				args.Completed += _onConnectCompleted;
				args.RemoteEndPoint = _ip_end_point;
				args.UserToken = socket;
				if( !socket.ConnectAsync( args ) )
				{ // 操作完成
					_onConnectCompleted( null, args );
				}
				return true;
			}
			catch( Exception e )
			{
				Log.Error( "Socket Error while connect to server: {0}", e.Message );
			}
			return false;
		}
		private void _onConnectCompleted( Object _sender, SocketAsyncEventArgs _args )
		{
			if( SocketError.Success != _args.SocketError )
				return;
			lock ( m_connect_socket_lock )
			{
				m_connect_socket = _args.UserToken as Socket;
			}
			if( null != eventPacketArrived )
			{
				NetPacket packet = new NetPacket();
				packet.PacketType = PacketType.Connected;
				packet.Data = null;
				packet.DataSize = 0;
				eventPacketArrived( StreamId, packet );
			}
			_receiveNextMessage();
		}
		public bool IsConnected()
		{
			lock ( m_connect_socket_lock )
			{
				if( null == m_connect_socket )
					return false;
				return m_connect_socket.Connected;
			}
		}
		/// <summary>
		/// 从服务器断开连接
		/// </summary>
		public void Disconnect()
		{
			Action action = new Action();
			action.ActionType = ActionType.Disconnect;
			action.Message = null;
			_pushAction( action );
		}
		/// <summary>
		/// 接受下一条网络消息
		/// </summary>
		private void _receiveNextMessage()
		{
			m_recv_head_args.AsyncArgs.SetBuffer( 0, RecvMessageHeadSize );
			m_recv_head_args.BytesWanted = RecvMessageHeadSize;
			m_recv_head_args.BytesReceived = 0;
			m_receiving_body = false;
			_receivePacket( m_recv_head_args );
		}
		/// <summary>
		/// 接受指定大小的网络包
		/// </summary>
		/// <param name="_arg">要接收的网络包参数数据</param>
		private void _receivePacket( ReceivePacketArg _arg )
		{
			try
			{
				_arg.BytesReceived = 0;
				_arg.AsyncArgs.SetBuffer( 0, _arg.BytesWanted );
				bool will_raise_event = false;
				lock ( m_connect_socket_lock )
				{
					will_raise_event = m_connect_socket.ReceiveAsync( _arg.AsyncArgs );
				}
				if( !will_raise_event )
					_onReceivePacket( null, _arg.AsyncArgs );
			}
			catch( Exception e )
			{
				Log.Error( "Failed to receive packet: {0}", e.Message );
			}
		}
		/// <summary>
		/// 网络包接收到的处理函数
		/// </summary>
		/// <param name="_sender">not used</param>
		/// <param name="_args">这次操作相关的网络异步事件参数</param>
		private void _onReceivePacket( Object _sender, SocketAsyncEventArgs _args )
		{
			try
			{
				if( _args.BytesTransferred > 0 && SocketError.Success == _args.SocketError )
				{
					ReceivePacketArg arg = _args.UserToken as ReceivePacketArg;
					arg.BytesReceived += _args.BytesTransferred;
					if( arg.BytesReceived == arg.BytesWanted )
					{
						if( m_receiving_body )
						{ // 正在接受消息体
							if( null != eventPacketArrived )
							{
								NetPacket packet = new NetPacket();
								packet.PacketType = PacketType.Message;
								packet.Data = _args.Buffer;
								packet.DataSize = arg.BytesWanted;
								eventPacketArrived( StreamId, packet );
							}
							m_receiving_body = false;
							_args.SetBuffer( null, 0, 0 );
							_receiveNextMessage();
						}
						else
						{ // 正在接受消息头
							UInt16 message_length_u16 = BitConverter.ToUInt16( _args.Buffer, 2 );
							//Int32 message_length = IPAddress.NetworkToHostOrder( Convert.ToInt32( message_length_u16 ) );

							//Int32 message_length = BitConverter.ToInt32( _args.Buffer, 0 );
							//message_length = IPAddress.NetworkToHostOrder( message_length );

							Byte[] buffer = new Byte[message_length_u16];
							m_recv_body_args.AsyncArgs.SetBuffer( buffer, 0, message_length_u16 );
							m_recv_body_args.BytesWanted = message_length_u16;
							m_recv_body_args.BytesReceived = 0;
							m_receiving_body = true;
							_receivePacket( m_recv_body_args );
						}
					}
					else if( arg.BytesReceived > arg.BytesWanted )
					{
						throw new Exception( "异常的网络数据接收：已接收字节数大于需求接收的字节数。" );
					}
					else
					{
						_args.SetBuffer( arg.BytesReceived, arg.BytesWanted - arg.BytesReceived );
						bool will_raise_event = false;
						lock ( m_connect_socket_lock )
						{
							will_raise_event = m_connect_socket.ReceiveAsync( _args );
						}
						if( !will_raise_event )
							_onReceivePacket( null, _args );
					}
				}
				else
				{
					if( 0 == _args.BytesTransferred )
					{
						_closeSocket();
						if( null != eventPacketArrived )
						{
							NetPacket packet = new NetPacket();
							packet.PacketType = PacketType.Disconnected;
							packet.Data = null;
							packet.DataSize = 0;
							eventPacketArrived( StreamId, packet );
						}
					}
					else
					{
						Disconnect();
					}
				}
			}
			catch( Exception e )
			{
				Log.Error( "Failed to receive net packet: {0}", e.Message );
			}
		}
		/// <summary>
		/// 发送数据到服务器
		/// </summary>
		/// <param name="_message">待发送的数据</param>
		/// <returns>连接失效返回false，否则返回true</returns>
		public bool Send( Byte[] _message, Int32 _length )
		{
			if( !IsConnected() )
				return false;
			Action action = new Action();
			action.ActionType = ActionType.SendMessage;
			Byte[] full_message = new Byte[_length + SendMessageHeadSize];

			//Int32 message_length_32 = IPAddress.HostToNetworkOrder( _length );
			//Int16 message_length_16 = IPAddress.HostToNetworkOrder( (Int16)_length );
			//UInt16 message_length_u16 = Convert.ToUInt16( message_length_16 );
			//Array.Copy( BitConverter.GetBytes( message_length_u16 ), 0, full_message, 6, 2 );

			UInt16 message_length_u16 = Convert.ToUInt16( _length );
			Array.Copy( BitConverter.GetBytes( message_length_u16 ), 0, full_message, 6, 2 );


			//Int32 message_length = IPAddress.HostToNetworkOrder( _length );
			//Array.Copy( BitConverter.GetBytes( message_length ), full_message, MessageHeadSize );
			Array.Copy( _message, 0, full_message, SendMessageHeadSize, _length );
			action.Message = full_message;
			_pushAction( action );
			return true;
		}

		private void _pushAction( Action _action )
		{
			lock ( m_action_lock )
			{
				m_actions.Enqueue( _action );
			}
			_doNextAction();
		}
		private void _doNextAction()
		{
			Action action = null;
			lock ( m_action_lock )
			{
				if( m_doing_action )
					return;
				if( 0 == m_actions.Count )
					return;
				action = m_actions.Dequeue();
				m_doing_action = true;
			}
			switch( action.ActionType )
			{
			case ActionType.SendMessage:
				_sendMessage( action.Message );
				break;
			case ActionType.Disconnect:
				_doDisconnect();
				break;
			}
		}

		private void _doDisconnect()
		{
			try
			{
				lock ( m_connect_socket_lock )
				{
					if (null == m_connect_socket)
					{
						//在没有创建socket的情况下，是收不到服务器断开连接返回的。所以把m_doing_action = false;
						lock ( m_action_lock )
						{
							m_doing_action = false;
						}
						return;
					}
					m_connect_socket.Shutdown( SocketShutdown.Both );
				}
			}
			catch( Exception e )
			{
				Log.Error( "Socket Error while disconnect from server: {0}", e.Message );
			}
		}
		private void _sendMessage( Byte[] _message )
		{
			try
			{
				m_send_arg.AsyncArgs.SetBuffer( _message, 0, _message.Length );
				m_send_arg.BytesWanted = _message.Length;
				m_send_arg.BytesSent = 0;
				bool will_raise_event = false;
				lock ( m_connect_socket_lock )
				{
					will_raise_event = m_connect_socket.SendAsync( m_send_arg.AsyncArgs );
				}
				if( !will_raise_event )
					_onSendPacket( null, m_send_arg.AsyncArgs );
			}
			catch( Exception e )
			{
				lock ( m_action_lock )
				{
					m_doing_action = false;
				}
				Log.Error( "开始发送网络包时发生异常：{0}", e.Message );
				Disconnect();
			}
		}
		private void _onSendPacket( Object _sender, SocketAsyncEventArgs _args )
		{
			try
			{
				if( SocketError.Success == _args.SocketError )
				{
					SendPacketArg arg = _args.UserToken as SendPacketArg;
					arg.BytesSent += _args.BytesTransferred;
					if( arg.BytesSent == arg.BytesWanted )
					{
						lock ( m_action_lock )
						{
							m_doing_action = false;
						}
						_doNextAction();
					}
					else if( arg.BytesSent > arg.BytesWanted )
					{
						throw new Exception( "异常的网络数据发送：已发送字节数大于要发送的字节数。" );
					}
					else
					{
						m_send_arg.AsyncArgs.SetBuffer( arg.BytesSent, arg.BytesWanted - arg.BytesSent );
						bool will_raise_event = false;
						lock ( m_connect_socket_lock )
						{
							will_raise_event = m_connect_socket.SendAsync( m_send_arg.AsyncArgs );
						}
						if( !will_raise_event )
							_onSendPacket( null, m_send_arg.AsyncArgs );
					}
				}
				else
				{
					Disconnect();
				}
			}
			catch( Exception e )
			{
				Log.Error( "发送网络包时发生异常：{0}", e.Message );
			}
		}
		private void _closeSocket()
		{
			lock ( m_connect_socket_lock )
			{
				try
				{
					if( null == m_connect_socket )
						return;
					m_connect_socket.Close();
					m_connect_socket = null;
				}
				catch( Exception e )
				{
					Log.Error( "Failed to shutdown the socket : {0}", e.Message );
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 StreamId
		{ get; private set; }

		enum ActionType
		{
			SendMessage,
			Disconnect,
		}
		class Action
		{
			public ActionType ActionType;
			public Byte[] Message;
		}
		/// <summary>
		/// 服务器连接套接字
		/// </summary>
		private readonly Object m_connect_socket_lock = new Object();
		private Socket m_connect_socket = null;
		/// <summary>
		/// 接收消息相关数据
		/// </summary>
		private readonly ReceivePacketArg m_recv_head_args = new ReceivePacketArg();
		private readonly ReceivePacketArg m_recv_body_args = new ReceivePacketArg();
		private bool m_receiving_body = false;
		/// <summary>
		/// 待发送消息列表
		/// </summary>
		private readonly Queue<Action> m_actions = new Queue<Action>();
		private bool m_doing_action = false;
		private readonly Object m_action_lock = new Object();
		private readonly SendPacketArg m_send_arg = new SendPacketArg();
	}
}
