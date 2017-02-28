using System.Collections;
using System.Collections.Generic;
using Groot.Network;
using UnityEngine;
using UnityEngine.UI;
using Weiqi.UI;

public class UI_WaitResponse : UI_Base
{
	[SerializeField]
	private Image m_loading = null;

	void Update()
	{
		if( Time.frameCount % 6 != 0 )
			return;
		m_loading.transform.Rotate( Vector3.back, 30, Space.Self );
	}
}
