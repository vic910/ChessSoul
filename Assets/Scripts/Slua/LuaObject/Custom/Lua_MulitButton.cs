using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_MulitButton : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetOnClickFunction(IntPtr l) {
		try {
			MulitButton self=(MulitButton)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Action<System.Int32> a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.SetOnClickFunction(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetSecButtonString(IntPtr l) {
		try {
			MulitButton self=(MulitButton)checkSelf(l);
			var ret=self.GetSecButtonString();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"MulitButton");
		addMember(l,SetOnClickFunction);
		addMember(l,GetSecButtonString);
		createTypeMetatable(l,null, typeof(MulitButton),typeof(UnityEngine.MonoBehaviour));
	}
}
