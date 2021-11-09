/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Script_SpriteStudio6_RootEffect
{
	/* ----------------------------------------------- Functions */
	#region Functions
	/* ********************************************************* */
	//! Get Material
	/*!
	@param	indexCellMap
		CellMap's index
	@param	operationBlend
		Blend Operation for the target
	@param	masking
		masking for the target
	@param	nameShadcer
		Shader's name in animation-data
		null == Default(Standard) shader's name
	@param	shader
		Shader applied<br>
		null == Default(Standard) shader
	@param	flagCreateNew
		true == If not exist, create.
		false == If not exist, return null.
	@retval	Return-Value
		Instance of Material
		null == Not exist or Error

	Search material with the specified content among materials currently held.<br>
	Materials (held by Animation-Object) are affected by currently playback state
		since materials are dynamically generated.<br>
	Materials will not be created until Play-Cursor reachs actually using material,
		and once it is created, those will be retained until Animation-Object is destroyed.<br>
	*/
	public UnityEngine.Material MaterialGet(	int indexCellMap,
												Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
												Library_SpriteStudio6.KindMasking masking,
												string nameShader,
												UnityEngine.Shader shader,
												bool flagCreateNew
										)
	{
		if(null == DataEffect)
		{
			return(null);
		}
		if(0 > indexCellMap)
		{
			return(null);
		}

		if(true == TextureCheckOverride(indexCellMap))
		{	/* Texture Overrided */
			return(	CacheMaterial.MaterialGetEffect(	indexCellMap,
														operationBlend,
														masking,
														nameShader,
														shader,
														TableTexture,
														flagCreateNew
													)
				);
		}

		if(TableCellMap.Length <= indexCellMap)
		{
			return(null);
		}

		Script_SpriteStudio6_DataProject dataProject = DataEffect.DataProject;
		if(null == dataProject)
		{
			return(null);
		}
		return(	dataProject.MaterialGetEffect(	indexCellMap,
												operationBlend,
												masking,
												nameShader,
												shader,
												flagCreateNew
										)
			);
	}

	/* ********************************************************* */
	//! Replace Material
	/*!
	@param	indexCellMap
		CellMap's index
	@param	operationBlend
		Blend Operation for the target
	@param	masking
		masking for the target
	@param	nameShadcer
		Shader's name in animation-data
		null == Default(Standard) shader's name
	@param	material
		New material<br>
		null == Remove material (however can't be removed that Project has)
	@param	flagGlobal
		true == Replace material that Project(DataProject) has.<br>
		false == Replace material that only this Animation-Object has.
	@retval	Return-Value
		Originally set material<br>
		null == Not exist or Error

	Search and replace material with the specified content among materials currently held.<br>
	Material to be replaced is identified by "indexCellMap", "operationBlend", "masking", and "shader".<br>
	If the material cannot be found, this function will return error.<br>
	<br>
	Note that changes the material to "material" for drawing, which is determined by "indexCellMap", "operationBlend", "masking", and "nameShader".<br>
	"indexCellMap", "operationBlend", "masking", and "nameShader" are not parameters of the material to be changed, but keys to identify the material used in the original animation.<br>
	*/
	public UnityEngine.Material MaterialReplace(	int indexCellMap,
													Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
													Library_SpriteStudio6.KindMasking masking,
													string nameShader,
													UnityEngine.Material material,
													bool flagGlobal = false
											)
	{
		if(null == DataEffect)
		{
			return(null);
		}
		if(0 > indexCellMap)
		{
			return(null);
		}

		if(false == flagGlobal)
		{
			if(true == TextureCheckOverride(indexCellMap))
			{	/* Texture Overrided */
				return(	CacheMaterial.MaterialReplaceEffect(	indexCellMap,
																operationBlend,
																masking,
																nameShader,
																material
															)
					);
			}

			return(null);
		}

		if(null == material)
		{
			/* MEMO: Prohibited to remove material that Project has. */
			return(null);
		}
		if(TableCellMap.Length <= indexCellMap)
		{
			return(null);
		}
		Script_SpriteStudio6_DataProject dataProject = DataEffect.DataProject;
		if(null == dataProject)
		{
			return(null);
		}

		/* MEMO: The texture used in calculating hashcode to identify  material must be it stored in the project. */
		return(	dataProject.MaterialReplaceEffect(	indexCellMap,
													operationBlend,
													masking,
													nameShader,
													material
											)
			);
	}

	/* Obsolete *//* public UnityEngine.Material[] TableGetMaterial(bool flagInUse=true) */
	/* Obsolete *//* public bool TableSetMaterial(UnityEngine.Material[] tableMaterial) */
	/* Obsolete *//* public int CountGetTableMaterial(bool flagInUse=true) */
	/* Obsolete *//* public UnityEngine.Material[] TableCopyMaterialShallow(bool flagInUse=true) */
	/* Obsolete *//* public UnityEngine.Material[] TableCopyMaterialDeep(bool flagInUse=true) */
	/* Obsolete *//* public int CountGetTextureTableMaterial(bool flagInUse=true) */
	#endregion Functions

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	/* Obsolete *//* public static partial class Material */
	/* Obsolete *//* { */
		/* Obsolete *//* public static int CountGetTable(int countCellMap) */
		/* Obsolete *//* public static int IndexGetTable(int indexCellMap, Library_SpriteStudio6.KindOperationBlendEffect operationBlend, Library_SpriteStudio6.KindMasking masking) */
		/* Obsolete *//* public static int CountGetTexture(UnityEngine.Material[] tableMaterial) */
		/* Obsolete *//* public static bool TextureSet(UnityEngine.Material[] tableMaterial, int indexCellMap, Texture2D texture, bool flagMaterialNew) */
	/* Obsolete *//* } */
	#endregion Classes, Structs & Interfaces
}
