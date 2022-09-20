/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

#define MESSAGE_DATAVERSION_INVALID
#define SUPPORT_TIMELINE
#if UNITY_EDITOR
#define SUPPORT_PREVIEW
#endif
// #define EXPERIMENT_FOR_CAMERA

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[System.Serializable]
#if SUPPORT_TIMELINE
public partial class Script_SpriteStudio6_Root : Library_SpriteStudio6.Script.Root, UnityEngine.Timeline.ITimeControl
#else
public partial class Script_SpriteStudio6_Root : Library_SpriteStudio6.Script.Root
#endif
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public Script_SpriteStudio6_DataAnimation DataAnimation;

	internal int CountPartsSprite = 0;
	internal int CountSpriteMax = 0;	/* use only Highest-Parent-Root */
	internal int CountMeshMax = 0;	/* use only Highest-Parent-Root */
	internal int CountParticleMax = 0;	/* use only Highest-Parent-Root */

	public int LimitTrack;
	[System.NonSerialized] public Library_SpriteStudio6.Control.Animation.Track[] TableControlTrack = null;
	internal Library_SpriteStudio6.CallBack.FunctionControlEndTrackPlay[] FunctionPlayEndTrack = null;

	public InformationPlay[] TableInformationPlay;
	public Library_SpriteStudio6.Control.Animation.Parts[] TableControlParts;

	internal List<int> ListPartsDraw = null;
	internal List<int> ListPartsPreDraw = null;

	public bool FlagPlanarization;
	public bool FlagColliderInterlockHideForce;
	public int OrderInLayer;

#if SUPPORT_TIMELINE
	protected double TimePreviousTimeline = double.NaN;
	protected float TimeElapsedTimeline = float.NaN;
#endif

	private FlagBitStatus Status = FlagBitStatus.CLEAR;
	public bool StatusIsValid
	{
		get
		{
			return(0 != (Status & FlagBitStatus.VALID));
		}
	}
	public bool StatusIsPlaying
	{
		get
		{
			return(0 != (Status & FlagBitStatus.PLAYING));
		}
	}
#if SUPPORT_PREVIEW
	public bool StatusIsControlledPreview
	{
		get
		{
			return(0 != (Status & FlagBitStatus.CONTROLLED_PREVIEW));
		}
		set
		{
			if(true == value)
			{
				Status |= FlagBitStatus.CONTROLLED_PREVIEW;
			}
			else
			{
				Status &= ~FlagBitStatus.CONTROLLED_PREVIEW;
			}
		}
	}
#endif
#if SUPPORT_TIMELINE
	public bool StatusIsControlledTimeline
	{
		get
		{
			return(0 != (Status & FlagBitStatus.CONTROLLED_TIMELINE));
		}
	}
#endif
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
	internal bool StatusIsChangeTableMaterial
	{	/* Obsolete */
		get
		{
			return(0 != (Status & FlagBitStatus.CHANGE_TABLEMATERIAL));
		}
	}
	internal bool StatusIsChangeCellMap
	{
		get
		{
			return(0 != (Status & FlagBitStatus.CHANGE_CELLMAP));
		}
	}
	internal bool StatusIsChangeCacheMaterial
	{
		get
		{
			return(0 != (Status & FlagBitStatus.CHANGE_CACHEMATERIAL));
		}
		set
		{	/* MEMO: Change status from the parent Animation-Object. */
			if(true == value)
			{
				Status |= FlagBitStatus.CHANGE_CACHEMATERIAL;
			}
			else
			{
				Status &= ~FlagBitStatus.CHANGE_CACHEMATERIAL;
			}
		}
	}

	/* MEMO: Bellow 2 properties (RateOpacity/RateScaleLocal) are used to control from parent animation. */
	/*       In principle, do not change the value. Correctly operation is not guaranteed.               */
	public float RateOpacity
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
	public Vector2 RateScaleLocal
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

	/* MEMO: Do not use "StatusIsAnimationSynthesize". */
	/*       (Internal processing only)                */
	internal bool StatusIsAnimationSynthesize
	{
		get
		{
			return(0 != (Status & FlagBitStatus.ANIMATION_SYNTHESIZE));
		}
		set
		{
			/* MEMO: Once turn true, will not return to false. */
			Status |= FlagBitStatus.ANIMATION_SYNTHESIZE;
		}
	}

	/* MEMO: "FunctionTimeElapse" and "FunctionTimeElapseEffect" also apply to child-Animations. */
	private Library_SpriteStudio6.CallBack.FunctionTimeElapse FunctionExecTimeElapse = null;
	public Library_SpriteStudio6.CallBack.FunctionTimeElapse FunctionTimeElapse
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

	private Library_SpriteStudio6.CallBack.FunctionTimeElapseEffect FunctionExecTimeElapseEffect = null;
	public Library_SpriteStudio6.CallBack.FunctionTimeElapseEffect FunctionTimeElapseEffect
	{
		get
		{
			return(FunctionExecTimeElapseEffect);
		}
		set
		{
			FunctionExecTimeElapseEffect = (null != value) ? value : FunctionTimeElapseEffectDefault;
		}
	}

	public Library_SpriteStudio6.CallBack.FunctionPlayEnd FunctionPlayEnd = null;
	public Library_SpriteStudio6.CallBack.FunctionUserData FunctionUserData = null;
	public Library_SpriteStudio6.CallBack.FunctionSignal FunctionSignal = null;

	public Library_SpriteStudio6.CallBack.FunctionCallBackCollider FunctionColliderEnter = null;
	public Library_SpriteStudio6.CallBack.FunctionCallBackCollider FunctionColliderExit = null;
	public Library_SpriteStudio6.CallBack.FunctionCallBackCollider FunctionColliderStay = null;

#if SUPPORT_TIMELINE
	public Library_SpriteStudio6.CallBack.FunctionTimeline FunctionTimeline = null;
#endif

	/* MEMO: Integrated to "FunctionCollider..." */
	/* Obsolete */	// internal Library_SpriteStudio6.CallBack.FunctionCallBackCollision FunctionCollisionEnter = null;
	/* Obsolete */	// internal Library_SpriteStudio6.CallBack.FunctionCallBackCollision FunctionCollisionExit = null;
	/* Obsolete */	// internal Library_SpriteStudio6.CallBack.FunctionCallBackCollision FunctionCollisionStay = null;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
	void Awake()
	{
		/* Data Version Check */
		if((null == DataCellMap) || (null == DataAnimation))
		{	/* Data invalid */
			return;
		}
		if((false == DataCellMap.VersionCheckRuntime() || (false == DataAnimation.VersionCheckRuntime())))
		{	/* Data-Version invalid */
#if MESSAGE_DATAVERSION_INVALID
			Debug.LogError(	"SS6PU Error(Runtime): Not supported data-version. Need to re-import data. GameObject[" + name
							+ "] Animation[" + DataAnimation.Version.ToString()
							+ "] CellMap[" + DataCellMap.Version.ToString()
							+ "]"
				);
#endif
			return;
		}

#if EXPERIMENT_FOR_CAMERA
//		ArgumentShareEntire = null;
#endif

		/* Awake Base-Class */
		BaseAwake();
	}

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
		if((null == DataCellMap) || (null == DataAnimation))
		{	/* Data invalid */
			goto StartMain_ErrorEnd;
		}
		if((false == DataCellMap.VersionCheckRuntime() || (false == DataAnimation.VersionCheckRuntime())))
		{	/* Data-Version invalid */
			goto StartMain_ErrorEnd;
		}
		FunctionBootUpDataAnimation();

		/* Initialize "Elapsed-Time" CallBaks */
		/* MEMO: "null" is "Set Default-Function". */
		if(null == FunctionTimeElapse)
		{
			FunctionTimeElapse = null;
		}
		if(null == FunctionTimeElapseEffect)
		{
			FunctionTimeElapseEffect = null;
		}

		/* Get Counts */
		CountPartsSprite  = DataAnimation.CountGetPartsSprite();
		CountSpriteMax = 0;	/* Set in ClusterBootUpDraw */
		CountMeshMax = 0;	/* Set in ClusterBootUpDraw */
		CountParticleMax = 0;	/* Set in ClusterBootUpDraw */

		/* Start Base-Class */
		if(false == BaseStart())
		{
			goto StartMain_ErrorEnd;
		}

		/* Generate Play-Track */
		int countTrack = ControlBootUpTrack(-1);
		if(0 >= countTrack)
		{
			goto StartMain_ErrorEnd;
		}

		/* Check Play-Information */
		if(false == InformationCheckPlay(countTrack))
		{
			goto StartMain_ErrorEnd;
		}

		/* Boot up Parts-Control */
		if(false == ControlBootUpParts(CountSpriteMax))
		{
			goto StartMain_ErrorEnd;
		}

		/* Boot up Draw-Chain */
		if(false == ChainDrawBootUp())
		{
			goto StartMain_ErrorEnd;
		}

		/* Boot up Draw-Cluster */
		/* MEMO: CAUTION. Caution that Parent-"Root" is not necessarily initialized earlier in generation order of GameObjects on the scene. */
		/*       ("ClusterDraw" is set null if before the parent's start)                                                                    */
		if(false == ClusterBootUpDraw())
		{
			goto StartMain_ErrorEnd;
		}

		Status |= FlagBitStatus.VALID;

		/* Set Initial Animations */
		if(false == AnimationPlayInitial())
		{
			goto StartMain_ErrorEnd;
		}
		return;

	StartMain_ErrorEnd:;
		TableControlTrack = null;

		Status &= ~FlagBitStatus.VALID;
		return;
	}

//	void Update()
//	{
//	}

	void LateUpdate()
	{
#if SUPPORT_PREVIEW
		if(0 != (Status & FlagBitStatus.CONTROLLED_PREVIEW))
		{	/* in Preview */
			return;
		}
#endif

#if UNITY_EDITOR
		/* MEMO: Since time may pass even when not "Play Mode", prevents it. */
		if(false == EditorApplication.isPlaying)
		{
			LateUpdateEnter(0.0f);
		}
		else
		{
			LateUpdateEnter(1.0f);
		}
#else
		LateUpdateEnter(1.0f);
#endif
	}
#if SUPPORT_PREVIEW
	public void LateUpdatePreview()
	{
		if(0 == (Status & FlagBitStatus.CONTROLLED_PREVIEW))
		{	/* not in Preview */
			return;
		}

		LateUpdateEnter(1.0f);
	}
#endif
	void LateUpdateEnter(float rateTimeElapsedForce)
	{	/* MEMO: "rateTimeElapsedForce" is used to switch between advancing/stopping time on the editor. */
		if(null == InstanceRootParent)
		{
#if EXPERIMENT_FOR_CAMERA
			if(null == ArgumentShareEntire)
			{
				ArgumentShareEntire = new ArgumentContainer();
			}
			ArgumentShareEntire.CleanUp();
#endif

			/* MEMO: Execute only at the "Highest Parent(not under anyone's control)"-Root part.         */
			/*       "Child"-Root parts' "LateUpdatesMain" are called from Parent's internal processing. */
			if(true == RendererBootUpDraw(false))
			{
				Matrix4x4 matrixInverseMeshRenderer = Matrix4x4.identity;
#if UNITY_EDITOR
				float timeElapsed = 0.0f;
				if(null != FunctionExecTimeElapse)
				{
					timeElapsed = FunctionExecTimeElapse(this);
				}
#else
				float timeElapsed = FunctionExecTimeElapse(this);
#endif
				timeElapsed *= rateTimeElapsedForce;

				LateUpdateMain(	timeElapsed,
								false,
								Library_SpriteStudio6.KindMasking.FOLLOW_DATA,
								ref matrixInverseMeshRenderer,
								true,
								FlagPlanarization
							);
			}
		}
#if EXPERIMENT_FOR_CAMERA
		else
		{
			if(null == ArgumentShareEntire)
			{
				/* MEMO: Child animations share highest-parent's ArgumentContainer. */
				Script_SpriteStudio6_Root instanceRootParentHighest = RootGetHighest();
				if(null != instanceRootParentHighest)
				{
					ArgumentShareEntire = instanceRootParentHighest.ArgumentShareEntire;
				}
			}
		}
#endif
	}
	internal void LateUpdateMain(	float timeElapsed,
									bool flagHideDefault,
									Library_SpriteStudio6.KindMasking masking,
									ref Matrix4x4 matrixCorrection,
									bool flagInitializeMatrixCorrection,
									bool flagPlanarization
								)
	{
		if(0 == (Status & FlagBitStatus.VALID))
		{	/* Status invalid */
			return;
		}
		if((null == TableControlTrack) || (null == TableControlParts))
		{	/* Can not play */
			return;
		}
		if(null == DataAnimation)
		{	/* Master data lost */
			return;
		}
		if(false == DataAnimation.StatusIsBootup)
		{	/* Re-import data? or Re-compile during playing animation? */
			return;
		}

		/* Get Hidden state */
		bool flagHide = flagHideDefault | FlagHideForce;

		/* Clear Draw-Chain */
		if(false == ChainDrawBootUp())
		{	/* Failure Clear Draw-Chain */
			return;
		}

		/* Update Base */
		BaseLateUpdate(timeElapsed);

		/* Update Play-Track */
		if(null == TableControlTrack)
		{	/* Lost */
			ControlBootUpTrack(-1);
		}
		int countControlTrack = TableControlTrack.Length;
		for(int i=0; i<countControlTrack; i++)
		{
			TableControlTrack[i].Update(timeElapsed);
		}

		/* Update Parts' Common-Parameters (GameObject etc.) */
		/* MEMO: In case of the most-Parent-"Root" node, if MeshRenderer's matrix is not acquired after updating Transform first, "matrixCorrection" shifts 1 frame. */
		int countControlParts = TableControlParts.Length;
		int indexTrackRoot = TableControlParts[0].IndexControlTrack;	/* (0 < countControlParts) ? TableControlParts[0].IndexControlTrack : -1; */
		Status &= ~FlagBitStatus.ANIMATION_SYNTHESIZE;
		TableControlParts[0].Update(	this,
										0,
										flagHide,
										ref matrixCorrection,
										indexTrackRoot
								);
		if(true == flagInitializeMatrixCorrection)
		{
			matrixCorrection = InstanceMeshRenderer.localToWorldMatrix.inverse;
		}
		for(int i=1; i<countControlParts; i++)
		{
			TableControlParts[i].Update(	this,
											i,
											flagHide,
											ref matrixCorrection,
											indexTrackRoot
									);
		}
		bool flagAnimationSynthesize = (0 != (Status & FlagBitStatus.ANIMATION_SYNTHESIZE));	/* ? true : false */

		/* Recover Draw-Cluster & Component for Rendering */
		if(null == ClusterDraw)
		{	/* Lost */
			if(false == ClusterBootUpDraw())
			{	/* Recovery failure */
				return;
			}
		}

#if EXPERIMENT_FOR_CAMERA
		/* Fix Camera */
		if(null == InstanceRootParent)
		{
			if(null != ArgumentShareEntire)
			{
				if(null != ArgumentShareEntire.TransformPartsCamera)
				{
					Matrix4x4 matrixCamera = matrixCorrection * ArgumentShareEntire.TransformPartsCamera.localToWorldMatrix;
					ArgumentShareEntire.MatrixCamera = matrixCamera.inverse;
				}
			}
		}
#endif

		/* Exec Drawing */
		/* MEMO: At "Pre-Draw" ...                                                                                             */
		/*       First render all "Mask"s.                                                                                     */
		/*       After that, render "Mask"s again according to priority at "Draw" timing. (Process of removing "Mask"s)        */
		/*       Caution that rendering "Mask"s is only Highest-Parent-Root. ("Instance"s and "Effect"s do not render "Mask"s) */
		/* MEMO: At "Draw" ...                                                                                                 */
		/*       Caution that "Instance" and "Effect" are update in draw.                                                      */
		/*       Hidden "Normal" parts are not processed.(Not included in the Draw-Order-Chain)                                */
		/* MEMO: Before 1.0.x, draw-order are baked. */
		int idPartsDrawNext;
		if(true == flagAnimationSynthesize)
		{
			int countChainDraw;

			/* Sort Draw-Chain */
			/* MEMO: "PreDraw"'s drawing order is reversed, but since sort-key is inversed, not necessary to exec "Reverse()" after "Sort()". */
			ListPartsPreDraw.Sort();
			ListPartsDraw.Sort();

			if(null == InstanceRootParent)
			{
				/* Clean Draw-Cluster & Component for Rendering */
				/* MEMO: Execute combining and drawing only at Highest-Parent-Root. */
				ClusterDraw.DataPurge();

				/* Exec "Pre-Draw" */
				countChainDraw = ListPartsPreDraw.Count;
				for(int i=0; i<countChainDraw; i++)
				{
					idPartsDrawNext = ListPartsPreDraw[i] & Library_SpriteStudio6.Control.Animation.MaskSortKeyIDParts;
					TableControlParts[idPartsDrawNext].PreDraw(	this,
																idPartsDrawNext,
																flagHide,
																masking,
																ref matrixCorrection,
																flagPlanarization
															);
				}
			}

			/* Exec "Draw" */
			countChainDraw = ListPartsDraw.Count;
			for(int i=0; i<countChainDraw; i++)
			{
				idPartsDrawNext = ListPartsDraw[i] & Library_SpriteStudio6.Control.Animation.MaskSortKeyIDParts;
				TableControlParts[idPartsDrawNext].Draw(	this,
															idPartsDrawNext,
															flagHide,
															masking,
															ref matrixCorrection,
															flagPlanarization
													);
			}
		}
		else
		{
			if(null == InstanceRootParent)
			{
				/* Clean Draw-Cluster & Component for Rendering */
				/* MEMO: Execute combining and drawing only at Highest-Parent-Root. */
				ClusterDraw.DataPurge();

				/* Exec "Pre-Draw" */
				idPartsDrawNext = TableControlParts[0].IDPartsNextPreDraw;
				while(0 <= idPartsDrawNext)
				{
					TableControlParts[idPartsDrawNext].PreDraw(	this,
																idPartsDrawNext,
																flagHide,
																masking,
																ref matrixCorrection,
																flagPlanarization
															);
					idPartsDrawNext = TableControlParts[idPartsDrawNext].IDPartsNextPreDraw;
				}
			}

			/* Exec "Draw" */
			idPartsDrawNext = TableControlParts[0].IDPartsNextDraw;
			while(0 <= idPartsDrawNext)
			{
				TableControlParts[idPartsDrawNext].Draw(	this,
															idPartsDrawNext,
															flagHide,
															masking,
															ref matrixCorrection,
															flagPlanarization
													);
				idPartsDrawNext = TableControlParts[idPartsDrawNext].IDPartsNextDraw;
			}
		}

		/* Mesh Combine & Set to Renderer */
		if((null == InstanceRootParent) && (null != MeshCombined))
		{
			/* Clear Mesh */
			MeshCombined.Clear();
			MeshCombined.name = NameBatchedMesh;
			MeshCombined.hideFlags = HideFlags.DontSave;

			if(false == flagHide)
			{
				/* MEMO: Set the material-array to null issue "NullReferenceException". Leave as. */
				if(true == ClusterDraw.MeshCombine(MeshCombined, ref TableMaterialCombined, ref TableMaterialPropertyBlockCombined))
				{
					InstanceMeshRenderer.sortingOrder = OrderInLayer;
					InstanceMeshRenderer.sharedMaterials = TableMaterialCombined;
					int countMaterial = TableMaterialPropertyBlockCombined.Length;
					for(int i=0; i<countMaterial; i++)
					{
						InstanceMeshRenderer.SetPropertyBlock(TableMaterialPropertyBlockCombined[i], i);
					}
				}
			}
			InstanceMeshFilter.sharedMesh = MeshCombined;
		}

		/* Check Track-End */
		int indexTrackSecondary = -1;
		bool flagDecodeNextForce = false;
		bool flagStopAllTrack = true;
		bool flagRequestPlayEndTrack;
		int indexAnimation;
		for(int i=0; i<countControlTrack; i++)
		{
			if(true == TableControlTrack[i].StatusIsPlaying)
			{
				flagRequestPlayEndTrack = TableControlTrack[i].StatusIsRequestPlayEnd;
				indexAnimation = TableControlTrack[i].ArgumentContainer.IndexAnimation;

				/* Check Transition-End */
				flagDecodeNextForce = false;
				if(true == TableControlTrack[i].StatusIsRequestTransitionEnd)
				{
					indexTrackSecondary = TableControlTrack[i].IndexTrackSecondary;
					if(0 <= indexTrackSecondary)
					{
						/* Change Track Secondary to Primary */
						/* MEMO: Overwrite secondary track status. */
						flagRequestPlayEndTrack = TrackChangeSecondaryToPrimary(i, indexTrackSecondary);

						/* CallBack Transition-End */
						if((null != FunctionPlayEndTrack) && (null != FunctionPlayEndTrack[i]))
						{
							FunctionPlayEndTrack[i](	this,
														i,
														indexTrackSecondary,
														indexAnimation,
														TableControlTrack[i].ArgumentContainer.IndexAnimation
													);
						}
						flagDecodeNextForce = true;
					}
				}

				/* Check Track Play-End */
				if(true == flagRequestPlayEndTrack)
				{
					/* Stop Track */
					TableControlTrack[i].Stop();

					/* CallBack Play-End */
					/* MEMO: At Play-End callback, "indexTrackSecondary" is always -1. */
					if((null != FunctionPlayEndTrack) && (null != FunctionPlayEndTrack[i]))
					{
						FunctionPlayEndTrack[i](this, i, -1, indexAnimation, -1);
					}
				}
				else
				{
					flagStopAllTrack = false;
				}

				TableControlTrack[i].StatusIsRequestPlayEnd = false;
				TableControlTrack[i].StatusIsRequestTransitionEnd = false;
			}

#if false
			TableControlTrack[i].StatusClearTransient();
#else
			/* MEMO: Originally should call function, but directly process (taking call-cost into account). */
			/* MEMO: Since clear bits only, VALID is not judged.                                  */
			/*       (Even if clearing those bits of stopping track, processing is not affected.) */
			TableControlTrack[i].Status &= ~(	Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.PLAYING_START
												| Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.DECODE_ATTRIBUTE
												| Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.TRANSITION_START
												| Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.IGNORE_NEXTUPDATE_USERDATA
												| Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.IGNORE_NEXTUPDATE_SIGNAL
											);
			if(true == flagDecodeNextForce)
			{
				TableControlTrack[i].Status |= Library_SpriteStudio6.Control.Animation.Track.FlagBitStatus.DECODE_ATTRIBUTE;
			}

			TableControlTrack[i].TimeElapseReplacement = 0.0f;
#endif
		}

		/* Clear Transient-Status */
		Status &= ~(	FlagBitStatus.UPDATE_RATE_SCALELOCAL
						| FlagBitStatus.UPDATE_RATE_OPACITY
						| FlagBitStatus.CHANGE_TABLEMATERIAL
						| FlagBitStatus.CHANGE_CELLMAP
						| FlagBitStatus.CHANGE_CACHEMATERIAL
					);
		if(null != AdditionalColor)
		{
			AdditionalColor.Status &= ~Library_SpriteStudio6.Control.AdditionalColor.FlagBitStatus.CHANGE;
		}

		/* Check Animation-End */
		if(true == flagStopAllTrack)
		{
			if(0 != (Status & FlagBitStatus.PLAYING))
			{	/* Playing -> Stop */
				if(null != FunctionPlayEnd)
				{
					if(false == FunctionPlayEnd(this, InstanceGameObjectControl))
					{
						/* MEMO: When "FunctionPlayEnd" returns false, destroy self. */
						/*       If have "Control-Object", will destroy as well.     */
						/*       However, can not destroy when "Instance".           */
						SelfDestroy();
					}
				}
			}

			Status &= ~FlagBitStatus.PLAYING;
		}
		else
		{
			Status |= FlagBitStatus.PLAYING;
		}
	}

	void OnDestroy()
	{
		/* Shut-down Material-Cache */
		/* MEMO: Since child-animation's materials is also shut down comprehensively, */
		/*         need to be executed before BaseShutDown.                           */
		CacheShutDownMaterial();

		/* Shut-down base */
		base.BaseShutDown();
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- ITimeControl-Functions */
	#region ITimeControl-Functions
#if SUPPORT_TIMELINE
	public void OnControlTimeStart()
	{
		/* MEMO: Just in case, Call initialization. */
		Start();

		TimePreviousTimeline = 0.0;	/* Busy */
		TimeElapsedTimeline = 0.0f;
		Status |= FlagBitStatus.CONTROLLED_TIMELINE;

		/* Execute CallBack */
		if(null != FunctionTimeline)
		{
			/* MEMO: In this case, return value is ignored. */
			FunctionTimeline(this, Library_SpriteStudio6.KindSituationTimeline.START, float.NaN , double.NaN);
		}
	}

	public void OnControlTimeStop()
	{
		TimePreviousTimeline = double.NaN;	/* Not busy */
		TimeElapsedTimeline = float.NaN;
		Status &= ~FlagBitStatus.CONTROLLED_TIMELINE;

		/* Execute CallBack */
		if(null != FunctionTimeline)
		{
			if(false == FunctionTimeline(this, Library_SpriteStudio6.KindSituationTimeline.END, float.NaN, double.NaN))
			{
				/* MEMO: When "FunctionTimeline" (call at the end) returns false, destroy self. */
				/*       If have "Control-Object", will destroy as well.                        */
				/*       However, can not destroy when "Instance".                              */
				SelfDestroy();
			}
		}
	}

	public void SetTime(double time)
	{
		/* Calculate delta-Time */
		TimeElapsedTimeline = (float)(time - TimePreviousTimeline);

		/* Execute CallBack */
		if(null != FunctionTimeline)
		{
			/* MEMO: In this case, return value is ignored. */
			FunctionTimeline(this, Library_SpriteStudio6.KindSituationTimeline.UPDATE, TimeElapsedTimeline, time);
		}

		/* Update Time */
		TimePreviousTimeline = time;
	}
#endif
	#endregion ITimeControl-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	private void FunctionBootUpDataAnimation()
	{
		if((null == DataAnimation) || (null == DataAnimation.TableParts) || (null == DataAnimation.TableAnimation))
		{
			return;
		}
		if(true == DataAnimation.StatusIsBootup)
		{
			return;
		}

		/* Recover Material */
//		DataAnimation.BootUpTableMaterial();

		/* Set Attribute-Interface */
		DataAnimation.BootUpInterfaceAttribute();

		/* Set Signature-Bootup */
		DataAnimation.StatusIsBootup = true;
	}
	/* MEMO: Do not use this function. (only for internal processing, importer and inspectors) */
	public int LimitGetTrack()
	{
		return((0 < LimitTrack) ? LimitTrack : (int)Defaults.LIMIT_TRACK);
	}

	private int ControlBootUpTrack(int countTrack)
	{
		if(0 > countTrack)
		{
			countTrack = LimitGetTrack();
		}

		Library_SpriteStudio6.Control.Animation.Track[] tableControlTrackNow = TableControlTrack;
		Library_SpriteStudio6.CallBack.FunctionControlEndTrackPlay[] functionPlayEndTrackNow = FunctionPlayEndTrack;
		int countTrackNow = 0;
		bool flagRenew = true;

		if(null != tableControlTrackNow)
		{
			flagRenew = false;
			countTrackNow = tableControlTrackNow.Length;
			if(countTrackNow != countTrack)	/* (countTrackNow < countTrack) */
			{
				flagRenew = true;
			}
		}
		if(false == flagRenew)
		{
			return(countTrackNow);
		}

		/* Boot up */
		TableControlTrack = new Library_SpriteStudio6.Control.Animation.Track[countTrack];
		if(null == TableControlTrack)
		{
			goto ControlBootUpTrack_ErrorEnd;
		}

		FunctionPlayEndTrack = new Library_SpriteStudio6.CallBack.FunctionControlEndTrackPlay[countTrack];
		if(null == FunctionPlayEndTrack)
		{
			goto ControlBootUpTrack_ErrorEnd;
		}
		for(int i=0; i<countTrack; i++)
		{
			if(false == TableControlTrack[i].BootUp())
			{
				goto ControlBootUpTrack_ErrorEnd;
			}
			FunctionPlayEndTrack[i] = null;
		}

		/* Transfer state until just before */
		if(null != tableControlTrackNow)
		{
			if(countTrack < countTrackNow)
			{
				countTrackNow = countTrack;
			}
			for(int i=0; i<countTrackNow; i++)
			{
				TableControlTrack[i] = tableControlTrackNow[i];
				FunctionPlayEndTrack[i] = functionPlayEndTrackNow[i];
			}
		}
		tableControlTrackNow = null;
		functionPlayEndTrackNow = null;

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

	private bool ChainDrawBootUp()
	{
		int CountParts;
		if(null == ListPartsPreDraw)
		{
			CountParts = DataAnimation.CountGetPartsPreDraw();
			if(0 >= CountParts)
			{
				CountParts = 1;
			}
			ListPartsPreDraw = new List<int>(CountParts);
			if(null == ListPartsPreDraw)
			{
				goto ChainDrawBootUp_ErrorEnd;
			}
		}
		ListPartsPreDraw.Clear();
		if(null == ListPartsDraw)
		{
			CountParts = DataAnimation.CountGetPartsDraw();
			if(0 >= CountParts)
			{
				CountParts = 1;
			}
			ListPartsDraw = new List<int>(CountParts);
			if(null == ListPartsPreDraw)
			{
				goto ChainDrawBootUp_ErrorEnd;
			}
		}
		ListPartsDraw.Clear();

		return(true);

	ChainDrawBootUp_ErrorEnd:;
		ListPartsPreDraw = null;
		ListPartsDraw = null;
		return(false);
	}

	internal bool ClusterBootUpDraw()
	{
		CountSpriteMax = 0;
		CountMeshMax = 0;
		CountParticleMax = 0;
		if(null != InstanceRootParent)
		{	/* Child */
			ClusterDraw = InstanceRootParent.ClusterDraw;
		}
		else
		{	/* Highest-Root */
			if(false == CountGetDrawMesh(ref CountSpriteMax, ref CountMeshMax, ref CountParticleMax))
			{
				goto ClusterBootUpDraw_ErrorEnd;
			}

			ClusterDraw = new Library_SpriteStudio6.Draw.Cluster();
			if(null == ClusterDraw)
			{
				goto ClusterBootUpDraw_ErrorEnd;
			}
			if(false == ClusterDraw.BootUp(CountSpriteMax, CountMeshMax, CountParticleMax))
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
	private bool ClusterResetUnderControl()
	{
		if((null == DataAnimation) || (null == TableControlParts))
		{
			return(false);
		}

		int countParts = DataAnimation.TableParts.Length;
		for(int i=0; i<countParts; i++)
		{
			switch(DataAnimation.TableParts[i].Feature)
			{
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
					{
						Script_SpriteStudio6_Root rootUnderControl = TableControlParts[i].InstanceRootUnderControl;
						if(null != rootUnderControl)
						{
							/* MEMO: "Instance" can be nested. */
							rootUnderControl.ClusterBootUpDraw();
							rootUnderControl.ClusterResetUnderControl();
						}
					}
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
					{
						Script_SpriteStudio6_RootEffect rootUnderControl = TableControlParts[i].InstanceRootEffectUnderControl;
						if(null != rootUnderControl)
						{
							/* MEMO: "Effect" cannot control any animation-object. */
							rootUnderControl.ClusterBootUpDraw();
						}
					}
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MESH:
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.TRANSFORM_CONSTRAINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CAMERA:
					break;
			}
		}
		return(true);
	}

	internal bool CountGetDrawMesh(ref int countSprite, ref int countMesh, ref int countParticle)
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
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
					{
						Script_SpriteStudio6_Root rootUnderControl = TableControlParts[i].InstanceRootUnderControl;
						if(null != rootUnderControl)
						{
							/* MEMO: "Instance" can be nested. */
							rootUnderControl.CountGetDrawMesh(ref countSprite, ref countMesh, ref countParticle);
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

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MESH:
					countMesh += DataAnimation.TableParts[i].CountMesh;
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.TRANSFORM_CONSTRAINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CAMERA:
					break;
			}
		}
		return(true);
	}

	public bool AnimationPlayInitial()
	{
		if(0 != (Status & FlagBitStatus.PLAYING))
		{
			return(true);
		}

		/* MEMO: Initial animation only applies track 0.                                               */
		/*       For simplify setting (as interface for setting affecting part tables is complicated). */
		/*       And because animation blends are often controlled from scripts in many cases.         */
		int indexAnimation = -1;
		if(true == string.IsNullOrEmpty(TableInformationPlay[0].NameAnimation))
		{
			indexAnimation = 0;
			TableInformationPlay[0].NameAnimation = DataAnimation.TableAnimation[indexAnimation].Name;
		}
		else
		{
			indexAnimation = IndexGetAnimation(TableInformationPlay[0].NameAnimation);
			if(0 > indexAnimation)
			{
				indexAnimation = 0;
				TableInformationPlay[0].NameAnimation = DataAnimation.TableAnimation[indexAnimation].Name;
			}
		}
		AnimationPlay(	-1,
						indexAnimation,
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

		return(true);
	}

	private bool TrackChangeSecondaryToPrimary(int indexTrackPrimary, int indexTrackSecondary)
	{
		bool flagStartAnimation = TableControlTrack[indexTrackPrimary].StatusIsTransitionCancelPause;
		bool flagRequestPlayEndTrack = TableControlTrack[indexTrackSecondary].StatusIsRequestPlayEnd;

		/* Copy Track playing datas */
		/* MEMO: Since track-control manages only playing-frame, copy all. */
		/* MEMO: If destination-animation has ended at transition complete, will callback. */
		TableControlTrack[indexTrackPrimary] = TableControlTrack[indexTrackSecondary];

		/* Copy Parts-TRS */
		/* MEMO: copy TRSSecondary to TRSPrimary since decode states at transition end is in TRSSecondary. */
		int countControlParts = TableControlParts.Length;
		for(int i=0; i<countControlParts; i++)
		{
			if(TableControlParts[i].IndexControlTrack == indexTrackPrimary)
			{
				TableControlParts[i].TRSPrimary = TableControlParts[i].TRSSecondary;
				/* MEMO: Re-decode all attribute at next update. */
				TableControlParts[i].CacheClearAttribute(false, false);
			}
		}

		/* Clear Transition */
		TableControlTrack[indexTrackPrimary].StatusIsRequestTransitionEnd = false;
		TableControlTrack[indexTrackPrimary].StatusIsTransitionCancelPause = false;
		TableControlTrack[indexTrackPrimary].Transition(-1, 0.0f);

		/* Pause Cancel */
		if(true == flagStartAnimation)
		{
			TableControlTrack[indexTrackPrimary].Pause(false);
		}

		/* Stop Secaondary */
		TableControlTrack[indexTrackSecondary].Stop();

		return(flagRequestPlayEndTrack);
	}

	/* Part: SpriteStudio6/Script/Root/FunctionAnimation.cs */
	/* Part: SpriteStudio6/Script/Root/FunctionTrack.cs */
	/* Part: SpriteStudio6/Script/Root/FunctionCell.cs */
	/* Part: SpriteStudio6/Script/Root/FunctionMaterial.cs */
	/* Part: SpriteStudio6/Script/Root/FunctionMisc.cs */

	private float FunctionTimeElapseDefault(Script_SpriteStudio6_Root scriptRoot)
	{
		if(true == StatusIsControlledTimeline)
		{
			return(TimeElapsedTimeline);
		}
		return(Time.deltaTime);
	}
	private float FunctionTimeElapseEffectDefault(Script_SpriteStudio6_RootEffect scriptRoot)
	{
		if(true == StatusIsControlledTimeline)
		{
			return(TimeElapsedTimeline);
		}
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
		PLAYING = 0x20000000,

		UPDATE_RATE_SCALELOCAL = 0x08000000,
		UPDATE_RATE_OPACITY = 0x04000000,

		CHANGE_TABLEMATERIAL = 0x00800000,	/* Obsolete */
		CHANGE_CELLMAP = 0x00400000,
		CHANGE_CACHEMATERIAL = 0x00200000,

		ANIMATION_SYNTHESIZE = 0x00080000,
		CONTROLLED_PREVIEW = 0x00020000,
		CONTROLLED_TIMELINE = 0x00010000,

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
		public Script_SpriteStudio6_DataAnimation DataAnimation;

		public bool FlagSetInitial;
		public bool FlagStopInitial;

		public string NameAnimation;
		internal int IndexAnimation;
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
			DataAnimation = null;

			FlagSetInitial = false;
			FlagStopInitial = false;

			NameAnimation = "";
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

	public static partial class Cell
	{
		/* Part: SpriteStudio6/Script/Root/FunctionCell.cs */
	}

	public static partial class Material
	{
		/* Part: SpriteStudio6/Script/Root/FunctionMaterial.cs */
	}

	public static partial class Parts
	{
		/* Part: SpriteStudio6/Script/Root/FunctionMisc.cs */
	}
	#endregion Classes, Structs & Interfaces
}
