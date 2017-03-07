local t = { };

function t:OnLoaded() 
	t.ScrollRectList = t.mUIWidgets.ScrollRect:GetComponent( ScrollRectList );
	t.ScrollRectList.OnItemVisible = function( _obj, _index )
		t:OnItemVisible( _obj, _index );
	end
	t.ScrollRectList:SetMaxItemCount( 100 );
	local content = t.mUIWidgets.ScrollRect.transform:FindChild( "Content" );
	for i = 0, content.childCount - 1 do
		content:GetChild( i ):FindChild( "button_join" ).gameObject:GetComponent( UnityEngine.UI.Button ).onClick:AddListener( t.OnItemJoinClick );
	end
end

function t:OnUnloaded()
	local content = t.mUIWidgets.ScrollRect.transform:FindChild( "Content" );
	for i = 0, content.childCount - 1 do
		content:GetChild( i ):FindChild( "button_join" ).gameObject:GetComponent( UnityEngine.UI.Button ).onClick:RemoveAllListeners();
	end
end

function t:PreShow() 
	
end

function t:OnItemJoinClick()
	print( UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name );
	--print( type( tonumber( UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name ) ) );
end

function t:OnItemVisible( _obj, _index )
	_obj.transform:FindChild( "text_audience" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = _index;
	_obj.transform:FindChild( "text_room" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = _index;
end


return t
