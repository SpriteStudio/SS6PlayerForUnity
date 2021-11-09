/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
// #define EXECUTE_LATEUPDATE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Script_SpriteStudio6_Sequence : MonoBehaviour
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public Script_SpriteStudio6_DataProject DataProject;
	internal Script_SpriteStudio6_DataSequence DataSequence
	{
		get
		{
			return(DataSequencePack);
		}
	}

	/* MEMO: "NameSequencePack", "NameDataSequence" and "IndexStepInitial" are defined "public", */
	/*         but you must not access directly. Only for the inspector.                         */
	public string NameSequencePack;
	public string NameDataSequence;
	public int IndexStepInitial;

	public bool FlagStopInitial;
	public bool FlagHideForce;
	public bool FlagPlanarization;
	public bool FlagColliderInterlockHideForce;
	public int OrderInLayer;

	private FlagBitStatus Status = FlagBitStatus.CLEAR;
	protected float RateOpacityForce = 1.0f;
	protected Vector2 RateScaleLocalForce = Vector2.one;
	internal float RateTime = 1.0f;

	internal bool StatusIsValid
	{
		get
		{
			return(0 != (Status & FlagBitStatus.VALID));
		}
	}
	internal bool StatusIsPlaying
	{
		get
		{
			return(0 != (Status & FlagBitStatus.PLAYING));
		}
	}
	internal bool StatusIsPausing
	{
		get
		{
			return(0 != (Status & FlagBitStatus.PAUSING));
		}
	}
//	internal bool StatusIsInitialExecuteUpdate
//	{
//		get
//		{
//			return(0 != (Status & FlagBitStatus.INITIAL_EXECUTE_UPDATE));
//		}
//	}
	internal bool StatusIsUpdateStep
	{
		get
		{
			return(0 != (Status & FlagBitStatus.UPDATE_STEP));
		}
	}
	internal bool StatusIsUpdatePause
	{
		get
		{
			return(0 != (Status & FlagBitStatus.UPDATE_PAUSE));
		}
	}
	internal bool StatusIsUpdateRateScaleLocal
	{
		get
		{
			return(0 != (Status & FlagBitStatus.UPDATE_RATE_SCALELOCAL));
		}
	}
	internal bool StatusIsUpdateRateOpacity
	{
		get
		{
			return(0 != (Status & FlagBitStatus.UPDATE_RATE_OPACITY));
		}
	}

	public bool IsUsingSequencePack
	{
		get
		{
			return(null != DataSequencePack);	/* ? true : false */
		}
	}

	/* MEMO: Bellow 2 properties (RateOpacity/RateScaleLocal) are used to control from parent animation. */
	/*       In principle, do not change the value. Correctly operation is not guaranteed.               */
	internal float RateOpacity
	{
		get
		{
			return(RateOpacityForce);
		}
		set
		{
			RateOpacityForce = value;
			Status |= FlagBitStatus.UPDATE_RATE_OPACITY;
		}
	}
	internal Vector2 RateScaleLocal
	{
		get
		{
			return(RateScaleLocalForce);
		}
		set
		{
			RateScaleLocalForce = value;
			Status |= FlagBitStatus.UPDATE_RATE_SCALELOCAL;
		}
	}

	/* MEMO: Don't cache the following values when you use externally,                  */
	/*       Values(Instances) may be replaced depending on animation's playback state. */
	private GameObject GameObjectRoot = null;
	internal Script_SpriteStudio6_Root ScriptRoot = null;

	private Script_SpriteStudio6_DataSequence DataSequencePack = null;
	private Library_SpriteStudio6.Data.Sequence.Type TypeLoop = Library_SpriteStudio6.Data.Sequence.Type.INVALID;
	private Library_SpriteStudio6.Data.Sequence.Data.Step[] DataStep = null;

	private int IndexStep = -1;

	internal Library_SpriteStudio6.CallBack.FunctionPlayEndSequence FunctionPlayEnd = null;
	internal Library_SpriteStudio6.CallBack.FunctionDecodeStepSequence FunctionDecodeStep = null;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
	void Start()
	{
		if(0 == (Status & FlagBitStatus.VALID))
		{
			StartMain();
		}
	}
	internal void StartMain()
	{
		/* Boot up master datas */
		/* MEMO: Reason why initial setting of ScriptableObject is done here     */
		/*        (without processing with ScriptableObject's Awake or OnEnable) */
		/*        is to stabilize execution such when re-compile.                */
		if(null == DataProject)
		{	/* Data invalid */
			goto StartMain_ErrorEnd;
		}
		if(false == DataProject.VersionCheckRuntime())
		{	/* Data-Version invalid */
			goto StartMain_ErrorEnd;
		}

		/* Initialize parameters */
		IndexStep = -1;
		GameObjectRoot = null;
		ScriptRoot = null;
		IndexStep = -1;

		/* Inittial Play */
		if((false == string.IsNullOrEmpty(NameSequencePack)) && (false == string.IsNullOrEmpty(NameDataSequence)))
		{
			int index;
			index = IndexGetPack(NameSequencePack);
			if(0 > index)
			{
				goto StartMain_End;
			}
			if(false == PackSet(index))
			{
				goto StartMain_End;
			}

			index = IndexGetSequence(NameDataSequence);
			if(0 > index)
			{
				goto StartMain_End;
			}
			if(false == SequenceSet(index))
			{
				goto StartMain_End;
			}

			int countStep = CountGetStep();
			if((0 > IndexStepInitial) || (countStep <= IndexStepInitial))
			{
				IndexStepInitial = 0;
			}

			Play(IndexStepInitial);
//			if(true == FlagStopInitial)
//			{
//				Stop(false, false);
//			}
		}

	StartMain_End:;
		Status |= FlagBitStatus.VALID;
		Status |= FlagBitStatus.UPDATE_ALL;
		Status |= FlagBitStatus.INITIAL_EXECUTE_UPDATE;
		return;

	StartMain_ErrorEnd:;
		Status &= ~FlagBitStatus.VALID;
		return;
	}

#if EXECUTE_LATEUPDATE
	void LateUpdate()
#else
	void Update()
#endif
	{
		if(false == StatusIsValid)
		{	/* Not Initialized (Not Running) */
			StartMain();
			if(false == StatusIsValid)
			{	/* Failure to Initialize */
				return;
			}
		}
		if(false == StatusIsPlaying)
		{	/* Not Playing */
			return;
		}

		/* Updates */
		/* MEMO: Even when be hidden, playback continue.                                           */
		/*       Otherwise, interfere with animation's instance changes and frame synchronization. */
		/* MEMO: Make sure to run "StepProgress" first in the Animation-playing process. */
		/*       Because, Animation-object is instantiated in StepProgress.              */
		if(true == StatusIsUpdateStep)
		{
			StepProgress();

			Status &= ~FlagBitStatus.UPDATE_STEP;
			Status |= (FlagBitStatus.UPDATE_ALL & ~FlagBitStatus.UPDATE_STEP);
		}
		if(null == ScriptRoot)
		{	/* Missing Instance */
			return;
		}

		ScriptRoot.FlagHideForce = FlagHideForce;
		ScriptRoot.FlagPlanarization = FlagPlanarization;
		ScriptRoot.FlagColliderInterlockHideForce = FlagColliderInterlockHideForce;
		ScriptRoot.OrderInLayer = OrderInLayer;

		ScriptRoot.RateTimeSet(-1, RateTime);
		if(true == StatusIsUpdateRateOpacity)
		{
			ScriptRoot.RateOpacity = RateOpacityForce;
			Status &= ~FlagBitStatus.UPDATE_RATE_OPACITY;
		}
		if(true == StatusIsUpdateRateScaleLocal)
		{
			ScriptRoot.RateScaleLocal = RateScaleLocalForce;;
			Status &= ~FlagBitStatus.UPDATE_RATE_SCALELOCAL;
		}
		if(true == StatusIsUpdatePause)
		{
			ScriptRoot.AnimationPause(-1, StatusIsPausing);
			Status &= ~FlagBitStatus.UPDATE_PAUSE;
		}

		if(0 != (Status & FlagBitStatus.INITIAL_EXECUTE_UPDATE))
		{
			if(true == FlagStopInitial)
			{
				Stop(false, false);
			}
		}
		Status &= ~FlagBitStatus.INITIAL_EXECUTE_UPDATE;
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	/* ********************************************************* */
	//! Get Sequence-Pack count
	/*!
	@param	
		(none)
	@retval	Return-Value
		Count of Sequence-Pack<br>
		-1 == Failure (Error)

	Get count of Sequence-Packs in currently set Data-Project.<br>
	*/
	public int CountGetPack()
	{
		if(null == DataProject)
		{
			return(-1);
		}
		if(null == DataProject.DataSequence)
		{
			return(0);
		}

		return(DataProject.DataSequence.Length);
	}

	/* ********************************************************* */
	//! Get Sequence-Pack index
	/*!
	@param	name
		Sequence-Pack(SSQE) name
	@retval	Return-Value
		Sequence-Pack's index<br>
		-1 == Error / "name" is not-found.

	Get Sequence-Pack index by name.<br>
	Several Sequence-Packs are stored in Data-Project, each with
		the same name as SSQE file body-name on SpriteStudio6.<br>
	<br>
	Index is the serial-number (0 origins) in the Data-Project. <br>
	Index is needed when you call "PackSet" function.<br>
	<br>
	Since useless to search Sequence-Pack-index every time, recommend to cache frequently used indexes.
	*/
	public int IndexGetPack(string name)
	{
		if(null == DataProject)
		{
			return(-1);
		}

		return(DataProject.IndexGetPackSequence(name));
	}

	/* ********************************************************* */
	//! Set Sequence-Pack
	/*!
	@param	index
		Sequence-Pack index
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Set Sequence-Pack that contains sequence to want to play.<br>
	<br>
	Sequence-Pack cannot be changed during playing.
		(This function returns false.)<br>
	*/
	public bool PackSet(int index)
	{
		if(null == DataProject)
		{
			return(false);
		}
		if(true == StatusIsPlaying)
		{	/* Now playing */
			return(false);
		}

		int countPack = CountGetPack();
		if((0 > index) || (0 > countPack) || (countPack <= index))
		{
			return(false);
		}

		DataSequencePack = DataProject.DataSequence[index];

		return(true);
	}

	/* ********************************************************* */
	//! Disable Sequence-pack
	/*!
	@param	index

		Sequence-Pack index
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Set for using Sequence-Data generated at runtime, without using Sequence-Pack.<br>
	When using generated sequence data, call instead of "PackSet".<br>
	<br>
	After calling this function, call "SequenceSet(typeLoop, dataStep)" to specify Sequence-Data.<br>
	*/
	public bool PackSetNoUse()
	{
		if(null == DataProject)
		{
			return(false);
		}
		if(true == StatusIsPlaying)
		{	/* Now playing */
			return(false);
		}

		DataSequencePack = null;

		return(true);
	}

	/* ********************************************************* */
	//! Get sequence count
	/*!
	@param	
		(none)
	@retval	Return-Value
		Count of sequence<br>
		-1 == Failure (Error)

	Get count of sequence in currently set Sequence-Pack.<br>
	<br>
	Can only call when sequence pack is specified using "PackSet".<br>
	*/
	public int CountGetSequence()
	{
		if(null == DataProject)
		{
			return(-1);
		}
		if(null == DataSequencePack)
		{	/* Disable Sequence-Pack */
			return(-1);
		}
		if(null == DataSequencePack.TableSequence)
		{
			return(0);
		}

		return(DataSequencePack.TableSequence.Length);
	}

	/* ********************************************************* */
	//! Get sequence index
	/*!
	@param	name
		Sequence name
	@retval	Return-Value
		Sequence's index<br>
		-1 == Error / "name" is not-found.

	Get sequence index by name.<br>
	Several sequence are stored in Sequence-Pack, each with
		the same name as sequence-name in SSQE file on SpriteStudio6.<br>
	<br>
	Index is the serial-number (0 origins) in current Sequence-Pack. <br>
	Index is needed when you call "SequenceSet" function.<br>
	<br>
	Since useless to search Sequence every time, recommend to cache frequently used indexes.<br>
	<br>
	Can only call when sequence pack is specified using "PackSet".<br>
	*/
	public int IndexGetSequence(string name)
	{
		if((null == DataProject) || (null == DataSequencePack))
		{
			return(-1);
		}

		return(DataSequencePack.IndexGetSequence(name));
	}

	/* ********************************************************* */
	//! Set sequence
	/*!
	@param	index
		Sequence index
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Set sequence in Sequence-Pack.<br>
	<br>
	Sequence cannot be changed during playing. (This function returns false.)<br>
	<br>
	Can only call when sequence pack is specified using "PackSet".<br>
	*/
	public bool SequenceSet(int index)
	{
		if((null == DataProject) || (null == DataSequencePack))
		{
			return(false);
		}
		if(true == StatusIsPlaying)
		{	/* Now playing */
			return(false);
		}
		if(null == DataSequencePack.TableSequence)
		{
			return(false);
		}

		int countSequence = CountGetSequence();
		if((0 > index) || (0 > countSequence) || (countSequence <= index))
		{
			return(false);
		}

		TypeLoop = DataSequencePack.TableSequence[index].Type;
		DataStep = DataSequencePack.TableSequence[index].TableStep;

		return(true);
	}

	/* ********************************************************* */
	//! Set sequence (Data generated)
	/*!
	@param	typeLoop
		How to loop when "dataStep" is finished playing to the end.
	@param	dataStep
		Sequence step data
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Set sequence data you generated.<br>
	<br>
	Sequence cannot be changed during playing. (This function returns false.)<br>
	<br>
	Can only call when sequence pack is specified using "PackSetNoUse".<br>
	*/
	public bool SequenceSet(Library_SpriteStudio6.Data.Sequence.Type typeLoop, Library_SpriteStudio6.Data.Sequence.Data.Step[] dataStep)
	{
		if(null == DataProject)
		{
			return(false);
		}
		if(null != DataSequencePack)
		{
			return(false);
		}
		if(true == StatusIsPlaying)
		{	/* Now playing */
			return(false);
		}

		TypeLoop = typeLoop;
		DataStep = dataStep;

		return(true);
	}

	/* ********************************************************* */
	//! Get step count
	/*!
	@param	
		(none)
	@retval	Return-Value
		Count of step in the sequence<br>
		-1 == Failure (Error)

	Get count of step in currently set sequence.<br>
	*/
	public int CountGetStep()
	{
		if((null == DataProject) || (null == DataSequencePack))
		{
			return(-1);
		}
		if(null == DataStep)
		{
			return(-1);
		}

		return(DataStep.Length);
	}

	/* ********************************************************* */
	//! Start playing current sequence
	/*!
	@param	step
		Step to start playback
		0 == From the top
	@param	rateTime
		Coefficient of time-passage of animation.<br>
		Must not be negative or 0.0f.
		"float.NaN" is given, Apply previous setting.<br>
		default: float.NaN

	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	The playing of current sequence begins. <br>
	<br>
	When "step" is given a value from 0 to CountGetStep()-1,
		 will play from the middle of the sequence.<br>
	*/
	public bool Play(int step=0, float rateTime=float.NaN)
	{
		if((null == DataProject) || (null == DataStep))
		{
			return(false);
		}
		if(true == StatusIsPlaying)
		{	/* Now playing */
			return(false);
		}

		int countStep = CountGetStep();
		if((0 > step) || (0 > countStep) || (countStep <= step))
		{
			return(false);
		}
		if(true == float.IsNaN(rateTime))
		{
			rateTime = RateTime;
			if(rateTime <= 0.0f)
			{
				rateTime = 1.0f;
			}
		}
		else
		{
			if(0.0f >= rateTime)
			{
				return(false);
			}
		}

		IndexStep = step;
		RateTime = rateTime;
		Status |= FlagBitStatus.PLAYING;
		Status &= ~FlagBitStatus.PAUSING;
		Status |= FlagBitStatus.UPDATE_ALL;

		return(true);
	}

	/* ********************************************************* */
	//! Stop playing the current sequence
	/*!
	@param	flagJumpEndStep
		* Not working, now (Reserved) 
		false == Sequence is stopped with maintaining the current state.<br>
		true == Sequence is stop and jump to last step.<br>
		default: false
	@param	flagJumpEndFrame
		false == Sequence is stopped with maintaining the current state.<br>
		true == Sequence is stop and jump to animation's last frame.<br>
		default: false
	@retval	Return-Value
		(None)

	The playing of sequence stops.<br>
	*/
	public void Stop(bool flagJumpEndStep, bool flagJumpEndFrame)
	{
		if((null == DataProject) || (null == DataStep))
		{
			return;
		}
		if(false == StatusIsPlaying)
		{	/* Not playing */
			return;
		}

		if(null != ScriptRoot)
		{
			ScriptRoot.AnimationPause(-1, false);
			ScriptRoot.AnimationStop(-1, true);
		}

		Status &= ~(	FlagBitStatus.PLAYING
						| FlagBitStatus.PAUSING
						| FlagBitStatus.UPDATE_ALL
				);
	}

	/* ********************************************************* */
	//! Set the pause-status
	/*!
	@param	flagSwitch
		true == Set pause (Suspend)<br>
		false == Rerease pause (Resume)
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	The playing of sequence suspends or resumes.<br>
	*/
	public bool PauseSet(bool flagSwitch)
	{
		if((null == DataProject) || (null == DataStep))
		{
			return(false);
		}
		if(false == StatusIsPlaying)
		{	/* Not Playing */
			return(false);
		}

		if(true == flagSwitch)
		{
			if(false == StatusIsPausing)
			{
				Status |= FlagBitStatus.PAUSING;
				Status |= FlagBitStatus.UPDATE_PAUSE;
			}
		}
		else
		{
			if(true == StatusIsPausing)
			{
				Status &= FlagBitStatus.PAUSING;
				Status |= FlagBitStatus.UPDATE_PAUSE;
			}
		}

		return(true);
	}

	/* ********************************************************* */
	//! Set the step of current sequence
	/*!
	@param	step
		true == Set pause (Suspend)<br>
		false == Rerease pause (Resume)
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Force jump the playback step of current sequence.
	*/
	public bool StepSet(int step)
	{
		if((null == DataProject) || (null == DataStep))
		{
			return(false);
		}
		if(false == StatusIsPlaying)
		{	/* Not Playing */
			return(false);
		}

		int countStep = CountGetStep();
		if((0 > step) || (0 > countStep) || (countStep <= step))
		{
			return(false);
		}

		IndexStep = step;
		Status |= FlagBitStatus.UPDATE_STEP;

		return(true);
	}

	private bool StepProgress()
	{
		/* Next play data determination */
		int indexStep = IndexStep;
		Library_SpriteStudio6.Data.Sequence.Data.Step dataStep = DataStep[indexStep];
		if(null!= FunctionDecodeStep)
		{
			int indexStepNext = FunctionDecodeStep(ref dataStep, this, indexStep);
			if(0 <= indexStepNext)
			{	/* Jump Step */
				indexStep = indexStepNext;
				dataStep = DataStep[indexStep];
			}
			else
			{	/* Not Jump */
				if(false == dataStep.IsValid)
				{	/* Stop Playing */
					if(null != ScriptRoot)
					{
						ScriptRoot.AnimationStop(-1);
					}
					return(true);
				}
			}
		}

		/* Renew Animation-Object */
		/* MEMO: Different Animation-Packs have different structure of parts. */
		/*       So in that case, need to re-instantiate Animation-Object.    */
		if(null == DataProject)
		{
			return(false);
		}
		int indexPackAnimation = DataProject.IndexGetPackAnimation(dataStep.NamePackAnimation);
		if(0 > indexPackAnimation)
		{
			return(false);
		}
		Script_SpriteStudio6_DataAnimation dataAnimation = DataProject.DataAnimation[indexPackAnimation];
		bool flagRootRenew = false;
		if(null != ScriptRoot)
		{	/* Exist Animation-Object */
			if(dataAnimation != ScriptRoot.DataAnimation)
			{	/* Different Animation-Pack */
				/* Destroy current Animation-Object */
				Destroy(GameObjectRoot);
				flagRootRenew = true;
			}
		}
		else
		{	/* Now No Animation-Object */
			flagRootRenew = true;
		}
		if(true == flagRootRenew)
		{
			/* Instantiate Animation-Object */
			GameObjectRoot = Library_SpriteStudio6.Utility.Asset.PrefabInstantiate(	(GameObject)DataProject.PrefabAnimation[indexPackAnimation],
																					null,
																					transform.gameObject,
																					false
																				);
			if(null == GameObjectRoot)
			{
				return(false);
			}
			ScriptRoot = GameObjectRoot.GetComponent<Script_SpriteStudio6_Root>();
			ScriptRoot.FunctionPlayEnd = CallBackFunctionPlayEnd;
		}

		/* Play Animation */
		if(null == ScriptRoot)
		{
			return(false);
		}
		ScriptRoot.AnimationStop(-1);

		int indexAnimation = ScriptRoot.IndexGetAnimation(dataStep.NameAnimation);
		if(0 > indexAnimation)
		{
			return(false);
		}
		return(ScriptRoot.AnimationPlay(-1, indexAnimation, dataStep.PlayCount));
	}

	private bool CallBackFunctionPlayEnd(Script_SpriteStudio6_Root scriptRoot, GameObject objectControl)
	{
		/* Progress Step */
		IndexStep++;
		int countStep = CountGetStep();
		if(countStep <= IndexStep)
		{	/* Range Over */
			if(null != FunctionPlayEnd)
			{
				FunctionPlayEnd(this);
			}

			/* Set behavior after finished */
			switch(TypeLoop)
			{
				case Library_SpriteStudio6.Data.Sequence.Type.LAST:
					IndexStep = countStep - 1;
					break;

				case Library_SpriteStudio6.Data.Sequence.Type.TOP:
					IndexStep = 0;
					break;

				case Library_SpriteStudio6.Data.Sequence.Type.KEEP:
				default:
					/* MEMO: Step is not changed. */
					return(true);
			}
		}

		Status |= FlagBitStatus.UPDATE_STEP;

		return(true);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	[System.Flags]
	private enum FlagBitStatus
	{
		VALID = 0x40000000,
		PLAYING = 0x20000000,
		PAUSING = 0x10000000,

		INITIAL_EXECUTE_UPDATE = 0x08000000,

		UPDATE_STEP = 0x00800000,
		UPDATE_PAUSE = 0x00400000,
		UPDATE_RATE_SCALELOCAL = 0x00200000,
		UPDATE_RATE_OPACITY = 0x00100000,

		CLEAR = 0x00000000,
		UPDATE_ALL = (UPDATE_STEP | UPDATE_PAUSE | UPDATE_RATE_SCALELOCAL | UPDATE_RATE_OPACITY),
	}
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	#endregion Classes, Structs & Interfaces
}
