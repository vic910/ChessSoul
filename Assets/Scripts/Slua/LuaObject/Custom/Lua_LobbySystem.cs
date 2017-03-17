using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_LobbySystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			LobbySystem o;
			o=new LobbySystem();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetPlayerInfo(IntPtr l) {
		try {
			LobbySystem self=(LobbySystem)checkSelf(l);
			System.UInt64 a1;
			checkType(l,2,out a1);
			var ret=self.GetPlayerInfo(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetAllPlayerInfo(IntPtr l) {
		try {
			LobbySystem self=(LobbySystem)checkSelf(l);
			var ret=self.GetAllPlayerInfo();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,LobbySystem.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"LobbySystem");
		addMember(l,GetPlayerInfo);
		addMember(l,GetAllPlayerInfo);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(LobbySystem));
	}
}
