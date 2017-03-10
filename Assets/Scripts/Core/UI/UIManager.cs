using System;
using System.Collections;
using System.Collections.Generic;
using Groot.Res;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Utility;

namespace Weiqi.UI
{
	/// <summary>
	///     界面的层级
	/// </summary>
	public enum UILayer
	{
		/// <summary>
		///     背景层
		/// </summary>
		Background,

		/// <summary>
		///     默认层
		/// </summary>
		Default,

		/// <summary>
		///     弹出窗口层
		/// </summary>
		Popup,

		/// <summary>
		///     最上层
		/// </summary>
		TopMost,

		/// <summary>
		///     调试用(这个是物理上的最上层，但在发布版中没有这一层次)
		/// </summary>
		Debug,

		/// <summary>
		///		隐藏层（这个层将不会直接被渲染到相机上 一般用来做后台UI拼图分享）
		/// </summary>
		Hide,

		Max,
	}

	/// <summary>
	/// 手机UI的情况，Windows通常为全屏覆盖，dialog通常为前景弹出，并且同时只会存在一个
	/// </summary>
	public enum UIType
	{
		Window,
		Dialog,
		Other,
	}

	/// <summary>
	///     UI管理相关逻辑，在gameapp场景的GameApp对象上
	/// </summary>
	internal class UIManager : UnitySingleton<UIManager>
	{
		[SerializeField]
		private Transform m_ui_root = null;

		/// <summary>
		///     EventSystem控件，永不销毁
		/// </summary>
		[SerializeField]
		private GameObject m_event_system = null;

		/// <summary>
		///     窗口Stack中的window数量
		/// </summary>
		public Int32 WindowCountInStack
		{
			get { return m_windows_name_stack.Count; }
		}

		/// <summary>
		///     当前windows的名称
		/// </summary>
		public String CurrentUIName
		{
			get { return m_current_window == null ? String.Empty : m_current_window.name; }
		}

		/// <summary>
		///     所有的层次根节点
		/// </summary>
		private Transform[] m_layer_transforms = new Transform[(Int32) UILayer.Max];

		/// <summary>
		///     所有层次的Canvas
		/// </summary>
		private Canvas[] m_layer_canvases = new Canvas[(Int32) UILayer.Max];

		/// <summary>
		///     windows的stack
		/// </summary>
		private Stack<String> m_windows_name_stack = new Stack<String>();

		/// <summary>
		///     dialog的stack
		/// </summary>
		private LinkedList<String> m_dialog_name_list = new LinkedList<String>();

		/// <summary>
		///     所以已经被加载的界面
		/// </summary>
		private List<UI_Base> m_loaded_ui = new List<UI_Base>();

		/// <summary>
		///     当前显示的界面
		/// </summary>
		private UI_Base m_current_window = null;


		/// <summary>
		/// 置顶的标题界面
		/// 手游中通常有这样一个在每个界面的时候都会显示的东西，
		/// 通常会放置金币，体力，钻石等东西
		/// </summary>
		private UI_Base m_title_obj = null;


		protected override void OnStart()
		{
			////获取UIROOT根下所有的物体作为已加载的物体
			Int32 count = m_ui_root.childCount;
			for( Int32 i = 0; i < m_ui_root.childCount; i++ )
			{
				for( Int32 j = 0; j < (Int32)UILayer.Max; j++ )
				{
					if( m_ui_root.GetChild( i ).name.Equals( ( (UILayer)j ).ToString() ) )
					{
						m_layer_transforms[i] = m_ui_root.GetChild( i );
						m_layer_canvases[i] = m_layer_transforms[i].GetComponent<Canvas>();
					}
				}
			}
		}

		/// <summary>
		/// 创建标题栏这个特殊的存在
		/// </summary>
		public void CreateTitle()
		{
			m_title_obj = ShowUI( "ui_title" );
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public void Initialize()
		{

		}

		/// <summary>
		/// 反初始化
		/// </summary>
		public void Uninitialize()
		{
			foreach( UI_Base ui_base in m_loaded_ui )
			{
				if( ui_base.gameObject.activeSelf )
					ui_base.OnHide( null );
				ui_base.OnUnload();
				DestroyImmediate( ui_base.gameObject, true );
			}
			m_title_obj = null;
			m_loaded_ui.Clear();
			m_current_window = null;
			m_windows_name_stack.Clear();
		}

		/// <summary>
		///     清理需要清除的UI
		/// </summary>
		private void _autoDestory()
		{
			m_loaded_ui.RemoveAll( _ui_base => {
				if( !_ui_base.AutoDestroy )
					return false;
				_ui_base.OnUnload();
				DestroyImmediate( _ui_base.gameObject );
				return true;
			} );
		}

		/// <summary>
		/// 获取一个UIObject
		/// </summary>
		/// <param name="_name">UI名称</param>
		/// <param name="_create">如果没有获取到是否需要重新创建</param>
		/// <returns></returns>
		private UI_Base _getUIObject( String _name, Boolean _create )
		{
			UI_Base ui_object = m_loaded_ui.Find( ( v ) => { return v.gameObject.name == _name; } );
			if( ui_object == null && _create )
				ui_object = _createUIObject( _name );
			return ui_object;
		}

		public UI_Base ShowUI( String _name, params object[] _args )
		{
			UI_Base ui_obj = _getUIObject( _name, true );
			if( ui_obj == null )
				return null;

			if( ui_obj.gameObject.activeSelf )
				return ui_obj;

			if( ui_obj.Type == UIType.Window )
				StartCoroutine( _showWindow( ui_obj, false, _args ) );
			else if( ui_obj.Type == UIType.Dialog )
				StartCoroutine( _showDialog( ui_obj, _args ) );
			else
				StartCoroutine( _showOtherUI( ui_obj, _args ) );
			return ui_obj;
		}

		public void HideUI( String _name )
		{
			UI_Base obj = _getUIObject( _name, false );
			if( obj == null )
			{
				Utility.Log.Verbose( "HideUI: 需要隐藏的界面 [{0}]不存在!", _name );
				return;
			}
			HideUI( obj );
		}

		public void HideUI( UI_Base _ui_object )
		{
			if( _ui_object.Type == UIType.Window )
			{
				Utility.Log.Error( "HideUI: Window不能使用HideUI接口!" );
				return;
			}
			else if( _ui_object.Type == UIType.Dialog )
			{
				StartCoroutine( _hideDialog( _ui_object ) );
			}
			else
			{
				StartCoroutine( _hideOtherUI( _ui_object ) );
			}
		}

		public UI_Base ShowBackGroundUI( String _name, params object[] _args )
		{
			UI_Base ui_obj = _getUIObject( _name, true );
			if( ui_obj == null )
				return null;
			ui_obj.PreShow( null, _args );
			ui_obj.gameObject.SetActive( true );
			return ui_obj;
		}

		/// <summary>
		///     显示Windows并且重建堆栈
		/// </summary>
		/// <param name="_name">Window名称</param>
		/// <param name="_args">额外参数</param>
		/// <returns></returns>
		public UI_Base ShowWindowWithStackRebuild( String _name, params object[] _args )
		{
			UI_Base ui_win_obj = _getUIObject( _name, true );
			if( ui_win_obj == null )
			{
				Log.Error( "[UIManager.ShowWindowWithStackRebuild]: 目标UI: [0]不存在！", _name );
				return null;
			}

			if( ui_win_obj.Type != UIType.Window )
			{
				Log.Error( "[UIManager.ShowWindowWithStackRebuild]: 不是Window: {0}", _name );
				return null;
			}

			StartCoroutine( _showWindow( ui_win_obj, true, _args ) );
			return ui_win_obj;
		}


		private IEnumerator _showWindow( UI_Base _ui_object, Boolean _rebuild_stack, params object[] _args )
		{
			SetEventSystemEnable( false );
			UI_Base current_win_obj = null;
			if( m_windows_name_stack.Count != 0 )
			{
				current_win_obj = _getUIObject( m_windows_name_stack.Peek(), false );

				if( current_win_obj != null )
				{
					Single exit_time_1 = current_win_obj.PreHide( _ui_object );
					Single exit_time_2 = m_title_obj.PreHide( _ui_object );

					current_win_obj.PlayExitAnimation();
					m_title_obj.PlayExitAnimation();

					if( m_dialog_name_list.Count == 0 )
						current_win_obj.OnFocusChanged( false );

					yield return new WaitForSeconds( exit_time_1 > exit_time_2 ? exit_time_1 : exit_time_2 );

					current_win_obj.gameObject.SetActive( false );
					m_title_obj.gameObject.SetActive( false );
					current_win_obj.OnHide( _ui_object );
					m_title_obj.OnHide( _ui_object );
				}
			}

			// 重建堆栈
			if( _rebuild_stack )
				_rebuidStack( _ui_object.name );


			_ui_object.gameObject.SetActive( true );
			m_title_obj.gameObject.SetActive( true );
			Single entrance_time_1 = _ui_object.PreShow( current_win_obj, _args );
			Single entrance_time_2 = m_title_obj.PreShow(
				null,
				_args,
				_ui_object.GetTitleEntranceAnimationName( current_win_obj != null ? current_win_obj.name : String.Empty ) );

			//Single entrance_time = entrance_time_1 > entrance_time_2 ? entrance_time_1 : entrance_time_2;

			_ui_object.transform.SetAsLastSibling();
			m_title_obj.transform.SetAsLastSibling();
			_ui_object.PlayEntranceAnimation();

			if( entrance_time_1 > entrance_time_2 )
				yield return new WaitForSeconds( entrance_time_1 - entrance_time_2 );

			m_title_obj.PlayEntranceAnimation();

			m_windows_name_stack.Push( _ui_object.name );
			m_current_window = _ui_object;

			yield return new WaitForSeconds( entrance_time_1 < entrance_time_2 ? entrance_time_1 : entrance_time_2 );

			if( m_dialog_name_list.Count == 0 )
				_ui_object.OnFocusChanged( true );

			_ui_object.OnShow( current_win_obj, _args );
			m_title_obj.OnShow( null );
			yield return new WaitForEndOfFrame();
			SetEventSystemEnable( true );
			//SignalSystem.FireSignal( SignalId.UICore_OnLobbyUIShowed, _ui_object );
		}

		private IEnumerator _showDialog( UI_Base _ui_dialog, params object[] _args )
		{
			if( m_dialog_name_list.Count == 0 )
			{
				if( m_windows_name_stack.Count > 0 )
				{
					UI_Base ui_win = _getUIObject( m_windows_name_stack.Peek(), false );
					if( ui_win != null )
						ui_win.OnFocusChanged( false );
				}
			}
			else
			{
				UI_Base ui_dialog = _getUIObject( m_dialog_name_list.Last.Value, false );
				if( ui_dialog != null )
					ui_dialog.OnFocusChanged( false );
			}
			_ui_dialog.gameObject.SetActive( true );
			Single entrance_time = _ui_dialog.PreShow( null, _args );
			_ui_dialog.transform.SetAsLastSibling();
			_ui_dialog.PlayEntranceAnimation();
			m_dialog_name_list.AddLast( _ui_dialog.name );

			if( entrance_time > float.Epsilon )
				yield return new WaitForSeconds( entrance_time );

			_ui_dialog.OnFocusChanged( true );
			_ui_dialog.OnShow( null );
			yield return new WaitForEndOfFrame();
			//SignalSystem.FireSignal( SignalId.UICore_OnLobbyUIShowed, _ui_dialog );
		}

		private IEnumerator _showOtherUI( UI_Base _ui_object, params object[] _args )
		{
			_ui_object.gameObject.SetActive( true );
			Single entrance_time = _ui_object.PreShow( null, _args );
			_ui_object.transform.SetAsLastSibling();
			_ui_object.PlayEntranceAnimation();

			if( entrance_time > float.Epsilon )
				yield return new WaitForSeconds( entrance_time );
			_ui_object.OnShow( null );
		}

		private IEnumerator _hideDialog( UI_Base _ui_dialog )
		{
			if( m_dialog_name_list.Count == 0 )
			{
				Utility.Log.Warning( "[UIManager._hideDialog]: 试图隐藏Dialog: {0}, 但dialog list 为空！", _ui_dialog.name );
				yield break;
			}
			// 处理dialog焦点
			if( m_dialog_name_list.Last.Value == _ui_dialog.name )
			{
				m_dialog_name_list.RemoveLast();
				if( m_dialog_name_list.Count > 0 )
				{
					UI_Base next_topmost_dialog = _getUIObject( m_dialog_name_list.Last.Value, false );
					if( next_topmost_dialog != null )
					{
						next_topmost_dialog.OnFocusChanged( true );
					}
				}
			}
			else
			{
				var target_node = m_dialog_name_list.Find( _ui_dialog.name );
				if( null == target_node )
				{
					yield break;
				}
				m_dialog_name_list.Remove( target_node );
			}


			_ui_dialog.PlayExitAnimation();
			Single exit_time = _ui_dialog.PreHide( null );
			_ui_dialog.OnFocusChanged( false );
			if( exit_time > float.Epsilon )
				yield return new WaitForSeconds( exit_time );

			//  处理windows焦点
			if( m_dialog_name_list.Count == 0 && m_windows_name_stack.Count != 0 )
			{
				UI_Base ui_win = _getUIObject( m_windows_name_stack.Peek(), false );
				if( ui_win == null )
					Log.Error( "[UIManager._hideDialog]: 最上层windows没有被显示?!" );
				else
					ui_win.OnFocusChanged( true );
			}
			_ui_dialog.gameObject.SetActive( false );
			_ui_dialog.OnHide( null );
		}

		private IEnumerator _hideOtherUI( UI_Base _ui_object )
		{
			_ui_object.PlayExitAnimation();
			Single exit_time = _ui_object.PreHide( null );
			if( exit_time > float.Epsilon )
				yield return new WaitForSeconds( exit_time );
			_ui_object.gameObject.SetActive( false );
			_ui_object.OnHide( null );
		}

		private Boolean _rebuidStack( String _ui_name )
		{
			m_windows_name_stack.Clear();
			if( _ui_name == "ui_main" )
				return true;
			List<String> stack_list = UIStackConfigureManager.Instance.GetReBuildStackList( _ui_name );
			if( stack_list == null )
			{
				Utility.Log.Error( "[UIManager._rebuildStack]: 信息没有取到!" );
				return false;
			}
			for( Int32 i = stack_list.Count - 1; i >= 0; i-- )
				m_windows_name_stack.Push( stack_list[i] );
			return true;
		}

		public UI_Base GetCurrentWindow()
		{
			if( m_windows_name_stack.Count == 0 )
				return null;
			return _getUIObject( m_windows_name_stack.Peek(), false );
		}

		/// <summary>
		/// 创建一个UI
		/// </summary>
		/// <param name="_name"></param>
		/// <returns></returns>
		private UI_Base _createUIObject( String _name )
		{
			UnityEngine.Object prefab = null;
#if !GROOT_ASSETBUNDLE_SIMULATION && UNITY_EDITOR
			String path = String.Format( "{0}/{1}.prefab", WeiqiApp.UI_PREFAB_ROOT_PATH, _name );
			prefab = AssetDatabase.LoadMainAssetAtPath( path );
			if( prefab == null )
			{
				Utility.Log.Error( "[UIManager._create]ui: {0} 不存在!", path );
				return null;
			}

#else
			Resource_Assetbundle bundle = ResourceManager.Instance.LoadAssetbundleSync( String.Concat( "ui/", _name, ".ui" ) );
			if ( bundle.Assetbundle == null )
			{
				Utility.Log.Error( "[UIManager._create]ui: {0} 不存在!", _name );
				return null;
			}
			prefab = bundle.Assetbundle.LoadAsset( _name );
			bundle.Unload( false );
			//prefab = ResourceManager.Instance.LoadUIObject( _name );
#endif
			GameObject ui_object = Instantiate( prefab ) as GameObject;
			ui_object.SetActive( false );
			ui_object.name = _name; //预制创建出来名字中会有(Clone)所以去掉

			// 放置ui到对应的层
			UI_Base ui_base = ui_object.GetComponent<UI_Base>();
			ui_object.transform.SetParent( m_layer_transforms[(Int32)ui_base.Layer], false );

			ui_base.OnLoaded();
			m_loaded_ui.Add( ui_base );
			return ui_base;
		}

		public void SetEventSystemEnable( Boolean _enable )
		{
			m_event_system.SetActive( _enable );
		}

		/// <summary>
		///     Unity Scene切换时，清理无用的UI
		/// </summary>
		private void _onLevelBeginLoad()
		{
			foreach( UI_Base ui_object in m_loaded_ui )
			{
				if( !ui_object.AutoDestroy )
					continue;
				ui_object.gameObject.SetActive( false );
			}

			m_dialog_name_list.Clear();
			_autoDestory();
		}

		#region 处理返回按钮

		/// <summary>
		///     退回上一个窗口
		/// </summary>
		public void NavigatorBack()
		{
			StartCoroutine( _navigatorBack() );
		}

		private IEnumerator _navigatorBack()
		{
			if( m_windows_name_stack.Count < 2 )
			{
				Utility.Log.Exception( "ui_win的Stack已经到底，不能再返回" );
				yield break;
			}

			SetEventSystemEnable( false );

			// 先处理当前窗口的隐藏
			UI_Base current_win_obj = _getUIObject( m_windows_name_stack.Pop(), false );
			UI_Base next_win_obj = _getUIObject( m_windows_name_stack.Peek(), true );

			Single exit_time_1 = current_win_obj.PreHide( next_win_obj );
			Single exit_time_2 = m_title_obj.PreHide( next_win_obj );
			Single exit_time = exit_time_1 > exit_time_2 ? exit_time_1 : exit_time_2;

			current_win_obj.PlayExitAnimation();
			m_title_obj.PlayExitAnimation();

			if( m_dialog_name_list.Count == 0 )
				current_win_obj.OnFocusChanged( false );

			yield return new WaitForSeconds( exit_time );

			current_win_obj.gameObject.SetActive( false );
			m_title_obj.gameObject.SetActive( false );
			current_win_obj.OnHide( next_win_obj );
			m_title_obj.OnHide( next_win_obj );
			// 处理新窗口的显示

			next_win_obj.gameObject.SetActive( true );
			m_title_obj.gameObject.SetActive( true );
			// 打开窗口的参数暂时先不要
			Single entrance_time_1 = next_win_obj.PreShow( current_win_obj );
			Single entrance_time_2 = m_title_obj.PreShow( null, next_win_obj.GetTitleEntranceAnimationName( current_win_obj.name ) );

			Single entrance_time = entrance_time_1 > entrance_time_2 ? entrance_time_1 : entrance_time_2;

			next_win_obj.transform.SetAsLastSibling();
			m_title_obj.transform.SetAsLastSibling();
			next_win_obj.PlayEntranceAnimation();
			m_title_obj.PlayEntranceAnimation();
			m_current_window = next_win_obj;

			yield return new WaitForSeconds( entrance_time );

			if( m_dialog_name_list.Count == 0 )
				next_win_obj.OnFocusChanged( true );

			next_win_obj.OnShow( current_win_obj );
			m_title_obj.OnShow( null );
			SetEventSystemEnable( true );
		}
		#endregion

		/// <summary>
		///     重新显示栈顶窗口
		/// </summary>
		public void OnReturnLobby( Int32 _idx )
		{
			StartCoroutine( _onReturnLobby( _idx ) );
		}

		private IEnumerator _onReturnLobby( Int32 _idx )
		{
			if( m_windows_name_stack.Count == 0 )
			{
				Log.Exception( "[UIManager._onReturnLobby]: 返回到一个不存在的窗口!" );
				yield break;
			}

			if( _idx < 0 )
			{
				Int32 tmp = -_idx - 1;
				for( Int32 i = 0; i < tmp; ++i )
					m_windows_name_stack.Pop();
			}
			else
			{
				if( m_windows_name_stack.Count < _idx )
				{
					Utility.Log.Exception( "[UIManager._onReturnLobby]: 返回窗口，栈idx 无效." );
					yield break;
				}
				Int32 tmp = m_windows_name_stack.Count - _idx;
				for( Int32 i = 0; i < tmp; ++i )
					m_windows_name_stack.Pop();
			}
			UI_Base ui_object = _getUIObject( m_windows_name_stack.Peek(), true );
			Single entrance_time_1 = ui_object.PreShow( null );
			Single entrance_time_2 = m_title_obj.PreShow( null, ui_object.GetTitleEntranceAnimationName( ui_object.name ) );

			Single entrance_time = entrance_time_1 > entrance_time_2 ? entrance_time_1 : entrance_time_2;

			ui_object.gameObject.SetActive( true );
			m_title_obj.gameObject.SetActive( true );
			m_current_window = ui_object;
			ui_object.transform.SetAsLastSibling();
			m_title_obj.transform.SetAsLastSibling();
			ui_object.PlayEntranceAnimation();
			m_title_obj.PlayEntranceAnimation();

			yield return entrance_time;

			if( m_dialog_name_list.Count == 0 )
				ui_object.OnFocusChanged( true );
			ui_object.OnFocusChanged( true );

			ui_object.OnShow( null );
			m_title_obj.OnShow( null );
		}

		public void OnExitRobby()
		{
			// 清理所有无用的UI
			m_dialog_name_list.Clear();
			m_loaded_ui.RemoveAll(
				_ui_base => {
					if( _ui_base.AutoDestroy )
					{
						_ui_base.OnUnload();
						DestroyImmediate( _ui_base.gameObject );
						return true;
					}
					if( _ui_base.gameObject.activeSelf )
					{
						_ui_base.PreHide( null );
						_ui_base.OnHide( null );
						_ui_base.gameObject.SetActive( false );
					}
					return false;
				} );
		}
	}
}