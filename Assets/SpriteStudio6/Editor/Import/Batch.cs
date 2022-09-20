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
		public static partial class Batch
		{
			/* ----------------------------------------------- Variables & Properties */
			#region Variables & Properties
			private static string NameFolderBaseExternal = "";
			private static string NameFolderRootAsset = "";

			private static string NameBaseFolderSetting = "";
			private static string NameBaseFolderAsset = "";
			private static string NameBaseFolderData = "";

			private static LibraryEditor_SpriteStudio6.Import.Setting.KindMode Mode;
			#endregion Variables & Properties

			/* ----------------------------------------------- Functions */
			#region Functions
			public static bool Exec(	ref Setting settingBatchImporter,
										ref LibraryEditor_SpriteStudio6.Import.Setting settingImportInitial,
										string nameFileList,	/* Full-Path */
										string nameFileLog	/* Full-Path */
									)
			{
//				const string messageLogPrefix = "Batch-Importer";

				if(true == string.IsNullOrEmpty(nameFileList))
				{
					return(false);
				}

				/* Copy Setting (for Overwriting) */
				LibraryEditor_SpriteStudio6.Import.Setting settingImport = settingImportInitial;

				/* Get BaseDirectory (External File) */
#if false
				/* MEMO: Base directory is asset's directory when specify relative path in ListFile. */
//				string nameDirectoryBaseExternal = LibraryEditor_SpriteStudio6.Utility.File.NamePathRootNative;
#else
				/* MEMO: Base directory is ListFile's directory when specify relative path in ListFile. */
				string nameFile;
				string nameExtension;
				LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out NameFolderBaseExternal, out nameFile, out nameExtension, nameFileList);
#endif
				NameFolderBaseExternal = PathNormalizeDelimiter(NameFolderBaseExternal, true);

				NameFolderRootAsset = "/";
				NameFolderRootAsset = PathNormalizeDelimiter(NameFolderRootAsset, true);

				NameBaseFolderSetting = string.Copy(NameFolderBaseExternal);
				NameBaseFolderAsset = string.Copy(NameFolderRootAsset);
				NameBaseFolderData = string.Copy(NameFolderBaseExternal);

				/* Set Log-File */
				System.IO.StreamWriter streamLog = null;
				if(false == string.IsNullOrEmpty(nameFileLog))
				{
					streamLog = new System.IO.StreamWriter(nameFileLog, false, System.Text.Encoding.Default);	/* Overwrite */
				}
				LibraryEditor_SpriteStudio6.Utility.Log.StreamExternal = streamLog;

				/* Open List-File */
				System.IO.StreamReader streamList = new System.IO.StreamReader(nameFileList, System.Text.Encoding.Default);

				/* Log Date */
				System.DateTimeOffset dateTime = System.DateTimeOffset.Now;
				LibraryEditor_SpriteStudio6.Utility.Log.Message("[Date imported] " + dateTime.ToString(), true, false);	/* External-File only */
				LibraryEditor_SpriteStudio6.Utility.Log.Message("[In charge] " + System.Environment.MachineName + " (" + System.Environment.UserName + ")", true, false);	/* External-File only */

				/* Decode List-File (1 Line) */
				Mode = LibraryEditor_SpriteStudio6.Import.Setting.KindMode.SS6PU;
				int indexLine = 0;
				string textLine = "";
				string textLineValid = "";
				bool flagValid;
				while(0 <= streamList.Peek())
				{
					/* Read & Trim 1-Line */
					flagValid = true;
					textLine = streamList.ReadLine();
					indexLine++;
					switch(LibraryEditor_SpriteStudio6.Utility.ExternalText.TypeGetLine(out textLineValid, textLine))
					{
						case LibraryEditor_SpriteStudio6.Utility.ExternalText.KindType.COMMAND:
							/* Setting Command */
							flagValid = DecodeCommand(ref settingBatchImporter, ref settingImport, indexLine, textLineValid);
							break;

						case LibraryEditor_SpriteStudio6.Utility.ExternalText.KindType.NORMAL:
							/* File-Name to import */
							flagValid = ImportFile(ref settingBatchImporter, ref settingImport, indexLine, textLineValid);
							break;

						case LibraryEditor_SpriteStudio6.Utility.ExternalText.KindType.IGNORE:
							/* Remarks */
							flagValid = true;
							break;

						default:
							LogError("Syntax Error [" + textLine + "]", indexLine);
							flagValid = false;
							break;
					}

					/* Check Stopping-Processing */
					if((false == flagValid) && (false == settingBatchImporter.FlagNotBreakOnError))
					{
						return(false);
					}
				}

				/* Close List-File */
				if(null != streamList)
				{
					streamList.Close();
					streamList = null;
				}

				/* Close Log-File */
				if(null != streamLog)
				{
					streamLog.Close();
					streamLog = null;
				}
				LibraryEditor_SpriteStudio6.Utility.Log.StreamExternal = null;

				return(true);
			}

			private static bool DecodeCommand(	ref Setting settingBatchImporter,
												ref LibraryEditor_SpriteStudio6.Import.Setting settingImport,
												int indexLine,
												string textLine
											)
			{
				/* Check Text */
				string[] textSplit = LibraryEditor_SpriteStudio6.Utility.ExternalText.LineDecodeCommand(textLine);
				if(null == textSplit)
				{
					LogError("Command Line empty [" + textLine + "]", indexLine);
					return(false);
				}

				/* Decode Command */
				int countArgument = textSplit.Length;
				switch(textSplit[0])
				{
					case TextKeyNameBaseFolderSetting:
						if(1 >= countArgument)
						{	/* Set default */
							NameBaseFolderSetting = string.Copy(NameFolderBaseExternal);
						}
						else
						{
							NameBaseFolderSetting = PathGetNormalizedAbsolute(textSplit[1], NameBaseFolderSetting, true);
							if(null == NameBaseFolderSetting)
							{	/* Error */
								LogError("Path contains illegal characters. [" + textSplit[1] + "]", indexLine);
								NameBaseFolderSetting = string.Copy(NameFolderBaseExternal);
								return(false);
							}
						}
						return(true);

					case TextKeyNameBaseFolderAsset:
						if(1 >= countArgument)
						{	/* Set default */
							NameBaseFolderAsset = string.Copy(NameFolderRootAsset);
						}
						else
						{
							NameBaseFolderAsset = PathGetNormalizedAbsolute(textSplit[1], NameBaseFolderAsset, true);
							if(null == NameBaseFolderAsset)
							{	/* Error */
								LogError("Path contains illegal characters. [" + textSplit[1] + "]", indexLine);
								NameBaseFolderAsset = string.Copy(NameFolderRootAsset);
								return(false);
							}
						}
						return(true);

					case TextKeyNameBaseFolderData:
						if(1 >= countArgument)
						{	/* Set default */
							NameBaseFolderAsset = string.Copy(NameFolderBaseExternal);
						}
						else
						{
							NameBaseFolderData = PathGetNormalizedAbsolute(textSplit[1], NameBaseFolderData, true);
							if(null == NameBaseFolderData)
							{	/* Error */
								LogError("Path contains illegal characters. [" + textSplit[1] + "]", indexLine);
								NameBaseFolderData = string.Copy(NameFolderBaseExternal);
								return(false);
							}
						}
						return(true);

					case TextKeyNameBaseSettingFile:
						{
							string nameSettingFile = PathGetNormalizedAbsolute(textSplit[1], NameBaseFolderSetting , false);
							if(true == settingImport.ImportFile(nameSettingFile))
							{
								/* MEMO: Overwrite only when decoding is successful. */
								settingBatchImporter.SettingOverwriteImport(settingImport);
							}
							else
							{	/* Error */
								LogError("Setting File Not Found [" + textSplit[1] + "]", indexLine);
								return(false);
							}
						}
						return(true);

					case LibraryEditor_SpriteStudio6.Import.Setting.TextKeyMode:
						{
							LibraryEditor_SpriteStudio6.Import.Setting.KindMode mode = LibraryEditor_SpriteStudio6.Import.Setting.ImportCommonMode(textSplit[1]);
							if(LibraryEditor_SpriteStudio6.Import.Setting.KindMode.SS6PU <= mode)
							{	/* Valid */
								Mode = mode;
							}
							else
							{	/* Invalid */
								LogError("Invalid Import-Mode [" + textSplit[1] + "]", indexLine);
								return(false);
							}
						}
						return(true);

					default:
						break;
				}

				/* MEMO: Interpret as change of individual setting of Importer when not command dedicated to Batch-Importer. */
				/* MEMO: When individual setting change, not overwrite with Batch-Importer setting. */
				if(false == settingImport.Import(textLine))
				{
					LogError("Unknown Command [" + textLine + "]", indexLine);
					return(false);
				}

				return(true);
			}

			private static bool ImportFile(	ref Setting settingBatchImporter,
											ref LibraryEditor_SpriteStudio6.Import.Setting settingImport,
											int indexLine,
											string textLine
										)
			{
				string nameFile = string.Copy(textLine);
				nameFile = PathGetNormalizedAbsolute(textLine, NameBaseFolderData, false);

				/* Import SSPJ */
				settingImport.Mode = Mode;	/* Overwrite */
				bool flagSuccess = LibraryEditor_SpriteStudio6.Import.Exec(	ref settingImport,
																			nameFile,
																			LibraryEditor_SpriteStudio6.Utility.File.NamePathRootAsset + NameBaseFolderAsset,	/* Caution that absolute path in Assets (start at "/") */
																			true
																		);

				/* Clean up memory (Garbage-Collection) */
				System.GC.Collect();

				return(flagSuccess);
			}

			private static string PathNormalizeDelimiter(string namePath, bool flagDirectory)
			{
				if(true == string.IsNullOrEmpty(namePath))
				{	/* become root if add "/" to end of empty... */
					return("");
				}

				string namePathNormalized = namePath.Replace("\\", "/");	/* "\" -> "/" */
				if(true == flagDirectory)
				{
					if(false == namePathNormalized.EndsWith("/"))
					{
						namePathNormalized += "/";
					}
				}
				return(namePathNormalized);
			}

			private static string PathGetNormalizedAbsolute(string namePath, string namePathBase, bool flagDirectory)
			{
				/* MEMO: "System.IO.GetFullPath" can not normalize path normally if path points to machines on LAN.   */
				/*       ("../" etc. are ignored. e.g. "//machine/folder1/../aaa.txt" -> "//machine/folder1/aaa.txt") */
				/* MEMO: If "namePathBase" is a relative path, "System.Uri" misinterpret the first folder name */
				/*        as machine name and converted to lowercase. (e.g. "Assets/" -> "file://assets/)      */
				/* MEMO: If "file://" does not exist, "System.Uri"'s constructor fails with directory specification such as "/". */
				/*       However, if add "file://", always be treated as root. So need to delete it at the end.                  */
				bool flagPathBaseIsRooted = LibraryEditor_SpriteStudio6.Utility.File.PathCheckRoot(namePathBase);
				System.Uri uriBase = new System.Uri(PathNormalizeDelimiter("file://" + namePathBase, true));
				System.Uri uri = new System.Uri(uriBase, PathNormalizeDelimiter(namePath, flagDirectory));
				string namePathNormalized = PathNormalizeDelimiter(uri.LocalPath, flagDirectory);	/* LocalPath's delimiter is "\". */
				if(false == flagPathBaseIsRooted)
				{
					if(true == namePathNormalized.StartsWith("//"))
					{
						namePathNormalized = namePathNormalized.Remove(0, 2);
					}
				}
				return(namePathNormalized);
			}

			private static string PathGetNormalizedRelative(string namePath, string namePathBase, bool flagDirectory)
			{
				/* MEMO: Code at a later date. */
				return(null);
			}

			private static void LogError(string message, int indexLine)
			{
				LibraryEditor_SpriteStudio6.Utility.Log.Error(	"Batch-Importer: "
																+ message
																+ " in Line=" + indexLine.ToString()
															);
			}

			private static void LogWarning(string message, int indexLine)
			{
				LibraryEditor_SpriteStudio6.Utility.Log.Warning(	"Batch-Importer: "
																	+ message
																	+ " in Line=" + indexLine.ToString()
																);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public const string TextKeyNameBaseFolderSetting = "NameBaseFolderSetting";
			public const string TextKeyNameBaseFolderAsset = "NameBaseFolderAsset";
			public const string TextKeyNameBaseFolderData = "NameBaseFolderData";
			public const string TextKeyNameBaseSettingFile = "SettingFile";
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public struct Setting
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public bool FlagNotBreakOnError;
				public bool FlagEnableConfirmOverWrite;
				public bool FlagEnableCheckVersion;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public Setting(	bool flagNotBreakOnError,
								bool flagEnableConfirmOverWrite,
								bool flagEnableCheckVersion
							)
				{
					FlagNotBreakOnError = flagNotBreakOnError;
					FlagEnableConfirmOverWrite = flagEnableConfirmOverWrite;
					FlagEnableCheckVersion = flagEnableCheckVersion;
				}

				public void CleanUp()
				{
					this = Default;
				}

				public bool Load()
				{
					FlagNotBreakOnError = EditorPrefs.GetBool(PrefsKeyFlagNotBreakOnError, Default.FlagNotBreakOnError);
					FlagEnableConfirmOverWrite = EditorPrefs.GetBool(PrefsKeyFlagEnableConfirmOverWrite, Default.FlagEnableConfirmOverWrite);
					FlagEnableCheckVersion = EditorPrefs.GetBool(PrefsKeyFlagEnableCheckVersion, Default.FlagEnableCheckVersion);

					return(true);
				}

				public bool Save()
				{
					EditorPrefs.SetBool(PrefsKeyFlagNotBreakOnError, FlagNotBreakOnError);
					EditorPrefs.SetBool(PrefsKeyFlagEnableConfirmOverWrite, FlagEnableConfirmOverWrite);
					EditorPrefs.SetBool(PrefsKeyFlagEnableCheckVersion, FlagEnableCheckVersion);

					return(true);
				}

				public bool Export()
				{
					/* MEMO: Export no parameters */
					return(false);
				}

				public bool Import()
				{
					/* MEMO: Import no parameters */
					return(false);
				}

				internal void SettingOverwriteImport(LibraryEditor_SpriteStudio6.Import.Setting settingImport)
				{
					if(false == FlagEnableConfirmOverWrite)
					{
						settingImport.ConfirmOverWrite = LibraryEditor_SpriteStudio6.Import.Setting.GroupConfirmOverWrite.Default;	/* All false */
					}
					if(false == FlagEnableCheckVersion)
					{
						settingImport.CheckVersion = LibraryEditor_SpriteStudio6.Import.Setting.GroupCheckVersion.Default;	/* All false */
					}
				}
				#endregion Functions

				/* ----------------------------------------------- Enums & Constants */
				#region Enums & Constants
				private const string KeyFlagNotBreakOnError = "FlagNotBreakOnError";
				private const string KeyFlagEnableConfirmOverWrite = "FlagEnableConfirmOverWrite";
				private const string KeyFlagEnableCheckVersion = "FlagEnableCheckVersion";

				private const string TextKeyPrefix = "Batch_";
				private const string TextKeyFlagNotBreakOnError = TextKeyPrefix + KeyFlagNotBreakOnError;
				private const string TextKeyFlagEnableConfirmOverWrite = TextKeyPrefix + KeyFlagEnableConfirmOverWrite;
				private const string TextKeyFlagEnableCheckVersion = TextKeyPrefix + KeyFlagEnableCheckVersion;

				private const string PrefsKeyPrefix = LibraryEditor_SpriteStudio6.Import.Setting.PrefsKeyPrefix + TextKeyPrefix;
				private const string PrefsKeyFlagNotBreakOnError = PrefsKeyPrefix + TextKeyFlagNotBreakOnError;
				private const string PrefsKeyFlagEnableConfirmOverWrite = PrefsKeyPrefix + TextKeyFlagEnableConfirmOverWrite;
				private const string PrefsKeyFlagEnableCheckVersion = PrefsKeyPrefix + TextKeyFlagEnableCheckVersion;

				private readonly static Setting Default = new Setting(
					true,	/* FlagNotBreakOnError */
					false,	/* FlagEnableConfirmOverWrite */
					true	/* FlagEnableCheckVersion */
				);
				#endregion Enums & Constants
			}
			#endregion Classes, Structs & Interfaces
		}
		#endregion Classes, Structs & Interfaces
	}
}