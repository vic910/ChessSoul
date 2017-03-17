using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Groot.Network;
using SLua;

[CustomLuaClassAttribute]
public class PlayerOnlineSystem
{
    public enum PlayerType
    {
        All,//所有
        Friends,//好友
        BlackList,//黑名单
        Appoint//显示
    }

    private static PlayerOnlineSystem m_instance;

    public static PlayerOnlineSystem Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new PlayerOnlineSystem();
            return m_instance;
        }
    }

    //玩家ID存储表
    private Dictionary<PlayerType, List<UInt64>> m_PlayerID = new Dictionary<PlayerType, List<UInt64>>();

    [DoNotToLua]
    public void Initialize()
    {
        m_PlayerID.Add(PlayerType.All, new List<UInt64>());
        m_PlayerID.Add(PlayerType.Friends, new List<UInt64>());
        m_PlayerID.Add(PlayerType.BlackList, new List<UInt64>());
        m_PlayerID.Add(PlayerType.Appoint, new List<UInt64>());
    }

    [DoNotToLua]
    public void Uninitialize()
    {
        m_PlayerID.Clear();
    }


    #region//表操作

    /// <summary>
    /// 根据分类得到玩家信息
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_type"></param>
    /// <returns></returns>
    public PlayerInfoBase GetPlayerInfoByIndex(int _index, PlayerType _type)
    {
        UInt64 id = m_PlayerID[_type][_index];
        PlayerInfoBase playerInfo = LobbySystem.Instance.GetPlayerInfo(id);
        return playerInfo;
    }

    /// <summary>
    /// 根据玩家名字得到玩家信息
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_type"></param>
    /// <returns></returns>
    public PlayerInfoBase GetPlayerInfoByName(string _name, PlayerType _type)
    {
        if (String.IsNullOrEmpty(_name))
            return null;

        List<PlayerInfoBase> playerInfo = LobbySystem.Instance.GetAllPlayerInfo();
        foreach (PlayerInfoBase item in playerInfo)
        {
            if (item.PlayerName == _name)
            {
                if (m_PlayerID[_type].Contains(item.PlayerID))
                    return item;
            }
        }
        return null;
    }

    /// <summary>
    /// 得到分类玩家总数
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public int GetPlayerInfoCount(PlayerType _type)
    {
        return m_PlayerID[_type].Count;
    }

    /// <summary>
    /// 更新指定分类玩家列表
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_newID"></param>
    public void UpdatePlayerInfo(PlayerType _type, UInt64 _newID)
    {
        if (m_PlayerID[_type].Contains(_newID))
            return;
        m_PlayerID[_type].Add(_newID);
    }

    /// <summary>
    /// 更新显示分类玩家列表
    /// </summary>
    /// <param name="_newLis"></param>
    public void UpdateAppoint(List<UInt64> _newLis)
    {
        m_PlayerID[PlayerType.Appoint].Clear();
        m_PlayerID[PlayerType.Appoint].AddRange(_newLis);
    }

    /// <summary>
    /// 得到指定类型玩家列表
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public List<UInt64> GetPlayerInfoList(PlayerType _type)
    {
        return m_PlayerID[_type];
    }
    #endregion
}
