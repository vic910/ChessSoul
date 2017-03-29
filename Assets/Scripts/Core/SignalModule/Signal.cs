
using SLua;

namespace Groot
{
	[CustomLuaClassAttribute]
	public enum SignalId
	{
		#region NetworkState
		NetworkState_EnterConnected,            //进入网络连接状态
		#endregion
		#region Login
		Login_Success,                          //登录成功
		Login_ForceLogin,						//强制登录
		#endregion

		Chat_ReceiveChat,                       //收到聊天信息

		Item_Update,							//物品更新

        Money_Update,                           //银两更新

        Gold_Update,                            //元宝更新

		Test
	}
}
