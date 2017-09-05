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
						true,		/* PositionAnchor */
						true,		/* SizeForce */
						false,		/* UserData (Trigger) *//* Not Supported */
						false,		/* Instance (Trigger) *//* Not Supported */
						false,		/* Effect (Trigger) *//* Not Supported */
						true,		/* RadiusCollision */
						true,		/* Plain.Cell */
						true,		/* Plain.ColorBlend */
						true,		/* Plain.VertexCorrection */
						true,		/* Plain.OffsetPivot */
						true,		/* Plain.PositionTexture */
						true,		/* Plain.ScalingTexture */
						true,		/* Plain.RotationTexture */
						true,		/* Fix.IndexCellMap */
						true,		/* Fix.Coordinate */
						true,		/* Fix.ColorBlend */
						true,		/* Fix.UV0 */
						true,		/* Fix.SizeCollision */
						true		/* Fix.PivotCollision */
					);

					public const string ID = "StandardUncompressed";
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
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public override bool ValueGet(ref int outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.ValueGet(ref outValue, ref outFrameKey, argument.Frame, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 *//* MEMO: No inheritance is related to attribute stored in this type. */
							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];

							if(0 >= listKeyData[0].CountGetKey())
							{
								TableValue = new int[0];
								return(true);
							}

							/* MEMO: Default value when attribute has no key data differs depending on attribute. */
							int valueDefault = 0;
							switch(nameAttribute)
							{
								case Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixIndexCellMap:
									valueDefault = -1;
									break;

								default:
									valueDefault = 0;
									break;
							}

							int value;
							TableValue = new int[countFrame];
							for(int i=0; i<countFrame; i++)
							{
								if(false == listKeyData[0].ValueGet(out value, i))
								{
									value = valueDefault;
								}
								TableValue[i] = value;
							}
							return(true);
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
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public override bool ValueGet(ref float outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.ValueGet(ref outValue, ref outFrameKey, argument.Frame, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];

							/* MEMO: In attributes with the float value, default value when has no key differs only "RateOpacity". */
							/*       RateOpacity = 1.0f / other = 0.0f                                                             */
							float value;
							TableValue = new float[countFrame];
							switch(nameAttribute)
							{
								case Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeRateOpacity:
									/* MEMO: Attribute"RateOpacity" inherits value. */
									for(int i=0; i<countFrame; i++)
									{
										Library_SpriteStudio6.Data.Animation.Attribute.Importer.Inheritance.ValueGetFloatMultiple(out value, listKeyData[0], i, 1.0f);
										TableValue[i] = value;
									}
									break;

								default:
									if(0 >= listKeyData[0].CountGetKey())
									{
										TableValue = new float[0];
										return(true);
									}

									for(int i=0; i<countFrame; i++)
									{
										if(false == listKeyData[0].ValueGet(out value, i))
										{
											value = 0.0f;
										}
										TableValue[i] = value;
									}
									break;
							}
							return(true);
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
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public override bool ValueGet(ref Vector2 outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.ValueGet(ref outValue, ref outFrameKey, argument.Frame, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 2 (X, Y) *//* MEMO: No inheritance is related to attribute stored in this type. */
							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];

							if((0 >= listKeyData[0].CountGetKey()) && (0 >= listKeyData[1].CountGetKey()))
							{
								TableValue = new Vector2[0];
								return(true);
							}

							/* MEMO: For attributes of the scales, default value when has no key is 1.0f. */
							float valueDefault = 0.0f;
							switch(nameAttribute)
							{
								case Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeScaling: 
								case Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainScalingTexture: 
									valueDefault = 1.0f;
									break;

								default:
									valueDefault = 0.0f;
									break;
							}

							float value;
							TableValue = new Vector2[countFrame];
							for(int i=0; i<countFrame; i++)
							{
								if(false == listKeyData[0].ValueGet(out value, i))
								{
									value = valueDefault;
								}
								TableValue[i].x = value;

								if(false == listKeyData[1].ValueGet(out value, i))
								{
									value = valueDefault;
								}
								TableValue[i].y = value;
							}
							return(true);
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
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public override bool ValueGet(ref Vector3 outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.ValueGet(ref outValue, ref outFrameKey, argument.Frame, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 3 (X, Y, Z) *//* MEMO: No inheritance is related to attribute stored in this type. */
							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];

							if((0 >= listKeyData[0].CountGetKey()) && (0 >= listKeyData[1].CountGetKey()) && (0 >= listKeyData[2].CountGetKey()))
							{
								TableValue = new Vector3[0];
								return(true);
							}

							float valueDefault = 0.0f;
							float value;
							TableValue = new Vector3[countFrame];
							for(int i=0; i<countFrame; i++)
							{
								if(false == listKeyData[0].ValueGet(out value, i))
								{
									value = valueDefault;
								}
								TableValue[i].x = value;
								
								if(false == listKeyData[1].ValueGet(out value, i))
								{
									value = valueDefault;
								}
								TableValue[i].y = value;

								if(false == listKeyData[2].ValueGet(out value, i))
								{
									value = valueDefault;
								}
								TableValue[i].z = value;
							}
							return(true);
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
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.Status outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.ValueGet(ref outValue, ref outFrameKey, argument.Frame, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 5 (Hide, FlipX, FlipY, FlipTextureX, FlipTextureY) */
							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];

							/* MEMO: Attribute"Status" is never omitted. */
							TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Status[countFrame];
							bool valueAttribute;
							for(int i=0; i<countFrame; i++)
							{
								TableValue[i].Flags = Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.CLEAR;

								Library_SpriteStudio6.Data.Animation.Attribute.Importer.Inheritance.ValueGetBoolOR(	out valueAttribute,
																													listKeyData[0],
																													i,
																													true
																												);
								TableValue[i].Flags |= (true == valueAttribute) ? Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.HIDE : Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.CLEAR;

								Library_SpriteStudio6.Data.Animation.Attribute.Importer.Inheritance.ValueGetBoolToggle(	out valueAttribute,
																														listKeyData[1],
																														i
																													);
								TableValue[i].Flags |= (true == valueAttribute) ? Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_X : Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.CLEAR;

								Library_SpriteStudio6.Data.Animation.Attribute.Importer.Inheritance.ValueGetBoolToggle(	out valueAttribute,
																														listKeyData[2],
																														i
																													);
								TableValue[i].Flags |= (true == valueAttribute) ? Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_Y : Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.CLEAR;

								Library_SpriteStudio6.Data.Animation.Attribute.Importer.Inheritance.ValueGetBoolToggle(	out valueAttribute,
																														listKeyData[3],
																														i
																													);
								TableValue[i].Flags |= (true == valueAttribute) ? Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_TEXTURE_Y : Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.CLEAR;

								Library_SpriteStudio6.Data.Animation.Attribute.Importer.Inheritance.ValueGetBoolToggle(	out valueAttribute,
																														listKeyData[4],
																														i
																													);
								TableValue[i].Flags |= (true == valueAttribute) ? Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_TEXTURE_Y : Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.CLEAR;

								TableValue[i].Flags |= (null != tableOrderDraw)
														? (Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit)tableOrderDraw[i] & Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.PARTS_ID_NEXT
														: Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.PARTS_ID_NEXT;	/* -1 */
							}
							return(true);
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
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.Cell outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.ValueGet(ref outValue, ref outFrameKey, argument.Frame, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 *//* MEMO: No inheritance is related to attribute stored in this type. */
							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];

							if(0 >= listKeyData[0].CountGetKey())
							{
								TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Cell[0];
								return(true);
							}

							TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Cell[countFrame];
							for(int i=0; i<countFrame; i++)
							{
								listKeyData[0].ValueGet(out TableValue[i], i);
							}
							return(true);
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
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.ValueGet(ref outValue, ref outFrameKey, argument.Frame, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlend[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 *//* MEMO: No inheritance is related to attribute stored in this type. */
							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];

							if(0 >= listKeyData[0].CountGetKey())
							{
								TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend[0];
								return(true);
							}

							TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend[countFrame];
							for(int i=0; i<countFrame; i++)
							{
								listKeyData[0].ValueGet(out TableValue[i], i);
							}
							return(true);
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
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.ValueGet(ref outValue, ref outFrameKey, argument.Frame, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 *//* MEMO: No inheritance is related to attribute stored in this type. */
							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];

							if(0 >= listKeyData[0].CountGetKey())
							{
								TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection[0];
								return(true);
							}

							TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection[countFrame];
							for(int i=0; i<countFrame; i++)
							{
								listKeyData[0].ValueGet(out TableValue[i], i);
							}
							return(true);
						}
						#endregion Functions
					}

					/* MEMO: Not Support */
//					[System.Serializable]
//					public class PackAttributeUserData :
//						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData,
//						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerUserData

					/* MEMO: Not Support */
//					[System.Serializable]
//					public class PackAttributeInstance :
//						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance,
//						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerInstance

					/* MEMO: Not Support */
//					[System.Serializable]
//					public class PackAttributeEffect :
//						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect,
//						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerEffect

					[System.Serializable]
					public class PackAttributeCoordinateFix :
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix,
						Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerCoordinateFix
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.ValueGet(ref outValue, ref outFrameKey, argument.Frame, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCoordinateFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 *//* MEMO: No inheritance is related to attribute stored in this type. */
							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];

							if(0 >= listKeyData[0].CountGetKey())
							{
								TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix[0];
								return(true);
							}

							TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix[countFrame];
							for(int i=0; i<countFrame; i++)
							{
								listKeyData[0].ValueGet(out TableValue[i], i);
							}
							return(true);
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
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.ValueGet(ref outValue, ref outFrameKey, argument.Frame, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlendFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 *//* MEMO: No inheritance is related to attribute stored in this type. */
							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];

							if(0 >= listKeyData[0].CountGetKey())
							{
								TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix[0];
								return(true);
							}

							TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix[countFrame];
							for(int i=0; i<countFrame; i++)
							{
								listKeyData[0].ValueGet(out TableValue[i], i);
							}
							return(true);
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
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED);
						}

						public override bool ValueGet(ref Library_SpriteStudio6.Data.Animation.Attribute.UVFix outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.ValueGet(ref outValue, ref outFrameKey, argument.Frame, TableValue));
						}

						public bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
											int countFrame,
											string nameAttribute,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUVFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 *//* MEMO: No inheritance is related to attribute stored in this type. */
							TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];

							if(0 >= listKeyData[0].CountGetKey())
							{
								TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.UVFix[0];
								return(true);
							}

							TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.UVFix[countFrame];
							for(int i=0; i<countFrame; i++)
							{
								listKeyData[0].ValueGet(out TableValue[i], i);
							}
							return(true);
						}
						#endregion Functions
					}
					#endregion Classes, Structs & Interfaces

					/* ----------------------------------------------- Functions */
					#region Functions
					public static bool ValueGet<_Type>(	ref _Type outValue,
														ref int outFrameKey,
														int frame,
														_Type[] tableValue
													)
						where _Type : struct
					{
						if((0 > frame) || (tableValue.Length <= frame))
						{
							return(false);
						}

						outValue = tableValue[frame];
						outFrameKey = frame;
						return(true);
					}
					#endregion Functions
				}
				#endregion Classes, Structs & Interfaces
			}
		}
	}
}
