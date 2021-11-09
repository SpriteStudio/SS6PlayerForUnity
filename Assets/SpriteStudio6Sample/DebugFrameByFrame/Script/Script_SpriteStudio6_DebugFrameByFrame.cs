/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SpriteStudio6_DebugFrameByFrame : MonoBehaviour
{
	/* ----------------------------------------------- Notes */
	#region Notes
	/* ---------------------------- Sample "Doll (Instance Control)" [Expert] */
	/* The points of this sample are as follows.                              */
	/*                                                                        */
	/* - Provide a simple frame-by-frame debug sample                         */
	/* - How to use the time elapse management function                       */
 	#endregion Notes

	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public GameObject GameObjectWatching;
	public KindTimePassage TimePassage;

	private Script_SpriteStudio6_Root ScriptRootWatching;
	private bool FlagKeyDown;
	#endregion Variables & Properties

	void Start()
	{
		if(null != GameObjectWatching)
		{
			ScriptRootWatching = Script_SpriteStudio6_Root.Parts.RootGet(GameObjectWatching);
			if(null == ScriptRootWatching)
			{	/* Error */
				return;
			}

			/* MEMO: If you want animation to elapse special time for application's circumstances, */
			/*        you can set a function to manage time passage.                               */
			/* MEMO: When set to null, the default(Time.deltaTime) is used. */
			ScriptRootWatching.FunctionTimeElapse = FunctionTimeElapse;
		}
	}
	
	void Update()
	{
		/* MEMO: Progress 1 cycle processing when press spacebar once. */
		FlagKeyDown = Input.GetKeyDown(KeyCode.Space);
	}

	void OnGUI()
	{
		const float TextWidth = 500;
		const float TextHeight = 20;

		if(null != ScriptRootWatching)
		{
			int countTrack = ScriptRootWatching.CountGetTrack();
			string text;

			for(int i=0; i<countTrack; i++)
			{
				text = string.Format(	"Track[" + i.ToString()
										+ "]: Animation[" + ScriptRootWatching.TableControlTrack[i].ArgumentContainer.IndexAnimation.ToString()
										+ "] Frame[" + ScriptRootWatching.TableControlTrack[i].ArgumentContainer.Frame.ToString()
										+ "]"
									);
				GUI.Label(new Rect(20, 20 + (TextHeight * i), TextWidth, TextHeight), text);
			}
		}
	}

	/* ----------------------------------------------- Functions */
	#region Functions
	private float FunctionTimeElapse(Script_SpriteStudio6_Root scriptRoot)
	{
		/* MEMO: In this case, the set time is allowed to elapse only when the key is pressed. */
		float time = 0.0f;
		if(true == FlagKeyDown)
		{
			switch(TimePassage)
			{
				case KindTimePassage.FPS_60:
					time = 1.0f / 60.0f;
					break;

				case KindTimePassage.FPS_30:
					time = 1.0f / 30.0f;
					break;

				case KindTimePassage.ELAPSED:
					time = Time.deltaTime;
					break;
			}
		}
		return(time);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindTimePassage
	{
		FPS_60 = 0,
		FPS_30,
		ELAPSED,
	}
	#endregion Enums & Constants
}
