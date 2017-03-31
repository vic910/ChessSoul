local t={};

function t:OnLoaded() 
	t.mUIWidgets.Btn_sendMail.onClick:AddListener( t.OnSendMailHandler );
end

function t:OnUnloaded()
	t.mUIWidgets.Btn_sendMail.onClick:RemoveAllListeners();
end

function t:OnSendMailHandler()
	local msg = SendMailMessage();
	msg.receiverAddress=t.mUIWidgets.Txt_receivePersonValue.text;
	msg.mailTitle=t.mUIWidgets.Txt_mailTitleValue.text;
	msg.mailContent=t.mUIWidgets.Txt_mailContentValue.text;
	UnityLuaUtils.SendMessage(msg);
	UnityLuaUtils.UIRetuenBack();
end

return t;