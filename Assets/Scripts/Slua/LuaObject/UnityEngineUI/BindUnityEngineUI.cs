using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(1)]
	public class BindUnityEngineUI {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_UnityEngine_Events_UnityEvent.reg,
				Lua_UnityEngine_UI_Button_ButtonClickedEvent.reg,
				Lua_UnityEngine_UI_Text.reg,
				Lua_UnityEngine_UI_Image.reg,
				Lua_UnityEngine_UI_Button.reg,
			};
			return list;
		}
	}
}
