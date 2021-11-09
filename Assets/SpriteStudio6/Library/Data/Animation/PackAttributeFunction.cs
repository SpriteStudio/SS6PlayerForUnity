/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
// #define TEST_PERFORMANCE_SPEEDUP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Library_SpriteStudio6
{
	public static partial class Data
	{
		public partial class Animation
		{
			public static partial class PackAttribute
			{
				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindTypePack
				{
					STANDARD_UNCOMPRESSED = 0,	/* Standard-Uncompressed (Plain Array) */
					STANDARD_CPE,	/* Standard-Compressed (Changing-Point Extracting) */
					CPE_FLYWEIGHT,	/* CPE & GoF-Flyweight (Commonized CPE's dictionary) */
					CPE_INTERPOLATE,	/* CPE & Interpolate (Linear,Acceleration and Deceleration: Interpolate / Other: CPE) */

					TERMINATOR,
				}
				#endregion Enums & Constants

				/* ----------------------------------------------- Functions */
				#region Functions
				public static CapacityContainer CapacityGet(KindTypePack pack)
				{
					switch(pack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							return(StandardUncompressed.Capacity);

						case KindTypePack.STANDARD_CPE:
							return(StandardCPE.Capacity);

						case KindTypePack.CPE_FLYWEIGHT:
							return(CPE_Flyweight.Capacity);

						case KindTypePack.CPE_INTERPOLATE:
							return(CPE_Interpolate.Capacity);

						default:
							break;
					}
					return(null);
				}

				public static string IDGetPack(KindTypePack typePack)
				{
					switch(typePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							return(StandardUncompressed.ID);

						case KindTypePack.STANDARD_CPE:
							return(StandardCPE.ID);

						case KindTypePack.CPE_FLYWEIGHT:
							return(CPE_Flyweight.ID);

						case KindTypePack.CPE_INTERPOLATE:
							return(CPE_Interpolate.ID);

						default:
							break;
					}
					return(null);
				}

				public static void BootUpFunctionInt(ContainerInt container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionInt;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionInt;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = CPE_Flyweight.FunctionInt;
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = CPE_Interpolate.FunctionInt;
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionFloat(ContainerFloat container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionFloat;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionFloat;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = CPE_Flyweight.FunctionFloat;
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = CPE_Interpolate.FunctionFloat;
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionVector2(ContainerVector2 container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionVector2;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionVector2;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = CPE_Flyweight.FunctionVector2;
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = CPE_Interpolate.FunctionVector2;
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionVector3(ContainerVector3 container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionVector3;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionVector3;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = CPE_Flyweight.FunctionVector3;
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = CPE_Interpolate.FunctionVector3;
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionStatus(ContainerStatus container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionStatus;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionStatus;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = null;	/* Not Support */
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionCell(ContainerCell container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionCell;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionCell;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = null;	/* Not Support */
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionPartsColor(ContainerPartsColor container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionPartsColor;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionPartsColor;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = CPE_Flyweight.FunctionPartsColor;
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = CPE_Interpolate.FunctionPartsColor;
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionVertexCorrection(ContainerVertexCorrection container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionVertexCorrection;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionVertexCorrection;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = CPE_Flyweight.FunctionVertexCorrection;
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = CPE_Interpolate.FunctionVertexCorrection;
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionUserData(ContainerUserData container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionUserData;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = null;	/* Not Support */
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionInstance(ContainerInstance container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionInstance;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = null;	/* Not Support */
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionEffect(ContainerEffect container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionEffect;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = null;	/* Not Support */
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionDeform(ContainerDeform container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionDeform;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionDeform;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = CPE_Interpolate.FunctionDeform;
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionShader(ContainerShader container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionShader;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionShader;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = CPE_Interpolate.FunctionShader;
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionSignal(ContainerSignal container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionSignal;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							container.Function = null;	/* Not Support */
							break;

						case KindTypePack.CPE_INTERPOLATE:
							container.Function = null;	/* Not Support */
							break;

						default:
							break;
					}
				}

				public static bool DictionaryBootUp(int indexAnimation, int indexParts, Script_SpriteStudio6_DataAnimation dataAnimation)
				{
					bool flagSuccess = true;

					flagSuccess &= StandardUncompressed.DictionaryBootUp(indexAnimation, indexParts, dataAnimation);
					flagSuccess &= StandardCPE.DictionaryBootUp(indexAnimation, indexParts, dataAnimation);
					flagSuccess &= CPE_Flyweight.DictionaryBootUp(indexAnimation, indexParts, dataAnimation);
					flagSuccess &= CPE_Interpolate.DictionaryBootUp(indexAnimation, indexParts, dataAnimation);

					return(flagSuccess);
				}

				public static bool DictionaryShutDown(int indexAnimation, int indexParts, Script_SpriteStudio6_DataAnimation dataAnimation)
				{
					bool flagSuccess = true;

					flagSuccess &= StandardUncompressed.DictionaryShutDown(indexAnimation, indexParts, dataAnimation);
					flagSuccess &= StandardCPE.DictionaryShutDown(indexAnimation, indexParts, dataAnimation);
					flagSuccess &= CPE_Flyweight.DictionaryShutDown(indexAnimation, indexParts, dataAnimation);
					flagSuccess &= CPE_Interpolate.DictionaryShutDown(indexAnimation, indexParts, dataAnimation);

					return(flagSuccess);
				}

#if TEST_PERFORMANCE_SPEEDUP
				/* MEMO: Those functions is used inside the each attributes' decoding function. */
				private static int IndexSearcLimitRange(	ref int indexMinimum,
															ref int indexMaximum,
															int frame,
															int frameCache,
															int indexCache,
															int[] tableStatus,
															int countTableStatus,
															int bitMaskFrameKey
													)
				{
					/* MEMO: When previous result is cached, limit scope of the binary search (for reducing */
					/*         number of searches). And, survey the nearest key-data.                       */
					int index = indexCache;
					if(0 <= index)
					{
						if(frameCache < frame)
						{	/* Behind part */
							indexMinimum = index;
							if((indexMinimum + 1) < countTableStatus)
							{
								indexMinimum++;

								frameCache = tableStatus[indexMinimum] & bitMaskFrameKey;	/* recycle "frameCache" */
								if(frame == frameCache)
								{	/* Hit nearest key */
									/* index = */	indexMaximum = indexMinimum;

									return(indexMinimum);
								}
								if(frame < frameCache)
								{	/* Hit nearest key */
									indexMinimum = indexMaximum = index;

									return(index);
								}
							}
						}
						else
						{	/* later part */
							if(frame == frameCache)
							{	/* Hit just key */
								indexMaximum = indexMinimum = indexCache;

								return(indexMinimum);
							}

							indexMaximum = index;
							if(0 <= (indexMaximum - 1))
							{
								indexMaximum--;
								frameCache = tableStatus[indexMaximum] & bitMaskFrameKey;	/* recycle "frameCache" */
								if(frame >= frameCache)
								{	/* Hit nearest key */
									indexMinimum = indexMaximum = index;

									return(index);
								}
							}
						}
					}

					return(-1);
				}
				private static int IndexSearchTraverse(	int frame,
														int indexMinimum,
														int indexMaximum,
														int[] tableStatus,
														int countTableStatus,
														int bitMaskFrameKey
													)
				{
					int index;
					int frameKey;
					
//					while(indexMinimum != indexMaximum)
					while(indexMinimum < indexMaximum)
					{
						index = indexMinimum + indexMaximum;
						index = (index >> 1) + (index & 1);	/* (index / 2) + (index % 2) */
						frameKey = tableStatus[index] & bitMaskFrameKey;
						if(frame == frameKey)
						{
							indexMinimum = indexMaximum = index;

//							break;	/* while-Loop */
							return(indexMinimum);
						}
						else
						{
//							if((frame < frameKey) || (-1 == frameKey))
							if(frame < frameKey)
							{
								indexMaximum = index - 1;
							}
							else
							{
								indexMinimum = index;
							}
						}
					}

					return(indexMinimum);
				}
#endif
				#endregion Functions
			}
		}
	}
}
