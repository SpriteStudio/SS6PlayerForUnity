/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

#define COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[System.Serializable]
public partial class Script_SpriteStudio6_RootUnityNative : MonoBehaviour
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public int SortingOrder;

	public int CountDrawPartsMax;
	public int SortingOffsetPartsDraw;
	public Transform[] TableTransformBonePoint;

#if COMPILEOPTION_BUFFERING_LOCAL_UNITYNATIVE
	/* MEMO: Do not change these variables externally. (Buffers "Writing from AnimationClip" and "Storing Initial-State") */
	public Matrix4x4[] TableMatrixBoneSetup;
#else
#endif
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
//	void Awake()
//	{
//	}

//	void Start()
//	{
//	}

//	void Update()
//	{
//	}

//	void LateUpdate()
//	{
//	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	#endregion Functions

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	/* Class for getting UserData in bulk. (New method) */
	/* MEMO: To use this class, the "Integrate "UserData" event Callbak-Function" in the importer must be checked (true). */
	/*       Operation of this class is mostly as same as "Library_SpriteStudio6.Data.Animation.Attribute.UserData".      */
	/* MEMO: To receive events, the following function must be added to the "Script_SpriteStudio6_RootUnityNative" class.      */
	/*                                                                                                                         */
	/*       public void FunctionEventUserData(string value)                                                                   */
	/*       {                                                                                                                 */
	/*           Script_SpriteStudio6_RootUnityNative.UserData userData = new Script_SpriteStudio6_RootUnityNative.UserData(); */
	/*           userData.DeserializeJSON(value);                                                                              */
	/*                                                                                                                         */
	/*           if(true == userData.IsNumber)                                                                                 */
	/*           {                                                                                                             */
	/*               int valueInt = userData.NumberInt;                                                                        */
	/*           }                                                                                                             */
	/*                                                                                                                         */
	/*           if(true == userData.IsCoordinate)                                                                             */
	/*           {                                                                                                             */
	/*               Vector2 valueCoordinate = userData.Coordinate;                                                            */
	/*           }                                                                                                             */
	/*                                                                                                                         */
	/*           if(true == userData.IsRectangle)                                                                              */
	/*           {                                                                                                             */
	/*               Rect valueRectangle = userData.Rectangle;                                                                 */
	/*           }                                                                                                             */
	/*                                                                                                                         */
	/*           if(true == userData.IsText)                                                                                   */
	/*           {                                                                                                             */
	/*               string valueText = userData.Text;                                                                         */
	/*           }                                                                                                             */
	/*       }                                                                                                                 */
	[System.Serializable]
	public partial class UserData
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		public FlagBit Flags;
		public int NumberInt;
		public Rect Rectangle;
		public Vector2 Coordinate;
		public string Text;

		public bool IsValid
		{
			get
			{
				return(0 != (Flags & FlagBit.VALID));
			}
		}
		public bool IsNumber
		{
			get
			{
				return(0 != (Flags & FlagBit.NUMBER));
			}
		}
		public bool IsRectangle
		{
			get
			{
				return(0 != (Flags & FlagBit.RECTANGLE));
			}
		}
		public bool IsCoordinate
		{
			get
			{
				return(0 != (Flags & FlagBit.COORDINATE));
			}
		}
		public bool IsText
		{
			get
			{
				return(0 != (Flags & FlagBit.TEXT));
			}
		}
		public uint Number
		{
			get
			{
				return((uint)NumberInt);
			}
		}
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public UserData()
		{
			CleanUp();
		}
		public UserData(FlagBit flags, int numberInt, Rect rectangle, Vector2 coordinate, string text)
		{
			Flags = flags;
			NumberInt = numberInt;
			Rectangle = rectangle;
			Coordinate = coordinate;
			Text = text;
		}

		public void CleanUp()
		{
			Flags = FlagBit.CLEAR;
			NumberInt = 0;
			Rectangle = Rect.zero;
			Coordinate = Vector2.zero;
			Text = "";
		}

		public static UserData CreateFromJSON(string json)
		{
			return(JsonUtility.FromJson<UserData>(json));
		}

		public void DeserializeJSON(string json)
		{
			JsonUtility.FromJsonOverwrite(json, this);
		}
		public string SerializeJSON()
		{
			return(JsonUtility.ToJson(this));
		}
		#endregion Functions

		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		[System.Flags]
		public enum FlagBit
		{
			VALID = 0x40000000,

			COORDINATE = 0x00000004,
			TEXT = 0x00000008,
			RECTANGLE = 0x00000002,
			NUMBER = 0x00000001,

			CLEAR = 0x00000000,
		}
		#endregion Functions
	}

	/* Conventional method for UserData.Rectangle (Added same method of "Ver. 2.1.11 or earlier") */
	/* MEMO: To receive events, the following function must be added to the "Script_SpriteStudio6_RootUnityNative" class.                         */
	/*       public void FunctionEventRectangle(string value)                                                                                     */
	/*       {                                                                                                                                    */
	/*           Script_SpriteStudio6_RootUnityNative.UserDataRectangle rectangle = new Script_SpriteStudio6_RootUnityNative.UserDataRectangle(); */
	/*           rectangle.DeserializeJSON(value);                                                                                                */
	/*                                                                                                                                            */
    /*       }                                                                                                                                    */
	[System.Serializable]
	public class UserDataRectangle
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		public int Left;
		public int Right;
		public int Top;
		public int Bottom;
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public UserDataRectangle()
		{
			CleanUp();
		}
		public UserDataRectangle(int left, int right, int top, int bottom)
		{
			Left = left;
			Right = right;
			Top = top;
			Bottom = bottom;
		}

		public void CleanUp()
		{
			Left = 0;
			Right = 0;
			Top = 0;
			Bottom = 0;
		}

		public void RectGet(ref UnityEngine.Rect rectangle)
		{
			rectangle.x = (float)Left;
			rectangle.y = (float)Top;
			rectangle.width = (float)(Right - Left);
			rectangle.height = (float)(Bottom - Top);
		}

		public static UserDataRectangle CreateFromJSON(string json)
		{
			return(JsonUtility.FromJson<UserDataRectangle>(json));
		}

		public void DeserializeJSON(string json)
		{
			JsonUtility.FromJsonOverwrite(json, this);
		}
		public string SerializeJSON()
		{
			return(JsonUtility.ToJson(this));
		}
		#endregion Functions
	}

	/* Conventional method for UserData.Coordinate (Added same method of "Ver. 2.1.11 or earlier") */
	/* MEMO: To receive events, the following function must be added to the "Script_SpriteStudio6_RootUnityNative" class.                            */
	/*       public void FunctionEventCoordinate(string value)                                                                                       */
	/*       {                                                                                                                                       */
	/*           Script_SpriteStudio6_RootUnityNative.UserDataCoordinate coordinate = new Script_SpriteStudio6_RootUnityNative.UserDataCoordinate(); */
	/*           coordinate.DeserializeJSON(value);                                                                                                  */
	/*                                                                                                                                               */
    /*       }                                                                                                                                       */
	[System.Serializable]
	public class UserDataCoordinate
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		public int X;
		public int Y;
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public UserDataCoordinate()
		{
			CleanUp();
		}
		public UserDataCoordinate(int x, int y)
		{
			X = x;
			Y = y;
		}

		public void CleanUp()
		{
			X = 0;
			Y = 0;
		}

		public void Vector2Get(ref UnityEngine.Vector2 vector2)
		{
			vector2.x = (float)X;
			vector2.y = (float)Y;
		}

		public static UserDataCoordinate CreateFromJSON(string json)
		{
			return(JsonUtility.FromJson<UserDataCoordinate>(json));
		}

		public void DeserializeJSON(string json)
		{
			JsonUtility.FromJsonOverwrite(json, this);
		}
		public string SerializeJSON()
		{
			return(JsonUtility.ToJson(this));
		}
		#endregion Functions
	}
	#endregion Classes, Structs & Interfaces
}
