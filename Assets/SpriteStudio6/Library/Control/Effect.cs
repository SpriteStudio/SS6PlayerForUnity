/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Library_SpriteStudio6
{
	public static partial class Control
	{
		public partial struct Effect
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			internal FlagBitStatus Status;

			internal Emitter[] TableEmitter;
			internal int CountMeshParticle;

			internal Script_SpriteStudio6_RootEffect InstanceRootEffect;
			internal int DurationFull;

			internal int LimitParticleDraw;
			internal int CountParticleDraw;

			internal uint Seed;
			internal uint SeedOffset;

			internal Library_SpriteStudio6.KindMasking Masking;

			internal Matrix4x4 MatrixRoot;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			internal void CleanUp()
			{
				Status = FlagBitStatus.CLEAR;

				TableEmitter = null;
				CountMeshParticle = 0;

				InstanceRootEffect = null;
				DurationFull = -1;

				LimitParticleDraw = 0;
				CountParticleDraw = 0;

				Seed = 0;
				SeedOffset = 0;

				Masking = (Library_SpriteStudio6.KindMasking)(-1);

				MatrixRoot = Matrix4x4.identity;
			}

			internal void SeedSet(uint seed)
			{
				Seed = seed * (uint)Library_SpriteStudio6.Data.Effect.Emitter.Constant.SEED_MAGIC;
			}

			internal void SeedOffsetSet(uint offsetSeed)
			{
				SeedOffset = (0 != (Status & FlagBitStatus.LOCKSEED)) ? 0 : offsetSeed;
			}

			internal void StatusSetLoop(bool flagSwitch)
			{
				Status = (true == flagSwitch) ? (Status | FlagBitStatus.LOOP) : (Status & ~FlagBitStatus.LOOP);
			}

			internal bool BootUp(Script_SpriteStudio6_RootEffect instanceRoot)
			{
				if((null == instanceRoot) || (null == instanceRoot.DataCellMap) || (null == instanceRoot.DataEffect))
				{
					return(false);
				}

				/* Set Masking */
				/* MEMO: Tentatively, set initial to THROUGH. */
				Masking = Library_SpriteStudio6.KindMasking.THROUGH;

				/* Boot up Emitters */
				Status = FlagBitStatus.CLEAR;

				SeedSet(Script_SpriteStudio6_RootEffect.KeyCreateRandom());

				int countParts = instanceRoot.DataEffect.CountGetParts();
				int countEmitter = instanceRoot.DataEffect.CountGetEmitter();
				if((null == TableEmitter) || (countEmitter != TableEmitter.Length))
				{
					TableEmitter = new Emitter[countEmitter];
					if(null == TableEmitter)
					{
						goto BootUp_ErrorEnd;
					}
				}

				int indexEmitter;
				int indexEmitterParent;
				bool flagInfinite = false;
				for(int i=1; i<countParts; i++)	/* MEMO: 0 == Root (has no emitter) */
				{
					indexEmitter = instanceRoot.DataEffect.TableParts[i].IDParent;	/* Temporary */
					indexEmitterParent = (0 >= indexEmitter) ? -1 : instanceRoot.DataEffect.TableParts[indexEmitter].IndexEmitter;	/* Root has no emitter */

					indexEmitter = instanceRoot.DataEffect.TableParts[i].IndexEmitter;
					TableEmitter[indexEmitter].CleanUp();
					if(false == TableEmitter[indexEmitter].BootUp(	instanceRoot,
																	instanceRoot.DataEffect,
																	i,
																	indexEmitter,
																	indexEmitterParent,
																	ref this
																)
						)
					{
						goto BootUp_ErrorEnd;
					}

					flagInfinite |= (0 != (instanceRoot.DataEffect.TableEmitter[indexEmitter].FlagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.EMIT_INFINITE)) ? true : false;
				}

				instanceRoot.StatusIsPlayingInfinity = flagInfinite;
				Status |= (true == flagInfinite) ? FlagBitStatus.INFINITE : FlagBitStatus.CLEAR;
				Status |= (true == instanceRoot.DataEffect.StatusIsLockSeedRandom) ? FlagBitStatus.LOCKSEED : FlagBitStatus.CLEAR;

				/* Get Effect's length */
				int frameFull;
				DurationFull = 0;
				for (int i=0; i<countEmitter; i++)
				{
					indexEmitterParent = TableEmitter[i].IndexParent;
					frameFull = TableEmitter[i].FrameFull;
					if(0 <= indexEmitterParent)
					{	/* Sub-Emitters */
						frameFull += TableEmitter[indexEmitterParent].FrameFull;
					}
					TableEmitter[i].FrameGlobal = frameFull;

					DurationFull = (frameFull > DurationFull) ? frameFull : DurationFull;
				}

				/* Reset Transform */
				MatrixRoot = Matrix4x4.identity;

				/* Set Limit drawing particle */
				LimitParticleDraw = instanceRoot.CountParticleMax;
				CountParticleDraw = 0;

				Status |= FlagBitStatus.RUNNING;

				return(true);

			BootUp_ErrorEnd:;
				CleanUp();
				return(false);
			}

			internal void Update(	Script_SpriteStudio6_RootEffect instanceRoot,
									Library_SpriteStudio6.KindMasking masking,
									ref Matrix4x4 matrixCorrection,
									bool flagPlanarization
								)
			{
				/* Check WorkArea lost */
				if(null == TableEmitter)
				{
					BootUp(instanceRoot);
				}

				/* Create Emitter's matrix */
				Matrix4x4 matrix = Matrix4x4.TRS(	Vector3.zero,
													Quaternion.Euler(0.0f, 0.0f, 0.0f),
													instanceRoot.RateScaleLocal
												);
				MatrixRoot = matrixCorrection * instanceRoot.transform.localToWorldMatrix * matrix;
				if(true == flagPlanarization)
				{
					/* MEMO: Z-coordinate is always set to 0 after transformation. */
					MatrixRoot[2, 0] = 
					MatrixRoot[2, 1] = 
					MatrixRoot[2, 2] = 
					MatrixRoot[2, 3] = 0.0f;
				}
				CountParticleDraw = 0;

				/* Emitters' Random-Seed Refresh */
				int frame = (int)instanceRoot.Frame;
				int frameTarget = frame;
				int countLoop = 0;
				if(0 == (Status & FlagBitStatus.INFINITE))
				{
					if(0 != (Status & FlagBitStatus.LOOP))
					{
						if(frame > DurationFull)
						{
							frameTarget = frame % DurationFull;
							countLoop = frame / DurationFull;
							SeedOffsetSet((uint)countLoop);
						}
					}
				}

				/* Check Masking & Update Cells */
				bool flagUpdateCell = instanceRoot.StatusIsChangeCellMap;
				if(Library_SpriteStudio6.KindMasking.FOLLOW_DATA == masking)
				{
					masking = Library_SpriteStudio6.KindMasking.THROUGH;
				}
				if(Masking != masking)
				{
					Masking = masking;
					flagUpdateCell |= true;
				}

				/* Update Emitters */
				int[] tableIndexEmitter = instanceRoot.DataEffect.TableIndexEmitterOrderDraw;
				int countEmitter = tableIndexEmitter.Length;
				int indexEmitter;
				int indexEmitterParent;
				bool flagDrawAll;
				for(int i=0; i<countEmitter; i++)
				{
					indexEmitter = tableIndexEmitter[i];

					/* Check Redecode Materials */
					if(true == instanceRoot.StatusIsChangeCacheMaterial)
					{
						TableEmitter[indexEmitter].Status |= Emitter.FlagBitStatus.REDECODE_MATERIAL;
					}

					/* Update Material & Cell */
					if(	(true == flagUpdateCell)
						|| (0 != (TableEmitter[indexEmitter].Status & Emitter.FlagBitStatus.CHANGE_CELL_UNREFLECTED | Emitter.FlagBitStatus.REDECODE_MATERIAL))
					)
					{
						TableEmitter[indexEmitter].CellPresetParticle(instanceRoot, Masking);
					}

					/* Update Random-Seed-Offset */
					TableEmitter[indexEmitter].SeedOffset = SeedOffset;

					/* Update Emitter */
					indexEmitterParent = TableEmitter[indexEmitter].IndexParent;
					if(0 <= indexEmitterParent)
					{   /* Has Parent-Emitter */
						flagDrawAll = TableEmitter[indexEmitterParent].UpdateSubEmitters(	frameTarget,
																							instanceRoot,
																							ref this,
																							indexEmitter,	// indexEmitterParent,
																							ref TableEmitter[indexEmitter]
																						);
					}
					else
					{	/* Has no Parent-Emitter */
						flagDrawAll = TableEmitter[indexEmitter].Update(	frameTarget,
																			instanceRoot,
																			ref this,
																			-1
																		);
					}

					if(false == flagDrawAll)
					{	/* Draw-Limit Over */
						return;
					}
				}
				return;
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			internal enum FlagBitStatus
			{
				RUNNING = 0x40000000,
				PLAYING = 0x02000000,

				LOCKSEED = 0x08000000,
				INFINITE = 0x04000000,
				LOOP = 0x02000000,

				CLEAR = 0x00000000
			}
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			internal struct Emitter
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				internal FlagBitStatus Status;
				internal int IndexParent;

				internal Script_SpriteStudio6_DataEffect DataEffect;
				internal int IDParts;
				internal int IndexEmitter;

				internal Library_SpriteStudio6.Data.Animation.Attribute.Cell DataCellApply;

				internal Library_SpriteStudio6.Utility.Random.Generator Random;
				internal Library_SpriteStudio6.Data.Effect.Emitter.PatternEmit[] TablePatternEmit;
				internal int[] TablePatternOffset;
				internal long[] TableSeedParticle;
				internal Library_SpriteStudio6.Control.Effect.Particle.Activity[] TableActivityParticle;

				internal uint SeedRandom;
				internal uint SeedOffset;

				internal int Duration;
				internal Vector2 Position;
				internal int FrameGlobal;

				internal Library_SpriteStudio6.Control.Effect.Particle ParticleTempolary2;	/* (mainly) for TurnToDirection (mainly) */
				internal Library_SpriteStudio6.Control.Effect.Particle ParticleTempolary;	/* (mainly) for Parent */
				internal Library_SpriteStudio6.Control.Effect.Particle Particle;

				internal Material MaterialDraw;
				internal Vector3[] CoordinateTransformDraw;
				internal Vector3[] CoordinateDraw;
				internal Color32[] ColorVertexDraw;
				internal Vector4[] UVTextureDraw;
				internal Vector4[] UVMaxMinDraw;
				internal Vector4[] UVAverageDraw;
				internal Library_SpriteStudio6.Draw.Cluster.Chain ChainDraw;

				internal int FrameFull
				{
					get
					{
						return(	DataEffect.TableEmitter[IndexEmitter].DurationEmitter
								+ (int)DataEffect.TableEmitter[IndexEmitter].DurationParticle.Main
//								+ (int)(DataEffect.TableEmitter[IndexEmitter].DurationParticle.Main + DataEffect.TableEmitter[IndexEmitter].DurationParticle.Sub)
								+ (int)DataEffect.TableEmitter[IndexEmitter].DurationParticle.Sub
								+ DataEffect.TableEmitter[IndexEmitter].Delay
							);
					}
				}
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				internal void CleanUp()
				{
					Status = FlagBitStatus.CLEAR;
					IndexParent = -1;

					DataEffect = null;
					IDParts = -1;
					IndexEmitter = -1;

					DataCellApply.CleanUp();

					Random = null;
					TablePatternEmit = null;
					TableSeedParticle = null;
					TableActivityParticle = null;

					SeedRandom = (uint)Library_SpriteStudio6.Data.Effect.Emitter.Constant.SEED_MAGIC;
					SeedOffset = 0;

					Duration = 0;
					Position = Vector2.zero;
					FrameGlobal = 0;

					MaterialDraw = null;
					CoordinateTransformDraw = null;
					CoordinateDraw = null;
					ColorVertexDraw = null;
					UVTextureDraw = null;
					UVMaxMinDraw = null;
					UVAverageDraw = null;
					ChainDraw = null;
				}

				internal bool BootUp(	Script_SpriteStudio6_RootEffect instanceRoot,
										Script_SpriteStudio6_DataEffect dataEffect,
										int idParts,
										int indexDataEmitter,
										int indexParent,
										ref Library_SpriteStudio6.Control.Effect controlEffect
									)
				{
					Status = FlagBitStatus.CLEAR;

					DataEffect = dataEffect;
					IDParts = idParts;
					IndexEmitter = indexDataEmitter;
					IndexParent = indexParent;

					/* Initialize Random */
					if(null == Random)
					{
						Random = Script_SpriteStudio6_RootEffect.InstanceCreateRandom();
					}

					SeedRandom = controlEffect.Seed;
					if(0 != (DataEffect.TableEmitter[IndexEmitter].FlagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.SEEDRANDOM))
					{	/* Overwrite Seed */
						/* MEMO: Overwritten to the Emitter's Seed. */
						SeedRandom = (uint)DataEffect.TableEmitter[IndexEmitter].SeedRandom + (uint)Library_SpriteStudio6.Data.Effect.Emitter.Constant.SEED_MAGIC;
					}
					else
					{
						if(0 != (DataEffect.FlagData & Script_SpriteStudio6_DataEffect.FlagBit.SEEDRANDOM_LOCK))
						{	/* Seed Locked */
							/* MEMO: Overwritten to the Effect's Seed. */
							SeedRandom = ((uint)DataEffect.SeedRandom + 1) * (uint)Library_SpriteStudio6.Data.Effect.Emitter.Constant.SEED_MAGIC;
						}
					}

					/* Get Data-Tables */
					TablePatternOffset = DataEffect.TableEmitter[IndexEmitter].TablePatternOffset;
					TablePatternEmit = DataEffect.TableEmitter[IndexEmitter].TablePatternEmit;
					TableSeedParticle = DataEffect.TableEmitter[IndexEmitter].TableSeedParticle;
					if(((null == TablePatternEmit) || (0 >= TablePatternEmit.Length)) || ((null == TableSeedParticle) || (0 >= TableSeedParticle.Length)))
					{	/* Calculate on Runtime ... Not Fixed Random-Seed */
						DataEffect.TableEmitter[IndexEmitter].TableGetPatternEmit(	ref TablePatternEmit,
																					ref TableSeedParticle,
																					Random,
																					SeedRandom
																				);
					}

					/* Create Draw-Temporary */
					ChainDraw = new Draw.Cluster.Chain();
					if(null == ChainDraw)
					{
						goto BootUp_ErrorEnd;
					}
//					ChainDraw.CleanUp();
					ChainDraw.BootUp();

					CoordinateTransformDraw = new Vector3[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2];
					if(null == CoordinateTransformDraw)
					{
						goto BootUp_ErrorEnd;
					}
					ColorVertexDraw = new Color32[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2];
					if(null == ColorVertexDraw)
					{
						goto BootUp_ErrorEnd;
					}

					/* Set Particle's UV */
					if(false == CellPresetParticle(instanceRoot, controlEffect.Masking))
					{
						goto BootUp_ErrorEnd;
					}

					/* Create Generating-Particle WorkArea */
					int countParticleMax = DataEffect.TableEmitter[IndexEmitter].CountParticleMax;
					TableActivityParticle = new Library_SpriteStudio6.Control.Effect.Particle.Activity[countParticleMax];
					for(int i=0; i<countParticleMax; i++)
					{
						TableActivityParticle[i].CleanUp();
					}

					/* Set Duration */
					Duration = DataEffect.TableEmitter[IndexEmitter].DurationEmitter + DataEffect.TableEmitter[IndexEmitter].Delay;

					Status |= (FlagBitStatus.VALID | FlagBitStatus.RUNNING);

					return(true);

				BootUp_ErrorEnd:;
					CleanUp();
					return(false);
				}

				internal bool Update(	float frame,
										Script_SpriteStudio6_RootEffect instanceRoot,
										ref Library_SpriteStudio6.Control.Effect controlEffect,
										int indexEmitterParent
									)
				{
					if((0 > IndexEmitter) || (0 > IDParts))
					{
						return(false);
					}

					/* Update Particles-Activity */
					int countParticle = TablePatternEmit.Length;
					int countOffset = TablePatternOffset.Length;
					int frameNow = (int)frame;
					uint slide = (0 > indexEmitterParent) ? 0 : (uint)ParticleTempolary.ID;
					slide = slide * (uint)Library_SpriteStudio6.Data.Effect.Emitter.Constant.SEED_MAGIC;
					uint indexSlide;
					for(int i=0; i<countOffset; i++)
					{
						indexSlide = ((uint)i + slide) % (uint)countParticle;
						TableActivityParticle[i].Update(	frameNow,
															ref this,
															ref DataEffect.TableEmitter[IndexEmitter],
															TablePatternEmit[i],
															TablePatternOffset[i],
															TablePatternEmit[indexSlide]
														);
					}

					/* Calculate & Draw Particles */
					countParticle = DataEffect.TableEmitter[IndexEmitter].CountParticleMax;
					for(int i=0; i<countParticle; i++)
					{
						if(false == Particle.Exec(	frameNow,
													i,
													instanceRoot,
													ref controlEffect,
													ref this,
													ref TableActivityParticle[i],
													indexEmitterParent,
													ref ParticleTempolary
												)
							)
						{	/* Draw-Limit Over */
							return(false);
						}
					}
					return(true);
				}

				internal bool UpdateSubEmitters(	float frame,
													Script_SpriteStudio6_RootEffect instanceRoot,
													ref Library_SpriteStudio6.Control.Effect controlEffect,
													int indexEmitterTarget,
													ref Emitter emitterTarget
												)
				{
					/* Update Particles-Activity */
					int countParticle = TablePatternEmit.Length;
					int countOffset = TablePatternOffset.Length;
					for(int i=0; i<countOffset; i++)
					{
						/* MEMO: Slide is always 0. */
						TableActivityParticle[i].Update(	(int)frame,
															ref this,
															ref DataEffect.TableEmitter[IndexEmitter],
															TablePatternEmit[i],
															TablePatternOffset[i],
															TablePatternEmit[i]
														);
					}

					/* Update Sub-Emitters */
					int frameTop;
					countParticle = DataEffect.TableEmitter[IndexEmitter].CountParticleMax;
					for(int i=0; i<countParticle; i++)
					{
						if(0 != (TableActivityParticle[i].Status & Library_SpriteStudio6.Control.Effect.Particle.Activity.FlagBitStatus.BORN))
						{
							/* MEMO: "ParticleTempolary" is parent's parameter. */
							frameTop = TableActivityParticle[i].FrameStart;
							emitterTarget.ParticleTempolary.FrameStart = frameTop;
							emitterTarget.ParticleTempolary.FrameEnd = TableActivityParticle[i].FrameEnd;
							emitterTarget.ParticleTempolary.ID = i;
							emitterTarget.ParticleTempolary.IDParent = 0;

							/* CAUTION: "ParticleTempolary" will be broken. */
							if(false == emitterTarget.Update(	(frame - (float)frameTop),
																instanceRoot,
																ref controlEffect,
																IndexEmitter
															)
								)
							{
								return(false);
							}
						}
					}

					return(true);
				}

				internal bool CellPresetParticle(Script_SpriteStudio6_RootEffect instanceRoot, Library_SpriteStudio6.KindMasking masking)
				{
					int indexCellMap = DataCellApply.IndexCellMap;
					int indexCell = DataCellApply.IndexCell;
					if((0 > indexCellMap) || (0 > indexCell))
					{
						indexCellMap = DataEffect.TableEmitter[IndexEmitter].IndexCellMap;
						indexCell = DataEffect.TableEmitter[IndexEmitter].IndexCell;
					}

					Status &= ~(	FlagBitStatus.CHANGE_CELL_UNREFLECTED
									| FlagBitStatus.REDECODE_MATERIAL
							);

					Library_SpriteStudio6.Data.CellMap dataCellMap = instanceRoot.DataGetCellMap(indexCellMap);
					if(null == dataCellMap)
					{
						goto CellPresetParticle_ErrorEnd;
					}
					else
					{
						if((0 > indexCell) || (dataCellMap.CountGetCell() <= indexCell))
						{
							goto CellPresetParticle_ErrorEnd;
						}
						else
						{
							/* MEMO: "Effect" uses only standard shaders. */
							DataCellApply.IndexCellMap = indexCellMap;
							DataCellApply.IndexCell = indexCell;
							MaterialDraw = instanceRoot.MaterialGet(	indexCellMap,
																		DataEffect.TableEmitter[IndexEmitter].OperationBlendTarget,
																		masking,
																		null,
																		true,
																		null,
																		null
																);

							float pivotXCell = dataCellMap.TableCell[indexCell].Pivot.x;
							float pivotYCell = dataCellMap.TableCell[indexCell].Pivot.y;
							float coordinateLUx = -pivotXCell;
							float coordinateLUy = pivotYCell;
							float coordinateRDx = dataCellMap.TableCell[indexCell].Rectangle.width - pivotXCell;
							float coordinateRDy = -(dataCellMap.TableCell[indexCell].Rectangle.height - pivotYCell);
							if(null == CoordinateDraw)
							{
								CoordinateDraw = new Vector3[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2];
								if(null == CoordinateDraw)
								{
									goto CellPresetParticle_ErrorEnd;
								}
							}
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LD].x = 
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LU].x = coordinateLUx;
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RU].y = 
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LU].y = coordinateLUy;
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RD].x = 
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RU].x = coordinateRDx;
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LD].y = 
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RD].y = coordinateRDy;
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LU].z = 
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RU].z = 
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.RD].z = 
							CoordinateDraw[(int)Library_SpriteStudio6.KindVertex.LD].z = 0.0f;

							float sizeXTexture = dataCellMap.SizeOriginal.x;
							float sizeYTexture = dataCellMap.SizeOriginal.y;
							coordinateLUx = dataCellMap.TableCell[indexCell].Rectangle.xMin / sizeXTexture;	/* L */
							coordinateRDx = dataCellMap.TableCell[indexCell].Rectangle.xMax / sizeXTexture;	/* R */
							coordinateLUy = (sizeYTexture - dataCellMap.TableCell[indexCell].Rectangle.yMin) / sizeYTexture;	/* U */
							coordinateRDy = (sizeYTexture - dataCellMap.TableCell[indexCell].Rectangle.yMax) / sizeYTexture;	/* D */
							if(null == UVTextureDraw)
							{
								UVTextureDraw = new Vector4[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2];
								if(null == UVTextureDraw)
								{
									goto CellPresetParticle_ErrorEnd;
								}
							}
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LU] = new Vector4(coordinateLUx, coordinateLUy, 0.0f, 0.0f);
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.LD] = new Vector4(coordinateLUx, coordinateRDy, 0.0f, 0.0f);
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RU] = new Vector4(coordinateRDx, coordinateLUy, 0.0f, 0.0f);
							UVTextureDraw[(int)Library_SpriteStudio6.KindVertex.RD] = new Vector4(coordinateRDx, coordinateRDy, 0.0f, 0.0f);

							if(null == UVMaxMinDraw)
							{
								UVMaxMinDraw = new Vector4[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2];
								if(null == UVMaxMinDraw)
								{
									goto CellPresetParticle_ErrorEnd;
								}
							}
							UVMaxMinDraw[(int)Library_SpriteStudio6.KindVertex.LU] = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
							UVMaxMinDraw[(int)Library_SpriteStudio6.KindVertex.LD] = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
							UVMaxMinDraw[(int)Library_SpriteStudio6.KindVertex.RU] = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
							UVMaxMinDraw[(int)Library_SpriteStudio6.KindVertex.RD] = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);

							if(null == UVAverageDraw)
							{
								UVAverageDraw = new Vector4[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2];
								if(null == UVAverageDraw)
								{
									goto CellPresetParticle_ErrorEnd;
								}
							}
							UVAverageDraw[(int)Library_SpriteStudio6.KindVertex.LU] = new Vector4(0.5f, 0.5f, 0.0f, 0.0f);
							UVAverageDraw[(int)Library_SpriteStudio6.KindVertex.LD] = new Vector4(0.5f, 0.5f, 0.0f, 0.0f);
							UVAverageDraw[(int)Library_SpriteStudio6.KindVertex.RU] = new Vector4(0.5f, 0.5f, 0.0f, 0.0f);
							UVAverageDraw[(int)Library_SpriteStudio6.KindVertex.RD] = new Vector4(0.5f, 0.5f, 0.0f, 0.0f);
						}
					}

					return(true);

				CellPresetParticle_ErrorEnd:;
					DataCellApply.CleanUp();
					MaterialDraw = null;
					return(false);
				}

				internal bool DrawParticle(Script_SpriteStudio6_RootEffect instanceRoot, ref Matrix4x4 matrixTransform, ref Color colorVertex)
				{
					ColorVertexDraw[(int)Library_SpriteStudio6.KindVertex.LU] = 
					ColorVertexDraw[(int)Library_SpriteStudio6.KindVertex.RU] = 
					ColorVertexDraw[(int)Library_SpriteStudio6.KindVertex.RD] = 
					ColorVertexDraw[(int)Library_SpriteStudio6.KindVertex.LD] = colorVertex;

					for(int i=0; i<(int)Library_SpriteStudio6.KindVertex.TERMINATOR2; i++)
					{
						CoordinateTransformDraw[i] = matrixTransform.MultiplyPoint3x4(CoordinateDraw[i]);
					}
					instanceRoot.ClusterDraw.VertexAdd(	ChainDraw,
														false,	/* MEMO: "Effect" is not support "Shader" attribute, so "Mesh-Batching" is always enabled. */
														MaterialDraw,
														ConstantShaderDummy,	/* null, */	/* No-Shader Constants */
														(int)Library_SpriteStudio6.KindVertex.TERMINATOR2,
														CoordinateTransformDraw,
														ColorVertexDraw,
														UVTextureDraw,
														UVMaxMinDraw,
														UVAverageDraw
													);

					return(true);
				}
				#endregion Functions

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				[System.Flags]
				internal enum FlagBitStatus
				{
					VALID = 0x40000000,
					RUNNING = 0x20000000,
					REDECODE_MATERIAL = 0x10000000,

					CHANGE_CELL_UNREFLECTED = 0x08000000,

					CLEAR = 0x00000000
				}

				private readonly static Library_SpriteStudio6.Control.Animation.Parts.BufferParameterSprite.BufferUniformShader ConstantShaderDummy = new Animation.Parts.BufferParameterSprite.BufferUniformShader();
				#endregion Classes, Structs & Interfaces
			}

			internal struct Particle
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				internal int ID;
				internal int IDParent;
				internal int FrameStart;
				internal int FrameEnd;

				internal float PositionX;
				internal float PositionY;
				internal float RotateZ;
				internal float Direction;

				internal Color ColorVertex;
				internal Vector3 Scale;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				internal void CleanUp()
				{
					ID = -1;
					IDParent = -1;
					FrameStart = -1;
					FrameEnd = -1;

//					PositionX = 
//					PositionY = 
//					RotateZ = 
//					Direction = 

//					ColorVertex = 
//					Scale = 
				}

				internal bool Exec(	float frame,
									int Index,
									Script_SpriteStudio6_RootEffect instanceRoot,
									ref Library_SpriteStudio6.Control.Effect controlEffect,
									ref Library_SpriteStudio6.Control.Effect.Emitter emitter,
									ref Activity activity,
									int indexEmitterParent,
									ref Particle particleParent
								)
				{	/* CAUTION: "calculateParent" will be broken. */
					Activity.FlagBitStatus flagStatusActivity = activity.Status;
					if(0 == (flagStatusActivity & Activity.FlagBitStatus.BORN))
					{
						return(true);
					}

					float frameTarget = frame;
					FrameStart = activity.FrameStart;
					FrameEnd = activity.FrameEnd;
					ID = Index + activity.Cycle;
					IDParent = (0 <= indexEmitterParent) ? particleParent.ID : 0;

					if(0 != (flagStatusActivity & Activity.FlagBitStatus.EXIST))
					{
						if(0 <= indexEmitterParent)
						{	/* Has Parent */
							particleParent.PositionX = particleParent.PositionY = 0.0f;

							particleParent.Calculate(	(FrameStart + particleParent.FrameStart),
														instanceRoot,
														ref controlEffect,
														ref controlEffect.TableEmitter[indexEmitterParent],
														ref controlEffect.TableEmitter[indexEmitterParent].DataEffect.TableEmitter[indexEmitterParent],
														false
													);
							emitter.Position.x = particleParent.PositionX;
							emitter.Position.y = particleParent.PositionY;
						}

						if(true == Calculate(	frameTarget,
												instanceRoot,
												ref controlEffect,
												ref emitter,
												ref emitter.DataEffect.TableEmitter[emitter.IndexEmitter],
												false
											)
							)
						{	/* Draw */
							controlEffect.CountParticleDraw++;
							if(controlEffect.LimitParticleDraw <= controlEffect.CountParticleDraw)
							{	/* Draw-Limit Over */
								return(false);
							}

							Vector2 scaleLayout = instanceRoot.DataEffect.ScaleLayout;
							Matrix4x4 matrixTransform = controlEffect.MatrixRoot * Matrix4x4.TRS(	new Vector3((PositionX * scaleLayout.x), (PositionY * scaleLayout.y), 0.0f),
																									Quaternion.Euler(0.0f, 0.0f, (RotateZ + Direction)),
																									Scale
																								);
							Color colorVertex = ColorVertex;
							colorVertex.a *= instanceRoot.RateOpacity;
							emitter.DrawParticle(instanceRoot, ref matrixTransform, ref colorVertex);
						}
					}

					return(true);
				}

				private bool Calculate(	float frame,
										Script_SpriteStudio6_RootEffect instanceRoot,
										ref Library_SpriteStudio6.Control.Effect controlEffect,
										ref Library_SpriteStudio6.Control.Effect.Emitter emitter,
										ref Library_SpriteStudio6.Data.Effect.Emitter dataEmitter,
										bool flagSimplicity = false
									)
				{
					Library_SpriteStudio6.Utility.Random.Generator random = emitter.Random;
					float frameRelative = (frame - (float)FrameStart);
					float framePower2 = frameRelative * frameRelative;
					float life = (float)(FrameEnd - FrameStart);
					if(0.0f >= life)	/* (0 == life) */
					{
						return(false);
					}

					float rateLife = frameRelative / life;
					long seedParticle = emitter.TableSeedParticle[ID % emitter.TableSeedParticle.Length];
					random.InitSeed((uint)(	(ulong)seedParticle
											+ (ulong)emitter.SeedRandom
											+ (ulong)IDParent
											+ (ulong)emitter.SeedOffset
										)
								);

					/* Calc Parameters */
					Library_SpriteStudio6.Data.Effect.Emitter.FlagBit flagData = dataEmitter.FlagData;

					float radianSub = dataEmitter.Angle.Sub * Mathf.Deg2Rad;
					float radian = random.RandomFloat(radianSub);
					radian = radian - (radianSub * 0.5f);
					radian += ((dataEmitter.Angle.Main + 90.0f) * Mathf.Deg2Rad);

					float speed = dataEmitter.Speed.Main + random.RandomFloat(dataEmitter.Speed.Sub);

					float radianOffset = 0;
					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.TANGENTIALACCELATION))
					{
						float accel = dataEmitter.RateTangentialAcceleration.Main + random.RandomFloat(dataEmitter.RateTangentialAcceleration.Sub);
						float speedTemp = speed;
						speedTemp = (0.0f >= speedTemp) ? 0.1f : speedTemp;
						radianOffset = (accel / (3.14f * (life * speedTemp * 0.2f))) * frameRelative;
					}

					float angleTemp = radian + radianOffset;
					float cos = Mathf.Cos(angleTemp);
					float sin = Mathf.Sin(angleTemp);
					float speedX = cos * speed;
					float speedY = sin * speed;
					float x = speedX * frameRelative;
					float y = speedY * frameRelative;
					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.SPEED_FLUCTUATION))
					{
						float speedFluctuation = dataEmitter.SpeedFluctuation.Main + random.RandomFloat(dataEmitter.SpeedFluctuation.Sub);
						float speedOffset = speedFluctuation / life;

						x = (((cos * speedOffset) * frameRelative) + speedX) * ((frameRelative + 1.0f) * 0.5f);
						y = (((sin * speedOffset) * frameRelative) + speedY) * ((frameRelative + 1.0f) * 0.5f);
					}

					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.GRAVITY_DIRECTION))
					{
						x += (0.5f * dataEmitter.GravityDirectional.x * framePower2);
						y += (0.5f * dataEmitter.GravityDirectional.y * framePower2);
					}
					float offsetX = 0.0f;
					float offsetY = 0.0f;
					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.POSITION))
					{
						offsetX = dataEmitter.Position.Main.x + random.RandomFloat(dataEmitter.Position.Sub.x);
						offsetY = dataEmitter.Position.Main.y + random.RandomFloat(dataEmitter.Position.Sub.y);
					}

					RotateZ = 0.0f;
					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.ROTATION))
					{
						RotateZ = dataEmitter.Rotation.Main + random.RandomFloat(dataEmitter.Rotation.Sub);

						float rotationFluctuation = dataEmitter.RotationFluctuation.Main + random.RandomFloat(dataEmitter.RotationFluctuation.Sub);
						if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.ROTATION_FLUCTUATION))
						{
							float frameLast = life * dataEmitter.RotationFluctuationRateTime;

							float rateRotationFluctuation = 0.0f;
							if(0.0f >= frameLast)	/* Minus??? */
							{
								RotateZ += (rotationFluctuation * dataEmitter.RotationFluctuationRate) * frameRelative;
							}
							else
							{
								rateRotationFluctuation = ((rotationFluctuation * dataEmitter.RotationFluctuationRate) - rotationFluctuation) / frameLast;

								float frameModuration = frameRelative - frameLast;
								frameModuration = (0.0f > frameModuration) ? 0.0f : frameModuration;

								float frameRelativeNow = frameRelative;
								frameRelativeNow = (frameRelativeNow > frameLast) ? frameLast : frameRelativeNow;

								float rotateOffsetTemp = rateRotationFluctuation * frameRelativeNow;
								rotateOffsetTemp += rotationFluctuation;
								float rotateOffset = (rotateOffsetTemp + rotationFluctuation) * (frameRelativeNow + 1.0f) * 0.5f;
								rotateOffset -= rotationFluctuation;
								rotateOffset += (frameModuration * rotateOffsetTemp);
								RotateZ += rotateOffset;
							}
						}
						else
						{
							RotateZ += (rotationFluctuation * frameRelative);
						}
					}

					/* ColorVertex/AlphaFade */
					ColorVertex.r =
					ColorVertex.g =
					ColorVertex.b =
					ColorVertex.a = 1.0f;

					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.COLORVERTEX))
					{
						ColorVertex.a = dataEmitter.ColorVertex.Main.a + random.RandomFloat(dataEmitter.ColorVertex.Sub.a);
						ColorVertex.r = dataEmitter.ColorVertex.Main.r + random.RandomFloat(dataEmitter.ColorVertex.Sub.r);
						ColorVertex.g = dataEmitter.ColorVertex.Main.g + random.RandomFloat(dataEmitter.ColorVertex.Sub.g);
						ColorVertex.b = dataEmitter.ColorVertex.Main.b + random.RandomFloat(dataEmitter.ColorVertex.Sub.b);
					}
					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.COLORVERTEX_FLUCTUATION))
					{
						Color ColorFluctuation;
						ColorFluctuation.a = dataEmitter.ColorVertexFluctuation.Main.a + random.RandomFloat(dataEmitter.ColorVertexFluctuation.Sub.a);
						ColorFluctuation.r = dataEmitter.ColorVertexFluctuation.Main.r + random.RandomFloat(dataEmitter.ColorVertexFluctuation.Sub.r);
						ColorFluctuation.g = dataEmitter.ColorVertexFluctuation.Main.g + random.RandomFloat(dataEmitter.ColorVertexFluctuation.Sub.g);
						ColorFluctuation.b = dataEmitter.ColorVertexFluctuation.Main.b + random.RandomFloat(dataEmitter.ColorVertexFluctuation.Sub.b);

						ColorVertex = Color.Lerp(ColorVertex, ColorFluctuation, rateLife);
					}

					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.FADEALPHA))
					{
						float rateStart = dataEmitter.AlphaFadeStart;
						float rateEnd = dataEmitter.AlphaFadeEnd;
						if(rateLife < rateStart)
						{
							ColorVertex.a *= (1.0f - ((rateStart - rateLife) / rateStart));
						}
						else
						{
							if(rateLife > rateEnd)
							{
								if(1.0f <= rateEnd)
								{
									ColorVertex.a = 0.0f;
								}
								else
								{
									float Alpha = (rateLife - rateEnd) / (1.0f - rateEnd);
									Alpha = (1.0f <= Alpha) ? 1.0f : Alpha;
									ColorVertex.a *= (1.0f - Alpha);
								}
							}
						}
					}

					/* Scale */
					Scale = Vector2.one;
					float scaleRate = 1.0f;

					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.SCALE_START))
					{
						Scale.x = dataEmitter.ScaleStart.Main.x + random.RandomFloat(dataEmitter.ScaleStart.Sub.x);
						Scale.y = dataEmitter.ScaleStart.Main.y + random.RandomFloat(dataEmitter.ScaleStart.Sub.y);
						scaleRate = dataEmitter.ScaleRateStart.Main + random.RandomFloat(dataEmitter.ScaleRateStart.Sub);
					}
					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.SCALE_END))
					{
						Vector3 scaleEnd;
						float scaleRateEnd;
						scaleEnd.x = dataEmitter.ScaleEnd.Main.x + random.RandomFloat(dataEmitter.ScaleEnd.Sub.x);
						scaleEnd.y = dataEmitter.ScaleEnd.Main.y + random.RandomFloat(dataEmitter.ScaleEnd.Sub.y);
						scaleEnd.z = 1.0f;
						scaleRateEnd = dataEmitter.ScaleRateEnd.Main + random.RandomFloat(dataEmitter.ScaleRateEnd.Sub);

						Scale = Vector3.Lerp(Scale, scaleEnd, rateLife);
						scaleRate = Mathf.Lerp(scaleRate, scaleRateEnd, rateLife);
					}
					Scale *= scaleRate;
					Scale.z = 1.0f;	/* Overwrite, force */

					/* Position/Gravity */
					PositionX = x + (emitter.Position.x + offsetX);
					PositionY = y + (emitter.Position.y + offsetY);

					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.GRAVITY_POINT))
					{
						Vector2 vectorPosition;
						float positionXGravity = dataEmitter.GravityPointPosition.x;
						float positionYGravity = dataEmitter.GravityPointPosition.y;
//						vectorPosition.x = positionXGravity - (offsetX + PositionX);
//						vectorPosition.y = positionYGravity - (offsetY + PositionY);
						vectorPosition.x = positionXGravity - (offsetX + emitter.Position.x);
						vectorPosition.y = positionYGravity - (offsetY + emitter.Position.y);
						Vector2 vectorNormal = vectorPosition.normalized;
						float gravityPower = dataEmitter.GravityPointPower;
						if(0.0f < gravityPower)
						{
							float eFrame = (vectorPosition.magnitude / gravityPower) * 0.9f;
							float gFrame = (frameRelative >= (int)eFrame) ? (eFrame * 0.9f) : frameRelative;

//							vectorNormal = vectorNormal * gravityPower * gFrame;
							vectorNormal *= gravityPower * gFrame;
							PositionX += vectorNormal.x;
							PositionY += vectorNormal.y;

							float Blend = OutQuad(gFrame, eFrame, 0.9f, 0.0f);
							Blend += (frameRelative / life * 0.1f);

							PositionX = PositionX + ((positionXGravity - PositionX) * Blend);	/* CAUTION!: Don't use "Mathf.Lerp" */
							PositionY = PositionY + ((positionYGravity - PositionY) * Blend);	/* CAUTION!: Don't use "Mathf.Lerp" */
						}
						else
						{
							/* MEMO: In the case negative power, Simply repulsion. Attenuation due to distance is not taken into account. */
//							vectorNormal = vectorNormal * gravityPower * frameRelative;
							vectorNormal *= gravityPower * frameRelative;
							PositionX += vectorNormal.x;
							PositionY += vectorNormal.y;
						}
					}

					/* Turn-Direction */
					Direction = 0.0f;
					if((0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.TURNDIRECTION)) && (false == flagSimplicity))
					{
						emitter.ParticleTempolary2 = this;
						emitter.ParticleTempolary2.Calculate(	(frame + 1.0f),	/* (Frame + 0.1f), */
																instanceRoot,
																ref controlEffect,
																ref emitter,
																ref emitter.DataEffect.TableEmitter[emitter.IndexEmitter],
																true
															);
						float radianDirection = AngleGetCCW(	new Vector2(1.0f, 0.0f),
																new Vector2((PositionX - emitter.ParticleTempolary2.PositionX), (PositionY - emitter.ParticleTempolary2.PositionY))
															);
						Direction = (radianDirection * Mathf.Rad2Deg) + 90.0f + dataEmitter.TurnDirectionFluctuation;
					}

					return (true);
				}
				private static float OutQuad(float time, float timeFull, float valueMax, float valueMin)
				{
					if(0.0f >= timeFull)
					{
						return(0.0f);
					}
					if(time > timeFull)
					{
						time = timeFull;
					}

					valueMax -= valueMin;
					time /= timeFull;
					return(-valueMax * time * (time - 2.0f) + valueMin);
				}
				private static float AngleGetCCW(Vector2 start, Vector2 end)
				{
					Vector2 startNormalized = start.normalized;
					Vector2 endNormalized = end.normalized;

					float dot = Vector2.Dot(startNormalized, endNormalized);
					dot = Mathf.Clamp(dot, -1.0f, 1.0f);

					float angle = Mathf.Acos(dot);
					float cross = (startNormalized.x * endNormalized.y) - (endNormalized.x * startNormalized.y);
					angle = (0.0f > cross) ? ((2.0f * Mathf.PI) - angle) : angle;

					return(angle);
				}
				#endregion Functions

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				internal struct Activity
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					internal FlagBitStatus Status;
					internal int Cycle;
					internal int FrameStart;
					internal int FrameEnd;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					internal void CleanUp()
					{
						Status = FlagBitStatus.CLEAR;
						Cycle = -1;
						FrameStart = -1;
						FrameEnd = -1;
					}

					internal void Update(	int frame,
											ref Library_SpriteStudio6.Control.Effect.Emitter emitter,
											ref Library_SpriteStudio6.Data.Effect.Emitter dataEmitter,
											Library_SpriteStudio6.Data.Effect.Emitter.PatternEmit patternEmit,
											int patternOffset,
											Library_SpriteStudio6.Data.Effect.Emitter.PatternEmit patternEmitTarget
										)
					{
						Status &= ~(FlagBitStatus.BORN | FlagBitStatus.EXIST);

						int duration = emitter.Duration;
						int durationTarget = patternEmitTarget.Duration;
						int frameNow = (int)(frame - patternOffset);
						int cycleTarget = patternEmitTarget.Cycle;
						if(0 != cycleTarget)
						{
							int countLoop = frameNow / cycleTarget;
							int cycleTop = countLoop * cycleTarget;

							Cycle = countLoop;
							FrameStart = cycleTop + patternOffset;
							FrameEnd = FrameStart +	durationTarget;

							if((frame >= FrameStart) && (frame < FrameEnd))
							{
								Status |= (FlagBitStatus.BORN | FlagBitStatus.EXIST);
							}

							if(0 == (dataEmitter.FlagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.EMIT_INFINITE))
							{
								if(FrameStart >= duration)
								{
									Status &= ~FlagBitStatus.EXIST;

									FrameStart = ((duration - patternOffset) / cycleTarget) * cycleTarget + patternOffset;
									FrameEnd = FrameStart + durationTarget;
									Status &= ~FlagBitStatus.BORN;
								}
								else
								{
									Status |= FlagBitStatus.BORN;
								}
							}

							if(0 > frameNow)
							{
								Status &= ~(FlagBitStatus.BORN | FlagBitStatus.EXIST);
							}
						}
					}
					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					[System.Flags]
					internal enum FlagBitStatus
					{
						EXIST = 0x40000000,	/* RUNNING */
						BORN = 	0x20000000, /* GETUP */

						CLEAR = 0x00000000,
					}
					#endregion Enums & Constants
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
