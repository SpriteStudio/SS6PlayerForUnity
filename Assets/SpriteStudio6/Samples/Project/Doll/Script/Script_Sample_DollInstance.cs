/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Sample_DollInstance : MonoBehaviour
{
	/* ----------------------------------------------- Notes */
	#region Notes
	/* ---------------------------- Sample "Doll (Instance Control)" [Expert] */
	/* The points of this sample are as follows.                              */
	/*                                                                        */
	/* - How to use PlayEnd-callback                                          */
	/* - How to change "Instance"                                             */
	/* - How to change "Instance"'s animation                                 */
	/* - How to change "Effect"                                               */
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

	/* WorkArea */
	private Script_SpriteStudio6_Root ScriptRoot = null;
	private Script_SpriteStudio6_Root ScriptRootInstanceEyeL = null;
	private int[] TableIDPartsControlEye = new int[(int)KindEye.TERMINATOR];

	private float TimeWaitBlinkEye = 0.0f;
	private bool FlagBlinkingEye = false;

	private bool FlagInitialized = false;
	private bool FlagInitializedEye = false;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
	void Start()
	{
		/* Initialize WorkArea */
		FlagBlinkingEye = false;
		for(int i=0; i<(int)KindEye.TERMINATOR; i++)
		{
			TableIDPartsControlEye[i] = -1;
		}

		/* Get Animation Control Script-Component */
		GameObject gameObjectBase = GameObjectRoot;
		if(null == gameObjectBase)
		{
			gameObjectBase = this.gameObject;
		}
		ScriptRoot = Script_SpriteStudio6_Root.Parts.RootGet(gameObjectBase);
		if(null == ScriptRoot)
		{	/* Error */
			return;
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

		/* Control Eye's Animations */
		if(true == EyeCheckInitialized())
		{
			if(false == FlagBlinkingEye)
			{	/* Now Waiting */
				/* Check when start blinking */
				TimeWaitBlinkEye -= Time.deltaTime;
				if(0.0f >= TimeWaitBlinkEye)
				{	/* Wait -> Blink */
					AnimatonSetEye(KindAnimationEye.BLINK);

					FlagBlinkingEye = true;
				}
			}
			else
			{	/* Now Blinking */
				if(true == float.IsNaN(TimeWaitBlinkEye))
				{	/* Blink -> Wait */
					AnimatonSetEye(KindAnimationEye.WAIT);

					TimeWaitBlinkEye = Random.Range((float)((int)Constant.BLANK_EYE_MIN), (float)((int)Constant.BLANK_EYE_MAX));
					FlagBlinkingEye = false;
				}
			}
		}
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region Functions
	private bool EyeCheckInitialized()
	{
		if(false == FlagInitializedEye)
		{
			FlagInitializedEye = true;

			/* Get PartID (Eye's "Instance"-control parts) & "Instance" */
			for(int i=0; i<(int)KindEye.TERMINATOR; i++)
			{
				TableIDPartsControlEye[i] = ScriptRoot.IDGetParts(TableNamePartsEye[i]);
			}

			int indexPartsEyeL = TableIDPartsControlEye[(int)KindEye.L];
			if(0 <= indexPartsEyeL)
			{
				ScriptRootInstanceEyeL = ScriptRoot.InstanceGet(indexPartsEyeL);
				FlagInitializedEye = true;
			}

			TimeWaitBlinkEye = float.NaN;
			FlagBlinkingEye = true;
		}

		return(FlagInitializedEye);
	}

	private bool AnimatonSetEye(KindAnimationEye kind)
	{
		int idPartsEye;

		/* Set Animation */
		for(int i=0; i<(int)KindEye.TERMINATOR; i++)
		{
			idPartsEye = TableIDPartsControlEye[i];
			if(0 <= idPartsEye)
			{
				ScriptRoot.AnimationChangeInstance(	idPartsEye,
													TableNameAnimationEye[(int)kind],
													Library_SpriteStudio6.KindIgnoreAttribute.PERMANENT,
													true,	/* Start immediate */
													1,
													1.0f,
													Library_SpriteStudio6.KindStylePlay.NORMAL,
													"",
													0,
													"",
													0
												);
			}
		}

		/* Set "Instance"'s CallBack-End */
		if(null != ScriptRootInstanceEyeL)
		{
			if(KindAnimationEye.WAIT == kind)
			{
				ScriptRootInstanceEyeL.FunctionPlayEnd = null;
			}
			else
			{
				ScriptRootInstanceEyeL.FunctionPlayEnd = FunctionPlayEndEye;
			}
		}

		return(false);
	}

	private bool FunctionPlayEndEye(Script_SpriteStudio6_Root scriptRoot, GameObject objectControl)
	{
		TimeWaitBlinkEye = float.NaN;

		return(true);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	private enum Constant
	{
		BLANK_EYE_MIN = 0,
		BLANK_EYE_MAX = 2,
		BLANK_EYE_INITIAL= 1,
	}

	private enum KindAnimationBody
	{
		WAIT = 0,
		PUT_OUT_L,
		RETURN_L,

		TERMINATOR
	}
	private readonly static string[] TableNameAnimationBody = new string[(int)KindAnimationBody.TERMINATOR]
	{
		"Wait",
		"PutOutL",
		"ReturnL",
	};

	private enum KindEye
	{
		L = 0,
		R,

		TERMINATOR
	}
	private readonly static string[] TableNamePartsEye = new string[(int)KindEye.TERMINATOR]
	{
		"Eye-L",
		"Eye-R",
	};
	private enum KindAnimationEye
	{
		WAIT = 0,
		BLINK,

		TERMINATOR
	}
	private readonly static string[] TableNameAnimationEye = new string[(int)KindAnimationEye.TERMINATOR]
	{
		"Wait",
		"Blink",
	};
	#endregion Enums & Constants
}
