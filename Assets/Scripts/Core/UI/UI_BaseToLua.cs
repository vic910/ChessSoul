using System;
using Slua;
using UnityEngine;
using UnityEngine.UI;
using SLua;
using Object = System.Object;

namespace Weiqi.UI
{
    /// <summary>
    /// 全脚本逻辑UI用这个
    /// </summary>
    public class UI_BaseToLua : UI_Base
    {
        [SerializeField]
        private GameObject[] m_game_objects;

        [SerializeField]
        private Text[] m_texts;

        [SerializeField]
        private Button[] m_buttons;

        [SerializeField]
        private Image[] m_images;

        [SerializeField]
        private InputField[] m_editors;

        [SerializeField]
        private Toggle[] m_toggles;

        private LuaTable m_lua_script;

        #region LuaFunctions 
        enum LuaFunctionName
        {
            OnLoaded,
            OnUnload,
            PreShow,
            OnShow,
            PreHide,
            OnHide,
            OnFocusChanged,

            Max
        }

        private static string[] s_lua_function_name = new[]
        {
            "OnLoaded",
            "OnUnloaded",
            "PreShow",
            "OnShow",
            "PreHide",
            "OnHide",
            "OnFocusChanged"
        };
        private LuaFunction[] m_lua_functions = new LuaFunction[(Int32)LuaFunctionName.Max];
        #endregion

        public override void OnLoaded()
        {
            m_lua_script = UILuaSvr.Instance.GetScript(name);
            if (m_lua_script == null)
            {
                Utility.Log.Critical("[UI_Base.OnLoaded]: ui [{0}] 没有lua脚本!", name);
                return;
            }


            LuaTable go_table = new LuaTable(LuaState.main);
            m_lua_script["mUIWidgets"] = go_table;

            Int32 count = m_game_objects.Length;
            for (Int32 i = 0; i < count; i++)
                go_table[m_game_objects[i].name] = m_game_objects[i];

            count = m_texts.Length;
            for (Int32 i = 0; i < count; i++)
                go_table[m_texts[i].name] = m_texts[i];

            count = m_buttons.Length;
            for (Int32 i = 0; i < count; i++)
                go_table[m_buttons[i].name] = m_buttons[i];

            count = m_images.Length;
            for (Int32 i = 0; i < count; i++)
                go_table[m_images[i].name] = m_images[i];

            count = m_editors.Length;
            for (Int32 i = 0; i < count; i++)
                go_table[m_editors[i].name] = m_editors[i];

            count = m_toggles.Length;
            for (Int32 i = 0; i < count; i++)
                go_table[m_toggles[i].name] = m_toggles[i];

            m_lua_script["mRectTransform"] = (RectTransform)transform;
            m_lua_script["mAnimator"] = AnimatorObject;

            for (Int32 i = 0; i < (Int32)LuaFunctionName.Max; ++i)
            {
                m_lua_functions[i] = (LuaFunction)m_lua_script[s_lua_function_name[i]];
            }

            if (m_lua_functions[(Int32)LuaFunctionName.OnLoaded] != null)
                m_lua_functions[(Int32)LuaFunctionName.OnLoaded].call();
        }

        public override float PreShow(UI_Base _pre_ui, params object[] _args)
        {
            if (m_lua_functions[(Int32)LuaFunctionName.PreShow] == null)
                return m_entrance_anim_time;
            object re = m_lua_functions[(Int32)LuaFunctionName.PreShow].call(_pre_ui == null ? "" : _pre_ui.name, _args);
            if (re == null)
                return m_entrance_anim_time;
            return (Single)re;
        }

        /// <summary>
        /// 播放进入动画
        /// </summary>
        public override void PlayEntranceAnimation()
        {
            if (AnimatorObject == null)
                return;
            AnimatorObject.CrossFade("Base Layer.Entrance", 0f, 0, 0f);
        }

        public override void OnShow(UI_Base _pre_ui, params object[] _args)
        {
            if (m_lua_functions[(Int32)LuaFunctionName.OnShow] == null)
                return;
            m_lua_functions[(Int32)LuaFunctionName.OnShow].call(_pre_ui == null ? String.Empty : _pre_ui.name, _args);
        }

        /// <summary>
        /// 界面被即将隐藏, 调用退出动画之前
        /// </summary>
        /// <returns>PreHide到OnHide调用的间隔时间</returns>
        public override Single PreHide(UI_Base _next_ui)
        {
            if (m_lua_functions[(Int32)LuaFunctionName.PreHide] == null)
                return m_exit_anim_time;
            object re = m_lua_functions[(Int32)LuaFunctionName.PreHide].call(_next_ui == null ? String.Empty : _next_ui.name);
            if (re == null)
                return m_exit_anim_time;
            return (Single)re;
        }

        /// <summary>
        /// 界面被隐藏时调用。
        /// <param name="_next_ui"></param>
        /// </summary>
        public override void OnHide(UI_Base _next_ui)
        {
            if (m_lua_functions[(Int32)LuaFunctionName.OnHide] == null)
                return;
            m_lua_functions[(Int32)LuaFunctionName.OnHide].call(_next_ui == null ? String.Empty : _next_ui.name);
        }

        /// <summary>
        /// 播放退出动画
        /// </summary>
        public override void PlayExitAnimation()
        {
            if (AnimatorObject == null)
                return;
            AnimatorObject.CrossFade("Base Layer.Exit", 0f, 0, 0f);
        }

        /// <summary>
        /// 当前界面被销毁前由UIManager触发
        /// </summary>
        public override void OnUnload()
        {
            if (m_lua_functions[(Int32)LuaFunctionName.OnUnload] == null)
                return;
            m_lua_functions[(Int32)LuaFunctionName.OnUnload].call();
        }

        /// <summary>
        /// 焦点变化
        /// </summary>
        /// <param name="_focused">获得焦点或是失去焦点</param>
        public override void OnFocusChanged(Boolean _focused)
        {
            if (m_lua_functions[(Int32)LuaFunctionName.OnFocusChanged] == null)
                return;
            m_lua_functions[(Int32)LuaFunctionName.OnFocusChanged].call(_focused);
        }

        /// <summary>
        /// 获取当前UI打开时对应的Title的进入动画名称
        /// </summary>
        /// <param name="_from">之前的UI的名称</param>
        /// <returns></returns>
        public override String GetTitleEntranceAnimationName(String _from)
        {
            return String.Empty;
        }
    }
}