local t = { };

function t:OnLoaded()
    t.mUIWidgets.button_all.onClick:AddListener(t.OnAllClick)
    t.mUIWidgets.button_friends.onClick:AddListener(t.OnFriendsClick)
    t.mUIWidgets.button_blacklist.onClick:AddListener(t.OnBlacklistClick)
    t.mUIWidgets.button_search.onClick:AddListener(t.OnSearchClick)

    t.scrollrect_playershow = t.mUIWidgets.scrollrect_playershow:GetComponent(ScrollRectList)
    t.scrollrect_playershow.OnItemVisible = function(_obj, _index) t:OnItemVisible(_obj, _index) end

    local lis = PlayerOnlineSystem.Instance:GetPlayerInfoList(0)
    PlayerOnlineSystem.Instance:UpdateAppoint(lis)
end

function t:OnUnloaded()
    t.mUIWidgets.button_all.onClick:RemoveAllListeners()
    t.mUIWidgets.button_friends.onClick:RemoveAllListeners()
    t.mUIWidgets.button_blacklist.onClick:RemoveAllListeners()
    t.mUIWidgets.button_search.onClick:RemoveAllListeners()
end

function t:PreShow()
    t.scrollrect_playershow:SetMaxItemCount(PlayerOnlineSystem.Instance:GetPlayerInfoCount(3))

    local content = t.mUIWidgets.scrollrect_playershow.transform:FindChild("content")
    for i = 0, content.childCount - 1 do
        local muliButton = content:GetChild(i):FindChild("button_playeroption").gameObject:GetComponent(MulitButton);
        muliButton:SetOnClickFunction(0, function(value) t:OnClickPlayerInfo(value) end)
        muliButton:SetOnClickFunction(1, function(value) t:OnClickAddFriend(value) end)
        muliButton:SetOnClickFunction(2, function(value) t:OnClickSendInfo(value) end)
        muliButton:SetOnClickFunction(3, function(value) t:OnClickAddBlackList(value) end)
    end
end

function t:OnAllClick()
    local lis = PlayerOnlineSystem.Instance:GetPlayerInfoList(0)
    PlayerOnlineSystem.Instance:UpdateAppoint(lis)
    t.scrollrect_playershow:SetMaxItemCount(PlayerOnlineSystem.Instance:GetPlayerInfoCount(3))
end

function t:OnFriendsClick()
    local lis = PlayerOnlineSystem.Instance:GetPlayerInfoList(1)
    PlayerOnlineSystem.Instance:UpdateAppoint(lis)
    t.scrollrect_playershow:SetMaxItemCount(PlayerOnlineSystem.Instance:GetPlayerInfoCount(3))
end

function t:OnBlacklistClick()
    local lis = PlayerOnlineSystem.Instance:GetPlayerInfoList(2)
    PlayerOnlineSystem.Instance:UpdateAppoint(lis)
    t.scrollrect_playershow:SetMaxItemCount(PlayerOnlineSystem.Instance:GetPlayerInfoCount(3))
end

function t:OnSearchClick()
    info = PlayerOnlineSystem.Instance:GetPlayerInfoByName(t.mUIWidgets.edit_search.text, 3)
    if (info == nil) then
        UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Player@NoPlayer"), "", nil, nil);
        return
    end
    lis = PlayerOnlineSystem.Instance:GetPlayerInfoList(3)
    lis:Clear()
    lis:Add(info.PlayerID)
    t.scrollrect_playershow:SetMaxItemCount(PlayerOnlineSystem.Instance:GetPlayerInfoCount(3))
end

function t:OnItemVisible(_obj, _index)
    local player_info = PlayerOnlineSystem.Instance:GetPlayerInfoByIndex(_index, 3)
    if player_info == nil then
        _obj:SetActive(false)
        return
    end
    _obj.transform:FindChild("text_name").gameObject:GetComponent(UnityEngine.UI.Text).text = player_info.PlayerName
    _obj.transform:FindChild("text_level").gameObject:GetComponent(UnityEngine.UI.Text).text = player_info.Level .. UnityLuaUtils.GetLocaleString("Common@Level");
    _obj.transform:FindChild("text_win").gameObject:GetComponent(UnityEngine.UI.Text).text = player_info.WinCount .. UnityLuaUtils.GetLocaleString("Common@Win");
    _obj.transform:FindChild("text_lose").gameObject:GetComponent(UnityEngine.UI.Text).text = player_info.LossCount .. UnityLuaUtils.GetLocaleString("Common@Lose");
    _obj.transform:FindChild("text_state").gameObject:GetComponent(UnityEngine.UI.Text).text = UnityLuaUtils.GetLocaleString("Player@State" .. player_info.State)
end

function t:OnClickPlayerInfo(_index)
    local player_info = PlayerOnlineSystem.Instance:GetPlayerInfoByIndex(_index, 3)
    print("PlayerInfo: " .. player_info.PlayerName)
end

function t:OnClickAddFriend(_index)
    local player_info = PlayerOnlineSystem.Instance:GetPlayerInfoByIndex(_index, 3)
    print("AddFriend: " .. player_info.PlayerName)
end

function t:OnClickSendInfo(_index)
    local player_info = PlayerOnlineSystem.Instance:GetPlayerInfoByIndex(_index, 3)
    print("SendInfo: " .. player_info.PlayerName)
end

function t:OnClickAddBlackList(_index)
    local player_info = PlayerOnlineSystem.Instance:GetPlayerInfoByIndex(_index, 3)
    print("AddBlackList: " .. player_info.PlayerName)
end

return t