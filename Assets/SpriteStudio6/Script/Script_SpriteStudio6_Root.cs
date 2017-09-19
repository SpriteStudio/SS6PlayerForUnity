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

	public InformationPlay[] TableInformationPlay;
	public Library_SpriteStudio6.Control.Animation.Parts[] TableControlParts;

	private FlagBitStatus Status;
	internal bool StatusIsValid
	{
		get
		{
			return(0 != (Status & FlagBitStatus.VALID));
		}
	}
	internal bool StatusIsCellMapChang
	{
		get
		{
			return(0 != (Status & FlagBitStatus.CHANGE_CELLMAP));
		}
	}

	private Library_SpriteStudio6.CallBack.FunctionTimeElapse FunctionTimeElapse = FunctionTimeElapseDefault;
	internal Library_SpriteStudio6.CallBack.FunctionTimeElapse CallBackFunctionTimeElapse
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
	internal Library_SpriteStudio6.CallBack.FunctionPlayEnd CallBackFunctionPlayEnd = null;
	internal Library_SpriteStudio6.CallBack.FunctionUserData CallBackFunctionUserData = null;

	public int LimitTrack;
	internal Library_SpriteStudio6.Control.Animation.Track[] TableControlTrack;
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

		/* Check master datas */
		FunctionBootUpDataAnimation();

		/* Check master datas */
		if((null == DataCellMap) || (null == DataAnimation))
		{
			goto Start_ErrorEnd;
		}

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
		if(false == ControlBootUpParts())
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
		if(null == ScriptRootParent)
		{
			/* MEMO: Execute only at the "Highest Parent(not under anyone's control)"-Root part.         */
			/*       "Child"-Root parts' "LateUpdatesMain" are called from Parent's internal processing. */
			LateUpdateMain(FunctionTimeElapse(this));
		}
	}
	internal void LateUpdateMain(float timeElapsed)
	{
		if(0 == (Status & FlagBitStatus.VALID))
		{
			return;
		}

		/* Update Base */
		BaseLateUpdate(timeElapsed);

		/* Update Play-Track */
		if(null == TableControlTrack)
		{	/* Lost */
			ControlBootUpTrack();
//			return;
		}
		int countControlTrack = TableControlTrack.Length;
		for(int i=0; i<countControlTrack; i++)
		{
			TableControlTrack[i].Update(timeElapsed);
		}

		/* Update Parts' GameObject(TRS) */
		int countControlParts = TableControlParts.Length;
		for(int i=0; i<countControlParts; i++)
		{
			TableControlParts[i].UpdateGameObject(this, i);
		}

		/* Draw Parts */
		if(false == FlagHideForce)
		{
			int idPartsDrawNext = TableControlParts[0].IDPartsDrawNext;
			while(0 <= idPartsDrawNext)
			{
				TableControlParts[idPartsDrawNext].UpdateDraw(this, idPartsDrawNext);
				idPartsDrawNext = TableControlParts[idPartsDrawNext].IDPartsDrawNext;
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

	void OnDestroy()
	{
		if(null != TableControlParts)
		{
			int countControlParts = TableControlParts.Length;
			for(int i=0; i<countControlParts; i++)
			{
				TableControlParts[i].ShutDown();
			}
		}

//		BaseOnDestroy();
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	/* ******************************************************** */
	//! Get Material
	/*!
	@param	indexCellMap
		Serial-number of using Cell-Map
	@param	operationBlend
		Color-Blend Operation for the target
	@retval	Return-Value
		Material
	*/
	public Material MaterialGet(int indexCellMap, Library_SpriteStudio6.KindOperationBlend operationBlend)
	{
		const int CountLength = (int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR;
		if(	(0 <= indexCellMap)
			&& ((null != TableMaterial) && ((TableMaterial.Length / CountLength) > indexCellMap))
			&& (Library_SpriteStudio6.KindOperationBlend.NON < operationBlend) && (Library_SpriteStudio6.KindOperationBlend.TERMINATOR > operationBlend)
			)
		{
			return(TableMaterial[(indexCellMap * CountLength) + (int)operationBlend]);
		}
		return(null);
	}

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

		int countAnimation = DataAnimation.TableAnimation.Length;
		int countParts = DataAnimation.TableParts.Length;

		for(int i=0; i<countAnimation; i++)
		{
			for(int j=0; j<countParts; j++)
			{
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionStatus(DataAnimation.TableAnimation[i].TableParts[j].Status);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector3(DataAnimation.TableAnimation[i].TableParts[j].Position);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector3(DataAnimation.TableAnimation[i].TableParts[j].Rotation);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(DataAnimation.TableAnimation[i].TableParts[j].Scaling);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(DataAnimation.TableAnimation[i].TableParts[j].RateOpacity);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(DataAnimation.TableAnimation[i].TableParts[j].PositionAnchor);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(DataAnimation.TableAnimation[i].TableParts[j].SizeForce);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionUserData(DataAnimation.TableAnimation[i].TableParts[j].UserData);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionInstance(DataAnimation.TableAnimation[i].TableParts[j].Instance);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionEffect(DataAnimation.TableAnimation[i].TableParts[j].Effect);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(DataAnimation.TableAnimation[i].TableParts[j].RadiusCollision);

				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionCell(DataAnimation.TableAnimation[i].TableParts[j].Plain.Cell);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionColorBlend(DataAnimation.TableAnimation[i].TableParts[j].Plain.ColorBlend);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVertexCorrection(DataAnimation.TableAnimation[i].TableParts[j].Plain.VertexCorrection);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(DataAnimation.TableAnimation[i].TableParts[j].Plain.OffsetPivot);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(DataAnimation.TableAnimation[i].TableParts[j].Plain.PositionTexture);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(DataAnimation.TableAnimation[i].TableParts[j].Plain.ScalingTexture);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(DataAnimation.TableAnimation[i].TableParts[j].Plain.RotationTexture);

				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionInt(DataAnimation.TableAnimation[i].TableParts[j].Fix.IndexCellMap);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionCoordinateFix(DataAnimation.TableAnimation[i].TableParts[j].Fix.Coordinate);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionColorBlendFix(DataAnimation.TableAnimation[i].TableParts[j].Fix.ColorBlend);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionUVFix(DataAnimation.TableAnimation[i].TableParts[j].Fix.UV0);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(DataAnimation.TableAnimation[i].TableParts[j].Fix.SizeCollision);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(DataAnimation.TableAnimation[i].TableParts[j].Fix.PivotCollision);
			}
		}
	}
	private static void FunctionBootUpDataAnimationSignature()
	{
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

	private bool ControlBootUpParts()
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
			if(false == TableControlParts[i].BootUp(this, i))
			{
				goto ControlBootUpParts_ErrorEnd;
			}
		}
		return(true);

	ControlBootUpParts_ErrorEnd:;
		return(false);
	}

	private static float FunctionTimeElapseDefault(Script_SpriteStudio6_Root scriptRoot)
	{
		return(Time.deltaTime);
	}

	/* Part: SpriteStudio6/Script/Root/FunctionAnimation.cs */
	/* Part: SpriteStudio6/Script/Root/FunctionPlayTrack.cs */
	/* Part: SpriteStudio6/Script/Root/FunctionCellChange.cs */
	/* Part: SpriteStudio6/Script/Root/FunctionColorBlend.cs */
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
