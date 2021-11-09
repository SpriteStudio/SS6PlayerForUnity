/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Sample_DollRun : MonoBehaviour
{
	/* ----------------------------------------------- Notes */
	#region Notes
	/* ------------------------- Sample "Doll-Run (Animation Blend)" [Expert] */
	/* The points of this sample are as follows.                              */
	/*                                                                        */
	/* - How to use multi play track                                          */
	/* - How to use "Animation Transition" (transition TRS)                   */
	/* - How to use track-play-end callback & transition-end callback         */
	/* - Tips to use parts' "Color-Label"                                     */
	/*                                                                        */
	/* *) First, recommend to refer to the following samples                  */
	/*     since annotation for rudimentary handling is omitted.              */
	/*                                                                        */
	/*    (in "Assets/SpriteStudio6/Samples/Project/BitmapFont/Script/")      */
	/*     - "Script_Sample_CounterSimple.cs"                                 */
	/*     - "Script_Sample_CounterComplex.cs"                                */
 	#endregion Notes

	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	/* Target Animation-Object */
	public GameObject GameObjectRoot;

	public KindAnimation AnimationUpperBody;
	public KindAnimation AnimationLowerBody;

	public bool FlagAnimationTransition;
	public float TimeAnimationTransition;

	/* WorkArea */
	private Script_SpriteStudio6_Root ScriptRoot = null;

	private KindAnimation AnimationUpperBodyPrevious = (KindAnimation)(-1);
	private KindAnimation AnimationLowerBodyPrevious = (KindAnimation)(-1);

	private int[] TableIndexAnimation = new int[KindAnimationTERMINATOR];

	private bool FlagInitialized = false;
	private bool FlagInitializedTrack = false;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
	void Start()
	{
		/* Get Animation Control Script-Component */
		GameObject gameObjectBase = GameObjectRoot;
		if(null == gameObjectBase)
		{
			gameObjectBase = gameObject;
		}
		ScriptRoot = Script_SpriteStudio6_Root.Parts.RootGet(gameObjectBase);
		if(null == ScriptRoot)
		{	/* Error */
			return;
		}

		/* Cache Animation index */
		for(int i=0; i<KindAnimationTERMINATOR; i++)
		{
			TableIndexAnimation[i] = ScriptRoot.IndexGetAnimation(TableNameAnimation[i]);
		}

		/* Initialize Complete */
		FlagInitialized = true;
	}

	void Update ()
	{
		int indexAnimation;

		/* Check Validity */
		if(false == FlagInitialized)
		{	/* Failed to initialize */
			return;
		}

		/* Initialize Multi-Track & Connect Parts to Track */
		if(false == FlagInitializedTrack)
		{
			ScriptRoot.AnimationStop(-1);

			/* Initialize Multi-Track */
			/* MEMO: The number of tracks must be equal or more than the number of animations played at the same time.                      */
			/*       In this sample, allocate different animations for "upper body" and "lower body", so two are required at least.         */
			/*                                                                                                                              */
			/*       Also, the reason for creating 4 tracks is because of "Animation Transition".                                           */
			/*       "Animation Transitions" will play 2 animations during the transition so need 2 tracks when use "Animation Transition". */
			/*       In this sample, "upper body" and "lower body" are run separately, so the following 4 tracks are required.              */
			/*       - For playing "upper body"'s animation                                                                                 */
			/*       - For playing "lower body"'s animation                                                                                 */
			/*       - For "upper body"'s animation-transition                                                                              */
			/*       - For "lower body"'s animation-transition                                                                              */
			/*                                                                                                                              */
			/*       "Animation transition" is function to transition between 2 animations while interpolating TRS(Position, Rotation       */
			/*        and Scaling).                                                                                                         */
			/* MEMO: To initialize multi tracks, Seting number of tracks directly from "Script_SpriteStudio6_Root"'s inspector is also OK. */
			/*       In this sample, provide as a way to initialize from a script.                                                         */
			/*       However, not recommended that Increasing or decreasing the number of tracks frequently during appliacation running.   */
			/* MEMO: "TrackReboot" function will not reinstall the track if more than specified number of tracks are already exist. */
			/*       Also, current playing state is preserved.                                                                      */
			if(false == ScriptRoot.TrackReboot((int)KindTrack.TERMINATOR))
			{	/* Error */
				return;
			}

			/* Connect Parts to Track */
			/* MEMO: You can link(connect) each animation's parts to "Track"s.                             */
			/*       Each "Track"s can play different animations, so you can blend animations as a result. */
			/*       However, can not assign plural tracks to 1 part.                                      */
			if(false == PartsConnectTrack())
			{
				return;
			}

			/* Initialize Animation */
			indexAnimation = TableIndexAnimation[(int)AnimationUpperBody];
			ScriptRoot.AnimationPlay((int)KindTrack.BODY_UPPER, indexAnimation, 0);
			AnimationUpperBodyPrevious = AnimationUpperBody;

			indexAnimation = TableIndexAnimation[(int)AnimationLowerBody];
			ScriptRoot.AnimationPlay((int)KindTrack.BODY_LOWER, indexAnimation, 0);
			AnimationLowerBodyPrevious = AnimationLowerBody;

			FlagInitializedTrack = true;
		}

		/* Adjust values */
		if(0.1f >= TimeAnimationTransition)
		{
			TimeAnimationTransition = 0.1f;
		}

		/* Update Animations */
		if(AnimationLowerBodyPrevious != AnimationLowerBody)
		{
			indexAnimation = TableIndexAnimation[(int)AnimationLowerBody];
			if(0 <= indexAnimation)
			{
				if(true == FlagAnimationTransition)
				{	/* Transition (Fade) */
					/* MEMO: When start transition, you need to play destination animation on track for transition first. */
					ScriptRoot.AnimationPlay((int)KindTrack.TRANSITION_LOWER, indexAnimation, 0);

					/* MEMO: Connect transition track to current animation's track after destination animation is successfully played. */
					/*       Set the transtion track to "indexTrackSecondary".                                                         */
					/* MEMO: Transition ends at set time(TimeAnimationTransition).                                     */
					/*       At the end of the transition, transition track's playing state will move to primary track. */
					/* MEMO: Transition while animating when current animation or destination animation is not in the pause state. */
					ScriptRoot.TrackTransition(	(int)KindTrack.BODY_LOWER,
												(int)KindTrack.TRANSITION_LOWER,
												TimeAnimationTransition,
												false	/* Ignored if destination animation is not in pause state. */
											);

					/* MEMO: If you want timing of transition end, set callback function to "FunctionPlayEndTrack [primary track's index]" of ScriptRoot. */
					ScriptRoot.FunctionPlayEndTrack[(int)KindTrack.BODY_LOWER] = FunctionPlayEndTrackBody;

				}
				else
				{	/* Immediate */
					ScriptRoot.AnimationPlay((int)KindTrack.BODY_LOWER, indexAnimation, 0);
				}
			}

			AnimationLowerBodyPrevious = AnimationLowerBody;
		}
		if(AnimationUpperBodyPrevious != AnimationUpperBody)
		{
			indexAnimation = TableIndexAnimation[(int)AnimationUpperBody];
			if(0 <= indexAnimation)
			{
				if(true == FlagAnimationTransition)
				{	/* Transition (Fade) */
					/* MEMO: If you pause animation and make a transition, you can perform transition while stopping the animation. */
					ScriptRoot.AnimationPlay((int)KindTrack.TRANSITION_UPPER, indexAnimation, 0);

					ScriptRoot.AnimationPause((int)KindTrack.TRANSITION_UPPER, true);
					ScriptRoot.AnimationPause((int)KindTrack.BODY_UPPER, true);

					/* MEMO: Set "flagCancelPauseAfterTransition" to true to automatically unpause animation when transition is complete. */
					/*       Of course, you can get the timing with transition end callback and cancel pause state by yourself.           */
					/*       But this way is more easier.                                                                                 */
					ScriptRoot.TrackTransition(	(int)KindTrack.BODY_UPPER,
												(int)KindTrack.TRANSITION_UPPER,
												TimeAnimationTransition,
												true
											);
				}
				else
				{	/* Immediate */
					ScriptRoot.AnimationPlay((int)KindTrack.BODY_UPPER, indexAnimation, 0);
				}
			}

			AnimationUpperBodyPrevious = AnimationUpperBody;
		}
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	private bool PartsConnectTrack()
	{
		if(null == ScriptRoot.DataAnimation)
		{
			return(false);
		}

		/* MEMO: Here, present one of a method of using "Color-Label".                                        */
		/*       "Color-Label" is a "SpriteStudio 6"'s function to visually classify parts by coloring parts. */
		/*       This "Color-Label" does not affect animation, but "SS6PU" has this as data.                  */
		/*       This time, use "Color-Label" as classification of "which track part is used?".               */
		int countParts = ScriptRoot.CountGetParts();
		int indexTrack;
		for(int i=0; i<countParts; i++)
		{
			switch(ScriptRoot.FormGetColorLabel(i))
			{
				case Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.RED:
					indexTrack = (int)KindTrack.BODY_LOWER;
					break;

				case Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.BLUE:
					indexTrack = (int)KindTrack.BODY_UPPER;
					break;

				default:
					goto case Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.RED;
			}

			/* MEMO: "TrackConnectParts" function can also specify children of specific part at once. */
			/*        (When set "flagChildParts" to true)                                             */
			/*        This time, set all parts one by one.                                            */
			ScriptRoot.TrackConnectParts(i, indexTrack, false);
		}

		return(true);
	}

	private void FunctionPlayEndTrackBody(	Script_SpriteStudio6_Root scriptRoot,
											int indexTrackPlay,
											int indexTrackSecondary,
											int indexAnimation,
											int indexAnimationSecondary
										)
	{
		/* MEMO: Same function is called when transition end and track play-end.                                                                   */
		/*       As method to distinguish them, at the end of the transition,                                                                      */
		/*        "indexTrackSecondary" is set to "transition track index"  and "indexAnimationSecondary" is set to "destination animation index". */
		/*       (In case of playback end of track, both are set to -1)                                                                            */
		/* MEMO: When "track play end" and "transition end" occur simultaneously, callbacks will be execute in following order. */
		/*       1. Transition end                                                                                              */
		/*       2. Track play end                                                                                              */
		/* MEMO: Recommend not to change animation playing state in TrackPlayEnd callback processing function. */
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	private enum KindTrack
	{
		BODY_LOWER = 0,
		BODY_UPPER,
		TRANSITION_LOWER,
		TRANSITION_UPPER,

		TERMINATOR
	}

	public enum KindAnimation
	{
		WAIT = 0,
		TROT,
		DASH,
	}
	private const int KindAnimationTERMINATOR = (int)KindAnimation.DASH + 1;	/* want to not include in the list ... */
	private readonly static string[] TableNameAnimation = new string[KindAnimationTERMINATOR]
	{
		"Wait",
		"Run01",
		"Run02",
	};
	#endregion Enums & Constants
}
