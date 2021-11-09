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
	internal Library_SpriteStudio6.Control.CacheMaterial CacheMaterialAnimation = null;
	internal Library_SpriteStudio6.Control.CacheMaterial CacheMaterialEffect = null;

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
		if(null != CacheMaterialAnimation)
		{
			CacheMaterialAnimation.ShutDown(true);
			CacheMaterialAnimation = null;
		}
		if(null != CacheMaterialEffect)
		{
			CacheMaterialEffect.ShutDown(true);
			CacheMaterialEffect = null;
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
		CacheMaterialAnimation = null;
		CacheMaterialEffect = null;
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
		/* Material-Cache Set up */
		/* MEMO: Take into account possibility of generating materials without textures. */
		int countTexture = (null != TableTexture) ? TableTexture.Length : 0;
		if(0 >= countTexture)
		{
			countTexture++;
		}

		CacheMaterialAnimation = new Library_SpriteStudio6.Control.CacheMaterial();
		if(false == CacheMaterialAnimation.BootUp(countTexture * (int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR_TABLEMATERIAL, false))
		{
			goto Start_ErrorEnd;
		}

		CacheMaterialEffect = new Library_SpriteStudio6.Control.CacheMaterial();
		if(false == CacheMaterialEffect.BootUp(countTexture * (int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_TABLEMATERIAL, true))
		{
			goto Start_ErrorEnd;
		}

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
		Shader's name in animation-data
		null == Default(Standard) shader's name
	@param	shader
		Shader
	@param	masking
		Masking type
	@param	flagCreateNew
		true == If not exist, create.
		false == If not exist, return null.
	@retval	Return-Value
		Instance of Material
		null == Not exist or Error

	Get specified-spec material (for "Animation").<br>
	<br>
	If existing, return that has already created (Will not create
		multiple of same material).<br>
	If not existing, material will be created and returned.<br>
	*/
	internal UnityEngine.Material MaterialGetAnimation(	int indexCellMap,
														Library_SpriteStudio6.KindOperationBlend operationBlend,
														Library_SpriteStudio6.KindMasking masking,
														string nameShader,
														Shader shader,
														bool flagCreateNew
													)
	{
		if(null == CacheMaterialAnimation)
		{
			return(null);
		}

		return(CacheMaterialAnimation.MaterialGetAnimation(	indexCellMap,
															operationBlend,
															masking,
															nameShader,
															shader,
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
		Shader's name in animation-data
		null == Default(Standard) shader's name
	@param	shader
		Shader
	@param	flagCreateNew
		true == If not exist, create.
		false == If not exist, return null.
	@retval	Return-Value
		Instance of Material
		null == Error

	Get specified-spec material (for "Effect").<br>
	<br>
	If existing, return that has already created (Will not create
		multiple of same material).<br>
	If not existing, material will be created and returned.<br>
	*/
	internal UnityEngine.Material MaterialGetEffect(	int indexCellMap,
														Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
														Library_SpriteStudio6.KindMasking masking,
														string nameShader,
														Shader shader,
														bool flagCreateNew
												)
	{
		if(null == CacheMaterialEffect)
		{
			return(null);
		}

		return(CacheMaterialEffect.MaterialGetEffect(	indexCellMap,
														operationBlend,
														masking,
														nameShader,
														shader,
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
		Shader's name in animation-data
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
		if(null == CacheMaterialAnimation)
		{
			return(null);
		}

		return(CacheMaterialAnimation.MaterialReplaceAnimation(	indexCellMap,
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
		if(null == CacheMaterialEffect)
		{
			return(null);
		}

		return(CacheMaterialEffect.MaterialReplaceEffect(	indexCellMap,
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
		if((null != CacheMaterialAnimation) && (true == flagPurgeAnimation))
		{
			CacheMaterialAnimation.DataPurge(false);
		}

		if((null != CacheMaterialEffect) && (true == flagPurgeEffect))
		{
			CacheMaterialEffect.DataPurge(false);
		}
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
