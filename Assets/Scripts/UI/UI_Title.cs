using System.Collections;
using System.Collections.Generic;
using Groot.Network;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Weiqi.UI;

public class UI_Title : UI_Base
{
    [SerializeField]
    private Button m_btn_return = null;

    [SerializeField]
    private Button m_btn_menu = null;

    [SerializeField]
    private Text m_text_title_name = null;

    [SerializeField]
    private List<string> closeMenuList;

    public override void OnLoaded()
    {
        m_btn_return.onClick.AddListener(_onReturnButtonClick);
        m_btn_menu.onClick.AddListener(_onMenuButtonClick);
        MulitButton mulitBtn = m_btn_menu.GetComponent<MulitButton>();
        mulitBtn.SetOnClickFunction(0, _onEmailButtonClick);
        mulitBtn.SetOnClickFunction(1, _onOptionButtonClick);
        mulitBtn.SetOnClickFunction(2, _onShopButtonClick);
    }

    public override void OnUnload()
    {
        m_btn_return.onClick.RemoveAllListeners();
        m_btn_menu.onClick.RemoveAllListeners();
    }

    public override float PreShow(UI_Base _pre_ui, params object[] _args)
    {
        return m_entrance_anim_time;
    }

    public override void OnShow(UI_Base _pre_ui, params object[] _args)
    {
        if (UIManager.Instance.WindowCountInStack > 1)
        {
            m_btn_return.gameObject.SetActive(true);
        }
        else
        {
            m_btn_return.gameObject.SetActive(false);
        }
        UI_Base cur = UIManager.Instance.GetCurrentWindow();
	    if( cur == null )
	    {
			m_btn_menu.gameObject.SetActive( false );
			m_text_title_name.text = string.Empty;
		}
        else
        {
            m_text_title_name.text = cur.name;
            if ( closeMenuList.Contains( cur.name ) )
            {
                m_btn_menu.gameObject.SetActive( false );
            }
            else
            {
				m_btn_menu.gameObject.SetActive( true );
			}
        }

    }

    private void _onReturnButtonClick()
    {
        UIManager.Instance.NavigatorBack();
    }

    private void _onMenuButtonClick()
    {

    }

    private void _onEmailButtonClick(int value)
    {
        //UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Common@NotOpen"), "", null, null);
        //UIManager.Instance.ShowUI("ui_emailSystem");
        UIManager.Instance.ShowUI( "ui_email_list" );
    }

    private void _onOptionButtonClick(int value)
    {
        UIManager.Instance.ShowUI( "ui_option" );
    }

    private void _onShopButtonClick(int value)
    {
        UIManager.Instance.ShowUI( "ui_shop" );
    }
}
