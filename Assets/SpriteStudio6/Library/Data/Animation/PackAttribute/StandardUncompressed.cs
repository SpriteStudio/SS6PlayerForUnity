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

						public bool Pack(_Type[] tableDataRaw, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							int countRange = tableDataRaw.Length;
							int countOver = 0;
							frameMax++;	/* Index to Range */
							if(0 <= frameMax)
							{
								if(countRange > frameMax)
								{
									countRange = frameMax;
								}
								else
								{
									countOver = frameMax - countRange;
								}
							}
							TableValue = new _Type[countRange + countOver];

							countOver += countRange;
							frameMax = countRange - 1;
							for(int i=0; i<countRange; i++)
							{
								TableValue[i] = tableDataRaw[i];
							}
							for(int i=countRange; i<countOver; i++)
							{
								TableValue[i] = tableDataRaw[frameMax];
							}
							return(true);
						}

						public bool PackTrigger(_Type[] tableDataRaw, int[] tableFrameKey, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{	/* Not-Supported */
							TableValue = new _Type[0];
							return(false);
						}

						public bool Unpack(ref _Type[] outTableDataRaw, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							int countRange = TableValue.Length;
							int countOver = 0;
							frameMax++;	/* Index to Range */
							if(0 <= frameMax)
							{
								if(countRange > frameMax)
								{
									countRange = frameMax;
								}
								else
								{
									countOver = frameMax - countRange;
								}
							}
							outTableDataRaw = new _Type[countRange + countOver];

							countOver += countRange;
							frameMax = countRange - 1;
							for(int i=0; i<countRange; i++)
							{
								outTableDataRaw[i] = TableValue[i];
							}
							for(int i=countRange; i<countOver; i++)
							{
								outTableDataRaw[i] = TableValue[frameMax];
							}
							return(true);
						}

						public bool UnpackTrigger(ref _Type[] outTableDataRaw, ref int[] outTableFrameKey, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{	/* Not-Supported */
							outTableDataRaw = null;
							outTableFrameKey = null;
							return(false);
						}
						#endregion Functions
					}

					[System.Serializable]
					public class PackAttributeInt : Base<int>
					{
					}
					[System.Serializable]
					public class PackAttributeFloat : Base<float>
					{
					}
					[System.Serializable]
					public class PackAttributeVector2 : Base<Vector2>
					{
					}
					[System.Serializable]
					public class PackAttributeVector3 : Base<Vector3>
					{
					}
					[System.Serializable]
					public class PackAttributeStatus : Base<Library_SpriteStudio6.Data.Animation.Attribute.Status>
					{
					}
					[System.Serializable]
					public class PackAttributeCell : Base<Library_SpriteStudio6.Data.Animation.Attribute.Cell>
					{
					}
					[System.Serializable]
					public class PackAttributeColorBlend : Base<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend>
					{
					}
					[System.Serializable]
					public class PackAttributeVertexCorrection : Base<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection>
					{
					}
//					[System.Serializable]
//					public class PackAttributeUserData : Base<Library_SpriteStudio6.Data.Animation.Attribute.UserData>
//					{
//					}
//					[System.Serializable]
//					public class PackAttributeInstance : Base<Library_SpriteStudio6.Data.Animation.Attribute.Instance>
//					{
//					}
//					[System.Serializable]
//					public class PackAttributeEffect : Base<Library_SpriteStudio6.Data.Animation.Attribute.Effect>
//					{
//					}
					[System.Serializable]
					public class PackAttributeCoordinateFix : Base<Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix>
					{
					}
					[System.Serializable]
					public class PackAttributeColorBlendFix : Base<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix>
					{
					}
					[System.Serializable]
					public class PackAttributeUVFix : Base<Library_SpriteStudio6.Data.Animation.Attribute.UVFix>
					{
					}
					#endregion Classes, Structs & Interfaces
				}
				#endregion Classes, Structs & Interfaces
			}
		}
	}
}
