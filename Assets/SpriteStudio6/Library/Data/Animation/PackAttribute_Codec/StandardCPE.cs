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
					public readonly static Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer Capacity = new Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer(
						true,	/* Status */
						true,	/* Position */
						true,	/* Rotation */
						true,	/* Scaling */
						true,	/* ScalingLocal */
						true,	/* RateOpacity */
						true,	/* PartsColor */
						true,	/* PositionAnchor */
						true,	/* RadiusCollision */
						true,	/* UserData (Trigger) */
						true,	/* Instance (Trigger) */
						true,	/* Effect (Trigger) */
						true,	/* Plain.Cell */
						true,	/* Plain.SizeForce */
						true,	/* Plain.VertexCorrection */
						true,	/* Plain.OffsetPivot */
						true,	/* Plain.PositionTexture */
						true,	/* Plain.ScalingTexture */
						true,	/* Plain.RotationTexture */
						true,	/* Fix.IndexCellMap */
						true,	/* Fix.Coordinate */
						true,	/* Fix.PartsColor */
						true,	/* Fix.UV0 */
						true,	/* Fix.SizeCollision */
						true	/* Fix.PivotCollision */
					);

					public const string ID = "StandardCPE";

					internal readonly static InterfaceFunctionInt FunctionInt = new InterfaceFunctionInt();
					internal readonly static InterfaceFunctionFloat FunctionFloat = new InterfaceFunctionFloat();
					internal readonly static InterfaceFunctionVector2 FunctionVector2 = new InterfaceFunctionVector2();
					internal readonly static InterfaceFunctionVector3 FunctionVector3 = new InterfaceFunctionVector3();
					internal readonly static InterfaceFunctionStatus FunctionStatus = new InterfaceFunctionStatus();
					internal readonly static InterfaceFunctionCell FunctionCell = new InterfaceFunctionCell();
					internal readonly static InterfaceFunctionPartsColor FunctionPartsColor = new InterfaceFunctionPartsColor();
					internal readonly static InterfaceFunctionVertexCorrection FunctionVertexCorrection = new InterfaceFunctionVertexCorrection();
					internal readonly static InterfaceFunctionUserData FunctionUserData = new InterfaceFunctionUserData();
					internal readonly static InterfaceFunctionInstance FunctionInstance = new InterfaceFunctionInstance();
					internal readonly static InterfaceFunctionEffect FunctionEffect = new InterfaceFunctionEffect();
					internal readonly static InterfaceFunctionCoordinateFix FunctionCoordinateFix = new InterfaceFunctionCoordinateFix();
					internal readonly static InterfaceFunctionPartsColorFix FunctionPartsColorFix = new InterfaceFunctionPartsColorFix();
					internal readonly static InterfaceFunctionUVFix FunctionUVFix = new InterfaceFunctionUVFix();

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
					public class InterfaceFunctionInt : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerInt
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref int outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref int outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionInt(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionFloat : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerFloat
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref float outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref float outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionVector2 : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerVector2
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Vector2 outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Vector2 outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 2 (X, Y) */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionVector3 : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerVector3
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Vector3 outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Vector3 outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 3 (X, Y, Z) */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector3(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionStatus : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerStatus
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Library_SpriteStudio6.Data.Animation.Attribute.Status outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Library_SpriteStudio6.Data.Animation.Attribute.Status outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 5 (Hide, FlipX, FlipY, FlipTextureX, FlipTextureY) */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionStatus(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionCell : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerCell
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Library_SpriteStudio6.Data.Animation.Attribute.Cell outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Library_SpriteStudio6.Data.Animation.Attribute.Cell outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionCell(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionPartsColor : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerPartsColor
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Library_SpriteStudio6.Data.Animation.Attribute.PartsColor outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Library_SpriteStudio6.Data.Animation.Attribute.PartsColor outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributePartsColor[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionPartsColor(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionVertexCorrection : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerVertexCorrection
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVertexCorrection(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionUserData : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerUserData
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Library_SpriteStudio6.Data.Animation.Attribute.UserData outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Library_SpriteStudio6.Data.Animation.Attribute.UserData outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUserData[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							int countKey = listKeyData[0].CountGetKey();
							if(0 >= countKey)
							{
								container.TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];
								container.TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.UserData[0];
								return(true);
							}

							container.TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[1];	/* only 1 type status */
							container.TableCodeValue[0].TableCode = new int[countKey];
							container.TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.UserData[countKey];
							for(int i=0; i<countKey; i++)
							{
								container.TableCodeValue[0].TableCode[i] = (int)FlagBit.INDEX & (i << 15);
								container.TableCodeValue[0].TableCode[i] |= (int)FlagBit.FRAMEKEY & listKeyData[0].ListKey[i].Frame;

								container.TableValue[i] = listKeyData[0].ListKey[i].Value;
							}
							return(true);
						}
						#endregion Functions
					}

					public class InterfaceFunctionInstance : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerInstance
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Library_SpriteStudio6.Data.Animation.Attribute.Instance outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Library_SpriteStudio6.Data.Animation.Attribute.Instance outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInstance[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							int countKey = listKeyData[0].CountGetKey();
							if(0 >= countKey)
							{
								container.TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];
								container.TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Instance[0];
								return(true);
							}

							container.TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[1];	/* only 1 type status */
							container.TableCodeValue[0].TableCode = new int[countKey];
							container.TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Instance[countKey];
							for(int i=0; i<countKey; i++)
							{
								container.TableCodeValue[0].TableCode[i] = (int)FlagBit.INDEX & (i << 15);
								container.TableCodeValue[0].TableCode[i] |= (int)FlagBit.FRAMEKEY & listKeyData[0].ListKey[i].Frame;

								container.TableValue[i] = listKeyData[0].ListKey[i].Value;
							}
							return(true);
						}
						#endregion Functions
					}

					public class InterfaceFunctionEffect : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerEffect
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Library_SpriteStudio6.Data.Animation.Attribute.Effect outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Library_SpriteStudio6.Data.Animation.Attribute.Effect outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeEffect[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							int countKey = listKeyData[0].CountGetKey();
							if(0 >= countKey)
							{
								container.TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];
								container.TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Effect[0];
								return(true);
							}

							container.TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[1];	/* only 1 type status */
							container.TableCodeValue[0].TableCode = new int[countKey];
							container.TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Effect[countKey];
							for(int i=0; i<countKey; i++)
							{
								container.TableCodeValue[0].TableCode[i] = (int)FlagBit.INDEX & (i << 15);
								container.TableCodeValue[0].TableCode[i] |= (int)FlagBit.FRAMEKEY & listKeyData[0].ListKey[i].Frame;

								container.TableValue[i] = listKeyData[0].ListKey[i].Value;
							}
							return(true);
						}
						#endregion Functions
					}

					public class InterfaceFunctionCoordinateFix : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerCoordinateFix
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCoordinateFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionCoordinateFix(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionPartsColorFix : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerPartsColorFix
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Library_SpriteStudio6.Data.Animation.Attribute.PartsColorFix outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColorFix container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Library_SpriteStudio6.Data.Animation.Attribute.PartsColorFix outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColorFix container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColorFix container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColorFix container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributePartsColorFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColorFix dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColorFix();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionPartsColorFix(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionUVFix : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerUVFix
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref Library_SpriteStudio6.Data.Animation.Attribute.UVFix outValue,
												ref int outFrameKey,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref outValue, ref outFrameKey, argument.Frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref Library_SpriteStudio6.Data.Animation.Attribute.UVFix outValue,
													ref int outFrameKey,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref outValue, ref outFrameKey, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix container)
						{
							return(container.TableCodeValue.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUVFix[] listKeyData
										)
						{	/* MEMO: "ListKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionUVFix(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}
					#endregion Classes, Structs & Interfaces

					/* ----------------------------------------------- Functions */
					#region Functions
					public static bool ValueGet<_Type>(	ref _Type outValue,
														ref int outFrameKey,
														int frame,
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
						if(outFrameKey == frameKey)
						{
							return(false);	/* outValue is not overwritten. */
						}
						outFrameKey = frameKey;

						index = (status & (int)FlagBit.INDEX) >> 15;
						outValue = tableValue[index];
						return(true);	/* outValue is overwritten. */
					}

					public static bool ValueGetIndex<_Type>(	ref _Type outValue,
																ref int outFrameKey,
																int index,
																int[] tableStatus,
																_Type[] tableValue
															)
						where _Type : struct
					{
						int status;
						status = tableStatus[index];
						outFrameKey = status & (int)FlagBit.FRAMEKEY;

						index = (status & (int)FlagBit.INDEX) >> 15;
						outValue = tableValue[index];
						return(true);
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
