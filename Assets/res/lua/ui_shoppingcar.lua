local t = { };

function t:OnLoaded()
    t.mUIWidgets.button_type_name.onClick:AddListener(t.OnClickNameType)
    t.mUIWidgets.button_type_num.onClick:AddListener(t.OnClickNumType)
    t.mUIWidgets.button_type_cost.onClick:AddListener(t.OnClickCostType)
    t.mUIWidgets.button_pay.onClick:AddListener(t.OnClickPay)


    t.scrollrect_playershow = t.mUIWidgets.scrollrect_playershow:GetComponent(ScrollRectList)
    t.scrollrect_playershow.OnItemVisible = function(_obj, _index) t:OnItemVisible(_obj, _index) end

end

function t:OnUnloaded()
    t.mUIWidgets.button_type_name.onClick:RemoveAllListeners()
    t.mUIWidgets.button_type_num.onClick:RemoveAllListeners()
    t.mUIWidgets.button_type_cost.onClick:RemoveAllListeners()
    t.mUIWidgets.button_pay.onClick:RemoveAllListeners()
end

function t:PreShow()
    --    t.scrollrect_playershow:SetMaxItemCount(PlayerOnlineSystem.Instance:GetPlayerInfoCount(3))

    function func()
        t.scrollrect_playershow:SetMaxItemCount(ShopSystem.Instance:GetShoppingcarItemList().Count)
        t:ShowShoppingTotal()
    end
    Groot.SignalSystem.Register(Groot.SignalId.ShoppingCar_Update, func);
    t:InitShoppingCarItemList()
    t:UpdateMoneyShow()
    t:ShowShoppingTotal()

end

function t:OnClickNameType()

end

function t:OnClickNumType()

end

function t:OnClickCostType()

end

function t:OnClickPay()
    local str = "Total: " .. ShopSystem.Instance:GetShoppingcarItemList().Count .. "  goods\n\n"
    str = str .. "Cost: " .. t.mUIWidgets.text_totalcast_value.text .. t.mUIWidgets.text_totalcast_gold_unit.text
    str = str .. "  " .. t.mUIWidgets.text_totalcast_money_value.text .. t.mUIWidgets.text_totalcast_money_unit.text
    UnityLuaUtils.ShowSingleMsgBox(str, "Confirm Buy", t.ConfirmBuy, t.ConcelBuy);
end

function t:ConfirmBuy()
    ShopSystem.Instance:BuyItemToSystem()
end

function t:ConcelBuy()

end


function t:UpdateMoneyShow()
    t.mUIWidgets.text_player_gold_value.text = MainPlayer.Instance.PlayerInfo.Money
    t.mUIWidgets.text_player_money_value.text = MainPlayer.Instance.PlayerInfo.Gold
end

function t:ShowShoppingTotal()
    local saleItem_infoList = ShopSystem.Instance:GetShoppingcarItemList()
    local money = 0;
    local gold = 0;

    for i = 0, saleItem_infoList.Count - 1 do
        saleItem = ShopSystem.Instance:GetSaleItem(saleItem_infoList[i].PropID)
        if saleItem.MoneyType == 1 then
            money = money + saleItem.Price * saleItem_infoList[i].Count
        else
            gold = gold + saleItem.Price * saleItem_infoList[i].Count
        end
    end
    t.mUIWidgets.text_totalcast_gold_unit.text = "gold"
    t.mUIWidgets.text_totalcast_value.text = gold
    t.mUIWidgets.text_totalcast_money_value.text = money
    t.mUIWidgets.text_totalcast_money_unit.text = "money"
end



function t:InitShoppingCarItemList()
    t.scrollrect_playershow = t.mUIWidgets.scrollrect_playershow:GetComponent(ScrollRectList)
    t.scrollrect_playershow.OnItemVisible = function(_obj, _index) t:OnItemVisible(_obj, _index) end

    t.scrollrect_playershow:SetMaxItemCount(ShopSystem.Instance:GetShoppingcarItemList().Count)

    local content = t.mUIWidgets.scrollrect_playershow.transform:FindChild("content")

    for i = 0, content.childCount - 1 do
        function func() t:OnClickShowItemInfo(i) end
        content:GetChild(i).gameObject:GetComponent(UnityEngine.UI.Button).onClick:AddListener(func)
    end
end

function t:OnClickShowItemInfo(_index)
    local saleItem_infoList = ShopSystem.Instance:GetShoppingcarItemList()
    UnityLuaUtils.ShowUI("ui_item_info", saleItem_infoList[_index].PropID, saleItem_infoList[_index].Count, 0, 0, "UpdateShoppingCar")
end

function t:OnItemVisible(_obj, _index)
    local saleItem_infoList = ShopSystem.Instance:GetShoppingcarItemList()

    local saleItem = ShopSystem.Instance:GetSaleItem(saleItem_infoList[_index].PropID)
    local item_info = ItemSystem.Instance:GetItemAttr(saleItem_infoList[_index].PropID)
    if item_info == nil then
        return
    end

    _obj.transform:FindChild("image_good/text_good").gameObject:GetComponent(UnityEngine.UI.Text).text = item_info.Name
    _obj.transform:FindChild("text_count").gameObject:GetComponent(UnityEngine.UI.Text).text = tostring(saleItem_infoList[_index].Count)

    local unit
    local price

    if saleItem.MoneyType == 1 then
        price = saleItem.Price * saleItem_infoList[_index].Count
        unit = "money"
    else
        price = saleItem.Price * saleItem_infoList[_index].Count
        unit = "gold"
    end
    _obj.transform:FindChild("image_good_value/text_good_value").gameObject:GetComponent(UnityEngine.UI.Text).text = tostring(price)
    _obj.transform:FindChild("image_good_value/text_good_value/text_good_value_unit").gameObject:GetComponent(UnityEngine.UI.Text).text = unit;
end

return t