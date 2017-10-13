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
	public Library_SpriteStudio6.Data.Animation[] TableAnimation;

	public Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer CapacitySetup;
	public AttributeSetup Setup;

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
		TableMaterial = null;

		TableParts = null;
		TableAnimation = null;

		CapacitySetup = null;
		Setup.CleanUp();

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
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionPartsColor(TableAnimation[i].TableParts[j].Plain.PartsColor);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVertexCorrection(TableAnimation[i].TableParts[j].Plain.VertexCorrection);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Plain.OffsetPivot);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Plain.PositionTexture);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Plain.ScalingTexture);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(TableAnimation[i].TableParts[j].Plain.RotationTexture);

				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionInt(TableAnimation[i].TableParts[j].Fix.IndexCellMap);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionCoordinateFix(TableAnimation[i].TableParts[j].Fix.Coordinate);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionPartsColorFix(TableAnimation[i].TableParts[j].Fix.PartsColor);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionUVFix(TableAnimation[i].TableParts[j].Fix.UV0);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Fix.SizeCollision);
				Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(TableAnimation[i].TableParts[j].Fix.PivotCollision);
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
		CODE_010000 = 0x00010000,	/* SS6PU Ver.1.0.0 */

		SUPPORT_EARLIEST = CODE_010000,
		SUPPORT_LATEST = CODE_010000
	}
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	[System.Serializable]
	public struct AttributeSetup
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		public Library_SpriteStudio6.Data.Animation.Attribute.Status Status;

		public Vector3 Position;
		public Vector3 Rotation;
		public Vector2 Scaling;

		public float RateOpacity;

		public Vector2 PositionAnchor;
		public Vector2 SizeForce;

		public Library_SpriteStudio6.Data.Animation.Attribute.UserData UserData;
		public Library_SpriteStudio6.Data.Animation.Attribute.Instance Instance;
		public Library_SpriteStudio6.Data.Animation.Attribute.Effect Effect;

		public float RadiusCollision;

		public AttributeGroupPlain Plain;
		public AttributeGroupFix Fix;
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public void CleanUp()
		{
			Status.CleanUp();

			Position = Vector3.zero;
			Rotation = Vector3.zero;
			Scaling = Vector2.one;

			RateOpacity = 0.0f;

			PositionAnchor = Vector2.zero;
			SizeForce = Vector2.zero;

			UserData.CleanUp();
			Instance.CleanUp();
			Effect.CleanUp();

			RadiusCollision = 0.0f;

			Plain.Cell.CleanUp();
			Plain.PartsColor.CleanUp();
			Plain.VertexCorrection.CleanUp();
			Plain.OffsetPivot = Vector2.zero;
			Plain.PositionTexture = Vector2.zero;
			Plain.ScalingTexture = Vector2.one;
			Plain.RotationTexture = 0.0f;

			Fix.IndexCellMap = -1;
			Fix.Coordinate.CleanUp();
			Fix.PartsColor.CleanUp();
			Fix.UV0.CleanUp();
			Fix.SizeCollision = Vector2.zero;
			Fix.PivotCollision = Vector2.zero;
		}
		#endregion Functions

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		[System.Serializable]
		public struct AttributeGroupPlain
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public Library_SpriteStudio6.Data.Animation.Attribute.Cell Cell;

			public Library_SpriteStudio6.Data.Animation.Attribute.PartsColor PartsColor;
			public Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection VertexCorrection;
			public Vector2 OffsetPivot;

			public Vector2 PositionTexture;
			public Vector2 ScalingTexture;
			public float RotationTexture;
			#endregion Variables & Properties
		}

		[System.Serializable]
		public struct AttributeGroupFix
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public int IndexCellMap;
			public Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix Coordinate;
			public Library_SpriteStudio6.Data.Animation.Attribute.PartsColorFix PartsColor;
			public Library_SpriteStudio6.Data.Animation.Attribute.UVFix UV0;

			public Vector2 SizeCollision;
			public Vector2 PivotCollision;
			#endregion Variables & Properties
		}
		#endregion Classes, Structs & Interfaces
	}
	#endregion Classes, Structs & Interfaces

	/* ----------------------------------------------- Deligates */
	#region Deligates
	private delegate void FunctionSignatureBootUpFunction();
	#endregion Deligates
}
