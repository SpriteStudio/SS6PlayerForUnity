/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RandomGenerator = Library_SpriteStudio6.Utility.Random.XorShift32;

[ExecuteInEditMode]
[System.Serializable]
public partial class Script_SpriteStudio6_RootEffect : Library_SpriteStudio6.Script.Root
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public Script_SpriteStudio6_DataEffect DataEffect;

	public int LimitParticleDraw;
	private FlagBitStatus Status;
	internal bool StatusIsValid
	{
		get
		{
			return(0 != (Status & FlagBitStatus.VALID));
		}
	}
	internal bool StatusIsPlayingInfinity
	{
		get
		{
			return(0 != (Status & FlagBitStatus.PLAYING_INFINITY));
		}
		set
		{
			Status = (true == value) ? (Status | FlagBitStatus.PLAYING_INFINITY) : (Status & ~FlagBitStatus.PLAYING_INFINITY);
		}
	}

	/* MEMO: Status of animation's play-track are diverted. (Since useless of redefine same content) */
	/* MEMO: "Effect" have no multi-track playing capcity. */
	internal Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus StatusPlaying;
	internal float TimePerFrame;
	internal float TimeElapsed;
	public float RateTime;

	internal float Frame;
	internal float FrameRange;
	internal float FramePerSecond;

	private Library_SpriteStudio6.CallBack.FunctionTimeElapseEffect FunctionTimeElapse = FunctionTimeElapseDefault;
	internal Library_SpriteStudio6.CallBack.FunctionTimeElapseEffect CallBackFunctionTimeElapse
	{
		get
		{
			return(FunctionTimeElapse);
		}
		set
		{
			FunctionTimeElapse = (null != value) ? value : FunctionTimeElapseDefault;
		}
	}
	internal Library_SpriteStudio6.CallBack.FunctionPlayEndEffect CallBackFunctionPlayEnd = null;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
	void Awake()
	{
		/* Awake Base-Class */
		BaseAwake();
	}

	void Start()
	{
		/* Clear Status */
		Status = FlagBitStatus.CLEAR;

		/* Check master datas */
		if((null == DataCellMap) || (null == DataEffect))
		{
			goto Start_ErrorEnd;
		}

		/* Start Base-Class */
		BaseStart();

		/* えみっとパターンからフレーム長を作る */
//		FrameRange = これ、エミットパターンからできる

		Status |= FlagBitStatus.VALID;

		return;

	Start_ErrorEnd:;
		Status &= ~FlagBitStatus.VALID;
		return;
	}

//	void Update()
//	{
//	}

	void LateUpdate()
	{
		if(null == InstanceRootParent)
		{
			/* MEMO: Execute only at the "Highest Parent(not under anyone's control)"-Root part.         */
			/*       "Child"-Root parts' "LateUpdatesMain" are called from Parent's internal processing. */
			LateUpdateMain(FunctionTimeElapse(this));
		}
	}
	internal void LateUpdateMain(float timeElapsed)
	{
		if(0 != (Status & FlagBitStatus.VALID))
		{
			return;
		}

		/* Update Base */
		BaseLateUpdate(timeElapsed);

		/* Update Playing-Status */
		if(0 == (StatusPlaying & Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.PLAYING)) 
		{	/* Not-Playing */
			return;
		}
		TimeElapsed += (	(0 != (StatusPlaying & Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.PAUSING))
							|| (0 != (StatusPlaying & Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.PLAYING_START))
						) ? 0.0f : timeElapsed;
		Frame = TimeElapsed * FramePerSecond;
		if(0 != (Status & FlagBitStatus.PLAYING_INFINITY))
		{	/* Independent */
//			Frame %= FrameRange;
		}
		else
		{	/* Dependent */
			Frame = Mathf.Clamp(Frame, 0.0f, FrameRange);
		}

		/* Update & Draw Emitters */


		/* Clear transient status */
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	private static float FunctionTimeElapseDefault(Script_SpriteStudio6_RootEffect scriptRoot)
	{
		return(Time.deltaTime);
	}

	public static Library_SpriteStudio6.Utility.Random.Generator InstanceCreateRandom()
	{
		return(new RandomGenerator());
	}

	public static uint KeyCreateRandom()
	{
		RandomKeyMakeID++;

		/* MEMO: time(0) at C++ */
		System.DateTime TimeNow = System.DateTime.Now;
		TimeNow.ToUniversalTime();
		System.TimeSpan SecNow = TimeNow - TimeUnixEpoch;
		
		return(RandomKeyMakeID + (uint)SecNow.TotalSeconds);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum Constants
	{
		LIMIT_SUBEMITTER_DEPTH = 2,
		LIMIT_SUBEMITTER_COUNT = 10,
	}
	public enum Defaults
	{
		LIMIT_PARTICLEDRAW = 1024,
	}

	[System.Flags]
	private enum FlagBitStatus
	{
		VALID = 0x40000000,
		PLAYING_INFINITY = 0x1000000,

		CHANGE_CELLMAP = 0x08000000,

		CLEAR = 0x00000000,
	}

	private readonly static System.DateTime TimeUnixEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
	private static uint RandomKeyMakeID = 123456;
	#endregion Enums & Constants
}
