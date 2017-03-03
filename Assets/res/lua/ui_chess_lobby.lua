local t = { };

function t:OnLoaded() 
	--t.mUIWidgets.button_item.onClick:AddListener( t._onButtonItemClick );
	t.ScrollRectList = t.mUIWidgets.ScrollRect:GetComponent( ScrollRectList );
	t.ScrollRectList.OnItemVisible = function( _obj, _index )
		t:OnItemVisible( _obj, _index )
	end
	t.ScrollRectList:SetMaxItemCount( 100 );
end

function t:OnUnloaded()
	--t.mUIWidgets.button_item.onClick:RemoveAllListeners();
end

function t:PreShow() 
	
end

function t:OnItemVisible( _obj, _index )
	_obj.transform:FindChild( "text_audience" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = _index;
	_obj.transform:FindChild( "text_room" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = _index;
end


return t
