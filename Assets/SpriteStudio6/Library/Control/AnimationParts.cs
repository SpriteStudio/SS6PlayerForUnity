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
				internal int FramePreviousUpdateUnderControl;

				public Script_SpriteStudio6_Collider InstanceScriptCollider;
				internal int FramePreviousUpdateRadiusCollision;

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
					FramePreviousUpdateUnderControl = -1;

//					InstanceScriptCollider =
					FramePreviousUpdateRadiusCollision = -1;

					StatusAnimationParts = Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.CLEAR;
					FrameKeyStatusAnimationFrame = -1;
					StatusAnimationFrame.CleanUp();

					TRSMaster.CleanUp();
					TRSSlave.CleanUp();

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
							if(false == ParameterSprite.BootUp((int)Library_SpriteStudio6.KindVertex.TERMINATOR2, countPartsSprite))
							{
								goto BootUp_ErrorEnd;
							}
							break;

						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
							/* MEMO: Erase, because can not have undercontrol object. */
							PrefabUnderControl = null;
							InstanceGameObjectUnderControl = null;

							if(false == ParameterSprite.BootUp((int)Library_SpriteStudio6.KindVertex.TERMINATOR4, countPartsSprite))
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

					AnimationRefresh();
					Status |= (FlagBitStatus.CHANGE_TRANSFORM_POSITION | FlagBitStatus.CHANGE_TRANSFORM_ROTATION | FlagBitStatus.CHANGE_TRANSFORM_SCALING);
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

				internal void Update(Script_SpriteStudio6_Root instanceRoot, int idParts)
				{
					int indexTrack = IndexControlTrack;
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
					UpdateUserData(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);

					/* Update for each parts' feature */
					switch(instanceRoot.DataAnimation.TableParts[idParts].Feature)
					{
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
							break;
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
							UpdateNormal(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
							break;
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
							/* Update Instance */
							/* MEMO: No processing */
//							UpdateInstance(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
							break;
						case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
							/* Update Effect */
							/* MEMO: No processing */
//							UpdateEffect(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack]);
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
					StatusAnimationParts = statusParts;	/* cache */

					/* MEMO: StatusParts's NOT_USED should not be judged here, because will fail Refresh at start of animation. */
//					if(0 != (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED))
//					{
//						return;
//					}

					/* Set Position, Rotation and Scaling */
					Transform transform = InstanceGameObject.transform;
					Data.Animation.PackAttribute.ArgumentContainer argument = new Data.Animation.PackAttribute.ArgumentContainer();
					argument.Frame = 0;

					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION))
					{
						if(true == dataAnimationParts.Position.Function.ValueGet(ref TRSMaster.Position.Value, ref TRSMaster.Position.FrameKey, dataAnimationParts.Position, ref controlTrack.ArgumentContainer))
						{	/* New Value */
							transform.localPosition = TRSMaster.Position.Value;

							Status |= FlagBitStatus.CHANGE_TRANSFORM_POSITION;
						}
					}
					else
					{
						if((FlagBitStatus.CHANGE_TRANSFORM_POSITION | FlagBitStatus.REFRESH_TRANSFORM_POSITION) == (Status & (FlagBitStatus.CHANGE_TRANSFORM_POSITION | FlagBitStatus.REFRESH_TRANSFORM_POSITION)))
						{	/* Refresh */
							transform.localPosition = Vector3.zero;
						}
					}

					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION))
					{
						if(true == dataAnimationParts.Rotation.Function.ValueGet(ref TRSMaster.Rotation.Value, ref TRSMaster.Rotation.FrameKey, dataAnimationParts.Rotation, ref controlTrack.ArgumentContainer))
						{	/* New Value */
							transform.localEulerAngles = TRSMaster.Rotation.Value;

							Status |= FlagBitStatus.CHANGE_TRANSFORM_ROTATION;
						}
					}
					else
					{
						if((FlagBitStatus.CHANGE_TRANSFORM_ROTATION | FlagBitStatus.REFRESH_TRANSFORM_ROTATION) == (Status & (FlagBitStatus.CHANGE_TRANSFORM_ROTATION | FlagBitStatus.REFRESH_TRANSFORM_ROTATION)))
						{	/* Refresh */
							transform.localEulerAngles = Vector3.zero;
						}
					}

					if(0 == (statusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING))
					{
						if(true == dataAnimationParts.Scaling.Function.ValueGet(ref TRSMaster.Scaling.Value, ref TRSMaster.Scaling.FrameKey, dataAnimationParts.Scaling, ref controlTrack.ArgumentContainer))
						{	/* New Value */
							Vector3 scaling = TRSMaster.Scaling.Value;
							scaling.z = 1.0f;
							transform.localScale = scaling;

							Status |= FlagBitStatus.CHANGE_TRANSFORM_SCALING;
						}
					}
					else
					{
						if((FlagBitStatus.REFRESH_TRANSFORM_SCALING | FlagBitStatus.REFRESH_TRANSFORM_SCALING) == (Status & (FlagBitStatus.REFRESH_TRANSFORM_SCALING | FlagBitStatus.REFRESH_TRANSFORM_SCALING)))
						{	/* Refresh */
							transform.localScale = Vector3.one;
						}
					}

					Status &= ~(FlagBitStatus.REFRESH_TRANSFORM_POSITION | FlagBitStatus.REFRESH_TRANSFORM_ROTATION | FlagBitStatus.REFRESH_TRANSFORM_SCALING);

					/* Get Status & Hide */
					dataAnimationParts.Status.Function.ValueGet(ref StatusAnimationFrame, ref FrameKeyStatusAnimationFrame, dataAnimationParts.Status, ref controlTrack.ArgumentContainer);
					Status = (true == StatusAnimationFrame.IsHide) ? (Status | FlagBitStatus.HIDE) : (Status & ~FlagBitStatus.HIDE);
				}
				private void UpdateUserData(	Script_SpriteStudio6_Root instanceRoot,
												int idParts,
												ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
												ref Library_SpriteStudio6.Control.Animation.Track controlTrack
											)
				{
					if(0 != (StatusAnimationParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_USERDATA))
					{	/* Has no UserData-s */
						return;
					}
					if((true == controlTrack.StatusIsIgnoreUserData) || (true == controlTrack.StatusIsDecodeAttribute) || (null == instanceRoot.FunctionUserData))
					{	/* No Need to decode UserData-s */
						return;
					}

					int countLoop = controlTrack.CountLoopNow;
					if(true == controlTrack.StatusIsIgnoreSkipLoop)
					{
						countLoop = 0;
					}
					bool flagLoop = (0 < countLoop) ? true : false;
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
//							if(true == flagLoop)
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
//							if(true == flagLoop)
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
						dataAnimationParts.UserData.Function.ValueGetIndex(ref userData, ref frameKey, i, dataAnimationParts.UserData, ref controlTrack.ArgumentContainer);
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
						dataAnimationParts.UserData.Function.ValueGetIndex(ref userData, ref frameKey, i, dataAnimationParts.UserData, ref controlTrack.ArgumentContainer);
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
					/* MEMO:  Flips of Sprite and Texture have effect only on NORMAL2/NORMAL4.  */
					/*        (Not work, although can set to "Instance" or "Effect" on SS5/SS6) */
					ParameterSprite.StatusSetFlip(ref StatusAnimationFrame);
					switch(dataAnimationParts.Format)
					{
						case Library_SpriteStudio6.Data.Animation.Parts.KindFormat.PLAIN:
							ParameterSprite.UpdateSpriteSizePlain(instanceRoot, idParts, InstanceGameObject, InstanceTransform, ref Status, ref dataAnimationParts, ref controlTrack.ArgumentContainer);
							break;
						case Library_SpriteStudio6.Data.Animation.Parts.KindFormat.FIX:
							ParameterSprite.UpdateSpriteSizeFix(instanceRoot, idParts, InstanceGameObject, InstanceTransform, ref Status, ref dataAnimationParts, ref controlTrack.ArgumentContainer);
							break;
					}
				}
				private void UpdateInstance(	Script_SpriteStudio6_Root instanceRoot,
												int idParts,
												ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
												ref Library_SpriteStudio6.Control.Animation.Track controlTrack
											)
				{
					/* MEMO: This function is present only for explicitness, not called. (dummy)                 */
					/*       Originally, the processing to be written in this function is  in "DrawInstance".    */
					/*                                                                                           */
					/*       "Instance"-parts are updated and rendered in same function "DrawInstance".          */
				}
				private void UpdateEffect(	Script_SpriteStudio6_Root instanceRoot,
											int idParts,
											ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
											ref Library_SpriteStudio6.Control.Animation.Track controlTrack
										)
				{
					/* MEMO: This function is present only for explicitness, not called. (dummy)                 */
					/*       Originally, the processing to be written in this function is  in "DrawEffect".      */
					/*                                                                                           */
					/*       "Effect"-parts are updated and rendered in same function "DrawEffect".              */
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

					if(0 != (ParameterSprite.Status & BufferParameterSprite.FlagBitStatus.UPDATE_COORDINATE))
					{
						InstanceScriptCollider.ColliderSetRectangle(ref ParameterSprite.SizeSprite, ref ParameterSprite.PivotSprite);
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
					if(true == dataAnimationParts.RadiusCollision.Function.ValueGet(ref radius, ref frameKey, dataAnimationParts.RadiusCollision, ref controlTrack.ArgumentContainer))
					{   /* New Valid Data */
						FramePreviousUpdateRadiusCollision = frameKey;
						InstanceScriptCollider.ColliderSetRadius(radius);
					}
				}

				internal void Draw(Script_SpriteStudio6_Root instanceRoot, int idParts, bool flagHideDefault, ref Matrix4x4 matrixCorrection)
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
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
								DrawNormal(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack], flagHideDefault, ref matrixCorrection);
								break;
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
								/* Update Instance */
								DrawInstance(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack], flagHideDefault, ref matrixCorrection);
								break;
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
								/* Update Effect */
								DrawEffect(instanceRoot, idParts, ref instanceRoot.DataAnimation.TableAnimation[indexAnimation].TableParts[idParts], ref instanceRoot.TableControlTrack[indexTrack], flagHideDefault, ref matrixCorrection);
								break;
						}
					}
				}
				private void DrawNormal(	Script_SpriteStudio6_Root instanceRoot,
												int idParts,
												ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
												ref Library_SpriteStudio6.Control.Animation.Track controlTrack,
												bool flagHideDefault,
												ref Matrix4x4 matrixCorrection
											)
				{
					controlTrack.ArgumentContainer.IDParts = idParts;

					/* Draw Sprite */
					bool flagHide = flagHideDefault;
					flagHide |= (0 != (Status & (FlagBitStatus.HIDE_FORCE | FlagBitStatus.HIDE))) ? true : false;
					if(false == flagHide)
					{
						switch(dataAnimationParts.Format)
						{
							case Library_SpriteStudio6.Data.Animation.Parts.KindFormat.PLAIN:
								ParameterSprite.DrawPlain(instanceRoot, idParts, InstanceGameObject, InstanceTransform, ref matrixCorrection, ref Status, ref dataAnimationParts, ref controlTrack.ArgumentContainer);
								break;
							case Library_SpriteStudio6.Data.Animation.Parts.KindFormat.FIX:
								ParameterSprite.DrawFix(instanceRoot, idParts, InstanceGameObject, InstanceTransform, ref matrixCorrection, ref Status, ref dataAnimationParts, ref controlTrack.ArgumentContainer);
								break;
						}
					}
				}
				internal void DrawInstance(	Script_SpriteStudio6_Root instanceRoot,
											int idParts,
											ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
											ref Library_SpriteStudio6.Control.Animation.Track controlTrack,
											bool flagHideDefault,
											ref Matrix4x4 matrixCorrection
										)
				{

					if(null == InstanceRootUnderControl)
					{	/* "Instance" animation object invalid */
						return;
					}

					int frame = controlTrack.ArgumentContainer.Frame;
					bool flagDecode = controlTrack.StatusIsDecodeAttribute;
					bool flagPlayIndependentNowInstance = (0 != (Status & FlagBitStatus.INSTANCE_PLAYINDEPENDENT)) ? true : false;
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
						if(true == dataAnimationParts.Instance.Function.ValueGet(ref dataInstance, ref frameKey, dataAnimationParts.Instance, ref controlTrack.ArgumentContainer))
						{   /* New Valid Data */
							if(FramePreviousUpdateUnderControl != frameKey)
							{	/* Different attribute */
								bool flagPlayReverseInstanceData = (0.0f > dataInstance.RateTime) ? true : false;
								bool flagPlayReverseInstance = flagPlayReverseInstanceData ^ flagPlayReverse;

								/* MEMO: Playing target are all tracks. And TableInformationPlay[0] is always used. */
								InstanceRootUnderControl.AnimationPlay(	-1,	/* All track */
																		IndexAnimationUnderControl,
																		dataInstance.PlayCount,
																		0,
																		dataInstance.RateTime * ((true == flagPlayReverse) ? -1.0f : 1.0f),
																		((0 != (dataInstance.Flags & Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.PINGPONG)) ? Library_SpriteStudio6.KindStylePlay.PINGPONG : Library_SpriteStudio6.KindStylePlay.NORMAL),
																		dataInstance.LabelStart,
																		dataInstance.OffsetStart,
																		dataInstance.LabelEnd,
																		dataInstance.OffsetEnd
																	);

								/* Adjust Starting-Time */
								/* MEMO: Necessary to set time, not frame. Because parent's elapsed time has a small excess. */
								if(true == flagPlayReverse)
								{   /* Play-Reverse */
									flagTimeWrap = flagTopFrame & flagPlayReverseInstanceData;
									if(frameKey <= frame)
									{   /* Immediately */
										timeOffset = (float)(frameKey - controlTrack.FrameStart);
										timeOffset = controlTrack.TimeElapsed - (timeOffset * controlTrack.TimePerFrame);
										InstanceRootUnderControl.TableControlTrack[0].TimeElapse(timeOffset, flagPlayReverse, flagTimeWrap);
									}
									else
									{	/* Wait */
										if(true == flagPlayReverseInstance)
										{	/* Instance: Play-Reverse */
											InstanceRootUnderControl.TableControlTrack[0].TimeElapse(0.0f, flagPlayReverse, flagTimeWrap);
											InstanceRootUnderControl.TableControlTrack[0].TimeDelay = 0.0f;
											InstanceRootUnderControl.AnimationStop(-1, false);	/* ??? */
										}
										else
										{	/* Instance: Play-Foward */
											timeOffset = ((float)frameKey * controlTrack.TimePerFrame) - controlTrack.TimeElapsed;
											InstanceRootUnderControl.TableControlTrack[0].TimeElapse(0.0f, flagPlayReverse, flagTimeWrap);
											InstanceRootUnderControl.TableControlTrack[0].TimeDelay = timeOffset;
										}
									}
								}
								else
								{   /* Play-Foward */
									flagTimeWrap = flagTopFrame & flagPlayReverseInstanceData;
									if(frameKey <= frame)
									{   /* Immediately */
										timeOffset = (float)(frameKey - controlTrack.FrameStart);
										timeOffset = controlTrack.TimeElapsed - (timeOffset * controlTrack.TimePerFrame);
										InstanceRootUnderControl.TableControlTrack[0].TimeElapse(timeOffset, flagPlayReverse, flagTimeWrap);
									}
									else
									{	/* Wait */
										if(true == flagPlayReverseInstance)
										{	/* Instance: Play-Reverse */
											InstanceRootUnderControl.TableControlTrack[0].TimeElapse(0.0f, flagPlayReverse, flagTimeWrap);
											InstanceRootUnderControl.TableControlTrack[0].TimeDelay = 0.0f;
											InstanceRootUnderControl.AnimationStop(-1, false);	/* ??? */
										}
										else
										{	/* Instance: Play-Foward */
											timeOffset = ((float)frameKey * controlTrack.TimePerFrame) - controlTrack.TimeElapsed;
											InstanceRootUnderControl.TableControlTrack[0].TimeElapse(0.0f, flagPlayReverse, flagTimeWrap);
											InstanceRootUnderControl.TableControlTrack[0].TimeDelay = timeOffset;
										}
									}
								}

								/* Status Update */
								FramePreviousUpdateUnderControl = frameKey;
								Status = (0 != (dataInstance.Flags & Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.INDEPENDENT)) ? (Status | FlagBitStatus.INSTANCE_PLAYINDEPENDENT) : (Status & ~FlagBitStatus.INSTANCE_PLAYINDEPENDENT);
							}
						}
					}

					/* Update "Instance" */
					/* MEMO: "Instance" is updated from here. (Not updated from Monobehaviour's LateUpdate) */
					bool flagHide = flagHideDefault;
					flagHide |= (0 != (Status & (FlagBitStatus.HIDE_FORCE | FlagBitStatus.HIDE))) ? true : false;
					InstanceRootUnderControl.LateUpdateMain(controlTrack.TimeElapsedNow, flagHide, ref matrixCorrection);
				}
				internal void DrawEffect(	Script_SpriteStudio6_Root instanceRoot,
											int idParts,
											ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
											ref Library_SpriteStudio6.Control.Animation.Track controlTrack,
											bool flagHideDefault,
											ref Matrix4x4 matrixCorrection
										)
				{	/* CAUTION!: Ver.SS5.6 Unsupported. */
					if(null == InstanceRootEffectUnderControl)
					{	/* "Effect" animation object invalid */
						return;
					}

					int frame = controlTrack.ArgumentContainer.Frame;
					bool flagPlayIndependentNowInstance = (0 != (Status & FlagBitStatus.EFFECT_PLAYINDEPENDENT)) ? true : false;
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
					if(true == controlTrack.StatusIsDecodeAttribute)
					{
						/* Decode data */
						Library_SpriteStudio6.Data.Animation.Attribute.Effect dataEffect = new Library_SpriteStudio6.Data.Animation.Attribute.Effect();
						if(true == dataAnimationParts.Effect.Function.ValueGet(ref dataEffect, ref frameKey, dataAnimationParts.Effect, ref controlTrack.ArgumentContainer))
						{   /* New Valid Data */
							if(FramePreviousUpdateUnderControl != frameKey)
							{	/* Different attribute */
								/* Wait Set */
								if(frameKey <= frame)
								{	/* Immediately */
									/* Play-Start */
									InstanceRootEffectUnderControl.AnimationPlay(	1,
																					dataEffect.RateTime * ((true == flagPlayReverse) ? -1.0f : 1.0f),
																					controlTrack.FramePerSecond
																				);

									InstanceRootEffectUnderControl.SeedOffsetSet((uint)controlTrack.CountLoop);

									/* Adjust Time */
									timeOffset = controlTrack.TimeElapsed - ((float)((frameKey - controlTrack.FrameStart) - dataEffect.FrameStart) * controlTrack.TimePerFrame);
									InstanceRootEffectUnderControl.TimeElapse(timeOffset, false);

									/* Status Update */
									FramePreviousUpdateUnderControl = frameKey;
									Status = (0 != (dataEffect.Flags & Library_SpriteStudio6.Data.Animation.Attribute.Effect.FlagBit.INDEPENDENT)) ? (Status | FlagBitStatus.EFFECT_PLAYINDEPENDENT) : (Status & ~FlagBitStatus.EFFECT_PLAYINDEPENDENT);
								}
							}
						}
					}

					/* Update Effect */
					/* MEMO: "Instance" is updated from here. (Not updated from Monobehaviour's LateUpdate) */
					bool flagHide = flagHideDefault;
					flagHide |= (0 != (Status & (FlagBitStatus.HIDE_FORCE | FlagBitStatus.HIDE))) ? true : false;
					InstanceRootEffectUnderControl.LateUpdateMain(controlTrack.TimeElapsedNow, flagHide, ref matrixCorrection);
				}

				internal void AnimationRefresh()
				{
					Status |= (	FlagBitStatus.REFRESH_TRANSFORM_POSITION
								| FlagBitStatus.REFRESH_TRANSFORM_ROTATION
								| FlagBitStatus.REFRESH_TRANSFORM_SCALING
								| FlagBitStatus.INSTANCE_PLAYINDEPENDENT
							);

					FramePreviousUpdateUnderControl = -1;
					FramePreviousUpdateRadiusCollision = -1;
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

					OVERWRITE_CELL_UNREFLECTED = 0x00080000,
					OVERWRITE_CELL_IGNOREATTRIBUTE = 0x00040000,

					INSTANCE_VALID = 0x00008000,
					INSTANCE_PLAYINDEPENDENT = 0x00004000,
					EFFECT_VALID = 0x00002000,
					EFFECT_PLAYINDEPENDENT = 0x00001000,

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
					internal Vector2 RateScaleMesh;
					internal Vector2 RateScaleTexture;

					internal int CountVertex;
					internal Material MaterialDraw;
					internal Vector3[] CoordinateTransformDraw;
					internal Vector3[] CoordinateDraw;
					internal Color32[] ColorPartsDraw;
					internal Vector2[] ParameterBlendDraw;
					internal Vector2[] UVTextureDraw;
					internal Library_SpriteStudio6.Draw.Cluster.Chain ChainDraw;

					/* MEMO: Only "Data-Plain" */
					internal int IndexCellMapDraw;
					internal int IndexCellDraw;
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
					internal BufferAttribute<Vector2> OffsetPivot;
					internal BufferAttribute<Vector2> SizeForce;
					internal BufferAttribute<Vector2> ScalingTexture;
					internal BufferAttribute<Vector2> PositionTexture;
					internal BufferAttribute<float> RotationTexture;
					internal BufferAttribute<float> RateOpacity;
					internal BufferAttribute<Library_SpriteStudio6.Data.Animation.Attribute.PartsColor> PartsColor;
					internal BufferAttribute<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection> VertexCorrection;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					internal void CleanUp()
					{
						Status = FlagBitStatus.CLEAR;
						RateScaleMesh = Vector2.one;
						RateScaleTexture = Vector2.one;

						MaterialDraw = null;
						CoordinateDraw = null;
						CoordinateTransformDraw = null;
						ColorPartsDraw = null;
						UVTextureDraw = null;
						ParameterBlendDraw = null;
						ChainDraw = null;

						IndexCellMapDraw = -1;
						IndexCellDraw = -1;
						SizeCell = Vector2.zero;
						PivotCell = Vector2.zero;
						SizeTexture = SizeTextureDefault;
						SizeCell = SizeTextureDefault;
						PivotCell = Vector2.zero;
						PositionCell = Vector2.zero;
						MatrixTexture = Matrix4x4.identity;
						DataCellApply.CleanUp();
						IndexVertexCollectionTable = 0;

						DataCell.CleanUp();	DataCell.Value.CleanUp();
						OffsetPivot.CleanUp();	OffsetPivot.Value = Vector2.zero;
						SizeForce.CleanUp();	SizeForce.Value = Vector2.zero;
						ScalingTexture.CleanUp();	ScalingTexture.Value = Vector2.one;
						PositionTexture.CleanUp();	PositionTexture.Value = Vector2.zero;
						RotationTexture.CleanUp();	RotationTexture.Value = 0.0f;
						RateOpacity.CleanUp();	RateOpacity.Value = 1.0f;
						PartsColor.CleanUp();	PartsColor.Value.CleanUp();
						VertexCorrection.CleanUp();	VertexCorrection.Value.CleanUp();
					}

					internal bool BootUp(int countVertex, int countPartsSprite)
					{
						CleanUp();

						CountVertex = countVertex;

						MaterialDraw = null;
						CoordinateDraw = new Vector3[countVertex];
						if(null == CoordinateDraw)
						{
							goto BootUp_ErrorEnd;
						}
						CoordinateTransformDraw = new Vector3[countVertex];
						if(null == CoordinateTransformDraw)
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

						for(int i=0; i<countVertex; i++)
						{
							CoordinateDraw[i] = Library_SpriteStudio6.Draw.Model.TableCoordinate[i];
							ColorPartsDraw[i] = Library_SpriteStudio6.Draw.Model.TableColor32[i];
							UVTextureDraw[i] = Library_SpriteStudio6.Draw.Model.TableUVMapping[i];
							ParameterBlendDraw[i] = Vector2.zero;
						}

						ChainDraw = new Draw.Cluster.Chain();
						if(null == ChainDraw)
						{
							goto BootUp_ErrorEnd;
						}
//						ChainDraw.CleanUp();
						ChainDraw.BootUp();

						PartsColor.Value.BootUp();
						VertexCorrection.Value.BootUp();

						Status |= (	FlagBitStatus.UPDATE_COORDINATE
									| FlagBitStatus.UPDATE_UVTEXTURE
									| FlagBitStatus.UPDATE_PARAMETERBLEND
									| FlagBitStatus.UPDATE_COLORPARTS
								);

						return(true);

					BootUp_ErrorEnd:;
						MaterialDraw = null;
						CoordinateDraw = null;
						ColorPartsDraw = null;
						UVTextureDraw = null;
						ParameterBlendDraw = null;
						ChainDraw = null;
						return(false);
					}

					internal void StatusSetFlip(ref Library_SpriteStudio6.Data.Animation.Attribute.Status status)
					{
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

					internal bool UpdateSpriteSizePlain(	Script_SpriteStudio6_Root instanceRoot,
															int idParts,
															GameObject instanceGameObject,
															Transform instanceTransform,
															ref Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus statusControlParts,
															ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
															ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer
														)
					{
						bool flagUpdateValueAttribute;

						/* Create sprite data (from cell to use) */
						if(true == dataAnimationParts.Plain.Cell.Function.ValueGet(ref DataCell.Value, ref DataCell.FrameKey, dataAnimationParts.Plain.Cell, ref argumentContainer))
						{
							if(0 == (statusControlParts & (Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.OVERWRITE_CELL_IGNOREATTRIBUTE | Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.OVERWRITE_CELL_UNREFLECTED)))
							{	/* Use data in attribute. */
								DataCellApply = DataCell.Value;

								Status |= FlagBitStatus.UPDATE_COORDINATE;
								Status |= FlagBitStatus.UPDATE_UVTEXTURE;
							}
							else
							{	/* Overwrite */
								if(0 != (statusControlParts & Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.OVERWRITE_CELL_UNREFLECTED))
								{	/* Unreflected */
									Status |= FlagBitStatus.UPDATE_COORDINATE;
									Status |= FlagBitStatus.UPDATE_UVTEXTURE;
								}
							}
							statusControlParts &= ~Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.OVERWRITE_CELL_UNREFLECTED;
						}

						IndexCellMapDraw = -1;
						IndexCellDraw = -1;
						Library_SpriteStudio6.Data.CellMap cellMap = instanceRoot.DataGetCellMap(DataCellApply.IndexCellMap);
						if(null != cellMap)
						{	/* CellMap Valid */
							IndexCellDraw = DataCellApply.IndexCell;
							if((0 <= IndexCellDraw) && (cellMap.CountGetCell() > IndexCellDraw))
							{	/* Cell Valid */
								IndexCellMapDraw = DataCellApply.IndexCellMap;
								IndexCellDraw = DataCellApply.IndexCell;
							}
						}
						if(0 > IndexCellDraw)
						{	/* Invalid */
							SizeTexture = SizeTextureDefault;

							SizeCell = SizeTextureDefault;
							PivotCell = Vector2.zero;
							PositionCell =Vector2.zero;
						}
						else
						{	/* Valid */
							SizeTexture = cellMap.SizeOriginal;

							SizeCell.x = cellMap.TableCell[IndexCellDraw].Rectangle.width;
							SizeCell.y = cellMap.TableCell[IndexCellDraw].Rectangle.height;
							PivotCell = cellMap.TableCell[IndexCellDraw].Pivot;
							PositionCell.x = cellMap.TableCell[IndexCellDraw].Rectangle.xMin;
							PositionCell.y = cellMap.TableCell[IndexCellDraw].Rectangle.yMin;
						}

						Vector2 sizeSprite = SizeCell;
						Vector2 pivotSprite = PivotCell;

						/* Correct Sprite data (by attributes) */
						flagUpdateValueAttribute = dataAnimationParts.Plain.OffsetPivot.Function.ValueGet(ref OffsetPivot.Value, ref OffsetPivot.FrameKey, dataAnimationParts.Plain.OffsetPivot, ref argumentContainer);
						if(true == flagUpdateValueAttribute)
						{
							Status |= FlagBitStatus.UPDATE_COORDINATE;
						}
						pivotSprite.x += (sizeSprite.x * OffsetPivot.Value.x) * RateScaleMesh.x;
						pivotSprite.y -= (sizeSprite.y * OffsetPivot.Value.y) * RateScaleMesh.y;

						flagUpdateValueAttribute = dataAnimationParts.SizeForce.Function.ValueGet(ref SizeForce.Value, ref SizeForce.FrameKey, dataAnimationParts.SizeForce, ref argumentContainer);
						if(true == flagUpdateValueAttribute)
						{
							Status |= FlagBitStatus.UPDATE_COORDINATE;
						}
						if(0 <= SizeForce.FrameKey)
						{
							float ratePivot;
							float size;
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

						SizeSprite = sizeSprite;
						PivotSprite = pivotSprite;
						return(true);
					}
					internal bool UpdateSpriteSizeFix(	Script_SpriteStudio6_Root instanceRoot,
														int idParts,
														GameObject instanceGameObject,
														Transform instanceTransform,
														ref Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus statusControlParts,
														ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
														ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer
													)
					{
						return(false);
					}

					internal void DrawPlain(	Script_SpriteStudio6_Root instanceRoot,
												int idParts,
												GameObject instanceGameObject,
												Transform instanceTransform,
												ref Matrix4x4 matrixCorrection,
												ref Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus statusControlParts,
												ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
												ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer
											)
					{
						bool flagUpdateValueAttribute;

						Vector2 sizeSprite = SizeSprite;
						Vector2 pivotSprite = PivotSprite;
						Vector2 sizeMapping = SizeCell;
						Vector2 positionMapping = PositionCell;

						/* Get Rate-Opacity */
						flagUpdateValueAttribute = dataAnimationParts.RateOpacity.Function.ValueGet(ref RateOpacity.Value, ref RateOpacity.FrameKey, dataAnimationParts.RateOpacity, ref argumentContainer); 
						if(true == flagUpdateValueAttribute)
						{
							Status |= FlagBitStatus.UPDATE_PARAMETERBLEND;
						}

						/* Calculate Texture-UV */
						if(0 != (dataAnimationParts.StatusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_TRANSFORMATION_TEXTURE))
						{	/* No Transform (Ordinary rectangle) */
							Vector2 uLR = new Vector2(positionMapping.x, positionMapping.x + sizeMapping.x);
							float mappingYInverse = SizeTexture.y - positionMapping.y;
							Vector2 vUD = new Vector2(mappingYInverse, mappingYInverse - sizeMapping.y);
							uLR /= SizeTexture.x;
							vUD /= SizeTexture.y;

							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LU].x = uLR.x;
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LU].y = vUD.x;

							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RU].x = uLR.y;
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RU].y = vUD.x;

							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RD].x = uLR.y;
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RD].y = vUD.y;

							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LD].x = uLR.x;
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LD].y = vUD.y;
						}
						else
						{	/* Transform Texure */
							bool flagUpdateMatrixTexrure = false;
							flagUpdateMatrixTexrure |= dataAnimationParts.Plain.PositionTexture.Function.ValueGet(ref PositionTexture.Value, ref PositionTexture.FrameKey, dataAnimationParts.Plain.PositionTexture, ref argumentContainer);
							flagUpdateMatrixTexrure |= dataAnimationParts.Plain.ScalingTexture.Function.ValueGet(ref ScalingTexture.Value, ref ScalingTexture.FrameKey, dataAnimationParts.Plain.ScalingTexture, ref argumentContainer);
							flagUpdateMatrixTexrure |= dataAnimationParts.Plain.RotationTexture.Function.ValueGet(ref RotationTexture.Value, ref RotationTexture.FrameKey, dataAnimationParts.Plain.RotationTexture, ref argumentContainer);
							if(true == flagUpdateMatrixTexrure)
							{
								Vector2 centerMapping = (sizeMapping * 0.5f) + positionMapping;
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

							for(int i=0; i<CountVertex; i++)
							{
								UVTextureDraw[i] = MatrixTexture.MultiplyPoint3x4(Library_SpriteStudio6.Draw.Model.TableUVMapping[i]);
							}
						}

						/* Set Parts-Color */
//						if(null != DataPartsColorOverwrite)
//						{
//							if(KindColorOperation.NON != DataPartsColorOverwrite.Operation))
//							{
//								goto Plain_PartColor_Clear;
//							}
//						}
//						else
						{
							flagUpdateValueAttribute = dataAnimationParts.Plain.PartsColor.Function.ValueGet(ref PartsColor.Value, ref PartsColor.FrameKey, dataAnimationParts.Plain.PartsColor, ref argumentContainer);
							if(true == flagUpdateValueAttribute)
							{
								Status |= FlagBitStatus.UPDATE_COLORPARTS;
							}
							if(0 != (Status & FlagBitStatus.UPDATE_COLORPARTS))
							{
								Status |= FlagBitStatus.UPDATE_PARAMETERBLEND;

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
								ParameterBlendDraw[i].x = operation;
								ParameterBlendDraw[i].y = tableAlpha[i] * RateOpacity.Value;
								sumPower += ParameterBlendDraw[i].y;

								ColorPartsDraw[i] = tableColor[i];
								sumColor += tableColor[i];
							}
							tableAlpha = null;
							tableColor = null;

							if((int)Library_SpriteStudio6.KindVertex.TERMINATOR4 == CountVertex)
							{
								sumColor *= 0.25f;
								sumPower *= 0.25f;

								ParameterBlendDraw[(int)Library_SpriteStudio6.KindVertex.C].x = operation;
								ParameterBlendDraw[(int)Library_SpriteStudio6.KindVertex.C].y = sumPower;
								ColorPartsDraw[(int)Library_SpriteStudio6.KindVertex.C] = sumColor;
							}
						}

						goto Plain_PartColor_End;

					Plain_PartColor_Clear:;
						for(int i=0; i<CountVertex; i++)
						{
							ParameterBlendDraw[i].x = (float)((int)Library_SpriteStudio6.KindOperationBlend.NON) + 1.01f;	/* "+1.0f" for -1->0 *//* "+0.01f" for Rounding-off-Error */
							ParameterBlendDraw[i].y = RateOpacity.Value;	/* Opacity */
						}
					Plain_PartColor_End:;

						/* Calculate Mesh coordinates */
						float left = (-pivotSprite.x) * RateScaleMesh.x;
						float right = (sizeSprite.x - pivotSprite.x) * RateScaleMesh.x;
						float top = -((-pivotSprite.y) * RateScaleMesh.y);	/* * -1.0f ... Y-Axis Inverse */
						float bottom = -((sizeSprite.y - pivotSprite.y) * RateScaleMesh.y);	/* * -1.0f ... Y-Axis Inverse */

						if((int)Library_SpriteStudio6.KindVertex.TERMINATOR4 == CountVertex)
						{	/* 4-Triangles Mesh */
							/* Set Mapping (Center) */
							Vector2 uv2C = UVTextureDraw[0];
							uv2C += UVTextureDraw[1];
							uv2C += UVTextureDraw[2];
							uv2C += UVTextureDraw[3];
							uv2C *= 0.25f;
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.C] = uv2C;

							/* Set Coordinates */
							dataAnimationParts.Plain.VertexCorrection.Function.ValueGet(ref VertexCorrection.Value, ref VertexCorrection.FrameKey, dataAnimationParts.Plain.VertexCorrection, ref argumentContainer);
							int indexVertex;
							int[] tableIndex = TableIndexVertexCorrectionOrder[IndexVertexCollectionTable];
							Vector2[] tableCoordinate = VertexCorrection.Value.Coordinate;

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.LU];
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LU] = new Vector3((left + tableCoordinate[indexVertex].x), (top + tableCoordinate[indexVertex].y), 0.0f);

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.RU];
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RU] = new Vector3((right + tableCoordinate[indexVertex].x), (top + tableCoordinate[indexVertex].y), 0.0f);

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.RD];
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RD] = new Vector3((right + tableCoordinate[indexVertex].x), (bottom + tableCoordinate[indexVertex].y), 0.0f);

							indexVertex = tableIndex[(int)Library_SpriteStudio6.KindVertex.LD];
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LD] = new Vector3((left + tableCoordinate[indexVertex].x), (bottom + tableCoordinate[indexVertex].y), 0.0f);

							/* MEMO: Centering on intersection of diagonals of the 4 sides' midpoints. (not 4 vertices.) */
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
						else
						{	/* 2-Triangles Mesh */
							/* Set Coordinates */
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LU] = new Vector3(left, top, 0.0f);
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RU] = new Vector3(right, top, 0.0f);
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RD] = new Vector3(right, bottom, 0.0f);
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LD] = new Vector3(left, bottom, 0.0f);
						}

						/* Draw */
						/* MEMO: Prevent double effect MeshRenderer's world-matrix and InstanceTransform's world-matrix. */
						Matrix4x4 matrixTransform = matrixCorrection * instanceTransform.localToWorldMatrix;
						for(int i=0; i<CountVertex; i++)
						{
							CoordinateTransformDraw[i] = matrixTransform.MultiplyPoint3x4(CoordinateDraw[i]);
						}

//						if(0 != (Status & FlagBitStatus.UPDATE_COORDINATE))
//						{
//						}
						if(0 != (Status & FlagBitStatus.UPDATE_UVTEXTURE))
						{
							MaterialDraw = instanceRoot.MaterialGet(IndexCellMapDraw, instanceRoot.DataAnimation.TableParts[idParts].OperationBlendTarget);
						}
//						if(0 != (Status & FlagBitStatus.UPDATE_COLORPARTS))
//						{
//						}
//						if(0 != (Status & FlagBitStatus.UPDATE_PARAMETERBLEND))
//						{
//						}

						Library_SpriteStudio6.Draw.Cluster cluster = instanceRoot.ClusterDraw;
						cluster.VertexAdd(	ChainDraw,
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
								);
					}

					internal void DrawFix(	Script_SpriteStudio6_Root instanceRoot,
											int idParts,
											GameObject instanceGameObject,
											Transform instanceTransform,
											ref Matrix4x4 matrixCorrection,
											ref Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus statusControlParts,
											ref Library_SpriteStudio6.Data.Animation.Parts dataAnimationParts,
											ref Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer
										)
					{
					}
					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					[System.Flags]
					internal enum FlagBitStatus
					{
						UPDATE_COORDINATE = 0x00000001,
						UPDATE_UVTEXTURE = 0x00000002,
						UPDATE_PARAMETERBLEND = 0x00000004,
						UPDATE_COLORPARTS = 0x00000008,

						CLEAR = 0x00000000
					}

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
