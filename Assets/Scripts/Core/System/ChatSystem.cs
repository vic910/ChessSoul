using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Groot;
using Groot.Network;
using Utility;
using Weiqi;

public class ChatSystem
{
	public static readonly ChatSystem Instance = new ChatSystem();
	public static readonly Int32 MaxChatCount = 30;

	private Dictionary<Byte,LinkedList<GC_ChatMsg>> ChatInfo = new Dictionary<Byte, LinkedList<GC_ChatMsg>>();

	public void Initialize()
	{
		NetManager.Instance.Register<GC_ChatMsg>( _onPacketArrived );
	}

	public void Uninitialize()
	{
		ChatInfo.Clear();
		NetManager.Instance.Unregister<GC_ChatMsg>();
	}

	public LinkedList<GC_ChatMsg> GetChats( EChatType _type )
	{
		if( !ChatInfo.ContainsKey( (Byte)_type ) )
			return null;
		return ChatInfo[(Byte)_type];
	}

	private void _onPacketArrived( Int32 _stream_id, PacketType _packet_type, GC_ChatMsg _msg )
	{
		switch( (EChatType)_msg.ChatType )
		{
		case EChatType.CHAT_HALL:
		case EChatType.CHAT_ROOM:
		case EChatType.CHAT_CLUB:
		case EChatType.CHAT_TRADE:
			{
				if( ChatInfo.ContainsKey( _msg.ChatType ) )
				{
					if( ChatInfo[_msg.ChatType].Count >= MaxChatCount )
						ChatInfo[_msg.ChatType].RemoveFirst();
					ChatInfo[_msg.ChatType].AddLast( _msg );
				}
				else
				{
					LinkedList<GC_ChatMsg> chats = new LinkedList<GC_ChatMsg>();
					chats.AddLast( _msg );
					ChatInfo.Add( _msg.ChatType, chats );
				}
				SignalSystem.FireSignal( SignalId.Chat_ReceiveChat, _msg );
			}
			break;		
		}
	}
}
