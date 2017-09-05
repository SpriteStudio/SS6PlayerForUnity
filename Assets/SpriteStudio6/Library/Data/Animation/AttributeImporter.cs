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
			public static partial class Attribute
			{
				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				/* MEMO: Use this class only in Editor (only when "UNITY_EDITOR" is defined).                                               */
				/*       Written here to make simple "PackAttribute" section, but originally should be written in the assembly for Editor.  */
				/*       Therefore, presence of this class is not mentioned in "Libray_SpriteStudio.cs". (Intentionally hidden)             */
				public static partial class Importer
				{
					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					public const string NameAttributeStatus = "Status";
					public const string NameAttributePosition = "Position";
					public const string NameAttributeRotation = "Rotation";
					public const string NameAttributeScaling = "Scaling";
					public const string NameAttributeRateOpacity = "RateOpacity";
					public const string NameAttributePositionAnchor = "PositionAnchor";
					public const string NameAttributeSizeForce = "SizeForce";
					public const string NameAttributeUserData = "UserData";
					public const string NameAttributeInstance = "Instance";
					public const string NameAttributeEffect = "Effect";
					public const string NameAttributeRadiusCollision = "RadiusCollision";

					public const string NameAttributePlainCell = "Plain_Cell";
					public const string NameAttributePlainColorBlend = "Plain_ColorBlend";
					public const string NameAttributePlainVertexCorrection = "Plain_VertexCorrection";
					public const string NameAttributePlainOffsetPivot = "Plain_OffsetPivot";
					public const string NameAttributePlainPositionTexture = "Plain_PositionTexture";
					public const string NameAttributePlainScalingTexture = "Plain_ScalingTexture";
					public const string NameAttributePlainRotationTexture = "Plain_RotationTexture";

					public const string NameAttributeFixIndexCellMap = "Fix_IndexCellMap";
					public const string NameAttributeFixCoordinate = "Fix_Coordinate";
					public const string NameAttributeFixColorBlend = "Fix_ColorBlend";
					public const string NameAttributeFixUV0 = "Fix_UV0";
					public const string NameAttributeFixSizeCollision = "Fix_SizeCollision";
					public const string NameAttributeFixPivotCollision = "Fix_PivotCollision";
					#endregion Enums & Constants

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					public static class PackAttribute
					{
						/* ----------------------------------------------- Classes, Structs & Interfaces */
						#region Classes, Structs & Interfaces
						public interface Container<_Type>
						{
							/* ----------------------------------------------- Functions */
							#region Functions
							bool Pack(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument,
										int countFrame,
										string nameAttribute,
										Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus flagStatusParts,
										int[] tableOrderDraw,
										params _Type[] listKeyData
									);
							#endregion Functions
						}

						public interface ContainerBool : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool> {}
						public interface ContainerInt : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt> {}
						public interface ContainerFloat : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat> {}
						public interface ContainerStatus : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool> {}
						public interface ContainerCell : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell> {}
						public interface ContainerColorBlend : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlend> {}
						public interface ContainerVertexCorrection : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection> {}
						public interface ContainerUserData : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUserData> {}
						public interface ContainerInstance : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInstance> {}
						public interface ContainerEffect : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeEffect> {}
						public interface ContainerCoordinateFix : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCoordinateFix> {}
						public interface ContainerColorBlendFix : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlendFix> {}
						public interface ContainerUVFix : Container<Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUVFix> {}
						#endregion Classes, Structs & Interfaces
					}

					public class AttributeBool : Attribute<bool>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out bool valueOutput, int frame)
						{
							int indexStart = IndexGetFramePrevious(frame);
							if(0 <= indexStart)
							{
								valueOutput = ListKey[indexStart].Value;
								return(true);
							}

							valueOutput = false;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeInt : Attribute<int>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out int valueOutput, int frame)
						{
							int indexStart = IndexGetFramePrevious(frame);
							int indexEnd = IndexGetFrameNext(frame);

							if(0 > frame)
							{
								goto ValueGet_ErrorEnd;
							}
							if(0 > indexStart)
							{	/* Front blank */
								if(0 > indexEnd)
								{	/* No Key */
									goto ValueGet_ErrorEnd;
								}
								valueOutput = ListKey[indexEnd].Value;
								return(true);
							}
							else
							{
								if(0 > indexEnd)
								{	/* End Blank */
									valueOutput = ListKey[indexStart].Value;
									return(true);
								}
							}

							float value = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																										frame,
																										ListKey[indexStart].Frame,
																										(float)ListKey[indexStart].Value,
																										ListKey[indexEnd].Frame,
																										(float)ListKey[indexEnd].Value,
																										ListKey[indexStart].FrameCurveStart,
																										ListKey[indexStart].ValueCurveStart,
																										ListKey[indexStart].FrameCurveEnd,
																										ListKey[indexStart].ValueCurveEnd
																									);
							valueOutput = (int)value;
							return(true);

						ValueGet_ErrorEnd:;
							valueOutput = 0;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeFloat : Attribute<float>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out float valueOutput, int frame)
						{
							int indexStart = IndexGetFramePrevious(frame);
							int indexEnd = IndexGetFrameNext(frame);

							if(0 > frame)
							{
								goto ValueGet_ErrorEnd;
							}
							if(0 > indexStart)
							{	/* Front blank */
								if(0 > indexEnd)
								{	/* No Key */
									goto ValueGet_ErrorEnd;
								}
								valueOutput = ListKey[indexEnd].Value;
								return(true);
							}
							else
							{
								if(0 > indexEnd)
								{	/* End Blank */
									valueOutput = ListKey[indexStart].Value;
									return(true);
								}
							}

							valueOutput = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																										frame,
																										ListKey[indexStart].Frame,
																										ListKey[indexStart].Value,
																										ListKey[indexEnd].Frame,
																										ListKey[indexEnd].Value,
																										ListKey[indexStart].FrameCurveStart,
																										ListKey[indexStart].ValueCurveStart,
																										ListKey[indexStart].FrameCurveEnd,
																										ListKey[indexStart].ValueCurveEnd
																									);
							return(true);

						ValueGet_ErrorEnd:;
							valueOutput = 0.0f;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeVector2 : Attribute<Vector2>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out Vector2 valueOutput, int frame)
						{
							int indexStart = IndexGetFramePrevious(frame);
							int indexEnd = IndexGetFrameNext(frame);

							if(0 > frame)
							{
								goto ValueGet_ErrorEnd;
							}
							if(0 > indexStart)
							{	/* Front blank */
								if(0 > indexEnd)
								{	/* No Key */
									goto ValueGet_ErrorEnd;
								}
								valueOutput = ListKey[indexEnd].Value;
								return(true);
							}
							else
							{
								if(0 > indexEnd)
								{	/* End Blank */
									valueOutput = ListKey[indexStart].Value;
									return(true);
								}
							}

							valueOutput.x = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																										frame,
																										ListKey[indexStart].Frame,
																										ListKey[indexStart].Value.x,
																										ListKey[indexEnd].Frame,
																										ListKey[indexEnd].Value.x,
																										ListKey[indexStart].FrameCurveStart,
																										ListKey[indexStart].ValueCurveStart,
																										ListKey[indexStart].FrameCurveEnd,
																										ListKey[indexStart].ValueCurveEnd
																									);
							valueOutput.y = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																										frame,
																										ListKey[indexStart].Frame,
																										ListKey[indexStart].Value.y,
																										ListKey[indexEnd].Frame,
																										ListKey[indexEnd].Value.y,
																										ListKey[indexStart].FrameCurveStart,
																										ListKey[indexStart].ValueCurveStart,
																										ListKey[indexStart].FrameCurveEnd,
																										ListKey[indexStart].ValueCurveEnd
																									);
							return(true);

						ValueGet_ErrorEnd:;
							valueOutput = Vector2.zero;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeVector3 : Attribute<Vector3>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out Vector3 valueOutput, int frame)
						{
							int indexStart = IndexGetFramePrevious(frame);
							int indexEnd = IndexGetFrameNext(frame);

							if(0 > frame)
							{
								goto ValueGet_ErrorEnd;
							}
							if(0 > indexStart)
							{	/* Front blank */
								if(0 > indexEnd)
								{	/* No Key */
									goto ValueGet_ErrorEnd;
								}
								valueOutput = ListKey[indexEnd].Value;
								return(true);
							}
							else
							{
								if(0 > indexEnd)
								{	/* End Blank */
									valueOutput = ListKey[indexStart].Value;
									return(true);
								}
							}

							valueOutput.x = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																										frame,
																										ListKey[indexStart].Frame,
																										ListKey[indexStart].Value.x,
																										ListKey[indexEnd].Frame,
																										ListKey[indexEnd].Value.x,
																										ListKey[indexStart].FrameCurveStart,
																										ListKey[indexStart].ValueCurveStart,
																										ListKey[indexStart].FrameCurveEnd,
																										ListKey[indexStart].ValueCurveEnd
																									);
							valueOutput.y = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																										frame,
																										ListKey[indexStart].Frame,
																										ListKey[indexStart].Value.y,
																										ListKey[indexEnd].Frame,
																										ListKey[indexEnd].Value.y,
																										ListKey[indexStart].FrameCurveStart,
																										ListKey[indexStart].ValueCurveStart,
																										ListKey[indexStart].FrameCurveEnd,
																										ListKey[indexStart].ValueCurveEnd
																									);
							valueOutput.z = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																										frame,
																										ListKey[indexStart].Frame,
																										ListKey[indexStart].Value.z,
																										ListKey[indexEnd].Frame,
																										ListKey[indexEnd].Value.z,
																										ListKey[indexStart].FrameCurveStart,
																										ListKey[indexStart].ValueCurveStart,
																										ListKey[indexStart].FrameCurveEnd,
																										ListKey[indexStart].ValueCurveEnd
																									);
							return(true);

						ValueGet_ErrorEnd:;
							valueOutput = Vector3.zero;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeUserData : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.UserData>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out Library_SpriteStudio6.Data.Animation.Attribute.UserData valueOutput, int frame)
						{
							int count = (null != ListKey) ? ListKey.Count : 0;
							for(int i=0; i<count; i++)
							{
								if(ListKey[i].Frame == frame)
								{
									valueOutput = ListKey[i].Value;
									return(true);
								}
							}

							valueOutput = Library_SpriteStudio6.Data.Animation.Attribute.DefaultUseData;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeCell : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.Cell>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out Library_SpriteStudio6.Data.Animation.Attribute.Cell valueOutput, int frame)
						{
							int indexStart = IndexGetFramePrevious(frame);
							int indexEnd = IndexGetFrameNext(frame);

							if(0 > frame)
							{
								goto ValueGet_ErrorEnd;
							}
							if(0 > indexStart)
							{	/* Front blank */
								if(0 > indexEnd)
								{	/* No Key */
									goto ValueGet_ErrorEnd;
								}
								valueOutput = ListKey[indexEnd].Value;
								return(true);
							}

							valueOutput = ListKey[indexStart].Value;
							return(true);

						ValueGet_ErrorEnd:;
							valueOutput = Library_SpriteStudio6.Data.Animation.Attribute.DefaultCell;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeColorBlend : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend valueOutput, int frame)
						{
							int count = (int)Library_SpriteStudio6.KindVertex.TERMINATOR2;
							int indexStart = IndexGetFramePrevious(frame);
							int indexEnd = IndexGetFrameNext(frame);

							if(0 > frame)
							{
								goto ValueGet_ErrorEnd;
							}
							if(0 > indexStart)
							{	/* Front blank */
								if(0 > indexEnd)
								{	/* No Key */
									goto ValueGet_ErrorEnd;
								}
								valueOutput = ListKey[indexEnd].Value;
								return(false);
							}
							else
							{
								if(0 > indexEnd)
								{	/* End Blank */
									valueOutput = ListKey[indexStart].Value;
									return(false);
								}
							}

							valueOutput.Bound = ListKey[indexStart].Value.Bound;
							valueOutput.Operation = ListKey[indexStart].Value.Operation;
							valueOutput.VertexColor = new Color[count];
							valueOutput.RatePixelAlpha = new float[count];
							for(int i=0; i<count; i++)
							{
#if false
								/* MEMO: SpriteStudio Ver.5.0-5.2 */
								valueOutput.VertexColor[i].r = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																															frame,
																															ListKey[indexStart].Frame,
																															ListKey[indexStart].Value.VertexColor[i].r,
																															ListKey[indexEnd].Frame,
																															ListKey[indexEnd].Value.VertexColor[i].r,
																															ListKey[indexStart].FrameCurveStart,
																															ListKey[indexStart].ValueCurveStart,
																															ListKey[indexStart].FrameCurveEnd,
																															ListKey[indexStart].ValueCurveEnd
																														);
								valueOutput.VertexColor[i].g = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																															frame,
																															ListKey[indexStart].Frame,
																															ListKey[indexStart].Value.VertexColor[i].g,
																															ListKey[indexEnd].Frame,
																															ListKey[indexEnd].Value.VertexColor[i].g,
																															ListKey[indexStart].FrameCurveStart,
																															ListKey[indexStart].ValueCurveStart,
																															ListKey[indexStart].FrameCurveEnd,
																															ListKey[indexStart].ValueCurveEnd
																														);
								valueOutput.VertexColor[i].b = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																															frame,
																															ListKey[indexStart].Frame,
																															ListKey[indexStart].Value.VertexColor[i].b,
																															ListKey[indexEnd].Frame,
																															ListKey[indexEnd].Value.VertexColor[i].b,
																															ListKey[indexStart].FrameCurveStart,
																															ListKey[indexStart].ValueCurveStart,
																															ListKey[indexStart].FrameCurveEnd,
																															ListKey[indexStart].ValueCurveEnd
																														);
								valueOutput.VertexColor[i].a = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																															frame,
																															ListKey[indexStart].Frame,
																															ListKey[indexStart].Value.VertexColor[i].a,
																															ListKey[indexEnd].Frame,
																															ListKey[indexEnd].Value.VertexColor[i].a,
																															ListKey[indexStart].FrameCurveStart,
																															ListKey[indexStart].ValueCurveStart,
																															ListKey[indexStart].FrameCurveEnd,
																															ListKey[indexStart].ValueCurveEnd
																														);

								valueOutput.RatePixelAlpha[i] = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																															frame,
																															ListKey[indexStart].Frame,
																															ListKey[indexStart].Value.RatePixelAlpha[i],
																															ListKey[indexEnd].Frame,
																															ListKey[indexEnd].Value.RatePixelAlpha[i],
																															ListKey[indexStart].FrameCurveStart,
																															ListKey[indexStart].ValueCurveStart,
																															ListKey[indexStart].FrameCurveEnd,
																															ListKey[indexStart].ValueCurveEnd
																														);
#else
								/* MEMO: SpriteStudio Ver.5.2- or Ver -4.x */
								float rate = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																										frame,
																										ListKey[indexStart].Frame,
																										0.0f,
																										ListKey[indexEnd].Frame,
																										1.0f,
																										ListKey[indexStart].FrameCurveStart,
																										ListKey[indexStart].ValueCurveStart,
																										ListKey[indexStart].FrameCurveEnd,
																										ListKey[indexStart].ValueCurveEnd
																									);
								rate = Mathf.Clamp01(rate);

								valueOutput.VertexColor[i].r = Library_SpriteStudio6.Utility.Interpolation.Linear(ListKey[indexStart].Value.VertexColor[i].r, ListKey[indexStart].Value.VertexColor[i].r, rate);
								valueOutput.VertexColor[i].g = Library_SpriteStudio6.Utility.Interpolation.Linear(ListKey[indexStart].Value.VertexColor[i].g, ListKey[indexStart].Value.VertexColor[i].g, rate);
								valueOutput.VertexColor[i].b = Library_SpriteStudio6.Utility.Interpolation.Linear(ListKey[indexStart].Value.VertexColor[i].b, ListKey[indexStart].Value.VertexColor[i].b, rate);
								valueOutput.VertexColor[i].a = Library_SpriteStudio6.Utility.Interpolation.Linear(ListKey[indexStart].Value.VertexColor[i].a, ListKey[indexStart].Value.VertexColor[i].a, rate);
								valueOutput.RatePixelAlpha[i] = Library_SpriteStudio6.Utility.Interpolation.Linear(ListKey[indexStart].Value.RatePixelAlpha[i], ListKey[indexStart].Value.RatePixelAlpha[i], rate);
#endif
							}
							return(true);

						ValueGet_ErrorEnd:;
							valueOutput = Library_SpriteStudio6.Data.Animation.Attribute.DefaultColorBlend;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeVertexCorrection : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection valueOutput, int frame)
						{	/* MEMO: This attribute does not consider inheritance. */
							int count = (int)Library_SpriteStudio6.KindVertex.TERMINATOR2;
							int indexStart = IndexGetFramePrevious(frame);
							int indexEnd = IndexGetFrameNext(frame);

							if(0 > frame)
							{
								goto ValueGet_ErrorEnd;
							}
							if(0 > indexStart)
							{	/* Front blank */
								if(0 > indexEnd)
								{	/* No Key */
									goto ValueGet_ErrorEnd;
								}
								valueOutput = ListKey[indexEnd].Value;
								return(false);
							}
							else
							{
								if(0 > indexEnd)
								{	/* End Blank */
									valueOutput = ListKey[indexStart].Value;
									return(false);
								}
							}

							valueOutput.Coordinate = new Vector2[count];
							for(int i=0; i<count; i++)
							{
								valueOutput.Coordinate[i].x = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																															frame,
																															ListKey[indexStart].Frame,
																															ListKey[indexStart].Value.Coordinate[i].x,
																															ListKey[indexEnd].Frame,
																															ListKey[indexEnd].Value.Coordinate[i].x,
																															ListKey[indexStart].FrameCurveStart,
																															ListKey[indexStart].ValueCurveStart,
																															ListKey[indexStart].FrameCurveEnd,
																															ListKey[indexStart].ValueCurveEnd
																														);
								valueOutput.Coordinate[i].y = Library_SpriteStudio6.Utility.Interpolation.ValueGetFloat(	ListKey[indexStart].Formula,
																															frame,
																															ListKey[indexStart].Frame,
																															ListKey[indexStart].Value.Coordinate[i].y,
																															ListKey[indexEnd].Frame,
																															ListKey[indexEnd].Value.Coordinate[i].y,
																															ListKey[indexStart].FrameCurveStart,
																															ListKey[indexStart].ValueCurveStart,
																															ListKey[indexStart].FrameCurveEnd,
																															ListKey[indexStart].ValueCurveEnd
																														);
							}
							return(true);

						ValueGet_ErrorEnd:;
							valueOutput = Library_SpriteStudio6.Data.Animation.Attribute.DefaultVertexCorrection;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeInstance : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.Instance>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out Library_SpriteStudio6.Data.Animation.Attribute.Instance valueOutput, int frame)
						{
							int count = (null != ListKey) ? ListKey.Count : 0;
							for(int i=0; i<count; i++)
							{
								if(ListKey[i].Frame == frame)
								{
									valueOutput = ListKey[i].Value;
									return(true);
								}
							}

							valueOutput = Library_SpriteStudio6.Data.Animation.Attribute.DefaultInstance;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeEffect : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.Effect>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out Library_SpriteStudio6.Data.Animation.Attribute.Effect valueOutput, int frame)
						{
							int count = (null != ListKey) ? ListKey.Count : 0;
							for(int i=0; i<count; i++)
							{
								if(ListKey[i].Frame == frame)
								{
									valueOutput = ListKey[i].Value;
									return(true);
								}
							}

							valueOutput = Library_SpriteStudio6.Data.Animation.Attribute.DefaultEffect;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeCoordinateFix : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix valueOutput, int frame)
						{	/* MEMO: This attribute has keyframes in all frames. */
							int count = CountGetKey();
							if((0 >= count) || (0 > frame))
							{
								goto ValueGet_ErrorEnd;
							}
							if(count <= frame)
							{
								valueOutput = ListKey[count - 1].Value;
								return(true);
							}
							if(ListKey[frame].Frame != frame)
							{	/* Is the key missing ?? */
								goto ValueGet_ErrorEnd;
							}

							valueOutput = ListKey[frame].Value;
							return(true);
	
						ValueGet_ErrorEnd:;
							valueOutput = Library_SpriteStudio6.Data.Animation.Attribute.DefaultCoordinateFix;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeColorBlendFix : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix valueOutput, int frame)
						{	/* MEMO: This attribute has keyframes in all frames. */
							int count = CountGetKey();
							if((0 >= count) || (0 > frame))
							{
								goto ValueGet_ErrorEnd;
							}
							if(count <= frame)
							{
								valueOutput = ListKey[count - 1].Value;
								return(true);
							}
							if(ListKey[frame].Frame != frame)
							{	/* Is the key missing ?? */
								goto ValueGet_ErrorEnd;
							}

							valueOutput = ListKey[frame].Value;
							return(true);
	
						ValueGet_ErrorEnd:;
							valueOutput = Library_SpriteStudio6.Data.Animation.Attribute.DefaultColorBlendFix;
							return(false);
						}
						#endregion Functions
					}
					public class AttributeUVFix : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.UVFix>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(out Library_SpriteStudio6.Data.Animation.Attribute.UVFix valueOutput, int frame)
						{	/* MEMO: This attribute has keyframes in all frames. */
							int count = CountGetKey();
							if((0 >= count) || (0 > frame))
							{
								goto ValueGet_ErrorEnd;
							}
							if(count <= frame)
							{
								valueOutput = ListKey[count - 1].Value;
								return(true);
							}
							if(ListKey[frame].Frame != frame)
							{	/* Is the key missing ?? */
								goto ValueGet_ErrorEnd;
							}

							valueOutput = ListKey[frame].Value;
							return(true);
	
						ValueGet_ErrorEnd:;
							valueOutput = Library_SpriteStudio6.Data.Animation.Attribute.DefaultUVFix;
							return(false);
						}
						#endregion Functions
					}

					public abstract class Attribute<_Type>
						where _Type : struct
					{
						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						public List<KeyData> ListKey;
						public Attribute<_Type> Parent;
						#endregion Variables & Properties

						/* ----------------------------------------------- Functions */
						#region Functions
						public abstract bool ValueGet(out _Type valueOutput, int frame);

						public void CleanUp()
						{
							ListKey = null;
							Parent = null;
						}

						public bool BootUp()
						{
							ListKey = new List<KeyData>();
							ListKey.Clear();

							Parent = null;

							return(true);
						}

						public void ShutDown()
						{
							if(null != ListKey)
							{
								ListKey.Clear();
							}
							ListKey = null;
							Parent = null;
						}

						public int CountGetKey()
						{
							return((null == ListKey) ? 0 : ListKey.Count);
						}

						public int IndexGetFramePrevious(int frame)
						{
							if((null != ListKey) && (0 <= frame))
							{
								int indexPrevious = -1;
								int count = ListKey.Count;
								for(int i=0; i<count; i++)
								{
									int frameNow = ListKey[i].Frame;
									if(frameNow == frame)
									{
										return(i);
									}
									if(frameNow > frame)
									{
										return(indexPrevious);
									}
									indexPrevious = i;
								}
								return(indexPrevious);
							}
							return(-1);
						}

						public int IndexGetFrameNext(int frame)
						{
							if((null != ListKey) && (0 <= frame))
							{
								int count = ListKey.Count;
								for(int i=0; i<count; i++)
								{
									if(ListKey[i].Frame > frame)
									{
										return(i);
									}
								}
								return(count - 1);
							}
							return(-1);
						}

						public void KeyDataAdjustTopFrame()
						{
							if(0 >= ListKey.Count)
							{	/* No Keys */
								return;
							}

							if(0 < ListKey[0].Frame)
							{
								/* Create Top Key-Data */
								/* MEMO: Same value. However, "frame = 0" and "no interpolation". */
								KeyData KeyDataTopFrame = new KeyData();
								KeyDataTopFrame.Frame = 0;
								KeyDataTopFrame.Value = ListKey[0].Value;
								KeyDataTopFrame.Formula = Utility.Interpolation.KindFormula.NON;
								KeyDataTopFrame.FrameCurveStart = 0.0f;
								KeyDataTopFrame.ValueCurveStart = 0.0f;
								KeyDataTopFrame.FrameCurveEnd = 0.0f;
								KeyDataTopFrame.ValueCurveEnd = 0.0f;

								ListKey.Insert(0, KeyDataTopFrame);
							}
						}

						public void KeyDataAdjustTopFrame(_Type valueDefault)
						{
							if(0 >= ListKey.Count)
							{	/* No Keys */
								return;
							}

							if(0 < ListKey[0].Frame)
							{
								/* Create Top Key-Data */
								KeyData KeyDataTopFrame = new KeyData();
								KeyDataTopFrame.Frame = 0;
								KeyDataTopFrame.Value = valueDefault;
								KeyDataTopFrame.Formula = Utility.Interpolation.KindFormula.NON;
								KeyDataTopFrame.FrameCurveStart = 0.0f;
								KeyDataTopFrame.ValueCurveStart = 0.0f;
								KeyDataTopFrame.FrameCurveEnd = 0.0f;
								KeyDataTopFrame.ValueCurveEnd = 0.0f;

								ListKey.Insert(0, KeyDataTopFrame);
							}
						}
						#endregion Functions

						/* ----------------------------------------------- Classes, Structs & Interfaces */
						#region Classes, Structs & Interfaces
						public class KeyData
						{
							/* ----------------------------------------------- Variables & Properties */
							#region Variables & Properties
							public int Frame;
							public _Type Value;

							public Library_SpriteStudio6.Utility.Interpolation.KindFormula Formula;
							public float FrameCurveStart;
							public float ValueCurveStart;
							public float FrameCurveEnd;
							public float ValueCurveEnd;
							#endregion Variables & Properties

							/* ----------------------------------------------- Functions */
							#region Functions
							public void CleanUp()
							{
								Frame = -1;	/* Frame-Value Invalid */
								Value = default(_Type);

								Formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.NON;
								FrameCurveStart = 0.0f;
								ValueCurveStart = 0.0f;
								FrameCurveEnd = 0.0f;
								ValueCurveEnd = 0.0f;
							}
							#endregion Functions
						}
						#endregion Classes, Structs & Interfaces
					}

					public static class Inheritance
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public static bool ValueGetBoolToggle(out bool valueOutput, Library_SpriteStudio6.Data.Animation.Attribute.Importer.Attribute<bool> attribute, int frame)
						{	/* MEMO: Mainly used for acquiring inheritance value of "Flip". */
							bool valueParent = false;
							if(null != attribute.Parent)
							{
								ValueGetBoolToggle(out valueParent, attribute.Parent, frame);
							}

							bool value;
							attribute.ValueGet(out value, frame);	/* "value" will always be false in case of error. */

							valueOutput = (true == value) ? !valueParent : valueParent;
							return(true);

//						ValueGetBoolToggle_ErrorEnd:;
//							valueOutput = false;
//							return(false);
						}

						public static bool ValueGetBoolOR(out bool valueOutput, Library_SpriteStudio6.Data.Animation.Attribute.Importer.Attribute<bool> attribute, int frame, bool valueDefault)
						{	/* MEMO: Mainly used for acquiring inheritance value of "Hide". */
							bool value = valueDefault;
							if(false == attribute.ValueGet(out value, frame))
							{
								value = valueDefault;
							}
							if(true == value)
							{
								valueOutput = true;
								return(true);
							}

							bool valueParent = false;
							if(null != attribute.Parent)
							{
								ValueGetBoolOR(out valueParent, attribute.Parent, frame, valueDefault);
							}

							valueOutput = valueParent;	/* valueParent | value *//* "value" is always false. */
							return(true);

//						ValueGetBoolOR_ErrorEnd:;
//							valueOutput = valueDefault;
//							return(false);
						}

						public static bool ValueGetFloatMultiple(out float valueOutput, Library_SpriteStudio6.Data.Animation.Attribute.Importer.Attribute<float> attribute, int frame, float valueDefault)
						{
							float valueParent = 1.0f;
							if(null != attribute.Parent)
							{
								ValueGetFloatMultiple(out valueParent, attribute.Parent, frame, valueDefault);
							}

							float value = valueDefault;
							if(false == attribute.ValueGet(out value, frame))
							{
								value = valueDefault;
							}

							valueOutput = valueParent * value;
							return(true);

//						ValueGetFloatMultiple_ErrorEnd:;
//							valueOutput = valueDefault;
//							return(false);
						}
						#endregion Functions
					}
					#endregion Classes, Structs & Interfaces
				}
				#endregion Classes, Structs & Interfaces
			}
		}
	}
}
