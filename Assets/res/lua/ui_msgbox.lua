local t = { }

-- 这里是所有的UIManager调用的接口
function t.OnLoaded( ) 
	t.mUIWidgets.btnConfirm.onClick:AddListener( t.onConfirmClicked )
end

function t.PreShow( _preUIName, ... ) 
	local type, arg1, arg2, arg3, arg4, arg5 = ...
	--print( _preUIName .. "UIName")
	print( type );
	print( arg1 );  
	print( arg2 );
	print( arg3 );
	print( arg4 );
	print( arg5 );
	if type == 1 then 
		t.mUIWidgets.textContent.text = arg1;
		t.mUIWidgets.textBtnConfirm.text = arg2;
		t.mConfirmCallback = arg3;
	--else if ( type == 2 )
		--// TODO
	--print("this is ui_title show!!")
	end
end

-- 这里是所有的自定义方法
function t.onConfirmClicked()
	UnityLuaUtils.HideUI( "ui_msgbox" );
	UnityLuaUtils.Invoke( t.mConfirmCallback );
end

function t.onCancelClicked()
	UnityLuaUtils.HideUI( "ui_msgbox" );
end
return t
