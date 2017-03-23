using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Groot_Network_NetManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Initialize(IntPtr l) {
		try {
			Groot.Network.NetManager self=(Groot.Network.NetManager)checkSelf(l);
			var ret=self.Initialize();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Uninitialize(IntPtr l) {
		try {
			Groot.Network.NetManager self=(Groot.Network.NetManager)checkSelf(l);
			self.Uninitialize();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SendMsg(IntPtr l) {
		try {
			Groot.Network.NetManager self=(Groot.Network.NetManager)checkSelf(l);
			Groot.Network.MessageBase a1;
			checkType(l,2,out a1);
			var ret=self.SendMsg(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RequestConnect(IntPtr l) {
		try {
			Groot.Network.NetManager self=(Groot.Network.NetManager)checkSelf(l);
			self.RequestConnect();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RequestDisConnect(IntPtr l) {
		try {
			Groot.Network.NetManager self=(Groot.Network.NetManager)checkSelf(l);
			self.RequestDisConnect();
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
			pushValue(l,Groot.Network.NetManager.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Groot.Network.NetManager");
		addMember(l,Initialize);
		addMember(l,Uninitialize);
		addMember(l,SendMsg);
		addMember(l,RequestConnect);
		addMember(l,RequestDisConnect);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(Groot.Network.NetManager));
	}
}
