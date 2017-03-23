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

	[DoNotToLua]
	public void InitializePlayerInfo( PlayerInfo _info )
	{
		   PlayerInfo = _info;
	}
}
