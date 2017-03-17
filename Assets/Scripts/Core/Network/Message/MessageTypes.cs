using System;
using System.Collections.Generic;
using System.Text;

namespace Groot.Network
{
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



		CONNECT = 65534,
		DIS_CONNECT = 65535,	
	}

	public enum EGameMsgId : ushort
	{
		GAME_HALL_ROOM_INFO_GC = 2,
		GAME_HALL_PLAYER_INFO_GC = 4,
		GAME_PLAYER_JOIN_GC = 7,
	}

	public enum EChatMsgId : ushort
	{
		CHAT_CG = 1,
		CHAT_GC = 2,
	}
}
