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

	public float RateTime;

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
		BaseAwake();
	}

	void Start()
	{
		/* Clear Status */
		Status = FlagBitStatus.CLEAR;

		/* Initialize Base-Class */
		BaseStart();

		/* Set Status Valid */
		Status |= FlagBitStatus.VALID;

		return;

//	Start_ErrorEnd:;
//		Status &= ~FlagBitStatus.VALID;
//		return;
	}

//	void Update()
//	{
//	}

	void LateUpdate()
	{
		if(null == ScriptRootParent)
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
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	public static Library_SpriteStudio6.Utility.Random.Generator InstanceCreateRandom()
	{
		return(new RandomGenerator());
	}

	private static float FunctionTimeElapseDefault(Script_SpriteStudio6_RootEffect scriptRoot)
	{
		return(Time.deltaTime);
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

		CLEAR = 0x00000000,
	}
	#endregion Enums & Constants
}
