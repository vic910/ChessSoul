using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ShortMessageBaseInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ShortMessageBaseInfo o;
			o=new ShortMessageBaseInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_iMessageID(IntPtr l) {
		try {
			ShortMessageBaseInfo self=(ShortMessageBaseInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.iMessageID);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_iMessageID(IntPtr l) {
		try {
			ShortMessageBaseInfo self=(ShortMessageBaseInfo)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.iMessageID=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_stSendTime(IntPtr l) {
		try {
			ShortMessageBaseInfo self=(ShortMessageBaseInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.stSendTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_stSendTime(IntPtr l) {
		try {
			ShortMessageBaseInfo self=(ShortMessageBaseInfo)checkSelf(l);
			Groot.Network.SysTimeData v;
			checkType(l,2,out v);
			self.stSendTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cRead(IntPtr l) {
		try {
			ShortMessageBaseInfo self=(ShortMessageBaseInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cRead);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cRead(IntPtr l) {
		try {
			ShortMessageBaseInfo self=(ShortMessageBaseInfo)checkSelf(l);
			System.Byte v;
			checkType(l,2,out v);
			self.cRead=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_szSender(IntPtr l) {
		try {
			ShortMessageBaseInfo self=(ShortMessageBaseInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.szSender);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_szSender(IntPtr l) {
		try {
			ShortMessageBaseInfo self=(ShortMessageBaseInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.szSender=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_szTitle(IntPtr l) {
		try {
			ShortMessageBaseInfo self=(ShortMessageBaseInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.szTitle);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_szTitle(IntPtr l) {
		try {
			ShortMessageBaseInfo self=(ShortMessageBaseInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.szTitle=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ShortMessageBaseInfo");
		addMember(l,"iMessageID",get_iMessageID,set_iMessageID,true);
		addMember(l,"stSendTime",get_stSendTime,set_stSendTime,true);
		addMember(l,"cRead",get_cRead,set_cRead,true);
		addMember(l,"szSender",get_szSender,set_szSender,true);
		addMember(l,"szTitle",get_szTitle,set_szTitle,true);
		createTypeMetatable(l,constructor, typeof(ShortMessageBaseInfo));
	}
}
