using System;
using System.Collections.Generic;
using System.Text;
using Groot.Network;
using Utility;
namespace Groot.Network
{
	//房间列表
	class GC_HallRoomInfoMsg : MessageBase
	{
		[MessageFiled( 0 )]
		public Int32 Size;

		[MessageFiled( 1 )]
		public Int16 TotalCount;

		[MessageFiled( 2 )]
		public Int16 Count;

		[MessageFiled( 3, 100 )]
		public List<RoomInfoBase> Rooms;

		public GC_HallRoomInfoMsg() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_GAME, (UInt16)EGameMsgId.GAME_HALL_ROOM_INFO_GC )
		{

		}
	}

	// 房间基本信息
	public class RoomInfoBase
	{
		[MessageFiled( 0 )]
		public Int32   RoomID;                 // 室号

		[MessageFiled( 1 )]
		public Byte    RoomType;               // 房间类型 ERoomType

		[MessageFiled( 2 )]
		public Int32   PlayerCount;            // 房间内当前参与及旁观人数的总和

		[MessageFiled( 3 )]
		public Byte    State;                  // 房间状态

		[MessageFiled( 4 )]
		public UInt64  WhitePlayerID;          // 白方ID

		[MessageFiled( 5 )]
		public UInt64  BlackPlayerID;          // 黑方ID

		[MessageFiled( 6, 19 )]
		public string  WhitePlayer;

		[MessageFiled( 7 )]
		public Byte    WhiteLevel;

		[MessageFiled( 8 )]
		public Int32   WhitePictureID;

		[MessageFiled( 9 )]
		public Byte    WhiteSex;

		[MessageFiled( 10 )]
		public ComboHeadPic   WhiteComboHeadPic;

		[MessageFiled( 11, 19 )]
		public string  BlackPlayer;

		[MessageFiled( 12 )]
		public Byte    BlackLevel;

		[MessageFiled( 13 )]
		public Int32 BlackPictureID;

		[MessageFiled( 14 )]
		public Byte    BlackSex;

		[MessageFiled( 15 )]
		public ComboHeadPic   BlackComboHeadPic;

		[MessageFiled( 16 )]
		public Byte    Encrypt;

		[MessageFiled( 17, 100 )]
		public string  Desc;					// 问候语

		[MessageFiled( 18 )]
		public UInt64  RoomOwnerID;

		[MessageFiled( 19 )]
		public AudioInfo  Audio;

		[MessageFiled( 20 )]
		public UInt64  NeedMoney;             //该房所需碁币

		[MessageFiled( 21 )]
		public Byte    LiveType;              //验证码 20130904 gly
	};


	public class AudioInfo
	{
		[MessageFiled( 0 )]
		public Byte  BitRate;

		[MessageFiled( 1 )]
		public Byte  Codec;
	};

	// 房间状态
	public enum ERoomState : byte
	{
		PLAYGO_PROCESS_FORECAST	= 0,	// 预告
		PLAYGO_PROCESS_FUSEKI   = 1,	// 布局
		PLAYGO_PROCESS_JOBAN	= 2,	// 序盘
		PLAYGO_PROCESS_CHUBAN	= 3,	// 中盘
		PLAYGO_PROCESS_YOSE		= 4,	// 官子
		PLAYGO_PROCESS_SHUBAN	= 5,	// 终盘
		PLAYGO_PROCESS_OVER		= 6,	// 结束
		PLAYGO_PROCESS_PLAYING	= 7,	// 进行
		PLAYGO_PROCESS_REPLAY	= 8,	// 复盘
		PLAYGO_PROCESS_READY	= 9,	// 准备

		PLAYGO_PROCESS_ADJOURNMENT,     // 封盘
	};

	// 房间类型
	public enum ERoomType : byte
	{
		ROOM_TYPE_UNKNOWN = 0,
		ROOM_TYPE_UP = 1,			// 升降级房间
		ROOM_TYPE_FRIENDSHIP,       // 友谊对局房间
		ROOM_TYPE_OWL,				// 死活对局房间
		ROOM_TYPE_LIVEBROADCAST = 4,// 直播房间
		ROOM_TYPE_EXPLAIN = 5,      // 解说房间
		ROOM_TYPE_PLAYGO = 6,		// 普通对局房间
		ROOM_TYPE_REDIFFUSION,      // 转播房间
		ROOM_TYPE_RACE,             // 比赛房间
		ROOM_TYPE_ALLIANCE,         // 联棋房间
		ROOM_TYPE_RENJU,            // 五子棋房间
		ROOM_TYPE_LIVEBROADCAST_LINK,   // 直播链接房间
		ROOM_TYPE_UP_HIGH_LINK,     // 高段位对局链接房间
		ROOM_TYPE_EXPLAIN_LINK,     // 语音解说链接房间
		ROOM_TYPE_NEW_COMPETITION,  //新对战房间

		ROOM_TYPE_SEARCH = 253,		//搜索
		ROOM_TYPE_MINE = 254,       //我的
		ROOM_TYPE_ALL = 255,		//全部
	};
}