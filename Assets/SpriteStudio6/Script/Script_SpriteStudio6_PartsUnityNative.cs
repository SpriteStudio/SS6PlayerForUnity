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
public partial class Script_SpriteStudio6_PartsUnityNative :MonoBehaviour
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	/* MEMO: Can not control except float from "AnimationClip". */
	public float OrderInLayer;
	/* MEMO: When changing a cell from a script, change this valiable not "SpriteRenderer.sprite (or SpriteMask.sprite)". */
	public Sprite Cell;

	private SpriteRenderer InstanceSpriteRenderer = null;
	private SpriteMask InstanceSpriteMask = null;
	private MaterialPropertyBlock PropertyMaterial = null;
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
		InstanceSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		InstanceSpriteMask = gameObject.GetComponent<SpriteMask>();
	}

//	void Update()
//	{
//	}

	void LateUpdate()
	{
		if(null == PropertyMaterial)
		{
			PropertyMaterial = new MaterialPropertyBlock();

			IDMaterialRectangleCell =  Shader.PropertyToID("_CellRectangle");
			IDMaterialPivotCell =  Shader.PropertyToID("_CellPivot_LocalScale");
		}

		if(null != InstanceSpriteRenderer)
		{
			if(OrderInlayerDefault < OrderInLayer)
			{
				InstanceSpriteRenderer.sortingOrder = (int)OrderInLayer;
				OrderInLayer = OrderInlayerDefault;
			}

			if(null != Cell)
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
				Cell = null;
			}
			return;
		}

		if(null != InstanceSpriteMask)
		{
			if(OrderInlayerDefault < OrderInLayer)
			{
				InstanceSpriteMask.frontSortingOrder = (int)OrderInLayer;
				OrderInLayer = OrderInlayerDefault;
			}

			if(null != Cell)
			{
				InstanceSpriteMask.sprite = Cell;
				Cell = null;
			}
			return;
		}
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	private const float OrderInlayerDefault = -100000.0f;
	#endregion Enums & Constants
}
