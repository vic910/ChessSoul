using System;
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
        NetManager.Instance.Register<GC_UpdatePlayerMoney>(_onPacketArrived);
		NetManager.Instance.Register<GC_UpdatePlayerLiveness>( _onPacketArrived );
    }

	public void UnInitialize()
	{
        NetManager.Instance.Unregister<GC_UpdatePlayerGold>();
        NetManager.Instance.Unregister<GC_UpdatePlayerMoney>();
		NetManager.Instance.Unregister<GC_UpdatePlayerLiveness>();
	}

	[DoNotToLua]
	public void InitializePlayerInfo( PlayerInfo _info )
	{
		   PlayerInfo = _info;
	}

    private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_UpdatePlayerGold _msg)
    {
        PlayerInfo.Gold = Convert.ToUInt32( _msg.GoldNow);
        SignalSystem.FireSignal(SignalId.Gold_Update, PlayerInfo.Gold);
    }

    private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_UpdatePlayerMoney _msg)
    {
        PlayerInfo.Money = Convert.ToUInt64(_msg.GoldNow);
        SignalSystem.FireSignal(SignalId.Money_Update, PlayerInfo.Money);
    }

	private void _onPacketArrived( Int32 _stream_id, PacketType _packet_type, GC_UpdatePlayerLiveness _msg )
	{
		PlayerInfo.Liveness = Convert.ToUInt64( _msg.Liveness );
		SignalSystem.FireSignal( SignalId.Liveness_Update, PlayerInfo.Liveness );
	}
}
