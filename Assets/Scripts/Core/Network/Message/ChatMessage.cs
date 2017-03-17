using System;
using System.Collections.Generic;
using System.Text;
using Groot.Network;
using Utility;
namespace Groot.Network
{
	//发送聊天消息
	class CG_ChatMsg : MessageBase
	{
		[MessageFiled( 0 )]
		public UInt64      PlayerID;

		[MessageFiled( 1 )]
		public Byte        ChatType;        //聊天类型,见EChatType

		[MessageFiled( 2 )]
		public UInt64      TypeData;        //类型相关数据:
											//	私聊或一对一聊  -->对方ID
											//  房间聊          -->房间ID
											//  棋友会聊        -->棋友会ID
											//  其它            -->待定义或未使用
		[MessageFiled( 3 )]
		public ChatStyle   ChatStyle = new ChatStyle();       //字体样式

		//聊天信息
		[MessageFiled( 4 )]
		public Int16       ChatLen;         // strlen(msg.szChat)+1

		[MessageFiled( 5, 100 )]
		public string      Chat;            // 聊天内容,最大长度限制 MAX_CHAT_LEN

		public CG_ChatMsg() : base( EMsgDirection.MSG_CG, EMsgType.TYPE_CHAT, (UInt16)EChatMsgId.CHAT_CG )
		{
		
		}
	};

	//广播聊天消息
	public class GC_ChatMsg : MessageBase
	{
		[MessageFiled( 0 )]
		public UInt64		PlayerID;

		[MessageFiled( 1 )]
		public Byte			ChatType;  //聊天类型,见EChatType

		[MessageFiled( 2 )]
		public UInt64		TypeData; //类型相关数据,同上

		[MessageFiled( 3 )]
		public ChatStyle	ChatStyle;     //字体样式

		//聊天信息
		[MessageFiled( 4 )]
		public Int16		ChatLen;

		[MessageFiled( 5, 100 )]
		public string       Chat;

		public GC_ChatMsg() : base( EMsgDirection.MSG_GC, EMsgType.TYPE_CHAT, (UInt16)EChatMsgId.CHAT_GC )
		{
			
		}
	};

	enum EChatType
	{
		CHAT_HALL,			// 0大厅频道
		CHAT_ROOM,			// 1房间聊天
		CHAT_CLUB,			// 2帮派频道
		CHAT_TRADE,			// 3交易频道
		CHAT_PRIVATE,       // 4密聊频道
		CHAT_ONE2ONE,
		CHAT_CLUB_FORBID,
		CHAT_ONE2ONE_TRADE,
	};

	public class ChatStyle
	{
		public ChatStyle()
		{
			FontSize = 11;
			Effects = 67108864;
			TextColor = 0;
			FontIndex = 0;

		}

		[MessageFiled( 0 )]
		public Int32    FontSize;     //字体大小(字体选择时的数字为单位,如:9,获取yHeight时,yHeight = cFontSize * 20)

		[MessageFiled( 1 )]
		public UInt32	Effects;      //字体特效

		[MessageFiled( 2 )]
		public Int32	TextColor;    //字体颜色

		[MessageFiled( 3 )]
		public Int32    FontIndex;    //字体索引,见GoRule.xml
	};
}