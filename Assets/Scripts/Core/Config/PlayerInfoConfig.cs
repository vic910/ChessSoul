using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using SLua;
using Utility.Variant;
using Utility;
using Utility.VariantSystem;


[CustomLuaClassAttribute]
public class PlayerInfoConfig
{
	public static PlayerInfoConfig Instance = new PlayerInfoConfig();
	/// <summary>
	/// 地域信息
	/// </summary>
	private Dictionary<Int32, PlayerAreaInfo> m_area_info = new Dictionary<Int32, PlayerAreaInfo>();
	/// <summary>
	/// 活跃度信息
	/// </summary>
	private List<PlayerLivenessInfo> m_liveness_info = new List<PlayerLivenessInfo>();

	[DoNotToLua]
	public void Initialize()
	{
		_loadAreaInfoConfig();
		_loadLivenessInfoConfig();
	}

	[DoNotToLua]
	public void UnInitialize()
	{
		m_area_info.Clear();
		m_liveness_info.Clear();
	}

	public PlayerAreaInfo GetAreaInfo( Int32 _id )
	{
		if( !m_area_info.ContainsKey( _id ) )
		{
			Log.Error( "没有ID为{0}的地域配置！", _id );
			return null;
		}
		return m_area_info[_id];
	}

	public PlayerLivenessInfo GetLivenessInfo( Int32 _liveness )
	{
		for( Int32 i = 0; i < m_liveness_info.Count; ++i )
		{
			if( i + 1 == m_liveness_info.Count )
				return m_liveness_info[i];
			if( _liveness >= m_liveness_info[i].Liveness && _liveness < m_liveness_info[i+1].Liveness )
				return m_liveness_info[i];
		}
		return m_liveness_info[0];
	}

	private void _loadAreaInfoConfig()
	{
		byte[] bytes = XmlReader.GetXmlConfigBytes( "area_info_config" );
		MemoryStream memory_stream = new MemoryStream( bytes );
		StreamReader stream = new StreamReader( memory_stream, Encoding.UTF8 );
		if( Encoding.UTF8 != stream.CurrentEncoding )
			throw new ConfigFileNotUTF8( "area_info_config.xml" );
		XmlDocument document = new XmlDocument();
		document.Load( stream );
		XmlElement element = document.ChildNodes[1] as XmlElement;

		foreach( XmlNode node in element.ChildNodes )
		{
			if( node == null )
				continue;
			if( node.Name == "State" )
			{
				PlayerAreaInfo configure = new PlayerAreaInfo();
				configure.Parse( node as XmlElement );
				m_area_info[configure.Id] = configure;
			}
		}
	}

	private void _loadLivenessInfoConfig()
	{
		byte[] bytes = XmlReader.GetXmlConfigBytes( "livenessval_info_config" );
		MemoryStream memory_stream = new MemoryStream( bytes );
		StreamReader stream = new StreamReader( memory_stream, Encoding.UTF8 );
		if( Encoding.UTF8 != stream.CurrentEncoding )
			throw new ConfigFileNotUTF8( "livenessval_info_config.xml" );
		XmlDocument document = new XmlDocument();
		document.Load( stream );
		XmlElement element = document.ChildNodes[1] as XmlElement;

		foreach( XmlNode node in element.ChildNodes )
		{
			if( node == null )
				continue;
			if( node.Name == "RuleItem" )
			{
				PlayerLivenessInfo configure = new PlayerLivenessInfo();
				configure.Parse( node as XmlElement );
				m_liveness_info.Add( configure );
			}
		}
	}
}

/// <summary>
/// 地域信息
/// </summary>
public class PlayerAreaInfo
{
	public Int32 Id;
	public string Name;
	public string ShortName;
	public string Country;

	public void Parse( XmlElement _xml )
	{
		Id = Convert.ToInt32( _xml.GetAttribute( "ID" ) );
		Name = _xml.GetAttribute( "Name" );
		ShortName = _xml.GetAttribute( "ShortName" );
		Country = _xml.GetAttribute( "ConutryName" );
	}
}

/// <summary>
/// 活跃度信息
/// </summary>
public class PlayerLivenessInfo
{
	public Int32  Liveness;
	public Int32  Level;
	public string Name;

	public void Parse( XmlElement _xml )
	{
		Liveness = Convert.ToInt32( _xml.GetAttribute( "livenessval" ) );
		Level = Convert.ToInt32( _xml.GetAttribute( "level" ) );
		Name = _xml.GetAttribute( "showname" );
	}
}