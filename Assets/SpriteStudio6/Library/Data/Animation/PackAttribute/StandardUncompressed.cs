/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Library_SpriteStudio6
{
	public static partial class Data
	{
		public partial class Animation
		{
			public static partial class PackAttribute
			{
				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				public static class StandardUncompressed
				{
					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					public static readonly Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer Capacity = new Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer(
						true,		/* Status */
						true,		/* Position */
						true,		/* Rotation */
						true,		/* Scaling */
						true,		/* RateOpacity */
						true,		/* Priority */
						true,		/* PositionAnchor */
						true,		/* SizeForce */
						false,		/* UserData (Trigger) *//* Not Supported */
						false,		/* Instance (Trigger) *//* Not Supported */
						false,		/* Effect (Trigger) *//* Not Supported */
						true,		/* Plain.Cell */
						true,		/* Plain.ColorBlend */
						true,		/* Plain.VertexCorrection */
						true,		/* Plain.OffsetPivot */
						true,		/* Plain.PositionTexture */
						true,		/* Plain.ScalingTexture */
						true,		/* Plain.RotationTexture */
						true,		/* Plain.RadiusCollision */
						true,		/* Fix.IndexCellMap */
						true,		/* Fix.Coordinate */
						true,		/* Fix.ColorBlend */
						true,		/* Fix.UV0 */
						true,		/* Fix.SizeCollision */
						true,		/* Fix.PivotCollision */
						true		/* Fix.RadiusCollision */
					);

					public const string ID = "StandardUncompressed";
					#endregion Enums & Constants

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					public class Base<_Type> : Library_SpriteStudio6.Data.Animation.PackAttribute.Container<_Type>
						where _Type : struct
					{
						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						private _Type[] TableValue;
						#endregion Variables & Properties

						/* ----------------------------------------------- Functions */
						#region Functions
						public void CleanUp()
						{
							TableValue = null;
						}

						public Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public bool ValueGet(ref _Type outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							int frame = argument.Frame;
							if((0 > frame) || (TableValue.Length <= frame))
							{
								return(false);
							}

							outValue = TableValue[frame];
							outFrameKey = frame;
							return(true);
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeInt :
						Base<int>,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt>
					{
						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int frameMax,
											string nameAttribute,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							return(false);
						}
					}
					[System.Serializable]
					public class PackAttributeFloat :
						Base<float>,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat>
					{
						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int frameMax,
											string nameAttribute,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							return(false);
						}
					}
					[System.Serializable]
					public class PackAttributeVector2 :
						Base<Vector2>,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat>
					{
						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int frameMax,
											string nameAttribute,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 2 (X, Y) */
							return(false);
						}
					}
					[System.Serializable]
					public class PackAttributeVector3 :
						Base<Vector3>,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat>
					{
						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int frameMax,
											string nameAttribute,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 3 (X, Y, Z) */
							return(false);
						}
					}
					[System.Serializable]
					public class PackAttributeStatus :
						Base<Library_SpriteStudio6.Data.Animation.Attribute.Status>,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool>
					{
						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int frameMax,
											string nameAttribute,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 5 (Hide, FlipX, FlipY, FlipTextureX, FlipTextureY) */
							return(false);
						}
					}
					[System.Serializable]
					public class PackAttributeCell :
						Base<Library_SpriteStudio6.Data.Animation.Attribute.Cell>,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell>
					{
						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int frameMax,
											string nameAttribute,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							return(false);
						}
					}
					[System.Serializable]
					public class PackAttributeColorBlend :
						Base<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend>,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlend>
					{
						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int frameMax,
											string nameAttribute,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlend[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							return(false);
						}
					}
					[System.Serializable]
					public class PackAttributeVertexCorrection :
						Base<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection>,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection>
					{
						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int frameMax,
											string nameAttribute,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							return(false);
						}
					}
					/* MEMO: Not Support */
//					[System.Serializable]
//					public class PackAttributeUserData :
//						Base<Library_SpriteStudio6.Data.Animation.Attribute.UserData>,
//						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerImporter<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUserData>
					/* MEMO: Not Support */
//					[System.Serializable]
//					public class PackAttributeInstance :
//						Base<Library_SpriteStudio6.Data.Animation.Attribute.Instance>,
//						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerImporter<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInstance>
					/* MEMO: Not Support */
//					[System.Serializable]
//					public class PackAttributeEffect :
//						Base<Library_SpriteStudio6.Data.Animation.Attribute.Effect>,
//						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerImporter<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeEffect>
					[System.Serializable]
					public class PackAttributeCoordinateFix :
						Base<Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix>,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCoordinateFix>
					{
						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int frameMax,
											string nameAttribute,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCoordinateFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							return(false);
						}
					}
					[System.Serializable]
					public class PackAttributeColorBlendFix :
						Base<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix>,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlendFix>
					{
						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int frameMax,
											string nameAttribute,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlendFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							return(false);
						}
					}
					[System.Serializable]
					public class PackAttributeUVFix :
						Base<Library_SpriteStudio6.Data.Animation.Attribute.UVFix>,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUVFix>
					{
						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int frameMax,
											string nameAttribute,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUVFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							return(false);
						}
					}
					#endregion Classes, Structs & Interfaces
				}
				#endregion Classes, Structs & Interfaces
			}
		}
	}
}
