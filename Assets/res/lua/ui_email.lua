local t={};

function t:OnLoaded() 
	t.mUIWidgets.Btn_sendMail.onClick:AddListener( t.OnSendMailHandler );
	t.mUIWidgets.Btn_saveAsTemp.onClick:AddListener( t.OnSaveAsTempHandler );
end

function t:OnUnloaded()
	t.mUIWidgets.Btn_sendMail.onClick:RemoveAllListeners();
	t.mUIWidgets.Btn_saveAsTemp.onClick:RemoveAllListeners();
end

function t:OnSendMailHandler()
	print("发送邮件被点击！！");
	local msg = SendMailMessage();
	msg.receiverAddress=t.mUIWidgets.Txt_receivePersonValue.text;
	msg.mailTitle=t.mUIWidgets.Txt_mailTitleValue.text;
	msg.mailContent=t.mUIWidgets.Txt_mailContentValue.text;
	Groot.Network.NetManager.Instance:SendMsg(msg);
end

function t:OnSaveAsTempHandler()
	print("保存邮件被点击！！");
	EMailSystem.Instance:SendMessageGetSentAll();
	--local msg=msg_MessageGetSentAll_CG();
	--Groot.Network.NetManager.Instance:SendMsg(msg);
end

return t;