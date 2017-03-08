using System;
using System.Text;
using Utility;

namespace Groot.Network
{
	/// <summary>
	/// 接收消息时的头
	/// </summary>
	public class RecvMessageHead
	{
		[MessageFiled( 0 )]
		public UInt16 Seqnum;

		[MessageFiled( 1 )]
		public UInt16 Paclen;
	}

	/// <summary>
	/// 发送消息时的头
	/// </summary>
	public class SendMessageHead
	{
		[MessageFiled( 0 )]
		public UInt32 Crc32 = 0;

		[MessageFiled( 1 )]
		public RecvMessageHead Head;

		[MessageFiled( 2 )]
		public UInt64 Signature = 0;
	}

	/// <summary>
	/// 消息基类
	/// </summary>
	public class MessageBase
	{
		/// <summary>
		/// 用于区分不同服务器消息和不同平台特定消息
		/// </summary>
		[MessageFiled( 0 )]
		public UInt16 MsgId;
		/// <summary>
		/// 消息类型
		/// </summary>
		[MessageFiled( 1 )]
		public Byte MsgType;
		/// <summary>
		/// 消息流向
		/// </summary>
		[MessageFiled( 2 )]
		public Byte MsgDirection;
		public MessageBase() {}
		public MessageBase( EMsgDirection _dir, EMsgType _type, UInt16 _msg_id )
		{
			MsgDirection = (Byte)_dir;
			MsgType = (Byte)_type;
			MsgId = _msg_id;
		}
	}

	class ConnectMsg : MessageBase
	{
		public static Byte   Dir  = (Byte)EMsgDirection.MSG_MAX;
		public static Byte   Type = (Byte)EMsgType.TYPE_MAX;
		public static UInt16 MessageId = (UInt16)ELoginMsgId.CONNECT;

		public ConnectMsg()
			: base( EMsgDirection.MSG_MAX, EMsgType.TYPE_MAX, (UInt16)ELoginMsgId.CONNECT )
		{
		}
	}

	class DisconnectMsg : MessageBase
	{
		public static Byte   Dir  = (Byte)EMsgDirection.MSG_MAX;
		public static Byte   Type = (Byte)EMsgType.TYPE_MAX;
		public static UInt16 MessageId = (UInt16)ELoginMsgId.DIS_CONNECT;
		public DisconnectMsg()
			: base( EMsgDirection.MSG_MAX, EMsgType.TYPE_MAX, (UInt16)ELoginMsgId.DIS_CONNECT )
		{
		}
	}
}