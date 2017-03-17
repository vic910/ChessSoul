﻿using System;
using System.Collections;
using System.Collections.Generic;
using Groot.Network;
using SLua;
using UnityEngine;
using Weiqi;

/// <summary>
/// 我的信息
/// </summary>
[CustomLuaClassAttribute]
public class MainPlayer
{
	public static readonly MainPlayer Instance = new MainPlayer();

	public PlayerInfo PlayerInfo { get; private set; }

	[DoNotToLua]
	public void InitializePlayerInfo( PlayerInfo _info )
	{
		   PlayerInfo = _info;
	}
}
