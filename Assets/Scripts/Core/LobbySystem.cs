using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Groot.Network;
using SLua;
using Utility;
using Weiqi;

[CustomLuaClassAttribute]
public class LobbySystem
{
	public static readonly LobbySystem Instance = new LobbySystem();
	private Dictionary<UInt64, PlayerInfoBase> m_players = new Dictionary<ulong, PlayerInfoBase>();

	[DoNotToLua]
	public void Initialize()
	{
		NetManager.Instance.Register<GC_HallPlayerInfo>( _onPacketArrived );
		NetManager.Instance.Register<GC_PlayerJoinGame>( _onPacketArrived );
	}

	[DoNotToLua]
	public void Uninitialize()
	{
		NetManager.Instance.Unregister<GC_HallPlayerInfo>();
		NetManager.Instance.Unregister<GC_PlayerJoinGame>();
	}

	public PlayerInfoBase GetPlayerInfo( UInt64 _id )
	{
		if( m_players.ContainsKey( _id ) )
			return m_players[_id];
		return null;
	}

    public List<PlayerInfoBase> GetAllPlayerInfo()
    {
        List<PlayerInfoBase> lis = new List<PlayerInfoBase>();
        foreach (var item in m_players)
        {
            lis.Add(item.Value);
        }
        return lis;
    }

    private void _onPacketArrived( Int32 _stream_id, PacketType _packet_type, GC_HallPlayerInfo _msg )
	{
		for( int i = 0; i < _msg.PlayerCount; i++ )
		{
			if( _msg.PlayerInfo[i].PlayerID == 0 )
				continue;
			if( m_players.ContainsKey( _msg.PlayerInfo[i].PlayerID ) )
				m_players[_msg.PlayerInfo[i].PlayerID] = _msg.PlayerInfo[i];
			else
				m_players.Add( _msg.PlayerInfo[i].PlayerID, _msg.PlayerInfo[i] );
             PlayerOnlineSystem.Instance.UpdatePlayerInfo(0, _msg.PlayerInfo[i].PlayerID);
        }
	}

	private void _onPacketArrived( Int32 _stream_id, PacketType _packet_type, GC_PlayerJoinGame _msg )
	{
		for( int i = 0; i < _msg.Count; i++ )
		{
			if( _msg.PlayerInfo[i].PlayerID == 0 )
				continue;
			if( m_players.ContainsKey( _msg.PlayerInfo[i].PlayerID ) )
				m_players[_msg.PlayerInfo[i].PlayerID] = _msg.PlayerInfo[i];
			else
				m_players.Add( _msg.PlayerInfo[i].PlayerID, _msg.PlayerInfo[i] );
		}
	}
}
