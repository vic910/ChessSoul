using System;
using System.Collections.Generic;
using System.Text;

namespace Groot.Network
{
	public class MsgDefine
	{
		/// <summary>
		/// 玩家昵称长度
		/// </summary>
		public const Int32 MAX_PLAYER_NAME_LEN = 19;
		/// <summary>
		/// 邮件标题长度
		/// </summary>
		public const Int32 MAX_MESSAGE_TITLE_LEN = 50;
		/// <summary>
		/// 邮件内容长度
		/// </summary>
		public const Int32 MAX_MESSAGE_CONTENT_LEN = 500;

		//用户ID的字符最大值
		public const Int32 MAX_PLAYER_ENAME_LEN = 33;
	}

	//消息流向
	public enum EMsgDirection : byte
	{
		MSG_CG = 0x01,
		MSG_GC = 2,
		MSG_GL = 3,
		MSG_LG = 4,
		MSG_GW = 5,
		MSG_WG = 6,
		MSG_GG = 7,
		MSG_LL = 8,
		MSG_WW = 9,
		MSG_MM = 10,
		MSG_GM = 11,
		MSG_MG = 12,
		MSG_MC = 13,
		MSG_CM = 14,

		MSG_MAX = 255,
	};

	//消息类型定义
	public enum EMsgType : byte
	{
		TYPE_SYSTEM = 1,				// 系统消息
		TYPE_LOGIN = 16,				// 登录
		TYPE_GAME,						// 游戏相关,比如获取玩家信息等
		TYPE_CHAT,						// 聊天
		TYPE_IMPAWN,					// 押分
		TYPE_AWARD,                     // 抽奖
		TYPE_PLAYGO,					// 对局
		TYPE_ALLIANCE,                  // 联棋扩展
		TYPE_DB,						// 数据库访问
		TYPE_VALIDATE,					// 认证
		TYPE_MESSAGE,					// 短消息(信件)
		TYPE_KIFU,						// 棋谱
		TYPE_EXPLAIN,					// 讲解
		TYPE_REDIFFUSION,				// 转播
		TYPE_LIVEBROADCAST,				// 直播
		TYPE_SETTING,					// 设置选项
		TYPE_FRIEND,					// 好友，黑名单
		TYPE_CONNECT,
		TYPE_GM,
		TYPE_REG,
		TYPE_VOTE,
		TYPE_PAYMENT,					// 交易,将注释
		TYPE_REPLAY,					// 复盘
		TYPE_VOICE,						// 语音
		TYPE_MAGICFACE,					// 魔法表情
		TYPE_SERVERCOMMAND,				// 服务器命令
		TYPE_LOG,
		TYPE_HELP,						// 请求帮助
		TYPE_RACE,                      // 比赛
		TYPE_PROPS,
		TYPE_BANK,                      // 个人银行
		TYPE_TRADE,                     // 交易,启用时将注释TYPE_PAYMENT
		TYPE_SALE,                      // 商铺
		TYPE_GOLD,                      // 元宝操作
		TYPE_ERROR,                     // 错误提示
		TYPE_SYNC,                      // 线程同步消息
		TYPE_LIVEBROADCAST_LINK,        // 直播跨服链接
		TYPE_HIGHUP_LINK,				// 高段位跨服链接
		TYPE_MONEY,
		TYPE_KICKOUT,					//踢人专用
		TYPE_NEW_COMPETITION,           //全民对战
		TYPE_CLUB = 56,					//CLUB	

		TYPE_MAX = 255,
	};

	public enum ELoginMsgId : ushort
	{
		LOGIN_REQUEST_CG = 1,
		LOGIN_OK_GC = 2,
		LOGIN_FAILED_GC = 3,
		LOGIN_FORCE_CG = 5,

		LOGIN_OK_NOPLAYER_GC = 10,       // 帐号登录成功，无角色信息

		CONNECT = 65534,
		DIS_CONNECT = 65535,	
	}

	public enum EGameMsgId : ushort
	{
		GAME_HALL_GETINITINFO_CG = 0x0001,  // 获取初始化信息(通常在玩家登录成功后发送该消息)
		GAME_HALL_ROOM_INFO_GC,         // 大厅内的房间信息
		GAME_HALL_UPDATE_ROOM_INFO_GC,      // 在大厅更新房间信息
		GAME_HALL_PLAYER_INFO_GC,           // 大厅内的玩家信息
		GAME_HALL_UPDATE_PLAYER_INFO_GC,    // 在大厅更新某玩家信息
		GAME_HALL_LATEST_IMPAWN_ROOM_GC,    // 大厅里最近押分房间
		GAME_PLAYER_JOIN_GC,                // 一个用户登录后,GS通知所有玩家该用户上线
		GAME_ROOM_ENTERROOM_CG,             // 玩家进入房间(C->S)
		GAME_ROOM_ENTERROOM_GC,             // 玩家进入房间(S->C)
		GAME_ROOM_LEAVEROOM_CG,             // 玩家退出房间(C->S)
		GAME_ROOM_LEAVEROOM_GC,             // 玩家退出房间(S->C)
		GAME_HALL_NEW_ROOM_GC,              // 新建房间
		GAME_HALL_DESTROY_ROOM_GC,          // 销毁房间
		GAME_HALL_BEGIN_ROOM_GC,            // 房间内的对局开始(客户端用于更新房间列表)
		GAME_PLAYGOROOM_INFO_GC,            // 用户进入对局房间后，服务器发送该消息给该用户
		GAME_EXPLAINROOM_INFO_GC,           // 用户进入讲解房间后，服务器发送该消息给该用户
		GAME_FIND_PLAYER_CG,                // 查找用户
		GAME_FIND_PLAYER_RESULT_GC,         // 查找用户,服务器返回信息

		GAME_PLAYER_DISCONNECT_GC,          // 玩家(正在对局中)掉线
		GAME_ISPLAYING_WHENDISCONNECT_GC,   // 玩家在掉线后重新登录，服务器通知玩家对局正在进行
		GAME_REPLAYGO_AFTERDISCONNECT_CG,   // 玩家确定继续对局
		GAME_DISCONNECT_TIMEOUT_GC,         // 玩家掉线超时
		GAME_DISCONNECT_COUNTOUT_GC,        // 玩家掉线次数用完
		GAME_RESUME_PLAYGO_GC,              // 掉线后再次上线，服务器通知该player可以继续下棋
		GAME_PLAYGO_OFFLINEINFO_GC,         // 掉线信息(旁观者进入房间，若房间内的对局者掉线，则发送该消息)

		GAME_PLAYER_UPDATE_STATE_GC,        // 玩家状态改变

		GAME_VIEW_PLAYER_INFO_CG,           // 查看玩家信息
		GAME_VIEW_PLAYER_INFO_GC,           // 返回玩家信息

		GAME_UPDATE_PLAYER_WINLOSS_GC,      // 对局结束后发送该消息，更新客户端的玩家胜败信息 
		GAME_PLAYERUPGRADE_GC,              // 玩家升级或降级

		GAME_CREATEROOM_CG,
		GAME_CREATEROOM_GC,

		GAME_UPDATE_GSPLAYERCOUNT_GC,     // 更新GS上的玩家数量
		GAME_AFFICHEINFO_GC,              // 发送公告信息
		GAME_GETALLGSINFO_CG,             // 获取所有gameserver的信息（目前只针对服务器人数）
		GAME_GETALLGSINFO_GC,
		GAME_UPDATEROOMOWNER_GC,          // 更新房主

		GAME_GETCUSTOMHEADPIC_CG,         // 获取用户上传的头像图片
		GAME_GETCUSTOMHEADPIC_GC,

		GAME_UPDATE_PLAYER_MONEY_GC,      // 更新玩家的money
		GAME_ROOM_INVITE_PALYER_CG,       // 邀请玩家进入房间
		GAME_ROOM_INVITE_PALYER_GC,
		GAME_UPDATE_MONEY_DELTA_GC,       // 以增量的形式调整用户金钱,如挂机送钱,奖励送钱等

		GAME_ROOM_PLAYERLIST_GC,          // 房间内玩家列表
		GAME_ROOM_RAISEHANDLIST_GC,       // 房间内举手的玩家列表
		GAME_ROOM_POSVOTEINFO_GC,         // 房间内投票信息
		GAME_ROOM_DRAWLINEINFO_GC,        // 画线信息
		GAME_ROOM_EXPLAINMARKINFO_GC,     // 讲解标记信息

		GAME_ROOM_GETKIFU_CG,             // 调棋谱
		GAME_ROOM_GETKIFU_GC,

		GAME_GETTEMPSESSION_CG,           // 临时创建一个session
		GAME_GETTEMPSESSION_GC,

		GAME_UPLOAD_KIFU_CG,              // 上传棋谱
		GAME_UPLOAD_KIFU_GC,              // 上传棋谱

		GAME_SWAP_BLACKWHITE_GC,          // 交换黑白

		GAME_UPDATE_PLAYERLIVENESS_GC,    // 调整用户活跃度
		GAME_UPDATE_PLAYERLEVELSCORE_GC,  // 等积分调整

		GAME_SET_PLAYER_GOLEVEL_GC,       // 用户段位被调整
		GAME_SET_PLAYER_SHOWNAME_GC,      // 用户炫彩昵称
		GAME_SET_PLAYER_RENDER_GC,        // 用户性别被设置

		GAME_GET_PLAYERLIVENESS_CG,       // 获取用户活跃度 2014-04-24 add by nava
		GAME_GET_PLAYERLIVENESS_GC,       // 获取用户活跃度 2014-04-24 add by nava
		GAME_ROOM_PLAYERCOUNT_GC,         // 房间玩家数量更新(S->C),2013-07-08 add by nava
		GAME_ROOM_PLAYERCACHELIST_GC,     // 通知房间玩家数量变化列表, 2013-07-8 add by nava
		GAME_NEW_COMPETITION_PLAY_GO_GC,    //全民对战对战方进房间

		GAME_HALL_LATEST_IMPAWN_ROOM_LINK_GC,    // 大厅里最近押分房间(跨服) 20140127 gly
		GAME_UPDATE_PLAYER_GOLD_GC,   // 更新玩家的元宝
		GAME_UPDATE_GOLD_DELTA_GC,       // 以增量的形式调整用户元宝,如挂机送钱,奖励送钱等
	}

	enum EPropsMsgId : ushort
	{
		PROPS_GETATTR_CG,           //获取用户道具属性
		PROPS_GETATTR_GC,
		PROPS_GETPROPS_CG,          //获取用户道具
		PROPS_GETPROPS_GC,
		PROPS_GETUSINGPROPS_CG,     //获取使用中的道具
		PROPS_GETUSINGPROPS_GC,
		PROPS_USEPROPS_CG,          //使用道具
		PROPS_USEPROPS_GC,
		PROPS_USEPROPS_ORDER_CG,    //使用订单
		PROPS_USEPROPS_ORDER_GC,
		PROPS_USEPROPS_GOLEVEL_CG,  //使用段位更改道具
		PROPS_USEPROPS_GOLEVEL_GC,
		PROPS_USEPROPS_SHOWNAME_CG, //使用炫彩昵称道具
		PROPS_USEPROPS_SHOWNAME_GC,
		PROPS_USEPROPS_IMPAWN_CLEAR_CG,//使用押分战绩清零道具
		PROPS_USEPROPS_IMPAWN_CLEAR_GC,
		PROPS_USEPROPS_RENDER_CG,   //使用更改性别道具
		PROPS_USEPROPS_RENDER_GC,
		PROPS_UPDATEPROPERTY_GC,    //调整用户道具的数量
		PROPS_USEPROPS_CHANGEUSERNAME_CG,//更改昵称道具消息 20130604
		PROPS_USEPROPS_CHANGEUSERNAME_GC,
		// 梦游岛抽奖
		PROPS_MYD_GETAWARDLIST_CG,
		PROPS_MYD_GETAWARDLIST_GC,
		PROPS_MYD_GETAWARD_CG,
		PROPS_MYD_GETAWARD_GC,

		// 梦游岛上传
		PROPS_MYD_UPLOAD_CG,        //梦游岛上传道具
		PROPS_MYD_UPLOAD_GC,        //

		PROPS_USEBOX_COUNT_CG,    //使用宝箱数量 gly 20140121
		PROPS_USEBOX_COUNT_GC,    //使用宝箱数量
		PROPS_HUODONG_ITEMLIST_GC,    //活动登录获得的道具列表
	};

	public enum EChatMsgId : ushort
	{
		CHAT_CG = 1,
		CHAT_GC = 2,
	}

    public enum EMessageId : ushort
    {
        MESSAGE_SEND_CG = 0,
        MESSAGE_GETALL_GC = 7,
        MESSAGE_GETSENTALL_CG = 12,
        MESSAGE_GETSENTALL_GC = 13,
    }

	public enum ESaleMsgId : ushort
	{
		SALE_NEWSALE_CG,        // 新建商铺
		SALE_NEWSALE_GC,        // -----------------成功后需广播到所有正在商城浏览的用户
		SALE_CANCELSALE_CG,     // 取消商铺
		SALE_CANCELSALE_GC,     // -----------------成功后需广播到所有正在商城浏览的用户
		SALE_BUY_CG,            // 从商铺购买物品
		SALE_BUY_GC,            // -----------------成功后需广播到所有正在商城浏览的用户
		SALE_UPDATESALE_GC,     // 更新商铺信息
		SALE_GETALLSALE_CG,     // 获取所有商铺信息
		SALE_GETALLSALE_GC,     // -----------------成功后需广播到所有正在商城浏览的用户
		SALE_USERSALETOSYSTEM_CG,//用户向系统出售,即系统回收
		SALE_USERSALETOSYSTEM_GC,//出售情况
	}
}
