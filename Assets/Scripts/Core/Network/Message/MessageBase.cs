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
		public MessageBase( Byte _type, Byte _dir, UInt16 _msg_id )
		{
			MsgType = _type;
			MsgId = _msg_id;
			MsgDirection = _dir;
		}
	}

	class ConnectMsg : MessageBase
	{
		public static readonly Byte Type = 255;
		public static readonly Byte Dir = 0;
		public static readonly UInt16 MessageId = 65535;

		public ConnectMsg()
			: base( Type, Dir, MessageId )
		{
		}
	}

	class DisconnectMsg : MessageBase
	{
		public static readonly Byte Type = 255;
		public static readonly Byte Dir = 0;
		public static readonly UInt16 MessageId = 65534;

		public DisconnectMsg()
			: base( Type, Dir, MessageId )
		{
		}
	}

	class CG_LoginRequestMsg : MessageBase
	{
		[MessageFiled(0)]
		public Byte PlatformID;

		[MessageFiled(1,33)]
		public string PlayerName;

		[MessageFiled(2,18)]
		public string PlayerPassword;

		[MessageFiled(3,33)]
		public string Md5;

		[MessageFiled(4)]
		public Int32 Version;

		[MessageFiled(5)]
		public Byte NameType;

		[MessageFiled(6)]
		public UInt64 Mac;

		public CG_LoginRequestMsg() : base( 16, 1, 1 )
		{
			
		}
	}

	class GC_LoginFailedMsg : MessageBase
	{
		[MessageFiled(0)]
		public Int32 Reason;

		[MessageFiled(1,33)]
		public string PlayerName;

		[MessageFiled(2,18)]
		public string PlayerPassword;

		[MessageFiled(3)]
		public UInt64 LeaveTimeWhenForbid;

		public GC_LoginFailedMsg() : base( 16, 2, 3 )
		{
			
		}
	}
}