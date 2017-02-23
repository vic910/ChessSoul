using System;
using System.Collections.Generic;

namespace Utility.SheetLite
{
	public class Manager
	{
		public static readonly Manager Instance = new Manager();

		private Manager()
		{
#if !GROOT_ASSETBUNDLE_SIMULATION && UNITY_EDITOR
			SheetLiteDbNormal default_db = new SheetLiteDbNormal( WeiqiApp.CONFIG_ROOT_PATH );
			SheetLiteDbNormal languages_db = new SheetLiteDbNormal( WeiqiApp.LANGUAGES_ROOT_PATH );
#else
			SheetLiteDbAssetBundle default_db = new SheetLiteDbAssetBundle( WeiqiApp.CONFIG_ROOT_PATH );
			SheetLiteDbAssetBundle languages_db = new SheetLiteDbAssetBundle( WeiqiApp.LANGUAGES_ROOT_PATH );
#endif
			m_dbs.Add( "data", default_db );
			SheetReader.BindDB( default_db );
			m_dbs.Add( "language", languages_db );
		}

		public SheetLiteDb GetDB( String _db_name )
		{
			SheetLiteDb db;
			return !m_dbs.TryGetValue( _db_name, out db ) ? null : db;
		}

		private readonly Dictionary<String, SheetLiteDb> m_dbs = new Dictionary<String, SheetLiteDb>();
	}
}