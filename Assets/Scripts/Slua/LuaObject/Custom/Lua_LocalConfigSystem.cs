using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_LocalConfigSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			LocalConfigSystem o;
			o=new LocalConfigSystem();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Update(IntPtr l) {
		try {
			LocalConfigSystem self=(LocalConfigSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.Update(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetOptionConfig(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				LocalConfigSystem self=(LocalConfigSystem)checkSelf(l);
				var ret=self.GetOptionConfig();
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				LocalConfigSystem self=(LocalConfigSystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.GetOptionConfig(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UpdateCurAccount(IntPtr l) {
		try {
			LocalConfigSystem self=(LocalConfigSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.UpdateCurAccount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Instacne(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,LocalConfigSystem.Instacne);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"LocalConfigSystem");
		addMember(l,Update);
		addMember(l,GetOptionConfig);
		addMember(l,UpdateCurAccount);
		addMember(l,"Instacne",get_Instacne,null,false);
		createTypeMetatable(l,constructor, typeof(LocalConfigSystem));
	}
}
