local t={};

function t:OnLoaded() 
	t.mUIWidgets.btn_open_send.onClick:AddListener( t._onSendButtonClick );
	t.mUIWidgets.button_receive.onClick:AddListener( function()
		t:OnTabClick( 0 );
	end );
	t.mUIWidgets.button_send.onClick:AddListener( function()
		t:OnTabClick( 1 );
	end  );
	t.ScrollRectList = t.mUIWidgets.ScrollRect:GetComponent( ScrollRectList );
	t.ScrollRectList.OnItemVisible = function( _obj, _index )
		t:OnItemVisible( _obj, _index );
	end
	EMailSystem.Instance:SendMessageGetSentAll();
	t.cur_tab = 0;
end

function t:OnUnloaded()
	t.mUIWidgets.btn_open_send.onClick:RemoveAllListeners();
	t.mUIWidgets.button_receive.onClick:RemoveAllListeners();
	t.mUIWidgets.button_send.onClick:RemoveAllListeners();
	local content = t.mUIWidgets.ScrollRect.transform:FindChild( "Content" );
	for i = 0, content.childCount - 1 do
		content:GetChild( i ):FindChild( "btn_click" ).gameObject:GetComponent( UnityEngine.UI.Button ).onClick:RemoveAllListeners();
	end
end

function t:PreShow()
	t.ScrollRectList:SetMaxItemCount( EMailSystem.Instance:GetEmailCount( t.cur_tab ) );
end

function t:OnTabClick( _type )
	if t.cur_tab == _type then
		return;
	end
	t.cur_tab = _type;
	t.ScrollRectList:SetMaxItemCount( EMailSystem.Instance:GetEmailCount( t.cur_tab ) );
end

function t:_onSendButtonClick()
	UnityLuaUtils.ShowUI( "ui_email_send" );
end

function t:OnEmailClick( _index )
	print( _index );
end

function t:OnItemVisible( _obj, _index )
	local data = EMailSystem.Instance:GetEmailInfo( t.cur_tab, _index );

	_obj.transform:FindChild( "btn_click" ).gameObject:GetComponent( UnityEngine.UI.Button ).onClick:AddListener( function()
		t:OnEmailClick( _index );
	end );	


	if data.cRead == 0 then
		--_obj.transform:FindChild( "image_icon" ).gameObject:GetComponent( UnityEngine.UI.Image ).sprite = 
	else
		--_obj.transform:FindChild( "image_icon" ).gameObject:GetComponent( UnityEngine.UI.Image ).sprite = 
	end
	_obj.transform:FindChild( "text_title" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = UnityLuaUtils.GetLocaleString( "Email@Title" )..data.szTitle;
	if t.cur_tab == 0 then
		_obj.transform:FindChild( "text_sender" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = UnityLuaUtils.GetLocaleString( "Email@Sender" )..data.szSender;
	else
		_obj.transform:FindChild( "text_sender" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = UnityLuaUtils.GetLocaleString( "Email@Receiver" )..data.szSender;
	end
	_obj.transform:FindChild( "text_time" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = data.stSendTime.Year.."."..data.stSendTime.Month.."."..data.stSendTime.Day.." "..data.stSendTime.Hour..":"..data.stSendTime.Minute;
end


return t;