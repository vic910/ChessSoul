using System;
using System.Net;
using UnityEngine;

using Utility.FSM;

public class WeiqiApp
{
	public const String PROJECT_NAME = "棋魂";

#if !GROOT_ASSETBUNDLE_SIMULATION && UNITY_EDITOR
	public const String CONFIG_ROOT_PATH = "Assets/res/data";
	public const String LANGUAGES_ROOT_PATH = "Assets/res/data/language";
	public const String UI_PREFAB_ROOT_PATH = "Assets/res/ui/prefab";
	public const String LUA_ROOT = "Assets/res/lua";
#else
	public const String CONFIG_ROOT_PATH = "data";
	public const String LANGUAGES_ROOT_PATH = "data/language";
#endif


	public static String ResServerUrl
	{
		get { return "http://127.0.0.1/dev/"; }
	}

	public static Boolean DevMode
	{
		get
		{
			return true;
		}
	}
}
