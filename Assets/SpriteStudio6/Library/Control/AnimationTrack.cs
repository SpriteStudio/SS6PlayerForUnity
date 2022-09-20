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
		public static partial class Animation
		{
			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public partial struct Track
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				internal FlagBitStatus Status;
				public bool StatusIsValid
				{
					get
					{
						return(0 != (Status & FlagBitStatus.VALID));
					}
				}
				public bool StatusIsPlaying
				{
					get
					{
						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING)));
					}
				}
				public bool StatusIsPausing
				{
					get
					{
						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PAUSING) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PAUSING)));
					}
				}
				public bool StatusIsPlayStylePingpong
				{
					get
					{
						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.STYLE_PINGPONG) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.STYLE_PINGPONG)));
					}
				}
				public bool StatusIsPlayStyleReverse
				{
					get
					{
						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.STYLE_REVERSE) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.STYLE_REVERSE)));
					}
				}
				public bool StatusIsPlayingStart
				{
					get
					{
						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PLAYING_START) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PLAYING_START)));
					}
				}
				public bool StatusIsPlayingReverse
				{
					get
					{
						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PLAYING_REVERSE) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PLAYING_REVERSE)));
					}
				}
				public bool StatusIsPlayingReversePrevious
				{
					get
					{
//						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PLAYING_REVERSEPREVIOUS) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PLAYING_REVERSEPREVIOUS)));
						return(0 != (Status & FlagBitStatus.PLAYING_REVERSEPREVIOUS));
					}
				}
				public bool StatusIsPlayingTurn
				{
					get
					{
						return((FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PLAYING_TURN) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING | FlagBitStatus.PLAYING_TURN)));
					}
				}
				public bool StatusIsDecodeAttribute
				{
					get
					{
						return(0 != (Status & FlagBitStatus.DECODE_ATTRIBUTE));
					}
				}
				public bool StatusIsIgnoreSkipLoop
				{
					get
					{
						return(0 != (Status & FlagBitStatus.IGNORE_SKIPLOOP));
					}
				}
				public bool StatusIsIgnoreUserData
				{
					get
					{
						return(0 != (Status & FlagBitStatus.IGNORE_USERDATA));
					}
				}
				public bool StatusIsIgnoreSignal
				{
					get
					{
						return(0 != (Status & FlagBitStatus.IGNORE_SIGNAL));
					}
				}
				public bool StatusIsIgnoreNextUpdateUserData
				{
					get
					{
						return(0 != (Status & FlagBitStatus.IGNORE_NEXTUPDATE_USERDATA));
					}
				}
				public bool StatusIsIgnoreNextUpdateSignal
				{
					get
					{
						return(0 != (Status & FlagBitStatus.IGNORE_NEXTUPDATE_SIGNAL));
					}
				}
				public bool StatusIsTransitionStart
				{
					get
					{
						return(0 != (Status & FlagBitStatus.TRANSITION_START));
					}
					set
					{
						if(true == value)
						{
							Status |= FlagBitStatus.TRANSITION_START;
						}
						else
						{
							Status &= ~FlagBitStatus.TRANSITION_START;
						}
					}
				}
				public bool StatusIsTransitionCancelPause
				{
					get
					{
						return(0 != (Status & FlagBitStatus.TRANSITION_CANCEL_PAUSE));
					}
					set
					{
						if(true == value)
						{
							Status |= FlagBitStatus.TRANSITION_CANCEL_PAUSE;
						}
						else
						{
							Status &= ~FlagBitStatus.TRANSITION_CANCEL_PAUSE;
						}
					}
				}
				public bool StatusIsRequestPlayEnd
				{
					get
					{
						return(0 != (Status & FlagBitStatus.REQUEST_PLAYEND));
					}
					set
					{
						if(true == value)
						{
							Status |= FlagBitStatus.REQUEST_PLAYEND;
						}
						else
						{
							Status &= ~FlagBitStatus.REQUEST_PLAYEND;
						}
					}
				}
				public bool StatusIsRequestTransitionEnd
				{
					get
					{
						return(0 != (Status & FlagBitStatus.REQUEST_TRANSITIONEND));
					}
					set
					{
						if(true == value)
						{
							Status |= FlagBitStatus.REQUEST_TRANSITIONEND;
						}
						else
						{
							Status &= ~FlagBitStatus.REQUEST_TRANSITIONEND;
						}
					}
				}

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer ArgumentContainer;

				public float TimeDelay;
				public float TimeElapsed;
				public float TimeElapsedNow;
				public float RateTime;

				public int TimesPlay;
				public int CountLoop;
				public int CountLoopNow;

				public int FramePerSecond;
				public float TimePerFrame;
				public float TimePerFrameConsideredRateTime;

				public int FrameStart;
				public int FrameEnd;
				public int FrameRange;
				public float TimeRange;

				internal float TimeElapsedTransition;
				internal float TimeLimitTransition;
				internal float RateTransition;
				internal int IndexTrackSecondary;

				/* MEMO: This value is applicable even in a stopping.                            */
				/*       Mainly used for shifting display-frame in cycle that executed "Stop()". */
				internal float TimeElapseReplacement;
				internal float TimeElapseInRangeReplacement;

				/* MEMO: This value is used for "Sequence".                        */
				/*       The overrun is added to the start time of next animation. */
				internal float TimeOverrun;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Status = FlagBitStatus.CLEAR;

					ArgumentContainer.CleanUp();

					TimeDelay = 0.0f;
					TimeElapsed = 0.0f;
					RateTime = 1.0f;
					TimeElapsedNow = 0.0f;

					TimesPlay = -1;
					CountLoop = -1;
					CountLoopNow = -1;

					FramePerSecond = 60;
					TimePerFrame = 0.0f;
					TimePerFrameConsideredRateTime = 0.0f;

					FrameStart = -1;
					FrameEnd = -1;
					FrameRange = 0;
					TimeRange = 0.0f;

					TimeElapsedTransition = 0.0f;
					TimeLimitTransition = 0.0f;
					RateTransition = 0.0f;
					IndexTrackSecondary = -1;

					TimeElapseReplacement = 0.0f;
					TimeElapseInRangeReplacement = 0.0f;

					TimeOverrun = 0.0f;
				}

				public bool BootUp()
				{
					CleanUp();

					Status |= FlagBitStatus.VALID;

					return(true);
				}

				public bool Start(	Script_SpriteStudio6_Root instanceRoot,
									Script_SpriteStudio6_DataAnimation dataAnimation,
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
					ArgumentContainer.DataAnimation = dataAnimation;
					ArgumentContainer.IndexAnimation = indexAnimation;
					ArgumentContainer.FramePrevious = -1;

					/* Set datas */
					Status &= FlagBitStatus.VALID;	/* Clear */

					Status = (true == flagPingpong) ? (Status | FlagBitStatus.STYLE_PINGPONG) : (Status & ~FlagBitStatus.STYLE_PINGPONG);
					Status &= ~FlagBitStatus.STYLE_REVERSE;
					RateTime = rateTime;
					if(0.0f > RateTime)
					{
						Status |= (FlagBitStatus.STYLE_REVERSE | FlagBitStatus.PLAYING_REVERSE);
						RateTime *= -1.0f;
					}
					FramePerSecond = framePerSecond;
					TimePerFrame = 1.0f / (float)FramePerSecond;

					FrameStart = frameRangeStart;
					FrameEnd = frameRangeEnd;
					if(0 != (Status & FlagBitStatus.PLAYING_REVERSE))
					{	/* Play-Reverse */
						if(FrameStart >= frame)
						{
							frame = FrameEnd + 1;
						}
					}
					else
					{	/* Play-Foward */
						if((FrameEnd + 1) <= frame)
						{
							frame = FrameStart - 1;
						}
					}
					ArgumentContainer.Frame = frame;

					TimesPlay = timesPlay;
					TimeDelay = timeDelay;

					CountLoop = 0;
					TimeElapsed = (ArgumentContainer.Frame - FrameStart) * TimePerFrame;
					TimeElapsedNow = 0.0f;
//					ArgumentContainer.Frame = Mathf.Clamp(ArgumentContainer.Frame, FrameStart, FrameEnd);

					Status |= (	FlagBitStatus.PLAYING
								| FlagBitStatus.PLAYING_START
							);
					Status &= ~(	FlagBitStatus.IGNORE_NEXTUPDATE_USERDATA
									| FlagBitStatus.IGNORE_NEXTUPDATE_SIGNAL
								);

					FrameRange = (FrameEnd - FrameStart) + 1;
					TimeRange = (float)FrameRange * TimePerFrame;
					TimePerFrameConsideredRateTime = TimePerFrame * RateTime;

					TimeElapseReplacement = 0.0f;
					TimeElapseInRangeReplacement = 0.0f;

					TimeOverrun = 0.0f;

					return(true);
				}

				public bool Transition(int indexTrackSecondary, float time)
				{
					if(0 == (Status & FlagBitStatus.VALID))
					{
						return(false);
					}

					IndexTrackSecondary = indexTrackSecondary;

					TimeElapsedTransition = 0.0f;
					TimeLimitTransition = time;
					RateTransition = 0.0f;

					return(true);
				}

				public bool Stop()
				{
					if(0 == (Status & FlagBitStatus.VALID))
					{
						return(false);
					}

					if(0 == (Status & FlagBitStatus.PLAYING))
					{
						return(true);
					}

					Status &= ~(FlagBitStatus.PLAYING | FlagBitStatus.PAUSING);

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
					float timeElapsedAnimation = timeElapsed * RateTime;
					bool flagStopJumpTime = (0.0f != TimeElapseReplacement);
					if(0 == (Status & FlagBitStatus.PLAYING))
					{	/* Stoping */
						if(true == flagStopJumpTime)
						{
							/* MEMO: In frame-Jump, Elapsed time is absolute. */
							timeElapsedAnimation = TimeElapseReplacement;
						}
						else
						{
							timeElapsedAnimation = 0.0f;
						}
					}
					TimeElapsedNow = timeElapsedAnimation;

					if(0 == (Status & FlagBitStatus.VALID))
					{
						return(false);
					}

					/* Reset status */
					CountLoopNow = 0;
					Status &= ~FlagBitStatus.PLAYING_TURN;
					if(0 != (Status & FlagBitStatus.PLAYING_REVERSE))
					{
						Status |= FlagBitStatus.PLAYING_REVERSEPREVIOUS;
					}
					else
					{
						Status &= ~FlagBitStatus.PLAYING_REVERSEPREVIOUS;
					}

					/* Check Playng status */
					if(0 == (Status & FlagBitStatus.PLAYING))
					{	/* Not-Playing */
						/* MEMO: Even if the animation has ended, there are cases when you are transitioning. */
						if(false == flagStopJumpTime)
						{
							goto Update_UpdateTransition;
						}
					}

					if(0 == (Status & FlagBitStatus.PLAYING_START))
					{	/* (Not Right-After-Starting) */
						if(0 != (Status & FlagBitStatus.PAUSING))
						{	/* Play & Pausing */
#if false
							/* MEMO: Transition does not progress during paused. */
							return(true);
#else
							goto Update_UpdateTransition;
#endif
						}
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
							TimeDelay -= timeElapsedAnimation;
							if(0.0f < TimeDelay)
							{
								Status &= ~(FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);	/* Cancel Start & Decoding Attribute */
								return(true);
							}

							/* Start */
							timeElapsedAnimation += -TimeDelay * ((0 == (Status & FlagBitStatus.PLAYING_REVERSE)) ? 1.0f : -1.0f);
							TimeDelay = 0.0f;
							ArgumentContainer.FramePrevious = -1;
							Status |= (FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);
						}
					}
					if(0 != (Status & FlagBitStatus.PLAYING_START))
					{	/* Play & Right-After-Starting */
						Status |= (FlagBitStatus.PLAYING_START | FlagBitStatus.DECODE_ATTRIBUTE);
						timeElapsedAnimation = 0.0f;
						goto Update_End;	/* Display the first frame, force */
					}

					/* Calculate New-Frame */
					ArgumentContainer.FramePrevious = ArgumentContainer.Frame;
					bool FlagRangeOverPrevious = false;
					if(0 != (Status & FlagBitStatus.STYLE_PINGPONG))
					{	/* Play-Style: PingPong */
						if(0 != (Status & FlagBitStatus.PLAYING_REVERSE))
						{
							FlagRangeOverPrevious = (0.0f > TimeElapsed) ? true : false;
							TimeElapsed -= timeElapsedAnimation;
							if((0.0f > TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto Update_End;
							}
						}
						else
						{
							FlagRangeOverPrevious = (TimeRange <= TimeElapsed) ? true : false;
							TimeElapsed += timeElapsedAnimation;
							if((TimeRange <= TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto Update_End;
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
											if(0 < TimesPlay)
											{	/* Limited-Count Loop */
												TimesPlay--;
												if(0 >= TimesPlay)
												{	/* End */
													TimeOverrun = TimeElapsed;
													goto Update_PlayEnd_Foward;
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
											if(0 < TimesPlay)
											{	/* Limited-Count Loop */
												TimesPlay--;
												if(0 >= TimesPlay)
												{	/* End */
													TimeOverrun = -TimeElapsed;
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
							TimeElapsed -= timeElapsedAnimation;
							if((0.0f > TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto Update_End;
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
									if(0 < TimesPlay)
									{	/* Limited-Count Loop */
										TimesPlay--;
										if(0 >= TimesPlay)
										{	/* End */
											TimeOverrun = -TimeElapsed;
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
							TimeElapsed += timeElapsedAnimation;
							if((TimeRange <= TimeElapsed) && (true == FlagRangeOverPrevious))
							{	/* Still Range-Over */
								goto Update_End;
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
									if(0 < TimesPlay)
									{	/* Limited-Count Loop */
										TimesPlay--;
										if(0 >= TimesPlay)
										{	/* End */
											TimeOverrun = TimeElapsed;
											goto Update_PlayEnd_Foward;
										}
									}

									/* Not-End */
									Status |= FlagBitStatus.PLAYING_TURN;
									CountLoopNow++;
								}
							}
						}
					}

				Update_End:;
					{
						int frame = (int)(TimeElapsed / TimePerFrame);
//						frame = Mathf.Clamp(frame, 0, (FrameRange - 1));
						frame += FrameStart;
						if(	((frame != ArgumentContainer.FramePrevious) || (0 != (Status & FlagBitStatus.PLAYING_TURN)))
							|| (true == flagStopJumpTime)
							)
						{
							Status |= FlagBitStatus.DECODE_ATTRIBUTE;
						}
						ArgumentContainer.Frame = frame;
					}
//					goto Update_UpdateTransition;

				Update_UpdateTransition:;

					Status &= ~FlagBitStatus.REQUEST_TRANSITIONEND;
					if(0 <= IndexTrackSecondary)
					{
						TimeElapsedTransition += timeElapsed;	/* Transition's elapsed time exclude RateTime */
						if(TimeLimitTransition <= TimeElapsedTransition)
						{	/* End */
							RateTransition = 1.0f;	/* Clip */
							Status |= FlagBitStatus.REQUEST_TRANSITIONEND;
						}
						else
						{
							RateTransition = TimeElapsedTransition / TimeLimitTransition;
						}
					}
					return(true);

				Update_PlayEnd_Foward:;
					TimesPlay = 0;	/* Clip */
					Status |= (FlagBitStatus.REQUEST_PLAYEND | FlagBitStatus.DECODE_ATTRIBUTE);
					TimeElapsed = TimeRange;
					ArgumentContainer.Frame = FrameEnd;
					goto Update_UpdateTransition;

				AnimationUpdate_PlayEnd_Reverse:;
					TimesPlay = 0;	/* Clip */
					Status |= (FlagBitStatus.REQUEST_PLAYEND | FlagBitStatus.DECODE_ATTRIBUTE);
					TimeElapsed = 0.0f;
					ArgumentContainer.Frame = FrameStart;
					goto Update_UpdateTransition;
				}

				/* MEMO: Originally should be function, but call-cost is high(taking processing content into account). */
				/*       Processed directly in "Script_SpriteStudio6_Root".                                            */
//				public void StatusClearTransient()
//				{
//					if((FlagBitStatus.VALID | FlagBitStatus.PLAYING) == (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING)))
//					{	/* Not-Playing */
//						Status &= ~(	FlagBitStatus.PLAYING_START
//										| FlagBitStatus.DECODE_ATTRIBUTE
//										| FlagBitStatus.TRANSITION_START
//										| FlagBitStatus.IGNORE_NEXTUPDATE_USERDATA
//									);
//					}
//					TimeElapseReplacement = 0.0f;
//				}

				public void TimeSkip(float time, bool flagReverseParent, bool flagWrapRange)
				{	/* MEMO: In principle, This Function is for calling from "DrawInstance". */
					if(0.0f > time)
					{	/* Wait Infinity */
						TimeDelay = -1.0f;
						return;
					}

					bool flagPongPong = (0 != (Status & FlagBitStatus.STYLE_PINGPONG));	/* ? true : false */
					bool flagReverseStyle = (0 != (Status & FlagBitStatus.STYLE_REVERSE));	/* ? true : false */
					float timeLoop = TimeRange * ((true == flagPongPong) ? 2.0f : 1.0f);
					float timeCursor = time;
					int countLoop = 0;

					/* Loop-Count Get */
					while(timeLoop <= timeCursor)
					{
						timeCursor -= timeLoop;
						countLoop++;
					}

					/* Solving Play-Count */
					if(0 >= TimesPlay)
					{	/* Infinite-Loop */
						/* MEMO: "TimesPlay" does not change. */
						countLoop = 0;
						TimeDelay = 0.0f;
					}
					else
					{	/* Limited-Loop */
						if(0 >= countLoop)
						{	/* No-Wrap-Around */
							/* MEMO: "TimesPlay" does not change. */
							countLoop = 0;
							TimeDelay = 0.0f;
						}
						else
						{	/* Wrap-Around */
							if(TimesPlay <= countLoop)
							{	/* Over */
								if(true == flagReverseParent)
								{	/* Reverse ... Play-Delay */
									/* MEMO: "TimesPlay" does not change. */
									TimeDelay = ((float)(countLoop - TimesPlay) * timeLoop) + timeCursor;
									timeCursor = timeLoop;
								}
								else
								{	/* Foward ... Play-End */
									TimeDelay = 0.0f;
									TimesPlay = 0;

									if(true == flagPongPong)
									{	/* Play-Style: PingPong */
										timeCursor = (0 != (Status & FlagBitStatus.STYLE_REVERSE)) ? TimeRange : 0.0f;
									}
									else
									{	/* Play-Style: OneWay */
										timeCursor = (0 != (Status & FlagBitStatus.STYLE_REVERSE)) ? 0.0f : TimeRange;
									}

									Stop();
								}
							}
							else
							{   /* In-Range */
								TimesPlay -= countLoop;
							}
						}
					}

					/* Time Adjust */
					if(true == flagPongPong)
					{	/* Play-Style: PingPong */
						Status &= ~FlagBitStatus.PLAYING_REVERSE;
						if(true == flagReverseStyle)
						{	/* Play-Stype: PingPong & Reverse */
							if(TimeRange <= timeCursor)
							{	/* Start: Foward */
								timeCursor -= TimeRange;
//								Status &= ~FlagBitStatus.PLAYING_REVERSE;
							}
							else
							{	/* Start: Reverse */
								Status |= FlagBitStatus.PLAYING_REVERSE;
							}
						}
						else
						{	/* Play-Style: PingPong & Foward */
							if(TimeRange <= timeCursor)
							{	/* Start: Reverse */
								timeCursor -= TimeRange;
								timeCursor = TimeRange - timeCursor;
								Status |= FlagBitStatus.PLAYING_REVERSE;
							}
							else
							{	/* Start: Foward */
//								Status &= ~FlagBitStatus.PLAYING_REVERSE;
							}
						}
					}
					else
					{	/* Play-Style: One-Way */
						if(true == flagReverseStyle)
						{	/* Play-Stype: One-Way & Reverse */
							Status |= FlagBitStatus.PLAYING_REVERSE;
							if(false == flagReverseParent)
							{
								timeCursor = TimeRange - timeCursor;
							}
						}
						else
						{	/* Play-Stype: One-Way & Foward */
							Status &= ~FlagBitStatus.PLAYING_REVERSE;
							if(true == flagReverseParent)
							{
								timeCursor = TimeRange - timeCursor;
							}
						}
					}

					TimeElapsed = timeCursor;
					int frame = (int)(TimeElapsed / TimePerFrame);
//					frame = Mathf.Clamp(frame, 0, (FrameRange - 1));
					frame += FrameStart;
					ArgumentContainer.Frame = frame;
				}

				public bool TimeGetRemain(out float timeRemainTotal, out float timeRemainInRange)
				{
					timeRemainTotal = 0.0f;
					timeRemainInRange = 0.0f;

					if((FlagBitStatus.VALID | FlagBitStatus.PLAYING) != (Status & (FlagBitStatus.VALID | FlagBitStatus.PLAYING)))
					{
						return(false);
					}

					int timesPlayRemain = 0;
					if(0 < TimesPlay)
					{	/* Limited-Loop */
						timesPlayRemain = TimesPlay - 1;
					}

					if(0 != (Status & FlagBitStatus.STYLE_PINGPONG))
					{	/* Style-PingPong */
						timeRemainTotal = TimeRange * (float)(timesPlayRemain * 2);

						if(0 != (Status & FlagBitStatus.STYLE_REVERSE))
						{	/* Style-Reverse */
							if(0 != (Status & FlagBitStatus.PLAYING_REVERSE))
							{	/* timeRemain-Reverse (Out-bound) */
								timeRemainInRange = TimeRange + TimeElapsed;
							}
							else
							{	/* Play-Normal (In-bound) */
								timeRemainInRange = (TimeRange - TimeElapsed);
							}
						}
						else
						{	/* Style-Normal */
							if(0 == (Status & FlagBitStatus.PLAYING_REVERSE))
							{	/* Play-Normal (Out-bound) */
								timeRemainInRange = TimeRange + (TimeRange - TimeElapsed);
							}
							else
							{	/* Play-Reverse (In-bound) */
								timeRemainInRange = TimeElapsed;
							}
						}

						timeRemainTotal += timeRemainInRange;
					}
					else
					{
						timeRemainTotal += TimeRange * (float)timesPlayRemain;

						if(0 != (Status & FlagBitStatus.STYLE_REVERSE))
						{	/* Style-Reverse */
							timeRemainInRange = TimeElapsed;
						}
						else
						{	/* Style-Normal */
							timeRemainInRange = (TimeRange - TimeElapsed);
						}

						timeRemainTotal += timeRemainInRange;
					}

					return(true);
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

					STYLE_PINGPONG = 0x02000000,
					STYLE_REVERSE = 0x01000000,

					PLAYING_START = 0x00800000,	/* PLAYING_SKIPTIME */
					PLAYING_REVERSE = 0x00400000,
					PLAYING_REVERSEPREVIOUS = 0x00200000,
					PLAYING_TURN = 0x00100000,

					DECODE_ATTRIBUTE = 0x0008000,

					IGNORE_SKIPLOOP = 0x00001000,
					IGNORE_USERDATA = 0x00000800,
					IGNORE_SIGNAL = 0x00000400,
					IGNORE_NEXTUPDATE_USERDATA = 0x00000200,
					IGNORE_NEXTUPDATE_SIGNAL = 0x00000100,

					TRANSITION_START = 0x00000080,
					TRANSITION_CANCEL_PAUSE = 0x00000040,

					REQUEST_PLAYEND = 0x00000008,
					REQUEST_TRANSITIONEND = 0x00000004,

					CLEAR = 0x00000000,
				}
				#endregion Enums & Constants
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
