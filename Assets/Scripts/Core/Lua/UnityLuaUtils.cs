using System;
using Groot;
using Groot.Network;
using LuaInterface;
using UnityEngine;
using SLua;
using Weiqi;
using Weiqi.UI;


[CustomLuaClassAttribute]
public static class UnityLuaUtils
{
    public static void SetPos(Transform _transform, float _x, float _y, float _z)
    {
        _transform.position = new Vector3(_x, _y, _z);
    }

    public static void GetPos(Transform _transform, out float _x, out float _y, out float _z)
    {
        var pos = _transform.position;
        _x = pos.x;
        _y = pos.y;
        _z = pos.z;
    }

    public static Vector3 GetPos(Transform _transform)
    {
        return _transform.position;
    }

    //public static void Invoke( Action _action )
    //{
    //	_action();
    //}

    public static void HideUI( String _name )
    {
        UIManager.Instance.HideUI(_name);
    }

    public static void ShowUI( String _name, params object[] _args )
    {
        UIManager.Instance.ShowUI( _name, _args );
    }

    public static void ShowSingleMsgBox(string _tip, string _button_tip, Action _button_action, Action _close_action)
    {
        UI_MessageBox.Show(_tip, _button_tip, _button_action, _close_action);
    }

    public static void ShowSelectMsgBox(string _tip, string _button1_tip, string _button2_tip, Action _button1_action, Action _button2_action, Action _close_action)
    {
        UI_MessageBox.Show(_tip, _button1_tip, _button2_tip, _button1_action, _button2_action, _close_action);
    }

    public static string GetLocaleString(string _key)
    {
        return Locale.Instance[_key];
    }

    public static bool StringConvertToBool(string str)
    {
        return Convert.ToBoolean(str);
    }

	public static void SendMessage( MessageBase _msg )
	{
		NetManager.Instance.SendMsg( _msg );
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    [StaticExport]
    public static int Test(IntPtr l)
    {
        LuaObject.pushValue(l, true);
        LuaDLL.lua_pushstring(l, "xiaoming");
        LuaDLL.lua_pushstring(l, "hanmeimei");
        LuaDLL.lua_pushinteger(l, 2);
        return 4;
    }
}
