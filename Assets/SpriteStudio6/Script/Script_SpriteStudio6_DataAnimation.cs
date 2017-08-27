/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script_SpriteStudio6_DataAnimation : ScriptableObject
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public KindVersion Version;

	public Library_SpriteStudio6.Data.Parts.Animation[] TableParts;
	public Library_SpriteStudio6.Data.Animation[] TableAnimation;

	/* MEMO: Can use only from Implemented-Class of Library_SpriteStudio6.Data.Animation.PackAttribute.Parameter.Container */
	public Library_SpriteStudio6.Data.Animation.PackAttribute.Parameter ParameterPackAttribute;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	public void CleanUp()
	{
		Version = (KindVersion)(-1);
		TableParts = null;
		TableAnimation = null;
		ParameterPackAttribute = null;
	}

	public int CountGetParts()
	{
		return(TableParts.Length);
	}

	public int CountGetPartsDraw()
	{
		int countParts = TableParts.Length;
		int count = 0;
		for(int i=0; i<countParts; i++)
		{
			switch(TableParts[i].Feature)
			{
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
					count++;
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
					break;
			}
		}
		return(count);
	}

	public int IndexGetParts(string name)
	{
		if(true == string.IsNullOrEmpty(name))
		{
			return(-1);
		}

		int count = TableParts.Length;
		for(int i=0; i<count; i++)
		{
			if(name == TableParts[i].Name)
			{
				return(i);
			}
		}
		return(-1);
	}

	public int CountGetAnimation()
	{
		return(TableAnimation.Length);
	}

	public int IndexGetAnimation(string name)
	{
		if(true == string.IsNullOrEmpty(name))
		{
			return(-1);
		}

		int count = TableAnimation.Length;
		for(int i=0; i<count; i++)
		{
			if(name == TableAnimation[i].Name)
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
		SS5PU = 0,	/* Before SS5PU *//* (Reserved) */
		CODE_010000 = 0x00010000,	/* SS6PU Ver.1.0.0 */

		SUPPORT_EARLIEST = CODE_010000,
		SUPPORT_LATEST = CODE_010000
	}
	#endregion Enums & Constants
}
