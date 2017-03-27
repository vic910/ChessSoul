using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ItemSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ItemSystem o;
			o=new ItemSystem();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetItemAttr(IntPtr l) {
		try {
			ItemSystem self=(ItemSystem)checkSelf(l);
			System.UInt64 a1;
			checkType(l,2,out a1);
			var ret=self.GetItemAttr(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetMyItemIDByIndex(IntPtr l) {
		try {
			ItemSystem self=(ItemSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetMyItemIDByIndex(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetAllMyItem(IntPtr l) {
		try {
			ItemSystem self=(ItemSystem)checkSelf(l);
			var ret=self.GetAllMyItem();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SaleItemToSystem(IntPtr l) {
		try {
			ItemSystem self=(ItemSystem)checkSelf(l);
			System.UInt64 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.SaleItemToSystem(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ItemSystem.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ItemSystem");
		addMember(l,GetItemAttr);
		addMember(l,GetMyItemIDByIndex);
		addMember(l,GetAllMyItem);
		addMember(l,SaleItemToSystem);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(ItemSystem));
	}
}
