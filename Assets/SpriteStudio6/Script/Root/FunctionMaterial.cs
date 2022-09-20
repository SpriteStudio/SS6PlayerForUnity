/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Script_SpriteStudio6_Root
{
	/* ----------------------------------------------- Functions */
	#region Functions
	/* ********************************************************* */
	//! Activate (Boot up) Material-Cache used for drawing
	/*!
	@param	flagInvolveChildren
		true == Children are set same state.<br>
		false == only oneself.<br>
		Default: true

	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Activates the Material-Cache and Textreu-Table for use in drawing.<br>
	This Material-Cache is valid for instance (1 animation-object) of this class.<br>
	(Originally, have similar cache in DataProject, and use it.)<br>
	<br>
	When you want to use shaders or materials that are valid for animation object
		only, need to execute this function first.<br>
	(In particular, when using function "ShaderSetStandard...", this function
		must be called before that.)<br>
	<br>
	When "flagInvolveChildren" is set to true, child Animation-objects ("Instance"-Aniamtions
		and "Effect"-Animations under the control of this Animation-Object) will share
		same Material-Cache and Texture-Table.<br>
	When set false, child Animation-objects' setting are not changed and activate only
		this Animation-Object's Material-Cache and Texture-Table.<br>
	<br>
	Invariably, need to call this function after the execution of Start() has been completed.<br>
	Otherwise, child animations will not be set correctly when "flagInvolveChildren=true".<br>
	*/
	public bool CacheBootUpMaterial(bool flagInvolveChildren=true)
	{
		/* Boot up Material-Cache */
		int countTexture = CountGetCellMap(false);
		if(false == base.CacheBootUpMaterial(countTexture, countTexture * (int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR))
		{
			return(false);
		}

		/* Boot up Children's Materrial-Cache */
		if(true == flagInvolveChildren)
		{
			CacheBootUpMaterialChild(CacheMaterial, TableTexture, true);
		}

		return(true);
	}
	private void CacheBootUpMaterialChild(Library_SpriteStudio6.Control.CacheMaterial cacheMaterial, UnityEngine.Texture[] tableTexture, bool flagIsRoot)
	{
		/* MEMO: When cache is existing, Not overwrite */
		if((null == TableTexture) && (null == CacheMaterial))
		{
			if(false == flagIsRoot)
			{
				TableTexture = tableTexture;
				CacheMaterial = cacheMaterial;
			}
		}

		int countParts = CountGetParts();
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
						/* MEMO: Even if the part exists, controlled Animation-Object may not exist. */
						Script_SpriteStudio6_Root instanceRootChild = InstanceGet(i);
						if(null != instanceRootChild)
						{
							instanceRootChild.CacheBootUpMaterialChild(cacheMaterial, tableTexture, false);
						}
					}
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
					{
						/* MEMO: Effect has no child Animation-Object. */
						/* MEMO: Even if the part exists, controlled Animation-Object may not exist. */
 						Script_SpriteStudio6_RootEffect instanceRootChild = EffectGet(i);
						if(null != instanceRootChild)
						{
							if((null == instanceRootChild.TableTexture) && (null == instanceRootChild.CacheMaterial))
							{
								instanceRootChild.TableTexture = tableTexture;
								instanceRootChild.CacheMaterial = cacheMaterial;
							}
						}
					}
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MESH:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.TRANSFORM_CONSTRAINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CAMERA:
					break;
			}
		}
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
	<br>
	Call this function from same (this class)instance of "CacheBootUpMaterial" function calling.<br>
	If Material-Cache(and Texture-Table) is shared with child Animation-Objects,
		will be terminated at the same time.<br>
	*/
	public new void CacheShutDownMaterial()
	{
		if((null == TableTexture) && (null == CacheMaterial))
		{
			return;
		}

		/* Shut down Children's Materrial-Cache */
		CacheShutDownMaterial(CacheMaterial, TableTexture, true);

		/* Shut down Materrial-Cache */
		base.CacheShutDownMaterial();
	}
	private void CacheShutDownMaterial(Library_SpriteStudio6.Control.CacheMaterial cacheMaterial, UnityEngine.Texture[] tableTexture, bool flagIsRoot)
	{
		/* MEMO: When cache is existing, Not overwrite */
		if((tableTexture == TableTexture) && (cacheMaterial == CacheMaterial))
		{
			if(false == flagIsRoot)
			{
				TableTexture = null;
				CacheMaterial = null;

				Status |= FlagBitStatus.CHANGE_CACHEMATERIAL;
			}
		}

		int countParts = CountGetParts();
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
						Script_SpriteStudio6_Root instanceRootChild = InstanceGet(i);
						instanceRootChild.CacheShutDownMaterial(cacheMaterial, tableTexture, false);
					}
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
					{
						/* MEMO: Effect has no child Animation-Object. */
						Script_SpriteStudio6_RootEffect instanceRootChild = EffectGet(i);
						if((tableTexture == instanceRootChild.TableTexture) && (cacheMaterial == instanceRootChild.CacheMaterial))
						{
							instanceRootChild.TableTexture = null;
							instanceRootChild.CacheMaterial = null;
						}
					}
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MESH:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.TRANSFORM_CONSTRAINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CAMERA:
					break;
			}
		}
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
	@param	nameShader
		Shader's name in animation-data<br>
		null == Standard-shader's name
	@param	flagCreateNew
		true == If not exist, create.<br>
		false == If not exist, return null.
	@param	shader
		Shader applied<br>
		null == Default(Standard) shader
	@param	functionMaterialSetUp
		Function called to set parameters when creating new material<br>
		null == Default Function<br>
		Default: null
	@retval	Return-Value
		Instance of Material<br>
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
		int(Library_SpriteStudio6.KindOperation) operationBlend,<br>
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
												Library_SpriteStudio6.KindOperationBlend operationBlend,
												Library_SpriteStudio6.KindMasking masking,
												string nameShader,
												bool flagCreateNew,
												UnityEngine.Shader shader=null,
												Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp=null
										)
	{
		if(null == DataAnimation)
		{
			return(null);
		}
		if(0 > indexCellMap)
		{
			return(null);
		}
		Script_SpriteStudio6_DataProject dataProject = DataAnimation.DataProject;
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

				if(Library_SpriteStudio6.KindOperationBlend.MIX > operationBlend)
				{	/* Stencil */
					shader = CacheMaterial.ShaderStandardStencil;
					if(null == shader)
					{
						flagIsLocalShader = false;
						shader = dataProject.CacheMaterial.ShaderStandardStencil;
					}
					else
					{
						flagIsLocalShader = true;
					}
				}
				else
				{	/* Pixel */
					shader = CacheMaterial.ShaderStandardAnimation;
					if(null == shader)
					{
						flagIsLocalShader = false;
						shader = dataProject.CacheMaterial.ShaderStandardAnimation;
					}
					else
					{
						flagIsLocalShader = true;
					}
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
			return(CacheMaterial.MaterialGetAnimation(	indexCellMap,
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

		return(dataProject.MaterialGetAnimation(	indexCellMap,
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
	@param	nameShader
		Shader's name in animation-data<br>
		null == Standard-shader's name
	@param	material
		New material<br>
		null == Remove material (however can't be removed that Project has)
	@param	flagGlobal
		true == Replace material that "Global"(DataProject) has.<br>
		false == Replace material that "Local"(only this Animation-Object) has.<br>
		Default: false
	@retval	Return-Value
		Originally set material<br>
		null == Not exist or Error

	Search and replace material with the specified content among materials currently held.<br>
	Material to be replaced is identified by "indexCellMap", "operationBlend", "masking", and "shader".<br>
	If the material cannot be found, this function will return error.<br>
	<br>
	Note that changes the material to "material" for drawing, which is determined by 
		indexCellMap", "operationBlend", "masking", and "nameShader".<br>
	"indexCellMap", "operationBlend", "masking", and "nameShader" are not parameters of the material
		to be changed, but keys to identify the material used in the original animation.<br>
	*/
	public UnityEngine.Material MaterialReplaceAnimation(	int indexCellMap,
															Library_SpriteStudio6.KindOperationBlend operationBlend,
															Library_SpriteStudio6.KindMasking masking,
															string nameShader,
															UnityEngine.Material material,
															bool flagGlobal = false
													)
	{
		if(null == DataAnimation)
		{
			return(null);
		}
		if(0 > indexCellMap)
		{
			return(null);
		}

		if(false == flagGlobal)
		{	/* Local-Cache (Animation-Object data) */
			return(	CacheMaterial.MaterialReplaceAnimation(	indexCellMap,
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
		Script_SpriteStudio6_DataProject dataProject = DataAnimation.DataProject;
		if(null == dataProject)
		{
			return(null);
		}

		/* MEMO: The texture used in calculating hashcode to identify  material must be it stored in the project. */
		return(	dataProject.MaterialReplaceAnimation(	indexCellMap,
														operationBlend,
														masking,
														nameShader,
														material
												)
			);
	}

	/* ********************************************************* */
	//! Replace Standard-(Pixel)Shader for Animation
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

	Changes the "Standard-Shader" (for Animation) used in animations.<br>
	Always "Standard-Shader" is used when attribute "Shader" is not used in animations(SSAEs).<br>
	<br>
	Only affects Animation-Objects controlled by instances of this class.<br>
	<br>
	"functionMaterialSetUp" is a material initialization function that is called when create material using "shader".
		(Also used when replace material in cache in this function)<br>
	<br>
	specification of "functionMaterialSetUp" is the same as argument of the same name in "MaterialGet".<br>
	<br>
	This process may affect performance adversely as materials will be rebuilt and re-searched.<br>
	Therefore, not recommended to call this function even if not necessary.
		(e.g., Calling this function in every loop process.)<br>
	*/
	public UnityEngine.Shader ShaderSetStandardAnimation(	UnityEngine.Shader shader,
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
			return(CacheMaterial.ShaderReplaceStandardPixelAnimation(	shader,
																		null,
																		functionMaterialSetUp,
																		null,
																		flagReplaceMaterialCache
																)
				);
		}

		if(null == DataAnimation)
		{
			return(null);
		}
		Script_SpriteStudio6_DataProject dataProject = DataAnimation.DataProject;
		if(null == dataProject)
		{
			return(null);
		}

		return(dataProject.ShaderSetStandardAnimation(shader, functionMaterialSetUp, flagReplaceMaterialCache));
	}

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

	Changes the "Standard-Shader" (for Effect) used in animations.<br>
	Always "Standard-Shader" (for Effect) is used when Play "Effect"-animation(SSEEs).<br>
	<br>
	Only affects "Effect" objects controlled by Animation-Object instances of this class.<br>
	(Affects child "Effect" Animation-Objects only.)<br>
	<br>
	This process may affect performance adversely as materials will be rebuilt and re-searched.<br>
	Therefore, not recommended to call this function even if not necessary.
		(e.g., Calling this function in every loop process.)<br>
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
	public UnityEngine.Shader ShaderSetStandardEffect(	UnityEngine.Shader shader,
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

		if(null == DataAnimation)
		{
			return(null);
		}
		Script_SpriteStudio6_DataProject dataProject = DataAnimation.DataProject;
		if(null == dataProject)
		{
			return(null);
		}

		return(dataProject.ShaderSetStandardEffect(shader, functionMaterialSetUp, flagReplaceMaterialCache));
	}

	/* ********************************************************* */
	//! Replace Standard-(Stencil)Shader for Animation
	/*!
	@param	shader
		Shader to be set<br>
		null == Reset to initial
	@param	flagReplaceMaterialCache
		true == Cached materials (that using  standard-shaders) are replaced new shader.<br>
		false == Cached materials are not changing.
	@param	flagGlobal
		true == Replace Standard-Shader that "Global"(DataProject) has.<br>
		false == Replace Standard-Shader that "Local"(only this Animation-Object) has.<br>
		Default: false
	@retval	Return-Value
		Previous shader

	Changes the "Stencil-Shader" used in animations.<br>
	Always "Stencil-Shader" is used for drawing "Mask" parts.<br>
	<br>
	Only affects Animation-Objects controlled by instances of this class.<br>
	<br>
	This process may affect performance adversely as materials will be rebuilt and re-searched.<br>
	Therefore, not recommended to call this function even if not necessary.
		(e.g., Calling this function in every loop process.)<br>
	<br>
	Normally, do not use this function.<br>
	(Unless change implementation of "Masking", should not have a chance to use this function.)<br>
	<br>
	When create material using "shader", called material initialize function will be shared
		with for animation (specified with "ShaderSetStandardAnimation").<br>
	*/
	public UnityEngine.Shader ShaderSetStandardStencil(UnityEngine.Shader shader, bool flagReplaceMaterialCache, bool flagGlobal=false)
	{
		if(false == flagGlobal)
		{	/* Local (Animation-Object) */
			if(null == CacheMaterial)
			{
				return(null);
			}

			Status |= FlagBitStatus.CHANGE_CACHEMATERIAL;
			return(CacheMaterial.ShaderReplaceStandardStencil(	shader,
																null,
																null,
																flagReplaceMaterialCache
														)
				);
		}

		if(null == DataAnimation)
		{
			return(null);
		}
		Script_SpriteStudio6_DataProject dataProject = DataAnimation.DataProject;
		if(null == dataProject)
		{
			return(null);
		}

		return(dataProject.ShaderSetStandardStencil(shader, flagReplaceMaterialCache));
	}

	/* ********************************************************* */
	//! Replace Texture for Cell-Map
	/*!
	@param	indexCellMap
		index of CellMap
	@param	texture
		Texture to be set
		null == Stop replacing (revert to master-data)
	@param	flagReplaceMaterialCache
		true == Cached materials (that using  standard-shaders) are replaced new shader.<br>
		false == Cached materials are not changing.
	@retval	Return-Value
		Previous texture

	Change texture assigned to "Cell-Map".
	<br>
	Only affects Animation-Objects controlled by instances of this class.<br>
	CAUTION: There is no way to change texture on master-data (DataProject).<br>
	<br>
	This process may affect performance adversely as materials will be rebuilt and re-searched.<br>
	Therefore, not recommended to call this function even if not necessary.
		(e.g., Calling this function in every loop process.)<br>
	*/
	public UnityEngine.Texture TextureSetCellMap(int indexCellMap, UnityEngine.Texture texture, bool flagReplaceMaterialCache)
	{
		if((null == CacheMaterial) || (null == TableTexture))
		{
			return(null);
		}
		if((0 > indexCellMap) || (TableTexture.Length <= indexCellMap))
		{
			return(null);
		}

		Status |= FlagBitStatus.CHANGE_CACHEMATERIAL;

		/* Replace texture */
		UnityEngine.Texture textureOld = TableTexture[indexCellMap];
		TableTexture[indexCellMap] = texture;

		/* Rebuild Material */
		if(true == flagReplaceMaterialCache)
		{
			CacheMaterial.TextureReplace(indexCellMap, texture);
		}

		return(textureOld);
	}

	/* Obsolete *//* public UnityEngine.Material[] TableGetMaterial(bool flagInUse=true) */
	/* Obsolete *//* public bool TableSetMaterial(UnityEngine.Material[] tableMaterial) */
	/* Obsolete *//* public int CountGetTableMaterial(bool flagInUse=true) */
	/* Obsolete *//* public UnityEngine.Material[] TableCopyMaterialShallow(bool flagInUse=true) */
	/* Obsolete *//* public UnityEngine.Material[] TableCopyMaterialDeep(bool flagInUse=true) */
	/* Obsolete *//* public int CountGetTextureTableMaterial(bool flagInUse=true) */
	/* Obsolete *//* public bool TableSetMaterialInstance(int idParts, UnityEngine.Material[] tableMaterial, bool flagInvolveChildInstance=true) */
	/* Obsolete *//* public bool TableSetMaterialEffect(int idParts, UnityEngine.Material[] tableMaterial) */
	#endregion Functions

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	/* Obsolete *//* public static partial class Material */
	/* Obsolete *//* { */
		/* Obsolete *//* public static int CountGetTable(int countCellMap) */
		/* Obsolete *//* public static int IndexGetTable(int indexCellMap, Library_SpriteStudio6.KindOperationBlend operationBlend, Library_SpriteStudio6.KindMasking masking) */
		/* Obsolete *//* public static int CountGetTexture(UnityEngine.Material[] tableMaterial) */
		/* Obsolete *//* public static bool TextureSet(UnityEngine.Material[] tableMaterial, int indexCellMap, Texture2D texture, bool flagMaterialNew) */
	/* Obsolete *//* } */
	#endregion Classes, Structs & Interfaces
}
