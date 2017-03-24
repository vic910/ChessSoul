local t = { };



function t:OnLoaded()
    t.mUIWidgets.button_close.onClick:AddListener(t.OnCloseClick)
    t.mUIWidgets.button_sale.onClick:AddListener(t.OnSaleClick)
end


function t:OnUnloaded()
    t.mUIWidgets.button_close.onClick:RemoveAllListeners()
    t.mUIWidgets.button_num_sub.onClick:RemoveAllListeners()
    t.mUIWidgets.button_num_add.onClick:RemoveAllListeners()
    t.mUIWidgets.button_sale.onClick:RemoveAllListeners()
end

function t:PreShow(...)
    local itemID, itemCount, handleName = ...
    local itemInfo = ItemSystem.Instance:GetItemAttr(itemID)
    -- t.mUIWidgets.text_title.text
    t.mUIWidgets.text_name.text = itemInfo.Name
    t.mUIWidgets.text_info.text = itemInfo.Desc

    -- t.mUIWidgets.text_unitprice_name.text
    if itemInfo.CurrMoneyType == 1 then
        t.mUIWidgets.text_unitprice_value.text = itemInfo.PriceMoney
        -- t.mUIWidgets.text_unitprice_value.text = "PriceMoney"	
    else
        t.mUIWidgets.text_unitprice_value.text = itemInfo.PriceGold
        -- t.mUIWidgets.text_unitprice_value.text = "PriceGold"
    end
    -- t.mUIWidgets.text_saleprice_name.text
    t.mUIWidgets.text_saleprice_value.text = itemInfo.RecycleMoney
    -- t.mUIWidgets.text_saleprice_unit.text = "PriceMoney"

    t.mUIWidgets.text_num_value.text = itemCount
    function func() t:OnAddClick(itemCount) end
    t.mUIWidgets.button_num_sub.onClick:AddListener(t.OnSubClick)
    t.mUIWidgets.button_num_add.onClick:AddListener(func)

    t.mUIWidgets.text_sale_name.text = handleName
end

function t:OnCloseClick()
    UnityLuaUtils.HideUI("ui_item_info")
end


function t:OnAddClick(_count)
    local value = tonumber(t.mUIWidgets.text_num_value.text)
    value = value + 1
    if value > _count then
        value = _count
    end
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
