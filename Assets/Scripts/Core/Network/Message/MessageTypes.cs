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
		GAME_HALL_ROOM_INFO_GC = 2,
		GAME_HALL_PLAYER_INFO_GC = 4,
		GAME_PLAYER_JOIN_GC = 7,
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
}
