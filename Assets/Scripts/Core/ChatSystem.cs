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

	public LinkedList<GC_ChatMsg> ChatInfo { get; private set; }

	public void Initialize()
	{
		ChatInfo = new LinkedList<GC_ChatMsg>();
		NetManager.Instance.Register<GC_ChatMsg>( _onPacketArrived );
	}

	public void Uninitialize()
	{
		ChatInfo.Clear();
		NetManager.Instance.Unregister<GC_ChatMsg>();
	}

	private void _onPacketArrived( Int32 _stream_id, PacketType _packet_type, GC_ChatMsg _msg )
	{
		if( ChatInfo.Count >= MaxChatCount )
			ChatInfo.RemoveFirst();
		ChatInfo.AddLast( _msg );
		SignalSystem.FireSignal( SignalId.Chat_ReceiveChat, _msg );
	}
}
