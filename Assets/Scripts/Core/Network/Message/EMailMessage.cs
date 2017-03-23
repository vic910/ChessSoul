/********************************************
 ** Author:LWJ
 ** Create Time:2017/3/22 15:33:21
 ** Description:邮件系统的消息定义
 ** 
 ********************************************/

using System;
using Groot.Network;
using SLua;

/// <summary>
/// 登录时服务推送的邮件列表
/// </summary>
public class msg_MessageGetAll_GC : MessageBase
{
    [MessageFiled(0)]
    public int iCount;
    [MessageFiled(1, 13)]
    public ShortMessageBaseInfo[] messageBaseInfo;

    public msg_MessageGetAll_GC() : base(EMsgDirection.MSG_GC, EMsgType.TYPE_MESSAGE, (ushort)EMessageId.MESSAGE_GETALL_GC)
    {

    }
}

/// <summary>
/// 请求已发送的邮件
/// </summary>
[CustomLuaClass]
public class msg_MessageGetSentAll_CG : MessageBase
{
    [MessageFiled(0)]
    public UInt64 iPlayerID;

    public msg_MessageGetSentAll_CG()
        : base(EMsgDirection.MSG_CG, EMsgType.TYPE_MESSAGE, (ushort)EMessageId.MESSAGE_GETSENTALL_CG)
    {
        iPlayerID = MainPlayer.Instance.PlayerInfo.PlayerID;
    }
}

/// <summary>
/// 返回已发送的邮件
/// </summary>
public class msg_MessageGetSentAll_GC : MessageBase
{
    [MessageFiled(0)]
    public UInt32 iCount;
    [MessageFiled(1, 13)]
    public ShortMessageBaseInfo[] messageBaseInfo;

    public msg_MessageGetSentAll_GC() : base(EMsgDirection.MSG_GC, EMsgType.TYPE_MESSAGE, (ushort)EMessageId.MESSAGE_GETSENTALL_GC)
    {

    }
}

[CustomLuaClass]
public class SendMailMessage : MessageBase
{
    [MessageFiled(0)]
    public UInt64 senderId;
    [MessageFiled(1, GlobalVariant.MAX_PLAYER_NAME_LEN)]
    public string receiverAddress;
    [MessageFiled(2, GlobalVariant.MAX_MESSAGE_TITLE_LEN)]
    public string mailTitle;
    [MessageFiled(3, GlobalVariant.MAX_MESSAGE_CONTENT_LEN)]
    public string mailContent;

    public SendMailMessage() : base(EMsgDirection.MSG_CG, EMsgType.TYPE_MESSAGE, (ushort)EMessageId.MESSAGE_SEND_CG)
    {
        senderId = MainPlayer.Instance.PlayerInfo.PlayerID;
    }
}



