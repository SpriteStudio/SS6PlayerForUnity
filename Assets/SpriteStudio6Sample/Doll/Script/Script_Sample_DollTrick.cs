/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Sample_DollTrick : MonoBehaviour
{
	/* ----------------------------------------------- Notes */
	#region Notes
	/* ---------------------------- Sample "Doll (Instance Control)" [Expert] */
	/* The points of this sample are as follows.                              */
	/*                                                                        */
	/* - How to use PlayEnd-callback                                          */
	/* - How to change "Instance"                                             */
	/* - How to change "Instance"'s animation                                 */
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
	private KindAnimationEye AnimationEyeRequest = KindAnimationEye.NON;
	private KindAnimationEye AnimationEyePlaying = KindAnimationEye.NON;

	private KindAnimationBody AnimationBodyRequest = KindAnimationBody.NON;
	private KindAnimationBody AnimationBodyPlaying = KindAnimationBody.NON;
	private int CountRemain = (int)Constant.LIFESPAN_BODY;

	private bool FlagInitialized = false;
	private bool FlagInitializedEye = false;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
	void Start()
	{
		/* Initialize WorkArea */
		FlagInitializedEye = false;
		for(int i=0; i<(int)KindEye.TERMINATOR; i++)
		{
			TableIDPartsControlEye[i] = -1;
		}

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

		/* Set PlayEnd-callback (for "Body") */
		ScriptRoot.FunctionPlayEnd = FunctionPlayEndBody;

		/* Set initial animation */
		AnimationBodyRequest = KindAnimationBody.WAIT;

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
		/* MEMO: "Blinking eyes" works asynchronously with body's  animation. */
		if(true == EyeCheckInitialized())
		{
			TimeWaitBlinkEye -= Time.deltaTime;
			if(0.0f >= TimeWaitBlinkEye)
			{
				if(KindAnimationEye.BLINK != AnimationEyePlaying)
				{
					AnimationEyeRequest = KindAnimationEye.BLINK;
				}
			}

			if(KindAnimationBody.NON == AnimationBodyRequest)
			{
				if(KindAnimationEye.NON != AnimationEyeRequest)
				{	/* Requested */
					AnimationSetEye();
				}
			}
		}

		/* Control Body's Animation */
		if(KindAnimationBody.NON != AnimationBodyRequest)
		{	/* Requested */
			AnimationSetBody();
		}
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
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

			/* MEMO: The reason for getting an instance is because it is necessary to get "Instance" animation's "PlayEnd" callback. */
			/*       In principle, not recommended that you directly control the "Instance" animation.                               */
			/*       However, since may be necessary such implementation,  will provide basic method as a sample.                    */
			/* MEMO: In the case of this sample, "right eye" and "left eye" play in synchronously. */
			/*       Enough to callbacking from one side("left eye") only.                         */
			int indexPartsEyeL = TableIDPartsControlEye[(int)KindEye.L];
			if(0 <= indexPartsEyeL)
			{
				ScriptRootInstanceEyeL = ScriptRoot.InstanceGet(indexPartsEyeL);
				FlagInitializedEye = true;
			}

			TimeWaitBlinkEye = (float)((int)Constant.BLANK_EYE_INITIAL);
		}

		return(FlagInitializedEye);
	}

	private bool AnimationSetEye()
	{
		int idPartsEye;

		/* Set Animation (Left-eye and Right-eye) */
		for(int i=0; i<(int)KindEye.TERMINATOR; i++)
		{
			idPartsEye = TableIDPartsControlEye[i];
			if(0 <= idPartsEye)
			{
				/* MEMO: Do not use ("Instance"'s) "AnimationSet" directly when changing "Instance"'s animation from script. */
				/*       Conflict with operation from parent animation.                                                      */
				ScriptRoot.AnimationChangeInstance(	idPartsEye,
													TableNameAnimationEye[(int)AnimationEyeRequest],
													Library_SpriteStudio6.KindIgnoreAttribute.PERMANENT,	/* Maintain change even if parent's animation is changed */
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

		AnimationEyePlaying = AnimationEyeRequest;
		AnimationEyeRequest = KindAnimationEye.NON;

		/* Set "Instance"'s PlayEnd-callback */
		/* MEMO: To be honest, not advisable to change callback handling function considerably frequently. */
		/*       (Because take time to build delegate)                                                     */
		/*       Originally, seems that implementation like "Body" is desirable...                         */
		if(null != ScriptRootInstanceEyeL)
		{
			if(KindAnimationEye.WAIT == AnimationEyePlaying)
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
		/* MEMO: Recommend not to change animation playing state in PlayEnd callback processing function. */
		AnimationEyeRequest = KindAnimationEye.WAIT;
		TimeWaitBlinkEye = Random.Range((float)((int)Constant.BLANK_EYE_MIN), (float)((int)Constant.BLANK_EYE_MAX)) + 0.5f;

		/* MEMO: "Instance"'s PlayEnd callback processing function should never return false.                                        */
		/*       "Instance" animation can not destroy self. (Returning false will also ignore it)                                    */
		/*       If you really want to create state that "Instance" is disappeared, create hiding animation or hide "Instance"-part. */
		return(true);
	}

	private bool AnimationSetBody()
	{
		int indexAnimation = ScriptRoot.IndexGetAnimation(TableNameAnimationBody[(int)AnimationBodyRequest]);
		AnimationBodyPlaying = AnimationBodyRequest;
		AnimationBodyRequest = KindAnimationBody.NON;
		if(0 > indexAnimation)
		{	/* Ignore */
			return(false);
		}

		ScriptRoot.AnimationPlay(-1, indexAnimation, 1);
		return(true);
	}

	private bool FunctionPlayEndBody(Script_SpriteStudio6_Root scriptRoot, GameObject objectControl)
	{
		/* MEMO: Recommend not to change animation playing state in PlayEnd callback processing function. */
		switch(AnimationBodyPlaying)
		{
			case KindAnimationBody.WAIT:
				if(0.5f < Random.value)
				{
					AnimationBodyRequest = KindAnimationBody.PUT_OUT_L;
				}
				else
				{
					AnimationBodyRequest = KindAnimationBody.WAIT;
				}

				CountRemain--;
				if(0 > CountRemain)
				{	/* Destroy Self */
					/* MEMO: Animation object destroys self when Highest-Parent animation's PlayEnd callback processing function returns false. */
					return(false);
				}
				break;

			case KindAnimationBody.PUT_OUT_L:
				AnimationBodyRequest = KindAnimationBody.RETURN_L;
				break;

			case KindAnimationBody.RETURN_L:
				AnimationBodyRequest = KindAnimationBody.WAIT;
				break;
		}

		return(true);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	private enum Constant
	{
		BLANK_EYE_MIN = 0,
		BLANK_EYE_MAX = 2,
		BLANK_EYE_INITIAL = 1,

		LIFESPAN_BODY = 50,
	}

	private enum KindAnimationBody
	{
		NON = -1,

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
		NON = -1,

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
