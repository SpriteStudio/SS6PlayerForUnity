/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Script_SpriteStudio6_Root
{
	/* ----------------------------------------------- Functions */
	#region Functions
	public int IDGetParts(string name)
	{
		if(null == DataAnimation)
		{
			return(-1);
		}

		return(DataAnimation.IndexGetParts(name));
	}

	public bool HideSet(int idParts, bool flagSwitch, bool flagInvolveChildren=false)
	{
		if(0 == idParts)
		{	/* "Root"-Parts */
			FlagHideForce = flagSwitch;
			return(true);
		}

		if((null == DataAnimation) || (null == TableControlParts))
		{
			return(false);
		}

		int countParts = TableControlParts.Length;
		if(countParts <= idParts)
		{
			return(false);
		}

		if(0 > idParts)
		{	/* All parts */
			for(int i=1; i<countParts; i++)
			{
				HideSetMain(i, flagSwitch, false);
			}
			return(true);
		}

		HideSetMain(idParts, flagSwitch, flagInvolveChildren);

		return(true);
	}
	private void HideSetMain(int idParts, bool flagSwitch, bool flagInvolveChildren=false)
	{
		if(true == flagSwitch)
		{
			TableControlParts[idParts].Status |= Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.HIDE_FORCE;
		}
		else
		{
			TableControlParts[idParts].Status &= ~Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.HIDE_FORCE;
		}

		if(true == flagInvolveChildren)
		{
			int[] tableIDPartsChild = DataAnimation.TableParts[idParts].TableIDChild;
			int countPartsChild = tableIDPartsChild.Length;
			for(int i=0; i<countPartsChild; i++)
			{
				HideSetMain(tableIDPartsChild[i], flagSwitch, true);
			}
		}
	}

//	HideSetForce
//	InstanceChange
//	EffectChange
	#endregion Functions
	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	public static partial class Parts
	{
		/* ----------------------------------------------- Functions */
		#region Functions
		/* ******************************************************** */
		//! Get Root-Parts
		/*!
		@param	gameObject
			GameObject of starting search
		@retval	Return-Value
			Instance of "Script_SpriteStudio6_Root"<br>
			null == Not-Found / Failure	

		Get component "Script_SpriteStudio6_Root" by examining "gameObject" and direct-children.<br>
		*/
		public static Script_SpriteStudio6_Root RootGetChild(GameObject gameObject)
		{
			Script_SpriteStudio6_Root scriptRoot = null;

			/* Check Origin */
			scriptRoot = gameObject.GetComponent<Script_SpriteStudio6_Root>();
			if(null != scriptRoot)
			{	/* Has Root-Parts */
				return(scriptRoot);
			}

			/* Check Direct-Children */
			int countChild = gameObject.transform.childCount;
			Transform transformChild = null;
			for(int i=0; i<countChild; i++)
			{
				transformChild = gameObject.transform.GetChild(i);
				scriptRoot = transformChild.gameObject.GetComponent<Script_SpriteStudio6_Root>();
				if(null != scriptRoot)
				{	/* Has Root-Parts */
					return(scriptRoot);
				}
			}

			return(null);
		}
		#endregion Functions
	}
	#endregion Classes, Structs & Interfaces
}
