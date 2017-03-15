using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(0)]
	public class BindUnityEngine {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_UnityEngine_Object.reg,
				Lua_UnityEngine_GameObject.reg,
				Lua_UnityEngine_Transform.reg,
				Lua_UnityEngine_Vector2.reg,
				Lua_UnityEngine_Vector3.reg,
				Lua_UnityEngine_Vector4.reg,
				Lua_UnityEngine_Quaternion.reg,
				Lua_UnityEngine_Matrix4x4.reg,
				Lua_UnityEngine_Color.reg,
			};
			return list;
		}
	}
}
