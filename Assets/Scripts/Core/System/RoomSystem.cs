using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Groot.Network;
using SLua;
using Utility;
using Weiqi;

[CustomLuaClassAttribute]
public class RoomSystem
{
	public static readonly RoomSystem Instance = new RoomSystem();

	private List<RoomInfoBase> m_rooms = new List<RoomInfoBase>();
	/// <summary>
	/// 4直播 6对局 5讲解 253搜索的 254我的对局 255全部
	/// </summary>
	private Dictionary<Byte, List<Int32>> m_dic_rooms = new Dictionary<byte, List<Int32>>();

	[DoNotToLua]
	public void Initialize()
	{
		NetManager.Instance.Register<GC_HallRoomInfoMsg>( _onPacketArrived );
	}

	[DoNotToLua]
	public void Uninitialize()
	{
		NetManager.Instance.Unregister<GC_HallRoomInfoMsg>();
	}

	public Int32 GetRoomCount( Byte _type )
	{
		if( m_dic_rooms.ContainsKey( _type ) )
			return m_dic_rooms[_type].Count;
		return 0;
	}

	public RoomInfoBase GetRoomInfo( Byte _type, Int32 _index )
	{
		if( !m_dic_rooms.ContainsKey( _type ) )
			return null;
		if( _index >= m_dic_rooms[_type].Count )
			return null;
		Int32 index = m_dic_rooms[_type][_index];
		if( index >= m_rooms.Count )
			return null;
		return m_rooms[index];
	}

	public Int32 SearchRoom( Int32 _room )
	{
		m_dic_rooms[(Byte)ERoomType.ROOM_TYPE_SEARCH].Clear();
		for( Int32 i = 0; i < m_rooms.Count; ++i )
		{
			if( m_rooms[i].RoomID == _room )
			{
				m_dic_rooms[(Byte)ERoomType.ROOM_TYPE_SEARCH].Add( i );
				return 1;
			}
		}
		return 0;
	}

	private void _onPacketArrived( Int32 _stream_id, PacketType _packet_type, GC_HallRoomInfoMsg _msg )
	{
		m_rooms.Clear();
		m_dic_rooms.Clear();

		m_dic_rooms.Add( (Byte)ERoomType.ROOM_TYPE_LIVEBROADCAST, new List<Int32>() );
		m_dic_rooms.Add( (Byte)ERoomType.ROOM_TYPE_PLAYGO, new List<Int32>() );
		m_dic_rooms.Add( (Byte)ERoomType.ROOM_TYPE_EXPLAIN, new List<Int32>() );
		m_dic_rooms.Add( (Byte)ERoomType.ROOM_TYPE_MINE, new List<Int32>() );
		m_dic_rooms.Add( (Byte)ERoomType.ROOM_TYPE_ALL, new List<Int32>() );
		m_dic_rooms.Add( (Byte)ERoomType.ROOM_TYPE_SEARCH, new List<Int32>() );
		for( int i = 0; i < _msg.Rooms.Count; i++ )
		{
			if( _msg.Rooms[i].RoomID == 0 )
				break;
			m_rooms.Add( _msg.Rooms[i] );
			switch( m_rooms.Last().RoomType )
			{
			case (byte)ERoomType.ROOM_TYPE_LIVEBROADCAST:
			case (byte)ERoomType.ROOM_TYPE_PLAYGO:
			case (byte)ERoomType.ROOM_TYPE_EXPLAIN:
				m_dic_rooms[m_rooms.Last().RoomType].Add( m_rooms.Count - 1 );
				break;
			}
			//全部
			m_dic_rooms[(Byte)ERoomType.ROOM_TYPE_ALL].Add( m_rooms.Count - 1 );
			//我的
			if( _msg.Rooms[i].BlackPlayerID == MainPlayer.Instance.PlayerInfo.PlayerID || _msg.Rooms[i].WhitePlayerID == MainPlayer.Instance.PlayerInfo.PlayerID )
				m_dic_rooms[(Byte)ERoomType.ROOM_TYPE_MINE].Add( m_rooms.Count - 1 );
		}
	}
}
