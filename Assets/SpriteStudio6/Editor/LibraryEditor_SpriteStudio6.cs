/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static partial class LibraryEditor_SpriteStudio6
{
	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public const string NameAsset = "SpriteStudio6 Player for Unity";
	public const string NameDistributor = "Web Technology Corp.";
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	public static partial class Import
	{
		/* ----------------------------------------------- Functions */
		#region Functions
		public static bool Exec(	ref Setting setting,
									string nameInputFullPathSSPJ,
									string nameOutputAssetFolderBase,
									bool flagDisplayProgressBar = true
								)
		{
			const string messageLogPrefix = "Importer-Main";
			SSPJ.Information informationSSPJ = null;
			int countProgressNow = 0;
			int countProgressMax = 0;

			/* Select Project(SSPJ) */
			string nameDirectory = "";
			string nameFileBody = "";
			string nameFileExtension = "";
			nameInputFullPathSSPJ = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(nameInputFullPathSSPJ);
			if(false == System.IO.File.Exists(nameInputFullPathSSPJ))
			{	/* Not Found */
				LogError(messageLogPrefix, "File Not Found [" + nameInputFullPathSSPJ +"]");
				goto Exec_ErrorEnd;
			}
			LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out nameDirectory, out nameFileBody, out nameFileExtension, nameInputFullPathSSPJ);

			/* Read Project (SSPJ) */
			/* MEMO: "nameDirectory" must be Absolute-Path. */
			informationSSPJ = SSPJ.Parse(ref setting, nameDirectory, nameFileBody, nameFileExtension);
			if(null == informationSSPJ)
			{
				goto Exec_ErrorEnd;
			}

			/* Get ConvertFiles-Count & ConvertProgress-Count */
			int countSSCE = informationSSPJ.TableNameSSCE.Length;
			int countSSAE = informationSSPJ.TableNameSSAE.Length;
			int countSSEE = informationSSPJ.TableNameSSEE.Length;
			int countTexture = countSSCE;	/* Decretal order */
			countProgressMax += (countSSCE + countSSAE + countSSEE + 1);	/* Parse *//* +1 = Create Texture-Information */
			switch(setting.Mode)
			{
				case Setting.KindMode.SS6PU:
					{
						countProgressMax += (countSSCE + countSSAE + countSSEE);	/* Convert */
						countProgressMax += 1;	/* Create-Asset(Materials) */
						countProgressMax += (countTexture + 1 + countSSAE + countSSEE);	/* Create-Asset *//* 1==CellMap */

						if(true == setting.PreCalcualation.FlagFixMesh)
						{
							countProgressMax += countSSAE;	/* Convert-Animation Pass 2 (PreCalculate Mesh-Fix) */
						}
						countProgressMax += countSSAE;	/* Convert-Animation Pass 3 (Attributes Pack) */

						if(true == setting.PreCalcualation.FlagTrimTransparentPixelsCell)
						{
							countProgressMax += countSSCE;	/* Convert-CellMap Pass 2 (PreCalculate Trim-TransparentPixels) */
						}
					}
					break;

				case Setting.KindMode.UNITY_NATIVE:
					{
						countProgressMax += countTexture;	/* Create-Asset (Texture) */
					}
					break;

				case Setting.KindMode.BATCH_IMPORTER:
				default:
					return(false);
			}

			/* Read CellMap(SSCE) & Collect Texture-FileNames */
			for(int i=0; i<countSSCE; i++)
			{
				informationSSPJ.TableInformationSSCE[i] = SSCE.Parse(ref setting, informationSSPJ.TableNameSSCE[i], informationSSPJ);
				if(null == informationSSPJ.TableInformationSSCE[i])
				{
					goto Exec_ErrorEnd;
				}

				ProgressBarUpdate("Reading SSCEs", flagDisplayProgressBar, ref countProgressNow, countProgressMax);
			}

			/* Read Animation (SSAE) */
			for(int i=0; i<countSSAE; i++)
			{
				informationSSPJ.TableInformationSSAE[i] = SSAE.Parse(ref setting, informationSSPJ.TableNameSSAE[i], informationSSPJ);
				if(null == informationSSPJ.TableInformationSSAE[i])
				{
					goto Exec_ErrorEnd;
				}

				ProgressBarUpdate("Reading SSAEs", flagDisplayProgressBar, ref countProgressNow, countProgressMax);
			}

			/* Read Effect (SSEE) */
			for(int i=0; i<countSSEE; i++)
			{
				informationSSPJ.TableInformationSSEE[i] = SSEE.Parse(ref setting, informationSSPJ.TableNameSSEE[i], informationSSPJ);
				if(null == informationSSPJ.TableInformationSSEE[i])
				{
					goto Exec_ErrorEnd;
				}

				ProgressBarUpdate("Reading SSEEs", flagDisplayProgressBar, ref countProgressNow, countProgressMax);
			}

			/* Create Texture-Information */
			if(false == informationSSPJ.InformationCreateTexture(ref setting))
			{
				goto Exec_ErrorEnd;
			}
			ProgressBarUpdate("Create Texture Information", flagDisplayProgressBar, ref countProgressNow, countProgressMax);

			/* Delete Temporary (FileName Buffer) */
			informationSSPJ.TableNameSSCE = null;
			informationSSPJ.TableNameSSAE = null;
			informationSSPJ.TableNameSSEE = null;

			/* Create Destination Base-Folder */
			nameOutputAssetFolderBase = LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(nameOutputAssetFolderBase));
			if(false == nameOutputAssetFolderBase.EndsWith("/"))
			{
				nameOutputAssetFolderBase += "/";
			}
			if(true == setting.Basic.FlagCreateProjectFolder)
			{
				nameOutputAssetFolderBase += informationSSPJ.NameFileBody + "/";
			}

			/* Convert Datas */
			switch(setting.Mode)
			{
				case Setting.KindMode.SS6PU:
					if(false == ConvertSS6PU(	ref setting,
												ref countProgressNow,
												countProgressMax,
												flagDisplayProgressBar,
												informationSSPJ,
												nameOutputAssetFolderBase
											)
						)
					{
						goto Exec_ErrorEnd;
					}
					break;

				case Setting.KindMode.UNITY_NATIVE:
					if(false == ConvertUnityNative(	ref setting,
													ref countProgressNow,
													countProgressMax,
													flagDisplayProgressBar,
													informationSSPJ,
													nameOutputAssetFolderBase
												)
						)
					{
						goto Exec_ErrorEnd;
					}
					break;

				default:
					LogError(messageLogPrefix, "Invalid Convert-Mode at [" + nameInputFullPathSSPJ + "]");
					goto Exec_ErrorEnd;
			}

			countProgressNow = -1;
			ProgressBarUpdate("", flagDisplayProgressBar, ref countProgressNow, -1);
			return(true);

		Exec_ErrorEnd:;
			ProgressBarUpdate("", flagDisplayProgressBar, ref countProgressNow, -1);
			if(null != informationSSPJ)
			{
				informationSSPJ.CleanUp();
			}
			return(false);
		}
		private static bool ConvertSS6PU(	ref Setting setting,
											ref int countProgressNow,
											int countProgressMax,
											bool flagDisplayProgressBar,
											SSPJ.Information informationSSPJ,
											string nameOutputAssetFolderBase
										)
		{
			const string messageLogPrefix = "Importer-Convert(SS6PU)";
			string nameOutputAssetFolder = "";
			string nameOutputAssetBody = "";
			string nameOutputAssetExtention = "";
			bool flagCreateAsset = true;

			/* Decide Asset Names & Check Assets existing */
			informationSSPJ.AssetNameDecideSS6PU(ref setting, nameOutputAssetFolderBase);

			/* Get Datas' count */
			int countTexture = informationSSPJ.TableInformationTexture.Length;
			int countSSCE = informationSSPJ.TableInformationSSCE.Length;
			int countSSAE = informationSSPJ.TableInformationSSAE.Length;
			int countSSEE = informationSSPJ.TableInformationSSEE.Length;

			/* Create Asset: Texture */
			/* MEMO: Create Texture-Assets before CellMap for "Trim Transparent-Pixel". */
			if(0 < countTexture)
			{
				/* Copy Texture files */
				SSCE.Information.Texture informationTexture = null;
				for(int i=0; i<countTexture; i++)
				{
					flagCreateAsset = true;
					informationTexture = informationSSPJ.TableInformationTexture[i];

					/* Check Overwrite */
					/* MEMO: Texture always has only 1 prefab. */
					if(null == informationTexture.PrefabTexture.TableData[0])
					{	/* New */
						/* Create Output Asset-Folder */
						LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																			informationTexture.PrefabTexture.TableName[0]
																		);
						if(true == string.IsNullOrEmpty( LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
						{
							LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
							goto ConvertSS6PU_ErrorEnd;
						}
					}
					else
					{	/* Exist */
						if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagTexture,
																													informationTexture.PrefabTexture.TableName[0],
																													"Texture"
																												)
							)
						{	/* Not overwrite */
							countProgressNow++;	/* Pretend to update */
							flagCreateAsset = false;
						}
					}

					/* Create-Asset */
					if(true == flagCreateAsset)
					{
						if(false == informationTexture.AssetCreateTexture(ref setting, informationSSPJ))
						{
							goto ConvertSS6PU_ErrorEnd;
						}
						ProgressBarUpdate("Copy Textures", flagDisplayProgressBar, ref countProgressNow, countProgressMax);
					}
				}
			}
			countProgressNow += (countSSEE - countTexture);	/* The number of textures and SSEEs do not necessarily match. When the number is different, SSEEs are more. */

			/* Create Asset: Material (Animation) */
			if((0 < countTexture) && (0 < countSSAE))
			{
				/* Create Materials */
				SSCE.Information.Texture informationTexture = null;
				for(int i=0; i<countTexture; i++)
				{
					informationTexture = informationSSPJ.TableInformationTexture[i];
					for(int j=0; j<(int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR; j++)
					{
						flagCreateAsset = true;
						/* Check Overwrite */
						/* MEMO: Texture always has only 1 prefab. */
						if(null == informationTexture.MaterialAnimation.TableData[j])
						{	/* New */
							/* Create Output Asset-Folder */
							LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																				informationTexture.MaterialAnimation.TableName[j]
																			);
							if(true == string.IsNullOrEmpty( LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
							{
								LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
								goto ConvertSS6PU_ErrorEnd;
							}
						}
						else
						{	/* Exist */
							if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagMaterialAnimation,
																														informationTexture.MaterialAnimation.TableName[j],
																														"Material Animation"
																													)
								)
							{	/* Not overwrite */
								flagCreateAsset = false;
							}
						}

						/* Create-Asset */
						if(true == flagCreateAsset)
						{
							if(false == informationTexture.AssetCreateMaterialAnimationSS6PU(ref setting, informationSSPJ, (Library_SpriteStudio6.KindOperationBlend)j))
							{
								goto ConvertSS6PU_ErrorEnd;
							}
						}
					}
				}
			}

			/* Create Asset: Material (Effect) */
			if((0 < countTexture) && (0 < countSSEE))
			{
				/* Create Materials */
				SSCE.Information.Texture informationTexture = null;
				for(int i=0; i<countTexture; i++)
				{
					informationTexture = informationSSPJ.TableInformationTexture[i];
					for(int j=0; j<(int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR; j++)
					{
						flagCreateAsset = true;

						/* Check Overwrite */
						/* MEMO: Texture always has only 1 prefab. */
						if(null == informationTexture.MaterialEffect.TableData[j])
						{	/* New */
							/* Create Output Asset-Folder */
							LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																				informationTexture.MaterialEffect.TableName[j]
																			);
							if(true == string.IsNullOrEmpty( LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
							{
								LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
								goto ConvertSS6PU_ErrorEnd;
							}
						}
						else
						{	/* Exist */
							if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagMaterialEffect,
																														informationTexture.MaterialEffect.TableName[j],
																														"Material Effect"
																													)
								)
							{	/* Not overwrite */
								flagCreateAsset = false;
							}
						}

						/* Create-Asset */
						if(true == flagCreateAsset)
						{
							if(false == informationTexture.AssetCreateMaterialEffectSS6PU(ref setting, informationSSPJ, (Library_SpriteStudio6.KindOperationBlendEffect)j))
							{
								goto ConvertSS6PU_ErrorEnd;
							}
						}
					}
				}
			}
			ProgressBarUpdate("Create Materials", flagDisplayProgressBar, ref countProgressNow, countProgressMax);

			/* Create Asset: CellMap */
			/* MEMO: Process after creating all Texture-Assets. */
			if(0 < countSSEE)
			{
				/* MEMO: Since informations of SSCE files are grouped in 1 CellMap data-asset, always only 1 CellMap data-asset for a SSPJ. */
				SSCE.Information informationSSCE = null;
				for(int i=0; i<countSSCE; i++)
				{
					/* MEMO: Be sure to "Convert" even when not create CellMap data-assets. Datas may be used at converting SSAE. */
					informationSSCE = informationSSPJ.TableInformationSSCE[i];

					/* Convert Pass1 (Normal) */
					if(false == informationSSCE.ConvertSS6PU(ref setting, informationSSPJ))
					{
						goto ConvertSS6PU_ErrorEnd;
					}
					ProgressBarUpdate("Convert SSCEs Pass-1 (" + i + "/" + countSSCE + ")", flagDisplayProgressBar, ref countProgressNow, countProgressMax);

					/* Convert Pass2 (Trim Transparent-Pixel) */
					if(true == setting.PreCalcualation.FlagTrimTransparentPixelsCell)
					{
						if(false == informationSSCE.ConvertTrimPixelSS6PU(ref setting, informationSSPJ))
						{
							goto ConvertSS6PU_ErrorEnd;
						}
						ProgressBarUpdate("Convert SSCEs Pass-2 (" + i + "/" + countSSCE + ")", flagDisplayProgressBar, ref countProgressNow, countProgressMax);
					}
				}

				/* Check Overwrite */
				flagCreateAsset = true;
				if(null == informationSSPJ.PrefabCellMap.TableData[0])
				{	/* New */
					/* Create Output Asset-Folder */
					LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																		informationSSPJ.PrefabCellMap.TableName[0]
																	);
					if(true == string.IsNullOrEmpty( LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
					{
						LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
						goto ConvertSS6PU_ErrorEnd;
					}
				}
				else
				{	/* Exist */
					if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataCellMap,
																												informationSSPJ.PrefabCellMap.TableName[0],
																												"Data CellMap"
																											)
						)
					{	/* Not overwrite */
						countProgressNow++;	/* Pretend to update */
						flagCreateAsset = false;
					}
				}

				/* Create-Asset */
				if(true == flagCreateAsset)
				{
					if(false == informationSSPJ.AssetCreateCellMapSS6PU(ref setting))
					{
						goto ConvertSS6PU_ErrorEnd;
					}
					ProgressBarUpdate("Create Asset \"Data-CellMap\"", flagDisplayProgressBar, ref countProgressNow, countProgressMax);
				}
			}

#if false
			/* Create-Asset: Effect */
			for(int i=0; i<countSSEE; i++)
			{
				countProgressNow++;
			}

			/* Get Convert-Order SSAE */

			/* Create-Asset: Animation */
			for(int i=0; i<countSSEE; i++)
			{
				/* Convert Pass1 (Create Unpack) */
				countProgressNow++;

				/* Convert Pass2 (Fix) */
				if(true == setting.PreCalcualation.FlagFixMesh)
				{
					countProgressNow++;
				}

				/* Convert Pass3 (Pack) */
				countProgressNow++;

				/* Create-Asset */
				countProgressNow++;
			}
#endif

			return(true);

		ConvertSS6PU_ErrorEnd:;
			return(false);
		}
		private static bool ConvertUnityNative(	ref Setting setting,
												ref int countProgressNow,
												int countProgressMax,
												bool flagDisplayProgressBar,
												SSPJ.Information informationSSPJ,
												string nameOutputAssetFolderBase
											)
		{
			/* Animation -> Animator/AnimationClip */

			/* Effect -> Particle */


			return(false);
		}

		private static void LogError(string messagePrefix, string message)
		{
			LibraryEditor_SpriteStudio6.Utility.Log.Error(messagePrefix + ": " + message);
		}

		private static void LogWarning(string messagePrefix, string message)
		{
			LibraryEditor_SpriteStudio6.Utility.Log.Warning(messagePrefix + ": " + message);
		}

		public static void ProgressBarUpdate(string nameTask, bool flagSwitch, ref int step, int stepFull)
		{
			LibraryEditor_SpriteStudio6.Utility.Miscellaneous.ProgressBarUpdate(	LibraryEditor_SpriteStudio6.NameAsset + " Data Import",
																					nameTask,
																					flagSwitch,
																					step,
																					stepFull
																				);
			step++;
		}
		#endregion Functions

		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		public enum KindFile
		{
			NON = -1,	/* Through */
			TEXTURE = 0,
			SSPJ,
			SSCE,
			SSAE,
			SSEE,
		}
		#endregion Enums & Constants

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public struct Assets<_Type>
			where _Type : class
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public string[] TableName;
			public _Type[] TableData;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public void CleanUp()
			{
				TableName = null;
				TableData = null;
			}

			public void BootUp(int count)
			{
				TableName = new string[count];
				TableData = new _Type[count];
				for(int i=0; i<count; i++)
				{
					TableName[i] = null;
					TableData[i] = null;
				}
			}
			#endregion Functions
		}

		public static partial class SSPJ
		{
			/* Part: SpriteStudio6/Editor/Import/SSPJE.cs */
		}

		public static partial class SSCE
		{
			/* Part: SpriteStudio6/Editor/Import/SSCE.cs */
		}

		public static partial class SSAE
		{
			/* Part: SpriteStudio6/Editor/Import/SSAE.cs */
		}

		public static partial class SSEE
		{
			/* Part: SpriteStudio6/Editor/Import/SSEE.cs */
		}

		public static partial class Batch
		{
			/* Part: SpriteStudio6/Editor/Import/Batch.cs */
		}

		/* Part: SpriteStudio6/Editor/Import/Setting.cs */
		#endregion Classes, Structs & Interfaces
	}

	public static partial class Utility
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public static class Interpolation
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
				float PointNow = Linear(start.x, end.x, point);
				float PointTemp;

				float AreaNow = 0.5f;
				float RangeNow = 0.5f;

				float Base;
				float BasePow2;
				float BasePow3;
				float AreaNowPow2;
				for(int i=0; i<8; i++)
				{
					Base = 1.0f - AreaNow;
					BasePow2 = Base * Base;
					BasePow3 = BasePow2 * Base;
					AreaNowPow2 = AreaNow * AreaNow;
					PointTemp = (BasePow3 * start.x)
								+ (3.0f * BasePow2 * AreaNow * (vectorStart.x + start.x))
								+ (3.0f * Base * AreaNowPow2 * (vectorEnd.x + end.x))
								+ (AreaNow * AreaNowPow2 * end.x);
					RangeNow *= 0.5f;
					AreaNow += ((PointTemp > PointNow) ? (-RangeNow) : (RangeNow));
				}

				AreaNowPow2 = AreaNow * AreaNow;
				Base = 1.0f - AreaNow;
				BasePow2 = Base * Base;
				BasePow3 = BasePow2 * Base;
				return(	(BasePow3 * start.y)
						+ (3.0f * BasePow2 * AreaNow * (vectorStart.y + start.y))
						+ (3.0f * Base * AreaNowPow2 * (vectorEnd.y + end.y))
						+ (AreaNow * AreaNowPow2 * end.y)
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

			public static float ValueGetFloat(	Library_SpriteStudio6.Data.Animation.Attribute.KindInterpolation interpolation,
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

				switch(interpolation)
				{
					case Library_SpriteStudio6.Data.Animation.Attribute.KindInterpolation.NON:
						return(valueStart);

					case Library_SpriteStudio6.Data.Animation.Attribute.KindInterpolation.LINEAR:
						return(Linear(valueStart, valueEnd, frameNormalized));

					case Library_SpriteStudio6.Data.Animation.Attribute.KindInterpolation.HERMITE:
						return(Hermite(valueStart, valueEnd, frameNormalized, curveValueStart, curveValueEnd));

					case Library_SpriteStudio6.Data.Animation.Attribute.KindInterpolation.BEZIER:
						{
							Vector2 start = new Vector2((float)frameStart, valueStart);
							Vector2 vectorStart = new Vector2(curveFrameStart, curveValueStart);
							Vector2 end = new Vector2((float)frameEnd, valueEnd);
							Vector2 vectorEnd = new Vector2(curveFrameEnd, curveValueEnd);
							return(Interpolation.Bezier(ref start, ref end, frameNormalized, ref vectorStart, ref vectorEnd));
						}
//						break;	/* Redundant */

					case Library_SpriteStudio6.Data.Animation.Attribute.KindInterpolation.ACCELERATE:
						return(Accelerate(valueStart, valueEnd, frameNormalized));

					case Library_SpriteStudio6.Data.Animation.Attribute.KindInterpolation.DECELERATE:
						return(Decelerate(valueStart, valueEnd, frameNormalized));

					default:
						break;
				}
				return(valueStart);	/* Error */
			}
			#endregion Functions
		}

		public static class File
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static bool NamesGetFileDialog(out string nameDirectory, out string nameFileBody, out string nameFileExtension, string textTitleDialog, string filterExtension)
			{
				/* Get Previous Folder-Name */
				string directoryPrevious = "";
				LibraryEditor_SpriteStudio6.Import.Setting.FolderLoadPrevious(out directoryPrevious);

				/* Choose Import-File */
				string fileNameFullPath = EditorUtility.OpenFilePanel(textTitleDialog, directoryPrevious, filterExtension);
				if(0 == fileNameFullPath.Length)
				{	/* Cancelled */
					nameDirectory = "";
					nameFileBody = "";
					nameFileExtension = "";

					return(false);
				}
				bool rv = PathSplit(out nameDirectory, out nameFileBody, out nameFileExtension, fileNameFullPath);

				/* Save Folder-Name */
				LibraryEditor_SpriteStudio6.Import.Setting.FolderSavePrevious(nameDirectory);

				return(rv);
			}

			internal static bool PermissionGetConfirmDialogueOverwrite(ref bool flagSwitchSetting, string nameAsset, string nameTypeAsset)
			{
				if(false == flagSwitchSetting)
				{	/* No-Confirm */
					return(true);
				}

				bool rv = false;
				int KindResult = EditorUtility.DisplayDialogComplex(	"Asset already exists.",
																		"Do you want to overwrite?\n" + nameAsset,
																		"Yes",
																		"Yes, all \"" + nameTypeAsset +"\"s",
																		"No"
																	);
				switch(KindResult)
				{
					case 0:	/* Yes */
						rv = true;
						break;

					case 1:	/* All */
						flagSwitchSetting = false;
						rv = true;
						break;

					case 2:	/* No */
						rv = false;
						break;

				}
				return(rv);
			}

			public static string AssetFolderCreate(string namePath)
			{
				if(true == string.IsNullOrEmpty(namePath))
				{
					return(null);
				}
#if false
				/* MEMO: when use "AssetDataBase.CreateFolder" to create folders recursively, processing may be delayed. */
				/* Create Folder Recursive */
				string namePathParent = "Assets";
				string namePathChild = "";
				string[] namePathSplit = namePath.Split(TextSplitFolder);
				int count = namePathSplit.Length;
				if(0 >= count)
				{
					return(null);
				}

				int indexTop = (NamePathRootAsset.ToLower() == namePathSplit[0].ToLower()) ? 1 : 0;
				for(int i=indexTop; i<count; i++)
				{
					namePathChild = namePathSplit[i];
					if(false == string.IsNullOrEmpty(namePathChild))
					{
						if(false == AssetDatabase.IsValidFolder(namePathParent + "/" + namePathChild))
						{
							AssetDatabase.CreateFolder(namePathParent, namePathChild);
						}
						namePathParent += "/" + namePathChild;
					}
				}

				namePathParent += "/";
				return(namePathParent);
#else
				/* MEMO: Originally, way that should not take. Use "System.IO.Directory.CreateDirectory" to create folders. */
				string[] namePathSplit = namePath.Split(TextSplitFolder);
				int count = namePathSplit.Length;
				if(0 >= count)
				{
					return(null);
				}

				/* Reconstruct path */
				int indexTop = (NamePathRootAsset.ToLower() == namePathSplit[0].ToLower()) ? 1 : 0;
				string namePathAsset = string.Copy(NamePathRootAsset);
				string namePathNative = string.Copy(NamePathRootNative);
				string namePathChild = null;
				for(int i=indexTop; i<count; i++)
				{
					namePathChild = namePathSplit[i];
					if(false == string.IsNullOrEmpty(namePathChild))
					{
						namePathNative += "/" + namePathChild;
						namePathAsset += "/" + namePathChild;
					}
				}

				/* Create folder, if not exist. */
				if(false == System.IO.Directory.Exists(namePathNative))
				{
					System.IO.Directory.CreateDirectory(namePathNative);
				}

				namePathAsset += "/";
				return(namePathAsset);
#endif
			}

			public static string AssetPathGetSelected(string namePath=null)
			{
				string namePathAsset = "";
				if(true == string.IsNullOrEmpty(namePath))
				{	/* Now Selected Path in "Project" */
					Object objectNow = Selection.activeObject;
					if(null == objectNow)
					{	/* No Selected *//* Error */
						namePathAsset = null;
					}
					else
					{	/* Selected */
						namePathAsset = AssetDatabase.GetAssetPath(objectNow);
					}
				}
				else
				{	/* Specified */
					namePathAsset = System.String.Copy(namePath);
				}

				return(namePathAsset);
			}

			public static bool FileCopyToAsset(string nameAsset, string nameOriginalFileName, bool flagOverCopy)
			{
				System.IO.File.Copy(nameOriginalFileName, nameAsset, flagOverCopy);
				return(true);
			}

			public static bool PathSplit(	out string nameDirectory,
											out string nameFileBody,
											out string nameFileExtention,
											string namePath
										)
			{
				if(true == string.IsNullOrEmpty(namePath))
				{
					nameDirectory = "";
					nameFileBody = "";
					nameFileExtention = "";
					return(false);
				}

				string namePathNormalized = PathNormalize(namePath);
				nameDirectory = System.IO.Path.GetDirectoryName(namePathNormalized) + "/";
				nameFileBody = System.IO.Path.GetFileNameWithoutExtension(namePathNormalized);
				nameFileExtention = System.IO.Path.GetExtension(namePathNormalized);

				return(true);
			}

			public static string PathNormalize(string namePath)
			{
				string namePathNew = namePath.Replace("\\", "/");	/* "\" -> "/" */
				return(namePathNew);
			}

			public static string PathGetAbsolute(string namePath, string nameBase)
			{
				string nameCurrent = System.Environment.CurrentDirectory;
				System.Environment.CurrentDirectory = nameBase;

				string rv = System.IO.Path.GetFullPath(namePath);
				rv = PathNormalize(rv);

				System.Environment.CurrentDirectory = nameCurrent;
				return(rv);
			}

			public static string PathGetAssetNative(string namePathAsset)
			{
				string namePathNative = string.Copy(NamePathRootNative);
				if(false == string.IsNullOrEmpty(namePathAsset))
				{
					namePathNative += "/" + namePathAsset.Substring(NamePathRootAsset.Length + 1);
					namePathNative = PathNormalize(namePathNative);
				}
				return(namePathNative);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			private readonly static char[] TextSplitFolder = 
			{
				'/',
				'\\',
			};

			private readonly static string NamePathRootNative = Application.dataPath;
			private const string NamePathRootAsset = "Assets";
			#endregion Enums & Constants
		}

		public static partial class Prefs
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static void StringSave(string prefsKey, string text)
			{
				string text64 = System.Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(text));
				EditorPrefs.SetString(prefsKey, text64);
			}

			public static string StringLoad(string prefsKey, string textDefault)
			{
				string textDefault64 = System.Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(textDefault));
				string text64 = EditorPrefs.GetString(prefsKey, textDefault64);
				return(System.Text.UTF8Encoding.UTF8.GetString(System.Convert.FromBase64String(text64)));
			}
			#endregion Functions
		}

		public static partial class Text
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static bool ValueGetBool<_Type>(_Type source)
			{
				return((0 != ValueGetInt(source)) ? true : false);
			}

			public static byte ValueGetByte<_Type>(_Type source)
			{
				return(System.Convert.ToByte(source));
			}

			public static int ValueGetInt<_Type>(_Type source)
			{
				return(System.Convert.ToInt32(source));
			}

			public static uint ValueGetUInt<_Type>(_Type source)
			{
				return(System.Convert.ToUInt32(source));
			}

			public static float ValueGetFloat<_Type>(_Type source)
			{
				return(System.Convert.ToSingle(source));
			}

			internal static double ValueGetDouble<_Type>(_Type source)
			{
				return(System.Convert.ToDouble(source));
			}

			public static int HexToInt(string text)
			{
				return(System.Convert.ToInt32(text, 16));
			}

			public static uint HexToUInt(string text)
			{
				return(System.Convert.ToUInt32(text, 16));
			}

			public static bool TextToBool(string text)
			{
				bool rv = false;
				try
				{
					rv = System.Convert.ToBoolean(text);
				}
				catch(System.FormatException)
				{
					rv = (0 == System.Convert.ToInt32(text));
				}
				return(rv);
			}

			public static int TextToVersion(string text)
			{	/* MEMO: Text = "Major:1"."Minor:2"."Revison:2" */
				string[] item = text.Split('.');
				if (3 != item.Length)
				{
					return(-1);
				}

				int versionMajor = HexToInt(item[0]);
				int versionMinor = HexToInt(item[1]);
				int revision = HexToInt(item[2]);
				return((versionMajor << 16) | (versionMinor << 8) | revision);
			}

			public static string DataNameGetFromPath(	string namePath,
														bool flagRuleOld = false
													)
			{
				string rv = "";
				if(true == string.IsNullOrEmpty(namePath))
				{
					return("");
				}

				string nameNewDirectory = "";
				string nameNewFileBody = "";
				string nameNewFileExtention = "";
				LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out nameNewDirectory, out nameNewFileBody, out nameNewFileExtention, namePath);
				if((true == string.IsNullOrEmpty(nameNewDirectory)) || (true == flagRuleOld))
				{
					rv = nameNewFileBody;
				}
				else
				{
					rv = nameNewDirectory + "/" + nameNewFileBody;
				}
				rv = rv.Replace("\\", "/");	/* "\" -> "/" */

				rv = rv.Replace("../", "_");	/* "../" -> "_" */
				rv = rv.Replace("/", "_");	/* "/" -> "_" */
				return(rv);
			}
			#endregion Functions
		}

		public static partial class XML
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static System.Xml.XmlNodeList ListGetNode(System.Xml.XmlNode node, string namePath, System.Xml.XmlNamespaceManager manager)
			{
				return(node.SelectNodes(namePath, manager));
			}

			public static System.Xml.XmlNode NodeGet(System.Xml.XmlNode node, string namePath, System.Xml.XmlNamespaceManager manager)
			{
				return(node.SelectSingleNode(namePath, manager));
			}

			public static string TextGetNode(System.Xml.XmlNode node, string namePath, System.Xml.XmlNamespaceManager manager)
			{
				System.Xml.XmlNode nodeNow = NodeGet(node, namePath, manager);
				return((null != nodeNow) ? nodeNow.InnerText : null);
			}

			public static int VersionGet(System.Xml.XmlNode nodeRoot, string nameTag, int errorValue, bool flagMaskRevision)
			{
				System.Xml.XmlAttributeCollection attributeNodeRoot = nodeRoot.Attributes;
				if(nameTag != nodeRoot.Name)
				{
					return(errorValue);
				}

				System.Xml.XmlNode NodeVersion = attributeNodeRoot["version"];
				string versionText = NodeVersion.Value;
				int version = LibraryEditor_SpriteStudio6.Utility.Text.TextToVersion(versionText);
				if(-1 == version)
				{
					return(errorValue);
				}

				if(true == flagMaskRevision)
				{
					version &= ~0x000000ff;
				}
				return(version);
			}
			#endregion Functions
		}

		public static partial class ExternalText
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static KindType TypeGetLine(out string textValid, string text)
			{
				textValid = TextTrim(text);
				if(true == string.IsNullOrEmpty(textValid))
				{	/* Space */
					goto TypeGetLine_EndIgnore;
				}

				char prefixLine = textValid[0];
				switch(prefixLine)
				{
					case PrefixChangeCommand:
						return(KindType.COMMAND);

					case PrefixRemarks:
						goto TypeGetLine_EndIgnore;

					default:
						break;
				}

				return(KindType.NORMAL);

			TypeGetLine_EndIgnore:;
				textValid = "";
				return(KindType.IGNORE);
			}

			public static string LineEncodeIgnore(string text)
			{
				return(PrefixRemarks + " " + text);
			}

			public static string LineEncodeCommand(params string[] textArgument)
			{
				string text = "";
				int count = textArgument.Length;
				bool flagSeparator = false;
				for(int i=0; i<count; i++)
				{
					if(0 == i)
					{
						text += PrefixChangeCommand;
					}
					text += " " + textArgument[i];
					if((count - 1) > i)
					{
						text += " ";
						text += (false == flagSeparator) ? SeparatorCommand[(int)KindSeparator.COMMAND] : SeparatorCommand[(int)KindSeparator.ARGUMENT];

						flagSeparator = true;
					}
				}
				return(text);
			}

			public static string[] LineDecodeCommand(string textLine)
			{
				string[] textArgument = textLine.Split(SeparatorCommand);
				if(null != textArgument)
				{
					int count = textArgument.Length;
					for(int i=0; i<count; i++)
					{
						textArgument[i] = textArgument[i].Trim(PrefixChangeCommand);
						textArgument[i] = TextTrim(textArgument[i]);
					}
				}
				return(textArgument);
			}

			public static string TextTrim(string text)
			{
				return(text.Trim(IgnoreText));
			}

			public static string BoolEncode(bool value)
			{
				return((true == value) ? ArgumentBoolTrue : ArgumentBoolFalse);
			}

			public static bool BoolDecode(string text)
			{
				switch(text)
				{
					case ArgumentBoolTrue:
						return(true);

					case ArgumentBoolFalse:
						return(false);

					default:
						break;
				}
				return(false);
			}

			public static string FloatEncode(float value)
			{
				return(value.ToString());
			}

			public static float FloatDecode(string text)
			{
				float value;
				if(false == float.TryParse(text, out value))
				{
					value = 0.0f;
				}
				return(value);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public enum KindType
			{
				NORMAL = 0,
				IGNORE,
				COMMAND,
			}
			public enum KindSeparator
			{
				COMMAND = 0,
				ARGUMENT,
			}

			private const char PrefixChangeCommand = '?';
			private const char PrefixRemarks = '*';
			private readonly static char[] SeparatorCommand =
			{	/* [KindSeparator] */
				'>',	/* between Command and 1st-argument */
				',',	/* between arguments */
			};
			private readonly static char[] IgnoreText =
			{
				' ',
				'\t',
			};

			private const string ArgumentBoolTrue = "true";
			private const string ArgumentBoolFalse = "false";
			#endregion Enums & Constants
		}

		public static partial class Log
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public static System.IO.StreamWriter StreamExternal = null;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public static void Error(string message, bool flagIndent=true)
			{
				string text = "SS6PU Error: " + message;
				Debug.LogError(text);
				if(null != StreamExternal)
				{
					if(true == flagIndent)
					{
						text = "\t" + text;
					}
					StreamExternal.WriteLine(text);
				}
			}

			public static void Warning(string message, bool flagIndent=true)
			{
				string text = "SS6PU Warning: " + message;
				Debug.LogWarning(text);
				if(null != StreamExternal)
				{
					if(true == flagIndent)
					{
						text = "\t" + text;
					}
					StreamExternal.WriteLine(text);
				}
			}

			public static void Message(string message, bool flagIndent=true)
			{
				string text = "SS6PU-Message: " + message;
				Debug.Log(text);
				if(null != StreamExternal)
				{
					if(true == flagIndent)
					{
						text = "\t" + text;
					}
					StreamExternal.WriteLine(text);
				}
			}
			#endregion Functions
		}

		public static class Miscellaneous
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static void ProgressBarUpdate(string title, string nameTask, bool flagSwitch, int step, int stepFull)
			{
				if(false == flagSwitch)
				{
					return;
				}

				if((-1 == step) || (-1 == stepFull))
				{
					EditorUtility.ClearProgressBar();
					return;
				}

				EditorUtility.DisplayProgressBar(title, nameTask, ((float)step / (float)stepFull));
			}
			#endregion Functions
		}
		#endregion Classes, Structs & Interfaces
	}
	#endregion Classes, Structs & Interfaces
}
