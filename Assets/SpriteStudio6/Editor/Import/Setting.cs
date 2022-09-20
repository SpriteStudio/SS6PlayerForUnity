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
	public static partial class Import
	{
		/* ----------------------------------------------- Classes, Structs & Interfaces */
		#region Classes, Structs & Interfaces
		public partial struct Setting
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			/* Normal Options */
			public KindMode Mode;
			public GroupBasic Basic;
			public GroupPreCalculation PreCalcualation;
			public GroupConfirmOverWrite ConfirmOverWrite;
			public GroupCollider Collider;
			public GroupCheckVersion CheckVersion;

			/* Advanced Options */
			public GroupRuleNameAsset RuleNameAsset;
			public GroupRuleNameAssetFolder RuleNameAssetFolder;
			public GroupPackAttributeAnimation PackAttributeAnimation;
			public GroupPresetMaterial PresetMaterial;
			public GroupHolderAsset HolderAsset;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public void CleanUp()
			{
				Mode = KindMode.SS6PU;

				Basic.CleanUp();
				PreCalcualation.CleanUp();
				ConfirmOverWrite.CleanUp();
				Collider.CleanUp();
				CheckVersion.CleanUp();

				RuleNameAsset.CleanUp();
				RuleNameAssetFolder.CleanUp();
				PackAttributeAnimation.CleanUp();
				PresetMaterial.CleanUp();
				HolderAsset.CleanUp();
			}

			public bool Load()
			{
				Mode = (KindMode)(EditorPrefs.GetInt(PrefsKeyMode, (int)DefaultMode));

				Basic.Load();
				PreCalcualation.Load();
				ConfirmOverWrite.Load();
				Collider.Load();
				CheckVersion.Load();

				RuleNameAsset.Load();
				RuleNameAssetFolder.Load();
				PackAttributeAnimation.Load();
				PresetMaterial.Load();
				HolderAsset.Load();

				return(true);
			}

			public bool Save()
			{
				EditorPrefs.SetInt(PrefsKeyMode, (int)Mode);

				Basic.Save();
				PreCalcualation.Save();
				ConfirmOverWrite.Save();
				Collider.Save();
				CheckVersion.Save();

				RuleNameAsset.Save();
				RuleNameAssetFolder.Save();
				PackAttributeAnimation.Save();
				PresetMaterial.Save();
				HolderAsset.Save();

				return(true);
			}

			public string[] Export(	bool flagExportCommon,
									bool flagExportBasic,
									bool flagExportPrecalculation,
									bool flagExportCollider,
									bool flagExportConfirmOverWrite,
									bool flagExportCheckVersion,
									bool flagExportRuleNameAsset,
									bool flagExportRuleNameAssetFolder,
									bool flagExportPackAttributeAnimation,
									bool flagExportPresetMaterial,
									bool flagExportHolderAsset
								)
			{
				string[] exportCommon = (true == flagExportCommon) ? ExportCommon() : null;
				string[] exportBasic = (true == flagExportBasic) ? Basic.Export() : null;
				string[] exportPrecalculation = (true == flagExportPrecalculation) ? PreCalcualation.Export() : null;
				string[] exportConfirmOverWrite = (true == flagExportConfirmOverWrite) ? ConfirmOverWrite.Export() : null;
				string[] exportCollider = (true == flagExportCollider) ? Collider.Export() : null;
				string[] exportCheckVersion = (true == flagExportCheckVersion) ? CheckVersion.Export() : null;
				string[] exportRuleNameAsset = (true == flagExportRuleNameAsset) ? RuleNameAsset.Export() : null;
				string[] exportRuleNameAssetFolder = (true == flagExportRuleNameAssetFolder) ? RuleNameAssetFolder.Export() : null;
				string[] exportPackAttributeAnimation = (true == flagExportPackAttributeAnimation) ? PackAttributeAnimation.Export() : null;
				string[] exportPresetMaterial = (true == flagExportPresetMaterial) ? PresetMaterial.Export() : null;
				string[] exportHolderAsset = (true == flagExportHolderAsset) ? HolderAsset.Export() : null;

				int countCommon = (null != exportCommon) ? exportCommon.Length : 0;
				int countBasic = (null != exportBasic) ? exportBasic.Length : 0;
				int countPrecalculation = (null != exportPrecalculation) ? exportPrecalculation.Length : 0;
				int countConfirmOverWrite = (null != exportConfirmOverWrite) ? exportConfirmOverWrite.Length : 0;
				int countCollider = (null != exportCollider) ? exportCollider.Length : 0;
				int countCheckVersion = (null != exportCheckVersion) ? exportCheckVersion.Length : 0;
				int counttRuleNameAsset = (null != exportRuleNameAsset) ? exportRuleNameAsset.Length : 0;
				int counttRuleNameAssetFolder = (null != exportRuleNameAssetFolder) ? exportRuleNameAssetFolder.Length : 0;
				int countPackAttributeAnimation = (null != exportPackAttributeAnimation) ? exportPackAttributeAnimation.Length : 0;
				int countPresetMaterial = (null != exportPresetMaterial) ? exportPresetMaterial.Length : 0;
				int countHolderAsset = (null != exportHolderAsset) ? exportHolderAsset.Length : 0;

				int count = 0;
				count += countCommon;
				count += countBasic;
				count += countPrecalculation;
				count += countConfirmOverWrite;
				count += countCollider;
				count += countCheckVersion;
				count += counttRuleNameAsset;
				count += counttRuleNameAssetFolder;
				count += countPackAttributeAnimation;
				count += countPresetMaterial;
				count += countHolderAsset;

				string[] exportAll = new string[count];
				count = 0;
				for(int i=0; i<countCommon; i++)
				{
					exportAll[count] = exportCommon[i];
					count++;
				}
				for(int i=0; i<countBasic; i++)
				{
					exportAll[count] = exportBasic[i];
					count++;
				}
				for(int i=0; i<countPrecalculation; i++)
				{
					exportAll[count] = exportPrecalculation[i];
					count++;
				}
				for(int i=0; i<countConfirmOverWrite; i++)
				{
					exportAll[count] = exportConfirmOverWrite[i];
					count++;
				}
				for(int i=0; i<countCollider; i++)
				{
					exportAll[count] = exportCollider[i];
					count++;
				}
				for(int i=0; i<countCheckVersion; i++)
				{
					exportAll[count] = exportCheckVersion[i];
					count++;
				}

				for(int i=0; i<counttRuleNameAsset; i++)
				{
					exportAll[count] = exportRuleNameAsset[i];
					count++;
				}
				for(int i=0; i<counttRuleNameAssetFolder; i++)
				{
					exportAll[count] = exportRuleNameAssetFolder[i];
					count++;
				}
				for(int i=0; i<countPackAttributeAnimation; i++)
				{
					exportAll[count] = exportPackAttributeAnimation[i];
					count++;
				}
				for(int i=0; i<countPresetMaterial; i++)
				{
					exportAll[count] = exportPresetMaterial[i];
					count++;
				}
				for(int i=0; i<countHolderAsset; i++)
				{
					exportAll[count] = exportHolderAsset[i];
					count++;
				}

				return(exportAll);
			}

			public bool Import(string text)
			{
				string[] textArgument = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineDecodeCommand(text);
				if((null == textArgument) || (0 >= textArgument.Length))
				{	/* Invalid */
					return(false);
				}

				if(true == ImportCommon(textArgument))
				{
					return(true);
				}
				if(true == Basic.Import(textArgument))
				{
					return(true);
				}
				if(true == PreCalcualation.Import(textArgument))
				{
					return(true);
				}
				if(true == ConfirmOverWrite.Import(textArgument))
				{
					return(true);
				}
				if(true == Collider.Import(textArgument))
				{
					return(true);
				}
				if(true == CheckVersion.Import(textArgument))
				{
					return(true);
				}

				if(true == RuleNameAsset.Import(textArgument))
				{
					return(true);
				}
				if(true == RuleNameAssetFolder.Import(textArgument))
				{
					return(true);
				}
				if(true == PackAttributeAnimation.Import(textArgument))
				{
					return(true);
				}
				if(true == PresetMaterial.Import(textArgument))
				{
					return(true);
				}
				if(true == HolderAsset.Import(textArgument))
				{
					return(true);
				}

				return(false);
			}

			private string[] ExportCommon()
			{
				string[] textExport = new string[1];

				textExport[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyMode, TextArgumentMode[(int)Mode]);

				return(textExport);
			}

			private bool ImportCommon(string[] textArgument)
			{
				switch(textArgument[0])
				{
					case TextKeyMode:
						{
							KindMode mode = ImportCommonMode(textArgument[1]);
							if(KindMode.SS6PU <= mode)
							{	/* Valid */
								Mode = mode;
							}
						}
						return(true);

					case TextKeyFolderPrevious:
						break;

					default:
						break;
				}
				return(false);
			}
			public static KindMode ImportCommonMode(string text)
			{
				for(int i=0; i<TextArgumentMode.Length; i++)
				{
					if(text == TextArgumentMode[i])
					{
						return((KindMode)i);
					}
				}
				return((KindMode)(-1));
			}

			public bool ExportFile(	string nameFile,
									bool flagExportCommon,
									bool flagExportBasic,
									bool flagExportPrecalculation,
									bool flagExportCollider,
									bool flagExportConfirmOverWrite,
									bool flagExportCheckVersion,
									bool flagExportRuleNameAsset,
									bool flagExportRuleNameAssetFolder,
									bool flagExportPackAttributeAnimation,
									bool flagExportPresetMaterial,
									bool flagExportHolderAsset
								)
			{
				bool rv = false;
				using(System.IO.StreamWriter fileStream = new System.IO.StreamWriter(nameFile, false, System.Text.Encoding.UTF8))
				{
					int count = TextSignatureSettingFile.Length;
					for(int i=0; i<count; i++)
					{
						fileStream.WriteLine(LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeIgnore(TextSignatureSettingFile[i]));
					}

					string[] textLine = Export(	flagExportCommon,
												flagExportBasic,
												flagExportPrecalculation,
												flagExportCollider,
												flagExportConfirmOverWrite,
												flagExportCheckVersion,
												flagExportRuleNameAsset,
												flagExportRuleNameAssetFolder,
												flagExportPackAttributeAnimation,
												flagExportPresetMaterial,
												flagExportHolderAsset
											);
					if(null != textLine)
					{
						count = textLine.Length;
						for(int i=0; i<count; i++)
						{
							fileStream.WriteLine(textLine[i]);
						}
					}
					rv = true;
				}

				return(rv);
			}

			public bool ImportFile(string nameFile)
			{
				bool rv = false;
				if(true == System.IO.File.Exists(nameFile))
				{
					using(System.IO.StreamReader fileStream = new System.IO.StreamReader(nameFile, System.Text.Encoding.UTF8))
					{
						string textLine = "";
						string textLineValid = "";
						while(0 <= fileStream.Peek())
						{
							textLine = fileStream.ReadLine();
							switch(LibraryEditor_SpriteStudio6.Utility.ExternalText.TypeGetLine(out textLineValid, textLine))
							{
								case LibraryEditor_SpriteStudio6.Utility.ExternalText.KindType.COMMAND:
									Import(textLineValid);
									break;

								case LibraryEditor_SpriteStudio6.Utility.ExternalText.KindType.IGNORE:
								case LibraryEditor_SpriteStudio6.Utility.ExternalText.KindType.NORMAL:
								default:
									break;
							}
						}
						rv = true;
					}
				}

				return(rv);
			}

			public static void FolderSavePrevious(string nameFolder)
			{
				LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyFolderPrevious, nameFolder);
			}

			public static void FolderLoadPrevious(out string nameFolder)
			{
				nameFolder = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyFolderPrevious, "");
			}

			public static string AssetNameNormalize(string name)
			{
				/* Spaces */
				string nameNormalize = name.Replace(" ", "");
				nameNormalize = nameNormalize.Replace("\t", "");

				/* File & Hierarchy Delimiter */
				nameNormalize = nameNormalize.Replace(":", "");
				nameNormalize = nameNormalize.Replace("/", "");
				nameNormalize = nameNormalize.Replace("\\", "");
				nameNormalize = nameNormalize.Replace(".", "");
				nameNormalize = nameNormalize.Replace("*", "");
				nameNormalize = nameNormalize.Replace("?", "");

				/* Line-Feeds */
				nameNormalize = nameNormalize.Replace("\n", "");
				nameNormalize = nameNormalize.Replace("\r", "");

				/* Quotations */
				nameNormalize = nameNormalize.Replace("\"", "");
				nameNormalize = nameNormalize.Replace("\'", "");

				return(nameNormalize);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public enum KindMode
			{
				SS6PU = 0,
				UNITY_NATIVE,
				UNITY_UI,

				TERMINATOR,
				BATCH_IMPORTER = TERMINATOR,
			}
			public enum KindAsset
			{
				/* (Common) */
				TEXTURE = 0,

				/* (Mode SS6PU) */
				PREFAB_CONTROL_ANIMATION_SS6PU,
				PREFAB_ANIMATION_SS6PU,
				PREFAB_EFFECT_SS6PU,
				DATA_PROJECT_SS6PU,
				DATA_CELLMAP_SS6PU,
				DATA_ANIMATION_SS6PU,
				DATA_EFFECT_SS6PU,
				DATA_SEQUENCE_SS6PU,
				/* Obsolete */	// MATERIAL_ANIMATION_SS6PU,
				/* Obsolete */	// MATERIAL_EFFECT_SS6PU,

				/* (Mode UnityNative) */
				PREFAB_CONTROL_ANIMATION_UNITYNATIVE,
				PREFAB_ANIMATION_UNITYNATIVE,
				/* Obsolete */	// PREFAB_EFFECT_UNITYNATIVE,
				DATA_ANIMATION_UNITYNATIVE,
				DATA_MESH_UNITYNATIVE,
				/* Obsolete */	// MATERIAL_ANIMATION_UNITYNATIVE,
				/* Obsolete */	// MATERIAL_EFFECT_UNITYNATIVE,

				/* (Mode UnityUI) */
				PREFAB_ANIMATION_UNITYUI,
				DATA_ANIMATION_UNITYUI,
				/* Obsolete */	// MATERIAL_ANIMATION_UNITYUI,
			}

			private const string KeyMode = "Mode";
			private const string KeyFolderPrevious = "FolderPrevious";

			private const string TextKeyPrefixCommon = "Common_";
			internal const string TextKeyMode = TextKeyPrefixCommon + KeyMode;	/* Referenced from batch-importer */
			private const string TextKeyFolderPrevious = TextKeyPrefixCommon + KeyFolderPrevious;

			public const string PrefsKeyPrefix = "SS6PU_ImportSetting_";	/* Common for all settings */
			private const string PrefsKeyPrefixCommon = PrefsKeyPrefix + TextKeyPrefixCommon;
			private const string PrefsKeyMode = PrefsKeyPrefixCommon + KeyMode;
			private const string PrefsKeyFolderPrevious = PrefsKeyPrefixCommon + KeyFolderPrevious;

			private readonly static string[] TextArgumentMode =
			{
				"SS6PU",
				"UNITY_NATIVE",

				/* "BATCH_IMPORTER" *//* Can not set the mode "batch import" from batch-importer's list. */
			};

			private const KindMode DefaultMode = KindMode.SS6PU;
			private const string DefaultFolderPrevious = "";

			private readonly static string[] TextSignatureSettingFile =
			{
				"========================================================",
				" Importer Setting-File",
				"--------------------------------------------------------",
				" Generated with \"SpriteStudio6 Player for Unity\"",
				"",
				" Copyright(C) 1997-2021 Web Technology Corp.",
				" Copyright(C) CRI Middleware Co., Ltd.",
				" All rights reserved.",
				"========================================================",
			};

			/* Assets Install Base-Path */
			internal const string PathAssetDefaultFolderHome = "SpriteStudio6/";
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public struct GroupPreCalculation
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagTrimTransparentPixelsCell;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupPreCalculation(	bool flagTrimTransparentPixelsCell
										)
				{
					FlagTrimTransparentPixelsCell = flagTrimTransparentPixelsCell;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					FlagTrimTransparentPixelsCell = EditorPrefs.GetBool(PrefsKeyFlagTrimTransparentPixelsCell, Default.FlagTrimTransparentPixelsCell);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagTrimTransparentPixelsCell, FlagTrimTransparentPixelsCell);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[1];
					string textValue;

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagTrimTransparentPixelsCell);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagTrimTransparentPixelsCell, textValue);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					switch(textArgument[0])
					{
						case TextKeyFlagTrimTransparentPixelsCell:
							FlagTrimTransparentPixelsCell = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagFixSprite:	/* Obsolete command */
							return(true);

						default:
							break;
					}
					return(false);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				/* Obsolete */	private const string KeyFlagFixSprite = "FlagFixSprite";
				private const string KeyFlagTrimTransparentPixelsCell = "FlagTrimTransparentPixelsCell";

				private const string TextKeyPrefix = "PreCalculation_";
				/* Obsolete */	private const string TextKeyFlagFixSprite = TextKeyPrefix + KeyFlagFixSprite;
				private const string TextKeyFlagTrimTransparentPixelsCell = TextKeyPrefix + KeyFlagTrimTransparentPixelsCell;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				/* Obsolete */	private const string PrefsKeyFlagFixSprite = PrefsKeyPrefix + KeyFlagFixSprite;
				private const string PrefsKeyFlagTrimTransparentPixelsCell = PrefsKeyPrefix + KeyFlagTrimTransparentPixelsCell;

				private readonly static GroupPreCalculation Default = new GroupPreCalculation(
					false	/* FlagTrimTransparentPixelsCell */
				);
				#endregion Enums & Constants
			}

			public struct GroupConfirmOverWrite
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagPrefabAnimation;
				public bool FlagPrefabEffect;
				public bool FlagDataProject;
				public bool FlagDataCellMap;
				public bool FlagDataAnimation;
				public bool FlagDataEffect;
				public bool FlagDataSequence;
				public bool FlagTexture;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupConfirmOverWrite(	bool flagPrefabAnimation,
												bool flagPrefabEffect,
												bool flagDataProject,
												bool flagDataCellMap,
												bool flagDataAnimation,
												bool flagDataEffect,
												bool flagDataSequence,
												bool flagTexture
											)
				{
					FlagPrefabAnimation = flagPrefabAnimation;
					FlagPrefabEffect = flagPrefabEffect;
					FlagDataProject = flagDataProject;
					FlagDataCellMap = flagDataCellMap;
					FlagDataAnimation = flagDataAnimation;
					FlagDataEffect = flagDataEffect;
					FlagDataSequence = flagDataSequence;
					FlagTexture = flagTexture;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					FlagPrefabAnimation = EditorPrefs.GetBool(PrefsKeyFlagPrefabAnimation, Default.FlagPrefabAnimation);
					FlagPrefabEffect = EditorPrefs.GetBool(PrefsKeyFlagPrefabEffect, Default.FlagPrefabEffect);
					FlagDataProject = EditorPrefs.GetBool(PrefsKeyFlagDataProject, Default.FlagDataProject);
					FlagDataCellMap = EditorPrefs.GetBool(PrefsKeyFlagDataCellMap, Default.FlagDataCellMap);
					FlagDataAnimation = EditorPrefs.GetBool(PrefsKeyFlagDataAnimation, Default.FlagDataAnimation);
					FlagDataEffect = EditorPrefs.GetBool(PrefsKeyFlagDataEffect, Default.FlagDataEffect);
					FlagDataSequence = EditorPrefs.GetBool(PrefsKeyFlagDataSequence, Default.FlagDataSequence);
					FlagTexture = EditorPrefs.GetBool(PrefsKeyFlagTexture, Default.FlagTexture);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagPrefabAnimation, FlagPrefabAnimation);
					EditorPrefs.SetBool(PrefsKeyFlagPrefabEffect, FlagPrefabEffect);
					EditorPrefs.SetBool(PrefsKeyFlagDataProject, FlagDataProject);
					EditorPrefs.SetBool(PrefsKeyFlagDataCellMap, FlagDataCellMap);
					EditorPrefs.SetBool(PrefsKeyFlagDataAnimation, FlagDataAnimation);
					EditorPrefs.SetBool(PrefsKeyFlagDataEffect, FlagDataEffect);
					EditorPrefs.SetBool(PrefsKeyFlagDataSequence, FlagDataSequence);
					EditorPrefs.SetBool(PrefsKeyFlagTexture, FlagTexture);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[8];
					string textValue;

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagPrefabAnimation);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagPrefabAnimation, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagPrefabEffect);
					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagPrefabEffect, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagDataProject);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagDataProject, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagDataCellMap);
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagDataCellMap, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagDataAnimation);
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagDataAnimation, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagDataEffect);
					textEncode[5] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagDataEffect, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagDataSequence);
					textEncode[6] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagDataSequence, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagTexture);
					textEncode[7] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagTexture, textValue);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					switch(textArgument[0])
					{
						case TextKeyFlagPrefabAnimation:
							FlagPrefabAnimation = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagPrefabEffect:
							FlagPrefabEffect = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagDataProject:
							FlagDataProject = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagDataCellMap:
							FlagDataCellMap = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagDataAnimation:
							FlagDataAnimation = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagDataEffect:
							FlagDataEffect = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagDataSequence:
							FlagDataSequence = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagTexture:
							FlagTexture = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						default:
							break;
					}
					return(false);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyFlagPrefabAnimation = "FlagPrefabAnimation";
				private const string KeyFlagPrefabEffect = "FlagPrefabEffect";
				private const string KeyFlagDataProject = "FlagDataProject";
				private const string KeyFlagDataCellMap = "FlagDataCellMap";
				private const string KeyFlagDataAnimation = "FlagDataAnimation";
				private const string KeyFlagDataEffect = "FlagDataEffect";
				private const string KeyFlagDataSequence = "FlagDataSequence";
				/* Obsolete */	// private const string KeyFlagMaterialAnimation = "FlagMaterialAnimation";
				/* Obsolete */	// private const string KeyFlagMaterialEffect = "FlagMaterialEffect";
				private const string KeyFlagTexture = "FlagTexture";

				private const string TextKeyPrefix = "ConfirmOverWrite_";
				private const string TextKeyFlagPrefabAnimation = TextKeyPrefix + KeyFlagPrefabAnimation;
				private const string TextKeyFlagPrefabEffect = TextKeyPrefix + KeyFlagPrefabEffect;
				private const string TextKeyFlagDataProject = TextKeyPrefix + KeyFlagDataProject;
				private const string TextKeyFlagDataCellMap = TextKeyPrefix + KeyFlagDataCellMap;
				private const string TextKeyFlagDataAnimation = TextKeyPrefix + KeyFlagDataAnimation;
				private const string TextKeyFlagDataEffect = TextKeyPrefix + KeyFlagDataEffect;
				private const string TextKeyFlagDataSequence = TextKeyPrefix + KeyFlagDataSequence;
				/* Obsolete */	// private const string TextKeyFlagMaterialAnimation = TextKeyPrefix + KeyFlagMaterialAnimation;
				/* Obsolete */	// private const string TextKeyFlagMaterialEffect = TextKeyPrefix + KeyFlagMaterialEffect;
				private const string TextKeyFlagTexture = TextKeyPrefix + KeyFlagTexture;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagPrefabAnimation = PrefsKeyPrefix + KeyFlagPrefabAnimation;
				private const string PrefsKeyFlagPrefabEffect = PrefsKeyPrefix + KeyFlagPrefabEffect;
				private const string PrefsKeyFlagDataProject = PrefsKeyPrefix + KeyFlagDataProject;
				private const string PrefsKeyFlagDataCellMap = PrefsKeyPrefix + KeyFlagDataCellMap;
				private const string PrefsKeyFlagDataAnimation = PrefsKeyPrefix + KeyFlagDataAnimation;
				private const string PrefsKeyFlagDataEffect = PrefsKeyPrefix + KeyFlagDataEffect;
				private const string PrefsKeyFlagDataSequence = PrefsKeyPrefix + KeyFlagDataSequence;
				/* Obsolete */	// private const string PrefsKeyFlagMaterialAnimation = PrefsKeyPrefix + KeyFlagMaterialAnimation;
				/* Obsolete */	// private const string PrefsKeyFlagMaterialEffect = PrefsKeyPrefix + KeyFlagMaterialEffect;
				private const string PrefsKeyFlagTexture = PrefsKeyPrefix + KeyFlagTexture;

				internal readonly static GroupConfirmOverWrite Default = new GroupConfirmOverWrite(
					false,	/* FlagPrefabAnimation */
					false,	/* FlagPrefabEffect */
					false,	/* FlagDataProject */
					false,	/* FlagDataCellMap */
					false,	/* FlagDataAnimation */
					false,	/* FlagDataEffect */
					false,	/* FlagDataSequence */
					false	/* FlagTexture */
				);
				#endregion Enums & Constants
			}

			public struct GroupCollider
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagAttachCollider;
				public bool FlagCollider2D;
				public bool FlagAttachRigidBody;
				public bool FlagIsTrigger;
				public float SizeZ;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupCollider(	bool flagAttachColider,
										bool flagCollider2D,
										bool flagAttachRigidBody,
										bool flagIsTrigger,
										float sizeZ
									)
				{
					FlagAttachCollider = flagAttachColider;
					FlagCollider2D = flagCollider2D;
					FlagAttachRigidBody = flagAttachRigidBody;
					FlagIsTrigger = flagIsTrigger;
					SizeZ = sizeZ;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					FlagAttachCollider = EditorPrefs.GetBool(PrefsKeyFlagAttachCollider, Default.FlagAttachCollider);
					FlagCollider2D = EditorPrefs.GetBool(PrefsKeyFlagCollider2D, Default.FlagCollider2D);
					FlagAttachRigidBody = EditorPrefs.GetBool(PrefsKeyFlagAttachRigidBody, Default.FlagAttachRigidBody);
					FlagIsTrigger = EditorPrefs.GetBool(PrefsKeyFlagIsTrigger, Default.FlagIsTrigger);
					SizeZ = EditorPrefs.GetFloat(PrefsKeySizeZ, Default.SizeZ);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagAttachCollider, FlagAttachCollider);
					EditorPrefs.SetBool(PrefsKeyFlagCollider2D, FlagCollider2D);
					EditorPrefs.SetBool(PrefsKeyFlagAttachRigidBody, FlagAttachRigidBody);
					EditorPrefs.SetBool(PrefsKeyFlagIsTrigger, FlagIsTrigger);
					EditorPrefs.SetFloat(PrefsKeySizeZ, SizeZ);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[5];
					string textValue;

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagAttachCollider);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagAttachCollider, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagCollider2D);
					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagCollider2D, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagAttachRigidBody);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagAttachRigidBody, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagIsTrigger);
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagIsTrigger, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.FloatEncode(SizeZ);
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySizeZ, textValue);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					switch(textArgument[0])
					{
						case TextKeyFlagAttachCollider:
							FlagAttachCollider = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagCollider2D:
							FlagAttachCollider = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagAttachRigidBody:
							FlagAttachRigidBody = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagIsTrigger:
							FlagIsTrigger = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeySizeZ:
							SizeZ = LibraryEditor_SpriteStudio6.Utility.ExternalText.FloatDecode(textArgument[1]);
							return(true);

						default:
							break;
					}
					return(false);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyFlagAttachCollider = "FlagAttachCollider";
				private const string KeyFlagCollider2D = "FlagCollider2D";
				private const string KeyFlagAttachRigidBody = "FlagAttachRigidBody";
				private const string KeyFlagIsTrigger = "FlagIsTrigger";
				private const string KeySizeZ = "SizeZ";

				private const string TextKeyPrefix = "Collider_";
				private const string TextKeyFlagAttachCollider = TextKeyPrefix + KeyFlagAttachCollider;
				private const string TextKeyFlagCollider2D = TextKeyPrefix + KeyFlagCollider2D;
				private const string TextKeyFlagAttachRigidBody = TextKeyPrefix + KeyFlagAttachRigidBody;
				private const string TextKeyFlagIsTrigger = TextKeyPrefix + KeyFlagIsTrigger;
				private const string TextKeySizeZ = TextKeyPrefix + KeySizeZ;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagAttachCollider = PrefsKeyPrefix + KeyFlagAttachCollider;
				private const string PrefsKeyFlagCollider2D = PrefsKeyPrefix + KeyFlagCollider2D;
				private const string PrefsKeyFlagAttachRigidBody = PrefsKeyPrefix + KeyFlagAttachRigidBody;
				private const string PrefsKeyFlagIsTrigger = PrefsKeyPrefix + KeyFlagIsTrigger;
				private const string PrefsKeySizeZ = PrefsKeyPrefix + KeySizeZ;

				private readonly static GroupCollider Default = new GroupCollider(
					true,	/* FlagAttachCollider */
					false,	/* FlagCollider2D */
					false,	/* FlagAttachRigidBody */
					false,	/* FlagIsTrigger */
					1.0f	/* SizeZ */
				);
				#endregion Enums & Constants
			}

			public struct GroupCheckVersion
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagInvalidSSPJ;
				public bool FlagInvalidSSCE;
				public bool FlagInvalidSSAE;
				public bool FlagInvalidSSEE;
				public bool FlagInvalidSSQE;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupCheckVersion(	bool flagInvalidSSPJ,
											bool flagInvalidSSCE,
											bool flagInvalidSSAE,
											bool flagInvalidSSEE,
											bool flagInvalidSSQE
										)
				{
					FlagInvalidSSPJ = flagInvalidSSPJ;
					FlagInvalidSSCE = flagInvalidSSCE;
					FlagInvalidSSAE = flagInvalidSSAE;
					FlagInvalidSSEE = flagInvalidSSEE;
					FlagInvalidSSQE = flagInvalidSSQE;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					FlagInvalidSSPJ = EditorPrefs.GetBool(PrefsKeyFlagInvalidSSPJ, Default.FlagInvalidSSPJ);
					FlagInvalidSSCE = EditorPrefs.GetBool(PrefsKeyFlagInvalidSSCE, Default.FlagInvalidSSCE);
					FlagInvalidSSAE = EditorPrefs.GetBool(PrefsKeyFlagInvalidSSAE, Default.FlagInvalidSSAE);
					FlagInvalidSSEE = EditorPrefs.GetBool(PrefsKeyFlagInvalidSSEE, Default.FlagInvalidSSEE);
					FlagInvalidSSQE = EditorPrefs.GetBool(PrefsKeyFlagInvalidSSQE, Default.FlagInvalidSSQE);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.GetBool(PrefsKeyFlagInvalidSSPJ, FlagInvalidSSPJ);
					EditorPrefs.GetBool(PrefsKeyFlagInvalidSSCE, FlagInvalidSSCE);
					EditorPrefs.GetBool(PrefsKeyFlagInvalidSSAE, FlagInvalidSSAE);
					EditorPrefs.GetBool(PrefsKeyFlagInvalidSSEE, FlagInvalidSSEE);
					EditorPrefs.GetBool(PrefsKeyFlagInvalidSSQE, FlagInvalidSSQE);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[5];
					string textValue;

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagInvalidSSPJ);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagInvalidSSPJ, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagInvalidSSCE);
					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagInvalidSSCE, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagInvalidSSAE);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagInvalidSSAE, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagInvalidSSEE);
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagInvalidSSEE, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagInvalidSSQE);
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagInvalidSSQE, textValue);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					switch(textArgument[0])
					{
						case TextKeyFlagInvalidSSPJ:
							FlagInvalidSSPJ = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagInvalidSSCE:
							FlagInvalidSSCE = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagInvalidSSAE:
							FlagInvalidSSAE = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagInvalidSSEE:
							FlagInvalidSSEE = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagInvalidSSQE:
							FlagInvalidSSQE = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						default:
							break;
					}
					return(false);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyFlagInvalidSSPJ = "FlagInvalidSSPJ";
				private const string KeyFlagInvalidSSCE = "FlagInvalidSSCE";
				private const string KeyFlagInvalidSSAE = "FlagInvalidSSAE";
				private const string KeyFlagInvalidSSEE = "FlagInvalidSSEE";
				private const string KeyFlagInvalidSSQE = "FlagInvalidSSQE";

				private const string TextKeyPrefix = "CheckVersion_";
				private const string TextKeyFlagInvalidSSPJ = TextKeyPrefix + KeyFlagInvalidSSPJ;
				private const string TextKeyFlagInvalidSSCE = TextKeyPrefix + KeyFlagInvalidSSCE;
				private const string TextKeyFlagInvalidSSAE = TextKeyPrefix + KeyFlagInvalidSSAE;
				private const string TextKeyFlagInvalidSSEE = TextKeyPrefix + KeyFlagInvalidSSEE;
				private const string TextKeyFlagInvalidSSQE = TextKeyPrefix + KeyFlagInvalidSSQE;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagInvalidSSPJ = PrefsKeyPrefix + KeyFlagInvalidSSPJ;
				private const string PrefsKeyFlagInvalidSSCE = PrefsKeyPrefix + KeyFlagInvalidSSCE;
				private const string PrefsKeyFlagInvalidSSAE = PrefsKeyPrefix + KeyFlagInvalidSSAE;
				private const string PrefsKeyFlagInvalidSSEE = PrefsKeyPrefix + KeyFlagInvalidSSEE;
				private const string PrefsKeyFlagInvalidSSQE = PrefsKeyPrefix + KeyFlagInvalidSSQE;

				internal readonly static GroupCheckVersion Default = new GroupCheckVersion(
					false,	/* FlagInvalidSSPJ */
					false,	/* FlagInvalidSSCE */
					false,	/* FlagInvalidSSAE */
					false,	/* FlagInvalidSSEE */
					false	/* FlagInvalidSSQE */
				);
				#endregion Enums & Constants
			}

			public struct GroupBasic
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagCreateControlGameObject;
				public bool FlagCreateProjectFolder;
				public bool FlagTextureReadable;
				public bool FlagCreateHolderAsset;
				public bool FlagInvisibleToHideAll;
				public bool FlagTrackAssets;
				public bool FlagDisableInitialLightRenderer;
				public bool FlagTakeOverLightRenderer;
				public bool FlagConvertSignal;
				public bool FlagIgnoreSetup;
				public bool FlagNestedPrefabUseScript;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupBasic(	bool flagCreateControlGameObject,
									bool flagCreateProjectFolder,
									bool flagTextureReadable,
									bool flagCreateHolderAsset,
									bool flagInvisibleToHideAll,
									bool flagTrackAssets,
									bool flagDisableInitialLightRenderer,
									bool flagTakeOverLightRenderer,
									bool flagConvertSignal,
									bool flagIgnoreSetup,
									bool flagNestedPrefabUseScript
								)
				{
					FlagCreateControlGameObject = flagCreateControlGameObject;
					FlagCreateProjectFolder = flagCreateProjectFolder;
					FlagTextureReadable = flagTextureReadable;
					FlagCreateHolderAsset = flagCreateHolderAsset;
					FlagInvisibleToHideAll = flagInvisibleToHideAll;
					FlagTrackAssets = flagTrackAssets;
					FlagDisableInitialLightRenderer = flagDisableInitialLightRenderer;
					FlagTakeOverLightRenderer = flagTakeOverLightRenderer;
					FlagConvertSignal = flagConvertSignal;
					FlagIgnoreSetup = flagIgnoreSetup;
					FlagNestedPrefabUseScript = flagNestedPrefabUseScript;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					FlagCreateControlGameObject = EditorPrefs.GetBool(PrefsKeyFlagCreateControlGameObject, Default.FlagCreateControlGameObject);
					FlagCreateProjectFolder = EditorPrefs.GetBool(PrefsKeyFlagCreateProjectFolder, Default.FlagCreateProjectFolder);
					FlagTextureReadable = EditorPrefs.GetBool(PrefsKeyFlagTextureReadable, Default.FlagTextureReadable);
					FlagCreateHolderAsset = EditorPrefs.GetBool(PrefsKeyFlagCreateHolderAsset, Default.FlagCreateHolderAsset);
					FlagInvisibleToHideAll = EditorPrefs.GetBool(PrefsKeyFlagInvisibleToHideAll, Default.FlagInvisibleToHideAll);
					FlagTrackAssets = EditorPrefs.GetBool(PrefsKeyFlagTrackAssets, Default.FlagTrackAssets);
					FlagDisableInitialLightRenderer = EditorPrefs.GetBool(PrefsKeyFlagDisableInitialLightRenderer, Default.FlagDisableInitialLightRenderer);
					FlagTakeOverLightRenderer = EditorPrefs.GetBool(PrefsKeyFlagTakeOverLightRenderer, Default.FlagTakeOverLightRenderer);
					FlagConvertSignal = EditorPrefs.GetBool(PrefsKeyFlagConvertSignal, Default.FlagConvertSignal);
					FlagIgnoreSetup = EditorPrefs.GetBool(PrefsKeyFlagIgnoreSetup, Default.FlagIgnoreSetup);
					FlagNestedPrefabUseScript = EditorPrefs.GetBool(PrefsKeyFlagNestedPrefabUseScript, Default.FlagNestedPrefabUseScript);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagCreateControlGameObject, FlagCreateControlGameObject);
					EditorPrefs.SetBool(PrefsKeyFlagCreateProjectFolder, FlagCreateProjectFolder);
					EditorPrefs.SetBool(PrefsKeyFlagTextureReadable, FlagTextureReadable);
					EditorPrefs.SetBool(PrefsKeyFlagCreateHolderAsset, FlagCreateHolderAsset);
					EditorPrefs.SetBool(PrefsKeyFlagInvisibleToHideAll, FlagInvisibleToHideAll);
					EditorPrefs.SetBool(PrefsKeyFlagTrackAssets, FlagTrackAssets);
					EditorPrefs.SetBool(PrefsKeyFlagDisableInitialLightRenderer, FlagDisableInitialLightRenderer);
					EditorPrefs.SetBool(PrefsKeyFlagTakeOverLightRenderer, FlagTakeOverLightRenderer);
					EditorPrefs.SetBool(PrefsKeyFlagConvertSignal, FlagConvertSignal);
					EditorPrefs.SetBool(PrefsKeyFlagIgnoreSetup, FlagIgnoreSetup);
					EditorPrefs.SetBool(PrefsKeyFlagNestedPrefabUseScript, FlagNestedPrefabUseScript);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[11];
					string textValue;

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagCreateControlGameObject);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagCreateControlGameObject, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagCreateProjectFolder);
					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagCreateProjectFolder, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagTextureReadable);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagTextureReadable, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagCreateHolderAsset);
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagCreateHolderAsset, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagInvisibleToHideAll);
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagInvisibleToHideAll, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagTrackAssets);
					textEncode[5] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagTrackAssets, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagDisableInitialLightRenderer);
					textEncode[6] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagDisableInitialLightRenderer, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagTakeOverLightRenderer);
					textEncode[7] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagTakeOverLightRenderer, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagConvertSignal);
					textEncode[8] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagConvertSignal, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagIgnoreSetup);
					textEncode[9] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagIgnoreSetup, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagNestedPrefabUseScript);
					textEncode[10] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagNestedPrefabUseScript, textValue);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					switch(textArgument[0])
					{
						case TextKeyFlagCreateControlGameObject:
							FlagCreateControlGameObject = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagCreateProjectFolder:
							FlagCreateProjectFolder = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagTextureReadable:
							FlagTextureReadable = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagCreateHolderAsset:
							FlagCreateHolderAsset = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagInvisibleToHideAll:
							FlagInvisibleToHideAll = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagTrackAssets:
							FlagTrackAssets = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagDeleteMaterialUnreferenced:	/* Obsolete command */
							return(true);

						case TextKeyFlagDisableInitialLightRenderer:
							FlagDisableInitialLightRenderer = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagTakeOverLightRenderer:
							FlagTakeOverLightRenderer = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagConvertSignal:
							FlagConvertSignal = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagIgnoreSetup:
							FlagIgnoreSetup = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagNestedPrefabUseScript:
							FlagNestedPrefabUseScript = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						default:
							break;
					}
					return(false);
				}

				private KindNoCreateMaterialUnreferenced KindGetNoCreateMaterialUnreferenced(string text)
				{
					string textUpper = text.ToUpper();
					for(KindNoCreateMaterialUnreferenced i=KindNoCreateMaterialUnreferenced.NONE; i<KindNoCreateMaterialUnreferenced.TERMINATOR; i++)
					{
						if(i.ToString() == textUpper)
						{
							return(i);
						}
					}

					/* MEMO: In case of error, return NON. */
					return(KindNoCreateMaterialUnreferenced.NONE);
				}

				private string TextGetNoCreateMaterialUnreferenced(KindNoCreateMaterialUnreferenced kind)
				{
					return(kind.ToString());
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				public enum KindNoCreateMaterialUnreferenced
				{
					NONE = 0,
					BLENDING,
					BLENDING_CELLMAP,

					TERMINATOR
				}

				private const string KeyFlagCreateControlGameObject = "FlagCreateControlGameObject";
				private const string KeyFlagCreateProjectFolder = "FlagCreateProjectFolder";
				private const string KeyFlagTextureReadable = "FlagTextureReadable";
				private const string KeyFlagCreateHolderAsset = "FlagCreateHolderAsset";
				private const string KeyFlagInvisibleToHideAll = "FlagInvisibleToHideAll";
				private const string KeyFlagTrackAssets = "FlagTrackAssets";
				/* Obsolete */		private const string KeyFlagDeleteMaterialUnreferenced = "FlagDeleteMaterialUnreferenced";
				private const string KeyFlagDisableInitialLightRenderer = "FlagDisableInitialLightRenderer";
				private const string KeyFlagTakeOverLightRenderer = "FlagTakeOverLightRenderer";
				private const string KeyFlagConvertSignal = "FlagConvertSignal";
				private const string KeyFlagIgnoreSetup = "FlagIgnoreSetup";
				private const string KeyFlagNestedPrefabUseScript = "FlagNestedPrefabUseScript";

				private const string TextKeyPrefix = "Basic_";
				private const string TextKeyFlagCreateControlGameObject = TextKeyPrefix + KeyFlagCreateControlGameObject;
				private const string TextKeyFlagCreateProjectFolder = TextKeyPrefix + KeyFlagCreateProjectFolder;
				private const string TextKeyFlagTextureReadable = TextKeyPrefix + KeyFlagTextureReadable;
				private const string TextKeyFlagCreateHolderAsset = TextKeyPrefix + KeyFlagCreateHolderAsset;
				private const string TextKeyFlagInvisibleToHideAll = TextKeyPrefix + KeyFlagInvisibleToHideAll;
				private const string TextKeyFlagTrackAssets = TextKeyPrefix + KeyFlagTrackAssets;
				/* Obsolete */	private const string TextKeyFlagDeleteMaterialUnreferenced = TextKeyPrefix + KeyFlagDeleteMaterialUnreferenced;
				private const string TextKeyFlagDisableInitialLightRenderer = TextKeyPrefix + KeyFlagDisableInitialLightRenderer;
				private const string TextKeyFlagTakeOverLightRenderer = TextKeyPrefix + KeyFlagTakeOverLightRenderer;
				private const string TextKeyFlagConvertSignal = TextKeyPrefix + KeyFlagConvertSignal;
				private const string TextKeyFlagIgnoreSetup = TextKeyPrefix + KeyFlagIgnoreSetup;
				private const string TextKeyFlagNestedPrefabUseScript = TextKeyPrefix + KeyFlagNestedPrefabUseScript;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagCreateControlGameObject = PrefsKeyPrefix + KeyFlagCreateControlGameObject;
				private const string PrefsKeyFlagCreateProjectFolder = PrefsKeyPrefix + KeyFlagCreateProjectFolder;
				private const string PrefsKeyFlagTextureReadable = PrefsKeyPrefix + KeyFlagTextureReadable;
				private const string PrefsKeyFlagCreateHolderAsset = PrefsKeyPrefix + KeyFlagCreateHolderAsset;
				private const string PrefsKeyFlagInvisibleToHideAll = PrefsKeyPrefix + KeyFlagInvisibleToHideAll;
				private const string PrefsKeyFlagTrackAssets = PrefsKeyPrefix + KeyFlagTrackAssets;
				/* Obsolete */	private const string PrefsKeyFlagDeleteMaterialUnreferenced = PrefsKeyPrefix + KeyFlagDeleteMaterialUnreferenced;
				private const string PrefsKeyFlagDisableInitialLightRenderer = PrefsKeyPrefix + KeyFlagDisableInitialLightRenderer;
				private const string PrefsKeyFlagTakeOverLightRenderer = PrefsKeyPrefix + KeyFlagTakeOverLightRenderer;
				private const string PrefsKeyFlagConvertSignal = PrefsKeyPrefix + KeyFlagConvertSignal;
				private const string PrefsKeyFlagIgnoreSetup = PrefsKeyPrefix + KeyFlagIgnoreSetup;
				private const string PrefsKeyFlagNestedPrefabUseScript = PrefsKeyPrefix + KeyFlagNestedPrefabUseScript;

				private readonly static GroupBasic Default = new GroupBasic(
					true,									/* FlagCreateControlGameObject */
					true,									/* FlagCreateProjectFolder */
					false,									/* FlagTextureReadable */
					true,									/* FlagCreateHolderAsset */
					false,									/* FlagInvisibleToHideAll */
					true,									/* FlagTrackAssets */
					false,									/* FlagDisableInitialLightRenderer */
					false,									/* FlagTakeOverLightRenderer */
					false,									/* FlagConvertSignal */
					false,									/* FlagIgnoreSetup */
					false									/* FlagNestedPrefabUseScript */
				);
				#endregion Enums & Constants
			}

			public struct GroupRuleNameAsset
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagAttachSpecificNameSSPJ;

				/* Prefix (Common) */
				public string NamePrefixTexture;

				/* Prefix for SS6PU */
				public string NamePrefixPrefabAnimationSS6PU;
				public string NamePrefixPrefabEffectSS6PU;
				public string NamePrefixDataProjectSS6PU;
				public string NamePrefixDataCellMapSS6PU;
				public string NamePrefixDataAnimationSS6PU;
				public string NamePrefixDataEffectSS6PU;
				public string NamePrefixDataSequenceSS6PU;

				/* Prefix Unity-Native */
				public string NamePrefixPrefabAnimationUnityNative;
				public string NamePrefixAnimationClipUnityNative;
				public string NamePrefixSkinnedMeshUnityNative;

				/* Prefix Unity-Native */
				public string NamePrefixPrefabAnimationUnityUI;
				public string NamePrefixAnimationClipUnityUI;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupRuleNameAsset(	bool flagAttachSpecificNameSSPJ,
											string namePrefixTexture,
											string namePrefixPrefabAnimationSS6PU,
											string namePrefixPrefabEffectSS6PU,
											string namePrefixDataProjectSS6PU,
											string namePrefixDataCellMapSS6PU,
											string namePrefixDataAnimationSS6PU,
											string namePrefixDataEffectSS6PU,
											string namePrefixDataSequenceSS6PU,
											string namePrefixPrefabAnimationUnityNative,
											string namePrefixAnimationClipUnityNative,
											string namePrefixSkinnedMeshUnityNative,
											string namePrefixPrefabAnimationUnityUI,
											string namePrefixAnimationClipUnityUI
										)
				{
					FlagAttachSpecificNameSSPJ = flagAttachSpecificNameSSPJ;

					NamePrefixTexture = namePrefixTexture;

					NamePrefixPrefabAnimationSS6PU = namePrefixPrefabAnimationSS6PU;
					NamePrefixPrefabEffectSS6PU =namePrefixPrefabEffectSS6PU ;
					NamePrefixDataProjectSS6PU = namePrefixDataProjectSS6PU;
					NamePrefixDataCellMapSS6PU = namePrefixDataCellMapSS6PU;
					NamePrefixDataAnimationSS6PU = namePrefixDataAnimationSS6PU;
					NamePrefixDataEffectSS6PU = namePrefixDataEffectSS6PU;
					NamePrefixDataSequenceSS6PU = namePrefixDataSequenceSS6PU;

					NamePrefixPrefabAnimationUnityNative = namePrefixPrefabAnimationUnityNative;
					NamePrefixAnimationClipUnityNative = namePrefixAnimationClipUnityNative;
					NamePrefixSkinnedMeshUnityNative = namePrefixSkinnedMeshUnityNative;

					NamePrefixPrefabAnimationUnityUI = namePrefixPrefabAnimationUnityUI;
					NamePrefixAnimationClipUnityUI = namePrefixAnimationClipUnityUI;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					FlagAttachSpecificNameSSPJ = EditorPrefs.GetBool(PrefsKeyFlagAttachSpecificNameSSPJ, Default.FlagAttachSpecificNameSSPJ);

					NamePrefixTexture = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixTexture, Default.NamePrefixTexture);

					NamePrefixPrefabAnimationSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixPrefabAnimationSS6PU, Default.NamePrefixPrefabAnimationSS6PU);
					NamePrefixPrefabEffectSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixPrefabEffectSS6PU, Default.NamePrefixPrefabEffectSS6PU);
					NamePrefixDataProjectSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixDataProjectSS6PU, Default.NamePrefixDataProjectSS6PU);
					NamePrefixDataCellMapSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixDataCellMapSS6PU, Default.NamePrefixDataCellMapSS6PU);
					NamePrefixDataAnimationSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixDataAnimationSS6PU, Default.NamePrefixDataAnimationSS6PU);
					NamePrefixDataEffectSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixDataEffectSS6PU, Default.NamePrefixDataEffectSS6PU);
					NamePrefixDataSequenceSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixDataSequenceSS6PU, Default.NamePrefixDataSequenceSS6PU);

					NamePrefixPrefabAnimationUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixPrefabAnimationUnityNative, Default.NamePrefixPrefabAnimationUnityNative);
					NamePrefixAnimationClipUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixAnimationClipUnityNative, Default.NamePrefixAnimationClipUnityNative);
					NamePrefixSkinnedMeshUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixSkinnedMeshUnityNative, Default.NamePrefixSkinnedMeshUnityNative);

					NamePrefixPrefabAnimationUnityUI = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixPrefabAnimationUnityUI, Default.NamePrefixPrefabAnimationUnityUI);
					NamePrefixAnimationClipUnityUI = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixAnimationClipUnityUI, Default.NamePrefixAnimationClipUnityUI);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagAttachSpecificNameSSPJ, FlagAttachSpecificNameSSPJ);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixTexture, NamePrefixTexture);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixPrefabAnimationSS6PU, NamePrefixPrefabAnimationSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixPrefabEffectSS6PU, NamePrefixPrefabEffectSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixDataProjectSS6PU, NamePrefixDataProjectSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixDataCellMapSS6PU, NamePrefixDataCellMapSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixDataAnimationSS6PU, NamePrefixDataAnimationSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixDataEffectSS6PU, NamePrefixDataEffectSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixDataSequenceSS6PU, NamePrefixDataSequenceSS6PU);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixPrefabAnimationUnityNative, NamePrefixPrefabAnimationUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixAnimationClipUnityNative, NamePrefixAnimationClipUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixSkinnedMeshUnityNative, NamePrefixSkinnedMeshUnityNative);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixPrefabAnimationUnityUI, NamePrefixPrefabAnimationUnityUI);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixAnimationClipUnityUI, NamePrefixAnimationClipUnityUI);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[14];
					string textValue;

					Adjust();

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagAttachSpecificNameSSPJ);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagAttachSpecificNameSSPJ, textValue);

					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixTexture, NamePrefixTexture);

					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixPrefabAnimationSS6PU, NamePrefixPrefabAnimationSS6PU);
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixPrefabEffectSS6PU, NamePrefixPrefabEffectSS6PU);
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixDataProjectSS6PU, NamePrefixDataProjectSS6PU);
					textEncode[5] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixDataCellMapSS6PU, NamePrefixDataCellMapSS6PU);
					textEncode[6] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixDataAnimationSS6PU, NamePrefixDataAnimationSS6PU);
					textEncode[7] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixDataEffectSS6PU, NamePrefixDataEffectSS6PU);
					textEncode[8] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixDataSequenceSS6PU, NamePrefixDataSequenceSS6PU);

					textEncode[9] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixPrefabAnimationUnityNative, NamePrefixPrefabAnimationUnityNative);
					textEncode[10] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixAnimationClipUnityNative, NamePrefixAnimationClipUnityNative);
					textEncode[11] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixSkinnedMeshUnityNative, NamePrefixSkinnedMeshUnityNative);

					textEncode[12] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixPrefabAnimationUnityUI, NamePrefixPrefabAnimationUnityUI);
					textEncode[13] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixAnimationClipUnityUI, NamePrefixAnimationClipUnityUI);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					switch(textArgument[0])
					{
						case TextKeyFlagAttachSpecificNameSSPJ:
							FlagAttachSpecificNameSSPJ = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyNamePrefixTexture:
							NamePrefixTexture = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);

						case TextKeyNamePrefixPrefabAnimationSS6PU:
							NamePrefixPrefabAnimationSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixPrefabEffectSS6PU:
							NamePrefixPrefabEffectSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixDataProjectSS6PU:
							NamePrefixDataProjectSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixDataCellMapSS6PU:
							NamePrefixDataCellMapSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixDataAnimationSS6PU:
							NamePrefixDataAnimationSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixDataEffectSS6PU:
							NamePrefixDataEffectSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixDataSequenceSS6PU:
							NamePrefixDataSequenceSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);

						case TextKeyNamePrefixPrefabAnimationUnityNative:
							NamePrefixPrefabAnimationUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixAnimationClipUnityNative:
							NamePrefixAnimationClipUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixSkinnedMeshUnityNative:
							NamePrefixSkinnedMeshUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);

						case TextKeyNamePrefixPrefabAnimationUnityUI:
							NamePrefixPrefabAnimationUnityUI = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixAnimationClipUnityUI:
							NamePrefixAnimationClipUnityUI = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);

						default:
							break;
					}
					return(false);
				}

				public void Adjust()
				{
//					FlagAttachSpecificNameSSPJ =

					NamePrefixTexture = Adjust(NamePrefixTexture);

					NamePrefixPrefabAnimationSS6PU = Adjust(NamePrefixPrefabAnimationSS6PU);
					NamePrefixPrefabEffectSS6PU = Adjust(NamePrefixPrefabEffectSS6PU);
					NamePrefixDataProjectSS6PU = Adjust(NamePrefixDataProjectSS6PU);
					NamePrefixDataCellMapSS6PU = Adjust(NamePrefixDataCellMapSS6PU);
					NamePrefixDataAnimationSS6PU = Adjust(NamePrefixDataAnimationSS6PU);
					NamePrefixDataEffectSS6PU = Adjust(NamePrefixDataEffectSS6PU);
					NamePrefixDataSequenceSS6PU = Adjust(NamePrefixDataSequenceSS6PU);

					NamePrefixPrefabAnimationUnityNative = Adjust(NamePrefixPrefabAnimationUnityNative);
					NamePrefixAnimationClipUnityNative = Adjust(NamePrefixAnimationClipUnityNative);
					NamePrefixSkinnedMeshUnityNative = Adjust(NamePrefixSkinnedMeshUnityNative);

					NamePrefixPrefabAnimationUnityUI = Adjust(NamePrefixPrefabAnimationUnityUI);
					NamePrefixAnimationClipUnityUI = Adjust(NamePrefixAnimationClipUnityUI);
				}

				public static string Adjust(string text)
				{
					return(LibraryEditor_SpriteStudio6.Import.Setting.AssetNameNormalize(text));
				}

				public string NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset kind, string nameBase, string nameSSPJ)
				{
					string name = nameBase;
					switch(kind)
					{
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.TEXTURE:
							name = NamePrefixTexture
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;

						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_CONTROL_ANIMATION_SS6PU:
							/* MEMO: (PrefabAnimation)_Control */
							name = NamePrefixPrefabAnimationSS6PU
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase
									+ "_Control";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_SS6PU:
							name = NamePrefixPrefabAnimationSS6PU
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_EFFECT_SS6PU:
							name = NamePrefixPrefabEffectSS6PU
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_PROJECT_SS6PU:
							name = NamePrefixDataProjectSS6PU
//									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_CELLMAP_SS6PU:
							name = NamePrefixDataCellMapSS6PU
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_SS6PU:
							name = NamePrefixDataAnimationSS6PU
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_EFFECT_SS6PU:
							name = NamePrefixDataEffectSS6PU
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_SEQUENCE_SS6PU:
							name = NamePrefixDataSequenceSS6PU
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;

						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_CONTROL_ANIMATION_UNITYNATIVE:
							/* MEMO: (PrefabAnimation)_Control */
							name = NamePrefixPrefabAnimationUnityNative
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase
									+ "_Control";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_UNITYNATIVE:
							name = NamePrefixPrefabAnimationUnityNative
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_UNITYNATIVE:
							name = NamePrefixAnimationClipUnityNative
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_MESH_UNITYNATIVE:
							name = NamePrefixSkinnedMeshUnityNative
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;

						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_UNITYUI:
							name = NamePrefixPrefabAnimationUnityUI
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_UNITYUI:
							name = NamePrefixAnimationClipUnityUI
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;

						default:
							return(null);
					}
					return(name);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyFlagAttachSpecificNameSSPJ = "FlagAttachSpecificNameSSPJ";
				private const string KeyNamePrefixTexture = "NamePrefixTexture";
				private const string KeyNamePrefixPrefabAnimationSS6PU = "NamePrefixPrefabAnimationSS6PU";
				private const string KeyNamePrefixPrefabEffectSS6PU = "NamePrefixPrefabEffectSS6PU";
				private const string KeyNamePrefixDataProjectSS6PU = "NamePrefixDataProjectSS6PU";
				private const string KeyNamePrefixDataCellMapSS6PU = "NamePrefixDataCellMapSS6PU";
				private const string KeyNamePrefixDataAnimationSS6PU = "NamePrefixDataAnimationSS6PU";
				private const string KeyNamePrefixDataEffectSS6PU = "NamePrefixDataEffectSS6PU";
				private const string KeyNamePrefixDataSequenceSS6PU = "NamePrefixDataSequenceSS6PU";
				/* Obsolete */	// private const string KeyNamePrefixMaterialAnimationSS6PU = "NamePrefixMaterialAnimationSS6PU";
				/* Obsolete */	// private const string KeyNamePrefixMaterialEffectSS6PU = "NamePrefixMaterialEffectSS6PU";
				private const string KeyNamePrefixPrefabAnimationUnityNative = "NamePrefixPrefabAnimatorUnityNative";	/* Typo: miss"Animator" / correct"Animation" */
				/* Obsolete */	// private const string KeyNamePrefixPrefabParticleUnityNative = "NamePrefixPrefabParticleUnityNative";
				private const string KeyNamePrefixAnimationClipUnityNative = "NamePrefixAnimationClipUnityNative";
				private const string KeyNamePrefixSkinnedMeshUnityNative = "NamePrefixSkinnedMeshUnityNative";
				/* Obsolete */	// private const string KeyNamePrefixMaterialAnimationUnityNative = "NamePrefixMaterialAnimatorUnityNative";	/* Typo: miss"Animator" / correct"Animation" */
				/* Obsolete */	// private const string KeyNamePrefixMaterialParticleUnityNative = "NamePrefixMaterialParticleUnityNative";
				private const string KeyNamePrefixPrefabAnimationUnityUI = "NamePrefixPrefabAnimationUnityUI";
				private const string KeyNamePrefixAnimationClipUnityUI = "NamePrefixAnimationClipUnityUI";
				/* Obsolete */	// private const string KeyNamePrefixMaterialAnimationUnityUI = "NamePrefixMaterialAnimationUnityUI";

				private const string TextKeyPrefix = "RuleNameAsset_";
				private const string TextKeyFlagAttachSpecificNameSSPJ = TextKeyPrefix + KeyFlagAttachSpecificNameSSPJ;
				private const string TextKeyNamePrefixTexture = TextKeyPrefix + KeyNamePrefixTexture;
				private const string TextKeyNamePrefixPrefabAnimationSS6PU = TextKeyPrefix + KeyNamePrefixPrefabAnimationSS6PU;
				private const string TextKeyNamePrefixPrefabEffectSS6PU = TextKeyPrefix + KeyNamePrefixPrefabEffectSS6PU;
				private const string TextKeyNamePrefixDataProjectSS6PU = TextKeyPrefix + KeyNamePrefixDataProjectSS6PU;
				private const string TextKeyNamePrefixDataCellMapSS6PU = TextKeyPrefix + KeyNamePrefixDataCellMapSS6PU;
				private const string TextKeyNamePrefixDataAnimationSS6PU = TextKeyPrefix + KeyNamePrefixDataAnimationSS6PU;
				private const string TextKeyNamePrefixDataEffectSS6PU = TextKeyPrefix + KeyNamePrefixDataEffectSS6PU;
				private const string TextKeyNamePrefixDataSequenceSS6PU = TextKeyPrefix + KeyNamePrefixDataSequenceSS6PU;
				/* Obsolete */	// private const string TextKeyNamePrefixMaterialAnimationSS6PU = TextKeyPrefix + KeyNamePrefixMaterialAnimationSS6PU;
				/* Obsolete */	// private const string TextKeyNamePrefixMaterialEffectSS6PU = TextKeyPrefix + KeyNamePrefixMaterialEffectSS6PU;
				private const string TextKeyNamePrefixPrefabAnimationUnityNative = TextKeyPrefix + KeyNamePrefixPrefabAnimationUnityNative;
				/* Obsolete */	// private const string TextKeyNamePrefixPrefabParticleUnityNative = TextKeyPrefix + KeyNamePrefixPrefabParticleUnityNative;
				private const string TextKeyNamePrefixAnimationClipUnityNative = TextKeyPrefix + KeyNamePrefixAnimationClipUnityNative;
				private const string TextKeyNamePrefixSkinnedMeshUnityNative = TextKeyPrefix + KeyNamePrefixSkinnedMeshUnityNative;
				/* Obsolete */	// private const string TextKeyNamePrefixMaterialAnimationUnityNative = TextKeyPrefix + KeyNamePrefixMaterialAnimationUnityNative;
				/* Obsolete */	// private const string TextKeyNamePrefixMaterialParticleUnityNative = TextKeyPrefix + KeyNamePrefixMaterialParticleUnityNative;
				private const string TextKeyNamePrefixPrefabAnimationUnityUI = TextKeyPrefix + KeyNamePrefixPrefabAnimationUnityUI;
				private const string TextKeyNamePrefixAnimationClipUnityUI = TextKeyPrefix + KeyNamePrefixAnimationClipUnityUI;
				/* Obsolete */	// private const string TextKeyNamePrefixMaterialAnimationUnityUI = TextKeyPrefix + KeyNamePrefixMaterialAnimationUnityUI;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagAttachSpecificNameSSPJ = PrefsKeyPrefix + KeyFlagAttachSpecificNameSSPJ;
				private const string PrefsKeyNamePrefixTexture = PrefsKeyPrefix + KeyNamePrefixTexture;
				private const string PrefsKeyNamePrefixPrefabAnimationSS6PU = PrefsKeyPrefix + KeyNamePrefixPrefabAnimationSS6PU;
				private const string PrefsKeyNamePrefixPrefabEffectSS6PU = PrefsKeyPrefix + KeyNamePrefixPrefabEffectSS6PU;
				private const string PrefsKeyNamePrefixDataProjectSS6PU = PrefsKeyPrefix + KeyNamePrefixDataProjectSS6PU;
				private const string PrefsKeyNamePrefixDataCellMapSS6PU = PrefsKeyPrefix + KeyNamePrefixDataCellMapSS6PU;
				private const string PrefsKeyNamePrefixDataAnimationSS6PU = PrefsKeyPrefix + KeyNamePrefixDataAnimationSS6PU;
				private const string PrefsKeyNamePrefixDataEffectSS6PU = PrefsKeyPrefix + KeyNamePrefixDataEffectSS6PU;
				private const string PrefsKeyNamePrefixDataSequenceSS6PU = PrefsKeyPrefix + KeyNamePrefixDataSequenceSS6PU;
				/* Obsolete */	// private const string PrefsKeyNamePrefixMaterialAnimationSS6PU = PrefsKeyPrefix + KeyNamePrefixMaterialAnimationSS6PU;
				/* Obsolete */	// private const string PrefsKeyNamePrefixMaterialEffectSS6PU = PrefsKeyPrefix + KeyNamePrefixMaterialEffectSS6PU;
				private const string PrefsKeyNamePrefixPrefabAnimationUnityNative = PrefsKeyPrefix + KeyNamePrefixPrefabAnimationUnityNative;
				/* Obsolete */	// private const string PrefsKeyNamePrefixPrefabParticleUnityNative = PrefsKeyPrefix + KeyNamePrefixPrefabParticleUnityNative;
				private const string PrefsKeyNamePrefixAnimationClipUnityNative = PrefsKeyPrefix + KeyNamePrefixAnimationClipUnityNative;
				private const string PrefsKeyNamePrefixSkinnedMeshUnityNative = PrefsKeyPrefix + KeyNamePrefixSkinnedMeshUnityNative;
				/* Obsolete */	// private const string PrefsKeyNamePrefixMaterialAnimationUnityNative = PrefsKeyPrefix + KeyNamePrefixMaterialAnimationUnityNative;
				/* Obsolete */	// private const string PrefsKeyNamePrefixMaterialParticleUnityNative = PrefsKeyPrefix + KeyNamePrefixMaterialParticleUnityNative;
				private const string PrefsKeyNamePrefixPrefabAnimationUnityUI = PrefsKeyPrefix + KeyNamePrefixPrefabAnimationUnityUI;
				private const string PrefsKeyNamePrefixAnimationClipUnityUI = PrefsKeyPrefix + KeyNamePrefixAnimationClipUnityUI;
				/* Obsolete */	// private const string PrefsKeyNamePrefixMaterialAnimationUnityUI = PrefsKeyPrefix + KeyNamePrefixMaterialAnimationUnityUI;

				private readonly static GroupRuleNameAsset Default = new GroupRuleNameAsset(
					false,	/* FlagAttachSpecificNameSSPJ */
					"",		/* NamePrefixTexture */
					"",		/* NamePrefixPrefabAnimationSS6PU */
					"pe_",	/* NamePrefixPrefabEffectSS6PU */
					"dp_",	/* NamePrefixDataProjectSS6PU */
					"dc_",	/* NamePrefixDataCellMapSS6PU */
					"da_",	/* NamePrefixDataAnimationSS6PU */
					"de_",	/* NamePrefixDataEffectSS6PU */
					"ds_",	/* NamePrefixDataSequenceSS6PU */
					"ps_",	/* NamePrefixPrefabAnimationUnityNative */
					"ac_",	/* NamePrefixAnimationClipUnityNative */
					"sm_",	/* NamePrefixSkinnedMeshUnityNative */
					"",		/* NamePrefixPrefabAnimationUnityUI */
					"au_"	/* NamePrefixAnimationClipUnityUI */
				);
				#endregion Enums & Constants
			}

			public struct GroupRuleNameAssetFolder
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				/* Folder Names (Common) */
				public string NameFolderTexture;

				/* Folder Names for SS6PU */
				public string NameFolderPrefabAnimationSS6PU;
				public string NameFolderPrefabEffectSS6PU;
				public string NameFolderDataProjectSS6PU;
				public string NameFolderDataCellMapSS6PU;
				public string NameFolderDataAnimationSS6PU;
				public string NameFolderDataEffectSS6PU;
				public string NameFolderDataSequenceSS6PU;

				/* Folder Names for Unity-Native */
				public string NameFolderPrefabAnimationUnityNative;
				public string NameFolderAnimationClipUnityNative;
				public string NameFolderSkinnedMeshUnityNative;

				/* Folder Names for Unity-UI */
				public string NameFolderAnimationClipUnityUI;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupRuleNameAssetFolder(	string nameFolderTexture,
													string nameFolderPrefabAnimationSS6PU,
													string nameFolderPrefabEffectSS6PU,
													string nameFolderDataProjectSS6PU,
													string nameFolderDataCellMapSS6PU,
													string nameFolderDataAnimationSS6PU,
													string nameFolderDataEffectSS6PU,
													string nameFolderDataSequenceSS6PU,
													string nameFolderPrefabAnimationUnityNative,
													string nameFolderAnimationClipUnityNative,
													string nameFolderSkinnedMeshUnityNative,
													string nameFolderAnimationClipUnityUI
												)
				{
						NameFolderTexture = nameFolderTexture;

						NameFolderPrefabAnimationSS6PU = nameFolderPrefabAnimationSS6PU;
						NameFolderPrefabEffectSS6PU = nameFolderPrefabEffectSS6PU;
						NameFolderDataProjectSS6PU = nameFolderDataProjectSS6PU;
						NameFolderDataCellMapSS6PU = nameFolderDataCellMapSS6PU;
						NameFolderDataAnimationSS6PU = nameFolderDataAnimationSS6PU;
						NameFolderDataEffectSS6PU = nameFolderDataEffectSS6PU;
						NameFolderDataSequenceSS6PU = nameFolderDataSequenceSS6PU;

						NameFolderPrefabAnimationUnityNative = nameFolderPrefabAnimationUnityNative;
						NameFolderAnimationClipUnityNative = nameFolderAnimationClipUnityNative;
						NameFolderSkinnedMeshUnityNative = nameFolderSkinnedMeshUnityNative;

						NameFolderAnimationClipUnityUI = nameFolderAnimationClipUnityUI;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					NameFolderTexture = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderTexture, Default.NameFolderTexture);

					NameFolderPrefabAnimationSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderPrefabAnimationSS6PU, Default.NameFolderPrefabAnimationSS6PU);
					NameFolderPrefabEffectSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderPrefabEffectSS6PU, Default.NameFolderPrefabEffectSS6PU);
					NameFolderDataProjectSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderDataProjectSS6PU, Default.NameFolderDataProjectSS6PU);
					NameFolderDataCellMapSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderDataCellMapSS6PU, Default.NameFolderDataCellMapSS6PU);
					NameFolderDataAnimationSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderDataAnimationSS6PU, Default.NameFolderDataAnimationSS6PU);
					NameFolderDataEffectSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderDataEffectSS6PU, Default.NameFolderDataEffectSS6PU);
					NameFolderDataSequenceSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderDataSequenceSS6PU, Default.NameFolderDataSequenceSS6PU);

					NameFolderPrefabAnimationUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderPrefabAnimationUnityNative, Default.NameFolderPrefabAnimationUnityNative);
					NameFolderAnimationClipUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderAnimationClipUnityNative, Default.NameFolderAnimationClipUnityNative);
					NameFolderSkinnedMeshUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderSkinnedMeshUnityNative, Default.NameFolderSkinnedMeshUnityNative);

					NameFolderAnimationClipUnityUI = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderAnimationClipUnityUI, Default.NameFolderAnimationClipUnityUI);

					Adjust();

					return(true);
				}

				public bool Save()
				{
					Adjust();

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderTexture, NameFolderTexture);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderPrefabAnimationSS6PU, NameFolderPrefabAnimationSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderPrefabEffectSS6PU, NameFolderPrefabEffectSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderDataProjectSS6PU, NameFolderDataProjectSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderDataCellMapSS6PU, NameFolderDataCellMapSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderDataAnimationSS6PU, NameFolderDataAnimationSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderDataEffectSS6PU, NameFolderDataEffectSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderDataSequenceSS6PU, NameFolderDataSequenceSS6PU);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderPrefabAnimationUnityNative, NameFolderPrefabAnimationUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderAnimationClipUnityNative, NameFolderAnimationClipUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderSkinnedMeshUnityNative, NameFolderSkinnedMeshUnityNative);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderAnimationClipUnityUI, NameFolderAnimationClipUnityUI);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[12];

					Adjust();

					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderTexture, NameFolderTexture);

					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderPrefabAnimationSS6PU, NameFolderPrefabAnimationSS6PU);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderPrefabEffectSS6PU, NameFolderPrefabEffectSS6PU);
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderDataProjectSS6PU, NameFolderDataProjectSS6PU);
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderDataCellMapSS6PU, NameFolderDataCellMapSS6PU);
					textEncode[5] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderDataAnimationSS6PU, NameFolderDataAnimationSS6PU);
					textEncode[6] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderDataEffectSS6PU, NameFolderDataEffectSS6PU);
					textEncode[7] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderDataSequenceSS6PU, NameFolderDataSequenceSS6PU);

					textEncode[8] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderPrefabAnimationUnityNative, NameFolderPrefabAnimationUnityNative);
					textEncode[9] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderAnimationClipUnityNative, NameFolderAnimationClipUnityNative);
					textEncode[10] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderSkinnedMeshUnityNative, NameFolderSkinnedMeshUnityNative);

					textEncode[11] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderAnimationClipUnityUI, NameFolderAnimationClipUnityUI);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					switch(textArgument[0])
					{
						case TextKeyNameFolderTexture:
							NameFolderTexture = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);

						case TextKeyNameFolderPrefabAnimationSS6PU:
							NameFolderPrefabAnimationSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderPrefabEffectSS6PU:
							NameFolderPrefabEffectSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderDataProjectSS6PU:
							NameFolderDataProjectSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderDataCellMapSS6PU:
							NameFolderDataCellMapSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderDataAnimationSS6PU:
							NameFolderDataAnimationSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderDataEffectSS6PU:
							NameFolderDataEffectSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderDataSequenceSS6PU:
							NameFolderDataSequenceSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);

						case TextKeyNameFolderPrefabAnimationUnityNative:
							NameFolderPrefabAnimationUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderAnimationClipUnityNative:
							NameFolderAnimationClipUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderSkinnedMeshUnityNative:
							NameFolderSkinnedMeshUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);

						case TextKeyNameFolderAnimationClipUnityUI:
							NameFolderAnimationClipUnityUI = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);

						default:
							break;
					}
					return(false);
				}

				public void Adjust()
				{
						NameFolderTexture = Adjust(NameFolderTexture);

						NameFolderPrefabAnimationSS6PU = Adjust(NameFolderPrefabAnimationSS6PU);
						NameFolderPrefabEffectSS6PU = Adjust(NameFolderPrefabEffectSS6PU);
						NameFolderDataProjectSS6PU = Adjust(NameFolderDataProjectSS6PU);
						NameFolderDataCellMapSS6PU = Adjust(NameFolderDataCellMapSS6PU);
						NameFolderDataAnimationSS6PU = Adjust(NameFolderDataAnimationSS6PU);
						NameFolderDataEffectSS6PU = Adjust(NameFolderDataEffectSS6PU);
						NameFolderDataSequenceSS6PU = Adjust(NameFolderDataSequenceSS6PU);

						NameFolderPrefabAnimationUnityNative = Adjust(NameFolderPrefabAnimationUnityNative);
						NameFolderAnimationClipUnityNative = Adjust(NameFolderAnimationClipUnityNative);
						NameFolderSkinnedMeshUnityNative = Adjust(NameFolderSkinnedMeshUnityNative);

						NameFolderAnimationClipUnityUI = Adjust(NameFolderAnimationClipUnityUI);
				}

				public static string Adjust(string text)
				{
					return(LibraryEditor_SpriteStudio6.Import.Setting.AssetNameNormalize(text));
				}

				public string NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset kind, string nameBase)
				{
					string name = nameBase;
					switch(kind)
					{
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.TEXTURE:
							name += NameFolderTexture + "/";
							break;

						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_CONTROL_ANIMATION_SS6PU:
							/* MEMO: Not stored in subfolder. */
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_SS6PU:
							name += NameFolderPrefabAnimationSS6PU + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_EFFECT_SS6PU:
							name += NameFolderPrefabEffectSS6PU + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_PROJECT_SS6PU:
							name += NameFolderDataProjectSS6PU + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_CELLMAP_SS6PU:
							name += NameFolderDataCellMapSS6PU + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_SS6PU:
							name += NameFolderDataAnimationSS6PU + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_EFFECT_SS6PU:
							name += NameFolderDataEffectSS6PU + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_SEQUENCE_SS6PU:
							name += NameFolderDataSequenceSS6PU + "/";
							break;

						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_CONTROL_ANIMATION_UNITYNATIVE:
							/* MEMO: Not stored in subfolder. */
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_UNITYNATIVE:
							name += NameFolderPrefabAnimationUnityNative + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_UNITYNATIVE:
							name += NameFolderAnimationClipUnityNative + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_MESH_UNITYNATIVE:
							name += NameFolderSkinnedMeshUnityNative + "/";
							break;

						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_UNITYUI:
							/* MEMO: Not stored in subfolder. */
							break;

						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_UNITYUI:
							name += NameFolderAnimationClipUnityUI + "/";
							break;

						default:
							return(null);
					}
					return(name);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyNameFolderTexture = "NameFolderTexture";
				private const string KeyNameFolderPrefabAnimationSS6PU = "NameFolderPrefabAnimationSS6PU";
				private const string KeyNameFolderPrefabEffectSS6PU = "NameFolderPrefabEffectSS6PU";
				private const string KeyNameFolderDataProjectSS6PU = "NameFolderDataProjectSS6PU";
				private const string KeyNameFolderDataCellMapSS6PU = "NameFolderDataCellMapSS6PU";
				private const string KeyNameFolderDataAnimationSS6PU = "NameFolderDataAnimationSS6PU";
				private const string KeyNameFolderDataEffectSS6PU = "NameFolderDataEffectSS6PU";
				private const string KeyNameFolderDataSequenceSS6PU = "NameFolderDataSequenceSS6PU";
				/* Obsolete */	// private const string KeyNameFolderMaterialAnimationSS6PU = "NameFolderMaterialAnimationSS6PU";
				/* Obsolete */	// private const string KeyNameFolderMaterialEffectSS6PU = "NameFolderMaterialEffectSS6PU";
				private const string KeyNameFolderPrefabAnimationUnityNative = "NameFolderPrefabAnimatorUnityNative";	/* Typo: miss"Animator" / correct"Animation" */
				/* Obsolete */	// private const string KeyNameFolderPrefabParticleUnityNative = "NameFolderPrefabParticleUnityNative";
				private const string KeyNameFolderAnimationClipUnityNative = "NameFolderAnimationClipUnityNative";
				private const string KeyNameFolderSkinnedMeshUnityNative = "NameFolderSkinnedMeshUnityNative";
				/* Obsolete */	// private const string KeyNameFolderMaterialAnimationUnityNative = "NameFolderMaterialAnimatorUnityNative";	/* Typo: miss"Animator" / correct"Animation" */
				/* Obsolete */	// private const string KeyNameFolderMaterialParticleUnityNative = "NameFolderMaterialParticleUnityNative";
				private const string KeyNameFolderAnimationClipUnityUI = "NameFolderAnimationClipUnityUI";
				/* Obsolete */	// private const string KeyNameFolderMaterialAnimationUnityUI = "NameFolderMaterialAnimatorUnityUI";

				private const string TextKeyPrefix = "RuleNameAssetFolder_";
				private const string TextKeyNameFolderTexture = TextKeyPrefix + KeyNameFolderTexture;
				private const string TextKeyNameFolderPrefabAnimationSS6PU = TextKeyPrefix + KeyNameFolderPrefabAnimationSS6PU;
				private const string TextKeyNameFolderPrefabEffectSS6PU = TextKeyPrefix + KeyNameFolderPrefabEffectSS6PU;
				private const string TextKeyNameFolderDataProjectSS6PU = TextKeyPrefix + KeyNameFolderDataProjectSS6PU;
				private const string TextKeyNameFolderDataCellMapSS6PU = TextKeyPrefix + KeyNameFolderDataCellMapSS6PU;
				private const string TextKeyNameFolderDataAnimationSS6PU = TextKeyPrefix + KeyNameFolderDataAnimationSS6PU;
				private const string TextKeyNameFolderDataEffectSS6PU = TextKeyPrefix + KeyNameFolderDataEffectSS6PU;
				private const string TextKeyNameFolderDataSequenceSS6PU = TextKeyPrefix + KeyNameFolderDataSequenceSS6PU;
				/* Obsolete */	// private const string TextKeyNameFolderMaterialAnimationSS6PU = TextKeyPrefix + KeyNameFolderMaterialAnimationSS6PU;
				/* Obsolete */	// private const string TextKeyNameFolderMaterialEffectSS6PU = TextKeyPrefix + KeyNameFolderMaterialEffectSS6PU;
				private const string TextKeyNameFolderPrefabAnimationUnityNative = TextKeyPrefix + KeyNameFolderPrefabAnimationUnityNative;
				/* Obsolete */	// private const string TextKeyNameFolderPrefabParticleUnityNative = TextKeyPrefix + KeyNameFolderPrefabParticleUnityNative;
				private const string TextKeyNameFolderAnimationClipUnityNative = TextKeyPrefix + KeyNameFolderAnimationClipUnityNative;
				private const string TextKeyNameFolderSkinnedMeshUnityNative = TextKeyPrefix + KeyNameFolderSkinnedMeshUnityNative;
				/* Obsolete */	// private const string TextKeyNameFolderMaterialAnimationUnityNative = TextKeyPrefix + KeyNameFolderMaterialAnimationUnityNative;
				/* Obsolete */	// private const string TextKeyNameFolderMaterialParticleUnityNative = TextKeyPrefix + KeyNameFolderMaterialParticleUnityNative;
				private const string TextKeyNameFolderAnimationClipUnityUI = TextKeyPrefix + KeyNameFolderAnimationClipUnityUI;
				/* Obsolete */	// private const string TextKeyNameFolderMaterialAnimationUnityUI = TextKeyPrefix + KeyNameFolderMaterialAnimationUnityUI;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyNameFolderTexture = PrefsKeyPrefix + KeyNameFolderTexture;
				private const string PrefsKeyNameFolderPrefabAnimationSS6PU = PrefsKeyPrefix + KeyNameFolderPrefabAnimationSS6PU;
				private const string PrefsKeyNameFolderPrefabEffectSS6PU = PrefsKeyPrefix + KeyNameFolderPrefabEffectSS6PU;
				private const string PrefsKeyNameFolderDataProjectSS6PU = PrefsKeyPrefix + KeyNameFolderDataProjectSS6PU;
				private const string PrefsKeyNameFolderDataCellMapSS6PU = PrefsKeyPrefix + KeyNameFolderDataCellMapSS6PU;
				private const string PrefsKeyNameFolderDataAnimationSS6PU = PrefsKeyPrefix + KeyNameFolderDataAnimationSS6PU;
				private const string PrefsKeyNameFolderDataEffectSS6PU = PrefsKeyPrefix + KeyNameFolderDataEffectSS6PU;
				private const string PrefsKeyNameFolderDataSequenceSS6PU = PrefsKeyPrefix + KeyNameFolderDataSequenceSS6PU;
				/* Obsolete */	// private const string PrefsKeyNameFolderMaterialAnimationSS6PU = PrefsKeyPrefix + KeyNameFolderMaterialAnimationSS6PU;
				/* Obsolete */	// private const string PrefsKeyNameFolderMaterialEffectSS6PU = PrefsKeyPrefix + KeyNameFolderMaterialEffectSS6PU;
				private const string PrefsKeyNameFolderPrefabAnimationUnityNative = PrefsKeyPrefix + KeyNameFolderPrefabAnimationUnityNative;
				/* Obsolete */	// private const string PrefsKeyNameFolderPrefabParticleUnityNative = PrefsKeyPrefix + KeyNameFolderPrefabParticleUnityNative;
				private const string PrefsKeyNameFolderAnimationClipUnityNative = PrefsKeyPrefix + KeyNameFolderAnimationClipUnityNative;
				private const string PrefsKeyNameFolderSkinnedMeshUnityNative = PrefsKeyPrefix + KeyNameFolderSkinnedMeshUnityNative;
				/* Obsolete */	// private const string PrefsKeyNameFolderMaterialAnimationUnityNative = PrefsKeyPrefix + KeyNameFolderMaterialAnimationUnityNative;
				/* Obsolete */	// private const string PrefsKeyNameFolderMaterialParticleUnityNative = PrefsKeyPrefix + KeyNameFolderMaterialParticleUnityNative;
				private const string PrefsKeyNameFolderAnimationClipUnityUI = PrefsKeyPrefix + KeyNameFolderAnimationClipUnityUI;
				/* Obsolete */	// private const string PrefsKeyNameFolderMaterialAnimationUnityUI = PrefsKeyPrefix + KeyNameFolderMaterialAnimationUnityUI;

				private readonly static GroupRuleNameAssetFolder Default = new GroupRuleNameAssetFolder(
					"Texture",				/* NameFolderTexture */
					"PrefabAnimation",		/* NameFolderPrefabAnimationSS6PU */
					"PrefabEffect",			/* NameFolderPrefabEffectSS6PU */
					"DataProject",			/* NameFolderDataProjectSS6PU */
					"DataCellMap",			/* NameFolderDataCellMapSS6PU */
					"DataAnimation",		/* NameFolderDataAnimationSS6PU */
					"DataEffect",			/* NameFolderDataEffectSS6PU */
					"DataSequence",			/* NameFolderDataSequenceSS6PU */
					"PrefabSprite",			/* NameFolderPrefabAnimationUnityNative */
					"DataAnimationClip",	/* NameFolderAnimationClipUnityNative */
					"DataMesh",				/* NameFolderSkinnedMeshUnityNative */
					"DataAnimationClip"		/* NameFolderAnimationClipUnityUI */
				);
				#endregion Enums & Constants
			}

			public struct GroupPackAttributeAnimation
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Status;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Cell;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Position;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Rotation;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Scaling;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack ScalingLocal;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack RateOpacity;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Priority;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PartsColor;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack VertexCorrection;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack OffsetPivot;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PositionAnchor;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack RadiusCollision;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack SizeForce;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PositionTexture;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack RotationTexture;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack ScalingTexture;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack UserData;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Instance;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Effect;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Deform;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Shader;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Signal;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupPackAttributeAnimation(	Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack status,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack cell,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack position,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack rotation,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack scaling,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack scalingLocal,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack rateOpacity,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack priority,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack partsColor,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack vertexCorrection,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack offsetPivot,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack positionAnchor,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack sizeForce,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack positionTexture,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack rotationTexture,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack scalingTexture,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack radiusCollision,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack userData,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack instance,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack effect,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack deform,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack shader,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack signal
												)
				{
					Status = status;
					Cell = cell;
					Position = position;
					Rotation = rotation;
					Scaling = scaling;
					ScalingLocal = scalingLocal;
					RateOpacity = rateOpacity;
					Priority = priority;
					PartsColor = partsColor;
					VertexCorrection = vertexCorrection;
					OffsetPivot = offsetPivot;
					PositionAnchor = positionAnchor;
					SizeForce = sizeForce;
					PositionTexture = positionTexture;
					RotationTexture = rotationTexture;
					ScalingTexture = scalingTexture;
					RadiusCollision = radiusCollision;
					UserData = userData;
					Instance = instance;
					Effect = effect;
					Deform = deform;
					Shader = shader;
					Signal = signal;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					Status = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyStatus, (int)Default.Status));
					Cell = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyCell, (int)Default.Cell));
					Position = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPosition, (int)Default.Position));
					Rotation = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyRotation, (int)Default.Rotation));
					Scaling = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyScaling, (int)Default.Scaling));
					ScalingLocal = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyScalingLocal, (int)Default.ScalingLocal));
					RateOpacity = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyRateOpacity, (int)Default.RateOpacity));
					Priority = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPriority, (int)Default.Priority));
					PartsColor = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPartsColor, (int)Default.PartsColor));
					VertexCorrection = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyVertexCorrection, (int)Default.VertexCorrection));
					OffsetPivot = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyOffsetPivot, (int)Default.OffsetPivot));
					PositionAnchor = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPositionAnchor, (int)Default.PositionAnchor));
					SizeForce = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeySizeForce, (int)Default.SizeForce));
					PositionTexture = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPositionTexture, (int)Default.PositionTexture));
					RotationTexture = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyRotationTexture, (int)Default.RotationTexture));
					ScalingTexture = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyScalingTexture, (int)Default.ScalingTexture));
					RadiusCollision = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyRadiusCollision, (int)Default.RadiusCollision));
					UserData = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyUserData, (int)Default.UserData));
					Instance = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyInstance, (int)Default.Instance));
					Effect = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyEffect, (int)Default.Effect));
					Deform = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyDeform, (int)Default.Deform));
					Shader = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyShader, (int)Default.Shader));
					Signal = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeySignal, (int)Default.Signal));

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetInt(PrefsKeyStatus, (int)Status);
					EditorPrefs.SetInt(PrefsKeyPosition, (int)Position);
					EditorPrefs.SetInt(PrefsKeyRotation, (int)Rotation);
					EditorPrefs.SetInt(PrefsKeyScaling, (int)Scaling);
					EditorPrefs.SetInt(PrefsKeyScalingLocal, (int)ScalingLocal);
					EditorPrefs.SetInt(PrefsKeyRateOpacity, (int)RateOpacity);
					EditorPrefs.SetInt(PrefsKeyPriority, (int)Priority);
					EditorPrefs.SetInt(PrefsKeyPartsColor, (int)PartsColor);
					EditorPrefs.SetInt(PrefsKeyPositionAnchor, (int)PositionAnchor);
					EditorPrefs.SetInt(PrefsKeyRadiusCollision, (int)RadiusCollision);
					EditorPrefs.SetInt(PrefsKeyUserData, (int)UserData);
					EditorPrefs.SetInt(PrefsKeyInstance, (int)Instance);
					EditorPrefs.SetInt(PrefsKeyEffect, (int)Effect);
					EditorPrefs.SetInt(PrefsKeyDeform, (int)Deform);
					EditorPrefs.SetInt(PrefsKeyShader, (int)Shader);
					EditorPrefs.SetInt(PrefsKeySignal, (int)Signal);

					EditorPrefs.SetInt(PrefsKeyCell, (int)Cell);
					EditorPrefs.SetInt(PrefsKeySizeForce, (int)SizeForce);
					EditorPrefs.SetInt(PrefsKeyVertexCorrection, (int)VertexCorrection);
					EditorPrefs.SetInt(PrefsKeyOffsetPivot, (int)OffsetPivot);
					EditorPrefs.SetInt(PrefsKeyPositionTexture, (int)PositionTexture);
					EditorPrefs.SetInt(PrefsKeyScalingTexture, (int)ScalingTexture);
					EditorPrefs.SetInt(PrefsKeyRotationTexture, (int)RotationTexture);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[23];
					string textValue;

					textValue = NameGetPackKind(Status);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyStatus, textValue);

					textValue = NameGetPackKind(Cell);
					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyCell, textValue);

					textValue = NameGetPackKind(Position);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPosition, textValue);

					textValue = NameGetPackKind(Rotation);
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyRotation, textValue);

					textValue = NameGetPackKind(Scaling);
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyScaling, textValue);

					textValue = NameGetPackKind(ScalingLocal);
					textEncode[5] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyScalingLocal, textValue);

					textValue = NameGetPackKind(RateOpacity);
					textEncode[6] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyRateOpacity, textValue);

					textValue = NameGetPackKind(Priority);
					textEncode[7] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPriority, textValue);

					textValue = NameGetPackKind(PartsColor);
					textEncode[8] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPartsColor, textValue);

					textValue = NameGetPackKind(VertexCorrection);
					textEncode[9] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyVertexCorrection, textValue);

					textValue = NameGetPackKind(OffsetPivot);
					textEncode[10] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyOffsetPivot, textValue);

					textValue = NameGetPackKind(PositionAnchor);
					textEncode[11] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPositionAnchor, textValue);

					textValue = NameGetPackKind(SizeForce);
					textEncode[12] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySizeForce, textValue);

					textValue = NameGetPackKind(PositionTexture);
					textEncode[13] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPositionTexture, textValue);

					textValue = NameGetPackKind(RotationTexture);
					textEncode[14] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyRotationTexture, textValue);

					textValue = NameGetPackKind(ScalingTexture);
					textEncode[15] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyScalingTexture, textValue);

					textValue = NameGetPackKind(RadiusCollision);
					textEncode[16] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyRadiusCollision, textValue);

					textValue = NameGetPackKind(UserData);
					textEncode[17] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyUserData, textValue);

					textValue = NameGetPackKind(Instance);
					textEncode[18] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyInstance, textValue);

					textValue = NameGetPackKind(Effect);
					textEncode[19] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyEffect, textValue);

					textValue = NameGetPackKind(Deform);
					textEncode[20] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyDeform, textValue);

					textValue = NameGetPackKind(Shader);
					textEncode[21] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyShader, textValue);

					textValue = NameGetPackKind(Signal);
					textEncode[22] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySignal, textValue);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					switch(textArgument[0])
					{
						case TextKeyStatus:
							Status = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainCell:	/* Obsolete command */
						case TextKeyCell:
							Cell = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPosition:
							Position = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyRotation:
							Rotation = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyScaling:
							Scaling = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyScalingLocal:
							ScalingLocal = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyRateOpacity:
							RateOpacity = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPriority:
							Priority = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPartsColor:
							PartsColor = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainVertexCorrection:	/* Obsolete command */
						case TextKeyVertexCorrection:
							VertexCorrection = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainOffsetPivot:	/* Obsolete command */
						case TextKeyOffsetPivot:
							OffsetPivot = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPositionAnchor:
							PositionAnchor = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainSizeForce:	/* Obsolete command */
						case TextKeySizeForce:
							SizeForce = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainPositionTexture:	/* Obsolete command */
						case TextKeyPositionTexture:
							PositionTexture = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainRotationTexture:	/* Obsolete command */
						case TextKeyRotationTexture:
							RotationTexture = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainScalingTexture:	/* Obsolete command */
						case TextKeyScalingTexture:
							ScalingTexture = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyRadiusCollision:
							RadiusCollision = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyUserData:
							UserData = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyInstance:
							Instance = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyEffect:
							Effect = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyDeform:
							Deform = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyShader:
							Shader = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeySignal:
							Signal = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyFixIndexCellMap:	/* Obsolete command */
						case TextKeyFixCoordinate:	/* Obsolete command */
						case TextKeyFixUV0:	/* Obsolete command */
						case TextKeyFixSizeCollision:	/* Obsolete command */
						case TextKeyFixPivotCollision:	/* Obsolete command */
							return(true);

						default:
							break;
					}
					return(false);
				}

				public void Adjust()
				{
					int countPack = (int)Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.TERMINATOR;
					Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer[] capacityPack = new Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityContainer[countPack];
					for(int i=0; i<countPack; i++)
					{
						capacityPack[i] = Library_SpriteStudio6.Data.Animation.PackAttribute.CapacityGet((Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)i);
					}

					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PackError = Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE;	/* all Attributes Support */
					if(false == capacityPack[(int)Status].Status)
					{
						Status = PackError;
					}
					if(false == capacityPack[(int)Cell].Cell)
					{
						Cell = PackError;
					}
					if(false == capacityPack[(int)Position].Position)
					{
						Position = PackError;
					}
					if(false == capacityPack[(int)Rotation].Rotation)
					{
						Rotation = PackError;
					}
					if(false == capacityPack[(int)Scaling].Scaling)
					{
						Scaling = PackError;
					}
					if(false == capacityPack[(int)ScalingLocal].ScalingLocal)
					{
						ScalingLocal = PackError;
					}
					if(false == capacityPack[(int)RateOpacity].RateOpacity)
					{
						RateOpacity = PackError;
					}
					if(false == capacityPack[(int)Priority].Priority)
					{
						Priority = PackError;
					}
					if(false == capacityPack[(int)PartsColor].PartsColor)
					{
						PartsColor = PackError;
					}
					if(false == capacityPack[(int)VertexCorrection].VertexCorrection)
					{
						VertexCorrection = PackError;
					}
					if(false == capacityPack[(int)OffsetPivot].OffsetPivot)
					{
						OffsetPivot = PackError;
					}
					if(false == capacityPack[(int)PositionAnchor].PositionAnchor)
					{
						PositionAnchor = PackError;
					}
					if(false == capacityPack[(int)SizeForce].SizeForce)
					{
						SizeForce = PackError;
					}
					if(false == capacityPack[(int)PositionTexture].PositionTexture)
					{
						PositionTexture = PackError;
					}
					if(false == capacityPack[(int)RotationTexture].RotationTexture)
					{
						RotationTexture = PackError;
					}
					if(false == capacityPack[(int)ScalingTexture].ScalingTexture)
					{
						ScalingTexture = PackError;
					}
					if(false == capacityPack[(int)RadiusCollision].RadiusCollision)
					{
						RadiusCollision = PackError;
					}
					if(false == capacityPack[(int)UserData].UserData)
					{
						UserData = PackError;
					}
					if(false == capacityPack[(int)Instance].Instance)
					{
						Instance = PackError;
					}
					if(false == capacityPack[(int)Effect].Effect)
					{
						Effect = PackError;
					}
					if(false == capacityPack[(int)Deform].Deform)
					{
						Deform = PackError;
					}
					if(false == capacityPack[(int)Shader].Shader)
					{
						Deform = PackError;
					}
					if(false == capacityPack[(int)Signal].Signal)
					{
						Deform = PackError;
					}
				}

				private static void BootUpNamePack()
				{
					int countPack = (int)Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.TERMINATOR;
					if(null == TableNamePack)
					{
						TableNamePack = new string[countPack];
						for(int i=0; i<countPack; i++)
						{
							TableNamePack[i] = Library_SpriteStudio6.Data.Animation.PackAttribute.IDGetPack((Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)i);
							
						}
					}
				}
				private static string NameGetPackKind(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
				{
					BootUpNamePack();
					return(TableNamePack[(int)pack]);
				}
				private static Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack KindGetPackName(string name)
				{
					BootUpNamePack();
					int count = TableNamePack.Length;
					for(int i=0; i<count; i++)
					{
						if(name == TableNamePack[i])
						{
							return((Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)i);
						}
					}
					return((Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(-1));
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private static string[] TableNamePack = null;

				private const string KeyStatus = "Status";
				private const string KeyCell = "Cell";
				private const string KeyPosition = "Position";
				private const string KeyRotation = "Rotation";
				private const string KeyScaling = "Scaling";
				private const string KeyScalingLocal = "ScalingLocal";
				private const string KeyRateOpacity = "RateOpacity";
				private const string KeyPriority = "Priority";
				private const string KeyPartsColor = "PartsColor";
				private const string KeyVertexCorrection = "VertexCorrection";
				private const string KeyOffsetPivot = "OffsetPivot";
				private const string KeyPositionAnchor = "PositionAnchor";
				private const string KeySizeForce = "SizeForce";
				private const string KeyPositionTexture = "PositionTexture";
				private const string KeyRotationTexture = "RotationTexture";
				private const string KeyScalingTexture = "ScalingTexture";
				private const string KeyRadiusCollision = "RadiusCollision";
				private const string KeyUserData = "UserData";
				private const string KeyInstance = "Instance";
				private const string KeyEffect = "Effect";
				private const string KeyDeform = "Deform";
				private const string KeyShader = "Shader";
				private const string KeySignal = "Signal";
				/* Obsolete */	private const string KeyPlainCell = "PlainCell";
				/* Obsolete */	private const string KeyPlainSizeForce = "PlainSizeForce";
				/* Obsolete */	private const string KeyPlainVertexCorrection = "PlainVertexCorrection";
				/* Obsolete */	private const string KeyPlainOffsetPivot = "PlainOffsetPivot";
				/* Obsolete */	private const string KeyPlainPositionTexture = "PlainPositionTexture";
				/* Obsolete */	private const string KeyPlainScalingTexture = "PlainScalingTexture";
				/* Obsolete */	private const string KeyPlainRotationTexture = "PlainRotationTexture";
				/* Obsolete */	private const string KeyFixIndexCellMap = "FixIndexCellMap";
				/* Obsolete */	private const string KeyFixCoordinate = "FixCoordinate";
				/* Obsolete */	private const string KeyFixUV0 = "FixUV0";
				/* Obsolete */	private const string KeyFixSizeCollision = "FixSizeCollision";
				/* Obsolete */	private const string KeyFixPivotCollision = "FixPivotCollision";

				private const string TextKeyPrefix = "PackAttributeAnimation_";
				private const string TextKeyStatus = TextKeyPrefix + KeyStatus;
				private const string TextKeyCell = TextKeyPrefix + KeyCell;
				private const string TextKeyPosition = TextKeyPrefix + KeyPosition;
				private const string TextKeyRotation = TextKeyPrefix + KeyRotation;
				private const string TextKeyScaling = TextKeyPrefix + KeyScaling;
				private const string TextKeyScalingLocal = TextKeyPrefix + KeyScalingLocal;
				private const string TextKeyRateOpacity = TextKeyPrefix + KeyRateOpacity;
				private const string TextKeyPriority = TextKeyPrefix + KeyPriority;
				private const string TextKeyPartsColor = TextKeyPrefix + KeyPartsColor;
				private const string TextKeyVertexCorrection = TextKeyPrefix + KeyVertexCorrection;
				private const string TextKeyOffsetPivot = TextKeyPrefix + KeyOffsetPivot;
				private const string TextKeyPositionAnchor = TextKeyPrefix + KeyPositionAnchor;
				private const string TextKeySizeForce = TextKeyPrefix + KeySizeForce;
				private const string TextKeyPositionTexture = TextKeyPrefix + KeyPositionTexture;
				private const string TextKeyRotationTexture = TextKeyPrefix + KeyRotationTexture;
				private const string TextKeyScalingTexture = TextKeyPrefix + KeyScalingTexture;
				private const string TextKeyRadiusCollision = TextKeyPrefix + KeyRadiusCollision;
				private const string TextKeyUserData = TextKeyPrefix + KeyUserData;
				private const string TextKeyInstance = TextKeyPrefix + KeyInstance;
				private const string TextKeyEffect = TextKeyPrefix + KeyEffect;
				private const string TextKeyDeform = TextKeyPrefix + KeyDeform;
				private const string TextKeyShader = TextKeyPrefix + KeyShader;
				private const string TextKeySignal = TextKeyPrefix + KeySignal;
				/* Obsolete */	private const string TextKeyPlainCell = TextKeyPrefix + KeyPlainCell;
				/* Obsolete */	private const string TextKeyPlainSizeForce = TextKeyPrefix + KeyPlainSizeForce;
				/* Obsolete */	private const string TextKeyPlainVertexCorrection = TextKeyPrefix + KeyPlainVertexCorrection;
				/* Obsolete */	private const string TextKeyPlainOffsetPivot = TextKeyPrefix + KeyPlainOffsetPivot;
				/* Obsolete */	private const string TextKeyPlainPositionTexture = TextKeyPrefix + KeyPlainPositionTexture;
				/* Obsolete */	private const string TextKeyPlainScalingTexture = TextKeyPrefix + KeyPlainScalingTexture;
				/* Obsolete */	private const string TextKeyPlainRotationTexture = TextKeyPrefix + KeyPlainRotationTexture;
				/* Obsolete */	private const string TextKeyFixIndexCellMap = TextKeyPrefix + KeyFixIndexCellMap;
				/* Obsolete */	private const string TextKeyFixCoordinate = TextKeyPrefix + KeyFixCoordinate;
				/* Obsolete */	private const string TextKeyFixUV0 = TextKeyPrefix + KeyFixUV0;
				/* Obsolete */	private const string TextKeyFixSizeCollision = TextKeyPrefix + KeyFixSizeCollision;
				/* Obsolete */	private const string TextKeyFixPivotCollision = TextKeyPrefix + KeyFixPivotCollision;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyStatus = PrefsKeyPrefix + KeyStatus;
				private const string PrefsKeyCell = PrefsKeyPrefix + KeyCell;
				private const string PrefsKeyPosition = PrefsKeyPrefix + KeyPosition;
				private const string PrefsKeyRotation = PrefsKeyPrefix + KeyRotation;
				private const string PrefsKeyScaling = PrefsKeyPrefix + KeyScaling;
				private const string PrefsKeyScalingLocal = PrefsKeyPrefix + KeyScalingLocal;
				private const string PrefsKeyRateOpacity = PrefsKeyPrefix + KeyRateOpacity;
				private const string PrefsKeyPriority = PrefsKeyPrefix + KeyPriority;
				private const string PrefsKeyPartsColor = PrefsKeyPrefix + KeyPartsColor;
				private const string PrefsKeyVertexCorrection = PrefsKeyPrefix + KeyVertexCorrection;
				private const string PrefsKeyOffsetPivot = PrefsKeyPrefix + KeyOffsetPivot;
				private const string PrefsKeyPositionAnchor = PrefsKeyPrefix + KeyPositionAnchor;
				private const string PrefsKeySizeForce = PrefsKeyPrefix + KeySizeForce;
				private const string PrefsKeyPositionTexture = PrefsKeyPrefix + KeyPositionTexture;
				private const string PrefsKeyRotationTexture = PrefsKeyPrefix + KeyRotationTexture;
				private const string PrefsKeyScalingTexture = PrefsKeyPrefix + KeyScalingTexture;
				private const string PrefsKeyRadiusCollision = PrefsKeyPrefix + KeyRadiusCollision;
				private const string PrefsKeyUserData = PrefsKeyPrefix + KeyUserData;
				private const string PrefsKeyInstance = PrefsKeyPrefix + KeyInstance;
				private const string PrefsKeyEffect = PrefsKeyPrefix + KeyEffect;
				private const string PrefsKeyDeform = PrefsKeyPrefix + KeyDeform;
				private const string PrefsKeyShader = PrefsKeyPrefix + KeyShader;
				private const string PrefsKeySignal = PrefsKeyPrefix + KeySignal;
				/* Obsolete */	private const string PrefsKeyPlainCell = PrefsKeyPrefix + KeyPlainCell;
				/* Obsolete */	private const string PrefsKeyPlainSizeForce = PrefsKeyPrefix + KeyPlainSizeForce;
				/* Obsolete */	private const string PrefsKeyPlainVertexCorrection = PrefsKeyPrefix + KeyPlainVertexCorrection;
				/* Obsolete */	private const string PrefsKeyPlainOffsetPivot = PrefsKeyPrefix + KeyPlainOffsetPivot;
				/* Obsolete */	private const string PrefsKeyPlainPositionTexture = PrefsKeyPrefix + KeyPlainPositionTexture;
				/* Obsolete */	private const string PrefsKeyPlainScalingTexture = PrefsKeyPrefix + KeyPlainScalingTexture;
				/* Obsolete */	private const string PrefsKeyPlainRotationTexture = PrefsKeyPrefix + KeyPlainRotationTexture;
				/* Obsolete */	private const string PrefsKeyFixIndexCellMap = PrefsKeyPrefix + KeyFixIndexCellMap;
				/* Obsolete */	private const string PrefsKeyFixCoordinate = PrefsKeyPrefix + KeyFixCoordinate;
				/* Obsolete */	private const string PrefsKeyFixUV0 = PrefsKeyPrefix + KeyFixUV0;
				/* Obsolete */	private const string PrefsKeyFixSizeCollision = PrefsKeyPrefix + KeyFixSizeCollision;
				/* Obsolete */	private const string PrefsKeyFixPivotCollision = PrefsKeyPrefix + KeyFixPivotCollision;

				private readonly static GroupPackAttributeAnimation Default = new GroupPackAttributeAnimation(
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* Status */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* Cell */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* Position */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* Rotation */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* Scaling */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* ScalingLocal */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* RateOpacity */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* Priority */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* PartsColor */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* VertexCorrection */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* OffsetPivot */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* PositionAnchor */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* SizeForce */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* PositionTexture */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* RotationTexture */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* ScalingTexture */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* RadiusCollision */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* UserData */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* Instance */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* Effect */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.CPE_INTERPOLATE,	/* Deform */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,		/* Shader */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE		/* Signal */
				);
				#endregion Enums & Constants
			}

			public struct GroupPresetMaterial
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public Material AnimationUnityNativeMix;
				public Material AnimationUnityNativeAdd;
				public Material AnimationUnityNativeSub;
				public Material AnimationUnityNativeMul;
				public Material AnimationUnityNativeMulNA;
				public Material AnimationUnityNativeScr;
				public Material AnimationUnityNativeExc;
				public Material AnimationUnityNativeInv;
				public Material AnimationUnityNativeNonBatchMix;
				public Material AnimationUnityNativeNonBatchAdd;
				public Material AnimationUnityNativeNonBatchSub;
				public Material AnimationUnityNativeNonBatchMul;
				public Material AnimationUnityNativeNonBatchMulNA;
				public Material AnimationUnityNativeNonBatchScr;
				public Material AnimationUnityNativeNonBatchExc;
				public Material AnimationUnityNativeNonBatchInv;

				public Material SkinnedMeshUnityNativeMix;
				public Material SkinnedMeshUnityNativeAdd;
				public Material SkinnedMeshUnityNativeSub;
				public Material SkinnedMeshUnityNativeMul;
				public Material SkinnedMeshUnityNativeMulNA;
				public Material SkinnedMeshUnityNativeScr;
				public Material SkinnedMeshUnityNativeExc;
				public Material SkinnedMeshUnityNativeInv;

				public Material AnimationUnityUI;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupPresetMaterial(	string animationUnityNativeMix,
											string animationUnityNativeAdd,
											string animationUnityNativeSub,
											string animationUnityNativeMul,
											string animationUnityNativeMulNA,
											string animationUnityNativeScr,
											string animationUnityNativeExc,
											string animationUnityNativeInv,
											string animationUnityNativeNonBatchMix,
											string animationUnityNativeNonBatchAdd,
											string animationUnityNativeNonBatchSub,
											string animationUnityNativeNonBatchMul,
											string animationUnityNativeNonBatchMulNA,
											string animationUnityNativeNonBatchScr,
											string animationUnityNativeNonBatchExc,
											string animationUnityNativeNonBatchInv,
											string skinnedMeshUnityNativeMix,
											string skinnedMeshUnityNativeAdd,
											string skinnedMeshUnityNativeSub,
											string skinnedMeshUnityNativeMul,
											string skinnedMeshUnityNativeMulNA,
											string skinnedMeshUnityNativeScr,
											string skinnedMeshUnityNativeExc,
											string skinnedMeshUnityNativeInv,
											string animationUnityUI
										)
				{
					AnimationUnityNativeMix = MaterlalGetPath(animationUnityNativeMix);
					AnimationUnityNativeAdd = MaterlalGetPath(animationUnityNativeAdd);
					AnimationUnityNativeSub = MaterlalGetPath(animationUnityNativeSub);
					AnimationUnityNativeMul = MaterlalGetPath(animationUnityNativeMul);
					AnimationUnityNativeMulNA = MaterlalGetPath(animationUnityNativeMulNA);
					AnimationUnityNativeScr = MaterlalGetPath(animationUnityNativeScr);
					AnimationUnityNativeExc = MaterlalGetPath(animationUnityNativeExc);
					AnimationUnityNativeInv = MaterlalGetPath(animationUnityNativeInv);

					AnimationUnityNativeNonBatchMix = MaterlalGetPath(animationUnityNativeNonBatchMix);
					AnimationUnityNativeNonBatchAdd = MaterlalGetPath(animationUnityNativeNonBatchAdd);
					AnimationUnityNativeNonBatchSub = MaterlalGetPath(animationUnityNativeNonBatchSub);
					AnimationUnityNativeNonBatchMul = MaterlalGetPath(animationUnityNativeNonBatchMul);
					AnimationUnityNativeNonBatchMulNA = MaterlalGetPath(animationUnityNativeNonBatchMulNA);
					AnimationUnityNativeNonBatchScr = MaterlalGetPath(animationUnityNativeNonBatchScr);
					AnimationUnityNativeNonBatchExc = MaterlalGetPath(animationUnityNativeNonBatchExc);
					AnimationUnityNativeNonBatchInv = MaterlalGetPath(animationUnityNativeNonBatchInv);

					SkinnedMeshUnityNativeMix = MaterlalGetPath(skinnedMeshUnityNativeMix);
					SkinnedMeshUnityNativeAdd = MaterlalGetPath(skinnedMeshUnityNativeAdd);
					SkinnedMeshUnityNativeSub = MaterlalGetPath(skinnedMeshUnityNativeSub);
					SkinnedMeshUnityNativeMul = MaterlalGetPath(skinnedMeshUnityNativeMul);
					SkinnedMeshUnityNativeMulNA = MaterlalGetPath(skinnedMeshUnityNativeMulNA);
					SkinnedMeshUnityNativeScr = MaterlalGetPath(skinnedMeshUnityNativeScr);
					SkinnedMeshUnityNativeExc = MaterlalGetPath(skinnedMeshUnityNativeExc);
					SkinnedMeshUnityNativeInv = MaterlalGetPath(skinnedMeshUnityNativeInv);

					AnimationUnityUI = MaterlalGetPath(animationUnityUI);
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					string guid = "";

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeMix, GUIDGetMaterial(Default.AnimationUnityNativeMix));
					AnimationUnityNativeMix = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeAdd, GUIDGetMaterial(Default.AnimationUnityNativeAdd));
					AnimationUnityNativeAdd = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeSub, GUIDGetMaterial(Default.AnimationUnityNativeSub));
					AnimationUnityNativeSub = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeMul, GUIDGetMaterial(Default.AnimationUnityNativeMul));
					AnimationUnityNativeMul = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeMulNA, GUIDGetMaterial(Default.AnimationUnityNativeMulNA));
					AnimationUnityNativeMulNA = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeScr, GUIDGetMaterial(Default.AnimationUnityNativeScr));
					AnimationUnityNativeScr = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeExc, GUIDGetMaterial(Default.AnimationUnityNativeExc));
					AnimationUnityNativeExc = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeInv, GUIDGetMaterial(Default.AnimationUnityNativeInv));
					AnimationUnityNativeInv = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeNonBatchMix, GUIDGetMaterial(Default.AnimationUnityNativeNonBatchMix));
					AnimationUnityNativeNonBatchMix = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeNonBatchAdd, GUIDGetMaterial(Default.AnimationUnityNativeNonBatchAdd));
					AnimationUnityNativeNonBatchAdd = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeNonBatchSub, GUIDGetMaterial(Default.AnimationUnityNativeNonBatchSub));
					AnimationUnityNativeNonBatchSub = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeNonBatchMul, GUIDGetMaterial(Default.AnimationUnityNativeNonBatchMul));
					AnimationUnityNativeNonBatchMul = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeNonBatchMulNA, GUIDGetMaterial(Default.AnimationUnityNativeNonBatchMulNA));
					AnimationUnityNativeNonBatchMulNA = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeNonBatchScr, GUIDGetMaterial(Default.AnimationUnityNativeNonBatchScr));
					AnimationUnityNativeNonBatchScr = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeNonBatchExc, GUIDGetMaterial(Default.AnimationUnityNativeNonBatchExc));
					AnimationUnityNativeNonBatchExc = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityNativeNonBatchInv, GUIDGetMaterial(Default.AnimationUnityNativeNonBatchInv));
					AnimationUnityNativeNonBatchInv = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeySkinnedMeshUnityNativeMix, GUIDGetMaterial(Default.SkinnedMeshUnityNativeMix));
					SkinnedMeshUnityNativeMix = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeySkinnedMeshUnityNativeAdd, GUIDGetMaterial(Default.SkinnedMeshUnityNativeAdd));
					SkinnedMeshUnityNativeAdd = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeySkinnedMeshUnityNativeSub, GUIDGetMaterial(Default.SkinnedMeshUnityNativeSub));
					SkinnedMeshUnityNativeSub = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeySkinnedMeshUnityNativeMul, GUIDGetMaterial(Default.SkinnedMeshUnityNativeMul));
					SkinnedMeshUnityNativeMul = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeySkinnedMeshUnityNativeMulNA, GUIDGetMaterial(Default.SkinnedMeshUnityNativeMulNA));
					SkinnedMeshUnityNativeMulNA = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeySkinnedMeshUnityNativeScr, GUIDGetMaterial(Default.SkinnedMeshUnityNativeScr));
					SkinnedMeshUnityNativeScr = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeySkinnedMeshUnityNativeExc, GUIDGetMaterial(Default.SkinnedMeshUnityNativeExc));
					SkinnedMeshUnityNativeExc = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeySkinnedMeshUnityNativeInv, GUIDGetMaterial(Default.SkinnedMeshUnityNativeInv));
					SkinnedMeshUnityNativeInv = MaterialGetGUID(guid);

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyAnimationUnityUI, GUIDGetMaterial(Default.AnimationUnityUI));
					AnimationUnityUI = MaterialGetGUID(guid);

					return(true);
				}

				public bool Save()
				{
					string guid = "";

					guid = GUIDGetMaterial(AnimationUnityNativeMix);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeMix, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeAdd);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeAdd, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeSub);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeSub, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeMul);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeMul, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeMulNA);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeMulNA, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeScr);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeScr, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeExc);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeExc, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeInv);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeInv, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeNonBatchMix);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeNonBatchMix, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeNonBatchAdd);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeNonBatchAdd, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeNonBatchSub);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeNonBatchSub, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeNonBatchMul);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeNonBatchMul, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeNonBatchMulNA);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeNonBatchMulNA, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeNonBatchScr);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeNonBatchScr, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeNonBatchExc);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeNonBatchExc, guid);

					guid = GUIDGetMaterial(AnimationUnityNativeNonBatchInv);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityNativeNonBatchInv, guid);

					guid = GUIDGetMaterial(SkinnedMeshUnityNativeMix);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeySkinnedMeshUnityNativeMix, guid);

					guid = GUIDGetMaterial(SkinnedMeshUnityNativeAdd);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeySkinnedMeshUnityNativeAdd, guid);

					guid = GUIDGetMaterial(SkinnedMeshUnityNativeSub);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeySkinnedMeshUnityNativeSub, guid);

					guid = GUIDGetMaterial(SkinnedMeshUnityNativeMul);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeySkinnedMeshUnityNativeMul, guid);

					guid = GUIDGetMaterial(SkinnedMeshUnityNativeMulNA);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeySkinnedMeshUnityNativeMulNA, guid);

					guid = GUIDGetMaterial(SkinnedMeshUnityNativeScr);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeySkinnedMeshUnityNativeScr, guid);

					guid = GUIDGetMaterial(SkinnedMeshUnityNativeExc);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeySkinnedMeshUnityNativeExc, guid);

					guid = GUIDGetMaterial(SkinnedMeshUnityNativeInv);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeySkinnedMeshUnityNativeInv, guid);

					guid = GUIDGetMaterial(AnimationUnityUI);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyAnimationUnityUI, guid);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[25];
					string textValue;

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeMix));
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeMix, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeAdd));
					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeAdd, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeSub));
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeSub, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeMul));
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeMul, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeMulNA));
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeMulNA, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeScr));
					textEncode[5] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeScr, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeExc));
					textEncode[6] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeExc, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeInv));
					textEncode[7] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeInv, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeNonBatchMix));
					textEncode[8] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeNonBatchMix, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeNonBatchAdd));
					textEncode[9] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeNonBatchAdd, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeNonBatchSub));
					textEncode[10] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeNonBatchSub, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeNonBatchMul));
					textEncode[11] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeNonBatchMul, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeNonBatchMulNA));
					textEncode[12] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeNonBatchMulNA, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeNonBatchScr));
					textEncode[13] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeNonBatchScr, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeNonBatchExc));
					textEncode[14] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeNonBatchExc, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityNativeNonBatchInv));
					textEncode[15] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityNativeNonBatchInv, textValue);

					textValue = PathGetForExport(PathGetMaterial(SkinnedMeshUnityNativeMix));
					textEncode[16] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySkinnedMeshUnityNativeMix, textValue);

					textValue = PathGetForExport(PathGetMaterial(SkinnedMeshUnityNativeAdd));
					textEncode[17] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySkinnedMeshUnityNativeAdd, textValue);

					textValue = PathGetForExport(PathGetMaterial(SkinnedMeshUnityNativeSub));
					textEncode[18] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySkinnedMeshUnityNativeSub, textValue);

					textValue = PathGetForExport(PathGetMaterial(SkinnedMeshUnityNativeMul));
					textEncode[19] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySkinnedMeshUnityNativeMul, textValue);

					textValue = PathGetForExport(PathGetMaterial(SkinnedMeshUnityNativeMulNA));
					textEncode[20] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySkinnedMeshUnityNativeMulNA, textValue);

					textValue = PathGetForExport(PathGetMaterial(SkinnedMeshUnityNativeScr));
					textEncode[21] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySkinnedMeshUnityNativeScr, textValue);

					textValue = PathGetForExport(PathGetMaterial(SkinnedMeshUnityNativeExc));
					textEncode[22] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySkinnedMeshUnityNativeExc, textValue);

					textValue = PathGetForExport(PathGetMaterial(SkinnedMeshUnityNativeInv));
					textEncode[23] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySkinnedMeshUnityNativeInv, textValue);

					textValue = PathGetForExport(PathGetMaterial(AnimationUnityUI));
					textEncode[24] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyAnimationUnityUI, textValue);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					string namePathMaterial = textArgument[1];
					switch(textArgument[0])
					{
						case TextKeyAnimationUnityNativeMix:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeMix));
							}
							AnimationUnityNativeMix = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeAdd:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeAdd));
							}
							AnimationUnityNativeAdd = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeSub:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeSub));
							}
							AnimationUnityNativeSub = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeMul:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeMul));
							}
							AnimationUnityNativeMul = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeMulNA:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeMulNA));
							}
							AnimationUnityNativeMulNA = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeScr:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeScr));
							}
							AnimationUnityNativeScr = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeExc:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeExc));
							}
							AnimationUnityNativeExc = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeInv:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeInv));
							}
							AnimationUnityNativeInv = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeNonBatchMix:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeNonBatchMix));
							}
							AnimationUnityNativeNonBatchMix = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeNonBatchAdd:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeNonBatchAdd));
							}
							AnimationUnityNativeNonBatchAdd = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeNonBatchSub:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeNonBatchSub));
							}
							AnimationUnityNativeNonBatchSub = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeNonBatchMul:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeNonBatchMul));
							}
							AnimationUnityNativeNonBatchMul = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeNonBatchMulNA:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeNonBatchMulNA));
							}
							AnimationUnityNativeNonBatchMulNA = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeNonBatchScr:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeNonBatchScr));
							}
							AnimationUnityNativeNonBatchScr = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeNonBatchExc:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeNonBatchExc));
							}
							AnimationUnityNativeNonBatchExc = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityNativeNonBatchInv:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityNativeNonBatchInv));
							}
							AnimationUnityNativeNonBatchInv = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeySkinnedMeshUnityNativeMix:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.SkinnedMeshUnityNativeMix));
							}
							SkinnedMeshUnityNativeMix = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeySkinnedMeshUnityNativeAdd:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.SkinnedMeshUnityNativeAdd));
							}
							SkinnedMeshUnityNativeAdd = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeySkinnedMeshUnityNativeSub:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.SkinnedMeshUnityNativeSub));
							}
							SkinnedMeshUnityNativeSub = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeySkinnedMeshUnityNativeMul:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.SkinnedMeshUnityNativeMul));
							}
							SkinnedMeshUnityNativeMul = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeySkinnedMeshUnityNativeMulNA:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.SkinnedMeshUnityNativeMulNA));
							}
							SkinnedMeshUnityNativeMulNA = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeySkinnedMeshUnityNativeScr:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.SkinnedMeshUnityNativeScr));
							}
							SkinnedMeshUnityNativeScr = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeySkinnedMeshUnityNativeExc:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.SkinnedMeshUnityNativeExc));
							}
							SkinnedMeshUnityNativeExc = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeySkinnedMeshUnityNativeInv:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.SkinnedMeshUnityNativeInv));
							}
							SkinnedMeshUnityNativeInv = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						case TextKeyAnimationUnityUI:
							if(true == string.IsNullOrEmpty(namePathMaterial))
							{
								namePathMaterial = PathGetForExport(PathGetMaterial(Default.AnimationUnityUI));
							}
							AnimationUnityUI = MaterlalGetPath(PathGetForImport(namePathMaterial));
							return(true);

						default:
							break;
					}

					return(false);
				}

				public void AssetRecover(string pathBase)
				{
					if(true == string.IsNullOrEmpty(pathBase))
					{
						pathBase = NamePathSubPreset;
					}
					const string delimiterPath = "/";
					if(false == pathBase.EndsWith(delimiterPath))
					{
						pathBase += delimiterPath;
					}

					/* Reset Materials */
					AnimationUnityNativeMix = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeMix);
					AnimationUnityNativeAdd = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeAdd);
					AnimationUnityNativeSub = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeSub);
					AnimationUnityNativeMul = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeMul);
					AnimationUnityNativeMulNA = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeMulNA);
					AnimationUnityNativeScr = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeScr);
					AnimationUnityNativeExc = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeExc);
					AnimationUnityNativeInv = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeInv);

					AnimationUnityNativeNonBatchMix = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeNonBatchMix);
					AnimationUnityNativeNonBatchAdd = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeNonBatchAdd);
					AnimationUnityNativeNonBatchSub = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeNonBatchSub);
					AnimationUnityNativeNonBatchMul = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeNonBatchMul);
					AnimationUnityNativeNonBatchMulNA = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeNonBatchMulNA);
					AnimationUnityNativeNonBatchScr = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeNonBatchScr);
					AnimationUnityNativeNonBatchExc = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeNonBatchExc);
					AnimationUnityNativeNonBatchInv = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeNonBatchInv);

					SkinnedMeshUnityNativeMix = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeSkinnedMeshMix);
					SkinnedMeshUnityNativeAdd = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeSkinnedMeshAdd);
					SkinnedMeshUnityNativeSub = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeSkinnedMeshSub);
					SkinnedMeshUnityNativeMul = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeSkinnedMeshMul);
					SkinnedMeshUnityNativeMulNA = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeSkinnedMeshMulNA);
					SkinnedMeshUnityNativeScr = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeSkinnedMeshScr);
					SkinnedMeshUnityNativeExc = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeSkinnedMeshExc);
					SkinnedMeshUnityNativeInv = MaterlalGetPath(pathBase + NameFileBodyPresetUnityNativeSkinnedMeshInv);

					/* MEMO: Save information of redefined assets */
					Save();
				}

				public void AssetRecoverUI(string pathBase)
				{
					if(true == string.IsNullOrEmpty(pathBase))
					{
						pathBase = NamePathSubPresetUI;
					}
					const string delimiterPath = "/";
					if(false == pathBase.EndsWith(delimiterPath))
					{
						pathBase += delimiterPath;
					}

					/* Reset Materials */
					AnimationUnityUI = MaterlalGetPath(pathBase + NameFileBodyPresetUnityUI);

					/* MEMO: Save information of redefined assets */
					Save();
				}

				internal string PathFolderGetValidMaterial()
				{
					Material material = null;

					/* Find Valid Material */
					if(null != AnimationUnityNativeMix)
					{
						material = AnimationUnityNativeMix;
					}
					else if(null == AnimationUnityNativeAdd)
					{
						material = AnimationUnityNativeAdd;
					}
					else if(null == AnimationUnityNativeSub)
					{
						material = AnimationUnityNativeSub;
					}
					else if(null == AnimationUnityNativeMul)
					{
						material = AnimationUnityNativeMul;
					}
					else if(null == AnimationUnityNativeMulNA)
					{
						material = AnimationUnityNativeMulNA;
					}
					else if(null == AnimationUnityNativeScr)
					{
						material = AnimationUnityNativeScr;
					}
					else if(null == AnimationUnityNativeExc)
					{
						material = AnimationUnityNativeExc;
					}
					else if(null == AnimationUnityNativeInv)
					{
						material = AnimationUnityNativeInv;
					}
					else if(null == AnimationUnityNativeNonBatchMix)
					{
						material = AnimationUnityNativeNonBatchMix;
					}
					else if(null == AnimationUnityNativeNonBatchAdd)
					{
						material = AnimationUnityNativeNonBatchAdd;
					}
					else if(null == AnimationUnityNativeNonBatchSub)
					{
						material = AnimationUnityNativeNonBatchSub;
					}
					else if(null == AnimationUnityNativeNonBatchMul)
					{
						material = AnimationUnityNativeNonBatchMul;
					}
					else if(null == AnimationUnityNativeNonBatchMulNA)
					{
						material = AnimationUnityNativeNonBatchMulNA;
					}
					else if(null == AnimationUnityNativeNonBatchScr)
					{
						material = AnimationUnityNativeNonBatchScr;
					}
					else if(null == AnimationUnityNativeNonBatchExc)
					{
						material = AnimationUnityNativeNonBatchExc;
					}
					else if(null == AnimationUnityNativeNonBatchInv)
					{
						material = AnimationUnityNativeNonBatchInv;
					}
					else if(null == SkinnedMeshUnityNativeMix)
					{
						material = SkinnedMeshUnityNativeMix;
					}
					else if(null == SkinnedMeshUnityNativeAdd)
					{
						material = SkinnedMeshUnityNativeAdd;
					}
					else if(null == SkinnedMeshUnityNativeSub)
					{
						material = SkinnedMeshUnityNativeSub;
					}
					else if(null == SkinnedMeshUnityNativeMul)
					{
						material = SkinnedMeshUnityNativeMul;
					}
					else if(null == SkinnedMeshUnityNativeMulNA)
					{
						material = SkinnedMeshUnityNativeMulNA;
					}
					else if(null == SkinnedMeshUnityNativeScr)
					{
						material = SkinnedMeshUnityNativeScr;
					}
					else if(null == SkinnedMeshUnityNativeExc)
					{
						material = SkinnedMeshUnityNativeExc;
					}
					else if(null == SkinnedMeshUnityNativeInv)
					{
						material = SkinnedMeshUnityNativeInv;
					}

					/* Get and Split Material's path */
					string path = null;
					if(null != material)
					{
						string pathMaterial = AssetDatabase.GetAssetPath(material);
						if(null != pathMaterial)
						{
							string nameFileBody;
							string nameExtention;
							LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out path, out nameFileBody, out nameExtention, pathMaterial);
						}
					}

					return(path);
				}

				internal string PathFolderGetValidMaterialUI()
				{
					Material material = null;

					if(null != AnimationUnityUI)
					{
						material = AnimationUnityUI;
					}

					/* Get and Split Material's path */
					string path = null;
					if(null != material)
					{
						string pathMaterial = AssetDatabase.GetAssetPath(material);
						if(null != pathMaterial)
						{
							string nameFileBody;
							string nameExtention;
							LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out path, out nameFileBody, out nameExtention, pathMaterial);
						}
					}

					return(path);
				}

				/* MEMO: For access by short name. */
				private static Material MaterlalGetPath(string path)
				{
					if(true == string.IsNullOrEmpty(path))
					{
						return(null);
					}

					return(LibraryEditor_SpriteStudio6.Utility.File.AssetGetPath<Material>(path));
				}
				private static Material MaterialGetGUID(string guid)
				{
					return(LibraryEditor_SpriteStudio6.Utility.File.AssetGetGUID<Material>(guid));
				}
				private static string PathGetMaterial(Material material)
				{
					if(null == material)
					{
						return(string.Empty);
					}

					return(LibraryEditor_SpriteStudio6.Utility.File.PathGetAsset(material));
				}
				private static string GUIDGetMaterial(Material material)
				{
					return(LibraryEditor_SpriteStudio6.Utility.File.GUIDGetAsset(material));
				}

				/* MEMO: For path adjustment. */
				private static string PathGetForImport(string namePath)
				{
					string name = namePath;
					if(false == name.StartsWith("/"))
					{
						name = "/" + name;
					}
					return(LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + name);
				}
				private static string PathGetForExport(string namePath)
				{
					if(true == string.IsNullOrEmpty(namePath))
					{
						return(string.Empty);
					}
					return(namePath.Remove(0, LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset.Length));
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyKindAnimation = "Animation";
				private const string KeyKindEffect = "Effect";
				private const string KeyKindSkinnedMesh = "SkinnedMesh";

				private const string KeyModeSS6PU = "SS6PU";
				private const string KeyModeUnityNative = "UnityNative";
				private const string KeyModeUnityUI = "UnityUI";

				private const string KeyMaskingMask = "Mask";
				private const string KeyMaskingThrough = "Through";

				private const string KeyNonBatch = "NonBatch";

				private const string KeyOperationStencilPreDraw = "StencilPreDraw";
				private const string KeyOperationStencilDraw = "StencilDraw";
				private const string KeyOperationMix = "Mix";
				private const string KeyOperationAdd = "Add";
				private const string KeyOperationSub = "Sub";
				private const string KeyOperationMul = "Mul";
				private const string KeyOperationMulNA = "MulNA";
				private const string KeyOperationScr = "Scr";
				private const string KeyOperationExc = "Exc";
				private const string KeyOperationInv = "Inv";

				private const string KeyAnimationUnityNativeMix = KeyKindAnimation + KeyModeUnityNative + KeyOperationMix;
				private const string KeyAnimationUnityNativeAdd = KeyKindAnimation + KeyModeUnityNative + KeyOperationAdd;
				private const string KeyAnimationUnityNativeSub = KeyKindAnimation + KeyModeUnityNative + KeyOperationSub;
				private const string KeyAnimationUnityNativeMul = KeyKindAnimation + KeyModeUnityNative + KeyOperationMul;
				private const string KeyAnimationUnityNativeMulNA = KeyKindAnimation + KeyModeUnityNative + KeyOperationMulNA;
				private const string KeyAnimationUnityNativeScr = KeyKindAnimation + KeyModeUnityNative + KeyOperationScr;
				private const string KeyAnimationUnityNativeExc = KeyKindAnimation + KeyModeUnityNative + KeyOperationExc;
				private const string KeyAnimationUnityNativeInv = KeyKindAnimation + KeyModeUnityNative + KeyOperationInv;
				private const string KeyAnimationUnityNativeNonBatchMix = KeyKindAnimation + KeyModeUnityNative + KeyNonBatch + KeyOperationMix;
				private const string KeyAnimationUnityNativeNonBatchAdd = KeyKindAnimation + KeyModeUnityNative + KeyNonBatch + KeyOperationAdd;
				private const string KeyAnimationUnityNativeNonBatchSub = KeyKindAnimation + KeyModeUnityNative + KeyNonBatch + KeyOperationSub;
				private const string KeyAnimationUnityNativeNonBatchMul = KeyKindAnimation + KeyModeUnityNative + KeyNonBatch + KeyOperationMul;
				private const string KeyAnimationUnityNativeNonBatchMulNA = KeyKindAnimation + KeyModeUnityNative + KeyNonBatch + KeyOperationMulNA;
				private const string KeyAnimationUnityNativeNonBatchScr = KeyKindAnimation + KeyModeUnityNative + KeyNonBatch + KeyOperationScr;
				private const string KeyAnimationUnityNativeNonBatchExc = KeyKindAnimation + KeyModeUnityNative + KeyNonBatch + KeyOperationExc;
				private const string KeyAnimationUnityNativeNonBatchInv = KeyKindAnimation + KeyModeUnityNative + KeyNonBatch + KeyOperationInv;
				private const string KeySkinnedMeshUnityNativeMix = KeyKindSkinnedMesh + KeyModeUnityNative + KeyOperationMix;
				private const string KeySkinnedMeshUnityNativeAdd = KeyKindSkinnedMesh + KeyModeUnityNative + KeyOperationAdd;
				private const string KeySkinnedMeshUnityNativeSub = KeyKindSkinnedMesh + KeyModeUnityNative + KeyOperationSub;
				private const string KeySkinnedMeshUnityNativeMul = KeyKindSkinnedMesh + KeyModeUnityNative + KeyOperationMul;
				private const string KeySkinnedMeshUnityNativeMulNA = KeyKindSkinnedMesh + KeyModeUnityNative + KeyOperationMulNA;
				private const string KeySkinnedMeshUnityNativeScr = KeyKindSkinnedMesh + KeyModeUnityNative + KeyOperationScr;
				private const string KeySkinnedMeshUnityNativeExc = KeyKindSkinnedMesh + KeyModeUnityNative + KeyOperationExc;
				private const string KeySkinnedMeshUnityNativeInv = KeyKindSkinnedMesh + KeyModeUnityNative + KeyOperationInv;
				private const string KeyAnimationUnityUI = KeyKindAnimation + KeyModeUnityUI + KeyOperationMix;

				private const string TextKeyPrefix = "PresetMaterial_";

				private const string TextKeyAnimationUnityNativeMix = TextKeyPrefix + KeyAnimationUnityNativeMix;
				private const string TextKeyAnimationUnityNativeAdd = TextKeyPrefix + KeyAnimationUnityNativeAdd;
				private const string TextKeyAnimationUnityNativeSub = TextKeyPrefix + KeyAnimationUnityNativeSub;
				private const string TextKeyAnimationUnityNativeMul = TextKeyPrefix + KeyAnimationUnityNativeMul;
				private const string TextKeyAnimationUnityNativeMulNA = TextKeyPrefix + KeyAnimationUnityNativeMulNA;
				private const string TextKeyAnimationUnityNativeScr = TextKeyPrefix + KeyAnimationUnityNativeScr;
				private const string TextKeyAnimationUnityNativeExc = TextKeyPrefix + KeyAnimationUnityNativeExc;
				private const string TextKeyAnimationUnityNativeInv = TextKeyPrefix + KeyAnimationUnityNativeInv;
				private const string TextKeyAnimationUnityNativeNonBatchMix = TextKeyPrefix + KeyAnimationUnityNativeNonBatchMix;
				private const string TextKeyAnimationUnityNativeNonBatchAdd = TextKeyPrefix + KeyAnimationUnityNativeNonBatchAdd;
				private const string TextKeyAnimationUnityNativeNonBatchSub = TextKeyPrefix + KeyAnimationUnityNativeNonBatchSub;
				private const string TextKeyAnimationUnityNativeNonBatchMul = TextKeyPrefix + KeyAnimationUnityNativeNonBatchMul;
				private const string TextKeyAnimationUnityNativeNonBatchMulNA = TextKeyPrefix + KeyAnimationUnityNativeNonBatchMulNA;
				private const string TextKeyAnimationUnityNativeNonBatchScr = TextKeyPrefix + KeyAnimationUnityNativeNonBatchScr;
				private const string TextKeyAnimationUnityNativeNonBatchExc = TextKeyPrefix + KeyAnimationUnityNativeNonBatchExc;
				private const string TextKeyAnimationUnityNativeNonBatchInv = TextKeyPrefix + KeyAnimationUnityNativeNonBatchInv;
				private const string TextKeySkinnedMeshUnityNativeMix = TextKeyPrefix + KeySkinnedMeshUnityNativeMix;
				private const string TextKeySkinnedMeshUnityNativeAdd = TextKeyPrefix + KeySkinnedMeshUnityNativeAdd;
				private const string TextKeySkinnedMeshUnityNativeSub = TextKeyPrefix + KeySkinnedMeshUnityNativeSub;
				private const string TextKeySkinnedMeshUnityNativeMul = TextKeyPrefix + KeySkinnedMeshUnityNativeMul;
				private const string TextKeySkinnedMeshUnityNativeMulNA = TextKeyPrefix + KeySkinnedMeshUnityNativeMulNA;
				private const string TextKeySkinnedMeshUnityNativeScr = TextKeyPrefix + KeySkinnedMeshUnityNativeScr;
				private const string TextKeySkinnedMeshUnityNativeExc = TextKeyPrefix + KeySkinnedMeshUnityNativeExc;
				private const string TextKeySkinnedMeshUnityNativeInv = TextKeyPrefix + KeySkinnedMeshUnityNativeInv;
				private const string TextKeyAnimationUnityUI = TextKeyPrefix + KeyAnimationUnityUI;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;

				private const string PrefsKeyAnimationUnityNativeMix = PrefsKeyPrefix + KeyAnimationUnityNativeMix;
				private const string PrefsKeyAnimationUnityNativeAdd = PrefsKeyPrefix + KeyAnimationUnityNativeAdd;
				private const string PrefsKeyAnimationUnityNativeSub = PrefsKeyPrefix + KeyAnimationUnityNativeSub;
				private const string PrefsKeyAnimationUnityNativeMul = PrefsKeyPrefix + KeyAnimationUnityNativeMul;
				private const string PrefsKeyAnimationUnityNativeMulNA = PrefsKeyPrefix + KeyAnimationUnityNativeMulNA;
				private const string PrefsKeyAnimationUnityNativeScr = PrefsKeyPrefix + KeyAnimationUnityNativeScr;
				private const string PrefsKeyAnimationUnityNativeExc = PrefsKeyPrefix + KeyAnimationUnityNativeExc;
				private const string PrefsKeyAnimationUnityNativeInv = PrefsKeyPrefix + KeyAnimationUnityNativeInv;
				private const string PrefsKeyAnimationUnityNativeNonBatchMix = PrefsKeyPrefix + KeyAnimationUnityNativeNonBatchMix;
				private const string PrefsKeyAnimationUnityNativeNonBatchAdd = PrefsKeyPrefix + KeyAnimationUnityNativeNonBatchAdd;
				private const string PrefsKeyAnimationUnityNativeNonBatchSub = PrefsKeyPrefix + KeyAnimationUnityNativeNonBatchSub;
				private const string PrefsKeyAnimationUnityNativeNonBatchMul = PrefsKeyPrefix + KeyAnimationUnityNativeNonBatchMul;
				private const string PrefsKeyAnimationUnityNativeNonBatchMulNA = PrefsKeyPrefix + KeyAnimationUnityNativeNonBatchMulNA;
				private const string PrefsKeyAnimationUnityNativeNonBatchScr = PrefsKeyPrefix + KeyAnimationUnityNativeNonBatchScr;
				private const string PrefsKeyAnimationUnityNativeNonBatchExc = PrefsKeyPrefix + KeyAnimationUnityNativeNonBatchExc;
				private const string PrefsKeyAnimationUnityNativeNonBatchInv = PrefsKeyPrefix + KeyAnimationUnityNativeNonBatchInv;
				private const string PrefsKeySkinnedMeshUnityNativeMix = PrefsKeyPrefix + KeySkinnedMeshUnityNativeMix;
				private const string PrefsKeySkinnedMeshUnityNativeAdd = PrefsKeyPrefix + KeySkinnedMeshUnityNativeAdd;
				private const string PrefsKeySkinnedMeshUnityNativeSub = PrefsKeyPrefix + KeySkinnedMeshUnityNativeSub;
				private const string PrefsKeySkinnedMeshUnityNativeMul = PrefsKeyPrefix + KeySkinnedMeshUnityNativeMul;
				private const string PrefsKeySkinnedMeshUnityNativeMulNA = PrefsKeyPrefix + KeySkinnedMeshUnityNativeMulNA;
				private const string PrefsKeySkinnedMeshUnityNativeScr = PrefsKeyPrefix + KeySkinnedMeshUnityNativeScr;
				private const string PrefsKeySkinnedMeshUnityNativeExc = PrefsKeyPrefix + KeySkinnedMeshUnityNativeExc;
				private const string PrefsKeySkinnedMeshUnityNativeInv = PrefsKeyPrefix + KeySkinnedMeshUnityNativeInv;
				private const string PrefsKeyAnimationUnityUI = PrefsKeyPrefix + KeyAnimationUnityUI;

				private const string NamePathSubPreset = "SpriteStudio6/Material/UnityNative/";

				internal const string NameFileBodyPresetUnityNativeMix = "Sprite_UnityNative_MIX.mat";
				internal const string NameFileBodyPresetUnityNativeAdd = "Sprite_UnityNative_ADD.mat";
				internal const string NameFileBodyPresetUnityNativeSub = "Sprite_UnityNative_SUB.mat";
				internal const string NameFileBodyPresetUnityNativeMul = "Sprite_UnityNative_MUL.mat";
				internal const string NameFileBodyPresetUnityNativeMulNA = "Sprite_UnityNative_MUL_NA.mat";
				internal const string NameFileBodyPresetUnityNativeScr = "Sprite_UnityNative_SCR.mat";
				internal const string NameFileBodyPresetUnityNativeExc = "Sprite_UnityNative_EXC.mat";
				internal const string NameFileBodyPresetUnityNativeInv = "Sprite_UnityNative_INV.mat";
				internal const string NameFileBodyPresetUnityNativeNonBatchMix = "Sprite_UnityNative_NonBatch_MIX.mat";
				internal const string NameFileBodyPresetUnityNativeNonBatchAdd = "Sprite_UnityNative_NonBatch_ADD.mat";
				internal const string NameFileBodyPresetUnityNativeNonBatchSub = "Sprite_UnityNative_NonBatch_SUB.mat";
				internal const string NameFileBodyPresetUnityNativeNonBatchMul = "Sprite_UnityNative_NonBatch_MUL.mat";
				internal const string NameFileBodyPresetUnityNativeNonBatchMulNA = "Sprite_UnityNative_NonBatch_MUL_NA.mat";
				internal const string NameFileBodyPresetUnityNativeNonBatchScr = "Sprite_UnityNative_NonBatch_SCR.mat";
				internal const string NameFileBodyPresetUnityNativeNonBatchExc = "Sprite_UnityNative_NonBatch_EXC.mat";
				internal const string NameFileBodyPresetUnityNativeNonBatchInv = "Sprite_UnityNative_NonBatch_INV.mat";
				internal const string NameFileBodyPresetUnityNativeSkinnedMeshMix = "SkinnedMesh_UnityNative_MIX.mat";
				internal const string NameFileBodyPresetUnityNativeSkinnedMeshAdd = "SkinnedMesh_UnityNative_ADD.mat";
				internal const string NameFileBodyPresetUnityNativeSkinnedMeshSub = "SkinnedMesh_UnityNative_SUB.mat";
				internal const string NameFileBodyPresetUnityNativeSkinnedMeshMul = "SkinnedMesh_UnityNative_MUL.mat";
				internal const string NameFileBodyPresetUnityNativeSkinnedMeshMulNA = "SkinnedMesh_UnityNative_MUL_NA.mat";
				internal const string NameFileBodyPresetUnityNativeSkinnedMeshScr = "SkinnedMesh_UnityNative_SCR.mat";
				internal const string NameFileBodyPresetUnityNativeSkinnedMeshExc = "SkinnedMesh_UnityNative_EXC.mat";
				internal const string NameFileBodyPresetUnityNativeSkinnedMeshInv = "SkinnedMesh_UnityNative_INV.mat";
				private const string NamePathSubPresetUI = "SpriteStudio6/Material/UnityUI/";
				internal const string NameFileBodyPresetUnityUI = "Sprite_UnityUI.mat";

				private readonly static GroupPresetMaterial Default = new GroupPresetMaterial(
					/* AnimationUnityNativeMix */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeMix,
					/* AnimationUnityNativeAdd */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeAdd,
					/* AnimationUnityNativeSub */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeSub,
					/* AnimationUnityNativeMul */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeMul,
					/* AnimationUnityNativeMulNA */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeMulNA,
					/* AnimationUnityNativeScr */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeScr,
					/* AnimationUnityNativeExc */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeExc,
					/* AnimationUnityNativeInv */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeInv,
					/* AnimationUnityNativeNonBatchMix */	LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeNonBatchMix,
					/* AnimationUnityNativeNonBatchAdd */	LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeNonBatchAdd,
					/* AnimationUnityNativeNonBatchSub */	LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeNonBatchSub,
					/* AnimationUnityNativeNonBatchMul */	LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeNonBatchMul,
					/* AnimationUnityNativeNonBatchMulNA */	LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeNonBatchMulNA,
					/* AnimationUnityNativeNonBatchScr */	LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeNonBatchScr,
					/* AnimationUnityNativeNonBatchExc */	LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeNonBatchExc,
					/* AnimationUnityNativeNonBatchInv */	LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeNonBatchInv,
					/* SkinnedMeshUnityNativeMix */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeSkinnedMeshMix,
					/* SkinnedMeshUnityNativeAdd */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeSkinnedMeshAdd,
					/* SkinnedMeshUnityNativeSub */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeSkinnedMeshSub,
					/* SkinnedMeshUnityNativeMul */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeSkinnedMeshMul,
					/* SkinnedMeshUnityNativeMulNA */		LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeSkinnedMeshMulNA,
					/* SkinnedMeshUnityNativeScr */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeSkinnedMeshScr,
					/* SkinnedMeshUnityNativeExc */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeSkinnedMeshExc,
					/* SkinnedMeshUnityNativeInv */			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyPresetUnityNativeSkinnedMeshInv,
					/* AnimationUnityUI */					string.Empty	/* LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPresetUI + NameFileBodyPresetUnityUI */
				);
				#endregion Enums & Constants
			}

			public struct GroupHolderAsset
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public GameObject PrefabHolderAssetSS6PU;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupHolderAsset(	string prefabHolderAssetSS6PU
										)
				{
					PrefabHolderAssetSS6PU = PrefabGetPath(prefabHolderAssetSS6PU);
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					string guid = "";

					guid = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsPrefabHolderAssetSS6PU, GUIDGetPrefab(Default.PrefabHolderAssetSS6PU));
					PrefabHolderAssetSS6PU = PrefabGetGUID(guid);

					return(true);
				}

				public bool Save()
				{
					string guid = "";

					guid = GUIDGetPrefab(PrefabHolderAssetSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsPrefabHolderAssetSS6PU, guid);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[1];
					string textValue;

					textValue = PathGetForExport(PathGetPrefab(PrefabHolderAssetSS6PU));
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefabHolderAssetSS6PU, textValue);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					string namePathPrefab = textArgument[1];
					switch(textArgument[0])
					{
						case TextKeyNamePrefabHolderAssetSS6PU:
							if(true == string.IsNullOrEmpty(namePathPrefab))
							{
								namePathPrefab = PathGetForExport(PathGetPrefab(Default.PrefabHolderAssetSS6PU));
							}
							PrefabHolderAssetSS6PU = PrefabGetPath(PathGetForImport(namePathPrefab));
							return(true);

						default:
							break;
					}

					return(false);
				}

				public void AssetRecover(string pathBase)
				{
					if(true == string.IsNullOrEmpty(pathBase))
					{
						pathBase = NamePathSubPreset;
					}
					const string delimiterPath = "/";
					if(false == pathBase.EndsWith(delimiterPath))
					{
						pathBase += delimiterPath;
					}

					/* Reset Prefabs */
					PrefabHolderAssetSS6PU = PrefabGetPath(pathBase + NameFileBodyHolderAssetSS6PUPreset);

					/* MEMO: Save information of redefined assets */
					Save();
				}

				internal string PathFolderGetValidMaterial()
				{
					GameObject prefab = null;

					/* Find Valid Prefab */
					if(null != PrefabHolderAssetSS6PU)
					{
						prefab = PrefabHolderAssetSS6PU;
					}

					/* Get and Split Material's path */
					string path = null;
					if(null != prefab)
					{
						string pathPrefab = AssetDatabase.GetAssetPath(prefab);
						if(null != pathPrefab)
						{
							string nameFileBody;
							string nameExtention;
							LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out path, out nameFileBody, out nameExtention, pathPrefab);
						}
					}

					return(path);
				}

				/* MEMO: For access by short name. */
				private static GameObject PrefabGetPath(string path)
				{
					return(LibraryEditor_SpriteStudio6.Utility.File.AssetGetPath<GameObject>(path));
				}
				private static GameObject PrefabGetGUID(string guid)
				{
					return(LibraryEditor_SpriteStudio6.Utility.File.AssetGetGUID<GameObject>(guid));
				}
				private static string PathGetPrefab(GameObject prefab)
				{
					return(LibraryEditor_SpriteStudio6.Utility.File.PathGetAsset(prefab));
				}
				private static string GUIDGetPrefab(GameObject prefab)
				{
					return(LibraryEditor_SpriteStudio6.Utility.File.GUIDGetAsset(prefab));
				}

				/* MEMO: For path adjustment. */
				private static string PathGetForImport(string namePath)
				{
					string name = namePath;
					if(false == name.StartsWith("/"))
					{
						name = "/" + name;
					}
					return(LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + name);
				}
				private static string PathGetForExport(string namePath)
				{
					if(true == string.IsNullOrEmpty(namePath))
					{
						return(string.Empty);
					}
					return(namePath.Remove(0, LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset.Length));
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyNamePrefabHolderAssetSS6PU = "PrefabHolderAssetSS6PU";

				private const string TextKeyPrefix = "HolderAsset_";
				private const string TextKeyNamePrefabHolderAssetSS6PU = TextKeyPrefix + KeyNamePrefabHolderAssetSS6PU;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsPrefabHolderAssetSS6PU = PrefsKeyPrefix + KeyNamePrefabHolderAssetSS6PU;

				private const string NamePathSubPreset = "SpriteStudio6/Prefab/Holder/";

				internal const string NameFileBodyHolderAssetSS6PUPreset = "HolderAssetSS6PU.prefab";

				private readonly static GroupHolderAsset Default = new GroupHolderAsset(
					/* PrefabHolderShaderSS6PU */	LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + "/" + NamePathSubPreset + NameFileBodyHolderAssetSS6PUPreset
				);
				#endregion Enums & Constants
			}
			#endregion Classes, Structs & Interfaces
		}
		#endregion Classes, Structs & Interfaces
	}
}
