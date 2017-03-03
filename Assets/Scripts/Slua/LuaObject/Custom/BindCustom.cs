using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(3)]
	public class BindCustom {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_UnityLuaUtils.reg,
				Lua_MainPlayer.reg,
				Lua_PlayerInfoConfig.reg,
				Lua_ScrollRectList.reg,
			};
			return list;
		}
	}
}
