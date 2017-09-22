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
	public static partial class Control
	{
		public static partial class Animation
		{
			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			[System.Serializable]
			public partial struct Parts
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				internal int IDParts;
				internal FlagBitStatus Status;
				internal int IndexControlTrack;

				public GameObject InstanceGameObject;
				internal Transform InstanceTransform;

				public Object PrefabUnderControl;
				public GameObject InstanceGameObjectUnderControl;
				internal Script_SpriteStudio6_Root InstanceRootUnderControl;
				internal int IndexAnimationUnderControl;
				internal Script_SpriteStudio6_RootEffect InstanceRootEffectUnderControl;

				public Collider InstanceComponentCollider;

				internal Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus StatusAnimationParts;
				private int FrameKeyStatusAnimationFrame;
				private Library_SpriteStudio6.Data.Animation.Attribute.Status StatusAnimationFrame;
				internal int IDPartsDrawNext
				{
					get
					{
						return(StatusAnimationFrame.IDPartsNext);
					}
				}

				internal BufferTRS TRSMaster;
				internal BufferTRS TRSSlave;

				internal BufferParameterSprite ParameterSprite;
				/* Mesh Buffer */
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				internal void CleanUp()
				{
					IDParts = -1;
					Status = FlagBitStatus.CLEAR;
					IndexControlTrack = 0;

//					InstanceGameObject =
					InstanceTransform = null;

//					PrefabUnderControl =
//					InstanceGameObjectUnderControl =
					InstanceRootUnderControl = null;
					IndexAnimationUnderControl = -1;
					InstanceRootEffectUnderControl = null;

//					InstanceComponentCollider =

					StatusAnimationParts = Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.CLEAR;
					FrameKeyStatusAnimationFrame = -1;
					StatusAnimationFrame.CleanUp();

					TRSMaster.CleanUp();
					TRSSlave.CleanUp();

					ParameterSprite.CleanUp();
				}

				internal bool BootUp(Script_SpriteStudio6_Root instanceRoot, int idParts)
				{
					CleanUp();

					IDParts = idParts;
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

					/* Boot up for each part feature */
					switch(instanceRoot.DataAnimation.TableParts[idParts].Feature)
					{
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
							break;

						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
							/* MEMO: Erase, because can not have undercontrol object. */
							PrefabUnderControl = null;
							InstanceGameObjectUnderControl = null;

							/* Clean up Sprite/Mesh Buffer */
							if(false == ParameterSprite.BootUp((int)Library_SpriteStudio6.KindVertex.TERMINATOR2))
							{
								goto BootUp_ErrorEnd;
							}
							break;

						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
							/* MEMO: Erase, because can not have undercontrol object. */
							PrefabUnderControl = null;
							InstanceGameObjectUnderControl = null;

							if(false == ParameterSprite.BootUp((int)Library_SpriteStudio6.KindVertex.TERMINATOR4))
							{
								goto BootUp_ErrorEnd;
							}
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
							InstanceRootUnderControl.InstanceRootParent = instanceRoot;

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
							InstanceRootEffectUnderControl.InstanceRootParent = instanceRoot;
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

					/* Set Position, Rotation and Scaling */
					Transform transform = InstanceGameObject.transform;
					Data.Animation.PackAttribute.ArgumentContainer argument = new Data.Animation.PackAttribute.ArgumentContainer();
					argument.Frame = 0;

					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION))
					{
						if(true == dataAnimationParts.Position.Function.ValueGet(ref TRSMaster.Position.Value, ref TRSMaster.Position.FrameKey, dataAnimationParts.Position, ref controlTrack.ArgumentContainer))
						{	/* New Value */
							transform.localPosition = TRSMaster.Position.Value;
						}
					}

					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION))
					{
						if(true == dataAnimationParts.Rotation.Function.ValueGet(ref TRSMaster.Rotation.Value, ref TRSMaster.Rotation.FrameKey, dataAnimationParts.Rotation, ref controlTrack.ArgumentContainer))
						{	/* New Value */
							transform.localEulerAngles = TRSMaster.Rotation.Value;
						}
					}

					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING))
					{
						if(true == dataAnimationParts.Scaling.Function.ValueGet(ref TRSMaster.Scaling.Value, ref TRSMaster.Scaling.FrameKey, dataAnimationParts.Scaling, ref controlTrack.ArgumentContainer))
						{	/* New Value */
							Vector3 scaling = TRSMaster.Scaling.Value;
							scaling.z = 1.0f;
							transform.localScale = scaling;
						}
					}

					/* Get Status */
					dataAnimationParts.Status.Function.ValueGet(ref StatusAnimationFrame, ref FrameKeyStatusAnimationFrame, dataAnimationParts.Status, ref controlTrack.ArgumentContainer);

					/* User Data */
				}

				internal void UpdateDraw(Script_SpriteStudio6_Root instanceRoot, int idParts)
				{
					int indexTrack = IndexControlTrack;
					int indexAnimation = instanceRoot.TableControlTrack[indexTrack].ArgumentContainer.IndexAnimation;
					if(0 <= indexAnimation)
					{
						ParameterSprite.StatusSet(ref StatusAnimationFrame);

						switch(instanceRoot.DataAnimation.TableParts[idParts].Feature)
						{
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
								break;
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
								UpdateDrawNormal(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
								break;
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
								/* Update Instance */
//								UpdateDrawInstance(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
								break;
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
								/* Update Effect */
//								UpdateDrawEffect(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
								break;
						}
					}
				}
				private void UpdateDrawNormal(	Script_SpriteStudio6_Root instanceRoot,
												int idParts,
												ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
												ref Library_SpriteStudio6.Control.Animation.Track controlTrack
											)
				{
					controlTrack.ArgumentContainer.IDParts = idParts;

					/* Update Sprite */
					switch(dataAnimationParts.Format)
					{
						case Library_SpriteStudio6.Data.Animation.Parts.KindFormat.PLAIN:
							ParameterSprite.Plain(instanceRoot, idParts, InstanceGameObject, InstanceTransform, ref Status, ref dataAnimationParts, ref controlTrack.ArgumentContainer);
							break;
						case Library_SpriteStudio6.Data.Animation.Parts.KindFormat.FIX:
							ParameterSprite.Fix(instanceRoot, idParts, InstanceGameObject, InstanceTransform, ref Status, ref dataAnimationParts, ref controlTrack.ArgumentContainer);
							break;
					}

					/* Draw Mesh */

				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				[System.Flags]
				internal enum FlagBitStatus
				{
					VALID = 0x40000000,
					RUNNING = 0x20000000,

					HIDE_FORCE = 0x08000000,

//					CHANGE_TRANSFORM_POSITION = 0x00100000,
//					CHANGE_TRANSFORM_ROTATION = 0x00200000,
//					CHANGE_TRANSFORM_SCALING = 0x00400000,

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
				internal struct BufferAttribute<_Type>
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					internal int FrameKey;
					internal _Type Value;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					internal void CleanUp()
					{
						FrameKey = -1;
					}
					#endregion Functions
				}

				internal partial struct BufferTRS
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					internal BufferAttribute<Vector3> Position;
					internal BufferAttribute<Vector3> Rotation;
					internal BufferAttribute<Vector2> Scaling;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					internal void CleanUp()
					{
						Position.CleanUp();
						Position.Value = Vector3.zero;
						Rotation.CleanUp();
						Rotation.Value = Vector3.zero;
						Scaling.CleanUp();
						Scaling.Value = Vector2.one;
					}
					#endregion Functions
				}

				internal partial struct BufferParameterSprite
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					/* MEMO: Common */
					internal bool FlagHide;
					internal Vector2 RateScaleMesh;
					internal Vector2 RateScaleTexture;

					/* MEMO: Only "Data-Plain" */
					internal Vector2 SizeTexture;
					internal Vector2 SizeCell;
					internal Vector2 PivotCell;
					internal Vector2 PositionCell;
					internal Matrix4x4 MatrixTexture;
					internal Library_SpriteStudio6.Data.Animation.Attribute.Cell DataCellApply;
					internal int IndexVertexCollectionTable;

					internal BufferAttribute<Library_SpriteStudio6.Data.Animation.Attribute.Cell> DataCell;
					internal BufferAttribute<Vector2> OffsetPivot;
					internal BufferAttribute<Vector2> SizeMeshForce;
					internal BufferAttribute<Vector2> ScalingTexture;
					internal BufferAttribute<Vector2> PositionTexture;
					internal BufferAttribute<float> RotationTexture;
					internal BufferAttribute<float> RateOpacity;
					internal BufferAttribute<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend> PartsColor;
					internal BufferAttribute<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection> VertexCorrection;

					internal Material instanceMaterial;
					internal Library_SpriteStudio6.Draw.BufferMesh DrawMesh;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					internal void CleanUp()
					{
						FlagHide = false;
						RateScaleMesh = Vector2.one;
						RateScaleTexture = Vector2.one;

						SizeTexture = SizeTextureDefault;
						SizeCell = SizeTextureDefault;
						PivotCell = Vector2.zero;
						PositionCell = Vector2.zero;
						MatrixTexture = Matrix4x4.identity;
						DataCellApply.CleanUp();
						IndexVertexCollectionTable = 0;

						DataCell.CleanUp();	DataCell.Value.CleanUp();
						OffsetPivot.CleanUp();	OffsetPivot.Value = Vector2.zero;
						SizeMeshForce.CleanUp();	SizeMeshForce.Value = Vector2.zero;
						ScalingTexture.CleanUp();	ScalingTexture.Value = Vector2.one;
						PositionTexture.CleanUp();	PositionTexture.Value = Vector2.zero;
						RotationTexture.CleanUp();	RotationTexture.Value = 0.0f;
						RateOpacity.CleanUp();	RateOpacity.Value = 1.0f;
						PartsColor.CleanUp();	PartsColor.Value.CleanUp();
						VertexCorrection.CleanUp();	VertexCorrection.Value.CleanUp();

						DrawMesh.CleanUp();
					}

					internal bool BootUp(int countVertex)
					{
						CleanUp();

						PartsColor.Value.BootUp();
						VertexCorrection.Value.BootUp();

						if(false == DrawMesh.BootUp(countVertex))
						{
							return(false);
						}
						return(true);
					}

					internal void StatusSet(ref Library_SpriteStudio6.Data.Animation.Attribute.Status status)
					{
						/* Get Hide */
						FlagHide = status.IsHide;

						/* Dicide Sprite's scale (Flipping) & Vertex Order */
						IndexVertexCollectionTable = 0;
						if(true == status.IsFlipX)
						{
							RateScaleMesh.x = -1.0f;
							IndexVertexCollectionTable += 1;
						}
						else
						{
							RateScaleMesh.x = 1.0f;
						}
						if(true == status.IsFlipY)
						{
							RateScaleMesh.y = -1.0f;
							IndexVertexCollectionTable += 2;
							}
						else
						{
							RateScaleMesh.y = 1.0f;
						}

						/* Dicide Texture's scale (Flipping) */
						if(true == status.IsTextureFlipX)
						{
							RateScaleTexture.x = -1.0f;
						}
						else
						{
							RateScaleTexture.x = 1.0f;
						}
						if(true == status.IsTextureFlipY)
						{
							RateScaleTexture.y = -1.0f;
						}
						else
						{
							RateScaleTexture.y = 1.0f;
						}
					}

					internal void Plain(	Script_SpriteStudio6_Root instanceRoot,
											int idParts,
											GameObject instanceGameObject,
											Transform instanceTransform,
											ref Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus statusControlParts,
											ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
											ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer
										)
					{
						/* Create sprite data (from cell to use) */
						if(true == dataAnimationParts.Plain.Cell.Function.ValueGet(ref DataCell.Value, ref DataCell.FrameKey, dataAnimationParts.Plain.Cell, ref argumentContainer))
						{
							if(0 == (statusControlParts & (FlagBitStatus.OVERWRITE_CELL_IGNOREATTRIBUTE | FlagBitStatus.OVERWRITE_CELL_UNREFLECTED)))
							{
								/* MEMO: Use data in attribute. */
								DataCellApply = DataCell.Value;
							}
							statusControlParts &= ~FlagBitStatus.OVERWRITE_CELL_UNREFLECTED;
						}

						int indexCellMap = -1;
						int indexCell = -1;
						Library_SpriteStudio6.Data.CellMap cellMap = instanceRoot.DataGetCellMap(DataCellApply.IndexCellMap);
						if(null != cellMap)
						{	/* CellMap Valid */
							indexCell = DataCellApply.IndexCell;
							if((0 > indexCell) || (cellMap.CountGetCell() <= indexCell))
							{	/* Cell Invalid */
								indexCellMap = -1;
								indexCell = -1;
							}
							else
							{	/* Cell Valid */
								indexCellMap = DataCellApply.IndexCellMap;
								indexCell = DataCellApply.IndexCell;
							}
						}
						if(0 > indexCell)
						{	/* Invalid */
							SizeTexture = SizeTextureDefault;

							SizeCell = SizeTextureDefault;
							PivotCell = Vector2.zero;
							PositionCell =Vector2.zero;
						}
						else
						{	/* Valid */
							SizeTexture = cellMap.SizeOriginal;

							SizeCell.x = cellMap.TableCell[indexCell].Rectangle.width;
							SizeCell.y = cellMap.TableCell[indexCell].Rectangle.height;
							PivotCell = cellMap.TableCell[indexCell].Pivot;
							PositionCell.x = cellMap.TableCell[indexCell].Rectangle.xMin;
							PositionCell.y = cellMap.TableCell[indexCell].Rectangle.yMin;
						}

						int countVertex = DrawMesh.Coordinate.Length;
						Vector2 sizeMesh = SizeCell;
						Vector2 sizeMapping = SizeCell;
						Vector2 positionMapping = PositionCell;
						Vector2 pivotMesh = PivotCell;

						/* Correct Sprite data (by attributes) */
						dataAnimationParts.Plain.OffsetPivot.Function.ValueGet(ref OffsetPivot.Value, ref OffsetPivot.FrameKey, dataAnimationParts.Plain.OffsetPivot, ref argumentContainer); 
						pivotMesh.x += (sizeMesh.x * OffsetPivot.Value.x) * RateScaleMesh.x;
						pivotMesh.y -= (sizeMesh.y * OffsetPivot.Value.y) * RateScaleMesh.y;

						dataAnimationParts.SizeForce.Function.ValueGet(ref SizeMeshForce.Value, ref SizeMeshForce.FrameKey, dataAnimationParts.SizeForce, ref argumentContainer); 
						if(0 <= SizeMeshForce.FrameKey)
						{
							float ratePivot;
							float size;
							size = SizeMeshForce.Value.x;
							if(0.0f <= size)
							{
								ratePivot = pivotMesh.x / sizeMesh.x;
								sizeMesh.x = size;
								pivotMesh.x = size * ratePivot;
							}
							size = SizeMeshForce.Value.y;
							if(0.0f <= size)
							{
								ratePivot = pivotMesh.y / sizeMesh.y;
								sizeMesh.y = size;
								pivotMesh.y = size * ratePivot;
							}
						}

						/* Update Collider */


						/* Get Rate-Opacity */
						dataAnimationParts.RateOpacity.Function.ValueGet(ref RateOpacity.Value, ref RateOpacity.FrameKey, dataAnimationParts.RateOpacity, ref argumentContainer); 

						/* Calculate Texture-UV */
#if true
#if false
						if(0 != (dataAnimationParts.StatusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_TRANSFORMATION_TEXTURE))
						{	/* No Transform (Ordinary rectangle) */
							Vector2 uLR = new Vector2(positionMapping.x, positionMapping.x + sizeMapping.x);
							float mappingYInverse = SizeTexture.y - positionMapping.y;
							Vector2 vUD = new Vector2(mappingYInverse, mappingYInverse - sizeMapping.y);
							uLR /= SizeTexture.x;
							vUD /= SizeTexture.y;

							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.LU].x = uLR.x;
							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.LU].y = vUD.x;

							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.RU].x = uLR.y;
							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.RU].y = vUD.x;

							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.RD].x = uLR.y;
							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.RD].y = vUD.y;

							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.LD].x = uLR.x;
							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.LD].y = vUD.y;
						}
						else
#endif
						{	/* Transform Texure */
							bool flagUpdateMatrixTexrure = false;
							flagUpdateMatrixTexrure |= dataAnimationParts.Plain.PositionTexture.Function.ValueGet(ref PositionTexture.Value, ref PositionTexture.FrameKey, dataAnimationParts.Plain.PositionTexture, ref argumentContainer);
							flagUpdateMatrixTexrure |= dataAnimationParts.Plain.ScalingTexture.Function.ValueGet(ref ScalingTexture.Value, ref ScalingTexture.FrameKey, dataAnimationParts.Plain.ScalingTexture, ref argumentContainer);
							flagUpdateMatrixTexrure |= dataAnimationParts.Plain.RotationTexture.Function.ValueGet(ref RotationTexture.Value, ref RotationTexture.FrameKey, dataAnimationParts.Plain.RotationTexture, ref argumentContainer);
//							if(true == flagUpdateMatrixTexrure)
							{
								Vector2 centerMapping = (sizeMesh * 0.5f) + positionMapping;
								Vector3 Translation = new Vector3(	(centerMapping.x / SizeTexture.x) + PositionTexture.Value.x,
																	((SizeTexture.y - centerMapping.y) / SizeTexture.y) - PositionTexture.Value.y,
																	0.0f
																);
								Vector3 Scaling = new Vector3(	(sizeMapping.x / SizeTexture.x) * RateScaleTexture.x,
																(sizeMapping.y / SizeTexture.y) * RateScaleTexture.y,
																1.0f
															);
								Quaternion Rotation = Quaternion.Euler(0.0f, 0.0f, -RotationTexture.Value);
								MatrixTexture = Matrix4x4.TRS(Translation, Rotation, Scaling);
							}

							for(int i=0; i<countVertex; i++)
							{
								DrawMesh.UV[i] = MatrixTexture.MultiplyPoint3x4(Library_SpriteStudio6.Draw.BufferMesh.TableUVMapping[i]);
							}
						}
#else
						{
							Vector2 uLR = new Vector2(positionMapping.x, positionMapping.x + sizeMapping.x);
							float mappingYInverse = SizeTexture.y - positionMapping.y;
							Vector2 vUD = new Vector2(mappingYInverse, mappingYInverse - sizeMapping.y);
							uLR /= SizeTexture.x;
							vUD /= SizeTexture.y;

							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.LU].x = uLR.x;
							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.LU].y = vUD.x;

							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.RU].x = uLR.y;
							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.RU].y = vUD.x;

							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.RD].x = uLR.y;
							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.RD].y = vUD.y;

							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.LD].x = uLR.x;
							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.LD].y = vUD.y;
						}
#endif

						/* Set Parts-Color */
//						if(null != DataPartsColorOverwrite)
//						{
//							if(KindColorOperation.NON != DataColorBlendOverwrite.Operation))
//							{
//								goto Plain_PartColor_Clear;
//							}
//						}
//						else
						{
							if(true == dataAnimationParts.Plain.ColorBlend.Function.ValueGet(ref PartsColor.Value, ref PartsColor.FrameKey, dataAnimationParts.Plain.ColorBlend, ref argumentContainer))
							{
								if(Library_SpriteStudio6.KindBoundBlend.NON == PartsColor.Value.Bound)
								{
									goto Plain_PartColor_Clear;
								}
							}

							float operation = (Library_SpriteStudio6.KindBoundBlend.NON == PartsColor.Value.Bound)
												? (float)((int)Library_SpriteStudio6.KindOperationBlend.NON) + 1.01f	/* "+1.0f" for -1->0 *//* "+0.01f" for Rounding-off-Error */
												: (float)((int)PartsColor.Value.Operation) + 1.01f;	/* "+1.0f" for -1->0 *//* "+0.01f" for Rounding-off-Error */
							Color sumColor = Color.black;
							float sumPower = 0.0f;

							float[] tableAlpha = PartsColor.Value.RatePixelAlpha;
							Color[] tableColor = PartsColor.Value.VertexColor;
							for(int i=0; i<(int)Library_SpriteStudio6.KindVertex.TERMINATOR2; i++)
							{
								DrawMesh.UV2[i].x = operation;
								DrawMesh.UV2[i].y = tableAlpha[i] * RateOpacity.Value;
								sumPower += DrawMesh.UV2[i].y;

								DrawMesh.ColorOverlay[i] = tableColor[i];
								sumColor += tableColor[i];
							}
							tableAlpha = null;
							tableColor = null;

							if((int)Library_SpriteStudio6.KindVertex.TERMINATOR4 == countVertex)
							{
								sumColor *= 0.25f;
								sumPower *= 0.25f;

								DrawMesh.UV2[(int)Library_SpriteStudio6.KindVertex.C].x = operation;
								DrawMesh.UV2[(int)Library_SpriteStudio6.KindVertex.C].y = sumPower;
								DrawMesh.ColorOverlay[(int)Library_SpriteStudio6.KindVertex.C] = sumColor;
							}
						}

						goto Plain_PartColor_End;

					Plain_PartColor_Clear:;
						for(int i=0; i<countVertex; i++)
						{
							DrawMesh.UV2[i].x = (float)((int)Library_SpriteStudio6.KindOperationBlend.NON) + 1.01f;	/* "+1.0f" for -1->0 *//* "+0.01f" for Rounding-off-Error */
							DrawMesh.UV2[i].y = RateOpacity.Value;	/* Opacity */
						}
					Plain_PartColor_End:;

						/* Calculate Mesh coordinates */
						float left = (-pivotMesh.x) * RateScaleMesh.x;
						float right = (sizeMesh.x - pivotMesh.x) * RateScaleMesh.x;
						float top = -((-pivotMesh.y) * RateScaleMesh.y);	/* * -1.0f ... Y-Axis Inverse */
						float bottom = -((sizeMesh.y - pivotMesh.y) * RateScaleMesh.y);	/* * -1.0f ... Y-Axis Inverse */

						if((int)Library_SpriteStudio6.KindVertex.TERMINATOR4 == countVertex)
						{	/* 4-Triangles Mesh */
							/* Set Mapping (Center) */
							Vector2 uv2C = DrawMesh.UV[0];
							uv2C += DrawMesh.UV[1];
							uv2C += DrawMesh.UV[2];
							uv2C += DrawMesh.UV[3];
							uv2C *= 0.25f;
							DrawMesh.UV[(int)Library_SpriteStudio6.KindVertex.C] = uv2C;

							/* Set Coordinates */
							dataAnimationParts.Plain.VertexCorrection.Function.ValueGet(ref VertexCorrection.Value, ref VertexCorrection.FrameKey, dataAnimationParts.Plain.VertexCorrection, ref argumentContainer);
							int indexVertex;
							int[] tableIndex = TableIndexVertexCorrectionOrder[IndexVertexCollectionTable];
							Vector2[] tableCoordinate = VertexCorrection.Value.Coordinate;

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.LU];
							DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.LU] = new Vector3((left + tableCoordinate[indexVertex].x), (top + tableCoordinate[indexVertex].y), 0.0f);

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.RU];
							DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.RU] = new Vector3((right + tableCoordinate[indexVertex].x), (top + tableCoordinate[indexVertex].y), 0.0f);

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.RD];
							DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.RD] = new Vector3((right + tableCoordinate[indexVertex].x), (bottom + tableCoordinate[indexVertex].y), 0.0f);

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.LD];
							DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.LD] = new Vector3((left + tableCoordinate[indexVertex].x), (bottom + tableCoordinate[indexVertex].y), 0.0f);

							/* MEMO: Centering on intersection of diagonals of the 4 sides' midpoints. (not 4 vertices.) */
							Vector3 coordinateLURU = (DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.LU] + DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.RU]) * 0.5f;
							Vector3 coordinateLULD = (DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.LU] + DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.LD]) * 0.5f;
							Vector3 coordinateLDRD = (DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.LD] + DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.RD]) * 0.5f;
							Vector3 coordinateRURD = (DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.RU] + DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.RD]) * 0.5f;
							Library_SpriteStudio6.Utility.Math.CoordinateGetDiagonalIntersection(	out DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.C],
																									ref coordinateLURU,
																									ref coordinateRURD,
																									ref coordinateLULD,
																									ref coordinateLDRD
																								);
						}
						else
						{	/* 2-Triangles Mesh */
							/* Set Coordinates */
							DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.LU] = new Vector3(left, top, 0.0f);
							DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.RU] = new Vector3(right, top, 0.0f);
							DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.RD] = new Vector3(right, bottom, 0.0f);
							DrawMesh.Coordinate[(int)Library_SpriteStudio6.KindVertex.LD] = new Vector3(left, bottom, 0.0f);
						}
						/* Draw */
						Material instanceMaterialDraw = instanceRoot.MaterialGet(indexCellMap, instanceRoot.DataAnimation.TableParts[idParts].OperationBlendTarget);
						DrawMesh.Exec(	instanceTransform,
										instanceMaterialDraw,
										instanceGameObject.layer
									);
					}

					internal void Fix(	Script_SpriteStudio6_Root instanceRoot,
										int idParts,
										GameObject instanceGameObject,
										Transform instanceTransform,
										ref Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus statusControlParts,
										ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
										ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer
									)
					{
					}
					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					private readonly static Vector2 SizeTextureDefault = new Vector2(64.0f, 64.0f);

					private readonly static int[][] TableIndexVertexCorrectionOrder = new int[4][]
					{
						new int[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2]
						{	/* Normal */
							(int)Library_SpriteStudio6.KindVertex.LU,
							(int)Library_SpriteStudio6.KindVertex.RU,
							(int)Library_SpriteStudio6.KindVertex.RD,
							(int)Library_SpriteStudio6.KindVertex.LD,
						},
						new int[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2]
						{	/* Flip-X */
							(int)Library_SpriteStudio6.KindVertex.RU,
							(int)Library_SpriteStudio6.KindVertex.LU,
							(int)Library_SpriteStudio6.KindVertex.LD,
							(int)Library_SpriteStudio6.KindVertex.RD,
						},
						new int[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2]
						{	/* Flip-Y */
							(int)Library_SpriteStudio6.KindVertex.LD,
							(int)Library_SpriteStudio6.KindVertex.RD,
							(int)Library_SpriteStudio6.KindVertex.RU,
							(int)Library_SpriteStudio6.KindVertex.LU,
						},
						new int[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2]
						{	/* FlipX&Y */
							(int)Library_SpriteStudio6.KindVertex.RD,
							(int)Library_SpriteStudio6.KindVertex.LD,
							(int)Library_SpriteStudio6.KindVertex.LU,
							(int)Library_SpriteStudio6.KindVertex.RU,
						}
					};
					#endregion Enums & Constants
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
