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
	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindOperationBlend
	{
		NON = -1,

		MIX = 0,
		ADD,
		SUB,
		MUL,

		TERMINATOR,
	}

	public enum KindOperationBlendEffect
	{
		NON = -1,

		MIX = 0,
		ADD,
		ADD2,

		TERMINATOR,
		TERMINATOR_KIND = ADD,
	}

	public enum KindBoundBlend
	{
		NON = 0,
		OVERALL,
		VERTEX
	}

	public enum KindVertex
	{
		LU = 0,	/* Left-Up (TRIANGLE2 & TRIANGLE4) */
		RU,	/* Right-Up (TRIANGLE2 & TRIANGLE4) */
		RD,	/* Right-Down (TRIANGLE2 & TRIANGLE4) */
		LD,	/* Left-Down (TRIANGLE2 & TRIANGLE4) */
		C,	/* Center (TRIANGLE4) */

		TERMINATOR,
		TERMINATOR4 = TERMINATOR,
		TERMINATOR2 = C
	}

	public enum KindStylePlay
	{
		NO_CHANGE = -1,
		NORMAL = 0,
		PINGPONG = 1,
	}
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	public static partial class CallBack
	{
		/* ----------------------------------------------- Delegates */
		#region Delegates
		public delegate bool FunctionPlayEnd(Script_SpriteStudio6_Root scriptRoot, GameObject objectControl);
		public delegate bool FunctionPlayEndEffect(Script_SpriteStudio6_RootEffect scriptRoot);
		public delegate void FunctionUserData(Script_SpriteStudio6_Root scriptRoot, string nameParts, int indexParts, int indexAnimation, int frameDecode, int frameKeyData, ref Library_SpriteStudio6.Data.Animation.Attribute.UserData userData, bool flagWayBack);

		public delegate float FunctionTimeElapse(Script_SpriteStudio6_Root scriptRoot);
		public delegate float FunctionTimeElapseEffect(Script_SpriteStudio6_RootEffect scriptRoot);

		public delegate bool FunctionControlEndTrackPlay(Script_SpriteStudio6_Root scriptRoot, int indexTrackPlay);
		#endregion Delegates
	}

	public static partial class Data
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		[System.Serializable]
		public partial class Animation
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public string Name;
			public int FramePerSecond;
			public int CountFrame;

			public Label[] TableLabel;
			public Parts[] TableParts;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public void CleanUp()
			{
				Name = "";
				FramePerSecond = 0;
				CountFrame = 0;

				TableLabel = null;
				TableParts = null;
			}

			public int CountGetLabel()
			{
				return((null == TableLabel) ? 0 : TableLabel.Length);
			}

			public int IndexGetLabel(string name)
			{
				if((true == string.IsNullOrEmpty(name)) || (null == TableLabel))
				{
					return(-1);
				}

				int count;
				count = (int)Label.KindLabelReserved.TERMINATOR;
				for(int i=0; i<count; i++)
				{
					if(name == Label.TableNameLabelReserved[i])
					{
						return((int)Label.KindLabelReserved.INDEX_RESERVED + i);
					}
				}

				count = TableLabel.Length;
				for(int i=0; i<count; i++)
				{
					if(name == TableLabel[i].Name)
					{
						return(i);
					}
				}
				return(-1);
			}

			public int FrameGetLabel(int index)
			{
				if((int)Label.KindLabelReserved.INDEX_RESERVED <= index)
				{	/* Reserved-Index */
					index -= (int)Label.KindLabelReserved.INDEX_RESERVED;
					switch(index)
					{
						case (int)Label.KindLabelReserved.START:
							return(0);

						case (int)Label.KindLabelReserved.END:
							return(CountFrame - 1);

						default:
							break;
					}
					return(-1);
				}

				if((0 > index) || (TableLabel.Length <= index))
				{
					return(-1);
				}
				return(TableLabel[index].Frame);
			}

			public string NameGetLabel(int index)
			{
				if((int)Label.KindLabelReserved.INDEX_RESERVED <= index)
				{	/* Reserved-Index */
					index -= (int)Label.KindLabelReserved.INDEX_RESERVED;
					if((0 > index) || ((int)Label.KindLabelReserved.TERMINATOR <= index))
					{	/* Error */
						return(null);
					}
					return(Label.TableNameLabelReserved[index]);
				}

				if((0 > index) || (TableLabel.Length <= index))
				{
					return(null);
				}
				return(TableLabel[index].Name);
			}

			public int CountGetParts()
			{
				return((null == TableParts) ? 0 : TableParts.Length);
			}

			public int IndexGetParts(int indexParts)
			{
				return(((0 > indexParts) || (TableParts.Length <= indexParts)) ? -1 : indexParts);
			}

			public void FrameRangeGet(	out int frameRangeStart, out int frameRangeEnd,
										string labelStart, int frameOffsetStart,
										string labelEnd, int frameOffsetEnd
									)
			{
				int frameEnd = CountFrame - 1;
				string label;
				int indexLabel;

				/* Get Start frame */
				label = (true == string.IsNullOrEmpty(labelStart))
						? Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START]
						: labelStart;
				indexLabel = IndexGetLabel(label);
				if(0 > indexLabel)
				{	/* Not found */
					indexLabel = (int)(Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START | Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.INDEX_RESERVED);
				}

				frameRangeStart = FrameGetLabel(indexLabel);
				if(0 > frameRangeStart)
				{
					frameRangeStart = 0;
				}
				frameRangeStart += frameOffsetStart;
				if((0 > frameRangeStart) || (frameEnd < frameRangeStart))
				{
					frameRangeStart = 0;
				}

				/* Get End frame */
				label = (true == string.IsNullOrEmpty(labelEnd))
						? Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END]
						: labelEnd;
				indexLabel = IndexGetLabel(label);
				if(0 > indexLabel)
				{	/* Not found */
					indexLabel = (int)(Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END | Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.INDEX_RESERVED);
				}

				frameRangeEnd = FrameGetLabel(indexLabel);
				if(0 > frameRangeEnd)
				{
					frameRangeEnd = frameEnd;
				}
				frameRangeEnd += frameOffsetEnd;
				if((0 > frameRangeEnd) || (frameEnd < frameRangeEnd))
				{
					frameRangeEnd = frameEnd;
				}
			}
			#endregion Functions

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces

			[System.Serializable]
			public struct Parts
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public KindFormat Format;
				public FlagBitStatus StatusParts;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus Status;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 Position;	/* Always Compressed */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 Rotation;	/* Always Compressed */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 Scaling;	/* Always Compressed */

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat RateOpacity;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 PositionAnchor;	/* Reserved */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 SizeForce;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData UserData;	/* Trigger (Always Compressed) */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance Instance;	/* Trigger (Always Compressed) */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect Effect;	/* Trigger (Always Compressed) */

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat RadiusCollision;	/* for Sphere-Collider *//* Always Compressed */

				public AttributeGroupPlain Plain;
				public AttributeGroupFix Fix;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Format = KindFormat.PLAIN;
					StatusParts = FlagBitStatus.CLEAR;

					Status = null;

					Position = null;
					Rotation = null;
					Scaling = null;

					RateOpacity = null;

					PositionAnchor = null;
					SizeForce = null;

					UserData = null;
					Instance = null;
					Effect = null;

					RadiusCollision = null;

					Plain.Cell = null;
					Plain.ColorBlend = null;
					Plain.VertexCorrection = null;
					Plain.OffsetPivot = null;
					Plain.PositionTexture = null;
					Plain.ScalingTexture = null;
					Plain.RotationTexture = null;

					Fix.IndexCellMap = null;
					Fix.Coordinate = null;
					Fix.ColorBlend = null;
					Fix.UV0 = null;
					Fix.SizeCollision = null;
					Fix.PivotCollision = null;
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindFormat
				{	/* ERROR/NON: -1 */
					PLAIN = 0,	/* Data-Format: Plain-Data */
					FIX,	/* Data-Format: Precalculated "Fix Mesh" */
				}

				[System.Flags]
				public enum FlagBitStatus
				{
					VALID = 0x40000000,
					NOT_USED = 0x20000000,
					HIDE_FULL = 0x10000000,

					NO_POSITION = 0x08000000,
					NO_ROTATION = 0x04000000,
					NO_SCALING = 0x02000000,

					CLEAR = 0x00000000
				}
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				[System.Serializable]
				public struct AttributeGroupPlain
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell Cell;	/* Always Compressed */

					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerColorBlend ColorBlend;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection VertexCorrection;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 OffsetPivot;

					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 PositionTexture;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 ScalingTexture;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat RotationTexture;
					#endregion Variables & Properties
				}

				[System.Serializable]
				public struct AttributeGroupFix
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt IndexCellMap;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix Coordinate;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerColorBlendFix ColorBlend;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix UV0;

					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 SizeCollision;	/* for Box-Collider *//* Always Compressed */
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 PivotCollision;	/* for Box-Collider *//* Always Compressed */
					#endregion Variables & Properties
				}
				#endregion Classes, Structs & Interfaces
			}

			[System.Serializable]
			public struct Label
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public string Name;
				public int Frame;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Name = "";
					Frame = -1;
				}

				public static int NameCheckReserved(string name)
				{
					if(false == string.IsNullOrEmpty(name))
					{
						for(int i=0; i<(int)KindLabelReserved.TERMINATOR; i++)
						{
							if(name == TableNameLabelReserved[i])
							{
								return(i);
							}
						}
					}
					return(-1);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindLabelReserved
				{
					START = 0,	/* "_start" *//* START + INDEX_RESERVED */
					END,	/* "_end" *//* END + INDEX_RESERVED */

					TERMINATOR,
					INDEX_RESERVED = 0x10000000,
				}

				public readonly static string[] TableNameLabelReserved = new string[(int)KindLabelReserved.TERMINATOR]
				{
					"_start",
					"_end",
				};
				#endregion Enums & Constants
			}

			public static partial class Attribute
			{
				/* Part: SpriteStudio6/Library/Data/Animation/Attribute.cs */
			}

			public static partial class PackAttribute
			{
				/* ----------------------------------------------- Functions */
				#region Functions
				public static CapacityContainer CapacityGet(KindTypePack pack)
				{
					switch(pack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							return(StandardUncompressed.Capacity);

						case KindTypePack.STANDARD_CPE:
							return(StandardCPE.Capacity);

						case KindTypePack.CPE_FLYWEIGHT:
							return(CapacityContainerDummy);

						default:
							break;
					}
					return(null);
				}

				public static string IDGetPack(KindTypePack typePack)
				{
					switch(typePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							return(StandardUncompressed.ID);

						case KindTypePack.STANDARD_CPE:
							return(StandardCPE.ID);

						case KindTypePack.CPE_FLYWEIGHT:
							return("Dummy");

						default:
							break;
					}
					return(null);
				}

				public static void BootUpFunctionInt(ContainerInt container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionInt;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionInt;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionFloat(ContainerFloat container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionFloat;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionFloat;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionVector2(ContainerVector2 container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionVector2;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionVector2;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionVector3(ContainerVector3 container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionVector3;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionVector3;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionStatus(ContainerStatus container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionStatus;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionStatus;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionCell(ContainerCell container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionCell;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionCell;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionColorBlend(ContainerColorBlend container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionColorBlend;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionColorBlend;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionVertexCorrection(ContainerVertexCorrection container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionVertexCorrection;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionVertexCorrection;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionUserData(ContainerUserData container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							/* MEMO: Unsupported */
//							container.Function = StandardUncompressed.FunctionUserData;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionUserData;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionInstance(ContainerInstance container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							/* MEMO: Unsupported */
//							container.Function = StandardUncompressed.FunctionInstance;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionInstance;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionEffect(ContainerEffect container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							/* MEMO: Unsupported */
//							container.Function = StandardUncompressed.FunctionEffect;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionEffect;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionCoordinateFix(ContainerCoordinateFix container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionCoordinateFix;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionCoordinateFix;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionColorBlendFix(ContainerColorBlendFix container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionColorBlendFix;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionColorBlendFix;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionUVFix(ContainerUVFix container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionUVFix;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionUVFix;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindTypePack
				{
					STANDARD_UNCOMPRESSED = 0,	/* Standard-Uncompressed (Plain Array) */
					STANDARD_CPE,	/* Standard-Compressed (Changing-Point Extracting) */
					CPE_FLYWEIGHT,	/* CPE & GoF-Flyweight */

					TERMINATOR,
				}

				private readonly static CapacityContainer CapacityContainerDummy = new CapacityContainer(
					false,		/* Status */
					false,		/* Position *//* Always Compressed */
					false,		/* Rotation *//* Always Compressed */
					false,		/* Scaling *//* Always Compressed */
					false,		/* RateOpacity */
					false,		/* PositionAnchor */
					false,		/* SizeForce */
					false,		/* UserData (Trigger) *//* Always Compressed */
					false,		/* Instance (Trigger) *//* Always Compressed */
					false,		/* Effect (Trigger) *//* Always Compressed */
					false,		/* RadiusCollision *//* Always Compressed */
					false,		/* Plain.Cell */
					false,		/* Plain.ColorBlend */
					false,		/* Plain.VertexCorrection */
					false,		/* Plain.OffsetPivot */
					false,		/* Plain.PositionTexture */
					false,		/* Plain.ScalingTexture */
					false,		/* Plain.RotationTexture */
					false,		/* Fix.IndexCellMap */
					false,		/* Fix.Coordinate */
					false,		/* Fix.ColorBlend */
					false,		/* Fix.UV0 */
					false,		/* Fix.SizeCollision *//* Always Compressed */
					false		/* Fix.PivotCollision *//* Always Compressed */
				);
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				[System.Serializable]
				public class ContainerInt : Container<int, InterfaceContainerInt> {}
				[System.Serializable]
				public class ContainerFloat : Container<float, InterfaceContainerFloat> {}
				[System.Serializable]
				public class ContainerVector2 : Container<Vector2, InterfaceContainerVector2> {}
				[System.Serializable]
				public class ContainerVector3 : Container<Vector3, InterfaceContainerVector3> {}
				[System.Serializable]
				public class ContainerStatus : Container<Library_SpriteStudio6.Data.Animation.Attribute.Status, InterfaceContainerStatus> {}
				[System.Serializable]
				public class ContainerCell : Container<Library_SpriteStudio6.Data.Animation.Attribute.Cell, InterfaceContainerCell> {}
				[System.Serializable]
				public class ContainerColorBlend : Container<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend, InterfaceContainerColorBlend> {}
				[System.Serializable]
				public class ContainerVertexCorrection : Container<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection, InterfaceContainerVertexCorrection> {}
				[System.Serializable]
				public class ContainerUserData : Container<Library_SpriteStudio6.Data.Animation.Attribute.UserData, InterfaceContainerUserData> {}
				[System.Serializable]
				public class ContainerInstance : Container<Library_SpriteStudio6.Data.Animation.Attribute.Instance, InterfaceContainerInstance> {}
				[System.Serializable]
				public class ContainerEffect : Container<Library_SpriteStudio6.Data.Animation.Attribute.Effect, InterfaceContainerEffect> {}
				[System.Serializable]
				public class ContainerCoordinateFix : Container<Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix, InterfaceContainerCoordinateFix> {}
				[System.Serializable]
				public class ContainerColorBlendFix : Container<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix, InterfaceContainerColorBlendFix> {}
				[System.Serializable]
				public class ContainerUVFix : Container<Library_SpriteStudio6.Data.Animation.Attribute.UVFix, InterfaceContainerUVFix> {}

				[System.Serializable]
				public class Container<_TypeValue, _TypeInterface>
					where _TypeValue : struct
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack TypePack;
					public CodeValueContainer[] TableCodeValue;
					public _TypeValue[] TableValue;

					public _TypeInterface Function;	/* NonSerialized */
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					/* MEMO: Be sure to override virtual-functions with implementation class. */
					#region Functions
					public void CleanUp()
					{
						TypePack = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(-1);
						TableCodeValue = null;
						TableValue = null;

						Function = default(_TypeInterface);
					}
					#endregion Functions
				}

				public interface InterfaceContainerInt : InterfaceContainer<	int,
																				ContainerInt,
																				Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt
																			> {}
				public interface InterfaceContainerFloat : InterfaceContainer<	float,
																				ContainerFloat,
																				Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat
																			> {}
				public interface InterfaceContainerVector2 : InterfaceContainer<	Vector2,
																					ContainerVector2,
																					Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat
																				> {}
				public interface InterfaceContainerVector3 : InterfaceContainer<	Vector3,
																					ContainerVector3,
																					Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat
																				> {}
				public interface InterfaceContainerStatus : InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.Status,
																					ContainerStatus,
																					Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool
																			> {}
				public interface InterfaceContainerCell : InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.Cell,
																				ContainerCell,
																				Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell
																			> {}
				public interface InterfaceContainerColorBlend : InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend,
																						ContainerColorBlend,
																						Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlend
																				> {}
				public interface InterfaceContainerVertexCorrection : InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection,
																							ContainerVertexCorrection,
																							Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection
																						> {}
				public interface InterfaceContainerUserData : InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.UserData,
																					ContainerUserData,
																					Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUserData
																				> {}
				public interface InterfaceContainerInstance : InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.Instance,
																					ContainerInstance,
																					Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInstance
																				> {}
				public interface InterfaceContainerEffect : InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.Effect,
																				ContainerEffect,
																				Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeEffect
																			> {}
				public interface InterfaceContainerCoordinateFix : InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix,
																							ContainerCoordinateFix,
																							Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCoordinateFix
																					> {}
				public interface InterfaceContainerColorBlendFix : InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix,
																							ContainerColorBlendFix,
																							Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlendFix
																					> {}
				public interface InterfaceContainerUVFix : InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.UVFix,
																				ContainerUVFix,
																				Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUVFix
																			> {}

				public interface InterfaceContainer<_TypeValue, _TypeContainer, _TypeSource>
					where _TypeValue : struct
				{
					/* ----------------------------------------------- Functions */
					#region Functions
					bool ValueGet(	ref _TypeValue outValue,
									ref int outFrameKey,
									_TypeContainer container,
									ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
								);
					bool Pack(	_TypeContainer container,
								string nameAttribute,
								int countFrame,
								Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
								int[] tableOrderDraw,
								params _TypeSource[] listKeyData
							);
					#endregion Functions
				}

				[System.Serializable]
				public struct CodeValueContainer
				{	/* MEMO: Since Jagged-Array can not be serialized...  */
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public int[] TableCode;
					#endregion Variables & Properties
				}

				public struct ArgumentContainer
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Script_SpriteStudio6_DataAnimation DataAnimation;
					public int IndexAnimation;
					public int IDParts;
					public int Frame;
					public int FramePrevious;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public ArgumentContainer(Script_SpriteStudio6_DataAnimation dataAnimation, int indexAnimation, int idParts, int frame, int framePrevious)
					{
						DataAnimation = dataAnimation;
						IndexAnimation = indexAnimation;
						IDParts = idParts;
						Frame = frame;
						FramePrevious = framePrevious;
					}

					public void CleanUp()
					{
						DataAnimation = null;
						IndexAnimation = -1;
						IDParts = -1;
						Frame = -1;
						FramePrevious = -1;
					}
					#endregion Functions
				}

				public class CapacityContainer
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					private FlagBit Flags;
					private FlagBitPlain FlagsPlain;
					private FlagBitFix FlagsFix;

					public bool Status
					{
						get
						{
							return(0 != (Flags & FlagBit.STATUS));
						}
					}
					public bool Position
					{
						get
						{
							return(0 != (Flags & FlagBit.POSITION));
						}
					}
					public bool Rotation
					{
						get
						{
							return(0 != (Flags & FlagBit.ROTATION));
						}
					}
					public bool Scaling
					{
						get
						{
							return(0 != (Flags & FlagBit.SCALING));
						}
					}
					public bool RateOpacity
					{
						get
						{
							return(0 != (Flags & FlagBit.RATE_OPACITY));
						}
					}
					public bool PositionAnchor
					{
						get
						{
							return(0 != (Flags & FlagBit.POSITION_ANCHOR));
						}
					}
					public bool SizeForce
					{
						get
						{
							return(0 != (Flags & FlagBit.SIZE_FORCE));
						}
					}
					public bool UserData
					{
						get
						{
							return(0 != (Flags & FlagBit.USER_DATA));
						}
					}
					public bool Instance
					{
						get
						{
							return(0 != (Flags & FlagBit.INSTANCE));
						}
					}
					public bool Effect
					{
						get
						{
							return(0 != (Flags & FlagBit.EFFECT));
						}
					}
					public bool RadiusCollision
					{
						get
						{
							return(0 != (Flags & FlagBit.RADIUS_COLLISION));
						}
					}

					public bool PlainCell
					{
						get
						{
							return(0 != (FlagsPlain & FlagBitPlain.CELL));
						}
					}
					public bool PlainColorBlend
					{
						get
						{
							return(0 != (FlagsPlain & FlagBitPlain.COLOR_BLEND));
						}
					}
					public bool PlainVertexCorrection
					{
						get
						{
							return(0 != (FlagsPlain & FlagBitPlain.VERTEX_CORRECTION));
						}
					}
					public bool PlainOffsetPivot
					{
						get
						{
							return(0 != (FlagsPlain & FlagBitPlain.OFFSET_PIVOT));
						}
					}
					public bool PlainPositionTexture
					{
						get
						{
							return(0 != (FlagsPlain & FlagBitPlain.POSITION_TEXTURE));
						}
					}
					public bool PlainScalingTexture
					{
						get
						{
							return(0 != (FlagsPlain & FlagBitPlain.SCALING_TEXTURE));
						}
					}
					public bool PlainRotationTexture
					{
						get
						{
							return(0 != (FlagsPlain & FlagBitPlain.ROTATION_TEXTURE));
						}
					}

					public bool FixIndexCellMap
					{
						get
						{
							return(0 != (FlagsFix & FlagBitFix.INDEX_CELL_MAP));
						}
					}
					public bool FixCoordinate
					{
						get
						{
							return(0 != (FlagsFix & FlagBitFix.COORDINATE));
						}
					}
					public bool FixColorBlend
					{
						get
						{
							return(0 != (FlagsFix & FlagBitFix.COLOR_BLEND));
						}
					}
					public bool FixUV0
					{
						get
						{
							return(0 != (FlagsFix & FlagBitFix.UV0));
						}
					}
					public bool FixSizeCollision
					{
						get
						{
							return(0 != (FlagsFix & FlagBitFix.SIZE_COLLISION));
						}
					}
					public bool FixPivotCollision
					{
						get
						{
							return(0 != (FlagsFix & FlagBitFix.PIVOT_COLLISION));
						}
					}
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public CapacityContainer(	bool status,
												bool position,
												bool rotation,
												bool scaling,
												bool rateOpacity,
												bool positionAnchor,
												bool sizeForce,
												bool userData,
												bool instance,
												bool effect,
												bool radiusCollision,
												bool plainCell,
												bool plainColorBlend,
												bool plainVertexCorrection,
												bool plainOffsetPivot,
												bool plainPositionTexture,
												bool plainScalingTexture,
												bool plainRotationTexture,
												bool fixIndexCellMap,
												bool fixCoordinate,
												bool fixColorBlend,
												bool fixUV0,
												bool fixSizeCollision,
												bool fixPivotCollision
											)
					{
						Flags = 0;
						Flags |= (true == status) ? FlagBit.STATUS : (FlagBit)0;
						Flags |= (true == position) ? FlagBit.POSITION : (FlagBit)0;
						Flags |= (true == rotation) ? FlagBit.ROTATION : (FlagBit)0;
						Flags |= (true == scaling) ? FlagBit.SCALING : (FlagBit)0;
						Flags |= (true == rateOpacity) ? FlagBit.RATE_OPACITY : (FlagBit)0;
						Flags |= (true == positionAnchor) ? FlagBit.POSITION_ANCHOR : (FlagBit)0;
						Flags |= (true == sizeForce) ? FlagBit.SIZE_FORCE : (FlagBit)0;
						Flags |= (true == userData) ? FlagBit.USER_DATA : (FlagBit)0;
						Flags |= (true == instance) ? FlagBit.INSTANCE : (FlagBit)0;
						Flags |= (true == effect) ? FlagBit.EFFECT : (FlagBit)0;
						Flags |= (true == radiusCollision) ? FlagBit.RADIUS_COLLISION : (FlagBit)0;

						FlagsPlain = 0;
						FlagsPlain |= (true == plainCell) ? FlagBitPlain.CELL : (FlagBitPlain)0;
						FlagsPlain |= (true == plainColorBlend) ? FlagBitPlain.COLOR_BLEND : (FlagBitPlain)0;
						FlagsPlain |= (true == plainVertexCorrection) ? FlagBitPlain.VERTEX_CORRECTION : (FlagBitPlain)0;
						FlagsPlain |= (true == plainOffsetPivot) ? FlagBitPlain.OFFSET_PIVOT : (FlagBitPlain)0;
						FlagsPlain |= (true == plainPositionTexture) ? FlagBitPlain.POSITION_TEXTURE : (FlagBitPlain)0;
						FlagsPlain |= (true == plainScalingTexture) ? FlagBitPlain.SCALING_TEXTURE : (FlagBitPlain)0;
						FlagsPlain |= (true == plainRotationTexture) ? FlagBitPlain.ROTATION_TEXTURE : (FlagBitPlain)0;

						FlagsFix = 0;
						FlagsFix |= (true == fixIndexCellMap) ? FlagBitFix.INDEX_CELL_MAP : (FlagBitFix)0;
						FlagsFix |= (true == fixCoordinate) ? FlagBitFix.COORDINATE : (FlagBitFix)0;
						FlagsFix |= (true == fixColorBlend) ? FlagBitFix.COLOR_BLEND : (FlagBitFix)0;
						FlagsFix |= (true == fixUV0) ? FlagBitFix.UV0 : (FlagBitFix)0;
						FlagsFix |= (true == fixSizeCollision) ? FlagBitFix.SIZE_COLLISION : (FlagBitFix)0;
						FlagsFix |= (true == fixPivotCollision) ? FlagBitFix.PIVOT_COLLISION : (FlagBitFix)0;
					}
					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					[System.Flags]
					private enum FlagBit
					{
						STATUS = 0x00000001,
						POSITION = 0x00000002,
						ROTATION = 0x00000004,
						SCALING = 0x00000008,
						RATE_OPACITY = 0x00000010,
						POSITION_ANCHOR = 0x00000020,
						SIZE_FORCE = 0x00000040,
						USER_DATA = 0x00000080,
						INSTANCE = 0x00000100,
						EFFECT = 0x00000200,
						RADIUS_COLLISION = 0x00000400,
					}

					[System.Flags]
					private enum FlagBitPlain
					{
						CELL = 0x00000001,
						COLOR_BLEND = 0x00000002,
						VERTEX_CORRECTION = 0x00000004,
						OFFSET_PIVOT = 0x00000008,
						POSITION_TEXTURE = 0x00000010,
						SCALING_TEXTURE = 0x00000020,
						ROTATION_TEXTURE = 0x00000040,
					}

					[System.Flags]
					private enum FlagBitFix
					{
						INDEX_CELL_MAP = 0x00000001,
						COORDINATE = 0x00000002,
						COLOR_BLEND = 0x00000004,
						UV0 = 0x00000008,
						SIZE_COLLISION = 0x00000010,
						PIVOT_COLLISION = 0x00000020,
					}
					#endregion Enums & Constants
				}
				#endregion Classes, Structs & Interfaces

				/* Implementation: SpriteStudio6/Library/Data/Animation/PackAttribute/*.cs */
			}
			#endregion Classes, Structs & Interfaces
		}

		public static partial class Effect
		{
			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			[System.Serializable]
			public partial struct Emitter
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public FlagBit FlagData;

				/* Datas for Particle */
				public Library_SpriteStudio6.KindOperationBlendEffect OperationBlendTarget;
				public int IndexCellMap;
				public int IndexCell;

				public RangeFloat Angle;

				public Vector2 GravityDirectional;
				public Vector2 GravityPointPosition;
				public float GravityPointPower;

				public RangeVector2 Position;

				public RangeFloat Rotation;
				public RangeFloat RotationFluctuation;
				public float RotationFluctuationRate;
				public float RotationFluctuationRateTime;

				public RangeFloat RateTangentialAcceleration;

				public RangeVector2 ScaleStart;
				public RangeFloat ScaleRateStart;

				public RangeVector2 ScaleEnd;
				public RangeFloat ScaleRateEnd;

				public int Delay;

				public RangeColor ColorVertex;
				public RangeColor ColorVertexFluctuation;

				public float AlphaFadeStart;
				public float AlphaFadeEnd;

				public RangeFloat Speed;
				public RangeFloat SpeedFluctuation;

				public float TurnDirectionFluctuation;

				public long SeedRandom;

				/* Datas for Emitter */
				public int DurationEmitter;
				public int Interval;
				public RangeFloat DurationParticle;

				public float PriorityParticle;
				public int CountParticleMax;
				public int CountParticleEmit;

				public int CountPartsMaximum;	/* Disuse?? */
				public PatternEmit[] TablePatternEmit;
				public int[] TablePatternOffset;
				public long[] TableSeedParticle;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					FlagData = FlagBit.CLEAR;

					OperationBlendTarget = Library_SpriteStudio6.KindOperationBlendEffect.MIX;
					IndexCellMap = -1;
					IndexCell = -1;

					DurationParticle.Main = 0.0f;
					DurationParticle.Sub = 0.0f;

					Angle.Main = 0.0f;
					Angle.Sub = 0.0f;

					GravityDirectional = Vector2.zero;
					GravityPointPosition = Vector2.zero;
					GravityPointPower = 0.0f;

					Position.Main = Vector2.zero;
					Position.Sub = Vector2.zero;

					Rotation.Main = 0.0f;
					Rotation.Sub = 0.0f;

					RotationFluctuation.Main = 0.0f;
					RotationFluctuation.Sub = 0.0f;
					RotationFluctuationRate = 0.0f;
					RotationFluctuationRateTime = 0.0f;

					RateTangentialAcceleration.Main = 0.0f;
					RateTangentialAcceleration.Sub = 0.0f;

					ScaleStart.Main = Vector2.zero;
					ScaleStart.Sub = Vector2.zero;
					ScaleRateStart.Main = 0.0f;
					ScaleRateStart.Sub = 0.0f;

					ScaleEnd.Main = Vector2.zero;
					ScaleEnd.Sub = Vector2.zero;
					ScaleRateEnd.Main = 0.0f;
					ScaleRateEnd.Sub = 0.0f;

					Delay = 0;

					ColorVertex.Main = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					ColorVertex.Sub = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					ColorVertexFluctuation.Main = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					ColorVertexFluctuation.Sub = new Color(1.0f, 1.0f, 1.0f, 1.0f);

					AlphaFadeStart = 0.0f;
					AlphaFadeEnd = 0.0f;

					Speed.Main = 0.0f;
					Speed.Sub = 0.0f;
					SpeedFluctuation.Main = 0.0f;
					SpeedFluctuation.Sub = 0.0f;

					TurnDirectionFluctuation = 0.0f;

					SeedRandom = (int)Constant.SEED_MAGIC;

					DurationEmitter = 15;
					Interval = 1;
					DurationParticle.Main = 15.0f;
					DurationParticle.Sub = 15.0f;

					PriorityParticle = 64.0f;
					CountParticleEmit = 2;
					CountParticleMax = 32;

					CountPartsMaximum = 0;
					TablePatternEmit = null;
					TablePatternOffset = null;
					TableSeedParticle = null;
				}

				public void Duplicate(Emitter original)
				{
					FlagData = original.FlagData;

					OperationBlendTarget = original.OperationBlendTarget;
					IndexCellMap = original.IndexCellMap;
					IndexCell = original.IndexCell;

					DurationParticle = original.DurationParticle;

					Angle = original.Angle;

					GravityDirectional = original.GravityDirectional;
					GravityPointPosition = original.GravityPointPosition;
					GravityPointPower = original.GravityPointPower;

					Position = original.Position;

					Rotation = original.Rotation;
					RotationFluctuation = original.RotationFluctuation;
					RotationFluctuationRate = original.RotationFluctuationRate;
					RotationFluctuationRateTime = original.RotationFluctuationRateTime;

					RateTangentialAcceleration = original.RateTangentialAcceleration;

					ScaleStart = original.ScaleStart;
					ScaleRateStart = original.ScaleRateStart;
					ScaleEnd = original.ScaleEnd;
					ScaleRateEnd = original.ScaleRateEnd;

					Delay = original.Delay;

					ColorVertex = original.ColorVertex;
					ColorVertexFluctuation = original.ColorVertexFluctuation;

					AlphaFadeStart = original.AlphaFadeStart;
					AlphaFadeEnd = original.AlphaFadeEnd;

					Speed = original.Speed;
					SpeedFluctuation = original.SpeedFluctuation;

					TurnDirectionFluctuation = original.TurnDirectionFluctuation;
					SeedRandom = original.SeedRandom;

					DurationEmitter = original.DurationEmitter;
					Interval = original.Interval;
					DurationParticle = original.DurationParticle;

					PriorityParticle = original.PriorityParticle;
					CountParticleMax = original.CountParticleMax;
					CountParticleEmit = original.CountParticleEmit;

					CountPartsMaximum = original.CountPartsMaximum;
					TablePatternEmit = original.TablePatternEmit;
					TablePatternOffset = original.TablePatternOffset;
					TableSeedParticle = original.TableSeedParticle;
				}

				public void TableGetPatternOffset(ref int[] dataTablePatternOffset)
				{
					int countEmitMax = CountParticleMax;
					int countEmit = CountParticleEmit;
					countEmit = (1 > countEmit) ? 1 : countEmit;

					/* Create Offset-Pattern Table */
					/* MEMO: This Table will be solved at Importing. */
					int shot = 0;
					int offset = Delay;
					int count = countEmitMax;
					dataTablePatternOffset = new int[count];
					for(int i=0; i<count; i++)
					{
						if(shot >= countEmit)
						{
							shot = 0;
							offset += Interval;
						}
						dataTablePatternOffset[i] = offset;
						shot++;
					}
				}

				public void TableGetPatternEmit(	ref PatternEmit[] tablePatternEmit,
													ref long[] tableSeedParticle,
													Library_SpriteStudio6.Utility.Random.Generator random,
													uint seedRandom
												)
				{	/* CAUTION!: Obtain "TablePatternOffset" before executing this function. */
					int count;
					int countEmitMax = CountParticleMax;

					List<PatternEmit> listPatternEmit = new List<PatternEmit>();
					listPatternEmit.Clear();

					int countEmit = CountParticleEmit;
					countEmit = (1 > countEmit) ? 1 : countEmit;

					/* Create Emit-Pattern Table */
					/* MEMO: This Table will be solved at Importing (at seedRandom is fixed). */
					random.InitSeed(seedRandom);
					int cycle = (int)(((float)(countEmitMax * Interval) / (float)countEmit) + 0.5f);
					count = countEmitMax * (int)Constant.LIFE_EXTEND_SCALE;
					if((int)Constant.LIFE_EXTEND_MIN > count)
					{
						count = (int)Constant.LIFE_EXTEND_MIN;
					}
					tablePatternEmit = new PatternEmit[count];
					int duration;
					for(int i=0; i<count; i++)
					{
						tablePatternEmit[i] = new PatternEmit();
						tablePatternEmit[i].IndexGenerate = i;
						duration = (int)((float)DurationParticle.Main + random.RandomFloat((float)DurationParticle.Sub));
						tablePatternEmit[i].Duration = duration;
						tablePatternEmit[i].Cycle = (duration > cycle) ? duration : cycle;
					}

					/* Create Random-Seed Table */
					/* MEMO: This Table will be solved at Importing (at seedRandom is fixed). */
					count = countEmitMax * 3;
					tableSeedParticle = new long[count];
					random.InitSeed(seedRandom);
					for(int i=0; i<count; i++)
					{
						tableSeedParticle[i] = (long)((ulong)random.RandomUint32());
					}
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum Constant
				{
					SEED_MAGIC = 7573,

					LIFE_EXTEND_SCALE = 8,
					LIFE_EXTEND_MIN = 64,
				}

				[System.Flags]
				public enum FlagBit
				{
					/* for Particle */
					BASIC = 0x00000001,	/* (Reserved) */
					TANGENTIALACCELATION = 0x00000002,
					TURNDIRECTION = 0x00000004,
					SEEDRANDOM = 0x00000008,
					DELAY = 0x00000010,

					POSITION = 0x00000100,
					POSITION_FLUCTUATION = 0x00000200,	/* (Reserved) */
					ROTATION = 0x00000400,
					ROTATION_FLUCTUATION = 0x00000800,
					SCALE_START = 0x00001000,
					SCALE_END = 0x00002000,

					SPEED = 0x00010000,	/* (Reserved) */
					SPEED_FLUCTUATION = 0x00020000,
					GRAVITY_DIRECTION = 0x00040000,
					GRAVITY_POINT = 0x00080000,

					COLORVERTEX = 0x00100000,
					COLORVERTEX_FLUCTUATION = 0x00200000,
					FADEALPHA = 0x00400000,

					/* for Emitter */
					EMIT_INFINITE = 0x01000000,

					/* Mask-Bit and etc. */
					CLEAR = 0x00000000,
					MASK_EMITTER = 0x7f000000,
					MASK_PARTICLE = 0x00ffffff,
					MASK_VALID = 0x7fffffff,
				}
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				[System.Serializable]
				public struct PatternEmit
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public int IndexGenerate;
//					public int Offset;
					public int Duration;
					public int Cycle;
					#endregion Variables & Properties

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					public void CleanUp()
					{
						IndexGenerate = -1;
//						Offset = -1;
						Duration = -1;
						Cycle = -1;
					}
					#endregion Enums & Constants
				}

				[System.Serializable]
				public struct RangeFloat
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public float Main;
					public float Sub;
					#endregion Variables & Properties
				}
				[System.Serializable]
				public struct RangeVector2
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Vector2 Main;
					public Vector2 Sub;
					#endregion Variables & Properties
				}
				[System.Serializable]
				public struct RangeColor
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Color Main;
					public Color Sub;
					#endregion Variables & Properties
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}

		[System.Serializable]
		public partial class CellMap
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public string Name;
			public Vector2 SizeOriginal;
			public Cell[] TableCell;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public void CleanUp()
			{
				Name = "";
				SizeOriginal = Vector2.zero;
				TableCell = null;
			}

			public int CountGetCell()
			{
				return((null != TableCell) ? TableCell.Length : -1);
			}

			public int IndexGetCell(string name)
			{
				if((true == string.IsNullOrEmpty(name)) || (null == TableCell))
				{
					return(-1);
				}

				int count = TableCell.Length;
				for(int i=0; i<count; i++)
				{
					if(name == TableCell[i].Name)
					{
						return(i);
					}
				}
				return(-1);
			}

			public void Duplicate(CellMap original)
			{
				Name = string.Copy(original.Name);
				SizeOriginal = original.SizeOriginal;
				TableCell = original.TableCell;
			}
			#endregion Functions

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			[System.Serializable]
			public struct Cell
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public string Name;
				public Rect Rectangle;
				public Vector2 Pivot;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Name = "";
					Rectangle.x = 0.0f;
					Rectangle.y = 0.0f;
					Rectangle.width = 0.0f;
					Rectangle.height = 0.0f;
					Pivot = Vector2.zero;
				}

				public void Duplicate(Cell original)
				{
					Name = string.Copy(original.Name);
					Rectangle = original.Rectangle;
					Pivot = original.Pivot;
				}
				#endregion Functions
			}
			#endregion Classes, Structs & Interfaces
		}

		public static partial class Texture
		{
			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public enum KindWrap
			{
				CLAMP = 0,
				REPEAT,
				MIRROR,	/* (Unsupported) */
			}
			public enum KindFilter
			{
				NEAREST = 0,
				LINEAR,
				BILINEAR,
			}
			#endregion Enums & Constants
		}

		public static partial class Parts
		{
			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			[System.Serializable]
			public struct Animation
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public string Name;
				public int ID;
				public int IDParent;
				public int[] TableIDChild;
				public KindFeature Feature;
				public KindColorLabel ColorLabel;
				public Library_SpriteStudio6.KindOperationBlend OperationBlendTarget;

				public KindCollision ShapeCollision;
				public float SizeCollisionZ;

				public Object PrefabUnderControl;
				public string NameAnimationUnderControl;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Name = "";

					ID = -1;
					IDParent = -1;

					Feature = (KindFeature)(-1);
					OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.NON;
					ColorLabel = KindColorLabel.NON;

					ShapeCollision = KindCollision.NON;
					SizeCollisionZ = 0.0f;

					PrefabUnderControl = null;
					NameAnimationUnderControl = "";
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindFeature
				{	/* ERROR/NON: -1 */
					ROOT = 0,	/* Root-Parts (Subspecies of "NULL"-Parts) */
					NULL,
					NORMAL_TRIANGLE2,	/* No use Vertex-Collection Sprite-Parts */
					NORMAL_TRIANGLE4,	/* Use Vertex-Collection Sprite-Parts */

					INSTANCE,
					EFFECT,

					TERMINATOR,
					NORMAL = TERMINATOR	/* NORMAL_TRIANGLE2 or NORMAL_TRIANGLE4 *//* only during import */
				}

				public enum KindColorLabel
				{
					NON = 0,
					RED,
					ORANGE,
					YELLOW,
					GREEN,
					BLUE,
					VIOLET,
					GRAY,
				}

				public enum KindCollision
				{
					NON = 0,
					SQUARE,
					AABB,
					CIRCLE,
					CIRCLE_SCALEMINIMUM,
					CIRCLE_SCALEMAXIMUM
				}
				#endregion Enums & Constants
			}

			[System.Serializable]
			public struct Effect
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public string Name;

				public int ID;
				public int IDParent;
				public int[] TableIDChild;

				public KindFeature Feature;	/* Preliminary ... "Root"or"Emitter" */
				public int IndexEmitter;	/* -1 == Not "Emitter" */
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Name = "";

					ID = -1;
					IDParent = -1;
					TableIDChild = null;

					Feature = (KindFeature)(-1);
					IndexEmitter = -1;
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindFeature
				{	/* ERROR: -1 */
					ROOT = 0,	/* Root-Parts (Subspecies of "Particle"-Parts) */
					EMITTER,	/* Emitter */
					PARTICLE,	/* Particle */

					TERMINATOR
				}
				#endregion Enums & Constants
			}
			#endregion Classes, Structs & Interfaces
		}

		public static partial class Shader
		{
			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public readonly static UnityEngine.Shader[] TableSprite = new UnityEngine.Shader[(int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR]
			{
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Mix"),
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Add"),
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Sub"),
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Mul")
			};

			public readonly static UnityEngine.Shader[] TableEffect = new UnityEngine.Shader[(int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR]
			{
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Effect/Mix"),
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Effect/Add"),
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Effect/Add2"),
			};
			#endregion Enums & Constants
		}
		#endregion Classes, Structs & Interfaces
	}

	public static partial class Script
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		[System.Serializable]
		public class Root : MonoBehaviour
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public Material[] TableMaterial;

			public Script_SpriteStudio6_DataCellMap DataCellMap;
			internal Library_SpriteStudio6.Data.CellMap[] TableCellMap = null;

			internal Script_SpriteStudio6_Root ScriptRootParent = null;

			public bool FlagReassignMaterialForce;
			public bool FlagHideForce;

			internal float RateOpacity = 1.0f;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			protected bool BaseAwake()
			{
				/* Reassignment for shader lost */
				/* MEMO: Memory leak occasionally, so normally no reassign. */
				if(true == FlagReassignMaterialForce)
				{
					int Count = (null != TableMaterial) ? TableMaterial.Length : 0;
					Material instanceMaterial = null;
					for(int i=0; i<Count; i++)
					{
						instanceMaterial = TableMaterial[i];
						if(null != instanceMaterial)
						{
							instanceMaterial.shader = Shader.Find(instanceMaterial.shader.name);
						}
					}
					instanceMaterial = null;
				}

				return(true);
			}

			protected bool BaseStart()
			{
				/* Generate CellMap table */
				if(false == CellMapBootUp())
				{
					return(false);
				}

				return(true);
			}

			protected bool BaseLateUpdate(float timeElapsed)
			{
				/* Recover CellMap table */
				if(null == TableCellMap)
				{
					if(false == CellMapBootUp())
					{
						return(false);
					}
				}

				return(true);
			}

			public int CountGetCellMap()
			{
				return((null == TableCellMap) ? -1 : TableCellMap.Length);
			}

			public int IndexGetCellMap(string name)
			{
				if(true == string.IsNullOrEmpty(name))
				{
					return(-1);
				}

				int count = TableCellMap.Length;
				for(int i=0; i<count; i++)
				{
					if(name == TableCellMap[i].Name)
					{
						return(i);
					}
				}
				return(-1);
			}

			public Library_SpriteStudio6.Data.CellMap DataGetCellMap(int index)
			{
				return(((0 > index) || (TableCellMap.Length <= index)) ? null : TableCellMap[index]);
			}

			public  Library_SpriteStudio6.Data.CellMap CellMapCopyFull(int indexCellMap, bool flagSourceIsOriginal)
			{
				Library_SpriteStudio6.Data.CellMap[] tableCellMapSource = (true == flagSourceIsOriginal) ? DataCellMap.TableCellMap : TableCellMap;

				if((0 > indexCellMap) || (tableCellMapSource.Length <= indexCellMap))
				{
					goto CellMapDuplicate_ErrorEnd;
				}

				Library_SpriteStudio6.Data.CellMap cellMap = new Library_SpriteStudio6.Data.CellMap();
				if(null == cellMap)
				{
					goto CellMapDuplicate_ErrorEnd;
				}
				Library_SpriteStudio6.Data.CellMap cellMapSource = tableCellMapSource[indexCellMap];

				int countCell = cellMapSource.TableCell.Length;
				cellMap.Name = string.Copy(cellMapSource.Name);
				cellMap.SizeOriginal = cellMapSource.SizeOriginal;
				cellMap.TableCell = new Library_SpriteStudio6.Data.CellMap.Cell[countCell];
				if(null == cellMap.TableCell)
				{
					goto CellMapDuplicate_ErrorEnd;
				}
				for(int i=0; i<countCell; i++)
				{
					cellMap.TableCell[i] = cellMapSource.TableCell[i];
				}
				return(cellMap);

			CellMapDuplicate_ErrorEnd:;
				return(null);
			}

			private bool CellMapBootUp()
			{
				if(null == DataCellMap)
				{
					return(false);
				}

				int countCellMap = DataCellMap.CountGetCellMap();
				if((null == TableCellMap) || (countCellMap > TableCellMap.Length))
				{
					TableCellMap = new Library_SpriteStudio6.Data.CellMap[DataCellMap.TableCellMap.Length];
					if(null == TableCellMap)
					{
						return(false);
					}
				}
				if(null != TableCellMap)
				{
					for(int i=0; i<countCellMap; i++)
					{
						TableCellMap[i] = DataCellMap.DataGetCellMap(i);
					}
				}

				return(true);
			}
			#endregion Functions
		}
		#endregion Classes, Structs & Interfaces
	}

	public static partial class Control
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public static partial class Animation
		{
			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			internal struct Track
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				internal KindMode Mode;
				internal FlagBitStatus Status;
				internal bool StatusIsPlaying
				{
					get
					{
						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING)));
					}
				}
				internal bool StatusIsPausing
				{
					get
					{
						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PAUSING) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PAUSING)));
					}
				}

				internal Library_SpriteStudio6.CallBack.FunctionControlEndTrackPlay FunctionPlayEnd;

				internal Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer ArgumentContainer;

				internal float TimeDelay;
				internal float TimeElapsed;
				internal float RateTime;

				internal int TimesPlay;
				internal int TimesPlayNow;
				internal int CountLoop;
				internal int CountLoopNow;

				internal int FramePerSecond;
				internal float TimePerFrame;
				internal float TimePerFrameConsideredRateTime;

				internal int FrameStart;
				internal int FrameEnd;
				internal int FrameRange;
				internal float TimeRange;

				internal float RateBlend;
				internal float RateBlendNormalized;

				internal int IndexTrackSlave;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Mode = KindMode.NORMAL;
					Status = FlagBitStatus.CLEAR;

					FunctionPlayEnd = null;

					ArgumentContainer.CleanUp();

					TimeDelay = 0.0f;
					TimeElapsed = 0.0f;
					RateTime = 1.0f;

					TimesPlay = -1;
					TimesPlayNow = -1;
					CountLoop = -1;
					CountLoopNow = -1;

					FramePerSecond = 60;
					TimePerFrame = 0.0f;
					TimePerFrameConsideredRateTime = 0.0f;

					FrameStart = -1;
					FrameEnd = -1;
					FrameRange = 0;
					TimeRange = 0.0f;

					RateBlend = 1.0f;
					RateBlendNormalized = 1.0f;

					IndexTrackSlave = -1;
				}

				public bool BootUp()
				{
					CleanUp();

					Status |= FlagBitStatus.VALID;

					return(true);
				}

				public bool Start(	Script_SpriteStudio6_Root instanceRoot,
									int indexAnimation,
									int frameRangeStart,
									int frameRangeEnd,
									int frame,
									int framePerSecond,
									float rateTime,
									float timeDelay,
									bool flagPingpong,
									int timesPlay
								)
									
				{
					/* Check booted-up */
					if(0 == (Status & FlagBitStatus.VALID))
					{
						if(false == BootUp())
						{
							return(false);
						}
					}

					/* Reset Animation */
					ArgumentContainer.DataAnimation = instanceRoot.DataAnimation;
					ArgumentContainer.IndexAnimation = indexAnimation;

					/* Set datas */
					Status &= FlagBitStatus.VALID;	/* Clear */

					Status = (true == flagPingpong) ? (Status | FlagBitStatus.STYLE_PINGPONG) : (Status & ~FlagBitStatus.STYLE_PINGPONG); 
					RateTime = rateTime;
					if(0.0f > RateTime)
					{
						Status = (0 == (Status & FlagBitStatus.STYLE_REVERSE)) ? (Status | FlagBitStatus.STYLE_REVERSE) : (Status & ~FlagBitStatus.STYLE_REVERSE);
						RateTime *= -1.0f;
					}
					TimePerFrame = 1.0f / (float)FramePerSecond;

					FramePerSecond = framePerSecond;
					FrameStart = frameRangeStart;
					FrameEnd = frameRangeEnd;
					ArgumentContainer.Frame = frame;
					if(0 != (Status & FlagBitStatus.STYLE_REVERSE))
					{	/* Play-Reverse */
						Status |= FlagBitStatus.PLAYING_REVERSE;
						ArgumentContainer.Frame = (ArgumentContainer.Frame <= FrameStart) ? (FrameEnd + 1) : ArgumentContainer.Frame;
					}
					else
					{	/* Play-Normal */
						Status &= ~FlagBitStatus.PLAYING_REVERSE;
						ArgumentContainer.Frame = (ArgumentContainer.Frame >= (FrameEnd + 1)) ? (FrameStart - 1) : ArgumentContainer.Frame;
					}

					TimesPlay = timesPlay;
					TimeDelay = timeDelay;

					CountLoop = 0;
					TimeElapsed = (ArgumentContainer.Frame - FrameStart) * TimePerFrame;
					ArgumentContainer.Frame = Mathf.Clamp(ArgumentContainer.Frame, FrameStart, FrameEnd);

					Status |= FlagBitStatus.PLAYING;
					Status |= FlagBitStatus.PLAYING_START;

					FrameRange = (FrameEnd - FrameStart) + 1;
					TimeRange = (float)FrameRange * TimePerFrame;
					TimePerFrameConsideredRateTime = TimePerFrame * RateTime;

					return(true);
				}

				public bool Stop(bool flagJumpEnd)
				{
					if(0 == (Status & FlagBitStatus.VALID))
					{
						return(false);
					}

					if(0 == (Status & FlagBitStatus.PLAYING))
					{
						return(true);
					}

					if(true == flagJumpEnd)
					{
					}

					return(true);
				}

				public bool Pause(bool flagSwitch)
				{
					if(0 == (Status & FlagBitStatus.VALID))
					{
						return(false);
					}

					if(true == flagSwitch)
					{
						Status |= FlagBitStatus.PAUSING;
					}
					else
					{
						Status &= ~FlagBitStatus.PAUSING;
					}

					return(true);
				}

				public bool Update(float timeElapsed)
				{
					timeElapsed *= RateTime;

					if((FlagBitStatus.VALID | FlagBitStatus.PLAYING) != (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING)))
					{	/* Not-Playing */
						return(false);
					}
					if((0 != (Status & FlagBitStatus.PAUSING)) && (0 == (Status & FlagBitStatus.PLAYING_START)))
					{	/* Play & Pausing (Through, Right-After-Starting) */
						return(true);
					}
					if(0.0f > TimeDelay)
					{	/* Wait Infinite */
						TimeDelay = -1.0f;
						Status &= ~(FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);	/* Cancel Start & Decoding Attribute */
						return(true);
					}
					else
					{	/* Wait Limited-Time */
						if(0.0f < TimeDelay)
						{	/* Waiting */
							TimeDelay -= timeElapsed;
							if(0.0f < TimeDelay)
							{
								Status &= ~(FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);	/* Cancel Start & Decoding Attribute */
								return(true);
							}

							/* Start */
							timeElapsed += -TimeDelay * ((0 == (Status & FlagBitStatus.PLAYING_REVERSE)) ? 1.0f : -1.0f);
							TimeDelay = 0.0f;
							ArgumentContainer.FramePrevious = -1;
							Status |= (FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);
						}
					}
					if(0 != (Status & FlagBitStatus.PLAYING_START))
					{	/* Play & Right-After-Starting */
						Status |= (FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);
						timeElapsed = 0.0f;
						goto AnimationUpdate_End;	/* Display the first frame, force */
					}

					/* Calculate New-Frame */
					ArgumentContainer.FramePrevious = ArgumentContainer.Frame;
					bool FlagRangeOverPrevious = false;
					if(0 != (Status & FlagBitStatus.STYLE_PINGPONG))
					{	/* Play-Style: PingPong */
						if(0 != (Status & FlagBitStatus.PLAYING_REVERSE))
						{
							FlagRangeOverPrevious = (0.0f > TimeElapsed) ? true : false;
							TimeElapsed -= timeElapsed;
							if((0.0f > TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto AnimationUpdate_End;
							}
						}
						else
						{
							FlagRangeOverPrevious = (TimeRange <= TimeElapsed) ? true : false;
							TimeElapsed += timeElapsed;
							if((TimeRange <= TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto AnimationUpdate_End;
							}
						}

						if(0 != (Status & FlagBitStatus.STYLE_REVERSE))
						{	/* Play-Style: PingPong & Reverse */
							while((TimeRange <= TimeElapsed) || (0.0f > TimeElapsed))
							{
								if(0 != (Status & FlagBitStatus.PLAYING_REVERSE))
								{	/* Now: Reverse */
									if(TimeRange <= TimeElapsed)
									{	/* MEMO: Follow "FlagRangeOverPrevious" */
										break;
									}
									if(0.0f > TimeElapsed)
									{	/* Frame-Over: Turn */
										TimeElapsed += TimeRange;
										TimeElapsed = TimeRange - TimeElapsed;
										Status |= FlagBitStatus.PLAYING_TURN;
										Status &= ~FlagBitStatus.PLAYING_REVERSE;
									}
								}
								else
								{   /* Now: Foward */
									if(true == FlagRangeOverPrevious)
									{
										FlagRangeOverPrevious = false;
										Status |= FlagBitStatus.PLAYING_TURN;
										break;
									}
									else
									{
										CountLoop++;
										if(TimeRange <= TimeElapsed)
										{	/* Frame-Over: Loop/End */
											if(0 < TimesPlayNow)
											{	/* Limited-Count Loop */
												TimesPlayNow--;
												if(0 >= TimesPlayNow)
												{	/* End */
													goto AnimationUpdate_PlayEnd_Foward;
												}
											}

											/* Not-End */
											TimeElapsed -= TimeRange;
											TimeElapsed = TimeRange - TimeElapsed;
											Status |= FlagBitStatus.PLAYING_REVERSE;
											Status |= FlagBitStatus.PLAYING_TURN;
											CountLoopNow++;
										}
									}
								}
							}
						}
						else
						{	/* Play-Style: PingPong & Foward */
							while((TimeRange <= TimeElapsed) || (0.0f > TimeElapsed))
							{
								if(0 != (Status & FlagBitStatus.PLAYING_REVERSE))
								{	/* Now: Reverse */
									if(true == FlagRangeOverPrevious)
									{
										FlagRangeOverPrevious = false;
										Status |= FlagBitStatus.PLAYING_TURN;
										break;
									}
									else
									{
										CountLoop++;
										if(0.0f > TimeElapsed)
										{	/* Frame-Over: Loop/End */
											if(0 < TimesPlayNow)
											{	/* Limited-Count Loop */
												TimesPlayNow--;
												if(0 >= TimesPlayNow)
												{	/* End */
													goto AnimationUpdate_PlayEnd_Reverse;
												}
											}

											/* Not-End */
											TimeElapsed += TimeRange;
											TimeElapsed = TimeRange - TimeElapsed;
											Status &= ~FlagBitStatus.PLAYING_REVERSE;
											Status |= FlagBitStatus.PLAYING_TURN;
											CountLoopNow++;
										}
									}
								}
								else
								{	/* Now: Foward */
									if(0.0f > TimeElapsed)
									{	/* MEMO: Follow "FlagRangeOverPrevious" */
										break;
									}
									if(TimeRange <= TimeElapsed)
									{	/* Frame-Over: Turn */
										TimeElapsed -= TimeRange;
										TimeElapsed = TimeRange - TimeElapsed;
										Status |= FlagBitStatus.PLAYING_TURN;
										Status |= FlagBitStatus.PLAYING_REVERSE;
									}
								}
							}
						}
					}
					else
					{	/* Play-Style: OneWay */
						if(0 != (Status & FlagBitStatus.STYLE_REVERSE))
						{	/* Play-Style: OneWay & Reverse */
							FlagRangeOverPrevious = (0.0f > TimeElapsed) ? true : false;
							TimeElapsed -= timeElapsed;
							if((0.0f > TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto AnimationUpdate_End;
							}

							while(0.0f > TimeElapsed)
							{
								TimeElapsed += TimeRange;
								if(true == FlagRangeOverPrevious)
								{
									FlagRangeOverPrevious = false;
									Status |= FlagBitStatus.PLAYING_TURN;
								}
								else
								{
									CountLoop++;
									if(0 < TimesPlayNow)
									{	/* Limited-Count Loop */
										TimesPlayNow--;
										if(0 >= TimesPlayNow)
										{	/* End */
											goto AnimationUpdate_PlayEnd_Reverse;
										}
									}

									/* Not-End */
									CountLoopNow++;
									Status |= FlagBitStatus.PLAYING_TURN;
								}
							}
						}
						else
						{	/* Play-Style: OneWay & Foward */
							FlagRangeOverPrevious = (TimeRange <= TimeElapsed) ? true : false;
							TimeElapsed += timeElapsed;
							if((TimeRange <= TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto AnimationUpdate_End;
							}

							while(TimeRange <= TimeElapsed)
							{
								TimeElapsed -= TimeRange;
								if(true == FlagRangeOverPrevious)
								{
									FlagRangeOverPrevious = false;
									Status |= FlagBitStatus.PLAYING_TURN;
								}
								else
								{
									CountLoop++;
									if(0 < TimesPlayNow)
									{	/* Limited-Count Loop */
										TimesPlayNow--;
										if(0 >= TimesPlayNow)
										{	/* End */
											goto AnimationUpdate_PlayEnd_Foward;
										}
									}

									/* Not-End */
									Status |= FlagBitStatus.PLAYING_TURN;
									CountLoopNow++;
								}
							}
						}
					}

				AnimationUpdate_End:;
					ArgumentContainer.Frame = (int)(TimeElapsed / TimePerFrame);
					ArgumentContainer.Frame = Mathf.Clamp(ArgumentContainer.Frame, 0, (FrameRange - 1));
					ArgumentContainer.Frame += FrameStart;
					if((ArgumentContainer.Frame != ArgumentContainer.FramePrevious) || (0 != (Status & FlagBitStatus.PLAYING_TURN)))
					{
						Status |= FlagBitStatus.DECODE_ATTRIBUTE;
					}
					return(true);

				AnimationUpdate_PlayEnd_Foward:;
					TimesPlayNow = 0;	/* Clip */
					Status |= (FlagBitStatus.REQUEST_PLAYEND | FlagBitStatus.DECODE_ATTRIBUTE);
					TimeElapsed = TimeRange;
					ArgumentContainer.Frame = FrameEnd;
					return(true);

				AnimationUpdate_PlayEnd_Reverse:;
					TimesPlayNow = 0;	/* Clip */
					Status |= (FlagBitStatus.REQUEST_PLAYEND | FlagBitStatus.DECODE_ATTRIBUTE);
					TimeElapsed = 0.0f;
					ArgumentContainer.Frame = FrameStart;
					return(true);
				}

				/* MEMO: Originally should be function, but call-cost is high(taking processing content into account). */
				/*       Processed directly in "Script_SpriteStudio6_Root".                                            */
//				public void StatusClearTransient()
//				{
//					if((FlagBitStatus.VALID | FlagBitStatus.PLAYING) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING)))
//					{	/* Not-Playing */
//						Status &= ~(FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);
//					}
//				}

				public void TimeElapse(float time, bool flagReverseParent, bool flagRangeEnd)
				{	/* MEMO: In principle, This Function is for calling from "UpdateInstance". */
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				[System.Flags]
				internal enum FlagBitStatus
				{
					VALID = 0x40000000,
					PLAYING = 0x20000000,
					PAUSING = 0x10000000,

					STYLE_PINGPONG = 0x08000000,
					STYLE_REVERSE = 0x04000000,

					PLAYING_START = 0x00800000,
					PLAYING_REVERSE = 0x00400000,
					PLAYING_REVERSEPREVIOUS = 0x00200000,
					PLAYING_TURN = 0x00100000,

					DECODE_ATTRIBUTE = 0x00080000,

					IGNORE_USERDATA = 0x00008000,
					IGNORE_SKIPLOOP = 0x00004000,

					REQUEST_PLAYEND = 0x00000080,

					CLEAR = 0x00000000,
				}

				internal enum KindMode
				{
					NORMAL = 0,
					SLAVE,	/* Fade destination when bridging animation */
				}
				#endregion Enums & Constants
			}

			[System.Serializable]
			public struct Parts
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				internal int IDParts;
				internal int IDPartsDrawNext;

				internal FlagBitStatus Status;

				public GameObject InstanceGameObject;
				internal Transform InstanceTransform;

				public Object PrefabUnderControl;
				public GameObject InstanceGameObjectUnderControl;
				internal Script_SpriteStudio6_Root InstanceRootUnderControl;
				internal int IndexAnimationUnderControl;
				internal Script_SpriteStudio6_RootEffect InstanceRootEffectUnderControl;

				public Collider InstanceComponentCollider;

				internal int IndexControlTrack;

				internal Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus StatusAnimationParts;

				internal BufferTRS TRSMaster;
				internal BufferTRS TRSSlave;

				/* Mesh Buffer */
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				internal void CleanUp()
				{
					IDParts = -1;
					IDPartsDrawNext = -1;

					Status = FlagBitStatus.CLEAR;

//					InstanceGameObject =
					InstanceTransform = null;

//					PrefabUnderControl =
//					InstanceGameObjectUnderControl =
					InstanceRootUnderControl = null;
					IndexAnimationUnderControl = -1;
					InstanceRootEffectUnderControl = null;

//					InstanceComponentCollider =

					IndexControlTrack = 0;

					StatusAnimationParts = Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.CLEAR;

					TRSMaster.CleanUp();
					TRSSlave.CleanUp();
				}

				internal bool BootUp(Script_SpriteStudio6_Root instanceRoot, int idParts)
				{
					IDParts = idParts;
					IDPartsDrawNext = -1;

					Status = FlagBitStatus.CLEAR;

					/* Get Part's Transform */
					if(null == InstanceGameObject)
					{
						return(false);
					}
					InstanceTransform = InstanceGameObject.transform;

					/* Clean up TRS Buffer */
					TRSMaster.CleanUp();
					TRSSlave.CleanUp();

					/* Instance/Effect BootUp */
					switch(instanceRoot.DataAnimation.TableParts[idParts].Feature)
					{
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
							/* MEMO: Erase, because can not have undercontrol object. */
							PrefabUnderControl = null;
							InstanceGameObjectUnderControl = null;
							break;

						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
							if(null == PrefabUnderControl)
							{
								PrefabUnderControl = instanceRoot.DataAnimation.TableParts[idParts].PrefabUnderControl;
								IndexAnimationUnderControl = -1;
							}
							if(false == BootUpInstance(instanceRoot, idParts, false))
							{
								goto BootUp_ErrorEnd;
							}
							break;

						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
							if(null == PrefabUnderControl)
							{
								PrefabUnderControl = instanceRoot.DataAnimation.TableParts[idParts].PrefabUnderControl;
								IndexAnimationUnderControl = -1;
							}
							if(false == BootUpEffect(instanceRoot, idParts, false))
							{
								goto BootUp_ErrorEnd;
							}
							break;
					}

					Status |= FlagBitStatus.VALID;
					return(true);

				BootUp_ErrorEnd:;
					CleanUp();
					Status = FlagBitStatus.CLEAR;
					return(false);
				}
				private bool BootUpInstance(Script_SpriteStudio6_Root instanceRoot, int idParts, bool flagRenewInstance)
				{
					if(null != PrefabUnderControl)
					{
						/* Create UnderControl-Instance */
						InstanceGameObjectUnderControl = Library_SpriteStudio6.Utility.Asset.PrefabInstantiate(	(GameObject)PrefabUnderControl,
																												InstanceGameObjectUnderControl,
																												InstanceGameObject,
																												flagRenewInstance
																											);
						if(null != InstanceGameObjectUnderControl)
						{
							InstanceRootUnderControl = InstanceGameObjectUnderControl.GetComponent<Script_SpriteStudio6_Root>();
							InstanceRootUnderControl.ScriptRootParent = instanceRoot;

							int indexAnimation = (true == string.IsNullOrEmpty(instanceRoot.DataAnimation.TableParts[idParts].NameAnimationUnderControl))
													? 0 : instanceRoot.IndexGetAnimation(instanceRoot.DataAnimation.TableParts[idParts].NameAnimationUnderControl);
							IndexAnimationUnderControl = (-1 == indexAnimation) ? 0 : indexAnimation;
//							InstanceRootUnderControl.AnimationPlay(-1, IndexAnimationUnderControl);
//							InstanceRootUnderControl.AnimationStop();
						}
					}
					return(true);
				}
				private bool BootUpEffect(Script_SpriteStudio6_Root instanceRoot, int idParts, bool flagRenewInstance)
				{
					if(null != PrefabUnderControl)
					{
						/* Create UnderControl-Instance */
						InstanceGameObjectUnderControl = Library_SpriteStudio6.Utility.Asset.PrefabInstantiate(	(GameObject)PrefabUnderControl,
																												InstanceGameObjectUnderControl,
																												InstanceGameObject,
																												flagRenewInstance
																											);
						if(null != InstanceGameObjectUnderControl)
						{
							InstanceRootEffectUnderControl = InstanceGameObjectUnderControl.GetComponent<Script_SpriteStudio6_RootEffect>();
							InstanceRootEffectUnderControl.ScriptRootParent = instanceRoot;
							IndexAnimationUnderControl = -1;
//							InstanceRootEffectUnderControl.AnimationPlay(-1, IndexAnimationUnderControl);
//							InstanceRootEffectUnderControl.AnimationStop();
						}
					}
					return(true);
				}

				internal void UpdateGameObject(Script_SpriteStudio6_Root instanceRoot, int idParts)
				{
					int indexTrack = IndexControlTrack;
					int indexAnimation = instanceRoot.TableControlTrack[indexTrack].ArgumentContainer.IndexAnimation;
					if(0 <= indexAnimation)
					{
						UpdateGameObjectMain(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
					}
				}
				private void UpdateGameObjectMain(	Script_SpriteStudio6_Root instanceRoot,
													int idParts,
													ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
													ref Library_SpriteStudio6.Control.Animation.Track controlTrack
												)
				{
					controlTrack.ArgumentContainer.IDParts = idParts;

					Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus statusParts = dataAnimationParts.StatusParts;
					StatusAnimationParts = statusParts;	/* cache */
					if(0 != (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED))
					{
						return;
					}

					Transform transform = InstanceGameObject.transform;
					Data.Animation.PackAttribute.ArgumentContainer argument = new Data.Animation.PackAttribute.ArgumentContainer();
					argument.Frame = 0;

					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION))
					{
						if(true == dataAnimationParts.Position.Function.ValueGet(ref TRSMaster.Position, ref TRSMaster.IndexPosition, dataAnimationParts.Position, ref controlTrack.ArgumentContainer))
						{	/* New Value */
							transform.localPosition = TRSMaster.Position;
						}
					}

					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION))
					{
						if(true == dataAnimationParts.Rotation.Function.ValueGet(ref TRSMaster.Rotation, ref TRSMaster.IndexRotation, dataAnimationParts.Rotation, ref controlTrack.ArgumentContainer))
						{	/* New Value */
							transform.localEulerAngles = TRSMaster.Rotation;
						}
					}

					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING))
					{
						if(true == dataAnimationParts.Scaling.Function.ValueGet(ref TRSMaster.Scaling, ref TRSMaster.IndexScaling, dataAnimationParts.Scaling, ref controlTrack.ArgumentContainer))
						{	/* New Value */
							Vector3 scaling = TRSMaster.Scaling;
							scaling.z = 1.0f;
							transform.localEulerAngles = scaling;
						}
					}
				}

				internal void UpdateDraw(Script_SpriteStudio6_Root instanceRoot, int idParts)
				{
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				[System.Flags]
				internal enum FlagBitStatus
				{
					VALID = 0x40000000,
					RUNNING = 0x20000000,

					HIDEFORCE = 0x08000000,

					CHANGE_TRANSFORM_POSITION = 0x00100000,
					CHANGE_TRANSFORM_ROTATION = 0x00200000,
					CHANGE_TRANSFORM_SCALING = 0x00400000,

					OVERWRITE_CELL_UNREFLECTED = 0x00080000,
					OVERWRITE_CELL_IGNOREATTRIBUTE = 0x00040000,

					INSTANCE_VALID = 0x00008000,
					INSTANCE_PLAYINDEPENDENT = 0x00004000,

					EFFECT_VALID = 0x00000800,
					EFFECT_PLAYINDEPENDENT = 0x00000400,

					CLEAR = 0x00000000
				}
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				internal struct BufferTRS
				{
					internal Vector3 Position;
					internal Vector3 Rotation;
					internal Vector2 Scaling;

					internal int IndexPosition;
					internal int IndexRotation;
					internal int IndexScaling;

					internal void CleanUp()
					{
						Position = Vector3.zero;
						Rotation = Vector3.zero;
						Scaling = Vector2.one;

						IndexPosition = -1;
						IndexRotation = -1;
						IndexScaling = -1;
					}
				}
				#endregion Classes, Structs & Interfaces
			}

			public class ColorBlend
			{
			}
			#endregion Classes, Structs & Interfaces
		}

		public class Effect
		{
		}
		#endregion Classes, Structs & Interfaces
	}

	public static partial class BatchDraw
	{
	}

	public static partial class Miscellaneousness
	{
	}

	public static partial class Utility
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public class Asset
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static GameObject GameObjectCreate(string name, bool flagActive, GameObject gameObjectParent)
			{
				GameObject gameObject = new GameObject(name);
				if(null != gameObject)
				{
					gameObject.SetActive(flagActive);
					Transform transform = gameObject.transform;
					if(null != gameObjectParent)
					{
						transform.parent = gameObjectParent.transform;
					}
					transform.localPosition = Vector3.zero;
					transform.localEulerAngles = Vector3.zero;
					transform.localScale = Vector3.one;
				}
				return(gameObject);
			}

			public static Transform TransformSearchNameChild(string name, Transform transformParent)
			{
				if(null == transformParent)
				{
					return(null);
				}

				Transform transform;
				int countChild = transformParent.childCount;
				for(int i=0; i<countChild; i++)
				{
					transform = transformParent.GetChild(i);
					if(null != transform)
					{
						if(name == transform.name)
						{
							return(transform);
						}
					}

					transform = TransformSearchNameChild(name, transform);	/* Recursion */
					if(null != transform)
					{
						return(transform);
					}
				}
				return(null);
			}

			public static GameObject PrefabInstantiate(	GameObject prefab,
														GameObject gameObjectOld,
														GameObject gameObjectParent,
														bool flagInstanceUnderControlRenew
													)
			{
				/* Error-Check */
				if(null == prefab)
				{
					return(null);
				}

				GameObject gameObject = gameObjectOld;
				Transform transformParent = gameObjectParent.transform;
				Transform transform;

				if(null == gameObject)
				{	/* Lost (Not-Found) */
					transform = transformParent.Find(prefab.name);
					if(null != transform)
					{	/* Found */
						gameObject = transform.gameObject;
					}
				}

				if(true == flagInstanceUnderControlRenew)
				{	/* Renew Force */
					if(null != gameObject)
					{	/* Exist */
						Object.DestroyImmediate(gameObject);
					}
					gameObject = null;
				}

				if(null == gameObject)
				{	/* Instantiate */
#if UNITY_EDITOR
					gameObject = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
					if(null == gameObject)
					{	/* for not-prefab */
						gameObject = Object.Instantiate(prefab) as GameObject;
						gameObject.name = prefab.name;	/* Remove "(clone)" */
					}
#else
					gameObject = Object.Instantiate(prefab) as GameObject;
					gameObject.name = prefab.name;	/* Remove "(clone)" */
#endif
					transform = gameObject.transform;

					if (null != gameObjectParent)
					{
						transform = gameObject.transform;
						transform.parent = transformParent;
					}
					if(null != gameObject)
					{
						transform.localPosition = Vector3.zero;
						transform.localEulerAngles = Vector3.zero;
						transform.localScale = Vector3.one;
					}
				}

				return(gameObject);
			}
			#endregion Functions
		}

		public static partial class Interpolation
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static float Linear(float start, float end, float point)
			{
				return(((end - start) * point) + start);
			}

			public static float Hermite(float start, float end, float point, float speedStart, float speedEnd)
			{
				float pointPow2 = point * point;
				float pointPow3 = pointPow2 * point;
				return(	(((2.0f * pointPow3) - (3.0f * pointPow2) + 1.0f) * start)
						+ (((3.0f * pointPow2) - (2.0f * pointPow3)) * end)
						+ ((pointPow3 - (2.0f * pointPow2) + point) * (speedStart - start))
						+ ((pointPow3 - pointPow2) * (speedEnd - end))
					);
			}

			public static float Bezier(ref Vector2 start, ref Vector2 end, float point, ref Vector2 vectorStart, ref Vector2 vectorEnd)
			{
				float pointNow = Linear(start.x, end.x, point);
				float pointTemp;

				float areaNow = 0.5f;
				float RangeNow = 0.5f;

				float baseNow;
				float baseNowPow2;
				float baseNowPow3;
				float areaNowPow2;
				for(int i=0; i<8; i++)
				{
					baseNow = 1.0f - areaNow;
					baseNowPow2 = baseNow * baseNow;
					baseNowPow3 = baseNowPow2 * baseNow;
					areaNowPow2 = areaNow * areaNow;
					pointTemp = (baseNowPow3 * start.x)
								+ (3.0f * baseNowPow2 * areaNow * (vectorStart.x + start.x))
								+ (3.0f * baseNow * areaNowPow2 * (vectorEnd.x + end.x))
								+ (areaNow * areaNowPow2 * end.x);
					RangeNow *= 0.5f;
					areaNow += ((pointTemp > pointNow) ? (-RangeNow) : (RangeNow));
				}

				areaNowPow2 = areaNow * areaNow;
				baseNow = 1.0f - areaNow;
				baseNowPow2 = baseNow * baseNow;
				baseNowPow3 = baseNowPow2 * baseNow;
				return(	(baseNowPow3 * start.y)
						+ (3.0f * baseNowPow2 * areaNow * (vectorStart.y + start.y))
						+ (3.0f * baseNow * areaNowPow2 * (vectorEnd.y + end.y))
						+ (areaNow * areaNowPow2 * end.y)
					);
			}

			public static float Accelerate(float start, float end, float point)
			{
				return(((end - start) * (point * point)) + start);
			}

			public static float Decelerate(float start, float end, float point)
			{
				float pointInverse = 1.0f - point;
				float rate = 1.0f - (pointInverse * pointInverse);
				return(((end - start) * rate) + start);
			}

			public static float ValueGetFloat(	Library_SpriteStudio6.Utility.Interpolation.KindFormula formula,
												int frameNow,
												int frameStart,
												float valueStart,
												int frameEnd,
												float valueEnd,
												float curveFrameStart,
												float curveValueStart,
												float curveFrameEnd,
												float curveValueEnd
											)
			{
				if(frameEnd <= frameStart)
				{
					return(valueStart);
				}
				float frameNormalized = ((float)(frameNow - frameStart)) / ((float)(frameEnd - frameStart));
				frameNormalized = Mathf.Clamp01(frameNormalized);

				switch(formula)
				{
					case KindFormula.NON:
						return(valueStart);

					case KindFormula.LINEAR:
						return(Linear(valueStart, valueEnd, frameNormalized));

					case KindFormula.HERMITE:
						return(Hermite(valueStart, valueEnd, frameNormalized, curveValueStart, curveValueEnd));

					case KindFormula.BEZIER:
						{
							Vector2 start = new Vector2((float)frameStart, valueStart);
							Vector2 vectorStart = new Vector2(curveFrameStart, curveValueStart);
							Vector2 end = new Vector2((float)frameEnd, valueEnd);
							Vector2 vectorEnd = new Vector2(curveFrameEnd, curveValueEnd);
							return(Interpolation.Bezier(ref start, ref end, frameNormalized, ref vectorStart, ref vectorEnd));
						}
//						break;	/* Redundant */

					case KindFormula.ACCELERATE:
						return(Accelerate(valueStart, valueEnd, frameNormalized));

					case KindFormula.DECELERATE:
						return(Decelerate(valueStart, valueEnd, frameNormalized));

					default:
						break;
				}
				return(valueStart);	/* Error */
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public enum KindFormula
			{
				NON = 0,
				LINEAR,
				HERMITE,
				BEZIER,
				ACCELERATE,
				DECELERATE,
			}
			#endregion Enums & Constants
		}

		public static partial class Random
		{
			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public interface Generator
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				uint[] ListSeed
				{
					get;
				}
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				void InitSeed(uint seed);
				uint RandomUint32();
				double RandomDouble(double valueMax=1.0);
				float RandomFloat(float valueMax=1.0f);
				int RandomN(int valueMax);
				#endregion Functions
			}
			#endregion Classes, Structs & Interfaces
		}
		#endregion Classes, Structs & Interfaces
	}
	#endregion Classes, Structs & Interfaces
}
