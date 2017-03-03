local t = { };

function t:OnLoaded() 
	t.mUIWidgets.button_item.onClick:AddListener( t._onButtonItemClick );
	t.mUIWidgets.button_record.onClick:AddListener( t._onButtonRecordClick );
	t.mUIWidgets.button_jetton.onClick:AddListener( t._onButtonJettonClick );
	t.mUIWidgets.button_pay.onClick:AddListener( t._onButtonPayClick );
	t.mUIWidgets.button_option.onClick:AddListener( t._onButtonOptionClick );
end

function t:OnUnloaded()
	t.mUIWidgets.button_item.onClick:RemoveAllListeners();
	t.mUIWidgets.button_record.onClick:RemoveAllListeners();
	t.mUIWidgets.button_jetton.onClick:RemoveAllListeners();
	t.mUIWidgets.button_pay.onClick:RemoveAllListeners();
	t.mUIWidgets.button_option.onClick:RemoveAllListeners();
end

function t:PreShow() 
	t:_updatePlayerInfo();
end

function t:_onButtonItemClick()
	UnityLuaUtils.ShowSingleMsgBox( UnityLuaUtils.GetLocaleString( "Common@NotOpen" ), "", nil, nil );
end

function t:_onButtonRecordClick()
	UnityLuaUtils.ShowSingleMsgBox( UnityLuaUtils.GetLocaleString( "Common@NotOpen" ), "", nil, nil );
end

function t:_onButtonJettonClick()
	UnityLuaUtils.ShowSingleMsgBox( UnityLuaUtils.GetLocaleString( "Common@NotOpen" ), "", nil, nil );
end

function t:_onButtonPayClick()
	UnityLuaUtils.ShowSingleMsgBox( UnityLuaUtils.GetLocaleString( "Common@NotOpen" ), "", nil, nil );
end

function t:_onButtonOptionClick()
	UnityLuaUtils.ShowUI( "ui_option" );
end

function t:_updatePlayerInfo()
	t.mUIWidgets.text_name.text = MainPlayer.Instance.PlayerInfo.PlayerName;
	t.mUIWidgets.text_level.text = MainPlayer.Instance.PlayerInfo.Level;
	t.mUIWidgets.text_score.text = MainPlayer.Instance.PlayerInfo.LevelScore;
	local area_info = PlayerInfoConfig.Instance:GetAreaInfo( MainPlayer.Instance.PlayerInfo.AreaID );
	t.mUIWidgets.text_area.text = area_info.ShortName;
	local liveness_info = PlayerInfoConfig.Instance:GetLivenessInfo( MainPlayer.Instance.PlayerInfo.Liveness );
	t.mUIWidgets.text_activity.text = liveness_info.Name.."("..MainPlayer.Instance.PlayerInfo.Liveness..")";
	if MainPlayer.Instance.PlayerInfo.ClubName == "" then
		t.mUIWidgets.text_union.text = UnityLuaUtils.GetLocaleString( "Common@No" );
	else
		t.mUIWidgets.text_union.text = MainPlayer.Instance.PlayerInfo.ClubName;
	end
	if MainPlayer.Instance.PlayerInfo.ClubPositionName == "" then
		t.mUIWidgets.text_job.text = UnityLuaUtils.GetLocaleString( "Common@No" );
	else
		t.mUIWidgets.text_job.text = MainPlayer.Instance.PlayerInfo.ClubPositionName;
	end
	t.mUIWidgets.text_money.text = MainPlayer.Instance.PlayerInfo.Money;
end

return t
