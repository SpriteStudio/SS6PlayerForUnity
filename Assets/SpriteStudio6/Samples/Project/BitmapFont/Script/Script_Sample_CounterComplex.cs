/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Sample_CounterComplex : MonoBehaviour
{
	/* ----------------------------------------------- Notes */
	#region Notes
	/* The points of this sample are as follows.                              */
	/*                                                                        */
	/* - How to change animation at real-time                                 */
	/* - How to use AdditionalColor                                           */
	/* - How to use UserData-callback                                         */
	/* - How to use PlayEnd-callback                                          */
	/* - How to change CellMap from script (CellMap-Change)                   */
	/* - How to change TableMaterial from script (TableMaterial-Change)       */
	/* - Tips to change Parts' Transform of animation parts from script       */
	/*                                                                        */
	/* *) Refer to "Script_Sample_CounterSimple.cs" before this sample.       */
	#endregion Notes

	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	/* Target Animation-Object */
	public GameObject GameObjectRoot;

	/* Value */
	public int Value;

	/* Form */
	public bool FlagPaddingZero;
	public int IndexFont;
	public int SizePixelFontX;
	public bool FlagProportional;
	public bool FlagChangeColor;
	public bool FlagVibration;

	/* WorkArea */
	private Script_SpriteStudio6_Root ScriptRoot = null;
	private Library_SpriteStudio6.Control.AdditionalColor AdditionalColor = null;

	private int[] TableIndexAnimation = new int[(int)KindAnimation.TERMINATOR];
	private InformationPartsDigit[] TablePartsDigit = new InformationPartsDigit[(int)Constant.DIGIT_MAX];
	private InformationCellCharacter[,] TableCellCharacter = new InformationCellCharacter[(int)Constant.FONT_MAX, (int)KindCharacter.TERMINATOR];

	private int ValuePrevious = int.MaxValue;
	private bool FlagPaddingZeroPrevious;
	private int IndexFontPrevious = -1;
//	private int SizePixelFontXPrevious;
	private bool FlagProportionalPrevious;
	private bool FlagChangeColorPrevious;

	private KindAnimation KindAnimationPlay = KindAnimation.NON;

	private bool FlagInitialized = false;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
	void Start()
	{
		/* Initialize WorkArea */
		FlagPaddingZeroPrevious = !FlagPaddingZero;	/* Since this value can not be set with initializer... */

		/* Get Animation Control Script-Component */
		if(null == GameObjectRoot)
		{	/* Error */
			return;
		}
		ScriptRoot = Script_SpriteStudio6_Root.Parts.RootGetChild(GameObjectRoot);
		if(null == ScriptRoot)
		{	/* Error */
			return;
		}

		/* Get Animation Index */
		TableIndexAnimation = AnimationIndexGet();
		if(null == TableIndexAnimation)
		{
			return;
		}

		/* Animation Start */
		KindAnimationPlay = (true == FlagVibration) ? KindAnimation.ANIMATION_VIBRATION : KindAnimation.ANIMATION_NOMOVE;
		AnimationPlay();

		/* Get Digit's Animation Parts */
		TablePartsDigit = PartsInformationGet();
		if(null == TablePartsDigit)
		{
			return;
		}

		/* Get Cell Information */
		TableCellCharacter = CellInformationGet();
		if(null == TableCellCharacter)
		{
			return;
		}

		/* Create Additional-Color Parameter */
		/* MEMO: Call "AdditionalColorRelease" when created parameter with "AdditionalColorCreate" are no longer used. */
		AdditionalColor = ScriptRoot.AdditionalColorCreate();

		/* Initialize Complete */
		FlagInitialized = true;
	}

	void Update()
	{
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region Functions
	private int[] AnimationIndexGet()
	{
		int countAnimation = (int)KindAnimation.TERMINATOR;
		int[] table = new int[countAnimation];
		if(null == table)
		{
			return(null);
		}

		for(int i=0; i<countAnimation; i++)
		{
			table[i] = ScriptRoot.IndexGetAnimation(NameAnimation[i]);
		}

		return(table);
	}

	private InformationPartsDigit[] PartsInformationGet()
	{
		int countDigit = (int)TableNameParts.Length;
		InformationPartsDigit[] table = new InformationPartsDigit[countDigit];
		if(null == table)
		{
			return(null);
		}

		int idParts;
		string nameParts;
		Transform transformRoot = ScriptRoot.transform;
		Transform transform;
		for(int i=0; i<countDigit; i++)
		{
			table[i].CleanUp();

			/* Get Part's ID */
			nameParts = TableNameParts[i];
			idParts = ScriptRoot.IDGetParts(nameParts);

			/* Get Part's Transform */
#if false
			/* MEMO: Since parts' GameObject name are the same as parts' name unless changed, "Transform.Find" can be used. */
			transform = transformRoot.Find(nameParts);
#else
			/* MEMO: If initialization of animation is guaranteed, can be get also with "ScriptRoot.InstanceTransform". */
			transform = ScriptRoot.TableControlParts[idParts].InstanceGameObject.transform;
#endif

			table[i].Set(idParts, transform);
		}

		return(table);
	}

	private InformationCellCharacter[,] CellInformationGet()
	{
		int countFont = (int)Constant.FONT_MAX;
		int countCharacter = (int)KindCharacter.TERMINATOR;

		InformationCellCharacter[,] table = new InformationCellCharacter[countFont, countCharacter];
		if(null == table)
		{
			return(null);
		}

		/* MEMO: The CellMap name is the same as SSCE name in SpriteStudio6 project. */
		/*       Since there is only 1 SSCE in sample, 0 is always returned.         */
		int indexCellMap = ScriptRoot.IndexGetCellMap("BitmapFontNumeric_Complex");
		if(0 > indexCellMap)
		{	/* Not found */
			table = null;
			return(null);
		}
		Library_SpriteStudio6.Data.CellMap cellMap = ScriptRoot.CellMapGet(indexCellMap);
		if(null == cellMap)
		{	/* Invalid index */
			table = null;
			return(null);
		}

		int indexCell;
		for(int i=0; i<countFont; i++)
		{
			for(int j=0; j<countCharacter; j++)
			{
				table[i, j].CleanUp();

				/* MEMO: The cell's index stored in CellMap can be gotten from CellMap. */
				indexCell = cellMap.IndexGetCell(TableNameCell[i][j]);

				/* MEMO: Since caution-points when get Cell's informations are special, */
				/*        annotate in separated function(InformationCellCharacter.Set). */
				table[i, j].Set(indexCell, cellMap);
			}
		}

		return(table);
	}

	private bool AnimationPlay()
	{
		if(KindAnimation.NON == KindAnimationPlay)
		{	/* Not being requested changing animation */
			return(true);
		}

		int indexAnimation = TableIndexAnimation[(int)KindAnimationPlay];
		if(0 > indexAnimation)
		{	/* Invalid Animation's index */
			return(false);
		}

		ScriptRoot.AnimationPlay(-1, indexAnimation, 0);	/* play infinite */
		KindAnimationPlay = KindAnimation.NON;

		return(true);
	}

	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	/* Number of digits, maximum-value and minimum-values */
	private enum Constant
	{
		DIGIT_MAX = 8,
		FONT_MAX = 4,
	};

	private readonly static int ValueMax = (int)(Mathf.Pow(10.0f, (int)Constant.DIGIT_MAX)) - 1;
	private readonly static int ValueMin = -((int)(Mathf.Pow(10.0f, (int)Constant.DIGIT_MAX - 1)) - 1);

	/* Animations */
	private enum KindAnimation
	{
		NON = -1,

		ANIMATION_NOMOVE = 0,
		ANIMATION_VIBRATION,

		TERMINATOR
	}
	private readonly static string[] NameAnimation = new string[(int)KindAnimation.TERMINATOR]
	{
		"NoMove",
		"Vibration"
	};

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
	};
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
	private static readonly string[][] TableNameCell = new string[(int)Constant.FONT_MAX][]
	{
		new string[(int)KindCharacter.TERMINATOR]	/* Font-0 */
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
		},
		new string[(int)KindCharacter.TERMINATOR]	/* Font-1 */
		{
			"Font2_0",
			"Font2_1",
			"Font2_2",
			"Font2_3",
			"Font2_4",
			"Font2_5",
			"Font2_6",
			"Font2_7",
			"Font2_8",
			"Font2_9",
			"Font2_Period",
			"Font2_Comma",
			"Font2_Plus",
			"Font2_Minus",
			"Font2_Mul",
			"Font2_Div",
		},
		new string[(int)KindCharacter.TERMINATOR]	/* Font-2 */
		{
			"Font3_0",
			"Font3_1",
			"Font3_2",
			"Font3_3",
			"Font3_4",
			"Font3_5",
			"Font3_6",
			"Font3_7",
			"Font3_8",
			"Font3_9",
			"Font3_Period",
			"Font3_Comma",
			"Font3_Plus",
			"Font3_Minus",
			"Font3_Mul",
			"Font3_Div",
		},
		new string[(int)KindCharacter.TERMINATOR]	/* Font-3 */
		{
			"Font4_0",
			"Font4_1",
			"Font4_2",
			"Font4_3",
			"Font4_4",
			"Font4_5",
			"Font4_6",
			"Font4_7",
			"Font4_8",
			"Font4_9",
			"Font4_Period",
			"Font4_Comma",
			"Font4_Plus",
			"Font4_Minus",
			"Font4_Mul",
			"Font4_Div",
		},
	};

	/* Overlay-Color */
	private readonly static Color32[] TableColorFont = new Color32[]
	{
		new Color(0.0f, 0.0f, 1.0f, 1.0f),	/* Blue */
		new Color(1.0f, 0.0f, 0.0f, 1.0f),	/* Red */
		new Color(1.0f, 0.0f, 1.0f, 1.0f),	/* Violet */
		new Color(0.0f, 1.0f, 0.0f, 1.0f),	/* Green */
		new Color(0.0f, 1.0f, 1.0f, 1.0f),	/* Cian */
		new Color(1.0f, 1.0f, 0.0f, 1.0f),	/* Yellow */
	};
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	private struct InformationPartsDigit
	{
		internal int IDParts;
		internal UnityEngine.Transform Transform;

		internal bool IsValid
		{
			get
			{
				if((0 > IDParts) || (null == Transform))
				{
					return(false);
				}
				return(true);
			}
		}

		internal void CleanUp()
		{
			IDParts = -1;
			Transform = null;
		}

		internal void Set(int idParts, UnityEngine.Transform transform)
		{
			IDParts = idParts;
			Transform = transform;
		}
	}

	private struct InformationCellCharacter
	{
		internal int IndexCell;
		internal Vector2 SizeCell;

		internal void CleanUp()
		{
			IndexCell = -1;
			SizeCell = Vector2.zero;
		}

		internal bool IsValid
		{
			get
			{
				if(0 > IndexCell)
				{
					return(false);
				}
				return(true);
			}
		}

		internal void Set(int indexCell, Library_SpriteStudio6.Data.CellMap cellMap)
		{
			if(null == cellMap)
			{	/* Error */
				CleanUp();
				return;
			}

			if((0 > indexCell) || (cellMap.CountGetCell() <= indexCell))
			{	/* Invalid Cell-index */
				CleanUp();
				return;
			}

			IndexCell = indexCell;
			/* MEMO: Cell's Information in CellMap can be get with TableCell[cell's index].        */
			/*       Do not change the contents of TableCell without reason.                       */
			/*       Excepting deep-copied or built-from-scratch, will overwrite master-data       */
			/*        of ScriptableObject(Script_SpriteStudio6_DataCellMap) which stores CellMaps. */
			SizeCell.x = cellMap.TableCell[indexCell].Rectangle.width;
			SizeCell.y = cellMap.TableCell[indexCell].Rectangle.height;
		}
	}
	#endregion Classes, Structs & Interfaces
}
