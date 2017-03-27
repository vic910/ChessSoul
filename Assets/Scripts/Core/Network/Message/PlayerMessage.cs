using System;
using System.Collections.Generic;
using System.Text;
using Groot.Network;
using Utility;

namespace Groot.Network
{
	/// <summary>
	/// 更新元宝消息
	/// </summary>
	class GC_UpdatePlayerGold : MessageBase
	{
		[MessageFiled( 0 )]
		public Byte Reason;     //EItemReason

		[MessageFiled( 1 )]
		public Int32 GoldNow;

		public GC_UpdatePlayerGold() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_GAME, (ushort)EGameMsgId.GAME_UPDATE_PLAYER_GOLD_GC )
		{
			
		}
	}

	/// <summary>
	/// 更新银两
	/// </summary>
	class GC_UpdatePlayerMoney : MessageBase
	{
		[MessageFiled( 0 )]
		public Byte Reason;     //EItemReason

		[MessageFiled( 1 )]
		public Int64 GoldNow;

		public GC_UpdatePlayerMoney() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_GAME, (ushort)EGameMsgId.GAME_UPDATE_PLAYER_MONEY_GC )
		{

		}
	}
}