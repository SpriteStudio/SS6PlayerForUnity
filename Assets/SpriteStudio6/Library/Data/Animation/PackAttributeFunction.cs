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
					CPE_FLYWEIGHT,	/* CPE & GoF-Flyweight */

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
							return(CapacityContainerDummy);

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
							return("Dummy");

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
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionColorBlend(ContainerColorBlend container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionColorBlend;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionColorBlend;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
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
							/* MEMO: Unsupported */
//							container.Function = StandardUncompressed.FunctionUserData;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionUserData;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
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
							/* MEMO: Unsupported */
//							container.Function = StandardUncompressed.FunctionInstance;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionInstance;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
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
							/* MEMO: Unsupported */
//							container.Function = StandardUncompressed.FunctionEffect;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionEffect;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionCoordinateFix(ContainerCoordinateFix container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionCoordinateFix;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionCoordinateFix;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionColorBlendFix(ContainerColorBlendFix container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionColorBlendFix;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionColorBlendFix;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}

				public static void BootUpFunctionUVFix(ContainerUVFix container)
				{
					switch(container.TypePack)
					{
						case KindTypePack.STANDARD_UNCOMPRESSED:
							container.Function = StandardUncompressed.FunctionUVFix;
							break;

						case KindTypePack.STANDARD_CPE:
							container.Function = StandardCPE.FunctionUVFix;
							break;

						case KindTypePack.CPE_FLYWEIGHT:
							break;

						default:
							break;
					}
				}
				#endregion Functions
			}
		}
	}
}
