/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasRenderer))]
[ExecuteInEditMode]
[System.Serializable]
public partial class Script_SpriteStudio6_RootUnityUI : UnityEngine.UI.MaskableGraphic
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public UnityEngine.Material MaterialMaster;
	protected UnityEngine.Material MaterialInUse = null;

	public UnityEngine.Texture Texture;
	public override Texture mainTexture
	{
		get
		{
			return(Texture);
		}
	}

	[HideInInspector] public Script_SpriteStudio6_PartsUnityUI[] ScriptParts;

	protected List<Script_SpriteStudio6_PartsUnityUI> OrderDraw = null;
	protected UnityEngine.CanvasRenderer InstanceRendererCanvas = null;

	/* Settings */
	public bool FlagHideForce;
	public bool FlagPlanarization;
	public Vector3 RateScaleLocal;

	public UnityEngine.Rendering.CompareFunction StencilCompare;
	public int StencilID;							/* -1: Auto / Lower-8bits Valid */

	/* WorkArea */
	protected bool MaskablePrevious = false;
	protected UnityEngine.Rendering.CompareFunction StencilComparePrevious = UnityEngine.Rendering.CompareFunction.Disabled;
	protected int StencilIDPrevious = -1;

	internal static Library_SpriteStudio6.Control.CacheMaterialStatic CacheMaterial = null;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
//	void Awake()
//	{
//	}

//	void Start()
//	{
//	}

	void Update()
	{
		/* MEMO: To be called "OnPopulateMesh" each loop, keep constantly dirty. */
		/*       (To be honest, not want to implement "Update"...)               */
		SetVerticesDirty();
	}

//	void LateUpdate()
//	{
//	}

	protected override void OnDestroy()
	{
		if(null != MaterialInUse)
		{
			if(null != CacheMaterial)
			{
				CacheMaterial.MaterialReleaseSpriteUI(MaterialInUse);
			}
		}
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Override-Functions */
	#region Override-Functions
	protected override void OnPopulateMesh(UnityEngine.UI.VertexHelper vertexHelper)
	{
		Texture texture = mainTexture;
		if(null == texture)
		{	/* Material is missing */
			return;
		}
		if(null == ScriptParts)
		{	/* Missing */
			return;
		}

		/* Renderer Initiallize */
		if(null == InstanceRendererCanvas)
		{	/* Not yet get  */
			InstanceRendererCanvas = gameObject.GetComponent<CanvasRenderer>();
			if(null == InstanceRendererCanvas)
			{	/* Failure (Not exist renderer) */
				return;
			}
		}

		int countParts = ScriptParts.Length;

		if(null == OrderDraw)
		{
			OrderDraw = new List<Script_SpriteStudio6_PartsUnityUI>(countParts);
		}
		OrderDraw.Clear();

		/* Update Parts */
		Matrix4x4 matrixTransform;
		for(int i=0; i<countParts; i++)
		{
			Script_SpriteStudio6_PartsUnityUI scriptParts = ScriptParts[i];

			/* Calculate Transform-Matrix */
			{
				Transform transform = scriptParts.transform;
//				Vector3 position = transform.localPosition;
//				Quaternion rotation = transform.localRotation;
				Vector3 scaling = transform.localScale;
				scaling.x *= RateScaleLocal.x;
				scaling.y *= RateScaleLocal.y;
				scaling.z *= RateScaleLocal.z;
				Matrix4x4 matrixTransformLocal = Matrix4x4.TRS(transform.localPosition, transform.localRotation, scaling);

				int indexPartsParent = scriptParts.IndexPartsParent;
				if(0 > indexPartsParent)
				{	/* Rooted */
					matrixTransform = Matrix4x4.identity;
				}
				else
				{	/* Child */
					matrixTransform = ScriptParts[indexPartsParent].MatrixTransform;
				}
				matrixTransform *= matrixTransformLocal;
				if(true == FlagPlanarization)
				{
					/* MEMO: Z-coordinate is always set to 0 after transformation. */
					matrixTransform[2, 0] = 
					matrixTransform[2, 1] = 
					matrixTransform[2, 2] = 
					matrixTransform[2, 3] = 0.0f;
				}
				scriptParts.MatrixTransform = matrixTransform;
			}

			/* Gather Draw-Order */
			switch(scriptParts.Feature)
			{
//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
					/* MEMO: Not draw */
					continue;	/* i-Loop */

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
					break;

//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONE:
//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MESH:
//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.TRANSFORM_CONSTRAINT:
//				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CAMERA:

				default:
					/* MEMO: Basically, unreachable. */
					continue;	/* i-Loop */
			}

			if(0.5f < scriptParts.FlagHide)	/* true == scriptParts.FlagHide */
			{	/* Hide */
				continue;	/* i-Loop */
			}

			float orderDrawParts = scriptParts.OrderDraw;
			int countOrder = OrderDraw.Count;
			bool flagAdded = false;
			for(int j=0; j<countOrder; j++)
			{
				if(OrderDraw[j].OrderDraw > orderDrawParts)
				{
					OrderDraw.Insert(j, scriptParts);

					flagAdded = true;
					break;	/* j-Loop */
				}
			}
			if(false == flagAdded)
			{
				OrderDraw.Add(scriptParts);
			}
		}

		/* Populate Mesh */
		countParts = OrderDraw.Count;
		vertexHelper.Clear();

		if(false == FlagHideForce)
		{
			int indexVertex = 0;
			Vector2 reciprocalSizeTexture = new Vector2(1.0f / texture.width, 1.0f / texture.height);
			for(int i=0; i<countParts; i++)
			{
				indexVertex = OrderDraw[i].MeshPopulate(	vertexHelper,
															indexVertex,
															reciprocalSizeTexture,
															color
													);
			}
		}
	}

	public override UnityEngine.Material GetModifiedMaterial(UnityEngine.Material materialBase)
	{
		/* Update material in use */
		bool flagModifyMaterial = false;

		if(null == CacheMaterial)
		{
			/* Boot-up cache */
			CacheMaterial = new Library_SpriteStudio6.Control.CacheMaterialStatic();
			CacheMaterial.BootUp(CountCacheMaterialInitial);
		}

		if(null == MaterialInUse)
		{	/* Has no instance */
			flagModifyMaterial = true;
		}

		if(MaskablePrevious != maskable)
		{	/* State changed */
			flagModifyMaterial = true;
		}
		else
		{
			if(true == maskable)
			{
				if(	(StencilComparePrevious != StencilCompare)
					|| (StencilIDPrevious != StencilID)
				)
				{	/* Stencil-parameter changed */
					flagModifyMaterial = true;
				}
			}
		}
		if(false == flagModifyMaterial)
		{	/* No changed */
			return(MaterialInUse);
		}

		/* Determine parameters */
		UnityEngine.Rendering.CompareFunction stencilCompare;
		int stencilID;

		MaskablePrevious = maskable;
		StencilComparePrevious = StencilCompare;
		StencilIDPrevious = StencilID;

		if(false == maskable)
		{	/* UnMaskable */
			stencilCompare = UnityEngine.Rendering.CompareFunction.Always;
			stencilID = 0;
		}
		else
		{	/* Maskable */
			stencilCompare = StencilComparePrevious;
			stencilID = StencilIDPrevious;
			
			if(UnityEngine.Rendering.CompareFunction.Disabled == stencilCompare)
			{	/* Default */
				stencilCompare = UnityEngine.Rendering.CompareFunction.Equal;
			}
			if(0 > stencilID)
			{	/* Auto ID */
				UnityEngine.Transform canvasRoot = UnityEngine.UI.MaskUtilities.FindRootSortOverrideCanvas(transform);
				stencilID = UnityEngine.UI.MaskUtilities.GetStencilDepth(transform, canvasRoot);
			}
		}

		/* Get Material (from Material-Cache) */
		if(null != MaterialInUse)
		{
			CacheMaterial.MaterialReleaseSpriteUI(MaterialInUse);
		}
		MaterialInUse = CacheMaterial.MaterialGetSpriteUI(mainTexture, stencilCompare, stencilID, MaterialMaster, true);

		return(MaterialInUse);
	}
	#endregion Override-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	private int CountCacheMaterialInitial = 50;
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	#endregion Classes, Structs & Interfaces
}
