using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Core.App;
using Groot;
using Groot.Network;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Weiqi;
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
        m_btn_register.onClick.AddListener(_onRegisterButtonClick);
        m_btn_login.onClick.AddListener(_onLoginButtonClick);
        m_btn_get_password.onClick.AddListener(_onGetPasswordClick);
    }

    public override void OnUnload()
    {
        m_btn_register.onClick.RemoveAllListeners();
        m_btn_login.onClick.RemoveAllListeners();
        m_btn_get_password.onClick.RemoveAllListeners();
    }

    public override float PreShow(UI_Base _pre_ui, params object[] _args)
    {
        //显示上次登陆的用户名
        m_edit_account.text = LocalConfigSystem.Instacne.GetOptionConfig("CurrentAccount");
        m_edit_password.text = LocalConfigSystem.Instacne.GetOptionConfig("CurrentPassword");

        SignalSystem.Register(SignalId.Login_ForceLogin, _forceLogin);
        return m_entrance_anim_time;
    }

    public override void OnHide(UI_Base _next_ui)
    {
        SignalSystem.Unregister(SignalId.Login_ForceLogin, _forceLogin);
    }

    private void _onRegisterButtonClick()
    {
        Application.OpenURL(GlobalConfig.Instance.GetValue<string>("RegisterWeb"));
    }

    private void _onGetPasswordClick()
    {
        Application.OpenURL(GlobalConfig.Instance.GetValue<string>("GetPassword"));
    }

    private void _forceLogin(SignalId _signal_id, SignalParameters _parameters)
    {
        CG_ForceLoginRequestMsg msg = new CG_ForceLoginRequestMsg();
        msg.PlatformID = 0;
        msg.PlayerName = m_edit_account.text;
        msg.PlayerPassword = m_edit_password.text;
        var bt = Encoding.UTF8.GetBytes(UnityEngine.SystemInfo.deviceUniqueIdentifier);
        msg.Mac = BitConverter.ToUInt64(bt, 0);
        msg.Md5 = MD5Helper.GetMD5Hash(bt);
        msg.NameType = 0;
        NetManager.Instance.SendMsg(msg);
        WaitForResponse.Retain();
    }

    private void _onLoginButtonClick()
    {
        if (m_edit_account.text == string.Empty)
        {
            UI_MessageBox.Show(Locale.Instance["Login@PleaseInputAccount"]);
            return;
        }
        if (m_edit_password.text == string.Empty)
        {
            UI_MessageBox.Show(Locale.Instance["Login@PleaseInputPassword"]);
            return;
        }
        CG_LoginRequestMsg msg = new CG_LoginRequestMsg();
        msg.PlatformID = 0;
        msg.PlayerName = m_edit_account.text;
        msg.PlayerPassword = m_edit_password.text;
        var bt = Encoding.UTF8.GetBytes(UnityEngine.SystemInfo.deviceUniqueIdentifier);
        msg.Mac = BitConverter.ToUInt64(bt, 0);
        msg.Md5 = MD5Helper.GetMD5Hash(bt);
        msg.NameType = 0;
        msg.Version = 17696793;
        NetManager.Instance.SendMsg(msg);
        LoginSystem.Instance.CurAccount = msg.PlayerName;
        LoginSystem.Instance.CurPassword = msg.PlayerPassword;
        WaitForResponse.Retain();
    }
}
