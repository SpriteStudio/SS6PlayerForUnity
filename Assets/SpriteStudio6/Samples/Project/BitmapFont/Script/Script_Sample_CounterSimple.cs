/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Sample_CounterSimple : MonoBehaviour
{
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
	private int[] TableIndexCell = new int[(int)KindCharacter.TERMINATOR];
	private int[] TableIDPartsDigit = new int[(int)Constant.DIGIT_MAX];
	private int ValuePrevious = int.MaxValue;
	private bool FlagPaddingZeroPrevious;
	private bool FlagInitialized = false;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
	void Start()
	{
		/* Initialize WorkArea */
		FlagPaddingZeroPrevious = FlagPaddingZero;

		/* Get(Cache) Animation Control Script-Component */
		if(null == GameObjectRoot)
		{	/* Error */
			return;
		}
		ScriptRoot = Script_SpriteStudio6_Root.Parts.RootGetChild(GameObjectRoot);
		if(null == ScriptRoot)
		{	/* Error */
			return;
		}

		/* Animation Start */
		int indexAnimation = ScriptRoot.IndexGetAnimation("Digit08");
		if(0 > indexAnimation)
		{
			return;
		}
		/* MEMO: Since animation without movement, no problem even if  stops right after playing. */
		/*       (no problem even if does not stop too.)                                          */
		ScriptRoot.AnimationPlay(-1, indexAnimation, 1);
		ScriptRoot.AnimationStop(-1);

		/* Get Digit-Parts */
		for(int i=0; i<(int)Constant.DIGIT_MAX; i++)
		{
			/* MEMO: Be careful. When part's name is not found, "-1" is assigned. */
			TableIDPartsDigit[i] = ScriptRoot.IDGetParts(TableNameParts[i]);
		}

		/* Get Characters' Cell Index */
		/* MEMO: Since only 1 texture, specifying direct-value. (Cut corners ...) */
		Library_SpriteStudio6.Data.CellMap cellMap = ScriptRoot.CellMapGet(0);
		for (int i=0; i<(int)KindCharacter.TERMINATOR; i++)
		{
			TableIndexCell[i] = cellMap.IndexGetCell(TableNameCells[i]);
		}
	}
	
	void Update ()
	{
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	/* [Constant] Number of digits, maximum-value and minimum-values */
	private enum Constant
	{
		DIGIT_MAX = 8,
	};

	private static readonly int ValueMax = (int)(Mathf.Pow(10.0f, (int)Constant.DIGIT_MAX)) - 1;
	private static readonly int ValueMin = -((int)(Mathf.Pow(10.0f, (int)Constant.DIGIT_MAX - 1)) - 1);

	/* [Constant] Characters defined */
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
	};
	private static readonly char[] TableCharacters = new char[(int)KindCharacter.TERMINATOR]
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

	/* [Constant] Parts-Names and Cell-Names */
	private static readonly string[] TableNameParts = new string[(int)Constant.DIGIT_MAX]
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
	private static readonly string[] TableNameCells = new string[(int)KindCharacter.TERMINATOR]
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

	/* [Constant] Others */
	private enum KindFormat
	{
		NORMAL = 0,
		PADDING_ZERO,

		TERMINATOR
	};
	private static readonly string[] TableFormatToString = new string[(int)KindFormat.TERMINATOR]
	{
		"D",
		"D" + ((int)(Constant.DIGIT_MAX)).ToString(),
	};
	#endregion Enums & Constants
}
