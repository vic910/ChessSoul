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
	static public int get_receivedMails(IntPtr l) {
		try {
			EMailSystem self=(EMailSystem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.receivedMails);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_receivedMails(IntPtr l) {
		try {
			EMailSystem self=(EMailSystem)checkSelf(l);
			System.Collections.Generic.List<ShortMessageBaseInfo> v;
			checkType(l,2,out v);
			self.receivedMails=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_sentMails(IntPtr l) {
		try {
			EMailSystem self=(EMailSystem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.sentMails);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_sentMails(IntPtr l) {
		try {
			EMailSystem self=(EMailSystem)checkSelf(l);
			System.Collections.Generic.List<ShortMessageBaseInfo> v;
			checkType(l,2,out v);
			self.sentMails=v;
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
		addMember(l,"receivedMails",get_receivedMails,set_receivedMails,true);
		addMember(l,"sentMails",get_sentMails,set_sentMails,true);
		createTypeMetatable(l,constructor, typeof(EMailSystem));
	}
}
