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

	public int CountGetTrack()
	{
		return((null != TableControlTrack) ? TableControlTrack.Length : -1);
	}

	public bool TrackReboot(int count)
	{
		if(0 >= count)
		{
			return(false);
		}

		int countTrack = ControlBootUpTrack(count);
		if(0 >= countTrack)
		{
			return(false);
		}

		/* Renew Play-Informations */
		if(TableInformationPlay.Length < countTrack)
		{
			InformationPlay[] tableInformationPlayNow = TableInformationPlay;
			TableInformationPlay = new InformationPlay[countTrack];
			if(null == TableInformationPlay)
			{
				return(false);
			}

			for(int i=0; i<countTrack; i++)
			{
				TableInformationPlay[i].CleanUp();
			}

			count = tableInformationPlayNow.Length;	/* Recycle */
			for(int i=0; i<count; i++)
			{
				TableInformationPlay[i] = tableInformationPlayNow[i];
			}

			tableInformationPlayNow = null;
		}

		return(true);
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

	/* ********************************************************* */
	//! Transition the animation
	/*!
	@param	indexTrack
		Track index of now playing (Master track)
	@param	indexTrackSlave
		Track index to manage transition destination animation (Slave track)<br>
		-1 == Cancel transition
	@param	time
		Time to transition (1.0f = 1 second)
	@param	flagCancelPauseAfterTransition
		Cancel pause state after transition is completed<br>
		true == Cancel (Playing)<br>
		false == Leave master track's settings
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Fades from the current playing state to first frame of the specified animation.<br>
	However, Transition is targeting only TRS(Position, Rotation and Scaling. Except "Local-Scaling").<br>
	Not apply to "Instance" animation too.<br>
	<br>
	Track 0 should not be used Slave side. (because Track 0 is master track of the entire animation)<br>
	<br>
	When transition is complete, destination-animation will be played on indexTrack and indexTrackSlave will be in stopped state.<br>
	(IndexTrackSlave is only used for managing fade destination animation)<br>
	<br>
	When master track is in transition, this function returns error.<br>
	<br>
	If transition is canceled in the middle, state of the transition is also canceled.<br>
	(Return to the same state as not being transitioned)<br>
	*/
	public bool TrackTransition(	int indexTrack,
									int indexTrackSlave,
									float time,
									bool flagCancelPauseAfterTransition
							)
	{
		if(null == TableControlTrack)
		{
			return(false);
		}

		int countTrack = TableControlTrack.Length;
		if(0 > indexTrack)
		{
			if(0 <= indexTrackSlave)
			{
				return(false);
			}

			for(int i=0; i<countTrack; i++)
			{
				indexTrackSlave = TableControlTrack[i].IndexTrackSlave;
				if(0 <= indexTrackSlave)
				{
					TableControlTrack[indexTrackSlave].Stop(false);
					TableControlTrack[indexTrack].Transition(-1, 0.0f);
				}
			}

			return(true);
		}

		if(countTrack <= indexTrack)
		{
			return(false);
		}
		if(0 > indexTrackSlave)
		{	/* Cancel Transition */
			indexTrackSlave = TableControlTrack[indexTrack].IndexTrackSlave;
			TableControlTrack[indexTrackSlave].Stop(false);
			TableControlTrack[indexTrack].Transition(-1, 0.0f);

			return(true);
		}
		if(countTrack <= indexTrackSlave)
		{
			return(false);
		}

		if(0 <= TableControlTrack[indexTrack].IndexTrackSlave)
		{	/* Master, Transitioning now */
			return(false);
		}
		if(0.0f >= time)
		{	/* time Invalid */
			return(false);
		}

		/* Set Master-Track to fade mode */
		TableControlTrack[indexTrack].StatusIsStartAfterTransition = flagCancelPauseAfterTransition;
		return(TableControlTrack[indexTrack].Transition(indexTrackSlave, time));
	}

	#endregion Functions
}
