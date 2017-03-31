using System;
using System.Collections;
using System.Collections.Generic;
using Groot;
using Groot.Network;
using SLua;
using UnityEngine;
using UnityEngine.VR;

[CustomLuaClass]
public class ShopSystem
{
    public static readonly ShopSystem Instance = new ShopSystem();
    private Dictionary<int, List<SaleItem>> m_saleItem_attr = new Dictionary<int, List<SaleItem>>();
    private List<PropItem> m_shoppingcarItem_attr = new List<PropItem>();
    private List<BuyRetItem> m_buyRetItem_attr = new List<BuyRetItem>();

    [DoNotToLua]
    public void Initialize()
    {
        NetManager.Instance.Register<GC_GetAllSales>(_onPacketArrived);
        NetManager.Instance.Register<GC_Buy>(_onPacketArrived);
        InitSaleItemAttr();
    }

    [DoNotToLua]
    public void Uninitialize()
    {
        m_saleItem_attr.Clear();
        m_shoppingcarItem_attr.Clear();
        NetManager.Instance.Unregister<CG_UserSaleToSystem>();
        NetManager.Instance.Unregister<GC_Buy>();
    }

    /// <summary>
    /// 接受商品信息
    /// </summary>
    /// <param name="_stream_id"></param>
    /// <param name="_packet_type"></param>
    /// <param name="_msg"></param>
    private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_GetAllSales _msg)
    {
        if (_msg.CurrentStartNo == 1)
        {
            InitSaleItemAttr();
        }
        for (Int32 i = 0; i < _msg.SaleItemCount; i++)
        {
            SaleItem saleItem = _msg.SaleItem[i];
            ItemAttr item = ItemSystem.Instance.GetItemAttr(saleItem.ItemID);
            int type = 0;
            if (item != null)
                type = item.Type;
            switch (type)
            {
                case 0:
                    m_saleItem_attr[1].Add(saleItem);//减损
                    break;
                case 1:
                    m_saleItem_attr[0].Add(saleItem);//加倍
                    break;
                case 6:
                    m_saleItem_attr[2].Add(saleItem);//宝箱
                    break;
                default:
                    m_saleItem_attr[3].Add(saleItem);//其他
                    break;
            }
        }
    }

    /// <summary>
    /// 购买失败
    /// </summary>
    /// <param name="_stream_id"></param>
    /// <param name="_packet_type"></param>
    /// <param name="_msg"></param>
    private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_Buy _msg)
    {
        m_buyRetItem_attr.Clear();
        for (int i = 0; i < _msg.BuyStatusCount; i++)
        {
            m_buyRetItem_attr.Add(_msg.BuyStatusItem[i]);
        }
        SignalSystem.FireSignal(SignalId.BuyFail_Update);
    }


    /// <summary>
    /// 购物车购买
    /// </summary>
    public void BuyItemToSystem()
    {
        CG_Buy msg = new CG_Buy();
        msg.BuyerID = MainPlayer.Instance.PlayerInfo.PlayerID;
        msg.BuyItemCount = Convert.ToUInt32(m_shoppingcarItem_attr.Count);
        foreach (PropItem item in m_shoppingcarItem_attr)
        {
            BuyItem buyItem = new BuyItem();
            buyItem.SaleID = GetSaleItem(item.PropID).SaleID;
            buyItem.BuyCount = Convert.ToUInt32(item.Count);
            msg.BuyItems.Add(buyItem);
        }
        NetManager.Instance.SendMsg(msg);
    }

    /// <summary>
    /// 购买单个商品
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_count"></param>
    public void BuyItemtoSystem(UInt64 _id, UInt32 _count)
    {
        CG_Buy msg = new CG_Buy();
        msg.BuyerID = MainPlayer.Instance.PlayerInfo.PlayerID;
        msg.BuyItemCount = 1;

        BuyItem buyItem = new BuyItem();
        buyItem.SaleID = GetSaleItem(_id).SaleID;
        buyItem.BuyCount = _count;

        msg.BuyItems = new List<BuyItem>();
        msg.BuyItems.Add(buyItem);

        NetManager.Instance.SendMsg(msg);
    }

    #region//商城及购物车数据操作

    /// <summary>
    /// 0.加倍 1.减损 2.宝箱 3.其他 4.当前显示
    /// </summary>
    private void InitSaleItemAttr()
    {
        m_saleItem_attr.Clear();
        m_saleItem_attr.Add(0, new List<SaleItem>());
        m_saleItem_attr.Add(1, new List<SaleItem>());
        m_saleItem_attr.Add(2, new List<SaleItem>());
        m_saleItem_attr.Add(3, new List<SaleItem>());
        m_saleItem_attr.Add(4, new List<SaleItem>());
    }

    /// <summary>
    /// 得到售卖信息
    /// </summary>
    /// <param name="_itemID">物品ID</param>
    /// <returns></returns>
    public SaleItem GetSaleItem(ulong _itemID)
    {
        foreach (var item in m_saleItem_attr)
        {
            foreach (SaleItem saleItem in item.Value)
            {
                if (saleItem.ItemID == _itemID)
                    return saleItem;
            }
        }
        return null;
    }

    public SaleItem GetSaleItemBySaleID(ulong _saleID)
    {
        foreach (var item in m_saleItem_attr)
        {
            foreach (SaleItem saleItem in item.Value)
            {
                if (saleItem.SaleID == _saleID)
                    return saleItem;
            }
        }
        return null;
    }

    /// <summary>
    /// 得到对应的商品列表
    /// </summary>
    /// <param name="_index">见初始化列表</param>
    /// <returns></returns>
    public List<SaleItem> GetSaleItemAttr(int _index)
    {
        if (!m_saleItem_attr.ContainsKey(_index))
            return null;
        return m_saleItem_attr[_index];
    }

    /// <summary>
    /// 设置当前显示列表
    /// </summary>
    /// <param name="_newShowList">新的需要显示的列表</param>
    public void SetShowList(List<SaleItem> _newShowList)
    {
        m_saleItem_attr[4].Clear();
        m_saleItem_attr[4].AddRange(_newShowList);
    }

    /// <summary>
    /// 得到售卖商品信息
    /// </summary>
    /// <param name="_index">列表中的序列</param>
    /// <param name="_list_index">对应列表分类</param>
    /// <returns></returns>
    public SaleItem GetItemInfo(int _index, int _list_index)
    {
        List<SaleItem> list = m_saleItem_attr[_list_index];
        if (_index < list.Count)
            return list[_index];
        return null;
    }

    /// <summary>
    /// 得到购物车列表
    /// </summary>
    /// <returns></returns>
    public List<PropItem> GetShoppingcarItemList()
    {
        return m_shoppingcarItem_attr;
    }

    /// <summary>
    /// 设置购物车列表 用于购物车界面增删商品
    /// </summary>
    /// <param name="_id">物品ID</param>
    /// <param name="_count">设置数量</param>
    public void SetShoppingcarItemList(UInt64 _id, Int32 _count)
    {
        foreach (PropItem item in m_shoppingcarItem_attr)
        {
            if (item.PropID == _id)
            {
                if (_count == 0)
                {
                    m_shoppingcarItem_attr.Remove(item);
                    Groot.SignalSystem.FireSignal(Groot.SignalId.ShoppingCar_Update);
                    return;
                }
                item.Count = _count;
                Groot.SignalSystem.FireSignal(Groot.SignalId.ShoppingCar_Update);
                return;
            }
        }
        PropItem it = new PropItem();
        it.PropID = _id;
        it.Count = _count;
        m_shoppingcarItem_attr.Add(it);
        Groot.SignalSystem.FireSignal(Groot.SignalId.ShoppingCar_Update);
    }

    /// <summary>
    /// 在商城界面添加商品
    /// </summary>
    /// <param name="_id">物品ID</param>
    /// <param name="_count">物品数量</param>
    public void AddShoppingcarItemList(UInt64 _id, Int32 _count)
    {
        foreach (PropItem item in m_shoppingcarItem_attr)
        {
            if (item.PropID == _id)
            {
                item.Count += _count;
                return;
            }
        }
        PropItem it = new PropItem();
        it.PropID = _id;
        it.Count = _count;
        m_shoppingcarItem_attr.Add(it);
        Groot.SignalSystem.FireSignal(Groot.SignalId.ShoppingCar_Update);
    }

    public List<BuyRetItem> GetBuyRet()
    {
        return m_buyRetItem_attr;
    }

    #endregion
}
