using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(2)]
	public class BindDotNET {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_System_Int32.reg,
				Lua_System_String.reg,
			};
			return list;
		}
	}
}
