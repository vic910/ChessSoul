using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Groot_Network_MessageBase : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			Groot.Network.MessageBase o;
			if(argc==1){
				o=new Groot.Network.MessageBase();
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==4){
				Groot.Network.EMsgDirection a1;
				checkEnum(l,2,out a1);
				Groot.Network.EMsgType a2;
				checkEnum(l,3,out a2);
				System.UInt16 a3;
				checkType(l,4,out a3);
				o=new Groot.Network.MessageBase(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MsgId(IntPtr l) {
		try {
			Groot.Network.MessageBase self=(Groot.Network.MessageBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MsgId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MsgId(IntPtr l) {
		try {
			Groot.Network.MessageBase self=(Groot.Network.MessageBase)checkSelf(l);
			System.UInt16 v;
			checkType(l,2,out v);
			self.MsgId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MsgType(IntPtr l) {
		try {
			Groot.Network.MessageBase self=(Groot.Network.MessageBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MsgType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MsgType(IntPtr l) {
		try {
			Groot.Network.MessageBase self=(Groot.Network.MessageBase)checkSelf(l);
			System.Byte v;
			checkType(l,2,out v);
			self.MsgType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MsgDirection(IntPtr l) {
		try {
			Groot.Network.MessageBase self=(Groot.Network.MessageBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MsgDirection);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MsgDirection(IntPtr l) {
		try {
			Groot.Network.MessageBase self=(Groot.Network.MessageBase)checkSelf(l);
			System.Byte v;
			checkType(l,2,out v);
			self.MsgDirection=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Groot.Network.MessageBase");
		addMember(l,"MsgId",get_MsgId,set_MsgId,true);
		addMember(l,"MsgType",get_MsgType,set_MsgType,true);
		addMember(l,"MsgDirection",get_MsgDirection,set_MsgDirection,true);
		createTypeMetatable(l,constructor, typeof(Groot.Network.MessageBase));
	}
}
