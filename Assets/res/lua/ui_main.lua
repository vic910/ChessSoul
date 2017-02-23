local t = { }

-- 这里是所有的UIManager调用的接口
function t:OnLoaded() 
	t.mUIWidgets.button_test.onClick:AddListener( t.onTestButtonClicked )

	for k,v in pairs( t.mUIWidgets ) do
		print( k )
	end
end

function t:OnShow() 
	--for k,v in pairs( t.mUIWidgets ) do
		--	print( k .. "    " .. m_tmp );
	--end
end

function t:OnUpdate()
end 

-- 这里是所有的自定义方法

function t:onTestButtonClicked( self )
	t.mTransformSprite = GameObject.Find( "Text" ).transform 
	local v = t.mTransformSprite.position;
	--local v = UnityLuaUtil.GetPos( t.m_transform_sprite );
	print( v );
	--print( t.m_rect_transform:getTypeName() );
	local x,y,z = UnityLuaUtils.GetPos( t.mTransformSprite, x, y, z )
	y = y + 10
	UnityLuaUtils.SetPos( t.mTransformSprite, x, y, z )
	--UnityLuaUtil.SetPos( GameObject.Find( "sprite_test" ).transform )
end

return t