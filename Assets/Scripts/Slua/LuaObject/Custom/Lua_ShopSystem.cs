using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ShopSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ShopSystem o;
			o=new ShopSystem();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int BuyItemToSystem(IntPtr l) {
		try {
			ShopSystem self=(ShopSystem)checkSelf(l);
			self.BuyItemToSystem();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetSaleItem(IntPtr l) {
		try {
			ShopSystem self=(ShopSystem)checkSelf(l);
			System.UInt64 a1;
			checkType(l,2,out a1);
			var ret=self.GetSaleItem(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetSaleItemAttr(IntPtr l) {
		try {
			ShopSystem self=(ShopSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetSaleItemAttr(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetShowList(IntPtr l) {
		try {
			ShopSystem self=(ShopSystem)checkSelf(l);
			System.Collections.Generic.List<Groot.Network.SaleItem> a1;
			checkType(l,2,out a1);
			self.SetShowList(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetItemInfo(IntPtr l) {
		try {
			ShopSystem self=(ShopSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.GetItemInfo(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetShoppingcarItemList(IntPtr l) {
		try {
			ShopSystem self=(ShopSystem)checkSelf(l);
			var ret=self.GetShoppingcarItemList();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetShoppingcarItemList(IntPtr l) {
		try {
			ShopSystem self=(ShopSystem)checkSelf(l);
			System.UInt64 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.SetShoppingcarItemList(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddShoppingcarItemList(IntPtr l) {
		try {
			ShopSystem self=(ShopSystem)checkSelf(l);
			System.UInt64 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.AddShoppingcarItemList(a1,a2);
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
			pushValue(l,ShopSystem.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ShopSystem");
		addMember(l,BuyItemToSystem);
		addMember(l,GetSaleItem);
		addMember(l,GetSaleItemAttr);
		addMember(l,SetShowList);
		addMember(l,GetItemInfo);
		addMember(l,GetShoppingcarItemList);
		addMember(l,SetShoppingcarItemList);
		addMember(l,AddShoppingcarItemList);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(ShopSystem));
	}
}
