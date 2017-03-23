/********************************************
 ** Author:LWJ
 ** Create Time:2017/3/23 11:24:25
 ** Description:邮件系统数据层相关东西
 ** 
 ********************************************/

using System;
using System.Collections.Generic;
using Groot.Network;
using SLua;
using Weiqi;

[CustomLuaClass]
public class EMailSystem
{

    public static readonly EMailSystem Instance = new EMailSystem();

    /// <summary>
    /// 已接收到的邮件列表
    /// </summary>
    public List<ShortMessageBaseInfo> receivedMails = new List<ShortMessageBaseInfo>();
    /// <summary>
    /// 已发送邮件列表
    /// </summary>
    public List<ShortMessageBaseInfo> sentMails = new List<ShortMessageBaseInfo>();

    [DoNotToLua]
    public void Initialize()
    {
        NetManager.Instance.Register<msg_MessageGetAll_GC>(messageGetAllGC);
        NetManager.Instance.Register<msg_MessageGetSentAll_GC>(messageGetSentAll);
    }

    [DoNotToLua]
    public void Uninitialize()
    {
        NetManager.Instance.Unregister<msg_MessageGetAll_GC>();
        NetManager.Instance.Unregister<msg_MessageGetSentAll_GC>();
    }


    //---------------------------------接收消息处理-----------------------------------------------
    #region 接收消息处理

    /// <summary>
    /// 登录时接收到服务器推送的收件箱列表
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="msg"></param>
    [DoNotToLua]
    private void messageGetAllGC(Int32 id, PacketType type, msg_MessageGetAll_GC msg)
    {
        receivedMails.AddRange(msg.messageBaseInfo);
    }

    /// <summary>
    /// 服务器返回的所有已发送邮件
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="msg"></param>
    [DoNotToLua]
    private void messageGetSentAll(Int32 id, PacketType type, msg_MessageGetSentAll_GC msg)
    {
        sentMails.AddRange(msg.messageBaseInfo);
    }

    #endregion

    //-------------------------------------发送消息处理------------------------------------------

    #region 发送消息处理

    private bool haveGetSentAll;
    /// <summary>
    /// 首次获取所有已发送的邮件(允许请求一次)
    /// </summary>
    public void SendMessageGetSentAll()
    {
        //if (!haveGetSentAll)
        //{
            NetManager.Instance.SendMsg(new msg_MessageGetSentAll_CG());
            //haveGetSentAll = true;
        //}
    }

    #endregion
}


/// <summary>
/// 已发送邮件数据结构
/// </summary>
public class ShortMessageBaseInfo
{
    [MessageFiled(0)]
    public UInt64 iMessageID;
    [MessageFiled(1)]
    public SysTimeData stSendTime;
    [MessageFiled(2)]
    public byte cRead;// 是否已经读取 0:未读, >0 已读(数字表示读的次数)
    [MessageFiled(3, GlobalVariant.MAX_PLAYER_NAME_LEN)]
    public string szSender;
    [MessageFiled(4, GlobalVariant.MAX_MESSAGE_TITLE_LEN)]
    public string szTitle;
}
