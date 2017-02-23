// The MIT License (MIT)

// Copyright 2015 Siney/Pangweiwei siney@yeah.net
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Runtime.InteropServices.ComTypes;
using UnityEngine.UI;

namespace SLua
{
	using System.Collections.Generic;
	using System;

	public class CustomExport
	{
#if GROOT_SLUA_EXTENSION
		public static void OnAddExporClass( LuaCodeGen.ExportGenericDelegate _add, LuaCodeGen.ExportType _type )
		{
			// UnityEngine
			if( _type == LuaCodeGen.ExportType.UnityEngine )
			{
				_add( typeof( UnityEngine.GameObject ), null );
				_add( typeof( UnityEngine.Transform ), null );
				_add( typeof( UnityEngine.Vector2 ), null );
				_add( typeof( UnityEngine.Vector3 ), null );
				_add( typeof( UnityEngine.Vector4 ), null );
				_add( typeof( UnityEngine.Quaternion ), null );
				_add( typeof( UnityEngine.Matrix4x4 ), null );
				_add( typeof( UnityEngine.Color ), null );

			}
			else if( _type == LuaCodeGen.ExportType.UnityEngineUI )
			{
				_add( typeof( UnityEngine.Events.UnityEvent ), null );
				_add( typeof( UnityEngine.UI.Button.ButtonClickedEvent ), null );
				_add( typeof( UnityEngine.UI.Text ), null );
				_add( typeof( UnityEngine.UI.Image ), null );
				_add( typeof( UnityEngine.UI.Button ), null );
			}
			else if( _type == LuaCodeGen.ExportType.DotNET )
			{
				// .NET
				_add( typeof( System.Int32 ), null );
				_add( typeof( System.String ), null );
				//_add( typeof( System.Action ), null );
			}
			else if( _type == LuaCodeGen.ExportType.Custom )
			{

			}
		}
#endif

		public static void OnGetAssemblyToGenerateExtensionMethod( out List<string> list )
		{
			list = new List<string> {
				"Assembly-CSharp",
			};
		}

		public static void OnAddCustomClass( LuaCodeGen.ExportGenericDelegate add )
		{
			// below lines only used for demostrate how to add custom class to export, can be delete on your app

			add( typeof( System.Func<int> ), null );
			add( typeof( System.Action<int, string> ), null );
			add( typeof( System.Action<int, Dictionary<int, object>> ), null );
			add( typeof( List<int> ), "ListInt" );
			add( typeof( Dictionary<int, string> ), "DictIntStr" );
			add( typeof( string ), "String" );

			// add your custom class here
			// add( type, typename)
			// type is what you want to export
			// typename used for simplify generic type name or rename, like List<int> named to "ListInt", if not a generic type keep typename as null or rename as new type name
		}

		public static void OnAddCustomAssembly( ref List<string> list )
		{
			// add your custom assembly here
			// you can build a dll for 3rd library like ngui titled assembly name "NGUI", put it in Assets folder
			// add its name into list, slua will generate all exported interface automatically for you

			//list.Add("NGUI");
		}

		public static HashSet<string> OnAddCustomNamespace()
		{
			return new HashSet<string>
			{
				//"NLuaTest.Mock"
			};
		}

		// if uselist return a white list, don't check noUseList(black list) again
		public static void OnGetUseList( out List<string> list )
		{
			list = new List<string>
			{
				"UnityEngine.GameObject",
			};
		}

		public static List<string> FunctionFilterList = new List<string>()
		{
			"UIWidget.showHandles",
			"UIWidget.showHandlesWithMoveTool",
		};
		// black list if white list not given
		public static void OnGetNoUseList( out List<string> list )
		{
			list = new List<string>
			{
				"HideInInspector",
				"ExecuteInEditMode",
				"AddComponentMenu",
				"ContextMenu",
				"RequireComponent",
				"DisallowMultipleComponent",
				"SerializeField",
				"AssemblyIsEditorAssembly",
				"Attribute",
				"Types",
				"UnitySurrogateSelector",
				"TrackedReference",
				"TypeInferenceRules",
				"FFTWindow",
				"RPC",
				"Network",
				"MasterServer",
				"BitStream",
				"HostData",
				"ConnectionTesterStatus",
				"GUI",
				"EventType",
				"EventModifiers",
				"FontStyle",
				"TextAlignment",
				"TextEditor",
				"TextEditorDblClickSnapping",
				"TextGenerator",
				"TextClipping",
				"Gizmos",
				"ADBannerView",
				"ADInterstitialAd",
				"Android",
				"Tizen",
				"jvalue",
				"iPhone",
				"iOS",
				"Windows",
				"CalendarIdentifier",
				"CalendarUnit",
				"CalendarUnit",
				"ClusterInput",
				"FullScreenMovieControlMode",
				"FullScreenMovieScalingMode",
				"Handheld",
				"LocalNotification",
				"NotificationServices",
				"RemoteNotificationType",
				"RemoteNotification",
				"SamsungTV",
				"TextureCompressionQuality",
				"TouchScreenKeyboardType",
				"TouchScreenKeyboard",
				"MovieTexture",
				"UnityEngineInternal",
				"Terrain",
				"Tree",
				"SplatPrototype",
				"DetailPrototype",
				"DetailRenderMode",
				"MeshSubsetCombineUtility",
				"AOT",
				"Social",
				"Enumerator",
				"SendMouseEvents",
				"Cursor",
				"Flash",
				"ActionScript",
				"OnRequestRebuild",
				"Ping",
				"ShaderVariantCollection",
				"SimpleJson.Reflection",
				"CoroutineTween",
				"GraphicRebuildTracker",
				"Advertisements",
				"UnityEditor",
				"WSA",
				"EventProvider",
				"Apple",
				"ClusterInput",
				"Motion",
			};
		}
	}
}