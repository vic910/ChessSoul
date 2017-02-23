using System.Collections;
using System.Collections.Generic;
using Groot.Network;
using UnityEngine;
using UnityEngine.UI;
using Weiqi.UI;

public class UI_Self : UI_Base
{

	public override void OnLoaded()
	{
		
	}

	public override void OnUnload()
	{
		
	}

	public override float PreShow( UI_Base _pre_ui, params object[] _args )
	{
		return m_entrance_anim_time;
	}
}
