/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

#define MESSAGE_DATAVERSION_INVALID
#define SUPPORT_TIMELINE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// [DefaultExecutionOrder(20)]
[ExecuteInEditMode]
[System.Serializable]
#if SUPPORT_TIMELINE
public partial class Script_SpriteStudio6_Replicate : MonoBehaviour, UnityEngine.Timeline.ITimeControl
#else
public partial class Script_SpriteStudio6_Replicate : MonoBehaviour
#endif
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	/* MEMO: "InstanceRootOriginal", "InstanceRootEffectOriginal" and "InstanceSequenceOriginal" are */
	/*         defined "public", but you must not access directly. Only for the inspector.           */
	/*       And Mutually-Exclusive. Therefore, cannot set more than one original.                   */
	public Script_SpriteStudio6_Root InstanceRootOriginal;
	public Script_SpriteStudio6_RootEffect InstanceRootEffectOriginal;
	public Script_SpriteStudio6_Sequence InstanceSequenceOriginal;

	public bool FlagHideForce;

#if SUPPORT_TIMELINE
	protected double TimePreviousTimeline = double.NaN;
	protected float TimeElapsedTimeline = float.NaN;
#endif

	public bool StatusIsValidOriginal
	{
		get
		{
			return(	(null != InstanceRootOriginal)
					|| (null != InstanceRootEffectOriginal)
					|| (null != InstanceSequenceOriginal)
				);	/* ? true : false */
		}
	}
	public bool StatusIsCompeteSetOriginal
	{
		get
		{
			return(	(null != InstanceMeshRendererOriginal)
					&& (null != InstanceMeshFilterOriginal)
				);
		}
	}
	public bool StatusIsBusy
	{
		get
		{
//			return(	(true == StatusIsCompeteSetOriginal)
//					&& (true == StatusIsRenderable)
//				);	/* ? true : false */
			return(null != DrawUpdate);	/* ? true : false */
		}
	}
	public bool StatusIsRenderable
	{
		get
		{
			return(	(null != InstanceMeshRenderer)
					&& (null != InstanceMeshFilter)
				);	/* ? true : false */
		}
	}
#if SUPPORT_TIMELINE
	public bool StatusIsControlledTimeline
	{
		get
		{
			/* MEMO:  */
			return(true == double.IsNaN(TimePreviousTimeline));	/* ? true : false */
		}
	}
#endif

	private FunctionDrawUpdate DrawUpdate = null;
	private MeshRenderer InstanceMeshRendererOriginal = null;
	private MeshFilter InstanceMeshFilterOriginal = null;
	private MeshRenderer InstanceMeshRenderer = null;
	private MeshFilter InstanceMeshFilter = null;

#if SUPPORT_TIMELINE
	public Library_SpriteStudio6.CallBack.FunctionTimelineReplicate FunctionTimeline = null;
#endif
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
//	void Awake()
//	{
//	}

//	void Start()
//	{
//	}

//	void Update()
//	{
//	}

	void LateUpdate()
	{
		if(true == BootUp())
		{
			if(true == StatusIsRenderable)
			{
				/* Set Hide-State */
				InstanceMeshRenderer.enabled = !FlagHideForce;

				/* Update Drawing Mesh */
				if(true == StatusIsBusy)
				{
					DrawUpdate();
				}
			}
		}
	}

//	void OnDestroy()
//	{
//	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- ITimeControl-Functions */
	#region ITimeControl-Functions
#if SUPPORT_TIMELINE
	public void OnControlTimeStart()
	{
		/* MEMO: Just in case, Call initialization. */
		TimePreviousTimeline = 0.0;	/* Busy */
		TimeElapsedTimeline = 0.0f;

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

		/* Execute CallBack */
		if(null != FunctionTimeline)
		{
			if(false == FunctionTimeline(this, Library_SpriteStudio6.KindSituationTimeline.END, float.NaN, double.NaN))
			{
				/* MEMO: When "FunctionTimeline" (call at the end) returns false, destroy self. */
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
	private bool BootUp()
	{
		/* Correct own-required components */
		if(null == InstanceMeshRenderer)
		{
			InstanceMeshRenderer = gameObject.GetComponent<MeshRenderer>();
		}
		if(null == InstanceMeshFilter)
		{
			InstanceMeshFilter = gameObject.GetComponent<MeshFilter>();
		}

		/* Set Original Object */
		if(false == StatusIsCompeteSetOriginal)
		{
			if(null != InstanceRootOriginal)
			{	/* Original: Animation */
				if(false == OriginalSet(InstanceRootOriginal))
				{
					/* MEMO: Error-Recover Process */
				}
			}
			else if(null != InstanceRootEffectOriginal)
			{	/* Original: Effect */
				if(false == OriginalSet(InstanceRootEffectOriginal))
				{
					/* MEMO: Error-Recover Process */
				}
			}
			else if(null != InstanceSequenceOriginal)
			{	/* Original: Sequence */
				if(false == OriginalSet(InstanceSequenceOriginal))
				{
					/* MEMO: Error-Recover Process */
				}
			}
			/* MEMO: Not Busy */
		}

		return(StatusIsValidOriginal);
	}

	/* ********************************************************* */
	//! Stop Replicating Animation
	/*!
	@param	none

	@retval	none

	Stop replication animation and purge all resources.<br>
	After calling this function, this class will not work unless
		  function"OriginalSet" is used again.<br>
	*/
	public void Stop()
	{
		/* Erase reference to original */
		OriginalCleanUp();

		/* Purge rendering resources */
		if(null != InstanceMeshFilter)
		{
			InstanceMeshFilter.sharedMesh = null;
		}
		if(null != InstanceMeshRenderer)
		{
			/* MEMO: Must not set "sharedMaterials" to null. Exception will be occurred. */
//			InstanceMeshRenderer.sharedMaterials = null;
			InstanceMeshRenderer.sharedMaterials = TableMaterialEmpty;
		}
		DrawUpdateRenderer(null);

		/* WorkArea Clean */
		DrawUpdate = null;

		InstanceMeshRendererOriginal = null;
		InstanceMeshFilterOriginal = null;
	}

	/* ********************************************************* */
	//! Set Source animation (for Animation-Object)
	/*!
	@param	instanceOriginal
		Source Animation-Object

	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Set the Animation-Object as replicate source.
	*/
	public bool OriginalSet(Script_SpriteStudio6_Root instanceOriginal)
	{
		OriginalCleanUp();

		/* Mesh-Control Components Correct */
		if(false == ComponentSetMesh(instanceOriginal.gameObject))
		{
			return(false);
		}

		DrawUpdate = DrawUpdateAnimation;
		InstanceRootOriginal = instanceOriginal;
		return(true);
	}

	/* ********************************************************* */
	//! Set Source animation (for Effect-Object)
	/*!
	@param	instanceOriginal
		Source Effect-Object

	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Set the Effect-Object as replicate source.
	*/
	public bool OriginalSet(Script_SpriteStudio6_RootEffect instanceOriginal)
	{
		OriginalCleanUp();

		/* Mesh-Control Components Correct */
		if(false == ComponentSetMesh(instanceOriginal.gameObject))
		{
			return(false);
		}

		DrawUpdate = DrawUpdateEffect;
		InstanceRootEffectOriginal = instanceOriginal;
		return(true);
	}

	/* ********************************************************* */
	//! Set Source animation (for Sequence-Object)
	/*!
	@param	instanceOriginal
		Source Sequecen-Object

	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Set the Sequence-Object as replicate source.
	*/
	public bool OriginalSet(Script_SpriteStudio6_Sequence instanceOriginal)
	{
		OriginalCleanUp();

		/* Mesh-Control Components Correct */
		/* MEMO: When "Sequence", Do not correct here.                                    */
		/*       Checked in DrawUpdate, since instance of "Script_SpriteStudio6_Root"     */
		/*         that actually performs drawing changes depending on the playing state, */

		DrawUpdate = DrawUpdateSequence;
		InstanceSequenceOriginal = instanceOriginal;
		return(true);
	}
	private void OriginalCleanUp()
	{
		InstanceRootOriginal = null;
		InstanceRootEffectOriginal = null;
		InstanceSequenceOriginal = null;
	}

	/* ******************************************************** */
	//! Get Replicate-Component
	/*!
	@param	gameObject
		GameObject of starting search
	@param	flagApplySelf
		true == Include "gameObject" as check target<br>
		false == exclude "gameObject"<br>
		Default: true
	@retval	Return-Value
		Instance of "Script_SpriteStudio6_Replicate"<br>
		null == Not-Found / Failure	

	Get component "Script_SpriteStudio6_Replicate" by examining "gameObject" and direct-children.<br>
	<br>
	This function returns "Script_SpriteStudio6_Sequence" first found.<br>
	However, it is not necessarily in shallowest GameObject-hierarchy.<br>
	(Although guarantee up to direct-children, can not guarantee if farther than direct-children)<br>
	*/
	public static Script_SpriteStudio6_Replicate ReplicateGet(GameObject gameObject, bool flagApplySelf=true)
	{
		Script_SpriteStudio6_Replicate scriptReplicate = null;

		/* Check Origin */
		if(true == flagApplySelf)
		{
			scriptReplicate = ReplicateGetMain(gameObject);
			if(null != scriptReplicate)
			{
				return(scriptReplicate);
			}
		}

		/* Check Direct-Children */
		/* MEMO: Processing is wastefull, but check direct-children first so that make to find in closely-relation as much as possible. */
		int countChild = gameObject.transform.childCount;
		Transform transformChild = null;

		for(int i=0; i<countChild; i++)
		{
			transformChild = gameObject.transform.GetChild(i);
			scriptReplicate = ReplicateGetMain(transformChild.gameObject);
			if(null != scriptReplicate)
			{
				return(scriptReplicate);
			}
		}

		/* Check Children */
		for(int i=0; i<countChild; i++)
		{
			transformChild = gameObject.transform.GetChild(i);
			scriptReplicate = ReplicateGet(transformChild.gameObject, false);
			if(null != scriptReplicate)
			{	/* Has Root-Parts */
				return(scriptReplicate);
			}
		}

		return(null);
	}
	private static Script_SpriteStudio6_Replicate ReplicateGetMain(GameObject gameObject)
	{
		Script_SpriteStudio6_Replicate scriptReplicate = null;
		scriptReplicate = gameObject.GetComponent<Script_SpriteStudio6_Replicate>();
		if(null != scriptReplicate)
		{	/* Has Root-Parts */
			return(scriptReplicate);
		}

		return(null);
	}

	private void SelfDestroy()
	{
		Library_SpriteStudio6.Utility.Asset.ObjectDestroy(gameObject);
	}

	private bool ComponentSetMesh(GameObject instanceGameObject)
	{
		if(null == instanceGameObject)
		{
			return(false);
		}

		InstanceMeshRendererOriginal = instanceGameObject.GetComponent<MeshRenderer>();
		InstanceMeshFilterOriginal = instanceGameObject.GetComponent<MeshFilter>();

		return(	(null != InstanceMeshRendererOriginal)
				&& (null != InstanceMeshFilterOriginal)
			);	/* ? true : false */
	}

	private void DrawUpdateAnimation()
	{
		InstanceMeshFilter.sharedMesh = InstanceMeshFilterOriginal.sharedMesh;
		InstanceMeshRenderer.sharedMaterials = InstanceMeshRendererOriginal.sharedMaterials;

		DrawUpdateRenderer(InstanceRootOriginal.TableMaterialPropertyBlock);
	}
	private void DrawUpdateEffect()
	{
		InstanceMeshFilter.sharedMesh = InstanceMeshFilterOriginal.sharedMesh;
		InstanceMeshRenderer.sharedMaterials = InstanceMeshRendererOriginal.sharedMaterials;

		DrawUpdateRenderer(InstanceRootEffectOriginal.TableMaterialPropertyBlock);
	}
	private void DrawUpdateSequence()
	{
		/* Mesh-Control Components Correct */
		Script_SpriteStudio6_Root scriptRoot = InstanceSequenceOriginal.ScriptRoot;
		if(null == scriptRoot)
		{
			return;
		}
		if(false == ComponentSetMesh(scriptRoot.gameObject))
		{
			return;
		}

		InstanceMeshFilter.sharedMesh = InstanceMeshFilterOriginal.sharedMesh;
		InstanceMeshRenderer.sharedMaterials = InstanceMeshRendererOriginal.sharedMaterials;

		DrawUpdateRenderer(scriptRoot.TableMaterialPropertyBlock);
	}
	private void DrawUpdateRenderer(UnityEngine.MaterialPropertyBlock[] tableMaterialPropertyBlock)
	{
		if(null == tableMaterialPropertyBlock)
		{
			InstanceMeshRenderer.SetPropertyBlock(null);
		}
		else
		{
			int countMaterial = tableMaterialPropertyBlock.Length;
			if(0 == countMaterial)
			{
				InstanceMeshRenderer.SetPropertyBlock(null);
			}
			else
			{
				for(int i=0; i<countMaterial; i++)
				{
					InstanceMeshRenderer.SetPropertyBlock(tableMaterialPropertyBlock[i], i);
				}
			}
		}
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	private readonly static UnityEngine.Material[] TableMaterialEmpty = new Material[0];
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	private delegate void FunctionDrawUpdate();
	#endregion Classes, Structs & Interfaces

	/* ----------------------------------------------- Delegates */
	#region Delegates
	#endregion Delegates
}
