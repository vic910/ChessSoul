using System;

namespace Groot.Network
{
	internal abstract class NetPacketEvent : Groot.GameEvent
	{
		public abstract bool Initialize( Int32 _stream_id, NetPacket _packet );
	}

	internal class PacketHandler<TyMessage>
	{
		public Action<Int32, PacketType, TyMessage> Handler;
		public bool Invalid = false;
	}
	internal class TNetPacketEvent<TyMessage> : NetPacketEvent
		where TyMessage : MessageBase, new()
	{
		private Int32 m_stream_id = 0;
		private TyMessage m_message = null;
		private PacketType m_packet_type = PacketType.Error;
		//private readonly Action<Int32, PacketType, TyMessage> m_handler;
		private readonly PacketHandler<TyMessage> m_handler;

		public TNetPacketEvent( PacketHandler<TyMessage> _handler )
		{
			m_handler = _handler;
		}
		public override bool Initialize( Int32 _stream_id, NetPacket _packet )
		{
			m_stream_id = _stream_id;
			m_packet_type = _packet.PacketType;
			if( _packet.Data == null )
				m_message = new TyMessage();
			else
				m_message = SerializerHelper.Deserializer<TyMessage>( _packet.Data );
			Utility.Log.Info( "收到消息{0}", m_message.ToString() );
			return true;
		}

		public override void ExecuteEvent()
		{
			if( !m_handler.Invalid )
				m_handler.Handler( m_stream_id, m_packet_type, m_message );
		}
	}

	internal class PacketHandlerEx<TyMessage>
	{
		public Action<Int32, NetPacket, TyMessage> Handler;
		public bool Invalid = false;
	}
	internal class TNetPacketEventEx<TyMessage> : NetPacketEvent
		where TyMessage : MessageBase, new()
	{
		private Int32 m_stream_id = 0;
		private TyMessage m_message = null;
		private NetPacket m_packet = null;
		//private readonly Action<Int32, NetPacket, TyMessage> m_handler;
		private readonly PacketHandlerEx<TyMessage> m_handler;

		public TNetPacketEventEx( PacketHandlerEx<TyMessage> _handler )
		{
			m_handler = _handler;
		}
		public override bool Initialize( Int32 _stream_id, NetPacket _packet )
		{
			m_stream_id = _stream_id;
			m_packet = _packet;
			if( _packet.Data == null )
				m_message = new TyMessage();
			else
				m_message = SerializerHelper.Deserializer<TyMessage>( _packet.Data );
			return true;
		}

		public override void ExecuteEvent()
		{
			if( !m_handler.Invalid )
				m_handler.Handler( m_stream_id, m_packet, m_message );
		}
	}
}
