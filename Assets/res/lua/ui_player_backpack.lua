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

    --t.mUIWidgets.text_vipmoney_name.text = "text_vipmoney_name"
    --t.mUIWidgets.text_money_name.text = "text_money_name"
    t.mUIWidgets.text_money_value.text = "32,432,653,234"
    t.mUIWidgets.text_vipmoney_value.text = "24,543"

    --t.mUIWidgets.text_safebox.text = "text_safebox"
    --t.mUIWidgets.text_hunt.text = "text_hunt"
end

function t:InitBtnLis()
    local content = t.mUIWidgets.gameobject_bagList.transform
    for i = 0, content.childCount - 1 do
        function func() t:OnBagClick(i) end
        content:GetChild(i).gameObject:GetComponent(UnityEngine.UI.Button).onClick:AddListener(func)
    end
end

function t:OnBagClick(_index)
    UnityLuaUtils.ShowUI( "ui_item_info")
end

function t:OnSafeboxClick()
    UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Common@NotOpen"), "", nil, nil);
end

function t:OnHuntClick()
    UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Common@NotOpen"), "", nil, nil);
end

return t