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

	/* MEMO: 念のため、セットアップポーズも入れておく */
	public Library_SpriteStudio6.Data.Parts.Animation[] TableParts;
	public Library_SpriteStudio6.Data.Animation[] TableAnimation;

	/* MEMO: Use "delegate" instead of bool because value is cleared each compiling. */
	internal FunctionSignatureBootUpFunction SignatureBootUpFunction = null;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	public void CleanUp()
	{
		Version = (KindVersion)(-1);
		TableParts = null;
		TableAnimation = null;
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
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
					count++;
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
				case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
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
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector3(TableAnimation[i].TableParts[j].Position);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector3(TableAnimation[i].TableParts[j].Rotation);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Scaling);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(TableAnimation[i].TableParts[j].RateOpacity);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].PositionAnchor);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].SizeForce);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionUserData(TableAnimation[i].TableParts[j].UserData);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionInstance(TableAnimation[i].TableParts[j].Instance);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionEffect(TableAnimation[i].TableParts[j].Effect);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(TableAnimation[i].TableParts[j].RadiusCollision);

				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionCell(TableAnimation[i].TableParts[j].Plain.Cell);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionColorBlend(TableAnimation[i].TableParts[j].Plain.ColorBlend);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVertexCorrection(TableAnimation[i].TableParts[j].Plain.VertexCorrection);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Plain.OffsetPivot);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Plain.PositionTexture);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Plain.ScalingTexture);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(TableAnimation[i].TableParts[j].Plain.RotationTexture);

				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionInt(TableAnimation[i].TableParts[j].Fix.IndexCellMap);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionCoordinateFix(TableAnimation[i].TableParts[j].Fix.Coordinate);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionColorBlendFix(TableAnimation[i].TableParts[j].Fix.ColorBlend);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionUVFix(TableAnimation[i].TableParts[j].Fix.UV0);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Fix.SizeCollision);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Fix.PivotCollision);
			}
		}
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindVersion
	{
		SS5PU = 0,	/* Before SS5PU *//* (Reserved) */
		CODE_010000 = 0x00010000,	/* SS6PU Ver.1.0.0 */

		SUPPORT_EARLIEST = CODE_010000,
		SUPPORT_LATEST = CODE_010000
	}
	#endregion Enums & Constants

	/* ----------------------------------------------- Deligates */
	#region Deligates
	internal delegate void FunctionSignatureBootUpFunction();
	#endregion Deligates
}
