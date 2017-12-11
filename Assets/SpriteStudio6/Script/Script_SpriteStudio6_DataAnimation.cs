/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script_SpriteStudio6_DataAnimation : ScriptableObject
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public KindVersion Version;

	public Material[] TableMaterial;

	public Library_SpriteStudio6.Data.Parts.Animation[] TableParts;
	public Library_SpriteStudio6.Data.Parts.Animation.Catalog CatalogParts;

	public Library_SpriteStudio6.Data.Animation[] TableAnimation;

	/* PackAttribute's Dictionaries */
//	public Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.Dictionary Dictionary_StandardUnCompressed;
//	public Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Dictionary Dictionary_StandardCPE;
	public Library_SpriteStudio6.Data.Animation.PackAttribute.CPE_Flyweight.Dictionary Dictionary_CPE_Flyweight;
//	public Library_SpriteStudio6.Data.Animation.PackAttribute.CPE_Interpolate.Dictionary Dictionary_CPE_Interpolate;

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
	void Start()
	{
		/* Awake Base-Class */
		CountGetPartsSprite();
		StatusIsBootup = true;
	}
	#endregion ScriptableObject-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	public void CleanUp()
	{
		Version = (KindVersion)(-1);
		TableMaterial = null;

		TableParts = null;
		CatalogParts.CleanUp();

		TableAnimation = null;

		SignatureBootUpFunction = null;
	}

	public int CountGetParts()
	{
		return(TableParts.Length);
	}

	public int CountGetPartsSprite()
	{
		int countParts = TableParts.Length;
		int count = 0;
		for(int i=0; i<countParts; i++)
		{
			switch(TableParts[i].Feature)
			{
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
					count++;
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4:
					/* MEMO: "Mask"s are drawn twice(Predraw + Draw). */
					count += 2;
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MESH:
					/* MEMO: Not count. (not sprite) */
					break;
			}
		}
		return(count);
	}

	public int IndexGetParts(string name)
	{
		if(true == string.IsNullOrEmpty(name))
		{
			return(-1);
		}

		int count = TableParts.Length;
		for(int i=0; i<count; i++)
		{
			if(name == TableParts[i].Name)
			{
				return(i);
			}
		}
		return(-1);
	}

	public int CountGetAnimation()
	{
		return(TableAnimation.Length);
	}

	public int IndexGetAnimation(string name)
	{
		if(true == string.IsNullOrEmpty(name))
		{
			return(-1);
		}

		int count = TableAnimation.Length;
		for(int i=0; i<count; i++)
		{
			if(name == TableAnimation[i].Name)
			{
				return(i);
			}
		}
		return(-1);
	}

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

	internal void BootUpInterfaceAttribute()
	{
		int countAnimation = TableAnimation.Length;
		int countParts = TableParts.Length;
		for(int i=0; i<countAnimation; i++)
		{
			for(int j=0; j<countParts; j++)
			{
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionStatus(TableAnimation[i].TableParts[j].Status);

				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionCell(TableAnimation[i].TableParts[j].Cell);

				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector3(TableAnimation[i].TableParts[j].Position);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector3(TableAnimation[i].TableParts[j].Rotation);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Scaling);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].ScalingLocal);

				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(TableAnimation[i].TableParts[j].RateOpacity);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionPartsColor(TableAnimation[i].TableParts[j].PartsColor);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVertexCorrection(TableAnimation[i].TableParts[j].VertexCorrection);

				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].OffsetPivot);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].PositionAnchor);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].SizeForce);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].PositionTexture);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(TableAnimation[i].TableParts[j].RotationTexture);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].ScalingTexture);

				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(TableAnimation[i].TableParts[j].RadiusCollision);

				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionUserData(TableAnimation[i].TableParts[j].UserData);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionInstance(TableAnimation[i].TableParts[j].Instance);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionEffect(TableAnimation[i].TableParts[j].Effect);
			}
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
		SS5PU = 0,	/* Before SS5PU *//* (Reserved) */
		CODE_010000 = 0x00010000,	/* SS6PU Ver.0.8.0 */
		CODE_010001 = 0x00010001,	/* SS6PU Ver.0.9.0 */
			/* MEMO: Add "Library_SpriteStudio6.Data.Parts.Animation.CountMesh" */
			/* MEMO: Add "Library_SpriteStudio6.Data.Parts.Animation.Category" */
		CODE_010002 = 0x00010002,	/* SS6PU Ver.0.9.8 */
			/* MEMO: Add "PackAttribute's Dictionaries" */

		SUPPORT_EARLIEST = CODE_010000,
		SUPPORT_LATEST = CODE_010002
	}
	#endregion Enums & Constants

	/* ----------------------------------------------- Deligates */
	#region Deligates
	private delegate void FunctionSignatureBootUpFunction();
	#endregion Deligates
}
