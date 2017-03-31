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
    public List<ShortMessageBaseInfo> m_received_mails = new List<ShortMessageBaseInfo>();
    /// <summary>
    /// 已发送邮件列表
    /// </summary>
    public List<ShortMessageBaseInfo> m_send_mails = new List<ShortMessageBaseInfo>();

	private bool m_is_get_send;

	[DoNotToLua]
    public void Initialize()
    {
		m_is_get_send = false;
		m_received_mails.Clear();
		m_send_mails.Clear();
		NetManager.Instance.Register<msg_MessageGetAll_GC>( _onPacketArrived );
        NetManager.Instance.Register<msg_MessageGetSentAll_GC>( _onPacketArrived );
    }

    [DoNotToLua]
    public void Uninitialize()
    {
		NetManager.Instance.Unregister<msg_MessageGetAll_GC>();
        NetManager.Instance.Unregister<msg_MessageGetSentAll_GC>();
    }

	public Int32 GetEmailCount( Int32 _type )
	{
		if( _type == 0 )
			return m_received_mails.Count;
		else
			return m_send_mails.Count;
	}

	public ShortMessageBaseInfo GetEmailInfo( Int32 _type, Int32 _index )
	{
		if( _type == 0 )
			return m_received_mails[_index];
		else
			return m_send_mails[_index];
	}

	//---------------------------------接收消息处理-----------------------------------------------
	#region 接收消息处理

	/// <summary>
	/// 登录时接收到服务器推送的收件箱列表
	/// </summary>
	/// <param name="id"></param>
	/// <param name="type"></param>
	/// <param name="msg"></param>
	private void _onPacketArrived( Int32 _id, PacketType _type, msg_MessageGetAll_GC _msg )
    {
		for( Int32 i = 0; i < _msg.iCount; ++i )
			m_received_mails.Add( _msg.messageBaseInfo[i] );
    }

    /// <summary>
    /// 服务器返回的所有已发送邮件
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="msg"></param>
    private void _onPacketArrived( Int32 _id, PacketType _type, msg_MessageGetSentAll_GC _msg )
    {
		for( Int32 i = 0; i < _msg.iCount; ++i )
			m_send_mails.Add( _msg.messageBaseInfo[i] );
    }

    #endregion

    //-------------------------------------发送消息处理------------------------------------------

    #region 发送消息处理
    /// <summary>
    /// 首次获取所有已发送的邮件(允许请求一次)
    /// </summary>
    public void SendMessageGetSentAll()
    {
        if ( !m_is_get_send )
        {
			msg_MessageGetSentAll_CG msg = new msg_MessageGetSentAll_CG();
			msg.iPlayerID = MainPlayer.Instance.PlayerInfo.PlayerID;
			NetManager.Instance.SendMsg( msg );
			m_is_get_send = true;
        }
    }

    #endregion
}

