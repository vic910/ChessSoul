local t = { }

-- 这里是所有的UIManager调用的接口
function t:OnLoaded()
    t.mUIWidgets.toggle_invite.onValueChanged:AddListener( function(value) t:_onToggleInviteValueChanged(value) end);
    t.mUIWidgets.toggle_email.onValueChanged:AddListener( function(value) t:_onToggleEmailValueChanged(value) end);
    t.mUIWidgets.toggle_transaction.onValueChanged:AddListener( function(value) t:_onToggleTransactionValueChanged(value) end);
    t.mUIWidgets.toggle_sound.onValueChanged:AddListener( function(value) t:_onToggleSoundValueChanged(value) end);

    t.mUIWidgets.button_match.onClick:AddListener(t._onButtonMatchClick);
    t.mUIWidgets.button_advise.onClick:AddListener(t._onButtonAdviseClick);
    t.mUIWidgets.button_help.onClick:AddListener(t._onButtonHelpClick);
    t.mUIWidgets.button_version.onClick:AddListener(t._onButtonVersionClick);
end

function t:OnUnload()
    t.mUIWidgets.toggle_invite.onValueChang:RemoveAllListeners();
    t.mUIWidgets.toggle_email.onValueChang:RemoveAllListeners();
    t.mUIWidgets.toggle_transaction.onValueChang:RemoveAllListeners();
    t.mUIWidgets.toggle_sound.onValueChang:RemoveAllListeners();

    t.mUIWidgets.button_match.onClick:RemoveAllListeners();
    t.mUIWidgets.button_advise.onClick:RemoveAllListeners();
    t.mUIWidgets.button_help.onClick:RemoveAllListeners();
    t.mUIWidgets.button_version.onClick:RemoveAllListeners();
end

function t:PreShow()
    t._updateShowInfo()
end

function t:OnShow()
    -- print("this is ui_title show!!")
end

-- 这里是所有的自定义方法

-- Text显示
function t:_updateShowInfo()
    t.mUIWidgets.text_curAccount.text = LocalConfigSystem.Instacne:GetOptionConfig("CurrentAccount")
    t.mUIWidgets.toggle_sound.isOn = UnityLuaUtils.StringConvertToBool(LocalConfigSystem.Instacne:GetOptionConfig("OpenSound"))
end

-- Toggle响应方法
function t:_onToggleInviteValueChanged(value)
    print(value)
end

function t:_onToggleEmailValueChanged(value)
    print(value)
end

function t:_onToggleTransactionValueChanged(value)
    print(value)
end

function t:_onToggleSoundValueChanged(value)
    LocalConfigSystem.Instacne:Update("OpenSound", tostring(value))
end

-- Button响应方法

function t:_onButtonMatchClick()
    UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Common@NotOpen"), "", nil, nil);
end

function t:_onButtonAdviseClick()
    UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Common@NotOpen"), "", nil, nil);
end

function t:_onButtonHelpClick()
    UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Common@NotOpen"), "", nil, nil);
end

function t:_onButtonVersionClick()
    UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Common@NotOpen"), "", nil, nil);
end

--

return t
