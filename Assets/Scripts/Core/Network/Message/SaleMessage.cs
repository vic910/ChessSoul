using System;
using System.Collections.Generic;
using System.Text;
using Groot.Network;
using Utility;

namespace Groot.Network
{
    class CG_UserSaleToSystem : MessageBase
    {
        [MessageFiled(0)]
        public UInt64 PlayerID;

        [MessageFiled(1)]
        public UInt32 SaleItemCount;

        [MessageFiled(2, 1)]
        public List<PropItem> Items;

        public CG_UserSaleToSystem() : base(EMsgDirection.MSG_CG, EMsgType.TYPE_SALE, (ushort)ESaleMsgId.SALE_USERSALETOSYSTEM_CG)
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

        [MessageFiled(3, 100)]
        public List<SaleItem> SaleItem;  //道具

        public GC_GetAllSales() : base(EMsgDirection.MSG_GC, EMsgType.TYPE_SALE, (ushort)ESaleMsgId.SALE_GETALLSALE_GC)
        {

        }
    };

    class CG_Buy : MessageBase
    {
        [MessageFiled(0)]
        public UInt64 BuyerID; // 购买者ID
        [MessageFiled(1)]
        public UInt32 BuyItemCount; // 一直次购买多少
        [MessageFiled(2, 30)]
        public List<BuyItem> BuyItems; //购买信息

        public CG_Buy() : base(EMsgDirection.MSG_CG, EMsgType.TYPE_SALE, (ushort)ESaleMsgId.SALE_BUY_CG)
        {

        }
    };

    class BuyItem
    {
        [MessageFiled(0)]
        public UInt64 SaleID; // 临时商铺ID( 这个叫道具ID准确些)
        [MessageFiled(1)]
        public UInt32 BuyCount; // 购买的此道具数量
    };

    class GC_Buy : MessageBase
    {
        [MessageFiled(0)]
        public UInt32 BuyStatusCount;
        [MessageFiled(1, 30)]
        public List<BuyRetItem> BuyStatusItem;

        public GC_Buy() : base(EMsgDirection.MSG_GC, EMsgType.TYPE_SALE, (ushort)ESaleMsgId.SALE_BUY_GC)
        {

        }
    };

    public class BuyRetItem
    {
        [MessageFiled(0)]
        public Byte Status;           // 参见EIGoRetStatus
        [MessageFiled(1)]
        public UInt64 SaleID;         // 临时商铺ID，GS生成，生存期为出售起至停售或售出止
        [MessageFiled(2)]
        public UInt32 BuyCount;         // 购买数量
    };

    public class SaleItem
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
        public Byte ItemPresentCount;                     // 赠送项数目
        [MessageFiled(5, 10)]
        public List<PresentItem> ItemPresent; // 增送项目(道具ID,数量).0表示棋币,1表示元宝,2表示活跃度    
        [MessageFiled(6)]
        public Byte SaleType;                         // 1:系统商铺,2:用户商铺  参见上面 enum
        [MessageFiled(7)]
        public Byte MoneyType;                        // 货币种类，1：棋魂币，2：元宝
        [MessageFiled(8)]
        public UInt64 Price;                      // 出售单价
        [MessageFiled(9, 255)]
        public string Desc;             // 商铺描述
    };

    //赠送道具
    public class PresentItem
    {
        [MessageFiled(0)]
        public UInt64 PropertyID;     //赠送道具ID
        [MessageFiled(1)]
        public Int32 count;          //赠送道具数量
    };

    enum EIGoRetStatus
    {
        // 公用
        IGORET_SUCCESS,                         // 成功
        IGORET_FAILED,                          // 失败,未知原因
        IGORET_TIMEOUT,                         // 超时

        // 对局
        IGORET_PLAYGO_PLAYEROFFLINE = 0x10,     // 用户不在线
        IGORET_PLAYGO_DISAGREE,                 // 对方拒绝
        IGORET_PLAYGO_CANNOTINVITE,             // 对方设置为不允许邀请
        IGORET_PLAYGO_ERRORSTATE,               // 错误的用户状态
        IGORET_PLAYGO_ROOMFULL,                 // 玩家当前房间已满
        IGORET_PLAYGO_NOENOUGHMONEY,            // 玩家彩棋金额不足
        IGORET_PLAYGO_OTHER_NOTINROOM,          // 对方不在房间内
        IGORET_PLAYGO_ROOM_DESTORYED,           // 房间不存在
        IGORET_PLAYGO_ROOM_ISPLAYING,           // 房间正在对局中
        IGORET_PLAYGO_NOONE_ISROOMOWNER,        // 双方没有一个是房间拥有者
        IGORET_PLAYGO_REQUESTER_NOTEXIST,       // 请求者不存在
        IGORET_PLAYGO_SERVERROOMFULL,           // 创建房间失败,服务器房间已满
        IGORET_PLAYGO_SAMEPC,                   // 同一台电脑不可申请对弈
        IGORET_PLAYGO_SAMEIP,                   // 同一IP的电脑不可申请对弈

        // 公用
        IGORET_WORLDSERVERDISCONNECTED,         // 世界服务器不可连接
        IGORET_LOGINSERVERDISCONNECTED,         // 登录服务器不可连接

        // 交易
        IGORET_TRADE_NOENOUGHPROPERTY,          // 无足够的道具用来交易
        IGORET_TRADE_NOENOUGHMONEY,             // 无足够的货币用来交易
        IGORET_TRADE_NOTTRADEPLAYER,            // 非交易玩家
        IGORET_TRADE_NOTEXIST,                  // 交易不存在
        IGORET_TRADE_AGREE,                     // 用户同意交易
        IGORET_TRADE_DISAGREE,                  // 用户拒绝交易
        IGORET_TRADE_INVALIDPROPERTY,           // 非法道具
        IGORET_TRADE_PLAYEROFFLINE,             // 用户不在线
        IGORET_TRADE_STATE_ERROR,               // 用户状态错误

        // 商铺
        IGORET_SALE_NOENOUGHPROPERTY,           // 无足够的道具用来出售
        IGORET_SALE_NOENOUGHMONEY,              // 无足够的货币用来支付手续费或购买
        IGORET_SALE_NOTEXIST,                   // 商铺不存在
        IGORET_SALE_NOTOWNER,                   // 不是商铺所有人
        IGORET_SALE_CANNOTBUYYOURSELF,          // 不能自己买自己的东西
        IGORET_SALE_INVALIDPROPERTY,            // 非法道具
        IGORET_SALE_PLAYEROFFLINE,              // 用户不在线
        IGORET_SALE_LS_DECREASEGOLD_FAILED,     // LS扣除元宝失败
        IGORET_SALE_LS_LOGGOLDACTION_FAILED,    // LS元宝日志记录错误

        // 道具使用
        IGORET_PROP_NOENOUGH_LIVENESS,          // 无足够的活跃度
        IGORET_PROP_NOENOUGH_PROPLEVEL,         // 道具等级不匹配
        IGORET_PROP_NOENOUGH_MONEY,             // 无足够的金钱
        IGORET_PROP_NOENOUGH_PLAYERLEVEL,       // 无足够的用户等级
        IGORET_PROP_UPTOMAXLIMIT,               // 达到使用数量限制
        IGORET_PROP_NOTEXIST,                   // 无此类型道具
        IGORET_PROP_NOENOUGH_BAGSPACE,          // 背包容量不足

        // 注册
        IGORET_REG_AREANOTEXIST,                // 地区不存在

        // 2013-07-31 add by nava
        IGORET_PLAYGO_CLUB_NOPRIVILEGE,         // 没有权限申请帮派对战
        IGORET_PLAYGO_CLUB_ISPLAYING,           // 帮派已经处于对局阶段
    };
}