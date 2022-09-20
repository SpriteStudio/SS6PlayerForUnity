/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script_SpriteStudio6_DataProject : ScriptableObject
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public KindVersion Version;
	public string Name;								/* Body-Name of SSPJ */

	public Script_SpriteStudio6_DataCellMap DataCellMap;
	public Script_SpriteStudio6_DataAnimation[] DataAnimation;
	public Script_SpriteStudio6_DataEffect[] DataEffect;
	public Script_SpriteStudio6_DataSequence[] DataSequence;

	public Object[] PrefabAnimation;
	public Object[] PrefabEffect;

	public Texture[] TableTexture;					/* Index is the same as for "DataCellMap.TableCellMap". */
	internal Library_SpriteStudio6.Control.CacheMaterial CacheMaterial = null;

	/* MEMO: Use "delegate" instead of bool because value is cleared each compiling. */
	private FunctionSignatureBootUpFunction SignatureBootUpFunction = null;
	internal bool StatusIsBootup
	{
		get
		{
			return((null != SignatureBootUpFunction) ? true : false);
		}
		set
		{
			if(true == value)
			{
				SignatureBootUpFunction = FunctionBootUpSignature;
			}
			else
			{
				SignatureBootUpFunction = null;
			}
		}
	}
	#endregion Variables & Properties

	/* ----------------------------------------------- ScriptableObject-Functions */
	#region ScriptableObject-Functions
	void Awake()
	{
		if(false == StatusIsBootup)
		{
			BootUp();
		}
	}

	void OnEnable()
	{
		if(false == StatusIsBootup)
		{
			BootUp();
		}
	}

	void OnDestroy()
	{
		/* All Material-Cache shut-down */
		if(null != CacheMaterial)
		{
			CacheMaterial.ShutDown(true);
			CacheMaterial = null;
		}
	}
	#endregion ScriptableObject-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	public void CleanUp()
	{
		Version = (KindVersion)(-1);

		DataCellMap = null;
		DataAnimation = null;
		DataEffect = null;
		DataSequence = null;

		PrefabAnimation = null;
		PrefabEffect = null;

		TableTexture = null;
		CacheMaterial = null;
	}

	public bool VersionCheckRuntime()
	{
		return(((KindVersion.SUPPORT_EARLIEST <= Version) && (KindVersion.SUPPORT_LATEST >= Version)));	/* ? true : false */
	}

	/* ********************************************************* */
	//! Get Animation-Pack index
	/*!
	@param	name
		Animation-Pack name
	@retval	Return-Value
		Animation-Pack's index<br>
		-1 == Error / "name" is not-found.

	Get Animation-Pack index by name.<br>
	<br>
	Animation-Pack name is the same as the SSAE file body-name (without extension).
	This value is used to array-index of "DataAnimation".
	*/
	public int IndexGetPackAnimation(string name)
	{
		if(true == string.IsNullOrEmpty(name))
		{
			return(-1);
		}

		int count = DataAnimation.Length;
		for(int i=0; i<count; i++)
		{
			if(name == DataAnimation[i].Name)
			{
				return(i);
			}
		}
		return(-1);
	}

	/* ********************************************************* */
	//! Get Effect index
	/*!
	@param	name
		Effect name
	@retval	Return-Value
		Effect's index<br>
		-1 == Error / "name" is not-found.

	Get Effect index by name.<br>
	<br>
	Effect name is the same as the SSEE file body-name (without extension).
	This value is used to array-index of "DataEffect".
	*/
	public int IndexGetPackEffect(string name)
	{
		if(true == string.IsNullOrEmpty(name))
		{
			return(-1);
		}

		int count = DataEffect.Length;
		for(int i=0; i<count; i++)
		{
			if(name == DataEffect[i].Name)
			{
				return(i);
			}
		}
		return(-1);
	}

	/* ********************************************************* */
	//! Get Sequence-Pack index
	/*!
	@param	name
		Sequence-Pack name
	@retval	Return-Value
		Sequence-Pack's index<br>
		-1 == Error / "name" is not-found.

	Get Sequence-Pack index by name.<br>
	<br>
	Sequence-Pack name is the same as the SSQE file body-name (without extension).
	This value is used to array-index of "DataSequence".
	*/
	public int IndexGetPackSequence(string name)
	{
		if(true == string.IsNullOrEmpty(name))
		{
			return(-1);
		}

		int count = DataSequence.Length;
		for(int i=0; i<count; i++)
		{
			if(name == DataSequence[i].Name)
			{
				return(i);
			}
		}
		return(-1);
	}

	private bool BootUp()
	{
		/* Texture Table Set up */
		/* MEMO: Take into account possibility of generating materials without textures. */
		int countTexture = (null != TableTexture) ? TableTexture.Length : 0;
		if(0 >= countTexture)
		{
			countTexture++;
		}

		/* Material-Cache (for Animation) Set up */
		CacheMaterial = new Library_SpriteStudio6.Control.CacheMaterial();
		if(false == CacheMaterial.BootUp(countTexture * (int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR_TABLEMATERIAL))
		{
			goto Start_ErrorEnd;
		}
		CacheMaterial.ShaderStandardAnimation = Library_SpriteStudio6.Data.Shader.SpriteSS6PU;	/* Standard-Shader(Animation) */
		CacheMaterial.ShaderStandardEffect = Library_SpriteStudio6.Data.Shader.EffectSS6PU;	/* Standard-Shader(Effect) */
		CacheMaterial.ShaderStandardStencil = Library_SpriteStudio6.Data.Shader.StencilSS6PU;	/* Standard-Shader(Stencil) */

		CacheMaterial.FunctionMaterialSetUpAnimation = Library_SpriteStudio6.Data.Shader.FunctionMaterialSetUpAnimation;
		CacheMaterial.FunctionMaterialSetUpEffect = Library_SpriteStudio6.Data.Shader.FunctionMaterialSetUpEffect;

		/* Status Set */
		StatusIsBootup = true;

		return(true);

	Start_ErrorEnd:;
		return(false);
	}

	/* ********************************************************* */
	//! Get Material (for "Animation")
	/*!
	@param	indexCellMap
		CellMap's index
	@param	operationBlend
		Blending Operation
	@param	nameShader
		Name identifies shader. (Specified in SSAE:Animation-Data)<br>
		null == Standard-Shader's name
	@param	masking
		Masking type
	@param	flagCreateNew
		true == If not exist, create.
		false == If not exist, return null.
	@param	shader
		Shader used to set parameters when creating new material<br>
		null == Standard-Shader<br>
		Default: null
	@param	functionMaterialSetUp
		Function called to set parameters when creating new material<br>
		null == Default Function<br>
		Default: null
	@retval	Return-Value
		Instance of Material
		null == Not exist or Error

	Get specified-spec material (for "Animation").<br>
	<br>
	If existing, return that has already created (Will not create
		multiple of same material).<br>
	If not existing, material will be created and returned.<br>
	<br>
	When "flagCreateNew" is false, "shader" and "functionMaterialSetUp" can be null. (be ignored.)<br>
	<br>
	Specify to "functionMaterialSetUp" must must be following type function.<br>
	UnityEngine.Material FunctionMaterialSetUpAnimation(<br>
		UnityEngine.Material material,<br>
		int(Library_SpriteStudio6.KindOperationBlend) operationBlend,<br>
		Library_SpriteStudio6.KindMasking masking,<br>
		bool flagZWrite<br>
	);<br>
	[Arguments]<br>
		material: Material to set parameters (blending method, etc.)<br>
		operationBlend: Blend type given to this function. (Casted to int) <br>
		masking: Masking type given to this function.<br>
		flagZWrite: true==Write to Z-Buffer. / false==No-touching Z-Buffer.<br>
	[Return]<br>
		On success, instance of the material (same as argument "material")<br>
		On failure, null.
	*/
	internal UnityEngine.Material MaterialGetAnimation(	int indexCellMap,
														Library_SpriteStudio6.KindOperationBlend operationBlend,
														Library_SpriteStudio6.KindMasking masking,
														string nameShader,
														bool flagCreateNew,
														Shader shader=null,
														Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp=null
													)
	{
		if(null == CacheMaterial)
		{
			return(null);
		}

		return(CacheMaterial.MaterialGetAnimation(	indexCellMap,
													operationBlend,
													masking,
													nameShader,
													shader,
													functionMaterialSetUp,
													TableTexture,
													flagCreateNew
												)
			);
	}

	/* ********************************************************* */
	//! Get Material (for "Effect")
	/*!
	@param	indexCellMap
		CellMap's index
	@param	operationBlend
		Blending Operation
	@param	masking
		Masking type
	@param	nameShader
		Shader's name in animation-data<br>
		null == Standard-Shader's name
	@param	flagCreateNew
		true == If not exist, create.
		false == If not exist, return null.
	@param	shader
		Shader used to set parameters when creating new material<br>
		null == Standard-Shader<br>
		Default: null
	@param	functionMaterialSetUp
		Function called to set parameters when creating new material<br>
		null == Default Function<br>
		Default: null
	@retval	Return-Value
		Instance of Material
		null == Error

	Get specified-spec material (for "Effect").<br>
	<br>
	If existing, return that has already created (Will not create
		multiple of same material).<br>
	If not existing, material will be created and returned.<br>
	<br>
	When "flagCreateNew" is false, "shader" and "functionMaterialSetUp" can be null. (be ignored.)<br>
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
	internal UnityEngine.Material MaterialGetEffect(	int indexCellMap,
														Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
														Library_SpriteStudio6.KindMasking masking,
														string nameShader,
														bool flagCreateNew,
														Shader shader=null,
														Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp=null
												)
	{
		if(null == CacheMaterial)
		{
			return(null);
		}

		return(CacheMaterial.MaterialGetEffect(	indexCellMap,
												operationBlend,
												masking,
												nameShader,
												shader,
												functionMaterialSetUp,
												TableTexture,
												flagCreateNew
										)
			);
	}

	/* ********************************************************* */
	//! Replace Material (for "Animation")
	/*!
	@param	indexCellMap
		CellMap's index
	@param	operationBlend
		Blending Operation
	@param	masking
		Masking type
	@param	nameShadcer
		Shader's name in animation-data<br>
		null == Default(Standard) shader's name
	@param	material
		New material
	@retval	Return-Value
		Originally set material<br>
		false == Failure (Error)

	Replaces already defined material for purpose identified by
		"shader", "texture", "operaTionBlend" ​and "masking"
		with "materialNew".<br>
	<br>
	If target material cannot be found, this function will fail.
	*/
	internal UnityEngine.Material MaterialReplaceAnimation(	int indexCellMap,
															Library_SpriteStudio6.KindOperationBlend operationBlend,
															Library_SpriteStudio6.KindMasking masking,
															string nameShader,
															UnityEngine.Material material
														)
	{
		if(null == CacheMaterial)
		{
			return(null);
		}

		return(CacheMaterial.MaterialReplaceAnimation(	indexCellMap,
														operationBlend,
														masking,
														nameShader,
														material
													)
			);
	}

	/* ********************************************************* */
	//! Replace Material (for "Effect")
	/*!
	@param	indexCellMap
		CellMap's index
	@param	operationBlend
		Blending Operation
	@param	masking
		Masking type
	@param	nameShadcer
		Shader's name in animation-data
		null == Default(Standard) shader's name
	@param	material
		New material
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Replaces already defined material for purpose identified by
		"shader", "texture", "operaTionBlend" ​and "masking"
		with "materialNew".<br>
	<br>
	If target material cannot be found, this function will fail.
	*/
	internal UnityEngine.Material MaterialReplaceEffect(	int indexCellMap,
															Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
															Library_SpriteStudio6.KindMasking masking,
															string nameShader,
															UnityEngine.Material material
														)
	{
		if(null == CacheMaterial)
		{
			return(null);
		}

		return(CacheMaterial.MaterialReplaceEffect(	indexCellMap,
													operationBlend,
													masking,
													nameShader,
													material
											)
			);
	}

	/* ********************************************************* */
	//! Purge Materials
	/*!
	@param
		(none)
	@retval	Return-Value
		(None)

	Purge all existing materials.
	*/
	private void MaterialPurge(bool flagPurgeAnimation=true, bool flagPurgeEffect=true)
	{
		if((null != CacheMaterial) && (true == flagPurgeAnimation))
		{
			CacheMaterial.DataPurge(false);
		}
	}

	/* ********************************************************* */
	//! Replace Standard-(Pixel)Shader for Animation
	/*!
	@param	shader
		Shader to be set<br>
		null == Reset to initial
	@param	functionMaterialSetUp
		Function called to set parameters when replacing material<br>
		null == Default Function<br>
	@param	flagReplaceMaterialCache
		true == Cached materials (that using  standard-shaders) are replaced new shader.
		false == Cached materials are not changing.
	@retval	Return-Value
		Previous shader

	Changes the "Standard-Shader" (for Animation) used in animations.<br>
	Always "Standard-Shader" is used when attribute "Shader" is not used in animations(SSAEs).<br>
	<br>
	Affects Animation-Objects that use Project-data (SSPJ) controlled by this class.<br>
	However, Animation-Objects' setting (Script_SpriteStudio6_Root) will take priority.<br>
	<br>
	"functionMaterialSetUp" is a material initialization function that is called when create material using "shader".
		(Also used when replace material in cache in this function)<br>
	<br>
	specification of "functionMaterialSetUp" is the same as argument of the same name in "MaterialGetAnimation".<br>
	<br>
	This process may affect performance adversely as materials will be rebuilt.<br>
	Therefore, not recommended to call this function even if not necessary.
		(e.g., Calling this function in every loop process.)<br>
	<br>
	*/
	internal UnityEngine.Shader ShaderSetStandardAnimation(	UnityEngine.Shader shader,
															Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp,
															bool flagReplaceMaterialCache
														)
	{
		return(CacheMaterial.ShaderReplaceStandardPixelAnimation(	shader,
																	Library_SpriteStudio6.Data.Shader.SpriteSS6PU,
																	functionMaterialSetUp,
																	Library_SpriteStudio6.Data.Shader.FunctionMaterialSetUpAnimation,
																	flagReplaceMaterialCache
															)
			);
	}

	/* ********************************************************* */
	//! Replace Standard-(Pixel)Shader for Effect
	/*!
	@param	shader
		Shader to be set<br>
		null == Reset to initial
	@param	functionMaterialSetUp
		Function called to set parameters when replacing material<br>
		null == Default Function<br>
	@param	flagReplaceMaterialCache
		true == Cached materials (that using  standard-shaders) are replaced new shader.
		false == Cached materials are not changing.
	@retval	Return-Value
		Previous shader

	Changes the "Standard-Shader" used in effects.<br>
	Always "Standard-Shader" (for Effect) is used when Play "Effect"-animation(SSEEs).<br>
	<br>
	Affects Animation-Objects that use Project-data (SSPJ) controlled by this class.<br>
	However, Animation-Objects' setting (Script_SpriteStudio6_RootEffect) will take priority.<br>
	<br>
	This process may affect performance adversely as materials will be rebuilt.<br>
	Therefore, not recommended to call this function even if not necessary.
		(e.g., Calling this function in every loop process.)<br>
	<br>
	Normally, should not have a chance to use this function.<br>
	<br>
	"functionMaterialSetUp" is a material initialization function that is called when create material using "shader".
		(Also used when replace material in cache in this function)<br>
	<br>
	specification of "functionMaterialSetUp" is the same as argument of the same name in "MaterialGetEffect".<br>
	*/
	internal UnityEngine.Shader ShaderSetStandardEffect(	UnityEngine.Shader shader,
															Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp,
															bool flagReplaceMaterialCache
													)
	{
		return(CacheMaterial.ShaderReplaceStandardPixelEffect(	shader,
																Library_SpriteStudio6.Data.Shader.EffectSS6PU,
																functionMaterialSetUp,
																Library_SpriteStudio6.Data.Shader.FunctionMaterialSetUpEffect,
																flagReplaceMaterialCache
															)
			);
	}

	/* ********************************************************* */
	//! Replace Standard-(Stencil)Shader for Animation
	/*!
	@param	shader
		Shader to be set<br>
		null == Reset to initial
	@param	flagReplaceMaterialCache
		true == Cached materials (that using  standard-shaders) are replaced new shader.
		false == Cached materials are not changing.
	@retval	Return-Value
		Previous shader

	Changes the "Stencil-Shader" (for Animation) used in animations.<br>
	Always "Stencil-Shader" is used for drawing "Mask" parts.<br>
	<br>
	Affects Animation-Objects that use Project-data (SSPJ) controlled by this class.<br>
	However, Animation-Objects' setting (Script_SpriteStudio6_Root) will take priority.	<br>
	<br>
	This process may affect performance adversely as materials will be rebuilt.<br>
	Therefore, not recommended to call this function even if not necessary.
		(e.g., Calling this function in every loop process.)<br>
	<br>
	Normally, do not use this function.<br>
	(Unless change implementation of "Masking", should not have a chance to use this function.)<br>
	<br>
	When create material using "shader", called material initialize function will be shared
		with for animation (specified with "ShaderSetStandardAnimation").<br>
	*/
	internal UnityEngine.Shader ShaderSetStandardStencil(UnityEngine.Shader shader, bool flagReplaceMaterialCache)
	{
		return(CacheMaterial.ShaderReplaceStandardStencil(	shader,
															Library_SpriteStudio6.Data.Shader.StencilSS6PU,
															Library_SpriteStudio6.Data.Shader.FunctionMaterialSetUpAnimation,
															flagReplaceMaterialCache
														)
			);
	}

	private static void FunctionBootUpSignature()
	{
		/* Dummy-Function */
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindVersion
	{
		SUPPORT_EARLIEST = CODE_010000,
		SUPPORT_LATEST = CODE_010000,

		SS5PU = 0x00000000,	/* Before SS5PU *//* (Reserved) */
			/* MEMO: There is no data equivalent to "Project(SSPJ)" by version 1.1.x. */
		CODE_010000 = 0x00010000,	/* SS6PU Ver.1.2.0 */
	}
	#endregion Enums & Constants

	/* ----------------------------------------------- Delegates */
	#region Delegates
	private delegate void FunctionSignatureBootUpFunction();
	#endregion Delegates
}
