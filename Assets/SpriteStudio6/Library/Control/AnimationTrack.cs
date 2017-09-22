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
			internal partial struct Track
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				internal KindMode Mode;
				internal FlagBitStatus Status;
				internal bool StatusIsPlaying
				{
					get
					{
						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING)));
					}
				}
				internal bool StatusIsPausing
				{
					get
					{
						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PAUSING) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PAUSING)));
					}
				}

				internal Library_SpriteStudio6.CallBack.FunctionControlEndTrackPlay FunctionPlayEnd;

				internal Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer ArgumentContainer;

				internal float TimeDelay;
				internal float TimeElapsed;
				internal float RateTime;

				internal int TimesPlay;
				internal int TimesPlayNow;
				internal int CountLoop;
				internal int CountLoopNow;

				internal int FramePerSecond;
				internal float TimePerFrame;
				internal float TimePerFrameConsideredRateTime;

				internal int FrameStart;
				internal int FrameEnd;
				internal int FrameRange;
				internal float TimeRange;

				internal float RateBlend;
				internal float RateBlendNormalized;

				internal int IndexTrackSlave;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Mode = KindMode.NORMAL;
					Status = FlagBitStatus.CLEAR;

					FunctionPlayEnd = null;

					ArgumentContainer.CleanUp();

					TimeDelay = 0.0f;
					TimeElapsed = 0.0f;
					RateTime = 1.0f;

					TimesPlay = -1;
					TimesPlayNow = -1;
					CountLoop = -1;
					CountLoopNow = -1;

					FramePerSecond = 60;
					TimePerFrame = 0.0f;
					TimePerFrameConsideredRateTime = 0.0f;

					FrameStart = -1;
					FrameEnd = -1;
					FrameRange = 0;
					TimeRange = 0.0f;

					RateBlend = 1.0f;
					RateBlendNormalized = 1.0f;

					IndexTrackSlave = -1;
				}

				public bool BootUp()
				{
					CleanUp();

					Status |= FlagBitStatus.VALID;

					return(true);
				}

				public bool Start(	Script_SpriteStudio6_Root instanceRoot,
									int indexAnimation,
									int frameRangeStart,
									int frameRangeEnd,
									int frame,
									int framePerSecond,
									float rateTime,
									float timeDelay,
									bool flagPingpong,
									int timesPlay
								)
									
				{
					/* Check booted-up */
					if(0 == (Status & FlagBitStatus.VALID))
					{
						if(false == BootUp())
						{
							return(false);
						}
					}

					/* Reset Animation */
					ArgumentContainer.DataAnimation = instanceRoot.DataAnimation;
					ArgumentContainer.IndexAnimation = indexAnimation;

					/* Set datas */
					Status &= FlagBitStatus.VALID;	/* Clear */

					Status = (true == flagPingpong) ? (Status | FlagBitStatus.STYLE_PINGPONG) : (Status & ~FlagBitStatus.STYLE_PINGPONG); 
					RateTime = rateTime;
					if(0.0f > RateTime)
					{
						Status = (0 == (Status & FlagBitStatus.STYLE_REVERSE)) ? (Status | FlagBitStatus.STYLE_REVERSE) : (Status & ~FlagBitStatus.STYLE_REVERSE);
						RateTime *= -1.0f;
					}
					FramePerSecond = framePerSecond;
					TimePerFrame = 1.0f / (float)FramePerSecond;

					FrameStart = frameRangeStart;
					FrameEnd = frameRangeEnd;
					ArgumentContainer.Frame = frame;
					if(0 != (Status & FlagBitStatus.STYLE_REVERSE))
					{	/* Play-Reverse */
						Status |= FlagBitStatus.PLAYING_REVERSE;
						ArgumentContainer.Frame = (ArgumentContainer.Frame <= FrameStart) ? (FrameEnd + 1) : ArgumentContainer.Frame;
					}
					else
					{	/* Play-Normal */
						Status &= ~FlagBitStatus.PLAYING_REVERSE;
						ArgumentContainer.Frame = (ArgumentContainer.Frame >= (FrameEnd + 1)) ? (FrameStart - 1) : ArgumentContainer.Frame;
					}

					TimesPlay = timesPlay;
					TimeDelay = timeDelay;

					CountLoop = 0;
					TimeElapsed = (ArgumentContainer.Frame - FrameStart) * TimePerFrame;
					ArgumentContainer.Frame = Mathf.Clamp(ArgumentContainer.Frame, FrameStart, FrameEnd);

					Status |= FlagBitStatus.PLAYING;
					Status |= FlagBitStatus.PLAYING_START;

					FrameRange = (FrameEnd - FrameStart) + 1;
					TimeRange = (float)FrameRange * TimePerFrame;
					TimePerFrameConsideredRateTime = TimePerFrame * RateTime;

					return(true);
				}

				public bool Stop(bool flagJumpEnd)
				{
					if(0 == (Status & FlagBitStatus.VALID))
					{
						return(false);
					}

					if(0 == (Status & FlagBitStatus.PLAYING))
					{
						return(true);
					}

					if(true == flagJumpEnd)
					{
					}

					return(true);
				}

				public bool Pause(bool flagSwitch)
				{
					if(0 == (Status & FlagBitStatus.VALID))
					{
						return(false);
					}

					if(true == flagSwitch)
					{
						Status |= FlagBitStatus.PAUSING;
					}
					else
					{
						Status &= ~FlagBitStatus.PAUSING;
					}

					return(true);
				}

				public bool Update(float timeElapsed)
				{
					timeElapsed *= RateTime;

					if((FlagBitStatus.VALID | FlagBitStatus.PLAYING) != (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING)))
					{	/* Not-Playing */
						return(false);
					}
					if((0 != (Status & FlagBitStatus.PAUSING)) && (0 == (Status & FlagBitStatus.PLAYING_START)))
					{	/* Play & Pausing (Through, Right-After-Starting) */
						return(true);
					}
					if(0.0f > TimeDelay)
					{	/* Wait Infinite */
						TimeDelay = -1.0f;
						Status &= ~(FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);	/* Cancel Start & Decoding Attribute */
						return(true);
					}
					else
					{	/* Wait Limited-Time */
						if(0.0f < TimeDelay)
						{	/* Waiting */
							TimeDelay -= timeElapsed;
							if(0.0f < TimeDelay)
							{
								Status &= ~(FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);	/* Cancel Start & Decoding Attribute */
								return(true);
							}

							/* Start */
							timeElapsed += -TimeDelay * ((0 == (Status & FlagBitStatus.PLAYING_REVERSE)) ? 1.0f : -1.0f);
							TimeDelay = 0.0f;
							ArgumentContainer.FramePrevious = -1;
							Status |= (FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);
						}
					}
					if(0 != (Status & FlagBitStatus.PLAYING_START))
					{	/* Play & Right-After-Starting */
						Status |= (FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);
						timeElapsed = 0.0f;
						goto AnimationUpdate_End;	/* Display the first frame, force */
					}

					/* Calculate New-Frame */
					ArgumentContainer.FramePrevious = ArgumentContainer.Frame;
					bool FlagRangeOverPrevious = false;
					if(0 != (Status & FlagBitStatus.STYLE_PINGPONG))
					{	/* Play-Style: PingPong */
						if(0 != (Status & FlagBitStatus.PLAYING_REVERSE))
						{
							FlagRangeOverPrevious = (0.0f > TimeElapsed) ? true : false;
							TimeElapsed -= timeElapsed;
							if((0.0f > TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto AnimationUpdate_End;
							}
						}
						else
						{
							FlagRangeOverPrevious = (TimeRange <= TimeElapsed) ? true : false;
							TimeElapsed += timeElapsed;
							if((TimeRange <= TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto AnimationUpdate_End;
							}
						}

						if(0 != (Status & FlagBitStatus.STYLE_REVERSE))
						{	/* Play-Style: PingPong & Reverse */
							while((TimeRange <= TimeElapsed) || (0.0f > TimeElapsed))
							{
								if(0 != (Status & FlagBitStatus.PLAYING_REVERSE))
								{	/* Now: Reverse */
									if(TimeRange <= TimeElapsed)
									{	/* MEMO: Follow "FlagRangeOverPrevious" */
										break;
									}
									if(0.0f > TimeElapsed)
									{	/* Frame-Over: Turn */
										TimeElapsed += TimeRange;
										TimeElapsed = TimeRange - TimeElapsed;
										Status |= FlagBitStatus.PLAYING_TURN;
										Status &= ~FlagBitStatus.PLAYING_REVERSE;
									}
								}
								else
								{   /* Now: Foward */
									if(true == FlagRangeOverPrevious)
									{
										FlagRangeOverPrevious = false;
										Status |= FlagBitStatus.PLAYING_TURN;
										break;
									}
									else
									{
										CountLoop++;
										if(TimeRange <= TimeElapsed)
										{	/* Frame-Over: Loop/End */
											if(0 < TimesPlayNow)
											{	/* Limited-Count Loop */
												TimesPlayNow--;
												if(0 >= TimesPlayNow)
												{	/* End */
													goto AnimationUpdate_PlayEnd_Foward;
												}
											}

											/* Not-End */
											TimeElapsed -= TimeRange;
											TimeElapsed = TimeRange - TimeElapsed;
											Status |= FlagBitStatus.PLAYING_REVERSE;
											Status |= FlagBitStatus.PLAYING_TURN;
											CountLoopNow++;
										}
									}
								}
							}
						}
						else
						{	/* Play-Style: PingPong & Foward */
							while((TimeRange <= TimeElapsed) || (0.0f > TimeElapsed))
							{
								if(0 != (Status & FlagBitStatus.PLAYING_REVERSE))
								{	/* Now: Reverse */
									if(true == FlagRangeOverPrevious)
									{
										FlagRangeOverPrevious = false;
										Status |= FlagBitStatus.PLAYING_TURN;
										break;
									}
									else
									{
										CountLoop++;
										if(0.0f > TimeElapsed)
										{	/* Frame-Over: Loop/End */
											if(0 < TimesPlayNow)
											{	/* Limited-Count Loop */
												TimesPlayNow--;
												if(0 >= TimesPlayNow)
												{	/* End */
													goto AnimationUpdate_PlayEnd_Reverse;
												}
											}

											/* Not-End */
											TimeElapsed += TimeRange;
											TimeElapsed = TimeRange - TimeElapsed;
											Status &= ~FlagBitStatus.PLAYING_REVERSE;
											Status |= FlagBitStatus.PLAYING_TURN;
											CountLoopNow++;
										}
									}
								}
								else
								{	/* Now: Foward */
									if(0.0f > TimeElapsed)
									{	/* MEMO: Follow "FlagRangeOverPrevious" */
										break;
									}
									if(TimeRange <= TimeElapsed)
									{	/* Frame-Over: Turn */
										TimeElapsed -= TimeRange;
										TimeElapsed = TimeRange - TimeElapsed;
										Status |= FlagBitStatus.PLAYING_TURN;
										Status |= FlagBitStatus.PLAYING_REVERSE;
									}
								}
							}
						}
					}
					else
					{	/* Play-Style: OneWay */
						if(0 != (Status & FlagBitStatus.STYLE_REVERSE))
						{	/* Play-Style: OneWay & Reverse */
							FlagRangeOverPrevious = (0.0f > TimeElapsed) ? true : false;
							TimeElapsed -= timeElapsed;
							if((0.0f > TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto AnimationUpdate_End;
							}

							while(0.0f > TimeElapsed)
							{
								TimeElapsed += TimeRange;
								if(true == FlagRangeOverPrevious)
								{
									FlagRangeOverPrevious = false;
									Status |= FlagBitStatus.PLAYING_TURN;
								}
								else
								{
									CountLoop++;
									if(0 < TimesPlayNow)
									{	/* Limited-Count Loop */
										TimesPlayNow--;
										if(0 >= TimesPlayNow)
										{	/* End */
											goto AnimationUpdate_PlayEnd_Reverse;
										}
									}

									/* Not-End */
									CountLoopNow++;
									Status |= FlagBitStatus.PLAYING_TURN;
								}
							}
						}
						else
						{	/* Play-Style: OneWay & Foward */
							FlagRangeOverPrevious = (TimeRange <= TimeElapsed) ? true : false;
							TimeElapsed += timeElapsed;
							if((TimeRange <= TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto AnimationUpdate_End;
							}

							while(TimeRange <= TimeElapsed)
							{
								TimeElapsed -= TimeRange;
								if(true == FlagRangeOverPrevious)
								{
									FlagRangeOverPrevious = false;
									Status |= FlagBitStatus.PLAYING_TURN;
								}
								else
								{
									CountLoop++;
									if(0 < TimesPlayNow)
									{	/* Limited-Count Loop */
										TimesPlayNow--;
										if(0 >= TimesPlayNow)
										{	/* End */
											goto AnimationUpdate_PlayEnd_Foward;
										}
									}

									/* Not-End */
									Status |= FlagBitStatus.PLAYING_TURN;
									CountLoopNow++;
								}
							}
						}
					}

				AnimationUpdate_End:;
					ArgumentContainer.Frame = (int)(TimeElapsed / TimePerFrame);
					ArgumentContainer.Frame = Mathf.Clamp(ArgumentContainer.Frame, 0, (FrameRange - 1));
					ArgumentContainer.Frame += FrameStart;
					if((ArgumentContainer.Frame != ArgumentContainer.FramePrevious) || (0 != (Status & FlagBitStatus.PLAYING_TURN)))
					{
						Status |= FlagBitStatus.DECODE_ATTRIBUTE;
					}
					return(true);

				AnimationUpdate_PlayEnd_Foward:;
					TimesPlayNow = 0;	/* Clip */
					Status |= (FlagBitStatus.REQUEST_PLAYEND | FlagBitStatus.DECODE_ATTRIBUTE);
					TimeElapsed = TimeRange;
					ArgumentContainer.Frame = FrameEnd;
					return(true);

				AnimationUpdate_PlayEnd_Reverse:;
					TimesPlayNow = 0;	/* Clip */
					Status |= (FlagBitStatus.REQUEST_PLAYEND | FlagBitStatus.DECODE_ATTRIBUTE);
					TimeElapsed = 0.0f;
					ArgumentContainer.Frame = FrameStart;
					return(true);
				}

				/* MEMO: Originally should be function, but call-cost is high(taking processing content into account). */
				/*       Processed directly in "Script_SpriteStudio6_Root".                                            */
//				public void StatusClearTransient()
//				{
//					if((FlagBitStatus.VALID | FlagBitStatus.PLAYING) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING)))
//					{	/* Not-Playing */
//						Status &= ~(FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);
//					}
//				}

				public void TimeElapse(float time, bool flagReverseParent, bool flagRangeEnd)
				{	/* MEMO: In principle, This Function is for calling from "UpdateInstance". */
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				[System.Flags]
				internal enum FlagBitStatus
				{
					VALID = 0x40000000,
					PLAYING = 0x20000000,
					PAUSING = 0x10000000,

					STYLE_PINGPONG = 0x08000000,
					STYLE_REVERSE = 0x04000000,

					PLAYING_START = 0x00800000,
					PLAYING_REVERSE = 0x00400000,
					PLAYING_REVERSEPREVIOUS = 0x00200000,
					PLAYING_TURN = 0x00100000,

					DECODE_ATTRIBUTE = 0x00080000,

					IGNORE_USERDATA = 0x00008000,
					IGNORE_SKIPLOOP = 0x00004000,

					REQUEST_PLAYEND = 0x00000080,

					CLEAR = 0x00000000,
				}

				internal enum KindMode
				{
					NORMAL = 0,
					SLAVE,	/* Fade destination when bridging animation */
				}
				#endregion Enums & Constants
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
