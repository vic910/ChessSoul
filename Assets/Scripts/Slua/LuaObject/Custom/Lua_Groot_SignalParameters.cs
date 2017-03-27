using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Groot_SignalParameters : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			Groot.SignalParameters o;
			if(argc==1){
				o=new Groot.SignalParameters();
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==2){
				System.Int32 a1;
				checkType(l,2,out a1);
				o=new Groot.SignalParameters(a1);
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
	static public int AddParameter(IntPtr l) {
		try {
			Groot.SignalParameters self=(Groot.SignalParameters)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.AddParameter(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Clear(IntPtr l) {
		try {
			Groot.SignalParameters self=(Groot.SignalParameters)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Count(IntPtr l) {
		try {
			Groot.SignalParameters self=(Groot.SignalParameters)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getItem(IntPtr l) {
		try {
			Groot.SignalParameters self=(Groot.SignalParameters)checkSelf(l);
			int v;
			checkType(l,2,out v);
			var ret = self[v];
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Groot.SignalParameters");
		addMember(l,AddParameter);
		addMember(l,Clear);
		addMember(l,getItem);
		addMember(l,"Count",get_Count,null,true);
		createTypeMetatable(l,constructor, typeof(Groot.SignalParameters));
	}
}
