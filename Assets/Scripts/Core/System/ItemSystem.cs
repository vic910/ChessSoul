using System;
using System.Collections;
using System.Collections.Generic;
using SLua;
using System.Linq;
using Groot;
using Groot.Network;
using Utility;
using Weiqi;

[CustomLuaClassAttribute]
public class ItemSystem
{
	public static readonly ItemSystem Instance = new ItemSystem();
	private List<PropItem> m_items = new List<PropItem>();
	private Dictionary<UInt64, ItemAttr> m_item_attr = new Dictionary<UInt64, ItemAttr>();

	[DoNotToLua]
	public void Initialize()
	{
		NetManager.Instance.Register<GC_GetItems>( _onPacketArrived );
		NetManager.Instance.Register<GC_GetItemAttr>( _onPacketArrived );
		NetManager.Instance.Register<GC_UpdateItems>( _onPacketArrived );
	}

	[DoNotToLua]
	public void Uninitialize()
	{
		m_items.Clear();
		m_item_attr.Clear();
		NetManager.Instance.Unregister<GC_GetItems>();
		NetManager.Instance.Unregister<GC_GetItemAttr>();
		NetManager.Instance.Unregister<GC_UpdateItems>();
	}

	public ItemAttr GetItemAttr( UInt64 _id )
	{
		if( !m_item_attr.ContainsKey( _id ) )
			return null;
		return m_item_attr[_id];
	}

	public PropItem GetMyItemIDByIndex(int _index)
	{
		PropItem myItemInfo = new PropItem();

		if (_index > m_items.Count - 1)
			return null;
		myItemInfo.PropID = m_items [_index].PropID;
		myItemInfo.Count = m_items [_index].Count;
		return myItemInfo;
	}

	public List<PropItem> GetAllMyItem()
	{
		return m_items;
	}

	public void SaleItemToSystem( UInt64 _id, Int32 _count )
	{
		CG_UserSaleToSystem msg = new CG_UserSaleToSystem();
		msg.PlayerID = MainPlayer.Instance.PlayerInfo.PlayerID;
		msg.SaleItemCount = 1;
		PropItem item = new PropItem();
		item.PropID = _id;
		item.Count = _count;
		msg.Items.Add( item );
		NetManager.Instance.SendMsg( msg );
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

	private void _onPacketArrived( Int32 _stream_id, PacketType _packet_type, GC_UpdateItems _msg )
	{
		for( Int32 i = 0; i < _msg.PropItemsCount; i++ )
		{
			for( Int32 j = 0; j < m_items.Count; j++ )
			{
				if( m_items[j].PropID == _msg.PropItems[i].PropID )
				{
					m_items[j].Count += _msg.PropItems[i].Count;
					if( m_items[j].Count <= 0 )
					{
						m_items.RemoveAt( j );
					}
					break;
				}
				if( j + 1 == m_items.Count )
				{
					m_items.Add( _msg.PropItems[i] );
				}
			}
		}
		SignalSystem.FireSignal( SignalId.Item_Update );
	}
}
