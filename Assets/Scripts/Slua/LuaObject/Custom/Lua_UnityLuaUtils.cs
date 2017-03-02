using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityLuaUtils : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetPos_s(IntPtr l) {
		try {
			UnityEngine.Transform a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			UnityLuaUtils.SetPos(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetPos_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				UnityEngine.Transform a1;
				checkType(l,1,out a1);
				var ret=UnityLuaUtils.GetPos(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				UnityEngine.Transform a1;
				checkType(l,1,out a1);
				System.Single a2;
				System.Single a3;
				System.Single a4;
				UnityLuaUtils.GetPos(a1,out a2,out a3,out a4);
				pushValue(l,true);
				pushValue(l,a2);
				pushValue(l,a3);
				pushValue(l,a4);
				return 4;
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
	static public int HideUI_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UnityLuaUtils.HideUI(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShowUI_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			UnityLuaUtils.ShowUI(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShowSingleMsgBox_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.Action a3;
			LuaDelegation.checkDelegate(l,3,out a3);
			System.Action a4;
			LuaDelegation.checkDelegate(l,4,out a4);
			UnityLuaUtils.ShowSingleMsgBox(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ShowSelectMsgBox_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			System.Action a4;
			LuaDelegation.checkDelegate(l,4,out a4);
			System.Action a5;
			LuaDelegation.checkDelegate(l,5,out a5);
			System.Action a6;
			LuaDelegation.checkDelegate(l,6,out a6);
			UnityLuaUtils.ShowSelectMsgBox(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLocaleString_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=UnityLuaUtils.GetLocaleString(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityLuaUtils");
		addMember(l,SetPos_s);
		addMember(l,GetPos_s);
		addMember(l,HideUI_s);
		addMember(l,ShowUI_s);
		addMember(l,ShowSingleMsgBox_s);
		addMember(l,ShowSelectMsgBox_s);
		addMember(l,GetLocaleString_s);
		addMember(l,UnityLuaUtils.Test,false);
		createTypeMetatable(l,null, typeof(UnityLuaUtils));
	}
}
