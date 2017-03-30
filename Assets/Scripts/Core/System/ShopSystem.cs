using System;
using System.Collections;
using System.Collections.Generic;
using Groot.Network;
using UnityEngine;

public class ShopSystem
{
    public static readonly ShopSystem Instance = new ShopSystem();
    private Dictionary<ulong, List<SaleItem>> m_saleItem_attr = new Dictionary<ulong, List<SaleItem>>();

    public void Initialize()
    {
        NetManager.Instance.Register<GC_GetAllSales>(_onPacketArrived);
    }

    public void Uninitialize()
    {
        m_saleItem_attr.Clear();
        NetManager.Instance.Unregister<CG_UserSaleToSystem>();
    }

    private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_GetAllSales _msg)
    {
        if (_msg.CurrentStartNo == 1)
        {
            m_saleItem_attr.Clear();
            m_saleItem_attr.Add(1, new List<SaleItem>());
        }
        for (Int32 i = 0; i < _msg.SaleItemCount; i++)
        {
            m_saleItem_attr[1].Add(_msg.SaleItem[i]);
        }


    }
}
