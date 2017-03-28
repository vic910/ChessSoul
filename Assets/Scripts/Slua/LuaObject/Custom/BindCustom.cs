using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(3)]
	public class BindCustom {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_PlayerInfoConfig.reg,
				Lua_UnityLuaUtils.reg,
				Lua_MainPlayer.reg,
				Lua_Groot_Network_MessageBase.reg,
				Lua_SendMailMessage.reg,
				Lua_Groot_SignalId.reg,
				Lua_Groot_SignalParameters.reg,
				Lua_Groot_SignalSystem.reg,
				Lua_EMailSystem.reg,
				Lua_ItemSystem.reg,
				Lua_LobbySystem.reg,
				Lua_LocalConfigSystem.reg,
				Lua_PlayerOnlineSystem.reg,
				Lua_RoomSystem.reg,
				Lua_MulitButton.reg,
				Lua_ScrollRectList.reg,
			};
			return list;
		}
	}
}
