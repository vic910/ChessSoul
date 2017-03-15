using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_PlayerInfoConfig : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			PlayerInfoConfig o;
			o=new PlayerInfoConfig();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetAreaInfo(IntPtr l) {
		try {
			PlayerInfoConfig self=(PlayerInfoConfig)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetAreaInfo(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLivenessInfo(IntPtr l) {
		try {
			PlayerInfoConfig self=(PlayerInfoConfig)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetLivenessInfo(a1);
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
			pushValue(l,PlayerInfoConfig.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Instance(IntPtr l) {
		try {
			PlayerInfoConfig v;
			checkType(l,2,out v);
			PlayerInfoConfig.Instance=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"PlayerInfoConfig");
		addMember(l,GetAreaInfo);
		addMember(l,GetLivenessInfo);
		addMember(l,"Instance",get_Instance,set_Instance,false);
		createTypeMetatable(l,constructor, typeof(PlayerInfoConfig));
	}
}
