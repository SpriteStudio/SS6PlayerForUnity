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
		public partial struct Effect
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			internal FlagBitStatus Status;

			internal Emitter[] TableEmitter;
			internal int CountMeshParticle;

			internal Script_SpriteStudio6_RootEffect InstanceRootEffect;
			internal int EffectDurationFull;

			internal uint Seed;
			internal uint SeedOffset;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			internal void CleanUp()
			{
				Status = FlagBitStatus.CLEAR;

				TableEmitter = null;
				CountMeshParticle = 0;

				InstanceRootEffect = null;
				EffectDurationFull = -1;

				Seed = 0;
				SeedOffset = 0;
			}

			internal void ParticleReset()	/* ?????????? */
			{
				CountMeshParticle = 0;
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
				int frameGlobal;
				EffectDurationFull = 0;
				for (int i=0; i<countEmitter; i++)
				{
					indexEmitterParent = TableEmitter[i].IndexParent;
					frameGlobal = TableEmitter[i].FrameFull;
					if(0 < indexEmitterParent)
					{	/* Sub-Emitters */
						frameGlobal += TableEmitter[indexEmitterParent].FrameFull;
					}
					TableEmitter[i].FrameGlobal = frameGlobal;

					EffectDurationFull = (frameGlobal > EffectDurationFull) ? frameGlobal : EffectDurationFull;
				}

				Status |= FlagBitStatus.RUNNING;

				return(true);

			BootUp_ErrorEnd:;
				CleanUp();
				return(false);
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

				internal int IndexCellMapParticle;
				internal int IndexCellParticle;
				internal int IndexCellMapOverwrite;
				internal int IndexCellOverwrite;
				internal Vector3[] CoordinateMeshParticle;
				internal Vector2[] UVMeshParticle;

				internal Library_SpriteStudio6.Utility.Random.Generator InstanceRandom;
				internal Library_SpriteStudio6.Data.Effect.Emitter.PatternEmit[] TablePatternEmit;
				internal int[] TablePatternOffset;
				internal long[] TableSeedParticle;

				internal uint SeedRandom;
				internal uint SeedOffset;

				internal int Duration;
				internal Vector2 Position;
				internal int FrameGlobal;

				internal Library_SpriteStudio6.Control.Effect.Particle.PatternGenerate[] TablePatternGenerate;

//				internal ParameterParticle InstanceParameterParticleTempolary2;	/* (mainly) for TurnToDirection (mainly) */
//				internal ParameterParticle InstanceParameterParticleTempolary;	/* (mainly) for Parent */
//				internal ParameterParticle InstanceParameterParticle;

				internal int FrameFull
				{
					get
					{
						return(DataEffect.TableEmitter[IndexEmitter].DurationEmitter + (int)(DataEffect.TableEmitter[IndexEmitter].DurationParticle.Main + DataEffect.TableEmitter[IndexEmitter].DurationParticle.Sub));
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

					IndexCellMapParticle = -1;
					IndexCellParticle = -1;
					if(0 == (Status & FlagBitStatus.OVERWRITE_CELL_IGNOREATTRIBUTE))
					{
						IndexCellMapOverwrite = -1;
						IndexCellOverwrite = -1;
					}
					CoordinateMeshParticle = null;
					UVMeshParticle = null;

					InstanceRandom = null;
					TablePatternEmit = null;
					TableSeedParticle = null;

					SeedRandom = (uint)Library_SpriteStudio6.Data.Effect.Emitter.Constant.SEED_MAGIC;
					SeedOffset = 0;

					Duration = 0;
					Position = Vector2.zero;
					FrameGlobal = 0;

					TablePatternGenerate = null;
				}

				internal bool BootUp(	Script_SpriteStudio6_RootEffect instanceRoot,
										Script_SpriteStudio6_DataEffect dataEffect,
										int idParts,
										int indexDataEmitter,
										int indexParent,
										ref Library_SpriteStudio6.Control.Effect instanceControl
									)
				{
					Status = FlagBitStatus.CLEAR;

					DataEffect = dataEffect;
					IDParts = idParts;
					IndexEmitter = indexDataEmitter;
					IndexParent = indexParent;

					/* Initialize Random */
					if(null == InstanceRandom)
					{
						InstanceRandom = Script_SpriteStudio6_RootEffect.InstanceCreateRandom();
					}

					Library_SpriteStudio6.Data.Effect.Emitter.FlagBit flagData = DataEffect.TableEmitter[IndexEmitter].FlagData;

					SeedRandom = instanceControl.Seed;
					if(0 != (flagData & Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.SEEDRANDOM))
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
																					InstanceRandom,
																					SeedRandom
																				);
					}

					/* Calculate Particle's UV */
//					ParticleCellSet(instanceRoot);

					/* Create Generating-Particle WorkArea */
					int countParticleMax = DataEffect.TableEmitter[IndexEmitter].CountParticleMax;
					TablePatternGenerate = new Library_SpriteStudio6.Control.Effect.Particle.PatternGenerate[countParticleMax];
					for(int i=0; i<countParticleMax; i++)
					{
						TablePatternGenerate[i].CleanUp();
					}

					/* Set Duration */
					Duration = DataEffect.TableEmitter[IndexEmitter].DurationEmitter + DataEffect.TableEmitter[IndexEmitter].Delay;

					Status |= (FlagBitStatus.VALID | FlagBitStatus.RUNNING);

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

					OVERWRITE_CELL_UNREFLECTED = 0x00080000,
					OVERWRITE_CELL_IGNOREATTRIBUTE = 0x00040000,

					CLEAR = 0x00000000
				}
				#endregion Classes, Structs & Interfaces
			}

			internal static class Particle
			{
				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				internal struct PatternGenerate
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					internal FlagBitStatus Status;
					internal int ID;
					internal int Cycle;
					internal int FrameStart;
					internal int FrameEnd;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					internal void CleanUp()
					{
						Status = FlagBitStatus.CLEAR;
						ID = -1;
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
							FrameEnd = FrameStart + durationTarget;
								
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
