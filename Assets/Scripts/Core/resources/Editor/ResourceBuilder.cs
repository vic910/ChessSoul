using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Utility;


namespace Groot.Res
{
	class ResourceBuilder
	{
		[MenuItem( "Groot/构建/构建资源更新包" )]
		public static void Build()
		{
			var last = ResUpdatePkg.LoadLastPkg();
			ResUpdatePkg.BuildNew( last );
		}

		[MenuItem( "Groot/构建/复制最新资源包到StreammingAssets目录" )]
		public static void CopyNewestResToStreammingAssets()
		{
			var newest_pkg = ResUpdatePkg.LoadLastPkg();

			if( newest_pkg == null )
				EditorUtility.DisplayDialog( "提示", "没有找到资源包，请构建资源更新包后重试!", "确定" );

			newest_pkg.CopyResToStreammingAssets();

			EditorUtility.DisplayDialog( "提示", "复制资源包完成!", "确定" );
		}
	}
}