using System;
using System.Collections.Generic;
using System.Text;
using Groot.Network;
using Utility;

namespace Groot.Network
{
	class CG_UserSaleToSystem : MessageBase
	{
		[MessageFiled( 0 )]
		public UInt64 PlayerID;

		[MessageFiled( 1 )]
		public UInt32 SaleItemCount;

		[MessageFiled( 2, 1 )]
		public List<PropItem> Items;

		public CG_UserSaleToSystem() : base( EMsgDirection.MSG_CG, EMsgType.TYPE_SALE, (ushort)ESaleMsgId.SALE_USERSALETOSYSTEM_CG )
		{
			
		}
	};

    class GC_GetAllSales : MessageBase
    {
        [MessageFiled(0)]
        public UInt32 TotalCount;       //商城道具总数

        [MessageFiled(1)]
        public UInt32 CurrentStartNo;   //记录本次消息发送的起始道具在总数的序列数

        [MessageFiled(2)]
        public UInt32 SaleItemCount;    //发送道具个数

        [MessageFiled(3,100)]
        public List<SaleItem> SaleItem;  //道具

        public GC_GetAllSales() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_SALE, (ushort)ESaleMsgId.SALE_GETALLSALE_GC )
		{

        }
    };

    class SaleItem
    {
        //enum
        //{
        //    SALES_SYSTEM,   //系统商铺
        //    SALES_USER,     //用户商铺
        //    SALES_RECYCLE,  //系统回收商铺

        //    MAX_PRESEND_ITEM = 10, //最大赠送道具数量
        //}
        [MessageFiled(0)]
        public UInt64 SaleID;                         // 商铺ID
        [MessageFiled(1)]
        public UInt64 PlayerID;                       // 出售者ID
        [MessageFiled(2)]
        public UInt64 ItemID;                           // 要出售的道具ID
        [MessageFiled(3)]
        public UInt32 ItemCount;                      // 要出售的道具数量
        [MessageFiled(4)]
        public Byte   ItemPresentCount;                     // 赠送项数目
        [MessageFiled(5,10)]
        public List<PresentItem> ItemPresent; // 增送项目(道具ID,数量).0表示棋币,1表示元宝,2表示活跃度    
        [MessageFiled(6)]
        public Byte SaleType;                         // 1:系统商铺,2:用户商铺  参见上面 enum
        [MessageFiled(7)]
        public Byte MoneyType;                        // 货币种类，1：棋魂币，2：元宝
        [MessageFiled(8)]
        public UInt64 iPrice;                      // 出售单价
        [MessageFiled(9,255)]
        public string szDesc;             // 商铺描述
    };

    //赠送道具
    struct PresentItem
    {
        [MessageFiled(0)]
        public UInt64 PropertyID;     //赠送道具ID
        [MessageFiled(1)]
        public Int32 count;          //赠送道具数量
    };
}