using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Utility;

namespace Groot.Network
{
	public class NetManager
	{
		enum NetProviderType
		{
			Default,
			Localhost,
		}
		/// <summary>
		/// The instance
		/// </summary>
		public static readonly NetManager Instance = new NetManager();
		/// <summary>
		/// the actual net provider
		/// </summary>
		private NetProvider m_net_provider = null;

		private Utility.Factory.SimpleFactory<NetProviderType, NetProvider> m_provider_factory
			= new Utility.Factory.SimpleFactory<NetProviderType, NetProvider>();

		private NetManager()
		{
			m_provider_factory.Register<NetProvider_Default>( NetProviderType.Default );
			//m_provider_factory.Register<NetProvider_Localhost>( NetProviderType.Localhost );
		}

		public Boolean Initialize()
		{
			// create net provider according to config
			m_net_provider = m_provider_factory.CreateInstance( NetProviderType.Default );
			// initialize
			return m_net_provider.Initialize();
		}

		public void Uninitialize()
		{
			m_net_provider.Uninitialize();
			m_net_provider = null;
		}

		public bool SendMsg( MessageBase _msg )
		{
			return m_net_provider.SendMsg( _msg );
		}


		public void Register<TyMessage>( Action<Int32, PacketType, TyMessage> _handler )
			where TyMessage : MessageBase, new()
		{
			TyMessage obj = new TyMessage();
			m_net_provider.RegisterPacketPreprocessor( Helper.GenerateInt32( obj.MsgType, obj.MsgDirection, obj.MsgId )
				, new TNetPacketPreprocessor<TyMessage>( _handler ) );
		}
		/// <summary>
		/// 本函数为高级消息注册回调，不懂就莫用！
		/// </summary>
		/// <typeparam name="TyMessage"></typeparam>
		/// <param name="_handler"></param>
		public void RegisterEx<TyMessage>( Action<Int32, NetPacket, TyMessage> _handler )
			where TyMessage : MessageBase, new()
		{
			TyMessage obj = new TyMessage();
			m_net_provider.RegisterPacketPreprocessor( Helper.GenerateInt32( obj.MsgType, obj.MsgDirection, obj.MsgId )
				, new TNetPacketPreprocessorEx<TyMessage>( _handler ) );
		}

		public void Unregister<TyPacket>()
			where TyPacket : MessageBase, new()
		{
			TyPacket obj = new TyPacket();
			m_net_provider.UnregisterPacketPreprocessor( Helper.GenerateInt32( obj.MsgType, obj.MsgDirection, obj.MsgId ) );
		}


		public void RequestConnect()
		{
			m_net_provider.RequestConnect();
		}

		public void RequestDisConnect()
		{
			m_net_provider.RequestDisConnect();
		}

	}
}