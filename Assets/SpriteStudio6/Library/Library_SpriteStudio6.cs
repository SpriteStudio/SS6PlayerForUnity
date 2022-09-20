/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

// #define STATICDATA_DUPLICATE_DEEP
// #define EXPERIMENT_FOR_CAMERA

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Library_SpriteStudio6
{
	/* ----------------------------------------------- Signatures */
	#region Signatures
	public const string SignatureNameAsset = "SpriteStudio6 Player for Unity";
	public const string SignatureVersionAsset = "2.1.8";
	public const string SignatureNameDistributor = "CRI Middleware Co., Ltd.";
	#endregion Signatures

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindOperationBlend
	{
		TERMINATOR_TABLEMATERIAL = TERMINATOR - INITIATOR,	/* - (-x) = +(x) */
		TERMINATOR_BUILTIN = (INV + 1),	/* SpriteStudio6's built-in blending methods. */
		TERMINATOR_PARTSCOLOR = MUL_NA,

		INITIATOR = MASK_PRE,

		MASK_PRE = -2,
		MASK = -1,
		NON = -1,	/* for Control.AdditionalColor */

		MIX = 0,
		ADD,
		SUB,
		MUL,

		MUL_NA,
		SCR,
		EXC,
		INV,

		TERMINATOR,
	}

	public enum KindOperationBlendEffect
	{
		TERMINATOR_TABLEMATERIAL = TERMINATOR - INITIATOR,	/* - (-x) = +(x) */
		TERMINATOR_BUILTIN = (ADD + 1),	/* SpriteStudio6's built-in blending methods. */

		INITIATOR = MIX,

		NON = -1,

		MIX = 0,
		ADD,

		TERMINATOR,
	}

	public enum KindBoundBlend
	{
		NON = 0,
		OVERALL,
		VERTEX
	}

	public enum KindVertex
	{
		LU = 0,	/* Left-Up (TRIANGLE2 & TRIANGLE4) */
		RU,	/* Right-Up (TRIANGLE2 & TRIANGLE4) */
		RD,	/* Right-Down (TRIANGLE2 & TRIANGLE4) */
		LD,	/* Left-Down (TRIANGLE2 & TRIANGLE4) */
		C,	/* Center (TRIANGLE4) */

		TERMINATOR,
		TERMINATOR4 = TERMINATOR,	/* for "Normal(Sprite)" */
		TERMINATOR3 = LD,			/* for "Mesh" */
		TERMINATOR2 = C				/* for "Effect" */
	}

	public enum KindStylePlay
	{
		NO_CHANGE = -1,
		NORMAL = 0,
		PINGPONG = 1,
	}

	public enum KindIgnoreAttribute
	{
		NON = 0,
		NOW_ANIMATION,
		PERMANENT,
	}

	public enum KindMasking
	{
		THROUGH = 0,
		MASK,

		TERMINATOR,
		FOLLOW_DATA = TERMINATOR,
	}

	public enum KindSituationTimeline
	{
		START = 0,
		END,
		UPDATE,
	}
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	public static partial class CallBack
	{
		/* ----------------------------------------------- Delegates */
		#region Delegates
		public delegate bool FunctionPlayEnd(Script_SpriteStudio6_Root scriptRoot, GameObject objectControl);
		public delegate bool FunctionPlayEndEffect(Script_SpriteStudio6_RootEffect scriptRoot);
		public delegate bool FunctionPlayEndSequence(Script_SpriteStudio6_Sequence scriptSequence);
		public delegate void FunctionUserData(Script_SpriteStudio6_Root scriptRoot, string nameParts, int indexParts, int indexAnimation, int frameDecode, int frameKeyData, ref Library_SpriteStudio6.Data.Animation.Attribute.UserData userData, bool flagWayBack);
		public delegate void FunctionSignal(Script_SpriteStudio6_Root scriptRoot, string nameParts, int indexParts, int indexAnimation, int frameDecode, int frameKeyData, ref Library_SpriteStudio6.Data.Animation.Attribute.Signal signal, bool flagWayBack);

		public delegate void FunctionCallBackCollider(Script_SpriteStudio6_Root instanceRoot, GameObject instanceGameObject, string nameParts, int idParts, Library_SpriteStudio6.Control.InformationCollision information);

		public delegate float FunctionTimeElapse(Script_SpriteStudio6_Root scriptRoot);
		public delegate float FunctionTimeElapseEffect(Script_SpriteStudio6_RootEffect scriptRoot);
		public delegate float FunctionTimeElapseSequence(Script_SpriteStudio6_Sequence scriptSequence);

		public delegate void FunctionControlEndTrackPlay(Script_SpriteStudio6_Root scriptRoot, int indexTrackPlay, int indexTrackSecondary, int indexAnimation, int indexAnimationSecondary);

		public delegate int FunctionDecodeStepSequence(ref Library_SpriteStudio6.Data.Sequence.Data.Step dataStep, Script_SpriteStudio6_Sequence scriptSequence, int step);

		public delegate UnityEngine.Material FunctionMaterialSetUp(UnityEngine.Material material, int operationBlend, Library_SpriteStudio6.KindMasking masking, bool flagZWrite);

		public delegate bool FunctionTimeline(Script_SpriteStudio6_Root scriptRoot, KindSituationTimeline situation, float timeElapsed, double timeLocal);
		public delegate bool FunctionTimelineEffect(Script_SpriteStudio6_RootEffect scriptRoot, KindSituationTimeline situation, float timeElapsed, double timeLocal);
		public delegate bool FunctionTimelineSequence(Script_SpriteStudio6_Sequence scriptSequence, KindSituationTimeline situation, float timeElapsed, double timeLocal);
		public delegate bool FunctionTimelineReplicate(Script_SpriteStudio6_Replicate scriptReplicate, KindSituationTimeline situation, float timeElapsed, double timeLocal);
		#endregion Delegates
	}

	public static partial class Data
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		[System.Serializable]
		public partial class Animation
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public string Name;
			public int FramePerSecond;
			public int CountFrame;

			public int SizeCanvasX;
			public int SizeCanvasY;

			public int FrameValidStart;
			public int FrameValidEnd;
			public int CountFrameValid;

			public int DepthIK;
			public KindModeSort ModeSort;

			public Label[] TableLabel;
			public Parts[] TableParts;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public void CleanUp()
			{
				Name = "";
				FramePerSecond = 0;
				CountFrame = 0;

				SizeCanvasX = 1;
				SizeCanvasY = 1;

				FrameValidStart = 0;
				FrameValidEnd = 0;
				CountFrameValid = 0;

				DepthIK = 0;
				ModeSort = KindModeSort.PRIORITY;

				TableLabel = null;
				TableParts = null;
			}

			public int CountGetLabel()
			{
				return((null == TableLabel) ? 0 : TableLabel.Length);
			}

			public int IndexGetLabel(string name)
			{
				if((true == string.IsNullOrEmpty(name)) || (null == TableLabel))
				{
					return(-1);
				}

				int count;
				count = (int)Label.KindLabelReserved.TERMINATOR;
				for(int i=0; i<count; i++)
				{
					if(name == Label.TableNameLabelReserved[i])
					{
						return((int)Label.KindLabelReserved.INDEX_RESERVED + i);
					}
				}

				count = TableLabel.Length;
				for(int i=0; i<count; i++)
				{
					if(name == TableLabel[i].Name)
					{
						return(i);
					}
				}
				return(-1);
			}

			public int FrameGetLabel(int index)
			{
				if((int)Label.KindLabelReserved.INDEX_RESERVED <= index)
				{	/* Reserved-Index */
					index -= (int)Label.KindLabelReserved.INDEX_RESERVED;
					switch(index)
					{
						case (int)Label.KindLabelReserved.START:
							return(0);

						case (int)Label.KindLabelReserved.END:
							return(CountFrame - 1);

						default:
							break;
					}
					return(-1);
				}

				if((0 > index) || (TableLabel.Length <= index))
				{
					return(-1);
				}
				return(TableLabel[index].Frame);
			}

			public string NameGetLabel(int index)
			{
				if((int)Label.KindLabelReserved.INDEX_RESERVED <= index)
				{	/* Reserved-Index */
					index -= (int)Label.KindLabelReserved.INDEX_RESERVED;
					if((0 > index) || ((int)Label.KindLabelReserved.TERMINATOR <= index))
					{	/* Error */
						return(null);
					}
					return(Label.TableNameLabelReserved[index]);
				}

				if((0 > index) || (TableLabel.Length <= index))
				{
					return(null);
				}
				return(TableLabel[index].Name);
			}

			public int CountGetParts()
			{
				return((null == TableParts) ? 0 : TableParts.Length);
			}

			public int IndexGetParts(int indexParts)
			{
				return(((0 > indexParts) || (TableParts.Length <= indexParts)) ? -1 : indexParts);
			}

			public void FrameRangeGet(	out int frameRangeStart, out int frameRangeEnd,
										string labelStart, int frameOffsetStart,
										string labelEnd, int frameOffsetEnd
									)
			{
				int frameStart = FrameValidStart;	/* 0 */
				int frameEnd = FrameValidEnd;	/* CountFrame - 1 */
				string label;
				int indexLabel;

				/* Get Start frame */
				label = (true == string.IsNullOrEmpty(labelStart))
						? Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START]
						: labelStart;
				indexLabel = IndexGetLabel(label);
				if(0 > indexLabel)
				{	/* Not found */
					indexLabel = (int)(Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START | Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.INDEX_RESERVED);
				}

				frameRangeStart = FrameGetLabel(indexLabel);
				if(frameStart > frameRangeStart)
				{
					frameRangeStart = frameStart;
				}
				frameRangeStart += frameOffsetStart;
				if((frameStart > frameRangeStart) || (frameEnd < frameRangeStart))
				{
					frameRangeStart = frameStart;
				}

				/* Get End frame */
				label = (true == string.IsNullOrEmpty(labelEnd))
						? Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END]
						: labelEnd;
				indexLabel = IndexGetLabel(label);
				if(0 > indexLabel)
				{	/* Not found */
					indexLabel = (int)(Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END | Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.INDEX_RESERVED);
				}

				frameRangeEnd = FrameGetLabel(indexLabel);
				if(frameStart > frameRangeEnd)
				{
					frameRangeEnd = frameEnd;
				}
				frameRangeEnd += frameOffsetEnd;
				if((frameStart > frameRangeEnd) || (frameEnd < frameRangeEnd))
				{
					frameRangeEnd = frameEnd;
				}
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public enum KindModeSort
			{
				PRIORITY = 0,				/* Attribute "Priority" */
				POSITION_Z,					/* Transformed Z-Position */
			}
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			[System.Serializable]
			public partial struct Parts
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public FlagBitStatus StatusParts;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus Status;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell Cell;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 Position;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 Rotation;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 Scaling;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 ScalingLocal;	/* used in Sprite, Mask, Instance, Effect */

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat RateOpacity;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt Priority;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor PartsColor;	/* used in Sprite, Mask (Contents different) */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection VertexCorrection;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 OffsetPivot;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 PositionAnchor;	/* (Unsupported now) */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 SizeForce;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 PositionTexture;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat RotationTexture;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 ScalingTexture;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat RadiusCollision;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData UserData;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance Instance;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect Effect;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerDeform Deform;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerShader Shader;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerSignal Signal;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					StatusParts = FlagBitStatus.CLEAR;

					Status = null;

					Cell = null;

					Position = null;
					Rotation = null;
					Scaling = null;
					ScalingLocal = null;

					RateOpacity = null;
					Priority = null;
					PartsColor = null;
					VertexCorrection = null;

					OffsetPivot = null;
					PositionAnchor = null;
					SizeForce = null;
					PositionTexture = null;
					RotationTexture = null;
					ScalingTexture = null;

					RadiusCollision = null;

					UserData = null;
					Instance = null;
					Effect = null;

					Shader = null;
					Signal = null;
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				[System.Flags]
				public enum FlagBitStatus
				{
					VALID = 0x40000000,
					NOT_USED = 0x20000000,
					HIDE_FULL = 0x10000000,

					NOT_MASKING = 0x08000000,
					/* 0x01000000, */	/* Reserved */

					NO_POSITION = 0x00800000,
					NO_ROTATION = 0x00400000,
					NO_SCALING = 0x00200000,
					NO_TRANSFORMATION_TEXTURE = 0x00100000,

					NO_USERDATA = 0x00080000,
					NO_PARTSCOLOR = 0x00040000,
					NO_INSTANCE = 0x00020000,
					NO_EFFECT = 0x00010000,

					NO_SIGNAL = 0x00008000,
					NO_SHADER = 0x00004000,

					/* 0x00000001, */	/* Reserved */
					CLEAR = 0x00000000
				}
				#endregion Enums & Constants
			}

			[System.Serializable]
			public struct Label
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public string Name;
				public int Frame;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Name = "";
					Frame = -1;
				}

				public static int NameCheckReserved(string name)
				{
					if(false == string.IsNullOrEmpty(name))
					{
						for(int i=0; i<(int)KindLabelReserved.TERMINATOR; i++)
						{
							if(name == TableNameLabelReserved[i])
							{
								return(i);
							}
						}
					}
					return(-1);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindLabelReserved
				{
					START = 0,	/* "_start" *//* START + INDEX_RESERVED */
					END,	/* "_end" *//* END + INDEX_RESERVED */

					TERMINATOR,
					INDEX_RESERVED = 0x10000000,
				}

				public readonly static string[] TableNameLabelReserved = new string[(int)KindLabelReserved.TERMINATOR]
				{
					"_start",
					"_end",
				};
				#endregion Enums & Constants
			}

			public static partial class Attribute
			{
				/* Part: SpriteStudio6/Library/Data/Animation/Attribute.cs */
			}

			public static partial class PackAttribute
			{
				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				[System.Serializable]
				public class ContainerInt : Container<int, InterfaceContainerInt> {}
				[System.Serializable]
				public class ContainerFloat : Container<float, InterfaceContainerFloat> {}
				[System.Serializable]
				public class ContainerVector2 : Container<Vector2, InterfaceContainerVector2> {}
				[System.Serializable]
				public class ContainerVector3 : Container<Vector3, InterfaceContainerVector3> {}
				[System.Serializable]
				public class ContainerStatus : Container<Library_SpriteStudio6.Data.Animation.Attribute.Status, InterfaceContainerStatus> {}
				[System.Serializable]
				public class ContainerCell : Container<Library_SpriteStudio6.Data.Animation.Attribute.Cell, InterfaceContainerCell> {}
				[System.Serializable]
				public class ContainerPartsColor : Container<Library_SpriteStudio6.Data.Animation.Attribute.PartsColor, InterfaceContainerPartsColor> {}
				[System.Serializable]
				public class ContainerVertexCorrection : Container<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection, InterfaceContainerVertexCorrection> {}
				[System.Serializable]
				public class ContainerUserData : Container<Library_SpriteStudio6.Data.Animation.Attribute.UserData, InterfaceContainerUserData> {}
				[System.Serializable]
				public class ContainerInstance : Container<Library_SpriteStudio6.Data.Animation.Attribute.Instance, InterfaceContainerInstance> {}
				[System.Serializable]
				public class ContainerEffect : Container<Library_SpriteStudio6.Data.Animation.Attribute.Effect, InterfaceContainerEffect> {}
				[System.Serializable]
				public class ContainerDeform : Container<Library_SpriteStudio6.Data.Animation.Attribute.Deform, InterfaceContainerDeform>
				{
					/* MEMO: This class has a slightly different implementation from other "Container" derived classes. */
					/*       (Has additional data that affects key-datas in common.)                                    */
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public int CountVertexMesh;	/* for Error-Check */
					public int[] TableIndexVertex;

					public bool IsValid
					{
						get
						{
							return(!((null == TableIndexVertex) || (0 >= TableIndexVertex.Length)));	/* !(true : false) == false : true */
						}
					}
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public new void CleanUp()
					{
						base.CleanUp();

						CountVertexMesh = 0;
						TableIndexVertex = null;
					}
					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					#endregion Enums & Constants

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					#endregion Classes, Structs & Interfaces
				}
				[System.Serializable]
				public class ContainerShader : Container<Library_SpriteStudio6.Data.Animation.Attribute.Shader, InterfaceContainerShader> {}
				[System.Serializable]
				public class ContainerSignal : Container<Library_SpriteStudio6.Data.Animation.Attribute.Signal, InterfaceContainerSignal> {}
				#endregion Classes, Structs & Interfaces

				/* Part: SpriteStudio6/Library/Data/Animation/PackAttributeFunction.cs */
				/* Part: SpriteStudio6/Library/Data/Animation/PackAttributeContainer.cs */
				/* Part: SpriteStudio6/Library/Data/Animation/PackAttribute_Codec/*.cs */
			}
			#endregion Classes, Structs & Interfaces
		}

		public static partial class Effect
		{
			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			[System.Serializable]
			public partial struct Emitter
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public FlagBit FlagData;

				/* Datas for Particle */
				public Library_SpriteStudio6.KindOperationBlendEffect OperationBlendTarget;
				public int IndexCellMap;
				public int IndexCell;

				public RangeFloat Angle;

				public Vector2 GravityDirectional;
				public Vector2 GravityPointPosition;
				public float GravityPointPower;

				public RangeVector2 Position;

				public RangeFloat Rotation;
				public RangeFloat RotationFluctuation;
				public float RotationFluctuationRate;
				public float RotationFluctuationRateTime;

				public RangeFloat RateTangentialAcceleration;

				public RangeVector2 ScaleStart;
				public RangeFloat ScaleRateStart;

				public RangeVector2 ScaleEnd;
				public RangeFloat ScaleRateEnd;

				public int Delay;

				public RangeColor ColorVertex;
				public RangeColor ColorVertexFluctuation;

				public float AlphaFadeStart;
				public float AlphaFadeEnd;

				public RangeFloat Speed;
				public RangeFloat SpeedFluctuation;

				public float TurnDirectionFluctuation;

				public long SeedRandom;

				/* Datas for Emitter */
				public int DurationEmitter;
				public int Interval;
				public RangeFloat DurationParticle;

//				public float PriorityParticle;
				public int CountParticleMax;
				public int CountParticleEmit;

				public int CountPartsMaximum;	/* Disuse?? */
				public PatternEmit[] TablePatternEmit;
				public int[] TablePatternOffset;
				public long[] TableSeedParticle;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					FlagData = FlagBit.CLEAR;

					OperationBlendTarget = Library_SpriteStudio6.KindOperationBlendEffect.MIX;
					IndexCellMap = -1;
					IndexCell = -1;

					DurationParticle.Main = 0.0f;
					DurationParticle.Sub = 0.0f;

					Angle.Main = 0.0f;
					Angle.Sub = 0.0f;

					GravityDirectional = Vector2.zero;
					GravityPointPosition = Vector2.zero;
					GravityPointPower = 0.0f;

					Position.Main = Vector2.zero;
					Position.Sub = Vector2.zero;

					Rotation.Main = 0.0f;
					Rotation.Sub = 0.0f;

					RotationFluctuation.Main = 0.0f;
					RotationFluctuation.Sub = 0.0f;
					RotationFluctuationRate = 0.0f;
					RotationFluctuationRateTime = 0.0f;

					RateTangentialAcceleration.Main = 0.0f;
					RateTangentialAcceleration.Sub = 0.0f;

					ScaleStart.Main = Vector2.zero;
					ScaleStart.Sub = Vector2.zero;
					ScaleRateStart.Main = 0.0f;
					ScaleRateStart.Sub = 0.0f;

					ScaleEnd.Main = Vector2.zero;
					ScaleEnd.Sub = Vector2.zero;
					ScaleRateEnd.Main = 0.0f;
					ScaleRateEnd.Sub = 0.0f;

					Delay = 0;

					ColorVertex.Main = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					ColorVertex.Sub = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					ColorVertexFluctuation.Main = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					ColorVertexFluctuation.Sub = new Color(1.0f, 1.0f, 1.0f, 1.0f);

					AlphaFadeStart = 0.0f;
					AlphaFadeEnd = 0.0f;

					Speed.Main = 0.0f;
					Speed.Sub = 0.0f;
					SpeedFluctuation.Main = 0.0f;
					SpeedFluctuation.Sub = 0.0f;

					TurnDirectionFluctuation = 0.0f;

					SeedRandom = (int)Constant.SEED_MAGIC;

					DurationEmitter = 15;
					Interval = 1;
					DurationParticle.Main = 15.0f;
					DurationParticle.Sub = 15.0f;

//					PriorityParticle = 64.0f;
					CountParticleEmit = 2;
					CountParticleMax = 32;

					CountPartsMaximum = 0;
					TablePatternEmit = null;
					TablePatternOffset = null;
					TableSeedParticle = null;
				}

				public void Duplicate(Emitter original)
				{
					FlagData = original.FlagData;

					OperationBlendTarget = original.OperationBlendTarget;
					IndexCellMap = original.IndexCellMap;
					IndexCell = original.IndexCell;

					DurationParticle = original.DurationParticle;

					Angle = original.Angle;

					GravityDirectional = original.GravityDirectional;
					GravityPointPosition = original.GravityPointPosition;
					GravityPointPower = original.GravityPointPower;

					Position = original.Position;

					Rotation = original.Rotation;
					RotationFluctuation = original.RotationFluctuation;
					RotationFluctuationRate = original.RotationFluctuationRate;
					RotationFluctuationRateTime = original.RotationFluctuationRateTime;

					RateTangentialAcceleration = original.RateTangentialAcceleration;

					ScaleStart = original.ScaleStart;
					ScaleRateStart = original.ScaleRateStart;
					ScaleEnd = original.ScaleEnd;
					ScaleRateEnd = original.ScaleRateEnd;

					Delay = original.Delay;

					ColorVertex = original.ColorVertex;
					ColorVertexFluctuation = original.ColorVertexFluctuation;

					AlphaFadeStart = original.AlphaFadeStart;
					AlphaFadeEnd = original.AlphaFadeEnd;

					Speed = original.Speed;
					SpeedFluctuation = original.SpeedFluctuation;

					TurnDirectionFluctuation = original.TurnDirectionFluctuation;
					SeedRandom = original.SeedRandom;

					DurationEmitter = original.DurationEmitter;
					Interval = original.Interval;
					DurationParticle = original.DurationParticle;

//					PriorityParticle = original.PriorityParticle;
					CountParticleMax = original.CountParticleMax;
					CountParticleEmit = original.CountParticleEmit;

					CountPartsMaximum = original.CountPartsMaximum;
					TablePatternEmit = original.TablePatternEmit;
					TablePatternOffset = original.TablePatternOffset;
					TableSeedParticle = original.TableSeedParticle;
				}

				public void TableGetPatternOffset(ref int[] dataTablePatternOffset)
				{
					int countEmitMax = CountParticleMax;
					int countEmit = CountParticleEmit;
					countEmit = (1 > countEmit) ? 1 : countEmit;

					/* Create Offset-Pattern Table */
					/* MEMO: This Table will be solved at Importing. */
					int shot = 0;
					int offset = Delay;
					int count = countEmitMax;
					dataTablePatternOffset = new int[count];
					for(int i=0; i<count; i++)
					{
						if(shot >= countEmit)
						{
							shot = 0;
							offset += Interval;
						}
						dataTablePatternOffset[i] = offset;
						shot++;
					}
				}

				public void TableGetPatternEmit(	ref PatternEmit[] tablePatternEmit,
													ref long[] tableSeedParticle,
													Library_SpriteStudio6.Utility.Random.Generator random,
													uint seedRandom
												)
				{	/* CAUTION!: Get "TablePatternOffset" before executing this function. */
					int count;
					int countEmitMax = CountParticleMax;

					List<PatternEmit> listPatternEmit = new List<PatternEmit>();
					listPatternEmit.Clear();

					int countEmit = CountParticleEmit;
					countEmit = (1 > countEmit) ? 1 : countEmit;

					/* Create Emit-Pattern Table */
					/* MEMO: This Table will be solved at Importing (at seedRandom is fixed). */
					random.InitSeed(seedRandom);
					int cycle = (int)(((float)(countEmitMax * Interval) / (float)countEmit) + 0.5f);
					count = countEmitMax * (int)Constant.LIFE_EXTEND_SCALE;
					if((int)Constant.LIFE_EXTEND_MIN > count)
					{
						count = (int)Constant.LIFE_EXTEND_MIN;
					}
					tablePatternEmit = new PatternEmit[count];
					int duration;
					for(int i=0; i<count; i++)
					{
						tablePatternEmit[i] = new PatternEmit();
						tablePatternEmit[i].IndexGenerate = i;
						duration = (int)((float)DurationParticle.Main + random.RandomFloat((float)DurationParticle.Sub));
						tablePatternEmit[i].Duration = duration;
						tablePatternEmit[i].Cycle = (duration > cycle) ? duration : cycle;
					}

					/* Create Random-Seed Table */
					/* MEMO: This Table will be solved at Importing (at seedRandom is fixed). */
					count = countEmitMax * 3;
					tableSeedParticle = new long[count];
					random.InitSeed(seedRandom);
					for(int i=0; i<count; i++)
					{
						tableSeedParticle[i] = (long)((ulong)random.RandomUint32());
					}
				}

				public int CountGetFrame()
				{
					return(DurationEmitter + (int)(DurationParticle.Main + DurationParticle.Sub));
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum Constant
				{
					SEED_MAGIC = 7573,

					LIFE_EXTEND_SCALE = 8,
					LIFE_EXTEND_MIN = 64,
				}

				[System.Flags]
				public enum FlagBit
				{
					/* for Particle */
					BASIC = 0x00000001,	/* (Reserved) */
					TANGENTIALACCELATION = 0x00000002,
					TURNDIRECTION = 0x00000004,
					SEEDRANDOM = 0x00000008,
					DELAY = 0x00000010,

					POSITION = 0x00000100,
					POSITION_FLUCTUATION = 0x00000200,	/* (Reserved) */
					ROTATION = 0x00000400,
					ROTATION_FLUCTUATION = 0x00000800,
					SCALE_START = 0x00001000,
					SCALE_END = 0x00002000,

					SPEED = 0x00010000,	/* (Reserved) */
					SPEED_FLUCTUATION = 0x00020000,
					GRAVITY_DIRECTION = 0x00040000,
					GRAVITY_POINT = 0x00080000,

					COLORVERTEX = 0x00100000,
					COLORVERTEX_FLUCTUATION = 0x00200000,
					FADEALPHA = 0x00400000,

					/* for Emitter */
					EMIT_INFINITE = 0x01000000,

					/* Mask-Bit and etc. */
					CLEAR = 0x00000000,
					MASK_EMITTER = 0x7f000000,
					MASK_PARTICLE = 0x00ffffff,
					MASK_VALID = 0x7fffffff,
				}
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				[System.Serializable]
				public struct PatternEmit
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public int IndexGenerate;
//					public int Offset;
					public int Duration;
					public int Cycle;
					#endregion Variables & Properties

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					public void CleanUp()
					{
						IndexGenerate = -1;
//						Offset = -1;
						Duration = -1;
						Cycle = -1;
					}
					#endregion Enums & Constants
				}

				[System.Serializable]
				public struct RangeFloat
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public float Main;
					public float Sub;
					#endregion Variables & Properties
				}
				[System.Serializable]
				public struct RangeVector2
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Vector2 Main;
					public Vector2 Sub;
					#endregion Variables & Properties
				}
				[System.Serializable]
				public struct RangeColor
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Color Main;
					public Color Sub;
					#endregion Variables & Properties
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}

		[System.Serializable]
		public partial class CellMap
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public string Name;
			public Vector2 SizeOriginal;
			public Cell[] TableCell;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public void CleanUp()
			{
				Name = "";
				SizeOriginal = Vector2.zero;
				TableCell = null;
			}

			public int CountGetCell()
			{
				return((null != TableCell) ? TableCell.Length : -1);
			}

			public int IndexGetCell(string name)
			{
				if((true == string.IsNullOrEmpty(name)) || (null == TableCell))
				{
					return(-1);
				}

				int count = TableCell.Length;
				for(int i=0; i<count; i++)
				{
					if(name == TableCell[i].Name)
					{
						return(i);
					}
				}
				return(-1);
			}

			public void Duplicate(CellMap original)
			{
#if STATICDATA_DUPLICATE_DEEP
				CopyDeep(original);
#else
				CopyShallow(original);
#endif
			}

			public bool CopyShallow(CellMap original)
			{
				Name = original.Name;
				SizeOriginal = original.SizeOriginal;
				TableCell = original.TableCell;

				return(true);
			}

			public bool CopyDeep(CellMap original)
			{
				Name = string.Copy(original.Name);
				SizeOriginal = original.SizeOriginal;

				int countCell = original.TableCell.Length;
				TableCell = new Cell[countCell];
				for(int i=0; i<countCell; i++)
				{
					TableCell[i].CopyDeep(ref original.TableCell[i]);
				}

				return(true);
			}
			#endregion Functions

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			[System.Serializable]
			public struct Cell
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public string Name;
				public Rect Rectangle;
				public Vector2 Pivot;
				public DataMesh Mesh;
				public bool IsMesh
				{
					get
					{
						if(null == Mesh.TableCoordinate)
						{
							return(false);
						}
						return(0 < Mesh.TableCoordinate.Length);
					}
				}
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Name = "";
					Rectangle.x = 0.0f;
					Rectangle.y = 0.0f;
					Rectangle.width = 0.0f;
					Rectangle.height = 0.0f;
					Pivot = Vector2.zero;
					Mesh.CleanUp();
				}

				public void Duplicate(Cell original)
				{
#if STATICDATA_DUPLICATE_DEEP
					CopyDeep(ref original);
#else
					CopyShallow(ref original);
#endif
				}

				public bool CopyShallow(ref Cell original)
				{
					Name = string.Copy(original.Name);
					Rectangle = original.Rectangle;
					Pivot = original.Pivot;
					Mesh.CopyShallow(ref original.Mesh);

					return(true);
				}

				public bool CopyDeep(ref Cell original)
				{
					Name = original.Name;
					Rectangle = original.Rectangle;
					Pivot = original.Pivot;
					Mesh.CopyDeep(ref original.Mesh);

					return(true);
				}
				#endregion Functions

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				[System.Serializable]
				public struct DataMesh
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Vector2[] TableCoordinate;
					public int[] TableIndexVertex;

					public int CountMesh
					{
						get
						{
							return(TableIndexVertex.Length / (int)Constants.COUNT_VERTEX_SURFACE);
						}
					}
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public void CleanUp()
					{
						TableCoordinate = null;
						TableIndexVertex = null;
					}

					public bool BootUp(int countVertex, int countTriangle)
					{
						if((0 >= countVertex) || (0 >= countTriangle))
						{
							CleanUp();
							return(true);
						}

						TableCoordinate = new Vector2[countVertex];
						if(null == TableCoordinate)
						{
							goto BootUp_ErrorEnd;
						}

						TableIndexVertex = new int[countTriangle * (int)Constants.COUNT_VERTEX_SURFACE];
						if(null == TableIndexVertex)
						{
							goto BootUp_ErrorEnd;
						}

						return(true);

					BootUp_ErrorEnd:;
						CleanUp();
						return(false);
					}

					public void Duplicate(DataMesh original)
					{
#if STATICDATA_DUPLICATE_DEEP
						CopyDeep(ref original);
#else
						CopyShallow(ref original);
#endif
					}

					public bool CopyShallow(ref DataMesh original)
					{
						TableCoordinate = original.TableCoordinate;
						TableIndexVertex = original.TableIndexVertex;

						return(true);
					}

					public bool CopyDeep(ref DataMesh original)
					{
						int count;
						if(null != original.TableCoordinate)
						{
							count = original.TableCoordinate.Length;
							TableCoordinate = new Vector2[count];
							for(int i=0; i<count; i++)
							{
								TableCoordinate[i] = original.TableCoordinate[i];
							}
						}
						else
						{
							TableCoordinate = null;
						}

						if(null != original.TableIndexVertex)
						{
							count = original.TableIndexVertex.Length;
							TableIndexVertex = new int[count];
							for(int i=0; i<count; i++)
							{
								TableIndexVertex[i] = original.TableIndexVertex[i];
							}
						}
						else
						{
							TableIndexVertex = null;
						}

						return(true);
					}
					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					public enum Constants
					{
						COUNT_VERTEX_SURFACE = 3,	/* Vertice count per plane (Triangle only) */
					}
					#endregion Enums & Constants
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}

		public static partial class Texture
		{
			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public enum KindWrap
			{
				CLAMP = 0,
				REPEAT,
				MIRROR,	/* (Unsupported) */
			}
			public enum KindFilter
			{
				NEAREST = 0,
				LINEAR,
				BILINEAR,
			}
			#endregion Enums & Constants
		}

		public static partial class Parts
		{
			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			[System.Serializable]
			public struct Animation
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public string Name;
				public int ID;
				public int IDParent;
				public int[] TableIDChild;

				public KindFeature Feature;
				public int CountMesh;
				public BindMesh Mesh;

				public ColorLabel LabelColor;
				public Library_SpriteStudio6.KindOperationBlend OperationBlendTarget;

				public KindCollision ShapeCollision;
				public float SizeCollisionZ;

				public Object PrefabUnderControl;
				public string NameAnimationUnderControl;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Name = "";

					ID = -1;
					IDParent = -1;

					Feature = (KindFeature)(-1);
					CountMesh = 0;

					OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.NON;
					LabelColor.CleanUp();

					ShapeCollision = KindCollision.NON;
					SizeCollisionZ = 0.0f;

					PrefabUnderControl = null;
					NameAnimationUnderControl = "";
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindFeature
				{	/* ERROR/NON: -1 */
					/* MEMO: Abolished to switch type of triangulating of rectangle.(With or without using "Vertex Deformation(Correction)")            */
					/*       Always divided into 4 triangles, and center's coordinate is the intersection of lines through midpoints of opposite-sides. */
					/*       However, "Effect"'s particle is always divided into 2 triangles.                                                           */
					ROOT = 0,	/* Root-Parts (Subspecies of "NULL"-Parts) */
					NULL,
					NORMAL,

					INSTANCE,
					EFFECT,

					MASK,

					JOINT,
					BONE,
					MOVENODE,
					CONSTRAINT,
					BONEPOINT,
					MESH,

					/* MEMO: Ver.1.2.0 - */
					/* MEMO: 2 parts'-type are added. */
					TRANSFORM_CONSTRAINT,
					CAMERA,

					TERMINATOR,
				}

				public enum KindCollision
				{
					NON = 0,
					SQUARE,
					AABB,
					CIRCLE,
					CIRCLE_SCALEMINIMUM,
					CIRCLE_SCALEMAXIMUM
				}
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				[System.Serializable]
				public struct ColorLabel
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public KindForm Form;
					public Color32 Color;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public ColorLabel(KindForm form, Color color)
					{
						Form = form;
						Color = color;
					}

					public void CleanUp()
					{
						this = TableDefault[(int)KindForm.NON];
					}

					public void Set(KindForm form)
					{
						this = TableDefault[(int)form];
					}

					public void Set(Color color)
					{
						Form = KindForm.CUSTOM;
						Color = color;
					}
					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					public enum KindForm
					{
						NON = 0,

						RED,
						ORANGE,
						YELLOW,
						GREEN,
						BLUE,
						VIOLET,
						GRAY,

						TERMINATOR,
						CUSTOM = TERMINATOR,
					}

					internal readonly static ColorLabel[] TableDefault = new ColorLabel[(int)KindForm.TERMINATOR]
					{
						new ColorLabel(KindForm.NON, new Color(0.0f, 0.0f, 0.0f, 0.0f)),

						new ColorLabel(KindForm.RED, new Color(1.0f, 0.46f, 0.43f, 1.0f)),
						new ColorLabel(KindForm.ORANGE, new Color(0.98f, 0.65f, 0.33f, 1.0f)),
						new ColorLabel(KindForm.YELLOW, new Color(0.89f, 0.85f, 0.37f, 1.0f)),
						new ColorLabel(KindForm.GREEN, new Color(0.58f, 0.87f, 0.49f, 1.0f)),
						new ColorLabel(KindForm.BLUE, new Color(0.54f, 0.74f, 0.97f, 1.0f)),
						new ColorLabel(KindForm.VIOLET, new Color(0.64f, 0.55f, 0.87f, 1.0f)),
						new ColorLabel(KindForm.GRAY, new Color(0.67f, 0.67f, 0.67f, 1.0f)),
					};
					#endregion Enums & Constants
				}

				[System.Serializable]
				public struct BindMesh
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public int CountVertex;	/* number of "Cell-Mesh"'s vertices. */

					/* for Skeletal-animation */
					public Vertex[] TableVertex;
					public Vector2[] TableRateUV;
					public int[] TableIndexVertex;

					/* for Deform */
					public int CountVertexDeform;	/* same as "CountVertex" when use "Deform". (0 when not use) */
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public void CleanUp()
					{
						CountVertex = 0;

						TableVertex = null;
						TableRateUV = null;
						TableIndexVertex = null;

						CountVertexDeform = 0;
					}
					#endregion Functions

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					[System.Serializable]
					public struct Vertex
					{
						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						public Bone[] TableBone;
						#endregion Variables & Properties

						/* ----------------------------------------------- Functions */
						#region Functions
						public void CleanUp()
						{
							TableBone = null;
						}

						public bool BootUp(int countBone)
						{
							TableBone = new Bone[countBone];
							if(null == TableBone)
							{
								return(false);
							}

							for(int i=0; i<countBone; i++)
							{
								TableBone[i].CleanUp();
							}

							return(true);
						}
						#endregion Functions

						/* ----------------------------------------------- Classes, Structs & Interfaces */
						#region Classes, Structs & Interfaces
						[System.Serializable]
						public struct Bone
						{
							/* ----------------------------------------------- Variables & Properties */
							#region Variables & Properties
							public int Index;
							public float Weight;
							public Vector3 CoordinateOffset;
							#endregion Variables & Properties

							/* ----------------------------------------------- Functions */
							#region Functions
							public void CleanUp()
							{
								Index = -1;
								Weight = 0.0f;
								CoordinateOffset = Vector3.zero;
							}
							#endregion Functions
						}
						#endregion Classes, Structs & Interfaces
					}
					#endregion Classes, Structs & Interfaces
				}

				[System.Serializable]
				public struct Catalog
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public int[] TableIDPartsNULL;
//					public int[] TableIDPartsTriangle2;
//					public int[] TableIDPartsTriangle4;
					public int[] TableIDPartsNormal;
					public int[] TableIDPartsInstance;
					public int[] TableIDPartsEffect;
//					public int[] TableIDPartsMaskTriangle2;
//					public int[] TableIDPartsMaskTriangle4;
					public int[] TableIDPartsMask;
					public int[] TableIDPartsJoint;
					public int[] TableIDPartsBone;
					public int[] TableIDPartsMoveNode;
					public int[] TableIDPartsConstraint;
					public int[] TableIDPartsBonePoint;
					public int[] TableIDPartsMesh;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public void CleanUp()
					{
						TableIDPartsNULL = null;
//						TableIDPartsTriangle2 = null;
//						TableIDPartsTriangle4 = null;
						TableIDPartsNormal = null;
						TableIDPartsInstance = null;
						TableIDPartsEffect = null;
						TableIDPartsJoint = null;
//						TableIDPartsMaskTriangle2 = null;
//						TableIDPartsMaskTriangle4 = null;
						TableIDPartsMask = null;
						TableIDPartsBone = null;
						TableIDPartsMoveNode = null;
						TableIDPartsConstraint = null;
						TableIDPartsBonePoint = null;
						TableIDPartsMesh = null;
					}
					#endregion Functions
				}
				#endregion Classes, Structs & Interfaces
			}

			[System.Serializable]
			public struct Effect
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public string Name;

				public int ID;
				public int IDParent;
				public int[] TableIDChild;

				public KindFeature Feature;	/* Preliminary ... "Root"or"Emitter" */
				public int IndexEmitter;	/* -1 == Not "Emitter" */
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Name = "";

					ID = -1;
					IDParent = -1;
					TableIDChild = null;

					Feature = (KindFeature)(-1);
					IndexEmitter = -1;
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindFeature
				{	/* ERROR: -1 */
					ROOT = 0,	/* Root-Parts (Subspecies of "Particle"-Parts) */
					EMITTER,	/* Emitter */
					PARTICLE,	/* Particle */

					TERMINATOR
				}
				#endregion Enums & Constants
			}
			#endregion Classes, Structs & Interfaces
		}

		public static partial class Shader
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static UnityEngine.Shader ShaderGetAnimation(Library_SpriteStudio6.KindOperationBlend operationBlend)
			{
				switch(operationBlend)
				{
					case Library_SpriteStudio6.KindOperationBlend.MASK_PRE:
					case Library_SpriteStudio6.KindOperationBlend.MASK:
						return(StencilSS6PU);

					case Library_SpriteStudio6.KindOperationBlend.MIX:
					case Library_SpriteStudio6.KindOperationBlend.ADD:
					case Library_SpriteStudio6.KindOperationBlend.SUB:
					case Library_SpriteStudio6.KindOperationBlend.MUL:
					case Library_SpriteStudio6.KindOperationBlend.MUL_NA:
					case Library_SpriteStudio6.KindOperationBlend.SCR:
					case Library_SpriteStudio6.KindOperationBlend.EXC:
					case Library_SpriteStudio6.KindOperationBlend.INV:
						return(SpriteSS6PU);
				}

				return(null);
			}

			public static UnityEngine.Shader ShaderGetEffect(Library_SpriteStudio6.KindOperationBlendEffect operationBlend)
			{
#if false
				switch(operationBlend)
				{
					case Library_SpriteStudio6.KindOperationBlendEffect.MIX:
					case Library_SpriteStudio6.KindOperationBlendEffect.ADD:
						return(EffectSS6PU);
				}

				return(null);
#else
				return(EffectSS6PU);
#endif
			}

			public static UnityEngine.Material MaterialCreateAnimation(	Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp,
																		UnityEngine.Shader shader,
																		Library_SpriteStudio6.KindOperationBlend operationBlend,
																		Library_SpriteStudio6.KindMasking masking,
																		bool flagZWrite
																	)
			{
				UnityEngine.Material material = null;
				if(null == shader)
				{
					shader = ShaderGetAnimation(operationBlend);
				}
				material = new Material(shader);
				if(null == material)
				{
					return(null);
				}
				if(null == functionMaterialSetUp)
				{
					return(null);
				}
				material.hideFlags = HideFlags.DontSave;

				return(functionMaterialSetUp(material, (int)operationBlend, masking, flagZWrite));
			}
			public static UnityEngine.Material MaterialCreateEffect(	Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp,
																		UnityEngine.Shader shader,
																		Library_SpriteStudio6.KindOperationBlendEffect operationBlend, 
																		Library_SpriteStudio6.KindMasking masking,
																		bool flagZWrite
																)
			{
				UnityEngine.Material material = null;
				if(null == shader)
				{
					shader = ShaderGetEffect(operationBlend);
				}
#if UNITY_EDITOR
				if(null == shader)
				{
					return(null);
				}
#endif
				material = new Material(shader);
				if(null == material)
				{
					return(null);
				}
				if(null == functionMaterialSetUp)
				{
					return(null);
				}
				material.hideFlags = HideFlags.DontSave;

				return(functionMaterialSetUp(material, (int)operationBlend, KindMasking.MASK, flagZWrite));
			}

			internal static UnityEngine.Material FunctionMaterialSetUpAnimation(	UnityEngine.Material material,
																					int operationBlend,
																					Library_SpriteStudio6.KindMasking masking,
																					bool flagZWrite
																				)
			{
				switch((Library_SpriteStudio6.KindOperationBlend)operationBlend)
				{
					case Library_SpriteStudio6.KindOperationBlend.MASK_PRE:
						switch(masking)
						{
							case Library_SpriteStudio6.KindMasking.THROUGH:
								material.SetFloat(IDPropertyStencilOperation, (float)UnityEngine.Rendering.StencilOp.IncrementWrap);
								break;

							case Library_SpriteStudio6.KindMasking.MASK:
								material.SetFloat(IDPropertyStencilOperation, (float)UnityEngine.Rendering.StencilOp.Invert);
								break;
						}
						goto case Library_SpriteStudio6.KindOperationBlend.TERMINATOR;	/* Common Setting for Masking-Shader */

					case Library_SpriteStudio6.KindOperationBlend.MASK:
						switch(masking)
						{
							case Library_SpriteStudio6.KindMasking.THROUGH:
								material.SetFloat(IDPropertyStencilOperation, (float)UnityEngine.Rendering.StencilOp.DecrementWrap);
								break;

							case Library_SpriteStudio6.KindMasking.MASK:
								material.SetFloat(IDPropertyStencilOperation, (float)UnityEngine.Rendering.StencilOp.Invert);
								break;
						}
						goto case Library_SpriteStudio6.KindOperationBlend.TERMINATOR;	/* Common Setting for Masking-Shader */

					case Library_SpriteStudio6.KindOperationBlend.MIX:
						material.SetFloat(IDPropertyBlendSource, (float)UnityEngine.Rendering.BlendMode.One);	/* UnityEngine.Rendering.BlendMode.SrcAlpha */
						material.SetFloat(IDPropertyBlendDestination, (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
						material.SetFloat(IDPropertyBlendOperation, (float)UnityEngine.Rendering.BlendOp.Add);
						material.DisableKeyword(NamePropertyNotDiscardPixel);	/* false */
						material.EnableKeyword(NamePropertyOutputPixelPMA);	/* true */
						goto default;	/* Common Setting for Drawing-Shader */

					case Library_SpriteStudio6.KindOperationBlend.ADD:
						material.SetFloat(IDPropertyBlendSource, (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
						material.SetFloat(IDPropertyBlendDestination, (float)UnityEngine.Rendering.BlendMode.One);
						material.SetFloat(IDPropertyBlendOperation, (float)UnityEngine.Rendering.BlendOp.Add);
						material.DisableKeyword(NamePropertyNotDiscardPixel);	/* false */
						material.DisableKeyword(NamePropertyOutputPixelPMA);	/* false */
						goto default;	/* Common Setting for Drawing-Shader */

					case Library_SpriteStudio6.KindOperationBlend.SUB:
						material.SetFloat(IDPropertyBlendSource, (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
						material.SetFloat(IDPropertyBlendDestination, (float)UnityEngine.Rendering.BlendMode.One);
						material.SetFloat(IDPropertyBlendOperation, (float)UnityEngine.Rendering.BlendOp.ReverseSubtract);
						material.DisableKeyword(NamePropertyNotDiscardPixel);	/* false */
						material.DisableKeyword(NamePropertyOutputPixelPMA);	/* false */
						goto default;	/* Common Setting for Drawing-Shader */

					case Library_SpriteStudio6.KindOperationBlend.MUL:
						material.SetFloat(IDPropertyBlendSource, (float)UnityEngine.Rendering.BlendMode.DstColor);
						material.SetFloat(IDPropertyBlendDestination, (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
						material.SetFloat(IDPropertyBlendOperation, (float)UnityEngine.Rendering.BlendOp.Add);
						material.DisableKeyword(NamePropertyNotDiscardPixel);	/* false */
						material.EnableKeyword(NamePropertyOutputPixelPMA);		/* true */
						goto default;	/* Common Setting for Drawing-Shader */

					case Library_SpriteStudio6.KindOperationBlend.MUL_NA:
						material.SetFloat(IDPropertyBlendSource, (float)UnityEngine.Rendering.BlendMode.Zero);
						material.SetFloat(IDPropertyBlendDestination, (float)UnityEngine.Rendering.BlendMode.SrcColor);
						material.SetFloat(IDPropertyBlendOperation, (float)UnityEngine.Rendering.BlendOp.Add);
						material.EnableKeyword(NamePropertyNotDiscardPixel);	/* true */
						material.EnableKeyword(NamePropertyOutputPixelPMA);		/* true */
						goto default;	/* Common Setting for Drawing-Shader */

					case Library_SpriteStudio6.KindOperationBlend.SCR:
						material.SetFloat(IDPropertyBlendSource, (float)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
						material.SetFloat(IDPropertyBlendDestination, (float)UnityEngine.Rendering.BlendMode.One);
						material.SetFloat(IDPropertyBlendOperation, (float)UnityEngine.Rendering.BlendOp.Add);
						material.DisableKeyword(NamePropertyNotDiscardPixel);	/* false */
						material.DisableKeyword(NamePropertyOutputPixelPMA);	/* false */
						goto default;	/* Common Setting for Drawing-Shader */

					case Library_SpriteStudio6.KindOperationBlend.EXC:
						material.SetFloat(IDPropertyBlendSource, (float)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
						material.SetFloat(IDPropertyBlendDestination, (float)UnityEngine.Rendering.BlendMode.OneMinusSrcColor);
						material.SetFloat(IDPropertyBlendOperation, (float)UnityEngine.Rendering.BlendOp.Add);
						material.DisableKeyword(NamePropertyNotDiscardPixel);	/* false */
						material.DisableKeyword(NamePropertyOutputPixelPMA);	/* false */
						goto default;	/* Common Setting for Drawing-Shader */

					case Library_SpriteStudio6.KindOperationBlend.INV:
						material.SetFloat(IDPropertyBlendSource, (float)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
						material.SetFloat(IDPropertyBlendDestination, (float)UnityEngine.Rendering.BlendMode.Zero);
						material.SetFloat(IDPropertyBlendOperation, (float)UnityEngine.Rendering.BlendOp.Add);
						material.EnableKeyword(NamePropertyNotDiscardPixel);	/* true */
						material.EnableKeyword(NamePropertyOutputPixelPMA);		/* true */
						goto default;	/* Common Setting for Drawing-Shader */

					case Library_SpriteStudio6.KindOperationBlend.TERMINATOR:
						/* MEMO: Common Setting for Masking-Shader */
						break;

					default:
						/* MEMO: Common Setting for Drawing-Shader */
						switch(masking)
						{
							case Library_SpriteStudio6.KindMasking.THROUGH:
								material.SetFloat(IDPropertyCompareStencil, (float)UnityEngine.Rendering.CompareFunction.Always);
								break;

							case Library_SpriteStudio6.KindMasking.MASK:
								material.SetFloat(IDPropertyCompareStencil, (float)UnityEngine.Rendering.CompareFunction.Equal);
								break;
						}

						material.SetFloat(IDPropertyZWrite, ((true == flagZWrite) ? 1.0f : 0.0f));
						break;
				}
				return(material);
			}
			internal static UnityEngine.Material FunctionMaterialSetUpEffect(	UnityEngine.Material material,
																				int operationBlend, 
																				Library_SpriteStudio6.KindMasking masking,
																				bool flagZWrite
																		)
			{
				switch((Library_SpriteStudio6.KindOperationBlendEffect)operationBlend)
				{
					case Library_SpriteStudio6.KindOperationBlendEffect.MIX:
						material.SetFloat(IDPropertyBlendSource, (float)UnityEngine.Rendering.BlendMode.One);
						material.SetFloat(IDPropertyBlendDestination, (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
						material.SetFloat(IDPropertyBlendOperation, (float)UnityEngine.Rendering.BlendOp.Add);
						material.DisableKeyword(NamePropertyNotDiscardPixel);	/* false */
						material.EnableKeyword(NamePropertyOutputPixelPMA);	/* true */
						goto default;	/* Common Setting for Drawing-Shader */

					case Library_SpriteStudio6.KindOperationBlendEffect.ADD:
						material.SetFloat(IDPropertyBlendSource, (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
						material.SetFloat(IDPropertyBlendDestination, (float)UnityEngine.Rendering.BlendMode.One);
						material.SetFloat(IDPropertyBlendOperation, (float)UnityEngine.Rendering.BlendOp.Add);
						material.DisableKeyword(NamePropertyNotDiscardPixel);	/* false */
						material.DisableKeyword(NamePropertyOutputPixelPMA);	/* false */
						goto default;	/* Common Setting for Drawing-Shader */

					case Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR:
						/* MEMO: Common Setting for Masking-Shader */
						break;

					default:
						/* MEMO: Common Setting for Drawing-Shader */
						switch(masking)
						{
							case Library_SpriteStudio6.KindMasking.THROUGH:
								material.SetFloat(IDPropertyCompareStencil, (float)UnityEngine.Rendering.CompareFunction.Always);
								break;

							case Library_SpriteStudio6.KindMasking.MASK:
								material.SetFloat(IDPropertyCompareStencil, (float)UnityEngine.Rendering.CompareFunction.Equal);
								break;
						}

						material.SetFloat(IDPropertyZWrite, ((true == flagZWrite) ? 1.0f : 0.0f));
						break;
				}

				return(material);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public const string NameShaderPrefix = "Custom/SpriteStudio6/";
			public const string NameShaderPrefixSS6P = NameShaderPrefix + "SS6PU/";
			public const string NameShaderPrefixUnityNative = NameShaderPrefix + "UnityNative/";
			public const string NameShaderPrefixUnityUI = NameShaderPrefix + "UnityUI/";

			public readonly static UnityEngine.Shader SpriteSS6PU = UnityEngine.Shader.Find(NameShaderPrefixSS6P + "Sprite");
			public readonly static UnityEngine.Shader EffectSS6PU = UnityEngine.Shader.Find(NameShaderPrefixSS6P + "Effect");
			public readonly static UnityEngine.Shader StencilSS6PU = UnityEngine.Shader.Find(NameShaderPrefixSS6P + "Stencil");
			public readonly static UnityEngine.Shader SpriteUnityNative = UnityEngine.Shader.Find(NameShaderPrefixUnityNative + "Sprite");
			public readonly static UnityEngine.Shader SpriteUnityNativeNonBatch = UnityEngine.Shader.Find(NameShaderPrefixUnityNative + "Sprite_NonBatch");
			public readonly static UnityEngine.Shader SkinnedMeshUnityNative = UnityEngine.Shader.Find(NameShaderPrefixUnityNative + "SkinnedMesh");
			public readonly static UnityEngine.Shader SpriteUnityUI = UnityEngine.Shader.Find(NameShaderPrefixUnityUI + "Sprite");

			public const string NamePropertyAlphaTex = "_AlphaTex";							/* (Common) */
			public const string NamePropertyEnableExternalAlpha = "_EnableExternalAlpha";	/* (Common) */
			public const string NamePropertyBlendSource = "_BlendSource";					/* Sprite_SpriteStudio6 / Effect_SpriteStudio6 */
			public const string NamePropertyBlendDestination = "_BlendDestination";			/* Sprite_SpriteStudio6 / Effect_SpriteStudio6 */
			public const string NamePropertyBlendOperation = "_BlendOperation";				/* Sprite_SpriteStudio6 / Effect_SpriteStudio6 */
			public const string NamePropertyCompareStencil = "_CompareStencil";				/* Sprite_SpriteStudio6 / Effect_SpriteStudio6 */
			public const string NamePropertyZWrite = "_ZWrite";								/* Sprite_SpriteStudio6 / Effect_SpriteStudio6 */
			public const string NamePropertyArgumentFs00 = "_ArgumentFs00";					/* Sprite_SpriteStudio6 / Effect_SpriteStudio6 */
			public const string NamePropertyParameterFs00 = "_ParameterFs00";				/* Sprite_SpriteStudio6 / Effect_SpriteStudio6 */

			public const string NamePropertyNotDiscardPixel = "PS_NOT_DISCARD";				/* Sprite_SpriteStudio6 / Effect_SpriteStudio6 */
			public const string NamePropertyOutputPixelPMA = "PS_OUTPUT_PMA";				/* Sprite_SpriteStudio6 / Effect_SpriteStudio6 */
			public const string NamePropertyStencilOperation = "_StencilOperation";			/* Stencil_SpriteStudio6 */

			public const string NamePropertyUIColor = "_Color";								/* Sprite_UnityUI */
			public const string NamePropertyUIStencilComp = "_StencilComp";					/* Sprite_UnityUI */
			public const string NamePropertyUIStencil = "_Stencil";							/* Sprite_UnityUI */
			public const string NamePropertyUIStencilOp = "_StencilOp";						/* Sprite_UnityUI */
			public const string NamePropertyUIStencilWriteMask = "_StencilWriteMask";		/* Sprite_UnityUI */
			public const string NamePropertyUIStencilReadMask = "_StencilReadMask";			/* Sprite_UnityUI */
			public const string NamePropertyUIColorMask = "_ColorMask";						/* Sprite_UnityUI */

			public readonly static int IDPropertyStencilOperation = UnityEngine.Shader.PropertyToID(NamePropertyStencilOperation);
			public readonly static int IDPropertyBlendSource = UnityEngine.Shader.PropertyToID(NamePropertyBlendSource);
			public readonly static int IDPropertyBlendDestination = UnityEngine.Shader.PropertyToID(NamePropertyBlendDestination);
			public readonly static int IDPropertyBlendOperation = UnityEngine.Shader.PropertyToID(NamePropertyBlendOperation);
			public readonly static int IDPropertyCompareStencil = UnityEngine.Shader.PropertyToID(NamePropertyCompareStencil);
			public readonly static int IDPropertyZWrite = UnityEngine.Shader.PropertyToID(NamePropertyZWrite);
			public readonly static int IDPropertyArgumentFs00 = UnityEngine.Shader.PropertyToID(NamePropertyArgumentFs00);
			public readonly static int IDPropertyParameterFs00 = UnityEngine.Shader.PropertyToID(NamePropertyParameterFs00);
			public readonly static int IDPropertyAlphaTex = UnityEngine.Shader.PropertyToID(NamePropertyAlphaTex);
			public readonly static int IDPropertyEnableExternalAlpha = UnityEngine.Shader.PropertyToID(NamePropertyEnableExternalAlpha);

			public readonly static int IDPropertyUIColor = UnityEngine.Shader.PropertyToID(NamePropertyUIColor);
			public readonly static int IDPropertyUIStencilComp = UnityEngine.Shader.PropertyToID(NamePropertyUIStencilComp);
			public readonly static int IDPropertyUIStencil = UnityEngine.Shader.PropertyToID(NamePropertyUIStencil);
			public readonly static int IDPropertyUIStencilOp = UnityEngine.Shader.PropertyToID(NamePropertyUIStencilOp);
			public readonly static int IDPropertyUIStencilWriteMask = UnityEngine.Shader.PropertyToID(NamePropertyUIStencilWriteMask);
			public readonly static int IDPropertyUIStencilReadMask = UnityEngine.Shader.PropertyToID(NamePropertyUIStencilReadMask);
			public readonly static int IDPropertyUIColorMask = UnityEngine.Shader.PropertyToID(NamePropertyUIColorMask);
			#endregion Enums & Constants
		}

		public static partial class Sequence
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public enum Type
			{
				INVALID = -1,

				LAST,										/* 0: Last Item, Repeatedly */
				KEEP,										/* 1: Stop at the Last Frame */
				TOP,										/* 2: Repeat whole */

				TERMINATOR,
			}
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			[System.Serializable]
			public struct Data
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public string Name;
				public int Index;
				public Sequence.Type Type;
				public Step[] TableStep;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				[System.Serializable]
				public struct Step
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public string NamePackAnimation;
					public string NameAnimation;
					public int PlayCount;

					public bool IsValid
					{
						get
						{
							return(	!(	(null == NamePackAnimation)
//										|| (null == NameAnimation)
//										|| (0 > PlayCount)
									)
								);	/* ? true : false */
						}
					}
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public void CleanUp()
					{
						NamePackAnimation = null;
						NameAnimation = null;
						PlayCount = -1;
					}
					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					#endregion Enums & Constants

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					#endregion Classes, Structs & Interfaces
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}

		#endregion Classes, Structs & Interfaces
	}

	public static partial class Control
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public static partial class Animation
		{
			/* Part: SpriteStudio6/Library/Control/AnimationTrack.cs */
			/* Part: SpriteStudio6/Library/Control/AnimationParts.cs */
		}

//		public partial struct Effect
//		{
			/* Part: SpriteStudio6/Library/Control/Effect.cs */
//		}

		public class AdditionalColor
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			internal FlagBitStatus Status;
			public Library_SpriteStudio6.KindOperationBlend OperationBlend;
			public Color[] ColorVertex;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public void CleanUp()
			{
				Status = FlagBitStatus.CLEAR;
				OperationBlend = (Library_SpriteStudio6.KindOperationBlend)(-1);
				ColorVertex = null;
			}

			public bool BootUp()
			{
				CleanUp();

				OperationBlend = Library_SpriteStudio6.KindOperationBlend.MIX;

				int countVertex = (int)Library_SpriteStudio6.KindVertex.TERMINATOR2;
				ColorVertex = new Color[countVertex];
				if(null == ColorVertex)
				{
					return(false);
				}

				for(int i=0; i<countVertex; i++)
				{
					ColorVertex[i] = ColorClear[(int)Library_SpriteStudio6.KindOperationBlend.MIX];
				}

				Status |= FlagBitStatus.VALID;

				return(true);
			}

			public void ShutDown()
			{
				CleanUp();
			}

			/* ******************************************************** */
			//! Set single additional-color (Bounding: Overall)
			/*!
			@param	operationBlend
				kind of Blending Operation (NON/MIX/ADD/SUB/MUL)<br>
				Library_SpriteStudio6.KindOperationBlend.NON: Follow animation data
			@param	color
				Blending Color<br>
				AdditionalColor.ColorClear[operationBlend]: Color as blending nothing
			@retval	Return-Value
				(none)

			Set single additional-color.<br>
			<br>
			Do not set values other than NON, MIX, ADD, SUB or MUL to "operationBlend".<br>
			<br>
			When specify a "Library_SpriteStudio6.KindOperationBlend.NON" to "operationBlend", result will follow the setting of the original animation data.<br>
			The color for invalidating affect differs for each blend and can be get using this class's "ColorClear[blend-type]".<br>
			*/
			public void SetOverall(Library_SpriteStudio6.KindOperationBlend operationBlend, Color32 color)
			{
				if(Library_SpriteStudio6.KindOperationBlend.NON == operationBlend)
				{
					OperationBlend = Library_SpriteStudio6.KindOperationBlend.NON;
					Status |= FlagBitStatus.CHANGE;
					return;
				}

				if((Library_SpriteStudio6.KindOperationBlend.NON >= operationBlend) || (Library_SpriteStudio6.KindOperationBlend.TERMINATOR_PARTSCOLOR <= operationBlend))
				{	/* Ignore (Error) */
					return;
				}

				OperationBlend = operationBlend;
				for(int i=0; i<(int)Library_SpriteStudio6.KindVertex.TERMINATOR2; i++)
				{
					ColorVertex[i] = color;
				}

				Status |= FlagBitStatus.CHANGE;
			}

			/* ******************************************************** */
			//! Set separately additional-color of the 4-vertices (Bounding: Vertex)
			/*!
			@param	operationBlend
				kind of Blending Operation (NON/MIX/ADD/SUB/MUL)<br>
				Library_SpriteStudio6.KindOperationBlend.NON: Follow animation data
			@param	colorLU
				Blending Color for vertex left-top<br>
				AdditionalColor.ColorClear[operationBlend]: Color as blending nothing
			@param	colorRU
				Blending Color for vertex right-top<br>
				AdditionalColor.ColorClear[operationBlend]: Color as blending nothing
			@param	colorRD
				Blending Color for vertex right-bottom<br>
				AdditionalColor.ColorClear[operationBlend]: Color as blending nothing
			@param	colorLD
				Blending Color for vertex left-bottom<br>
				AdditionalColor.ColorClear[operationBlend]: Color as blending nothing
			@retval	Return-Value
				(none)

			Set separately additional-color of the 4-vertices.<br>
			<br>
			Do not set values other than NON, MIX, ADD, SUB or MUL to "operationBlend".<br>
			<br>
			When specify a "Library_SpriteStudio6.KindOperationBlend.NON" to "operationBlend", result will follow the setting of the original animation data.<br>
			The color for invalidating affect differs for each blend and can be get using this class's "ColorClear[blend-type]".<br>
			*/
			public void SetVertex(	Library_SpriteStudio6.KindOperationBlend operationBlend,
									Color32 colorLU,
									Color32 colorRU,
									Color32 colorRD,
									Color32 colorLD
								)
			{
				if(Library_SpriteStudio6.KindOperationBlend.NON == operationBlend)
				{
					OperationBlend = Library_SpriteStudio6.KindOperationBlend.NON;
					Status |= FlagBitStatus.CHANGE;
					return;
				}

				if((Library_SpriteStudio6.KindOperationBlend.NON >= operationBlend) || (Library_SpriteStudio6.KindOperationBlend.TERMINATOR_PARTSCOLOR <= operationBlend))
				{	/* Ignore (Error) */
					return;
				}

				OperationBlend = operationBlend;
				ColorVertex[(int)Library_SpriteStudio6.KindVertex.LU] = colorLU;
				ColorVertex[(int)Library_SpriteStudio6.KindVertex.RU] = colorRU;
				ColorVertex[(int)Library_SpriteStudio6.KindVertex.RD] = colorRD;
				ColorVertex[(int)Library_SpriteStudio6.KindVertex.LD] = colorLD;

				Status |= FlagBitStatus.CHANGE;
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			internal enum FlagBitStatus
			{
				VALID = 0x40000000,
				CHANGE = 0x20000000,

				CLEAR = 0
			}

			public readonly static Color32[] ColorClear = new Color32[(int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR_PARTSCOLOR]
			{
				/* MIX */	new Color(0.0f, 0.0f, 0.0f, 0.0f),
				/* ADD */	new Color(0.0f, 0.0f, 0.0f, 0.0f),
				/* SUB */	new Color(0.0f, 0.0f, 0.0f, 0.0f),
				/* MUL */	new Color(0.0f, 0.0f, 0.0f, 0.0f)
			};
			#endregion Enums & Constants
		}

		internal class CacheMaterial
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public List<InformationData> Data;
			public UnityEngine.Shader ShaderStandardAnimation;
			public UnityEngine.Shader ShaderStandardEffect;
			public UnityEngine.Shader ShaderStandardStencil;

			public Library_SpriteStudio6.CallBack.FunctionMaterialSetUp FunctionMaterialSetUpAnimation;
			public Library_SpriteStudio6.CallBack.FunctionMaterialSetUp FunctionMaterialSetUpEffect;
			/* MEMO: For "Stencil" is shared with for "Animation". */

			public bool StatusIsBootedUp
			{
				get
				{
					return(null != Data);	/*  ? true : false*/
				}
			}
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public bool BootUp(int capacity)
			{
				Data = new List<InformationData>(capacity);
				Data.Clear();

				ShaderStandardAnimation = null;
				ShaderStandardEffect = null;
				ShaderStandardStencil = null;

				FunctionMaterialSetUpAnimation = null;
				FunctionMaterialSetUpEffect = null;

				return(true);
			}

			public void ShutDown(bool flagDestroyInstance)
			{
				DataPurge(flagDestroyInstance);

				Data = null;
				ShaderStandardAnimation = null;
				ShaderStandardEffect = null;
				ShaderStandardStencil = null;

				FunctionMaterialSetUpAnimation = null;
				FunctionMaterialSetUpEffect = null;
			}

			public void DataPurge(bool flagDestroyInstance)
			{
				if(false == StatusIsBootedUp)
				{
					return;
				}

				int countData = Data.Count;
				for(int i=(countData-1); i>=0; i--)
				{
					DataRelease(i, flagDestroyInstance);
				}
				Data.Clear();
			}

			public void DataAppend(long codeHash, UnityEngine.Material instanecMaterial)
			{
				InformationData informationData = new InformationData(codeHash, instanecMaterial);
				Data.Add(informationData);
			}

			public void DataRelease(int index, bool flagDestroyInstance=false)
			{
				if(true == flagDestroyInstance)
				{
					UnityEngine.Material material = Data[index].Instance;
					if(null != material)
					{
						Utility.Asset.ObjectDestroy(material);
					}
				}

				Data.RemoveAt(index);
			}

			public int IndexGet(long codeHash)
			{
				/* MEMO: Normally, Binary-Search is faster.                                      */
				/*       But since delete invalid-cache at the same time, use Linear-Search now. */
				int countData = Data.Count;
				for(int i=0; i<countData; i++)
				{
					if(Data[i].CodeHash == codeHash)
					{
						if(null != Data[i].Instance)
						{	/* Valid Instance */
							return(i);
						}

						/* Delete Invalid-Cache (Instance is Destroyed) */
						/* MEMO: Check the validity as materials may be destroyed from external. */
						/*       Especially, when switching between Play-Modes on Unity-Editor.  */
						Data.RemoveAt(i);
						i--;
						countData--;
					}
				}

				return(-1);
			}

			public int IndexGet(UnityEngine.Material instanceMaterial)
			{
				if(null == instanceMaterial)
				{
					return(-1);
				}

				int countData = Data.Count;
				for(int i=0; i<countData; i++)
				{
					if(Data[i].Instance == instanceMaterial)
					{
						return(i);
					}
				}

				return(-1);
			}

			public UnityEngine.Material MaterialGet(long codeHash)
			{
				/* MEMO: Normally, Binary-Search is faster.                                      */
				/*       But since delete invalid-cache at the same time, use Linear-Search now. */
				int countData = Data.Count;
				for(int i=0; i<countData; i++)
				{
					if(Data[i].CodeHash == codeHash)
					{
						UnityEngine.Material instanceMaterial = Data[i].Instance;
						if(null != instanceMaterial)
						{	/* Valid Instance */
							return(instanceMaterial);
						}

						/* Delete Invalid-Cache (Instance is Destroyed) */
						/* MEMO: Check the validity as materials may be destroyed from external. */
						/*       Especially, when switching between Play-Modes on Unity-Editor.  */
						Data.RemoveAt(i);
						i--;
						countData--;
					}
				}

				return(null);
			}
			public UnityEngine.Material MaterialGetAnimation(	int indexCellMap,
																Library_SpriteStudio6.KindOperationBlend operationBlend,
																Library_SpriteStudio6.KindMasking masking,
																string nameShader,
																Shader shader,
																Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp,
																Texture[] tableTexture,
																bool flagCreateNew
															)
			{
				long codeHash = Library_SpriteStudio6.Control.CacheMaterial.InformationData.CodeGetAnimation(	indexCellMap,
																												operationBlend,
																												masking,
																												nameShader
																										);
				UnityEngine.Material instanceMaterial = MaterialGet(codeHash);
				if(null == instanceMaterial)
				{	/* Not exist */
//					instanceMaterial = null;
					if(true == flagCreateNew)
					{
						if(null == shader)
						{
							if(Library_SpriteStudio6.KindOperationBlend.MIX > operationBlend)
							{	/* for Mask */
								shader = ShaderStandardStencil;
							}
							else
							{	/* for Color */
								shader = ShaderStandardAnimation;
							}

							if(null == functionMaterialSetUp)
							{
								/* MEMO: For "Animation", use for "Stencil" too. */
								functionMaterialSetUp = FunctionMaterialSetUpAnimation;
							}
						}
						else
						{
							if(null == functionMaterialSetUp)
							{
								/* MEMO: For "Animation", use for "Stencil" too. */
								functionMaterialSetUp = Library_SpriteStudio6.Data.Shader.FunctionMaterialSetUpAnimation;
							}
						}

						/* Create new material */
						instanceMaterial = Library_SpriteStudio6.Data.Shader.MaterialCreateAnimation(functionMaterialSetUp, shader, operationBlend, masking, false);
						if(null == instanceMaterial)
						{	/* Miss-Create */
							return(null);
						}
						instanceMaterial.mainTexture = tableTexture[indexCellMap];
						DataAppend(codeHash, instanceMaterial);
					}
				}

				return(instanceMaterial);
			}
			public UnityEngine.Material MaterialGetEffect(	int indexCellMap,
																Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
																Library_SpriteStudio6.KindMasking masking,
																string nameShader,
																Shader shader,
																Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp,
																Texture[] tableTexture,
																bool flagCreateNew
															)
			{
				long codeHash = Library_SpriteStudio6.Control.CacheMaterial.InformationData.CodeGetEffect(	indexCellMap,
																											operationBlend,
																											masking,
																											nameShader
																									);
				UnityEngine.Material instanceMaterial = MaterialGet(codeHash);
				if(null == instanceMaterial)
				{	/* Not exist */
					/* MEMO: Not-exist */
//					instanceMaterial = null;
					if(true == flagCreateNew)
					{
						if(null == shader)
						{
							/* MEMO: "Effect" does not have "Masking" function. */
							shader = ShaderStandardEffect;
							if(null == functionMaterialSetUp)
							{
								functionMaterialSetUp = FunctionMaterialSetUpEffect;
							}
						}
						else
						{
							if(null == functionMaterialSetUp)
							{
								functionMaterialSetUp = Library_SpriteStudio6.Data.Shader.FunctionMaterialSetUpEffect;
							}
						}

						/* Create new material */
						instanceMaterial = Library_SpriteStudio6.Data.Shader.MaterialCreateEffect(functionMaterialSetUp, shader, operationBlend, masking, false);
						if(null == instanceMaterial)
						{	/* Miss-Create */
							return(null);
						}
						instanceMaterial.mainTexture = tableTexture[indexCellMap];
						DataAppend(codeHash, instanceMaterial);
					}
				}

				return(instanceMaterial);
			}

			public UnityEngine.Material MaterialReplaceAnimation(	int indexCellMap,
																	Library_SpriteStudio6.KindOperationBlend operationBlend,
																	Library_SpriteStudio6.KindMasking masking,
																	string nameShader,
																	UnityEngine.Material material
																)
			{
				long codeHash = Library_SpriteStudio6.Control.CacheMaterial.InformationData.CodeGetAnimation(	indexCellMap,
																												operationBlend,
																												masking,
																												nameShader
																										);
				int indexMaterial = IndexGet(codeHash);
				if(0 > indexMaterial)
				{	/* Not exist */
					return(null);
				}

				/* Replace material */
				UnityEngine.Material instanceMaterialOld = Data[indexMaterial].Instance;
				if(null == material)
				{	/* Remove */
					Data.RemoveAt(indexMaterial);
				}
				else
				{	/* Overwrite */
					InformationData information = new InformationData(Data[indexMaterial].CodeHash, material);
					Data[indexMaterial] = information;
				}

				return(instanceMaterialOld);
			}
			public UnityEngine.Material MaterialReplaceEffect(	int indexCellMap,
																	Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
																Library_SpriteStudio6.KindMasking masking,
																string nameShader,
																UnityEngine.Material material
															)
			{
				long codeHash = Library_SpriteStudio6.Control.CacheMaterial.InformationData.CodeGetEffect(	indexCellMap,
																											operationBlend,
																											masking,
																											nameShader
																									);
				int indexMaterial = IndexGet(codeHash);
				if(0 > indexMaterial)
				{	/* Not exist */
					return(null);
				}

				/* Replace material */
				UnityEngine.Material instanceMaterialOld = Data[indexMaterial].Instance;
				if(null == material)
				{	/* Remove */
					Data.RemoveAt(indexMaterial);
				}
				else
				{	/* Overwrite */
					InformationData information = new InformationData(Data[indexMaterial].CodeHash, material);
					Data[indexMaterial] = information;
				}

				return(instanceMaterialOld);
			}

			public UnityEngine.Shader ShaderReplaceStandardPixelAnimation(	UnityEngine.Shader shader,
																			UnityEngine.Shader shaderDefault,
																			Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp,
																			Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUpDefault,
																			bool flagReplaceMaterial
																		)
			{
				UnityEngine.Shader shaderOld = ShaderStandardAnimation;

				/* Shader replace */
				if(null == shader)
				{
					shader = shaderDefault;
				}
				if(null == functionMaterialSetUp)
				{
					functionMaterialSetUp = functionMaterialSetUpDefault;
				}
				ShaderStandardAnimation = shader;
				FunctionMaterialSetUpAnimation = functionMaterialSetUp;

				/* Material (using corresponding shader) replace */
				if(true == flagReplaceMaterial)
				{

					MaterialReplaceShaderStandard(shader, functionMaterialSetUp, false, false);
				}

				return(shaderOld);
			}
			public UnityEngine.Shader ShaderReplaceStandardPixelEffect(	UnityEngine.Shader shader,
																		UnityEngine.Shader shaderDefault,
																		Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp,
																		Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUpDefault,
																		bool flagReplaceMaterial
																	)
			{
				UnityEngine.Shader shaderOld = ShaderStandardEffect;

				/* Shader replace */
				if(null == shader)
				{
					shader = shaderDefault;
				}
				if(null == functionMaterialSetUp)
				{
					functionMaterialSetUp = functionMaterialSetUpDefault;
				}
				ShaderStandardEffect = shader;
				FunctionMaterialSetUpEffect = functionMaterialSetUp;

				/* Material (using corresponding shader) replace */
				if(true == flagReplaceMaterial)
				{
					MaterialReplaceShaderStandard(shader, functionMaterialSetUp, true, false);
				}

				return(shaderOld);
			}
			public UnityEngine.Shader ShaderReplaceStandardStencil(	UnityEngine.Shader shader,
																	UnityEngine.Shader shaderDefault,
																	Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUpDefault,
																	bool flagReplaceMaterial
																)
			{
				UnityEngine.Shader shaderOld = ShaderStandardStencil;

				/* Shader replace */
				if(null == shader)
				{
					shader = shaderDefault;
				}
				ShaderStandardStencil = shader;
				/* MEMO: For "Stencil" is shared with for "Animation". */
				Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp = FunctionMaterialSetUpAnimation;

				/* Material (using corresponding shader) replace */
				if(true == flagReplaceMaterial)
				{
					if(null == functionMaterialSetUp)
					{
						functionMaterialSetUp = functionMaterialSetUpDefault;
					}

					MaterialReplaceShaderStandard(shader, functionMaterialSetUp, false, true);
				}

				return(shaderOld);
			}
			public void MaterialReplaceShaderStandard(	UnityEngine.Shader shader,
														Library_SpriteStudio6.CallBack.FunctionMaterialSetUp functionMaterialSetUp,
														bool flagIsEffect,
														bool flagIsStencil
													)
			{
				long codeShader = InformationData.CodeGetNameShader(null, flagIsEffect);	/* Standard-Shader */

				int countData = Data.Count;
				for(int i=(countData-1); i>=0; i--)
				{
					long codeCache = Data[i].CodeHash;
					if((codeCache & (InformationData.MaskCodeNameShader | InformationData.FlagCodeIsEffect)) == codeShader)
					{
						/* MEMO: When "material is invalid" or "shader is None (null)", */
						/*       remove the material cache that using standard shader.  */
						UnityEngine.Material material = Data[i].Instance;
						if((null == material) || (null == shader))
						{	/* Invalid cache */
							Data.RemoveAt(i);
						}
						else
						{	/* Valid cache */
							/* Extract Shader Setting */
							int indexCellMap = (int)((codeCache >> InformationData.CountShiftCodeIndexCellMap) & InformationData.MaskCodeIndexCellMap);
							if((InformationData.MaskCodeIndexCellMap >> 1) < indexCellMap)
							{	/* Minus */
								indexCellMap |= ~((int)InformationData.MaskCodeIndexCellMap);
							}
							int operationBlend = (int)((codeCache >> InformationData.CountShiftCodeOperation) & InformationData.MaskCodeOperation);
							if((InformationData.MaskCodeOperation >> 1) < operationBlend)
							{	/* Minus */
								if(false == flagIsStencil)
								{	/* Ignore for Stencil */
									continue;
								}
								operationBlend |= ~((int)InformationData.MaskCodeOperation);
							}
							else
							{	/* Plus */
								if(true == flagIsStencil)
								{	/* Ignore for Animation */
									continue;
								}
							}
							Library_SpriteStudio6.KindMasking masking = (Library_SpriteStudio6.KindMasking)((codeCache >> InformationData.CountShiftCodeMasking) & InformationData.MaskCodeMasking);

							/* Overwrite material */
							/* MEMO: "Keyword"s need to be reconfigured when change shader, so reset material. */
							material.shader = shader;
							material = functionMaterialSetUp(material, operationBlend, masking, false);
							if(null == material)
							{	/* Failure to set up */
								Data.RemoveAt(i);
								continue;
							}

							/* Replace Cache */
							/* MEMO: Since material is exchanged between Standard-Shaders, "Code" will not change. */
							/* MEMO: Since changing instance of material directly, no need to recreate cache. */
//							InformationData information = new InformationData(codeCache, material);
//							Data[i] = information;
						}
					}
				}
			}

			public void TextureReplace(int indexCellMap, UnityEngine.Texture texture)
			{
				if(0 > indexCellMap)
				{
					return;
				}

				long codeIndexCellMap = InformationData.CodeGetIndexCellMap(indexCellMap);

				int countData = Data.Count;
				for(int i=(countData-1); i>=0; i--)
				{
					long codeCache = Data[i].CodeHash;
					if((codeCache & InformationData.MaskCodeIndexCellMap) == codeIndexCellMap)
					{
						/* MEMO: When "material is invalid" or "shader is None (null)", */
						/*       remove the material cache that using standard shader.  */
						UnityEngine.Material material = Data[i].Instance;
						if((null == material) || (null == texture))
						{	/* Invalid cache */
							Data.RemoveAt(i);
						}
						else
						{	/* Valid cache */
							/* MEMO: When replacing texture, change material directly.                */
							/*       Since no change in blend-state and shader, not rebuild material. */
							material.mainTexture = texture;
						}
					}
				}
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			internal struct InformationData
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				internal long CodeHash;
				internal Material Instance;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				internal InformationData(long codeHash, UnityEngine.Material instance)
				{
					CodeHash = codeHash;
					Instance = instance;
				}

				/* MEMO: Possess all information in raw.                                                    */
				/*       Not just a value for identification, but is also used extracting original setting. */
				internal static long CodeGetAnimation(int indexCellMap, KindOperationBlend operationBlend, Library_SpriteStudio6.KindMasking masking, string nameShader)
				{
					long code;
					if(null == nameShader)
					{
						code = (long)CodeHashNameShaderDefault;
					}
					else
					{
						code = (long)(nameShader.GetHashCode());
					}

					code &= MaskCodeNameShader;
					code |= ((long)operationBlend & MaskCodeOperation) << CountShiftCodeOperation;
					code |= ((long)masking & MaskCodeMasking) << CountShiftCodeMasking;
					code |= ((long)indexCellMap & MaskCodeIndexCellMap) << CountShiftCodeIndexCellMap;
//					code &= ~FlagCodeIsEffect;	/* for Animation (not for Effect) */

					return(code);
				}
				internal static long CodeGetEffect(int indexCellMap, KindOperationBlendEffect operationBlend, Library_SpriteStudio6.KindMasking masking, string nameShader)
				{
					long code;
					if(null == nameShader)
					{
						code = (long)CodeHashNameShaderDefault;
					}
					else
					{
						code = (long)(nameShader.GetHashCode());
					}

					code &= MaskCodeNameShader;
					code |= ((long)operationBlend & MaskCodeOperation) << CountShiftCodeOperation;
					code |= ((long)masking & MaskCodeMasking) << CountShiftCodeMasking;
					code |= ((long)indexCellMap & MaskCodeIndexCellMap) << CountShiftCodeIndexCellMap;
					code |= FlagCodeIsEffect;	/* for Effect */

					return(code);
				}

				internal static long CodeGetNameShader(UnityEngine.Shader shader, bool flagIsEffect)
				{
					long code;
					if(null == shader)
					{
						code = (long)CodeHashNameShaderDefault;
					}
					else
					{
						code = (long)((shader.name).GetHashCode());
					}

					code &= MaskCodeNameShader;
					if(true == flagIsEffect)
					{
						code |= FlagCodeIsEffect;
					}
					return(code);
				}

				internal static long CodeGetIndexCellMap(int indexCellMap)
				{
					return(((long)indexCellMap & MaskCodeIndexCellMap) << CountShiftCodeIndexCellMap);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string NameShaderDefault = "__SS6Shader_Default__";
				private readonly static int CodeHashNameShaderDefault = NameShaderDefault.GetHashCode();

				internal const long MaskCodeNameShader = 0x00000000ffffffffL;
				internal const long MaskCodeOperation = 0x00000000000000ffL;
				internal const long MaskCodeMasking = 0x000000000000000fL;
				internal const long MaskCodeIndexCellMap = 0x0000000000000fffL;
				internal const long MaskCodeIsEffect = 0x0000000000000001L;

//				internal const int CountShiftCodeNameShader = 0;
				internal const int CountShiftCodeOperation = 32;
				internal const int CountShiftCodeMasking = 40;
				internal const int CountShiftCodeIndexCellMap = 44;
				internal const int CountShiftCodeIsEffect = 61;

				internal const long FlagCodeIsEffect = MaskCodeIsEffect << CountShiftCodeIsEffect;
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}

		/* MEMO: This class is specialized for static allocation of "CacheMaterial". */
		/*       Currently dedicated to mode "UnityUI".                              */
		/*       Instance cannot be purged, so reference-count management is added.  */
		internal class CacheMaterialStatic
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public List<InformationData> Data;

			public bool StatusIsBootedUp
			{
				get
				{
					return(null != Data);	/*  ? true : false*/
				}
			}
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public bool BootUp(int capacity)
			{
				Data = new List<InformationData>(capacity);
				Data.Clear();

				return(true);
			}

			public void ShutDown(bool flagDestroyInstance)
			{
				DataPurge(flagDestroyInstance);

				Data = null;
			}

			public void DataPurge(bool flagDestroyInstance)
			{
				if(false == StatusIsBootedUp)
				{
					return;
				}

				int countData = Data.Count;
				for(int i=(countData-1); i>=0; i--)
				{
					DataRelease(i, flagDestroyInstance);
				}
				Data.Clear();
			}

			public void DataAppend(long codeHash, UnityEngine.Material instanecMaterial)
			{
				InformationData informationData = new InformationData(codeHash, instanecMaterial);
				informationData.Count++;
				Data.Add(informationData);
			}

			public void DataRelease(int index, bool flagDestroyInstance=false)
			{
				if(true == flagDestroyInstance)
				{
					UnityEngine.Material material = Data[index].Instance;
					if(null != material)
					{
						Utility.Asset.ObjectDestroy(material);
					}
				}

				Data.RemoveAt(index);
			}

			public int IndexGet(long codeHash)
			{
				/* MEMO: Normally, Binary-Search is faster.                                      */
				/*       But since delete invalid-cache at the same time, use Linear-Search now. */
				int countData = Data.Count;
				for(int i=0; i<countData; i++)
				{
					if(Data[i].CodeHash == codeHash)
					{
						if(null != Data[i].Instance)
						{	/* Valid Instance */
							return(i);
						}

						/* Delete Invalid-Cache (Instance is Destroyed) */
						/* MEMO: Check the validity as materials may be destroyed from external. */
						/*       Especially, when switching between Play-Modes on Unity-Editor.  */
						Data.RemoveAt(i);
						i--;
						countData--;
					}
				}

				return(-1);
			}

			public int IndexGet(UnityEngine.Material instanceMaterial)
			{
				if(null == instanceMaterial)
				{
					return(-1);
				}

				int countData = Data.Count;
				for(int i=0; i<countData; i++)
				{
					if(Data[i].Instance == instanceMaterial)
					{
						return(i);
					}
				}

				return(-1);
			}

			public UnityEngine.Material MaterialGet(long codeHash)
			{
				/* MEMO: Normally, Binary-Search is faster.                                      */
				/*       But since delete invalid-cache at the same time, use Linear-Search now. */
				int countData = Data.Count;
				for(int i=0; i<countData; i++)
				{
					if(Data[i].CodeHash == codeHash)
					{
						UnityEngine.Material instanceMaterial = Data[i].Instance;
						if(null != instanceMaterial)
						{	/* Valid Instance */
							return(instanceMaterial);
						}

						/* Delete Invalid-Cache (Instance is Destroyed) */
						/* MEMO: Check the validity as materials may be destroyed from external. */
						/*       Especially, when switching between Play-Modes on Unity-Editor.  */
						Data.RemoveAt(i);
						i--;
						countData--;
					}
				}

				return(null);
			}
			public UnityEngine.Material MaterialGetSpriteUI(	UnityEngine.Texture texture,
																UnityEngine.Rendering.CompareFunction functionCompareStenctil,
																int idStencil,
																UnityEngine.Material materialMaster,
																bool flagCreateNew
															)
			{
				/* Get shader's hash-code */
				UnityEngine.Shader shader = null;
				int hashShader = 0;
				if(null == materialMaster)
				{
					shader = Library_SpriteStudio6.Data.Shader.SpriteUnityUI;
				}
				else
				{
					shader = materialMaster.shader;
				}
				hashShader = shader.GetHashCode();

				/* Get texture's hash-code */
				int hashTexture = 0;
				if(null != texture)
				{
					hashTexture = texture.GetHashCode();
				}

				/* Get material's hash-code */
				long codeHash = Library_SpriteStudio6.Control.CacheMaterialStatic.InformationData.CodeGetSpriteUI(	functionCompareStenctil,
																													idStencil,
																													hashShader,
																													hashTexture
																											);
				UnityEngine.Material instanceMaterial = null;
				int indexCacheMaterial = IndexGet(codeHash);
				if(0 > indexCacheMaterial)
				{	/* Not exist */
					if(true == flagCreateNew)
					{
						/* Create new material */
						if(null == materialMaster)
						{
							instanceMaterial = new UnityEngine.Material(shader);
						}
						else
						{
							instanceMaterial = new UnityEngine.Material(materialMaster);
						}

						/* Set parameters */
						instanceMaterial.mainTexture = texture;

						int propertyID = Library_SpriteStudio6.Data.Shader.IDPropertyUIStencilComp;
						if((0 <= propertyID) && (true == instanceMaterial.HasProperty(propertyID)))
						{
							instanceMaterial.SetFloat(propertyID, (float)functionCompareStenctil);
						}
						propertyID = Library_SpriteStudio6.Data.Shader.IDPropertyUIStencil;
						if((0 <= propertyID) && (true == instanceMaterial.HasProperty(propertyID)))
						{
							instanceMaterial.SetFloat(propertyID, (float)(idStencil & 0xff));
						}

						DataAppend(codeHash, instanceMaterial);
					}
				}
				else
				{	/* Exist */
					/* Increment reference-count. */
					InformationData dataCache = Data[indexCacheMaterial];
					dataCache.Count++;
					Data[indexCacheMaterial] = dataCache;

					instanceMaterial = dataCache.Instance;
				}

				return(instanceMaterial);
			}

			public void MaterialReleaseSpriteUI(UnityEngine.Material instanceMaterial)
			{
				int indexCacheMaterial = IndexGet(instanceMaterial);
				if(0 > indexCacheMaterial)
				{	/* Not exist */
					return;
				}

				/* Decrement reference-count. */
				InformationData dataCache = Data[indexCacheMaterial];
				dataCache.Count--;
				if(0 < dataCache.Count)
				{	/* Still referenced */
					Data[indexCacheMaterial] = dataCache;
				}
				else
				{	/* No referenced */
					DataRelease(indexCacheMaterial, true);
				}
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			internal struct InformationData
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				internal long CodeHash;
				internal int Count;
				internal Material Instance;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				internal InformationData(long codeHash, UnityEngine.Material instance)
				{
					CodeHash = codeHash;
					Count = 0;
					Instance = instance;
				}

				internal static long CodeGetSpriteUI(UnityEngine.Rendering.CompareFunction functionCompare, int idStencil, int hashShader, int hashTexture)
				{
					long hashShaderLong = (long)hashShader & MaskCodeName;
					long hashCombined = hashShaderLong;
					hashCombined <<= 5;
					hashCombined |= (hashCombined >> 32);
					hashCombined += hashShaderLong;
					hashCombined ^= (long)hashTexture;
					hashCombined &= MaskCodeName;

					long code = (long)hashCombined & MaskCodeName;
					code |= ((long)idStencil & MaskCodeFunctionStencil) << CountShiftCodeFunctionStencil;
					code |= ((long)functionCompare & MaskCodeIDStencil) << CountShiftCodeIDStencil;

					return(code);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				internal const long MaskCodeName = 0x00000000ffffffffL;
				internal const long MaskCodeFunctionStencil = 0x00000000000000ffL;
				internal const long MaskCodeIDStencil = 0x000000000000000fL;

//				internal const int CountShiftCodeName = 0;
				internal const int CountShiftCodeFunctionStencil = 32;
				internal const int CountShiftCodeIDStencil = 40;
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}

		public class InformationCollision
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public bool IsTrigger
			{
				get
				{
					if(true == Is2D)
					{
						if(null != Collider2D)
						{
							return(Collider2D.isTrigger);
						}
					}
					else
					{
						if(null != Collider)
						{
							return(Collider.isTrigger);
						}
					}

					return(false);
				}
			}

			public bool Is2D
			{
				get
				{
					return((null == Collider) && (null != Collider2D));	/* ? true : false */
				}
			}

			/* MEMO: "Collider/Pair/Contact" and "Collider2D/Pair2D/Contact2D" are mutually exclusive.
			/* MEMO: When (Is2D==false), Valid information. */
			private UnityEngine.Collider InstanceCollider = null;
			public UnityEngine.Collider Collider
			{
				get
				{
					return(InstanceCollider);
				}
			}
			public UnityEngine.Collider Pair = null;			/* When (Collider.IsTrigger==false), Valid information */
			public UnityEngine.Collision Contact = null;		/* When (Collider.IsTrigger==true), Valid information */

			/* MEMO: When (Is2D==true), Valid information. */
			private UnityEngine.Collider2D InstanceCollider2D = null;
			public UnityEngine.Collider2D Collider2D
			{
				get
				{
					return(InstanceCollider2D);
				}
			}
			public UnityEngine.Collider2D Pair2D = null;		/* When (Collider2D.IsTrigger==false), Valid information */
			public UnityEngine.Collision2D Contact2D = null;	/* When (Collider2D.IsTrigger==true), Valid information */
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public InformationCollision()
			{
				CleanUp();
			}
			public InformationCollision(InformationCollision original)
			{
				InstanceCollider = original.InstanceCollider;
				Pair = original.Pair;
				Contact = original.Contact;

				InstanceCollider2D = original.InstanceCollider2D;
				Pair2D = original.Pair2D;
				Contact2D = original.Contact2D;
			}

			private void CleanUp()
			{
				InstanceCollider = null;
				Pair = null;
				Contact = null;

				InstanceCollider2D = null;
				Pair2D = null;
				Contact2D = null;
			}

			public void BootUp(Collider collider)
			{	/* MEMO: for 3D */
				CleanUp();

				InstanceCollider = collider;
			}
			public void BootUp(Collider2D collider)
			{	/* MEMO: for 2D */
				CleanUp();

				InstanceCollider2D = collider;
			}

			public void ShutDownBase()
			{
				CleanUp();
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			#endregion Classes, Structs & Interfaces
		}
		#endregion Classes, Structs & Interfaces
	}

	public static partial class Script
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		[System.Serializable]
		public abstract class Root : MonoBehaviour
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public Script_SpriteStudio6_DataCellMap DataCellMap;
			internal Library_SpriteStudio6.Data.CellMap[] TableCellMap = null;
			public Script_SpriteStudio6_HolderAsset HolderAsset;

			internal Texture[] TableTexture = null;
			internal Library_SpriteStudio6.Control.CacheMaterial CacheMaterial = null;

			/* MEMO: Do not define "InstanceRootParent" to "internal" in order to remember parent-"Root" even after be instantiated on scene. */
			public Script_SpriteStudio6_Root InstanceRootParent;
			public GameObject InstanceGameObjectControl;

			public bool FlagHideForce;

			protected float RateOpacityForce = 1.0f;
			protected Vector2 RateScaleLocalForce = Vector2.one;

			internal Library_SpriteStudio6.Control.AdditionalColor AdditionalColor = null;

#if EXPERIMENT_FOR_CAMERA
			internal ArgumentContainer ArgumentShareEntire = null;
#endif

			internal Library_SpriteStudio6.Draw.Cluster ClusterDraw = null;	/* refer to Highest-Parent-Root's ClusterDraw */
			internal MeshRenderer InstanceMeshRenderer = null;
			internal MeshFilter InstanceMeshFilter = null;

			protected UnityEngine.Material[] TableMaterialCombined = null;	/* use only Highest-Parent-Root */
			protected UnityEngine.MaterialPropertyBlock[] TableMaterialPropertyBlockCombined = null;	/* use only Highest-Parent-Root */
			protected Mesh MeshCombined = null;	/* use only Highest-Parent-Root */
			internal UnityEngine.MaterialPropertyBlock[] TableMaterialPropertyBlock
			{
				get
				{
					return(TableMaterialPropertyBlockCombined);
				}
			}
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			protected bool BaseAwake()
			{
				/* Generate CellMap table */
				if(false == CellMapBootUp())
				{
					return(false);
				}

				return(true);
			}

			protected bool BaseStart()
			{
				/* Generate CellMap table */
//				if(false == CellMapBootUp())
//				{
//					return(false);
//				}

				/* SetUp Mesh-Renderer & Combined-Mesh */
				if(false == RendererBootUpDraw(true))
				{
					return(false);
				}

				return(true);
			}

			protected bool BaseLateUpdate(float timeElapsed)
			{
				/* Recover CellMap table */
				if(null == TableCellMap)
				{
					if(false == CellMapBootUp())
					{
						return(false);
					}
				}

				return(true);
			}

			protected void BaseShutDown()
			{
				if(null != MeshCombined)
				{
					Library_SpriteStudio6.Utility.Asset.ObjectDestroy(MeshCombined);
				}

				/* MEMO: Since CacheMaterial's ShutDown is done on the inherited class side, */
				/*         it is not necessary to do it here. (But it is done just in case.) */
				if(null != CacheMaterial)
				{
					CacheMaterial.ShutDown(true);
				}
			}
			protected void SelfDestroy()
			{
				if(null == InstanceRootParent)
				{
					if(null != InstanceGameObjectControl)
					{
						Library_SpriteStudio6.Utility.Asset.ObjectDestroy(InstanceGameObjectControl);
					}
					else
					{
						Library_SpriteStudio6.Utility.Asset.ObjectDestroy(gameObject);
					}
				}
			}

			public Script_SpriteStudio6_Root RootGetHighest()
			{
				Script_SpriteStudio6_Root root = null;
				Script_SpriteStudio6_Root rootParent = InstanceRootParent;
				while(null != rootParent)
				{
					root = rootParent;
					rootParent = root.InstanceRootParent;
				}
				return(root);
			}

			public int CountGetCellMap()
			{
				return((null == TableCellMap) ? -1 : TableCellMap.Length);
			}

			public int IndexGetCellMap(string name)
			{
				if(true == string.IsNullOrEmpty(name))
				{
					return(-1);
				}

				int count = TableCellMap.Length;
				for(int i=0; i<count; i++)
				{
					if(name == TableCellMap[i].Name)
					{
						return(i);
					}
				}
				return(-1);
			}

			public Library_SpriteStudio6.Data.CellMap DataGetCellMap(int index)
			{
#if UNITY_EDITOR
				if(null == TableCellMap)
				{
					return(null);
				}
#endif
				return(((0 > index) || (TableCellMap.Length <= index)) ? null : TableCellMap[index]);
			}

			public  Library_SpriteStudio6.Data.CellMap CellMapCopyFull(int indexCellMap, bool flagSourceIsOriginal)
			{
				Library_SpriteStudio6.Data.CellMap[] tableCellMapSource = (true == flagSourceIsOriginal) ? DataCellMap.TableCellMap : TableCellMap;

				if((0 > indexCellMap) || (tableCellMapSource.Length <= indexCellMap))
				{
					goto CellMapDuplicate_ErrorEnd;
				}

				Library_SpriteStudio6.Data.CellMap cellMap = new Library_SpriteStudio6.Data.CellMap();
				if(null == cellMap)
				{
					goto CellMapDuplicate_ErrorEnd;
				}
				Library_SpriteStudio6.Data.CellMap cellMapSource = tableCellMapSource[indexCellMap];

				int countCell = cellMapSource.TableCell.Length;
				cellMap.Name = string.Copy(cellMapSource.Name);
				cellMap.SizeOriginal = cellMapSource.SizeOriginal;
				cellMap.TableCell = new Library_SpriteStudio6.Data.CellMap.Cell[countCell];
				if(null == cellMap.TableCell)
				{
					goto CellMapDuplicate_ErrorEnd;
				}
				for(int i=0; i<countCell; i++)
				{
					cellMap.TableCell[i] = cellMapSource.TableCell[i];
				}
				return(cellMap);

			CellMapDuplicate_ErrorEnd:;
				return(null);
			}

			private bool CellMapBootUp()
			{
				if(null == DataCellMap)
				{
					return(false);
				}

				int countCellMap = DataCellMap.CountGetCellMap();
				if((null == TableCellMap) || (countCellMap > TableCellMap.Length))
				{
					TableCellMap = new Library_SpriteStudio6.Data.CellMap[DataCellMap.TableCellMap.Length];
					if(null == TableCellMap)
					{
						return(false);
					}
				}
				if(null != TableCellMap)
				{
					for(int i=0; i<countCellMap; i++)
					{
						TableCellMap[i] = DataCellMap.DataGetCellMap(i);
					}
				}

				return(true);
			}

			protected bool RendererBootUpDraw(bool flagStart)
			{
				if(true == flagStart)
				{
					if(null != InstanceRootParent)
					{	/* Boot up */
						InstanceMeshRenderer = gameObject.GetComponent<MeshRenderer>();
						if(null != InstanceMeshRenderer)
						{
							InstanceMeshRenderer.enabled = false;
						}

//						InstanceMeshRenderer = gameObject.GetComponent<Fileter>();
//						if(null != InstanceMeshRenderer)
//						{
//						}

						MeshCombined = null;

						return(true);
					}
				}
				else
				{	/* Recover */
					if(null != InstanceRootParent)
					{
						return(true);
					}
				}

				/* MEMO: Since can not solve by "RequireComponent" unconditionally, use "AddComponent" depending on situation. */
				InstanceMeshFilter = gameObject.GetComponent<MeshFilter>();
				if(null == InstanceMeshFilter)
				{
					InstanceMeshFilter = gameObject.AddComponent<MeshFilter>();
					if(null == InstanceMeshFilter)
					{
						goto RendererBootUpDraw_ErrorEnd;
					}
				}

				InstanceMeshRenderer = gameObject.GetComponent<MeshRenderer>();
				if(null == InstanceMeshRenderer)
				{
					InstanceMeshRenderer = gameObject.AddComponent<MeshRenderer>();
					if(null == InstanceMeshRenderer)
					{
						goto RendererBootUpDraw_ErrorEnd;
					}
				}

				if(null == MeshCombined)
				{
					MeshCombined = new Mesh();
					if(null == MeshCombined)
					{
						goto RendererBootUpDraw_ErrorEnd;
					}
					MeshCombined.hideFlags = HideFlags.DontSave;
					MeshCombined.Clear();
				}
				return(true);

			RendererBootUpDraw_ErrorEnd:;
				InstanceMeshFilter = null;
				InstanceMeshRenderer = null;
				MeshCombined = null;
				return(false);
			}

			protected bool CacheBootUpMaterial(int countTexture, int capacityMaterial)
			{
				/* Boot up Texture-Table */
				if(null == TableTexture)
				{	/* First */
					TableTexture = new Texture[countTexture];
				}

				/* Boot up Material-Cache */
				if(null == CacheMaterial)
				{	/* First */
					CacheMaterial = new Library_SpriteStudio6.Control.CacheMaterial();
					if(false == CacheMaterial.BootUp(capacityMaterial))
					{
						return(false);
					}
					CacheMaterial.ShaderStandardAnimation = null;	/* Not override */
					CacheMaterial.ShaderStandardEffect = null;	/* Not override */
					CacheMaterial.ShaderStandardStencil = null;	/* Not override */
				}

				return(true);
			}
			protected void CacheShutDownMaterial()
			{
				if(null != CacheMaterial)
				{
					CacheMaterial.ShutDown(true);
					CacheMaterial = null;
				}

				if(null != TableTexture)
				{
					int countTexture = TableTexture.Length;
					for(int i=0; i<countTexture; i++)
					{
						TableTexture[i] = null;
					}
					TableTexture = null;
				}
			}
			#endregion Functions

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
#if EXPERIMENT_FOR_CAMERA
			/* MEMO: This class is for runtime-only, so not serialized. */
			internal class ArgumentContainer
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public Transform TransformPartsCamera;			/* Transform for Camera */
				public Matrix4x4 MatrixCamera;					/* Instance of matrix used for camera */
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public ArgumentContainer()
				{
					CleanUp();
				}

				public void CleanUp()
				{
					TransformPartsCamera = null;
					MatrixCamera = Matrix4x4.identity;
				}
				#endregion Functions
			}
#endif
			#endregion Classes, Structs & Interfaces

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			protected const string NameBatchedMesh = "Batched Mesh";
			#endregion Enums & Constants
		}

		[System.Serializable]
		public abstract class Collider : MonoBehaviour
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			protected GameObject InstanceGamaObject = null;

			public Script_SpriteStudio6_Root InstanceRoot;
			public int IDParts;

			protected float Radius = 1.0f;
			protected Vector3 SizeRectangle = Vector3.one;
			protected Vector3 PivotRectangle = Vector3.zero;

			protected Library_SpriteStudio6.Control.InformationCollision InformationEnter = new Library_SpriteStudio6.Control.InformationCollision();
			protected Library_SpriteStudio6.Control.InformationCollision InformationStay = new Library_SpriteStudio6.Control.InformationCollision();
			protected Library_SpriteStudio6.Control.InformationCollision InformationExit = new Library_SpriteStudio6.Control.InformationCollision();
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			protected abstract void BootUp();
			internal abstract bool ColliderSetEnable(bool flagSwitch);
			internal abstract bool ColliderSetRectangle(ref Vector3 size, ref Vector3 pivot);
			internal abstract bool ColliderSetRadius(float radius);
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			#endregion Enums & Constants
		}
		#endregion Classes, Structs & Interfaces
	}

	public static partial class Draw
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		internal class Cluster
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			internal Chain ChainTop;
			internal Chain ChainLast;
			internal int Count;

			internal List<Vector3> ListCoordinate;
			internal List<Color32> ListColorParts;
			internal List<Vector4> ListUVTexture;	/* .x:U / .y:V / .z:PartsColor-Blend / .w:PartsColor-Power */
			internal List<Vector4> ListUVMinMax;	/* .x:Min-U / .y:Min-V / .z:Max-U / .w:Max-V */
			internal List<Vector4> ListUVAverage;	/* .x:Avr-U / .y:Avr-V / .z:(No-Use) / .w:(No-Use) */
			internal List<int> ListIndexVertex;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			internal void CleanUp()
			{
				ChainTop = null;
				ChainLast = null;
				Count = 0;

				ListCoordinate = null;
				ListColorParts = null;
				ListUVTexture = null;
				ListUVTexture = null;
				ListUVMinMax = null;
				ListUVAverage = null;
				ListIndexVertex = null;
			}

			internal bool BootUp(int countSpriteMax, int countMeshMax, int countParticleMax)
			{
				/* MEMO: Buffer length is added up as "Sprite is Triangle-4" and "Effect is Triangle-2". */
				int countVertex =	(countSpriteMax * (int)Library_SpriteStudio6.KindVertex.TERMINATOR4)
									+ (countMeshMax * (int)Library_SpriteStudio6.KindVertex.TERMINATOR3)
									+ (countParticleMax * (int)Library_SpriteStudio6.KindVertex.TERMINATOR2);
				int countIndexVertex =	(countSpriteMax * Library_SpriteStudio6.Draw.Model.TableIndexVertex_Triangle4.Length)
										+ (countMeshMax * (int)Library_SpriteStudio6.KindVertex.TERMINATOR3)
										+ (countParticleMax * Library_SpriteStudio6.Draw.Model.TableIndexVertex_Triangle2.Length);

				bool flagRenew;
				if(null == ListCoordinate)
				{
					flagRenew = true;
				}
				else
				{
					flagRenew = (ListCoordinate.Count < countVertex) ? true : false;
				}
				if(true == flagRenew)
				{
					ListCoordinate = new List<Vector3>(countVertex);
					if(null == ListCoordinate)
					{
						goto BootUp_ErrorEnd;
					}
				}
				ListCoordinate.Clear();

				if(null == ListColorParts)
				{
					flagRenew = true;
				}
				else
				{
					flagRenew = (ListColorParts.Count < countVertex) ? true : false;
				}
				if(true == flagRenew)
				{
					ListColorParts = new List<Color32>(countVertex);
					if(null == ListColorParts)
					{
						goto BootUp_ErrorEnd;
					}
				}
				ListColorParts.Clear();

				if(null == ListUVTexture)
				{
					flagRenew = true;
				}
				else
				{
					flagRenew = (ListUVTexture.Count < countVertex) ? true : false;
				}
				if(true == flagRenew)
				{
					ListUVTexture = new List<Vector4>(countVertex);
					if(null == ListUVTexture)
					{
						goto BootUp_ErrorEnd;
					}
				}
				ListUVTexture.Clear();

				if(null == ListUVMinMax)
				{
					flagRenew = true;
				}
				else
				{
					flagRenew = (ListUVMinMax.Count < countVertex) ? true : false;
				}
				if(true == flagRenew)
				{
					ListUVMinMax = new List<Vector4>(countVertex);
					if(null == ListUVMinMax)
					{
						goto BootUp_ErrorEnd;
					}
				}
				ListUVMinMax.Clear();

				if(null == ListUVAverage)
				{
					flagRenew = true;
				}
				else
				{
					flagRenew = (ListUVAverage.Count < countVertex) ? true : false;
				}
				if(true == flagRenew)
				{
					ListUVAverage = new List<Vector4>(countVertex);
					if(null == ListUVAverage)
					{
						goto BootUp_ErrorEnd;
					}
				}
				ListUVAverage.Clear();

				if(null == ListIndexVertex)
				{
					flagRenew = true;
				}
				else
				{
					flagRenew = (ListIndexVertex.Count < countIndexVertex) ? true : false;
				}
				if(true == flagRenew)
				{
					ListIndexVertex = new List<int>(countIndexVertex);
					if(null == ListIndexVertex)
					{
						goto BootUp_ErrorEnd;
					}
				}
				ListIndexVertex.Clear();

				return(true);

			BootUp_ErrorEnd:;
				CleanUp();
				return(false);
			}

			internal void DataPurge()
			{
				ChainTop = null;
				ChainLast = null;
				Count = 0;

				ListCoordinate.Clear();
				ListColorParts.Clear();
				ListUVTexture.Clear();
				ListUVMinMax.Clear();
				ListUVAverage.Clear();
				ListIndexVertex.Clear();
			}

			private bool ChainAdd(Chain chain)
			{
				if(null == ChainTop)
				{
					ChainTop = chain;
				}
				if(null != ChainLast)
				{
					ChainLast.ChainNext = chain;
				}
				ChainLast = chain;
				Count++;

				return(true);
			}

			internal Chain VertexAdd(	Chain chain,
										bool flagNotCombine,
										Material material,
										Library_SpriteStudio6.Control.Animation.Parts.BufferParameterSprite.BufferUniformShader uniformShader,
										int countVertex,
										Vector3[] tableCoordinate,
										Color32[] tableColorParts,
										Vector4[] tableUVTexture,
										Vector4[] tableUVMinMax,
										Vector4[] tableUVAverage
									)
			{
				int countCoordinate = ListCoordinate.Count;
				if((countCoordinate + countVertex) > ListCoordinate.Capacity)
				{	/* Capacity Over */
					return(null);
				}

//				if(null == material)
//				{	/* Material Invalid */
//					return(null);
//				}

				/* Decide Chain */
				/* MEMO: Do not unite Sub-Cluster calls. */
				/* MEMO: UniformShader is additional data to the material, so not included in comparing at "Mesh-Batching". */
				/*       Assumption, shader-constants will be the same when same material.                                  */
				/*       Shader-constants also vary with "Shader" attribute's parameters. However, "Mesh-batching" is not   */
				/*        performed at using of "Shader" attributes.                                                        */
				if(	(null != ChainLast)
					&& ((false == ChainLast.FlagNotCombine) && (false == flagNotCombine))
					&& (material == ChainLast.MaterialDraw)
				)
				{	/* Same Material (Use exist Chain) */
					chain = ChainLast;
				}
				else
				{	/* Use new Chain */
					chain.DataPurge();

					chain.MaterialDraw = material;
					chain.FlagNotCombine = flagNotCombine;
					chain.StatusIsValid = true;
					chain.UniformShader = uniformShader;

					ChainAdd(chain);
				}

				/* Add Vertex data */
				for(int i=0; i<countVertex; i++)
				{
					ListCoordinate.Add(tableCoordinate[i]);
					ListColorParts.Add(tableColorParts[i]);
					ListUVTexture.Add(tableUVTexture[i]);
					ListUVMinMax.Add(tableUVMinMax[i]);
					ListUVAverage.Add(tableUVAverage[i]);
				}

				/* Add Vertex-Index data */
				int[] tableIndexVertex = Library_SpriteStudio6.Draw.Model.TableIndexVertex_Triangle2;
				if((int)Library_SpriteStudio6.KindVertex.TERMINATOR4 == countVertex)
				{
					tableIndexVertex = Library_SpriteStudio6.Draw.Model.TableIndexVertex_Triangle4;
				}
				int countIndex = tableIndexVertex.Length;
				int indexVertexTop = ListIndexVertex.Count;
				for(int i=0; i<countIndex; i++)
				{
					ListIndexVertex.Add(tableIndexVertex[i] + countCoordinate);
				}

				chain.Add(indexVertexTop, countIndex);

				return(chain);
			}

			internal Chain VertexAddMesh(	Chain chain,
											bool flagNotCombine,
											Material material,
											Library_SpriteStudio6.Control.Animation.Parts.BufferParameterSprite.BufferUniformShader uniformShader,
											int[] tableIndexVertex,
											Vector3[] tableCoordinate,
											Color32[] tableColorParts,
											Vector4[] tableUVTexture,
											Vector4[] tableUVMinMax,
											Vector4[] tableUVAverage
										)
			{
				int countCoordinate = ListCoordinate.Count;
				int countVertex = tableCoordinate.Length;
				if((countCoordinate + countVertex) > ListCoordinate.Capacity)
				{	/* Capacity Over */
					return(null);
				}

//				if(null == material)
//				{	/* Material Invalid */
//					return(null);
//				}

				/* Decide Chain */
				/* MEMO: Do not unite Sub-Cluster calls. */
				/* MEMO: UniformShader is additional data to the material, so not included in comparing at "Mesh-Batching". */
				/*       Assumption, shader-constants will be the same when same material.                                  */
				/*       Shader-constants also vary with "Shader" attribute's parameters. However, "Mesh-batching" is not   */
				/*        performed at using of "Shader" attributes.                                                        */
				if(	(null != ChainLast)
					&& ((false == ChainLast.FlagNotCombine) && (false == flagNotCombine))
					&& (material == ChainLast.MaterialDraw)
				)
				{	/* Same Material (Use exist Chain) */
					chain = ChainLast;
				}
				else
				{	/* Use new Chain */
					chain.DataPurge();

					chain.MaterialDraw = material;
					chain.FlagNotCombine = flagNotCombine;
					chain.StatusIsValid = true;
					chain.UniformShader = uniformShader;

					ChainAdd(chain);
				}

				/* Add Vertex data */
				for(int i=0; i<countVertex; i++)
				{
					ListCoordinate.Add(tableCoordinate[i]);
					ListColorParts.Add(tableColorParts[i]);
					ListUVTexture.Add(tableUVTexture[i]);
					ListUVMinMax.Add(tableUVMinMax[i]);
					ListUVAverage.Add(tableUVAverage[i]);
				}

				/* Add Vertex-Index data */
				int countIndex = tableIndexVertex.Length;
				int indexVertexTop = ListIndexVertex.Count;
				for(int i=0; i<countIndex; i++)
				{
					ListIndexVertex.Add(tableIndexVertex[i] + countCoordinate);
				}

				chain.Add(indexVertexTop, countIndex);

				return(chain);
			}

			internal int Fix()
			{
				int count = 0;
				Chain chain = ChainTop;
				Chain chainPrevious = null;
				while(null != chain)
				{
					count += chain.Count;	/* MEMO: Even when integrate chains, ChainPrevious.Count has already been added. */

					if(	(null != chainPrevious)
						&& ((false == chainPrevious.FlagNotCombine) && (false == chain.FlagNotCombine))
						&& (chainPrevious.MaterialDraw == chain.MaterialDraw)
					)
					{	/* Same Material ... Integrate Chain */
						chainPrevious.CountVertex += chain.CountVertex;
						chainPrevious.Count += chain.Count;
						chainPrevious.ChainNext = chain.ChainNext;
						if(chain == ChainLast)
						{
							ChainLast = chainPrevious;
						}
						Count--;
						chain = chainPrevious;
					}

					chainPrevious = chain;
					chain = chain.ChainNext;
				}

				return(count);
			}

			internal bool MeshCombine(Mesh mesh, ref Material[] tableMaterial, ref MaterialPropertyBlock[] tableMaterialPropertyBlock)
			{	/* MEMO: Combine meshes by own processing in avoiding overhead. (unuse "Mesh.CombineMeshes") */
				int countMaterial = Count;

				/* Renew Material-array */
				/* MEMO: do not consume managed-heap, when same table length. */
				if((null == tableMaterial) || (countMaterial != tableMaterial.Length))
				{
					tableMaterial = new Material[countMaterial];
				}
				if((null == tableMaterialPropertyBlock) || (countMaterial != tableMaterialPropertyBlock.Length))
				{
					tableMaterialPropertyBlock = new MaterialPropertyBlock[countMaterial];
				}

				/* Create Mesh */
				if(0 < countMaterial)
				{
					/* MEMO: Caution that "SetXXXXX(n, array)" consumes managed-heap. ("SetXXXXX(n, List<int>)" does not) */
					mesh.SetVertices(ListCoordinate);
					mesh.SetUVs(0, ListUVTexture);
					mesh.SetUVs(1, ListUVMinMax);
					mesh.SetUVs(2, ListUVAverage);
					mesh.SetColors(ListColorParts);

					MaterialPropertyBlock materialPropertyBlock = null;
					Chain chain = ChainTop;
					if(1 < countMaterial)
					{	/* Multi-Materials */
						/* MEMO: If have multi material, mesh is made with submesh in order to stabilize drawing order. */
						mesh.subMeshCount = countMaterial;

						int indexMaterial = 0;

						/* MEMO: Excepting manually copying "List", I don't know ways to split list without consuming managed-heap. */
						/*       Time to copy is waste, but seem that CPU-load is light.                                            */
						int indexVertexTop;
						while(null != chain)
						{
							/* Set Mesh Information */

							indexVertexTop = chain.IndexVertex;
							mesh.SetTriangles(ListIndexVertex, indexVertexTop, chain.CountVertex, indexMaterial);

							/* Set Material */
							tableMaterial[indexMaterial] = chain.MaterialDraw;

							/* Set Material Property */
							materialPropertyBlock = chain.MaterialPropertyBlock;
							materialPropertyBlock.SetVector(Library_SpriteStudio6.Data.Shader.IDPropertyArgumentFs00, chain.UniformShader.ShaderArgumentPixel0);
							materialPropertyBlock.SetVector(Library_SpriteStudio6.Data.Shader.IDPropertyParameterFs00, chain.UniformShader.ShaderParameterPixel0);
							tableMaterialPropertyBlock[indexMaterial] = materialPropertyBlock;

							/* Follow Chain */
							indexMaterial++;
							chain = chain.ChainNext;
						}
					}
					else
					{	/* Single-Material */
						/* MEMO: If have single material, mesh is made with no submeshes so that dynamic-batching is easy to apply. */
						mesh.SetTriangles(ListIndexVertex, 0);
						tableMaterial[0] = chain.MaterialDraw;

						/* Set Material Property */
						materialPropertyBlock = chain.MaterialPropertyBlock;
						materialPropertyBlock.SetVector(Library_SpriteStudio6.Data.Shader.IDPropertyArgumentFs00, chain.UniformShader.ShaderArgumentPixel0);
						materialPropertyBlock.SetVector(Library_SpriteStudio6.Data.Shader.IDPropertyParameterFs00, chain.UniformShader.ShaderParameterPixel0);
						tableMaterialPropertyBlock[0] = materialPropertyBlock;
					}
				}

				return(true);
			}
			#endregion Functions

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			internal class Chain
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				internal Chain ChainNext;

				private FlagBitStatus Status;
				internal bool StatusIsValid
				{
					get
					{
						return(0 != (Status & FlagBitStatus.VALID));	/* ? true : false */
					}
					set
					{
						if(true == value)
						{
							Status |= FlagBitStatus.VALID;
						}
						else
						{
							Status &= ~FlagBitStatus.VALID;
						}
					}
				}
				internal bool FlagNotCombine
				{
					get
					{
						return(0 != (Status & FlagBitStatus.NOT_COMBINE));	/* ? true : false */
					}
					set
					{
						if(true == value)
						{
							Status |= FlagBitStatus.NOT_COMBINE;
						}
						else
						{
							Status &= ~FlagBitStatus.NOT_COMBINE;
						}
					}
				}
				internal UnityEngine.Material MaterialDraw;
				internal UnityEngine.MaterialPropertyBlock MaterialPropertyBlock = new UnityEngine.MaterialPropertyBlock();

				internal Library_SpriteStudio6.Control.Animation.Parts.BufferParameterSprite.BufferUniformShader UniformShader;

				internal int Count;
				internal int IndexVertex;
				internal int CountVertex;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				internal void CleanUp()
				{
					DataPurge();
//					MaterialPropertyBlock = null;
				}

				internal bool BootUp()
				{
					DataPurge();
//					MaterialPropertyBlock = new MaterialPropertyBlock();

					return(true);
				}

				internal void DataPurge()
				{
					ChainNext = null;

					Status = FlagBitStatus.CLEAR;
					MaterialDraw = null;
					/* MEMO: "MaterialPropertyBlock" is not set null because use repeatedly. */

					Count = 0;
					IndexVertex = 0;
					CountVertex = 0;
				}

				internal bool Add(int indexVertex, int countVertex)
				{
					if(1 > CountVertex)
					{
						IndexVertex = indexVertex;
					}
					CountVertex += countVertex;
					Count++;

					return(true);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				[System.Flags]
				public enum FlagBitStatus
				{
					VALID = 0x40000000,

					NOT_COMBINE = 0x00000001,

					CLEAR = 0x00000000
				}
				#endregion Enums & Constants
			}
			#endregion Classes, Structs & Interfaces
		}

		public static class Model
		{
			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			internal readonly static int[] TableIndexVertex_Triangle2 =
			{
				(int)Library_SpriteStudio6.KindVertex.LU, (int)Library_SpriteStudio6.KindVertex.RU, (int)Library_SpriteStudio6.KindVertex.RD,
				(int)Library_SpriteStudio6.KindVertex.RD, (int)Library_SpriteStudio6.KindVertex.LD, (int)Library_SpriteStudio6.KindVertex.LU
			};
			internal readonly static int[] TableIndexVertex_Triangle4 =
			{
				(int)Library_SpriteStudio6.KindVertex.C, (int)Library_SpriteStudio6.KindVertex.LU, (int)Library_SpriteStudio6.KindVertex.RU,
				(int)Library_SpriteStudio6.KindVertex.C, (int)Library_SpriteStudio6.KindVertex.RU, (int)Library_SpriteStudio6.KindVertex.RD,
				(int)Library_SpriteStudio6.KindVertex.C, (int)Library_SpriteStudio6.KindVertex.RD, (int)Library_SpriteStudio6.KindVertex.LD,
				(int)Library_SpriteStudio6.KindVertex.C, (int)Library_SpriteStudio6.KindVertex.LD, (int)Library_SpriteStudio6.KindVertex.LU,
			};
			public readonly static Vector3[] TableUVMapping = new Vector3[]
			{	/* MEMO: Used externally */
				/* LU */	new Vector3(-0.5f, 0.5f, 0.0f),
				/* RU */	new Vector3(0.5f, 0.5f, 0.0f),
				/* RD */	new Vector3(0.5f, -0.5f, 0.0f),
				/* LD */	new Vector3(-0.5f, -0.5f, 0.0f),
				/* C */		new Vector3(0.0f, 0.0f, 0.0f)
			};
			internal readonly static Vector3[] TableCoordinate = new Vector3[]
			{
				/* LU */	new Vector3(-0.5f, 0.5f, 0.0f),
				/* RU */	new Vector3(0.5f, 0.5f, 0.0f),
				/* RD */	new Vector3(0.5f, -0.5f, 0.0f),
				/* LD */	new Vector3(-0.5f, -0.5f, 0.0f),
				/* C */		new Vector3(0.0f, 0.0f, 0.0f)
			};
			internal readonly static Color32[] TableColor32 = new Color32[]
			{
				/* LU */	new Color32(0xff, 0xff, 0xff, 0xff),
				/* RU */	new Color32(0xff, 0xff, 0xff, 0xff),
				/* RD */	new Color32(0xff, 0xff, 0xff, 0xff),
				/* LD */	new Color32(0xff, 0xff, 0xff, 0xff),
				/* C */		new Color32(0xff, 0xff, 0xff, 0xff)
			};
			#endregion Enums & Constants
		}
		#endregion Classes, Structs & Interfaces
	}

	public static partial class Utility
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public static partial class Asset
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static GameObject GameObjectCreate(string name, bool flagActive, GameObject gameObjectParent)
			{
				GameObject gameObject = new GameObject(name);
				if(null != gameObject)
				{
					gameObject.SetActive(flagActive);
					Transform transform = gameObject.transform;
					if(null != gameObjectParent)
					{
						transform.parent = gameObjectParent.transform;
					}
					transform.localPosition = Vector3.zero;
					transform.localEulerAngles = Vector3.zero;
					transform.localScale = Vector3.one;
				}
				return(gameObject);
			}

			public static void ObjectDestroy(UnityEngine.Object instanceObject)
			{
				if(null != instanceObject)
				{
#if UNITY_EDITOR
					if(false == UnityEditor.EditorApplication.isPlaying)
					{
						UnityEngine.Object.DestroyImmediate(instanceObject);
					}
					else
					{
						UnityEngine.Object.Destroy(instanceObject);
					}
#else
					UnityEngine.Object.Destroy(instanceObject);
#endif
				}
			}

			public static Transform TransformSearchNameChild(string name, Transform transformParent)
			{
				if(null == transformParent)
				{
					return(null);
				}

				Transform transform;
				int countChild = transformParent.childCount;
				for(int i=0; i<countChild; i++)
				{
					transform = transformParent.GetChild(i);
					if(null != transform)
					{
						if(name == transform.name)
						{
							return(transform);
						}
					}

					transform = TransformSearchNameChild(name, transform);	/* Recursion */
					if(null != transform)
					{
						return(transform);
					}
				}
				return(null);
			}

			public static GameObject PrefabInstantiate(	GameObject prefab,
														GameObject gameObjectOld,
														GameObject gameObjectParent,
														bool flagInstanceUnderControlRenew
													)
			{
				/* Error-Check */
				if(null == prefab)
				{
					return(null);
				}

				GameObject gameObject = gameObjectOld;
				Transform transform;
				Transform transformParent = null;
				if(null != gameObjectParent)
				{
					transformParent = gameObjectParent.transform;
				}

				if(null == gameObject)
				{	/* Lost (Not-Found) */
					if(null != transformParent)
					{
						transform = transformParent.Find(prefab.name);
						if(null != transform)
						{	/* Found */
							gameObject = transform.gameObject;
						}
					}
				}

				if(true == flagInstanceUnderControlRenew)
				{	/* Renew Force */
					if(null != gameObject)
					{	/* Exist */
//						UnityEngine.Object.DestroyImmediate(gameObject);
						Utility.Asset.ObjectDestroy(gameObject);
					}
					gameObject = null;
				}

				if(null == gameObject)
				{	/* Instantiate */
#if UNITY_EDITOR
					gameObject = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
					if(null == gameObject)
					{	/* for not-prefab */
						gameObject = Object.Instantiate(prefab) as GameObject;
						gameObject.name = prefab.name;	/* Remove "(clone)" */
					}
#else
					gameObject = Object.Instantiate(prefab) as GameObject;
					gameObject.name = prefab.name;	/* Remove "(clone)" */
#endif
					transform = gameObject.transform;

					if (null != gameObjectParent)
					{
						transform = gameObject.transform;
						transform.parent = transformParent;
					}
					if(null != gameObject)
					{
						transform.localPosition = Vector3.zero;
						transform.localEulerAngles = Vector3.zero;
						transform.localScale = Vector3.one;
					}
				}

				return(gameObject);
			}
			#endregion Functions
		}

		public static partial class Interpolation
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static float Linear(float start, float end, float point)
			{
				return(((end - start) * point) + start);
			}

			public static float Hermite(float start, float end, float point, float speedStart, float speedEnd)
			{
				float pointPow2 = point * point;
				float pointPow3 = pointPow2 * point;
				return(	(((2.0f * pointPow3) - (3.0f * pointPow2) + 1.0f) * start)
						+ (((3.0f * pointPow2) - (2.0f * pointPow3)) * end)
						+ ((pointPow3 - (2.0f * pointPow2) + point) * (speedStart - start))
						+ ((pointPow3 - pointPow2) * (speedEnd - end))
					);
			}

			public static float Bezier(ref Vector2 start, ref Vector2 end, float point, ref Vector2 vectorStart, ref Vector2 vectorEnd)
			{
				float pointNow = Linear(start.x, end.x, point);
				float pointTemp;

				float areaNow = 0.5f;
				float RangeNow = 0.5f;

				float baseNow;
				float baseNowPow2;
				float baseNowPow3;
				float areaNowPow2;
				for(int i=0; i<8; i++)
				{
					baseNow = 1.0f - areaNow;
					baseNowPow2 = baseNow * baseNow;
					baseNowPow3 = baseNowPow2 * baseNow;
					areaNowPow2 = areaNow * areaNow;
					pointTemp = (baseNowPow3 * start.x)
								+ (3.0f * baseNowPow2 * areaNow * (vectorStart.x + start.x))
								+ (3.0f * baseNow * areaNowPow2 * (vectorEnd.x + end.x))
								+ (areaNow * areaNowPow2 * end.x);
					RangeNow *= 0.5f;
					areaNow += ((pointTemp > pointNow) ? (-RangeNow) : (RangeNow));
				}

				areaNowPow2 = areaNow * areaNow;
				baseNow = 1.0f - areaNow;
				baseNowPow2 = baseNow * baseNow;
				baseNowPow3 = baseNowPow2 * baseNow;
				return(	(baseNowPow3 * start.y)
						+ (3.0f * baseNowPow2 * areaNow * (vectorStart.y + start.y))
						+ (3.0f * baseNow * areaNowPow2 * (vectorEnd.y + end.y))
						+ (areaNow * areaNowPow2 * end.y)
					);
			}

			public static float Accelerate(float start, float end, float point)
			{
				return(((end - start) * (point * point)) + start);
			}

			public static float Decelerate(float start, float end, float point)
			{
				float pointInverse = 1.0f - point;
				float rate = 1.0f - (pointInverse * pointInverse);
				return(((end - start) * rate) + start);
			}

			public static float ValueGetFloat(	Library_SpriteStudio6.Utility.Interpolation.KindFormula formula,
												int frameNow,
												int frameStart,
												float valueStart,
												int frameEnd,
												float valueEnd,
												float curveFrameStart,
												float curveValueStart,
												float curveFrameEnd,
												float curveValueEnd
											)
			{
				if(frameEnd <= frameStart)
				{
					return(valueStart);
				}
				float frameNormalized = ((float)(frameNow - frameStart)) / ((float)(frameEnd - frameStart));
				frameNormalized = Mathf.Clamp01(frameNormalized);

				switch(formula)
				{
					case KindFormula.NON:
						return(valueStart);

					case KindFormula.LINEAR:
						return(Linear(valueStart, valueEnd, frameNormalized));

					case KindFormula.HERMITE:
						return(Hermite(valueStart, valueEnd, frameNormalized, curveValueStart, curveValueEnd));

					case KindFormula.BEZIER:
						{
							Vector2 start = new Vector2((float)frameStart, valueStart);
							Vector2 vectorStart = new Vector2(curveFrameStart, curveValueStart);
							Vector2 end = new Vector2((float)frameEnd, valueEnd);
							Vector2 vectorEnd = new Vector2(curveFrameEnd, curveValueEnd);
							return(Interpolation.Bezier(ref start, ref end, frameNormalized, ref vectorStart, ref vectorEnd));
						}
//						break;	/* Redundant */

					case KindFormula.ACCELERATE:
						return(Accelerate(valueStart, valueEnd, frameNormalized));

					case KindFormula.DECELERATE:
						return(Decelerate(valueStart, valueEnd, frameNormalized));

					default:
						break;
				}
				return(valueStart);	/* Error */
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public enum KindFormula
			{
				NON = 0,
				LINEAR,
				HERMITE,
				BEZIER,
				ACCELERATE,
				DECELERATE,
			}
			#endregion Enums & Constants
		}

		public static partial class Material
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static UnityEngine.Material[] TableCopyShallow(UnityEngine.Material[] tableMaterial)
			{
				if(null == tableMaterial)
				{
					return(null);
				}

				int countMaterial = tableMaterial.Length;
				UnityEngine.Material[] tableMaterialNew = new UnityEngine.Material[countMaterial];
				if(null == tableMaterialNew)
				{
					return(null);
				}

				for(int i=0; i<countMaterial; i++)
				{
					tableMaterialNew[i] = tableMaterial[i];
				}

				return(tableMaterialNew);
			}

			public static UnityEngine.Material[] TableCopyDeep(UnityEngine.Material[] tableMaterial)
			{
				if(null == tableMaterial)
				{
					return(null);
				}

				int countMaterial = tableMaterial.Length;
				UnityEngine.Material[] tableMaterialNew = new UnityEngine.Material[countMaterial];
				if(null == tableMaterialNew)
				{
					return(null);
				}

				for(int i=0; i<countMaterial; i++)
				{
					tableMaterialNew[i] = new UnityEngine.Material(tableMaterial[i]);
				}

				return(tableMaterialNew);
			}
			#endregion Functions
		}

		public static partial class Cell
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static Library_SpriteStudio6.Data.CellMap[] TableCopyMapShallow(Library_SpriteStudio6.Data.CellMap[] tableCellMap)
			{
				if(null == tableCellMap)
				{
					return(null);
				}

				int countCellMap = tableCellMap.Length;
				Library_SpriteStudio6.Data.CellMap[] tableCellMapNew = new Library_SpriteStudio6.Data.CellMap[countCellMap];
				if(null == tableCellMap)
				{
					return(null);
				}

				for(int i=0; i<countCellMap; i++)
				{
					tableCellMapNew[i] = MapCopyShallow(tableCellMap[i]);
					if(null == tableCellMapNew[i])
					{
						tableCellMapNew = null;
						return(null);
					}
				}
				return(tableCellMapNew);
			}

			public static Library_SpriteStudio6.Data.CellMap[] TableCopyMapDeep(Library_SpriteStudio6.Data.CellMap[] tableCellMap)
			{
				if(null == tableCellMap)
				{
					return(null);
				}

				int countCellMap = tableCellMap.Length;
				Library_SpriteStudio6.Data.CellMap[] tableCellMapNew = new Library_SpriteStudio6.Data.CellMap[countCellMap];
				if(null == tableCellMap)
				{
					return(null);
				}

				for(int i=0; i<countCellMap; i++)
				{
					tableCellMapNew[i] = MapCopyDeep(tableCellMap[i]);
					if(null == tableCellMapNew[i])
					{
						tableCellMapNew = null;
						return(null);
					}
				}
				return(tableCellMapNew);
			}

			public static Library_SpriteStudio6.Data.CellMap MapCopyShallow(Library_SpriteStudio6.Data.CellMap cellMap)
			{
				if(null == cellMap)
				{
					return(null);
				}

				Library_SpriteStudio6.Data.CellMap cellMapNew = new Library_SpriteStudio6.Data.CellMap();
				if(null == cellMapNew)
				{
					return(null);
				}

				if(false == cellMapNew.CopyShallow(cellMap))
				{
					cellMapNew = null;
				}

				return(cellMapNew);
			}

			public static Library_SpriteStudio6.Data.CellMap MapCopyDeep(Library_SpriteStudio6.Data.CellMap cellMap)
			{
				if(null == cellMap)
				{
					return(null);
				}

				Library_SpriteStudio6.Data.CellMap cellMapNew = new Library_SpriteStudio6.Data.CellMap();
				if(null == cellMapNew)
				{
					return(null);
				}

				if(false == cellMapNew.CopyDeep(cellMap))
				{
					cellMapNew = null;
				}

				return(cellMapNew);
			}

			public static bool CopyShallow(ref Library_SpriteStudio6.Data.CellMap.Cell cellOutput, ref Library_SpriteStudio6.Data.CellMap.Cell cell)
			{
				return(cellOutput.CopyShallow(ref cell));
			}

			public static bool CopyDeep(ref Library_SpriteStudio6.Data.CellMap.Cell cellOutput, ref Library_SpriteStudio6.Data.CellMap.Cell cell)
			{
				return(cellOutput.CopyDeep(ref cell));
			}

			public static bool CopyShallow(	Library_SpriteStudio6.Data.CellMap cellMapOutput,
											int indexCellOutput,
											ref Library_SpriteStudio6.Data.CellMap.Cell cell
										)
			{
				if((null == cellMapOutput) || (null == cellMapOutput.TableCell))
				{
					return(false);
				}
				if((0 > indexCellOutput) || (cellMapOutput.TableCell.Length <= indexCellOutput))
				{
					return(false);
				}

				return(cellMapOutput.TableCell[indexCellOutput].CopyShallow(ref cell));
			}

			public static bool CopyDeep(	Library_SpriteStudio6.Data.CellMap cellMapOutput,
											int indexCellOutput,
											ref Library_SpriteStudio6.Data.CellMap.Cell cell
										)
			{
				if((null == cellMapOutput) || (null == cellMapOutput.TableCell))
				{
					return(false);
				}
				if((0 > indexCellOutput) || (cellMapOutput.TableCell.Length <= indexCellOutput))
				{
					return(false);
				}

				return(cellMapOutput.TableCell[indexCellOutput].CopyDeep(ref cell));
			}

			public static bool CopyShallow(	Library_SpriteStudio6.Data.CellMap cellMapOutput, 
											int indexCellOutput,
											Library_SpriteStudio6.Data.CellMap cellMap,
											int indexCell,
											int countCell
										)
			{
				if((null == cellMapOutput) || (null == cellMapOutput.TableCell))
				{
					return(false);
				}
				if((null == cellMap) || (null == cellMap.TableCell))
				{
					return(false);
				}

				countCell = CountGetCopyCell(countCell, indexCell, cellMap.TableCell.Length, indexCellOutput, cellMapOutput.TableCell.Length);
				if(0 > countCell)
				{
					return(false);
				}

				for(int i=0; i<countCell; i++)
				{
					cellMapOutput.TableCell[indexCellOutput + i].CopyShallow(ref cellMap.TableCell[indexCell + i]);
				}

				return(true);
			}
			private static int CountGetCopyCell(int countCell, int indexCellInput, int countCellInput, int indexCellOutput, int countCellOutput)
			{
				if((0 > indexCellOutput) || (countCellOutput <= indexCellOutput))
				{
					return(-1);
				}
				if((0 > indexCellInput) || (countCellInput <= indexCellInput))
				{
					return(-1);
				}

				if(0 > countCell)
				{
					countCell = countCellInput - indexCellInput;
				}
//				int indexLastInput = indexCellInput + countCell;
				int indexLastOutput = indexCellOutput + countCell;
				if(indexLastOutput >= countCellOutput)
				{
					countCell -= (indexLastOutput - countCellOutput) + 1;
				}

				return(countCell);
			}

			public static bool CopyDeep(	Library_SpriteStudio6.Data.CellMap cellMapOutput, 
											int indexCellOutput,
											Library_SpriteStudio6.Data.CellMap cellMap,
											int indexCell,
											int countCell
										)
			{
				if((null == cellMapOutput) || (null == cellMapOutput.TableCell))
				{
					return(false);
				}
				if((null == cellMap) || (null == cellMap.TableCell))
				{
					return(false);
				}

				countCell = CountGetCopyCell(countCell, indexCell, cellMap.TableCell.Length, indexCellOutput, cellMapOutput.TableCell.Length);
				if(0 > countCell)
				{
					return(false);
				}

				for(int i=0; i<countCell; i++)
				{
					cellMapOutput.TableCell[indexCellOutput + i].CopyDeep(ref cellMap.TableCell[indexCell + i]);
				}

				return(true);
			}
			#endregion Functions
		}

		public static partial class Math
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static void CoordinateGetDiagonalIntersection(out Vector3 intersection, ref Vector3 LU, ref Vector3 RU, ref Vector3 LD, ref Vector3 RD)
			{
				/* MEMO: Z-Values are ignored. */
				intersection = Vector3.zero;

				float c1 = (LD.y - RU.y) * (LD.x - LU.x) - (LD.x - RU.x) * (LD.y - LU.y);
				float c2 = (RD.x - LU.x) * (LD.y - LU.y) - (RD.y - LU.y) * (LD.x - LU.x);
				float c3 = (RD.x - LU.x) * (LD.y - RU.y) - (RD.y - LU.y) * (LD.x - RU.x);
				float ca = c1 / c3;
				float cb = c2 / c3;

				if(((0.0f <= ca) && (1.0f >= ca)) && ((0.0f <= cb) && (1.0f >= cb)))
				{
					intersection.x = LU.x + ca * (RD.x - LU.x);
					intersection.y = LU.y + ca * (RD.y - LU.y);
				}
			}

			public static void QuaternionGetEulerAngels(out Quaternion quaternion, ref Vector3 anglesEuler)
			{
				float fixedValue = Mathf.Deg2Rad * 0.5f;
				float halfX = anglesEuler.x * fixedValue;
				float halfY = anglesEuler.y * fixedValue;
				float halfZ = anglesEuler.z * fixedValue;
				float cosHalfX = Mathf.Cos(halfX);
				float sinHalfX = Mathf.Sin(halfX);
				float cosHalfY = Mathf.Cos(halfY);
				float sinHalfY = Mathf.Sin(halfY);
				float cosHalfZ = Mathf.Cos(halfZ);
				float sinHalfZ = Mathf.Sin(halfZ);

				/* MEMO: X-Y-Z (SpriteStudio6) */
				quaternion.w = (cosHalfX * cosHalfY * cosHalfZ) - (sinHalfX * sinHalfY * sinHalfZ);
				quaternion.x = (sinHalfX * cosHalfY * cosHalfZ) + (cosHalfX * sinHalfY * sinHalfZ);
				quaternion.y = (cosHalfX * sinHalfY * cosHalfZ) - (sinHalfX * cosHalfY * sinHalfZ);
				quaternion.z = (cosHalfX * cosHalfY * sinHalfZ) + (sinHalfX * sinHalfY * cosHalfZ);
			}
			#endregion Functions
		}

		public static partial class Random
		{
			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public interface Generator
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				uint[] ListSeed
				{
					get;
				}
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				void InitSeed(uint seed);
				uint RandomUint32();
				double RandomDouble(double valueMax=1.0);
				float RandomFloat(float valueMax=1.0f);
				int RandomN(int valueMax);
				#endregion Functions
			}
			#endregion Classes, Structs & Interfaces
		}
		#endregion Classes, Structs & Interfaces
	}
	#endregion Classes, Structs & Interfaces
}
