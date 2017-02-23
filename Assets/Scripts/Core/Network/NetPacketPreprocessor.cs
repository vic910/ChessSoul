using System;

namespace Groot.Network
{
	#region NetPacketPreprocessor helper class
	abstract class NetPacketPreprocessor
	{
		public abstract NetPacketEvent CreateInstance();
		public abstract void Invalid();
	}
	class TNetPacketPreprocessor<TyMessage> : NetPacketPreprocessor where TyMessage : MessageBase, new()
	{
		//private readonly Action<Int32, PacketType, TyMessage> m_handler;
		private readonly PacketHandler<TyMessage> m_handler = new PacketHandler<TyMessage>();
		public TNetPacketPreprocessor( Action<Int32, PacketType, TyMessage> _handler )
		{
			m_handler.Handler = _handler;
			m_handler.Invalid = false;
		}
		public override NetPacketEvent CreateInstance()
		{
			return new TNetPacketEvent<TyMessage>( m_handler );
		}

		public override void Invalid()
		{
			m_handler.Invalid = true;
		}
	}
	class TNetPacketPreprocessorEx<TyMessage> : NetPacketPreprocessor
		where TyMessage : MessageBase, new()
	{
		//private readonly Action<Int32, NetPacket, TyMessage> m_handler;
		private readonly PacketHandlerEx<TyMessage> m_handler = new PacketHandlerEx<TyMessage>();
		public TNetPacketPreprocessorEx( Action<Int32, NetPacket, TyMessage> _handler )
		{
			m_handler.Handler = _handler;
			m_handler.Invalid = false;
		}
		public override NetPacketEvent CreateInstance()
		{
			return new TNetPacketEventEx<TyMessage>( m_handler );
		}
		public override void Invalid()
		{
			m_handler.Invalid = true;
		}
	}
	#endregion
}
