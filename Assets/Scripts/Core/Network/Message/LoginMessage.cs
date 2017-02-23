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
		PlayerInfo  PlayerInfo;

		[MessageFiled(1)]
		Byte        HasPlayerBankAccount;

		[MessageFiled(2)]
		Byte        ServerType;          //当前服务器类型.0:普通 1:混战 2：比赛

		[MessageFiled(3)]
		Byte        IsHadEmail;          //是否有安全邮箱 gly 20140127

		public GC_LoginOK() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_LOGIN, EMsgId.LOGIN_OK_GC )
		{

		}
	}

	// 玩家基本信息
	public class PlayerInfoBase
	{
		[MessageFiled(0)]
		UInt64  PlayerID;                // 玩家ID

		[MessageFiled(1, 19)]
		string  PlayerName;             // 玩家名字

		[MessageFiled(2)]
		Byte    PlayerRank;				// 排名

		[MessageFiled(3)]
		Byte    Sex;

		[MessageFiled(4)]
		Byte    Level;                   // 棋力

		[MessageFiled(5)]
		bool    Honor;                   // 棋力

		[MessageFiled(6)]
		Int32	WinCount;                // 赢棋次数

		[MessageFiled(7)]
		Int32	LossCount;               // 输棋次数

		[MessageFiled(8)]
		Int32   LevelScore;				 // 等级分

		[MessageFiled(9)]
		Byte    CanInvite;               // 是否接收对局邀请

		[MessageFiled(10)]
		Int32	State;                   // 状态(位置)

		[MessageFiled(11)]
		Byte    PlatformID;              // 所属平台的平台ID

		[MessageFiled(12)]
		Int32	ClubID;                  // 棋友会ID

		[MessageFiled(13)]
		Int16   ClubIconVer;             // 会徽版本号

		[MessageFiled(14)]
		Int32   ClubPost;                // 棋友会post,见EPlayerPostInClub

		[MessageFiled(15)]
		Int32   ClubRole;                // 棋友会职位,见EPlayerPostInClub

		[MessageFiled(16)]
		Int32   ClubRight;               // 棋友会权限,见EPlayerPostInClub

		[MessageFiled(17)]
		Int32   AreaID;                  // 区域ID
	};


	class RecentResult
	{
		[MessageFiled(0)]
		char    Result;

		[MessageFiled(1,19)]
		char    OtherPlayerName;
	};

	// 部分对局胜负结果(x胜y负z和棋)
	class SWinResult
	{
		[MessageFiled(0)]
		Int32 iWinCount;

		[MessageFiled(1)]
		Int32 iLossCount;

		[MessageFiled(2)]
		Int32 iJigoCount;
	};

	class SysTimeData
	{
		[MessageFiled(0)]
		UInt16 Year;

		[MessageFiled(1)]
		UInt16 Month;

		[MessageFiled(2)]
		UInt16 DayOfWeek;

		[MessageFiled(3)]
		UInt16 Day;

		[MessageFiled(4)]
		UInt16 Hour;

		[MessageFiled(5)]
		UInt16 Minute;

		[MessageFiled(6)]
		UInt16 Second;

		[MessageFiled(7)]
		UInt16 Milliseconds;
	}

	class OnlineTimeInfo
	{
		[MessageFiled(0)]
		Int32     Total; //总在线时长

		[MessageFiled(1)]
		Int32     Today; //当日在线时长

		[MessageFiled(2)]
		Int32     Now;   //当次在线时长(未在线时表示最后一次在线时长)
	};

	// 系统定义的组合头像结构
	class ComboHeadPic
	{
		[MessageFiled(0)]
		Byte Eye;          // 眼

		[MessageFiled(1)]
		Byte Head;         // 头

		[MessageFiled(2)]
		Byte Nose;         // 鼻子

		[MessageFiled(3)]
		Byte Hair;         // 头发

		[MessageFiled(4)]
		Byte Mouth;        // 嘴

		[MessageFiled(5)]
		Byte Cloth;        // 衣服

		[MessageFiled(6)]
		Byte Goatee;       // 胡子

		[MessageFiled(7)]
		Byte Face;         // 特征
	};

	class TotalImpawnInfo
	{
		[MessageFiled(0)]
		Int32     WinCount;

		[MessageFiled(1)]
		Int32     LossCount;
	};

	public class PlayerInfo : PlayerInfoBase
	{
		[MessageFiled(0, 33)]
		string          EnglishName;

		[MessageFiled(1)]
		UInt64          Money;

		[MessageFiled(2)]
		UInt32          Gold;

		[MessageFiled(3,20)]
		List<RecentResult>    RecentResult;

		[MessageFiled(4)]
		SWinResult      NowLevelAchievement;

		[MessageFiled(5)]
		SWinResult      TotalAchievement;

		[MessageFiled(6)]
		SWinResult      UpgradeAchievement;

		[MessageFiled(7)]
		SWinResult      FriendshipAchievement;

		[MessageFiled(8)]
		SysTimeData     LastLoginTime;

		[MessageFiled(9)]
		SysTimeData     LastLogoutTime;                 //最后登出时间

		[MessageFiled(10)]
		SysTimeData     RegisterTime;

		[MessageFiled(11)]
		OnlineTimeInfo  OnlineTimeInfo;                 //在线时长信息

		[MessageFiled(12)]
		Int32			PictureID;

		[MessageFiled(13)]
		ComboHeadPic    ComboPic;

		[MessageFiled(14,20)]
		string          IP;

		[MessageFiled(15,20)]
		string          ClubName;						//棋友会名称

		[MessageFiled(16,100)]
		string          ClubPositionName;               //职务名

		[MessageFiled(17)]
		UInt64          Liveness;                      //用户活跃度

		[MessageFiled(18)]
		TotalImpawnInfo TotalImpawnInfo;           //押分统计信息

		[MessageFiled(19)]
		Int64           ImpawnStatOneDay;          // 每日押分收益

		[MessageFiled(20)]
		Int64           NewCompetitionPoints;

		[MessageFiled(21)]
		UInt32          ChildMark;					//儿童标记

		[MessageFiled(22)]
		UInt32          HuoDongMark;               //活动标记(每日登陆领奖类型标记)
	}
}