using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Weiqi.UI
{
	class UIStackConfigureManager
	{
		public static readonly UIStackConfigureManager Instance = new UIStackConfigureManager();
		private Dictionary<string, UIStackConfigure> m_ui_stack_configure = new Dictionary<string, UIStackConfigure>();
		UIStackConfigureManager()
		{

		}
		public bool LoadConfigure()
		{
			if( !LoadUIHelpConfigure() )
			{
				Utility.Log.Error( "IHelpConfigure load error" );
				return false;
			}
			return true;
		}

		public void UnLoadConfigure()
		{
			m_ui_stack_configure.Clear();
		}

		public List<string> GetReBuildStackList( string _begin_ui_name )
		{
			UIStackConfigure ui_help_info = null;
			if( m_ui_stack_configure.TryGetValue( _begin_ui_name, out ui_help_info ) )
			{
				return ui_help_info.UIStackList;
			}
			else
			{
				Utility.Log.Error( string.Format( "dont have {0}'s stack rebuild info from config", _begin_ui_name ) );
				return null;
			}
		
		}

		public bool NeedPopUI( string _from_ui_name, string _to_ui_name )
		{
			if( m_ui_stack_configure.ContainsKey( _from_ui_name ) )
			{
				UIStackConfigure ui_help_info = new UIStackConfigure();
				m_ui_stack_configure.TryGetValue( _from_ui_name, out ui_help_info );
				for( int i = 0; i < ui_help_info.UIStackList.Count; i++ )
				{
					if( ui_help_info.UIStackList[i] == _to_ui_name )
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool LoadUIHelpConfigure()
		{
			Utility.SheetLite.SheetReader reader = new Utility.SheetLite.SheetReader();
			if( !reader.OpenSheet( "ui/ui_stack_config" ) )
				return false;
			Int32 count = reader.Count;
			for( Int32 i = 0; i < count; ++i )
			{
				var row = reader[i];
				UIStackConfigure config = new UIStackConfigure();
				if( !config.Parse( row ) )
					continue;

				m_ui_stack_configure.Add( config.UI_Begin, config );
			}
			//MainPlayer.Instance.taskData.GetCurrentTask();
			return true;
		}
	}
}