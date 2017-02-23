using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weiqi.UI
{
	class UIStackConfigure
	{
		public string UI_Begin { get { return m_begin_ui; } }
		private string m_begin_ui = "";

		public List<string> UIStackList { get { return m_stacks_info; } }
		private List<string> m_stacks_info = new List<string>();

		public bool Parse( Utility.SheetLite.SheetRow _row )
		{
			m_begin_ui = _row["BeginUI"];
			//Utility.Log.Warning("_row.cout " + _row.Count);
			for( int i = 0; i < _row.Count - 1; i++ )
			{
				string rowname = "Stack_1_" + ( i + 1 ).ToString();
				if( _row[rowname] != String.Empty )
				{
					m_stacks_info.Add( _row[rowname] );
				}
			}
			return true;
		}
	}
}
