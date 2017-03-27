using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Groot_SignalSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Register_s(IntPtr l) {
		try {
			Groot.SignalId a1;
			checkEnum(l,1,out a1);
			Groot.SignalCallback a2;
			LuaDelegation.checkDelegate(l,2,out a2);
			var ret=Groot.SignalSystem.Register(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Unregister_s(IntPtr l) {
		try {
			Groot.SignalId a1;
			checkEnum(l,1,out a1);
			Groot.SignalCallback a2;
			LuaDelegation.checkDelegate(l,2,out a2);
			Groot.SignalSystem.Unregister(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FireSignal_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				Groot.SignalId a1;
				checkEnum(l,1,out a1);
				Groot.SignalSystem.FireSignal(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(Groot.SignalId),typeof(Groot.SignalParameters))){
				Groot.SignalId a1;
				checkEnum(l,1,out a1);
				Groot.SignalParameters a2;
				checkType(l,2,out a2);
				Groot.SignalSystem.FireSignal(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(Groot.SignalId),typeof(System.Object))){
				Groot.SignalId a1;
				checkEnum(l,1,out a1);
				System.Object a2;
				checkType(l,2,out a2);
				Groot.SignalSystem.FireSignal(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				Groot.SignalId a1;
				checkEnum(l,1,out a1);
				System.Object a2;
				checkType(l,2,out a2);
				System.Object a3;
				checkType(l,3,out a3);
				Groot.SignalSystem.FireSignal(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				Groot.SignalId a1;
				checkEnum(l,1,out a1);
				System.Object a2;
				checkType(l,2,out a2);
				System.Object a3;
				checkType(l,3,out a3);
				System.Object a4;
				checkType(l,4,out a4);
				Groot.SignalSystem.FireSignal(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				Groot.SignalId a1;
				checkEnum(l,1,out a1);
				System.Object a2;
				checkType(l,2,out a2);
				System.Object a3;
				checkType(l,3,out a3);
				System.Object a4;
				checkType(l,4,out a4);
				System.Object a5;
				checkType(l,5,out a5);
				Groot.SignalSystem.FireSignal(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(argc==7){
				Groot.SignalId a1;
				checkEnum(l,1,out a1);
				System.Object a2;
				checkType(l,2,out a2);
				System.Object a3;
				checkType(l,3,out a3);
				System.Object a4;
				checkType(l,4,out a4);
				System.Object a5;
				checkType(l,5,out a5);
				System.Object a6;
				checkType(l,6,out a6);
				System.Object a7;
				checkType(l,7,out a7);
				Groot.SignalSystem.FireSignal(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
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
	static public int Clear_s(IntPtr l) {
		try {
			Groot.SignalSystem.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Groot.SignalSystem");
		addMember(l,Register_s);
		addMember(l,Unregister_s);
		addMember(l,FireSignal_s);
		addMember(l,Clear_s);
		createTypeMetatable(l,null, typeof(Groot.SignalSystem));
	}
}
