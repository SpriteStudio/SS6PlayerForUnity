/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script_SpriteStudio6_DataSoundList : ScriptableObject
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public KindVersion Version;
	public Script_SpriteStudio6_DataProject DataProject;

	public Library_SpriteStudio6.Data.Sound.Inventory[] TableSoundList;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	public void CleanUp()
	{
		Version = (KindVersion)(-1);
		DataProject = null;

		TableSoundList = null;
	}

	public bool VersionCheckRuntime()
	{
		return(((KindVersion.SUPPORT_EARLIEST <= Version) && (KindVersion.SUPPORT_LATEST >= Version)));	/* ? true : false */
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindVersion
	{
		SUPPORT_EARLIEST = CODE_010000,
		SUPPORT_LATEST = CODE_010000,

		SS5PU = 0x00000000,	/* Before SS5PU *//* (Reserved) */
		CODE_010000 = 0x00010000,	/* SS6PU Ver.2.3.0 */
	}
	#endregion Enums & Constants
}
