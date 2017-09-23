/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Library_SpriteStudio6
{
	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindOperationBlend
	{
		NON = -1,

		MIX = 0,
		ADD,
		SUB,
		MUL,

		TERMINATOR,
	}

	public enum KindOperationBlendEffect
	{
		NON = -1,

		MIX = 0,
		ADD,
		ADD2,

		TERMINATOR,
		TERMINATOR_KIND = ADD,
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
		TERMINATOR4 = TERMINATOR,
		TERMINATOR2 = C
	}

	public enum KindStylePlay
	{
		NO_CHANGE = -1,
		NORMAL = 0,
		PINGPONG = 1,
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
		public delegate void FunctionUserData(Script_SpriteStudio6_Root scriptRoot, string nameParts, int indexParts, int indexAnimation, int frameDecode, int frameKeyData, ref Library_SpriteStudio6.Data.Animation.Attribute.UserData userData, bool flagWayBack);

		public delegate float FunctionTimeElapse(Script_SpriteStudio6_Root scriptRoot);
		public delegate float FunctionTimeElapseEffect(Script_SpriteStudio6_RootEffect scriptRoot);

		public delegate bool FunctionControlEndTrackPlay(Script_SpriteStudio6_Root scriptRoot, int indexTrackPlay);
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
				int frameEnd = CountFrame - 1;
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
				if(0 > frameRangeStart)
				{
					frameRangeStart = 0;
				}
				frameRangeStart += frameOffsetStart;
				if((0 > frameRangeStart) || (frameEnd < frameRangeStart))
				{
					frameRangeStart = 0;
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
				if(0 > frameRangeEnd)
				{
					frameRangeEnd = frameEnd;
				}
				frameRangeEnd += frameOffsetEnd;
				if((0 > frameRangeEnd) || (frameEnd < frameRangeEnd))
				{
					frameRangeEnd = frameEnd;
				}
			}
			#endregion Functions

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces

			[System.Serializable]
			public struct Parts
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public KindFormat Format;
				public FlagBitStatus StatusParts;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus Status;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 Position;	/* Always Compressed */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 Rotation;	/* Always Compressed */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 Scaling;	/* Always Compressed */

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat RateOpacity;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 PositionAnchor;	/* Reserved */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 SizeForce;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData UserData;	/* Trigger (Always Compressed) */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance Instance;	/* Trigger (Always Compressed) */
				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect Effect;	/* Trigger (Always Compressed) */

				public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat RadiusCollision;	/* for Sphere-Collider *//* Always Compressed */

				public AttributeGroupPlain Plain;
				public AttributeGroupFix Fix;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Format = KindFormat.PLAIN;
					StatusParts = FlagBitStatus.CLEAR;

					Status = null;

					Position = null;
					Rotation = null;
					Scaling = null;

					RateOpacity = null;

					PositionAnchor = null;
					SizeForce = null;

					UserData = null;
					Instance = null;
					Effect = null;

					RadiusCollision = null;

					Plain.Cell = null;
					Plain.ColorBlend = null;
					Plain.VertexCorrection = null;
					Plain.OffsetPivot = null;
					Plain.PositionTexture = null;
					Plain.ScalingTexture = null;
					Plain.RotationTexture = null;

					Fix.IndexCellMap = null;
					Fix.Coordinate = null;
					Fix.ColorBlend = null;
					Fix.UV0 = null;
					Fix.SizeCollision = null;
					Fix.PivotCollision = null;
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindFormat
				{	/* ERROR/NON: -1 */
					PLAIN = 0,	/* Data-Format: Plain-Data */
					FIX,	/* Data-Format: Precalculated "Fix Mesh" */
				}

				[System.Flags]
				public enum FlagBitStatus
				{
					VALID = 0x40000000,
					NOT_USED = 0x20000000,
					HIDE_FULL = 0x10000000,

					NO_POSITION = 0x08000000,
					NO_ROTATION = 0x04000000,
					NO_SCALING = 0x02000000,
					NO_TRANSFORMATION_TEXTURE = 0x01000000,

					NO_USERDATA = 0x00800000,
					NO_PARTSCOLOR = 0x00400000,

					CLEAR = 0x00000000
				}
				#endregion Enums & Constants

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				[System.Serializable]
				public struct AttributeGroupPlain
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell Cell;	/* Always Compressed */

					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerColorBlend ColorBlend;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection VertexCorrection;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 OffsetPivot;

					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 PositionTexture;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 ScalingTexture;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat RotationTexture;
					#endregion Variables & Properties
				}

				[System.Serializable]
				public struct AttributeGroupFix
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt IndexCellMap;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix Coordinate;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerColorBlendFix ColorBlend;
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix UV0;

					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 SizeCollision;	/* for Box-Collider *//* Always Compressed */
					public Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 PivotCollision;	/* for Box-Collider *//* Always Compressed */
					#endregion Variables & Properties
				}
				#endregion Classes, Structs & Interfaces
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
				public class ContainerColorBlend : Container<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlend, InterfaceContainerColorBlend> {}
				[System.Serializable]
				public class ContainerVertexCorrection : Container<Library_SpriteStudio6.Data.Animation.Attribute.VertexCorrection, InterfaceContainerVertexCorrection> {}
				[System.Serializable]
				public class ContainerUserData : Container<Library_SpriteStudio6.Data.Animation.Attribute.UserData, InterfaceContainerUserData> {}
				[System.Serializable]
				public class ContainerInstance : Container<Library_SpriteStudio6.Data.Animation.Attribute.Instance, InterfaceContainerInstance> {}
				[System.Serializable]
				public class ContainerEffect : Container<Library_SpriteStudio6.Data.Animation.Attribute.Effect, InterfaceContainerEffect> {}
				[System.Serializable]
				public class ContainerCoordinateFix : Container<Library_SpriteStudio6.Data.Animation.Attribute.CoordinateFix, InterfaceContainerCoordinateFix> {}
				[System.Serializable]
				public class ContainerColorBlendFix : Container<Library_SpriteStudio6.Data.Animation.Attribute.ColorBlendFix, InterfaceContainerColorBlendFix> {}
				[System.Serializable]
				public class ContainerUVFix : Container<Library_SpriteStudio6.Data.Animation.Attribute.UVFix, InterfaceContainerUVFix> {}
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

				public float PriorityParticle;
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

					PriorityParticle = 64.0f;
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

					PriorityParticle = original.PriorityParticle;
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
				{	/* CAUTION!: Obtain "TablePatternOffset" before executing this function. */
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
				Name = string.Copy(original.Name);
				SizeOriginal = original.SizeOriginal;
				TableCell = original.TableCell;
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
				}

				public void Duplicate(Cell original)
				{
					Name = string.Copy(original.Name);
					Rectangle = original.Rectangle;
					Pivot = original.Pivot;
				}
				#endregion Functions
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
				public KindColorLabel ColorLabel;
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
					OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.NON;
					ColorLabel = KindColorLabel.NON;

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
					ROOT = 0,	/* Root-Parts (Subspecies of "NULL"-Parts) */
					NULL,
					NORMAL_TRIANGLE2,	/* No use Vertex-Collection Sprite-Parts */
					NORMAL_TRIANGLE4,	/* Use Vertex-Collection Sprite-Parts */

					INSTANCE,
					EFFECT,

					TERMINATOR,
					NORMAL = TERMINATOR	/* NORMAL_TRIANGLE2 or NORMAL_TRIANGLE4 *//* only during import */
				}

				public enum KindColorLabel
				{
					NON = 0,
					RED,
					ORANGE,
					YELLOW,
					GREEN,
					BLUE,
					VIOLET,
					GRAY,
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
			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public readonly static UnityEngine.Shader[] TableSprite = new UnityEngine.Shader[(int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR]
			{
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Mix"),
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Add"),
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Sub"),
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Mul")
			};

			public readonly static UnityEngine.Shader[] TableEffect = new UnityEngine.Shader[(int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR]
			{
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Effect/Mix"),
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Effect/Add"),
				UnityEngine.Shader.Find("Custom/SpriteStudio6/Effect/Add2"),
			};
			#endregion Enums & Constants
		}
		#endregion Classes, Structs & Interfaces
	}

	public static partial class Control
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public static partial class Animation
		{
			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			/* Part: SpriteStudio6/Library/Control/AnimationTrack.cs */
			/* Part: SpriteStudio6/Library/Control/AnimationParts.cs */

			public class ColorBlend
			{
			}
			#endregion Classes, Structs & Interfaces
		}

		/* Part: SpriteStudio6/Library/Control/Effect.cs */
		#endregion Classes, Structs & Interfaces
	}

	public static partial class Script
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		[System.Serializable]
		public class Root : MonoBehaviour
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public Material[] TableMaterial;

			public Script_SpriteStudio6_DataCellMap DataCellMap;
			internal Library_SpriteStudio6.Data.CellMap[] TableCellMap = null;

			internal Script_SpriteStudio6_Root InstanceRootParent = null;

			public bool FlagReassignMaterialForce;
			public bool FlagHideForce;

			internal float RateOpacity = 1.0f;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			protected bool BaseAwake()
			{
#if true
				/* Reassignment for shader lost */
				/* MEMO: Memory leak occasionally, so normally no reassign. */
				if(true == FlagReassignMaterialForce)
				{
					int countTableMaterial = (null != TableMaterial) ? TableMaterial.Length : 0;
					Material instanceMaterial = null;
					for(int i=0; i<countTableMaterial; i++)
					{
						instanceMaterial = TableMaterial[i];
						if(null != instanceMaterial)
						{
							instanceMaterial.shader = Shader.Find(instanceMaterial.shader.name);
						}
					}
					instanceMaterial = null;
				}
#else
					int countTableMaterial = (null != TableMaterial) ? TableMaterial.Length : 0;
					Material materialOriginal = null;
					for(int i=0; i<countTableMaterial; i++)
					{
						materialOriginal = TableMaterial[i];
						if(null != materialOriginal)
						{
							TableMaterial[i] = new Material(materialOriginal);
						}
					}
					materialOriginal = null;
#endif

				return(true);
			}

			protected bool BaseStart()
			{
				/* Generate CellMap table */
				if(false == CellMapBootUp())
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
			#endregion Functions
		}
		#endregion Classes, Structs & Interfaces
	}

	public static partial class Draw
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		internal struct BufferMesh
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			internal Vector3[] Coordinate;
			internal Color32[] ColorOverlay;
			internal Vector2[] UV;
			internal Vector2[] UV2;

			internal Mesh InstanceMesh;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			internal void CleanUp()
			{
				Coordinate = null;
				ColorOverlay = null;
				UV = null;
				UV2 = null;

				InstanceMesh = null;
			}

			internal bool BootUp(int countVertex)
			{
				Coordinate = new Vector3[countVertex];
				if(null == Coordinate)
				{
					goto BootUp_ErrorEnd;
				}
				ColorOverlay = new Color32[countVertex];
				if(null == ColorOverlay)
				{
					goto BootUp_ErrorEnd;
				}
				UV = new Vector2[countVertex];
				if(null == UV)
				{
					goto BootUp_ErrorEnd;
				}
				UV2 = new Vector2[countVertex];
				if(null == UV2)
				{
					goto BootUp_ErrorEnd;
				}
				InstanceMesh = new Mesh();
				if(null == InstanceMesh)
				{
					goto BootUp_ErrorEnd;
				}
				InstanceMesh.Clear();

				Vector2 PartsColor = new Vector2((float)((int)Library_SpriteStudio6.KindOperationBlend.NON + 1), 1.0f);
				for(int i=0; i<countVertex; i++)
				{
					ColorOverlay[i] = TableColor32[i];
					Coordinate[i] = TableCoordinate[i];
					UV[i] = TableUVMapping[i];
					UV2[i] = PartsColor;
				}

				InstanceMesh.vertices = Coordinate;
				InstanceMesh.uv = UV;
				InstanceMesh.uv2 = UV2;
				InstanceMesh.colors32 = ColorOverlay;
				InstanceMesh.normals = null;	/* Disused */
				InstanceMesh.triangles = ((int)Library_SpriteStudio6.KindVertex.TERMINATOR4 == countVertex) ? TableIndexVertex_Triangle4 : TableIndexVertex_Triangle2;

				return(true);

			BootUp_ErrorEnd:;
				CleanUp();
				return(false);
			}

			public void Exec(Transform transform, Material material, int layer)
			{
				InstanceMesh.vertices = Coordinate;
				InstanceMesh.uv = UV;
				InstanceMesh.uv2 = UV2;
				InstanceMesh.colors32 = ColorOverlay;

				Graphics.DrawMesh(InstanceMesh, transform.localToWorldMatrix, material, layer);
			}

			public void Exec(ref Matrix4x4 matrixTransform, Material material, int layer)
			{
				InstanceMesh.vertices = Coordinate;
				InstanceMesh.uv = UV;
				InstanceMesh.uv2 = UV2;
				InstanceMesh.colors32 = ColorOverlay;

				Graphics.DrawMesh(InstanceMesh, matrixTransform, material, layer);
			}

			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			private readonly static int[] TableIndexVertex_Triangle2 =
			{
				(int)Library_SpriteStudio6.KindVertex.LU, (int)Library_SpriteStudio6.KindVertex.RU, (int)Library_SpriteStudio6.KindVertex.RD,
				(int)Library_SpriteStudio6.KindVertex.RD, (int)Library_SpriteStudio6.KindVertex.LD, (int)Library_SpriteStudio6.KindVertex.LU
			};
			private readonly static int[] TableIndexVertex_Triangle4 =
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
//			private readonly static Vector3[] TableNormal = new Vector3[]
//			{
//				/* LU */	new Vector3(0.0f, 0.0f, 1.0f),
//				/* RU */	new Vector3(0.0f, 0.0f, 1.0f),
//				/* RD */	new Vector3(0.0f, 0.0f, 1.0f),
//				/* LD */	new Vector3(0.0f, 0.0f, 1.0f),
//				/* C */		new Vector3(0.0f, 0.0f, 1.0f)
//			};
			private readonly static Vector3[] TableCoordinate = new Vector3[]
			{
				/* LU */	new Vector3(-0.5f, 0.5f, 0.0f),
				/* RU */	new Vector3(0.5f, 0.5f, 0.0f),
				/* RD */	new Vector3(0.5f, -0.5f, 0.0f),
				/* LD */	new Vector3(-0.5f, -0.5f, 0.0f),
				/* C */		new Vector3(0.0f, 0.0f, 0.0f)
			};
			private readonly static Color32[] TableColor32 = new Color32[]
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
		public class Asset
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
				Transform transformParent = gameObjectParent.transform;
				Transform transform;

				if(null == gameObject)
				{	/* Lost (Not-Found) */
					transform = transformParent.Find(prefab.name);
					if(null != transform)
					{	/* Found */
						gameObject = transform.gameObject;
					}
				}

				if(true == flagInstanceUnderControlRenew)
				{	/* Renew Force */
					if(null != gameObject)
					{	/* Exist */
						UnityEngine.Object.DestroyImmediate(gameObject);
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

		public static class Math
		{
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
