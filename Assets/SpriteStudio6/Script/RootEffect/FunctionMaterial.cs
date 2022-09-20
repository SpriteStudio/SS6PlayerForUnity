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
	//! Activate (Boot up) Material-Cache used for drawing
	/*!
	@param
		(none)

	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Activates the Material-Cache for use in drawing.<br>
	This Material-Cache is valid for instance (1 animation-object) of this class.<br>
	(Originally, have similar cache in DataProject, and use it.)<br>
	<br>
	When you want to use shaders or materials that are valid for animation object
		only, need to execute this function first.<br>
	(In particular, when using function "ShaderSetStandard...", this function
		must be called before that.)<br>
	<br>
	Invariably, need to call this function after the execution of Start() has been completed.<br>
	*/
	public bool CacheBootUpMaterial()
	{
		/* Boot up Material-Cache */
		int countTexture = CountGetCellMap(false);
		if(false == base.CacheBootUpMaterial(countTexture, countTexture * (int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR))
		{
			return(false);
		}

		/* MEMO: "Effect" animation has no child-Animation-Objects. */

		return(true);
	}

	/* ********************************************************* */
	//! Terminate (Shut down) Material-Cache used for drawing
	/*!
	@param
		(none)

	@retval	Return-Value
		(none)

	Terminate the Material-Cache for use in drawing.<br>
	After calling this function, Animation-Object's own material are released
		and Material-Cache in DataProject is used.<br>
	*/
	public new void CacheShutDownMaterial()
	{
		if((null == TableTexture) && (null == CacheMaterial))
		{
			return;
		}

		/* MEMO: "Effect" animation has no child-Animation-Objects. */

		/* Shut down Materrial-Cache */
		base.CacheShutDownMaterial();
	}

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
		Shader's name in animation-data<br>
		null == Standard-Shader's name
	@param	flagCreateNew
		true == If not exist, create.
		false == If not exist, return null.
	@param	shader
		Shader applied<br>
		null == Default(Standard) shader
	@param	functionMaterialSetUp
		Function called to set parameters when creating new material<br>
		null == Default Function<br>
		Default: null
	@retval	Return-Value
		Instance of Material
		null == Not exist or Error

	Search material with the specified content among materials currently held.<br>
	Materials (held by Animation-Object) are affected by currently playback state
		since materials are dynamically generated.<br>
	Materials will not be created until Play-Cursor reachs actually using material,
		and once it is created, those will be retained until Animation-Object is destroyed.<br>
	<br>
	When "flagCreateNew" is false, "shader" and "functionMaterialSetUp" can be null. (be ignored.)<br>
	<br>
	Specify to "functionMaterialSetUp" must must be following type function.<br>
	UnityEngine.Material FunctionMaterialSetUp(<br>
		UnityEngine.Material material,<br>
		int(Library_SpriteStudio6.KindOperationBlendEffect) operationBlend,<br>
		Library_SpriteStudio6.KindMasking masking,<br>
		bool flagZWrite<br>
	);<br>
	[Arguments]<br>
		material: Material to set parameters (blending method, etc.)<br>
		operationBlend: Blend type given to this function. (Casted to int)<br>
		masking: Masking type given to this function.<br>
		flagZWrite: true==Write to Z-Buffer. / false==No-touching Z-Buffer.<br>
	[Return]<br>
		On success, instance of the material (same as argument "material")<br>
		On failure, null.
	*/
	public UnityEngine.Material MaterialGet(	int indexCellMap,
												Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
												Library_SpriteStudio6.KindMasking masking,
												string nameShader,
												bool flagCreateNew,
												UnityEngine.Shader shader=null,
												Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp=null
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
		Script_SpriteStudio6_DataProject dataProject = DataEffect.DataProject;
		if(null == dataProject)
		{
			return(null);
		}

		/* Check Shader Override */
		bool flagIsLocalShader = false;
		if((null != CacheMaterial) && (true == CacheMaterial.StatusIsBootedUp))
		{	/* Material-Cache is running */
			if(null == shader)
			{	/* Standard-Shader */
				nameShader = null;	/* Standard-Shader */

				/* MEMO: "Effect" animation does not have masking capability. */

				shader = CacheMaterial.ShaderStandardEffect;
				if(null == shader)
				{
					flagIsLocalShader = false;
					shader = dataProject.CacheMaterial.ShaderStandardEffect;
				}
				else
				{
					flagIsLocalShader = true;
				}
			}
		}

		/* Check Texture Override */
		bool flagIsLocalTexture = false;
		UnityEngine.Texture[] tableTexture = null;
		if(null != TableTexture)
		{	/* Texture-Override is running */
			if(TableTexture.Length <= indexCellMap)
			{
				return(null);
			}

			if(null == TableTexture[indexCellMap])
			{
				tableTexture = dataProject.TableTexture;
				flagIsLocalTexture = false;
			}
			else
			{
				tableTexture = TableTexture;
				flagIsLocalTexture = true;
			}
		}
		else
		{
			tableTexture = dataProject.TableTexture;
//			flagIsLocalTexture = false;
		}

		/* Get Material */
		/* MEMO: Whether using Local(Animation object's)-cache or Global(DataProject's)-cache is determined */
		/*         by Shader or Texture are set to local. Local-Cache has priority.                         */
		if(true == (flagIsLocalShader | flagIsLocalTexture))
		{	/* Local(Animation-Object) */
			return(CacheMaterial.MaterialGetEffect(	indexCellMap,
													operationBlend,
													masking,
													nameShader,
													shader,
													functionMaterialSetUp,
													tableTexture,
													flagCreateNew
												)
				);
		}
		/* MEMO: Following is for Global-Cache. */

		return(dataProject.MaterialGetEffect(	indexCellMap,
												operationBlend,
												masking,
												nameShader,
												flagCreateNew,
												shader,
												functionMaterialSetUp
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
		{	/* Local-Cache (Animation-Object data) */
			return(	CacheMaterial.MaterialReplaceEffect(	indexCellMap,
															operationBlend,
															masking,
															nameShader,
															material
														)
				);
		}
		/* MEMO: Following is for Global-Cache. */

		/* Replaca in Global(Project data) Cache */
		if(null == material)
		{
			/* MEMO: Prohibited to remove material that Project has. */
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

	/* ********************************************************* */
	//! Replace Standard-(Pixel)Shader for Effect
	/*!
	@param	shader
		Shader to be set<br>
		null == Reset to initial
	@param	flagReplaceMaterialCache
		true == Cached materials (that using  standard-shaders) are replaced new shader.
		false == Cached materials are not changing.
	@retval	Return-Value
		Previous shader

	MEMO: Since Animation-Shader is not relevant for "Effect" animation, function is not supported.
	*/
//	internal UnityEngine.Shader ShaderSetStandardPixel(UnityEngine.Shader shader, bool flagReplaceMaterialCache)

	/* ********************************************************* */
	//! Replace Standard-(Pixel)Shader for Effect
	/*!
	@param	shader
		Shader to be set<br>
		null == Reset to initial
	@param	functionMaterialSetUp
		Function called to set parameters when creating new material<br>
		null == Default Function<br>
	@param	flagReplaceMaterialCache
		true == Cached materials (that using  standard-shaders) are replaced new shader.<br>
		false == Cached materials are not changing.
	@param	flagGlobal
		true == Replace Standard-Shader that "Global"(DataProject) has.<br>
		false == Replace Standard-Shader that "Local"(only this Animation-Object) has.<br>
		Default: false
	@retval	Return-Value
		Previous shader

	Changes the "Standard-Shader" used in "Effect"s.<br>
	Always "Standard-Shader" (for Effect) is used when Play "Effect"-animation(SSEEs).<br>
	<br>
	Only affects "Effect" objects controlled by Animation-Object instances of this class.<br>
	(Affects child "Effect" Animation-Objects only.)<br>
	<br>
	Normally, should not have a chance to use this function.<br>
	<br>
	Specify to "functionMaterialSetUp" must must be following type function.<br>
	UnityEngine.Material FunctionMaterialSetUpEffect(<br>
		UnityEngine.Material material,<br>
		int(Library_SpriteStudio6.KindOperationBlendEffect) operationBlend,<br>
		Library_SpriteStudio6.KindMasking masking,<br>
		bool flagZWrite<br>
	);<br>
	[Arguments]<br>
		material: Material to set parameters (blending method, etc.)<br>
		operationBlend: Blend type given to this function. (Casted to int)<br>
		masking: Masking type given to this function.<br>
		flagZWrite: true==Write to Z-Buffer. / false==No-touching Z-Buffer.<br>
	[Return]<br>
		On success, instance of the material (same as argument "material")<br>
		On failure, null.
	*/
	internal UnityEngine.Shader ShaderSetStandardEffect(	UnityEngine.Shader shader,
															Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp,
															bool flagReplaceMaterialCache,
															bool flagGlobal=false
													)
	{
		if(false == flagGlobal)
		{	/* Local (Animation-Object) */
			if(null == CacheMaterial)
			{
				return(null);
			}

			/* MEMO: In Material-Cache dedicated to Animation-Objects, initial value of Standard-Shader is null. */
			/*       When Standard-Shader in Material-Cache is null, DataProject's Standard-Shader is used.      */
			Status |= FlagBitStatus.CHANGE_CACHEMATERIAL;
			return(CacheMaterial.ShaderReplaceStandardPixelEffect(	shader,
																	null,
																	functionMaterialSetUp,
																	null,
																	flagReplaceMaterialCache
															)
				);
		}

		if(null == DataEffect)
		{
			return(null);
		}
		Script_SpriteStudio6_DataProject dataProject = DataEffect.DataProject;
		if(null == dataProject)
		{
			return(null);
		}

		return(dataProject.ShaderSetStandardEffect(shader, functionMaterialSetUp, flagReplaceMaterialCache));
	}

	/* ********************************************************* */
	//! Replace Standard-(Stencil)Shader for Effect
	/*!
	@param	shader
		Shader to be set<br>
		null == Reset to initial
	@param	flagReplaceMaterialCache
		true == Cached materials (that using  standard-shaders) are replaced new shader.
		false == Cached materials are not changing.
	@retval	Return-Value
		Previous shader

	MEMO: Since Stencil-Shader is not relevant for "Effect" animation, function is not supported.
	*/
//	internal UnityEngine.Shader ShaderSetStandardStencil(UnityEngine.Shader shader, bool flagReplaceMaterialCache)

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
