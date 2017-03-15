using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScrollRectList : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetMaxItemCount(IntPtr l) {
		try {
			ScrollRectList self=(ScrollRectList)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetMaxItemCount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_OnItemVisible(IntPtr l) {
		try {
			ScrollRectList self=(ScrollRectList)checkSelf(l);
			System.Action<UnityEngine.GameObject,System.Int32> v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.OnItemVisible=v;
			else if(op==1) self.OnItemVisible+=v;
			else if(op==2) self.OnItemVisible-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScrollRectList");
		addMember(l,SetMaxItemCount);
		addMember(l,"OnItemVisible",null,set_OnItemVisible,true);
		createTypeMetatable(l,null, typeof(ScrollRectList),typeof(UnityEngine.MonoBehaviour));
	}
}
