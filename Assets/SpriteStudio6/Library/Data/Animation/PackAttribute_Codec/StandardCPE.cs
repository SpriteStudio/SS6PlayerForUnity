/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

#define REDUCE_FREQUENCY_BINARYTREE
#define DEFORM_CALCULATE_STRICT

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
						true,	/* Cell */
						true,	/* Position */
						true,	/* Rotation */
						true,	/* Scaling */
						true,	/* ScalingLocal */
						true,	/* RateOpacity */
						true,	/* Priority */
						true,	/* PartsColor */
						true,	/* VertexCorrection */
						true,	/* OffsetPivot */
						true,	/* PositionAnchor */
						true,	/* SizeForce */
						true,	/* PositionTexture */
						true,	/* RotationTexture */
						true,	/* ScalingTexture */
						true,	/* RadiusCollision */
						true,	/* UserData (Trigger) */
						true,	/* Instance (Trigger) */
						true,	/* Effect (Trigger) */
						true,	/* Deform */
						true,	/* Shader */
						true	/* Signal */
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
					internal readonly static InterfaceFunctionDeform FunctionDeform = new InterfaceFunctionDeform();
					internal readonly static InterfaceFunctionShader FunctionShader = new InterfaceFunctionShader();
					internal readonly static InterfaceFunctionSignal FunctionSignal = new InterfaceFunctionSignal();

					/* MEMO: Originally "private", but other decoders need these values, so set to "public". */
					[System.Flags]
					public enum FlagBit
					{
						FRAMEKEY = 0x00007fff,
						INDEX = 0x3fff8000,

						CLEAR = 0x00000000,
					}
					#endregion Enums & Constants

					/* ----------------------------------------------- Dictionary-Functions */
					#region Dictionary-Functions
					public static bool DictionaryBootUp(int indexAnimation, int indexParts, Script_SpriteStudio6_DataAnimation dataAnimation)
					{
						/* MEMO: Have no external dictionary. */
						return(true);
					}

					public static bool DictionaryShutDown(int indexAnimation, int indexParts, Script_SpriteStudio6_DataAnimation dataAnimation)
					{
						/* MEMO: Have no external dictionary. */
						return(true);
					}
					#endregion Dictionary-Functions

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					public class InterfaceFunctionInt : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerInt
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref CacheDecode<int> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<int> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 1 */
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
						public bool ValueGet(	ref CacheDecode<float> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<float> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 1 */
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
						public bool ValueGet(	ref CacheDecode<Vector2> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<Vector2> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 2 (X, Y) */
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
						public bool ValueGet(	ref CacheDecode<Vector3> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<Vector3> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 3 (X, Y, Z) */
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
						public bool ValueGet(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Status> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Status> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 5 (Hide, FlipX, FlipY, FlipTextureX, FlipTextureY) */
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
						public bool ValueGet(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Cell> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Cell> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 1 */
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
						public bool ValueGet(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.PartsColor> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.PartsColor> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributePartsColor[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 1 */
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
						public bool ValueGet(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 1 */
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
						public bool ValueGet(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.UserData> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.UserData> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUserData[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 1 */
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
						public bool ValueGet(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Instance> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Instance> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInstance[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 1 */
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
						public bool ValueGet(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Effect> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Effect> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeEffect[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 1 */
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

					public class InterfaceFunctionDeform : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerDeform
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Deform> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerDeform container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int countVertexMesh = container.CountVertexMesh;
							if(0 >= countVertexMesh)
							{
								return(false);	/* outValue is not overwritten. */
							}
							/* MEMO: Must be "outValue.TableCoordinate.Length == container.CountVertexMesh". */
							if((null == cacheDecode.Value.TableCoordinate) || (countVertexMesh > cacheDecode.Value.TableCoordinate.Length))
							{	/* Error */
								return(false);	/* outValue is not overwritten. */
							}

							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetDeform(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue, container));
						}

						public bool ValueGetIndex(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Deform> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerDeform container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int countVertexMesh = container.CountVertexMesh;
							if(0 >= countVertexMesh)
							{
								return(false);	/* outValue is not overwritten. */
							}
							/* MEMO: Must be "outValue.TableCoordinate.Length == container.CountVertexMesh". */
							if((null == cacheDecode.Value.TableCoordinate) || (countVertexMesh > cacheDecode.Value.TableCoordinate.Length))
							{	/* Error */
								return(false);	/* outValue is not overwritten. */
							}

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndexDeform(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue, container));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerDeform container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerDeform container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeDeform[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerDeform dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerDeform();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionDeform(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							/* MEMO: Copy data except for each frame data. */
							container.CountVertexMesh = dataUncompressed.CountVertexMesh;
							container.TableIndexVertex = dataUncompressed.TableIndexVertex;

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionShader : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerShader
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Shader> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerShader container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Shader> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerShader container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerShader container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerShader container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeShader[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 1 */
							/* MEMO: Get values that have undergone dedicated processing and inheriting for each attribute. */
							Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerShader dataUncompressed = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerShader();
							dataUncompressed.TypePack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_UNCOMPRESSED;
							Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionShader(dataUncompressed);
							dataUncompressed.Function.Pack(dataUncompressed, nameAttribute, countFrame, flagStatusParts, tableOrderDraw, tableOrderPreDraw, listKeyData);

							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.Compress(out container.TableCodeValue, out container.TableValue, dataUncompressed.TableValue));
						}
						#endregion Functions
					}

					public class InterfaceFunctionSignal : Library_SpriteStudio6.Data.Animation.PackAttribute.InterfaceContainerSignal
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public bool ValueGet(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Signal> cacheDecode,
												Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerSignal container,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
											)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							int frame = argument.Frame;
							if((0 <= cacheDecode.FrameKey) && (frame == argument.FramePrevious))
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGet(ref cacheDecode, frame, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public bool ValueGetIndex(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Signal> cacheDecode,
													int index,
													Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerSignal container,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
												)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(false);
							}
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.ValueGetIndex(ref cacheDecode, index, container.TableCodeValue[0].TableCode, container.TableValue));
						}

						public int CountGetValue(Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerSignal container)
						{
							if(0 >= container.TableCodeValue.Length)
							{
								return(0);
							}
							return(container.TableCodeValue[0].TableCode.Length);
						}

						public bool Pack(	Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerSignal container,
											string nameAttribute,
											int countFrame,
											Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
											int[] tableOrderDraw,
											int[] tableOrderPreDraw,
											params Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeSignal[] listKeyData
										)
						{	/* MEMO: "listKeyData.Length" is always 1 */
							int countKey = listKeyData[0].CountGetKey();
							if(0 >= countKey)
							{
								container.TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[0];
								container.TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Signal[0];
								return(true);
							}

							container.TableCodeValue = new Library_SpriteStudio6.Data.Animation.PackAttribute.CodeValueContainer[1];	/* only 1 type status */
							container.TableCodeValue[0].TableCode = new int[countKey];
							container.TableValue = new Library_SpriteStudio6.Data.Animation.Attribute.Signal[countKey];
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
					#endregion Classes, Structs & Interfaces

					/* ----------------------------------------------- Functions */
					#region Functions
					public static bool ValueGet<_Type>(	ref CacheDecode<_Type> cacheDecode,
														int frame,
														int[] tableStatus,
														_Type[] tableValue
													)
						where _Type : struct
					{
#if UNITY_EDITOR
						if((null == tableValue) || (null == tableStatus))
						{
							/* MEMO: May reach before deserialization direct-after import. */
							return(false);
						}
#endif

#if REDUCE_FREQUENCY_BINARYTREE
						int frameKey;
						int index = CodeValueContainer.IndexGetBinaryTree(cacheDecode.IndexKey, frame, (int)FlagBit.FRAMEKEY, tableStatus);
						cacheDecode.IndexKey = index;

						/* Get Key-data */
						int status = tableStatus[index];
						frameKey = status & (int)FlagBit.FRAMEKEY;
						if(cacheDecode.FrameKey == frameKey)
						{
							return(false);	/* outValue is not overwritten. */
						}
						cacheDecode.FrameKey = frameKey;

						index = (status & (int)FlagBit.INDEX) >> 15;

						/* Get values */
						cacheDecode.Value = tableValue[index];

						return(true);	/* outValue is overwritten. */
#else
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
						if(cacheDecode.FrameKey == frameKey)
						{
							return(false);	/* outValue is not overwritten. */
						}
						cacheDecode.FrameKey = frameKey;

						index = (status & (int)FlagBit.INDEX) >> 15;
						cacheDecode.Value = tableValue[index];

						return(true);	/* outValue is overwritten. */
#endif
					}

					public static bool ValueGetDeform(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Deform> cacheDecode,
														int frame,
														int[] tableStatus,
														Library_SpriteStudio6.Data.Animation.Attribute.Deform[] tableValue,
														Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerDeform container
													)
					{
#if UNITY_EDITOR
						if((null == tableValue) || (null == tableStatus))
						{
							/* MEMO: May reach before deserialization direct-after import. */
							return(false);
						}
#endif

#if REDUCE_FREQUENCY_BINARYTREE
						int frameKey;
						int index = CodeValueContainer.IndexGetBinaryTree(cacheDecode.IndexKey, frame, (int)FlagBit.FRAMEKEY, tableStatus);
						cacheDecode.IndexKey = index;

						/* Get Key-data */
						int status = tableStatus[index];
						frameKey = status & (int)FlagBit.FRAMEKEY;
						if(cacheDecode.FrameKey == frameKey)
						{
							return(false);	/* outValue is not overwritten. */
						}
						cacheDecode.FrameKey = frameKey;

						index = (status & (int)FlagBit.INDEX) >> 15;

#if DEFORM_CALCULATE_STRICT
						/* Clean up Coordinate-Cache */
						cacheDecode.Value.CoordinateReset();
#endif

						/* Get values */
						int countVertexChange = container.TableIndexVertex.Length;
						int[] tableIndexVertex = container.TableIndexVertex;
						Vector2[] tableCoordinate = tableValue[index].TableCoordinate;
						Vector2[] tableCoordinateOutput = cacheDecode.Value.TableCoordinate;
						for(int i=0; i<countVertexChange; i++)
						{
							tableCoordinateOutput[tableIndexVertex[i]] = tableCoordinate[i];
						}

						return(true);	/* outValue is overwritten. */
#else
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
						if(cacheDecode.FrameKey == frameKey)
						{
							return(false);	/* outValue is not overwritten. */
						}
						cacheDecode.FrameKey = frameKey;
						index = (status & (int)FlagBit.INDEX) >> 15;

						int countVertexChange = container.TableIndexVertex.Length;
						int[] tableIndexVertex = container.TableIndexVertex;
						Vector2[] tableCoordinate = tableValue[index].TableCoordinate;
						Vector2[] tableCoordinateOutput = cacheDecode.Value.TableCoordinate;
						for(int i=0; i<countVertexChange; i++)
						{
							tableCoordinateOutput[tableIndexVertex[i]] = tableCoordinate[i];
						}

						return(true);	/* outValue is overwritten. */
#endif
					}

					public static bool ValueGetIndex<_Type>(	ref CacheDecode<_Type> cacheDecode,
																int index,
																int[] tableStatus,
																_Type[] tableValue
															)
						where _Type : struct
					{
						int status;
						status = tableStatus[index];
						cacheDecode.FrameKey = status & (int)FlagBit.FRAMEKEY;

						index = (status & (int)FlagBit.INDEX) >> 15;
						cacheDecode.Value = tableValue[index];
						return(true);
					}

					public static bool ValueGetIndexDeform(	ref CacheDecode<Library_SpriteStudio6.Data.Animation.Attribute.Deform> cacheDecode,
															int index,
															int[] tableStatus,
															Library_SpriteStudio6.Data.Animation.Attribute.Deform[] tableValue,
															Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerDeform container
														)
					{
						int status;
						status = tableStatus[index];
						cacheDecode.FrameKey = status & (int)FlagBit.FRAMEKEY;
						index = (status & (int)FlagBit.INDEX) >> 15;

#if DEFORM_CALCULATE_STRICT
						/* Clean up Coordinate-Cache */
						cacheDecode.Value.CoordinateReset();
#endif

						/* Get values */
						int countVertexChange = container.TableIndexVertex.Length;
						int[] tableIndexVertex = container.TableIndexVertex;
						Vector2[] tableCoordinate = tableValue[index].TableCoordinate;
						Vector2[] tableCoordinateOutput = cacheDecode.Value.TableCoordinate;
						for(int i=0; i<countVertexChange; i++)
						{
							tableCoordinateOutput[tableIndexVertex[i]] = tableCoordinate[i];
						}

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
										break;	/* j-Loop */
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
