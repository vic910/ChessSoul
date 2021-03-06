﻿using System;
using Groot;
using Groot.Network;
using SLua;

/// <summary>
/// 我的信息
/// </summary>
[CustomLuaClassAttribute]
public class MainPlayer
{
    public static readonly MainPlayer Instance = new MainPlayer();

    public PlayerInfo PlayerInfo { get; private set; }

    public void Initialize()
    {
        NetManager.Instance.Register<GC_UpdatePlayerGold>(_onPacketArrived);
        NetManager.Instance.Register<GC_UpdateDeltaGold>(_onPacketArrived);
        NetManager.Instance.Register<GC_UpdatePlayerMoney>(_onPacketArrived);
        NetManager.Instance.Register<GC_UpdateDeltaMoney>(_onPacketArrived);
        NetManager.Instance.Register<GC_UpdatePlayerLiveness>(_onPacketArrived);
    }

    public void UnInitialize()
    {
        NetManager.Instance.Unregister<GC_UpdatePlayerGold>();
        NetManager.Instance.Unregister<GC_UpdateDeltaGold>();
        NetManager.Instance.Unregister<GC_UpdatePlayerMoney>();
        NetManager.Instance.Unregister<GC_UpdateDeltaMoney>();
        NetManager.Instance.Unregister<GC_UpdatePlayerLiveness>();
    }

    [DoNotToLua]
    public void InitializePlayerInfo(PlayerInfo _info)
    {
        PlayerInfo = _info;
    }

    private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_UpdatePlayerGold _msg)
    {
        PlayerInfo.Gold = Convert.ToUInt32(_msg.GoldNow);
        SignalSystem.FireSignal(SignalId.Gold_Update, PlayerInfo.Gold);
    }

    private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_UpdateDeltaGold _msg)
    {
        if (_msg.DeltaGold < 0)
        {
            UInt64 deltaMoney = Convert.ToUInt64(_msg.DeltaGold);
            if (PlayerInfo.Gold < deltaMoney)
                PlayerInfo.Gold = 0;
            else
                PlayerInfo.Gold -= Convert.ToUInt32(_msg.DeltaGold);
        }
        PlayerInfo.Gold += Convert.ToUInt32(_msg.DeltaGold);
        SignalSystem.FireSignal(SignalId.Gold_Update, PlayerInfo.Gold);
    }

    private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_UpdatePlayerMoney _msg)
    {
        PlayerInfo.Money = Convert.ToUInt64(_msg.GoldNow);
        SignalSystem.FireSignal(SignalId.Money_Update, PlayerInfo.Money);
    }

    private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_UpdateDeltaMoney _msg)
    {
        if (_msg.DeltaMoney < 0)
        {
            UInt64 deltaMoney = Convert.ToUInt64(_msg.DeltaMoney);
            if (PlayerInfo.Money < deltaMoney)
                PlayerInfo.Money = 0;
            else
                PlayerInfo.Money -= Convert.ToUInt32(_msg.DeltaMoney);
        }
        else
            PlayerInfo.Money += Convert.ToUInt32(_msg.DeltaMoney);
        SignalSystem.FireSignal(SignalId.Gold_Update, PlayerInfo.Money);
    }

    private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_UpdatePlayerLiveness _msg)
    {
        PlayerInfo.Liveness = Convert.ToUInt64(_msg.Liveness);
        SignalSystem.FireSignal(SignalId.Liveness_Update, PlayerInfo.Liveness);
    }
}
