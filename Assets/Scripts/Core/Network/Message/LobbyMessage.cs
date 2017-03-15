using System;
using System.Collections.Generic;
using System.Text;
using Groot.Network;
using Utility;
namespace Groot.Network
{
	//登陆发送大厅内所有玩家信息
    class GC_HallPlayerInfo : MessageBase
	{
		[MessageFiled(0)]
		public UInt32   Size;

		[MessageFiled(1)]
		public UInt32   TotalPlayerCount;

		[MessageFiled(2)]
		public UInt16   PlayerCount;

		[MessageFiled(3,100)]
		public List<PlayerInfoBase>  PlayerInfo;

		public GC_HallPlayerInfo() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_GAME, (UInt16)EGameMsgId.GAME_HALL_PLAYER_INFO_GC )
		{

		}
	};


	//一个用户登录后,服务器通知所有玩家该用户上线
	class GC_PlayerJoinGame : MessageBase
	{
		[MessageFiled(0)]
		public Int16 Count;

		[MessageFiled(3,10)]
		public List<PlayerInfoBase>  PlayerInfo;

		public GC_PlayerJoinGame() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_GAME, (UInt16)EGameMsgId.GAME_PLAYER_JOIN_GC )
		{

		}
	};

}