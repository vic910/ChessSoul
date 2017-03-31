local t = { };

function t:OnLoaded()
    t.mUIWidgets.button_close.onClick:AddListener(t.OnCloseClick)
end

function t:OnUnloaded()
    t.mUIWidgets.button_close.onClick:RemoveAllListeners()
end

function t:PreShow(...)
    local itemID, curCount, minCount, maxCount, handleName = ...
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

    t.mUIWidgets.text_num_value.text = curCount

    function func() t:OnAddClick(maxCount) end
    t.mUIWidgets.button_num_add.onClick:AddListener(func)

    function func() t:OnSubClick(minCount) end
    t.mUIWidgets.button_num_sub.onClick:AddListener(func)


    t.mUIWidgets.text_sale_name.text = handleName

    -- Sale OnClick
    function func()
        t:OnSaleClick(itemID, handleName)
    end

    t.mUIWidgets.button_sale.onClick:AddListener(func)
end

function t:OnHide()
    t.mUIWidgets.button_num_sub.onClick:RemoveAllListeners()
    t.mUIWidgets.button_num_add.onClick:RemoveAllListeners()
    t.mUIWidgets.button_sale.onClick:RemoveAllListeners()
end

function t:OnCloseClick()
    UnityLuaUtils.HideUI("ui_item_info")
end


function t:OnAddClick(_count)
    local value = tonumber(t.mUIWidgets.text_num_value.text)
    value = value + 1
    if value > _count and _count ~= 0 then
        value = _count
    end
    t.mUIWidgets.text_num_value.text = tostring(value)
end


function t:OnSubClick(_count)
    local value = tonumber(t.mUIWidgets.text_num_value.text)
    value = value - 1
    if value < _count then
        value = _count
    end
    t.mUIWidgets.text_num_value.text = tostring(value)
end

function t:OnSaleClick(_id, _handleName)
    count = tonumber(t.mUIWidgets.text_num_value.text)
    if (count == nil) then
        return
    end

    if (_handleName == "Sale") then
        ItemSystem.Instance:SaleItemToSystem(_id, count)
    end

    if (_handleName == "AddShoppingCar") then
        ShopSystem.Instance:AddShoppingcarItemList(_id, count)
    end

    if (_handleName == "UpdateShoppingCar") then
        ShopSystem.Instance:SetShoppingcarItemList(_id, count)
    end

    t:OnCloseClick()
end

return t
