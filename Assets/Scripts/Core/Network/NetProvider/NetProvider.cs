using System;

namespace Groot.Network
{
	class NetProvider
	{
		public NetProvider()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual bool Initialize()
		{
			throw new NotImplementedException( "NetProvider: Initialize isn't implemented.");
		}
		/// <summary>
		/// 
		/// </summary>
		public virtual void Uninitialize()
		{
			throw new NotImplementedException( "NetProvider: Uninitialize isn't implemented.");
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Login()
		{
			throw new NotImplementedException( "NetProvider: Login isn't implemented.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_msg"></param>
		/// <returns></returns>
		public virtual bool SendMsg( MessageBase _msg )
		{
			throw new NotImplementedException( "NetProvider: SendMsg isn't implemented.");
		}

		/// <summary>
		/// 
		/// 
		/// </summary>
		/// <param name="_packet_id"></param>
		/// <param name="_preprocessor"></param>
		public virtual void RegisterPacketPreprocessor( Int32 _packet_id, NetPacketPreprocessor _preprocessor )
		{
			throw new NotImplementedException( "NetProvider: RegisterPacketPreprocessor isn't implemented.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_packet_id"></param>
		public virtual void UnregisterPacketPreprocessor( Int32 _packet_id )
		{
			throw new NotImplementedException( "NetProvider: UnregisterPacketPreprocessor isn't implemented.");
		}
	}
}
