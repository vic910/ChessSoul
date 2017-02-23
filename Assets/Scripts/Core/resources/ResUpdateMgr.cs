using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Utility;
using Weiqi;
using Weiqi.UI;

namespace Groot.Res
{
	class ResUpdateMgr : UnitySingleton<ResUpdateMgr>
	{
		/// <summary>
		///     apk/ipa包内的ResConfig文件
		/// </summary>
		private ResConfigFile mInAppConfig;

		/// <summary>
		///     本地存储的ResConfig文件
		/// </summary>
		private ResConfigFile mLocalConfig;

		/// <summary>
		///     服务器端下载的ResConfig文件
		/// </summary>
		private ResConfigFile mServerConfig;

		/// <summary>
		///     待更新的文件列表
		/// </summary>
		private LinkedList<ResInfo> mUpdateList;

		/// <summary>
		///     更新的总大小
		/// </summary>
		private Int64 mUpdateTotalSize;

		/// <summary>
		///     已经下载完成的更新大小
		/// </summary>
		private Int64 mDownloadedSize;

		/// <summary>
		///     更新完成回调
		///     返回值为False表示没有需要更新，范围值为true表示有文件被下载
		/// </summary>
		private Action<Boolean> mOnUpdateCompleted;

		public void UpdateRes( Action<Boolean> _onCompleted )
		{
			Log.Debug( "开始资源更新准备工作" );
			mOnUpdateCompleted = _onCompleted;
			var appConfigAb = AssetBundle.LoadFromFile(
				ResInfo.GetSystemApiLoadPath( "res_config.grt", AssetLocation.StreamingPath ) );
			if( appConfigAb == null )
			{
				Log.Critical( eLogType.Resources, "加载 In App res config 失败！！！" );
				appConfigAb.Unload( true );
				return;
			}

			mInAppConfig = ResConfigFile.Create( appConfigAb.LoadAsset<TextAsset>( "res_config" ).bytes );
			if( appConfigAb == null )
			{
				Log.Critical( eLogType.Resources, "解析 In App res config 失败！！！" );
				appConfigAb.Unload( true );
				return;
			}
			appConfigAb.Unload( true );
			mLocalConfig = ResConfigFile.Create( ResInfo.GetSystemApiLoadPath( "res_config", AssetLocation.PersistentPath ), true );

			if( mLocalConfig == null )
			{
				Log.Critical( eLogType.Resources, "创建 Local res config 失败！！！" );
				return;
			}

			StartCoroutine( checkUpdate() );
		}

		private IEnumerator checkUpdate()
		{
			Log.Debug( "开始检查资源更新" );
			using( UnityWebRequest uwr = UnityWebRequest.GetAssetBundle( ResUtility.ResConfigDownloadUrl ) )
			{
				yield return uwr.Send();
				if( uwr.isError )
				{
					Debug.Log( uwr.error );
					showCheckUpdateFailTip();
					yield break;
				}
				else
				{
					AssetBundle srvConfigAb = ( (DownloadHandlerAssetBundle) uwr.downloadHandler ).assetBundle;
					if( srvConfigAb == null )
					{
						showCheckUpdateFailTip();
						yield break;
					}
					TextAsset asset = srvConfigAb.LoadAsset<TextAsset>( "res_config" );
					if( asset == null )
					{
						showCheckUpdateFailTip();
						srvConfigAb.Unload( true );
						yield break;
					}
					mServerConfig = ResConfigFile.Create( asset.bytes );
					srvConfigAb.Unload( true );
					if( mServerConfig == null )
					{
						showCheckUpdateFailTip();
						yield break;
					}
				}
			}

			// 处理更新列表
			Boolean localResConfigUpdated;
			mUpdateTotalSize = mLocalConfig.GetUpdateList( mInAppConfig, mServerConfig, out mUpdateList, out localResConfigUpdated );
			if( mUpdateTotalSize == 0 )
				mOnUpdateCompleted( localResConfigUpdated );
			else
				showUpdateTip();
		}

		private IEnumerator downloadRes()
		{
			Int32 retryCount = 0;
			ResInfo firstErrorNode = null;
			LinkedListNode<ResInfo> node = mUpdateList.First;
			while( node != null )
			{
				if( firstErrorNode != null && node.Value == firstErrorNode )
				{
					if( retryCount++ > 2 )
					{
						showDownloadFailTip();
						yield break;
					}
					firstErrorNode = null;
				}
				String downloadUrl = node.Value.DownloadUrl; // 这个属性每次都会新创建
				Log.Info( eLogType.Resources, "开始下载: {0}", downloadUrl );
				using( UnityWebRequest www = UnityWebRequest.Get( downloadUrl ) )
				{
					yield return www.Send();
					if( !www.isError )
					{
						if( !saveResourceFile( node.Value, www.downloadHandler.data ) )
						{
							mUpdateList.AddLast( node.Value );
							if( firstErrorNode == null )
								firstErrorNode = node.Value;
						}
						else
						{
							//更新下载进度
							mDownloadedSize += www.downloadHandler.data.Length;
							//Single progress = (float)downloadsize / (float)m_update_total_size;
							//SignalSystem.FireSignal( SignalId.AssetUpdateLoading_UpdateProgress, progress, downloadsize, m_update_total_size );
						}
					}
					else
					{
						Log.Error( eLogType.Resources, "下载文件失败: {0}\n错误信息: {1}", node.Value.Name, www.error );
						mUpdateList.AddLast( node.Value );
						if( firstErrorNode == null )
							firstErrorNode = node.Value;
					}
				}
				var tmp = node.Next;
				mUpdateList.Remove( node );
				node = tmp;
			}
			mOnUpdateCompleted( true );
		}

		/// <summary>
		///     保存资源文件
		/// </summary>
		/// <param name="_data"></param>
		/// <param name="_bytes"></param>
		/// <returns></returns>
		private Boolean saveResourceFile( ResInfo _data, Byte[] _bytes )
		{
			try
			{
				String savePath = _data.SavePath;
				FileHelper.CreateDirectoryByFilePath( savePath );
				//Log.Debug( eLogType.Resources, "保存文件路径: {0}", _data.SavePath );
				FileStream fs = File.Create( savePath );
				fs.Write( _bytes, 0, _bytes.Length );
				fs.Flush();
				fs.Close();
#if UNITY_IPHONE
				iPhone.SetNoBackupFlag( savePath );
#endif
				//下载成功，记录文件位置
				ResInfo newRes = _data.Clone();
				newRes.Location = AssetLocation.PersistentPath;
				mLocalConfig.AddOrUpdateResInfo( newRes );
				mLocalConfig.Save();
				Log.Debug( eLogType.Resources, "成功下载并保存Asset: {0} update list 剩余: {1}", savePath, mUpdateList.Count );
			}
			catch( Exception ex )
			{
				Log.Error( eLogType.Resources, "下载文件保存失败: {0}\n 错误信息: {1}", _data.Name, ex.Message );
				return false;
			}
			return true;
		}

		#region 显示提示

		/// <summary>
		///     显示检查更新失败提示
		/// </summary>
		private void showCheckUpdateFailTip()
		{
			UI_MessageBox.Show( Locale.Instance["Download@Error"], Locale.Instance["Common@Confirm"], () => { StartCoroutine( checkUpdate() ); } );
		}

		/// <summary>
		///     显示更新提示
		/// </summary>
		private void showUpdateTip()
		{
			String tip = String.Format( Locale.Instance["Download@NeedUpdate"], (float)mUpdateTotalSize / 1024f / 1024f );
			UI_MessageBox.Show( tip, Locale.Instance["Common@Confirm"], Locale.Instance["Common@Cancel"], () => { StartCoroutine( downloadRes() ); } );
		}

		/// <summary>
		///     更新下载失败提示(包括部分失败)
		/// </summary>
		private void showDownloadFailTip()
		{
			String tip = String.Format( Locale.Instance["Download@Continue"], (float)( mUpdateTotalSize - mDownloadedSize ) / 1024f / 1024f );
			UI_MessageBox.Show( tip, Locale.Instance["Common@Confirm"], Locale.Instance["Common@Cancel"], () => { StartCoroutine( downloadRes() ); } );
		}

		#endregion
	}
}