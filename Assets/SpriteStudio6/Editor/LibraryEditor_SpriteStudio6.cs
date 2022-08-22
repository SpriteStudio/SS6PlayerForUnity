/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static partial class LibraryEditor_SpriteStudio6
{
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
			const string messageLogPrefix = "Main";
			SSPJ.Information informationSSPJ = null;
			int countProgressNow = 0;
			int countProgressMax = 0;

			/* Select Project(SSPJ) */
			string nameDirectory = "";
			string nameFileBody = "";
			string nameFileExtension = "";
			nameInputFullPathSSPJ = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(nameInputFullPathSSPJ);
			LibraryEditor_SpriteStudio6.Utility.Log.Message("Importing Start [" + nameInputFullPathSSPJ + "]", true, false);	/* External-File only, no indent */

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

			/* Set "Signal-Setting" information */
			informationSSPJ.InformationSignalSetting = new SignalSettings.Information();
			if(null == informationSSPJ.InformationSignalSetting)
			{
				goto Exec_ErrorEnd;
			}
			informationSSPJ.InformationSignalSetting.CleanUp();

			/* Get ConvertFiles-Count & ConvertProgress-Count */
			int countSSCE = informationSSPJ.TableNameSSCE.Length;
			int countSSAE = informationSSPJ.TableNameSSAE.Length;
			int countSSEE = informationSSPJ.TableNameSSEE.Length;
			int countSSQE = informationSSPJ.TableNameSSQE.Length;
			int countSSPJ = 1;	/* Force */
			int countTexture = countSSCE;
			countProgressMax += (	countSSCE
									+ countSSAE
									+ countSSEE
									+ countSSQE
//									+ countSSPJ
									+ 1				/* Create Texture-Information */
								);	/* Parse */
			switch(setting.Mode)
			{
				case Setting.KindMode.SS6PU:
					{
						countProgressMax += (	countSSCE
												+ countSSAE
												+ countSSEE
												+ countSSQE
												+ countSSPJ
											);	/* Convert */
						countProgressMax += (	countTexture
												+ countSSAE
												+ countSSEE
												+ countSSQE
												+ countSSPJ
											);	/* Create-Asset (Data) */
						countProgressMax += (countSSAE + countSSEE);	/* Create-Asset (Prefab) */

						if(true == setting.PreCalcualation.FlagTrimTransparentPixelsCell)
						{
							countProgressMax += countSSCE;	/* Convert-CellMap Pass 2 (PreCalculate Trim-TransparentPixels) */
						}
					}
					break;

				case Setting.KindMode.UNITY_NATIVE:
					{
						countProgressMax += countSSCE;	/* Convert (CellMap) */
						countProgressMax += countTexture;	/* Create-Asset (Texture) */
						countProgressMax += countSSAE;	/* Convert & Create-Asset (Animation) */

						if(true == setting.PreCalcualation.FlagTrimTransparentPixelsCell)
						{
							countProgressMax += countSSCE;	/* Convert-CellMap Pass 2 (PreCalculate Trim-TransparentPixels) */
						}
					}
					break;

				case Setting.KindMode.UNITY_UI:
					{
						countProgressMax += countSSCE;	/* Convert (CellMap) */
						countProgressMax += countTexture;	/* Create-Asset (Texture) */
						countProgressMax += countSSAE;	/* Convert & Create-Asset (Animation) */

						if(true == setting.PreCalcualation.FlagTrimTransparentPixelsCell)
						{
							countProgressMax += countSSCE;	/* Convert-CellMap Pass 2 (PreCalculate Trim-TransparentPixels) */
						}
					}
					break;

				case Setting.KindMode.BATCH_IMPORTER:
				default:
					return(false);
			}

			/* Read CellMap(SSCE) & Collect Texture-FileNames */
			for(int i=0; i<countSSCE; i++)
			{
				ProgressBarUpdate(	"Reading SSCEs (" + (i + 1).ToString() + "/" + countSSCE.ToString() + ")",
									flagDisplayProgressBar, ref countProgressNow, countProgressMax
								);

				informationSSPJ.TableInformationSSCE[i] = SSCE.Parse(ref setting, informationSSPJ.TableNameSSCE[i], informationSSPJ);
				if(null == informationSSPJ.TableInformationSSCE[i])
				{
					goto Exec_ErrorEnd;
				}
			}

			/* Read Animation (SSAE) */
			for(int i=0; i<countSSAE; i++)
			{
				ProgressBarUpdate(	"Reading SSAEs (" + (i + 1).ToString() + "/" + countSSAE.ToString() + ")",
									flagDisplayProgressBar, ref countProgressNow, countProgressMax
								);

				informationSSPJ.TableInformationSSAE[i] = SSAE.Parse(ref setting, informationSSPJ.TableNameSSAE[i], informationSSPJ);
				if(null == informationSSPJ.TableInformationSSAE[i])
				{
					goto Exec_ErrorEnd;
				}
			}

			/* Read Effect (SSEE) */
			for(int i=0; i<countSSEE; i++)
			{
				ProgressBarUpdate(	"Reading SSEEs (" + (i + 1).ToString() + "/" + countSSEE.ToString() + ")",
									flagDisplayProgressBar, ref countProgressNow, countProgressMax
								);

				informationSSPJ.TableInformationSSEE[i] = SSEE.Parse(ref setting, informationSSPJ.TableNameSSEE[i], informationSSPJ);
				if(null == informationSSPJ.TableInformationSSEE[i])
				{
					goto Exec_ErrorEnd;
				}
			}

			/* Read Sequence (SSQE) */
			for(int i=0; i<countSSQE; i++)
			{
				ProgressBarUpdate(	"Reading SSQEs (" + (i + 1).ToString() + "/" + countSSQE.ToString() + ")",
									flagDisplayProgressBar, ref countProgressNow, countProgressMax
								);

				informationSSPJ.TableInformationSSQE[i] = SSQE.Parse(ref setting, informationSSPJ.TableNameSSQE[i], informationSSPJ);
				if(null == informationSSPJ.TableInformationSSQE[i])
				{
					goto Exec_ErrorEnd;
				}
			}

			/* Create Texture-Information */
			ProgressBarUpdate(	"Create Texture Information",
								flagDisplayProgressBar, ref countProgressNow, countProgressMax
							);
			if(false == informationSSPJ.InformationCreateTexture(ref setting))
			{
				goto Exec_ErrorEnd;
			}

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

			/* Get Convert-Order SSAE */
			informationSSPJ.QueueConvertSSAE = informationSSPJ.QueueGetConvertSSAE(ref setting);
			if(null == informationSSPJ.QueueConvertSSAE)
			{
				goto Exec_ErrorEnd;
			}

			/* Garbage-Collection */
			LibraryEditor_SpriteStudio6.Utility.Miscellaneous.GarbageCollect();

			/* Convert & Create Assets */
			switch(setting.Mode)
			{
				case Setting.KindMode.SS6PU:
					if(false == ExecSS6PU(	ref setting,
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
					if(false == ExecUnityNative(	ref setting,
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

				case Setting.KindMode.UNITY_UI:
					if(false == ExecUnityUI(	ref setting,
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

			LibraryEditor_SpriteStudio6.Utility.Log.Message("Success", true, false);	/* External-File only */

			/* Garbage-Collection */
			LibraryEditor_SpriteStudio6.Utility.Miscellaneous.GarbageCollect();

			return(true);

		Exec_ErrorEnd:;
			ProgressBarUpdate("", flagDisplayProgressBar, ref countProgressNow, -1);
			if(null != informationSSPJ)
			{
				informationSSPJ.CleanUp();
			}

			LibraryEditor_SpriteStudio6.Utility.Log.Message("Failure", true, false);	/* External-File only */

			/* Garbage-Collection */
			LibraryEditor_SpriteStudio6.Utility.Miscellaneous.GarbageCollect();

			return(false);
		}
		private static bool ExecSS6PU(	ref Setting setting,
											ref int countProgressNow,
											int countProgressMax,
											bool flagDisplayProgressBar,
											SSPJ.Information informationSSPJ,
											string nameOutputAssetFolderBase
										)
		{
			const string messageLogPrefix = "Convert-Main (SS6PU)";
			string nameOutputAssetFolder = "";
			string nameOutputAssetBody = "";
			string nameOutputAssetExtention = "";
			bool flagCreateAssetData = true;

			/* Decide Asset Names & Check Assets existing */
			if(false == SSPJ.ModeSS6PU.AssetNameDecide(ref setting, informationSSPJ, nameOutputAssetFolderBase))
			{
				goto ExecSS6PU_ErrorEnd;
			}

			/* Get Datas' count */
			int countTexture = informationSSPJ.TableInformationTexture.Length;
			int countSSCE = informationSSPJ.TableInformationSSCE.Length;
			int countSSAE = informationSSPJ.TableInformationSSAE.Length;
			int countSSEE = informationSSPJ.TableInformationSSEE.Length;
			int countSSQE = informationSSPJ.TableInformationSSQE.Length;
			int countSSPJ = 1;	/* Force */
			bool flagOverwriteDataProject = false;

			/* Create Asset: Project */
			/* MEMO: Determine project's reference ahead of time so that all assets can also reference the project. */
			/*       In principle, data in project is dummy.                                                        */
			if(0 < countSSPJ)
			{
				/* Convert: Data */
				ProgressBarUpdate(	"Convert SSPJ",
									flagDisplayProgressBar, ref countProgressNow, countProgressMax
								);

				if(false == SSPJ.ModeSS6PU.ConvertDataProject(ref setting, informationSSPJ))
				{
					goto ExecSS6PU_ErrorEnd;
				}

				/* Create-Asset: Data */
				ProgressBarUpdate(	"Create Asset \"Data-Project",
									flagDisplayProgressBar, ref countProgressNow, countProgressMax
								);

				flagCreateAssetData = true;
				if(null == informationSSPJ.DataProjectSS6PU.TableData[0])
				{	/* New */
					/* Create Output Asset-Folder */
					LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																		informationSSPJ.DataProjectSS6PU.TableName[0]
																	);
					if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
					{
						LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
						goto ExecSS6PU_ErrorEnd;
					}
				}
				else
				{	/* Exist */
					if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataProject,
																												informationSSPJ.DataProjectSS6PU.TableName[0],
																												"Data Project"
																											)
						)
					{	/* Not overwrite */
						flagCreateAssetData = false;
						informationSSPJ.DataProjectSS6PU.FlagUpdate[0] = false;
					}
				}
				if(true == flagCreateAssetData)
				{
					if(false == SSPJ.ModeSS6PU.AssetCreateDataProject(ref setting, informationSSPJ))
					{
						goto ExecSS6PU_ErrorEnd;
					}

					flagOverwriteDataProject = true;
				}
			}

			/* Create Asset: Texture */
			/* MEMO: Create Texture-Assets before CellMap for "Trim Transparent-Pixel". */
			if(0 < countTexture)
			{
				/* Copy Texture files */
				for(int i=0; i<countTexture; i++)
				{
					ProgressBarUpdate(	"Copy Textures (" + (i + 1).ToString() + "/" + countTexture.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					if(false == AssetCreateTextureUnity(ref setting, informationSSPJ, informationSSPJ.TableInformationTexture[i], messageLogPrefix))
					{
						goto ExecSS6PU_ErrorEnd;
					}
				}
			}
			countProgressNow -= (countSSEE - countTexture);	/* The number of textures and SSEEs do not necessarily match. When the number is different, SSEEs are more. */

			/* Create Asset: CellMap */
			/* MEMO: Since informations of SSCE files are grouped in 1 CellMap data-asset, always only 1 CellMap data-asset for a SSPJ. */
			/* MEMO: Process after creating all Texture-Assets. */
			if(0 < countSSCE)
			{
				SSCE.Information informationSSCE = null;
				for(int i=0; i<countSSCE; i++)
				{
					/* MEMO: Be sure to "Convert" even when not create CellMap data-assets. Datas may be used at converting SSAE. */
					informationSSCE = informationSSPJ.TableInformationSSCE[i];

					/* Convert "Trim Transparent-Pixel" */
					if(true == setting.PreCalcualation.FlagTrimTransparentPixelsCell)
					{
						ProgressBarUpdate(	"Convert SSCEs \"Trim Pixel\" (" + (i + 1).ToString() + "/" + countSSCE.ToString() + ")",
											flagDisplayProgressBar, ref countProgressNow, countProgressMax
										);

						if(false == SSCE.CellTrimTransparentPixel(ref setting, informationSSPJ, informationSSCE))
						{
							goto ExecSS6PU_ErrorEnd;
						}
					}

					/* Convert */
					ProgressBarUpdate(	"Convert SSCEs (" + (i + 1).ToString() + "/" + countSSCE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);
					if(false == SSCE.ModeSS6PU.ConvertCellMap(ref setting, informationSSPJ, informationSSCE))
					{
						goto ExecSS6PU_ErrorEnd;
					}
				}

				/* Create-Asset */
				ProgressBarUpdate(	"Create Asset \"Data-CellMap\"",
									flagDisplayProgressBar, ref countProgressNow, countProgressMax
								);

				flagCreateAssetData = true;
				if(null == informationSSPJ.DataCellMapSS6PU.TableData[0])
				{	/* New */
					/* Create Output Asset-Folder */
					LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																		informationSSPJ.DataCellMapSS6PU.TableName[0]
																	);
					if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
					{
						LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
						goto ExecSS6PU_ErrorEnd;
					}
				}
				else
				{	/* Exist */
					if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataCellMap,
																												informationSSPJ.DataCellMapSS6PU.TableName[0],
																												"Data CellMap"
																											)
						)
					{	/* Not overwrite */
						flagCreateAssetData = false;
						informationSSPJ.DataCellMapSS6PU.FlagUpdate[0] = false;
					}
				}
				if(true == flagCreateAssetData)
				{
					if(false == SSPJ.ModeSS6PU.AssetCreateCellMap(ref setting, informationSSPJ))
					{
						goto ExecSS6PU_ErrorEnd;
					}
				}
			}

			/* Create-Asset: Effect */
			/* MEMO: SSEE always has only 1 data-asset & 1 prefab. */
			if(0 < countSSEE)
			{
				SSEE.Information informationSSEE = null;
				for(int i=0; i<countSSEE; i++)
				{
					informationSSEE = informationSSPJ.TableInformationSSEE[i];

					/* Convert: Data */
					ProgressBarUpdate(	"Convert SSEEs (" + (i + 1).ToString() + "/" + countSSEE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					if(false == SSEE.ModeSS6PU.ConvertData(ref setting, informationSSPJ, informationSSEE))
					{
						goto ExecSS6PU_ErrorEnd;
					}

					/* Create-Asset: Data */
					ProgressBarUpdate(	"Create Asset \"Data-Effect\" (" + (i + 1).ToString() + "/" + countSSEE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					flagCreateAssetData = true;
					if(null == informationSSEE.DataEffectSS6PU.TableData[0])
					{	/* New */
						/* Create Output Asset-Folder */
						LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																			informationSSEE.DataEffectSS6PU.TableName[0]
																		);
						if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
						{
							LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
							goto ExecSS6PU_ErrorEnd;
						}
					}
					else
					{	/* Exist */
						if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataEffect,
																													informationSSEE.DataEffectSS6PU.TableName[0],
																													"Data Effect"
																												)
							)
						{	/* Not overwrite */
							flagCreateAssetData = false;
							informationSSEE.DataEffectSS6PU.FlagUpdate[0] = false;
						}
					}
					if(true == flagCreateAssetData)
					{
						if(false == SSEE.ModeSS6PU.AssetCreateData(ref setting, informationSSPJ, informationSSEE))
						{
							goto ExecSS6PU_ErrorEnd;
						}
					}

					/* Create-Asset: Prefab */
					ProgressBarUpdate(	"Create Asset \"Prefab-Effect\" (" + (i + 1).ToString() + "/" + countSSEE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					flagCreateAssetData = true;
					if(null == informationSSEE.PrefabEffectSS6PU.TableData[0])
					{	/* New */
						/* Create Output Asset-Folder */
						LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																			informationSSEE.PrefabEffectSS6PU.TableName[0]
																		);
						if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
						{
							LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
							goto ExecSS6PU_ErrorEnd;
						}
					}
					else
					{	/* Exist */
						if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataEffect,
																													informationSSEE.PrefabEffectSS6PU.TableName[0],
																													"Prefab Effect"
																												)
							)
						{	/* Not overwrite */
							flagCreateAssetData = false;
							informationSSEE.PrefabEffectSS6PU.FlagUpdate[0] = false;
						}
					}
					if(true == flagCreateAssetData)
					{
						if(false == SSEE.ModeSS6PU.AssetCreatePrefab(ref setting, informationSSPJ, informationSSEE))
						{
							goto ExecSS6PU_ErrorEnd;
						}
					}
				}
			}

			/* Create-Asset: Animation */
			/* MEMO: SSAE always has only 1 data-asset & 1 prefab. */
			if(0 < countSSAE)
			{
				int indexSSAE;
				SSAE.Information informationSSAE = null;
				for(int i=0; i<countSSAE; i++)
				{
					indexSSAE = informationSSPJ.QueueConvertSSAE[i];
					informationSSAE = informationSSPJ.TableInformationSSAE[indexSSAE];

					/* Open Pack-Attribute's Dictionary */
					if(false == Library_SpriteStudio6.Data.Animation.PackAttribute.DictionaryBootUp(-1, -1, null))
					{
						LogError(messageLogPrefix, "Failure Open PackAttribute's dictionary (for entire-SSAE)  at [" + informationSSPJ.FileNameGetFullPath() + "]");
						goto ExecSS6PU_ErrorEnd;
					}

					/* Convert: Data */
					ProgressBarUpdate(	"Convert SSAEs (" + (i + 1).ToString() + "/" + countSSAE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					if(false == SSAE.ModeSS6PU.ConvertData(ref setting, informationSSPJ, informationSSAE))
					{
						goto ExecSS6PU_ErrorEnd;
					}

					/* Create-Asset: Data */
					ProgressBarUpdate(	"Create Asset \"Data-Animation\" (" + (i + 1).ToString() + "/" + countSSAE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					flagCreateAssetData = true;
					if(null == informationSSAE.DataAnimationSS6PU.TableData[0])
					{	/* New */
						/* Create Output Asset-Folder */
						LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																			informationSSAE.DataAnimationSS6PU.TableName[0]
																		);
						if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
						{
							LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
							goto ExecSS6PU_ErrorEnd;
						}
					}
					else
					{	/* Exist */
						if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataAnimation,
																													informationSSAE.DataAnimationSS6PU.TableName[0],
																													"Data Animation"
																												)
							)
						{	/* Not overwrite */
							flagCreateAssetData = false;
							informationSSAE.DataAnimationSS6PU.FlagUpdate[0] = false;
						}
					}
					if(true == flagCreateAssetData)
					{
						if(false == SSAE.ModeSS6PU.AssetCreateData(ref setting, informationSSPJ, informationSSAE))
						{
							goto ExecSS6PU_ErrorEnd;
						}
					}

					/* Close Pack-Attribute's Dictionary */
					Script_SpriteStudio6_DataAnimation scriptDtaAnimation = informationSSAE.DataAnimationSS6PU.TableData[0];
					if(false == Library_SpriteStudio6.Data.Animation.PackAttribute.DictionaryShutDown(-1, -1, scriptDtaAnimation))
					{
						LogError(messageLogPrefix, "Failure Close PackAttribute's dictionary (for entire-SSAE)  at [" + informationSSPJ.FileNameGetFullPath() + "]");
						goto ExecSS6PU_ErrorEnd;
					}
					if(null != scriptDtaAnimation)
					{	/* Re-Save */
						EditorUtility.SetDirty(scriptDtaAnimation);
						AssetDatabase.SaveAssets();
					}

					/* Create-Asset: Prefab */
					ProgressBarUpdate(	"Create Asset \"Prefab-Animation\" (" + (i + 1).ToString() + "/" + countSSAE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					flagCreateAssetData = true;
					if(null == informationSSAE.PrefabAnimationSS6PU.TableData[0])
					{	/* New */
						/* Create Output Asset-Folder */
						LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																			informationSSAE.PrefabAnimationSS6PU.TableName[0]
																		);
						if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
						{
							LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
							goto ExecSS6PU_ErrorEnd;
						}
					}
					else
					{	/* Exist */
						if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataAnimation,
																													informationSSAE.PrefabAnimationSS6PU.TableName[0],
																													"Prefab Animation"
																												)
							)
						{	/* Not overwrite */
							flagCreateAssetData = false;
							informationSSAE.PrefabAnimationSS6PU.FlagUpdate[0] = false;
						}
					}
					if(true == flagCreateAssetData)
					{
						if(false == SSAE.ModeSS6PU.AssetCreatePrefab(ref setting, informationSSPJ, informationSSAE))
						{
							goto ExecSS6PU_ErrorEnd;
						}
					}
				}
			}

			/* Create Asset: Sequence */
			if(0 < countSSQE)
			{
				SSQE.Information informationSSQE = null;
				for(int i=0; i<countSSQE; i++)
				{
					informationSSQE = informationSSPJ.TableInformationSSQE[i];

					/* Convert: Data */
					ProgressBarUpdate(	"Convert SSQEs (" + (i + 1).ToString() + "/" + countSSQE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					if(false == SSQE.ModeSS6PU.ConvertData(ref setting, informationSSPJ, informationSSQE))
					{
						goto ExecSS6PU_ErrorEnd;
					}

					/* Create-Asset: Data */
					ProgressBarUpdate(	"Create Asset \"Data-Sequence\" (" + (i + 1).ToString() + "/" + countSSAE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					flagCreateAssetData = true;
					if(null == informationSSQE.DataSequenceSS6PU.TableData[0])
					{	/* New */
						/* Create Output Asset-Folder */
						LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																			informationSSQE.DataSequenceSS6PU.TableName[0]
																		);
						if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
						{
							LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
							goto ExecSS6PU_ErrorEnd;
						}
					}
					else
					{	/* Exist */
						if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataSequence,
																													informationSSQE.DataSequenceSS6PU.TableName[0],
																													"Data Sequence"
																												)
							)
						{	/* Not overwrite */
							flagCreateAssetData = false;
							informationSSQE.DataSequenceSS6PU.FlagUpdate[0] = false;
						}
					}
					if(true == flagCreateAssetData)
					{
						if(false == SSQE.ModeSS6PU.AssetCreateData(ref setting, informationSSPJ, informationSSQE))
						{
							goto ExecSS6PU_ErrorEnd;
						}
					}
				}
			}

			/* Fix Asset: Project */
			/* MEMO: Now that all asset's references are finalized, set them. */
			if(true == flagOverwriteDataProject)
			{
				if(false == SSPJ.ModeSS6PU.AssetFixDataProject(ref setting, informationSSPJ))
				{
					goto ExecSS6PU_ErrorEnd;
				}
			}

			return(true);

		ExecSS6PU_ErrorEnd:;
			return(false);
		}
		private static bool ExecUnityNative(	ref Setting setting,
												ref int countProgressNow,
												int countProgressMax,
												bool flagDisplayProgressBar,
												SSPJ.Information informationSSPJ,
												string nameOutputAssetFolderBase
											)
		{
			const string messageLogPrefix = "Convert-Main (UnityNative)";
			string nameOutputAssetFolder = "";
			string nameOutputAssetBody = "";
			string nameOutputAssetExtention = "";
			bool flagCreateAssetData = true;

			/* Decide Asset Names & Check Assets existing */
			if(false == SSPJ.ModeUnityNative.AssetNameDecide(ref setting, informationSSPJ, nameOutputAssetFolderBase))
			{
				goto ExecUnityNative_ErrorEnd;
			}

			/* Get Datas' count */
			int countTexture = informationSSPJ.TableInformationTexture.Length;
			int countSSCE = informationSSPJ.TableInformationSSCE.Length;
			int countSSAE = informationSSPJ.TableInformationSSAE.Length;
			int countSSEE = informationSSPJ.TableInformationSSEE.Length;

			/* Create Asset: Texture */
			/* MEMO: Create Texture-Assets before CellMap for "Trim Transparent-Pixel". */
			/* MEMO: Enable read till sprite-datas set complete. */
			if(0 < countTexture)
			{
				/* Copy Texture files */
				for(int i=0; i<countTexture; i++)
				{
					ProgressBarUpdate(	"Copy Textures (" + (i + 1).ToString() + "/" + countTexture.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					if(false == AssetCreateTextureUnity(ref setting, informationSSPJ, informationSSPJ.TableInformationTexture[i], messageLogPrefix))
					{
						goto ExecUnityNative_ErrorEnd;
					}
				}
			}
			countProgressNow -= (countSSEE - countTexture);	/* The number of textures and SSEEs do not necessarily match. When the number is different, SSEEs are more. */

			/* Convert SSCEs */
			/* MEMO: Currently, SSCE does not has own data-file in "Unity-Native" mode. */
			/*       (... but might create later)                                       */
			if(0 < countSSCE)
			{
				SSCE.Information informationSSCE = null;
				for(int i=0; i<countSSCE; i++)
				{
					/* MEMO: Be sure to "Convert" even when not create CellMap data-assets. Datas may be used at converting SSAE. */
					informationSSCE = informationSSPJ.TableInformationSSCE[i];

					/* MEMO: "Trim Transparent-Pixel" processing is unnecessary since Unity's sprite trims transparent pixels automatically by mesh shape. */

					/* Convert (Create Textures' Atlas) */
					ProgressBarUpdate(	"Convert SSCEs (" + (i + 1).ToString() + "/" + countSSCE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);
					if(false == SSCE.ModeUnityNative.ConvertCellMap(ref setting, informationSSPJ, informationSSCE))
					{
						goto ExecUnityNative_ErrorEnd;
					}
				}

				/* Fix Texture */
				for(int i=0; i<countTexture; i++)
				{
					/* Add Atlases to Textures */
					if(false == SSCE.ModeUnityNative.CellMapSetTexture(ref setting, informationSSPJ, i))
					{
						goto ExecUnityNative_ErrorEnd;
					}
				}
			}

			/* Create Assrts SSAEs */
			if(0 < countSSAE)
			{
				SSAE.Information informationSSAE = null;
				for(int i=0; i<countSSAE; i++)
				{
					informationSSAE = informationSSPJ.TableInformationSSAE[i];

					/* Convert Parts (Create Temporary GameObjects) */
					ProgressBarUpdate(	"Convert & Create Asset SSAEs (" + (i + 1).ToString() + "/" + countSSAE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					GameObject gameObjectRoot = SSAE.ModeUnityNative.ConvertPartsAnimation(ref setting, informationSSPJ, informationSSAE);
					if(null == gameObjectRoot)
					{
						goto ExecUnityNative_ErrorEnd;
					}

					/* Convert SSAEs: Create Bone-Information */
					SSAE.ModeUnityNative.CreateBoneInformation(ref setting, informationSSPJ, informationSSAE);

					/* Create Asset: Mesh-Bind (Skinned-Mesh) */
					int countParts = informationSSAE.TableParts.Length;
					for(int j=0; j<countParts; j++)
					{
						flagCreateAssetData = true;
						if(	(false == string.IsNullOrEmpty(informationSSAE.TableParts[j].DataMeshSkinnedUnityNative.TableName[0]))
							&& (null == informationSSAE.TableParts[j].DataMeshSkinnedUnityNative.TableData[0])
							)
						{	/* New */
							/* Create Output Asset-Folder */
							LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																				informationSSAE.TableParts[j].DataMeshSkinnedUnityNative.TableName[0]
																			);
							if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
							{
								LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
								goto ExecUnityNative_ErrorEnd;
							}
						}
						else
						{	/* Exist */
							if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataAnimation,
																														informationSSAE.TableParts[j].DataMeshSkinnedUnityNative.TableName[0],
																														"Data Skinned-Mesh"
																													)
								)
							{	/* Not overwrite */
								flagCreateAssetData = false;
								informationSSAE.TableParts[j].DataMeshSkinnedUnityNative.FlagUpdate[0] = false;
							}
						}
						if(true == flagCreateAssetData)
						{
							/* MEMO: In this process only Bind-Pose are not determined. */
							/*       Bind-Pose are determined at runtime.               */
							/* MEMO: Since processing to optimize bones used in this function to the minimum is performed, */
							/*       set each "Mesh" part's "Script_SpriteStudio6_PartsUnityNative.TableTransformBone".    */
							/*       In addition, replace component for mesh parts not assigned bones with                 */
							/*        "MeshFilter & MeshRendere" within this function.                                     */
							if(false == SSAE.ModeUnityNative.AssetCreateDataMesh(ref setting, informationSSPJ, informationSSAE, j))
							{
								goto ExecUnityNative_ErrorEnd;
							}
						}
					}

					/* Create Asset: Animation (AnimationClip) */
					int countAnimation = informationSSAE.TableAnimation.Length;
					for(int j=0; j<countAnimation; j++)
					{
						flagCreateAssetData = true;
						if(null == informationSSAE.DataAnimationUnityNative.TableData[j])
						{	/* New */
							/* Create Output Asset-Folder */
							LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																				informationSSAE.DataAnimationUnityNative.TableName[j]
																			);
							if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
							{
								LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
								goto ExecUnityNative_ErrorEnd;
							}
						}
						else
						{	/* Exist */
							if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataAnimation,
																														informationSSAE.DataAnimationUnityNative.TableName[j],
																														"Data Animation"
																													)
								)
							{	/* Not overwrite */
								flagCreateAssetData = false;
								informationSSAE.DataAnimationUnityNative.FlagUpdate[j] = false;
							}
						}
						if(true == flagCreateAssetData)
						{
							if(false == SSAE.ModeUnityNative.AssetCreateData(ref setting, informationSSPJ, informationSSAE, j))
							{
								goto ExecUnityNative_ErrorEnd;
							}
						}
					}

					/* Create Asset: Animation (Prefab) */
					flagCreateAssetData = true;
					if(null == informationSSAE.PrefabAnimationUnityNative.TableData[0])
					{	/* New */
						/* Create Output Asset-Folder */
						LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																			informationSSAE.PrefabAnimationUnityNative.TableName[0]
																		);
						if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
						{
							LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
							goto ExecUnityNative_ErrorEnd;
						}
					}
					else
					{	/* Exist */
						if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataAnimation,
																													informationSSAE.PrefabAnimationUnityNative.TableName[0],
																													"Prefab Animation"
																												)
							)
						{	/* Not overwrite */
							flagCreateAssetData = false;
							informationSSAE.PrefabAnimationUnityNative.FlagUpdate[0] = false;
						}
					}
					if(true == flagCreateAssetData)
					{
						if(false == SSAE.ModeUnityNative.AssetCreatePrefab(ref setting, informationSSPJ, informationSSAE, gameObjectRoot))
						{
							goto ExecUnityNative_ErrorEnd;
						}
						gameObjectRoot = null;
					}
				}
			}

			return(true);

		ExecUnityNative_ErrorEnd:;
			return(false);
		}
		private static bool ExecUnityUI(	ref Setting setting,
												ref int countProgressNow,
												int countProgressMax,
												bool flagDisplayProgressBar,
												SSPJ.Information informationSSPJ,
												string nameOutputAssetFolderBase
											)
		{
			const string messageLogPrefix = "Convert-Main (UnityUI)";
			string nameOutputAssetFolder = "";
			string nameOutputAssetBody = "";
			string nameOutputAssetExtention = "";
			bool flagCreateAssetData = true;

			/* Decide Asset Names & Check Assets existing */
			if(false == SSPJ.ModeUnityUI.AssetNameDecide(ref setting, informationSSPJ, nameOutputAssetFolderBase))
			{
				goto ExecUnityUI_ErrorEnd;
			}

			/* Get Datas' count */
			int countTexture = informationSSPJ.TableInformationTexture.Length;
			int countSSCE = informationSSPJ.TableInformationSSCE.Length;
			int countSSAE = informationSSPJ.TableInformationSSAE.Length;
			int countSSEE = informationSSPJ.TableInformationSSEE.Length;

			/* Create Asset: Texture */
			/* MEMO: Create Texture-Assets before CellMap for "Trim Transparent-Pixel". */
			/* MEMO: Enable read till sprite-datas set complete. */
			if(0 < countTexture)
			{
				/* Copy Texture files */
				for(int i=0; i<countTexture; i++)
				{
					ProgressBarUpdate(	"Copy Textures (" + (i + 1).ToString() + "/" + countTexture.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					if(false == AssetCreateTextureUnity(ref setting, informationSSPJ, informationSSPJ.TableInformationTexture[i], messageLogPrefix))
					{
						goto ExecUnityUI_ErrorEnd;
					}
				}
			}

			/* Convert SSCEs */
			/* MEMO: Currently, "informationSSCE.ListSpriteMetaDataUnityNative" and "informationSSCE.ListSpriteUnityNative" */
			/*         are also shared in "UnityUI" mode.                                                                   */
			if(0 < countSSCE)
			{
				SSCE.Information informationSSCE = null;
				for(int i=0; i<countSSCE; i++)
				{
					/* MEMO: Be sure to "Convert" even when not create CellMap data-assets. Datas may be used at converting SSAE. */
					informationSSCE = informationSSPJ.TableInformationSSCE[i];

					/* MEMO: "Trim Transparent-Pixel" processing is unnecessary since Unity's sprite trims transparent pixels automatically by mesh shape. */

					/* Convert (Create Textures' Atlas) */
					ProgressBarUpdate(	"Convert SSCEs (" + (i + 1).ToString() + "/" + countSSCE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);
					if(false == SSCE.ModeUnityUI.ConvertCellMap(ref setting, informationSSPJ, informationSSCE))
					{
						goto ExecUnityUI_ErrorEnd;
					}
				}

				/* Fix Texture */
				for(int i=0; i<countTexture; i++)
				{
					/* Add Atlases to Textures */
					if(false == SSCE.ModeUnityUI.CellMapSetTexture(ref setting, informationSSPJ, i))
					{
						goto ExecUnityUI_ErrorEnd;
					}
				}
			}

			/* Create Assrts SSAEs (GameObjeccts & AnimationClips) */
			if(0 < countSSAE)
			{
				SSAE.Information informationSSAE = null;
				for(int i=0; i<countSSAE; i++)
				{
					informationSSAE = informationSSPJ.TableInformationSSAE[i];

					/* Convert Parts (Create Temporary GameObjects) */
					ProgressBarUpdate(	"Convert & Create Asset SSAEs (" + (i + 1).ToString() + "/" + countSSAE.ToString() + ")",
										flagDisplayProgressBar, ref countProgressNow, countProgressMax
									);

					GameObject gameObjectRoot = SSAE.ModeUnityUI.ConvertPartsAnimation(ref setting, informationSSPJ, informationSSAE);
					if(null == gameObjectRoot)
					{
						goto ExecUnityUI_ErrorEnd;
					}

					/* Create Asset: Animation (AnimationClip) */
					int countAnimation = informationSSAE.TableAnimation.Length;
					for(int j=0; j<countAnimation; j++)
					{
						flagCreateAssetData = true;
						if(null == informationSSAE.DataAnimationUnityUI.TableData[j])
						{	/* New */
							/* Create Output Asset-Folder */
							LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																				informationSSAE.DataAnimationUnityUI.TableName[j]
																			);
							if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
							{
								LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
								goto ExecUnityUI_ErrorEnd;
							}
						}
						else
						{	/* Exist */
							if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataAnimation,
																														informationSSAE.DataAnimationUnityUI.TableName[j],
																														"Data Animation"
																													)
								)
							{	/* Not overwrite */
								flagCreateAssetData = false;
								informationSSAE.DataAnimationUnityUI.FlagUpdate[j] = false;
							}
						}
						if(true == flagCreateAssetData)
						{
							if(false == SSAE.ModeUnityUI.AssetCreateData(ref setting, informationSSPJ, informationSSAE, j))
							{
								goto ExecUnityUI_ErrorEnd;
							}
						}
					}

					/* Create Asset: Animation (Prefab) */
					flagCreateAssetData = true;
					if(null == informationSSAE.PrefabAnimationUnityUI.TableData[0])
					{	/* New */
						/* Create Output Asset-Folder */
						LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																			informationSSAE.PrefabAnimationUnityUI.TableName[0]
																		);
						if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
						{
							LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
							goto ExecUnityUI_ErrorEnd;
						}
					}
					else
					{	/* Exist */
						if(false == LibraryEditor_SpriteStudio6.Utility.File.PermissionGetConfirmDialogueOverwrite(	ref setting.ConfirmOverWrite.FlagDataAnimation,
																													informationSSAE.PrefabAnimationUnityUI.TableName[0],
																													"Prefab Animation"
																												)
							)
						{	/* Not overwrite */
							flagCreateAssetData = false;
							informationSSAE.PrefabAnimationUnityUI.FlagUpdate[0] = false;
						}
					}
					if(true == flagCreateAssetData)
					{
						if(false == SSAE.ModeUnityUI.AssetCreatePrefab(ref setting, informationSSPJ, informationSSAE, gameObjectRoot))
						{
							goto ExecUnityUI_ErrorEnd;
						}
						gameObjectRoot = null;
					}
				}
			}

			return(true);

		ExecUnityUI_ErrorEnd:;
			return(false);
		}
		private static bool AssetCreateTextureUnity(	ref Setting setting,
														SSPJ.Information informationSSPJ,
														SSCE.Information.Texture informationTexture,
														string messageLogPrefix
											)
		{
			string nameOutputAssetFolder = "";
			string nameOutputAssetBody = "";
			string nameOutputAssetExtention = "";
			bool flagCreateAssetData = true;

			/* Create-Asset */
			if(null == informationTexture.PrefabTexture.TableData[0])
			{	/* New */
				/* Create Output Asset-Folder */
				LibraryEditor_SpriteStudio6.Utility.File.PathSplit(	out nameOutputAssetFolder, out nameOutputAssetBody, out nameOutputAssetExtention,
																	informationTexture.PrefabTexture.TableName[0]
																);
				if(true == string.IsNullOrEmpty(LibraryEditor_SpriteStudio6.Utility.File.AssetFolderCreate(nameOutputAssetFolder)))
				{
					LogError(messageLogPrefix, "Asset-Folder \"" + nameOutputAssetFolder + "\" could not be created at [" + informationSSPJ.FileNameGetFullPath() + "]");
					return(false);
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
					flagCreateAssetData = false;
					informationTexture.PrefabTexture.FlagUpdate[0] = false;
				}
			}
			if(true == flagCreateAssetData)
			{
				if(false == SSCE.AssetCreateTexture(ref setting, informationSSPJ, informationTexture))
				{
					return(false);
				}
			}

			return(true);
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
			LibraryEditor_SpriteStudio6.Utility.Miscellaneous.ProgressBarUpdate(	Library_SpriteStudio6.SignatureNameAsset + " Data Import",
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
			SSQE,
		}

		public const string NameExtentionMesh = ".asset";
		public const string NameExtentionScriptableObject = ".asset";
		public const string NameExtensionPrefab = ".prefab";

		public const string NameTagSpritePackerTexture = "SpriteStudio";

#if UNITY_2018_4_OR_NEWER || UNITY_2019_1_OR_NEWER
#else
		public const ReplacePrefabOptions OptionPrefabReplace = ReplacePrefabOptions.ReplaceNameBased;
#endif
		#endregion Enums & Constants

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public struct Assets<_Type>
			where _Type : class
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public bool[] FlagUpdate;
			public bool[] FlagInUse;						/* MEMO: Basically used only in materials */
			public int[] Version;
			public string[] TableName;
			public _Type[] TableData;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public void CleanUp()
			{
				FlagUpdate = null;
				FlagInUse = null;
				Version = null;
				TableName = null;
				TableData = null;
			}

			public void BootUp(int count)
			{
				FlagUpdate = new bool[count];
				FlagInUse = new bool[count];
				Version = new int[count];
				TableName = new string[count];
				TableData = new _Type[count];
				for(int i=0; i<count; i++)
				{
					FlagUpdate[i] = true;
					FlagInUse[i] = true;
					Version[i] = -1;
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
		public static partial class File
		{
			/* ----------------------------------------------- Functions */
			#region Functions
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
				nameDirectory = PathNormalize(System.IO.Path.GetDirectoryName(namePathNormalized) + "/");
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

			public static bool PathCheckRoot(string namePath)
			{	/* MEMO: Create another function separately, since possibility that can not be checked with IsPathRooted. */
				return(System.IO.Path.IsPathRooted(namePath));
			}

			public static bool FileCopyToAsset(string nameAsset, string nameOriginalFileName, bool flagOverCopy)
			{
				if(false == System.IO.File.Exists(nameOriginalFileName))
				{
					return(false);
				}

				System.IO.File.Copy(nameOriginalFileName, nameAsset, flagOverCopy);
				return(true);
			}

			public static bool NamesGetFileDialogLoad(	out string nameDirectory,
														out string nameFileBody,
														out string nameFileExtension,
														string nameDirectoryPrevious,
														string textTitleDialog,
														string filterExtension
													)
			{
				if(true == string.IsNullOrEmpty(nameDirectoryPrevious))
				{
					nameDirectoryPrevious = "";
				}

				/* Choose file */
				string fileNameFullPath = EditorUtility.OpenFilePanel(textTitleDialog, nameDirectoryPrevious, filterExtension);
				if(0 == fileNameFullPath.Length)
				{	/* Cancelled */
					nameDirectory = "";
					nameFileBody = "";
					nameFileExtension = "";

					return(false);
				}

				return(PathSplit(out nameDirectory, out nameFileBody, out nameFileExtension, fileNameFullPath));
			}

			public static bool NamesGetFileDialogSave(	out string nameDirectory,
														out string nameFileBody,
														out string nameFileExtension,
														string nameDirectoryPrevious,
														string nameFilePrevious,
														string textTitleDialog,
														string nameExtension
													)
			{
				/* Choose file */
				string fileNameFullPath = EditorUtility.SaveFilePanel(textTitleDialog, nameDirectoryPrevious, nameFilePrevious, nameExtension);
				if(0 == fileNameFullPath.Length)
				{	/* Cancelled */
					nameDirectory = "";
					nameFileBody = "";
					nameFileExtension = "";

					return(false);
				}

				return(PathSplit(out nameDirectory, out nameFileBody, out nameFileExtension, fileNameFullPath));
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
				/* MEMO: When use "AssetDataBase.CreateFolder" to create folders recursively, processing may be delayed. */
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

			public static UnityEngine.Object AssetFolderGetPath(string path)
			{
				/* MEMO: Cannot get Folder-asset when the path ends with "/". */
				path = path.TrimEnd('/');

				return(AssetDatabase.LoadAssetAtPath(path, typeof(DefaultAsset)));
			}

			public static string AssetPathGetSelected(string namePath=null)
			{
				string namePathAsset = string.Empty;
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

			public static bool AssetCheckFolder(string namePath)
			{
				if(true == string.IsNullOrEmpty(namePath))
				{
					return(false);
				}

				return(AssetDatabase.IsValidFolder(namePath));
			}

			public static string GUIDGetAsset(UnityEngine.Object asset)
			{
				string guid = string.Empty;
				if(null != asset)
				{
					string namePath = PathGetAsset(asset);
					if(true == string.IsNullOrEmpty(namePath))
					{
						return(string.Empty);
					}
					guid = AssetDatabase.AssetPathToGUID(namePath);
				}
				return(guid);
			}

			public static string PathGetAsset(UnityEngine.Object asset)
			{
				if(null == asset)
				{
					return(string.Empty);
				}
				return(AssetDatabase.GetAssetPath(asset));
			}

			public static _Type AssetGetGUID<_Type>(string guid)
				where _Type: class
			{
				if(true == string.IsNullOrEmpty(guid))
				{
					return(null);
				}

				string namePath = AssetDatabase.GUIDToAssetPath(guid);
				return(AssetGetPath<_Type>(namePath));
			}

			public static _Type AssetGetPath<_Type>(string path)
				where _Type: class
			{
				_Type asset = AssetDatabase.LoadAssetAtPath(path, typeof(_Type)) as _Type;
				return(asset);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			private readonly static char[] TextSplitFolder = 
			{
				'/',
				'\\',
			};

			internal readonly static string NamePathRootNative = Application.dataPath;
			internal const string NamePathRootAsset = "Assets";
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

			public static bool TextToColor(out float colorA, out float colorR, out float colorG, out float colorB, string text)
			{
				uint ARGB = LibraryEditor_SpriteStudio6.Utility.Text.HexToUInt(text);
				colorA = (float)((ARGB >> 24) & 0xff) / 255.0f;
				colorR = (float)((ARGB >> 16) & 0xff) / 255.0f;
				colorG = (float)((ARGB >> 8) & 0xff) / 255.0f;
				colorB = (float)(ARGB & 0xff) / 255.0f;
				return(true);
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

			public static string NameNormalize(string name)
			{
				if(true == string.IsNullOrEmpty(name))
				{
					return("");
				}

				string rv = string.Copy(name);
				rv = rv.Replace(":", "_");	/* "/" -> "_" */
				rv = rv.Replace("\\", "/");	/* "\" -> "/" */
				rv = rv.Replace("/", "_");	/* "/" -> "_" */
				rv = rv.Replace(".", "_");	/* "." -> "_" */
				rv = rv.Replace(",", "_");	/* "," -> "_" */
				rv = rv.Replace("\"", "_");	/* """ -> "_" */
				rv = rv.Replace("|", "_");	/* "|" -> "_" */
				rv = rv.Replace("[", "_");	/* "[" -> "_" */
				rv = rv.Replace("]", "_");	/* "]" -> "_" */
				rv = rv.Replace(";", "_");	/* ";" -> "_" */
				rv = rv.Replace("=", "_");	/* "=" -> "_" */
				rv = rv.Replace(" ", "_");	/* " " -> "_" */
				rv = rv.Replace("~", "_");	/* " " -> "_" */
				rv = rv.Replace("$", "_");	/* "$" -> "_" */
				rv = rv.Replace("@", "_");	/* "@" -> "_" */
				rv = rv.Replace("&", "_");	/* "&" -> "_" */
				rv = rv.Replace("\0x00", "_");	/* NULL -> "_" */

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

				System.Xml.XmlNode nodeVersion = attributeNodeRoot["version"];
				string versionText = nodeVersion.Value;
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
					int countValid = 0;
					for(int i=0; i<count; i++)
					{
						textArgument[i] = textArgument[i].Trim(PrefixChangeCommand);
						textArgument[i] = TextTrim(textArgument[i]);
						if(true == string.IsNullOrEmpty(textArgument[i]))
						{	/* Empty */
							textArgument[i] = null;
						}
						else
						{	/* Exist */
							countValid++;
						}
					}
					if(0 >= countValid)
					{
						return(null);
					}

					string[] textArgumentValid = new string[countValid];
					countValid = 0;
					for(int i=0; i<count; i++)
					{
						if(null != textArgument[i])
						{
							textArgumentValid[countValid] = textArgument[i];
							countValid++;
						}
					}
					textArgument = textArgumentValid;
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

			public static string IntEncode(int value)
			{
				return(value.ToString());
			}

			public static int IntDecode(string text)
			{
				int value;
				if(false == int.TryParse(text, out value))
				{
					value = 0;
				}
				return(value);
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
			public static void Error(string message, bool flagExternalOnly=false, bool flagIndentExternal=true)
			{
				string text = "SS6PU Error: " + message;
				if(false == flagExternalOnly)
				{
					Debug.LogError(text);
				}
				if(null != StreamExternal)
				{
					if(true == flagIndentExternal)
					{
						text = "\t" + text;
					}
					StreamExternal.WriteLine(text);
				}
			}

			public static void Warning(string message, bool flagExternalOnly=false, bool flagIndentExternal=true)
			{
				string text = "SS6PU Warning: " + message;
				if(false == flagExternalOnly)
				{
					Debug.LogWarning(text);
				}
				if(null != StreamExternal)
				{
					if(true == flagIndentExternal)
					{
						text = "\t" + text;
					}
					StreamExternal.WriteLine(text);
				}
			}

			public static void Message(string message, bool flagExternalOnly=false, bool flagIndentExternal=true)
			{
				string text = "SS6PU-Message: " + message;
				if(false == flagExternalOnly)
				{
					Debug.Log(text);
				}
				if(null != StreamExternal)
				{
					if(true == flagIndentExternal)
					{
						text = "\t" + text;
					}
					StreamExternal.WriteLine(text);
				}
			}
			#endregion Functions
		}

		public static partial class Inspector
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public partial class Preview : System.IDisposable
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				private bool FlagIsBusy;
				public bool StatusIsBusy
				{
					get
					{
						return(FlagIsBusy);
					}
				}

				private UnityEngine.RenderTexture InstanceTextureTarget;
				internal UnityEngine.RenderTexture TextureTarget
				{
					get
					{
						return(InstanceTextureTarget);
					}
				}

				private UnityEngine.SceneManagement.Scene Scene;
				private UnityEngine.GameObject GameObjectScene;
				private UnityEngine.Camera Camera;
				public UnityEngine.GameObject GameObjectAnimation;

				private System.Diagnostics.Stopwatch Timer = null;
				private float TimeDelta = float.NaN;
				internal float TimeElapsed
				{
					get
					{
						if((null == Timer) || (true == float.IsNaN(TimeDelta)))
						{
							return(0.0f);
						}

						return(TimeDelta);
					}
				}
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public Preview()
				{
					CleanUp();
				}
				private void CleanUp()
				{
					FlagIsBusy = false;

//					Scene = 

					InstanceTextureTarget = null;

					GameObjectScene = null;
					GameObjectAnimation = null;

					Timer = null;
					TimeDelta = float.NaN;
				}

				public bool Create(GameObject gameObjectAnimationSource)
				{
					if(true == FlagIsBusy)
					{
						return(true);
					}

					/* Create Render(Target)-Texture */
					{
						UnityEngine.Experimental.Rendering.GraphicsFormat formateTexture = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm;
						if(true == FlagAllowHDRCamera)
						{
							formateTexture = UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SFloat;	/* Half */
						}
						InstanceTextureTarget = new RenderTexture(SizeXTextureTarget, SizeYTextureTarget, 32, formateTexture);
					}

					/* Create Preview-Scene */
					Scene = UnityEditor.SceneManagement.EditorSceneManager.NewPreviewScene();

					/* Create Terminal-GameObject */
					GameObjectScene = new GameObject("Scene");
					UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(GameObjectScene, Scene);
					{
						/* Set Camera */
						GameObject gameObjectCamera = new GameObject("Camera");
						gameObjectCamera.transform.parent = GameObjectScene.transform;

						Camera = gameObjectCamera.AddComponent<UnityEngine.Camera>();
						Camera.transform.position = PositionCamera;
						Camera.transform.rotation = RotationCamera;
						Camera.nearClipPlane = 0.0f;
						Camera.farClipPlane = 10000.0f;
//						Camera.transform.lossyScale =
						Camera.orthographic = true;
						Camera.orthographicSize = SizeCamera;
						Camera.clearFlags = UnityEngine.CameraClearFlags.Color;
						Camera.backgroundColor = ColorCamera;
						Camera.renderingPath = UnityEngine.RenderingPath.UsePlayerSettings;
						Camera.depth = 0.0f;
						Camera.allowHDR = FlagAllowHDRCamera;
						Camera.useOcclusionCulling = false;
						Camera.allowMSAA = false;
						Camera.allowDynamicResolution = false;
						Camera.scene = Scene;
						Camera.forceIntoRenderTexture = true;
						Camera.targetTexture = InstanceTextureTarget;

						/* MEMO: Keep camera from running automatically. (2 updates run: scene lifecycle and manual) */
						Camera.enabled = false;

						/* Animation-Object Set (Copy from Select-Object) */
						GameObjectAnimation = UnityEngine.Object.Instantiate(gameObjectAnimationSource.gameObject, PositionAnimation, RotationAnimation, GameObjectScene.transform);
					}

					/* Create stopwatch */
					Timer = System.Diagnostics.Stopwatch.StartNew();
					TimeDelta = float.NaN;

					/* Set status */
					FlagIsBusy = true;

					return(true);
				}

				public void Update()
				{
					if(false == FlagIsBusy)
					{
						return;
					}

					/* Elapsed time Get */
					Timer.Stop();

					/* Caculate delta-time */
					TimeDelta = 0.0f;
					if(false == float.IsNaN(TimeDelta))
					{
						TimeDelta = (float)((double)Timer.ElapsedTicks * TickTimer);
					}

					/* Elapsed time Resett */
					Timer.Restart();
				}

				public void Render()
				{
					if((null != Camera) && (null != TextureTarget))
					{
						Camera.Render();
					}
				}

				public void Dispose()
				{
					if(true == FlagIsBusy)
					{
						if(null != Camera)
						{
							Camera.forceIntoRenderTexture = false;
							Camera.targetTexture = null;
						}

						/* Destroy Terminal-GameObject */
						if(null != GameObjectScene)
						{
							UnityEngine.Object.DestroyImmediate(GameObjectScene);
						}

						/* Destroy Preview-Scene */
						UnityEditor.SceneManagement.EditorSceneManager.ClosePreviewScene(Scene);

						/* Destroy Render-Texture */
						if(null != InstanceTextureTarget)
						{
							InstanceTextureTarget.Release();
							UnityEngine.Object.DestroyImmediate(InstanceTextureTarget);
						}

						/* Destroy Timer */
						Timer.Stop();
						Timer = null;
					}

					CleanUp();
				}

				public bool ObjectBootUpAnimation(UnityEngine.GameObject gameObjectAnimation)
				{
					if(null == gameObjectAnimation)
					{
						return(false);
					}

					gameObjectAnimation.transform.localPosition = Vector3.zero;
					gameObjectAnimation.transform.localScale = Vector3.one;
					gameObjectAnimation.transform.localRotation = Quaternion.identity;

					gameObjectAnimation.SetActive(true);	/* Allow preview even if selected object is disactive */

					return(true);
				}

				public int FrameSelectFPS(int frameOld, int widthList)
				{
					int framePerSecond = frameOld;
					int indexFPS = System.Array.IndexOf(TableFramePreSecondPreview, framePerSecond);
					if(0 > indexFPS)
					{
						indexFPS = 0;
						framePerSecond = TableFramePreSecondPreview[indexFPS];
					}

					int indexFPSNew = -1;
					if(0 > widthList)
					{
						indexFPSNew = EditorGUILayout.Popup(indexFPS, TableItemFramePerSecondPreview);
					}
					else
					{
						indexFPSNew = EditorGUILayout.Popup(indexFPS, TableItemFramePerSecondPreview, GUILayout.Width(widthList));
					}

					return(TableFramePreSecondPreview[indexFPSNew]);
				}
				public float RateSelectScale(float rateOld, int widthList)
				{
					float rate = rateOld;
					int indexRate = System.Array.IndexOf(TableRateScalePreview, rate);
					if(0 > indexRate)
					{
						indexRate = 0;
						rate = TableRateScalePreview[indexRate];
					}

					int indexRateNew = -1;
					if(0 > widthList)
					{
						indexRateNew = EditorGUILayout.Popup(indexRate, TableItemRateScalePreview);
					}
					else
					{
						indexRateNew = EditorGUILayout.Popup(indexRate, TableItemRateScalePreview, GUILayout.Width(widthList));
					}

					return(TableRateScalePreview[indexRateNew]);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private readonly static Vector3 PositionCamera = new Vector3(0.0f, 0.0f, -50.0f);
				private readonly static Quaternion RotationCamera = Quaternion.Euler(0.0f, 0.0f, 0.0f);
				private const float SizeCamera = 540.0f;
				private readonly static bool FlagAllowHDRCamera = false;
				private readonly static UnityEngine.Color ColorCamera = new UnityEngine.Color((49.0f / 255.0f), (77.0f / 255.0f), (121.0f / 2550f), (0.0f / 255.0f));

				private readonly static Vector3 PositionAnimation = Vector3.zero;
				private readonly static Quaternion RotationAnimation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

				private const int SizeXTextureTarget = 1024;
				private const int SizeYTextureTarget = 1024;
				
				private readonly static double TickTimer = 1.0f / (double)System.Diagnostics.Stopwatch.Frequency;

				private readonly static int[] TableFramePreSecondPreview = new int[]	{
					30,
					45,
					60,
					90,
					120,
				};
				private readonly static string[] TableItemFramePerSecondPreview = new string[]	{
					"30 fps",
					"45 fps",
					"60 fps",
					"90 fps",
					"120 fps",
				};

				private readonly static float[] TableRateScalePreview = new float[]	{
					1.0f / 4.0f,
					1.0f / 3.0f,
					1.0f / 2.0f,
					1.0f / 1.5f,
					1.0f,
					1.5f,
					2.0f,
					3.0f,
					4.0f,
				};
				private readonly static string[] TableItemRateScalePreview = new string[]	{
					"x 0.25",
					"x 0.33",
					"x 0.5",
					"x 0.66",
					"x 1.0",
					"x 1.5",
					"x 2.0",
					"x 3.0",
					"x 4.0",
				};
				#endregion Enums & Constants
			}
			#endregion Classes, Structs & Interfaces
		}

		public static partial class Miscellaneous
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

			public static void GarbageCollect()
			{
				System.GC.Collect();
				System.GC.WaitForPendingFinalizers();
				System.GC.Collect();
			}
			#endregion Functions
		}
		#endregion Classes, Structs & Interfaces
	}
	#endregion Classes, Structs & Interfaces
}
