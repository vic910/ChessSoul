using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Groot_Network_ItemAttr : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Groot.Network.ItemAttr o;
			o=new Groot.Network.ItemAttr();
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
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
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
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
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
	static public int get_NeedLiveness(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NeedLiveness);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_NeedLiveness(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.NeedLiveness=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_NeedMoney(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NeedMoney);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_NeedMoney(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.NeedMoney=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_NeedPlayerLevel(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NeedPlayerLevel);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_NeedPlayerLevel(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.NeedPlayerLevel=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Level(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Level(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.Byte v;
			checkType(l,2,out v);
			self.Level=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Type(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Type(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.Byte v;
			checkType(l,2,out v);
			self.Type=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PriceGold(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PriceGold);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PriceGold(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.PriceGold=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PriceMoney(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PriceMoney);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PriceMoney(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.PriceMoney=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CurrMoneyType(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurrMoneyType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_CurrMoneyType(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.Byte v;
			checkType(l,2,out v);
			self.CurrMoneyType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_RecycleMoney(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.RecycleMoney);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_RecycleMoney(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.RecycleMoney=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Attrs(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Attrs);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Attrs(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.Collections.Generic.List<System.Byte> v;
			checkType(l,2,out v);
			self.Attrs=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Name(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Name(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.Name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Desc(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Desc);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Desc(IntPtr l) {
		try {
			Groot.Network.ItemAttr self=(Groot.Network.ItemAttr)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.Desc=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Groot.Network.ItemAttr");
		addMember(l,"PropID",get_PropID,set_PropID,true);
		addMember(l,"NeedLiveness",get_NeedLiveness,set_NeedLiveness,true);
		addMember(l,"NeedMoney",get_NeedMoney,set_NeedMoney,true);
		addMember(l,"NeedPlayerLevel",get_NeedPlayerLevel,set_NeedPlayerLevel,true);
		addMember(l,"Level",get_Level,set_Level,true);
		addMember(l,"Type",get_Type,set_Type,true);
		addMember(l,"PriceGold",get_PriceGold,set_PriceGold,true);
		addMember(l,"PriceMoney",get_PriceMoney,set_PriceMoney,true);
		addMember(l,"CurrMoneyType",get_CurrMoneyType,set_CurrMoneyType,true);
		addMember(l,"RecycleMoney",get_RecycleMoney,set_RecycleMoney,true);
		addMember(l,"Attrs",get_Attrs,set_Attrs,true);
		addMember(l,"Name",get_Name,set_Name,true);
		addMember(l,"Desc",get_Desc,set_Desc,true);
		createTypeMetatable(l,constructor, typeof(Groot.Network.ItemAttr));
	}
}
