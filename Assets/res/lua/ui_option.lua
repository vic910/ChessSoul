local t = { }

-- 这里是所有的UIManager调用的接口
function t:OnLoaded() 
	t.mUIWidgets.test.onClick:AddListener( t._onTestButtonClick )
end

function t:OnShow() 
	--print("this is ui_title show!!")
end

function t:_onTestButtonClick()
	print( "test click" );
	local func = function()
		print( "msgbox click" );
	end
	local x,y,z = UnityLuaUtils.Test();
	print( x, y, z );

	print( Weiqi.MainPlayer.LuaInstance.PlayerInfo.Money );
	UnityLuaUtils.ShowSingleMsgBox( UnityLuaUtils.GetLocaleString( "Download@Error" ), UnityLuaUtils.GetLocaleString( "Common@Confirm" ), func, nil );
end

-- 这里是所有的自定义方法

return t
