using System;
using Slua;
using UnityEngine;
using UnityEngine.UI;
using SLua;
using Object = System.Object;

namespace Weiqi.UI
{
	public class UI_Base : MonoBehaviour
	{
		public Animator AnimatorObject
		{
			get
			{
				if( m_animator == null )
					m_animator = GetComponent<Animator>();
				return m_animator;
			}
		}

		private Animator m_animator = null;

		/// <summary>
		/// 使用的层次
		/// </summary>
		public UILayer Layer
		{
			get { return m_layer; }
			set { m_layer = value; }
		}

		[SerializeField]
		private UILayer m_layer = UILayer.Default;

		/// <summary>
		/// UI类型
		/// </summary>
		public UIType Type
		{
			get { return m_type; }
			set { m_type = value; }
		}
		[SerializeField]
		private UIType m_type = UIType.Window;

		/// <summary>
		/// 是否自动管理销毁
		/// </summary>
		public bool AutoDestroy
		{
			get { return m_auto_destory; }
			set { m_auto_destory = value; }
		}

		/// <summary>
		/// 是否在场景切换时销毁
		/// </summary>
		[SerializeField]
		private bool m_auto_destory = true;

		/// <summary>
		/// 进入动画时长
		/// </summary>
		[SerializeField]
		protected Single m_entrance_anim_time;

		/// <summary>
		/// 退出动画时长
		/// </summary>
		[SerializeField]
		protected Single m_exit_anim_time;

		/// <summary>
		/// 整个界面被加载完成相当于Start, 但触发同样依赖于UIManger这里不和u3d那堆逻辑混淆
		/// </summary>
		public virtual void OnLoaded()
		{
		}

		/// <summary>
		/// 界面被即将显示, 调用进入动画之前
		/// </summary>
		/// <param name="_pre_ui">之前被打开的界面</param>
		/// <param name="_args">其他界面调用时给过来的参数列表, 和传递给OnShow的参数相同</param>
		/// <returns>PreShow到OnShow调用的间隔时间</returns>
		public virtual Single PreShow( UI_Base _pre_ui, params object[] _args )
		{
			return m_entrance_anim_time;
		}

		/// <summary>
		///     播放进入动画
		/// </summary>
		public virtual void PlayEntranceAnimation()
		{
			if( AnimatorObject == null )
				return;
			AnimatorObject.CrossFade( "Base Layer.Entrance", 0f, 0, 0f );
		}

		/// <summary>
		/// 界面被显示时调用。
		/// <param name="_args">其他界面调用时给过来的参数，和传递给PreShow的参数相同</param>
		/// </summary>
		public virtual void OnShow( UI_Base _pre_ui, params object[] _args )
		{
		
		}

		/// <summary>
		/// 界面被即将隐藏, 调用退出动画之前
		/// </summary>
		/// <returns>PreHide到OnHide调用的间隔时间</returns>
		public virtual Single PreHide( UI_Base _next_ui )
		{
			return m_exit_anim_time;
		}

		/// <summary>
		/// 界面被隐藏时调用。
		/// <param name="_next_ui"></param>
		/// </summary>
		public virtual void OnHide( UI_Base _next_ui )
		{
			
		}

		/// <summary>
		/// 播放退出动画
		/// </summary>
		public virtual void PlayExitAnimation()
		{
			if( AnimatorObject == null )
				return;
			AnimatorObject.CrossFade( "Base Layer.Exit", 0f, 0, 0f );
		}

		/// <summary>
		/// 当前界面被销毁前由UIManager触发
		/// </summary>
		public virtual void OnUnload()
		{
			
		}

		/// <summary>
		/// 焦点变化
		/// </summary>
		/// <param name="_focused">获得焦点或是失去焦点</param>
		public virtual void OnFocusChanged( Boolean _focused )
		{
			
		}

		/// <summary>
		/// 获取当前UI打开时对应的Title的进入动画名称
		/// </summary>
		/// <param name="_from">之前的UI的名称</param>
		/// <returns></returns>
		public virtual String GetTitleEntranceAnimationName( String _from )
		{
			return String.Empty;
		}
	}
}