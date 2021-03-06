﻿using System;
using System.Collections.Generic;
using System.Text;
using Groot.Network;
using Utility;

namespace Groot.Network
{
	class GC_GetItems : MessageBase
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

	class GC_UpdateItems : MessageBase
	{
		[MessageFiled( 0 )]
		public UInt64      PlayerID;

		[MessageFiled( 1 )]
		public Int32       PropItemsCount;

		[MessageFiled( 2 )]
		public Byte        Reason;     //EItemReason

		[MessageFiled( 3, 35 )]
		public List<PropItem> PropItems;

		public GC_UpdateItems() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_PROPS, (ushort)EPropsMsgId.PROPS_UPDATEPROPERTY_GC )
		{

		}
	};

	public class PropItem
	{
		[MessageFiled( 0 )]
		public UInt64 PropID; // 道具ID

		[MessageFiled( 1 )]
		public Int32 Count; // 道具数量
	}

	class GC_GetItemAttr : MessageBase
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
		PROPERTY_IMPAWN_ASSURANCE,          //押分保险道具(减损卡)
		PROPERTY_IMPAWN_MULTIPLY,           //押分倍率道具(加倍卡)
		PROPERTY_IMPAWN_COMBO,              //押分倍率保险双属性道具
		PROPERTY_IMPAWN_END,                //押分道具结束
		PROPERTY_PLAYGO,                    //对局道具
		PROPERTY_KEY,                       //钥匙道具
		PROPERTY_BOX,                       //宝箱类道具（宝箱）
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

	enum EMoneyType
	{
		PROP_ISMONEY = 1,           //棋魂币
		PROP_ISGOLD,                //元宝,由于帐号服务器限制,元宝将不可交易
		PROP_LESSIDVALUE = 1001,    //道具最小ID
	};

	enum EItemReason
	{
		IGO_REASON_UNKNOWN,        //未知
		IGO_REASON_PLAYGO,         //对局费
		IGO_REASON_GOPRODUCE,      //对局掉落物品
		IGO_REASON_OPENBOX,        //宝箱开出物品
		IGO_REASON_PLAYGOWIN,      //对局抽成
		IGO_REASON_PLAYGOLOTTERY,  //对局抽奖
		IGO_REASON_FACE,           //大表情
		IGO_REASON_MAGICFACE,      //魔法表情
		IGO_REASON_HEADPIC,        //设置头像
		IGO_REASON_PAYMENT,        //支付
		IGO_REASON_IMPAWN,         //押分
		IGO_REASON_USE,            //使用道具(针对可独立使用的道具及被该道具所支配的附加道具)
		IGO_REASON_TRADE,          //交易
		IGO_REASON_SALE,           //商铺卖出或购买
		IGO_REASON_SALEFEE,        //商铺管理费
		IGO_REASON_SALEGIFT,       //商铺赠送
		IGO_REASON_LOTTERY,        //定期抽奖
		IGO_REASON_AWARD,          //押分抽奖
		IGO_REASON_EXPLAINFEE,     //讲解收费
		IGO_REASON_BROADCASTFEE,   //直播收费
		IGO_REASON_ONLINE,         //在线奖励
		IGO_REASON_RECYCLE,        //系统回收
		IGO_REASON_BANK,           //银行操作
		IGO_REASON_MYDAWARD,       //梦游岛抽奖
		IGO_REASON_GMMODIFY,       //GM调整
		IGO_REASON_CHAT,           //聊天
		IGO_REASON_MYD_UPLOAD,     //梦游岛上传道具
		IGO_REASON_ADJOURNMENT,     // 封盘费用 2014-01-22 add by nava
		IGO_REASON_USELIVENESSCARD,       //使用活跃度卷轴
		IGO_REASON_MAX,
	};
}