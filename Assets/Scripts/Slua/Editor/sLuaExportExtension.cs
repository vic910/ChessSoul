using System;
using System.Collections.Generic;
using System.Reflection;
using SLua;
using UnityEngine;
using UnityEditor;

namespace Assets.Slua.Editor
{
	class SLuaExportExtension
	{
		/*private static Dictionary<String, List<String>> s_import_whitelist = new Dictionary<String, List<String>>() {
			{
				"UnityEngine", new List<String>() {
				"UnityEngine.GameObject",
				"UnityEngine.Animator"}
			},
			{
				"UnityEngine.UI", new List<String>() {
				"UnityEngine.UI.Text",
				"UnityEngine.UI.Button"}
			},
			{
				"", new List<String>() {
				"UnityEngine.UI.Text",
				"UnityEngine.UI.Button"}
			}
		};

		private static bool s_is_compiling
		{
			get
			{
				if( EditorApplication.isCompiling )
				{
					Debug.LogError( "Unity Editor is compiling, please wait." );
				}
				return EditorApplication.isCompiling;
			}
		}

		[MenuItem( "SLua/Groot/Make" )]
		static public void _generateAll()
		{
			if( s_is_compiling )
				return;

			_generateLua( "UnityEngine" );
			AssetDatabase.Refresh();
		}

		private static void _generateLua( String _name )
		{
			Assembly assembly = Assembly.Load( _name );
			Type[] types = assembly.GetExportedTypes();

			List<Type> exports = new List<Type>();
			var white_list = s_import_whitelist[_name];
			String path = SLuaSetting.Instance.UnityEngineGeneratePath +"Unity/";

			foreach( var export_item in white_list )
			{
				Boolean found = false;
				foreach( var type in types )
				{
					if( type.ToString() == export_item && Generate( type, path ) )
					{
						exports.Add( type );
						found = true;
						break;
					}
				}
				if( !found )
					Debug.LogErrorFormat( "[sLua.LuaCodeGen.Generate]: {0} not contain in unity engine!", export_item );
			}

			//UnityEngine.LightProbeProxyVolume+RefreshMode
			GenerateBind( exports, "BindUnity", 0, path );
			if( autoRefresh )
				AssetDatabase.Refresh();
			Debug.Log( "Generate engine interface finished" );

		}*/
	}
}