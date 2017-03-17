using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_PlayerOnlineSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			PlayerOnlineSystem o;
			o=new PlayerOnlineSystem();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetPlayerInfoByIndex(IntPtr l) {
		try {
			PlayerOnlineSystem self=(PlayerOnlineSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			PlayerOnlineSystem.PlayerType a2;
			checkEnum(l,3,out a2);
			var ret=self.GetPlayerInfoByIndex(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetPlayerInfoByName(IntPtr l) {
		try {
			PlayerOnlineSystem self=(PlayerOnlineSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			PlayerOnlineSystem.PlayerType a2;
			checkEnum(l,3,out a2);
			var ret=self.GetPlayerInfoByName(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetPlayerInfoCount(IntPtr l) {
		try {
			PlayerOnlineSystem self=(PlayerOnlineSystem)checkSelf(l);
			PlayerOnlineSystem.PlayerType a1;
			checkEnum(l,2,out a1);
			var ret=self.GetPlayerInfoCount(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UpdatePlayerInfo(IntPtr l) {
		try {
			PlayerOnlineSystem self=(PlayerOnlineSystem)checkSelf(l);
			PlayerOnlineSystem.PlayerType a1;
			checkEnum(l,2,out a1);
			System.UInt64 a2;
			checkType(l,3,out a2);
			self.UpdatePlayerInfo(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UpdateAppoint(IntPtr l) {
		try {
			PlayerOnlineSystem self=(PlayerOnlineSystem)checkSelf(l);
			System.Collections.Generic.List<System.UInt64> a1;
			checkType(l,2,out a1);
			self.UpdateAppoint(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetPlayerInfoList(IntPtr l) {
		try {
			PlayerOnlineSystem self=(PlayerOnlineSystem)checkSelf(l);
			PlayerOnlineSystem.PlayerType a1;
			checkEnum(l,2,out a1);
			var ret=self.GetPlayerInfoList(a1);
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
			pushValue(l,PlayerOnlineSystem.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"PlayerOnlineSystem");
		addMember(l,GetPlayerInfoByIndex);
		addMember(l,GetPlayerInfoByName);
		addMember(l,GetPlayerInfoCount);
		addMember(l,UpdatePlayerInfo);
		addMember(l,UpdateAppoint);
		addMember(l,GetPlayerInfoList);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(PlayerOnlineSystem));
	}
}
