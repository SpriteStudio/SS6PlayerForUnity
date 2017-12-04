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
	public Sprite MaskSprite;

	private SpriteRenderer InstanceSpriteRenderer = null;
	private SpriteMask InstanceSpriteMask = null;
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
		if(null != InstanceSpriteRenderer)
		{
			InstanceSpriteRenderer.sortingOrder = (int)OrderInLayer;
		}
		if(null != InstanceSpriteMask)
		{
			InstanceSpriteMask.frontSortingOrder = (int)OrderInLayer;
			InstanceSpriteMask.sprite = MaskSprite;
		}
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	#endregion Enums & Constants
}
