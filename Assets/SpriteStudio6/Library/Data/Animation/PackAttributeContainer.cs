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
				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private readonly static CapacityContainer CapacityContainerDummy = new CapacityContainer(
					false,		/* Status */
					false,		/* Position */
					false,		/* Rotation */
					false,		/* Scaling */
					false,		/* ScalingLocal */
					false,		/* RateOpacity */
					false,		/* PositionAnchor */
					false,		/* SizeForce */
					false,		/* RadiusCollision */
					false,		/* PowerMask */
					false,		/* UserData (Trigger) */
					false,		/* Instance (Trigger) */
					false,		/* Effect (Trigger) */
					false,		/* Plain.Cell */
					false,		/* Plain.PartsColor */
					false,		/* Plain.VertexCorrection */
					false,		/* Plain.OffsetPivot */
					false,		/* Plain.PositionTexture */
					false,		/* Plain.ScalingTexture */
					false,		/* Plain.RotationTexture */
					false,		/* Fix.IndexCellMap */
					false,		/* Fix.Coordinate */
					false,		/* Fix.PartsColor */
					false,		/* Fix.UV0 */
					false,		/* Fix.SizeCollision *//* Always Compressed */
					false		/* Fix.PivotCollision *//* Always Compressed */
				);
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
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
				public interface InterfaceContainerPartsColor: InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.PartsColor,
																						ContainerPartsColor,
																						Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributePartsColor
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
				public interface InterfaceContainerPartsColorFix : InterfaceContainer<	Library_SpriteStudio6.Data.Animation.Attribute.PartsColorFix,
																							ContainerPartsColorFix,
																							Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributePartsColorFix
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
					bool ValueGetIndex(	ref _TypeValue outValue,
										ref int outFrameKey,
										int index,
										_TypeContainer container,
										ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument
									);
					int CountGetValue(_TypeContainer container);

					bool Pack(	_TypeContainer container,
								string nameAttribute,
								int countFrame,
								Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
								int[] tableOrderDraw,
								int[] tableOrderPreDraw,
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

				[System.Serializable]
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
					public bool ScalingLocal
					{
						get
						{
							return(0 != (Flags & FlagBit.SCALING_LOCAL));
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
					public bool PowerMask
					{
						get
						{
							return(0 != (Flags & FlagBit.POWER_MASK));
						}
					}

					public bool PlainCell
					{
						get
						{
							return(0 != (FlagsPlain & FlagBitPlain.CELL));
						}
					}
					public bool PlainPartsColor
					{
						get
						{
							return(0 != (FlagsPlain & FlagBitPlain.PARTS_COLOR));
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
					public bool FixPartsColor
					{
						get
						{
							return(0 != (FlagsFix & FlagBitFix.PARTS_COLOR));
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
												bool scalingLocal,
												bool rateOpacity,
												bool positionAnchor,
												bool sizeForce,
												bool radiusCollision,
												bool powerMask,
												bool userData,
												bool instance,
												bool effect,
												bool plainCell,
												bool plainPartsColor,
												bool plainVertexCorrection,
												bool plainOffsetPivot,
												bool plainPositionTexture,
												bool plainScalingTexture,
												bool plainRotationTexture,
												bool fixIndexCellMap,
												bool fixCoordinate,
												bool fixPartsColor,
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
						Flags |= (true == scalingLocal) ? FlagBit.SCALING_LOCAL : (FlagBit)0;
						Flags |= (true == rateOpacity) ? FlagBit.RATE_OPACITY : (FlagBit)0;
						Flags |= (true == positionAnchor) ? FlagBit.POSITION_ANCHOR : (FlagBit)0;
						Flags |= (true == sizeForce) ? FlagBit.SIZE_FORCE : (FlagBit)0;
						Flags |= (true == radiusCollision) ? FlagBit.RADIUS_COLLISION : (FlagBit)0;
						Flags |= (true == powerMask) ? FlagBit.POWER_MASK : (FlagBit)0;
						Flags |= (true == userData) ? FlagBit.USER_DATA : (FlagBit)0;
						Flags |= (true == instance) ? FlagBit.INSTANCE : (FlagBit)0;
						Flags |= (true == effect) ? FlagBit.EFFECT : (FlagBit)0;

						FlagsPlain = 0;
						FlagsPlain |= (true == plainCell) ? FlagBitPlain.CELL : (FlagBitPlain)0;
						FlagsPlain |= (true == plainPartsColor) ? FlagBitPlain.PARTS_COLOR : (FlagBitPlain)0;
						FlagsPlain |= (true == plainVertexCorrection) ? FlagBitPlain.VERTEX_CORRECTION : (FlagBitPlain)0;
						FlagsPlain |= (true == plainOffsetPivot) ? FlagBitPlain.OFFSET_PIVOT : (FlagBitPlain)0;
						FlagsPlain |= (true == plainPositionTexture) ? FlagBitPlain.POSITION_TEXTURE : (FlagBitPlain)0;
						FlagsPlain |= (true == plainScalingTexture) ? FlagBitPlain.SCALING_TEXTURE : (FlagBitPlain)0;
						FlagsPlain |= (true == plainRotationTexture) ? FlagBitPlain.ROTATION_TEXTURE : (FlagBitPlain)0;

						FlagsFix = 0;
						FlagsFix |= (true == fixIndexCellMap) ? FlagBitFix.INDEX_CELL_MAP : (FlagBitFix)0;
						FlagsFix |= (true == fixCoordinate) ? FlagBitFix.COORDINATE : (FlagBitFix)0;
						FlagsFix |= (true == fixPartsColor) ? FlagBitFix.PARTS_COLOR : (FlagBitFix)0;
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
						SCALING_LOCAL = 0x00000010,
						RATE_OPACITY = 0x00000020,
						POSITION_ANCHOR = 0x00000040,
						SIZE_FORCE = 0x00000080,
						RADIUS_COLLISION = 0x00000100,
						POWER_MASK = 0x00000200,
						USER_DATA = 0x00000400,
						INSTANCE = 0x00000800,
						EFFECT = 0x00001000,
					}

					[System.Flags]
					private enum FlagBitPlain
					{
						CELL = 0x00000001,
						PARTS_COLOR = 0x00000002,
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
						PARTS_COLOR = 0x00000004,
						UV0 = 0x00000008,
						SIZE_COLLISION = 0x00000010,
						PIVOT_COLLISION = 0x00000020,
					}
					#endregion Enums & Constants
				}
				#endregion Classes, Structs & Interfaces
			}
		}
	}
}
