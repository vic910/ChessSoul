using System;
using UnityEngine;
using SLua;
using Weiqi.UI;


[CustomLuaClassAttribute]
public static class UnityLuaUtils
{
	public static void SetPos( Transform _transform, float _x, float _y, float _z )
	{
		_transform.position = new Vector3( _x, _y, _z );
	}

	public static void GetPos( Transform _transform, out float _x, out float _y, out float _z )
	{
		var pos = _transform.position;
		_x = pos.x;
		_y = pos.y;
		_z = pos.z;
	}
	public static Vector3 GetPos( Transform _transform )
	{
		return _transform.position;
	}

	public static void Invoke( Action _action )
	{
		_action();
	}

	public static void HideUI( String _name )
	{
		UIManager.Instance.HideUI( _name );
	}

	public static void ShowUI( String _name )
	{
		UIManager.Instance.ShowUI( _name );
	}
}
