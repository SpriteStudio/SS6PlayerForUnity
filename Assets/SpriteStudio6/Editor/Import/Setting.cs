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

				return(true);
			}

			public string[] Export(	bool flagExportCommon,
									bool flagExportBasic,
									bool flagExportPrecalculation,
									bool flagExportConfirmOverWrite,
									bool flagExportCollider,
									bool flagExportCheckVersion,
									bool flagExportRuleNameAsset,
									bool flagExportRuleNameAssetFolder,
									bool flagPackAttributeAnimation
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
				string[] exportPackAttributeAnimation = (true == flagPackAttributeAnimation) ? PackAttributeAnimation.Export() : null;

				int countCommon = (null != exportCommon) ? exportCommon.Length : 0;
				int countBasic = (null != exportBasic) ? exportBasic.Length : 0;
				int countPrecalculation = (null != exportPrecalculation) ? exportPrecalculation.Length : 0;
				int countConfirmOverWrite = (null != exportConfirmOverWrite) ? exportConfirmOverWrite.Length : 0;
				int countCollider = (null != exportCollider) ? exportCollider.Length : 0;
				int countCheckVersion = (null != exportCheckVersion) ? exportCheckVersion.Length : 0;
				int counttRuleNameAsset = (null != exportRuleNameAsset) ? exportRuleNameAsset.Length : 0;
				int counttRuleNameAssetFolder = (null != exportRuleNameAssetFolder) ? exportRuleNameAssetFolder.Length : 0;
				int countPackAttributeAnimation = (null != exportPackAttributeAnimation) ? exportPackAttributeAnimation.Length : 0;

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
									bool flagExportBasic,
									bool flagExportCommon,
									bool flagExportPrecalculation,
									bool flagExportConfirmOverWrite,
									bool flagExportCollider,
									bool flagExportCheckVersion,
									bool flagExportRuleNameAsset,
									bool flagExportRuleNameAssetFolder,
									bool flagPackAttributeAnimation
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
												flagExportConfirmOverWrite,
												flagExportCollider,
												flagExportCheckVersion,
												flagExportRuleNameAsset,
												flagExportRuleNameAssetFolder,
												flagPackAttributeAnimation
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

				TERMINATOR,
				BATCH_IMPORTER = TERMINATOR,
			}
			public enum KindAsset
			{
				/* (Common) */
				TEXTURE = 0,

				/* (Mode SS6PU) */
				PREFAB_ANIMATION_SS6PU,
				PREFAB_EFFECT_SS6PU,
				DATA_CELLMAP_SS6PU,
				DATA_ANIMATION_SS6PU,
				DATA_EFFECT_SS6PU,
				MATERIAL_ANIMATION_SS6PU,
				MATERIAL_EFFECT_SS6PU,

				/* (Mode UnityNative) */
				PREFAB_ANIMATION_UNITYNATIVE,
				PREFAB_EFFECT_UNITYNATIVE,
				DATA_ANIMATION_UNITYNATIVE,
				MATERIAL_ANIMATION_UNITYNATIVE,
				MATERIAL_EFFECT_UNITYNATIVE,
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
				" Importer Setting-File                                  ",
				"--------------------------------------------------------",
				" Generated with \"SpriteStudio6 Player for Unity\"        ",
				" Copyright(C) Web Technology Corp. All rights reserved. ",
				"========================================================",
			};

			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public struct GroupPreCalculation
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagFixMesh;
				public bool FlagTrimTransparentPixelsCell;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupPreCalculation(	bool flagFixMesh,
											bool flagTrimTransparentPixelsCell
										)
				{
					FlagFixMesh = flagFixMesh;
					FlagTrimTransparentPixelsCell = flagTrimTransparentPixelsCell;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					FlagFixMesh = EditorPrefs.GetBool(PrefsKeyFlagFixMesh, Default.FlagFixMesh);
					FlagTrimTransparentPixelsCell = EditorPrefs.GetBool(PrefsKeyFlagTrimTransparentPixelsCell, Default.FlagTrimTransparentPixelsCell);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagFixMesh, FlagFixMesh);
					EditorPrefs.SetBool(PrefsKeyFlagTrimTransparentPixelsCell, FlagTrimTransparentPixelsCell);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[2];
					string textValue;

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagFixMesh);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagFixMesh, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagTrimTransparentPixelsCell);
					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagTrimTransparentPixelsCell, textValue);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					switch(textArgument[0])
					{
						case TextKeyFlagFixMesh:
							FlagFixMesh = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagTrimTransparentPixelsCell:
							FlagTrimTransparentPixelsCell = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						default:
							break;
					}
					return(false);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyFlagFixMesh = "FlagFixMesh";
				private const string KeyFlagTrimTransparentPixelsCell = "FlagTrimTransparentPixelsCell";

				private const string TextKeyPrefix = "PreCalculation_";
				private const string TextKeyFlagFixMesh = TextKeyPrefix + KeyFlagFixMesh;
				private const string TextKeyFlagTrimTransparentPixelsCell = TextKeyPrefix + KeyFlagTrimTransparentPixelsCell;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagFixMesh = PrefsKeyPrefix + KeyFlagFixMesh;
				private const string PrefsKeyFlagTrimTransparentPixelsCell = PrefsKeyPrefix + KeyFlagTrimTransparentPixelsCell;

				private readonly static GroupPreCalculation Default = new GroupPreCalculation(
					false,	/* FlagFixMesh */
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
				public bool FlagDataCellMap;
				public bool FlagDataAnimation;
				public bool FlagDataEffect;
				public bool FlagMaterialAnimation;
				public bool FlagMaterialEffect;
				public bool FlagTexture;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupConfirmOverWrite(	bool flagPrefabAnimation,
												bool flagPrefabEffect,
												bool flagDataCellMap,
												bool flagDataAnimation,
												bool flagDataEffect,
												bool flagMaterialAnimation,
												bool flagMaterialEffect,
												bool flagTexture
											)
				{
					FlagPrefabAnimation = flagPrefabAnimation;
					FlagPrefabEffect = flagPrefabEffect;
					FlagDataCellMap = flagDataCellMap;
					FlagDataAnimation = flagDataAnimation;
					FlagDataEffect = flagDataEffect;
					FlagMaterialAnimation = flagMaterialAnimation;
					FlagMaterialEffect = flagMaterialEffect;
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
					FlagDataCellMap = EditorPrefs.GetBool(PrefsKeyFlagDataCellMap, Default.FlagDataCellMap);
					FlagDataAnimation = EditorPrefs.GetBool(PrefsKeyFlagDataAnimation, Default.FlagDataAnimation);
					FlagDataEffect = EditorPrefs.GetBool(PrefsKeyFlagDataEffect, Default.FlagDataEffect);
					FlagMaterialAnimation = EditorPrefs.GetBool(PrefsKeyFlagMaterialAnimation, Default.FlagMaterialAnimation);
					FlagMaterialEffect = EditorPrefs.GetBool(PrefsKeyFlagMaterialEffect, Default.FlagMaterialEffect);
					FlagTexture = EditorPrefs.GetBool(PrefsKeyFlagTexture, Default.FlagTexture);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagPrefabAnimation, FlagPrefabAnimation);
					EditorPrefs.SetBool(PrefsKeyFlagPrefabEffect, FlagPrefabEffect);
					EditorPrefs.SetBool(PrefsKeyFlagDataCellMap, FlagDataCellMap);
					EditorPrefs.SetBool(PrefsKeyFlagDataAnimation, FlagDataAnimation);
					EditorPrefs.SetBool(PrefsKeyFlagDataEffect, FlagDataEffect);
					EditorPrefs.SetBool(PrefsKeyFlagMaterialAnimation, FlagMaterialAnimation);
					EditorPrefs.SetBool(PrefsKeyFlagMaterialEffect, FlagMaterialEffect);
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

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagDataCellMap);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagDataCellMap, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagDataAnimation);
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagDataAnimation, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagDataEffect);
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagDataEffect, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagMaterialAnimation);
					textEncode[5] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagMaterialAnimation, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagMaterialEffect);
					textEncode[6] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagMaterialEffect, textValue);

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

						case TextKeyFlagDataCellMap:
							FlagDataCellMap = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagDataAnimation:
							FlagDataAnimation = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagDataEffect:
							FlagDataEffect = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagMaterialAnimation:
							FlagMaterialAnimation = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagMaterialEffect:
							FlagMaterialEffect = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
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
				private const string KeyFlagDataCellMap = "FlagDataCellMap";
				private const string KeyFlagDataAnimation = "FlagDataAnimation";
				private const string KeyFlagDataEffect = "FlagDataEffect";
				private const string KeyFlagMaterialAnimation = "FlagMaterialAnimation";
				private const string KeyFlagMaterialEffect = "FlagMaterialEffect";
				private const string KeyFlagTexture = "FlagTexture";

				private const string TextKeyPrefix = "ConfirmOverWrite_";
				private const string TextKeyFlagPrefabAnimation = TextKeyPrefix + KeyFlagPrefabAnimation;
				private const string TextKeyFlagPrefabEffect = TextKeyPrefix + KeyFlagPrefabEffect;
				private const string TextKeyFlagDataCellMap = TextKeyPrefix + KeyFlagDataCellMap;
				private const string TextKeyFlagDataAnimation = TextKeyPrefix + KeyFlagDataAnimation;
				private const string TextKeyFlagDataEffect = TextKeyPrefix + KeyFlagDataEffect;
				private const string TextKeyFlagMaterialAnimation = TextKeyPrefix + KeyFlagMaterialAnimation;
				private const string TextKeyFlagMaterialEffect = TextKeyPrefix + KeyFlagMaterialEffect;
				private const string TextKeyFlagTexture = TextKeyPrefix + KeyFlagTexture;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagPrefabAnimation = PrefsKeyPrefix + KeyFlagPrefabAnimation;
				private const string PrefsKeyFlagPrefabEffect = PrefsKeyPrefix + KeyFlagPrefabEffect;
				private const string PrefsKeyFlagDataCellMap = PrefsKeyPrefix + KeyFlagDataCellMap;
				private const string PrefsKeyFlagDataAnimation = PrefsKeyPrefix + KeyFlagDataAnimation;
				private const string PrefsKeyFlagDataEffect = PrefsKeyPrefix + KeyFlagDataEffect;
				private const string PrefsKeyFlagMaterialAnimation = PrefsKeyPrefix + KeyFlagMaterialAnimation;
				private const string PrefsKeyFlagMaterialEffect = PrefsKeyPrefix + KeyFlagMaterialEffect;
				private const string PrefsKeyFlagTexture = PrefsKeyPrefix + KeyFlagTexture;

				internal readonly static GroupConfirmOverWrite Default = new GroupConfirmOverWrite(
					false,	/* FlagPrefabAnimation */
					false,	/* FlagPrefabEffect */
					false,	/* FlagDataCellMap */
					false,	/* FlagDataAnimation */
					false,	/* FlagDataEffect */
					false,	/* FlagMaterialAnimation */
					false,	/* FlagMaterialEffect */
					false	/* FlagTexture */
				);
				#endregion Enums & Constants
			}

			public struct GroupCollider
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagAttachCollider;
				public bool FlagAttachRigidBody;
				public float SizeZ;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupCollider(	bool flagAttachColider,
										bool flagAttachRigidBody,
										float sizeZ
									)
				{
					FlagAttachCollider = flagAttachColider;
					FlagAttachRigidBody = flagAttachRigidBody;
					SizeZ = sizeZ;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					FlagAttachCollider = EditorPrefs.GetBool(PrefsKeyFlagAttachCollider, Default.FlagAttachCollider);
					FlagAttachRigidBody = EditorPrefs.GetBool(PrefsKeyFlagAttachRigidBody, Default.FlagAttachRigidBody);
					SizeZ = EditorPrefs.GetFloat(PrefsKeySizeZ, Default.SizeZ);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagAttachCollider, FlagAttachCollider);
					EditorPrefs.SetBool(PrefsKeyFlagAttachRigidBody, FlagAttachRigidBody);
					EditorPrefs.SetFloat(PrefsKeySizeZ, SizeZ);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[3];
					string textValue;

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagAttachCollider);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagAttachCollider, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagAttachRigidBody);
					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagAttachRigidBody, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.FloatEncode(SizeZ);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeySizeZ, textValue);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					switch(textArgument[0])
					{
						case TextKeyFlagAttachCollider:
							FlagAttachCollider = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						case TextKeyFlagAttachRigidBody:
							FlagAttachRigidBody = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
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
				private const string KeyFlagAttachRigidBody = "FlagAttachRigidBody";
				private const string KeySizeZ = "SizeZ";

				private const string TextKeyPrefix = "Collider_";
				private const string TextKeyFlagAttachCollider = TextKeyPrefix + KeyFlagAttachCollider;
				private const string TextKeyFlagAttachRigidBody = TextKeyPrefix + KeyFlagAttachRigidBody;
				private const string TextKeySizeZ = TextKeyPrefix + KeySizeZ;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagAttachCollider = PrefsKeyPrefix + KeyFlagAttachCollider;
				private const string PrefsKeyFlagAttachRigidBody = PrefsKeyPrefix + KeyFlagAttachRigidBody;
				private const string PrefsKeySizeZ = PrefsKeyPrefix + KeySizeZ;

				private readonly static GroupCollider Default = new GroupCollider(
					true,	/* FlagAttachCollider */
					false,	/* FlagAttachRigidBody */
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
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupCheckVersion(	bool flagInvalidSSPJ,
											bool flagInvalidSSCE,
											bool flagInvalidSSAE,
											bool flagInvalidSSEE
										)
				{
					FlagInvalidSSPJ = flagInvalidSSPJ;
					FlagInvalidSSCE = flagInvalidSSCE;
					FlagInvalidSSAE = flagInvalidSSAE;
					FlagInvalidSSEE = flagInvalidSSEE;
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

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.GetBool(PrefsKeyFlagInvalidSSPJ, FlagInvalidSSPJ);
					EditorPrefs.GetBool(PrefsKeyFlagInvalidSSCE, FlagInvalidSSCE);
					EditorPrefs.GetBool(PrefsKeyFlagInvalidSSAE, FlagInvalidSSAE);
					EditorPrefs.GetBool(PrefsKeyFlagInvalidSSEE, FlagInvalidSSEE);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[4];
					string textValue;

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagInvalidSSPJ);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagInvalidSSPJ, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagInvalidSSCE);
					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagInvalidSSCE, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagInvalidSSAE);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagInvalidSSAE, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagInvalidSSEE);
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagInvalidSSEE, textValue);

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

				private const string TextKeyPrefix = "CheckVersion_";
				private const string TextKeyFlagInvalidSSPJ = TextKeyPrefix + KeyFlagInvalidSSPJ;
				private const string TextKeyFlagInvalidSSCE = TextKeyPrefix + KeyFlagInvalidSSCE;
				private const string TextKeyFlagInvalidSSAE = TextKeyPrefix + KeyFlagInvalidSSAE;
				private const string TextKeyFlagInvalidSSEE = TextKeyPrefix + KeyFlagInvalidSSEE;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagInvalidSSPJ = PrefsKeyPrefix + KeyFlagInvalidSSPJ;
				private const string PrefsKeyFlagInvalidSSCE = PrefsKeyPrefix + KeyFlagInvalidSSCE;
				private const string PrefsKeyFlagInvalidSSAE = PrefsKeyPrefix + KeyFlagInvalidSSAE;
				private const string PrefsKeyFlagInvalidSSEE = PrefsKeyPrefix + KeyFlagInvalidSSEE;

				internal readonly static GroupCheckVersion Default = new GroupCheckVersion(
					false,	/* FlagInvalidSSPJ */
					false,	/* FlagInvalidSSCE */
					false,	/* FlagInvalidSSAE */
					false	/* FlagInvalidSSEE */
				);
				#endregion Enums & Constants
			}

			public struct GroupBasic
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagCreateControlGameObject;
				public bool FlagCreateProjectFolder;
				public bool FlagInvisibleToHideAll;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupBasic(	bool flagCreateControlGameObject,
									bool flagCreateProjectFolder,
									bool flagInvisibleToHideAll
								)
				{
					FlagCreateControlGameObject = flagCreateControlGameObject;
					FlagCreateProjectFolder = flagCreateProjectFolder;
					FlagInvisibleToHideAll = flagInvisibleToHideAll;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					FlagCreateControlGameObject = EditorPrefs.GetBool(PrefsKeyFlagCreateControlGameObject, Default.FlagCreateControlGameObject);
					FlagCreateProjectFolder = EditorPrefs.GetBool(PrefsKeyFlagCreateProjectFolder, Default.FlagCreateProjectFolder);
					FlagInvisibleToHideAll = EditorPrefs.GetBool(PrefsKeyFlagInvisibleToHideAll, Default.FlagInvisibleToHideAll);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagCreateControlGameObject, FlagCreateControlGameObject);
					EditorPrefs.SetBool(PrefsKeyFlagCreateProjectFolder, FlagCreateProjectFolder);
					EditorPrefs.SetBool(PrefsKeyFlagInvisibleToHideAll, FlagInvisibleToHideAll);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[3];
					string textValue;

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagCreateControlGameObject);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagCreateControlGameObject, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagCreateProjectFolder);
					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagCreateProjectFolder, textValue);

					textValue = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolEncode(FlagInvisibleToHideAll);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFlagInvisibleToHideAll, textValue);

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

						case TextKeyFlagInvisibleToHideAll:
							FlagInvisibleToHideAll = LibraryEditor_SpriteStudio6.Utility.ExternalText.BoolDecode(textArgument[1]);
							return(true);

						default:
							break;
					}
					return(false);
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyFlagCreateControlGameObject = "FlagCreateControlGameObject";
				private const string KeyFlagCreateProjectFolder = "FlagCreateProjectFolder";
				private const string KeyFlagInvisibleToHideAll = "FlagInvisibleToHideAll";

				private const string TextKeyPrefix = "Basic_";
				private const string TextKeyFlagCreateControlGameObject = TextKeyPrefix + KeyFlagCreateControlGameObject;
				private const string TextKeyFlagCreateProjectFolder = TextKeyPrefix + KeyFlagCreateProjectFolder;
				private const string TextKeyFlagInvisibleToHideAll = TextKeyPrefix + KeyFlagInvisibleToHideAll;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagCreateControlGameObject = PrefsKeyPrefix + KeyFlagCreateControlGameObject;
				private const string PrefsKeyFlagCreateProjectFolder = PrefsKeyPrefix + KeyFlagCreateProjectFolder;
				private const string PrefsKeyFlagInvisibleToHideAll = PrefsKeyPrefix + KeyFlagInvisibleToHideAll;

				private readonly static GroupBasic Default = new GroupBasic(
					true,	/* flagCreateControlGameObject */
					true,	/* flagCreateProjectFolder */
					false	/* flagInvisibleToHideAll */
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
				public string NamePrefixDataCellMapSS6PU;
				public string NamePrefixDataAnimationSS6PU;
				public string NamePrefixDataEffectSS6PU;
				public string NamePrefixMaterialAnimationSS6PU;
				public string NamePrefixMaterialEffectSS6PU;

				/* Prefix Unity-Native */
				public string NamePrefixPrefabAnimatorUnityNative;
				public string NamePrefixPrefabParticleUnityNative;
				public string NamePrefixAnimationClipUnityNative;
				public string NamePrefixMaterialAnimatorUnityNative;
				public string NamePrefixMaterialParticleUnityNative;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupRuleNameAsset(	bool flagAttachSpecificNameSSPJ,
											string namePrefixTexture,
											string namePrefixPrefabAnimationSS6PU,
											string namePrefixPrefabEffectSS6PU,
											string namePrefixDataCellMapSS6PU,
											string namePrefixDataAnimationSS6PU,
											string namePrefixDataEffectSS6PU,
											string namePrefixMaterialAnimationSS6PU,
											string namePrefixMaterialEffectSS6PU,
											string namePrefixPrefabAnimatorUnityNative,
											string namePrefixPrefabParticleUnityNative,
											string namePrefixAnimationClipUnityNative,
											string namePrefixMaterialAnimatorUnityNative,
											string namePrefixMaterialParticleUnityNative
										)
				{
					FlagAttachSpecificNameSSPJ = flagAttachSpecificNameSSPJ;

					NamePrefixTexture = namePrefixTexture;

					NamePrefixPrefabAnimationSS6PU = namePrefixPrefabAnimationSS6PU;
					NamePrefixPrefabEffectSS6PU =namePrefixPrefabEffectSS6PU ;
					NamePrefixDataCellMapSS6PU = namePrefixDataCellMapSS6PU;
					NamePrefixDataAnimationSS6PU = namePrefixDataAnimationSS6PU;
					NamePrefixDataEffectSS6PU = namePrefixDataEffectSS6PU;
					NamePrefixMaterialAnimationSS6PU = namePrefixMaterialAnimationSS6PU;
					NamePrefixMaterialEffectSS6PU = namePrefixMaterialEffectSS6PU;

					NamePrefixPrefabAnimatorUnityNative = namePrefixPrefabAnimatorUnityNative;
					NamePrefixPrefabParticleUnityNative = namePrefixPrefabParticleUnityNative;
					NamePrefixAnimationClipUnityNative = namePrefixAnimationClipUnityNative;
					NamePrefixMaterialAnimatorUnityNative = namePrefixMaterialAnimatorUnityNative;
					NamePrefixMaterialParticleUnityNative = namePrefixMaterialParticleUnityNative;
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
					NamePrefixDataCellMapSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixDataCellMapSS6PU, Default.NamePrefixDataCellMapSS6PU);
					NamePrefixDataAnimationSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixDataAnimationSS6PU, Default.NamePrefixDataAnimationSS6PU);
					NamePrefixDataEffectSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixDataEffectSS6PU, Default.NamePrefixDataEffectSS6PU);
					NamePrefixMaterialAnimationSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixMaterialAnimationSS6PU, Default.NamePrefixMaterialAnimationSS6PU);
					NamePrefixMaterialEffectSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixMaterialEffectSS6PU, Default.NamePrefixMaterialEffectSS6PU);

					NamePrefixPrefabAnimatorUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixPrefabAnimatorUnityNative, Default.NamePrefixPrefabAnimatorUnityNative);
					NamePrefixPrefabParticleUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixPrefabParticleUnityNative, Default.NamePrefixPrefabParticleUnityNative);
					NamePrefixAnimationClipUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixAnimationClipUnityNative, Default.NamePrefixAnimationClipUnityNative);
					NamePrefixMaterialAnimatorUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixMaterialAnimatorUnityNative, Default.NamePrefixMaterialAnimatorUnityNative);
					NamePrefixMaterialParticleUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNamePrefixMaterialParticleUnityNative, Default.NamePrefixMaterialParticleUnityNative);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagAttachSpecificNameSSPJ, FlagAttachSpecificNameSSPJ);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixTexture, NamePrefixTexture);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixPrefabAnimationSS6PU, NamePrefixPrefabAnimationSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixPrefabEffectSS6PU, NamePrefixPrefabEffectSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixDataCellMapSS6PU, NamePrefixDataCellMapSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixDataAnimationSS6PU, NamePrefixDataAnimationSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixDataEffectSS6PU, NamePrefixDataEffectSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixMaterialAnimationSS6PU, NamePrefixMaterialAnimationSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixMaterialEffectSS6PU, NamePrefixMaterialEffectSS6PU);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixPrefabAnimatorUnityNative, NamePrefixPrefabAnimatorUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixPrefabParticleUnityNative, NamePrefixPrefabParticleUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixAnimationClipUnityNative, NamePrefixAnimationClipUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixMaterialAnimatorUnityNative, NamePrefixMaterialAnimatorUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNamePrefixMaterialParticleUnityNative, NamePrefixMaterialParticleUnityNative);

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
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixDataCellMapSS6PU, NamePrefixDataCellMapSS6PU);
					textEncode[5] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixDataAnimationSS6PU, NamePrefixDataAnimationSS6PU);
					textEncode[6] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixDataEffectSS6PU, NamePrefixDataEffectSS6PU);
					textEncode[7] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixMaterialAnimationSS6PU, NamePrefixMaterialAnimationSS6PU);
					textEncode[8] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixMaterialEffectSS6PU, NamePrefixMaterialEffectSS6PU);

					textEncode[9] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixPrefabAnimatorUnityNative, NamePrefixPrefabAnimatorUnityNative);
					textEncode[10] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixPrefabParticleUnityNative, NamePrefixPrefabParticleUnityNative);
					textEncode[11] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixAnimationClipUnityNative, NamePrefixAnimationClipUnityNative);
					textEncode[12] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixMaterialAnimatorUnityNative, NamePrefixMaterialAnimatorUnityNative);
					textEncode[13] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNamePrefixMaterialParticleUnityNative, NamePrefixMaterialParticleUnityNative);

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
						case TextKeyNamePrefixDataCellMapSS6PU:
							NamePrefixDataCellMapSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixDataAnimationSS6PU:
							NamePrefixDataAnimationSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixDataEffectSS6PU:
							NamePrefixDataEffectSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixMaterialAnimationSS6PU:
							NamePrefixMaterialAnimationSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixMaterialEffectSS6PU:
							NamePrefixMaterialEffectSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);

						case TextKeyNamePrefixPrefabAnimatorUnityNative:
							NamePrefixPrefabAnimatorUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixPrefabParticleUnityNative:
							NamePrefixPrefabParticleUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixAnimationClipUnityNative:
							NamePrefixAnimationClipUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixMaterialAnimatorUnityNative:
							NamePrefixMaterialAnimatorUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNamePrefixMaterialParticleUnityNative:
							NamePrefixMaterialParticleUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
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
					NamePrefixDataCellMapSS6PU = Adjust(NamePrefixDataCellMapSS6PU);
					NamePrefixDataAnimationSS6PU = Adjust(NamePrefixDataAnimationSS6PU);
					NamePrefixDataEffectSS6PU = Adjust(NamePrefixDataEffectSS6PU);
					NamePrefixMaterialAnimationSS6PU = Adjust(NamePrefixMaterialAnimationSS6PU);
					NamePrefixMaterialEffectSS6PU = Adjust(NamePrefixMaterialEffectSS6PU);

					NamePrefixPrefabAnimatorUnityNative = Adjust(NamePrefixPrefabAnimatorUnityNative);
					NamePrefixPrefabParticleUnityNative = Adjust(NamePrefixPrefabParticleUnityNative);
					NamePrefixAnimationClipUnityNative = Adjust(NamePrefixAnimationClipUnityNative);
					NamePrefixMaterialAnimatorUnityNative = Adjust(NamePrefixMaterialAnimatorUnityNative);
					NamePrefixMaterialParticleUnityNative = Adjust(NamePrefixMaterialParticleUnityNative);
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
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_ANIMATION_SS6PU:
							name = NamePrefixMaterialAnimationSS6PU
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_EFFECT_SS6PU:
							name = NamePrefixMaterialEffectSS6PU
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;

						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_UNITYNATIVE:
							name = NamePrefixPrefabAnimatorUnityNative
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_EFFECT_UNITYNATIVE:
							name = NamePrefixPrefabParticleUnityNative
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_UNITYNATIVE:
							name = NamePrefixAnimationClipUnityNative
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_ANIMATION_UNITYNATIVE:
							name = NamePrefixMaterialAnimatorUnityNative
									+ ((true == FlagAttachSpecificNameSSPJ) ? (nameSSPJ + "_") : "")
									+ nameBase;
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_EFFECT_UNITYNATIVE:
							name = NamePrefixMaterialParticleUnityNative
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
				private const string KeyNamePrefixDataCellMapSS6PU = "NamePrefixDataCellMapSS6PU";
				private const string KeyNamePrefixDataAnimationSS6PU = "NamePrefixDataAnimationSS6PU";
				private const string KeyNamePrefixDataEffectSS6PU = "NamePrefixDataEffectSS6PU";
				private const string KeyNamePrefixMaterialAnimationSS6PU = "NamePrefixMaterialAnimationSS6PU";
				private const string KeyNamePrefixMaterialEffectSS6PU = "NamePrefixMaterialEffectSS6PU";
				private const string KeyNamePrefixPrefabAnimatorUnityNative = "NamePrefixPrefabAnimatorUnityNative";
				private const string KeyNamePrefixPrefabParticleUnityNative = "NamePrefixPrefabParticleUnityNative";
				private const string KeyNamePrefixAnimationClipUnityNative = "NamePrefixAnimationClipUnityNative";
				private const string KeyNamePrefixMaterialAnimatorUnityNative = "NamePrefixMaterialAnimatorUnityNative";
				private const string KeyNamePrefixMaterialParticleUnityNative = "NamePrefixMaterialParticleUnityNative";

				private const string TextKeyPrefix = "RuleNameAsset_";
				private const string TextKeyFlagAttachSpecificNameSSPJ = TextKeyPrefix + KeyFlagAttachSpecificNameSSPJ;
				private const string TextKeyNamePrefixTexture = TextKeyPrefix + KeyNamePrefixTexture;
				private const string TextKeyNamePrefixPrefabAnimationSS6PU = TextKeyPrefix + KeyNamePrefixPrefabAnimationSS6PU;
				private const string TextKeyNamePrefixPrefabEffectSS6PU = TextKeyPrefix + KeyNamePrefixPrefabEffectSS6PU;
				private const string TextKeyNamePrefixDataCellMapSS6PU = TextKeyPrefix + KeyNamePrefixDataCellMapSS6PU;
				private const string TextKeyNamePrefixDataAnimationSS6PU = TextKeyPrefix + KeyNamePrefixDataAnimationSS6PU;
				private const string TextKeyNamePrefixDataEffectSS6PU = TextKeyPrefix + KeyNamePrefixDataEffectSS6PU;
				private const string TextKeyNamePrefixMaterialAnimationSS6PU = TextKeyPrefix + KeyNamePrefixMaterialAnimationSS6PU;
				private const string TextKeyNamePrefixMaterialEffectSS6PU = TextKeyPrefix + KeyNamePrefixMaterialEffectSS6PU;
				private const string TextKeyNamePrefixPrefabAnimatorUnityNative = TextKeyPrefix + KeyNamePrefixPrefabAnimatorUnityNative;
				private const string TextKeyNamePrefixPrefabParticleUnityNative = TextKeyPrefix + KeyNamePrefixPrefabParticleUnityNative;
				private const string TextKeyNamePrefixAnimationClipUnityNative = TextKeyPrefix + KeyNamePrefixAnimationClipUnityNative;
				private const string TextKeyNamePrefixMaterialAnimatorUnityNative = TextKeyPrefix + KeyNamePrefixMaterialAnimatorUnityNative;
				private const string TextKeyNamePrefixMaterialParticleUnityNative = TextKeyPrefix + KeyNamePrefixMaterialParticleUnityNative;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagAttachSpecificNameSSPJ = PrefsKeyPrefix + KeyFlagAttachSpecificNameSSPJ;
				private const string PrefsKeyNamePrefixTexture = PrefsKeyPrefix + KeyNamePrefixTexture;
				private const string PrefsKeyNamePrefixPrefabAnimationSS6PU = PrefsKeyPrefix + KeyNamePrefixPrefabAnimationSS6PU;
				private const string PrefsKeyNamePrefixPrefabEffectSS6PU = PrefsKeyPrefix + KeyNamePrefixPrefabEffectSS6PU;
				private const string PrefsKeyNamePrefixDataCellMapSS6PU = PrefsKeyPrefix + KeyNamePrefixDataCellMapSS6PU;
				private const string PrefsKeyNamePrefixDataAnimationSS6PU = PrefsKeyPrefix + KeyNamePrefixDataAnimationSS6PU;
				private const string PrefsKeyNamePrefixDataEffectSS6PU = PrefsKeyPrefix + KeyNamePrefixDataEffectSS6PU;
				private const string PrefsKeyNamePrefixMaterialAnimationSS6PU = PrefsKeyPrefix + KeyNamePrefixMaterialAnimationSS6PU;
				private const string PrefsKeyNamePrefixMaterialEffectSS6PU = PrefsKeyPrefix + KeyNamePrefixMaterialEffectSS6PU;
				private const string PrefsKeyNamePrefixPrefabAnimatorUnityNative = PrefsKeyPrefix + KeyNamePrefixPrefabAnimatorUnityNative;
				private const string PrefsKeyNamePrefixPrefabParticleUnityNative = PrefsKeyPrefix + KeyNamePrefixPrefabParticleUnityNative;
				private const string PrefsKeyNamePrefixAnimationClipUnityNative = PrefsKeyPrefix + KeyNamePrefixAnimationClipUnityNative;
				private const string PrefsKeyNamePrefixMaterialAnimatorUnityNative = PrefsKeyPrefix + KeyNamePrefixMaterialAnimatorUnityNative;
				private const string PrefsKeyNamePrefixMaterialParticleUnityNative = PrefsKeyPrefix + KeyNamePrefixMaterialParticleUnityNative;

				private readonly static GroupRuleNameAsset Default = new GroupRuleNameAsset(
					false,	/* FlagAttachSpecificNameSSPJ */
					"",		/* NamePrefixTexture */
					"",		/* NamePrefixPrefabAnimationSS6PU */
					"pe_",	/* NamePrefixPrefabEffectSS6PU */
					"dc_",	/* NamePrefixDataCellMapSS6PU */
					"da_",	/* NamePrefixDataAnimationSS6PU */
					"de_",	/* NamePrefixDataEffectSS6PU */
					"ma_",	/* NamePrefixMaterialAnimationSS6PU */
					"me_",	/* NamePrefixMaterialEffectSS6PU */
					"ps_",	/* NamePrefixPrefabAnimatorUnityNative */
					"pp_",	/* NamePrefixPrefabParticleUnityNative */
					"ac_",	/* NamePrefixAnimationClipUnityNative */
					"ms_",	/* NamePrefixMaterialAnimatorUnityNative */
					"mp_"	/* NamePrefixMaterialParticleUnityNative */
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
				public string NameFolderDataCellMapSS6PU;
				public string NameFolderDataAnimationSS6PU;
				public string NameFolderDataEffectSS6PU;
				public string NameFolderMaterialAnimationSS6PU;
				public string NameFolderMaterialEffectSS6PU;

				/* Folder Names for Unity-Native */
				public string NameFolderPrefabAnimatorUnityNative;
				public string NameFolderPrefabParticleUnityNative;
				public string NameFolderAnimationClipUnityNative;
				public string NameFolderMaterialAnimatorUnityNative;
				public string NameFolderMaterialParticleUnityNative;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupRuleNameAssetFolder(	string nameFolderTexture,
													string nameFolderPrefabAnimationSS6PU,
													string nameFolderPrefabEffectSS6PU,
													string nameFolderDataCellMapSS6PU,
													string nameFolderDataAnimationSS6PU,
													string nameFolderDataEffectSS6PU,
													string nameFolderMaterialAnimationSS6PU,
													string nameFolderMaterialEffectSS6PU,
													string nameFolderPrefabAnimatorUnityNative,
													string nameFolderPrefabParticleUnityNative,
													string nameFolderAnimationClipUnityNative,
													string nameFolderMaterialAnimatorUnityNative,
													string nameFolderMaterialParticleUnityNative
												)
				{
						NameFolderTexture = nameFolderTexture;

						NameFolderPrefabAnimationSS6PU = nameFolderPrefabAnimationSS6PU;
						NameFolderPrefabEffectSS6PU = nameFolderPrefabEffectSS6PU;
						NameFolderDataCellMapSS6PU = nameFolderDataCellMapSS6PU;
						NameFolderDataAnimationSS6PU = nameFolderDataAnimationSS6PU;
						NameFolderDataEffectSS6PU = nameFolderDataEffectSS6PU;
						NameFolderMaterialAnimationSS6PU = nameFolderMaterialAnimationSS6PU;
						NameFolderMaterialEffectSS6PU = nameFolderMaterialEffectSS6PU;

						NameFolderPrefabAnimatorUnityNative = nameFolderPrefabAnimatorUnityNative;
						NameFolderPrefabParticleUnityNative = nameFolderPrefabParticleUnityNative;
						NameFolderAnimationClipUnityNative = nameFolderAnimationClipUnityNative;
						NameFolderMaterialAnimatorUnityNative = nameFolderMaterialAnimatorUnityNative;
						NameFolderMaterialParticleUnityNative = nameFolderMaterialParticleUnityNative;
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
					NameFolderDataCellMapSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderDataCellMapSS6PU, Default.NameFolderDataCellMapSS6PU);
					NameFolderDataAnimationSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderDataAnimationSS6PU, Default.NameFolderDataAnimationSS6PU);
					NameFolderDataEffectSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderDataEffectSS6PU, Default.NameFolderDataEffectSS6PU);
					NameFolderMaterialAnimationSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderMaterialAnimationSS6PU, Default.NameFolderMaterialAnimationSS6PU);
					NameFolderMaterialEffectSS6PU = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderMaterialEffectSS6PU, Default.NameFolderMaterialEffectSS6PU);

					NameFolderPrefabAnimatorUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderPrefabAnimatorUnityNative, Default.NameFolderPrefabAnimatorUnityNative);
					NameFolderPrefabParticleUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderPrefabParticleUnityNative, Default.NameFolderPrefabParticleUnityNative);
					NameFolderAnimationClipUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderAnimationClipUnityNative, Default.NameFolderAnimationClipUnityNative);
					NameFolderMaterialAnimatorUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderMaterialAnimatorUnityNative, Default.NameFolderMaterialAnimatorUnityNative);
					NameFolderMaterialParticleUnityNative = LibraryEditor_SpriteStudio6.Utility.Prefs.StringLoad(PrefsKeyNameFolderMaterialParticleUnityNative, Default.NameFolderMaterialParticleUnityNative);

					Adjust();

					return(true);
				}

				public bool Save()
				{
					Adjust();

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderTexture, NameFolderTexture);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderPrefabAnimationSS6PU, NameFolderPrefabAnimationSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderPrefabEffectSS6PU, NameFolderPrefabEffectSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderDataCellMapSS6PU, NameFolderDataCellMapSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderDataAnimationSS6PU, NameFolderDataAnimationSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderDataEffectSS6PU, NameFolderDataEffectSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderMaterialAnimationSS6PU, NameFolderMaterialAnimationSS6PU);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderMaterialEffectSS6PU, NameFolderMaterialEffectSS6PU);

					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderPrefabAnimatorUnityNative, NameFolderPrefabAnimatorUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderPrefabParticleUnityNative, NameFolderPrefabParticleUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderAnimationClipUnityNative, NameFolderAnimationClipUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderMaterialAnimatorUnityNative, NameFolderMaterialAnimatorUnityNative);
					LibraryEditor_SpriteStudio6.Utility.Prefs.StringSave(PrefsKeyNameFolderMaterialParticleUnityNative, NameFolderMaterialParticleUnityNative);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[13];

					Adjust();

					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderTexture, NameFolderTexture);

					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderPrefabAnimationSS6PU, NameFolderPrefabAnimationSS6PU);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderPrefabEffectSS6PU, NameFolderPrefabEffectSS6PU);
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderDataCellMapSS6PU, NameFolderDataCellMapSS6PU);
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderDataAnimationSS6PU, NameFolderDataAnimationSS6PU);
					textEncode[5] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderDataEffectSS6PU, NameFolderDataEffectSS6PU);
					textEncode[6] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderMaterialAnimationSS6PU, NameFolderMaterialAnimationSS6PU);
					textEncode[7] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderMaterialEffectSS6PU, NameFolderMaterialEffectSS6PU);

					textEncode[8] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderPrefabAnimatorUnityNative, NameFolderPrefabAnimatorUnityNative);
					textEncode[9] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderPrefabParticleUnityNative, NameFolderPrefabParticleUnityNative);
					textEncode[10] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderAnimationClipUnityNative, NameFolderAnimationClipUnityNative);
					textEncode[11] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderMaterialAnimatorUnityNative, NameFolderMaterialAnimatorUnityNative);
					textEncode[12] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyNameFolderMaterialParticleUnityNative, NameFolderMaterialParticleUnityNative);

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
						case TextKeyNameFolderDataCellMapSS6PU:
							NameFolderDataCellMapSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderDataAnimationSS6PU:
							NameFolderDataAnimationSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderDataEffectSS6PU:
							NameFolderDataEffectSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderMaterialAnimationSS6PU:
							NameFolderMaterialAnimationSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderMaterialEffectSS6PU:
							NameFolderMaterialEffectSS6PU = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);

						case TextKeyNameFolderPrefabAnimatorUnityNative:
							NameFolderPrefabAnimatorUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderPrefabParticleUnityNative:
							NameFolderPrefabParticleUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderAnimationClipUnityNative:
							NameFolderAnimationClipUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderMaterialAnimatorUnityNative:
							NameFolderMaterialAnimatorUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
							return(true);
						case TextKeyNameFolderMaterialParticleUnityNative:
							NameFolderMaterialParticleUnityNative = (1 >= textArgument.Length) ? "" : Adjust(textArgument[1]);
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
						NameFolderDataCellMapSS6PU = Adjust(NameFolderDataCellMapSS6PU);
						NameFolderDataAnimationSS6PU = Adjust(NameFolderDataAnimationSS6PU);
						NameFolderDataEffectSS6PU = Adjust(NameFolderDataEffectSS6PU);
						NameFolderMaterialAnimationSS6PU = Adjust(NameFolderMaterialAnimationSS6PU);
						NameFolderMaterialEffectSS6PU = Adjust(NameFolderMaterialEffectSS6PU);

						NameFolderPrefabAnimatorUnityNative = Adjust(NameFolderPrefabAnimatorUnityNative);
						NameFolderPrefabParticleUnityNative = Adjust(NameFolderPrefabParticleUnityNative);
						NameFolderAnimationClipUnityNative = Adjust(NameFolderAnimationClipUnityNative);
						NameFolderMaterialAnimatorUnityNative = Adjust(NameFolderMaterialAnimatorUnityNative);
						NameFolderMaterialParticleUnityNative = Adjust(NameFolderMaterialParticleUnityNative);
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

						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_SS6PU:
							name += NameFolderPrefabAnimationSS6PU + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_EFFECT_SS6PU:
							name += NameFolderPrefabEffectSS6PU + "/";
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
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_ANIMATION_SS6PU:
							name += NameFolderMaterialAnimationSS6PU + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_EFFECT_SS6PU:
							name += NameFolderMaterialEffectSS6PU + "/";
							break;

						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_UNITYNATIVE:
							name += NameFolderPrefabAnimatorUnityNative + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_EFFECT_UNITYNATIVE:
							name += NameFolderPrefabParticleUnityNative + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_UNITYNATIVE:
							name += NameFolderAnimationClipUnityNative + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_ANIMATION_UNITYNATIVE:
							name += NameFolderMaterialAnimatorUnityNative + "/";
							break;
						case LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_EFFECT_UNITYNATIVE:
							name += NameFolderMaterialParticleUnityNative + "/";
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
				private const string KeyNameFolderDataCellMapSS6PU = "NameFolderDataCellMapSS6PU";
				private const string KeyNameFolderDataAnimationSS6PU = "NameFolderDataAnimationSS6PU";
				private const string KeyNameFolderDataEffectSS6PU = "NameFolderDataEffectSS6PU";
				private const string KeyNameFolderMaterialAnimationSS6PU = "NameFolderMaterialAnimationSS6PU";
				private const string KeyNameFolderMaterialEffectSS6PU = "NameFolderMaterialEffectSS6PU";
				private const string KeyNameFolderPrefabAnimatorUnityNative = "NameFolderPrefabAnimatorUnityNative";
				private const string KeyNameFolderPrefabParticleUnityNative = "NameFolderPrefabParticleUnityNative";
				private const string KeyNameFolderAnimationClipUnityNative = "NameFolderAnimationClipUnityNative";
				private const string KeyNameFolderMaterialAnimatorUnityNative = "NameFolderMaterialAnimatorUnityNative";
				private const string KeyNameFolderMaterialParticleUnityNative = "NameFolderMaterialParticleUnityNative";

				private const string TextKeyPrefix = "RuleNameAssetFolder_";
				private const string TextKeyNameFolderTexture = TextKeyPrefix + KeyNameFolderTexture;
				private const string TextKeyNameFolderPrefabAnimationSS6PU = TextKeyPrefix + KeyNameFolderPrefabAnimationSS6PU;
				private const string TextKeyNameFolderPrefabEffectSS6PU = TextKeyPrefix + KeyNameFolderPrefabEffectSS6PU;
				private const string TextKeyNameFolderDataCellMapSS6PU = TextKeyPrefix + KeyNameFolderDataCellMapSS6PU;
				private const string TextKeyNameFolderDataAnimationSS6PU = TextKeyPrefix + KeyNameFolderDataAnimationSS6PU;
				private const string TextKeyNameFolderDataEffectSS6PU = TextKeyPrefix + KeyNameFolderDataEffectSS6PU;
				private const string TextKeyNameFolderMaterialAnimationSS6PU = TextKeyPrefix + KeyNameFolderMaterialAnimationSS6PU;
				private const string TextKeyNameFolderMaterialEffectSS6PU = TextKeyPrefix + KeyNameFolderMaterialEffectSS6PU;
				private const string TextKeyNameFolderPrefabAnimatorUnityNative = TextKeyPrefix + KeyNameFolderPrefabAnimatorUnityNative;
				private const string TextKeyNameFolderPrefabParticleUnityNative = TextKeyPrefix + KeyNameFolderPrefabParticleUnityNative;
				private const string TextKeyNameFolderAnimationClipUnityNative = TextKeyPrefix + KeyNameFolderAnimationClipUnityNative;
				private const string TextKeyNameFolderMaterialAnimatorUnityNative = TextKeyPrefix + KeyNameFolderMaterialAnimatorUnityNative;
				private const string TextKeyNameFolderMaterialParticleUnityNative = TextKeyPrefix + KeyNameFolderMaterialParticleUnityNative;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyNameFolderTexture = PrefsKeyPrefix + KeyNameFolderTexture;
				private const string PrefsKeyNameFolderPrefabAnimationSS6PU = PrefsKeyPrefix + KeyNameFolderPrefabAnimationSS6PU;
				private const string PrefsKeyNameFolderPrefabEffectSS6PU = PrefsKeyPrefix + KeyNameFolderPrefabEffectSS6PU;
				private const string PrefsKeyNameFolderDataCellMapSS6PU = PrefsKeyPrefix + KeyNameFolderDataCellMapSS6PU;
				private const string PrefsKeyNameFolderDataAnimationSS6PU = PrefsKeyPrefix + KeyNameFolderDataAnimationSS6PU;
				private const string PrefsKeyNameFolderDataEffectSS6PU = PrefsKeyPrefix + KeyNameFolderDataEffectSS6PU;
				private const string PrefsKeyNameFolderMaterialAnimationSS6PU = PrefsKeyPrefix + KeyNameFolderMaterialAnimationSS6PU;
				private const string PrefsKeyNameFolderMaterialEffectSS6PU = PrefsKeyPrefix + KeyNameFolderMaterialEffectSS6PU;
				private const string PrefsKeyNameFolderPrefabAnimatorUnityNative = PrefsKeyPrefix + KeyNameFolderPrefabAnimatorUnityNative;
				private const string PrefsKeyNameFolderPrefabParticleUnityNative = PrefsKeyPrefix + KeyNameFolderPrefabParticleUnityNative;
				private const string PrefsKeyNameFolderAnimationClipUnityNative = PrefsKeyPrefix + KeyNameFolderAnimationClipUnityNative;
				private const string PrefsKeyNameFolderMaterialAnimatorUnityNative = PrefsKeyPrefix + KeyNameFolderMaterialAnimatorUnityNative;
				private const string PrefsKeyNameFolderMaterialParticleUnityNative = PrefsKeyPrefix + KeyNameFolderMaterialParticleUnityNative;

				private readonly static GroupRuleNameAssetFolder Default = new GroupRuleNameAssetFolder(
					"Texture",				/* NameFolderTexture */
					"PrefabAnimation",		/* NameFolderPrefabAnimationSS6PU */
					"PrefabEffect",			/* NameFolderPrefabEffectSS6PU */
					"DataCellMap",			/* NameFolderDataCellMapSS6PU */
					"DataAnimation",		/* NameFolderDataAnimationSS6PU */
					"DataEffect",			/* NameFolderDataEffectSS6PU */
					"Material",				/* NameFolderMaterialAnimationSS6PU */
					"Material",				/* NameFolderMaterialEffectSS6PU */
					"PrefabSprite",			/* NameFolderPrefabAnimatorUnityNative */
					"PrefabParticle",		/* NameFolderPrefabParticleUnityNative */
					"DataAnimationClip",	/* NameFolderAnimationClipUnityNative */
					"Material",				/* NameFolderMaterialAnimatorUnityNative */
					"Material"				/* NameFolderMaterialParticleUnityNative */
				);
				#endregion Enums & Constants
			}

			public struct GroupPackAttributeAnimation
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Status;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Position;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Rotation;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Scaling;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack ScalingLocal;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack RateOpacity;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PartsColor;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PositionAnchor;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack RadiusCollision;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack UserData;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Instance;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack Effect;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PlainCell;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PlainSizeForce;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PlainVertexCorrection;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PlainOffsetPivot;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PlainPositionTexture;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PlainScalingTexture;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack PlainRotationTexture;

				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack FixIndexCellMap;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack FixCoordinate;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack FixPartsColor;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack FixUV0;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack FixSizeCollision;
				public Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack FixPivotCollision;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public GroupPackAttributeAnimation(	Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack status,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack position,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack rotation,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack scaling,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack scalingLocal,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack rateOpacity,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack partsColor,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack positionAnchor,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack radiusCollision,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack userData,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack instance,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack effect,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack plainCell,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack plainSizeForce,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack plainVertexCorrection,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack plainOffsetPivot,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack plainPositionTexture,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack plainScalingTexture,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack plainRotationTexture,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack fixIndexCellMap,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack fixCoordinate,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack fixPartsColor,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack fixUV0,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack fixSizeCollision,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack fixPivotCollision
												)
				{
					Status = status;
					Position = position;
					Rotation = rotation;
					Scaling = scaling;
					ScalingLocal = scalingLocal;
					RateOpacity = rateOpacity;
					PartsColor = partsColor;
					PositionAnchor = positionAnchor;
					RadiusCollision = radiusCollision;
					UserData = userData;
					Instance = instance;
					Effect = effect;

					PlainCell = plainCell;
					PlainSizeForce = plainSizeForce;
					PlainVertexCorrection = plainVertexCorrection;
					PlainOffsetPivot = plainOffsetPivot;
					PlainPositionTexture = plainPositionTexture;
					PlainScalingTexture = plainScalingTexture;
					PlainRotationTexture = plainRotationTexture;

					FixIndexCellMap = fixIndexCellMap;
					FixCoordinate = fixCoordinate;
					FixPartsColor = fixPartsColor;
					FixUV0 = fixUV0;
					FixSizeCollision = fixSizeCollision;
					FixPivotCollision = fixPivotCollision;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					Status = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyStatus, (int)Default.Status));
					Position = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPosition, (int)Default.Position));
					Rotation = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyRotation, (int)Default.Rotation));
					Scaling = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyScaling, (int)Default.Scaling));
					ScalingLocal = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyScalingLocal, (int)Default.ScalingLocal));
					RateOpacity = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyRateOpacity, (int)Default.RateOpacity));
					PartsColor = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPartsColor, (int)Default.PartsColor));
					PositionAnchor = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPositionAnchor, (int)Default.PositionAnchor));
					RadiusCollision = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyRadiusCollision, (int)Default.RadiusCollision));
					UserData = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyUserData, (int)Default.UserData));
					Instance = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyInstance, (int)Default.Instance));
					Effect = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyEffect, (int)Default.Effect));

					PlainCell = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPlainCell, (int)Default.PlainCell));
					PlainSizeForce = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPlainSizeForce, (int)Default.PlainSizeForce));
					PlainVertexCorrection = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPlainVertexCorrection, (int)Default.PlainVertexCorrection));
					PlainOffsetPivot = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPlainOffsetPivot, (int)Default.PlainOffsetPivot));
					PlainPositionTexture = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPlainPositionTexture, (int)Default.PlainPositionTexture));
					PlainScalingTexture = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPlainScalingTexture, (int)Default.PlainScalingTexture));
					PlainRotationTexture = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyPlainRotationTexture, (int)Default.PlainRotationTexture));

					FixIndexCellMap = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyFixIndexCellMap, (int)Default.FixIndexCellMap));
					FixCoordinate = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyFixCoordinate, (int)Default.FixCoordinate));
					FixPartsColor = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyFixPartsColor, (int)Default.FixPartsColor));
					FixUV0 = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyFixUV0, (int)Default.FixUV0));
					FixSizeCollision = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyFixSizeCollision, (int)Default.FixSizeCollision));
					FixPivotCollision = (Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack)(EditorPrefs.GetInt(PrefsKeyFixPivotCollision, (int)Default.FixPivotCollision));

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
					EditorPrefs.SetInt(PrefsKeyPartsColor, (int)PartsColor);
					EditorPrefs.SetInt(PrefsKeyPositionAnchor, (int)PositionAnchor);
					EditorPrefs.SetInt(PrefsKeyRadiusCollision, (int)RadiusCollision);
					EditorPrefs.SetInt(PrefsKeyUserData, (int)UserData);
					EditorPrefs.SetInt(PrefsKeyInstance, (int)Instance);
					EditorPrefs.SetInt(PrefsKeyEffect, (int)Effect);

					EditorPrefs.SetInt(PrefsKeyPlainCell, (int)PlainCell);
					EditorPrefs.SetInt(PrefsKeyPlainSizeForce, (int)PlainSizeForce);
					EditorPrefs.SetInt(PrefsKeyPlainVertexCorrection, (int)PlainVertexCorrection);
					EditorPrefs.SetInt(PrefsKeyPlainOffsetPivot, (int)PlainOffsetPivot);
					EditorPrefs.SetInt(PrefsKeyPlainPositionTexture, (int)PlainPositionTexture);
					EditorPrefs.SetInt(PrefsKeyPlainScalingTexture, (int)PlainScalingTexture);
					EditorPrefs.SetInt(PrefsKeyPlainRotationTexture, (int)PlainRotationTexture);

					EditorPrefs.SetInt(PrefsKeyFixIndexCellMap, (int)FixIndexCellMap);
					EditorPrefs.SetInt(PrefsKeyFixCoordinate, (int)FixCoordinate);
					EditorPrefs.SetInt(PrefsKeyFixPartsColor, (int)FixPartsColor);
					EditorPrefs.SetInt(PrefsKeyFixUV0, (int)FixUV0);
					EditorPrefs.SetInt(PrefsKeyFixSizeCollision, (int)FixSizeCollision);
					EditorPrefs.SetInt(PrefsKeyFixPivotCollision, (int)FixPivotCollision);

					return(true);
				}

				public string[] Export()
				{
					string[] textEncode = new string[25];
					string textValue;

					textValue = NameGetPackKind(Status);
					textEncode[0] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyStatus, textValue);

					textValue = NameGetPackKind(Position);
					textEncode[1] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPosition, textValue);

					textValue = NameGetPackKind(Rotation);
					textEncode[2] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyRotation, textValue);

					textValue = NameGetPackKind(Scaling);
					textEncode[3] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyScaling, textValue);

					textValue = NameGetPackKind(ScalingLocal);
					textEncode[4] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyScalingLocal, textValue);

					textValue = NameGetPackKind(RateOpacity);
					textEncode[5] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyRateOpacity, textValue);

					textValue = NameGetPackKind(PartsColor);
					textEncode[6] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPartsColor, textValue);

					textValue = NameGetPackKind(PositionAnchor);
					textEncode[7] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPositionAnchor, textValue);

					textValue = NameGetPackKind(RadiusCollision);
					textEncode[8] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyRadiusCollision, textValue);

					textValue = NameGetPackKind(UserData);
					textEncode[9] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyUserData, textValue);

					textValue = NameGetPackKind(Instance);
					textEncode[10] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyInstance, textValue);

					textValue = NameGetPackKind(Effect);
					textEncode[11] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyEffect, textValue);

					textValue = NameGetPackKind(PlainCell);
					textEncode[12] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPlainCell, textValue);

					textValue = NameGetPackKind(PlainSizeForce);
					textEncode[13] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPlainSizeForce, textValue);

					textValue = NameGetPackKind(PlainVertexCorrection);
					textEncode[14] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPlainVertexCorrection, textValue);

					textValue = NameGetPackKind(PlainOffsetPivot);
					textEncode[15] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPlainOffsetPivot, textValue);

					textValue = NameGetPackKind(PlainPositionTexture);
					textEncode[16] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPlainPositionTexture, textValue);

					textValue = NameGetPackKind(PlainScalingTexture);
					textEncode[17] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPlainScalingTexture, textValue);

					textValue = NameGetPackKind(PlainRotationTexture);
					textEncode[18] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyPlainRotationTexture, textValue);

					textValue = NameGetPackKind(FixIndexCellMap);
					textEncode[19] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFixIndexCellMap, textValue);

					textValue = NameGetPackKind(FixCoordinate);
					textEncode[20] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFixCoordinate, textValue);

					textValue = NameGetPackKind(FixPartsColor);
					textEncode[21] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFixPartsColor, textValue);

					textValue = NameGetPackKind(FixUV0);
					textEncode[22] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFixUV0, textValue);

					textValue = NameGetPackKind(FixSizeCollision);
					textEncode[23] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFixSizeCollision, textValue);

					textValue = NameGetPackKind(FixPivotCollision);
					textEncode[24] = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineEncodeCommand(TextKeyFixPivotCollision, textValue);

					return(textEncode);
				}

				public bool Import(string[] textArgument)
				{
					switch(textArgument[0])
					{
						case TextKeyStatus:
							Status = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPosition:
							Position = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyRotation:
							Status = KindGetPackName(textArgument[1]);
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

						case TextKeyPartsColor:
							PartsColor = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPositionAnchor:
							PositionAnchor = KindGetPackName(textArgument[1]);
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

						case TextKeyRadiusCollision:
							RadiusCollision = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainCell:
							PlainCell = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainSizeForce:
							PlainSizeForce = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainVertexCorrection:
							PlainVertexCorrection = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainOffsetPivot:
							PlainOffsetPivot = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainPositionTexture:
							PlainPositionTexture = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainScalingTexture:
							PlainScalingTexture = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyPlainRotationTexture:
							PlainRotationTexture = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyFixIndexCellMap:
							FixIndexCellMap = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyFixCoordinate:
							FixCoordinate = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyFixPartsColor:
							FixPartsColor = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyFixUV0:
							FixUV0 = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyFixSizeCollision:
							FixSizeCollision = KindGetPackName(textArgument[1]);
							return(true);

						case TextKeyFixPivotCollision:
							FixPivotCollision = KindGetPackName(textArgument[1]);
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
					if(false == capacityPack[(int)PartsColor].PartsColor)
					{
						PartsColor = PackError;
					}
					if(false == capacityPack[(int)PositionAnchor].PositionAnchor)
					{
						PositionAnchor = PackError;
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
					if(false == capacityPack[(int)PlainCell].PlainCell)
					{
						PlainCell = PackError;
					}
					if(false == capacityPack[(int)PlainSizeForce].PlainSizeForce)
					{
						PlainSizeForce = PackError;
					}
					if(false == capacityPack[(int)PlainVertexCorrection].PlainVertexCorrection)
					{
						PlainVertexCorrection = PackError;
					}
					if(false == capacityPack[(int)PlainOffsetPivot].PlainOffsetPivot)
					{
						PlainOffsetPivot = PackError;
					}
					if(false == capacityPack[(int)PlainPositionTexture].PlainPositionTexture)
					{
						PlainPositionTexture = PackError;
					}
					if(false == capacityPack[(int)PlainScalingTexture].PlainScalingTexture)
					{
						PlainScalingTexture = PackError;
					}
					if(false == capacityPack[(int)PlainRotationTexture].PlainRotationTexture)
					{
						PlainRotationTexture = PackError;
					}
					if(false == capacityPack[(int)RadiusCollision].RadiusCollision)
					{
						RadiusCollision = PackError;
					}
					if(false == capacityPack[(int)FixIndexCellMap].FixIndexCellMap)
					{
						FixIndexCellMap = PackError;
					}
					if(false == capacityPack[(int)FixCoordinate].FixCoordinate)
					{
						FixCoordinate = PackError;
					}
					if(false == capacityPack[(int)FixPartsColor].FixPartsColor)
					{
						FixPartsColor = PackError;
					}
					if(false == capacityPack[(int)FixUV0].FixUV0)
					{
						FixUV0 = PackError;
					}
					if(false == capacityPack[(int)FixSizeCollision].FixSizeCollision)
					{
						FixSizeCollision = PackError;
					}
					if(false == capacityPack[(int)FixPivotCollision].FixPivotCollision)
					{
						FixPivotCollision = PackError;
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
				private const string KeyPosition = "Position";
				private const string KeyRotation = "Rotation";
				private const string KeyScaling = "Scaling";
				private const string KeyScalingLocal = "ScalingLocal";
				private const string KeyRateOpacity = "RateOpacity";
				private const string KeyPartsColor = "PartsColor";
				private const string KeyPositionAnchor = "PositionAnchor";
				private const string KeyRadiusCollision = "RadiusCollision";
				private const string KeyUserData = "UserData";
				private const string KeyInstance = "Instance";
				private const string KeyEffect = "Effect";
				private const string KeyPlainCell = "PlainCell";
				private const string KeyPlainSizeForce = "PlainSizeForce";
				private const string KeyPlainVertexCorrection = "PlainVertexCorrection";
				private const string KeyPlainOffsetPivot = "PlainOffsetPivot";
				private const string KeyPlainPositionTexture = "PlainPositionTexture";
				private const string KeyPlainScalingTexture = "PlainScalingTexture";
				private const string KeyPlainRotationTexture = "PlainRotationTexture";
				private const string KeyFixIndexCellMap = "FixIndexCellMap";
				private const string KeyFixCoordinate = "FixCoordinate";
				private const string KeyFixPartsColor = "FixPartsColor";
				private const string KeyFixUV0 = "FixUV0";
				private const string KeyFixSizeCollision = "FixSizeCollision";
				private const string KeyFixPivotCollision = "FixPivotCollision";

				private const string TextKeyPrefix = "PackAttributeAnimation_";
				private const string TextKeyStatus = TextKeyPrefix + KeyStatus;
				private const string TextKeyPosition = TextKeyPrefix + KeyPosition;
				private const string TextKeyRotation = TextKeyPrefix + KeyRotation;
				private const string TextKeyScaling = TextKeyPrefix + KeyScaling;
				private const string TextKeyScalingLocal = TextKeyPrefix + KeyScalingLocal;
				private const string TextKeyRateOpacity = TextKeyPrefix + KeyRateOpacity;
				private const string TextKeyPartsColor = TextKeyPrefix + KeyPartsColor;
				private const string TextKeyPositionAnchor = TextKeyPrefix + KeyPositionAnchor;
				private const string TextKeyRadiusCollision = TextKeyPrefix + KeyRadiusCollision;
				private const string TextKeyUserData = TextKeyPrefix + KeyUserData;
				private const string TextKeyInstance = TextKeyPrefix + KeyInstance;
				private const string TextKeyEffect = TextKeyPrefix + KeyEffect;
				private const string TextKeyPlainCell = TextKeyPrefix + KeyPlainCell;
				private const string TextKeyPlainSizeForce = TextKeyPrefix + KeyPlainSizeForce;
				private const string TextKeyPlainVertexCorrection = TextKeyPrefix + KeyPlainVertexCorrection;
				private const string TextKeyPlainOffsetPivot = TextKeyPrefix + KeyPlainOffsetPivot;
				private const string TextKeyPlainPositionTexture = TextKeyPrefix + KeyPlainPositionTexture;
				private const string TextKeyPlainScalingTexture = TextKeyPrefix + KeyPlainScalingTexture;
				private const string TextKeyPlainRotationTexture = TextKeyPrefix + KeyPlainRotationTexture;
				private const string TextKeyFixIndexCellMap = TextKeyPrefix + KeyFixIndexCellMap;
				private const string TextKeyFixCoordinate = TextKeyPrefix + KeyFixCoordinate;
				private const string TextKeyFixPartsColor = TextKeyPrefix + KeyFixPartsColor;
				private const string TextKeyFixUV0 = TextKeyPrefix + KeyFixUV0;
				private const string TextKeyFixSizeCollision = TextKeyPrefix + KeyFixSizeCollision;
				private const string TextKeyFixPivotCollision = TextKeyPrefix + KeyFixPivotCollision;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyStatus = PrefsKeyPrefix + KeyStatus;
				private const string PrefsKeyPosition = PrefsKeyPrefix + KeyPosition;
				private const string PrefsKeyRotation = PrefsKeyPrefix + KeyRotation;
				private const string PrefsKeyScaling = PrefsKeyPrefix + KeyScaling;
				private const string PrefsKeyScalingLocal = PrefsKeyPrefix + KeyScalingLocal;
				private const string PrefsKeyRateOpacity = PrefsKeyPrefix + KeyRateOpacity;
				private const string PrefsKeyPartsColor = PrefsKeyPrefix + KeyPartsColor;
				private const string PrefsKeyPositionAnchor = PrefsKeyPrefix + KeyPositionAnchor;
				private const string PrefsKeyRadiusCollision = PrefsKeyPrefix + KeyRadiusCollision;
				private const string PrefsKeyUserData = PrefsKeyPrefix + KeyUserData;
				private const string PrefsKeyInstance = PrefsKeyPrefix + KeyInstance;
				private const string PrefsKeyEffect = PrefsKeyPrefix + KeyEffect;
				private const string PrefsKeyPlainCell = PrefsKeyPrefix + KeyPlainCell;
				private const string PrefsKeyPlainSizeForce = PrefsKeyPrefix + KeyPlainSizeForce;
				private const string PrefsKeyPlainVertexCorrection = PrefsKeyPrefix + KeyPlainVertexCorrection;
				private const string PrefsKeyPlainOffsetPivot = PrefsKeyPrefix + KeyPlainOffsetPivot;
				private const string PrefsKeyPlainPositionTexture = PrefsKeyPrefix + KeyPlainPositionTexture;
				private const string PrefsKeyPlainScalingTexture = PrefsKeyPrefix + KeyPlainScalingTexture;
				private const string PrefsKeyPlainRotationTexture = PrefsKeyPrefix + KeyPlainRotationTexture;
				private const string PrefsKeyFixIndexCellMap = PrefsKeyPrefix + KeyFixIndexCellMap;
				private const string PrefsKeyFixCoordinate = PrefsKeyPrefix + KeyFixCoordinate;
				private const string PrefsKeyFixPartsColor = PrefsKeyPrefix + KeyFixPartsColor;
				private const string PrefsKeyFixUV0 = PrefsKeyPrefix + KeyFixUV0;
				private const string PrefsKeyFixSizeCollision = PrefsKeyPrefix + KeyFixSizeCollision;
				private const string PrefsKeyFixPivotCollision = PrefsKeyPrefix + KeyFixPivotCollision;

				private readonly static GroupPackAttributeAnimation Default = new GroupPackAttributeAnimation(
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* Status */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* Position */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* Rotation */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* Scaling */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* ScalingLocal */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* RateOpacity */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* PartsColor */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* PositionAnchor */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* RadiusCollision */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* UserData */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* Instance */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* Effect */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* PlainCell */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* PlainSizeForce */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* PlainVertexCorrection */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* PlainOffsetPivot */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* PlainPositionTexture */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* PlainScalingTexture */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* PlainRotationTexture */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* FixIndexCellMap */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.CPE_FLYWEIGHT,	/* FixCoordinate */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.CPE_FLYWEIGHT,	/* FixPartsColor */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.CPE_FLYWEIGHT,	/* FixUV0 */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE,	/* FixSizeCollision */
					Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack.STANDARD_CPE	/* FixPivotCollision */
				);
				#endregion Enums & Constants
			}
			#endregion Classes, Structs & Interfaces
		}
		#endregion Classes, Structs & Interfaces
	}
}
