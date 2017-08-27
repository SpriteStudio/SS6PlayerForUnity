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
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	public static partial class CallBack
	{
		public delegate bool FunctionPlayEnd(Script_SpriteStudio6_Root instanceRoot, GameObject objectControl);
		public delegate bool FunctionPlayEndEffect(Script_SpriteStudio6_RootEffect instanceRoot);
		public delegate bool FunctionControlEndFrame(Script_SpriteStudio6_Root instanceRoot, int indexControlFrame);
		public delegate void FunctionUserData(Script_SpriteStudio6_Root instanceRoot, string nameParts, int indexParts, int indexAnimation, int frameDecode, int frameKeyData, ref Library_SpriteStudio6.Data.Animation.Attribute.UserData userData, bool flagWayBack);
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

			public void Compress(Parts.BufferPack[] dataUnpacked, Script_SpriteStudio6_DataAnimation instanceDataAnimation, int indexAnimation)
			{
				Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer = new Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer(instanceDataAnimation, indexAnimation, -1, -1, -1);
				int count = TableParts.Length;
				for(int i=0; i<count; i++)
				{
					argumentContainer.IndexParts = i;

					TableParts[i].Pack(CountFrame, ref dataUnpacked[i], ref argumentContainer);
				}
			}

			public void Decompress(Script_SpriteStudio6_DataAnimation instanceDataAnimation, int indexAnimation)
			{
				Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer = new Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer(instanceDataAnimation, indexAnimation, -1, -1, -1);
				int count = TableParts.Length;
				Parts.BufferPack[] dataUnpacked = new Parts.BufferPack[count];
				for(int i=0; i<count; i++)
				{
					argumentContainer.IndexParts = i;

					dataUnpacked[i].CleanUp();
					dataUnpacked[i].ValidSetAll();
					TableParts[i].Unpack(ref dataUnpacked[i], CountFrame, ref argumentContainer);
				}
			}
			#endregion Functions

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public static partial class Attribute
			{
				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindInterpolation
				{
					NON = 0,
					LINEAR,
					HERMITE,
					BEZIER,
					ACCELERATE,
					DECELERATE,
				}
				#endregion Enums & Constants

				/* Part: SpriteStudio6/Library/Data/Animation/Attribute.cs */
			}

			public static partial class PackAttribute
			{
				/* ----------------------------------------------- Functions */
				#region Functions
				private readonly static CapacityContainer CapacityContainerDummy = new CapacityContainer(
					false,		/* Status */
					false,		/* Position *//* Always Compressed */
					false,		/* Rotation *//* Always Compressed */
					false,		/* Scaling *//* Always Compressed */
					false,		/* RateOpacity */
					false,		/* Priority */
					false,		/* PositionAnchor */
					false,		/* SizeForce */
					false,		/* UserData (Trigger) *//* Always Compressed */
					false,		/* Instance (Trigger) *//* Always Compressed */
					false,		/* Effect (Trigger) *//* Always Compressed */
					false,		/* Plain.Cell */
					false,		/* Plain.ColorBlend */
					false,		/* Plain.VertexCorrection */
					false,		/* Plain.OffsetPivot */
					false,		/* Plain.PositionTexture */
					false,		/* Plain.ScalingTexture */
					false,		/* Plain.RotationTexture */
					false,		/* Plain.RadiusCollision *//* Always Compressed */
					false,		/* Fix.IndexCellMap */
					false,		/* Fix.Coordinate */
					false,		/* Fix.ColorBlend */
					false,		/* Fix.UV0 */
					false,		/* Fix.SizeCollision *//* Always Compressed */
					false,		/* Fix.PivotCollision *//* Always Compressed */
					false		/* Fix.RadiusCollision *//* Always Compressed */
				);
				public static CapacityContainer CapacityGet(KindPack pack)
				{
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							return(StandardUncompressed.Capacity);

						case KindPack.STANDARD_CPE:
							return(StandardCPE.Capacity);

						case KindPack.CPE_FLYWEIGHT:
							return(CapacityContainerDummy);

						default:
							break;
					}
					return(null);
				}

				public static string IDGetPack(KindPack pack)
				{
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							return(StandardUncompressed.ID);

						case KindPack.STANDARD_CPE:
							return(StandardCPE.ID);

						case KindPack.CPE_FLYWEIGHT:
							return("Dummy");

						default:
							break;
					}
					return(null);
				}

				public static Container<int> FactoryInt(KindPack pack)
				{
					Container<int> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							rv = new StandardUncompressed.PackAttributeInt();
							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeInt();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<float> FactoryFloat(KindPack pack)
				{
					Container<float> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							rv = new StandardUncompressed.PackAttributeFloat();
							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeFloat();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Vector2> FactoryVector2(KindPack pack)
				{
					Container<Vector2> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							rv = new StandardUncompressed.PackAttributeVector2();
							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeVector2();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Vector3> FactoryVector3(KindPack pack)
				{
					Container<Vector3> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							rv = new StandardUncompressed.PackAttributeVector3();
							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeVector3();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Library_SpriteStudio6.Data.Animation.Attribute.Status> FactoryStatus(KindPack pack)
				{
					Container<Library_SpriteStudio6.Data.Animation.Attribute.Status> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							rv = new StandardUncompressed.PackAttributeStatus();
							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeStatus();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend> FactoryColorBlend(KindPack pack)
				{
					Container<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							rv = new StandardUncompressed.PackAttributeColorBlend();
							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeColorBlend();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection> FactoryVertexCorrection(KindPack pack)
				{
					Container<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							rv = new StandardUncompressed.PackAttributeVertexCorrection();
							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeVertexCorrection();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Library_SpriteStudio6.Data.Animation.Attribute.Cell> FactoryCell(KindPack pack)
				{
					Container<Library_SpriteStudio6.Data.Animation.Attribute.Cell> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							rv = new StandardUncompressed.PackAttributeCell();
							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeCell();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Library_SpriteStudio6.Data.Animation.Attribute.UserData> FactoryUserData(KindPack pack)
				{
					Container<Library_SpriteStudio6.Data.Animation.Attribute.UserData> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
//							rv = new StandardUncompressed.PackAttributeUserData();
//							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeUserData();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Library_SpriteStudio6.Data.Animation.Attribute.Instance> FactoryInstance(KindPack pack)
				{
					Container<Library_SpriteStudio6.Data.Animation.Attribute.Instance> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
//							rv = new StandardUncompressed.PackAttributeInstance();
//							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeInstance();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Library_SpriteStudio6.Data.Animation.Attribute.Effect> FactoryEffect(KindPack pack)
				{
					Container<Library_SpriteStudio6.Data.Animation.Attribute.Effect> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
//							rv = new StandardUncompressed.PackAttributeEffect();
//							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeEffect();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix> FactoryCoordinateFix(KindPack pack)
				{
					Container<Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							rv = new StandardUncompressed.PackAttributeCoordinateFix();
							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeCoordinateFix();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix> FactoryColorBlendFix(KindPack pack)
				{
					Container<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							rv = new StandardUncompressed.PackAttributeColorBlendFix();
							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeColorBlendFix();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				public static Container<Library_SpriteStudio6.Data.Animation.Attribute.UVFix> FactoryUVFix(KindPack pack)
				{
					Container<Library_SpriteStudio6.Data.Animation.Attribute.UVFix> rv = null;
					switch(pack)
					{
						case KindPack.STANDARD_UNCOMPRESSED:
							rv = new StandardUncompressed.PackAttributeUVFix();
							rv.CleanUp();
							break;

						case KindPack.STANDARD_CPE:
							rv = new StandardCPE.PackAttributeUVFix();
							rv.CleanUp();
							break;

						case KindPack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
					return(rv);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindPack
				{
					STANDARD_UNCOMPRESSED = 0,	/* Standard-Uncompressed (Plain Array) */
					STANDARD_CPE,	/* Standard-Compressed (Changing-Point Extracting) */
					CPE_FLYWEIGHT,	/* CPE & GoF-Flyweight */

					TERMINATOR,
				}
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				public interface Container<_Type>
					where _Type : struct
				{
					/* ----------------------------------------------- Functions */
					#region Functions
					void CleanUp();
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack();

					bool ValueGet(ref _Type outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument);

					bool Pack(_Type[] tableDataRaw, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument);
					bool PackTrigger(_Type[] tableDataRaw, int[] tableFrameKey, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument);
					bool Unpack(ref _Type[] outTableDataRaw, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument);
					bool UnpackTrigger(ref _Type[] outTableDataRaw, ref int[] outTableFrameKey, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument);
					#endregion Functions
				}

				public class Parameter
				{
					/* ----------------------------------------------- Functions */
					#region Functions
					public virtual KindPack KindGetPack()
					{	/* MEMO: Be sure to override in Derived-Class */
						return((KindPack)(-1));
					}
					#endregion Functions
				}

				public struct ArgumentContainer
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Script_SpriteStudio6_DataAnimation InstanceDataAnimation;
					public int IndexAnimation;
					public int IndexParts;	/* index of Data.Animation.TableParts[] */
					public int Frame;
					public int FrameKeyPrevious;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public ArgumentContainer(Script_SpriteStudio6_DataAnimation instanceDataAnimation, int indexAnimation, int indexParts, int frame, int frameKeyPrevious)
					{
						InstanceDataAnimation = instanceDataAnimation;
						IndexAnimation = indexAnimation;
						IndexParts = indexParts;
						Frame = frame;
						FrameKeyPrevious = frameKeyPrevious;
					}

					public void CleanUp()
					{
						InstanceDataAnimation = null;
						IndexAnimation = -1;
						IndexParts = -1;
						Frame = -1;
						FrameKeyPrevious = -1;
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
					public bool Priority
					{
						get
						{
							return(0 != (Flags & FlagBit.PRIORITY));
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
					public bool PlainRadiusCollision
					{
						get
						{
							return(0 != (FlagsPlain & FlagBitPlain.RADIUS_COLLISION));
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
					public bool FixRadiusCollision
					{
						get
						{
							return(0 != (FlagsFix & FlagBitFix.RADIUS_COLLISION));
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
												bool priority,
												bool positionAnchor,
												bool sizeForce,
												bool userData,
												bool instance,
												bool effect,
												bool plainCell,
												bool plainColorBlend,
												bool plainVertexCorrection,
												bool plainOffsetPivot,
												bool plainPositionTexture,
												bool plainScalingTexture,
												bool plainRotationTexture,
												bool plainRadiusCollision,
												bool fixIndexCellMap,
												bool fixCoordinate,
												bool fixColorBlend,
												bool fixUV0,
												bool fixSizeCollision,
												bool fixPivotCollision,
												bool fixRadiusCollision
											)
					{
						Flags = 0;
						Flags |= (true == status) ? FlagBit.STATUS : (FlagBit)0;
						Flags |= (true == position) ? FlagBit.POSITION : (FlagBit)0;
						Flags |= (true == rotation) ? FlagBit.ROTATION : (FlagBit)0;
						Flags |= (true == scaling) ? FlagBit.SCALING : (FlagBit)0;
						Flags |= (true == rateOpacity) ? FlagBit.RATE_OPACITY : (FlagBit)0;
						Flags |= (true == priority) ? FlagBit.PRIORITY : (FlagBit)0;
						Flags |= (true == positionAnchor) ? FlagBit.POSITION_ANCHOR : (FlagBit)0;
						Flags |= (true == sizeForce) ? FlagBit.SIZE_FORCE : (FlagBit)0;
						Flags |= (true == userData) ? FlagBit.USER_DATA : (FlagBit)0;
						Flags |= (true == instance) ? FlagBit.INSTANCE : (FlagBit)0;
						Flags |= (true == effect) ? FlagBit.EFFECT : (FlagBit)0;

						FlagsPlain = 0;
						FlagsPlain |= (true == plainCell) ? FlagBitPlain.CELL : (FlagBitPlain)0;
						FlagsPlain |= (true == plainColorBlend) ? FlagBitPlain.COLOR_BLEND : (FlagBitPlain)0;
						FlagsPlain |= (true == plainVertexCorrection) ? FlagBitPlain.VERTEX_CORRECTION : (FlagBitPlain)0;
						FlagsPlain |= (true == plainOffsetPivot) ? FlagBitPlain.OFFSET_PIVOT : (FlagBitPlain)0;
						FlagsPlain |= (true == plainPositionTexture) ? FlagBitPlain.POSITION_TEXTURE : (FlagBitPlain)0;
						FlagsPlain |= (true == plainScalingTexture) ? FlagBitPlain.SCALING_TEXTURE : (FlagBitPlain)0;
						FlagsPlain |= (true == plainRotationTexture) ? FlagBitPlain.ROTATION_TEXTURE : (FlagBitPlain)0;
						FlagsPlain |= (true == plainRadiusCollision) ? FlagBitPlain.RADIUS_COLLISION : (FlagBitPlain)0;

						FlagsFix = 0;
						FlagsFix |= (true == fixIndexCellMap) ? FlagBitFix.INDEX_CELL_MAP : (FlagBitFix)0;
						FlagsFix |= (true == fixCoordinate) ? FlagBitFix.COORDINATE : (FlagBitFix)0;
						FlagsFix |= (true == fixColorBlend) ? FlagBitFix.COLOR_BLEND : (FlagBitFix)0;
						FlagsFix |= (true == fixUV0) ? FlagBitFix.UV0 : (FlagBitFix)0;
						FlagsFix |= (true == fixSizeCollision) ? FlagBitFix.SIZE_COLLISION : (FlagBitFix)0;
						FlagsFix |= (true == fixPivotCollision) ? FlagBitFix.PIVOT_COLLISION : (FlagBitFix)0;
						FlagsFix |= (true == fixRadiusCollision) ? FlagBitFix.RADIUS_COLLISION : (FlagBitFix)0;
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
						PRIORITY = 0x00000020,
						POSITION_ANCHOR = 0x00000040,
						SIZE_FORCE = 0x00000080,
						USER_DATA = 0x00000100,
						INSTANCE = 0x00000200,
						EFFECT = 0x00000400,
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
						RADIUS_COLLISION = 0x00000080,
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
						RADIUS_COLLISION = 0x00000040,
					}
					#endregion Enums & Constants
				}
				#endregion Classes, Structs & Interfaces

				/* Implementation: SpriteStudio6/Library/Data/Animation/PackAttribute/*.cs */
			}

			[System.Serializable]
			public struct Parts
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public KindFormat Format;
				public FlagBitStatus StatusParts;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Status> Status;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Vector3> Position;	/* Always Compressed */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Vector3> Rotation;	/* Always Compressed */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Vector2> Scaling;	/* Always Compressed */

				public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<float> RateOpacity;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<float> Priority;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Vector2> PositionAnchor;	/* Reserved */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Vector2> SizeForce;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.UserData> UserData;			/* Always Compressed */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Instance> Instance;			/* Always Compressed */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Effect> Effect;				/* Always Compressed */

				public AttributeGroupPlain Plain;
				public AttributeGroupFix Fix;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void Pack(int frameMax, ref BufferPack dataUnpacked, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer)
				{
					Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer capacity;
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack;

					pack = dataUnpacked.Status.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Status.FlagValid) && (null != capacity) && (true == capacity.Status))
					{
						Status = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryStatus(pack);
						if(null != Status)
						{
							Status.Pack(dataUnpacked.Status.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Position.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Position.FlagValid) && (null != capacity) && (true == capacity.Position))
					{
						Position = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryVector3(pack);
						if(null != Position)
						{
							Position.Pack(dataUnpacked.Position.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Rotation.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Rotation.FlagValid) && (null != capacity) && (true == capacity.Rotation))
					{
						Rotation = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryVector3(pack);
						if(null != Rotation)
						{
							Rotation.Pack(dataUnpacked.Rotation.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Scaling.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Scaling.FlagValid) && (null != capacity) && (true == capacity.Scaling))
					{
						Scaling = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryVector2(pack);
						if(null != Scaling)
						{
							Scaling.Pack(dataUnpacked.Scaling.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.RateOpacity.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.RateOpacity.FlagValid) && (null != capacity) && (true == capacity.RateOpacity))
					{
						RateOpacity = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryFloat(pack);
						if(null != RateOpacity)
						{
							RateOpacity.Pack(dataUnpacked.RateOpacity.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Priority.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Priority.FlagValid) && (null != capacity) && (true == capacity.Priority))
					{
						Priority = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryFloat(pack);
						if(null != Priority)
						{
							Priority.Pack(dataUnpacked.Priority.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.PositionAnchor.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.PositionAnchor.FlagValid) && (null != capacity) && (true == capacity.PositionAnchor))
					{
						PositionAnchor = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryVector2(pack);
						if(null != PositionAnchor)
						{
							PositionAnchor.Pack(dataUnpacked.PositionAnchor.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.SizeForce.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.SizeForce.FlagValid) && (null != capacity) && (true == capacity.SizeForce))
					{
						SizeForce = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryVector2(pack);
						if(null != SizeForce)
						{
							SizeForce.Pack(dataUnpacked.SizeForce.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.UserData.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.UserData.FlagValid) && (null != capacity) && (true == capacity.UserData))
					{
						UserData = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryUserData(pack);
						if(null != UserData)
						{
							UserData.PackTrigger(dataUnpacked.UserData.TableValue, dataUnpacked.UserData.TableFrameKey, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Instance.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Instance.FlagValid) && (null != capacity) && (true == capacity.Instance))
					{
						Instance = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryInstance(pack);
						if(null != Instance)
						{
							Instance.PackTrigger(dataUnpacked.Instance.TableValue, dataUnpacked.Instance.TableFrameKey, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Effect.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Effect.FlagValid) && (null != capacity) && (true == capacity.Effect))
					{
						Effect = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryEffect(pack);
						if(null != Effect)
						{
							Effect.PackTrigger(dataUnpacked.Effect.TableValue, dataUnpacked.Effect.TableFrameKey, frameMax, ref argumentContainer);
						}
					}

					switch(dataUnpacked.Format)
					{
						case KindFormat.PLAIN:
							pack = dataUnpacked.Plain.Cell.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.Cell.FlagValid) && (null != capacity) && (true == capacity.PlainCell))
							{
								Plain.Cell = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryCell(pack);
								if(null != Plain.Cell)
								{
									Plain.Cell.Pack(dataUnpacked.Plain.Cell.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.ColorBlend.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.ColorBlend.FlagValid) && (null != capacity) && (true == capacity.PlainColorBlend))
							{
								Plain.ColorBlend = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryColorBlend(pack);
								if(null != Plain.ColorBlend)
								{
									Plain.ColorBlend.Pack(dataUnpacked.Plain.ColorBlend.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.VertexCorrection.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.VertexCorrection.FlagValid) && (null != capacity) && (true == capacity.PlainVertexCorrection))
							{
								Plain.VertexCorrection = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryVertexCorrection(pack);
								if(null != Plain.VertexCorrection)
								{
									Plain.VertexCorrection.Pack(dataUnpacked.Plain.VertexCorrection.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.OffsetPivot.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.OffsetPivot.FlagValid) && (null != capacity) && (true == capacity.PlainOffsetPivot))
							{
								Plain.OffsetPivot = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryVector2(pack);
								if(null != Plain.OffsetPivot)
								{
									Plain.OffsetPivot.Pack(dataUnpacked.Plain.OffsetPivot.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.PositionTexture.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.PositionTexture.FlagValid) && (null != capacity) && (true == capacity.PlainPositionTexture))
							{
								Plain.PositionTexture = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryVector2(pack);
								if(null != Plain.PositionTexture)
								{
									Plain.PositionTexture.Pack(dataUnpacked.Plain.PositionTexture.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.ScalingTexture.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.ScalingTexture.FlagValid) && (null != capacity) && (true == capacity.PlainScalingTexture))
							{
								Plain.ScalingTexture = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryVector2(pack);
								if(null != Plain.ScalingTexture)
								{
									Plain.ScalingTexture.Pack(dataUnpacked.Plain.ScalingTexture.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.RotationTexture.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.RotationTexture.FlagValid) && (null != capacity) && (true == capacity.PlainRotationTexture))
							{
								Plain.RotationTexture = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryFloat(pack);
								if(null != Plain.RotationTexture)
								{
									Plain.RotationTexture.Pack(dataUnpacked.Plain.RotationTexture.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.RadiusCollision.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.RadiusCollision.FlagValid) && (null != capacity) && (true == capacity.PlainRadiusCollision))
							{
								Plain.RadiusCollision = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryFloat(pack);
								if(null != Plain.RadiusCollision)
								{
									Plain.RadiusCollision.Pack(dataUnpacked.Plain.RadiusCollision.TableValue, frameMax, ref argumentContainer);
								}
							}
							break;

						case KindFormat.FIX:
							pack = dataUnpacked.Fix.IndexCellMap.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.IndexCellMap.FlagValid) && (null != capacity) && (true == capacity.FixIndexCellMap))
							{
								Fix.IndexCellMap = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryInt(pack);
								if(null != Fix.IndexCellMap)
								{
									Fix.IndexCellMap.Pack(dataUnpacked.Fix.IndexCellMap.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.Coordinate.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.Coordinate.FlagValid) && (null != capacity) && (true == capacity.FixCoordinate))
							{
								Fix.Coordinate = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryCoordinateFix(pack);
								if(null != Fix.Coordinate)
								{
									Fix.Coordinate.Pack(dataUnpacked.Fix.Coordinate.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.ColorBlend.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.ColorBlend.FlagValid) && (null != capacity) && (true == capacity.FixColorBlend))
							{
								Fix.ColorBlend = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryColorBlendFix(pack);
								if(null != Fix.ColorBlend)
								{
									Fix.ColorBlend.Pack(dataUnpacked.Fix.ColorBlend.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.UV0.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.UV0.FlagValid) && (null != capacity) && (true == capacity.FixUV0))
							{
								Fix.UV0 = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryUVFix(pack);
								if(null != Fix.UV0)
								{
									Fix.UV0.Pack(dataUnpacked.Fix.UV0.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.SizeCollision.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.SizeCollision.FlagValid) && (null != capacity) && (true == capacity.FixSizeCollision))
							{
								Fix.SizeCollision = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryVector2(pack);
								if(null != Fix.SizeCollision)
								{
									Fix.SizeCollision.Pack(dataUnpacked.Fix.SizeCollision.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.PivotCollision.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.PivotCollision.FlagValid) && (null != capacity) && (true == capacity.FixPivotCollision))
							{
								Fix.PivotCollision = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryVector2(pack);
								if(null != Fix.PivotCollision)
								{
									Fix.PivotCollision.Pack(dataUnpacked.Fix.PivotCollision.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.RadiusCollision.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.RadiusCollision.FlagValid) && (null != capacity) && (true == capacity.FixRadiusCollision))
							{
								Fix.RadiusCollision = Library_SpriteStudio6.Data.Animation.PackAttribute.FactoryFloat(pack);
								if(null != Fix.RadiusCollision)
								{
									Fix.RadiusCollision.Pack(dataUnpacked.Fix.RadiusCollision.TableValue, frameMax, ref argumentContainer);
								}
							}
							break;

						default:
							break;
					}
				}

				public void Unpack(ref BufferPack dataUnpacked, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer)
				{
					Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer capacity;
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack;

					pack = dataUnpacked.Status.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Status.FlagValid) && (null != capacity) && (true == capacity.Status))
					{
						if(null != Status)
						{
							dataUnpacked.Status.FlagValid = Status.Unpack(ref dataUnpacked.Status.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Position.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Position.FlagValid) && (null != capacity) && (true == capacity.Position))
					{
						if(null != Position)
						{
							dataUnpacked.Position.FlagValid = Position.Unpack(ref dataUnpacked.Position.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Rotation.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Rotation.FlagValid) && (null != capacity) && (true == capacity.Rotation))
					{
						if(null != Rotation)
						{
							dataUnpacked.Rotation.FlagValid = Rotation.Unpack(ref dataUnpacked.Rotation.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Scaling.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Scaling.FlagValid) && (null != capacity) && (true == capacity.Scaling))
					{
						if(null != Scaling)
						{
							dataUnpacked.Scaling.FlagValid = Scaling.Unpack(ref dataUnpacked.Scaling.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.RateOpacity.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.RateOpacity.FlagValid) && (null != capacity) && (true == capacity.RateOpacity))
					{
						if(null != RateOpacity)
						{
							dataUnpacked.RateOpacity.FlagValid = RateOpacity.Unpack(ref dataUnpacked.RateOpacity.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Priority.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Priority.FlagValid) && (null != capacity) && (true == capacity.Priority))
					{
						if(null != Priority)
						{
							dataUnpacked.Priority.FlagValid = Priority.Unpack(ref dataUnpacked.Priority.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.PositionAnchor.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.PositionAnchor.FlagValid) && (null != capacity) && (true == capacity.PositionAnchor))
					{
						if(null != PositionAnchor)
						{
							dataUnpacked.PositionAnchor.FlagValid = PositionAnchor.Unpack(ref dataUnpacked.PositionAnchor.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.SizeForce.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.SizeForce.FlagValid) && (null != capacity) && (true == capacity.SizeForce))
					{
						if(null != SizeForce)
						{
							dataUnpacked.SizeForce.FlagValid = SizeForce.Unpack(ref dataUnpacked.SizeForce.TableValue, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.UserData.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.UserData.FlagValid) && (null != capacity) && (true == capacity.UserData))
					{
						if(null != UserData)
						{
							dataUnpacked.UserData.FlagValid = UserData.UnpackTrigger(ref dataUnpacked.UserData.TableValue, ref dataUnpacked.UserData.TableFrameKey, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Instance.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Instance.FlagValid) && (null != capacity) && (true == capacity.Instance))
					{
						if(null != Instance)
						{
							dataUnpacked.Instance.FlagValid = Instance.UnpackTrigger(ref dataUnpacked.Instance.TableValue, ref dataUnpacked.Instance.TableFrameKey, frameMax, ref argumentContainer);
						}
					}

					pack = dataUnpacked.Effect.Pack;
					capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
					if((true == dataUnpacked.Effect.FlagValid) && (null != capacity) && (true == capacity.Effect))
					{
						if(null != Effect)
						{
							dataUnpacked.Effect.FlagValid = Effect.UnpackTrigger(ref dataUnpacked.Effect.TableValue, ref dataUnpacked.Effect.TableFrameKey, frameMax, ref argumentContainer);
						}
					}

					switch(dataUnpacked.Format)
					{
						case KindFormat.PLAIN:
							pack = dataUnpacked.Plain.Cell.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.Cell.FlagValid) && (null != capacity) && (true == capacity.PlainCell))
							{
								if(null != Plain.Cell)
								{
									dataUnpacked.Plain.Cell.FlagValid = Plain.Cell.Unpack(ref dataUnpacked.Plain.Cell.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.ColorBlend.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.ColorBlend.FlagValid) && (null != capacity) && (true == capacity.PlainColorBlend))
							{
								if(null != Plain.ColorBlend)
								{
									dataUnpacked.Plain.ColorBlend.FlagValid = Plain.ColorBlend.Unpack(ref dataUnpacked.Plain.ColorBlend.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.VertexCorrection.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.VertexCorrection.FlagValid) && (null != capacity) && (true == capacity.PlainVertexCorrection))
							{
								if(null != Plain.VertexCorrection)
								{
									dataUnpacked.Plain.VertexCorrection.FlagValid = Plain.VertexCorrection.Unpack(ref dataUnpacked.Plain.VertexCorrection.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.OffsetPivot.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.OffsetPivot.FlagValid) && (null != capacity) && (true == capacity.PlainOffsetPivot))
							{
								if(null != Plain.OffsetPivot)
								{
									dataUnpacked.Plain.OffsetPivot.FlagValid = Plain.OffsetPivot.Unpack(ref dataUnpacked.Plain.OffsetPivot.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.PositionTexture.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.PositionTexture.FlagValid) && (null != capacity) && (true == capacity.PlainPositionTexture))
							{
								if(null != Plain.PositionTexture)
								{
									dataUnpacked.Plain.PositionTexture.FlagValid = Plain.PositionTexture.Unpack(ref dataUnpacked.Plain.PositionTexture.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.ScalingTexture.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.ScalingTexture.FlagValid) && (null != capacity) && (true == capacity.PlainScalingTexture))
							{
								if(null != Plain.ScalingTexture)
								{
									dataUnpacked.Plain.ScalingTexture.FlagValid = Plain.ScalingTexture.Unpack(ref dataUnpacked.Plain.ScalingTexture.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.RotationTexture.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.RotationTexture.FlagValid) && (null != capacity) && (true == capacity.PlainRotationTexture))
							{
								if(null != Plain.RotationTexture)
								{
									dataUnpacked.Plain.RotationTexture.FlagValid = Plain.RotationTexture.Unpack(ref dataUnpacked.Plain.RotationTexture.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Plain.RadiusCollision.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Plain.RadiusCollision.FlagValid) && (null != capacity) && (true == capacity.PlainRadiusCollision))
							{
								if(null != Plain.RadiusCollision)
								{
									dataUnpacked.Plain.RadiusCollision.FlagValid = Plain.RadiusCollision.Unpack(ref dataUnpacked.Plain.RadiusCollision.TableValue, frameMax, ref argumentContainer);
								}
							}
							break;

						case KindFormat.FIX:
							pack = dataUnpacked.Fix.IndexCellMap.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.IndexCellMap.FlagValid) && (null != capacity) && (true == capacity.FixIndexCellMap))
							{
								if(null != Fix.IndexCellMap)
								{
									dataUnpacked.Fix.IndexCellMap.FlagValid = Fix.IndexCellMap.Unpack(ref dataUnpacked.Fix.IndexCellMap.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.Coordinate.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.Coordinate.FlagValid) && (null != capacity) && (true == capacity.FixCoordinate))
							{
								if(null != Fix.Coordinate)
								{
									dataUnpacked.Fix.Coordinate.FlagValid = Fix.Coordinate.Unpack(ref dataUnpacked.Fix.Coordinate.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.ColorBlend.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.ColorBlend.FlagValid) && (null != capacity) && (true == capacity.FixColorBlend))
							{
								if(null != Fix.ColorBlend)
								{
									dataUnpacked.Fix.ColorBlend.FlagValid = Fix.ColorBlend.Unpack(ref dataUnpacked.Fix.ColorBlend.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.UV0.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.UV0.FlagValid) && (null != capacity) && (true == capacity.FixUV0))
							{
								if(null != Fix.UV0)
								{
									dataUnpacked.Fix.UV0.FlagValid = Fix.UV0.Unpack(ref dataUnpacked.Fix.UV0.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.SizeCollision.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.SizeCollision.FlagValid) && (null != capacity) && (true == capacity.FixSizeCollision))
							{
								if(null != Fix.SizeCollision)
								{
									dataUnpacked.Fix.SizeCollision.FlagValid = Fix.SizeCollision.Unpack(ref dataUnpacked.Fix.SizeCollision.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.PivotCollision.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.PivotCollision.FlagValid) && (null != capacity) && (true == capacity.FixPivotCollision))
							{
								if(null != Fix.PivotCollision)
								{
									dataUnpacked.Fix.PivotCollision.FlagValid = Fix.PivotCollision.Unpack(ref dataUnpacked.Fix.PivotCollision.TableValue, frameMax, ref argumentContainer);
								}
							}

							pack = dataUnpacked.Fix.RadiusCollision.Pack;
							capacity = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet(pack);
							if((true == dataUnpacked.Fix.RadiusCollision.FlagValid) && (null != capacity) && (true == capacity.FixRadiusCollision))
							{
								if(null != Fix.RadiusCollision)
								{
									dataUnpacked.Fix.RadiusCollision.FlagValid = Fix.RadiusCollision.Unpack(ref dataUnpacked.Fix.RadiusCollision.TableValue, frameMax, ref argumentContainer);
								}
							}
							break;

						default:
							break;
					}
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindFormat
				{	/* ERROR/NON: -1 */
					PLAIN = 0,	/* Data-Format: Plain-Data */
					FIX,	/* Data-Format: Deformation of "Mesh" and "Collider" are Calculated-In-Advance. */
				}

				[System.Flags]
				public enum FlagBitStatus
				{
					UNUSED = 0x40000000,

					HIDE_FORCE = 0x08000000,
					HIDE_FULL = 0x04000000,

					CLEAR = 0x00000000
				}
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				[System.Serializable]
				public class AttributeGroupPlain
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.Cell> Cell;	/* Always Compressed */

					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend> ColorBlend;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection> VertexCorrection;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Vector2> OffsetPivot;

					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Vector2> PositionTexture;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Vector2> ScalingTexture;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<float> RotationTexture;

					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<float> RadiusCollision;	/* for Sphere-Collider *//* Always Compressed */

					#endregion Variables & Properties
				}

				[System.Serializable]
				public class AttributeGroupFix
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<int> IndexCellMap;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix> Coordinate;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix> ColorBlend;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Library_SpriteStudio6.Data.Animation.Attribute.UVFix> UV0;

					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Vector2> SizeCollision;	/* for Box-Collider *//* Always Compressed */
					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<Vector2> PivotCollision;	/* for Box-Collider *//* Always Compressed */

					public Library_SpriteStudio6.Data.Animation.PackAttribute.Container<float> RadiusCollision;	/* for Sphere-Collider *//* Always Compressed */
					#endregion Variables & Properties
				}

				public struct BufferPack
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Library_SpriteStudio6.Data.Animation.Parts.KindFormat Format;

					public Attribute<Library_SpriteStudio6.Data.Animation.Attribute.Status> Status;
					public Attribute<Vector3> Position;
					public Attribute<Vector3> Rotation;
					public Attribute<Vector2> Scaling;

					public Attribute<float> RateOpacity;
					public Attribute<float> Priority;

					public Attribute<Vector2> PositionAnchor;
					public Attribute<Vector2> SizeForce;

					public AttributeTrigger<Library_SpriteStudio6.Data.Animation.Attribute.UserData> UserData;
					public AttributeTrigger<Library_SpriteStudio6.Data.Animation.Attribute.Instance> Instance;
					public AttributeTrigger<Library_SpriteStudio6.Data.Animation.Attribute.Effect> Effect;

					public AttributeGroupPlain Plain;
					public AttributeGroupFix Fix;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public void CleanUp()
					{
						Format = Library_SpriteStudio6.Data.Animation.Parts.KindFormat.PLAIN;

						Status.CleanUp();
						Position.CleanUp();
						Rotation.CleanUp();
						Scaling.CleanUp();
						RateOpacity.CleanUp();
						Priority.CleanUp();
						PositionAnchor.CleanUp();
						SizeForce.CleanUp();
						UserData.CleanUp();
						Instance.CleanUp();
						Effect.CleanUp();

						Plain.Cell.CleanUp();
						Plain.ColorBlend.CleanUp();
						Plain.VertexCorrection.CleanUp();
						Plain.OffsetPivot.CleanUp();
						Plain.PositionTexture.CleanUp();
						Plain.RotationTexture.CleanUp();
						Plain.ScalingTexture.CleanUp();
						Plain.RadiusCollision.CleanUp();

						Fix.IndexCellMap.CleanUp();
						Fix.Coordinate.CleanUp();
						Fix.ColorBlend.CleanUp();
						Fix.UV0.CleanUp();
						Fix.SizeCollision.CleanUp();
						Fix.PivotCollision.CleanUp();
						Fix.RadiusCollision.CleanUp();
					}

					public void FormatChangeFix()
					{
					}

					public void ValidSetAll()
					{
						Status.FlagValid = true;
						Position.FlagValid = true;
						Rotation.FlagValid = true;
						Scaling.FlagValid = true;

						RateOpacity.FlagValid = true;
						Priority.FlagValid = true;
						PositionAnchor.FlagValid = true;
						SizeForce.FlagValid = true;
						UserData.FlagValid = true;
						Instance.FlagValid = true;
						Effect.FlagValid = true;

						Plain.Cell.FlagValid = true;
						Plain.ColorBlend.FlagValid = true;
						Plain.VertexCorrection.FlagValid = true;
						Plain.OffsetPivot.FlagValid = true;
						Plain.PositionTexture.FlagValid = true;
						Plain.RotationTexture.FlagValid = true;
						Plain.ScalingTexture.FlagValid = true;
						Plain.RadiusCollision.FlagValid = true;

						Fix.IndexCellMap.FlagValid = true;
						Fix.Coordinate.FlagValid = true;
						Fix.ColorBlend.FlagValid = true;
						Fix.UV0.FlagValid = true;
						Fix.SizeCollision.FlagValid = true;
						Fix.PivotCollision.FlagValid = true;
						Fix.RadiusCollision.FlagValid = true;
					}
					#endregion Functions

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					public struct Attribute<_Type>
					{
						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						public bool FlagValid;
						public Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack Pack;
						public _Type[] TableValue;
						#endregion Variables & Properties

						/* ----------------------------------------------- Functions */
						#region Functions
						public void CleanUp()
						{
							FlagValid = false;
							Pack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED;
							TableValue = null;
						}

						public void ValueSet(_Type[] tableValue)
						{
							TableValue = tableValue;
							FlagValid = true;
						}
						#endregion Functions
					}

					public struct AttributeTrigger<_Type>
					{
						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						public bool FlagValid;
						public Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack Pack;
						public _Type[] TableValue;
						public int[] TableFrameKey;
						#endregion Variables & Properties

						/* ----------------------------------------------- Functions */
						#region Functions
						public void CleanUp()
						{
							FlagValid = false;
							Pack = Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED;
							TableValue = null;
							TableFrameKey = null;
						}

						public void ValueSet(_Type[] tableValue, int[] tableFrameKey)
						{
							TableValue = tableValue;
							TableFrameKey = tableFrameKey;
							FlagValid = true;
						}
						#endregion Functions
					}

					public struct AttributeGroupPlain
					{
						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						public Attribute<Library_SpriteStudio6.Data.Animation.Attribute.Cell> Cell;

						public Attribute<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend> ColorBlend;
						public Attribute<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection> VertexCorrection;
						public Attribute<Vector2> OffsetPivot;

						public Attribute<Vector2> PositionTexture;
						public Attribute<Vector2> ScalingTexture;
						public Attribute<float> RotationTexture;

						public Attribute<float> RadiusCollision;
						#endregion Variables & Properties
					}

					public struct AttributeGroupFix
					{
						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						public Attribute<int> IndexCellMap;
						public Attribute<Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix> Coordinate;
						public Attribute<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix> ColorBlend;
						public Attribute<Library_SpriteStudio6.Data.Animation.Attribute.UVFix> UV0;

						public Attribute<Vector2> SizeCollision;
						public Attribute<Vector2> PivotCollision;

						public Attribute<float> RadiusCollision;
						#endregion Variables & Properties
					}
					#endregion Classes, Structs & Interfaces
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

		public static class Parts
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
				public int[] ListIDChild;

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
					ListIDChild = null;

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
		public class Root : MonoBehaviour
		{
		}
		#endregion Classes, Structs & Interfaces
	}

	public static partial class Control
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public class Frame
		{
		}

		public class Parts
		{
			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public class Animation
			{
			}

			public class Effect
			{
			}
			#endregion Classes, Structs & Interfaces
		}

		public class ColorBlend
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
	}
	#endregion Classes, Structs & Interfaces
}
