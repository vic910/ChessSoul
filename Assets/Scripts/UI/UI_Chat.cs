using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Groot;
using Groot.Network;
using UnityEngine;
using UnityEngine.UI;
using Weiqi.UI;

public class UI_Chat : UI_Base
{
	[SerializeField]
	private Toggle m_toggle_lobby_channel;

	[SerializeField]
	private Toggle m_toggle_busy_channel;

	[SerializeField]
	private Toggle m_toggle_union_channel;

	[SerializeField]
	private ScrollRect m_scroll_list;

	[SerializeField]
	private GameObject m_container;

	[SerializeField]
	private Button m_btn_clear;

	[SerializeField]
	private InputField m_edit_input;

	[SerializeField]
	private Button m_btn_send;

	[SerializeField]
	private Dropdown m_drop_tips_select;

	[SerializeField]
	private GameObject m_other_template;

	[SerializeField]
	private GameObject m_mine_template;

	[SerializeField]
	private GameObject m_notice_template;

	private Stack<GameObject> m_free_other = new Stack<GameObject>();

	private Stack<GameObject> m_free_mine = new Stack<GameObject>();

	private Stack<GameObject> m_free_notice = new Stack<GameObject>();

	enum eChatTarget
	{
		eChatTarget_other,		//他人
		eChatTarget_mine,		//我的
		eChatTarget_notice,		//通知
	}

	private EChatType m_cur_chat_type = EChatType.CHAT_HALL;

	public override void OnLoaded()
	{
		m_toggle_lobby_channel.onValueChanged.AddListener( delegate( bool _value )
		{
			_onToggleChannelChange( _value, EChatType.CHAT_HALL );
		} );
		m_toggle_busy_channel.onValueChanged.AddListener( delegate ( bool _value ) {
			_onToggleChannelChange( _value, EChatType.CHAT_TRADE );
		} );
		m_toggle_union_channel.onValueChanged.AddListener( delegate ( bool _value ) {
			_onToggleChannelChange( _value, EChatType.CHAT_CLUB );
		} );
		m_btn_send.onClick.AddListener( _onSendButtonClick );
		m_btn_clear.onClick.AddListener( _onClearButtonClick );
		m_drop_tips_select.onValueChanged.AddListener( _onDropDownChange );
	}

	public override void OnUnload()
	{
		m_toggle_lobby_channel.onValueChanged.RemoveAllListeners();
		m_toggle_busy_channel.onValueChanged.RemoveAllListeners();
		m_toggle_union_channel.onValueChanged.RemoveAllListeners();
		m_btn_send.onClick.RemoveAllListeners();
		m_btn_clear.onClick.RemoveAllListeners();
		m_drop_tips_select.onValueChanged.RemoveAllListeners();
	}

	public override float PreShow( UI_Base _pre_ui, params object[] _args )
	{
		SignalSystem.Register( SignalId.Chat_ReceiveChat, _receivePlayerChat );
		_updateChat();
		return m_entrance_anim_time;
	}

	public override void OnHide( UI_Base _next_ui )
	{
		SignalSystem.Unregister( SignalId.Chat_ReceiveChat, _receivePlayerChat );
	}

	private void _onToggleChannelChange( bool _selected, EChatType _type )
	{
		if( !_selected )
			return;
		m_cur_chat_type = _type;
		_updateChat();
	}

	private void _onDropDownChange( Int32 _index )
	{
		_sendChatMsg( m_cur_chat_type, m_drop_tips_select.options[_index].text );
	}

	private void _updateChat()
	{
		_onClearButtonClick();
		LinkedList<GC_ChatMsg> chats = ChatSystem.Instance.GetChats( m_cur_chat_type );
		if( chats == null )
			return;
		foreach( var data in chats )
		{
			_addChat( data );
		}
	}

	private void _addChat( GC_ChatMsg _data )
	{
		eChatTarget type = eChatTarget.eChatTarget_other;
		PlayerInfoBase player = null;
		if( _data.PlayerID == MainPlayer.Instance.PlayerInfo.PlayerID )
		{
			type = eChatTarget.eChatTarget_mine;
			player = MainPlayer.Instance.PlayerInfo;
		}
		else
		{
			type = eChatTarget.eChatTarget_other;
			player = LobbySystem.Instance.GetPlayerInfo( _data.PlayerID );
		}
		_addChat( type, player.PlayerName, player.Level, _data.Chat );
	}

	private void _receivePlayerChat( SignalId _signal_id, SignalParameters _parameters )
	{
		GC_ChatMsg data = _parameters[0] as GC_ChatMsg;
		_addChat( data );
	}

	private void _onClearButtonClick()
	{
		for( Int32 i = 0; i < m_container.transform.childCount; )
		{
			GameObject obj = m_container.transform.GetChild( i ).gameObject;
			_addFreeObj( obj );
			i = 0;
		}
	}

	private void _onSendButtonClick()
	{
		if( m_edit_input.text == string.Empty )
			return;
		_sendChatMsg( m_cur_chat_type, m_edit_input.text );
	}

	private void _sendChatMsg( EChatType _type, string _text )
	{
		CG_ChatMsg msg = new CG_ChatMsg();
		msg.PlayerID = MainPlayer.Instance.PlayerInfo.PlayerID;
		msg.ChatType = (Byte)_type;
		var bytes = Encoding.GetEncoding( "GB2312" ).GetBytes( _text );
		msg.ChatLen = (Int16)( bytes.Length + 1 );
		msg.Chat = _text;
		NetManager.Instance.SendMsg( msg );
	}

	private void _addFreeObj( GameObject _obj )
	{
		_obj.SetActive( false );
		_obj.transform.SetParent( transform, false );
		if( _obj.name.Substring( 0, 1 ) == "O" )
			m_free_other.Push( _obj );
		else if( _obj.name.Substring( 0, 1 ) == "M" )
			m_free_mine.Push( _obj );
		else
			m_free_notice.Push( _obj );
	}

	private GameObject _getFreeObj( eChatTarget _type )
	{
		GameObject obj = null;
		switch( _type )
		{
		case eChatTarget.eChatTarget_other:
			if( m_free_other.Count == 0 )
				obj = Instantiate( m_other_template );
			else
				obj = m_free_other.Pop();
			break;
		case eChatTarget.eChatTarget_mine:
			if( m_free_mine.Count == 0 )
				obj = Instantiate( m_mine_template );
			else
				obj = m_free_mine.Pop();
			break;
		case eChatTarget.eChatTarget_notice:
			if( m_free_notice.Count == 0 )
				obj = Instantiate( m_notice_template );
			else
				obj = m_free_notice.Pop();
			break;
		}
		return obj;
	}

	private void _addChat( eChatTarget _type, string _name, Int32 _level, string _content )
	{
		GameObject obj = null;
		obj = _getFreeObj( _type );
		switch( _type )
		{
		case eChatTarget.eChatTarget_other:
		case eChatTarget.eChatTarget_mine:
			obj.transform.FindChild( "text_name" ).GetComponent<Text>().text = _name;
			obj.transform.FindChild( "Image/text_chat" ).GetComponent<Text>().text = _content;
			break;
		case eChatTarget.eChatTarget_notice:
			obj.transform.FindChild( "Image/text_chat" ).GetComponent<Text>().text = _content;
			break;
		}
		obj.SetActive( true );
		obj.transform.SetParent( m_container.transform, false );
		if( m_container.transform.childCount > 30 )
		{
			_addFreeObj( m_container.transform.GetChild( 0 ).gameObject );
		}
	}
}
