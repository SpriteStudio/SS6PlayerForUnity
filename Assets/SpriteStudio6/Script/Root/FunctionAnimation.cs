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
	/* ******************************************************** */
	//! Get animation count
	/*!
	@param	
		(none)
	@retval	Return-Value
		Count of animation<br>
		-1 == Failure (Error)

	Get count of animation.<br>
	*/
	public int CountGetAnimation()
	{
		return((null == DataAnimation) ? -1 : DataAnimation.CountGetAnimation());
	}

	/* ******************************************************** */
	//! Get animation index
	/*!
	@param	name
		Animation name
	@retval	Return-Value
		Animation's index<br>
		-1 == Error / "name" is not-found.

	Get animation index by name.<br>
	<br>
	Index is the serial-number (0 origins) in the animation data. <br>
	Index is needed when you call "AnimationPlay" function.
	*/
	public int IndexGetAnimation(string Name)
	{
		return((null == DataAnimation) ? -1 : DataAnimation.IndexGetAnimation(Name));
	}

	/* ******************************************************** */
	//! Start playing the animation
	/*!
	@param	indexTrack
		Track index to play (0 origin)<br>
		-1 == Stop playing all tracks and play on track 0.(Affect parts-table of all track are deleted.)
	@param	indexAnimation
		Animation index (0 origin)<br>
		-1 == Apply previous setting.<br>
		default: -1
	@param	timesPlay
		-1 == Apply previous setting.<br>
		0 == Infinite-looping <br>
		1 == Not looping<br>
		2 <= Number of Plays<br>
		default: -1
	@param	frame
		Offset frame to start playing animation (0 origins). <br>
		At the time of the first play-loop, animation is started "labelRangeStart + frameOffsetStart + frame".<br>
		-1 == Apply previous setting.<br>
		default: -1
	@param	rateTime
		Coefficient of time-passage of animation.<br>
		Minus Value is given, Animation is played backwards.<br>
		"float.NaN" is given, Apply previous setting.<br>
		default: float.NaN
	@param	style
		Library_SpriteStudio6.KindStylePlay.NOMAL == Animation is played One-Way.<br>
		Library_SpriteStudio6.KindStylePlay.PINGPONG == Animation is played round-trip.<br>
		Library_SpriteStudio6.KindStylePlay.NO_CHANGE == Apply previous setting.<br>
		default: Library_SpriteStudio6.KindStylePlay.NO_CHANGE
	@param	labelRangeStart
		Label name to start playing animation.<br>
		"" or "_start" == Top frame of Animation (reserved label-name)<br>
		null == Apply previous setting.<br>
		default: null
	@param	frameRangeOffsetStart
		Offset frame from labelRangeStart<br>
		Start frame of animation play range is "labelRangeStart + frameRangeOffsetStart".<br>
		int.MinValue == Apply previous setting.<br>
		default: int.MinValue
	@param	labelRangeEnd
		Label-name of the terminal in animation.<br>
		"" or "_end" == Last frame of Animation (reserved label-name)<br>
		null == Apply previous setting.<br>
		default: null
	@param	frameRangeOffsetEnd
		Offset frame from labelRangeStart<br>
		End frame of animation play range is "labelRangeEnd + frameRangeOffsetEnd".<br>
		int.MaxValue == Apply previous setting.<br>
		default: int.MaxValue
	@retval	Return-Value
		true == Success <br>
		false == Failure (Error)

	The playing of animation begins. <br>
	<br>
	"indexAnimation" is Animation's Index (Get Index by "IndexGetAnimation"). <br>
	When index not existing is given, this function returns false. <br>
	<br>
	Playing quicknes animation when you give value that is bigger than 1.0f to "rateTime".<br>
	Playing backwards animation when you give minus value to "rateTime".
	*/
	public bool AnimationPlay(	int indexTrack,
								int indexAnimation = -1,
								int timesPlay = -1,
								int frame = -1,
								float rateTime = float.NaN,
								Library_SpriteStudio6.KindStylePlay style = Library_SpriteStudio6.KindStylePlay.NO_CHANGE,
								string labelRangeStart = null,
								int frameRangeOffsetStart = int.MinValue,
								string labelRangeEnd = null,
								int frameRangeOffsetEnd = int.MaxValue
							)
	{
		int countTrack = TableControlTrack.Length;
		if(0 > indexTrack)
		{	/* All track */
			/* MEMO: Stop all current playback and play single animation at track 0. */
			for(int i=0; i<countTrack; i++)
			{
				TableControlTrack[i].Stop(false);
			}
			TrackConnectParts(-1, 0);
			indexTrack = 0;
		}
		else
		{	/* Specific track */
			if(countTrack <= indexTrack)
			{
				goto AnimationPlay_ErrorEnd;
			}
		}

		indexAnimation = (0 > indexAnimation) ? TableInformationPlay[indexTrack].IndexAnimation : indexAnimation;
		if((0 > indexAnimation) || (CountGetAnimation() <= indexAnimation))
		{
			goto AnimationPlay_ErrorEnd;
		}
		timesPlay = (0 > timesPlay) ? TableInformationPlay[indexTrack].TimesPlay : timesPlay;

		frame = (0 > frame) ? TableInformationPlay[indexTrack].Frame : frame;

		rateTime = (true == float.IsNaN(rateTime)) ? TableInformationPlay[indexTrack].RateTime : rateTime;

		bool flagPingPong = false;
		switch(style)
		{
			case Library_SpriteStudio6.KindStylePlay.NO_CHANGE:
				flagPingPong = TableInformationPlay[indexTrack].FlagPingPong;
				break;

			case Library_SpriteStudio6.KindStylePlay.NORMAL:
				flagPingPong = false;
				break;

			case Library_SpriteStudio6.KindStylePlay.PINGPONG:
				flagPingPong = true;
				break;

			default:
				goto case Library_SpriteStudio6.KindStylePlay.NO_CHANGE;
		}

		if(null == labelRangeStart)
		{
			labelRangeStart = TableInformationPlay[indexTrack].LabelStart;
		}
		if(true == string.IsNullOrEmpty(labelRangeStart))
		{
			labelRangeStart = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START];
		}

		frameRangeOffsetStart = (int.MinValue == frameRangeOffsetStart) ? TableInformationPlay[indexTrack].FrameOffsetStart : frameRangeOffsetStart;

		if(null == labelRangeEnd)
		{
			labelRangeEnd = TableInformationPlay[indexTrack].LabelEnd;
		}
		if(true == string.IsNullOrEmpty(labelRangeEnd))
		{
			labelRangeEnd = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END];
		}

		frameRangeOffsetEnd = (int.MaxValue == frameRangeOffsetEnd) ? TableInformationPlay[indexTrack].FrameOffsetEnd : frameRangeOffsetEnd;

		/* Update play-Information */
//		TableInformationPlay[indexTrack].FlagSetInitial = 
//		TableInformationPlay[indexTrack].FlagStopInitial = 
		TableInformationPlay[indexTrack].IndexAnimation = indexAnimation;
		TableInformationPlay[indexTrack].FlagPingPong = flagPingPong;
		TableInformationPlay[indexTrack].LabelStart = labelRangeStart;	/* string.Copt(labelRangeStart); */
		TableInformationPlay[indexTrack].FrameOffsetStart = frameRangeOffsetStart;
		TableInformationPlay[indexTrack].LabelEnd = labelRangeEnd;	/* string.Copt(labelRangeEnd); */
		TableInformationPlay[indexTrack].FrameOffsetEnd = frameRangeOffsetEnd;
		TableInformationPlay[indexTrack].Frame = frame;
		TableInformationPlay[indexTrack].TimesPlay = timesPlay;
		TableInformationPlay[indexTrack].RateTime = rateTime;

		/* Get range */
		int frameRangeStart;
		int frameRangeEnd;
		DataAnimation.TableAnimation[indexAnimation].FrameRangeGet(	out frameRangeStart, out frameRangeEnd,
																	labelRangeStart,
																	frameRangeOffsetStart,
																	labelRangeEnd,
																	frameRangeOffsetEnd
																);

		/* Start Playing */
		return(TableControlTrack[indexTrack].Start(	this,
													indexAnimation,
													frameRangeStart,
													frameRangeEnd,
													frame,
													DataAnimation.TableAnimation[indexAnimation].FramePerSecond,
													rateTime,
													0.0f,
													flagPingPong,
													timesPlay
												)
			);

	AnimationPlay_ErrorEnd:;
		return(false);
	}

	/* ******************************************************** */
	//! Stop playing the animation
	/*!
	@param	indexTrack
		Track index to stop (0 origin)<br>
		-1 == Stop playing all tracks.
	@param	flagReachEnd
		false == Animation is stopped with maintaining the current state.<br>
		true == Animation is stop and jump to last frame.<br>
		default: false
	@retval	Return-Value
		(None)

	The playing of animation stops.
	*/
	public void AnimationStop(int indexTrack, bool flagJumpEnd = false)
	{
		int countTrack = TableControlTrack.Length;
		if(0 > indexTrack)
		{	/* All track */
			/* MEMO: Stop all current playback and play single animation at track 0. */
			for(int i=0; i<countTrack; i++)
			{
				TableControlTrack[i].Stop(flagJumpEnd);
			}
			/* MEMO: Would be better not erasing parts table here. Better to unify when set tracks -1 at "AnimationPlay". */
//			TrackConnectParts(-1, 0);
		}
		else
		{	/* Specific track */
			if(countTrack <= indexTrack)
			{
				return;	/* Ignore error */
			}
			TableControlTrack[indexTrack].Stop(flagJumpEnd);
		}
	}

	/* ******************************************************** */
	//! Set the pause-status of the animation
	/*!
	@param	indexTrack
		Track index to set pause-status (0 origin)<br>
		-1 == Set pause-status all tracks.
	@param	flagSwitch
		true == Set pause (Suspend)<br>
		false == Rerease pause (Resume)
	@retval	Return-Value
		true == Success <br>
		false == Failure (Error)

	The playing of animation suspends or resumes.<br>
	This function returns success if succeeds on all tracks (if you set -1 to "track", only tracks are playing will be targeted).<br>
	if specific track, return false when the track is not playing.<br>
	*/
	public bool AnimationPause(int indexTrack, bool flagSwitch)
	{
		int countTrack = TableControlTrack.Length;
		bool flagSuccess = true;
		if(0 > indexTrack)
		{	/* All track */
			/* MEMO: Stop all current playback and play single animation at track 0. */
			for(int i=0; i<countTrack; i++)
			{
				if(true == TableControlTrack[i].StatusIsPlaying)
				{
					flagSuccess &= TableControlTrack[i].Pause(flagSwitch);
				}
			}
		}
		else
		{	/* Specific track */
			if(countTrack <= indexTrack)
			{
				return(false);
			}
			if(false == TableControlTrack[indexTrack].StatusIsPlaying)
			{
				return(false);
			}
			return(TableControlTrack[indexTrack].Pause(flagSwitch));
		}
		return(flagSuccess);
	}

	/* ******************************************************** */
	//! Changing animations' playing speed
	/*!
	@param	indexTrack
		Track index to set pause-status (0 origin)<br>
		-1 == Set pause-status all tracks.
	@param	rateTime
		Coefficient of time-passage of animation.<br>
		Minus Value is given, Animation is played backwards.
	@retval	Return-Value
		true == Success <br>
		false == Failure (Error)

	Change speed of the animation during playing.<br>
	*/
	public bool AnimationSetRateTime(int indexTrack, float rateTime)
	{
		int countTrack = TableControlTrack.Length;
		if(0 > indexTrack)
		{	/* All track */
			/* MEMO: Stop all current playback and play single animation at track 0. */
			for(int i=0; i<countTrack; i++)
			{
				if(true == TableControlTrack[i].StatusIsPlaying)
				{
					TableControlTrack[i].RateTime = rateTime;
				}
			}
		}
		else
		{	/* Specific track */
			if(countTrack <= indexTrack)
			{
				return(false);
			}
			if(false == TableControlTrack[indexTrack].StatusIsPlaying)
			{
				return(false);
			}
			TableControlTrack[indexTrack].RateTime = rateTime;
		}
		return(true);
	}
	#endregion Functions
}
