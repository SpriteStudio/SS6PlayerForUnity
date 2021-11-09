/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Sample_CounterComplex : MonoBehaviour
{
	/* ----------------------------------------------- Notes */
	#region Notes
	/* ---------------------------------- Sample "Complex Counter" [Beginner] */
	/* The points of this sample are as follows.                              */
	/*                                                                        */
	/* - Tips to change animation                                             */
	/* - How to use AdditionalColor                                           */
	/* - How to use UserData-callback                                         */
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
	private int IndexColor = 0;

	private int[] TableIndexAnimation = new int[(int)KindAnimation.TERMINATOR];
	private InformationPartsDigit[] TablePartsDigit = new InformationPartsDigit[(int)Constant.DIGIT_MAX];
	private InformationCellCharacter[,] TableCellCharacter = new InformationCellCharacter[(int)Constant.FONT_MAX, (int)KindCharacter.TERMINATOR];

	private int ValuePrevious = int.MaxValue;
	private bool FlagPaddingZeroPrevious;
	private int IndexFontPrevious = -1;
//	private int SizePixelFontXPrevious;
	private bool FlagProportionalPrevious;
	private bool FlagChangeColorPrevious;
	private bool FlagVibrationPrevious;
	private int IndexColorPrevious = -1;

	private KindAnimation KindAnimationPlay = KindAnimation.NON;

	private bool FlagInitialized = false;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
	void Start()
	{
		/* Initialize WorkArea */
		FlagPaddingZeroPrevious = !FlagPaddingZero;	/* Since this value can not be set with initializer... */
		FlagProportionalPrevious = !FlagProportional;
		FlagChangeColorPrevious = !FlagChangeColorPrevious;
		FlagVibrationPrevious = !FlagVibrationPrevious;

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

		/* Set UserData callback function */
		/* MEMO: Set up function to handle UserData callback.                                     */
		/*       To avoid receiving UserData callback, set "ScriptRoot.FunctionUserData" to null. */
		ScriptRoot.FunctionUserData = FunctionCallBackUserData;

		/* Get Animation Index */
		/* MEMO: Collect animation-indexs ahead of time.                 */
		/*       Every change animation, also you can get index by name. */
		/*       But wasteful to search each time.                       */
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
		/* MEMO: When position refresh occurs at animation initialize, refresh is prohibited                  */
		/*        to prevent overwriting position of digits' animation parts set at Update() with refreshing. */
		int countPartsDigit = TablePartsDigit.Length;
		int idParts;
		for(int i=0; i<countPartsDigit; i++)
		{
			idParts = TablePartsDigit[i].IDParts;
			if(0 <= idParts)
			{
				ScriptRoot.RefreshCancelTransform(idParts, true, false, false);
			}
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
		/* Check Validity */
		if(false == FlagInitialized)
		{	/* Failed to initialize */
			return;
		}

		/* Adjust Parameters */
		if(0 >= SizePixelFontX)
		{
			SizePixelFontX = (int)Constant.FONTSIZE_DEFAULT;
		}

		if(0 > IndexFont)
		{
			IndexFont = 0;
		}
		if((int)Constant.FONT_MAX <= IndexFont)
		{
			IndexFont = (int)Constant.FONT_MAX - 1;
		}

		/* Clamp Value */
		int valueDisplay = Mathf.Clamp(Value, ValueMin, ValueMax);

		/* Check & Update Animation */
		if(FlagVibrationPrevious != FlagVibration)
		{
			/* MEMO: Simply, If animation-change is reserved, postpone to update.                                    */
			/*       This process does not matter because depend on the convenience for application's specification. */
			if(KindAnimation.NON == KindAnimationPlay)
			{	/* Not reserved */
				KindAnimationPlay = (true == FlagVibration) ? KindAnimation.ANIMATION_VIBRATION : KindAnimation.ANIMATION_NOMOVE;
				FlagVibrationPrevious = FlagVibration;
			}
			AnimationPlay();
		}

		/* Check & Update AdditionalColor */
		/* MEMO: The reason for checking two types of updates, "IndexColor" and "FlagChangeColor",                                   */
		/*        is that "FlagChangeColor" is updated from inspector and "IndexColor" is updated in the UserData-callback function. */
		if((FlagChangeColorPrevious != FlagChangeColor) || (IndexColorPrevious != IndexColor))
		{
			FlagChangeColorPrevious = FlagChangeColor;
			IndexColorPrevious = IndexColor;

			if(null != AdditionalColor)
			{
				/* MEMO: Created "AdditionalColor"'s Parameter can continue to be used unless call "AdditionalColorRelease". */
				/*       To temporarily-disable AdditionalColor, set "Library_SpriteStudio6.KindOperationBlend.NON".         */
				AdditionalColor.SetOverall((true == FlagChangeColor) ? Library_SpriteStudio6.KindOperationBlend.MUL : Library_SpriteStudio6.KindOperationBlend.NON,
											TableColorFont[IndexColor]
										);
			}
		}

		/* Check Update */
		bool flagUpdateText = false;
		if(ValuePrevious != Value)
		{
			ValuePrevious = Value;
			flagUpdateText |= true;
		}
		if(IndexFontPrevious != IndexFont)
		{
			IndexFontPrevious = IndexFont;
			flagUpdateText |= true;
		}
		if(FlagPaddingZeroPrevious != FlagPaddingZero)
		{
			FlagPaddingZeroPrevious = FlagPaddingZero;
			flagUpdateText |= true;
		}
//		if(SizePixelFontXPrevious != SizePixelFontX)
//		{
//			SizePixelFontXPrevious = SizePixelFontX;
//			flagUpdateText |= true;
//		}
		if(FlagProportionalPrevious != FlagProportional)
		{
			FlagProportionalPrevious = FlagProportional;
			flagUpdateText |= true;
		}

		/* Update Text */
		if(true == flagUpdateText)
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
				int positionPixelDigit = 0;
				for(int i=0; i<countDigit; i++)
				{
					/* MEMO: Since "idParts == 0" is the "Root"-part, intentionally excluded.           */
					/*       Setting HideSet's idParts to 0 is to control hidding the entire animation. */
					idParts = TablePartsDigit[i].IDParts;
					if(0 < idParts)
					{
						/* Change Cell */
						/* MEMO: (IndexCellMap == 0) Because this Animation has 1 Texture. */
						indexCharacter = IndexGetCharacter(charactersDigit[(countDigit - 1) - i]);
						ScriptRoot.CellChangeParts(	idParts,
													0,
													TableCellCharacter[IndexFont, indexCharacter].IndexCell,
													Library_SpriteStudio6.KindIgnoreAttribute.PERMANENT	/* Ignore "Reference-Cell" attribute even if change animation */
												);

						/* Show Digit */
						/* MEMO: Don't Effect to children */
						ScriptRoot.HideSet(idParts, false, false);

						/* Set Digit's position */
						{
							/* Get Pixel-Width */
							int pixelSpaceNow = (true == FlagProportional) ? (int)(TableCellCharacter[IndexFont, indexCharacter].SizeCell.x) : SizePixelFontX;
							pixelSpaceNow /= 2;	/* Spacing-width = (Previous digit's width + Now digit's width) / 2 */

							/* Adjust Position */
							if(0 < i)
							{	/* The 1st-digit is Fixed-Position */
								positionPixelDigit -= pixelSpaceNow;

								/* MEMO: There are conditions that must be met to successfully overwrite part's "Transform" from script. */
								/*       1. When overwrite position, "X Position", "Y Position" and "Z Position"                         */
								/*           must not have key-frame data in animation (and "Setup" animation).                          */
								/*       2. When overwrite rotation, "X-axis Rotation", "Y-axis Rotation" and "Z-axis Rotation"          */
								/*           must not have key-frame data in animation (and "Setup" animation).                          */
								/*       3. When overwrite scaling, "X Scale" and "Y Scale"                                              */
								/*           must not have key-frame data in animation (and "Setup" animation).                          */
								/*           ("Local X Scale" and "Local Y Scale" can have key-frame datas)                              */
								Transform transformDigit = TablePartsDigit[i].Transform;
								Vector3 localPositionDigit = transformDigit.localPosition;
								localPositionDigit.x = (float)positionPixelDigit;
								transformDigit.localPosition = localPositionDigit;
							}
							positionPixelDigit -= pixelSpaceNow;
						}
					}
				}
				for(int i=countDigit; i<(int)Constant.DIGIT_MAX; i++)
				{
					idParts = TablePartsDigit[i].IDParts;
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
			/*       However, there are case when GameObject of same name is added, so not recommended much.                */
			transform = transformRoot.Find(nameParts);
#else
			/* MEMO: If initialization of animation is guaranteed, can be get also with "ScriptRoot.InstanceTransform". */
			transform = ScriptRoot.TableControlParts[idParts].InstanceGameObject.transform;
#endif

			table[i].IDParts = idParts;
			table[i].Transform = transform;
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
				if((0 <= indexCell) || (cellMap.CountGetCell() > indexCell))
				{	/* Valid Cell-index */
					table[i, j].IndexCell = indexCell;

					/* MEMO: Cell's Information in CellMap can be get with TableCell[cell's index].        */
					/*       Do not change the contents of TableCell without reason.                       */
					/*       Excepting deep-copied or built-from-scratch, will overwrite master-data       */
					/*        of ScriptableObject(Script_SpriteStudio6_DataCellMap) which stores CellMaps. */
					table[i, j].SizeCell.x = cellMap.TableCell[indexCell].Rectangle.width;
					table[i, j].SizeCell.y = cellMap.TableCell[indexCell].Rectangle.height;
				}

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

	private void FunctionCallBackUserData(	Script_SpriteStudio6_Root scriptRoot,
											string nameParts,
											int indexParts,
											int indexAnimation,
											int frameDecode,
											int frameKeyData,
											ref Library_SpriteStudio6.Data.Animation.Attribute.UserData userData,
											bool flagWayBack
										)
	{
		/* MEMO: "UserData"-callback are execute during animation updating process.                       */
		/*       Therefore, do not change playing-status within "UserData"- callback processing function. */
		/*       As much as possible, please caching values for the next operation                        */
		/*        and perform processing with the next "MonoBehaviour.Update()".                          */
		/* MEMO: "UserData"-callback is processed in the order of "1. part" and "2. frame".            */
		/*       When many frames are skipped, please pay attention to the order of callback.          */
		/*       (In parts by parts, Will be called back according to the order of the played-frames.) */
		/* MEMO: To be honest, it is not recommended to place "UserData" in many parts.                                    */
		/*       As much as possible recommend that you put "UserData" together in 1 part. (Can also get good performance) */
		/* MEMO: Do not place "UserData" in "Instance" animations. */
		/*       (Will be ignored)                                 */
		/* MEMO: "userData" is a shallow-copy of UserData in animation's master(original)-data.                                            */
		/*       Therefore, overwriting "userData" risks affecting animations that use same master-data, so do not overwrite in principle. */
		/*       (Since "userData.Text" is a "string", pay particular attention)                                                           */
		/* MEMO: "userData" is original-data's shallow-copy, so can not guarantee that same "reference".      */
		/*       Therefore, when judging whether same "UserData", judge by compare "scriptRoot","indexParts", */
		/*        "indexAnimation" and "frameKeyData".                                                        */
		/* MEMO: "flagWayBack" becomes true when direction-play is "way-back" during ping-pong(round-trip) playing. */
		/*       Always false during normal(one-way) playing.                                                       */

		/* MEMO: If you know exactly what values are stored in the "UserData",                                          */
		/*        you can directly retrieve values from "NumberInt", "Rectangle", "Coordinate" or "Text".               */
		/*       But usually "IsNumber", "IsRectangle"," IsCoordinate" or "IsText" are used to check values are stored. */
		if((true == userData.IsText) && ("ColorChange" == userData.Text))
		{
			IndexColor++;
			IndexColor %= TableColorFont.Length;
		}
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	/* Number of digits, maximum-value and minimum-values */
	private enum Constant
	{
		DIGIT_MAX = 8,
		FONT_MAX = 4,
		FONTSIZE_DEFAULT = 28,
	}

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

		internal void CleanUp()
		{
			IDParts = -1;
			Transform = null;
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
	}
	#endregion Classes, Structs & Interfaces
}
