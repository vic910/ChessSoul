using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Weiqi.UI
{
	public class UI_MessageBox : UI_Base
	{
		/// <summary>
		/// 1个按钮的
		/// </summary>
		/// <param name="_tips"></param>
		/// <param name="_buttonText"></param>
		/// <param name="_callback"></param>
		public static void Show( String _tips, String _button_text, Action _callback = null, Action _close_callback = null )
		{
			UI_MessageBox messagebox = UIManager.Instance.ShowUI( "ui_msgbox" ) as UI_MessageBox;
			messagebox.OpenMsgBox( _tips, _button_text, _callback, _close_callback );
		}

		public static void Show( String _tips, String _button_text1, String _button_text2, Action _callback1 = null, Action _callback2 = null, Action _close_callback = null )
		{
			UI_MessageBox messagebox = UIManager.Instance.ShowUI( "ui_msgbox" ) as UI_MessageBox;
			messagebox.OpenMsgBox( _tips, _button_text1, _button_text2, _callback1, _callback2, _close_callback );
		}


		public override void OnLoaded()
		{
			m_button_close.onClick.AddListener( _onCloseButtonClicked );
			m_button_single.onClick.AddListener( _onSingleButtonClicked );
			m_button_double_1.onClick.AddListener( _onDoubleButton1Clicked );
			m_button_double_2.onClick.AddListener( _onDoubleButton2Clicked );
		}

		public override void OnUnload()
		{
			m_button_close.onClick.RemoveAllListeners();
			m_button_single.onClick.RemoveAllListeners();
			m_button_double_1.onClick.RemoveAllListeners();
			m_button_double_2.onClick.RemoveAllListeners();
		}

		/// <summary>
		/// 关闭按钮按下
		/// </summary>
		private void _onCloseButtonClicked()
		{
			UIManager.Instance.HideUI( this );
			if( m_close_callback != null )
				m_close_callback();
		}

		/// <summary>
		/// 单按钮按下
		/// </summary>
		private void _onSingleButtonClicked()
		{
			UIManager.Instance.HideUI( this );
			if( m_button_single_callback != null )
				m_button_single_callback();
		}

		/// <summary>
		/// 双按钮 第一个按键按下
		/// </summary>
		private void _onDoubleButton1Clicked()
		{
			UIManager.Instance.HideUI( this );
			if( m_button_double_1_callback != null )
				m_button_double_1_callback();
		}

		/// <summary>
		/// 双按钮 第二个按键按下
		/// </summary>
		private void _onDoubleButton2Clicked()
		{
			UIManager.Instance.HideUI( this );
			if( m_button_double_2_callback != null )
				m_button_double_2_callback();
		}

		/// <summary>
		/// 打开消息框
		/// </summary>
		public void OpenMsgBox( String _tips, String _button_text, Action _callback, Action _close_callback )
		{
			m_text_tips.text = _tips;
			m_button_double_1.gameObject.SetActive( false );
			m_button_double_2.gameObject.SetActive( false );
			m_button_single.gameObject.SetActive( true );
			m_button_text_single.text = String.IsNullOrEmpty( _button_text ) ? String.Empty : _button_text;
			m_button_single_callback = _callback;

			m_close_callback = _close_callback;
			if( m_close_callback == null )
			{
				m_button_close.gameObject.SetActive( false );
			}
			else
			{
				m_button_close.gameObject.SetActive( true );
			}
		}

		public void OpenMsgBox( String _tips, String _button_text_1, String _button_text_2, Action _callback_1, Action _callback_2, Action _close_callback )
		{
			m_text_tips.text = _tips;
			m_button_single.gameObject.SetActive( false );
			m_button_double_1.gameObject.SetActive( true );
			m_button_double_2.gameObject.SetActive( true );
			m_button_text_double_1.text = String.IsNullOrEmpty( _button_text_1 ) ? String.Empty : _button_text_1;
			m_button_text_double_2.text = String.IsNullOrEmpty( _button_text_2 ) ? String.Empty : _button_text_2;

			m_button_double_1_callback = _callback_1;
			m_button_double_2_callback = _callback_2;

			m_close_callback = _close_callback;
			if( m_close_callback == null )
			{
				m_button_close.gameObject.SetActive( false );
			}
			else
			{
				m_button_close.gameObject.SetActive( true );
			}
		}

		/// <summary>
		/// 单按钮需求
		/// </summary>
		[SerializeField]
		private Button m_button_single = null;
		[SerializeField]
		private Text m_button_text_single = null;
		private Action m_button_single_callback = null;

		/// <summary>
		/// 双按钮
		/// </summary>
		[SerializeField]
		private Button m_button_double_1 = null;
		[SerializeField]
		private Button m_button_double_2 = null;
		[SerializeField]
		private Text m_button_text_double_1 = null;
		[SerializeField]
		private Text m_button_text_double_2 = null;
		private Action m_button_double_1_callback = null;
		private Action m_button_double_2_callback = null;

		/// <summary>
		/// 关闭按钮
		/// </summary>
		[SerializeField]
		private Button m_button_close = null;
		private Action m_close_callback = null;

		/// <summary>
		/// 提示内容
		/// </summary>
		[SerializeField]
		private Text m_text_tips = null;
	}
}