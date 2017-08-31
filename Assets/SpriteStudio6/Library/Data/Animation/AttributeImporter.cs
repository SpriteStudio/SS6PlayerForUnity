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
				/* MEMO: Use this class only in Editor(only when "UNITY_EDITOR" is defined).                                                */
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
					public const string NameAttributePriority = "Priority";
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
							bool Pack(ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argument, int frameMax, string nameAttribute, params _Type[] listKeyData);
							#endregion Functions
						}
						#endregion Classes, Structs & Interfaces
					}

					public class AttributeBool : Attribute<bool>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override bool ValueGet(int frame)
						{
							return(false);
						}
						#endregion Functions
					}
					public class AttributeInt : Attribute<int>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override int ValueGet(int frame)
						{
							return(0);
						}
						#endregion Functions
					}
					public class AttributeFloat : Attribute<float>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override float ValueGet(int frame)
						{
							return(0);
						}
						#endregion Functions
					}
					public class AttributeVector2 : Attribute<Vector2>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Vector2 ValueGet(int frame)
						{
							return(Vector2.zero);
						}
						#endregion Functions
					}
					public class AttributeVector3 : Attribute<Vector3>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Vector3 ValueGet(int frame)
						{
							return(Vector2.zero);
						}
						#endregion Functions
					}
					public class AttributeUserData : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.UserData>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.Attribute.UserData ValueGet(int frame)
						{
							return(default(Library_SpriteStudio6.Data.Animation.Attribute.UserData));
						}
						#endregion Functions
					}
					public class AttributeCell : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.Cell>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.Attribute.Cell ValueGet(int frame)
						{
							return(default(Library_SpriteStudio6.Data.Animation.Attribute.Cell));
						}
						#endregion Functions
					}
					public class AttributeColorBlend : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend ValueGet(int frame)
						{
							return(default(Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend));
						}
						#endregion Functions
					}
					public class AttributeVertexCorrection : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection ValueGet(int frame)
						{
							return(default(Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection));
						}
						#endregion Functions
					}
					public class AttributeInstance : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.Instance>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.Attribute.Instance ValueGet(int frame)
						{
							return(default(Library_SpriteStudio6.Data.Animation.Attribute.Instance));
						}
						#endregion Functions
					}
					public class AttributeEffect : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.Effect>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.Attribute.Effect ValueGet(int frame)
						{
							return(default(Library_SpriteStudio6.Data.Animation.Attribute.Effect));
						}
						#endregion Functions
					}
					public class AttributeCoordinateFix : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix ValueGet(int frame)
						{
							return(default(Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix));
						}
						#endregion Functions
					}
					public class AttributeColorBlendFix : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix ValueGet(int frame)
						{
							return(default(Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix));
						}
						#endregion Functions
					}
					public class AttributeUVFix : Attribute<Library_SpriteStudio6.Data.Animation.Attribute.UVFix>
					{
						/* ----------------------------------------------- Functions */
						#region Functions
						public override Library_SpriteStudio6.Data.Animation.Attribute.UVFix ValueGet(int frame)
						{
							return(default(Library_SpriteStudio6.Data.Animation.Attribute.UVFix));
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
						public abstract _Type ValueGet(int frame);

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
								KeyData KeyDataTopFrame = ListKey[0];
								KeyDataTopFrame.Frame = 0;
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
					#endregion Classes, Structs & Interfaces
				}
				#endregion Classes, Structs & Interfaces
			}
		}
	}
}
