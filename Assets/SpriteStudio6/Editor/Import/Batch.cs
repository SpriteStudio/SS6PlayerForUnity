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
		public static partial class Batch
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static bool Exec(	ref Setting settingBatchImporter,
										ref LibraryEditor_SpriteStudio6.Import.Setting settingImport,
										string nameFileList,
										string nameFileLog
									)
			{
				return(true);
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