﻿/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script_SpriteStudio6_DataEffect : ScriptableObject
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public KindVersion Version;
	public string Name;								/* Body-Name of SSEE */
	public Script_SpriteStudio6_DataProject DataProject;

//	public Material[] TableMaterial;

	public FlagBit FlagData;
	public bool StatusIsLockSeedRandom
	{
		get
		{
			return(0 != (FlagData & FlagBit.SEEDRANDOM_LOCK));
		}
	}
	public int SeedRandom;
	public int VersionRenderer;

	public int CountMaxParticle;
	public int CountFramePerSecond;

	public Vector2 ScaleLayout;

	public Library_SpriteStudio6.Data.Parts.Effect[] TableParts;
	public Library_SpriteStudio6.Data.Effect.Emitter[] TableEmitter;
	public int[] TableIndexEmitterOrderDraw;

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

	/* ----------------------------------------------- Functions */
	#region Functions
	public void CleanUp()
	{
		Version = (KindVersion)(-1);
	}

	public bool VersionCheckRuntime()
	{
		return(((KindVersion.SUPPORT_EARLIEST <= Version) && (KindVersion.SUPPORT_LATEST >= Version)));	/* ? true : false */
	}

	public int CountGetParts()
	{
		return((null != TableParts) ? TableParts.Length : -1);
	}

	public int CountGetEmitter()
	{
		return((null != TableEmitter) ? TableEmitter.Length : -1);
	}

#if false
	internal void BootUpTableMaterial()
	{
#if UNITY_EDITOR
		/* Reassignment for shader lost */
		/* MEMO: This process will not work unless on editor. */
		int countTableMaterial = (null != TableMaterial) ? TableMaterial.Length : 0;
		Material material = null;
		for(int i=0; i<countTableMaterial; i++)
		{
			material = TableMaterial[i];
			if(null != material)
			{
				material.shader = Shader.Find(material.shader.name);
			}
		}
		material = null;
#endif
	}
#endif

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
		SUPPORT_LATEST = CODE_010003,

		SS5PU = 0,	/* Before SS5PU *//* (Reserved) */
		CODE_010000,	/* = 0x00010000 (Miss) */	/* SS6PU Ver.1.0.0 */
		CODE_010001 = 0x00010001,	/* SS6PU Ver.1.0.26 */
			/* MEMO: Shaders changed to integrated type */
		CODE_010002 = 0x00010002,	/* SS6PU Ver.1.0.27 */
			/* MEMO: Shader's stencil-test are changed.  */
		CODE_010003 = 0x00010002,	/* SS6PU Ver.1.2.0 */
			/* MEMO: Added "Name (Original SSCE file's Body-Name) */
	}

	[System.Flags]
	public enum FlagBit
	{
		SEEDRANDOM_LOCK = 0x00000001,

		CLEAR = 0x00000000
	}
	#endregion Enums & Constants

	/* ----------------------------------------------- Delegates */
	#region Delegates
	private delegate void FunctionSignatureBootUpFunction();
	#endregion Delegates
}
