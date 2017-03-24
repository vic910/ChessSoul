using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_EMailSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			EMailSystem o;
			o=new EMailSystem();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SendMessageGetSentAll(IntPtr l) {
		try {
			EMailSystem self=(EMailSystem)checkSelf(l);
			self.SendMessageGetSentAll();
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
			pushValue(l,EMailSystem.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_received_mails(IntPtr l) {
		try {
			EMailSystem self=(EMailSystem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_received_mails);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_received_mails(IntPtr l) {
		try {
			EMailSystem self=(EMailSystem)checkSelf(l);
			System.Collections.Generic.List<ShortMessageBaseInfo> v;
			checkType(l,2,out v);
			self.m_received_mails=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_send_mails(IntPtr l) {
		try {
			EMailSystem self=(EMailSystem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_send_mails);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_send_mails(IntPtr l) {
		try {
			EMailSystem self=(EMailSystem)checkSelf(l);
			System.Collections.Generic.List<ShortMessageBaseInfo> v;
			checkType(l,2,out v);
			self.m_send_mails=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"EMailSystem");
		addMember(l,SendMessageGetSentAll);
		addMember(l,"Instance",get_Instance,null,false);
		addMember(l,"m_received_mails",get_m_received_mails,set_m_received_mails,true);
		addMember(l,"m_send_mails",get_m_send_mails,set_m_send_mails,true);
		createTypeMetatable(l,constructor, typeof(EMailSystem));
	}
}
