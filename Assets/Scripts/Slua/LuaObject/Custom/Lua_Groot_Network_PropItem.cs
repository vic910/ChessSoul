using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Groot_Network_PropItem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Groot.Network.PropItem o;
			o=new Groot.Network.PropItem();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PropID(IntPtr l) {
		try {
			Groot.Network.PropItem self=(Groot.Network.PropItem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PropID);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PropID(IntPtr l) {
		try {
			Groot.Network.PropItem self=(Groot.Network.PropItem)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.PropID=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Count(IntPtr l) {
		try {
			Groot.Network.PropItem self=(Groot.Network.PropItem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Count(IntPtr l) {
		try {
			Groot.Network.PropItem self=(Groot.Network.PropItem)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.Count=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Groot.Network.PropItem");
		addMember(l,"PropID",get_PropID,set_PropID,true);
		addMember(l,"Count",get_Count,set_Count,true);
		createTypeMetatable(l,constructor, typeof(Groot.Network.PropItem));
	}
}
