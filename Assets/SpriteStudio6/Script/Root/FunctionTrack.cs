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
	public int LimitGetTrack()
	{
		return((0 < LimitTrack) ? LimitTrack : (int)Defaults.LIMIT_TRACK);
	}

	public bool TrackConnectParts(int idParts, int indexTrack, bool flagChildParts = false)
	{
		if((null == TableControlParts) || (null == DataAnimation))
		{
			return(false);
		}

		int countParts = TableControlParts.Length;
		if(0 > idParts)
		{	/* All parts */
			/* MEMO: Ignore "flagChildParts" */
			for(int i=0; i<countParts; i++)
			{
				TableControlParts[i].IndexControlTrack = indexTrack;
			}
		}
		else
		{	/* Specific pats */
			if(countParts <= idParts)
			{
				return(false);
			}
			if(true == flagChildParts)
			{
				TrackConnectPartsInvolveChild(idParts, indexTrack);
			}
			else
			{
				TableControlParts[idParts].IndexControlTrack = indexTrack;
			}
		}

		return(true);
	}
	private void TrackConnectPartsInvolveChild(int idParts, int indexTrack)
	{
		TableControlParts[idParts].IndexControlTrack = indexTrack;

		int countPartsChild = DataAnimation.TableParts[idParts].TableIDChild.Length;
		for(int i=0; i<countPartsChild; i++)
		{
			TrackConnectPartsInvolveChild(DataAnimation.TableParts[idParts].TableIDChild[i], indexTrack);
		}
	}
	#endregion Functions
}
