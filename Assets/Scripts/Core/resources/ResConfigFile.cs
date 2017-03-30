using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Linq;
using Utility;


namespace Groot.Res
{
	/// <summary>
	/// 资源配置文件
	/// </summary>
	public class ResConfigFile
	{

		private ResConfigFile() { }

		private List<ResInfo> m_assets = new List<ResInfo>();
		private Dictionary<String, ResInfo> m_asset_dictionary;
		private String m_file_path = String.Empty;

		/// <summary>
		/// 当前ResConfig的版本号
		/// </summary>
		public Int32 ResVersion { get; private set; }

		public void Clear()
		{
			m_assets.Clear();
			if( null != m_asset_dictionary )
				m_asset_dictionary.Clear();
		}

		private Dictionary<String, ResInfo> ResDictionary
		{
			get
			{
				if( m_asset_dictionary != null )
					return m_asset_dictionary;

				m_asset_dictionary = new Dictionary<String, ResInfo>( StringComparer.OrdinalIgnoreCase );
				foreach( ResInfo asset_info in m_assets )
				{
					m_asset_dictionary.Add( asset_info.Name, asset_info );
				}
				return m_asset_dictionary;
			}
		}

		public String FilePath
		{
			get { return m_file_path; }
		}

		public void TraverseAssets( Action<ResInfo> _action )
		{
			foreach( ResInfo asset_info in m_assets )
			{
				_action( asset_info );
			}
		}

		/// <summary>
		/// 尝试获取AssetInfo
		/// </summary>
		/// <param name="_name">Asset名称</param>
		/// <param name="_res_info">返回的AssetInfo</param>
		/// <returns></returns>
		public Boolean TryGetAssetInfo( String _name, out ResInfo _res_info )
		{
			return ResDictionary.TryGetValue( _name, out _res_info );
		}

		/// <summary>
		/// 是否存在Asset
		/// </summary>
		/// <param name="_name">Asset名称</param>
		/// <returns></returns>
		public Boolean ContainAsset( String _name )
		{
			return ResDictionary.ContainsKey( _name );
		}

		/// <summary>
		/// 添加或更新资源
		/// </summary>
		/// <param name="_new_res"></param>
		public void AddOrUpdateResInfo( ResInfo _new_res )
		{
			if( ResDictionary.ContainsKey( _new_res.Name ) )
			{
				for( int i = 0; i < m_assets.Count; i++ )
				{
					if( m_assets[i].Name.Equals( _new_res.Name, StringComparison.OrdinalIgnoreCase ) )
					{
						m_assets[i] = _new_res;
						ResDictionary[_new_res.Name] = _new_res;
						break;
					}
				}
			}
			else
			{
				ResDictionary.Add( _new_res.Name, _new_res );
				m_assets.Add( _new_res );
			}
		}

		/// <summary>
		/// 根据AppResConfig进行更新
		/// </summary>
		public void UpdateByAppResConfig( ResConfigFile _appConfig )
		{
			if( _appConfig.ResVersion < ResVersion )
				return;
			for( Int32 i = 0; i < _appConfig.m_assets.Count; ++i )
			{
				ResInfo localResInfo ;
				var appResInfo = _appConfig.m_assets[i];
				if( ResDictionary.TryGetValue( appResInfo.Name, out localResInfo ) )
				{
					if( appResInfo.Version > localResInfo.Version )
					{
						appResInfo.CopyTo( localResInfo );
						localResInfo.Location = AssetLocation.StreamingPath;
					}
				}
				else
				{
					localResInfo = appResInfo.Clone();
					localResInfo.Location = AssetLocation.StreamingPath;
					m_assets.Add( localResInfo );
					ResDictionary.Add( localResInfo.Name, localResInfo );
				}
			}
			Save();
		}

		/// <summary>
		/// 获取更新列表
		/// </summary>
		/// <param name="_appConfig"></param>
		/// <param name="_serverConfig"></param>
		/// <param name="_updateList"></param>
		/// <returns>更新文件大小(字节)</returns>
		public Int64 GetUpdateList( ResConfigFile _appConfig, ResConfigFile _serverConfig, out LinkedList<ResInfo> _updateList, out Boolean _localConfigUpdated )
		{
			_localConfigUpdated = false;
			Int64 updateSize = 0;
			_updateList = new LinkedList<ResInfo>();
			for( Int32 i = 0; i < _serverConfig.m_assets.Count; ++i )
			{
				var srvResInfo = _serverConfig.m_assets[i];
				ResInfo localResInfo;
				ResDictionary.TryGetValue( srvResInfo.Name, out localResInfo );
				ResInfo appResInfo;
				_appConfig.ResDictionary.TryGetValue( srvResInfo.Name, out appResInfo );

				// 无需更新
				if( localResInfo != null && !localResInfo.NeedUpdate( srvResInfo ) )
					continue;

				if( appResInfo != null && !appResInfo.NeedUpdate( srvResInfo ) )
				{
					localResInfo = appResInfo.Clone();
					localResInfo.Location = AssetLocation.StreamingPath;
					AddOrUpdateResInfo( localResInfo );
					Save();
					_localConfigUpdated = true;
				}
				else
				{
					localResInfo = srvResInfo.Clone();
					localResInfo.Location = AssetLocation.PersistentPath;
					_updateList.AddLast( localResInfo );
					updateSize += localResInfo.Size;
				}
			}
			return updateSize;
		}


		/// <summary>
		/// 从AssetsConfig中删除资源
		/// </summary>
		/// <param name="_srv_config"></param>
		/// <returns>是否有删除到东西</returns>
		public Boolean DeleteRes( ResConfigFile _srv_config )
		{
			Boolean found = false;
			for( int i = 0; i < m_assets.Count; i++ )
			{
				if( !_srv_config.ContainAsset( m_assets[i].Name ) )
				{
					m_assets[i] = null;
					if( m_asset_dictionary != null ) // 直接重建好了
						m_asset_dictionary = null;
					found = true;
				}
			}
			if( !found )
				return false;
			m_assets.RemoveNull();
			return true;
		}

		public void Save()
		{
			if( String.IsNullOrEmpty( m_file_path ) )
				throw new AssetException( "No file path set to this 'ResConfigFile', use SaveAs( String _file_path ) instead!" );
			// 强制覆盖原文件
			_serialize( m_file_path, FileMode.Create );
		}


		/// <summary>
		/// 使用字节流创建文件
		/// </summary>
		/// <param name="_bytes"></param>
		public static ResConfigFile Create( Byte[] _bytes )
		{
			return _deserialize( String.Empty, _bytes, false );
		}

		/// <summary>
		/// 从文本中创建, 如果文件不存在, 创建文件
		/// </summary>
		/// <param name="_path">文件路径</param>
		/// <param name="_create_if_err"></param>
		public static ResConfigFile Create( String _path, Boolean _create_if_err )
		{
			if( !File.Exists( _path ) )
			{
				if( _create_if_err )
				{
					Log.Info( "ResConfigFile not exsit, create new one!({0})", _path );
					return new ResConfigFile() { m_file_path = _path };
				}
				Log.Warning( "ResConfigFile [{0}] not exsit!", _path );
				return null;
			}
			Byte[] bytes = FileHelper.ReadAllBytesFromFile( _path );
			return _deserialize( _path, bytes, _create_if_err );
		}

		/// <summary>
		/// 反序列化ResouceConfigFile
		/// </summary>
		/// <param name="_file_path"></param>
		/// <param name="_bytes"></param>
		/// <param name="_create_if_err"></param>
		/// <returns></returns>
		private static ResConfigFile _deserialize( String _file_path, Byte[] _bytes, Boolean _create_if_err )
		{
			ResConfigFile res_config = new ResConfigFile() {m_file_path = _file_path};
			try
			{
				using( MemoryStream memory_stream = new MemoryStream( _bytes ) )
				{
					using( StreamReader reader = new StreamReader( memory_stream, Encoding.UTF8 ) )
					{
						if( !Equals( Encoding.UTF8, reader.CurrentEncoding ) )
							throw new ConfigFileNotUTF8( "AssetsConfig file with {0} encoding not UTF-8!", reader.CurrentEncoding.EncodingName );

						if( reader.EndOfStream )
							return res_config;
						Int32 line_count = 0;
						while( !reader.EndOfStream )
						{
							String line = reader.ReadLine();
							if( String.IsNullOrEmpty( line ) )
								continue;
							line_count++;
							String[] parts = line.Split( '\t' );
//#if UNITY_EDITOR
//							if( parts.Length != 8 && parts.Length != 9 )
//#else
//							if( parts.Length != 8 )
//#endif
//								throw new Exception( "Assetsinfo size error" );

							String name = parts[0];
							Int64 size = Int64.Parse( parts[1], NumberStyles.None );
							Int32 version = Int32.Parse( parts[2] );
							AssetLocation location = Configuration.ParseEnum<AssetLocation>( parts[3] );
							String sha1 = parts[4];
							String md5 = parts[5];
							UInt32 crc32 = UInt32.Parse( parts[6], NumberStyles.None );
							ResourceType resource_type = Configuration.ParseEnum<ResourceType>( parts[7] );
							ResInfo res = new ResInfo( name, size, version, sha1, md5, crc32, location, resource_type );
#if UNITY_EDITOR
							if( parts.Length == 9 ) res.AssetBundleTypeTreeHash = parts[8];
#endif
							// 处理版本号
							if( version > res_config.ResVersion ) res_config.ResVersion = version;
							res_config.m_assets.Add( res );
						}
					}
				}
			}
			catch( Exception e )
			{
				if( _create_if_err )
				{
					Log.Error( "解析 AssetsConfig 文件失败!  创建新文件" );
					return new ResConfigFile() { m_file_path = _file_path };
				}
				Log.Error( "解析 AssetsConfig 文件失败!\tMessage: {0}\nStackTrace: {1} {2}", e.Message, e.StackTrace, e.ToString() );
				res_config = null;
			}
			return res_config;
		}

#if UNITY_EDITOR
		private void _serialize( String _path, FileMode _file_mode, Boolean _write_typetree = false )
#else
		private void _serialize( String _path, FileMode _file_mode )
#endif
		{
			using( FileStream file_stream = new FileStream( _path, _file_mode ) )
			{
				StringBuilder stringbuilder = new StringBuilder( 102400 );
				foreach( ResInfo file in m_assets )
				{
					stringbuilder.AppendFormat( "{0}\t", file.Name );
					stringbuilder.AppendFormat( "{0}\t", file.Size );
					stringbuilder.AppendFormat( "{0}\t", file.Version );
					stringbuilder.AppendFormat( "{0}\t", (Int32)file.Location );
					stringbuilder.AppendFormat( "{0}\t", file.Sha1 );
					stringbuilder.AppendFormat( "{0}\t", file.Md5 );
					stringbuilder.AppendFormat( "{0}\t", file.Crc32 );
					stringbuilder.AppendFormat( "{0}", (Int32)file.ResType );
#if UNITY_EDITOR
					if( _write_typetree ) stringbuilder.AppendFormat( "\t{0}", file.AssetBundleTypeTreeHash );
#endif
					stringbuilder.AppendLine();
				}
				Byte[] bytes = Encoding.UTF8.GetBytes( stringbuilder.ToString() );
				file_stream.Write( bytes, 0, bytes.Length );
				file_stream.Flush();
			}
		}

#if UNITY_EDITOR

		public void SaveAs( String _path, Boolean _override )
		{
			if( _path == m_file_path )
				throw new AssetException(
					"Path use with ResConfigFile.SaveAs same as origin path, use ResConfigFile.Save instead!" );

			FileHelper.CreateDirectoryByFilePath( _path );
			// 如果_overrride == false, 使用CreateNew, 当文件存在时会抛出IOExpcetion.
			_serialize( _path, _override ? FileMode.Create : FileMode.CreateNew, true );
		}
		public Boolean BuilderMergeOld( ResConfigFile _old_file )
		{
			Boolean found = false;
			for( Int32 i = 0; i < m_assets.Count; ++i )
			{
				ResInfo old_res_info;
				if( !_old_file.TryGetAssetInfo( m_assets[i].Name, out old_res_info ) )
				{
					found = true;
					Log.Info( "[New]: {0}", m_assets[i].Name );
					continue;
				}
				if( m_assets[i].ResType == ResourceType.AssetBundle && m_assets[i].AssetBundleTypeTreeHash.Equals( old_res_info.AssetBundleTypeTreeHash ) )
				{
					m_assets[i] = old_res_info;
					ResDictionary[old_res_info.Name] = old_res_info;
					continue;
				}
				if( !old_res_info.NeedUpdate( m_assets[i] ) )
				{
					m_assets[i] = old_res_info;
					ResDictionary[old_res_info.Name] = old_res_info;
					continue;
				}
				Log.Info( "[Modify]: {0}", m_assets[i].Name );
				found = true;
			}
			return found;
		}

		/// <summary>
		/// 打包，添加文件信息
		/// </summary>
		public void BuidlerAndResInfo( String _root, String _name, Int32 _version )
		{
			String physics_path = String.Concat( _root, "/", _name );
			// 文件不存在则打包失败
			if( !File.Exists( physics_path ) )
				throw new FileNotFoundException( "[ResConfigFile.BuilderAndResInfo]: 文件 {0} 不存在!", physics_path );

			// 获取校验码
			Byte[] bytes = FileHelper.ReadAllBytesFromFile( physics_path );

			// 检查是否为assetbundle
			// res文件不验证，但却为Assetbundle
			String assetbundleFileHash = null;
			assetbundleFileHash = getAssetbundleFileHash( physics_path, _name );

			ResInfo res_info = new ResInfo ( _name, bytes.Length, _version
				, SHA1Helper.GetSHA1Hash( bytes ), MD5Helper.GetMD5Hash( bytes ), 0
				,  AssetLocation.Void
				,  assetbundleFileHash == null ? ResourceType.GrtPackage : ResourceType.AssetBundle );
			res_info.AssetBundleTypeTreeHash = assetbundleFileHash;
			AddOrUpdateResInfo( res_info );
			//Log.Info( "Build {0} Done! save to: {1}", _name, physics_path );
		}

		/// <summary>
		/// 获取Assetbundle的TypeTreeHash
		/// </summary>
		/// <param name="_assetbunel_path"></param>
		/// <returns></returns>
		private String getAssetbundleFileHash( String _assetbunel_path, String _name )
		{
			String abManifestPath = _assetbunel_path + ".manifest";
			if( !File.Exists( abManifestPath ) )
				return null;
			using( FileStream fs = new FileStream( abManifestPath, FileMode.Open ) )
			{
				using( StreamReader sr = new StreamReader( fs ) )
				{
					/*
					ManifestFileVersion: 0
					CRC: 1076828133
					Hashes:
						AssetFileHash:
						serializedVersion: 2
						Hash: 696f51959500d8be6b76a5c1122ca874
					TypeTreeHash:
						serializedVersion: 2
						Hash: 4778705f9b5cf02bcbaad48f9a31cf4a
					*/
					if( _name.Equals( "res.grt", StringComparison.OrdinalIgnoreCase ) )
					{
						sr.ReadLine();
					}
					else
					{
						for( int i = 0; i < 5; i++ )
						{
							sr.ReadLine();
						}
					}

					String line = sr.ReadLine();
					Int32 start = line.IndexOf( ':' );
					String hash = line.Substring( start + 1,line.Length - start - 1 );
					hash = hash.Trim();
					return hash;
				}
			}
		}

#endif
	}
}