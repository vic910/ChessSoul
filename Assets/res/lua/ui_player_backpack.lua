local t = { };

function t:OnLoaded()
    t:InitBtnLis()

    t.mUIWidgets.button_safebox.onClick:AddListener(t.OnSafeboxClick)
    t.mUIWidgets.button_hunt.onClick:AddListener(t.OnHuntClick)
end

function t:OnUnloaded()

end

function t:PreShow()
    -- t.mUIWidget.image_vipmoney.Sprite
    -- t.mUIWidget.image_money.Sprite

    -- t.mUIWidgets.text_vipmoney_name.text = "text_vipmoney_name"
    -- t.mUIWidgets.text_money_name.text = "text_money_name"
    t.mUIWidgets.text_money_value.text = MainPlayer.Instance.PlayerInfo.Money
    t.mUIWidgets.text_vipmoney_value.text = MainPlayer.Instance.PlayerInfo.Gold

    -- t.mUIWidgets.text_safebox.text = "text_safebox"
    -- t.mUIWidgets.text_hunt.text = "text_hunt"
    local content = t.mUIWidgets.gameobject_bagList.transform
    local mItem = ItemSystem.Instance:GetAllMyItem()
    for i = 0, mItem.Count - 1 do
	content:GetChild(i).gameObject:GetComponent(UnityEngine.UI.Image).sprite = nil
	end
end

function t:InitBtnLis()
    local content = t.mUIWidgets.gameobject_bagList.transform
    for i = 0, content.childCount - 1 do
        function func() t:OnBagClick(i) end
        content:GetChild(i).gameObject:GetComponent(UnityEngine.UI.Button).onClick:AddListener(func)
    end
end

function t:OnBagClick(_index)
    local propItem = ItemSystem.Instance:GetMyItemIDByIndex(_index)
    if(propItem == nil) then
	return
    end
    UnityLuaUtils.ShowUI("ui_item_info", propItem.PropID, propItem.Count, "Sale")
end

function t:OnSafeboxClick()
    UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Common@NotOpen"), "", nil, nil);
end

function t:OnHuntClick()
    UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Common@NotOpen"), "", nil, nil);
end

return t