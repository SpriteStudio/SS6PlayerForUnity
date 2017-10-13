/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public partial class Script_SpriteStudio6_Root :  Library_SpriteStudio6.Script.Root
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public Script_SpriteStudio6_DataAnimation DataAnimation;

	internal int CountPartsSprite = 0;
	internal int CountSpriteMax = 0;	/* use only Highest-Parent-Root */
	internal int CountParticleMax = 0;	/* use only Highest-Parent-Root */

	public int LimitTrack;
	internal Library_SpriteStudio6.Control.Animation.Track[] TableControlTrack = null;

	public InformationPlay[] TableInformationPlay;
	public Library_SpriteStudio6.Control.Animation.Parts[] TableControlParts;

	private FlagBitStatus Status = FlagBitStatus.CLEAR;
	internal bool StatusIsValid
	{
		get
		{
			return(0 != (Status & FlagBitStatus.VALID));
		}
	}
	internal bool StatusIsCellMapChange
	{
		get
		{
			return(0 != (Status & FlagBitStatus.CHANGE_CELLMAP));
		}
	}

	private Library_SpriteStudio6.CallBack.FunctionTimeElapse FunctionExecTimeElapse = FunctionTimeElapseDefault;
	internal Library_SpriteStudio6.CallBack.FunctionTimeElapse FunctionTimeElapse
	{
		get
		{
			return(FunctionExecTimeElapse);
		}
		set
		{
			FunctionExecTimeElapse = (null != value) ? value : FunctionTimeElapseDefault;
		}
	}
	internal Library_SpriteStudio6.CallBack.FunctionPlayEnd FunctionPlayEnd = null;
	internal Library_SpriteStudio6.CallBack.FunctionUserData FunctionUserData = null;

	internal Library_SpriteStudio6.CallBack.FunctionCallBackCollider FunctionColliderOnTriggerEnter = null;
	internal Library_SpriteStudio6.CallBack.FunctionCallBackCollider FunctionColliderOnTriggerExit = null;
	internal Library_SpriteStudio6.CallBack.FunctionCallBackCollider FunctionColliderOnTriggerStay = null;

	internal Library_SpriteStudio6.CallBack.FunctionCallBackCollision FunctionCollisionOnTriggerEnter = null;
	internal Library_SpriteStudio6.CallBack.FunctionCallBackCollision FunctionCollisionOnTriggerExit = null;
	internal Library_SpriteStudio6.CallBack.FunctionCallBackCollision FunctionCollisionOnTriggerStay = null;
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
		Status = FlagBitStatus.CLEAR;

		/* Boot up master datas */
		/* MEMO: Reason why initial setting of ScriptableObject is done here     */
		/*        (without processing with ScriptableObject's Awake or OnEnable) */
		/*        is to stabilize execution such when re-compile.                */
		if((null == DataCellMap) || (null == DataAnimation))
		{
			goto Start_ErrorEnd;
		}
		FunctionBootUpDataAnimation();

		/* Get Counts */
		CountPartsSprite  = DataAnimation.CountGetPartsSprite();
		CountSpriteMax = 0;	/* Set in ClusterBootUpDraw */
		CountParticleMax = 0;	/* Set in ClusterBootUpDraw */

		/* Start Base-Class */
		if(false == BaseStart())
		{
			goto Start_ErrorEnd;
		}

		/* Generate Play-Track */
		int countTrack = ControlBootUpTrack();
		if(0 >= countTrack)
		{
			goto Start_ErrorEnd;
		}

		/* Check Play-Information */
		if(false == InformationCheckPlay(countTrack))
		{
			goto Start_ErrorEnd;
		}

		/* Boot up Parts-Control */
		if(false == ControlBootUpParts(CountSpriteMax))
		{
			goto Start_ErrorEnd;
		}

		/* Boot up Draw-Cluster */
		/* MEMO: CAUTION. Caution that Parent-"Root" is not necessarily initialized earlier in generation order of GameObjects on the scene. */
		/*       ("ClusterDraw" is set null if before the parent's start)                                                                    */
		if(false == ClusterBootUpDraw())
		{
			goto Start_ErrorEnd;
		}

		Status |= FlagBitStatus.VALID;

		/* Set Initial Animations */
		/* MEMO: Initial animation only applies track 0.                                               */
		/*       For simplify setting (as interface for setting affecting part tables is complicated). */
		/*       And because animation blends are often controlled from scripts in many cases.         */
		AnimationPlay(	-1,
						((0 <= TableInformationPlay[0].IndexAnimation) ? TableInformationPlay[0].IndexAnimation : 0),
						TableInformationPlay[0].TimesPlay,
						TableInformationPlay[0].Frame,
						TableInformationPlay[0].RateTime,
						((true == TableInformationPlay[0].FlagPingPong) ? Library_SpriteStudio6.KindStylePlay.PINGPONG : Library_SpriteStudio6.KindStylePlay.NORMAL),
						TableInformationPlay[0].LabelStart,
						TableInformationPlay[0].FrameOffsetStart,
						TableInformationPlay[0].LabelEnd,
						TableInformationPlay[0].FrameOffsetEnd
					);
		if(true == TableInformationPlay[0].FlagStopInitial)
		{
			AnimationStop(-1, false);
		}

		return;

	Start_ErrorEnd:;
		TableControlTrack = null;

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
			if(true == RendererBootUpDraw(false))
			{
				Matrix4x4 matrixInverseMeshRenderer = InstanceMeshRenderer.localToWorldMatrix.inverse;
				LateUpdateMain(FunctionExecTimeElapse(this), false, ref matrixInverseMeshRenderer);
			}
		}
	}
	internal void LateUpdateMain(float timeElapsed, bool flagHideDefault, ref Matrix4x4 matrixCorrection)
	{
		if(0 == (Status & FlagBitStatus.VALID))
		{
			return;
		}

		bool flagHide = flagHideDefault | FlagHideForce;

		/* Update Base */
		BaseLateUpdate(timeElapsed);

		/* Update Play-Track */
		if(null == TableControlTrack)
		{	/* Lost */
			ControlBootUpTrack();
		}
		int countControlTrack = TableControlTrack.Length;
		for(int i=0; i<countControlTrack; i++)
		{
			TableControlTrack[i].Update(timeElapsed);
		}

		/* Update Parts' Common-Parameters (GameObject etc.) */
		int countControlParts = TableControlParts.Length;
		for(int i=0; i<countControlParts; i++)
		{
			TableControlParts[i].Update(this, i);
		}

		/* Recover Draw-Cluster & Component for Rendering */
		if(null == ClusterDraw)
		{	/* Lost */
			if(false == ClusterBootUpDraw())
			{	/* Recovery failure */
				return;
			}
		}

		/* MEMO: Execute combining and drawing only at Highest-Parent-Root. */
		/* Clean Draw-Cluster & Component for Rendering */
		if(null == InstanceRootParent)
		{
			ClusterDraw.DataPurge();

//			if(false == RendererBootUpDraw(false))
//			{	/* Recovery failure */
//				return;
//			}
		}

		/* Exec Drawing */
		/* MEMO: Caution that "Instance" and "Effect" are update in draw. */
		/* MEMO: Hidden "Normal" parts are not processed.(Not included in the Draw-Order-Chain) */
		int idPartsDrawNext = TableControlParts[0].IDPartsDrawNext;
		while(0 <= idPartsDrawNext)
		{
			TableControlParts[idPartsDrawNext].Draw(this, idPartsDrawNext, flagHide, ref matrixCorrection);
			idPartsDrawNext = TableControlParts[idPartsDrawNext].IDPartsDrawNext;
		}

		/* Mesh Combine & Set to Renderer */
		if(null == InstanceRootParent)
		{
			if(false == flagHide)
			{
				if(null != MeshCombined)	/* && (null == InstanceRootParent) */
				{
					Material[] tableMaterialCombine = ClusterDraw.MeshCombine(MeshCombined);
#if false
					if(null == tableMaterialCombine)
					{
						InstanceMeshRenderer.sharedMaterials = null;
						InstanceMeshFilter.sharedMesh = null;
					}
					else
					{
						InstanceMeshRenderer.sharedMaterials = tableMaterialCombine;
						InstanceMeshFilter.sharedMesh = MeshCombined;
					}
#else
					/* MEMO: Set the material-array to null issue "NullReferenceException". Leave as. */
					if(null != tableMaterialCombine)
					{
						InstanceMeshRenderer.sharedMaterials = tableMaterialCombine;
						InstanceMeshFilter.sharedMesh = MeshCombined;
					}
#endif
				}
			}
		}

		/* Clear transient status */
		for(int i=0; i<countControlTrack; i++)
		{
			/* MEMO: Originally should call function, but directly process (taking call-cost into account). */
			/* MEMO: Since clear bits only, VALID is not judged.                                  */
			/*       (Even if clearing those bits of stopping track, processing is not affected.) */
//			TableControlTrack[i].StatusClearTransient();
			TableControlTrack[i].Status &= ~(	Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.PLAYING_START
												| Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.DECODE_ATTRIBUTE
											);
		}
		Status &= ~FlagBitStatus.CHANGE_CELLMAP;
	}

//	void OnPostRender()
//	{
//		/* Clear Holding-Materials */
//		HolderMaterial.MaterialFree();
//	}
//	void OnDestroy()
//	{
//	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	private void FunctionBootUpDataAnimation()
	{
		if((null == DataAnimation) || (null == DataAnimation.TableParts) || (null == DataAnimation.TableAnimation))
		{
			return;
		}
		if(null != DataAnimation.SignatureBootUpFunction)
		{
			return;
		}

		DataAnimation.SignatureBootUpFunction = FunctionBootUpDataAnimationSignature;

		/* Recover Material */
		DataAnimation.BootUpTableMaterial();

		/* Set Attribute-Interface */
		DataAnimation.BootUpInterfaceAttribute();
	}
	private static void FunctionBootUpDataAnimationSignature()
	{
		/* Dummy-Function */
	}

	private int ControlBootUpTrack()
	{
		int countTrack = LimitGetTrack();
		TableControlTrack = new Library_SpriteStudio6.Control.Animation.Track[countTrack];
		if(null == TableControlTrack)
		{
			goto ControlBootUpTrack_ErrorEnd;
		}
		for(int i=0; i<countTrack; i++)
		{
			if(false == TableControlTrack[i].BootUp())
			{
				goto ControlBootUpTrack_ErrorEnd;
			}
		}

		return(countTrack);

	ControlBootUpTrack_ErrorEnd:;
		TableControlTrack = null;
		return(-1);
	}

	private bool InformationCheckPlay(int countTrack)
	{
		if(null == TableInformationPlay)
		{
			goto InformationCheckPlay_ErrorEnd;
		}
		if(countTrack != TableInformationPlay.Length)	/* (countTrack > TableInformationPlay.Length) */
		{
			goto InformationCheckPlay_ErrorEnd;
		}

		return(true);

	InformationCheckPlay_ErrorEnd:;
		return(false);
	}

	private bool ControlBootUpParts(int countPartsSprite)
	{
		int countParts;
		if(null == TableControlParts)
		{
			goto ControlBootUpParts_ErrorEnd;
		}

		countParts = TableControlParts.Length;
		if(DataAnimation.CountGetParts() != countParts)
		{
			goto ControlBootUpParts_ErrorEnd;
		}

		for(int i=0; i<countParts; i++)
		{
			if(false == TableControlParts[i].BootUp(this, i, countPartsSprite))
			{
				goto ControlBootUpParts_ErrorEnd;
			}
		}
		return(true);

	ControlBootUpParts_ErrorEnd:;
		return(false);
	}

	private bool ClusterBootUpDraw()
	{
		CountSpriteMax = 0;
		CountParticleMax = 0;
		if(null != InstanceRootParent)
		{	/* Child */
			ClusterDraw = InstanceRootParent.ClusterDraw;
		}
		else
		{	/* Highest-Root */
			if(false == CountGetDrawMesh(ref CountSpriteMax, ref CountParticleMax))
			{
				goto ClusterBootUpDraw_ErrorEnd;
			}

			ClusterDraw = new Library_SpriteStudio6.Draw.Cluster();
			if(null == ClusterDraw)
			{
				goto ClusterBootUpDraw_ErrorEnd;
			}
			if(false == ClusterDraw.BootUp(CountSpriteMax, CountParticleMax))
			{
				goto ClusterBootUpDraw_ErrorEnd;
			}
		}

		return(true);

	ClusterBootUpDraw_ErrorEnd:;
		CountSpriteMax = 0;
		CountParticleMax = 0;
		ClusterDraw = null;
		return(false);
	}

	internal bool CountGetDrawMesh(ref int countSprite, ref int countParticle)
	{
		if((null == DataAnimation) || (null == TableControlParts))
		{
			return(false);
		}

		countSprite += DataAnimation.CountGetPartsSprite();

		int countParts = DataAnimation.TableParts.Length;
		for(int i=0; i<countParts; i++)
		{
			switch(DataAnimation.TableParts[i].Feature)
			{
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
					{
						Script_SpriteStudio6_Root rootUnderControl = TableControlParts[i].InstanceRootUnderControl;
						if(null != rootUnderControl)
						{
							/* MEMO: "Instance" can be nested. */
							rootUnderControl.CountGetDrawMesh(ref countSprite, ref countParticle);
						}
					}
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
					{
						Script_SpriteStudio6_RootEffect rootUnderControl = TableControlParts[i].InstanceRootEffectUnderControl;
						if(null != rootUnderControl)
						{
							/* MEMO: "Effect" cannot control any animation-object. */
							countParticle += rootUnderControl.CountGetDrawMesh();
						}
					}
					break;
			}
		}
		return(true);
	}

	/* Part: SpriteStudio6/Script/Root/FunctionAnimation.cs */
	/* Part: SpriteStudio6/Script/Root/FunctionPlayTrack.cs */
	/* Part: SpriteStudio6/Script/Root/FunctionCellChange.cs */
	/* Part: SpriteStudio6/Script/Root/FunctionPartsColor.cs */

	private static float FunctionTimeElapseDefault(Script_SpriteStudio6_Root scriptRoot)
	{
		return(Time.deltaTime);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum Defaults
	{
		LIMIT_TRACK = 1,
	}

	[System.Flags]
	private enum FlagBitStatus
	{
		VALID = 0x40000000,

		CHANGE_CELLMAP = 0x08000000,

		CLEAR = 0x00000000,
	}
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	[System.Serializable]
	public struct InformationPlay
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		public bool FlagSetInitial;
		public bool FlagStopInitial;

		public int IndexAnimation;
		public bool FlagPingPong;
		public string LabelStart;
		public int FrameOffsetStart;
		public string LabelEnd;
		public int FrameOffsetEnd;
		public int Frame;
		public int TimesPlay;
		public float RateTime;
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public void CleanUp()
		{
			FlagSetInitial = false;
			FlagStopInitial = false;

			IndexAnimation = -1;
			FlagPingPong = false;
			LabelStart = "";
			FrameOffsetStart = 0;
			LabelEnd = "";
			FrameOffsetEnd = 0;
			Frame = 0;
			TimesPlay = 0;
			RateTime = 1.0f;
		}
		#endregion Functions
	}
	#endregion Classes, Structs & Interfaces
}
