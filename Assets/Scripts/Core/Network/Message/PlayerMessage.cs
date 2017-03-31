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
        [MessageFiled(0)]
        public Byte Reason;     //EItemReason

        [MessageFiled(1)]
        public Int32 GoldNow;

        public GC_UpdatePlayerGold() : base(EMsgDirection.MSG_GC, EMsgType.TYPE_GAME, (ushort)EGameMsgId.GAME_UPDATE_PLAYER_GOLD_GC)
        {

        }
    }

    class GC_UpdateDeltaGold : MessageBase
    {
        public Int32 DeltaGold;   // 调整数量(当为负,可实现减少元宝)
        public Byte Reason;         //参见:EIGoSoulReason
        public UInt64 RequestUUID;// 不用管

        public GC_UpdateDeltaGold() : base(EMsgDirection.MSG_GC, EMsgType.TYPE_GAME, (ushort)EGameMsgId.GAME_UPDATE_GOLD_DELTA_GC)
        {

        }
    };
  
    /// <summary>
    /// 更新银两
    /// </summary>
    class GC_UpdatePlayerMoney : MessageBase
    {
        [MessageFiled(0)]
        public Byte Reason;     //EItemReason

        [MessageFiled(1)]
        public Int64 GoldNow;

        public GC_UpdatePlayerMoney() : base(EMsgDirection.MSG_GC, EMsgType.TYPE_GAME, (ushort)EGameMsgId.GAME_UPDATE_PLAYER_MONEY_GC)
        {

        }
    }

    class GC_UpdateDeltaMoney :MessageBase
    {
        [MessageFiled(0)]
        public Byte Reason;       // 参见:EIGoSoulReason
        [MessageFiled(1)]
        public UInt64 DeltaMoney; // 调整金钱数量 ( 当值为负数时,可实现减少金钱)
        [MessageFiled(2)]
        public bool plus;      // 提示作用 true: 增加; false: 减少
        [MessageFiled(3)]
        public UInt64 RequestUUID;    // 不用管

        public GC_UpdateDeltaMoney() : base(EMsgDirection.MSG_GC, EMsgType.TYPE_GAME, (ushort)EGameMsgId.GAME_UPDATE_MONEY_DELTA_GC)
        {

        }
    };

    //更新用户活跃度
    class GC_UpdatePlayerLiveness : MessageBase
    {
        [MessageFiled(0)]
        public Byte Reason;     //参见:EIGoSoulReason 更新原因描述

        [MessageFiled(1)]
        public Int32 Liveness;  //活跃度增量

        public GC_UpdatePlayerLiveness() : base(EMsgDirection.MSG_GC, EMsgType.TYPE_GAME, (ushort)EGameMsgId.GAME_UPDATE_PLAYERLIVENESS_GC)
        {

        }
    };
}