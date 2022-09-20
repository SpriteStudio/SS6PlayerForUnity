/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
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
public partial class Script_SpriteStudio6_PartsUnityUI : MonoBehaviour
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public Script_SpriteStudio6_RootUnityUI PartsRoot;

	public Library_SpriteStudio6.Data.Parts.Animation.KindFeature Feature;
	public int IndexPartsParent;

	public float /* Library_SpriteStudio6.KindOperationBlend */ OperationBlend;
	public Vector2 CellUV;
	public Vector2 CellSize;
	public Vector2 CellPivot;

	public float OrderDraw;

	public Vector2 ScaleLocal;
	public float /* bool */ FlagHide;
	public float RateOpacity;
	public float /* Library_SpriteStudio6.KindOperationBlend */ PartsColorOperation;

	public Color PartsColorLU;
	public float PartsColorPowerLU;
	public Color PartsColorRU;
	public float PartsColorPowerRU;
	public Color PartsColorRD;
	public float PartsColorPowerRD;
	public Color PartsColorLD;
	public float PartsColorPowerLD;

	public Vector2 VertexCorrectionLU;
	public Vector2 VertexCorrectionRU;
	public Vector2 VertexCorrectionRD;
	public Vector2 VertexCorrectionLD;

	public Vector2 OffsetPivot;
	public Vector2 SizeForce;
	public Vector2 PositionTexture;
	public Vector2 ScalingTexture;
	public float RotationTexture;
	public float /* bool */ FlagFlipTextureX;
	public float /* bool */ FlagFlipTextureY;

	/* WorkArea */
	internal Matrix4x4 MatrixTransform;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
	/* MEMO: Since this class is storage for decoded animation data, */
	/*         implemented to process MonoBehaviour's lifecycle.     */
//	void Awake()
//	{
//	}

//	void Start()
//	{
//	}

//	void Update()
//	{
//	}

//	void LateUpdate()
//	{
//	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	internal int MeshPopulate(	UnityEngine.UI.VertexHelper vertexHelper,
								int indexVertex,
								Vector2 reciprocalSizeTexture,
								Color32 color
							)
	{
		UIVertex dataVertex = UIVertex.simpleVert;

		/* Calculate Transform-Matrix & Draw-Rectangle */
		Matrix4x4 matrixTransform = MatrixTransform * Matrix4x4.TRS(	Vector3.zero,
																		Quaternion.identity,
																		ScaleLocal
																);
		Vector2 sizeSprite = CellSize;
		Vector2 pivotSprite = CellPivot;
		{
			pivotSprite.x += (sizeSprite.x * OffsetPivot.x);
			pivotSprite.y -= (sizeSprite.y * OffsetPivot.y);

			/* Correction with Forced-Sizing */
			float size = SizeForce.x;
			float ratePivot;
			if(0.0f <= size)
			{
				ratePivot = pivotSprite.x / sizeSprite.x;
				sizeSprite.x = size;
				pivotSprite.x = size * ratePivot;
			}
			size = SizeForce.y;
			if(0.0f <= size)
			{
				ratePivot = pivotSprite.y / sizeSprite.y;
				sizeSprite.y = size;
				pivotSprite.y = size * ratePivot;
			}
		}

		Vector2 positionLU;
		Vector2 positionRU;
		Vector2 positionRD;
		Vector2 positionLD;
		Vector3 positionC;
		{
			float positionL = -pivotSprite.x;
			float positionU = pivotSprite.y;
			float positionR = positionL + sizeSprite.x;
			float positionD = positionU - sizeSprite.y;
			positionLU = new Vector2(positionL, positionU) + VertexCorrectionLU;
			positionRU = new Vector2(positionR, positionU) + VertexCorrectionRU;
			positionRD = new Vector2(positionR, positionD) + VertexCorrectionRD;
			positionLD = new Vector2(positionL, positionD) + VertexCorrectionLD;

			/* MEMO: Center is the intersection of lines through midpoints of opposite-sides. (not of diagonals) */
			Vector3 coordinateLURU = (positionLU + positionRU) * 0.5f;
			Vector3 coordinateLULD = (positionLU + positionLD) * 0.5f;
			Vector3 coordinateLDRD = (positionLD + positionRD) * 0.5f;
			Vector3 coordinateRURD = (positionRU + positionRD) * 0.5f;
			Library_SpriteStudio6.Utility.Math.CoordinateGetDiagonalIntersection(	out positionC,
																					ref coordinateLURU,
																					ref coordinateRURD,
																					ref coordinateLULD,
																					ref coordinateLDRD
																				);
		}

		/* Calculate Transform-Matrix for UV-Mapping */
		Vector2 sScaleTexture = ScalingTexture;
		{
			if(0.5f < FlagFlipTextureX)
			{
				sScaleTexture.x += -1.0f;
			}
			if(0.5f < FlagFlipTextureY)
			{
				sScaleTexture.y *= -1.0f;
			}
		}

		Vector2 centerMapping = (CellSize * 0.5f) + CellUV;
		Quaternion rotationTexture = Quaternion.Euler(0.0f, 0.0f, -RotationTexture);
		Matrix4x4 matrixTransformTexture = Matrix4x4.TRS(	new Vector3(	(centerMapping.x * reciprocalSizeTexture.x) + PositionTexture.x,
																			(1.0f - (centerMapping.y * reciprocalSizeTexture.y)) - PositionTexture.y,
																			0.0f
																	),
															Quaternion.Euler(0.0f, 0.0f, -RotationTexture),
															(CellSize * reciprocalSizeTexture * sScaleTexture)
													);
		/* Blend parameters */
		Vector2 blend = new Vector2(PartsColorOperation, OperationBlend);
		Color32 colorParts32;

		/* Set vertices */
		dataVertex.position = matrixTransform.MultiplyPoint3x4(positionLU);
		dataVertex.color = color;	// PartsColor;
		dataVertex.uv0 = matrixTransformTexture.MultiplyPoint3x4(Library_SpriteStudio6.Draw.Model.TableUVMapping[(int)Library_SpriteStudio6.KindVertex.LU]);
		dataVertex.uv1 = blend;
		dataVertex.uv2 = new Vector2(RateOpacity * PartsColorPowerLU, 0.0f);
		colorParts32 = PartsColorLU;
		dataVertex.uv3 = new Vector2(	((float)colorParts32.r * 256.0f + (float)colorParts32.g),
										((float)colorParts32.b * 256.0f + (float)colorParts32.a)
								);
		vertexHelper.AddVert(dataVertex);

		dataVertex.position = matrixTransform.MultiplyPoint3x4(positionRU);
//		dataVertex.color = color;	// PartsColor;
		dataVertex.uv0 = matrixTransformTexture.MultiplyPoint3x4(Library_SpriteStudio6.Draw.Model.TableUVMapping[(int)Library_SpriteStudio6.KindVertex.RU]);
//		dataVertex.uv1 = blend;
		dataVertex.uv2 = new Vector2(RateOpacity * PartsColorPowerRU, 0.0f);
		colorParts32 = PartsColorRU;
		dataVertex.uv3 = new Vector2(	((float)colorParts32.r * 256.0f + (float)colorParts32.g),
										((float)colorParts32.b * 256.0f + (float)colorParts32.a)
								);
		vertexHelper.AddVert(dataVertex);

		dataVertex.position = matrixTransform.MultiplyPoint3x4(positionRD);
//		dataVertex.color = color;	// PartsColor;
		dataVertex.uv0 = matrixTransformTexture.MultiplyPoint3x4(Library_SpriteStudio6.Draw.Model.TableUVMapping[(int)Library_SpriteStudio6.KindVertex.RD]);
//		dataVertex.uv1 = blend;
		dataVertex.uv2 = new Vector2(RateOpacity * PartsColorPowerRD, 0.0f);
		colorParts32 = PartsColorRD;
		dataVertex.uv3 = new Vector2(	((float)colorParts32.r * 256.0f + (float)colorParts32.g),
										((float)colorParts32.b * 256.0f + (float)colorParts32.a)
								);
		vertexHelper.AddVert(dataVertex);

		dataVertex.position = matrixTransform.MultiplyPoint3x4(positionLD);
//		dataVertex.color = color;	// PartsColor;
		dataVertex.uv0 = matrixTransformTexture.MultiplyPoint3x4(Library_SpriteStudio6.Draw.Model.TableUVMapping[(int)Library_SpriteStudio6.KindVertex.LD]);
//		dataVertex.uv1 = blend;
		dataVertex.uv2 = new Vector2(RateOpacity * PartsColorPowerLD, 0.0f);
		colorParts32 = PartsColorLD;
		dataVertex.uv3 = new Vector2(	((float)colorParts32.r * 256.0f + (float)colorParts32.g),
										((float)colorParts32.b * 256.0f + (float)colorParts32.a)
								);
		vertexHelper.AddVert(dataVertex);

		dataVertex.position = matrixTransform.MultiplyPoint3x4(positionC);
//		dataVertex.color = color;	// PartsColor;
		dataVertex.uv0 = matrixTransformTexture.MultiplyPoint3x4(Library_SpriteStudio6.Draw.Model.TableUVMapping[(int)Library_SpriteStudio6.KindVertex.C]);
//		dataVertex.uv1 = blend;
		dataVertex.uv2 = new Vector2(RateOpacity * ((PartsColorPowerLU + PartsColorPowerRU + PartsColorPowerRD + PartsColorPowerLD) * 0.25f), 0.0f);
		colorParts32 = (PartsColorLU + PartsColorRU + PartsColorRD + PartsColorLD) * 0.25f;
		dataVertex.uv3 = new Vector2(	((float)colorParts32.r * 256.0f + (float)colorParts32.g),
										((float)colorParts32.b * 256.0f + (float)colorParts32.a)
								);
		vertexHelper.AddVert(dataVertex);

		/* Set vertex-indices (triangles) */
		vertexHelper.AddTriangle(indexVertex + (int)Library_SpriteStudio6.KindVertex.C, indexVertex + (int)Library_SpriteStudio6.KindVertex.RU, indexVertex + (int)Library_SpriteStudio6.KindVertex.LU);
		vertexHelper.AddTriangle(indexVertex + (int)Library_SpriteStudio6.KindVertex.C, indexVertex + (int)Library_SpriteStudio6.KindVertex.RD, indexVertex + (int)Library_SpriteStudio6.KindVertex.RU);
		vertexHelper.AddTriangle(indexVertex + (int)Library_SpriteStudio6.KindVertex.C, indexVertex + (int)Library_SpriteStudio6.KindVertex.LD, indexVertex + (int)Library_SpriteStudio6.KindVertex.RD);
		vertexHelper.AddTriangle(indexVertex + (int)Library_SpriteStudio6.KindVertex.C, indexVertex + (int)Library_SpriteStudio6.KindVertex.LU, indexVertex + (int)Library_SpriteStudio6.KindVertex.LD);
		indexVertex += (int)Library_SpriteStudio6.KindVertex.TERMINATOR4;

		return(indexVertex);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	#endregion Classes, Structs & Interfaces
}
