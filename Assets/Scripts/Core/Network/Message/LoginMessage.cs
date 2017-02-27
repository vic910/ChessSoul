using System;
using System.Collections.Generic;
using System.Text;
using Utility;
namespace Groot.Network
{
	class CG_LoginRequestMsg : MessageBase
	{
		[MessageFiled(0)]
		public Byte PlatformID;

		[MessageFiled(1,33)]
		public string PlayerName;

		[MessageFiled(2,18)]
		public string PlayerPassword;

		[MessageFiled(3,33)]
		public string Md5;

		[MessageFiled(4)]
		public Int32 Version;

		[MessageFiled(5)]
		public Byte NameType;

		[MessageFiled(6)]
		public UInt64 Mac;

		public CG_LoginRequestMsg() : base( EMsgDirection.MSG_CG, EMsgType.TYPE_LOGIN, EMsgId.LOGIN_REQUEST_CG )
		{
			
		}
	}

	class GC_LoginFailedMsg : MessageBase
	{
		public enum ReasonInfo
		{
			// 帐号服务器返回的结果 0x0-0x99
			RET_SUCCESS,                // 成功
			RET_INVALID_ACCOUNT,        // 无效的账号
			RET_INVALID_PASSWORD,       // 无效的密码
			RET_NOCARDPOINT,            // 点卡值不足
			RET_LOGGEDIN,               // 已经处于登录状态
			RET_BADINFO,                // 异常信息
			RET_BLOCKED,                // 被封帐号
			RET_LIMITED,                // 被限制的账号
			RET_SUCCESS_WITH_ITEM,      // 成功并且有附加道具存在
			RET_LOGIN_OTHER_SERVER,     // 已在其它服务器登录

			// 登录服务器返回的结果 0x100-0x199
			ERROR_USER_PWD = 0x0100,        // 用户名-密码错误
			ERROR_HAS_LOGINED,				// 用户已登录
			ERROR_HAS_FORBID,				// 帐号被禁用
			ERROR_OTHERSERVER_NOTSTART,     // 帐号服务器未启动
			ERROR_LOGINTIMEOUT,             // 帐号验证超时
			ERROR_LOGINTOOFAST,				//登陆速度太快，暂时不予以登陆
			ERROR_UNKNOWN,

			// 游戏服务器返回的结果 0x200-0x299
			ERROR_VERSION = 0x200,          // 消息版本错误
			ERROR_PLAYERLIMIT,				// 人数已满
			ERROR_LS_NOTSTART,				// 登录服务器未启动
			ERROR_MULTIOPENCHECKED,         // 检测到多开
			ERROR_DATA_OPERATE_WRONG,       // 金钱操作数据异常
			ERROR_CHILDSERVER,				// 开启儿童准入服务器

			// 世界服务器返回的结果
			ERROR_WS_OPERATION = 0x300,     // 玩家操作未完成
			ERROR_ACCOUNT_CONFLICT = 0x400, //棋魂与棋趣合服时出现的账号冲突
		};

		[MessageFiled(0)]
		public Int32 Reason;

		[MessageFiled(1)]
		public UInt64 LeaveTimeWhenForbid;

		public GC_LoginFailedMsg() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_LOGIN, EMsgId.LOGIN_FAILED_GC )
		{
			
		}
	}

	class GC_LoginOK : MessageBase
	{
		[MessageFiled(0)]
		public PlayerInfo  PlayerInfo;

		[MessageFiled(1)]
		public Byte        HasPlayerBankAccount;

		[MessageFiled(2)]
		public Byte        ServerType;          //当前服务器类型.0:普通 1:混战 2：比赛

		[MessageFiled(3)]
		public Byte        IsHadEmail;          //是否有安全邮箱 gly 20140127

		public GC_LoginOK() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_LOGIN, EMsgId.LOGIN_OK_GC )
		{

		}
	}

	// 玩家基本信息
	public class PlayerInfoBase
	{
		[MessageFiled(0)]
		public UInt64  PlayerID;                // 玩家ID

		[MessageFiled(1, 19)]
		public string  PlayerName;             // 玩家名字

		[MessageFiled(2)]
		public Byte    PlayerRank;				// 排名

		[MessageFiled(3)]
		public Byte    Sex;

		[MessageFiled(4)]
		public Byte    Level;                   // 等级

		[MessageFiled(5)]
		public bool    Honor;                   // 棋力

		[MessageFiled(6)]
		public Int32 WinCount;                // 赢棋次数

		[MessageFiled(7)]
		public Int32 LossCount;               // 输棋次数

		[MessageFiled(8)]
		public Int32   LevelScore;				 // 等级分

		[MessageFiled(9)]
		public Byte    CanInvite;               // 是否接收对局邀请

		[MessageFiled(10)]
		public Int32 State;                   // 状态(位置)

		[MessageFiled(11)]
		public Byte    PlatformID;              // 所属平台的平台ID

		[MessageFiled(12)]
		public Int32 ClubID;                  // 棋友会ID

		[MessageFiled(13)]
		public Int16   ClubIconVer;             // 会徽版本号

		[MessageFiled(14)]
		public Int32   ClubPost;                // 棋友会post,见EPlayerPostInClub

		[MessageFiled(15)]
		public Int32   ClubRole;                // 棋友会职位,见EPlayerPostInClub

		[MessageFiled(16)]
		public Int32   ClubRight;               // 棋友会权限,见EPlayerPostInClub

		[MessageFiled(17)]
		public Int32   AreaID;                  // 区域ID
	};


	public class RecentResult
	{
		[MessageFiled(0)]
		public Byte    Result;

		[MessageFiled(1,19)]
		public Byte    OtherPlayerName;
	};

	// 部分对局胜负结果(x胜y负z和棋)
	public class SWinResult
	{
		[MessageFiled(0)]
		public Int32 iWinCount;

		[MessageFiled(1)]
		public Int32 iLossCount;

		[MessageFiled(2)]
		public Int32 iJigoCount;
	};

	public class SysTimeData
	{
		[MessageFiled(0)]
		public UInt16 Year;

		[MessageFiled(1)]
		public UInt16 Month;

		[MessageFiled(2)]
		public UInt16 DayOfWeek;

		[MessageFiled(3)]
		public UInt16 Day;

		[MessageFiled(4)]
		public UInt16 Hour;

		[MessageFiled(5)]
		public UInt16 Minute;

		[MessageFiled(6)]
		public UInt16 Second;

		[MessageFiled(7)]
		public UInt16 Milliseconds;
	}

	public class OnlineTimeInfo
	{
		[MessageFiled(0)]
		public Int32     Total; //总在线时长

		[MessageFiled(1)]
		public Int32     Today; //当日在线时长

		[MessageFiled(2)]
		public Int32     Now;   //当次在线时长(未在线时表示最后一次在线时长)
	};

	// 系统定义的组合头像结构
	public class ComboHeadPic
	{
		[MessageFiled(0)]
		public Byte Eye;          // 眼

		[MessageFiled(1)]
		public Byte Head;         // 头

		[MessageFiled(2)]
		public Byte Nose;         // 鼻子

		[MessageFiled(3)]
		public Byte Hair;         // 头发

		[MessageFiled(4)]
		public Byte Mouth;        // 嘴

		[MessageFiled(5)]
		public Byte Cloth;        // 衣服

		[MessageFiled(6)]
		public Byte Goatee;       // 胡子

		[MessageFiled(7)]
		public Byte Face;         // 特征
	};

	public class TotalImpawnInfo
	{
		[MessageFiled(0)]
		public Int32     WinCount;

		[MessageFiled(1)]
		public Int32     LossCount;
	};

	public class PlayerInfo : PlayerInfoBase
	{
		[MessageFiled(0, 33)]
		public string          EnglishName;

		[MessageFiled(1)]
		public UInt64          Money;

		[MessageFiled(2)]
		public UInt32          Gold;

		[MessageFiled(3,20)]
		public List<RecentResult>    RecentResult;

		[MessageFiled(4)]
		public SWinResult      NowLevelAchievement;

		[MessageFiled(5)]
		public SWinResult      TotalAchievement;

		[MessageFiled(6)]
		public SWinResult      UpgradeAchievement;

		[MessageFiled(7)]
		public SWinResult      FriendshipAchievement;

		[MessageFiled(8)]
		public SysTimeData     LastLoginTime;

		[MessageFiled(9)]
		public SysTimeData     LastLogoutTime;                 //最后登出时间

		[MessageFiled(10)]
		public SysTimeData     RegisterTime;

		[MessageFiled(11)]
		public OnlineTimeInfo  OnlineTimeInfo;                 //在线时长信息

		[MessageFiled(12)]
		public Int32         PictureID;

		[MessageFiled(13)]
		public ComboHeadPic    ComboPic;

		[MessageFiled(14,20)]
		public string          IP;

		[MessageFiled(15,20)]
		public string          ClubName;						//棋友会名称

		[MessageFiled(16,100)]
		public string          ClubPositionName;               //职务名

		[MessageFiled(17)]
		public UInt64          Liveness;                      //用户活跃度

		[MessageFiled(18)]
		public TotalImpawnInfo TotalImpawnInfo;           //押分统计信息

		[MessageFiled(19)]
		public Int64           ImpawnStatOneDay;          // 每日押分收益

		[MessageFiled(20)]
		public Int64           NewCompetitionPoints;

		[MessageFiled(21)]
		public UInt32          ChildMark;					//儿童标记

		[MessageFiled(22)]
		public UInt32          HuoDongMark;               //活动标记(每日登陆领奖类型标记)
	}
}