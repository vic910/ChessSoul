using System.Collections;
using System.Collections.Generic;
using Groot;
using Groot.Network;
using UnityEngine;
using UnityEngine.UI;
using Weiqi.UI;

public class UI_Main : UI_Base
{
	[SerializeField]
	private Button m_btn_chess = null;

	[SerializeField]
	private Button m_btn_online = null;

	[SerializeField]
	private Button m_btn_chat = null;

	[SerializeField]
	private Button m_btn_self = null;

	public override void OnLoaded()
	{
		m_btn_chess.onClick.AddListener( _onChessBtnClick );
		m_btn_online.onClick.AddListener( _onOnlineBtnClick );
		m_btn_chat.onClick.AddListener( _onChatBtnClick );
		m_btn_self.onClick.AddListener( _onSelfBtnClick );
	}

	public override void OnUnload()
	{
		m_btn_chess.onClick.RemoveAllListeners();
		m_btn_online.onClick.RemoveAllListeners();
		m_btn_chat.onClick.RemoveAllListeners();
		m_btn_self.onClick.RemoveAllListeners();
    }

	public override float PreShow( UI_Base _pre_ui, params object[] _args )
	{
		return m_entrance_anim_time;
	}

	private void _onChessBtnClick()
	{
		UIManager.Instance.ShowUI( "ui_chess_lobby" );
	}

	private void _onOnlineBtnClick()
	{
        UIManager.Instance.ShowUI("ui_player_online");
    }

	private void _onChatBtnClick()
	{
		UIManager.Instance.ShowUI( "ui_chat" );
	}

	private void _onSelfBtnClick()
	{
		UIManager.Instance.ShowUI( "ui_self" );
	}
}
