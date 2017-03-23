using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_SendMailMessage : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			SendMailMessage o;
			o=new SendMailMessage();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_senderId(IntPtr l) {
		try {
			SendMailMessage self=(SendMailMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.senderId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_senderId(IntPtr l) {
		try {
			SendMailMessage self=(SendMailMessage)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.senderId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_receiverAddress(IntPtr l) {
		try {
			SendMailMessage self=(SendMailMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.receiverAddress);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_receiverAddress(IntPtr l) {
		try {
			SendMailMessage self=(SendMailMessage)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.receiverAddress=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_mailTitle(IntPtr l) {
		try {
			SendMailMessage self=(SendMailMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.mailTitle);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_mailTitle(IntPtr l) {
		try {
			SendMailMessage self=(SendMailMessage)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.mailTitle=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_mailContent(IntPtr l) {
		try {
			SendMailMessage self=(SendMailMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.mailContent);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_mailContent(IntPtr l) {
		try {
			SendMailMessage self=(SendMailMessage)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.mailContent=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"SendMailMessage");
		addMember(l,"senderId",get_senderId,set_senderId,true);
		addMember(l,"receiverAddress",get_receiverAddress,set_receiverAddress,true);
		addMember(l,"mailTitle",get_mailTitle,set_mailTitle,true);
		addMember(l,"mailContent",get_mailContent,set_mailContent,true);
		createTypeMetatable(l,constructor, typeof(SendMailMessage),typeof(Groot.Network.MessageBase));
	}
}
