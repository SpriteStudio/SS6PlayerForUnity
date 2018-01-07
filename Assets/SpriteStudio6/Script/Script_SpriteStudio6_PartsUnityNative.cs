/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
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
	/* MEMO: Can not control except float from "AnimationClip". */
	public float OrderInLayer;

	/* MEMO: When change cell from script, change this valiable not "SpriteRenderer.sprite (or SpriteMask.sprite)". */
	public Sprite Cell;

	/* MEMO: When change cell from script, change this valiable not "SkinnedMeshRenderer.sharedMesh". */
	public Mesh CellMesh;
	public Texture2D TextureMesh;

	/* MEMO: Do not change these valiables. (Set only from importer) */
	public Transform[] TableTransformBone;

	public SpriteRenderer InstanceSpriteRenderer;
	public SpriteMask InstanceSpriteMask;
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
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
//	void Awake()
//	{
//	}

	void Start()
	{
//		InstanceSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		if(null != InstanceSpriteRenderer)
		{
			goto Start_End;
		}

//		InstanceSpriteMask = gameObject.GetComponent<SpriteMask>();
		if(null != InstanceSpriteMask)
		{
			goto Start_End;
		}

//		InstanceSkinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
		if(null != InstanceSkinnedMeshRenderer)
		{
			/* Create Bind-Pose */
			Matrix4x4 matrixLocalToWorld = transform.localToWorldMatrix;
			if(null != TableTransformBone)
			{
				int countTransformBone = TableTransformBone.Length;
				TableMatrixBindPose = new Matrix4x4[countTransformBone];
				if(null == TableMatrixBindPose)
				{
					goto Start_ErrorEnd;
				}

				for(int i=0; i<countTransformBone; i++)
				{
					TableMatrixBindPose[i] = TableTransformBone[i].worldToLocalMatrix * matrixLocalToWorld;
				}
			}

			InstanceSkinnedMeshRenderer.bones = TableTransformBone;

 			goto Start_End;
		}

		if(null != InstanceMeshRenderer)
		{
 			goto Start_End;
		}

	Start_ErrorEnd:;
		return;

	Start_End:;
		return;
	}

//	void Update()
//	{
//	}

	void LateUpdate()
	{
		if(null == PropertyMaterial)
		{
			PropertyMaterial = new MaterialPropertyBlock();

			IDMaterialMainTexture = Shader.PropertyToID("_MainTex");
			IDMaterialRectangleCell = Shader.PropertyToID("_CellRectangle");
			IDMaterialPivotCell = Shader.PropertyToID("_CellPivot_LocalScale");
		}

		/* MEMO: "SpriteRenderer", "SpriteMask" and "SkinnedMeshRenderer" do not coexist. */

		/* Sprite (SpriteRenderer) */
		if(null != InstanceSpriteRenderer)
		{
			if(OrderInLayerPrevious != OrderInLayer)
			{
				InstanceSpriteRenderer.sortingOrder = (int)OrderInLayer;
				OrderInLayerPrevious = OrderInLayer;
			}

			if(CellPrevious != Cell)
			{
				InstanceSpriteRenderer.sprite = Cell;

				if(null != PropertyMaterial)
				{
					InstanceSpriteRenderer.GetPropertyBlock(PropertyMaterial);

					Vector4 temp;
					Rect rectangleCell = Cell.rect;
					temp.x = rectangleCell.xMin;
					temp.y = rectangleCell.yMin;
					temp.z = rectangleCell.width;
					temp.w = rectangleCell.height;
					PropertyMaterial.SetVector(IDMaterialRectangleCell, temp);

					/* MEMO: Since "LocalScale" is stored together in "_CellPivot_LocalScale", overwrite value set by animation. */
					temp = PropertyMaterial.GetVector(IDMaterialPivotCell);
					Vector2 pivot = Cell.pivot;
					temp.x = pivot.x;
					temp.y = rectangleCell.height - pivot.y;
					PropertyMaterial.SetVector(IDMaterialPivotCell, temp);

					InstanceSpriteRenderer.SetPropertyBlock(PropertyMaterial);
				}
				CellPrevious = Cell;
			}

			return;
		}

		/* Mask (SpriteMask) */
		if(null != InstanceSpriteMask)
		{
			if(OrderInLayerPrevious != OrderInLayer)
			{
				InstanceSpriteMask.frontSortingOrder = (int)OrderInLayer;
				OrderInLayerPrevious = OrderInLayer;
			}

			if(null != Cell)
			{
				InstanceSpriteMask.sprite = Cell;
				Cell = null;
			}

			return;
		}

		/* Mesh (SkinnedMeshRenderer) */
		if(null != InstanceSkinnedMeshRenderer)
		{
			if(OrderInLayerPrevious != OrderInLayer)
			{
				InstanceSkinnedMeshRenderer.sortingOrder = (int)OrderInLayer;
				OrderInLayerPrevious = OrderInLayer;
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
			if(OrderInLayerPrevious != OrderInLayer)
			{
				InstanceMeshRenderer.sortingOrder = (int)OrderInLayer;
				OrderInLayerPrevious = OrderInLayer;
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
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	#endregion Functions
}
