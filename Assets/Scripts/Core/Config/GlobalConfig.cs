using System;
using Utility.Variant;
using Utility;
using Utility.VariantSystem;
using Weiqi;

public class GlobalConfig : GrootSingleton<GlobalConfig>
{
	private readonly VariantContainer m_container = new VariantContainer();
	public override void Initialize()
	{
		Utility.SheetLite.SheetReader reader = new Utility.SheetLite.SheetReader();
		if( !reader.OpenSheet( "global_config" ) )
			return;
		for( Int32 i = 0; i < reader.Count; ++i )
		{
			String key;
			Variant value;
			if( !VariantPairSerializer.CreateFromSheetLite( reader[i], out key, out value ) )
			{
				Log.Error( "È«¾ÖÅäÖÃ¶ÁÈ¡Ê§°Ü!" );
				return;
			}
			m_container.Add( key, value );
		}

       // LocalConfigSystem.Instacne.CreateXml();
    }

	public override void Uninitialize ( )
	{
		m_container.Clear();
	}

	public T GetValue<T>( String _key )
	{
		return m_container.GetValue<T>( _key );
	}
}