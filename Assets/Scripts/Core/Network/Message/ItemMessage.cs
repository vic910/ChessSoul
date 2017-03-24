using System;
using System.Collections.Generic;
using System.Text;
using Groot.Network;
using Utility;
using SLua;

namespace Groot.Network
{
	internal class GC_GetItems : MessageBase
	{
		[MessageFiled( 0 )]
		public UInt64 PlayerID;

		[MessageFiled( 1 )]
		public Int32 PropItemsCount;

		[MessageFiled( 2, 35 )]
		public List<PropItem> PropItems;

		public GC_GetItems() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_PROPS, (ushort)EPropsMsgId.PROPS_GETPROPS_GC )
		{

		}
	}

	[CustomLuaClass]
	public class PropItem
	{
		[MessageFiled( 0 )]
		public UInt64 PropID; // 道具ID

		[MessageFiled( 1 )]
		public Int32 Count; // 道具数量
	}

	internal class GC_GetItemAttr : MessageBase
	{
		[MessageFiled( 0 )]
		public UInt32 TotalCount;

		[MessageFiled( 1 )]
		public UInt32 CurrentStartNo;

		[MessageFiled( 2 )]
		public UInt32 ItemsCount;

		[MessageFiled( 3, 100 )]
		public List<ItemAttr> ItemsAttr;

		public GC_GetItemAttr() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_PROPS, (ushort)EPropsMsgId.PROPS_GETATTR_GC )
		{

		}
	}

	[CustomLuaClass]
	public class ItemAttr
	{
		[MessageFiled( 0 )]
		public UInt64 PropID; //道具ID

		// 使用条件
		[MessageFiled( 1 )]
		public UInt64 NeedLiveness; //所需活跃度

		[MessageFiled( 2 )]
		public UInt64 NeedMoney; //所需棋币

		[MessageFiled( 3 )]
		public Int32 NeedPlayerLevel; //所需用户等级

		[MessageFiled( 4 )]
		public Byte Level; //道具等级

		// 道具属性
		[MessageFiled( 5 )]
		public Byte Type; //道具类型,参见EPropertyType

		[MessageFiled( 6 )]
		public UInt64 PriceGold; //系统定价,货币为元宝

		[MessageFiled( 7 )]
		public UInt64 PriceMoney; //系统定价,货币为棋魂币

		[MessageFiled( 8 )]
		public Byte CurrMoneyType; //当前指定售价类型

		[MessageFiled( 9 )]
		public UInt64 RecycleMoney; //系统回收价,仅棋魂币

		[MessageFiled( 10, 28 )]
		public List<Byte> Attrs; //28    

		[MessageFiled( 11, 30 )]
		public string Name; //30道具名称

		[MessageFiled( 12, 50 )]
		public string Desc; //50道具描述

		//		union
		//    {
		//        struct

		//		{

		//			UINT64 iMaxEnableMoney;     //道具最大有效钱数
		//		float  fAssurance;          //押分保险率
		//		float  fMultiplication;     //押分返还倍率
		//	}
		//	ImpawnAttr;

		//        char   cByPassStep;             //另对方停手手数

		//	struct

		//		{

		//			int    iEnableMinutes;      //有效期,分钟,-1为不限
		//	int    iPeriod;             //时钟周期,分钟
		//	int    iMaxUseCount;        //最大重叠使用数量
		//	INT64  iMoneyPerPeriod;     //有效期内每时间周期调整的棋币
		//	INT64  iLivenessPerPeriod;  //有效期内每时间周期调整的棋币
		//}
		//OnlineAttr;

		//		struct
		//		{
		//			UINT64 iLiveness;			//活跃度
		//		}LivenessCard;

		//    }Attrs;

		//	}
	}

	enum EPropertyType
	{
		PROPERTY_IMPAWN_ASSURANCE,          //押分保险道具
		PROPERTY_IMPAWN_MULTIPLY,           //押分倍率道具
		PROPERTY_IMPAWN_COMBO,              //押分倍率保险双属性道具
		PROPERTY_IMPAWN_END,                //押分道具结束
		PROPERTY_PLAYGO,                    //对局道具
		PROPERTY_KEY,                       //钥匙道具
		PROPERTY_BOX,                       //宝箱类道具
		PROPERTY_ONLINETIMER,               //在线定时处理类道具
		PROPERTY_LIVENESS,                  //活跃度卡类型
		PROPERTY_ORDER,                     //订单道具
		PROPERTY_GOLEVEL,                   //段位更改道具
		PROPERTY_SHOWNAME,                  //炫彩昵称卡
		PROPERTY_IMPAWN_CLEAR,              //押分战线清零卡
		PROPERTY_GENDER,                    //性别更改卡
		PROPERTY_CHANGE_USERNAME,           //更名道具
		PROPERTY_OTHER,                     //杂项道具
		PROPERTY_MYD_REFINE_LIQUID = 100,     //梦游戏岛炼制药水             
	};
}