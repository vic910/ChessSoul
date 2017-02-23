using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

using UnityEngine;
using UnityEditor;

namespace Groot.Res
{
	class ResUpdatePkg
	{
		public const String OUTPUT_ROOT = "weiqi_build/update_pkg";
		public const String OUTPUT_TMP = "Tmp/update_pkg/res";
		//private const BuildAssetBundleOptions BUILD_OPTIONS = BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;
		private const BuildAssetBundleOptions BUILD_OPTIONS = BuildAssetBundleOptions.ChunkBasedCompression;



#if UNITY_ANDROID
		private const BuildTarget BUILD_TARGET = BuildTarget.Android;
#elif UNITY_IOS
		private const BuildTarget BUILD_TARGET = BuildTarget.iOS;
#elif UNITY_STANDALONE_WIN
		private const BuildTarget BUILD_TARGET = BuildTarget.StandaloneWindows;
#endif

		/// <summary>
		/// 资源版本号
		/// </summary>
		public Int32 Version;

		/// <summary>
		/// 当前的res_config
		/// </summary>
		private ResConfigFile m_res_config;

		/// <summary>
		/// 上一个版本
		/// </summary>
		private ResUpdatePkg m_pre_version;

		/// <summary>
		/// 加载一个最新的package
		/// </summary>
		/// <returns></returns>
		public static ResUpdatePkg LoadLastPkg()
		{
			var resConfigPath = String.Concat( OUTPUT_ROOT, "/res_config.grt" );
			ResUpdatePkg pkg = new ResUpdatePkg();

			if( !File.Exists( resConfigPath ) )
			{
				Log.Info( "没有更新包!" );
				return null;
			}
			else
			{
				// 直接读取txt
				pkg.m_res_config = ResConfigFile.Create( String.Concat( OUTPUT_ROOT, "/res_config.txt" ), false );
				pkg.Version = pkg.m_res_config.ResVersion;
				if( pkg.m_res_config == null )
				{
					Log.Error( "读取最新更新包失败!" );
					return null;
				}
			}
			return pkg;
		}

		public void CopyResToStreammingAssets()
		{
			FileUtil.DeleteFileOrDirectory( "Assets/StreamingAssets/res" );
			AssetDatabase.Refresh();
			FileHelper.CreateDirectory( "Assets/StreamingAssets/res" );
			m_res_config.TraverseAssets(
				_info => {
					String dest = String.Concat( "Assets/StreamingAssets/res/", _info.Name );
					FileHelper.CreateDirectoryByFilePath( dest );
					File.Copy( String.Concat( OUTPUT_ROOT, "/", _info.Version.ToString(), "/", _info.Name ), dest, true );
				} );

			File.Copy( String.Concat( OUTPUT_ROOT, "/res_config.grt" ), "Assets/StreamingAssets/res/res_config.grt", true );

			AssetDatabase.Refresh();
		}

		public static void BuildNew( ResUpdatePkg _old_pkg )
		{
			//AssetBundle bundle = AssetBundle.LoadFromFile( String.Concat( OUTPUT_TMP, "/data/global_configtest" ) );
			//TextAsset g = bundle.LoadAsset<TextAsset>( "global_configtest" );
			////TextAsset r = bundle.LoadAsset<TextAsset>( "rookie_config" );
			//bundle.Unload( true );
			////AssetBundle bundle = AssetBundle.LoadFromFile( String.Concat( OUTPUT_TMP, "/ui/ui_main.ui" ) );
			//return;
			FileHelper.DeleteDirectory( OUTPUT_TMP, true );
			FileHelper.CreateDirectory( OUTPUT_TMP );

			// 打包Assetbundle
			BuildPipeline.BuildAssetBundles( OUTPUT_TMP, BUILD_OPTIONS, BUILD_TARGET );
			AssetBundle assetbundleManifest = null;
			assetbundleManifest = AssetBundle.LoadFromFile( String.Concat( OUTPUT_TMP, "/res" ) );
			AssetBundleManifest manifest = assetbundleManifest.LoadAsset<AssetBundleManifest>( "AssetBundleManifest" );

			String[] allAssetBundles = manifest.GetAllAssetBundles();

			// 添加Assetbundle到config
			var newPkg = new ResUpdatePkg {Version = _old_pkg == null ? 1 : _old_pkg.Version + 1, m_pre_version = _old_pkg};
			newPkg.m_res_config = ResConfigFile.Create( String.Concat( OUTPUT_TMP, "/res_config.txt" ), true );

			foreach( String assetBundle in allAssetBundles )
				newPkg.m_res_config.BuidlerAndResInfo( OUTPUT_TMP, assetBundle, newPkg.Version );

			assetbundleManifest.Unload( true );

			File.Move( String.Concat( OUTPUT_TMP, "/res" ), String.Concat( OUTPUT_TMP, "/res.grt" ) );
			newPkg.m_res_config.BuidlerAndResInfo( OUTPUT_TMP, "res.grt", newPkg.Version );

			// 打包lua文件
			GrtFileEditor.Pack( "Assets/res/lua", OUTPUT_TMP, "scripts/ui.grt" );
			newPkg.m_res_config.BuidlerAndResInfo( OUTPUT_TMP, "scripts/ui.grt", newPkg.Version );

			Boolean hasNewRes = true;
			// 处理res_config
			if( newPkg.m_pre_version != null )
				hasNewRes = newPkg.m_res_config.BuilderMergeOld( _old_pkg.m_res_config );

			if( !hasNewRes )
			{
				EditorUtility.DisplayDialog( "", "资源无更新!", "确定" );
				return;
			}

			// 保存到版本目录
			String version_root = String.Concat( OUTPUT_ROOT, "/", newPkg.Version.ToString() );
			FileHelper.CreateDirectory( version_root );

			// 临时保存到Assets下以便打包
			newPkg.m_res_config.SaveAs( "Assets/res_config.txt", true );
			File.Copy( "Assets/res_config.txt", String.Concat( version_root, "/res_config.txt" ), true );
			File.Copy( "Assets/res_config.txt", String.Concat( OUTPUT_ROOT, "/res_config.txt" ), true );

			AssetDatabase.Refresh();
			BuildPipeline.BuildAssetBundles(
				OUTPUT_ROOT,
				new AssetBundleBuild[] { new AssetBundleBuild() { assetBundleName = "res_config.grt", assetNames = new[] { "Assets/res_config.txt" } } },
				BUILD_OPTIONS,
				BUILD_TARGET
				);

			AssetDatabase.Refresh();
			File.Copy( String.Concat( OUTPUT_ROOT, "/res_config.grt" ), String.Concat( version_root, "/res_config.grt" ), true );


			// 保存的根目录
			//new_pkg.m_res_config.SaveAs( String.Concat( OUTPUT_ROOT, "/res_config" ), true );

			FileHelper.CreateDirectory( version_root );
			// copy main assetbundle manifest
			//File.Copy( String.Concat( OUTPUT_TMP, "/res" ), String.Concat( version_root, "/res" ), true );

			newPkg.m_res_config.TraverseAssets(
				_info => {
					if( _info.Version != newPkg.Version )
						return;
					String dest = String.Concat(version_root, "/", _info.Name );
					FileHelper.CreateDirectoryByFilePath( dest );
					File.Copy( String.Concat( OUTPUT_TMP, "/", _info.Name ), dest, true );
				} );

			// 删除指定Assetbundle打包后生成的信息文件（没用）
			File.Delete( String.Concat( OUTPUT_ROOT, "/update_pkg" ) );
			File.Delete( String.Concat( OUTPUT_ROOT, "/update_pkg.manifest" ) );

			File.Delete( "Assets/res_config.txt" );
			File.Delete( "Assets/res_config.grt" );

			AssetDatabase.Refresh();


			EditorUtility.DisplayDialog( "成功", "打包完成!", "确定" );
		}
	}
}