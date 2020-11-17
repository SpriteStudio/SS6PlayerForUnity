/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
// #define BONEINDEX_CONVERT_PARTSID

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Library_SpriteStudio6
{
	public static partial class Control
	{
		public static partial class Animation
		{
			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public readonly static Vector2 SizeTextureDefault = new Vector2(64.0f, 64.0f);

			public readonly static int[][] TableIndexVertexCorrectionOrder = new int[4][]
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

#if false
			/* MEMO: Before Ver.1.0.x */
			public const int CountShiftSortKeyPriority = 16;
			public const int MaskSortKeyPriority = 0x7fff0000;	/* -16384 to 16383 */
			public const int MaskSortKeyIDParts = 0x0000ffff;	/* -32788 to 32767 */
#else
			/* MEMO: After Ver.1.1.x                                                       */
			/*       Necessary to improve Priority-Key's accuracy for supporting 2 sorts   */
			/*        of "Priority" and "Z- coordinate", so PartID-Key's range is reduced. */
			public const int CountShiftSortKeyPriority = 12;
			public const int MaskSortKeyPriority = 0x7ffff000;	/* -524288 to 524287 */
			public const int MaskSortKeyIDParts = 0x00000fff;	/* 0 to 4095 */
#endif
			#endregion Enums & Constants

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
				internal Matrix4x4 MatrixBoneWorld;

				public Object PrefabUnderControl;
				public GameObject InstanceGameObjectUnderControl;
				internal Script_SpriteStudio6_Root InstanceRootUnderControl;
				internal int IndexAnimationUnderControl;
				internal Script_SpriteStudio6_RootEffect InstanceRootEffectUnderControl;
				internal int FramePreviousUpdateUnderControl;
				internal Library_SpriteStudio6.Data.Animation.Attribute.Instance DataInstance;
//				internal Library_SpriteStudio6.Data.Animation.Attribute.Effect DataEffect;

				public Script_SpriteStudio6_Collider InstanceScriptCollider;
				internal int FramePreviousUpdateRadiusCollision;

				internal Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus StatusAnimationParts;
				private int FrameKeyStatusAnimationFrame;
				private Library_SpriteStudio6.Data.Animation.Attribute.Status StatusAnimationFrame;
				internal int IDPartsNextDraw
				{
					get
					{
						if(false == StatusAnimationFrame.IsValid)
						{
							return(-1);
						}
						return(StatusAnimationFrame.IDPartsNextDraw);
					}
				}
				internal int IDPartsNextPreDraw
				{
					get
					{
						if(false == StatusAnimationFrame.IsValid)
						{
							return(-1);
						}
						return(StatusAnimationFrame.IDPartsNextPreDraw);
					}
				}

				internal BufferTRS TRSMaster;
				internal BufferTRS TRSSlave;

				internal BufferAttribute<Vector2> ScaleLocal;
				internal BufferAttribute<float> RateOpacity;
				internal BufferAttribute<int> Priority;

				internal BufferParameterSprite ParameterSprite;
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
					MatrixBoneWorld = Matrix4x4.identity;

//					PrefabUnderControl =
//					InstanceGameObjectUnderControl =
					InstanceRootUnderControl = null;
					IndexAnimationUnderControl = -1;
					InstanceRootEffectUnderControl = null;
					FramePreviousUpdateUnderControl = -1;
					DataInstance.CleanUp();
//					DataEffect.CleanUp();

//					InstanceScriptCollider =
					FramePreviousUpdateRadiusCollision = -1;

					StatusAnimationParts = Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.CLEAR;
					FrameKeyStatusAnimationFrame = -1;
					StatusAnimationFrame.CleanUp();

					TRSMaster.CleanUp();
					TRSSlave.CleanUp();

					ScaleLocal.CleanUp();	ScaleLocal.Value = Vector2.one;
					RateOpacity.CleanUp();	RateOpacity.Value = 1.0f;
					Priority.CleanUp();	Priority.Value = 0;

					ParameterSprite.CleanUp();
				}

				internal bool BootUp(Script_SpriteStudio6_Root instanceRoot, int idParts, int countPartsSprite)
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

					/* Boot up for each part feature */
					switch(instanceRoot.DataAnimation.TableParts[idParts].Feature)
					{
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
							break;

//						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
//						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
							/* MEMO: Erase, because can not have undercontrol object. */
							PrefabUnderControl = null;
							InstanceGameObjectUnderControl = null;

							if(false == ParameterSprite.BootUp((int)Library_SpriteStudio6.KindVertex.TERMINATOR4, countPartsSprite, false))
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
							if(false == BootUpInstance(instanceRoot, idParts, false, null))
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

//						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2:
//						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
							/* MEMO: Erase, because can not have undercontrol object. */
							PrefabUnderControl = null;
							InstanceGameObjectUnderControl = null;

							if(false == ParameterSprite.BootUp((int)Library_SpriteStudio6.KindVertex.TERMINATOR4, countPartsSprite, true))
							{
								goto BootUp_ErrorEnd;
							}
							break;

						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONE:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
							break;

						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MESH:
							/* MEMO: Erase, because can not have undercontrol object. */
							PrefabUnderControl = null;
							InstanceGameObjectUnderControl = null;

							if(false == ParameterSprite.BootUpMesh(instanceRoot, idParts, false))
							{
								goto BootUp_ErrorEnd;
							}
							break;
					}

					AnimationChange();
					Status |= FlagBitStatus.VALID;
					/* MEMO: Make sure to refresh Transform at boot up */
					Status |= (	FlagBitStatus.CHANGE_TRANSFORM_POSITION
								| FlagBitStatus.CHANGE_TRANSFORM_ROTATION
								| FlagBitStatus.CHANGE_TRANSFORM_SCALING
							);

					return(true);

				BootUp_ErrorEnd:;
					CleanUp();
					Status = FlagBitStatus.CLEAR;
					return(false);
				}

				public bool BootUpInstance(Script_SpriteStudio6_Root instanceRoot, int idParts, bool flagRenewInstance, GameObject source)
				{
					bool flagRevert = false;
					if(null == source)
					{	/* Revert */
						source = (GameObject)PrefabUnderControl;
						if(null == source)
						{
							source = (GameObject)instanceRoot.DataAnimation.TableParts[idParts].PrefabUnderControl;
						}
						if(null == source)
						{	/* Error */
							return(false);
						}

						flagRevert = true;
					}
					if(null != source)
					{
						/* Create UnderControl-Instance */
						InstanceGameObjectUnderControl = Library_SpriteStudio6.Utility.Asset.PrefabInstantiate(	source,
																												InstanceGameObjectUnderControl,
																												InstanceGameObject,
																												flagRenewInstance
																											);
						if(null != InstanceGameObjectUnderControl)
						{
							InstanceRootUnderControl = InstanceGameObjectUnderControl.GetComponent<Script_SpriteStudio6_Root>();
							InstanceRootUnderControl.InstanceRootParent = instanceRoot;

							int indexAnimation = 0;
							if(true == flagRevert)
							{
								indexAnimation = (true == string.IsNullOrEmpty(instanceRoot.DataAnimation.TableParts[idParts].NameAnimationUnderControl))
													? 0
													: InstanceRootUnderControl.IndexGetAnimation(instanceRoot.DataAnimation.TableParts[idParts].NameAnimationUnderControl);
							}
							IndexAnimationUnderControl = (0 > indexAnimation) ? 0 : indexAnimation;
//							InstanceRootUnderControl.AnimationPlay(-1, IndexAnimationUnderControl);
//							InstanceRootUnderControl.AnimationStop();
						}

						FramePreviousUpdateUnderControl = -1;
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

				internal void Update(	Script_SpriteStudio6_Root instanceRoot,
										int idParts,
										bool flagHideDefault,
										ref Matrix4x4 matrixCorrection,
										int indexTrackRoot
									)
				{
					/* MEMO: Since "Track-Transition" and frame-jump at stop must be processed, should not return only by judging playing. */

					int indexTrack = IndexControlTrack;
					if(0 > indexTrack)
					{	/* Disconnect */
						return;
					}

					int indexAnimation = instanceRoot.TableControlTrack[indexTrack].ArgumentContainer.IndexAnimation;
					if(0 > indexAnimation)
					{	/* Not Playing */
						return;
					}

					/* Update Transform (GameObject) & Spatus */
					/* MEMO: For reseting at animation switching, "UpdateGameObject" is always called. */
					UpdateGameObject(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);

					/* Check Unused part */
					if(0 != (StatusAnimationParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED))
					{
						return;
					}

					/* Update UserData */
					/* MEMO: "UserData" is not decoded when not playing animation. */
					/* MEMO: "UserData" is not decoded in "Instance" animation. */
					if(true == instanceRoot.TableControlTrack[indexTrack].StatusIsPlaying)
					{
						if((null == instanceRoot.InstanceRootParent) && (0 == (StatusAnimationParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_USERDATA)))
						{
							UpdateUserData(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
						}
					}

					/* Update for each parts' feature */
					switch(instanceRoot.DataAnimation.TableParts[idParts].Feature)
					{
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
							break;
//						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
//						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
							UpdateNormal(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
							UpdateSetPartsDraw(instanceRoot, idParts, flagHideDefault, false, false, indexTrackRoot);
							break;
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
							/* Update Instance */
							/* MEMO: No processing */
//							UpdateInstance(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
							UpdateSetPartsDraw(instanceRoot, idParts, flagHideDefault, true, false, indexTrackRoot);
							break;
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
							/* Update Effect */
							/* MEMO: No processing */
//							UpdateEffect(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
							UpdateSetPartsDraw(instanceRoot, idParts, flagHideDefault, true, false, indexTrackRoot);
							break;

//						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2:
//						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
							UpdateNormal(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
							UpdateSetPartsDraw(instanceRoot, idParts, flagHideDefault, false, true, indexTrackRoot);
							break;

						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
							break;
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONE:
							/* MEMO: Since bones are referenced many times, cache in advance. */
							MatrixBoneWorld = matrixCorrection * InstanceTransform.localToWorldMatrix;
							break;
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
							break;

						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MESH:
							UpdateMesh(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
							UpdateSetPartsDraw(instanceRoot, idParts, flagHideDefault, false, false, indexTrackRoot);
							break;
					}

					/* Update Collider */
					/* MEMO: Ignore except "SQUARE" and "CIRCLE".(Not Supported) */
					switch(instanceRoot.DataAnimation.TableParts[idParts].ShapeCollision)
					{
						case Data.Parts.Animation.KindCollision.NON:
							break;

						case Data.Parts.Animation.KindCollision.SQUARE:
							UpdateColliderRectangle(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
							break;

						case Data.Parts.Animation.KindCollision.AABB:
							break;

						case Data.Parts.Animation.KindCollision.CIRCLE:
							UpdateColliderRadius(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
							break;

						case Data.Parts.Animation.KindCollision.CIRCLE_SCALEMINIMUM:
							break;

						case Data.Parts.Animation.KindCollision.CIRCLE_SCALEMAXIMUM:
							break;
					}
				}
				private void UpdateGameObject(	Script_SpriteStudio6_Root instanceRoot,
												int idParts,
												ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
												ref Library_SpriteStudio6.Control.Animation.Track controlTrack
											)
				{
					controlTrack.ArgumentContainer.IDParts = idParts;

					Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus statusParts = dataAnimationParts.StatusParts;
					StatusAnimationParts = statusParts;	/* cache for other Update/Draw-Functions */

					/* MEMO: StatusParts's NOT_USED should not be judged here, because will fail Refresh at animation-start. */
//					if(0 != (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED))
//					{
//						return;
//					}

					/* Check Transition & Cache Transition-Parameters */
					int indexTrackSlave = controlTrack.IndexTrackSlave;
					int indexAnimationSlave;
					Library_SpriteStudio6.Data.Animation dataAnimationSlave;
					float rateTransition;
					float rateTransitionInverse;
					Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus statusPartsSlave;
					if(0 > indexTrackSlave)
					{	/* No Slave (Not Transition) */
						indexAnimationSlave = -1;
						dataAnimationSlave = null;
						statusPartsSlave = (	Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION
												| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION
												| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING
										);

						rateTransition = 0.0f;
						rateTransitionInverse = 1.0f;
					}
					else
					{	/* Has Slave (Transition) */
						instanceRoot.TableControlTrack[indexTrackSlave].ArgumentContainer.IDParts = idParts;
						indexAnimationSlave = instanceRoot.TableControlTrack[indexTrackSlave].ArgumentContainer.IndexAnimation;
						if(0 > indexAnimationSlave)
						{	/* Invalid */
							indexAnimationSlave = -1;
							dataAnimationSlave = null;
							statusPartsSlave = (	Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION
													| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION
													| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING
											);

							rateTransition = 0.0f;
							rateTransitionInverse = 1.0f;
						}
						else
						{	/* Valid */
							dataAnimationSlave = instanceRoot.DataAnimation.TableAnimation[indexAnimationSlave];
							statusPartsSlave = dataAnimationSlave.TableParts[idParts].StatusParts;

							rateTransition = controlTrack.RateTransition;
							rateTransitionInverse = 1.0f - rateTransition;
						}
					}

					Transform transform = InstanceGameObject.transform;
					bool flagUpdate;
					Vector3 valueTRSMaster;
					Vector3 valueTRSSlave;

					if(true == controlTrack.StatusIsTransitionStart)
					{
						TRSSlave.CleanUp();
					}

					/* Update Position */
					flagUpdate = false;
					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION))
					{
#if UNITY_EDITOR
						if(	(null != dataAnimationParts.Position.Function)
							&& (true == dataAnimationParts.Position.Function.ValueGet(	ref TRSMaster.Position.Value,
																						ref TRSMaster.Position.FrameKey,
																						dataAnimationParts.Position,
																						ref controlTrack.ArgumentContainer
																					)
								)
							)
#else
						if(true == dataAnimationParts.Position.Function.ValueGet(	ref TRSMaster.Position.Value,
																					ref TRSMaster.Position.FrameKey,
																					dataAnimationParts.Position,
																					ref controlTrack.ArgumentContainer
																				)
							)
#endif
						{
							flagUpdate = true;
						}
						valueTRSMaster = TRSMaster.Position.Value;
					}
					else
					{
						if((FlagBitStatus.CHANGE_TRANSFORM_POSITION | FlagBitStatus.REFRESH_TRANSFORM_POSITION) == (Status & (FlagBitStatus.CHANGE_TRANSFORM_POSITION | FlagBitStatus.REFRESH_TRANSFORM_POSITION)))
						{	/* Refresh */
							flagUpdate = true;
						}
						valueTRSMaster = Vector3.zero;
					}

					if(0 > indexTrackSlave)
					{	/* No Slave (Not Transition) */
						/* Set Transform-Position */
						if(true == flagUpdate)
						{
							transform.localPosition = valueTRSMaster;
							Status |= FlagBitStatus.CHANGE_TRANSFORM_POSITION;
						}
					}
					else
					{	/* Has Slave (Transition) */
						/* Get Slave Position */
						if(0 == (statusPartsSlave & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION))
						{
#if UNITY_EDITOR
							if(	(null !=dataAnimationSlave.TableParts[idParts].Position.Function)
								&& (true == dataAnimationSlave.TableParts[idParts].Position.Function.ValueGet(	ref TRSSlave.Position.Value,
																												ref TRSSlave.Position.FrameKey,
																												dataAnimationSlave.TableParts[idParts].Position,
																												ref instanceRoot.TableControlTrack[indexTrackSlave].ArgumentContainer
																											)
									)
								)
#else
							if(true == dataAnimationSlave.TableParts[idParts].Position.Function.ValueGet(	ref TRSSlave.Position.Value,
																											ref TRSSlave.Position.FrameKey,
																											dataAnimationSlave.TableParts[idParts].Position,
																											ref instanceRoot.TableControlTrack[indexTrackSlave].ArgumentContainer
																										)
								)
#endif
							{
								flagUpdate |= true;
							}
							valueTRSSlave = TRSSlave.Position.Value;
						}
						else
						{
							valueTRSSlave = Vector3.zero;
						}

						/* Set Transform-Position */
						/* MEMO: As blending rate always changes, not check attribute updates. */
						transform.localPosition = (valueTRSMaster * rateTransitionInverse) + (valueTRSSlave * rateTransition);
						Status |= FlagBitStatus.CHANGE_TRANSFORM_POSITION;
					}

					/* Update Rotation */
					flagUpdate = false;
					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION))
					{
#if UNITY_EDITOR
						if(	(null != dataAnimationParts.Rotation.Function)
							&& (true == dataAnimationParts.Rotation.Function.ValueGet(	ref TRSMaster.Rotation.Value,
																						ref TRSMaster.Rotation.FrameKey,
																						dataAnimationParts.Rotation,
																						ref controlTrack.ArgumentContainer
																					)
								)
							)
#else
						if(true == dataAnimationParts.Rotation.Function.ValueGet(	ref TRSMaster.Rotation.Value,
																					ref TRSMaster.Rotation.FrameKey,
																					dataAnimationParts.Rotation,
																					ref controlTrack.ArgumentContainer
																				)
							)
#endif
						{
							flagUpdate = true;
						}
						valueTRSMaster = TRSMaster.Rotation.Value;
					}
					else
					{
						if((FlagBitStatus.CHANGE_TRANSFORM_ROTATION | FlagBitStatus.REFRESH_TRANSFORM_ROTATION) == (Status & (FlagBitStatus.CHANGE_TRANSFORM_ROTATION | FlagBitStatus.REFRESH_TRANSFORM_ROTATION)))
						{	/* Refresh */
							flagUpdate = true;
						}
						valueTRSMaster = Vector3.zero;
					}

					if(0 > indexTrackSlave)
					{	/* No Slave (Not Transition) */
						/* Set Transform-Rotation */
						if(true == flagUpdate)
						{
							/* MEMO: "SpriteStudio6" and "Unity" have different rotation order.  */
							Quaternion localQuaternion;
							Library_SpriteStudio6.Utility.Math.QuaternionGetEulerAngels(out localQuaternion, ref valueTRSMaster);
							transform.localRotation = localQuaternion;

							Status |= FlagBitStatus.CHANGE_TRANSFORM_ROTATION;
						}
					}
					else
					{	/* Has Slave (Transition) */
						/* Get Slave Rotation */
						if(0 == (statusPartsSlave & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION))
						{
#if UNITY_EDITOR
							if(	(null != dataAnimationSlave.TableParts[idParts].Rotation.Function)
								&& (true == dataAnimationSlave.TableParts[idParts].Rotation.Function.ValueGet(	ref TRSSlave.Rotation.Value,
																												ref TRSSlave.Rotation.FrameKey,
																												dataAnimationSlave.TableParts[idParts].Rotation,
																												ref instanceRoot.TableControlTrack[indexTrackSlave].ArgumentContainer
																											)
									)
								)
#else
							if(true == dataAnimationSlave.TableParts[idParts].Rotation.Function.ValueGet(	ref TRSSlave.Rotation.Value,
																											ref TRSSlave.Rotation.FrameKey,
																											dataAnimationSlave.TableParts[idParts].Rotation,
																											ref instanceRoot.TableControlTrack[indexTrackSlave].ArgumentContainer
																										)
								)
#endif
							{
								flagUpdate |= true;
							}
							valueTRSSlave = TRSSlave.Rotation.Value;
						}
						else
						{
							valueTRSSlave = Vector3.zero;
						}

						/* Set Transform-Rotation */
						/* MEMO: As blending rate always changes, not check attribute updates. */
						/* MEMO: "SpriteStudio6" and "Unity" have different rotation order.  */
						Vector3 localEulerAngles = (valueTRSMaster * rateTransitionInverse) + (valueTRSSlave * rateTransition);
						Quaternion localQuaternion;
						Library_SpriteStudio6.Utility.Math.QuaternionGetEulerAngels(out localQuaternion, ref localEulerAngles);
						transform.localRotation = localQuaternion;

						Status |= FlagBitStatus.CHANGE_TRANSFORM_ROTATION;
					}

					/* Update Scaling */
					flagUpdate = false;
					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING))
					{
#if UNITY_EDITOR
						if(	(null != dataAnimationParts.Scaling.Function)
							&& (true == dataAnimationParts.Scaling.Function.ValueGet(	ref TRSMaster.Scaling.Value,
																						ref TRSMaster.Scaling.FrameKey,
																						dataAnimationParts.Scaling,
																						ref controlTrack.ArgumentContainer
																					)
								)
							)
#else
						if(true == dataAnimationParts.Scaling.Function.ValueGet(	ref TRSMaster.Scaling.Value,
																					ref TRSMaster.Scaling.FrameKey,
																					dataAnimationParts.Scaling,
																					ref controlTrack.ArgumentContainer
																				)
							)
#endif
						{
							flagUpdate = true;
						}
						valueTRSMaster = TRSMaster.Scaling.Value;
					}
					else
					{
						if((FlagBitStatus.CHANGE_TRANSFORM_SCALING | FlagBitStatus.REFRESH_TRANSFORM_SCALING) == (Status & (FlagBitStatus.CHANGE_TRANSFORM_SCALING | FlagBitStatus.REFRESH_TRANSFORM_SCALING)))
						{	/* Refresh */
							flagUpdate = true;
						}
						valueTRSMaster = Vector3.one;
					}

					if(0 > indexTrackSlave)
					{	/* No Slave (Not Transition) */
						/* Set Transform-Scaling */
						if(true == flagUpdate)
						{
							valueTRSMaster.z = 1.0f;
							transform.localScale = valueTRSMaster;
							Status |= FlagBitStatus.CHANGE_TRANSFORM_SCALING;
						}
					}
					else
					{	/* Has Slave (Transition) */
						/* Get Slave Scaling */
						if(0 == (statusPartsSlave & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING))
						{
#if UNITY_EDITOR
							if(	(null !=  dataAnimationSlave.TableParts[idParts].Scaling.Function)
								&& (true == dataAnimationSlave.TableParts[idParts].Scaling.Function.ValueGet(	ref TRSSlave.Scaling.Value,
																												ref TRSSlave.Scaling.FrameKey,
																												dataAnimationSlave.TableParts[idParts].Scaling,
																												ref instanceRoot.TableControlTrack[indexTrackSlave].ArgumentContainer
																											)
									)
								)
#else
							if(true == dataAnimationSlave.TableParts[idParts].Scaling.Function.ValueGet(	ref TRSSlave.Scaling.Value,
																											ref TRSSlave.Scaling.FrameKey,
																											dataAnimationSlave.TableParts[idParts].Scaling,
																											ref instanceRoot.TableControlTrack[indexTrackSlave].ArgumentContainer
																										)
								)
#endif
							{
								flagUpdate |= true;
							}
							valueTRSSlave = TRSSlave.Scaling.Value;
						}
						else
						{
							valueTRSSlave = Vector3.one;
						}

						/* Set Transform-Scaling */
						/* MEMO: As blending rate always changes, not check attribute updates. */
						valueTRSMaster = (valueTRSMaster * rateTransitionInverse) + (valueTRSSlave * rateTransition);
						valueTRSMaster.z = 1.0f;
						transform.localScale = valueTRSMaster;
						Status |= FlagBitStatus.CHANGE_TRANSFORM_SCALING;
					}

					/* Clear "Refresh Transform" flags */
					Status &= ~(FlagBitStatus.REFRESH_TRANSFORM_POSITION | FlagBitStatus.REFRESH_TRANSFORM_ROTATION | FlagBitStatus.REFRESH_TRANSFORM_SCALING);

					/* Get Status & Hide */
#if UNITY_EDITOR
					if(null != dataAnimationParts.Status.Function)
					{
						dataAnimationParts.Status.Function.ValueGet(ref StatusAnimationFrame, ref FrameKeyStatusAnimationFrame, dataAnimationParts.Status, ref controlTrack.ArgumentContainer);
					}
#else
					dataAnimationParts.Status.Function.ValueGet(ref StatusAnimationFrame, ref FrameKeyStatusAnimationFrame, dataAnimationParts.Status, ref controlTrack.ArgumentContainer);
#endif
					if(true == StatusAnimationFrame.IsHide)
					{
						Status |= FlagBitStatus.HIDE;
					}
					else
					{
						Status &= ~FlagBitStatus.HIDE;
					}

					/* Check Not Used */
					if(0 != (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED))
					{
						return;
					}

					/* Get Local-Scale */
					/* MEMO: "ScaleLocal" are data that must be constantly updated in most parts, so decode here. */
#if UNITY_EDITOR
					if(	(null != dataAnimationParts.ScalingLocal.Function)
						&& (true == dataAnimationParts.ScalingLocal.Function.ValueGet(ref ScaleLocal.Value, ref ScaleLocal.FrameKey, dataAnimationParts.ScalingLocal, ref controlTrack.ArgumentContainer))
						)
#else
					if(true == dataAnimationParts.ScalingLocal.Function.ValueGet(ref ScaleLocal.Value, ref ScaleLocal.FrameKey, dataAnimationParts.ScalingLocal, ref controlTrack.ArgumentContainer))
#endif
					{
						Status |= FlagBitStatus.UPDATE_SCALELOCAL;
					}

					/* Get Rate-Opacity */
					/* MEMO: "RateOpacity" are data that must be constantly updated in most parts, so decode here. */
#if UNITY_EDITOR
					if(	(null != dataAnimationParts.RateOpacity.Function)
						&& (true == dataAnimationParts.RateOpacity.Function.ValueGet(ref RateOpacity.Value, ref RateOpacity.FrameKey, dataAnimationParts.RateOpacity, ref controlTrack.ArgumentContainer))
						)
#else
					if(true == dataAnimationParts.RateOpacity.Function.ValueGet(ref RateOpacity.Value, ref RateOpacity.FrameKey, dataAnimationParts.RateOpacity, ref controlTrack.ArgumentContainer))
#endif
					{
						Status |= FlagBitStatus.UPDATE_RATEOPACITY;
					}

					/* Get Priority */
					/* MEMO: "RateOpacity" are data that must be constantly updated in most parts, so decode here. */
#if UNITY_EDITOR
					if(null != dataAnimationParts.Priority.Function)
					{
						dataAnimationParts.Priority.Function.ValueGet(ref Priority.Value, ref Priority.FrameKey, dataAnimationParts.Priority, ref controlTrack.ArgumentContainer);
					}
#else
					dataAnimationParts.Priority.Function.ValueGet(ref Priority.Value, ref Priority.FrameKey, dataAnimationParts.Priority, ref controlTrack.ArgumentContainer);
#endif
				}

				private void UpdateUserData(	Script_SpriteStudio6_Root instanceRoot,
												int idParts,
												ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
												ref Library_SpriteStudio6.Control.Animation.Track controlTrack
											)
				{
					if(	(true == controlTrack.StatusIsIgnoreUserData)
						|| (true == controlTrack.StatusIsIgnoreNextUpdateUserData)
						|| (false == controlTrack.StatusIsDecodeAttribute)
						|| (null == instanceRoot.FunctionUserData))
					{	/* No Need to decode UserData-s */
						return;
					}

					int countLoop = controlTrack.CountLoopNow;
					if(true == controlTrack.StatusIsIgnoreSkipLoop)
					{
						countLoop = 0;
					}
					bool flagLoop = (0 < countLoop);
					bool flagFirst = controlTrack.StatusIsPlayingStart;
					bool flagReverse = controlTrack.StatusIsPlayingReverse;
					bool flagReversePrevious = controlTrack.StatusIsPlayingReversePrevious;
					bool flagTurn = controlTrack.StatusIsPlayingTurn;
					bool flagStylePingPong = controlTrack.StatusIsPlayStylePingpong;
					int frame = controlTrack.ArgumentContainer.Frame;
					int frameStart = controlTrack.FrameStart;
					int frameEnd = controlTrack.FrameEnd;
					int framePrevious = -1;

					/* Get decoding top frame */
					if(true == flagFirst)
					{
						framePrevious = frame;
					}
					else
					{
						framePrevious = controlTrack.ArgumentContainer.FramePrevious;
						if(true== flagReversePrevious)
						{
							framePrevious--;
							if((false == flagTurn) && (framePrevious < frame))
							{
								return;
							}
						}
						else
						{
							framePrevious++;
							if((false == flagTurn) && (framePrevious > frame))
							{
								return;
							}
						}
					}

					/* Decoding UserData-s */
					if(true == flagStylePingPong)
					{	/* Play-Style: PingPong */
						bool FlagStyleReverse = controlTrack.StatusIsPlayStyleReverse;

						/* Decoding skipped frame */
						if(true == FlagStyleReverse)
						{	/* Reverse */
							if(true == flagLoop)
							{
								/* Part-Head */
								if(true == flagReversePrevious)
								{
									framePrevious = controlTrack.ArgumentContainer.FramePrevious - 1;	/* Force */
									UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frameStart, frame, false);
									UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameStart, frameEnd, frame, true);
								}
								else
								{
									framePrevious = controlTrack.ArgumentContainer.FramePrevious + 1;	/* Force */
									UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frameEnd, frame, true);
								}

								/* Part-Loop */
								for(int i=1; i<countLoop; i++)
								{
									UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameEnd, frameStart, frame, false);
									UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameStart, frameEnd, frame, true);
								}

								/* Part-Tail & Just-Now */
								if(true == flagReverse)
								{	/* Now-Reverse */
									UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameEnd, frame, frame, false);
								}
								else
								{	/* Now-Foward */
									UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameEnd, frameStart, frame, false);
									UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameStart, frame, frame, true);
								}
							}
							else
							{	/* Normal */
								if(true == flagTurn)
								{	/* Turn-Back */
									/* MEMO: No-Loop & Turn-Back ... Always "Reverse to Foward" */
									framePrevious = controlTrack.ArgumentContainer.FramePrevious - 1;	/* Force */
									UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frameStart, frame, false);
									UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameStart, frame, frame, true);
								}
								else
								{	/* Normal */
									if(true == flagReverse)
									{	/* Reverse */
										UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frame, frame, false);
									}
									else
									{	/* Foward */
										UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frame, frame, true);
									}
								}
							}
						}
						else
						{	/* Normal */
							if(true == flagLoop)
							{
								/* Part-Head */
								if(true == flagReversePrevious)
								{
									framePrevious = controlTrack.ArgumentContainer.FramePrevious - 1;	/* Force */
									UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frameStart, frame, true);
								}
								else
								{
									framePrevious = controlTrack.ArgumentContainer.FramePrevious + 1;	/* Force */
									UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frameEnd, frame, false);
									UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameEnd, frameStart, frame, true);
								}

								/* Part-Loop */
								for(int i=1; i<countLoop; i++)
								{
									UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameStart, frameEnd, frame, false);
									UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameEnd, frameStart, frame, true);
								}

								/* Part-Tail & Just-Now */
								if(true == flagReverse)
								{	/* Now-Reverse */
									UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameStart, frameEnd, frame, false);
									UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameEnd, frame, frame, true);
								}
								else
								{	/* Now-Foward */
									UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameStart, frame, frame, false);
								}
							}
							else
							{	/* Normal */
								if(true == flagTurn)
								{	/* Turn-Back */
									/* MEMO: No-Loop & Turn-Back ... Always "Foward to Revese" */
									framePrevious = controlTrack.ArgumentContainer.FramePrevious + 1;	/* Force */
									UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frameEnd, frame, false);
									UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameEnd, frame, frame, true);
								}
								else
								{	/* Normal */
									if(true == flagReverse)
									{	/* Reverse */
										UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frame, frame, true);
									}
									else
									{	/* Foward */
										UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frame, frame, false);
									}
								}
							}
						}
					}
					else
					{	/* Play-Style: OneWay */
						/* Decoding skipped frame */
						if(true == flagReverse)
						{	/* Backwards */
							if(true == flagTurn)
							{	/* Wrap-Around */
								/* Part-Head */
								UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frameStart, frame, false);

								/* Part-Loop */
								for(int j=1; j<countLoop ; j++)
								{
									UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameEnd, frameStart, frame, false);
								}

								/* Part-Tail & Just-Now */
								UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameEnd, frame, frame, false);
							}
							else
							{	/* Normal */
								UpdateUserDataReverse(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frame, frame, false);
							}
						}
						else
						{	/* Foward */
							if(true == flagTurn)
							{	/* Wrap-Around */
								/* Part-Head */
								UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frameEnd, frame, false);

								/* Part-Loop */
								for(int j=1; j<countLoop; j++)
								{
									UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameStart, frameEnd, frame, false);
								}

								/* Part-Tail & Just-Now */
								UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, frameStart, frame, frame, false);
							}
							else
							{	/* Normal */
								UpdateUserDataFoward(instanceRoot, idParts, ref dataAnimationParts, ref controlTrack, framePrevious, frame, frame, false);
							}
						}
					}
				}
				private void UpdateUserDataFoward(	Script_SpriteStudio6_Root instanceRoot,
													int idParts,
													ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
													ref Library_SpriteStudio6.Control.Animation.Track controlTrack,
													int frameRangeStart,
													int frameRangeEnd,
													int frameDecode,
													bool flagTurnBack
												)
				{
					int countData = dataAnimationParts.UserData.Function.CountGetValue(dataAnimationParts.UserData);
					string nameParts = instanceRoot.DataAnimation.TableParts[idParts].Name;
					int frameKey = -1;
					int indexAnimation = controlTrack.ArgumentContainer.IndexAnimation;
					Library_SpriteStudio6.Data.Animation.Attribute.UserData userData = new Library_SpriteStudio6.Data.Animation.Attribute.UserData();
					for(int i=0; i<countData; i++)
					{
#if UNITY_EDITOR
						if(null != dataAnimationParts.UserData.Function)
						{
							dataAnimationParts.UserData.Function.ValueGetIndex(ref userData, ref frameKey, i, dataAnimationParts.UserData, ref controlTrack.ArgumentContainer);
						}
#else
						dataAnimationParts.UserData.Function.ValueGetIndex(ref userData, ref frameKey, i, dataAnimationParts.UserData, ref controlTrack.ArgumentContainer);
#endif
						if((frameRangeStart <= frameKey) && (frameRangeEnd >= frameKey))
						{	/* In range */
							instanceRoot.FunctionUserData(	instanceRoot,
															nameParts,
															idParts,
															indexAnimation,
															frameDecode,
															frameKey,
															ref userData,
															flagTurnBack
														);
						}
					}
				}
				private void UpdateUserDataReverse(	Script_SpriteStudio6_Root instanceRoot,
													int idParts,
													ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
													ref Library_SpriteStudio6.Control.Animation.Track controlTrack,
													int frameRangeEnd,
													int frameRangeStart,
													int frameDecode,
													bool flagTurnBack
												)
				{
					int countData = dataAnimationParts.UserData.Function.CountGetValue(dataAnimationParts.UserData);
					string nameParts = instanceRoot.DataAnimation.TableParts[idParts].Name;
					int frameKey = -1;
					int indexAnimation = controlTrack.ArgumentContainer.IndexAnimation;
					Library_SpriteStudio6.Data.Animation.Attribute.UserData userData = new Library_SpriteStudio6.Data.Animation.Attribute.UserData();
					for(int i=(countData-1); i>=0; i--)
					{
#if UNITY_EDITOR
						if(null != dataAnimationParts.UserData.Function)
						{
							dataAnimationParts.UserData.Function.ValueGetIndex(ref userData, ref frameKey, i, dataAnimationParts.UserData, ref controlTrack.ArgumentContainer);
						}
#else
						dataAnimationParts.UserData.Function.ValueGetIndex(ref userData, ref frameKey, i, dataAnimationParts.UserData, ref controlTrack.ArgumentContainer);
#endif
						if((frameRangeStart <= frameKey) && (frameRangeEnd >= frameKey))
						{	/* In range */
							instanceRoot.FunctionUserData(	instanceRoot,
															nameParts,
															idParts,
															indexAnimation,
															frameDecode,
															frameKey,
															ref userData,
															flagTurnBack
														);
						}
					}
				}

				private void UpdateNormal(	Script_SpriteStudio6_Root instanceRoot,
											int idParts,
											ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
											ref Library_SpriteStudio6.Control.Animation.Track controlTrack
										)
				{
					/* MEMO: Calcurate sprite size from attributes to affect sprite size except VertexCorrection. */
					/*       (Calcurate before draw, since sprite size is the shape of rectangle collider)        */

					/* Update Sprite */
					ParameterSprite.StatusSetFlip(ref StatusAnimationFrame);
					ParameterSprite.UpdatePlain(instanceRoot, idParts, InstanceGameObject, InstanceTransform, ref Status, ref dataAnimationParts, ref controlTrack.ArgumentContainer);
				}

				private void UpdateInstance(	Script_SpriteStudio6_Root instanceRoot,
												int idParts,
												ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
												ref Library_SpriteStudio6.Control.Animation.Track controlTrack
											)
				{
					/* MEMO: This function is not called. (dummy for explicitness)                               */
					/*       Originally, the processing to be written in this function is  in "DrawInstance".    */
					/*                                                                                           */
					/*       "Instance"-parts are updated and rendered in same function "DrawInstance".          */
					/*       Because "DrawInstance" is always called. (Mainly due to optimization)               */
					/*       "Instance"-parts is included in draw-order-list regardless hide status.             */
				}

				private void UpdateEffect(	Script_SpriteStudio6_Root instanceRoot,
											int idParts,
											ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
											ref Library_SpriteStudio6.Control.Animation.Track controlTrack
										)
				{
					/* MEMO: This function is not called. (dummy for explicitness)                               */
					/*       Originally, the processing to be written in this function is  in "DrawEffect".      */
					/*                                                                                           */
					/*       "Effect"-parts are updated and rendered in same function "DrawEffect".              */
					/*       Because "DrawEffect" is always called. (Mainly due to optimization)                 */
					/*       "Effect"-parts is included in draw-order-list regardless hide status.               */
				}

				private void UpdateMesh(	Script_SpriteStudio6_Root instanceRoot,
											int idParts,
											ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
											ref Library_SpriteStudio6.Control.Animation.Track controlTrack
										)
				{
					/* Update Mesh */
					/* MEMO: "Update" processing is same as "Normal". */
					ParameterSprite.StatusSetFlip(ref StatusAnimationFrame);
					ParameterSprite.UpdatePlain(instanceRoot, idParts, InstanceGameObject, InstanceTransform, ref Status, ref dataAnimationParts, ref controlTrack.ArgumentContainer);
				}

				private void UpdateSetPartsDraw(	Script_SpriteStudio6_Root instanceRoot,
													int idParts,
													bool flagHideDefault,
													bool flagSetForce,
													bool flagMask,
													int indexTrackRoot
												)
				{
					if(	(true == flagSetForce)
						|| ((false == flagHideDefault) && (0 == (Status & (FlagBitStatus.HIDE_FORCE | FlagBitStatus.HIDE))))
					)
					{
						/* Set Draw-Chain */
						int keySort;
						if(true == flagMask)
						{
							/* MEMO: At "PreDraw", "Mask"'s drawing order is reverse. */
							keySort = (-Priority.Value << CountShiftSortKeyPriority) | idParts;
							instanceRoot.ListPartsDraw.Add(keySort);
						}
						keySort = (Priority.Value << CountShiftSortKeyPriority) | idParts;
						instanceRoot.ListPartsDraw.Add(keySort);

						/* Set "Animation Synthesize" */
						if(indexTrackRoot != IndexControlTrack)
						{
							instanceRoot.StatusIsAnimationSynthesize = true;
						}
					}
				}

				private void UpdateColliderRectangle(	Script_SpriteStudio6_Root instanceRoot,
														int idParts,
														ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
														ref Library_SpriteStudio6.Control.Animation.Track controlTrack
													)
				{
					/* MEMO: Possible to reach here only when parts are "NORMAL2" and "NORMAL4" */
					if(null == InstanceScriptCollider)
					{
						return;
					}

//					if(0 != (ParameterSprite.Status & BufferParameterSprite.FlagBitStatus.UPDATE_COORDINATE))
					if(0 != (ParameterSprite.Status & BufferParameterSprite.FlagBitStatus.UPDATE_COLLIDERRECTANGLE_NOWFRAME))
					{
						/* MEMO: "Local-Scale" does not affect "Circle Collision". */
						Vector3 sizeSprite = ParameterSprite.SizeSprite;
						Vector3 pivotSprite = ParameterSprite.PivotSprite;
						pivotSprite -= sizeSprite * 0.5f;
						pivotSprite.x *= -1.0f;
						sizeSprite.z = instanceRoot.DataAnimation.TableParts[idParts].SizeCollisionZ;

						InstanceScriptCollider.ColliderSetRectangle(ref sizeSprite, ref pivotSprite);

						ParameterSprite.Status &= ~BufferParameterSprite.FlagBitStatus.UPDATE_COLLIDERRECTANGLE_NOWFRAME;
					}
				}

				private void UpdateColliderRadius(	Script_SpriteStudio6_Root instanceRoot,
													int idParts,
													ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
													ref Library_SpriteStudio6.Control.Animation.Track controlTrack
												)
				{
					if(null == InstanceScriptCollider)
					{
						return;
					}

					float radius = 0.0f;
					int frameKey = FramePreviousUpdateRadiusCollision;
#if UNITY_EDITOR
					if(	(null != dataAnimationParts.RadiusCollision.Function)
						&& (true == dataAnimationParts.RadiusCollision.Function.ValueGet(ref radius, ref frameKey, dataAnimationParts.RadiusCollision, ref controlTrack.ArgumentContainer))
						)
#else
					if(true == dataAnimationParts.RadiusCollision.Function.ValueGet(ref radius, ref frameKey, dataAnimationParts.RadiusCollision, ref controlTrack.ArgumentContainer))
#endif
					{   /* New Valid Data */
						/* MEMO: "Pivot Offset" and "Local-Scale" do not affect "Circle Collision". */
						FramePreviousUpdateRadiusCollision = frameKey;
						InstanceScriptCollider.ColliderSetRadius(radius);
					}
				}

				internal void PreDraw(	Script_SpriteStudio6_Root instanceRoot,
										int idParts,
										bool flagHideDefault,
										Library_SpriteStudio6.KindMasking masking,
										ref Matrix4x4 matrixCorrection,
										bool flagPlanarization
									)
				{
//					if(null != instanceRoot.InstanceRootParent)
//					{
//						return;
//					}

					/* Check Unused part */
					if(0 != (StatusAnimationParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED))
					{
						return;
					}

					int indexTrack = IndexControlTrack;
					int indexAnimation = instanceRoot.TableControlTrack[indexTrack].ArgumentContainer.IndexAnimation;
					if(0 <= indexAnimation)
					{
						switch(instanceRoot.DataAnimation.TableParts[idParts].Feature)
						{
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
//								break;

//							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
//							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
								break;

//							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2:
//							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
								/* MEMO: "Mask"s are rendered at "PreDraw", and at "Draw", changing shaders and render same. */
								DrawNormal(	instanceRoot,
											idParts,
											ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts],
											ref instanceRoot.TableControlParts[idParts],
											ref instanceRoot.TableControlTrack[indexTrack],
											flagHideDefault,
											masking,
											true,
											ref matrixCorrection,
											flagPlanarization
										);
								break;

							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONE:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
								break;

							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MESH:
								break;
						}
					}
				}

				internal void Draw(	Script_SpriteStudio6_Root instanceRoot,
									int idParts,
									bool flagHideDefault,
									Library_SpriteStudio6.KindMasking masking,
									ref Matrix4x4 matrixCorrection,
									bool flagPlanarization
								)
				{
					/* Check Unused part */
					if(0 != (StatusAnimationParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED))
					{
						return;
					}

					int indexTrack = IndexControlTrack;
					int indexAnimation = instanceRoot.TableControlTrack[indexTrack].ArgumentContainer.IndexAnimation;
					if(0 <= indexAnimation)
					{
						/* MEMO: Case "Normal" and "Effect", reach here only when part is not hidden.       */
						/*       (Only "Instance"-parts are registered in Draw-Order-Chain, even if hidden) */
						switch(instanceRoot.DataAnimation.TableParts[idParts].Feature)
						{
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
								break;

//							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
//							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
								DrawNormal(	instanceRoot,
											idParts,
											ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts],
											ref instanceRoot.TableControlParts[idParts],
											ref instanceRoot.TableControlTrack[indexTrack],
											flagHideDefault,
											masking,
											false,
											ref matrixCorrection,
											flagPlanarization
										);
								break;

							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
								/* Update Instance */
								DrawInstance(	instanceRoot,
												idParts,
												ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts],
												ref instanceRoot.TableControlParts[idParts],
												ref instanceRoot.TableControlTrack[indexTrack],
												flagHideDefault,
												masking,
												false,
												ref matrixCorrection,
												flagPlanarization
											);
								break;
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
								/* Update Effect */
								DrawEffect(	instanceRoot,
											idParts,
											ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts],
											ref instanceRoot.TableControlParts[idParts],
											ref instanceRoot.TableControlTrack[indexTrack],
											flagHideDefault,
											masking,
											false,
											ref matrixCorrection,
											flagPlanarization
										);
								break;

//							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2:
//							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
								if(null == instanceRoot.InstanceRootParent)
								{
									/* (Re)Draw Mask */
									/* MEMO: Caution that "Mask"s re-draw only. */
									/*       Updating is executed in "PreDraw". */
									DrawMask(	instanceRoot,
												idParts,
												ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts],
												ref instanceRoot.TableControlTrack[indexTrack],
												flagHideDefault,
												masking,
												false,
												ref matrixCorrection,
												flagPlanarization
											);
								}
								break;

							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONE:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
								break;

							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MESH:
								DrawMesh(	instanceRoot,
											idParts,
											ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts],
											ref instanceRoot.TableControlParts[idParts],
											ref instanceRoot.TableControlTrack[indexTrack],
											flagHideDefault,
											masking,
											false,
											ref matrixCorrection,
											flagPlanarization
										);
								break;
						}
					}
				}
				private void DrawNormal(	Script_SpriteStudio6_Root instanceRoot,
											int idParts,
											ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
											ref Library_SpriteStudio6.Control.Animation.Parts controlParts,
											ref Library_SpriteStudio6.Control.Animation.Track controlTrack,
											bool flagHideDefault,
											Library_SpriteStudio6.KindMasking masking,
											bool flagPreDraw,
											ref Matrix4x4 matrixCorrection,
											bool flagPlanarization
										)
				{
					controlTrack.ArgumentContainer.IDParts = idParts;

					/* Draw Sprite */
					bool flagHide = flagHideDefault;
					flagHide |= (0 != (Status & (FlagBitStatus.HIDE_FORCE | FlagBitStatus.HIDE)));	/* ? true : false */
					if(false == flagHide)
					{
						ParameterSprite.DrawPlain(	instanceRoot,
													idParts,
													ref controlParts,
													InstanceGameObject,
													InstanceTransform,
													masking,
													flagPreDraw,
													ref matrixCorrection,
													flagPlanarization,
													ref Status,
													ref dataAnimationParts,
													ref controlTrack.ArgumentContainer
												);
					}
				}
				private void DrawInstance(	Script_SpriteStudio6_Root instanceRoot,
											int idParts,
											ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
											ref Library_SpriteStudio6.Control.Animation.Parts controlParts,
											ref Library_SpriteStudio6.Control.Animation.Track controlTrack,
											bool flagHideDefault,
											Library_SpriteStudio6.KindMasking masking,
											bool flagPreDraw,
											ref Matrix4x4 matrixCorrection,
											bool flagPlanarization
										)
				{
					if(null == InstanceRootUnderControl)
					{	/* "Instance" animation object invalid */
						return;
					}
					if(false == InstanceRootUnderControl.StatusIsValid)
					{
						InstanceRootUnderControl.StartMain();
					}

					/* Get Instance's elapsed time */
					float timeElapsed = controlTrack.TimeElapsedNow;
					bool flagStopJumpTime = (0.0f != controlTrack.TimeElapseReplacement);
					if(false == controlTrack.StatusIsPlaying)
					{	/* Stop */
						if(true == flagStopJumpTime)
						{	/* Frame Jump */
							timeElapsed = controlTrack.TimeElapseInRangeReplacement;
						}
						else
						{
							timeElapsed = 0.0f;
						}
					}
					else
					{
						if(true == controlTrack.StatusIsPausing)
						{
							timeElapsed = 0.0f;
						}
					}

					bool flagDecode = true;
					flagDecode &= !(0 != (Status & FlagBitStatus.INSTANCE_IGNORE_ATTRIBUTE));	/* Ignore Decode */
					flagDecode |= (0 != (Status & FlagBitStatus.INSTANCE_IGNORE_EXCEPT_NEXTDATA));	/* Force Decode */
					flagDecode &= controlTrack.StatusIsDecodeAttribute;

					int frame = controlTrack.ArgumentContainer.Frame;
					bool flagPlayIndependentNowInstance = (0 != (Status & FlagBitStatus.INSTANCE_PLAY_INDEPENDENT)) ? true : false;
					bool flagPlayReverse = controlTrack.StatusIsPlayingReverse;
					bool flagPlayTurn = controlTrack.StatusIsPlayingTurn;
					bool flagTopFrame = false;
					bool flagTimeWrap = false;
					float timeOffset = 0.0f;

					/* Check top frame */
					if (true == flagPlayReverse)
					{
						flagTopFrame = (controlTrack.TimeRange <= (controlTrack.TimeElapsed + controlTrack.TimePerFrameConsideredRateTime)) ? true : false;
					}
					else
					{
						flagTopFrame = (0.0f > (controlTrack.TimeElapsed - controlTrack.TimePerFrameConsideredRateTime)) ? true : false;
					}

					/* Check force-applying */
					if(true == flagPlayTurn)
					{	/* Turn */
						if(false == flagPlayIndependentNowInstance)
						{	/* Instance-Animation is depending on parent. */
							FramePreviousUpdateUnderControl = -1;
						}
					}

					/* Decode "Instance" attribute */
					int frameKey = FramePreviousUpdateUnderControl;
					if((true == flagDecode) || (-1 == FramePreviousUpdateUnderControl))
					{
						/* Decode data */
						Library_SpriteStudio6.Data.Animation.Attribute.Instance dataInstance = new Library_SpriteStudio6.Data.Animation.Attribute.Instance();
#if UNITY_EDITOR
						if(	(null !=  dataAnimationParts.Instance.Function)
							&& (true == dataAnimationParts.Instance.Function.ValueGet(ref dataInstance, ref frameKey, dataAnimationParts.Instance, ref controlTrack.ArgumentContainer))
							)
#else
						if(true == dataAnimationParts.Instance.Function.ValueGet(ref dataInstance, ref frameKey, dataAnimationParts.Instance, ref controlTrack.ArgumentContainer))
#endif
						{   /* New Valid Data */
							if(0 == (Status & FlagBitStatus.INSTANCE_IGNORE_EXCEPT_NEXTDATA))
							{	/* Attribute has priority */
								DataInstance = dataInstance;
							}
							else
							{	/* Externally set */
								/* MEMO: Decoded data is discarded. */
								FramePreviousUpdateUnderControl = -1;
							}
							Status &= ~FlagBitStatus.INSTANCE_IGNORE_EXCEPT_NEXTDATA;

							if(FramePreviousUpdateUnderControl != frameKey)
							{	/* Different attribute */
								bool flagPlayReverseInstanceData = (0.0f > DataInstance.RateTime) ? true : false;
								bool flagPlayReverseInstance = flagPlayReverseInstanceData ^ flagPlayReverse;

								/* Set Animation */
								InstancePlayStart(	instanceRoot,
//													controlTrack.RateTime * ((true == flagPlayReverse) ? -1.0f : 1.0f)
													((true == flagPlayReverse) ? -1.0f : 1.0f)
												);

								/* Adjust Starting-Time */
								/* MEMO: Necessary to set time, not frame. Because parent's elapsed time has a small excess. */
								if(true == flagPlayReverse)
								{   /* Play-Reverse */
									flagTimeWrap = flagTopFrame & flagPlayReverseInstanceData;
									if(frameKey <= frame)
									{   /* Immediately */
										/* MEMO: Calculate "Instance"'s elapsed-time in the frame range. */
										timeOffset = controlTrack.TimeElapsed - ((float)(frameKey - controlTrack.FrameStart) * controlTrack.TimePerFrame);
										timeOffset *= Mathf.Abs(DataInstance.RateTime);
										InstanceRootUnderControl.TableControlTrack[0].TimeSkip(timeOffset, flagPlayReverse, flagTimeWrap);
									}
									else
									{	/* Wait */
										if(true == flagPlayReverseInstance)
										{	/* Instance: Play-Reverse */
											InstanceRootUnderControl.TableControlTrack[0].TimeSkip(0.0f, flagPlayReverse, flagTimeWrap);
											InstanceRootUnderControl.TableControlTrack[0].TimeDelay = 0.0f;
											InstanceRootUnderControl.AnimationStop(-1, false);	/* ??? */
										}
										else
										{	/* Instance: Play-Foward */
											timeOffset = ((float)frameKey * controlTrack.TimePerFrame) - controlTrack.TimeElapsed;
											InstanceRootUnderControl.TableControlTrack[0].TimeSkip(0.0f, flagPlayReverse, flagTimeWrap);
											InstanceRootUnderControl.TableControlTrack[0].TimeDelay = timeOffset;
										}
									}
								}
								else
								{   /* Play-Foward */
									flagTimeWrap = flagTopFrame & flagPlayReverseInstanceData;
									if(frameKey <= frame)
									{   /* Immediately */
										/* MEMO: Calculate "Instance"'s elapsed-time in the frame range. */
										timeOffset = controlTrack.TimeElapsed - ((float)(frameKey - controlTrack.FrameStart) * controlTrack.TimePerFrame);
										timeOffset *= Mathf.Abs(DataInstance.RateTime);
										InstanceRootUnderControl.TableControlTrack[0].TimeSkip(timeOffset, flagPlayReverse, flagTimeWrap);
									}
									else
									{	/* Wait */
										if(true == flagPlayReverseInstance)
										{	/* Instance: Play-Reverse */
											InstanceRootUnderControl.TableControlTrack[0].TimeSkip(0.0f, flagPlayReverse, flagTimeWrap);
											InstanceRootUnderControl.TableControlTrack[0].TimeDelay = 0.0f;
											InstanceRootUnderControl.AnimationStop(-1, false);	/* ??? */
										}
										else
										{	/* Instance: Play-Foward */
											timeOffset = ((float)frameKey * controlTrack.TimePerFrame) - controlTrack.TimeElapsed;
											InstanceRootUnderControl.TableControlTrack[0].TimeSkip(0.0f, flagPlayReverse, flagTimeWrap);
											InstanceRootUnderControl.TableControlTrack[0].TimeDelay = timeOffset;
										}
									}
								}

								/* Status Update */
								FramePreviousUpdateUnderControl = frameKey;
							}
						}
					}

					/* Update "Instance" */
					/* MEMO: "Instance" is updated from here. (Not updated from Monobehaviour's LateUpdate) */
					bool flagHide = flagHideDefault;
					flagHide |= (0 != (Status & (FlagBitStatus.HIDE_FORCE | FlagBitStatus.HIDE))) ? true : false;

					if((0 != (controlParts.Status & Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.UPDATE_SCALELOCAL)) || (true == instanceRoot.StatusIsUpdateRateScaleLocal))
					{
						Vector2 scaleLocal = instanceRoot.RateScaleLocal;
						scaleLocal.x *= controlParts.ScaleLocal.Value.x;
						scaleLocal.y *= controlParts.ScaleLocal.Value.y;
						InstanceRootUnderControl.RateScaleLocal = scaleLocal;
					}

					if((0 != (controlParts.Status & Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.UPDATE_RATEOPACITY)) || (true == instanceRoot.StatusIsUpdateRateOpacity))
					{
						InstanceRootUnderControl.RateOpacity = controlParts.RateOpacity.Value * instanceRoot.RateOpacity;
					}

					InstanceRootUnderControl.LateUpdateMain(	timeElapsed,
																flagHide,
																(0 != (StatusAnimationParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_MASKING)) ? Library_SpriteStudio6.KindMasking.THROUGH : Library_SpriteStudio6.KindMasking.MASK,
																ref matrixCorrection,
																false,
																flagPlanarization
															);

					Status &= ~(	FlagBitStatus.UPDATE_SCALELOCAL
									| FlagBitStatus.UPDATE_RATEOPACITY
							);
				}
				internal bool InstancePlayStart(Script_SpriteStudio6_Root instanceRoot, float rateTime)
				{
					if(0 != (DataInstance.Flags & Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.INDEPENDENT))
					{
						Status |= FlagBitStatus.INSTANCE_PLAY_INDEPENDENT;
					}
					else
					{
						Status &= ~FlagBitStatus.INSTANCE_PLAY_INDEPENDENT;
					}

					int framePerSecond = 60;
					if(0 <= IndexControlTrack)
					{
						framePerSecond = instanceRoot.TableControlTrack[IndexControlTrack].FramePerSecond;
					}

					/* MEMO: Playing target are all tracks. And TableInformationPlay[0] is always used. */
					return(InstanceRootUnderControl.AnimationPlay(	-1,	/* All track */
																	IndexAnimationUnderControl,
																	DataInstance.PlayCount,
																	0,
																	DataInstance.RateTime * rateTime,
																	((0 != (DataInstance.Flags & Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.PINGPONG)) ? Library_SpriteStudio6.KindStylePlay.PINGPONG : Library_SpriteStudio6.KindStylePlay.NORMAL),
																	DataInstance.LabelStart,
																	DataInstance.OffsetStart,
																	DataInstance.LabelEnd,
																	DataInstance.OffsetEnd,
																	framePerSecond
																)
						);
				}

				private void DrawEffect(	Script_SpriteStudio6_Root instanceRoot,
											int idParts,
											ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
											ref Library_SpriteStudio6.Control.Animation.Parts controlParts,
											ref Library_SpriteStudio6.Control.Animation.Track controlTrack,
											bool flagHideDefault,
											Library_SpriteStudio6.KindMasking masking,
											bool flagPreDraw,
											ref Matrix4x4 matrixCorrection,
											bool flagPlanarization
										)
				{	/* CAUTION!: SpriteStudio 5.6 Unsupported. */
					if(null == InstanceRootEffectUnderControl)
					{	/* "Effect" animation object invalid */
						return;
					}
					if(false == InstanceRootEffectUnderControl.StatusIsValid)
					{
						InstanceRootEffectUnderControl.StartMain();
					}

					/* Get Instance's elapsed time */
					float timeElapsed = controlTrack.TimeElapsedNow;
					bool flagStopJumpTime = (0.0f != controlTrack.TimeElapseReplacement);
					if(false == controlTrack.StatusIsPlaying)
					{	/* Stop */
						if(true == flagStopJumpTime)
						{	/* Frame Jump */
							timeElapsed = controlTrack.TimeElapseInRangeReplacement;
						}
						else
						{
							timeElapsed = 0.0f;
						}
					}
					else
					{
						if(true == controlTrack.StatusIsPausing)
						{
							timeElapsed = 0.0f;
						}
					}

					int frame = controlTrack.ArgumentContainer.Frame;
					bool flagDecode = !(0 != (Status & FlagBitStatus.EFFECT_IGNORE_ATTRIBUTE));	/* (0 != (Status &= FlagBitStatus.EFFECT_IGNORE_ATTRIBUTE)) ? false : true; */
					flagDecode |= (0 != (Status & FlagBitStatus.EFFECT_IGNORE_EXCEPT_NEXTDATA));	/* ? true : false */
					flagDecode &= controlTrack.StatusIsDecodeAttribute;
					bool flagPlayIndependentNowInstance = (0 != (Status & FlagBitStatus.EFFECT_PLAY_INDEPENDENT)) ? true : false;
					bool flagPlayTurn = controlTrack.StatusIsPlayingTurn;
					bool flagPlayReverse = controlTrack.StatusIsPlayingReverse;
					float timeOffset = 0.0f;
					if(true == flagPlayTurn)
					{
						if(false == flagPlayIndependentNowInstance)
						{
							FramePreviousUpdateUnderControl = -1;
						}
					}

					/* Decode "Effect" attribute */
					int frameKey = FramePreviousUpdateUnderControl;
					if(true == flagDecode)
					{
						/* Decode data */
						Library_SpriteStudio6.Data.Animation.Attribute.Effect dataEffect = new Library_SpriteStudio6.Data.Animation.Attribute.Effect();
#if UNITY_EDITOR
						if(	(null != dataAnimationParts.Effect.Function)
							&& (true == dataAnimationParts.Effect.Function.ValueGet(ref dataEffect, ref frameKey, dataAnimationParts.Effect, ref controlTrack.ArgumentContainer))
							)
#else
						if(true == dataAnimationParts.Effect.Function.ValueGet(ref dataEffect, ref frameKey, dataAnimationParts.Effect, ref controlTrack.ArgumentContainer))
#endif
						{   /* New Valid Data */
							if(FramePreviousUpdateUnderControl != frameKey)
							{	/* Different attribute */
								/* Wait Set */
								if(frameKey <= frame)
								{	/* Immediately */
									/* Play-Start */
									InstanceRootEffectUnderControl.AnimationPlay(	1,
//																					controlTrack.RateTime * ((true == flagPlayReverse) ? -1.0f : 1.0f) * dataEffect.RateTime,
																					((true == flagPlayReverse) ? -1.0f : 1.0f) * dataEffect.RateTime,
																					controlTrack.FramePerSecond
																				);

									InstanceRootEffectUnderControl.SeedOffsetSet((uint)controlTrack.CountLoop);

									/* Adjust Time */
									/* MEMO: Calculate "Effect"'s elapsed-time in the frame range. */
									timeOffset = controlTrack.TimeElapsed - ((float)(frameKey - controlTrack.FrameStart) * controlTrack.TimePerFrame);
									timeOffset *= dataEffect.RateTime;	/* "Effect" does not have a negative speed. */
									timeOffset += InstanceRootEffectUnderControl.TimeGetFramePosition(dataEffect.FrameStart);
									InstanceRootEffectUnderControl.TimeSkip(timeOffset, false);

									/* Status Update */
									FramePreviousUpdateUnderControl = frameKey;
									Status = (0 != (dataEffect.Flags & Library_SpriteStudio6.Data.Animation.Attribute.Effect.FlagBit.INDEPENDENT)) ? (Status | FlagBitStatus.EFFECT_PLAY_INDEPENDENT) : (Status & ~FlagBitStatus.EFFECT_PLAY_INDEPENDENT);
								}
							}
						}
					}

					/* Update Effect */
					/* MEMO: "Effect" is updated from here. (Not updated from Monobehaviour's LateUpdate) */
					bool flagHide = flagHideDefault;
					flagHide |= (0 != (Status & (FlagBitStatus.HIDE_FORCE | FlagBitStatus.HIDE))) ? true : false;

					/* MEMO: Always update "ScaleLocal" and "RateOpacity". */
					Vector2 scaleLocal = instanceRoot.RateScaleLocal;
					scaleLocal.x *= controlParts.ScaleLocal.Value.x;
					scaleLocal.y *= controlParts.ScaleLocal.Value.y;
					InstanceRootEffectUnderControl.RateScaleLocal = scaleLocal;

					InstanceRootEffectUnderControl.RateOpacity = controlParts.RateOpacity.Value * instanceRoot.RateOpacity;
					if(false == flagHide)
					{
						InstanceRootEffectUnderControl.LateUpdateMain(	timeElapsed,
																		flagHide,
																		(0 != (StatusAnimationParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_MASKING)) ? Library_SpriteStudio6.KindMasking.THROUGH : Library_SpriteStudio6.KindMasking.MASK,
																		ref matrixCorrection,
																		flagPlanarization
																	);
					}

					Status &= ~(	FlagBitStatus.UPDATE_SCALELOCAL
									| FlagBitStatus.UPDATE_RATEOPACITY
							);
				}
				private void DrawMask(	Script_SpriteStudio6_Root instanceRoot,
										int idParts,
										ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
										ref Library_SpriteStudio6.Control.Animation.Track controlTrack,
										bool flagHideDefault,
										Library_SpriteStudio6.KindMasking masking,
										bool flagPreDraw,
										ref Matrix4x4 matrixCorrection,
										bool flagPlanarization
									)
				{
					bool flagHide = flagHideDefault;
					flagHide |= (0 != (Status & (FlagBitStatus.HIDE_FORCE | FlagBitStatus.HIDE))) ? true : false;
					if(true == flagHide)
					{
						return;
					}

					/* Set to Draw-Cluster */
					/* MEMO: Use same value as in "PreDraw", except for shaders and Draw-Chain. */
#if false
					ParameterSprite.DrawAddCluster(instanceRoot.ClusterDraw, ParameterSprite.ChainDrawMask, ParameterSprite.MaterialDrawMask);
#else
					instanceRoot.ClusterDraw.VertexAdd(	ParameterSprite.ChainDrawMask,
														ParameterSprite.CountVertex,
														ParameterSprite.CoordinateTransformDraw,
														ParameterSprite.ColorPartsDraw,
														ParameterSprite.UVTextureDraw,
														ParameterSprite.ParameterBlendDraw,
														ParameterSprite.MaterialDrawMask
													);
#endif
				}

				internal void AnimationChange()
				{
					Status |= (	FlagBitStatus.REFRESH_TRANSFORM_POSITION
								| FlagBitStatus.REFRESH_TRANSFORM_ROTATION
								| FlagBitStatus.REFRESH_TRANSFORM_SCALING
							);

					StatusAnimationParts = Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.CLEAR;

					bool flagClearCellApply = false;
					if(0 == (Status & FlagBitStatus.CHANGE_CELL_IGNORE_NEWANIMATION))
					{
						Status &= ~(FlagBitStatus.CHANGE_CELL_IGNORE_ATTRIBUTE | FlagBitStatus.CHANGE_CELL_UNREFLECTED);
						flagClearCellApply = true;
					}

					if(0 == (Status & FlagBitStatus.INSTANCE_IGNORE_NEWANIMATION))
					{
						Status &= ~(FlagBitStatus.INSTANCE_IGNORE_ATTRIBUTE | FlagBitStatus.INSTANCE_PLAY_INDEPENDENT);
					}

					if(0 == (Status & FlagBitStatus.EFFECT_IGNORE_NEWANIMATION))
					{
						Status &= ~(FlagBitStatus.EFFECT_IGNORE_ATTRIBUTE | FlagBitStatus.EFFECT_PLAY_INDEPENDENT);
					}

					TRSMaster.CleanUp();
					TRSSlave.CleanUp();

					CacheClearAttribute(flagClearCellApply);
				}
				internal void CacheClearAttribute(bool flagClearCellApply)
				{
					ScaleLocal.CleanUp();	ScaleLocal.Value = Vector2.one;
					RateOpacity.CleanUp();	RateOpacity.Value = 1.0f;
					Priority.CleanUp();	Priority.Value = 0;

					FramePreviousUpdateUnderControl = -1;
					FramePreviousUpdateRadiusCollision = -1;
					FrameKeyStatusAnimationFrame = -1;
					StatusAnimationFrame.Flags = Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.INITIAL;

					ParameterSprite.AnimationChange(flagClearCellApply);
				}
				private void DrawMesh(	Script_SpriteStudio6_Root instanceRoot,
										int idParts,
										ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
										ref Library_SpriteStudio6.Control.Animation.Parts controlParts,
										ref Library_SpriteStudio6.Control.Animation.Track controlTrack,
										bool flagHideDefault,
										Library_SpriteStudio6.KindMasking masking,
										bool flagPreDraw,
										ref Matrix4x4 matrixCorrection,
										bool flagPlanarization
									)
				{
					/* MEMO: Since specification of pre-calculating has not been decided, currently use only "DrawMeshPlain". */
					controlTrack.ArgumentContainer.IDParts = idParts;

					/* Draw Sprite */
					bool flagHide = flagHideDefault;
					flagHide |= (0 != (Status & (FlagBitStatus.HIDE_FORCE | FlagBitStatus.HIDE)));	/* ? true : false */
					if(false == flagHide)
					{
						ParameterSprite.DrawMeshPlain(	instanceRoot,
														idParts,
														ref controlParts,
														InstanceGameObject,
														InstanceTransform,
														masking,
														flagPreDraw,
														ref matrixCorrection,
														flagPlanarization,
														ref Status,
														ref dataAnimationParts,
														ref controlTrack.ArgumentContainer
													);
					}
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				[System.Flags]
				internal enum FlagBitStatus
				{
					VALID = 0x40000000,
					HIDE_FORCE = 0x20000000,
					HIDE = 0x10000000,

					CHANGE_TRANSFORM_SCALING = 0x04000000,
					CHANGE_TRANSFORM_ROTATION = 0x02000000,
					CHANGE_TRANSFORM_POSITION = 0x01000000,

					REFRESH_TRANSFORM_SCALING = 0x00400000,
					REFRESH_TRANSFORM_ROTATION = 0x00200000,
					REFRESH_TRANSFORM_POSITION = 0x00100000,

					UPDATE_SCALELOCAL = 0x00080000,
					UPDATE_RATEOPACITY = 0x00040000,

					CHANGE_CELL_UNREFLECTED = 0x00000800,
					CHANGE_CELL_IGNORE_ATTRIBUTE = 0x00000200,
					CHANGE_CELL_IGNORE_NEWANIMATION = 0x00000100,

					INSTANCE_PLAY_INDEPENDENT = 0x00000080,
					INSTANCE_IGNORE_EXCEPT_NEXTDATA = 0x00000040,
					INSTANCE_IGNORE_ATTRIBUTE = 0x00000020,
					INSTANCE_IGNORE_NEWANIMATION = 0x00000010,

					EFFECT_PLAY_INDEPENDENT = 0x00000008,
					EFFECT_IGNORE_EXCEPT_NEXTDATA = 0x00000004,
					EFFECT_IGNORE_ATTRIBUTE = 0x00000002,
					EFFECT_IGNORE_NEWANIMATION = 0x00000001,

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
					internal FlagBitStatus Status;

					internal Library_SpriteStudio6.Data.Animation.Attribute.Status DataStatusPrevious;

					internal Library_SpriteStudio6.KindMasking Masking;

					internal Vector2 RateScaleMesh;
					internal Vector2 RateScaleTexture;

					internal int CountVertex;
					internal Vector3[] CoordinateTransformDraw;
					internal Vector3[] DeformDraw;
					internal Vector3[] CoordinateDraw;
					internal Color32[] ColorPartsDraw;
					internal Vector2[] ParameterBlendDraw;
					internal Vector2[] UVTextureDraw;
					internal int[] IndexVertexDraw;
					internal Material MaterialDraw;	/* "Sprite"'s Draw & "Mask"'s Pre-Draw */
					internal Material MaterialDrawMask;	/* "Mask"'s Draw */
					internal Library_SpriteStudio6.Draw.Cluster.Chain ChainDraw;	/* "Sprite"'s Draw & "Mask"'s Pre-Draw */
					internal Library_SpriteStudio6.Draw.Cluster.Chain ChainDrawMask;	/* "Mask"'s Draw */

					internal Vector2 SizeSprite;
					internal Vector2 PivotSprite;
					internal Vector2 SizeTexture;
					internal Vector2 SizeCell;
					internal Vector2 PivotCell;
					internal Vector2 PositionCell;
					internal Matrix4x4 MatrixTexture;
					internal Library_SpriteStudio6.Data.Animation.Attribute.Cell DataCellApply;
					internal int IndexVertexCollectionTable;

					internal BufferAttribute<Library_SpriteStudio6.Data.Animation.Attribute.Cell> DataCell;
					internal BufferAttribute<Library_SpriteStudio6.Data.Animation.Attribute.PartsColor> PartsColor;
					internal BufferAttribute<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection> VertexCorrection;
					internal BufferAttribute<Library_SpriteStudio6.Data.Animation.Attribute.Deform> Deform;
					internal BufferAttribute<Vector2> OffsetPivot;
					internal BufferAttribute<Vector2> SizeForce;
					internal BufferAttribute<Vector2> ScalingTexture;
					internal BufferAttribute<float> RotationTexture;
					internal BufferAttribute<Vector2> PositionTexture;

					/* MEMO: Backup for lost by shallow copy at decoding. */
					internal Color[] TableVertexColorPartsColor;
					internal float[] TableRateAlphaPartsColor;
					internal Vector2[] TableCoordinateVertexCorrection;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					internal void CleanUp()
					{
						Status = FlagBitStatus.CLEAR;

						MaterialDraw = null;
						CoordinateTransformDraw = null;
						CoordinateDraw = null;
						DeformDraw = null;
						ColorPartsDraw = null;
						UVTextureDraw = null;
						IndexVertexDraw = null;
						ParameterBlendDraw = null;
						ChainDraw = null;
						ChainDrawMask = null;

						TableVertexColorPartsColor = null;
						TableRateAlphaPartsColor = null;
						TableCoordinateVertexCorrection = null;

						AnimationChange(true);
					}

					internal void AnimationChange(bool flagClearDataCellApply)
					{
						/* MEMO: Do not clear dynamic flipping. */
//						Status = FlagBitStatus.CLEAR;
						Status &= (	FlagBitStatus.FLIP_COEFFICIENT_X
									| FlagBitStatus.FLIP_COEFFICIENT_Y
									| FlagBitStatus.FLIP_COEFFICIENT_TEXTURE_X
									| FlagBitStatus.FLIP_COEFFICIENT_TEXTURE_Y
								);

						DataStatusPrevious.CleanUp();

						Masking = (Library_SpriteStudio6.KindMasking)(-1);

						RateScaleMesh = Vector2.one;
						RateScaleTexture = Vector2.one;

						SizeCell = Vector2.zero;
						PivotCell = Vector2.zero;
						SizeTexture = Library_SpriteStudio6.Control.Animation.SizeTextureDefault;
						SizeCell = Library_SpriteStudio6.Control.Animation.SizeTextureDefault;
						PivotCell = Vector2.zero;
						PositionCell = Vector2.zero;
						MatrixTexture = Matrix4x4.identity;
						if(true == flagClearDataCellApply)
						{
							DataCellApply.CleanUp();
						}
						IndexVertexCollectionTable = 0;

						DataCell.CleanUp();	DataCell.Value.CleanUp();
						OffsetPivot.CleanUp();	OffsetPivot.Value = Vector2.zero;
						SizeForce.CleanUp();	SizeForce.Value = -Vector2.one;
						ScalingTexture.CleanUp();	ScalingTexture.Value = Vector2.one;
						PositionTexture.CleanUp();	PositionTexture.Value = Vector2.zero;
						RotationTexture.CleanUp();	RotationTexture.Value = 0.0f;
						PartsColor.CleanUp();	/* PartsColor.Value.CleanUp(); */
						VertexCorrection.CleanUp();	/* VertexCorrection.Value.CleanUp(); */
						Deform.CleanUp();	Deform.Value.CoordinateReset();
					}

					internal bool BootUp(int countVertex, int countPartsSprite, bool flagMask)
					{
						CleanUp();

						CountVertex = countVertex;

						MaterialDraw = null;
						CoordinateTransformDraw = new Vector3[countVertex];
						if(null == CoordinateTransformDraw)
						{
							goto BootUp_ErrorEnd;
						}

						DeformDraw = null;	/* unused */

						CoordinateDraw = new Vector3[countVertex];
						if(null == CoordinateDraw)
						{
							goto BootUp_ErrorEnd;
						}

						ColorPartsDraw = new Color32[countVertex];
						if(null == ColorPartsDraw)
						{
							goto BootUp_ErrorEnd;
						}

						UVTextureDraw = new Vector2[countVertex];
						if(null == UVTextureDraw)
						{
							goto BootUp_ErrorEnd;
						}

						ParameterBlendDraw = new Vector2[countVertex];
						if(null == ParameterBlendDraw)
						{
							goto BootUp_ErrorEnd;
						}

						IndexVertexDraw = null;	/* Disuse */

						for(int i=0; i<countVertex; i++)
						{
							CoordinateDraw[i] = Library_SpriteStudio6.Draw.Model.TableCoordinate[i];
							ColorPartsDraw[i] = Library_SpriteStudio6.Draw.Model.TableColor32[i];
							UVTextureDraw[i] = Library_SpriteStudio6.Draw.Model.TableUVMapping[i];
							ParameterBlendDraw[i] = Vector2.zero;
						}

						VertexCorrection.Value.BootUp();
						TableCoordinateVertexCorrection = VertexCorrection.Value.Coordinate;

						Deform.CleanUp();	Deform.Value.CleanUp();	/* Disused */

						/* MEMO: Direct-write into draw's workarea. */
//						CoordinateFix.Value.TableCoordinate = CoordinateDraw;
//						UV0Fix.Value.TableUV = UVTextureDraw;

						if(false == BootUpCommon(countVertex, flagMask))
						{
							/* MEMO: Since workareas have been cleared, return direct. */
							return(false);
						}
						return(true);

					BootUp_ErrorEnd:;
						MaterialDraw = null;
						CoordinateDraw = null;
						ColorPartsDraw = null;
						UVTextureDraw = null;
						IndexVertexDraw = null;
						ParameterBlendDraw = null;
						ChainDraw = null;
						return(false);
					}
					private bool BootUpCommon(int countPartsColorBuffer, bool flagMask)
					{
						ChainDraw = new Draw.Cluster.Chain();
						if(null == ChainDraw)
						{
							goto BootUpCommon_ErrorEnd;
						}
//						ChainDraw.CleanUp();
						ChainDraw.BootUp();

						if(true == flagMask)
						{
							ChainDrawMask = new Draw.Cluster.Chain();
							if(null == ChainDrawMask)
							{
								goto BootUpCommon_ErrorEnd;
							}
//							ChainDrawMask.CleanUp();
							ChainDrawMask.BootUp();
						}

						PartsColor.Value.BootUp((int)Library_SpriteStudio6.KindVertex.TERMINATOR2);
						TableVertexColorPartsColor = PartsColor.Value.VertexColor;
						TableRateAlphaPartsColor = PartsColor.Value.RateAlpha;

						Status |= (	FlagBitStatus.UPDATE_COORDINATE
									| FlagBitStatus.UPDATE_UVTEXTURE
									| FlagBitStatus.UPDATE_PARAMETERBLEND
									| FlagBitStatus.UPDATE_COLORPARTS
									| FlagBitStatus.UPDATE_MASKING
									| FlagBitStatus.UPDATE_DEFORM
									| FlagBitStatus.UPDATE_COORDINATE_NOWFRAME
								);

						return(true);

					BootUpCommon_ErrorEnd:;
						MaterialDraw = null;
						CoordinateTransformDraw = null;
						CoordinateDraw = null;
						DeformDraw = null;
						ColorPartsDraw = null;
						UVTextureDraw = null;
						ParameterBlendDraw = null;
						ChainDraw = null;
						return(false);
					}

					internal bool BootUpMesh(Script_SpriteStudio6_Root instanceRoot, int idParts, bool flagMask)
					{
						CleanUp();

						/* MEMO: On SpriteStudio6.0, Cell's vertices count and MeshBind's vertices count may be different. */
						/*       (Truncate surplus information)                                                            */
						int countVertex = instanceRoot.DataAnimation.TableParts[idParts].Mesh.CountVertex;
						int countTableBind = instanceRoot.DataAnimation.TableParts[idParts].Mesh.TableVertex.Length;
						int countTableUV = instanceRoot.DataAnimation.TableParts[idParts].Mesh.TableRateUV.Length;
						int countVertexDeform = instanceRoot.DataAnimation.TableParts[idParts].Mesh.CountVertexDeform;
						CountVertex = countVertex;

						MaterialDraw = null;
						CoordinateTransformDraw = new Vector3[countVertex];
						if(null == CoordinateTransformDraw)
						{
							goto BootUpMesh_ErrorEnd;
						}

						if(0 >= countVertexDeform)
						{	/* not use Deform */
							DeformDraw = null;	/* unused */
						}
						else
						{	/* use Deform */
							DeformDraw = new Vector3[countVertex];
							if(null == DeformDraw)
							{
								goto BootUpMesh_ErrorEnd;
							}
						}

						if(0 < countTableBind)
						{	/* Skeletal-Animation */
							CoordinateDraw = null;	/* unused */
						}
						else
						{	/* Fix-Animation */
							CoordinateDraw = new Vector3[countVertex];
							if(null == CoordinateDraw)
							{
								goto BootUpMesh_ErrorEnd;
							}
						}

						ColorPartsDraw = new Color32[countVertex];
						if(null == ColorPartsDraw)
						{
							goto BootUpMesh_ErrorEnd;
						}

						UVTextureDraw = new Vector2[countVertex];
						if(null == UVTextureDraw)
						{
							goto BootUpMesh_ErrorEnd;
						}

						ParameterBlendDraw = new Vector2[countVertex];
						if(null == ParameterBlendDraw)
						{
							goto BootUpMesh_ErrorEnd;
						}

						IndexVertexDraw = instanceRoot.DataAnimation.TableParts[idParts].Mesh.TableIndexVertex;

						VertexCorrection.CleanUp();	/* Disused */
						TableCoordinateVertexCorrection = null;	/* Disused */

						Deform.CleanUp();
						Deform.Value.CleanUp();
						if(0 < countVertexDeform)
						{
							Deform.Value.BootUp(countVertexDeform);
							Deform.Value.CoordinateReset();
						}

						if(false == BootUpCommon(0, flagMask))
						{
							/* MEMO: Since workareas have been cleared, return direct. */
							return(false);
						}
						return(true);

					BootUpMesh_ErrorEnd:;
						MaterialDraw = null;
						CoordinateDraw = null;
						ColorPartsDraw = null;
						UVTextureDraw = null;
						IndexVertexDraw = null;
						ParameterBlendDraw = null;
						ChainDraw = null;
						return(false);
					}

					internal void StatusSetFlip(ref Library_SpriteStudio6.Data.Animation.Attribute.Status status)
					{
						/* Update Check */
						Library_SpriteStudio6.Data.Animation.Attribute.Status statusNow = status;
						Library_SpriteStudio6.Data.Animation.Attribute.Status statusUpdate = new Library_SpriteStudio6.Data.Animation.Attribute.Status();
						Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit statusAdditionalFlip = (Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit)(
							(int)(Status & (	FlagBitStatus.FLIP_COEFFICIENT_X
												| FlagBitStatus.FLIP_COEFFICIENT_Y
												| FlagBitStatus.FLIP_COEFFICIENT_TEXTURE_X
												| FlagBitStatus.FLIP_COEFFICIENT_TEXTURE_Y
											)
									) << (int)FlagShiftStatus.FLIP_COEFFICIENT_TO_ATTRIBUTE
							);
						statusNow.Flags ^= statusAdditionalFlip;
						if(false == DataStatusPrevious.IsValid)
						{
							statusUpdate.Flags =	Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.HIDE
													| Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_X
													| Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_Y
													| Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_TEXTURE_X
													| Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_TEXTURE_Y;
						}
						else
						{
							/* MEMO: Extract only changed bits. */
//							statusUpdate.Flags = DataStatusPrevious.Flags ^ status.Flags;
							statusUpdate.Flags = DataStatusPrevious.Flags ^ statusNow.Flags;
						}

						if(0 != (statusUpdate.Flags & (	Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_TEXTURE_X
														| Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_TEXTURE_Y
													)
								)
							)	/* ((true == statusUpdate.IsTextureFlipX) || (true == statusUpdate.IsTextureFlipY)) */
						{
							Status |= FlagBitStatus.UPDATE_TRANSFORM_TEXTURE;
						}
						if(0 != (statusUpdate.Flags & (	Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_X
														| Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_Y
													)
								)
							)	/* ((true == statusUpdate.IsFlipX) || (true == statusUpdate.IsFlipY)) */
						{
							Status |= FlagBitStatus.UPDATE_COORDINATE;
						}
//						DataStatusPrevious.Flags = status.Flags;
						DataStatusPrevious.Flags = statusNow.Flags;

						/* Dicide Sprite's scale (Flipping) & Vertex Order */
						IndexVertexCollectionTable = 0;
//						if(true == status.IsFlipX)
						if(true == statusNow.IsFlipX)
						{
							RateScaleMesh.x = -1.0f;
							IndexVertexCollectionTable += 1;
						}
						else
						{
							RateScaleMesh.x = 1.0f;
						}
//						if(true == status.IsFlipY)
						if(true == statusNow.IsFlipY)
						{
							RateScaleMesh.y = -1.0f;
							IndexVertexCollectionTable += 2;
						}
						else
						{
							RateScaleMesh.y = 1.0f;
						}

						/* Dicide Texture's scale (Flipping) */
//						if(true == status.IsTextureFlipX)
						if(true == statusNow.IsTextureFlipX)
						{
							RateScaleTexture.x = -1.0f;
						}
						else
						{
							RateScaleTexture.x = 1.0f;
						}
//						if(true == status.IsTextureFlipY)
						if(true == statusNow.IsTextureFlipY)
						{
							RateScaleTexture.y = -1.0f;
						}
						else
						{
							RateScaleTexture.y = 1.0f;
						}
					}

					internal bool UpdatePlain(	Script_SpriteStudio6_Root instanceRoot,
												int idParts,
												GameObject instanceGameObject,
												Transform instanceTransform,
												ref Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus statusControlParts,
												ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer
											)
					{
						Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus statusPartsAnimation = dataAnimationParts.StatusParts;
						bool flagUpdateValueAttribute;

						/* Check Part-Control's update */
						if((0 != (statusControlParts & Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.UPDATE_SCALELOCAL)) || (true == instanceRoot.StatusIsUpdateRateScaleLocal))
						{
							Status |= FlagBitStatus.UPDATE_COORDINATE;
						}

						if((0 != (statusControlParts & Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.UPDATE_RATEOPACITY)) || (true == instanceRoot.StatusIsUpdateRateOpacity))
						{
							Status |= FlagBitStatus.UPDATE_PARAMETERBLEND;
						}

						/* Create sprite data (from cell to use) */
						/* MEMO: If do not always decode "Cell", malfunctions at restoration after cell-change. */
						/* MEMO: Since "UPDATE_COORDINATE" is cumulative status for drawing, it is necessary   */
						/*        to judge "Cell"-changing in current frame with "UPDATE_COORDINATE_NOWFRAME". */
						/*       If judge only with UPDATE_COORDINATE, miss-update with non-drawing parts.     */
#if UNITY_EDITOR
						if(null != dataAnimationParts.Cell.Function)
						{
							flagUpdateValueAttribute = dataAnimationParts.Cell.Function.ValueGet(ref DataCell.Value, ref DataCell.FrameKey, dataAnimationParts.Cell, ref argumentContainer);
						}
						else
						{
							flagUpdateValueAttribute = false;
						}
#else
						flagUpdateValueAttribute = dataAnimationParts.Cell.Function.ValueGet(ref DataCell.Value, ref DataCell.FrameKey, dataAnimationParts.Cell, ref argumentContainer);
#endif
						if(0 == (statusControlParts & Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.CHANGE_CELL_UNREFLECTED))
						{
							if(0 == (statusControlParts & Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.CHANGE_CELL_IGNORE_ATTRIBUTE))
							{
								if(true == flagUpdateValueAttribute)
								{	/* New Data */
									DataCellApply = DataCell.Value;

									Status |= FlagBitStatus.UPDATE_UVTEXTURE;
									Status |= FlagBitStatus.UPDATE_COORDINATE;
									Status |= FlagBitStatus.UPDATE_TRANSFORM_TEXTURE;
									Status |= FlagBitStatus.UPDATE_COORDINATE_NOWFRAME;
								}
							}
						}
						else
						{	/* New Value (Cell changed from script) */
							if((0 > DataCellApply.IndexCellMap) || (0 > DataCellApply.IndexCell))
							{
								DataCellApply = DataCell.Value;
							}

							Status |= FlagBitStatus.UPDATE_COORDINATE;
							Status |= FlagBitStatus.UPDATE_UVTEXTURE;
							Status |= FlagBitStatus.UPDATE_COORDINATE_NOWFRAME;
						}
						statusControlParts &= ~Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.CHANGE_CELL_UNREFLECTED;

						int indexCellMap = DataCellApply.IndexCellMap;
						int indexCell = DataCellApply.IndexCell;
						if(0 > indexCellMap)
						{
							Status |= FlagBitStatus.NO_DRAW;
						}
						else
						{
							Status &= ~FlagBitStatus.NO_DRAW;
						}

						Library_SpriteStudio6.Data.CellMap cellMap = instanceRoot.DataGetCellMap(indexCellMap);
						if(null == cellMap)
						{	/* CellMap Invalid */
							indexCellMap = -1;
							indexCell = -1;
						}
						else
						{	/* CellMap Valid */
							if((0 > indexCell) || (cellMap.CountGetCell() <= indexCell))
							{	/* Cell Invalid */
								indexCellMap = -1;
								indexCell = -1;
							}
						}
						if(0 > indexCellMap)
						{	/* Invalid */
							SizeTexture = Library_SpriteStudio6.Control.Animation.SizeTextureDefault;

							SizeCell = Library_SpriteStudio6.Control.Animation.SizeTextureDefault;
//							PivotCell = Vector2.zero;
							PivotCell = SizeCell * 0.5f;
							PositionCell = Vector2.zero;
						}
						else
						{	/* Valid */
							SizeTexture = cellMap.SizeOriginal;

							SizeCell = cellMap.TableCell[indexCell].Rectangle.size;
							PivotCell = cellMap.TableCell[indexCell].Pivot;
							PositionCell = cellMap.TableCell[indexCell].Rectangle.position;
						}

						Vector2 sizeSprite = SizeCell;
						Vector2 pivotSprite = PivotCell;

						/* Correct Sprite data (by attributes) */
//						bool flagRecalcSizeSprite = false;
						bool flagRecalcSizeSprite = (0 != (Status & FlagBitStatus.UPDATE_COORDINATE_NOWFRAME));	/* ? true : false */
#if UNITY_EDITOR
						if(null != dataAnimationParts.OffsetPivot.Function)
						{
							flagUpdateValueAttribute = dataAnimationParts.OffsetPivot.Function.ValueGet(ref OffsetPivot.Value, ref OffsetPivot.FrameKey, dataAnimationParts.OffsetPivot, ref argumentContainer);
						}
						else
						{
							flagUpdateValueAttribute = false;
						}
#else
						flagUpdateValueAttribute = dataAnimationParts.OffsetPivot.Function.ValueGet(ref OffsetPivot.Value, ref OffsetPivot.FrameKey, dataAnimationParts.OffsetPivot, ref argumentContainer);
#endif
						if(true == flagUpdateValueAttribute)
						{
							Status |= FlagBitStatus.UPDATE_COORDINATE;
							flagRecalcSizeSprite |= true;
						}

#if UNITY_EDITOR
						if(null != dataAnimationParts.SizeForce.Function)
						{
							flagUpdateValueAttribute = dataAnimationParts.SizeForce.Function.ValueGet(ref SizeForce.Value, ref SizeForce.FrameKey, dataAnimationParts.SizeForce, ref argumentContainer);
						}
						else
						{
							flagUpdateValueAttribute = false;
						}
#else
						flagUpdateValueAttribute = dataAnimationParts.SizeForce.Function.ValueGet(ref SizeForce.Value, ref SizeForce.FrameKey, dataAnimationParts.SizeForce, ref argumentContainer);
#endif
						if(true == flagUpdateValueAttribute)
						{
							Status |= FlagBitStatus.UPDATE_COORDINATE;
							flagRecalcSizeSprite |= true;
						}

						if(true == flagRecalcSizeSprite)
						{
							float ratePivot;
							float size;

							pivotSprite.x += (sizeSprite.x * OffsetPivot.Value.x);
							pivotSprite.y -= (sizeSprite.y * OffsetPivot.Value.y);

							size = SizeForce.Value.x;
							if(0.0f <= size)
							{
								ratePivot = pivotSprite.x / sizeSprite.x;
								sizeSprite.x = size;
								pivotSprite.x = size * ratePivot;
							}

							size = SizeForce.Value.y;
							if(0.0f <= size)
							{
								ratePivot = pivotSprite.y / sizeSprite.y;
								sizeSprite.y = size;
								pivotSprite.y = size * ratePivot;
							}
						}

//						if(0 != (Status & FlagBitStatus.UPDATE_COORDINATE))
						if(true == flagRecalcSizeSprite)
						{	/* Re-Set Sprite's Size & Pivot (only when coordinates updateed) */
							SizeSprite = sizeSprite;
							PivotSprite = pivotSprite;
							Status |= FlagBitStatus.UPDATE_COLLIDERRECTANGLE_NOWFRAME;
						}

						/* Get Texture-Transform */
						if(0 == (statusPartsAnimation & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_TRANSFORMATION_TEXTURE))
						{	/* Transform Texure */
#if UNITY_EDITOR
							bool flagUpdateMatrixTexrure = false;
							if(null != dataAnimationParts.PositionTexture.Function)
							{
								flagUpdateMatrixTexrure = dataAnimationParts.PositionTexture.Function.ValueGet(ref PositionTexture.Value, ref PositionTexture.FrameKey, dataAnimationParts.PositionTexture, ref argumentContainer);
							}
							if(null != dataAnimationParts.ScalingTexture.Function)
							{
								flagUpdateMatrixTexrure |= dataAnimationParts.ScalingTexture.Function.ValueGet(ref ScalingTexture.Value, ref ScalingTexture.FrameKey, dataAnimationParts.ScalingTexture, ref argumentContainer);
							}
							if(null != dataAnimationParts.RotationTexture.Function)
							{
								flagUpdateMatrixTexrure |= dataAnimationParts.RotationTexture.Function.ValueGet(ref RotationTexture.Value, ref RotationTexture.FrameKey, dataAnimationParts.RotationTexture, ref argumentContainer);
							}
#else
							bool flagUpdateMatrixTexrure = dataAnimationParts.PositionTexture.Function.ValueGet(ref PositionTexture.Value, ref PositionTexture.FrameKey, dataAnimationParts.PositionTexture, ref argumentContainer);
							flagUpdateMatrixTexrure |= dataAnimationParts.ScalingTexture.Function.ValueGet(ref ScalingTexture.Value, ref ScalingTexture.FrameKey, dataAnimationParts.ScalingTexture, ref argumentContainer);
							flagUpdateMatrixTexrure |= dataAnimationParts.RotationTexture.Function.ValueGet(ref RotationTexture.Value, ref RotationTexture.FrameKey, dataAnimationParts.RotationTexture, ref argumentContainer);
#endif
							if(true == flagUpdateMatrixTexrure)
							{
								Status |= FlagBitStatus.UPDATE_TRANSFORM_TEXTURE;
							}
						}

						/* Get & Select PartsColor or AdditionalColor */
						/* MEMO: Although data not depending on format(Plain or Fix), processing is somewhat different for each parts-feature. */
						bool flagUseAdditionalColor = false;
						if(0 != (Status & FlagBitStatus.USE_ADDITIONALCOLOR))
						{
							Status |= FlagBitStatus.USE_ADDITIONALCOLOR_PREVIOUS;
						}
						else
						{
							Status &= ~FlagBitStatus.USE_ADDITIONALCOLOR_PREVIOUS;
						}
						/* MEMO: When decode data, array may change to original data by shallow copy, so replace buffers. */
						/*       (Need to set writable buffer since some decoders write to buffer)                        */
						Color[] tableVertexColorPartsColorOld = PartsColor.Value.VertexColor;
						float[] tableRateAlphaPartsColorOld = PartsColor.Value.RateAlpha;
						PartsColor.Value.VertexColor = TableVertexColorPartsColor;
						PartsColor.Value.RateAlpha = TableRateAlphaPartsColor;
#if UNITY_EDITOR
						if(null != dataAnimationParts.PartsColor.Function)
						{
							flagUpdateValueAttribute = dataAnimationParts.PartsColor.Function.ValueGet(ref PartsColor.Value, ref PartsColor.FrameKey, dataAnimationParts.PartsColor, ref argumentContainer);
						}
						else
						{
							flagUpdateValueAttribute = false;
						}
#else
						flagUpdateValueAttribute = dataAnimationParts.PartsColor.Function.ValueGet(ref PartsColor.Value, ref PartsColor.FrameKey, dataAnimationParts.PartsColor, ref argumentContainer);
#endif
						Library_SpriteStudio6.Control.AdditionalColor additionalColor = instanceRoot.AdditionalColor;
						if(null != additionalColor)
						{	/* Has AdditionalColor */
							if(Library_SpriteStudio6.KindOperationBlend.NON != additionalColor.OperationBlend)
							{
								if(0 != (additionalColor.Status & Library_SpriteStudio6.Control.AdditionalColor.FlagBitStatus.CHANGE))
								{
									Status |= FlagBitStatus.UPDATE_COLORPARTS;
									Status |= FlagBitStatus.UPDATE_PARAMETERBLEND;
								}

								flagUseAdditionalColor = true;
							}
						}
						if(true == flagUseAdditionalColor)
						{	/* Use AdditionalColor */
							Status |= FlagBitStatus.USE_ADDITIONALCOLOR;
						}
						else
						{	/* Use PartsColor */
							/* MEMO: Update force when switching from AdditionalColor to PartColor.                             */
							/*       (Otherwise AdditionalColor will continue remaining until next PartsColor data is detected) */
							if(true == flagUpdateValueAttribute)
							{
								Status |= FlagBitStatus.UPDATE_COLORPARTS;
								Status |= FlagBitStatus.UPDATE_PARAMETERBLEND;
							}
							else
							{
								PartsColor.Value.VertexColor = tableVertexColorPartsColorOld;
								PartsColor.Value.RateAlpha = tableRateAlphaPartsColorOld;

								if(0 != (Status & FlagBitStatus.USE_ADDITIONALCOLOR_PREVIOUS))
								{
									Status |= FlagBitStatus.UPDATE_COLORPARTS;
									Status |= FlagBitStatus.UPDATE_PARAMETERBLEND;
								}
							}
							Status &= ~FlagBitStatus.USE_ADDITIONALCOLOR;
						}

						/* Get Vertex-Correction */
						/* MEMO: When decode data, array may change to original data by shallow copy, so replace buffers. */
						/*       (Need to set writable buffer since some decoders write to buffer)                        */
						Vector2[] tableCoordinateVertexCorrectionOld = VertexCorrection.Value.Coordinate;
						VertexCorrection.Value.Coordinate = TableCoordinateVertexCorrection;
						TableCoordinateVertexCorrection = VertexCorrection.Value.Coordinate;
#if UNITY_EDITOR
						if(	(null != dataAnimationParts.VertexCorrection.Function)
							&& (true == dataAnimationParts.VertexCorrection.Function.ValueGet(ref VertexCorrection.Value, ref VertexCorrection.FrameKey, dataAnimationParts.VertexCorrection, ref argumentContainer))
							)
#else
						if(true == dataAnimationParts.VertexCorrection.Function.ValueGet(ref VertexCorrection.Value, ref VertexCorrection.FrameKey, dataAnimationParts.VertexCorrection, ref argumentContainer))
#endif
						{
							Status |=  FlagBitStatus.UPDATE_COORDINATE;
						}
						else
						{
							VertexCorrection.Value.Coordinate = tableCoordinateVertexCorrectionOld;
						}

						/* Get(Update) Deform */
#if UNITY_EDITOR
						if((null != dataAnimationParts.Deform.Function)
							&& (true == Deform.Value.IsValid)
							)
#else
						if(true == Deform.Value.IsValid)
#endif
						{
							if(true == dataAnimationParts.Deform.Function.ValueGet(ref Deform.Value, ref Deform.FrameKey, dataAnimationParts.Deform, ref argumentContainer))
							{
								Status |= FlagBitStatus.UPDATE_DEFORM;
							}
						}

						/* Clear One-time status */
						Status &= ~FlagBitStatus.UPDATE_COORDINATE_NOWFRAME;

						return(true);
					}

					internal void DrawPlain(	Script_SpriteStudio6_Root instanceRoot,
												int idParts,
												ref Library_SpriteStudio6.Control.Animation.Parts controlParts,
												GameObject instanceGameObject,
												Transform instanceTransform,
												Library_SpriteStudio6.KindMasking masking,
												bool flagPreDraw,
												ref Matrix4x4 matrixCorrection,
												bool flagPlanarization,
												ref Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus statusControlParts,
												ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer
											)
					{
						if(0 != (Status & FlagBitStatus.NO_DRAW))
						{
							return;
						}

						bool flagTriangle4 = ((int)Library_SpriteStudio6.KindVertex.TERMINATOR4 == CountVertex);	/* ? true : false */
						Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus statusPartsAnimation = dataAnimationParts.StatusParts;

						Vector2 sizeSprite = SizeSprite;
						Vector2 pivotSprite = PivotSprite;
						Vector2 sizeMapping = SizeCell;
						Vector2 positionMapping = PositionCell;

						/* Check Masking */
						if(Library_SpriteStudio6.KindMasking.FOLLOW_DATA == masking)
						{
							masking = (0 != (statusPartsAnimation & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_MASKING)) ? Library_SpriteStudio6.KindMasking.THROUGH : Library_SpriteStudio6.KindMasking.MASK;
						}
						if(Masking != masking)
						{
							Masking = masking;
							Status |= FlagBitStatus.UPDATE_MASKING;
						}

						/* Calculate Texture-UV */
						/* MEMO: Calculate only corners at this point.(Center is sets average value later) */
						Vector2 uv2C = Vector2.zero;
						if(0 != (statusPartsAnimation & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_TRANSFORMATION_TEXTURE))
						{	/* No Transform (Ordinary rectangle) */
							Vector2 uLR = new Vector2(positionMapping.x, positionMapping.x + sizeMapping.x);
							float tempFloat = SizeTexture.y - positionMapping.y;	/* mapping-Y Inverse */
							Vector2 vUD = new Vector2(tempFloat, tempFloat - sizeMapping.y);
							uLR /= SizeTexture.x;
							vUD /= SizeTexture.y;

							if(0.0f > RateScaleTexture.x)
							{	/* Flip X */
								tempFloat = uLR.x;
								uLR.x = uLR.y;
								uLR.y = tempFloat;
							}
							if(0.0f > RateScaleTexture.y)
							{	/* Flip Y */
								tempFloat = vUD.x;
								vUD.x = vUD.y;
								vUD.y = tempFloat;
							}

							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LU].x = uLR.x;
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LU].y = vUD.x;
							uv2C += UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LU];

							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RU].x = uLR.y;
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RU].y = vUD.x;
							uv2C += UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RU];

							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RD].x = uLR.y;
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RD].y = vUD.y;
							uv2C += UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RD];

							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LD].x = uLR.x;
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LD].y = vUD.y;
							uv2C += UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LD];
						}
						else
						{	/* Transform Texure */
							if(0 != (Status & FlagBitStatus.UPDATE_TRANSFORM_TEXTURE))
							{
								/* Create Matrix & Transform Texture-UV */
								Vector2 centerMapping = (sizeMapping * 0.5f) + positionMapping;
								Vector3 translation = new Vector3(	(centerMapping.x / SizeTexture.x) + PositionTexture.Value.x,
																	((SizeTexture.y - centerMapping.y) / SizeTexture.y) - PositionTexture.Value.y,
																	0.0f
																);
								Vector3 scaling = new Vector3(	(sizeMapping.x / SizeTexture.x) * ScalingTexture.Value.x * RateScaleTexture.x,
																(sizeMapping.y / SizeTexture.y) * ScalingTexture.Value.y * RateScaleTexture.y,
																1.0f
															);

								Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, -RotationTexture.Value);
								MatrixTexture = Matrix4x4.TRS(translation, rotation, scaling);
							}
							for(int i=0; i<(int)Library_SpriteStudio6.KindVertex.TERMINATOR2; i++)
							{
								UVTextureDraw[i] = MatrixTexture.MultiplyPoint3x4(Library_SpriteStudio6.Draw.Model.TableUVMapping[i]);
								uv2C += UVTextureDraw[i];
							}
						}
						/* Set Mapping (Center) */
						uv2C *= 0.25f;
						UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.C] = uv2C;

						/* Set Parts-Color */
						float operationBlend;
						Color[] tableColor;
						float[] tableAlpha;
						if(0 != (Status & FlagBitStatus.USE_ADDITIONALCOLOR))
						{
							Library_SpriteStudio6.Control.AdditionalColor additionalColor = instanceRoot.AdditionalColor;
							operationBlend = (float)((int)additionalColor.OperationBlend) + 0.01f;	/* "+0.01f" for Rounding-off-Error */
							tableColor = additionalColor.ColorVertex;
							tableAlpha = Library_SpriteStudio6.Data.Animation.Attribute.TableRateAlphaPartsColorDefault;
						}
						else
						{
							operationBlend = (float)((int)PartsColor.Value.Operation) + 0.01f;	/* "+0.01f" for Rounding-off-Error */
							tableColor = PartsColor.Value.VertexColor;
							tableAlpha = PartsColor.Value.RateAlpha;
						}

						float rateOpacity = instanceRoot.RateOpacity * controlParts.RateOpacity.Value;
						if(0 != (Status & (FlagBitStatus.UPDATE_COLORPARTS | FlagBitStatus.UPDATE_PARAMETERBLEND)))
						{
							Color sumColor = Library_SpriteStudio6.Data.Animation.Attribute.ColorClear;
							float sumAlpha = 0.0f;
							for(int i=0; i<(int)Library_SpriteStudio6.KindVertex.TERMINATOR2; i++)
							{
								ParameterBlendDraw[i].x = operationBlend;
								ParameterBlendDraw[i].y = rateOpacity * tableAlpha[i];

								ColorPartsDraw[i] = tableColor[i];
								sumColor += tableColor[i];
								sumAlpha += tableAlpha[i];
							}
							tableColor = null;

							if(flagTriangle4)	/* (true == flagTriangle4) */
							{
								ParameterBlendDraw[(int)Library_SpriteStudio6.KindVertex.C].x = operationBlend;
								ParameterBlendDraw[(int)Library_SpriteStudio6.KindVertex.C].y = rateOpacity * (sumAlpha * 0.25f);

								ColorPartsDraw[(int)Library_SpriteStudio6.KindVertex.C] = sumColor * 0.25f;
							}
						}

						/* Calculate Mesh coordinates */
						if(0 != (Status & FlagBitStatus.UPDATE_COORDINATE))
						{
							/* Set Coordinates */
							float scaleMeshX = RateScaleMesh.x;
							float scaleMeshY = -RateScaleMesh.y;	/* * -1.0f ... Y-Axis Inverse */
							float left = (-pivotSprite.x) * scaleMeshX;
							float right = (sizeSprite.x - pivotSprite.x) * scaleMeshX;
							float top = (-pivotSprite.y) * scaleMeshY;
							float bottom = (sizeSprite.y - pivotSprite.y) * scaleMeshY;

							int indexVertex;
							int[] tableIndex = Library_SpriteStudio6.Control.Animation.TableIndexVertexCorrectionOrder[IndexVertexCollectionTable];
							Vector2[] tableCoordinate = VertexCorrection.Value.Coordinate;

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.LU];
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LU] = new Vector3((left + tableCoordinate[indexVertex].x), (top + tableCoordinate[indexVertex].y), 0.0f);

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.RU];
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RU] = new Vector3((right + tableCoordinate[indexVertex].x), (top + tableCoordinate[indexVertex].y), 0.0f);

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.RD];
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RD] = new Vector3((right + tableCoordinate[indexVertex].x), (bottom + tableCoordinate[indexVertex].y), 0.0f);

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.LD];
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LD] = new Vector3((left + tableCoordinate[indexVertex].x), (bottom + tableCoordinate[indexVertex].y), 0.0f);

							/* MEMO: Center is the intersection of lines through midpoints of opposite-sides. (not of diagonals) */
							Vector3 coordinateLURU = (CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LU] + CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RU]) * 0.5f;
							Vector3 coordinateLULD = (CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LU] + CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LD]) * 0.5f;
							Vector3 coordinateLDRD = (CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LD] + CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RD]) * 0.5f;
							Vector3 coordinateRURD = (CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RU] + CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RD]) * 0.5f;
							Library_SpriteStudio6.Utility.Math.CoordinateGetDiagonalIntersection(	out CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.C],
																									ref coordinateLURU,
																									ref coordinateRURD,
																									ref coordinateLULD,
																									ref coordinateLDRD
																								);
						}

						/* Transform Coordinates */
						/* MEMO: Prevent double effect MeshRenderer's world-matrix and InstanceTransform's world-matrix. */
						float scaleLocalX = controlParts.ScaleLocal.Value.x * instanceRoot.RateScaleLocal.x;
						float scaleLocalY = controlParts.ScaleLocal.Value.y * instanceRoot.RateScaleLocal.y;
						Matrix4x4 matrixTransform =	matrixCorrection * instanceTransform.localToWorldMatrix;
						if(true == flagPlanarization)
						{
							/* MEMO: Z-coordinate is always set to 0 after transformation. */
							matrixTransform[2, 0] = 
							matrixTransform[2, 1] = 
							matrixTransform[2, 2] = 
							matrixTransform[2, 3] = 0.0f;
						}
						Vector3 coordinate;

						/* MEMO: Expand "for" loop. */
						coordinate = CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LU];
						coordinate.x *= scaleLocalX;
						coordinate.y *= scaleLocalY;
						CoordinateTransformDraw[(int)Library_SpriteStudio6.KindVertex.LU] = matrixTransform.MultiplyPoint3x4(coordinate);

						coordinate = CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RU];
						coordinate.x *= scaleLocalX;
						coordinate.y *= scaleLocalY;
						CoordinateTransformDraw[(int)Library_SpriteStudio6.KindVertex.RU] = matrixTransform.MultiplyPoint3x4(coordinate);

						coordinate = CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RD];
						coordinate.x *= scaleLocalX;
						coordinate.y *= scaleLocalY;
						CoordinateTransformDraw[(int)Library_SpriteStudio6.KindVertex.RD] = matrixTransform.MultiplyPoint3x4(coordinate);

						coordinate = CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LD];
						coordinate.x *= scaleLocalX;
						coordinate.y *= scaleLocalY;
						CoordinateTransformDraw[(int)Library_SpriteStudio6.KindVertex.LD] = matrixTransform.MultiplyPoint3x4(coordinate);

						coordinate = CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.C];
						coordinate.x *= scaleLocalX;
						coordinate.y *= scaleLocalY;
						CoordinateTransformDraw[(int)Library_SpriteStudio6.KindVertex.C] = matrixTransform.MultiplyPoint3x4(coordinate);

						/* Update Material */
						if(0 != (Status & (FlagBitStatus.UPDATE_UVTEXTURE | FlagBitStatus.UPDATE_MASKING)))
						{
							if(true == flagPreDraw)
							{
								int indexCellMap = DataCellApply.IndexCellMap;
								MaterialDraw = instanceRoot.MaterialGet(indexCellMap, Library_SpriteStudio6.KindOperationBlend.MASK_PRE, masking);
								MaterialDrawMask = instanceRoot.MaterialGet(indexCellMap, Library_SpriteStudio6.KindOperationBlend.MASK, masking);
							}
							else
							{
								MaterialDraw = instanceRoot.MaterialGet(	DataCellApply.IndexCellMap,
																			instanceRoot.DataAnimation.TableParts[idParts].OperationBlendTarget,
																			masking
																		);
							}
						}

						/* Set to Draw-Cluster */
						instanceRoot.ClusterDraw.VertexAdd(	ChainDraw,
															CountVertex,
															CoordinateTransformDraw,
															ColorPartsDraw,
															UVTextureDraw,
															ParameterBlendDraw,
															MaterialDraw
														);

						/* MEMO: "UPDATE" flags need to be cleared after add to Draw-Cluster.       */
						/*       (Because "Draw" may not be executed even if "Update" is executed.) */
						Status &= ~(	FlagBitStatus.UPDATE_COORDINATE
										| FlagBitStatus.UPDATE_UVTEXTURE
										| FlagBitStatus.UPDATE_PARAMETERBLEND
										| FlagBitStatus.UPDATE_COLORPARTS
										| FlagBitStatus.UPDATE_MASKING
										| FlagBitStatus.UPDATE_DEFORM
										| FlagBitStatus.UPDATE_TRANSFORM_TEXTURE
//										| FlagBitStatus.USE_ADDITIONALCOLOR		/* update in "UpdatePlain", so not erase here */
								);
						statusControlParts &= ~(	Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.UPDATE_SCALELOCAL
													| Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.UPDATE_RATEOPACITY
											);
					}

					internal void DrawMeshPlain(	Script_SpriteStudio6_Root instanceRoot,
													int idParts,
													ref Library_SpriteStudio6.Control.Animation.Parts controlParts,
													GameObject instanceGameObject,
													Transform instanceTransform,
													Library_SpriteStudio6.KindMasking masking,
													bool flagPreDraw,
													ref Matrix4x4 matrixCorrection,
													bool flagPlanarization,
													ref Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus statusControlParts,
													ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
													ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer
												)
					{
						if(0 != (Status & FlagBitStatus.NO_DRAW))
						{
							return;
						}

						Library_SpriteStudio6.Data.Parts.Animation instanceParts = instanceRoot.DataAnimation.TableParts[idParts];
						Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus statusPartsAnimation = dataAnimationParts.StatusParts;
						Library_SpriteStudio6.Data.Parts.Animation.BindMesh.Vertex[] tableBindMesh = instanceParts.Mesh.TableVertex;
#if UNITY_EDITOR
						if(null == tableBindMesh)
						{
							/* MEMO: May reach before deserialization direct-after import. */
							return;
						}
#endif
						/* MEMO: These three values(countTableBindMesh/countTableUV/countVertexDeform) will have the same value except when 0. */
						int countTableBindMesh = tableBindMesh.Length;
						int countTableUV;
						int countVertexDeform = instanceParts.Mesh.CountVertexDeform;

						/* Check Masking */
						if(Library_SpriteStudio6.KindMasking.FOLLOW_DATA == masking)
						{
							masking = (0 != (statusPartsAnimation & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_MASKING)) ? Library_SpriteStudio6.KindMasking.THROUGH : Library_SpriteStudio6.KindMasking.MASK;
						}
						if(Masking != masking)
						{
							Masking = masking;
							Status |= FlagBitStatus.UPDATE_MASKING;
						}

						/* Check Cell-Change */
						int indexCellMap = DataCellApply.IndexCellMap;
						int indexCell = DataCellApply.IndexCell;
						Library_SpriteStudio6.Data.CellMap cellMap = instanceRoot.DataGetCellMap(indexCellMap);
						if(null == cellMap)
						{	/* CellMap Invalid */
							/* MEMO: Can not be drawn as a mesh. */
							goto DrawMeshPlain_ErrorEnd;
						}
						else
						{	/* CellMap Valid */
							if((0 > indexCell) || (cellMap.CountGetCell() <= indexCell))
							{	/* Cell Invalid */
								/* MEMO: Can not be drawn as a mesh. */
								goto DrawMeshPlain_ErrorEnd;
							}
						}
						if(false == cellMap.TableCell[indexCell].IsMesh)
						{	/* Has no mesh */
							/* MEMO: Can not be drawn as a mesh. */
							goto DrawMeshPlain_ErrorEnd;
						}

						/* MEMO: Mapping-control attributes can not be set for "Mesh" parts, so UV recalculation occurs only when "Cell" is changed or initialized. */
						if(0 != (Status & FlagBitStatus.UPDATE_UVTEXTURE))
						{
							/* Calculate UV */
							if(null != UVTextureDraw)
							{
								/* MEMO: Calculate UV when change Cell. (Mesh-Bind is not changed.) */
								/* MEMO: Each UV-coordinate is as ratio when original Mesh-Cell's size is 1. */
								Vector2[] tableUVRate = instanceParts.Mesh.TableRateUV;
								float sizeTextureX = SizeTexture.x;
								float sizeTextureY = SizeTexture.y;
								float sizeInverseTextureX = 1.0f / sizeTextureX;
								float sizeInverseTextureY = 1.0f / sizeTextureY;
								float positionCellX = PositionCell.x;
								float positionCellY = PositionCell.y;
								float sizeCellX = SizeCell.x;
								float sizeCellY = SizeCell.y;
								float rateUVX;
								float rateUVY;
								countTableUV = tableUVRate.Length;
								for(int i=0; i<countTableUV; i++)
								{
									/* MEMO: Round to integer since original coordinate is pixel-alignment. */
									rateUVX = tableUVRate[i].x;
									rateUVY = tableUVRate[i].y;

									rateUVX *= sizeCellX;
									rateUVX = Mathf.Floor(rateUVX);
									rateUVX += positionCellX;
									UVTextureDraw[i].x = rateUVX * sizeInverseTextureX;

									rateUVY *= sizeCellY;
									rateUVY = Mathf.Floor(rateUVY);
									rateUVY += positionCellY;
									rateUVY = sizeTextureY - rateUVY;
									UVTextureDraw[i].y = rateUVY * sizeInverseTextureY;
								}
							}

							/* Calculate Coordinate */
							if(null != CoordinateDraw)
							{	/* not Skeletal-Animation */
								Vector2 pivot = cellMap.TableCell[indexCell].Pivot;
								Vector2[] tableVertexCell = cellMap.TableCell[indexCell].Mesh.TableCoordinate;
								for(int i=0; i<CountVertex; i++)
								{
									CoordinateDraw[i] = tableVertexCell[i] - pivot;
									CoordinateDraw[i].y *= -1.0f;
								}
								if(null != DeformDraw)
								{	/* Use Deform */
									for(int i=0; i<CountVertex; i++)
									{
										DeformDraw[i] = CoordinateDraw[i];
									}
								}
							}

							/* MEMO: Need to reset parts color. */
							Status |= (FlagBitStatus.UPDATE_PARAMETERBLEND | FlagBitStatus.UPDATE_COLORPARTS);
						}

						/* Set Parts-Color */
						float operationBlend;
						Color32 colorParts;
						float rateOpacity;
						if(0 != (Status & FlagBitStatus.USE_ADDITIONALCOLOR))
						{
							Library_SpriteStudio6.Control.AdditionalColor additionalColor = instanceRoot.AdditionalColor;
							operationBlend = (float)((int)additionalColor.OperationBlend) + 0.01f;	/* "+0.01f" for Rounding-off-Error */
							colorParts = additionalColor.ColorVertex[(int)Library_SpriteStudio6.KindVertex.LU];
							rateOpacity = Library_SpriteStudio6.Data.Animation.Attribute.TableRateAlphaPartsColorDefault[(int)Library_SpriteStudio6.KindVertex.LU];
						}
						else
						{
							operationBlend = (float)((int)PartsColor.Value.Operation) + 0.01f;	/* "+0.01f" for Rounding-off-Error */
							colorParts = PartsColor.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LU];
							rateOpacity = PartsColor.Value.RateAlpha[(int)Library_SpriteStudio6.KindVertex.LU];
						}

						rateOpacity *= instanceRoot.RateOpacity * controlParts.RateOpacity.Value;
						if(0 != (Status & (FlagBitStatus.UPDATE_COLORPARTS | FlagBitStatus.UPDATE_PARAMETERBLEND)))
						{
							/* MEMO: "Mesh" supports only "Overall". */
							countTableUV = UVTextureDraw.Length;
							for(int i=0; i<countTableUV; i++)
							{
								ParameterBlendDraw[i].x = operationBlend;
								ParameterBlendDraw[i].y = rateOpacity;
								ColorPartsDraw[i] = colorParts;
							}
						}

						/* Transform Coordinates */
						/* MEMO: Part's "Local Scale" can not be used for "Mesh". */
						Matrix4x4 matrixTransform = instanceTransform.localToWorldMatrix;
						matrixTransform[0, 0] *= instanceRoot.RateScaleLocal.x;
						matrixTransform[1, 1] *= instanceRoot.RateScaleLocal.y;
						matrixTransform = matrixCorrection * matrixTransform;
						if(true == flagPlanarization)
						{
							/* MEMO: Z-coordinate is always set to 0 after transformation. */
							matrixTransform[2, 0] = 
							matrixTransform[2, 1] = 
							matrixTransform[2, 2] = 
							matrixTransform[2, 3] = 0.0f;
						}

						if(0 >= countTableBindMesh)
						{	/* not Skeletal-Animation */
							if(true == Deform.Value.IsValid)
							{	/* Use Deform */
								/* Transform including "Deform" */
								/* MEMO: In this case, "DeformDraw" is coordinate after posting "Deform". */
								if(0 != (Status & FlagBitStatus.UPDATE_DEFORM))
								{
									Vector2[] tableVertexDeform = Deform.Value.TableCoordinate;
									for(int i=0; i<CountVertex; i++)
									{
										DeformDraw[i] = CoordinateDraw[i] + (Vector3)tableVertexDeform[i];
									}
								}

								for(int i=0; i<CountVertex; i++)
								{
									CoordinateTransformDraw[i] = matrixTransform.MultiplyPoint3x4(DeformDraw[i]);	/* .z = 0 */
								}
							}
							else
							{	/* not use Deform */
								/* Transform ignoring "Deform" */
								/* MEMO: In this case, "DeformDraw" is not used. (Transform "CoordinateDraw" directly.) */
								for(int i=0; i<CountVertex; i++)
								{
									CoordinateTransformDraw[i] = matrixTransform.MultiplyPoint3x4(CoordinateDraw[i]);	/* .z = 0 */
								}
							}
						}
						else
						{	/* Skeletal-Animation */
							/* Calculate Coordinates */
							int countBone;
							Vector3 coordinate;
							Vector3 coordinateSum;
							int idPartsBone;
							float weight;
							for(int i=0; i<countTableBindMesh; i++)
							{
								coordinateSum = Vector3.zero;
								countBone = tableBindMesh[i].TableBone.Length;
								if(0 < countBone)
								{
									for(int j=0; j<countBone; j++)
									{
#if BONEINDEX_CONVERT_PARTSID
										idPartsBone = tableBindMesh[i].TableBone[j].Index;
#else
										idPartsBone = tableBindMesh[i].TableBone[j].Index;
										idPartsBone = instanceRoot.DataAnimation.CatalogParts.TableIDPartsBone[idPartsBone];
#endif
										if(0 <= idPartsBone)
										{
											coordinate = instanceRoot.TableControlParts[idPartsBone].MatrixBoneWorld.MultiplyPoint3x4(tableBindMesh[i].TableBone[j].CoordinateOffset);

											weight = tableBindMesh[i].TableBone[j].Weight;
											coordinate *= weight;
//											coordinate.z = 0.0f;

											coordinateSum += coordinate;
										}
									}
								}
								CoordinateTransformDraw[i] = coordinateSum;
							}

							/* Deform Coordinate */
							/* MEMO: In the case of Skeletal-Animated "Mesh", "DeformDraw" is a calculated buffer for Deform. */
							if(0 < countVertexDeform)
							{
								if(0 != (Status & FlagBitStatus.UPDATE_DEFORM))
								{
									for(int i=0; i<countVertexDeform; i++)
									{
										DeformDraw[i] = Vector3.zero;
									}

									/* MEMO: Calculate only vertices with changes. */
									int[] tableVertexChange = dataAnimationParts.Deform.TableIndexVertex;
									int countVertexChange = tableVertexChange.Length;
									int indexVertex;
									for(int i=0; i<countVertexChange; i++)
									{
										/* MEMO: Calculate only relative-value, so using "MultiplyVector". (Ignore translation) */
										indexVertex = tableVertexChange[i];
										DeformDraw[indexVertex] = matrixTransform.MultiplyVector(Deform.Value.TableCoordinate[indexVertex]);	/* .z = 0 */
									}
								}

								/* Add Skeletal-Animation and Deform */
								for(int i=0; i<countTableBindMesh; i++)
								{
									CoordinateTransformDraw[i] += DeformDraw[i];
								}
							}
						}

						/* Update Material */
						if(0 != (Status & (FlagBitStatus.UPDATE_UVTEXTURE | FlagBitStatus.UPDATE_MASKING)))
						{
							if(true == flagPreDraw)
							{
								/* MEMO: When "flagPreDraw" is true, only when "Mask"'s first time drawing. */
								/*       Set fixed values.                                                  */
								/* MEMO: Update material for "Draw" as well. */
								indexCellMap = DataCellApply.IndexCellMap;
								MaterialDraw = instanceRoot.MaterialGet(indexCellMap, Library_SpriteStudio6.KindOperationBlend.MASK_PRE, masking);
								MaterialDrawMask = instanceRoot.MaterialGet(indexCellMap, Library_SpriteStudio6.KindOperationBlend.MASK, masking);
							}
							else
							{
								MaterialDraw = instanceRoot.MaterialGet(	DataCellApply.IndexCellMap,
																			instanceParts.OperationBlendTarget,
																			masking
																		);
							}
						}

						/* Set to Draw-Cluster */
						instanceRoot.ClusterDraw.VertexAddMesh(	ChainDraw,
																CoordinateTransformDraw,
																ColorPartsDraw,
																UVTextureDraw,
																ParameterBlendDraw,
																IndexVertexDraw,
																MaterialDraw
															);

						/* MEMO: "UPDATE" flags need to be cleared after add to Draw-Cluster.       */
						/*       (Because "Draw" may not be executed even if "Update" is executed.) */
						Status &= ~(	FlagBitStatus.UPDATE_COORDINATE
										| FlagBitStatus.UPDATE_UVTEXTURE
										| FlagBitStatus.UPDATE_PARAMETERBLEND
										| FlagBitStatus.UPDATE_COLORPARTS
										| FlagBitStatus.UPDATE_MASKING
										| FlagBitStatus.UPDATE_DEFORM
										| FlagBitStatus.UPDATE_TRANSFORM_TEXTURE
//										| FlagBitStatus.UPDATE_FLIP_COEFFICIENT
//										| FlagBitStatus.USE_ADDITIONALCOLOR		/* update in "UpdatePlain", so not erase here */
								);
						statusControlParts &= ~(	Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.UPDATE_SCALELOCAL
													| Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.UPDATE_RATEOPACITY
											);

						return;

					DrawMeshPlain_ErrorEnd:;
						return;
					}
					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					[System.Flags]
					internal enum FlagBitStatus
					{
						/* Common (Accumulated until drawed) */
						NO_DRAW = 0x40000000,	/* Not "Hide" ... for when no cell designation */

						UPDATE_COORDINATE = 0x08000000,
						UPDATE_UVTEXTURE = 0x04000000,
						UPDATE_PARAMETERBLEND = 0x02000000,
						UPDATE_COLORPARTS = 0x01000000,

						UPDATE_MASKING = 0x00800000,
						UPDATE_DEFORM = 0x00400000,	/* Mesh only */

						/* Common (One-time) */
						UPDATE_COORDINATE_NOWFRAME = 0x00080000,
						UPDATE_COLLIDERRECTANGLE_NOWFRAME = 0x00040000,

						/* for Plain (Normal/Mesh) */
						UPDATE_TRANSFORM_TEXTURE = 0x00008000,
//						UPDATE_FLIP_COEFFICIENT = 0x00004000,	/* FLIP_X,FLIP_Y,FLIP_TEXTURE_X or FLIP_TEXTURE_Y are updated. */

						USE_ADDITIONALCOLOR_PREVIOUS = 0x00000800,
						USE_ADDITIONALCOLOR = 0x00000400,
						/* MEMO: Bit-order should be the same as "Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit.FLIP_*". */
						FLIP_COEFFICIENT_X = 0x00000080,	/* Normal(Sprite) Only */
						FLIP_COEFFICIENT_Y = 0x00000040,	/* Normal(Sprite) Only */
						FLIP_COEFFICIENT_TEXTURE_X = 0x00000020,	/* Normal(Sprite) Only */
						FLIP_COEFFICIENT_TEXTURE_Y = 0x00000010,	/* Normal(Sprite) Only */

						CLEAR = 0x00000000,
					}
					internal enum FlagShiftStatus
					{
						FLIP_COEFFICIENT_TO_ATTRIBUTE = 20,	/* FlagBitStatus.Shift -> Library_SpriteStudio6.Data.Animation.Attribute.Status.FlagBit */
					}
					#endregion Enums & Constants
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
