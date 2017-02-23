using System;
using Utility.Variant;
using Utility;
using Utility.VariantSystem;

public sealed class GlobalConfig
	{
		public static readonly GlobalConfig Instance = new GlobalConfig();
		private GlobalConfig() { }

		private readonly VariantContainer m_container = new VariantContainer();

		public Boolean Initialize()
		{
			Utility.SheetLite.SheetReader reader = new Utility.SheetLite.SheetReader();
			if( !reader.OpenSheet( "global_config" ) )
				return false;
			for( Int32 i = 0; i < reader.Count; ++i )
			{
				String key;
				Variant value;
				if( !VariantPairSerializer.CreateFromSheetLite( reader[i], out key, out value ) )
				{
					Log.Error( "È«¾ÖÅäÖÃ¶ÁÈ¡Ê§°Ü!" );
					return false;
				}
				m_container.Add( key, value );
			}
			return true;
		}

		public void Uninitialize()
		{
			m_container.Clear();
		}

		public T GetValue<T>( String _key )
		{
			return m_container.GetValue<T>( _key );
		}
	}