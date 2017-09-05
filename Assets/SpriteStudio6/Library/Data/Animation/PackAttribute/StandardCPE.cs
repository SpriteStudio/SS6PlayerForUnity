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
				public static class StandardCPE
				{
					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					public static readonly Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer Capacity = new Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer(
						true,	/* Status */
						true,	/* Position */
						true,	/* Rotation */
						true,	/* Scaling */
						true,	/* RateOpacity */
						true,	/* PositionAnchor */
						true,	/* SizeForce */
						true,	/* UserData (Trigger) */
						true,	/* Instance (Trigger) */
						true,	/* Effect (Trigger) */
						true,	/* RadiusCollision */
						true,	/* Plain.Cell */
						true,	/* Plain.ColorBlend */
						true,	/* Plain.VertexCorrection */
						true,	/* Plain.OffsetPivot */
						true,	/* Plain.PositionTexture */
						true,	/* Plain.ScalingTexture */
						true,	/* Plain.RotationTexture */
						true,	/* Fix.IndexCellMap */
						true,	/* Fix.Coordinate */
						true,	/* Fix.ColorBlend */
						true,	/* Fix.UV0 */
						true,	/* Fix.SizeCollision */
						true	/* Fix.PivotCollision */
					);

					public const string ID = "StandardCPE";

					[System.Flags]
					public enum FlagBit
					{
						FRAMEKEY = 0x00007fff,
						INDEX = 0x3fff8000,

						CLEAR = 0x00000000,
					}
					#endregion Enums & Constants

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					[System.Serializable]
					public class PackAttributeInt :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerInt
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref int outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeInt dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeInt();
							dataUncompressed.Pack(ref argument, countFrame, nameAttribute, flagStatusParts, tableOrderDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out TableCodeValue, out TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeFloat :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerFloat
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref float outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeFloat dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeFloat();
							dataUncompressed.Pack(ref argument, countFrame, nameAttribute, flagStatusParts, tableOrderDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out TableCodeValue, out TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeVector2 :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerFloat
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Vector2 outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 2 (X, Y) */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVector2 dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVector2();
							dataUncompressed.Pack(ref argument, countFrame, nameAttribute, flagStatusParts, tableOrderDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out TableCodeValue, out TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeVector3 :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerFloat
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Vector3 outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 3 (X, Y, Z) */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVector3 dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVector3();
							dataUncompressed.Pack(ref argument, countFrame, nameAttribute, flagStatusParts, tableOrderDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out TableCodeValue, out TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeStatus :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerBool
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.Status outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 5 (Hide, FlipX, FlipY, FlipTextureX, FlipTextureY) */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeStatus dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeStatus();
							dataUncompressed.Pack(ref argument, countFrame, nameAttribute, flagStatusParts, tableOrderDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out TableCodeValue, out TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeCell :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerCell
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.Cell outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeCell dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeCell();
							dataUncompressed.Pack(ref argument, countFrame, nameAttribute, flagStatusParts, tableOrderDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out TableCodeValue, out TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeColorBlend :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerColorBlend,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerColorBlend
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlend[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeColorBlend dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeColorBlend();
							dataUncompressed.Pack(ref argument, countFrame, nameAttribute, flagStatusParts, tableOrderDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out TableCodeValue, out TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeVertexCorrection :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerVertexCorrection
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVertexCorrection dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVertexCorrection();
							dataUncompressed.Pack(ref argument, countFrame, nameAttribute, flagStatusParts, tableOrderDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out TableCodeValue, out TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeUserData :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerUserData
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.UserData outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUserData[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							int countKey = listKeyData[0].CountGetKey();
							if(0 >= countKey)
							{
								TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];
								TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.UserData[0];
								return(true);
							}

							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[1];	/* only 1 type status */
							TableCodeValue[0].TableCode = new int[countKey];
							TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.UserData[countKey];
							for(int i=0; i<countKey; i++)
							{
								TableCodeValue[0].TableCode[i] = (int)FlagBit.INDEX & (i << 15);
								TableCodeValue[0].TableCode[i] |= (int)FlagBit.FRAMEKEY & listKeyData[0].ListKey[i].Frame;

								TableValue[i] = listKeyData[0].ListKey[i].Value;
							}
							return(true);
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeInstance :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerInstance
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.Instance outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInstance[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							int countKey = listKeyData[0].CountGetKey();
							if(0 >= countKey)
							{
								TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];
								TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Instance[0];
								return(true);
							}

							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[1];	/* only 1 type status */
							TableCodeValue[0].TableCode = new int[countKey];
							TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Instance[countKey];
							for(int i=0; i<countKey; i++)
							{
								TableCodeValue[0].TableCode[i] = (int)FlagBit.INDEX & (i << 15);
								TableCodeValue[0].TableCode[i] |= (int)FlagBit.FRAMEKEY & listKeyData[0].ListKey[i].Frame;

								TableValue[i] = listKeyData[0].ListKey[i].Value;
							}
							return(true);
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeEffect :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerEffect
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.Effect outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeEffect[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							int countKey = listKeyData[0].CountGetKey();
							if(0 >= countKey)
							{
								TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];
								TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Effect[0];
								return(true);
							}

							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[1];	/* only 1 type status */
							TableCodeValue[0].TableCode = new int[countKey];
							TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Effect[countKey];
							for(int i=0; i<countKey; i++)
							{
								TableCodeValue[0].TableCode[i] = (int)FlagBit.INDEX & (i << 15);
								TableCodeValue[0].TableCode[i] |= (int)FlagBit.FRAMEKEY & listKeyData[0].ListKey[i].Frame;

								TableValue[i] = listKeyData[0].ListKey[i].Value;
							}
							return(true);
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeCoordinateFix :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerCoordinateFix
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCoordinateFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeCoordinateFix dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeCoordinateFix();
							dataUncompressed.Pack(ref argument, countFrame, nameAttribute, flagStatusParts, tableOrderDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out TableCodeValue, out TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeColorBlendFix :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerColorBlendFix,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerColorBlendFix
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlendFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeColorBlendFix dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeColorBlendFix();
							dataUncompressed.Pack(ref argument, countFrame, nameAttribute, flagStatusParts, tableOrderDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out TableCodeValue, out TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeUVFix :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerUVFix
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.UVFix outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, framePrevious, TableCodeValue[0].TableCode, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUVFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeUVFix dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeUVFix();
							dataUncompressed.Pack(ref argument, countFrame, nameAttribute, flagStatusParts, tableOrderDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out TableCodeValue, out TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}
					#endregion Classes, Structs & Interfaces

					/* ----------------------------------------------- Functions */
					#region Functions
					public static bool ValueGet<_Type>(	ref _Type outValue,
														ref int outFrameKey,
														int frame,
														int framePrevious,
														int[] tableStatus,
														_Type[] tableValue
													)
						where _Type : struct
					{
						int frameKey = -1;

						int status;
						int indexMinimum = 0;
						int indexMaximum = tableStatus.Length - 1;
						int index;
						while(indexMinimum != indexMaximum)
						{
							index = indexMinimum + indexMaximum;
							index = (index >> 1) + (index & 1);	/* (index / 2) + (index % 2) */
							frameKey = tableStatus[index] & (int)FlagBit.FRAMEKEY;
							if(frame == frameKey)
							{
								indexMinimum = indexMaximum = index;
							}
							else
							{
								if((frame < frameKey) || (-1 == frameKey))
								{
									indexMaximum = index - 1;
								}
								else
								{
									indexMinimum = index;
								}
							}
						}

						status = tableStatus[indexMinimum];
						frameKey = status & (int)FlagBit.FRAMEKEY;
						outFrameKey = frameKey;
						if(framePrevious == frameKey)
						{
							return(false);	/* outValue is not overwritten. */
						}

						index = (status & (int)FlagBit.INDEX) >> 15;
						outValue = tableValue[index];
						return(true);	/* outValue is overwritten. */
					}

					public static bool Compress<_Type>(out Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[] tableCodeValue, out _Type[] tableValue, _Type[] tableValueUncompressed)
					{
						int countFrame = tableValueUncompressed.Length;
						if(0 >= countFrame)
						{
							tableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];
							tableValue = new _Type[0];
							return(true);
						}

						tableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[1];	/* only 1 type status */
						List<int> listStatus = new List<int>(countFrame);
						listStatus.Clear();
						List<_Type> listValue = new List<_Type>(countFrame);
						listValue.Clear();

						int index;
						int status = 0;	/* FRAMEKEY=0, INDEX=0 */
						_Type valuePrevious = tableValueUncompressed[0];
						listStatus.Add(status);
						listValue.Add(valuePrevious);
						for(int i=1; i<countFrame; i++)
						{
							if(false == tableValueUncompressed[i].Equals(valuePrevious))
							{
								valuePrevious = tableValueUncompressed[i];

								index = -1;
								int countValue = listValue.Count;
								for(int j=0; j<countValue; j++)
								{
									if(true == listValue[j].Equals(valuePrevious))
									{
										index = j;
										break;	/* Break for j-Loop */
									}
								}
								if(0 > index)
								{	/* New */
									status = (int)FlagBit.INDEX & (countValue << 15);
									status |= (int)FlagBit.FRAMEKEY & i;
									listStatus.Add(status);

									listValue.Add(valuePrevious);
								}
								else
								{	/* Exist */
									status = (int)FlagBit.INDEX & (index << 15);
									status |= (int)FlagBit.FRAMEKEY & i;
									listStatus.Add(status);
								}
							}
						}

						tableCodeValue[0].TableCode = listStatus.ToArray();
						listStatus.Clear();
						listStatus = null;

						tableValue = listValue.ToArray();
						listValue.Clear();
						listValue = null;

						return(true);

//					Compress_ErrorEnd:;
//						return(false);
					}
					#endregion Functions
				}
				#endregion Classes, Structs & Interfaces
			}
		}
	}
}
