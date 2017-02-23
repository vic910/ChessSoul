using System;

namespace Utility.SheetLite
{
	public class SheetReader
	{
		public Boolean OpenSheet( String _sheet_name )
		{
			if( null == m_db )
			{
				Log.Error( "Load Sheet: {0} Fail, Database is null!", _sheet_name );
				return false;
			}
			m_sheet = m_db.OpenSheet( _sheet_name );
			return ( null != m_sheet );
		}

		public Boolean OpenSheet( String _db_name, String _sheet_name )
		{
			SheetLiteDb db = Manager.Instance.GetDB( _db_name );
			if( null == db )
			{
				Log.Error( "Load Sheet: {0} Fail, Database is null!", _sheet_name );
				return false;
			}
			m_sheet = db.OpenSheet( _sheet_name );
			return ( null != m_sheet );
		}

		public SheetRow this[Int32 _index] { get { return m_sheet[_index]; } }
		public Int32 Count { get { return m_sheet.Count; } }
		// Static Methods
		public static void BindDB( SheetLiteDb _db )
		{
			m_db = _db;
		}

		public static void UnbindDB()
		{
			m_db = null;
		}

		private SheetLite m_sheet;
		private static SheetLiteDb m_db;
	}

#if UNITY_EDITOR
	/// <summary>
	/// Editor代码专用
	/// </summary>
	public class SheetReaderEditor
	{
		public Boolean OpenSheet( String _sheet_name )
		{
			if( null == m_db )
			{
				Log.Error( "Load Sheet: {0} Fail, Database is null!", _sheet_name );
				return false;
			}
			m_sheet = m_db.OpenSheet( _sheet_name );
			return ( null != m_sheet );
		}
		public SheetRow this[Int32 _index] { get { return m_sheet[_index]; } }
		public Int32 Count { get { return m_sheet.Count; } }
		// Static Methods
		public static void BindDB( SheetLiteDb _db )
		{
			m_db = _db;
		}
		public static void UnbindDB()
		{
			m_db = null;
		}

		private SheetLite m_sheet = null;
		private static SheetLiteDb m_db = null;
	}
#endif

}