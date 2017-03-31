local t = {};

function t:OnLoaded() 
	t.mUIWidgets.button_living.onClick:AddListener( t.OnLivingClick );
	t.mUIWidgets.button_chess.onClick:AddListener( t.OnChessClick );
	t.mUIWidgets.button_explain.onClick:AddListener( t.OnExplainClick );
	t.mUIWidgets.button_mine.onClick:AddListener( t.OnMineClick );
	t.mUIWidgets.button_search.onClick:AddListener( t.OnSearchClick );

	t.ScrollRectList = t.mUIWidgets.ScrollRect:GetComponent( ScrollRectList );
	t.ScrollRectList.OnItemVisible = function( _obj, _index )
		t:OnItemVisible( _obj, _index );
	end
end

function t:OnUnloaded()
	t.mUIWidgets.button_living.onClick:RemoveAllListeners();
	t.mUIWidgets.button_chess.onClick:RemoveAllListeners();
	t.mUIWidgets.button_explain.onClick:RemoveAllListeners();
	t.mUIWidgets.button_mine.onClick:RemoveAllListeners();
	t.mUIWidgets.button_search.onClick:RemoveAllListeners();

	local content = t.mUIWidgets.ScrollRect.transform:FindChild( "Content" );
	for i = 0, content.childCount - 1 do
		content:GetChild( i ):FindChild( "button_join" ).gameObject:GetComponent( UnityEngine.UI.Button ).onClick:RemoveAllListeners();
	end
end

function t:PreShow() 
	t.mCurSelectTab = 255;
	t.ScrollRectList:SetMaxItemCount( RoomSystem.Instance:GetRoomCount( t.mCurSelectTab ) );
end

function t:OnLivingClick()
	if t.mCurSelectTab == 4 then
		return;
	end
	t.mCurSelectTab = 4;
	t.ScrollRectList:SetMaxItemCount( RoomSystem.Instance:GetRoomCount( t.mCurSelectTab ) );
end

function t:OnChessClick()
	if t.mCurSelectTab == 6 then
		return;
	end
	t.mCurSelectTab = 6;
	t.ScrollRectList:SetMaxItemCount( RoomSystem.Instance:GetRoomCount( t.mCurSelectTab ) );
end

function t:OnExplainClick()
	if t.mCurSelectTab == 5 then
		return;
	end
	t.mCurSelectTab = 5;
	t.ScrollRectList:SetMaxItemCount( RoomSystem.Instance:GetRoomCount( t.mCurSelectTab ) );
end

function t:OnMineClick()
	if t.mCurSelectTab == 254 then
		return;
	end
	t.mCurSelectTab = 254;
	t.ScrollRectList:SetMaxItemCount( RoomSystem.Instance:GetRoomCount( t.mCurSelectTab ) );
end

function t:OnSearchClick()
	if t.mUIWidgets.edit_search.text == "" then
		UnityLuaUtils.ShowSingleMsgBox( UnityLuaUtils.GetLocaleString( "Room@NoRoom" ), "", nil, nil );
		return;
	end
	local count = RoomSystem.Instance:SearchRoom( tonumber( t.mUIWidgets.edit_search.text ) );
	if count == 0 then
		UnityLuaUtils.ShowSingleMsgBox( UnityLuaUtils.GetLocaleString( "Room@NoRoom" ), "", nil, nil );
		return;
	end
	t.mCurSelectTab = 253;
	t.ScrollRectList:SetMaxItemCount( count );
end

function t:OnItemJoinClick( _index )
	print( _index );
end

function t:OnItemVisible( _obj, _index )
	local room_info = RoomSystem.Instance:GetRoomInfo( t.mCurSelectTab, _index );
	if room_info == nil then
		_obj:SetActive( false );
		return;
	end

	local button = _obj.transform:FindChild( "button_join" ).gameObject:GetComponent( UnityEngine.UI.Button );
	button.onClick:RemoveAllListeners();
	button.onClick:AddListener( function() t:OnItemJoinClick( _index ); end );	

	_obj.transform:FindChild( "text_audience" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = UnityLuaUtils.GetLocaleString( "Common@People" )..room_info.PlayerCount;
	_obj.transform:FindChild( "text_room" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = room_info.RoomID..UnityLuaUtils.GetLocaleString( "Room@Room" );
	_obj.transform:FindChild( "text_type" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = UnityLuaUtils.GetLocaleString( "Room@Type"..room_info.RoomType );
	_obj.transform:FindChild( "text_progress" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = UnityLuaUtils.GetLocaleString( "Room@State"..room_info.State );
	_obj.transform:FindChild( "text_des" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = room_info.Desc;
	_obj.transform:FindChild( "text_black_name" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = room_info.BlackPlayer.."["..room_info.BlackLevel.."]";
	_obj.transform:FindChild( "text_white_name" ).gameObject:GetComponent( UnityEngine.UI.Text ).text = room_info.WhitePlayer.."["..room_info.WhiteLevel.."]";
end

return t
