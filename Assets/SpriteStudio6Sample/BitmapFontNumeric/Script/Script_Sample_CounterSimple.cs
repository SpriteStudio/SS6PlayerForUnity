/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Sample_CounterSimple : MonoBehaviour
{
	/* ----------------------------------------------- Notes */
	#region Notes
	/* ----------------------------------- Sample "Simple Counter" [Beginner] */
	/* The points of this sample are as follows.                              */
	/*                                                                        */
	/* - How to play animation (Simple playing-method)                        */
	/* - How to search for "Cell"s and parts                                  */
	/* - How to hide parts from script                                        */
	/* - How to change part's cell from script (Cell-Change)                  */
	#endregion Notes

	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	/* Target Animation-Object */
	public GameObject GameObjectRoot;

	/* Value */
	public int Value;

	/* Form */
	public bool FlagPaddingZero;

	/* WorkArea */
	private Script_SpriteStudio6_Root ScriptRoot = null;

	private int[] TableIDPartsDigit = new int[(int)Constant.DIGIT_MAX];
	private int[] TableIndexCell = new int[(int)KindCharacter.TERMINATOR];

	private int ValuePrevious = int.MaxValue;
	private bool FlagPaddingZeroPrevious = false;

	private bool FlagInitialized = false;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
	void Start()
	{
		/* Initialize WorkArea */
		FlagPaddingZeroPrevious = !FlagPaddingZero;	/* Since this value can not be set with initializer... */

		/* Get Animation Control Script-Component */
		GameObject gameObjectBase = GameObjectRoot;
		if(null == gameObjectBase)
		{
			gameObjectBase = gameObject;
		}
		/* MEMO: "Script_SpriteStudio6_Root.Parts.RootGet" is function for finding "Script_SpriteStudio6_Root" in GameObjects below.       */
		/*       (However, "Instance" is excluded. Find only "Highest-Root"-parts)                                                         */
		/*       You can find "shallowest hierarchy"-one from direct-children, but not guarantee the shallowest when deeper than children. */
		/*       Because wasteful to search every time access, recommend to cache.                                                         */
		ScriptRoot = Script_SpriteStudio6_Root.Parts.RootGet(gameObjectBase);
		if(null == ScriptRoot)
		{	/* Error */
			return;
		}

		/* Animation Start */
		/* MEMO: Since animation without movement, no problem even if stops right after playing. */
		/*       (no problem even if does not stop too.)                                         */
		/*       However, stopped animation has less CPU load than continue playing.             */
		/* MEMO: Since useless to search animation-index every time, recommend to cache frequently used indexes. */
		int indexAnimation = ScriptRoot.IndexGetAnimation("Digit08");
		if(0 > indexAnimation)
		{	/* Animation not found */
			return;
		}
		ScriptRoot.AnimationPlay(-1, indexAnimation, 1);
		ScriptRoot.AnimationStop(-1);

		/* Get Digit-Parts */
		/* MEMO: Get animation-parts for value's each digit. */
		/* MEMO: It is desirable that Animation's information are gotten after initialization of animation. */
		for(int i=0; i<(int)Constant.DIGIT_MAX; i++)
		{
			/* MEMO: Be careful. When part's name is not found, "-1" is assigned. */
			TableIDPartsDigit[i] = ScriptRoot.IDGetParts(TableNameParts[i]);
		}

		/* Get Characters' Cell Index */
		/* MEMO: Get cell index of bitmap font. */
		Library_SpriteStudio6.Data.CellMap cellMap = ScriptRoot.CellMapGet(0);	/* Since only 1 texture, specifying direct-value. (Cut corners ...) */
		for (int i=0; i<(int)KindCharacter.TERMINATOR; i++)
		{
			TableIndexCell[i] = cellMap.IndexGetCell(TableNameCell[i]);
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

		/* Clamp Value */
		int valueDisplay = Mathf.Clamp(Value, ValueMin, ValueMax);

		/* Check Update */
		bool flagUpdate = false;
		if(ValuePrevious != Value)
		{
			ValuePrevious = Value;
			flagUpdate |= true;
		}
		if(FlagPaddingZeroPrevious != FlagPaddingZero)
		{
			FlagPaddingZeroPrevious = FlagPaddingZero;
			flagUpdate |= true;
		}

		/* Update */
		if(true == flagUpdate)
		{
			string textValue;

			/* Get Text */
			if(true == FlagPaddingZero)
			{	/* Zero-padding */
				textValue = valueDisplay.ToString("D" + ((int)(Constant.DIGIT_MAX)).ToString());
			}
			else
			{	/* Right-alignment */
				textValue = valueDisplay.ToString("D");
			}

			/* Update Digits */
			{
				/* Generate Text */
				char[] charactersDigit = textValue.ToCharArray();	/* Split to digit */
				int countDigit = charactersDigit.Length;
				int idParts;
				int indexCharacter;
				for(int i=0; i<countDigit; i++)
				{
					/* MEMO: Since "idParts == 0" is the "Root"-part, intentionally excluded.           */
					/*       Setting HideSet's idParts to 0 is to control hidding the entire animation. */
					idParts = TableIDPartsDigit[i];
					if(0 < idParts)
					{
						/* Change Cell */
						/* MEMO: (IndexCellMap == 0) Because this Animation has 1 Texture. */
						indexCharacter = IndexGetCharacter(charactersDigit[(countDigit - 1) - i]);
						ScriptRoot.CellChangeParts(	idParts,
													0,
													TableIndexCell[indexCharacter],
													Library_SpriteStudio6.KindIgnoreAttribute.NOW_ANIMATION	/* Ignore "Reference-Cell" attribute during now animation */
												);
				
						/* Show Digit */
						/* MEMO: Don't Effect to children */
						ScriptRoot.HideSet(idParts, false, false);
					}
				}
				for(int i=countDigit; i<(int)Constant.DIGIT_MAX; i++)
				{
					idParts = TableIDPartsDigit[i];
					if(0 < idParts)
					{
						ScriptRoot.HideSet(idParts, true, false);
					}
				}
			}
		}
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	private int IndexGetCharacter(char character)
	{
		int Count = (int)KindCharacter.TERMINATOR;
		for(int i=0; i<Count; i++)
		{
			if(TableCharacters[i] == character)
			{
				return(i);
			}
		}

		return(-1);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	/* Number of digits, maximum-value and minimum-values */
	private enum Constant
	{
		DIGIT_MAX = 8,
	}

	private readonly static int ValueMax = (int)(Mathf.Pow(10.0f, (int)Constant.DIGIT_MAX)) - 1;
	private readonly static int ValueMin = -((int)(Mathf.Pow(10.0f, (int)Constant.DIGIT_MAX - 1)) - 1);

	/* Characters defined */
	private enum KindCharacter
	{
		NUMBER_0 = 0,
		NUMBER_1,
		NUMBER_2,
		NUMBER_3,
		NUMBER_4,
		NUMBER_5,
		NUMBER_6,
		NUMBER_7,
		NUMBER_8,
		NUMBER_9,
		SYMBOL_PERIOD,
		SYMBOL_COMMA,
		SYMBOL_PLUS,
		SYMBOL_MINUS,
		SYMBOL_MUL,
		SYMBOL_DIV,

		TERMINATOR
	}
	private readonly static char[] TableCharacters = new char[(int)KindCharacter.TERMINATOR]
	{
		'0',
		'1',
		'2',
		'3',
		'4',
		'5',
		'6',
		'7',
		'8',
		'9',
		'.',
		',',
		'+',
		'-',
		'*',
		'/',
	};

	/* Parts-Names and Cell-Names */
	private readonly static string[] TableNameParts = new string[(int)Constant.DIGIT_MAX]
	{
		"Digit00",
		"Digit01",
		"Digit02",
		"Digit03",
		"Digit04",
		"Digit05",
		"Digit06",
		"Digit07",
	};
	private readonly static string[] TableNameCell = new string[(int)KindCharacter.TERMINATOR]
	{
		"Font1_0",
		"Font1_1",
		"Font1_2",
		"Font1_3",
		"Font1_4",
		"Font1_5",
		"Font1_6",
		"Font1_7",
		"Font1_8",
		"Font1_9",
		"Font1_Period",
		"Font1_Comma",
		"Font1_Plus",
		"Font1_Minus",
		"Font1_Mul",
		"Font1_Div",
	};
	#endregion Enums & Constants
}
