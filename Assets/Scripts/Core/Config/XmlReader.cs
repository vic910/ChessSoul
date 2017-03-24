using System;
using System.Collections;
using System.Collections.Generic;
using Groot.Res;
using UnityEngine;
using Utility;
using Utility.SheetLite;

public class XmlReader
{
	public static Byte[] GetXmlConfigBytes( String _file_name )
	{
#if !GROOT_ASSETBUNDLE_SIMULATION && UNITY_EDITOR
		String path = String.Format( "{0}/{1}.xml", WeiqiApp.CONFIG_ROOT_PATH, _file_name );
		if( !System.IO.File.Exists( path ) )
			throw new ExceptionEx( "本地配置文件 {0} 不存在!", path );

		Byte[] bytes = FileHelper.ReadAllBytesFromFile( path );
		return bytes;
#else
		Resource_Assetbundle bundle = ResourceManager.Instance.LoadAssetbundleSync( string.Format( "{0}/{1}.cg", WeiqiApp.CONFIG_ROOT_PATH, _file_name ) );
		if( bundle == null )
		{
			throw new ExceptionEx( "不存在配置文件包: {0}", _file_name );
		}
		TextAsset asset = bundle.LoadAssetSync( _file_name, typeof( TextAsset ) ) as TextAsset;
		if( asset == null )
			throw new ExceptionEx( "配置文件assetbundle中不包含: {0}", _file_name );
		try
		{
			Byte[] bt = new byte[asset.bytes.Length];
			Array.Copy( asset.bytes, bt, bt.Length );
			bundle.Unload( true );
			return bt;
		}
		catch( Exception e )
		{
			Log.Error( "[SheetLiteDbAssetBundle]Failed to open sheet {0} \nmsg:  {1}.", _file_name, e.Message );
		}
		return null;
#endif
	}
}
