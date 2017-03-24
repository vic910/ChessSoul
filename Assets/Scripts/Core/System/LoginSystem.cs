using System;
using System.Collections;
using System.Collections.Generic;
using Core.App;
using Groot;
using Groot.Network;
using Utility;
using Weiqi.UI;

namespace Weiqi
{
    public class LoginSystem : GrootSingleton<LoginSystem>
    {
        public string CurAccount = string.Empty;
        public string CurPassword = string.Empty;

        public override void Initialize()
        {
            NetManager.Instance.Register<GC_LoginFailedMsg>(_onPacketArrived);
            NetManager.Instance.Register<GC_LoginOK>(_onPacketArrived);
            NetManager.Instance.RequestConnect();
        }

        public override void Uninitialize()
        {
            NetManager.Instance.Unregister<GC_LoginFailedMsg>();
            NetManager.Instance.Unregister<GC_LoginOK>();
        }

        private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_LoginFailedMsg _msg)
        {
            switch ((GC_LoginFailedMsg.ReasonInfo)_msg.Reason)
            {
                case GC_LoginFailedMsg.ReasonInfo.RET_SUCCESS:
                case GC_LoginFailedMsg.ReasonInfo.RET_SUCCESS_WITH_ITEM:
                    break;
                case GC_LoginFailedMsg.ReasonInfo.ERROR_HAS_LOGINED:
                    {
                        string tip = Locale.Instance[string.Format("Login@{0}", (GC_LoginFailedMsg.ReasonInfo)(_msg.Reason)).ToString()];
                        NetManager.Instance.RequestDisConnect();
                        UI_MessageBox.Show(tip, Locale.Instance["Common@Confirm"], Locale.Instance["Common@Cancel"], () =>
                       {
                           SignalSystem.FireSignal(SignalId.Login_ForceLogin);
                       });
                    }
                    break;
                default:
                    {
                        string tip = Locale.Instance[string.Format("Login@{0}", (GC_LoginFailedMsg.ReasonInfo)(_msg.Reason)).ToString()];
                        if (tip == string.Empty)
                            UI_MessageBox.Show(((GC_LoginFailedMsg.ReasonInfo)(_msg.Reason)).ToString());
                        else
                            UI_MessageBox.Show(tip);
                    }
                    break;
            }
            WaitForResponse.Release();
        }

        private void _onPacketArrived(Int32 _stream_id, PacketType _packet_type, GC_LoginOK _msg)
        {
            WaitForResponse.Release();
            Log.Info("收到玩家数据");
            MainPlayer.Instance.InitializePlayerInfo(_msg.PlayerInfo);
            SignalSystem.FireSignal(SignalId.Login_Success);

            //登陆成功后更新本地配置中的账户名
            LocalConfigSystem.Instacne.UpdateCurAccount(CurAccount);
            LocalConfigSystem.Instacne.Update("CurrentPassword", CurPassword);
        }
    }

}
