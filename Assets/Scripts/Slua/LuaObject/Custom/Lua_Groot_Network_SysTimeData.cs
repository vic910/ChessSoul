using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Groot_Network_SysTimeData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Groot.Network.SysTimeData o;
			o=new Groot.Network.SysTimeData();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Year(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Year);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Year(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			System.UInt16 v;
			checkType(l,2,out v);
			self.Year=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Month(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Month);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Month(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			System.UInt16 v;
			checkType(l,2,out v);
			self.Month=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DayOfWeek(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DayOfWeek);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_DayOfWeek(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			System.UInt16 v;
			checkType(l,2,out v);
			self.DayOfWeek=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Day(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Day);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Day(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			System.UInt16 v;
			checkType(l,2,out v);
			self.Day=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Hour(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Hour);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Hour(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			System.UInt16 v;
			checkType(l,2,out v);
			self.Hour=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Minute(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Minute);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Minute(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			System.UInt16 v;
			checkType(l,2,out v);
			self.Minute=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Second(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Second);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Second(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			System.UInt16 v;
			checkType(l,2,out v);
			self.Second=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Milliseconds(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Milliseconds);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Milliseconds(IntPtr l) {
		try {
			Groot.Network.SysTimeData self=(Groot.Network.SysTimeData)checkSelf(l);
			System.UInt16 v;
			checkType(l,2,out v);
			self.Milliseconds=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Groot.Network.SysTimeData");
		addMember(l,"Year",get_Year,set_Year,true);
		addMember(l,"Month",get_Month,set_Month,true);
		addMember(l,"DayOfWeek",get_DayOfWeek,set_DayOfWeek,true);
		addMember(l,"Day",get_Day,set_Day,true);
		addMember(l,"Hour",get_Hour,set_Hour,true);
		addMember(l,"Minute",get_Minute,set_Minute,true);
		addMember(l,"Second",get_Second,set_Second,true);
		addMember(l,"Milliseconds",get_Milliseconds,set_Milliseconds,true);
		createTypeMetatable(l,constructor, typeof(Groot.Network.SysTimeData));
	}
}
