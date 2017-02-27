using System;
using Groot.Res;
using UnityEngine;

namespace Utility.SheetLite
{
	public abstract class SheetLiteDb
	{
		protected SheetLiteDb( String _path )
		{
			m_data_path = _path;
		}
		public virtual SheetLite OpenSheet( String _sheet_name )
		{
			throw new ExceptionEx( "必须继承该方法！" );
		}
		protected String m_data_path;
	}

	public class SheetLiteDbNormal : SheetLiteDb
	{
		public SheetLiteDbNormal( String _path ) : base( _path )
		{
		}
		public override SheetLite OpenSheet( String _sheet_name )
		{
			try
			{
				SheetLite sheet = new SheetLite();
				return !sheet.Open( _sheet_name, FileHelper.ReadAllBytesFromFile( String.Format( "{0}/{1}.txt", m_data_path, _sheet_name ) ) ) ? null : sheet;
			}
			catch( Exception e )
			{
				Log.Error( "[SheetLiteDbNormal]打开SheetLite文件 {0} 失败! \n 错误信息:  {1}.", _sheet_name, e.Message );
			}
			return null;
		}
	}

	public class SheetLiteDbAssetBundle : SheetLiteDb
	{
		public SheetLiteDbAssetBundle( String _path ) : base( _path ) { }

		public override SheetLite OpenSheet( String _sheet_name )
		{
			//Resource_Assetbundle bundle = null;
			//if( m_data_path.CompareTo( WeiqiApp.CONFIG_ROOT_PATH ) == 0 )
			//{
			//	if( _sheet_name.Substring( 0, 3 ).CompareTo( "pre" ) == 0 )
			//		bundle = ResourceManager.Instance.GetResource( string.Format( "{0}/pre_config.cg", m_data_path ) ) as Resource_Assetbundle;
			//	else
			//		bundle = ResourceManager.Instance.GetResource( string.Format( "{0}/config.cg", m_data_path ) ) as Resource_Assetbundle;
			//}
			//else
			//{
			//	String[] array = _sheet_name.Split( '/' );
			//	string language = array[0];
			//	_sheet_name = array[1];
			//	if( _sheet_name.Substring( 0, 3 ).CompareTo( "pre" ) == 0 )
			//		bundle = ResourceManager.Instance.GetResource( string.Format( "{0}/pre_{1}.cg", m_data_path, language ) ) as Resource_Assetbundle;
			//	else
			//		bundle = ResourceManager.Instance.GetResource( string.Format( "{0}/{1}.cg", m_data_path, language ) ) as Resource_Assetbundle;
			//}
			//if ( bundle == null || !bundle.CheckLoaded() )
			//{
			//	Log.Error( "[ResouceManager]: data/config包不存在或没有加载！不能读取配置{0}", _sheet_name );
			//	return null;
			//}
			Resource_Assetbundle bundle = ResourceManager.Instance.LoadAssetbundleSync( string.Format( "{0}/{1}.cg", m_data_path, _sheet_name ) );
			if( bundle == null )
			{
				throw new ExceptionEx( "不存在配置文件包: {0}", _sheet_name );
			}
			if( m_data_path.CompareTo( WeiqiApp.LANGUAGES_ROOT_PATH ) == 0 )
			{
				String[] array = _sheet_name.Split( '/' );
				_sheet_name = array[1];
			}
			TextAsset asset = bundle.LoadAssetSync( _sheet_name, typeof( TextAsset ) ) as TextAsset;
			if( asset == null )
				throw new ExceptionEx( "配置文件assetbundle中不包含: {0}", _sheet_name );
			try
			{
				SheetLite sheet = new SheetLite();
				sheet = !sheet.Open( _sheet_name, asset.bytes ) ? null : sheet;
				bundle.Unload( true );
				return sheet;
			}
			catch( Exception e )
			{
				Log.Error( "[SheetLiteDbAssetBundle]Failed to open sheet {0} \nmsg:  {1}.", _sheet_name, e.Message );
			}
			return null;
		}
	}
}