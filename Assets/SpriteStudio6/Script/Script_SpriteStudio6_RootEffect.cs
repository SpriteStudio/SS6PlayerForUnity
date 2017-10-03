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

	public Library_SpriteStudio6.Control.Effect ControlEffect;

	public int LimitParticleDraw;
	internal int CountParticleMax = 0;	/* use only Highest-Parent-Root */

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
	internal bool StatusIsChangeCellMap
	{
		get
		{
			return(0 != (Status & FlagBitStatus.CHANGE_CELLMAP));
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

		/* Boot up Draw-Cluster */
		/* MEMO: Need to run before "ControlEffect.BootUp". */
		if(false == ClusterBootUpDraw())
		{
			goto Start_ErrorEnd;
		}

		/* Boot up Effect-Control */
		if(false == ControlEffect.BootUp(this))
		{
			goto Start_ErrorEnd;
		}
		FrameRange = (float)ControlEffect.DurationFull;

		Status |= FlagBitStatus.VALID;

		/* Play Animation Initialize */
		if(null == InstanceRootParent)
		{
			AnimationPlay();
		}

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
			Matrix4x4 matrixInverseMeshRenderer = InstanceMeshRenderer.localToWorldMatrix.inverse;
			LateUpdateMain(FunctionTimeElapse(this), ref matrixInverseMeshRenderer);
		}
	}
	internal void LateUpdateMain(float timeElapsed, ref Matrix4x4 matrixCorrection)
	{
		if(0 == (Status & FlagBitStatus.VALID))
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

		/* Clear Draw-Cluster */
		bool flagDraw = ((null == InstanceRootParent) && (false == FlagHideForce)) ? true : false;
		if(true == flagDraw)
		{
			if(null == ClusterDraw)
			{	/* Lost */
				if(false == ClusterBootUpDraw())
				{	/* Recovery failure */
					return;
				}
			}
			ClusterDraw.DataPurge();
		}

		/* Update & Draw Effect */
		ControlEffect.Update(this, ref matrixCorrection);

		/* Combine & Draw Mesh */
		/* MEMO: Execute combining and drawing only at Highest-Parent-Root. */
		if(true == flagDraw)
		{
			if(false == RendererBootUpDraw(false))
			{	/* Recovery failure */
				return;
			}

			if(null != MeshCombined)	/* && (null == InstanceRootParent) */
			{
				Material[] tableMaterialCombine = ClusterDraw.MeshCombine(MeshCombined);
				if(null != tableMaterialCombine)
				{
					InstanceMeshRenderer.sharedMaterials = tableMaterialCombine;
					InstanceMeshFilter.sharedMesh = MeshCombined;
				}
			}
		}

		/* Clear transient status */
		StatusPlaying &= ~Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.PLAYING_START;
		Status &= ~FlagBitStatus.CHANGE_CELLMAP;
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	/* ********************************************************* */
	//! Get Material
	/*!
	@param	indexCellMap
		Serial-number of using Cell-Map
	@param	operationBlend
		Color-Blend Operation for the target
	@retval	Return-Value
		Material
	*/
	public Material MaterialGet(int indexCellMap, Library_SpriteStudio6.KindOperationBlendEffect operationBlend)
	{
		const int CountLength = (int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR;
		if(	(0 <= indexCellMap)
			&& ((null != TableMaterial) && ((TableMaterial.Length / CountLength) > indexCellMap))
			&& (Library_SpriteStudio6.KindOperationBlendEffect.NON < operationBlend) && (Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR > operationBlend)
			)
		{
			return(TableMaterial[(indexCellMap * CountLength) + (int)operationBlend]);
		}
		return(null);
	}

	private bool ClusterBootUpDraw()
	{
		Script_SpriteStudio6_Root instanceRootHighest = RootGetHighest();
		CountParticleMax = CountGetDrawMesh();

		if(null != instanceRootHighest)
		{	/* Child */
			ClusterDraw = instanceRootHighest.ClusterDraw;
		}
		else
		{	/* Highest-Root */
			ClusterDraw = new Library_SpriteStudio6.Draw.Cluster();
			if(null == ClusterDraw)
			{
				goto ClusterBootUpDraw_ErrorEnd;
			}
			if(false == ClusterDraw.BootUp(0, CountParticleMax))
			{
				goto ClusterBootUpDraw_ErrorEnd;
			}
		}

		return(true);

	ClusterBootUpDraw_ErrorEnd:;
		CountParticleMax = 0;
		ClusterDraw = null;
		return(false);
	}

	internal int CountGetDrawMesh()
	{
		return((0 == LimitParticleDraw) ? (int)Defaults.LIMIT_PARTICLEDRAW : LimitParticleDraw);
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
		PLAYING_INFINITY = 0x1000000,

		CHANGE_CELLMAP = 0x08000000,

		CLEAR = 0x00000000,
	}

	private readonly static System.DateTime TimeUnixEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
	private static uint RandomKeyMakeID = 123456;
	#endregion Enums & Constants
}
