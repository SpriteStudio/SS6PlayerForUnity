/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

#define COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[System.Serializable]
public partial class Script_SpriteStudio6_PartsUnityNative : MonoBehaviour
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public Script_SpriteStudio6_RootUnityNative PartsRoot;

	/* MEMO: Can not control except float from "AnimationClip". */
	public float OrderInLayer;

	/* MEMO: When change cell from script, change this valiable not "SpriteRenderer.sprite (or SpriteMask.sprite)". */
	/* MEMO: Also used as buffer for mask's cell in Ver. 2.2.0 or later. (Shared between "Normal" parts and "Mask" parts) */
	public Sprite Cell;

	/* MEMO: When change cell from script, change this valiable not "SkinnedMeshRenderer.sharedMesh". */
	public Mesh CellMesh;
	public Texture2D TextureMesh;

#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
	/* MEMO: Do not change these variables externally. (Buffers "Writing from AnimationClip" and "Storing Initial-State") */
	public int[] TableIDPartsBone;
	public Matrix4x4 MatrixBase;

	public Vector2 ScaleLocal;
	public float /* bool */ FlagHide;
	public float RateOpacity;	/* At "Mask" parts, "(Masking) Power" stored. ("Mask-Power" is already inversed.) */

	public float /* Library_SpriteStudio6.KindOperationBlend */ PartsColorOperation;
	public Color PartsColorLU;
	public float PartsColorPowerLU;
	public Color PartsColorRU;
	public float PartsColorPowerRU;
	public Color PartsColorRD;
	public float PartsColorPowerRD;
	public Color PartsColorLD;
	public float PartsColorPowerLD;

	public Vector2 VertexCorrectionLU;
	public Vector2 VertexCorrectionRU;
	public Vector2 VertexCorrectionRD;
	public Vector2 VertexCorrectionLD;

	/* MEMO: "Unity Native" mode, UV-mapping or pivot modification are not supported. */
#else
#endif

	/* MEMO: Do not change these valiables. (Set only from importer) */
	public Transform[] TableTransformBone;

	public SpriteRenderer InstanceSpriteRenderer;
#if UNITY_2017_1_OR_NEWER
	public SpriteMask InstanceSpriteMask;
#else
	/* MEMO: Can not use "SpriteMask" in Unity5.6 or earlier.                               */
	/*       (For "Nintendo Switch" for the time being, corresponds to Unity5.6 or earlier) */
#endif

	public SkinnedMeshRenderer InstanceSkinnedMeshRenderer;
	public MeshRenderer InstanceMeshRenderer;	/* For mesh to which no bone is assigned. */
	public MeshFilter InstanceMeshFilter;	/* For mesh to which no bone is assigned. */

	private float OrderInLayerPrevious = float.NaN;
	private Sprite CellPrevious = null;
	private Mesh CellMeshPrevious = null;
	private Texture2D TextureMeshPrevious = null;

	private Mesh InstanceCellMesh = null;
	private Matrix4x4[] TableMatrixBindPose = null;

	private static MaterialPropertyBlock PropertyMaterial = null;
	private static int IDMaterialMainTexture = -1;
	private static int IDMaterialRectangleCell = -1;
	private static int IDMaterialPivotCell = -1;
#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
	private static int IDShaderBlendParam = -1;
	private static int IDShaderPartsColorLU = -1;
	private static int IDShaderPartsColorRU = -1;
	private static int IDShaderPartsColorRD = -1;
	private static int IDShaderPartsColorLD = -1;
	private static int IDShaderPartsColorOpacity = -1;
	private static int IDShaderVertexOffsetLURU = -1;
	private static int IDShaderVertexOffsetRDLD = -1;
//	private static int IDShaderCellPivotLocalScale =	/* IDMaterialPivotCell */
//	private static int IDShaderCellRectangle =	/* IDMaterialRectangleCell */
	private static int IDShaderPartsColorSkinnedMesh = -1;
#else
#endif

#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
	private FunctionUpdate FunctionExecUpdate = null;
#else
#endif
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
//	void Awake()
//	{
//	}

	void Start()
	{
		CleanUp();

		if(false == BootUp())
		{
			return;
		}

		BootUpPropertyMaterial();
	}

//	void Update()
//	{
//	}

	void LateUpdate()
	{
		int sortingOrder = 0;
		int sortingOffsetParts = 1;
		if(null != PartsRoot)
		{
			sortingOrder = PartsRoot.SortingOrder;
			sortingOffsetParts = PartsRoot.SortingOffsetPartsDraw;
			if(0 >= sortingOffsetParts)
			{
				sortingOffsetParts = 1;
			}
		}
#if UNITY_2017_1_OR_NEWER
		int sortingOrderBase = sortingOrder;
#else
		int sortingOrderBase = -1;
#endif
		sortingOrder += ((int)OrderInLayer) * sortingOffsetParts;

#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
		/* MEMO: Call dedicated Update-Function for each part kind. */
		if(null != FunctionExecUpdate)
		{
			FunctionExecUpdate(sortingOrder, sortingOrderBase);
		}
#else
		/* MEMO: "SpriteRenderer", "SpriteMask" and "SkinnedMeshRenderer" do not coexist. */
		/* Sprite (SpriteRenderer) */
		if(null != InstanceSpriteRenderer)
		{
			/* Priority Set */
			if(OrderInLayerPrevious != sortingOrder)
			{
				InstanceSpriteRenderer.sortingOrder = sortingOrder;
				OrderInLayerPrevious = sortingOrder;
			}

			if(null != PropertyMaterial)
			{
				/* Cell Set */
//				if(CellPrevious != Cell)
				{
					InstanceSpriteRenderer.GetPropertyBlock(PropertyMaterial);

					/* MEMO: Not enough to just set cell to "SpriteRenderer". (Need to set texture to shader) */
					InstanceSpriteRenderer.sprite = Cell;
//					PropertyMaterial.SetTexture(IDMaterialMainTexture, Cell.texture);

					Vector4 temp;
					Rect rectangleCell = Cell.rect;
					temp.x = rectangleCell.xMin;
					temp.y = rectangleCell.yMin;
					temp.z = rectangleCell.width;
					temp.w = rectangleCell.height;
					PropertyMaterial.SetVector(IDMaterialRectangleCell, temp);

					/* MEMO: Since "LocalScale" is stored together in "_CellPivot_LocalScale", overwrite value set by animation. */
#if false
					temp = PropertyMaterial.GetVector(IDMaterialPivotCell);
#else
					/* MEMO: Get result of AnimationClip, direct. */
					Material materialDraw = InstanceSpriteRenderer.sharedMaterial;
					if(	(null == materialDraw)
						|| (0 > IDMaterialPivotCell)
						|| (false == materialDraw.HasProperty(IDMaterialPivotCell))
					)
					{
						temp = Vector4.one;
					}
					else
					{
						temp = InstanceSpriteRenderer.sharedMaterial.GetVector(IDMaterialPivotCell);
					}
#endif

					Vector2 pivot = Cell.pivot;
					temp.x = pivot.x;
					temp.y = rectangleCell.height - pivot.y;
					PropertyMaterial.SetVector(IDMaterialPivotCell, temp);

					CellPrevious = Cell;

					InstanceSpriteRenderer.SetPropertyBlock(PropertyMaterial);
				}
			}

			return;
		}

		/* Mask (SpriteMask) */
#if UNITY_2017_1_OR_NEWER
		if(null != InstanceSpriteMask)
		{
			if(OrderInLayerPrevious != sortingOrder)
			{
				InstanceSpriteMask.frontSortingOrder = sortingOrder;
				InstanceSpriteMask.backSortingOrder = sortingOrderBase - PartsRoot.SortingOffsetPartsDraw;
				OrderInLayerPrevious = sortingOrder;
			}

			if(null != Cell)
			{
				InstanceSpriteMask.sprite = Cell;
				Cell = null;
			}

			return;
		}
#else
		/* MEMO: Can not use "SpriteMask" in Unity5.6 or earlier.                               */
		/*       (For "Nintendo Switch" for the time being, corresponds to Unity5.6 or earlier) */
#endif

		/* Mesh (SkinnedMeshRenderer) */
		if(null != InstanceSkinnedMeshRenderer)
		{
			if(OrderInLayerPrevious != sortingOrder)
			{
				InstanceSkinnedMeshRenderer.sortingOrder = sortingOrder;
				OrderInLayerPrevious = sortingOrder;
			}

			if(CellMeshPrevious != CellMesh)
			{
				if(null == InstanceCellMesh)
				{
					InstanceCellMesh = new Mesh();
					if(null == InstanceCellMesh)
					{	/* Error */
						return;
					}
				}

				/* Re-Set Mesh */
				InstanceCellMesh.Clear();
				InstanceCellMesh.name = CellMesh.name;
				InstanceCellMesh.vertices = CellMesh.vertices;
				InstanceCellMesh.uv = CellMesh.uv;
				InstanceCellMesh.triangles = CellMesh.triangles;
				InstanceCellMesh.boneWeights = CellMesh.boneWeights;
				InstanceCellMesh.bindposes = TableMatrixBindPose;

				InstanceSkinnedMeshRenderer.sharedMesh = InstanceCellMesh;

				CellMeshPrevious = CellMesh;
			}

			if(TextureMeshPrevious != TextureMesh)
			{
				InstanceSkinnedMeshRenderer.GetPropertyBlock(PropertyMaterial);

				PropertyMaterial.SetTexture(IDMaterialMainTexture, TextureMesh);

				InstanceSkinnedMeshRenderer.SetPropertyBlock(PropertyMaterial);

				TextureMeshPrevious = TextureMesh;
			}

			return;
		}

		/* Mesh (SkinnedMeshRenderer) */
		if(null != InstanceMeshRenderer)
		{
			if(OrderInLayerPrevious != sortingOrder)
			{
				InstanceMeshRenderer.sortingOrder = sortingOrder;
				OrderInLayerPrevious = sortingOrder;
			}

			if(CellMeshPrevious != CellMesh)
			{
				if(null == InstanceCellMesh)
				{
					InstanceCellMesh = new Mesh();
					if(null == InstanceCellMesh)
					{	/* Error */
						return;
					}
				}

				/* Re-Set Mesh */
				InstanceCellMesh.Clear();
				InstanceCellMesh.name = CellMesh.name;
				InstanceCellMesh.vertices = CellMesh.vertices;
				InstanceCellMesh.uv = CellMesh.uv;
				InstanceCellMesh.triangles = CellMesh.triangles;
//				InstanceCellMesh.boneWeights = 
//				InstanceCellMesh.bindposes = 

				InstanceMeshFilter.sharedMesh = InstanceCellMesh;

				CellMeshPrevious = CellMesh;
			}

			if(TextureMeshPrevious != TextureMesh)
			{
				InstanceMeshRenderer.GetPropertyBlock(PropertyMaterial);

				PropertyMaterial.SetTexture(IDMaterialMainTexture, TextureMesh);

				InstanceMeshRenderer.SetPropertyBlock(PropertyMaterial);

				TextureMeshPrevious = TextureMesh;
			}

			return;
		}
#endif
	}

	void OnDestroy()
	{
		if(null != InstanceCellMesh)
		{
			/* MEMO: "InstanceCellMesh.hideFlags" is set to "HideFlags.DontSave" so destroy manually. */
			InstanceCellMesh.Clear();
			Library_SpriteStudio6.Utility.Asset.ObjectDestroy(InstanceCellMesh);
			InstanceCellMesh = null;
		}
	}	

#if UNITY_EDITOR
	void OnValidate()
	{
		TextureMeshPrevious = null;
	}
#endif
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- MonoBehaviour */
	#region Functions
	private void CleanUp()
	{
		OrderInLayerPrevious = float.NaN;
		CellPrevious = null;
		CellMeshPrevious = null;
		TextureMeshPrevious = null;

		InstanceCellMesh = null;
		TableMatrixBindPose = null;
	}
	private bool BootUp()
	{
		FunctionExecUpdate = null;

//		InstanceSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		if(null != InstanceSpriteRenderer)
		{
#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
			/* Set Update-Function */
			FunctionExecUpdate = RenderingUpdateNormal;
#else
#endif
			return(true);
		}

#if UNITY_2017_1_OR_NEWER
//		InstanceSpriteMask = gameObject.GetComponent<SpriteMask>();
		if(null != InstanceSpriteMask)
		{
#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
			/* Set Update-Function */
#if UNITY_2017_1_OR_NEWER
			FunctionExecUpdate = RenderingUpdateMask;
#else
			/* MEMO: Can not use "SpriteMask" in Unity5.6 or earlier.                               */
			/*       (For "Nintendo Switch" for the time being, corresponds to Unity5.6 or earlier) */
//			FunctionExecUpdate = null;
#endif
#else
#endif
			return(true);
		}
#else
		/* MEMO: Can not use "SpriteMask" in Unity5.6 or earlier.                               */
		/*       (For "Nintendo Switch" for the time being, corresponds to Unity5.6 or earlier) */
#endif

//		InstanceSkinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
		if(null != InstanceSkinnedMeshRenderer)
		{
			/* Create Bind-Pose */
			PoseCreateBind();
			InstanceSkinnedMeshRenderer.bones = TableTransformBone;

#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
			/* Set Update-Function */
			FunctionExecUpdate = RenderingUpdateMeshSkinned;
#else
#endif
			return(true);
		}

		if(null != InstanceMeshRenderer)
		{
#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
			/* Set Update-Function */
			FunctionExecUpdate = RenderingUpdateMeshRigid;
#else
#endif
			return(true);
		}

#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
//		FunctionExecUpdate = null;
#else
#endif
		return(false);
	}
	private bool PoseCreateBind()
	{
		if(null != TableMatrixBindPose)
		{
			return(false);
		}

		/* Create Bind-Pose */
#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
		if(null != TableIDPartsBone)
		{
			int countBone = TableIDPartsBone.Length;
			TableMatrixBindPose = new Matrix4x4[countBone];
			Matrix4x4[] tableMatrixBone = PartsRoot.TableMatrixBoneSetup;
			if(null != TableMatrixBindPose)
			{

				Matrix4x4 matrixLocalToWorld = MatrixBase;	/* PartsRoot.transform.localToWorldMatrix * MatrixBase; */
				for(int i=0; i<countBone; i++)
				{
					int indexMatrix = TableIDPartsBone[i];
					TableMatrixBindPose[i] = tableMatrixBone[indexMatrix].inverse * matrixLocalToWorld;
				}
			}
		}
#else
		Matrix4x4 matrixLocalToWorld = transform.localToWorldMatrix;
		if(null != TableTransformBone)
		{
			int countTransformBone = TableTransformBone.Length;
			TableMatrixBindPose = new Matrix4x4[countTransformBone];
			if(null != TableMatrixBindPose)
			{
				for(int i=0; i<countTransformBone; i++)
				{
					TableMatrixBindPose[i] = TableTransformBone[i].worldToLocalMatrix * matrixLocalToWorld;
				}
			}
		}
#endif

		return(true);
	}
	private void BootUpPropertyMaterial()
	{
		if(null == PropertyMaterial)
		{
			PropertyMaterial = new MaterialPropertyBlock();
		}

		if(0 > IDMaterialMainTexture)
		{
			IDMaterialMainTexture = UnityEngine.Shader.PropertyToID("_MainTex");
		}
		if(0 > IDMaterialRectangleCell)
		{
			IDMaterialRectangleCell = UnityEngine.Shader.PropertyToID("_CellRectangle");
		}
		if(0 > IDMaterialPivotCell)
		{
			IDMaterialPivotCell = UnityEngine.Shader.PropertyToID("_CellPivot_LocalScale");
		}

#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
		if(0 > IDShaderBlendParam)
		{
			IDShaderBlendParam = UnityEngine.Shader.PropertyToID("_BlendParam");
		}
		if(0 > IDShaderPartsColorLU)
		{
			IDShaderPartsColorLU = UnityEngine.Shader.PropertyToID("_PartsColor_LU");
		}
		if(0 > IDShaderPartsColorRU)
		{
			IDShaderPartsColorRU = UnityEngine.Shader.PropertyToID("_PartsColor_RU");
		}
		if(0 > IDShaderPartsColorRD)
		{
			IDShaderPartsColorRD = UnityEngine.Shader.PropertyToID("_PartsColor_RD");
		}
		if(0 > IDShaderPartsColorLD)
		{
			IDShaderPartsColorLD = UnityEngine.Shader.PropertyToID("_PartsColor_LD");
		}
		if(0 > IDShaderPartsColorSkinnedMesh)
		{
			IDShaderPartsColorSkinnedMesh = UnityEngine.Shader.PropertyToID("_PartsColor");
		}
		if(0 > IDShaderPartsColorOpacity)
		{
			IDShaderPartsColorOpacity = UnityEngine.Shader.PropertyToID("_PartsColor_Opacity");
		}

		if(0 > IDShaderVertexOffsetLURU)
		{
			IDShaderVertexOffsetLURU = UnityEngine.Shader.PropertyToID("_VertexOffset_LURU");
		}
		if(0 > IDShaderVertexOffsetRDLD)
		{
			IDShaderVertexOffsetRDLD = UnityEngine.Shader.PropertyToID("_VertexOffset_RDLD");
		}
#else
#endif
	}

#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
	private bool RenderingUpdateNormal(int sortingOrder, int sortingOrderBase)
	{
		if(null == InstanceSpriteRenderer)
		{
			return(false);
		}

		/* Set Priority */
		if(OrderInLayerPrevious != sortingOrder)
		{
			InstanceSpriteRenderer.sortingOrder = sortingOrder;
			OrderInLayerPrevious = sortingOrder;
		}

		/* Set Hide */
		if(0.5f < FlagHide)
		{
			InstanceSpriteRenderer.enabled = false;
		}
		else
		{
			InstanceSpriteRenderer.enabled = true;
		}

		/* Set Material-Properties */
		if(null != PropertyMaterial)
		{
			Vector4 tempVector;

			InstanceSpriteRenderer.GetPropertyBlock(PropertyMaterial);

			/* Set Cell */
			if(CellPrevious != Cell)
			{
				InstanceSpriteRenderer.sprite = Cell;
				if(0 <= IDMaterialMainTexture)
				{
					PropertyMaterial.SetTexture(IDMaterialMainTexture, Cell.texture);
				}

				CellPrevious = Cell;
			}
			if(null != Cell)
			{
				Rect rectangleCell = Cell.rect;
				if(0 <= IDMaterialRectangleCell)
				{
					tempVector.x = rectangleCell.xMin;
					tempVector.y = rectangleCell.yMin;
					tempVector.z = rectangleCell.width;
					tempVector.w = rectangleCell.height;
					PropertyMaterial.SetVector(IDMaterialRectangleCell, tempVector);
				}

				/* Set Cell-Pivot & Local-Scale */
				if(0 <= IDMaterialPivotCell)
				{
					tempVector = Cell.pivot;
//					tempVector.x = pivot.x;
					tempVector.y = rectangleCell.height - tempVector.y;	/* Invert height */
					tempVector.z = ScaleLocal.x;
						tempVector.w = ScaleLocal.y;
					PropertyMaterial.SetVector(IDMaterialPivotCell, tempVector);
				}
			}

			/* Set Parts-Color / Opacity */
			if(0 <= IDShaderBlendParam)
			{
				tempVector.x = PartsColorOperation;
				tempVector.y = RateOpacity;
				tempVector.z =
				tempVector.w = 0.0f;	/* Not Used */
				PropertyMaterial.SetVector(IDShaderBlendParam, tempVector);
			}

			if(	(0 <= IDShaderPartsColorLU)
				&& (0 <= IDShaderPartsColorRU)
				&& (0 <= IDShaderPartsColorRD)
				&& (0 <= IDShaderPartsColorLD)
				&& (0 <= IDShaderPartsColorOpacity)
			)
			{
				PropertyMaterial.SetColor(IDShaderPartsColorLU, PartsColorLU);
				PropertyMaterial.SetColor(IDShaderPartsColorRU, PartsColorRU);
				PropertyMaterial.SetColor(IDShaderPartsColorRD, PartsColorRD);
				PropertyMaterial.SetColor(IDShaderPartsColorLD, PartsColorLD);

				tempVector.x = PartsColorPowerLU;
				tempVector.y = PartsColorPowerRU;
				tempVector.z = PartsColorPowerRD;
				tempVector.w = PartsColorPowerLD;
				PropertyMaterial.SetVector(IDShaderPartsColorOpacity, tempVector);
			}

			/* Set Vertex-Correction */
			if(	(0 <= IDShaderVertexOffsetLURU)
				&& (0 <= IDShaderVertexOffsetRDLD)
			)
			{
				tempVector.x = VertexCorrectionLU.x;
				tempVector.y = VertexCorrectionLU.y;
				tempVector.z = VertexCorrectionRU.x;
				tempVector.w = VertexCorrectionRU.y;
				PropertyMaterial.SetVector(IDShaderVertexOffsetLURU, tempVector);

				tempVector.x = VertexCorrectionRD.x;
				tempVector.y = VertexCorrectionRD.y;
				tempVector.z = VertexCorrectionLD.x;
				tempVector.w = VertexCorrectionLD.y;
				PropertyMaterial.SetVector(IDShaderVertexOffsetRDLD, tempVector);
			}

			InstanceSpriteRenderer.SetPropertyBlock(PropertyMaterial);
		}

		return(true);
	}
#if UNITY_2017_1_OR_NEWER
	private bool RenderingUpdateMask(int sortingOrder, int sortingOrderBase)
	{
		if(null != InstanceSpriteMask)
		{
			return(false);
		}

		/* Set Priority */
		if(OrderInLayerPrevious != sortingOrder)
		{
			InstanceSpriteMask.frontSortingOrder = sortingOrder;
			InstanceSpriteMask.backSortingOrder = sortingOrderBase - PartsRoot.SortingOffsetPartsDraw;
			OrderInLayerPrevious = sortingOrder;
		}

		/* Set Hide */
		if(0.5f < FlagHide)
		{
			InstanceSpriteMask.enabled = false;
		}
		else
		{
			InstanceSpriteMask.enabled = true;
		}

		/* Set Cell */
//		if(null != Cell)
		if(CellPrevious != Cell)
		{
			InstanceSpriteMask.sprite = Cell;
//			Cell = null;
			CellPrevious = Cell;
		}

		/* Set Mask-Power */
		/* MEMO: "Mask Power" is already inverted of band (1.0 - Mask-Power), so it is substituted directly into "Cutoff Alpha". */
		InstanceSpriteMask.alphaCutoff = RateOpacity;

		return(true);
	}
#else
	/* MEMO: Can not use "SpriteMask" in Unity5.6 or earlier.                               */
	/*       (For "Nintendo Switch" for the time being, corresponds to Unity5.6 or earlier) */
#endif
	private bool RenderingUpdateMeshSkinned(int sortingOrder, int sortingOrderBase)
	{
		if(null == InstanceSkinnedMeshRenderer)
		{
			return(false);
		}

		/* Set Priority */
		if(OrderInLayerPrevious != sortingOrder)
		{
			InstanceSkinnedMeshRenderer.sortingOrder = sortingOrder;
			OrderInLayerPrevious = sortingOrder;
		}

		/* Set Cell */
		if(CellMeshPrevious != CellMesh)
		{
			if(null == InstanceCellMesh)
			{
				InstanceCellMesh = new Mesh();
				if(null == InstanceCellMesh)
				{	/* Error */
					return(false);
				}
				InstanceCellMesh.hideFlags = HideFlags.DontSave;
			}

			/* Re-Set Mesh */
			InstanceCellMesh.Clear();
			InstanceCellMesh.name = CellMesh.name;
			InstanceCellMesh.vertices = CellMesh.vertices;
			InstanceCellMesh.uv = CellMesh.uv;
			InstanceCellMesh.triangles = CellMesh.triangles;
			InstanceCellMesh.boneWeights = CellMesh.boneWeights;
			InstanceCellMesh.bindposes = TableMatrixBindPose;

			InstanceSkinnedMeshRenderer.sharedMesh = InstanceCellMesh;

			CellMeshPrevious = CellMesh;
		}

		/* Set Hide */
		if(0.5f < FlagHide)
		{
			InstanceSkinnedMeshRenderer.enabled = false;
		}
		else
		{
			InstanceSkinnedMeshRenderer.enabled = true;
		}

		/* Set Material-Properties */
		if(null != PropertyMaterial)
		{
			Vector4 tempVector;

			InstanceSkinnedMeshRenderer.GetPropertyBlock(PropertyMaterial);

			/* Set Texture */
			if(TextureMeshPrevious != TextureMesh)
			{
				if(0 <= IDMaterialMainTexture)
				{
					PropertyMaterial.SetTexture(IDMaterialMainTexture, TextureMesh);
				}

				TextureMeshPrevious = TextureMesh;
			}

			/* Set Parts-Color */
			if(0 <= IDShaderBlendParam)
			{
				/* MEMO: When "SkinnedMesh", PartsColor-Power is stored in "BlendParam.z". */
				tempVector.x = PartsColorOperation;
				tempVector.y = RateOpacity;
				tempVector.z = PartsColorPowerLU;
				tempVector.w = 0.0f;	/* Not Used */
				PropertyMaterial.SetVector(IDShaderBlendParam, tempVector);
			}

			if(0 <= IDShaderPartsColorSkinnedMesh)
			{
				PropertyMaterial.SetColor(IDShaderPartsColorSkinnedMesh, PartsColorLU);
			}

			/* MEMO: "Mesh" parts is not supported "Vertex-Deformation(Correction)". */

			InstanceSkinnedMeshRenderer.SetPropertyBlock(PropertyMaterial);
		}

		return(true);
	}
	private bool RenderingUpdateMeshRigid(int sortingOrder, int sortingOrderBase)
	{	/* MEMO: When "Mesh" part is with no bones assigned, this function is called. */
		if(null == InstanceMeshRenderer)
		{
			return(false);
		}

		/* Set Priority */
		if(OrderInLayerPrevious != sortingOrder)
		{
			InstanceMeshRenderer.sortingOrder = sortingOrder;
			OrderInLayerPrevious = sortingOrder;
		}

		/* Set Cell */
		if(CellMeshPrevious != CellMesh)
		{
			if(null == InstanceCellMesh)
			{
				InstanceCellMesh = new Mesh();
				if(null == InstanceCellMesh)
				{	/* Error */
					return(false);
				}
				InstanceCellMesh.hideFlags = HideFlags.DontSave;
			}

			/* Re-Set Mesh */
			InstanceCellMesh.Clear();
			InstanceCellMesh.name = CellMesh.name;
			InstanceCellMesh.vertices = CellMesh.vertices;
			InstanceCellMesh.uv = CellMesh.uv;
			InstanceCellMesh.triangles = CellMesh.triangles;
//			InstanceCellMesh.boneWeights = 
//			InstanceCellMesh.bindposes = 

			InstanceMeshFilter.sharedMesh = InstanceCellMesh;

			CellMeshPrevious = CellMesh;
		}

		/* Set Hide */
		if(0.5f < FlagHide)
		{
			InstanceMeshRenderer.enabled = false;
		}
		else
		{
			InstanceMeshRenderer.enabled = true;
		}

		/* Set Material-Properties */
		if(null != PropertyMaterial)
		{
			Vector4 tempVector;

			InstanceMeshRenderer.GetPropertyBlock(PropertyMaterial);

			/* Set Texture */
			if(TextureMeshPrevious != TextureMesh)
			{
				if(0 <= IDMaterialMainTexture)
				{
					PropertyMaterial.SetTexture(IDMaterialMainTexture, TextureMesh);
				}

				TextureMeshPrevious = TextureMesh;
			}

			/* Set Parts-Color */
			if(0 <= IDShaderBlendParam)
			{
				/* MEMO: When "SkinnedMesh", PartsColor-Power is stored in "BlendParam.z". */
				tempVector.x = PartsColorOperation;
				tempVector.y = RateOpacity;
				tempVector.z = PartsColorPowerLU;
				tempVector.w = 0.0f;	/* Not Used */
				PropertyMaterial.SetVector(IDShaderBlendParam, tempVector);
			}

			if(0 <= IDShaderPartsColorSkinnedMesh)
			{
				PropertyMaterial.SetColor(IDShaderPartsColorSkinnedMesh, PartsColorLU);
			}

			/* MEMO: "Mesh" parts is not supported "Vertex-Deformation(Correction)". */

			InstanceMeshRenderer.SetPropertyBlock(PropertyMaterial);
		}

		return(true);
	}
#else
#endif
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	#endregion Enums & Constants

	/* ----------------------------------------------- Functions */
	#region Functions
	#endregion Functions

	/* ----------------------------------------------- Delegates */
	#region Delegates
#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
	protected delegate bool FunctionUpdate(int sortingOrder, int sortingOrderBase);
#else
#endif
	#endregion Delegates
}
