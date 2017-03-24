using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Groot;
using Groot.Network;
using Utility;
using Weiqi;

public class ItemSystem
{
	public static readonly ItemSystem Instance = new ItemSystem();
	private List<PropItem> m_items = new List<PropItem>();
	private Dictionary<UInt64, ItemAttr> m_item_attr = new Dictionary<UInt64, ItemAttr>();

	public void Initialize()
	{
		NetManager.Instance.Register<GC_GetItems>( _onPacketArrived );
		NetManager.Instance.Register<GC_GetItemAttr>( _onPacketArrived );
	}

	public void Uninitialize()
	{
		m_items.Clear();
		m_item_attr.Clear();
		NetManager.Instance.Unregister<GC_GetItems>();
		NetManager.Instance.Unregister<GC_GetItemAttr>();
	}

	public ItemAttr GetItemAttr( UInt64 _id )
	{
		if( !m_item_attr.ContainsKey( _id ) )
			return null;
		return m_item_attr[_id];
	}

	private void _onPacketArrived( Int32 _stream_id, PacketType _packet_type, GC_GetItems _msg )
	{
		m_items.Clear();
		for( Int32 i = 0; i < _msg.PropItemsCount; i++ )
		{
			m_items.Add( _msg.PropItems[i] );
		}
	}

	private void _onPacketArrived( Int32 _stream_id, PacketType _packet_type, GC_GetItemAttr _msg )
	{
		if( _msg.CurrentStartNo == 1 )
			m_item_attr.Clear();
		for( Int32 i = 0; i < _msg.ItemsCount; i++ )
		{
			m_item_attr.Add( _msg.ItemsAttr[i].PropID, _msg.ItemsAttr[i] );
		}
	}
}
