using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_RoomSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			RoomSystem o;
			o=new RoomSystem();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRoomCount(IntPtr l) {
		try {
			RoomSystem self=(RoomSystem)checkSelf(l);
			System.Byte a1;
			checkType(l,2,out a1);
			var ret=self.GetRoomCount(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRoomInfo(IntPtr l) {
		try {
			RoomSystem self=(RoomSystem)checkSelf(l);
			System.Byte a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.GetRoomInfo(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SearchRoom(IntPtr l) {
		try {
			RoomSystem self=(RoomSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.SearchRoom(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,RoomSystem.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"RoomSystem");
		addMember(l,GetRoomCount);
		addMember(l,GetRoomInfo);
		addMember(l,SearchRoom);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(RoomSystem));
	}
}
