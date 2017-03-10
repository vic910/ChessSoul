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

	public override void OnLoaded()
	{
		m_btn_return.onClick.AddListener( _onReturnButtonClick );
		m_btn_menu.onClick.AddListener( _onMenuButtonClick );
	}

	public override void OnUnload()
	{
		m_btn_return.onClick.RemoveAllListeners();
		m_btn_menu.onClick.RemoveAllListeners();
	}

	public override float PreShow( UI_Base _pre_ui, params object[] _args )
	{
		return m_entrance_anim_time;
	}

	public override void OnShow( UI_Base _pre_ui, params object[] _args )
	{
		if( UIManager.Instance.WindowCountInStack > 1 )
		{
			m_btn_return.gameObject.SetActive( true );
		}
		else
		{
			m_btn_return.gameObject.SetActive( false );
		}
		UI_Base cur = UIManager.Instance.GetCurrentWindow();
		if( cur == null )
			m_text_title_name.text = string.Empty;
		else
			m_text_title_name.text = cur.name;
	}

	private void _onReturnButtonClick()
	{
		UIManager.Instance.NavigatorBack();
	}

	private void _onMenuButtonClick()
	{

	}
}
