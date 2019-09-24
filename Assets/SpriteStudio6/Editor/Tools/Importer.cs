/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public sealed class MenuItem_SpriteStudio6_ImportProject : EditorWindow
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	private static LibraryEditor_SpriteStudio6.Import.Setting SettingImport;
	private static Setting SettingOption;
	private static PullDownPackAttribute PullDownPackAttributeAnimation;	/* = new PullDownPackAttribute(); */
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	[MenuItem("Tools/SpriteStudio6/Importer")]
	static void OpenWindow()
	{
		EditorWindow.GetWindow<MenuItem_SpriteStudio6_ImportProject>(true, Library_SpriteStudio6.SignatureNameAsset + " Import-Settings");
		SettingImport.Load();
		SettingOption.Load();

		PullDownPackAttributeAnimation = new PullDownPackAttribute();
		PullDownPackAttributeAnimation.BootUp();
	}
	void OnGUI()
	{
		int levelIndent = 0;
		EditorGUI.indentLevel = levelIndent;

		EditorGUILayout.Space();
		SettingImport.Mode = (LibraryEditor_SpriteStudio6.Import.Setting.KindMode)(EditorGUILayout.Popup("Import Mode", (int)SettingImport.Mode, NameMode));
		EditorGUILayout.Space();
		switch(SettingImport.Mode)
		{
			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.SS6PU:
				ModeSS6PU(levelIndent);
				break;

			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.UNITY_NATIVE:
				ModeUnityNative(levelIndent);
				break;

			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.BATCH_IMPORTER:
				ModeBatchImporter(levelIndent);
				break;

			default:
				break;
		}

		if(true == GUILayout.Button("Import"))
		{
			if(LibraryEditor_SpriteStudio6.Import.Setting.KindMode.BATCH_IMPORTER == SettingImport.Mode)
			{	/* Batch-Import */
				string nameFileList = "";
				string nameFileLog = "";

				string nameDirectoryList;
				string nameFileBodyList;
				string nameFileExtensionList;
				if(true == LibraryEditor_SpriteStudio6.Utility.File.NamesGetFileDialogLoad(	out nameDirectoryList,
																							out nameFileBodyList,
																							out nameFileExtensionList,
																							SettingOption.ModeBatchImporter.NameFolderList,
																							"Select Batch-Importing list file",
																							"txt"
																						)
					)
				{
					nameFileList = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(nameDirectoryList + "/" + nameFileBodyList + nameFileExtensionList);

					SettingOption.ModeBatchImporter.NameFolderList = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(nameDirectoryList);
					SettingOption.ModeBatchImporter.NameFileList = nameFileBodyList;

					if(true == SettingOption.ModeBatchImporter.FlagOutputLog)
					{
						string nameDirectoryLog;
						string nameFileBodyLog;
						string nameFileExtensionLog;
						if(true == LibraryEditor_SpriteStudio6.Utility.File.NamesGetFileDialogSave(	out nameDirectoryLog,
																									out nameFileBodyLog,
																									out nameFileExtensionLog,
																									SettingOption.ModeBatchImporter.NameFolderLog,
																									SettingOption.ModeBatchImporter.NameFileLog,
																									"Select Batch-Importing log file",
																									"txt"
																								)
							)
						{
							nameFileLog = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(nameDirectoryLog + "/" + nameFileBodyLog + nameFileExtensionLog);

							SettingOption.ModeBatchImporter.NameFolderLog = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(nameDirectoryLog);
							SettingOption.ModeBatchImporter.NameFileLog = nameFileBodyLog;
						}
						else
						{
							nameFileList = "";
						}
					}

					/* Batch-Import */
					if(false == string.IsNullOrEmpty(nameFileList))
					{
						SettingOption.Save();

						if(false == LibraryEditor_SpriteStudio6.Import.Batch.Exec(	ref SettingOption.ModeBatchImporter.Setting,
																					ref SettingImport,
																					nameFileList,
																					nameFileLog
																				)
							)
						{
							EditorUtility.DisplayDialog(	Library_SpriteStudio6.SignatureNameAsset,
															"Batch-Import Interrupted! Check Error on Console.",
															"OK"
													);
						}

						Close();
					}
				}
			}
			else
			{	/* Single File Import */
				string nameBaseAssetPath = LibraryEditor_SpriteStudio6.Utility.File.AssetPathGetSelected();
				if(false == string.IsNullOrEmpty(nameBaseAssetPath))
				{
					LibraryEditor_SpriteStudio6.Utility.Log.StreamExternal = null;	/* Log-File not output */

					string nameDirectory;
					string nameFileBody;
					string nameFileExtension;
					if(true == LibraryEditor_SpriteStudio6.Utility.File.NamesGetFileDialogLoad(	out nameDirectory,
																								out nameFileBody,
																								out nameFileExtension,
																								SettingOption.NameFolderImportPrevious,
																								"Select Importing SSPJ-File",
																								"sspj"
																							)
						)
					{
						string nameFile = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(nameDirectory + "/" + nameFileBody + nameFileExtension);
						SettingOption.NameFolderImportPrevious = nameDirectory;

						SettingOption.Save();
						SettingImport.Save();

						/* Import */
						if(false == LibraryEditor_SpriteStudio6.Import.Exec(	ref SettingImport,
																				nameFile,
																				nameBaseAssetPath,
																				true
																			)
							)
						{
							EditorUtility.DisplayDialog(	Library_SpriteStudio6.SignatureNameAsset,
															"Import Interrupted! Check Error on Console.",
													 		"OK"
													);
						}

						Close();
					}
				}
				else
				{	/* Error (No selected) */
					EditorUtility.DisplayDialog(	Library_SpriteStudio6.SignatureNameAsset,
													"Select Asset-Folder you want to store in before import, on the \"Project\" window.",
													"OK"
											);
				}
			}
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		SettingOption.FlagFoldOutSettingBackUp = EditorGUILayout.Foldout(SettingOption.FlagFoldOutSettingBackUp, "Setting Save/Load/Reset");
		if(true == SettingOption.FlagFoldOutSettingBackUp)
		{
			EditorGUI.indentLevel = levelIndent + 1;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Text file (setting file) save with \"Save to Text-File\" button");
			EditorGUILayout.LabelField(" can also be called from batch-list file with \"SettingFile\" command.");
			EditorGUILayout.Space();

			SettingOption.SettingBackUp.FlagExportCommon = EditorGUILayout.ToggleLeft("Export \"Mode\"", SettingOption.SettingBackUp.FlagExportCommon);
			SettingOption.SettingBackUp.FlagExportBasic = EditorGUILayout.ToggleLeft("Export \"Options: Basic\"", SettingOption.SettingBackUp.FlagExportBasic);
			SettingOption.SettingBackUp.FlagExportPrecalculation = EditorGUILayout.ToggleLeft("Export \"Options: PreCalculation\"", SettingOption.SettingBackUp.FlagExportPrecalculation);
			SettingOption.SettingBackUp.FlagExportCollider = EditorGUILayout.ToggleLeft("Export \"Options: Collider\"", SettingOption.SettingBackUp.FlagExportCollider);
			SettingOption.SettingBackUp.FlagExportConfirmOverWrite = EditorGUILayout.ToggleLeft("Export \"Options: Overwrite Confirm\"", SettingOption.SettingBackUp.FlagExportConfirmOverWrite);
			SettingOption.SettingBackUp.FlagExportCheckVersion = EditorGUILayout.ToggleLeft("Export \"Options: Checking SSxx Version\"", SettingOption.SettingBackUp.FlagExportCheckVersion);
			EditorGUILayout.Space();
			SettingOption.SettingBackUp.FlagExportRuleNameAsset = EditorGUILayout.ToggleLeft("Export \"Advanced Options: Naming Assets\"", SettingOption.SettingBackUp.FlagExportRuleNameAsset);
			SettingOption.SettingBackUp.FlagExportRuleNameAssetFolder = EditorGUILayout.ToggleLeft("Export \"Advanced Options: Naming Asset-Folders\"", SettingOption.SettingBackUp.FlagExportRuleNameAssetFolder);
			SettingOption.SettingBackUp.FlagPackAttributeAnimation = EditorGUILayout.ToggleLeft("Export \"Advanced Options: Attribute data Packing\"", SettingOption.SettingBackUp.FlagPackAttributeAnimation);
			SettingOption.SettingBackUp.FlagPresetMaterial = EditorGUILayout.ToggleLeft("Export \"Advanced Options: Preset Material\"", SettingOption.SettingBackUp.FlagPresetMaterial);
			EditorGUILayout.Space();

			if(true == GUILayout.Button("Save to Text-File"))
			{
//				SettingImport.Save();

				string nameFile = EditorUtility.SaveFilePanel(	"Save \"" + Library_SpriteStudio6.SignatureNameAsset + "\" Import Setting file",
																"",
																"SS6PU_ImportSetting",
																"txt"
															);
				if((null != nameFile) && (0 < nameFile.Length))
				{
					SettingImport.ExportFile(	nameFile,
												SettingOption.SettingBackUp.FlagExportCommon,
												SettingOption.SettingBackUp.FlagExportBasic,
												SettingOption.SettingBackUp.FlagExportPrecalculation,
												SettingOption.SettingBackUp.FlagExportCollider,
												SettingOption.SettingBackUp.FlagExportConfirmOverWrite,
												SettingOption.SettingBackUp.FlagExportCheckVersion,
												SettingOption.SettingBackUp.FlagExportRuleNameAsset,
												SettingOption.SettingBackUp.FlagExportRuleNameAssetFolder,
												SettingOption.SettingBackUp.FlagPackAttributeAnimation,
												SettingOption.SettingBackUp.FlagPresetMaterial
											);
				}
			}

			if(true == GUILayout.Button("Load from Text-File"))
			{
				string nameFile = EditorUtility.OpenFilePanel(	"Load \"" + Library_SpriteStudio6.SignatureNameAsset + "\" Import Setting file",
																"",
																"txt"
															);
				if((null != nameFile) && (0 < nameFile.Length))
				{
					SettingImport.ImportFile(nameFile);
					SettingImport.Save();
				}
			}
			EditorGUILayout.Space();

			if(true == GUILayout.Button("Reset (Restore initial setting)"))
			{
				SettingImport.CleanUp();
				SettingOption.ModeBatchImporter.Setting.CleanUp();	/* Batch-Importer */
			}

			EditorGUI.indentLevel = levelIndent;
		}
	}

	void ModeSS6PU(int levelIndent)
	{
		SettingOption.ModeSS6PU.FlagFoldOutBasic = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagFoldOutBasic, "Options: Basic");
		if(true == SettingOption.ModeSS6PU.FlagFoldOutBasic)
		{
			FoldOutExecBasic(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		SettingOption.ModeSS6PU.FlagFoldOutPreCalcualation = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagFoldOutPreCalcualation, "Options: PreCalculation");
		if(true == SettingOption.ModeSS6PU.FlagFoldOutPreCalcualation)
		{
			FoldOutExecPreCalcualation(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		SettingOption.ModeSS6PU.FlagFoldOutCollider = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagFoldOutCollider, "Options: Collider");
		if(true == SettingOption.ModeSS6PU.FlagFoldOutCollider)
		{
			FoldOutExecCollision(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		SettingOption.ModeSS6PU.FlagFoldOutConfirmOverWrite = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagFoldOutConfirmOverWrite, "Options: Overwrite Confirm");
		if(true == SettingOption.ModeSS6PU.FlagFoldOutConfirmOverWrite)
		{
			FoldOutExecConfirmOverWrite(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		SettingOption.ModeSS6PU.FlagFoldOutCheckVersion = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagFoldOutCheckVersion, "Options: Checking SSxx Version");
		if(true == SettingOption.ModeSS6PU.FlagFoldOutCheckVersion)
		{
			FoldOutExecCheckVersion(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		EditorGUILayout.Space();
		SettingOption.ModeSS6PU.FlagOpenAdvancedOprions = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagOpenAdvancedOprions, "Advanced Options");
		if(true == SettingOption.ModeSS6PU.FlagOpenAdvancedOprions)
		{
			levelIndent++;
			EditorGUI.indentLevel = levelIndent;

			SettingOption.ModeSS6PU.FlagFoldOutRuleNameAsset = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagFoldOutRuleNameAsset, "Advanced Options: Naming Assets");
			if(true == SettingOption.ModeSS6PU.FlagFoldOutRuleNameAsset)
			{
				FoldOutExecRuleNameAsset(levelIndent + 1);
				EditorGUI.indentLevel = levelIndent;
			}

			SettingOption.ModeUnityNative.FlagFoldOutRuleNameAssetFolder = EditorGUILayout.Foldout(SettingOption.ModeUnityNative.FlagFoldOutRuleNameAssetFolder, "Advanced Options: Naming Asset-Foldes");
			if(true == SettingOption.ModeUnityNative.FlagFoldOutRuleNameAssetFolder)
			{
				FoldOutExecRuleNameAssetFolder(levelIndent + 1);
				EditorGUI.indentLevel = levelIndent;
			}

			SettingOption.ModeSS6PU.FlagFoldOutPackAttributeAnimation = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagFoldOutPackAttributeAnimation, "Advanced Options: Attribute data Packing");
			if(true == SettingOption.ModeSS6PU.FlagFoldOutPackAttributeAnimation)
			{
				FoldOutExecPackAttributeAnimation(levelIndent + 1);
				EditorGUI.indentLevel = levelIndent;
			}

			SettingOption.ModeSS6PU.FlagFoldOutPresetMaterial = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagFoldOutPresetMaterial, "Advanced Options: Preset Material");
			if(true == SettingOption.ModeSS6PU.FlagFoldOutPresetMaterial)
			{
				FoldOutExecPresetMaterial(levelIndent + 1);
				EditorGUI.indentLevel = levelIndent;
			}

			levelIndent--;
			EditorGUI.indentLevel = levelIndent;
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}

	void ModeUnityNative(int levelIndent)
	{
		SettingOption.ModeUnityNative.FlagFoldOutCaution = EditorGUILayout.Foldout(SettingOption.ModeUnityNative.FlagFoldOutCaution, "Cautions");
		if(true == SettingOption.ModeUnityNative.FlagFoldOutCaution)
		{
			FoldOutExecCaution(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}
		EditorGUILayout.Space();

		SettingOption.ModeUnityNative.FlagFoldOutBasic = EditorGUILayout.Foldout(SettingOption.ModeUnityNative.FlagFoldOutBasic, "Options: Basic");
		if(true == SettingOption.ModeUnityNative.FlagFoldOutBasic)
		{
			FoldOutExecBasic(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		SettingOption.ModeUnityNative.FlagFoldOutCollider = EditorGUILayout.Foldout(SettingOption.ModeUnityNative.FlagFoldOutCollider, "Options: Collider");
		if(true == SettingOption.ModeUnityNative.FlagFoldOutCollider)
		{
			FoldOutExecCollision(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		SettingOption.ModeUnityNative.FlagFoldOutConfirmOverWrite = EditorGUILayout.Foldout(SettingOption.ModeUnityNative.FlagFoldOutConfirmOverWrite, "Options: Overwrite Confirm");
		if(true == SettingOption.ModeUnityNative.FlagFoldOutConfirmOverWrite)
		{
			FoldOutExecConfirmOverWrite(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		SettingOption.ModeUnityNative.FlagFoldOutCheckVersion = EditorGUILayout.Foldout(SettingOption.ModeUnityNative.FlagFoldOutCheckVersion, "Options: Checking SSxx Version");
		if(true == SettingOption.ModeUnityNative.FlagFoldOutCheckVersion)
		{
			FoldOutExecCheckVersion(levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		EditorGUILayout.Space();
		SettingOption.ModeUnityNative.FlagOpenAdvancedOprions = EditorGUILayout.Foldout(SettingOption.ModeUnityNative.FlagOpenAdvancedOprions, "Advanced Options");
		if(true == SettingOption.ModeUnityNative.FlagOpenAdvancedOprions)
		{
			levelIndent++;
			EditorGUI.indentLevel = levelIndent;

			SettingOption.ModeUnityNative.FlagFoldOutRuleNameAsset = EditorGUILayout.Foldout(SettingOption.ModeUnityNative.FlagFoldOutRuleNameAsset, "Advanced Options: Naming Assets");
			if(true == SettingOption.ModeUnityNative.FlagFoldOutRuleNameAsset)
			{
				FoldOutExecRuleNameAsset(levelIndent + 1);
				EditorGUI.indentLevel = levelIndent;
			}

			SettingOption.ModeUnityNative.FlagFoldOutRuleNameAssetFolder = EditorGUILayout.Foldout(SettingOption.ModeUnityNative.FlagFoldOutRuleNameAssetFolder, "Advanced Options: Naming Asset-Foldes");
			if(true == SettingOption.ModeUnityNative.FlagFoldOutRuleNameAssetFolder)
			{
				FoldOutExecRuleNameAssetFolder(levelIndent + 1);
				EditorGUI.indentLevel = levelIndent;
			}

			SettingOption.ModeUnityNative.FlagFoldOutPresetMaterial = EditorGUILayout.Foldout(SettingOption.ModeUnityNative.FlagFoldOutPresetMaterial, "Advanced Options: Preset Material");
			if(true == SettingOption.ModeUnityNative.FlagFoldOutPresetMaterial)
			{
				FoldOutExecPresetMaterial(levelIndent + 1);
				EditorGUI.indentLevel = levelIndent;
			}

			levelIndent--;
			EditorGUI.indentLevel = levelIndent;
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}

	private void ModeBatchImporter(int levelIndent)
	{
		EditorGUI.indentLevel = levelIndent;

		EditorGUILayout.Space();

		SettingOption.ModeBatchImporter.FlagOutputLog = EditorGUILayout.ToggleLeft("Output log file", SettingOption.ModeBatchImporter.FlagOutputLog);
		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.LabelField("Checked: Output Batch-Import log to text file.");
		EditorGUILayout.LabelField("Unchecked: Not output Batch-Import log to text file.");
		EditorGUI.indentLevel = levelIndent;
		EditorGUILayout.Space();

		EditorGUI.indentLevel = levelIndent;
		SettingOption.ModeBatchImporter.Setting.FlagNotBreakOnError = EditorGUILayout.ToggleLeft("Not break processing when error occurs", SettingOption.ModeBatchImporter.Setting.FlagNotBreakOnError);
		EditorGUILayout.Space();

		SettingOption.ModeBatchImporter.FlagFoldOptions = EditorGUILayout.Foldout(SettingOption.ModeBatchImporter.FlagFoldOptions, "Options");
		if(true == SettingOption.ModeBatchImporter.FlagFoldOptions)
		{
			EditorGUI.indentLevel = levelIndent + 1;

			SettingOption.ModeBatchImporter.Setting.FlagEnableConfirmOverWrite = EditorGUILayout.ToggleLeft("Enable \"OverWrite Confirm\" settings", SettingOption.ModeBatchImporter.Setting.FlagEnableConfirmOverWrite);
			EditorGUI.indentLevel = levelIndent + 2;
			EditorGUILayout.LabelField("Checked: Follow settings in batch-list file. (Apply)");
			EditorGUILayout.LabelField("Unchecked: Not perform, no matter setting in batch-list file. (Ignore)");
			EditorGUILayout.Space();
			EditorGUI.indentLevel = levelIndent + 1;

			SettingOption.ModeBatchImporter.Setting.FlagEnableCheckVersion = EditorGUILayout.ToggleLeft("Enable \"Checking SSxx Version\" settings", SettingOption.ModeBatchImporter.Setting.FlagEnableCheckVersion);
			EditorGUI.indentLevel = levelIndent + 2;
			EditorGUILayout.LabelField("Checked: Follow settings in batch-list file. (Apply)");
			EditorGUILayout.LabelField("Unchecked: Not perform, no matter setting in batch-list file. (Ignore)");
			EditorGUILayout.Space();
			EditorGUI.indentLevel = levelIndent + 1;

			EditorGUI.indentLevel = levelIndent;
		}

		EditorGUILayout.Space();
	}

	private void FoldOutExecBasic(int levelIndent)
	{
		EditorGUI.indentLevel = levelIndent;

		SettingImport.Basic.FlagCreateControlGameObject = EditorGUILayout.ToggleLeft("Create Control-Prefab", SettingImport.Basic.FlagCreateControlGameObject);
		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.LabelField("\"Control-Prefab\" is GameObject attached the script for Auto-Instantiate Body-Prefab.");
		EditorGUILayout.LabelField(" (\"Script_SpriteStudio_ControlPrefab.cs\")");
		EditorGUI.indentLevel = levelIndent;
		EditorGUILayout.Space();

		SettingImport.Basic.FlagCreateProjectFolder = EditorGUILayout.ToggleLeft("Create Project Folder", SettingImport.Basic.FlagCreateProjectFolder);
		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.LabelField("Create a folder with same name as SSPJ.");
		EditorGUI.indentLevel = levelIndent;
		EditorGUILayout.Space();

		SettingImport.Basic.FlagInvisibleToHideAll = EditorGUILayout.ToggleLeft("\"Invisible\" part to \"Hide\" attribute", SettingImport.Basic.FlagInvisibleToHideAll);
		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.LabelField("Convert invisible part setting \"Hide\" attribute. (Hide at all frame)");
		EditorGUI.indentLevel = levelIndent;
		EditorGUILayout.Space();

		SettingImport.Basic.FlagTextureReadable = EditorGUILayout.ToggleLeft("Set Texture \"Read/Write Enabled\"", SettingImport.Basic.FlagTextureReadable);
		EditorGUILayout.Space();

		switch(SettingImport.Mode)
		{
			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.SS6PU:
				SettingImport.Basic.FlagTrackAssets = EditorGUILayout.ToggleLeft("Tracking Assets", SettingImport.Basic.FlagTrackAssets);
				EditorGUI.indentLevel = levelIndent + 1;
				EditorGUILayout.LabelField("Existing assets are identified by tracking prefabs' references when re-importing.");
				EditorGUILayout.LabelField("However, \"Control-Prefab\", \"Animation's Prefab\" and \"Effect's Prefab\" must have");
				EditorGUILayout.LabelField("  the same filename and foldername as when first-import.");
				EditorGUILayout.LabelField("  (Existing of these datas is simply judged by filename and foldername)");
				EditorGUILayout.LabelField("Unchecked, identify by the same name(file and sub folder) as when first-import.");
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();

#if false
				SettingImport.Basic.NoCreateMaterialUnreferenced = (LibraryEditor_SpriteStudio6.Import.Setting.GroupBasic.KindNoCreateMaterialUnreferenced)(EditorGUILayout.Popup("No-Create unreferenced materials", (int)SettingImport.Basic.NoCreateMaterialUnreferenced, NameNoCreateUnreferencedMaterial));
#else
				EditorGUILayout.LabelField("No-Create unreferenced Materials");
				SettingImport.Basic.NoCreateMaterialUnreferenced = (LibraryEditor_SpriteStudio6.Import.Setting.GroupBasic.KindNoCreateMaterialUnreferenced)(EditorGUILayout.Popup(string.Empty, (int)SettingImport.Basic.NoCreateMaterialUnreferenced, NameNoCreateUnreferencedMaterial));
#endif
				EditorGUI.indentLevel = levelIndent + 1;
				EditorGUILayout.LabelField("Materials unreferenced from all SSAE and SSEE are not created.");
				EditorGUILayout.LabelField("Existing materials are remained.");
				EditorGUILayout.LabelField("Check Blending: Determine only from parts' \"Blending Mode\".");
				EditorGUILayout.LabelField("Check Blending and CellMap: Determine from parts' \"Blending Mode\" and \"Reference Cell\".");
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();

				if(LibraryEditor_SpriteStudio6.Import.Setting.GroupBasic.KindNoCreateMaterialUnreferenced.NONE != SettingImport.Basic.NoCreateMaterialUnreferenced)
				{
					SettingImport.Basic.FlagDeleteMaterialUnreferenced = EditorGUILayout.ToggleLeft("Delete unreferenced Materials", SettingImport.Basic.FlagDeleteMaterialUnreferenced);
					EditorGUI.indentLevel = levelIndent + 1;
					EditorGUILayout.LabelField("Materials unreferenced from created prefabs are deleted.");
					EditorGUILayout.LabelField("Existing materials are also deleted.");
					EditorGUI.indentLevel = levelIndent;
					EditorGUILayout.Space();
				}

				SettingImport.Basic.FlagDisableInitialLightRenderer = EditorGUILayout.ToggleLeft("Set disable Renderer's lighting", SettingImport.Basic.FlagDisableInitialLightRenderer);
				SettingImport.Basic.FlagTakeOverLightRenderer = EditorGUILayout.ToggleLeft("Take over Renderer's light setting", SettingImport.Basic.FlagTakeOverLightRenderer);
				EditorGUI.indentLevel = levelIndent + 1;
				EditorGUILayout.LabelField("When check \"Set disable renderer's lighting\", set no-lighting-effect to MeshRenderer.");
				EditorGUILayout.LabelField("When \"Take over renderer's light setting\" is checked, MeshRenderer's lighting");
				EditorGUILayout.LabelField("   setting are taken over to the data overwritten.");
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();

				break;

			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.UNITY_NATIVE:
				SettingImport.Basic.FlagCreateHolderAsset = EditorGUILayout.ToggleLeft("Create Asset-Holder", SettingImport.Basic.FlagCreateHolderAsset);
				EditorGUI.indentLevel = levelIndent + 1;
				EditorGUILayout.LabelField("Add \"Asset-Holder\" script of used asset (AnimationClip etc).");
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();
				break;

			default:
				break;
		}
	}

	private void FoldOutExecPreCalcualation(int levelIndent)
	{	/* MEMO: only "SS6PU" Mode */
		EditorGUI.indentLevel = levelIndent;

		SettingImport.PreCalcualation.FlagTrimTransparentPixelsCell = EditorGUILayout.ToggleLeft("Trim transparent-pixels", SettingImport.PreCalcualation.FlagTrimTransparentPixelsCell);
		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.LabelField("Adjust the cells' size so that transparent-pixels are not drawn as possible.");
		EditorGUILayout.LabelField("CAUTION: Some animation's attributes may not produce the intended result.");
		EditorGUI.indentLevel = levelIndent;
		EditorGUILayout.Space();
	}

	private void FoldOutExecCaution(int levelIndent)
	{	/* MEMO: only "Unity-Native" Mode */
		EditorGUI.indentLevel = levelIndent;
		EditorGUILayout.LabelField("In this mode, only the following Animation-Part-Types can be used.");
		EditorGUILayout.LabelField("(Other Part-Types are ignored)");

		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.LabelField("- root");
		EditorGUILayout.LabelField("- NULL");
		EditorGUILayout.LabelField("- Normal");
		EditorGUILayout.LabelField("- Mask");
		EditorGUILayout.LabelField("- Mesh");
		EditorGUI.indentLevel = levelIndent;

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("In this mode, only the following Animation-Attributes can be used.");
		EditorGUILayout.LabelField("(Other Attributes are ignored)");

		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.LabelField("- Reference Cell");
		EditorGUILayout.LabelField("- X Position");
		EditorGUILayout.LabelField("- Y Position");
		EditorGUILayout.LabelField("- Z Axis Rotation");
		EditorGUILayout.LabelField("- X Scale");
		EditorGUILayout.LabelField("- Y Scale");
		EditorGUILayout.LabelField("- X Local Scale (Ignored for \"Mask\" parts)");
		EditorGUILayout.LabelField("- Y Local Scale (Ignored for \"Mask\" parts)");
		EditorGUILayout.LabelField("- Opacity");
		EditorGUILayout.LabelField("- Local Opacity");
		EditorGUILayout.LabelField("- Priority");
		EditorGUILayout.LabelField("- Hide");
		EditorGUILayout.LabelField("- Parts Color");
		EditorGUILayout.LabelField("- Vertex Deformation (Ignored for \"Mask\" parts)");
		EditorGUILayout.LabelField("- Mask Power");
		EditorGUILayout.LabelField("- User Data (Only \"Numeric\" and \"String\" are valid)");
		EditorGUI.indentLevel = levelIndent;

		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}

	private void FoldOutExecCollision(int levelIndent)
	{
		EditorGUI.indentLevel = levelIndent;

		SettingImport.Collider.FlagAttachCollider = EditorGUILayout.ToggleLeft("Attach Collider", SettingImport.Collider.FlagAttachCollider);
		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.LabelField("When a collision is set for part, \"Collider\" component is added to the corresponding GameObjet.");
		EditorGUILayout.Space();
		EditorGUI.indentLevel = levelIndent;

		SettingImport.Collider.FlagAttachRigidBody = EditorGUILayout.ToggleLeft("Attach Rigid-Body", SettingImport.Collider.FlagAttachRigidBody);
		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.LabelField("Add \"Rigid-Body\" component when \"Collider\" component is attached to the GameObject of part.");
		EditorGUILayout.Space();
		EditorGUI.indentLevel = levelIndent;

		SettingImport.Collider.FlagIsTrigger = EditorGUILayout.ToggleLeft("Set \"Is Trigger\"", SettingImport.Collider.FlagIsTrigger);
		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.Space();
		EditorGUI.indentLevel = levelIndent;

		SettingImport.Collider.SizeZ = EditorGUILayout.FloatField("Collider Size-Z", SettingImport.Collider.SizeZ);
		EditorGUI.indentLevel = levelIndent + 1;
		EditorGUILayout.LabelField("\"Collider\"'s size of local Z-axis.");
		EditorGUILayout.Space();

		EditorGUI.indentLevel = levelIndent;
	}

	private void FoldOutExecConfirmOverWrite(int levelIndent)
	{
		EditorGUI.indentLevel = levelIndent;

		EditorGUILayout.LabelField("Confirm when overwrite existing data.");
		EditorGUILayout.Space();

		SettingImport.ConfirmOverWrite.FlagPrefabAnimation = EditorGUILayout.ToggleLeft("Prefab-Animation", SettingImport.ConfirmOverWrite.FlagPrefabAnimation);
		SettingImport.ConfirmOverWrite.FlagPrefabEffect = EditorGUILayout.ToggleLeft("Prefab-Effect", SettingImport.ConfirmOverWrite.FlagPrefabEffect);
		EditorGUILayout.Space();

		SettingImport.ConfirmOverWrite.FlagDataCellMap = EditorGUILayout.ToggleLeft("Data-CellMap", SettingImport.ConfirmOverWrite.FlagDataCellMap);
		SettingImport.ConfirmOverWrite.FlagDataAnimation = EditorGUILayout.ToggleLeft("Data-Animation", SettingImport.ConfirmOverWrite.FlagDataAnimation);
		SettingImport.ConfirmOverWrite.FlagDataEffect = EditorGUILayout.ToggleLeft("Data-Effect", SettingImport.ConfirmOverWrite.FlagDataEffect);
		EditorGUILayout.Space();

		SettingImport.ConfirmOverWrite.FlagMaterialAnimation = EditorGUILayout.ToggleLeft("Materials for Animation", SettingImport.ConfirmOverWrite.FlagMaterialAnimation);
		SettingImport.ConfirmOverWrite.FlagMaterialEffect = EditorGUILayout.ToggleLeft("Materials for Effect", SettingImport.ConfirmOverWrite.FlagMaterialEffect);
		EditorGUILayout.Space();

		SettingImport.ConfirmOverWrite.FlagTexture = EditorGUILayout.ToggleLeft("Textures", SettingImport.ConfirmOverWrite.FlagTexture);
		EditorGUILayout.Space();
	}

	private void FoldOutExecCheckVersion(int levelIndent)
	{
		EditorGUI.indentLevel = levelIndent;
		EditorGUILayout.LabelField("Warn on unsupported \"SpriteStudio\" data version.");
		EditorGUILayout.LabelField("The too early version is decoded as supported earliest version.");
		EditorGUILayout.LabelField("Otherwise, decoded as supported latest version.");
		EditorGUILayout.Space();
		EditorGUI.indentLevel = levelIndent;

		SettingImport.CheckVersion.FlagInvalidSSPJ = EditorGUILayout.ToggleLeft("SSPJ (Project)", SettingImport.CheckVersion.FlagInvalidSSPJ);
		SettingImport.CheckVersion.FlagInvalidSSCE = EditorGUILayout.ToggleLeft("SSCE (CellMap)", SettingImport.CheckVersion.FlagInvalidSSCE);
		SettingImport.CheckVersion.FlagInvalidSSAE = EditorGUILayout.ToggleLeft("SSAE (Animation)", SettingImport.CheckVersion.FlagInvalidSSAE);
		SettingImport.CheckVersion.FlagInvalidSSEE = EditorGUILayout.ToggleLeft("SSEE (Effect)", SettingImport.CheckVersion.FlagInvalidSSEE);
		EditorGUILayout.Space();
		EditorGUI.indentLevel = levelIndent;
	}

	private void FoldOutExecRuleNameAsset(int levelIndent)
	{
		EditorGUI.indentLevel = levelIndent;

		EditorGUILayout.LabelField("So that assets' name does not conflict when stored in AssetBundle, add the data identification string.");
		EditorGUILayout.LabelField("Changes in name when each option is set are displayed in \"State of Assets-Naming\".");
		EditorGUILayout.Space();

		SettingImport.RuleNameAsset.FlagAttachSpecificNameSSPJ = EditorGUILayout.ToggleLeft("Add SSPJ-Name", SettingImport.RuleNameAsset.FlagAttachSpecificNameSSPJ);
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("For each type to output, you can specify prefix to asset name.");
		EditorGUILayout.LabelField("The prohibited characters are \":\", \"/\", \"\\\", \".\", \"*\", \"?\", Space and Tab.");
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("- Asset-Name Prifix (Common)");
		EditorGUI.indentLevel = levelIndent + 1;
		SettingImport.RuleNameAsset.NamePrefixTexture = EditorGUILayout.TextField("Texture", SettingImport.RuleNameAsset.NamePrefixTexture);
		EditorGUI.indentLevel = levelIndent;
		EditorGUILayout.Space();

		switch(SettingImport.Mode)
		{
			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.SS6PU:
				EditorGUILayout.LabelField("- Asset-Name Prifix (Mode \"SS6Player for Unity\")");
				EditorGUI.indentLevel = levelIndent + 1;
				SettingImport.RuleNameAsset.NamePrefixPrefabAnimationSS6PU = EditorGUILayout.TextField("Prefab-Animation", SettingImport.RuleNameAsset.NamePrefixPrefabAnimationSS6PU);
				SettingImport.RuleNameAsset.NamePrefixPrefabEffectSS6PU = EditorGUILayout.TextField("Prefab-Effect", SettingImport.RuleNameAsset.NamePrefixPrefabEffectSS6PU);
				SettingImport.RuleNameAsset.NamePrefixDataCellMapSS6PU = EditorGUILayout.TextField("Data-CellMap", SettingImport.RuleNameAsset.NamePrefixDataCellMapSS6PU);
				SettingImport.RuleNameAsset.NamePrefixDataAnimationSS6PU = EditorGUILayout.TextField("Data-Animation", SettingImport.RuleNameAsset.NamePrefixDataAnimationSS6PU);
				SettingImport.RuleNameAsset.NamePrefixDataEffectSS6PU = EditorGUILayout.TextField("Data-Effect", SettingImport.RuleNameAsset.NamePrefixDataEffectSS6PU);
				SettingImport.RuleNameAsset.NamePrefixMaterialAnimationSS6PU = EditorGUILayout.TextField("Material-Animation", SettingImport.RuleNameAsset.NamePrefixMaterialAnimationSS6PU);
				SettingImport.RuleNameAsset.NamePrefixMaterialEffectSS6PU = EditorGUILayout.TextField("Material-Effect", SettingImport.RuleNameAsset.NamePrefixMaterialEffectSS6PU);
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();

				EditorGUILayout.LabelField("- Asset-Name Prifix (Mode \"Unity-Native\")");
				EditorGUI.indentLevel = levelIndent + 1;
				EditorGUILayout.LabelField("Prefab-Sprite2D", SettingImport.RuleNameAsset.NamePrefixPrefabAnimatorUnityNative);
				EditorGUILayout.LabelField("Prefab-Particle", SettingImport.RuleNameAsset.NamePrefixPrefabParticleUnityNative);
				EditorGUILayout.LabelField("Animation-Clip", SettingImport.RuleNameAsset.NamePrefixAnimationClipUnityNative);
				EditorGUILayout.LabelField("Skinned-Mesh", SettingImport.RuleNameAsset.NamePrefixSkinnedMeshUnityNative);
				EditorGUILayout.LabelField("Material-Sprite2D", SettingImport.RuleNameAsset.NamePrefixMaterialAnimatorUnityNative);
				EditorGUILayout.LabelField("Material-Particle", SettingImport.RuleNameAsset.NamePrefixMaterialParticleUnityNative);
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();

				SettingImport.RuleNameAsset.Adjust();

				SettingOption.ModeSS6PU.FlagFoldOutRuleNameAssetSample = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagFoldOutRuleNameAssetSample, "State of Assets-Naming");
				if(true == SettingOption.ModeSS6PU.FlagFoldOutRuleNameAssetSample)
				{
					EditorGUI.indentLevel = levelIndent + 1;

					EditorGUILayout.LabelField("Prefab-Animation: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_SS6PU, NameAssetBody, NameAssetSSPJ));
					EditorGUILayout.LabelField("Prefab-Effect: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_EFFECT_SS6PU, NameAssetBody, NameAssetSSPJ));
					EditorGUILayout.Space();

					EditorGUILayout.LabelField("Data-CellMap: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_CELLMAP_SS6PU, NameAssetBody, NameAssetSSPJ));
					EditorGUILayout.LabelField("Data-Animation: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_SS6PU, NameAssetBody, NameAssetSSPJ));
					EditorGUILayout.LabelField("Data-Effect: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_EFFECT_SS6PU, NameAssetBody, NameAssetSSPJ));
					EditorGUILayout.Space();

					EditorGUILayout.LabelField("Material-Animation: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_ANIMATION_SS6PU, NameAssetBody, NameAssetSSPJ));
					EditorGUILayout.LabelField("Material-Effect: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_EFFECT_SS6PU, NameAssetBody, NameAssetSSPJ));
					EditorGUILayout.Space();

					EditorGUILayout.LabelField("Texture: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.TEXTURE, NameAssetBody, NameAssetSSPJ));

					EditorGUI.indentLevel = levelIndent;
				}
				break;
			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.UNITY_NATIVE:
				EditorGUILayout.LabelField("- Asset-Name Prifix (Mode \"SS6Player for Unity\")");
				EditorGUI.indentLevel = levelIndent + 1;
				EditorGUILayout.LabelField("Prefab-Animation", SettingImport.RuleNameAsset.NamePrefixPrefabAnimationSS6PU);
				EditorGUILayout.LabelField("Prefab-Effect", SettingImport.RuleNameAsset.NamePrefixPrefabEffectSS6PU);
				EditorGUILayout.LabelField("Data-CellMap", SettingImport.RuleNameAsset.NamePrefixDataCellMapSS6PU);
				EditorGUILayout.LabelField("Data-Animation", SettingImport.RuleNameAsset.NamePrefixDataAnimationSS6PU);
				EditorGUILayout.LabelField("Data-Effect", SettingImport.RuleNameAsset.NamePrefixDataEffectSS6PU);
				EditorGUILayout.LabelField("Material-Animation", SettingImport.RuleNameAsset.NamePrefixMaterialAnimationSS6PU);
				EditorGUILayout.LabelField("Material-Effect", SettingImport.RuleNameAsset.NamePrefixMaterialEffectSS6PU);
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();

				EditorGUILayout.LabelField("- Asset-Name Prifix (Mode \"Unity-Native\")");
				EditorGUI.indentLevel = levelIndent + 1;
				SettingImport.RuleNameAsset.NamePrefixPrefabAnimatorUnityNative = EditorGUILayout.TextField("Prefab-Sprite2D", SettingImport.RuleNameAsset.NamePrefixPrefabAnimatorUnityNative);
				SettingImport.RuleNameAsset.NamePrefixPrefabParticleUnityNative = EditorGUILayout.TextField("Prefab-Particle", SettingImport.RuleNameAsset.NamePrefixPrefabParticleUnityNative);
				SettingImport.RuleNameAsset.NamePrefixAnimationClipUnityNative = EditorGUILayout.TextField("Animation-Clip", SettingImport.RuleNameAsset.NamePrefixAnimationClipUnityNative);
				SettingImport.RuleNameAsset.NamePrefixSkinnedMeshUnityNative = EditorGUILayout.TextField("Skinned-Mesh", SettingImport.RuleNameAsset.NamePrefixSkinnedMeshUnityNative);
				SettingImport.RuleNameAsset.NamePrefixMaterialAnimatorUnityNative = EditorGUILayout.TextField("Material-Sprite2D", SettingImport.RuleNameAsset.NamePrefixMaterialAnimatorUnityNative);
				SettingImport.RuleNameAsset.NamePrefixMaterialParticleUnityNative = EditorGUILayout.TextField("Material-Particle", SettingImport.RuleNameAsset.NamePrefixMaterialParticleUnityNative);
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();

				SettingImport.RuleNameAsset.Adjust();

				SettingOption.ModeUnityNative.FlagFoldOutRuleNameAssetSample = EditorGUILayout.Foldout(SettingOption.ModeUnityNative.FlagFoldOutRuleNameAssetSample, "State of Assets-Naming");
				if(true == SettingOption.ModeUnityNative.FlagFoldOutRuleNameAssetSample)
				{
					EditorGUI.indentLevel = levelIndent + 1;

					EditorGUILayout.LabelField("Prefab-Sprite2D: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_UNITYNATIVE, NameAssetBody, NameAssetSSPJ));
					EditorGUILayout.LabelField("Prefab-Particle: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_EFFECT_UNITYNATIVE, NameAssetBody, NameAssetSSPJ));
					EditorGUILayout.Space();

					EditorGUILayout.LabelField("Animation-Clip: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_UNITYNATIVE, NameAssetBodyUnityNative, NameAssetSSPJ));
					EditorGUILayout.LabelField("Skinned-Mesh: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_MESH_UNITYNATIVE, NameAssetBody, NameAssetSSPJ) + "_" + NameAssetPartsUnityNative);
					EditorGUILayout.Space();

					EditorGUILayout.LabelField("Material-Sprite2D: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_ANIMATION_UNITYNATIVE, NameAssetBody, NameAssetSSPJ));
					EditorGUILayout.LabelField("Material-Particle: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_EFFECT_UNITYNATIVE, NameAssetBody, NameAssetSSPJ));
					EditorGUILayout.Space();

					EditorGUILayout.LabelField("Texture: " + SettingImport.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.TEXTURE, NameAssetBody, NameAssetSSPJ));

					EditorGUI.indentLevel = levelIndent;
				}
				break;

			default:
				break;
		}
		EditorGUILayout.Space();
	}

	private void FoldOutExecRuleNameAssetFolder(int levelIndent)
	{
		EditorGUI.indentLevel = levelIndent;

		EditorGUILayout.LabelField("For each type to output, you can specify name of Asset-Folder.");
		EditorGUILayout.LabelField("The prohibited characters are \":\", \"/\", \"\\\", \".\", \"*\", \"?\", Space and Tab.");
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("- Asset Folder Name (Common)");
		EditorGUI.indentLevel = levelIndent + 1;
		SettingImport.RuleNameAssetFolder.NameFolderTexture = EditorGUILayout.TextField("Texture", SettingImport.RuleNameAssetFolder.NameFolderTexture);
		EditorGUI.indentLevel = levelIndent;
		EditorGUILayout.Space();

		switch(SettingImport.Mode)
		{
			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.SS6PU:
				EditorGUILayout.LabelField("- Asset Folder Name (Mode \"SS6Player for Unity\")");
				EditorGUI.indentLevel = levelIndent + 1;
				SettingImport.RuleNameAssetFolder.NameFolderPrefabAnimationSS6PU = EditorGUILayout.TextField("Prefab-Animation", SettingImport.RuleNameAssetFolder.NameFolderPrefabAnimationSS6PU);
				SettingImport.RuleNameAssetFolder.NameFolderPrefabEffectSS6PU = EditorGUILayout.TextField("Prefab-Effect", SettingImport.RuleNameAssetFolder.NameFolderPrefabEffectSS6PU);
				SettingImport.RuleNameAssetFolder.NameFolderDataCellMapSS6PU = EditorGUILayout.TextField("Data-CellMap", SettingImport.RuleNameAssetFolder.NameFolderDataCellMapSS6PU);
				SettingImport.RuleNameAssetFolder.NameFolderDataAnimationSS6PU = EditorGUILayout.TextField("Data-Animation", SettingImport.RuleNameAssetFolder.NameFolderDataAnimationSS6PU);
				SettingImport.RuleNameAssetFolder.NameFolderDataEffectSS6PU = EditorGUILayout.TextField("Data-Effect", SettingImport.RuleNameAssetFolder.NameFolderDataEffectSS6PU);
				SettingImport.RuleNameAssetFolder.NameFolderMaterialAnimationSS6PU = EditorGUILayout.TextField("Material-Animation", SettingImport.RuleNameAssetFolder.NameFolderMaterialAnimationSS6PU);
				SettingImport.RuleNameAssetFolder.NameFolderMaterialEffectSS6PU = EditorGUILayout.TextField("Material-Effect", SettingImport.RuleNameAssetFolder.NameFolderMaterialEffectSS6PU);
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();

				EditorGUILayout.LabelField("- Asset Folder Name (Mode \"Unity-Native\")");
				EditorGUI.indentLevel = levelIndent + 1;
				EditorGUILayout.LabelField("Prefab-Sprite2D", SettingImport.RuleNameAssetFolder.NameFolderPrefabAnimatorUnityNative);
				EditorGUILayout.LabelField("Prefab-Particle", SettingImport.RuleNameAssetFolder.NameFolderPrefabParticleUnityNative);
				EditorGUILayout.LabelField("Animation-Clip", SettingImport.RuleNameAssetFolder.NameFolderAnimationClipUnityNative);
				EditorGUILayout.LabelField("Skinned-Mesh", SettingImport.RuleNameAssetFolder.NameFolderSkinnedMeshUnityNative);
				EditorGUILayout.LabelField("Material-Sprite2D", SettingImport.RuleNameAssetFolder.NameFolderMaterialAnimatorUnityNative);
				EditorGUILayout.LabelField("Material-Particle", SettingImport.RuleNameAssetFolder.NameFolderMaterialParticleUnityNative);
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();

				SettingImport.RuleNameAsset.Adjust();
				break;

			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.UNITY_NATIVE:
				EditorGUILayout.LabelField("- Asset-Name Prifix (Mode \"SS6Player for Unity\")");
				EditorGUI.indentLevel = levelIndent + 1;
				EditorGUILayout.LabelField("Prefab-Animation", SettingImport.RuleNameAssetFolder.NameFolderPrefabAnimationSS6PU);
				EditorGUILayout.LabelField("Prefab-Effect", SettingImport.RuleNameAssetFolder.NameFolderPrefabEffectSS6PU);
				EditorGUILayout.LabelField("Data-CellMap", SettingImport.RuleNameAssetFolder.NameFolderDataCellMapSS6PU);
				EditorGUILayout.LabelField("Data-Animation", SettingImport.RuleNameAssetFolder.NameFolderDataAnimationSS6PU);
				EditorGUILayout.LabelField("Data-Effect", SettingImport.RuleNameAssetFolder.NameFolderDataEffectSS6PU);
				EditorGUILayout.LabelField("Material-Animation", SettingImport.RuleNameAssetFolder.NameFolderMaterialAnimationSS6PU);
				EditorGUILayout.LabelField("Material-Effect", SettingImport.RuleNameAssetFolder.NameFolderMaterialEffectSS6PU);
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();

				EditorGUILayout.LabelField("- Asset-Name Prifix (Mode \"Unity-Native\")");
				EditorGUI.indentLevel = levelIndent + 1;
				SettingImport.RuleNameAssetFolder.NameFolderPrefabAnimatorUnityNative = EditorGUILayout.TextField("Prefab-Sprite2D", SettingImport.RuleNameAssetFolder.NameFolderPrefabAnimatorUnityNative);
				SettingImport.RuleNameAssetFolder.NameFolderPrefabParticleUnityNative = EditorGUILayout.TextField("Prefab-Particle", SettingImport.RuleNameAssetFolder.NameFolderPrefabParticleUnityNative);
				SettingImport.RuleNameAssetFolder.NameFolderAnimationClipUnityNative = EditorGUILayout.TextField("Animation-Clip", SettingImport.RuleNameAssetFolder.NameFolderAnimationClipUnityNative);
				SettingImport.RuleNameAssetFolder.NameFolderSkinnedMeshUnityNative = EditorGUILayout.TextField("Skinned-Mesh", SettingImport.RuleNameAssetFolder.NameFolderSkinnedMeshUnityNative);
				SettingImport.RuleNameAssetFolder.NameFolderMaterialAnimatorUnityNative = EditorGUILayout.TextField("Material-Sprite2D", SettingImport.RuleNameAssetFolder.NameFolderMaterialAnimatorUnityNative);
				SettingImport.RuleNameAssetFolder.NameFolderMaterialParticleUnityNative = EditorGUILayout.TextField("Material-Particle", SettingImport.RuleNameAssetFolder.NameFolderMaterialParticleUnityNative);
				EditorGUI.indentLevel = levelIndent;
				EditorGUILayout.Space();
				break;

			default:
				break;
		}
		EditorGUILayout.Space();
	}

	private void FoldOutExecPackAttributeAnimation(int levelIndent)
	{	/* MEMO: only "SS6PU" Mode */
		EditorGUI.indentLevel = levelIndent;
		EditorGUILayout.LabelField("Don't manipulate this setting without reason.");
		EditorGUILayout.LabelField("Understand amply the implementation of SS6PU data format before setting.");
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("\"RateOpacity\" is shared for \"RateOpacity\", \"RateOpacityLocal\" and \"PowerMask\".");
		EditorGUILayout.Space();

		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.Status, "Status", ref PullDownPackAttributeAnimation.Status);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.Cell, "Cell", ref PullDownPackAttributeAnimation.Cell);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.Position, "Position", ref PullDownPackAttributeAnimation.Position);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.Rotation, "Rotation", ref PullDownPackAttributeAnimation.Rotation);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.Scaling, "Scaling", ref PullDownPackAttributeAnimation.Scaling);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.ScalingLocal, "ScalingLocal", ref PullDownPackAttributeAnimation.ScalingLocal);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.RateOpacity, "RateOpacity", ref PullDownPackAttributeAnimation.RateOpacity);
		if(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.CPE_INTERPOLATE == SettingImport.PackAttributeAnimation.RateOpacity)
		{
			EditorGUI.indentLevel = levelIndent + 1;
			EditorGUILayout.LabelField("Only when packing \"RateOpacity\", \"Standard CPE\" is used.");
			EditorGUI.indentLevel = levelIndent;
		}
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.Priority, "Priority", ref PullDownPackAttributeAnimation.Priority);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.PartsColor, "PartsColor", ref PullDownPackAttributeAnimation.PartsColor);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.VertexCorrection, "VertexCorrection", ref PullDownPackAttributeAnimation.VertexCorrection);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.OffsetPivot, "OffsetPivot", ref PullDownPackAttributeAnimation.OffsetPivot);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.PositionAnchor, "PositionAnchor", ref PullDownPackAttributeAnimation.PositionAnchor);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.SizeForce, "SizeForce", ref PullDownPackAttributeAnimation.SizeForce);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.PositionTexture, "PositionTexture", ref PullDownPackAttributeAnimation.PositionTexture);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.RotationTexture, "RotationTexture", ref PullDownPackAttributeAnimation.RotationTexture);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.ScalingTexture, "ScalingTexture", ref PullDownPackAttributeAnimation.ScalingTexture);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.RadiusCollision, "RadiusCollision", ref PullDownPackAttributeAnimation.RadiusCollision);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.UserData, "UserData", ref PullDownPackAttributeAnimation.UserData);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.Instance, "Instance", ref PullDownPackAttributeAnimation.Instance);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.Effect, "Effect", ref PullDownPackAttributeAnimation.Effect);
		PullDownExecPackAttributeAnimationPart(ref SettingImport.PackAttributeAnimation.Deform, "Deform", ref PullDownPackAttributeAnimation.Deform);
		EditorGUILayout.Space();
	}
	private void PullDownExecPackAttributeAnimationPart(ref Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack, string message, ref PullDownPackAttribute.Attribute dataPopup)
	{
		if(0 >= dataPopup.TableKindTypePack.Length)
		{
			EditorGUILayout.LabelField(message + ": ERROR");
		}
		else
		{
			pack = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorGUILayout.IntPopup(message, (int)pack, dataPopup.TableName, dataPopup.TableKindTypePack));
		}
	}

	private void FoldOutExecPresetMaterial(int levelIndent)
	{
		switch(SettingImport.Mode)
		{
			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.SS6PU:
				EditorGUI.indentLevel = levelIndent;

				SettingOption.ModeSS6PU.FlagFoldOutPresetMaterialThrough = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagFoldOutPresetMaterialThrough, "for parts Not-Masked");
				if(true == SettingOption.ModeSS6PU.FlagFoldOutPresetMaterialThrough)
				{
					EditorGUI.indentLevel = levelIndent + 1;
					EditorGUILayout.LabelField("Set each blend-operation's materials.");
					EditorGUILayout.LabelField("(Materials will be duplicated for each importing SSPJ data.)");
					EditorGUILayout.Space();

					SettingImport.PresetMaterial.AnimationSS6PUThroughStencilPreDraw = EditorGUILayout.ObjectField("[Mask]Pre-Draw", SettingImport.PresetMaterial.AnimationSS6PUThroughStencilPreDraw, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUThroughStencilDraw = EditorGUILayout.ObjectField("[Mask]Draw", SettingImport.PresetMaterial.AnimationSS6PUThroughStencilDraw, typeof(Material), false) as Material;
					EditorGUILayout.Space();

					SettingImport.PresetMaterial.AnimationSS6PUThroughMix = EditorGUILayout.ObjectField("[Sprite]Mix", SettingImport.PresetMaterial.AnimationSS6PUThroughMix, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUThroughAdd = EditorGUILayout.ObjectField("[Sprite]Add", SettingImport.PresetMaterial.AnimationSS6PUThroughAdd, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUThroughSub = EditorGUILayout.ObjectField("[Sprite]Sub", SettingImport.PresetMaterial.AnimationSS6PUThroughSub, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUThroughMul = EditorGUILayout.ObjectField("[Sprite]Mul", SettingImport.PresetMaterial.AnimationSS6PUThroughMul, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUThroughMulNA = EditorGUILayout.ObjectField("[Sprite]MulNA", SettingImport.PresetMaterial.AnimationSS6PUThroughMulNA, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUThroughScr = EditorGUILayout.ObjectField("[Sprite]Scr", SettingImport.PresetMaterial.AnimationSS6PUThroughScr, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUThroughExc = EditorGUILayout.ObjectField("[Sprite]Exc", SettingImport.PresetMaterial.AnimationSS6PUThroughExc, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUThroughInv = EditorGUILayout.ObjectField("[Sprite]Inv", SettingImport.PresetMaterial.AnimationSS6PUThroughInv, typeof(Material), false) as Material;
					EditorGUILayout.Space();
					SettingImport.PresetMaterial.EffectSS6PUThroughMix = EditorGUILayout.ObjectField("[Effect]Mix", SettingImport.PresetMaterial.EffectSS6PUThroughMix, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.EffectSS6PUThroughAdd = EditorGUILayout.ObjectField("[Effect]Add", SettingImport.PresetMaterial.EffectSS6PUThroughAdd, typeof(Material), false) as Material;
					EditorGUILayout.Space();
					EditorGUI.indentLevel = levelIndent;
				}

				SettingOption.ModeSS6PU.FlagFoldOutPresetMaterialMask = EditorGUILayout.Foldout(SettingOption.ModeSS6PU.FlagFoldOutPresetMaterialMask, "for parts Masked");
				if(true == SettingOption.ModeSS6PU.FlagFoldOutPresetMaterialMask)
				{
					EditorGUI.indentLevel = levelIndent + 1;
					EditorGUILayout.LabelField("Set each blend-operation's materials.");
					EditorGUILayout.LabelField("(Materials will be duplicated for each importing SSPJ data.)");
					EditorGUILayout.Space();

					SettingImport.PresetMaterial.AnimationSS6PUMaskStencilPreDraw = EditorGUILayout.ObjectField("[Mask]Pre-Draw", SettingImport.PresetMaterial.AnimationSS6PUMaskStencilPreDraw, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUMaskStencilDraw = EditorGUILayout.ObjectField("[Mask]Draw", SettingImport.PresetMaterial.AnimationSS6PUMaskStencilDraw, typeof(Material), false) as Material;
					EditorGUILayout.Space();

					SettingImport.PresetMaterial.AnimationSS6PUMaskMix = EditorGUILayout.ObjectField("[Sprite]Mix", SettingImport.PresetMaterial.AnimationSS6PUMaskMix, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUMaskAdd = EditorGUILayout.ObjectField("[Sprite]Add", SettingImport.PresetMaterial.AnimationSS6PUMaskAdd, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUMaskSub = EditorGUILayout.ObjectField("[Sprite]Sub", SettingImport.PresetMaterial.AnimationSS6PUMaskSub, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUMaskMul = EditorGUILayout.ObjectField("[Sprite]Mul", SettingImport.PresetMaterial.AnimationSS6PUMaskMul, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUMaskMulNA = EditorGUILayout.ObjectField("[Sprite]MulNA", SettingImport.PresetMaterial.AnimationSS6PUMaskMulNA, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUMaskScr = EditorGUILayout.ObjectField("[Sprite]Scr", SettingImport.PresetMaterial.AnimationSS6PUMaskScr, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUMaskExc = EditorGUILayout.ObjectField("[Sprite]Exc", SettingImport.PresetMaterial.AnimationSS6PUMaskExc, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.AnimationSS6PUMaskInv = EditorGUILayout.ObjectField("[Sprite]Inv", SettingImport.PresetMaterial.AnimationSS6PUMaskInv, typeof(Material), false) as Material;
					EditorGUILayout.Space();
					SettingImport.PresetMaterial.EffectSS6PUMaskMix = EditorGUILayout.ObjectField("[Effect]Mix", SettingImport.PresetMaterial.EffectSS6PUMaskMix, typeof(Material), false) as Material;
					SettingImport.PresetMaterial.EffectSS6PUMaskAdd = EditorGUILayout.ObjectField("[Effect]Add", SettingImport.PresetMaterial.EffectSS6PUMaskAdd, typeof(Material), false) as Material;
					EditorGUILayout.Space();
					EditorGUI.indentLevel = levelIndent;
				}
				EditorGUILayout.Space();
				break;

			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.UNITY_NATIVE:
				EditorGUI.indentLevel = levelIndent;

				EditorGUILayout.LabelField("Set each blend-operation's materials.");
				EditorGUILayout.Space();

				SettingImport.PresetMaterial.AnimationUnityNativeMix = EditorGUILayout.ObjectField("[Sprite]Mix", SettingImport.PresetMaterial.AnimationUnityNativeMix, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeAdd = EditorGUILayout.ObjectField("[Sprite]Add", SettingImport.PresetMaterial.AnimationUnityNativeAdd, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeSub = EditorGUILayout.ObjectField("[Sprite]Sub", SettingImport.PresetMaterial.AnimationUnityNativeSub, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeMul = EditorGUILayout.ObjectField("[Sprite]Mul", SettingImport.PresetMaterial.AnimationUnityNativeMul, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeMulNA = EditorGUILayout.ObjectField("[Sprite]MulNA", SettingImport.PresetMaterial.AnimationUnityNativeMulNA, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeScr = EditorGUILayout.ObjectField("[Sprite]Scr", SettingImport.PresetMaterial.AnimationUnityNativeScr, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeExc = EditorGUILayout.ObjectField("[Sprite]Exc", SettingImport.PresetMaterial.AnimationUnityNativeExc, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeInv = EditorGUILayout.ObjectField("[Sprite]Inv", SettingImport.PresetMaterial.AnimationUnityNativeInv, typeof(Material), false) as Material;
				EditorGUILayout.Space();
				SettingImport.PresetMaterial.AnimationUnityNativeNonBatchMix = EditorGUILayout.ObjectField("[Sprite]Mix", SettingImport.PresetMaterial.AnimationUnityNativeNonBatchMix, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeNonBatchAdd = EditorGUILayout.ObjectField("[Sprite]Add", SettingImport.PresetMaterial.AnimationUnityNativeNonBatchAdd, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeNonBatchSub = EditorGUILayout.ObjectField("[Sprite]Sub", SettingImport.PresetMaterial.AnimationUnityNativeNonBatchSub, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeNonBatchMul = EditorGUILayout.ObjectField("[Sprite]Mul", SettingImport.PresetMaterial.AnimationUnityNativeNonBatchMul, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeNonBatchMulNA = EditorGUILayout.ObjectField("[Sprite]MulNA", SettingImport.PresetMaterial.AnimationUnityNativeNonBatchMulNA, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeNonBatchScr = EditorGUILayout.ObjectField("[Sprite]Scr", SettingImport.PresetMaterial.AnimationUnityNativeNonBatchScr, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeNonBatchExc = EditorGUILayout.ObjectField("[Sprite]Exc", SettingImport.PresetMaterial.AnimationUnityNativeNonBatchExc, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.AnimationUnityNativeNonBatchInv = EditorGUILayout.ObjectField("[Sprite]Inv", SettingImport.PresetMaterial.AnimationUnityNativeNonBatchInv, typeof(Material), false) as Material;
				EditorGUILayout.Space();
				SettingImport.PresetMaterial.SkinnedMeshUnityNativeMix = EditorGUILayout.ObjectField("[Mesh]Mix", SettingImport.PresetMaterial.SkinnedMeshUnityNativeMix, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.SkinnedMeshUnityNativeAdd = EditorGUILayout.ObjectField("[Mesh]Add", SettingImport.PresetMaterial.SkinnedMeshUnityNativeAdd, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.SkinnedMeshUnityNativeSub = EditorGUILayout.ObjectField("[Mesh]Sub", SettingImport.PresetMaterial.SkinnedMeshUnityNativeSub, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.SkinnedMeshUnityNativeMul = EditorGUILayout.ObjectField("[Mesh]Mul", SettingImport.PresetMaterial.SkinnedMeshUnityNativeMul, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.SkinnedMeshUnityNativeMulNA = EditorGUILayout.ObjectField("[Mesh]MulNA", SettingImport.PresetMaterial.SkinnedMeshUnityNativeMulNA, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.SkinnedMeshUnityNativeScr = EditorGUILayout.ObjectField("[Mesh]Scr", SettingImport.PresetMaterial.SkinnedMeshUnityNativeScr, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.SkinnedMeshUnityNativeExc = EditorGUILayout.ObjectField("[Mesh]Exc", SettingImport.PresetMaterial.SkinnedMeshUnityNativeExc, typeof(Material), false) as Material;
				SettingImport.PresetMaterial.SkinnedMeshUnityNativeInv = EditorGUILayout.ObjectField("[Mesh]Inv", SettingImport.PresetMaterial.SkinnedMeshUnityNativeInv, typeof(Material), false) as Material;
				EditorGUILayout.Space();
				break;

			case LibraryEditor_SpriteStudio6.Import.Setting.KindMode.BATCH_IMPORTER:
			default:
				/* MEMO: Not reach here. */
				break;
		}
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	private readonly static string[] NameMode = new string[(int)LibraryEditor_SpriteStudio6.Import.Setting.KindMode.TERMINATOR + 1]
	{
		"SpriteStudio6 Player",
		"Convert To Unity-Native",

		"Batch Import",
	};
	private readonly static string[] NameNoCreateUnreferencedMaterial = new string[(int)LibraryEditor_SpriteStudio6.Import.Setting.GroupBasic.KindNoCreateMaterialUnreferenced.TERMINATOR] {
		"None (Create all)",
		"Check only Blending",
		"Check Blending and CellMaps",
	};

	private const string NameAssetBody = "(FileName-Body)";
	private const string NameAssetBodyUnityNative = "(FileName-Body)_(Animation-Name)";
	private const string NameAssetPartsUnityNative = "(Part-Name)";
	private const string NameAssetSSPJ = "(SSPJ-Name)";
	#endregion Enums & Constants

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	private struct Setting
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		public GroupSS6PU ModeSS6PU;
		public GroupUnityNative ModeUnityNative;
		public GroupBatchImporter ModeBatchImporter;

		public string NameFolderImportPrevious;

		public bool FlagFoldOutSettingBackUp;
		public GroupSettingBackUp SettingBackUp;
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public void CleanUp()
		{
			NameFolderImportPrevious = DefaultNameFolderImportPrevious;
			FlagFoldOutSettingBackUp = DefaultFlagFoldOutSettingBackUp;

			ModeSS6PU.CleanUp();
			ModeUnityNative.CleanUp();
			ModeBatchImporter.CleanUp();
			SettingBackUp.CleanUp();
		}

		public bool Load()
		{
			NameFolderImportPrevious = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderImportPrevious, DefaultNameFolderImportPrevious);
			FlagFoldOutSettingBackUp = EditorPrefs.GetBool(PrefsKeyFlagFoldOutSettingBackUp, DefaultFlagFoldOutSettingBackUp);

			ModeSS6PU.Load();
			ModeUnityNative.Load();
			ModeBatchImporter.Load();
			SettingBackUp.Load();

			return(true);
		}

		public bool Save()
		{
			LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderImportPrevious, NameFolderImportPrevious);
			EditorPrefs.SetBool(PrefsKeyFlagFoldOutSettingBackUp, FlagFoldOutSettingBackUp);

			ModeSS6PU.Save();
			ModeUnityNative.Save();
			ModeBatchImporter.Save();
			SettingBackUp.Save();

			return(true);
		}
		#endregion Functions

		/* ----------------------------------------------- Enums & Constants */
		#region Enums & Constants
		private const string PrefsKeyPrefix = "SS6PU_ToolImporter_";
		private const string PrefsKeyNameFolderImportPrevious = PrefsKeyPrefix + "NameFolderImportPrevious";
		private const string PrefsKeyFlagFoldOutSettingBackUp = PrefsKeyPrefix + "FlagFoldOutSettingBackUp";

		private const string DefaultNameFolderImportPrevious = "";
		private const bool DefaultFlagFoldOutSettingBackUp = false;
		#endregion Enums & Constants

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public struct GroupSS6PU
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public bool FlagFoldOutBasic;
			public bool FlagFoldOutPreCalcualation;
			public bool FlagFoldOutConfirmOverWrite;
			public bool FlagFoldOutCollider;
			public bool FlagFoldOutCheckVersion;

			public bool FlagOpenAdvancedOprions;
			public bool FlagFoldOutRuleNameAsset;
			public bool FlagFoldOutRuleNameAssetSample;
			public bool FlagFoldOutRuleNameAssetFolder;
			public bool FlagFoldOutPackAttributeAnimation;
			public bool FlagFoldOutPresetMaterial;
			public bool FlagFoldOutPresetMaterialThrough;
			public bool FlagFoldOutPresetMaterialMask;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public GroupSS6PU(	bool flagFoldOutBasic,
								bool flagFoldOutPreCalcualation,
								bool flagFoldOutConfirmOverWrite,
								bool flagFoldOutCollider,
								bool flagFoldOutCheckVersion,
								bool flagOpenAdvancedOprions,
								bool flagFoldOutRuleNameAsset,
								bool flagFoldOutRuleNameAssetSample,
								bool flagFoldOutRuleNameAssetFolder,
								bool flagFoldOutPackAttributeAnimation,
								bool flagFoldOutPresetMaterial,
								bool flagFoldOutPresetMaterialThrough,
								bool flagFoldOutPresetMaterialMask
							)
			{
				FlagFoldOutBasic = flagFoldOutBasic;
				FlagFoldOutPreCalcualation = flagFoldOutPreCalcualation;
				FlagFoldOutConfirmOverWrite = flagFoldOutConfirmOverWrite;
				FlagFoldOutCollider = flagFoldOutCollider;
				FlagFoldOutCheckVersion = flagFoldOutCheckVersion;

				FlagOpenAdvancedOprions = flagOpenAdvancedOprions;
				FlagFoldOutRuleNameAsset = flagFoldOutRuleNameAsset;
				FlagFoldOutRuleNameAssetSample = flagFoldOutRuleNameAssetSample;
				FlagFoldOutRuleNameAssetFolder = flagFoldOutRuleNameAssetFolder;
				FlagFoldOutPackAttributeAnimation = flagFoldOutPackAttributeAnimation;
				FlagFoldOutPresetMaterial = flagFoldOutPresetMaterial;
				FlagFoldOutPresetMaterialThrough = flagFoldOutPresetMaterialThrough;
				FlagFoldOutPresetMaterialMask = flagFoldOutPresetMaterialMask;
			}

			public void CleanUp()
			{
				this = Default;
			}

			public bool Load()
			{
				FlagFoldOutBasic = EditorPrefs.GetBool(PrefsKeyFlagFoldOutBasic, Default.FlagFoldOutBasic);
				FlagFoldOutPreCalcualation = EditorPrefs.GetBool(PrefsKeyFlagFoldOutPreCalcualation, Default.FlagFoldOutPreCalcualation);
				FlagFoldOutConfirmOverWrite = EditorPrefs.GetBool(PrefsKeyFlagFoldOutConfirmOverWrite, Default.FlagFoldOutConfirmOverWrite);
				FlagFoldOutCollider = EditorPrefs.GetBool(PrefsKeyFlagFoldOutCollider, Default.FlagFoldOutCollider);
				FlagFoldOutCheckVersion = EditorPrefs.GetBool(PrefsKeyFlagFoldOutCheckVersion, Default.FlagFoldOutCheckVersion);

				FlagOpenAdvancedOprions = EditorPrefs.GetBool(PrefsKeyFlagOpenAdvancedOprions, Default.FlagOpenAdvancedOprions);
				FlagFoldOutRuleNameAsset = EditorPrefs.GetBool(PrefsKeyFlagFoldOutRuleNameAsset, Default.FlagFoldOutRuleNameAsset);
				FlagFoldOutRuleNameAssetSample = EditorPrefs.GetBool(PrefsKeyFlagFoldOutRuleNameAssetSample, Default.FlagFoldOutRuleNameAssetSample);
				FlagFoldOutRuleNameAssetFolder = EditorPrefs.GetBool(PrefsKeyFlagFoldOutRuleNameAssetFolder, Default.FlagFoldOutRuleNameAssetFolder);
				FlagFoldOutPackAttributeAnimation = EditorPrefs.GetBool(PrefsKeyFlagFoldOutPackAttributeAnimation, Default.FlagFoldOutPackAttributeAnimation);
				FlagFoldOutPresetMaterial = EditorPrefs.GetBool(PrefsKeyFlagFoldFlagFoldOutPresetMaterial, Default.FlagFoldOutPresetMaterial);
				FlagFoldOutPresetMaterialThrough = EditorPrefs.GetBool(PrefsKeyFlagFlagFoldOutPresetMaterialThrough, Default.FlagFoldOutPresetMaterialThrough);
				FlagFoldOutPresetMaterialMask = EditorPrefs.GetBool(PrefsKeyFlagFlagFoldOutPresetMaterialMask, Default.FlagFoldOutPresetMaterialMask);

				return(true);
			}

			public bool Save()
			{
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutBasic, FlagFoldOutBasic);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutPreCalcualation, FlagFoldOutPreCalcualation);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutConfirmOverWrite, FlagFoldOutConfirmOverWrite);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutCollider, FlagFoldOutCollider);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutCheckVersion, FlagFoldOutCheckVersion);

				EditorPrefs.SetBool(PrefsKeyFlagOpenAdvancedOprions, FlagOpenAdvancedOprions);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutRuleNameAsset, FlagFoldOutRuleNameAsset);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutRuleNameAssetSample, FlagFoldOutRuleNameAssetSample);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutRuleNameAssetFolder, FlagFoldOutRuleNameAssetFolder);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutPackAttributeAnimation, FlagFoldOutPackAttributeAnimation);
				EditorPrefs.SetBool(PrefsKeyFlagFoldFlagFoldOutPresetMaterial, FlagFoldOutPresetMaterial);
				EditorPrefs.SetBool(PrefsKeyFlagFlagFoldOutPresetMaterialThrough, FlagFoldOutPresetMaterialThrough);
				EditorPrefs.SetBool(PrefsKeyFlagFlagFoldOutPresetMaterialMask, FlagFoldOutPresetMaterialMask);

				return(true);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			private const string PrefsKeyPrefix = "SS6PU_ToolImporter_SS6PU_";
			private const string PrefsKeyFlagFoldOutBasic = PrefsKeyPrefix + "FlagFoldOutBasic";
			private const string PrefsKeyFlagFoldOutPreCalcualation = PrefsKeyPrefix + "FlagFoldOutPreCalcualation";
			private const string PrefsKeyFlagFoldOutConfirmOverWrite = PrefsKeyPrefix + "FlagFoldOutConfirmOverWrite";
			private const string PrefsKeyFlagFoldOutCollider = PrefsKeyPrefix + "FlagFoldOutCollider";
			private const string PrefsKeyFlagFoldOutCheckVersion = PrefsKeyPrefix + "FlagFoldOutCheckVersion";
			private const string PrefsKeyFlagOpenAdvancedOprions = PrefsKeyPrefix + "FlagOpenAdvancedOprions";
			private const string PrefsKeyFlagFoldOutRuleNameAsset = PrefsKeyPrefix + "FlagFoldOutRuleNameAsset";
			private const string PrefsKeyFlagFoldOutRuleNameAssetSample = PrefsKeyPrefix + "FlagFoldOutRuleNameAssetSample";
			private const string PrefsKeyFlagFoldOutRuleNameAssetFolder = PrefsKeyPrefix + "FlagFoldOutRuleNameAssetFolder";
			private const string PrefsKeyFlagFoldOutPackAttributeAnimation = PrefsKeyPrefix + "FlagFoldOutPackAttributeAnimation";
			private const string PrefsKeyFlagFoldFlagFoldOutPresetMaterial = PrefsKeyPrefix + "FlagFoldOutPresetMaterial";
			private const string PrefsKeyFlagFlagFoldOutPresetMaterialThrough = PrefsKeyPrefix + "FlagFoldOutPresetMaterialThrough";
			private const string PrefsKeyFlagFlagFoldOutPresetMaterialMask = PrefsKeyPrefix + "FlagFoldOutPresetMaterialMask";

			private readonly static GroupSS6PU Default = new GroupSS6PU(
				false,	/* FlagFoldOutBasic */
				false,	/* FlagFoldOutPreCalcualation */
				false,	/* FlagFoldOutConfirmOverWrite */
				false,	/* FlagFoldOutCollider */
				false,	/* FlagFoldOutCheckVersion */
				false,	/* FlagOpenAdvancedOprions */
				false,	/* FlagFoldOutRuleNameAsset */
				true,	/* FlagFoldOutRuleNameAssetSample */
				false,	/* FlagFoldOutRuleNameAssetFolder */
				false,	/* FlagFoldOutPackAttributeAnimation */
				false,	/* FlagFoldFlagFoldOutPresetMaterial */
				false,	/* FlagFlagFoldOutPresetMaterialThrough */
				false	/* FlagFlagFoldOutPresetMaterialMask */
			);
			#endregion Enums & Constants
		}

		public struct GroupUnityNative
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public bool FlagFoldOutCaution;
			public bool FlagFoldOutBasic;
			public bool FlagFoldOutConfirmOverWrite;
			public bool FlagFoldOutCollider;
			public bool FlagFoldOutCheckVersion;

			public bool FlagOpenAdvancedOprions;
			public bool FlagFoldOutRuleNameAsset;
			public bool FlagFoldOutRuleNameAssetSample;
			public bool FlagFoldOutRuleNameAssetFolder;
			public bool FlagFoldOutPresetMaterial;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public GroupUnityNative(	bool flagFoldOutCaution,
										bool flagFoldOutBasic,
										bool flagFoldOutConfirmOverWrite,
										bool flagFoldOutCollider,
										bool flagFoldOutCheckVersion,
										bool flagOpenAdvancedOprions,
										bool flagFoldOutRuleNameAsset,
										bool flagFoldOutRuleNameAssetSample,
										bool flagFoldOutRuleNameAssetFolder,
										bool flagFoldOutPresetMaterial
									)
			{
				FlagFoldOutCaution = flagFoldOutCaution;
				FlagFoldOutBasic = flagFoldOutBasic;
				FlagFoldOutConfirmOverWrite = flagFoldOutConfirmOverWrite;
				FlagFoldOutCollider = flagFoldOutCollider;
				FlagFoldOutCheckVersion = flagFoldOutCheckVersion;

				FlagOpenAdvancedOprions = flagOpenAdvancedOprions;
				FlagFoldOutRuleNameAsset = flagFoldOutRuleNameAsset;
				FlagFoldOutRuleNameAssetSample = flagFoldOutRuleNameAssetSample;
				FlagFoldOutRuleNameAssetFolder = flagFoldOutRuleNameAssetFolder;
				FlagFoldOutPresetMaterial = flagFoldOutPresetMaterial;
			}

			public void CleanUp()
			{
				this = Default;
			}

			public bool Load()
			{
				FlagFoldOutCaution = EditorPrefs.GetBool(PrefsKeyFlagFoldOutCaution, Default.FlagFoldOutCaution);
				FlagFoldOutBasic = EditorPrefs.GetBool(PrefsKeyFlagFoldOutBasic, Default.FlagFoldOutBasic);
				FlagFoldOutConfirmOverWrite = EditorPrefs.GetBool(PrefsKeyFlagFoldOutConfirmOverWrite, Default.FlagFoldOutConfirmOverWrite);
				FlagFoldOutCollider = EditorPrefs.GetBool(PrefsKeyFlagFoldOutCollider, Default.FlagFoldOutCollider);
				FlagFoldOutCheckVersion = EditorPrefs.GetBool(PrefsKeyFlagFoldOutCheckVersion, Default.FlagFoldOutCheckVersion);

				FlagOpenAdvancedOprions = EditorPrefs.GetBool(PrefsKeyFlagOpenAdvancedOprions, Default.FlagOpenAdvancedOprions);
				FlagFoldOutRuleNameAsset = EditorPrefs.GetBool(PrefsKeyFlagFoldOutRuleNameAsset, Default.FlagFoldOutRuleNameAsset);
				FlagFoldOutRuleNameAssetSample = EditorPrefs.GetBool(PrefsKeyFlagFoldOutRuleNameAssetSample, Default.FlagFoldOutRuleNameAssetSample);
				FlagFoldOutRuleNameAssetFolder = EditorPrefs.GetBool(PrefsKeyFlagFoldOutRuleNameAssetFolder, Default.FlagFoldOutRuleNameAssetFolder);
				FlagFoldOutPresetMaterial = EditorPrefs.GetBool(PrefsKeyFlagFoldOutPresetMaterial, Default.FlagFoldOutPresetMaterial);

				return(true);
			}

			public bool Save()
			{
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutCaution, FlagFoldOutCaution);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutBasic, FlagFoldOutBasic);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutRuleNameAsset, FlagFoldOutRuleNameAsset);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutRuleNameAssetSample, FlagFoldOutRuleNameAssetSample);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutConfirmOverWrite, FlagFoldOutConfirmOverWrite);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutCollider, FlagFoldOutCollider);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutCheckVersion, FlagFoldOutCheckVersion);

				EditorPrefs.SetBool(PrefsKeyFlagOpenAdvancedOprions, FlagOpenAdvancedOprions);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutRuleNameAsset, FlagFoldOutRuleNameAsset);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutRuleNameAssetSample, FlagFoldOutRuleNameAssetSample);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutRuleNameAssetFolder, FlagFoldOutRuleNameAssetFolder);
				EditorPrefs.SetBool(PrefsKeyFlagFoldOutPresetMaterial, FlagFoldOutPresetMaterial);

				return(true);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			private const string PrefsKeyPrefix = "SS6PU_ToolImporter_UnityNative_";
			private const string PrefsKeyFlagFoldOutCaution = PrefsKeyPrefix + "FlagFoldOutCaution";
			private const string PrefsKeyFlagFoldOutBasic = PrefsKeyPrefix + "FlagFoldOutBasic";
			private const string PrefsKeyFlagFoldOutConfirmOverWrite = PrefsKeyPrefix + "FlagFoldOutConfirmOverWrite";
			private const string PrefsKeyFlagFoldOutCollider = PrefsKeyPrefix + "FlagFoldOutCollider";
			private const string PrefsKeyFlagFoldOutCheckVersion = PrefsKeyPrefix + "FlagFoldOutCheckVersion";
			private const string PrefsKeyFlagOpenAdvancedOprions = PrefsKeyPrefix + "FlagOpenAdvancedOprions";
			private const string PrefsKeyFlagFoldOutRuleNameAsset = PrefsKeyPrefix + "FlagFoldOutRuleNameAsset";
			private const string PrefsKeyFlagFoldOutRuleNameAssetSample = PrefsKeyPrefix + "FlagFoldOutRuleNameAssetSample";
			private const string PrefsKeyFlagFoldOutRuleNameAssetFolder = PrefsKeyPrefix + "FlagFoldOutRuleNameAssetFolder";
			private const string PrefsKeyFlagFoldOutPresetMaterial = PrefsKeyPrefix + "FlagFoldOutPresetMaterial";

			private readonly static GroupUnityNative Default = new GroupUnityNative(
				true,	/* FlagFoldOutCaution */
				false,	/* FlagFoldOutBasic */
				false,	/* FlagFoldOutConfirmOverWrite */
				false,	/* FlagFoldOutCollider */
				false,	/* FlagFoldOutCheckVersion */
				false,	/* FlagOpenAdvancedOprions */
				false,	/* FlagFoldOutRuleNameAsset */
				true,	/* FlagFoldOutRuleNameAssetSample */
				false,	/* FlagFoldOutRuleNameAssetFolder */
				false	/* FlagFoldOutPresetMaterial */
			);
			#endregion Enums & Constants
		}

		public struct GroupBatchImporter
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public string NameFolderList;
			public string NameFileList;
			public string NameFolderLog;
			public string NameFileLog;
			public bool FlagOutputLog;

			public bool FlagFoldOptions;

			public LibraryEditor_SpriteStudio6.Import.Batch.Setting Setting;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
//			public GroupBatchImporter(bool flagFoldOptions)
//			{
//				FlagFoldOptions = flagFoldOptions;
//			}

			public void CleanUp()
			{
				NameFolderList = DefaultNameFolderList;
				NameFileList = DefaultNameFileList;
				NameFolderLog = DefaultNameFolderLog;
				NameFileLog = DefaultNameFileLog;
				FlagOutputLog = DefaultFlagOutputLog;;

				FlagFoldOptions = DefaultFlagFoldOptions;

				Setting.CleanUp();
			}

			public bool Load()
			{
				NameFolderList = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderList, DefaultNameFolderList);
				NameFileList = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFileList, DefaultNameFileList);
				NameFolderLog = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderLog, DefaultNameFolderLog);
				NameFileLog = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFileLog, DefaultNameFileLog);
				FlagOutputLog  =EditorPrefs.GetBool(PrefsKeyFlagOutputLog, DefaultFlagOutputLog);

				FlagFoldOptions = EditorPrefs.GetBool(PrefsKeyFlagFoldOptions, DefaultFlagFoldOptions);

				Setting.Load();

				return(true);
			}

			public bool Save()
			{
				if(true == string.IsNullOrEmpty(NameFileLog))
				{
					NameFileLog = DefaultNameFileLog;
				}

				LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderList, NameFolderList);
				LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFileList, NameFileList);
				LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderLog, NameFolderLog);
				LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFileLog, NameFileLog);
				EditorPrefs.SetBool(PrefsKeyFlagOutputLog, FlagOutputLog);

				EditorPrefs.SetBool(PrefsKeyFlagFoldOptions, FlagFoldOptions);

				Setting.Save();

				return(true);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			private const string PrefsKeyPrefix = "SS6PU_ToolImporter_BatchImporter_";
			private const string PrefsKeyNameFolderList = PrefsKeyPrefix + "NameFolderList";
			private const string PrefsKeyNameFileList = PrefsKeyPrefix + "NameFileList";
			private const string PrefsKeyNameFolderLog = PrefsKeyPrefix + "NameFolderLog";
			private const string PrefsKeyNameFileLog = PrefsKeyPrefix + "NameFileLog";
			private const string PrefsKeyFlagFoldOptions = PrefsKeyPrefix + "FoldOptions";
			private const string PrefsKeyFlagOutputLog = PrefsKeyPrefix + "FlagOutputLog";

			private const string DefaultNameFolderList = "";
			private const string DefaultNameFileList = "";
			private const string DefaultNameFolderLog = "";
			private const string DefaultNameFileLog = "LogSS6PU_BatchImport";
			private const bool DefaultFlagOutputLog = false;
			private const bool DefaultFlagFoldOptions = false;
			#endregion Enums & Constants
		}

		public struct GroupSettingBackUp
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public bool FlagExportCommon;
			public bool FlagExportBasic;
			public bool FlagExportPrecalculation;
			public bool FlagExportConfirmOverWrite;
			public bool FlagExportCollider;
			public bool FlagExportCheckVersion;
			public bool FlagExportRuleNameAsset;
			public bool FlagExportRuleNameAssetFolder;
			public bool FlagPackAttributeAnimation;
			public bool FlagPresetMaterial;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public GroupSettingBackUp(	bool flagExportCommon,
										bool flagExportBasic,
										bool flagExportPrecalculation,
										bool flagExportCollider,
										bool flagExportConfirmOverWrite,
										bool flagExportCheckVersion,
										bool flagExportRuleNameAsset,
										bool flagExportRuleNameAssetFolder,
										bool flagPackAttributeAnimation,
										bool flagPresetMaterial
									)
			{
				FlagExportCommon = flagExportCommon;
				FlagExportBasic = flagExportBasic;
				FlagExportPrecalculation = flagExportPrecalculation;
				FlagExportCollider = flagExportCollider;
				FlagExportConfirmOverWrite = flagExportConfirmOverWrite;
				FlagExportCheckVersion = flagExportCheckVersion;

				FlagExportRuleNameAsset = flagExportRuleNameAsset;
				FlagExportRuleNameAssetFolder = flagExportRuleNameAssetFolder;
				FlagPackAttributeAnimation = flagPackAttributeAnimation;
				FlagPresetMaterial = flagPresetMaterial;
			}

			public void CleanUp()
			{
				this = Default;
			}

			public bool Load()
			{
				FlagExportCommon = EditorPrefs.GetBool(PrefsKeyFlagExportCommon, Default.FlagExportCommon);
				FlagExportBasic = EditorPrefs.GetBool(PrefsKeyFlagExportBasic, Default.FlagExportBasic);
				FlagExportPrecalculation = EditorPrefs.GetBool(PrefsKeyFlagExportPrecalculation, Default.FlagExportPrecalculation);
				FlagExportCollider = EditorPrefs.GetBool(PrefsKeyFlagExportCollider, Default.FlagExportCollider);
				FlagExportConfirmOverWrite = EditorPrefs.GetBool(PrefsKeyFlagExportConfirmOverWrite, Default.FlagExportConfirmOverWrite);
				FlagExportCheckVersion = EditorPrefs.GetBool(PrefsKeyFlagExportCheckVersion, Default.FlagExportCheckVersion);

				FlagExportRuleNameAsset = EditorPrefs.GetBool(PrefsKeyFlagExportRuleNameAsset, Default.FlagExportRuleNameAsset);
				FlagExportRuleNameAssetFolder = EditorPrefs.GetBool(PrefsKeyFlagExportRuleNameAsset, Default.FlagExportRuleNameAssetFolder);
				FlagPackAttributeAnimation = EditorPrefs.GetBool(PrefsKeyFlagPackAttributeAnimation, Default.FlagPackAttributeAnimation);
				FlagPresetMaterial = EditorPrefs.GetBool(PrefsKeyFlagPresetMaterial, Default.FlagPresetMaterial);

				return(true);
			}

			public bool Save()
			{
				EditorPrefs.SetBool(PrefsKeyFlagExportCommon, FlagExportCommon);
				EditorPrefs.SetBool(PrefsKeyFlagExportBasic, FlagExportBasic);
				EditorPrefs.SetBool(PrefsKeyFlagExportPrecalculation, FlagExportPrecalculation);
				EditorPrefs.SetBool(PrefsKeyFlagExportCollider, FlagExportCollider);
				EditorPrefs.SetBool(PrefsKeyFlagExportConfirmOverWrite, FlagExportConfirmOverWrite);
				EditorPrefs.SetBool(PrefsKeyFlagExportCheckVersion, FlagExportCheckVersion);

				EditorPrefs.SetBool(PrefsKeyFlagExportRuleNameAsset, FlagExportRuleNameAsset);
				EditorPrefs.SetBool(PrefsKeyFlagExportRuleNameAssetFolder, FlagExportRuleNameAssetFolder);
				EditorPrefs.SetBool(PrefsKeyFlagPackAttributeAnimation, FlagPackAttributeAnimation);
				EditorPrefs.SetBool(PrefsKeyFlagPresetMaterial, FlagPresetMaterial);

				return(true);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			private const string PrefsKeyPrefix = "SS6PU_ToolImporter_SettingBackUp_";
			private const string PrefsKeyFlagExportCommon = PrefsKeyPrefix + "FlagExportCommon";
			private const string PrefsKeyFlagExportBasic = PrefsKeyPrefix + "FlagExportBasic";
			private const string PrefsKeyFlagExportPrecalculation = PrefsKeyPrefix + "FlagExportPrecalculation";
			private const string PrefsKeyFlagExportCollider = PrefsKeyPrefix + "FlagExportCollider";
			private const string PrefsKeyFlagExportConfirmOverWrite = PrefsKeyPrefix + "FlagExportConfirmOverWrite";
			private const string PrefsKeyFlagExportCheckVersion = PrefsKeyPrefix + "FlagExportCheckVersion";
			private const string PrefsKeyFlagExportRuleNameAsset = PrefsKeyPrefix + "FlagExportRuleNameAsset";
			private const string PrefsKeyFlagExportRuleNameAssetFolder = PrefsKeyPrefix + "FlagExportRuleNameAssetFolder";
			private const string PrefsKeyFlagPackAttributeAnimation = PrefsKeyPrefix + "FlagPackAttributeAnimation";
			private const string PrefsKeyFlagPresetMaterial = PrefsKeyPrefix + "FlagPresetMaterial";

			private readonly static GroupSettingBackUp Default = new GroupSettingBackUp(
				false,	/* FlagExportCommon */
				true,	/* FlagExportBasic */
				true,	/* FlagExportPrecalculation */
				true,	/* FlagExportCollider */
				false,	/* FlagExportConfirmOverWrite */
				false,	/* FlagExportCheckVersion */
				true,	/* FlagExportRuleNameAsset */
				true,	/* FlagExportRuleNameAssetFolder */
				true,	/* FlagPackAttributeAnimation */
				true	/* FlagPresetMaterial */
			);
			#endregion Enums & Constants
		}
		#endregion Classes, Structs & Interfaces
	}

	/* MEMO: For some reason, if "PullDownPackAttribute" is struct, values are set case be correctly in BootUp not. */
//	private struct PullDownPackAttribute
	private class PullDownPackAttribute
	{
		/* ----------------------------------------------- Variables & Properties */
		#region Variables & Properties
		public Attribute Status;
		public Attribute Cell;
		public Attribute Position;
		public Attribute Rotation;
		public Attribute Scaling;
		public Attribute ScalingLocal;
		public Attribute RateOpacity;
		public Attribute Priority;
		public Attribute PartsColor;
		public Attribute VertexCorrection;
		public Attribute OffsetPivot;
		public Attribute PositionAnchor;
		public Attribute SizeForce;
		public Attribute PositionTexture;
		public Attribute RotationTexture;
		public Attribute ScalingTexture;
		public Attribute RadiusCollision;
		public Attribute UserData;
		public Attribute Instance;
		public Attribute Effect;
		public Attribute Deform;
		#endregion Variables & Properties

		/* ----------------------------------------------- Functions */
		#region Functions
		public void CleanUp()
		{
			Status.CleanUp();
			Cell.CleanUp();
			Position.CleanUp();
			Rotation.CleanUp();
			Scaling.CleanUp();
			ScalingLocal.CleanUp();
			RateOpacity.CleanUp();
			Priority.CleanUp();
			PartsColor.CleanUp();
			VertexCorrection.CleanUp();
			OffsetPivot.CleanUp();
			PositionAnchor.CleanUp();
			SizeForce.CleanUp();
			PositionTexture.CleanUp();
			RotationTexture.CleanUp();
			ScalingTexture.CleanUp();
			RadiusCollision.CleanUp();
			UserData.CleanUp();
			Instance.CleanUp();
			Effect.CleanUp();
			Deform.CleanUp();
		}

		public void BootUp()
		{
			CleanUp();

			int countPack = (int)Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.TERMINATOR;

			Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer[] capacityPack = new Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer[countPack];
			for(int i=0; i<countPack; i++)
			{
				capacityPack[i] = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet((Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)i);
			}

			/* MEMO: "Standard-CPE" packer can pack all attributes, so no way that "Packer can not be found". */
			/*       Just to make sure, doing formal checks and copes.                                        */
			bool[] tableFlagEnablePack = new bool[countPack];

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].Status;
			}
			Status.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].Cell;
			}
			Cell.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].Position;
			}
			Position.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].Rotation;
			}
			Rotation.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].Scaling;
			}
			Scaling.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].ScalingLocal;
			}
			ScalingLocal.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].RateOpacity;
			}
			RateOpacity.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].Priority;
			}
			Priority.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].PartsColor;
			}
			PartsColor.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].VertexCorrection;
			}
			VertexCorrection.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].OffsetPivot;
			}
			OffsetPivot.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].PositionAnchor;
			}
			PositionAnchor.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].SizeForce;
			}
			SizeForce.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].PositionTexture;
			}
			PositionTexture.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].RotationTexture;
			}
			RotationTexture.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].ScalingTexture;
			}
			ScalingTexture.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].RadiusCollision;
			}
			RadiusCollision.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].UserData;
			}
			UserData.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].Instance;
			}
			Instance.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].Effect;
			}
			Effect.BootUp(tableFlagEnablePack);

			for(int i=0; i<countPack; i++)
			{
				tableFlagEnablePack[i] = capacityPack[i].Deform;
			}
			Deform.BootUp(tableFlagEnablePack);

			SettingImport.PackAttributeAnimation.Adjust();
		}
		#endregion Functions

		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public struct Attribute
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			public string[] TableName;
			public int[] TableKindTypePack;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public void CleanUp()
			{
				TableName = null;
				TableKindTypePack = null;
			}

			public void BootUp(bool[] tableFlagEnable)
			{
				List<string> listName = new List<string>();
				List<int> listKindTypePack = new List<int>();
				listName.Clear();
				listKindTypePack.Clear();

				int count = tableFlagEnable.Length;
				if(0 >= count)
				{
					TableName = new string[0];
					TableKindTypePack = new int[0];
				}
				else
				{
					for(int i=0; i<count; i++)
					{
						if(true == tableFlagEnable[i])
						{
							listName.Add(PackName[i]);
							listKindTypePack.Add(i);
						}
					}

					TableName = listName.ToArray();
					TableKindTypePack = listKindTypePack.ToArray();

					listName.Clear();
					listKindTypePack.Clear();
				}
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			private readonly static string[] PackName = new string[(int)Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.TERMINATOR] {
				"Standard Uncompressed",
				"Standard CPE",
				"CPE + GOF-Flyweight",
				"CPE + Interpolate",
			};
			#endregion Enums & Constants
		}
		#endregion Classes, Structs & Interfaces
	}
	#endregion Classes, Structs & Interfaces
}
