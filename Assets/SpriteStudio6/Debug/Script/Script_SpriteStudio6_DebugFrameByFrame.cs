/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SpriteStudio6_DebugFrameByFrame : MonoBehaviour
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public GameObject GameObjectWatching;

	internal Script_SpriteStudio6_Root ScriptRootWatching;
	internal bool FlagKeyDown;
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

			ScriptRootWatching.FunctionTimeElapse = FunctionTimeElapse;
		}
	}
	
	void Update()
	{
		FlagKeyDown = Input.GetKeyDown(KeyCode.Space);
	}

	void OnGUI()
	{
		const float TextWidth = 200;
		const float TextHeight = 20;

		if(null != ScriptRootWatching)
		{
			int countTrack = ScriptRootWatching.CountGetTrack();
			string text;

			for(int i=0; i<countTrack; i++)
			{
				text = string.Format(	"Track[" + i.ToString()
										+ "]: Frame = {0}", ScriptRootWatching.TableControlTrack[i].ArgumentContainer.Frame.ToString()
									);
				GUI.Label(new Rect(20, 20 + (TextHeight * i), TextWidth, TextHeight), text);
			}
		}
	}

	/* ----------------------------------------------- Functions */
	#region Functions
	private float FunctionTimeElapse(Script_SpriteStudio6_Root scriptRoot)
	{
		return((true == FlagKeyDown) ? Time.deltaTime : 0.0f);
	}
	#endregion Functions
}
