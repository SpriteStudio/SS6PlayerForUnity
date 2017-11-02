/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
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
	/* - How to use animation blend (transition TRS)                          */
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

			FlagInitializedTrack = true;
		}

		/* Update Animations */
		int indexAnimation;
		if(AnimationUpperBodyPrevious != AnimationUpperBody)
		{
			indexAnimation = TableIndexAnimation[(int)AnimationUpperBody];
			if(0 <= indexAnimation)
			{
				ScriptRoot.AnimationPlay((int)KindTrack.BODY_UPPER, indexAnimation, 0);
			}

			AnimationUpperBodyPrevious = AnimationUpperBody;
		}
		if(AnimationLowerBodyPrevious != AnimationLowerBody)
		{
			indexAnimation = TableIndexAnimation[(int)AnimationLowerBody];
			if(0 <= indexAnimation)
			{
				ScriptRoot.AnimationPlay((int)KindTrack.BODY_LOWER, indexAnimation, 0);
			}

			AnimationLowerBodyPrevious = AnimationLowerBody;
		}
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- MonoBehaviour-Functions */
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
			switch(ScriptRoot.DataAnimation.TableParts[i].LabelColor.Form)
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
