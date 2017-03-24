local t = { };

function t:OnLoaded()
    t.mUIWidgets.button_close.onClick:AddListener(t.OnCloseClick)
    t.mUIWidgets.button_num_sub.onClick:AddListener(t.OnSubClick)
    t.mUIWidgets.button_num_add.onClick:AddListener(t.OnAddClick)
    t.mUIWidgets.button_sale.onClick:AddListener(t.OnSaleClick)
end

function t:OnUnloaded()
    t.mUIWidgets.button_close.onClick:RemoveAllListeners()
    t.mUIWidgets.button_num_sub.onClick:RemoveAllListeners()
    t.mUIWidgets.button_num_add.onClick:RemoveAllListeners()
    t.mUIWidgets.button_sale.onClick:RemoveAllListeners()
end

function t:PreShow( ...)
    -- t.mUIWidgets.text_title.text
    -- t.mUIWidgets.text_name.text
    -- t.mUIWidgets.text_info.text
    -- t.mUIWidgets.text_unitprice_name.text
    -- t.mUIWidgets.text_unitprice_value.text
    -- t.mUIWidgets.text_unitprice_unit.text
    -- t.mUIWidgets.text_saleprice_name.text
    -- t.mUIWidgets.text_saleprice_value.text
    -- t.mUIWidgets.text_saleprice_unit.text
    local index, handleName = ...
    print(index)
    t.mUIWidgets.text_num_value.text = "0"
    t.mUIWidgets.text_sale_name.text = handleName
end

function t:OnCloseClick()
    UnityLuaUtils.HideUI("ui_item_info")
end

function t:OnAddClick()
    local value = tonumber(t.mUIWidgets.text_num_value.text)
    value = value + 1
    t.mUIWidgets.text_num_value.text = tostring(value)
end

function t:OnSubClick()
    local value = tonumber(t.mUIWidgets.text_num_value.text)
    value = value - 1
    if value < 0 then
        value = 0
    end
    t.mUIWidgets.text_num_value.text = tostring(value)
end

function t:OnSaleClick()
    print("Sale")
end

return t
