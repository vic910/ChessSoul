local t = { };

function t:OnLoaded()
    t.mUIWidgets.button_type_add.onClick:AddListener(t.OnClickAddType)
    t.mUIWidgets.button_type_sub.onClick:AddListener(t.OnClickSubType)
    t.mUIWidgets.button_type_box.onClick:AddListener(t.OnClickBoxType)
    t.mUIWidgets.button_type_other.onClick:AddListener(t.OnClickOtherType)

    t.mUIWidgets.button_pay.onClick:AddListener(t.OnClickPay)
    t.mUIWidgets.button_shoppingcar.onClick:AddListener(t.OnClickShoppingCar)

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

    -- t.mUIWidgets.image_player_gold.Sprite
    -- t.mUIWidgets.image_player_money.text
    -- t.mUIWidgets.text_player_gold_name.Sprite
    -- t.mUIWidgets.text_player_money_name.text
    Groot.SignalSystem.Register(Groot.SignalId.ShoppingCar_Update, t.UpdateShoppingCarShow)
    Groot.SignalSystem.Register(Groot.SignalId.Money_Update, t.UpdateMoneyShow)
    Groot.SignalSystem.Register(Groot.SignalId.Gold_Update, t.UpdateMoneyShow)
    Groot.SignalSystem.Register(Groot.SignalId.BuyFail_Update, t.BuyFail)

    t:InitSaleItemList()
    t:UpdateShoppingCarShow()
    t:UpdateMoneyShow()

end

function t:OnHide()
    Groot.SignalSystem.Unregister(Groot.SignalId.ShoppingCar_Update, t.UpdateShoppingCarShow);
    Groot.SignalSystem.Unregister(Groot.SignalId.Money_Update, t.UpdateMoneyShow);
    Groot.SignalSystem.Unregister(Groot.SignalId.Gold_Update, t.UpdateMoneyShow);
    Groot.SignalSystem.Unregister(Groot.SignalId.BuyFail_Update, t.BuyFail);
end

function t:BuyFail()
    local ret = ShopSystem.Instance:GetBuyRet()
    local saleItem = ShopSystem.Instance:GetSaleItemBySaleID(ret[0].SaleID)
    local itemInfo = ItemSystem.Instance:GetItemAttr(saleItem.ItemID)

    if ret[0].Status == 0 then
        local moneyType
        if (saleItem.MoneyType == 1) then
            moneyType = "money"
        else
            moneyType = "gold"
        end
        local str = itemInfo.Name .. " * " .. ret[0].BuyCount .. "\ntotal use" .. saleItem.Price * ret[0].BuyCount .. "  " .. moneyType
        UnityLuaUtils.ShowSingleMsgBox(str, "Confirm", nil, nil);
    else
        UnityLuaUtils.ShowSingleMsgBox(ret[0].Status, "Confirm", nil, nil);
    end
end

function t:OnClickAddType()
    local showList = ShopSystem.Instance:GetSaleItemAttr(0)
    ShopSystem.Instance:SetShowList(showList)
    t.scrollrect_playershow:SetMaxItemCount(ShopSystem.Instance:GetSaleItemAttr(4).Count)
end

function t:OnClickSubType()
    local showList = ShopSystem.Instance:GetSaleItemAttr(1)
    ShopSystem.Instance:SetShowList(showList)
    t.scrollrect_playershow:SetMaxItemCount(ShopSystem.Instance:GetSaleItemAttr(4).Count)
end

function t:OnClickBoxType()
    local showList = ShopSystem.Instance:GetSaleItemAttr(2)
    ShopSystem.Instance:SetShowList(showList)
    t.scrollrect_playershow:SetMaxItemCount(ShopSystem.Instance:GetSaleItemAttr(4).Count)
end

function t:OnClickOtherType()
    local showList = ShopSystem.Instance:GetSaleItemAttr(3)
    ShopSystem.Instance:SetShowList(showList)
    t.scrollrect_playershow:SetMaxItemCount(ShopSystem.Instance:GetSaleItemAttr(4).Count)
end

function t:OnClickPay()
    UnityLuaUtils.ShowSingleMsgBox(UnityLuaUtils.GetLocaleString("Common@NotOpen"), "", nil, nil);
end

function t:OnClickShoppingCar()
    UnityLuaUtils.ShowUI("ui_shoppingcar")
end

function t:UpdateMoneyShow()
    t.mUIWidgets.text_player_gold_value.text = MainPlayer.Instance.PlayerInfo.Gold
    t.mUIWidgets.text_player_money_value.text = MainPlayer.Instance.PlayerInfo.Money
end

function t:UpdateShoppingCarShow()
    t.mUIWidgets.text_count.text = ShopSystem.Instance:GetShoppingcarItemList().Count
end

function t:InitSaleItemList()

    local defaultShowList = ShopSystem.Instance:GetSaleItemAttr(0)
    ShopSystem.Instance:SetShowList(defaultShowList)

    t.scrollrect_playershow = t.mUIWidgets.scrollrect_playershow:GetComponent(ScrollRectList)
    t.scrollrect_playershow.OnItemVisible = function(_obj, _index) t:OnItemVisible(_obj, _index) end

    t.scrollrect_playershow:SetMaxItemCount(ShopSystem.Instance:GetSaleItemAttr(4).Count)

    local content = t.mUIWidgets.scrollrect_playershow.transform:FindChild("content")

    for i = 0, content.childCount - 1 do
        function func() t:OnClickShowItemInfo(i) end
        content:GetChild(i).gameObject:GetComponent(UnityEngine.UI.Button).onClick:AddListener(func)
    end
end

function t:OnClickShowItemInfo(_index)
    local saleItem_info = ShopSystem.Instance:GetItemInfo(_index, 4)
    local propItem = ItemSystem.Instance:GetItemAttr(saleItem_info.ItemID)
    if (propItem == nil) then
        return
    end
    -- UnityLuaUtils.ShowUI("ui_item_info", propItem.PropID, 1, 1, saleItem_info.ItemCount, "AddShoppingCar")
    UnityLuaUtils.ShowUI("ui_item_info", propItem.PropID, 1, 1, saleItem_info.ItemCount, "BuyItem")
end


function t:OnItemVisible(_obj, _index)

    local saleItem_info = ShopSystem.Instance:GetItemInfo(_index, 4)

    if saleItem_info == nil then
        return
    end

    local item_info = ItemSystem.Instance:GetItemAttr(saleItem_info.ItemID)
    if item_info == nil then
        return
    end
    _obj.transform:FindChild("image_good/text_good").gameObject:GetComponent(UnityEngine.UI.Text).text = item_info.Name
    _obj.transform:FindChild("image_good_value/text_good_value").gameObject:GetComponent(UnityEngine.UI.Text).text = tostring(saleItem_info.Price);

    local unit;

    if saleItem_info.MoneyType == 1 then
        unit = "money"
    else
        unit = "gold"
    end

    _obj.transform:FindChild("image_good_value/text_good_value/text_good_value_unit").gameObject:GetComponent(UnityEngine.UI.Text).text = unit;
end

return t