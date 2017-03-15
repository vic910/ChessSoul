using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_MainPlayer : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			MainPlayer o;
			o=new MainPlayer();
			pushValue(l,true);
			pushValue(l,o);
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
			pushValue(l,MainPlayer.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PlayerInfo(IntPtr l) {
		try {
			MainPlayer self=(MainPlayer)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PlayerInfo);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"MainPlayer");
		addMember(l,"Instance",get_Instance,null,false);
		addMember(l,"PlayerInfo",get_PlayerInfo,null,true);
		createTypeMetatable(l,constructor, typeof(MainPlayer));
	}
}
