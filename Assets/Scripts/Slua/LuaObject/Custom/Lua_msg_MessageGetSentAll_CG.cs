using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_msg_MessageGetSentAll_CG : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			msg_MessageGetSentAll_CG o;
			o=new msg_MessageGetSentAll_CG();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_iPlayerID(IntPtr l) {
		try {
			msg_MessageGetSentAll_CG self=(msg_MessageGetSentAll_CG)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.iPlayerID);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_iPlayerID(IntPtr l) {
		try {
			msg_MessageGetSentAll_CG self=(msg_MessageGetSentAll_CG)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.iPlayerID=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"msg_MessageGetSentAll_CG");
		addMember(l,"iPlayerID",get_iPlayerID,set_iPlayerID,true);
		createTypeMetatable(l,constructor, typeof(msg_MessageGetSentAll_CG),typeof(Groot.Network.MessageBase));
	}
}
