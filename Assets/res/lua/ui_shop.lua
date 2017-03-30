local t = { };

function t:OnLoaded()
    t.mUIWidgets.button_type_add.onClick:AddListener(t.OnClickAddType)
    t.mUIWidgets.button_type_sub.onClick:AddListener(t.OnClickSubType)
    t.mUIWidgets.button_type_box.onClick:AddListener(t.OnClickBoxType)
    t.mUIWidgets.button_type_other.onClick:AddListener(t.OnClickOtherType)
    t.mUIWidgets.button_pay.onClick:AddListener(t.OnClickPay)
    t.mUIWidgets.button_shoppingcar.onClick:AddListener(t.OnClickShoppingCar)

    t.scrollrect_playershow = t.mUIWidgets.scrollrect_playershow:GetComponent(ScrollRectList)
    t.scrollrect_playershow.OnItemVisible = function(_obj, _index) t:OnItemVisible(_obj, _index) end

end

function t:OnUnloaded()
    t.mUIWidgets.button_type_add.onClick:RemoveAllListeners()
    t.mUIWidgets.button_type_sub.onClick:RemoveAllListeners()
    t.mUIWidgets.button_type_box.onClick:RemoveAllListeners()
    t.mUIWidgets.button_type_other.onClick:RemoveAllListeners()
end

function t:PreShow()
    --    t.scrollrect_playershow:SetMaxItemCount(PlayerOnlineSystem.Instance:GetPlayerInfoCount(3))

    --    local content = t.mUIWidgets.scrollrect_playershow.transform:FindChild("content")
    --    for i = 0, content.childCount - 1 do
    --        local muliButton = content:GetChild(i).gameObject:GetComponent(UnityEngine.UI.Button)
    --    end

    -- t.mUIWidgets.button_type_add.gameObject:GetComponent(UnityEngine.UI.Text).text
    -- t.mUIWidgets.button_type_sub.gameObject:GetComponent(UnityEngine.UI.Text).text
    -- t.mUIWidgets.button_type_box.gameObject:GetComponent(UnityEngine.UI.Text).text
    -- t.mUIWidgets.button_type_other.gameObject:GetComponent(UnityEngine.UI.Text).text

    --t.mUIWidgets.image_player_gold.Sprite
    --t.mUIWidgets.image_player_money.text
    --t.mUIWidgets.text_player_gold_name.Sprite
    --t.mUIWidgets.text_player_money_name.text

    t:UdateMoneyShow()

end

function t:OnItemVisible(_obj, _index)
    --    local player_info = PlayerOnlineSystem.Instance:GetPlayerInfoByIndex(_index, 3)
    --    if player_info == nil then
    --        _obj:SetActive(false)
    --        return
    --    end
    --    _obj.transform:FindChild("text_name").gameObject:GetComponent(UnityEngine.UI.Text).text = player_info.PlayerName
    --    _obj.transform:FindChild("text_level").gameObject:GetComponent(UnityEngine.UI.Text).text = player_info.Level .. UnityLuaUtils.GetLocaleString("Common@Level");
    --    _obj.transform:FindChild("text_win").gameObject:GetComponent(UnityEngine.UI.Text).text = player_info.WinCount .. UnityLuaUtils.GetLocaleString("Common@Win");
    --    _obj.transform:FindChild("text_lose").gameObject:GetComponent(UnityEngine.UI.Text).text = player_info.LossCount .. UnityLuaUtils.GetLocaleString("Common@Lose");
    --    _obj.transform:FindChild("text_state").gameObject:GetComponent(UnityEngine.UI.Text).text = UnityLuaUtils.GetLocaleString("Player@State" .. player_info.State)
end

function t:OnClickAddType()
    print("OnClickAddType")
end

function t:OnClickSubType()
    print("OnClickSubType: ")
end

function t:OnClickBoxType()
    print("OnClickBoxType: ")
end

function t:OnClickOtherType()
    print("OnClickOtherType")
end

function t:OnClickPay()
    UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Common@NotOpen"), "", nil, nil);
end

function t:OnClickShoppingCar()
     UnityLuaUtils.ShowUI( "ui_shoppingcar")
end

function t:UdateMoneyShow()
    t.mUIWidgets.text_player_gold_value.text = MainPlayer.Instance.PlayerInfo.Money
    t.mUIWidgets.text_player_money_value.text = MainPlayer.Instance.PlayerInfo.Gold
end

return t