using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(1)]
	public class BindUnityEngineUI {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_UnityEngine_EventSystems_EventSystem.reg,
				Lua_UnityEngine_Events_UnityEventBase.reg,
				Lua_UnityEngine_Events_UnityEvent.reg,
				Lua_UnityEngine_UI_Button_ButtonClickedEvent.reg,
				Lua_UnityEngine_UI_Text.reg,
				Lua_UnityEngine_UI_Image.reg,
				Lua_UnityEngine_UI_Button.reg,
				Lua_UnityEngine_UI_InputField.reg,
				Lua_UnityEngine_UI_Toggle_ToggleEvent.reg,
				Lua_UnityEngine_UI_Toggle.reg,
			};
			return list;
		}
	}
}
