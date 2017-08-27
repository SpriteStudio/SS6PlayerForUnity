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
						true,	/* Priority */
						true,	/* PositionAnchor */
						true,	/* SizeForce */
						true,	/* UserData (Trigger) */
						true,	/* Instance (Trigger) */
						true,	/* Effect (Trigger) */
						true,	/* Plain.Cell */
						true,	/* Plain.ColorBlend */
						true,	/* Plain.VertexCorrection */
						true,	/* Plain.OffsetPivot */
						true,	/* Plain.PositionTexture */
						true,	/* Plain.ScalingTexture */
						true,	/* Plain.RotationTexture */
						true,	/* Plain.RadiusCollision */
						true,	/* Fix.IndexCellMap */
						true,	/* Fix.Coordinate */
						true,	/* Fix.ColorBlend */
						true,	/* Fix.UV0 */
						true,	/* Fix.SizeCollision */
						true,	/* Fix.PivotCollision */
						true	/* Fix.RadiusCollision */
					);

					public const string ID = "StandardCPE";
					#endregion Enums & Constants

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					public class Base<_Type> : Library_SpriteStudio6.Data.Animation.PackAttribute.Container<_Type>
						where _Type : struct
					{
						[System.Flags]
						public enum FlagBit
						{
							FRAMEKEY = 0x00007fff,
							INDEX = 0x3fff8000,

							CLEAR = 0x00000000,
						}

						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						private FlagBit[] TableStatus;
						private _Type[] TableValue;
						#endregion Variables & Properties

						/* ----------------------------------------------- Functions */
						#region Functions
						public void CleanUp()
						{
							TableStatus = null;
							TableValue = null;
						}

						public Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack KindGetPack()
						{
							return(Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE);
						}

						public bool ValueGet(ref _Type outValue, ref int outFrameKey, int framePrevious, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							int frame = argument.Frame;
							int frameKey = -1;

							FlagBit status;
							int indexMinimum = 0;
							int indexMaximum = TableStatus.Length - 1;
							int index;
							while(indexMinimum != indexMaximum)
							{
								index = indexMinimum + indexMaximum;
								index = (index >> 1) + (index & 1);	/* (index / 2) + (index % 2) */
								frameKey = (int)(TableStatus[index] & FlagBit.FRAMEKEY);
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

							status = TableStatus[indexMinimum];
							frameKey = (int)(status & FlagBit.FRAMEKEY);
							outFrameKey = frameKey;
							if(framePrevious == frameKey)
							{
								return(false);	/* outValue is not overwritten. */
							}

							index = (int)(status & FlagBit.INDEX) >> 15;
							outValue = TableValue[index];
							return(true);	/* outValue is overwritten. */
						}

						public bool Pack(_Type[] tableDataRaw, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(false);
						}

						public bool PackTrigger(_Type[] tableDataRaw, int[] tableFrameKey, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(false);
						}

						public bool Unpack(ref _Type[] outTableDataRaw, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{
							return(false);
						}

						public bool UnpackTrigger(ref _Type[] outTableDataRaw, ref int[] outTableFrameKey, int frameMax, ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument)
						{	/* Not-Supported */
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
					[System.Serializable]
					public class PackAttributeUserData : Base<Library_SpriteStudio6.Data.Animation.Attribute.UserData>
					{
					}
					[System.Serializable]
					public class PackAttributeInstance : Base<Library_SpriteStudio6.Data.Animation.Attribute.Instance>
					{
					}
					[System.Serializable]
					public class PackAttributeEffect : Base<Library_SpriteStudio6.Data.Animation.Attribute.Effect>
					{
					}
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
