using System.Collections;
using System.Collections.Generic;
using Groot;
using Groot.Network;
using UnityEngine;
using UnityEngine.UI;
using Weiqi.UI;

public class UI_Login : UI_Base
{
	[SerializeField]
	private InputField m_edit_account = null;

	[SerializeField]
	private InputField m_edit_password = null;

	[SerializeField]
	private Button m_btn_register = null;

	[SerializeField]
	private Button m_btn_login = null;

	[SerializeField]
	private Button m_btn_get_password = null;

	public override void OnLoaded()
	{
		m_btn_register.onClick.AddListener( _onRegisterButtonClick );
		m_btn_login.onClick.AddListener( _onLoginButtonClick );
		m_btn_get_password.onClick.AddListener( _onGetPasswordClick );
	}

	public override void OnUnload()
	{
		m_btn_register.onClick.RemoveAllListeners();
		m_btn_login.onClick.RemoveAllListeners();
		m_btn_get_password.onClick.RemoveAllListeners();
	}

	public override float PreShow( UI_Base _pre_ui, params object[] _args )
	{
		return m_entrance_anim_time;
	}

	private void _onRegisterButtonClick()
	{
		Application.OpenURL( GlobalConfig.Instance.GetValue<string>( "RegisterWeb" ) );
	}

	private void _onLoginButtonClick()
	{
		UIManager.Instance.ShowUI( "ui_main" );
		UIManager.Instance.HideUI( this );
	}

	private void _onGetPasswordClick()
	{
		Application.OpenURL( GlobalConfig.Instance.GetValue<string>( "GetPassword" ) );
	}
}
