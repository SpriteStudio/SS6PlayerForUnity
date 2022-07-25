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
public class Script_SpriteStudio6_DataSequence : ScriptableObject
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public KindVersion Version;
	public string Name;								/* Body-Name of SSQE */
	public Script_SpriteStudio6_DataProject DataProject;

	public Library_SpriteStudio6.Data.Sequence.Data[] TableSequence;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	public void CleanUp()
	{
		Version = (KindVersion)(-1);
	}

	public bool VersionCheckRuntime()
	{
		return(((KindVersion.SUPPORT_EARLIEST <= Version) && (KindVersion.SUPPORT_LATEST >= Version)));	/* ? true : false */
	}

	public int IndexGetSequence(string name)
	{
		if(true == string.IsNullOrEmpty(name))
		{
			return(-1);
		}

		int count = TableSequence.Length;
		for(int i=0; i<count; i++)
		{
			if(name == TableSequence[i].Name)
			{
				return(i);
			}
		}
		return(-1);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindVersion
	{
		SUPPORT_EARLIEST = CODE_010000,
		SUPPORT_LATEST = CODE_010000,

		SS5PU = 0x00000000,	/* Before SS5PU *//* (Reserved) */
			/* MEMO: There is no data equivalent to "Sequence(SSQE)" by version 1.1.x. */
		CODE_010000 = 0x00010000,	/* SS6PU Ver.1.2.0 */
	}
	#endregion Enums & Constants
}
