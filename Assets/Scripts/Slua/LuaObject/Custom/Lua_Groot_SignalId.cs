using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Groot_SignalId : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Groot.SignalId");
		addMember(l,0,"NetworkState_EnterConnected");
		addMember(l,1,"Login_Success");
		addMember(l,2,"Login_ForceLogin");
		addMember(l,3,"Chat_ReceiveChat");
		addMember(l,4,"Item_Update");
		addMember(l,5,"Money_Update");
		addMember(l,6,"Gold_Update");
		addMember(l,7,"Liveness_Update");
		addMember(l,8,"ShoppingCar_Update");
		addMember(l,9,"BuyFail_Update");
		addMember(l,10,"Test");
		LuaDLL.lua_pop(l, 1);
	}
}
