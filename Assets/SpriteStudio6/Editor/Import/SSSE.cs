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
		public static partial class SSSE
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static Information Parse(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
												string nameFile,
												LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
											)
			{
				const string messageLogPrefix = "Parse SSSE";
				Information informationSSSE = null;

				/* ".ssce" Load */
				if(false == System.IO.File.Exists(nameFile))
				{
					LogError(messageLogPrefix, "File Not Found", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				System.Xml.XmlDocument xmlSSSE = new System.Xml.XmlDocument();
				xmlSSSE.Load(nameFile);

				/* Check Version */
				System.Xml.XmlNode nodeRoot = xmlSSSE.FirstChild;
				nodeRoot = nodeRoot.NextSibling;
				KindVersion version = (KindVersion)(LibraryEditor_SpriteStudio6.Utility.XML.VersionGet(nodeRoot, "SpriteStudioSoundList", (int)KindVersion.ERROR, true));
				switch(version)
				{
					case KindVersion.ERROR:
						LogError(messageLogPrefix, "Version Invalid", nameFile, informationSSPJ);
						goto Parse_ErrorEnd;

					case KindVersion.CODE_020000_DUMMY:
						/* MEMO: This version is still under development. (Just admitting transient) */
						break;

					case KindVersion.CODE_010000:
						break;

					default:
						if(KindVersion.TARGET_EARLIEST > version)
						{
							version = KindVersion.TARGET_EARLIEST;
							if(true == setting.CheckVersion.FlagInvalidSSSE)
							{
								LogWarning(messageLogPrefix, "Version Too Early", nameFile, informationSSPJ);
							}
						}
						else
						{
							version = KindVersion.TARGET_LATEST;
							if(true == setting.CheckVersion.FlagInvalidSSSE)
							{
								LogWarning(messageLogPrefix, "Version Unknown", nameFile, informationSSPJ);
							}
						}
						break;
				}

				/* Create Information */
				informationSSSE = new Information();
				if(null == informationSSSE)
				{
					LogError(messageLogPrefix, "Not Enough Memory", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				informationSSSE.CleanUp();
				informationSSSE.Version = version;

				/* Get Base-Directories */
				LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out informationSSSE.NameDirectory, out informationSSSE.NameFileBody, out informationSSSE.NameFileExtension, nameFile);

				/* Decode Tags */
				System.Xml.NameTable nodeNameSpace = new System.Xml.NameTable();
				System.Xml.XmlNamespaceManager managerNameSpace = new System.Xml.XmlNamespaceManager(nodeNameSpace);
				System.Xml.XmlNodeList listNode = null;
				string valueText = string.Empty;

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "table/id", managerNameSpace);
				if(true == string.IsNullOrEmpty(valueText))
				{
					/* MEMO: Usually impossible. */
					informationSSSE.ID = -1;
				}
				else
				{
					informationSSSE.ID = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
				}

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "table/Alias", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
					/* MEMO: Trim to be sure. */
					valueText = valueText.Trim();
				}
				if(true == string.IsNullOrEmpty(valueText))
				{
					/* MEMO: "Alias" may have no value. */
					informationSSSE.Name = string.Empty;
				}
				else
				{
					informationSSSE.Name = valueText;
				}

				/* Decode Sound-Data */
				listNode = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "table/sounds/value", managerNameSpace);
				List<Information.Sound> listSound = new List<Information.Sound>();
				listSound.Clear();
				foreach(System.Xml.XmlNode nodeSound in listNode)
				{
					Information.Sound dataSound = new Information.Sound();
					dataSound.CleanUp();

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeSound, "Alias", managerNameSpace);
					if(false == string.IsNullOrEmpty(valueText))
					{
						/* MEMO: Trim to be sure. */
						valueText = valueText.Trim();
					}
					if(true == string.IsNullOrEmpty(valueText))
					{
						dataSound.Name = string.Empty;
					}
					else
					{
						dataSound.Name = valueText;
					}

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeSound, "filename", managerNameSpace);
					if(false == string.IsNullOrEmpty(valueText))
					{
						/* MEMO: Text is file-name, so trim to be sure. */
						valueText = valueText.Trim();
					}
					if(true == string.IsNullOrEmpty(valueText))
					{
						dataSound.PathOriginalFile = string.Empty;
					}
					else
					{
						dataSound.PathOriginalFile = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(valueText);
					}

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeSound, "msec", managerNameSpace);
					if(true == string.IsNullOrEmpty(valueText))
					{
						/* MEMO: Usually impossible. */
						dataSound.TimeTotal = 0.0f;
					}
					else
					{
						int msec = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
						dataSound.TimeTotal = (float)msec * (1.0f / 1000.0f);	/* msec -> sec */
					}

					listSound.Add(dataSound);
				}
				informationSSSE.TableSound = listSound.ToArray();

				return(informationSSSE);

			Parse_ErrorEnd:
				if(null != informationSSSE)
				{
					informationSSSE.CleanUp();
				}
				return(null);
			}

			private static void LogError(string messagePrefix, string message, string nameFile, LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ)
			{
				LibraryEditor_SpriteStudio6.Utility.Log.Error(	messagePrefix
																+ ": " + message
																+ " [" + nameFile + "]"
																+ " in <" + informationSSPJ.FileNameGetFullPath() + ">"
															);
			}

			private static void LogWarning(string messagePrefix, string message, string nameFile, LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ)
			{
				LibraryEditor_SpriteStudio6.Utility.Log.Warning(	messagePrefix
																	+ ": " + message
																	+ " [" + nameFile + "]"
																	+ " in \"" + informationSSPJ.FileNameGetFullPath() + "\""
																);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public enum KindVersion
			{
				ERROR = 0x00000000,
				CODE_020000_DUMMY = 0x00020000,	/* after SS7.1 less than 1.00.00 (UnderDeveloping)  */
				CODE_010000 = 0x00010000,		/* after SS7.1 (Released) */

				TARGET_EARLIEST = CODE_010000,
				TARGET_LATEST = CODE_010000
			}

			private const string ExtentionFile = ".ssse";
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public class Information
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public LibraryEditor_SpriteStudio6.Import.SSSE.KindVersion Version;
				public Library_SpriteStudio6.Data.Sound.Inventory Data;

				public string NameDirectory;
				public string NameFileBody;
				public string NameFileExtension;

				public int ID;
				public string Name;
				public Sound[] TableSound;

				public LibraryEditor_SpriteStudio6.Import.Assets<Script_SpriteStudio6_DataSoundList> DataSoundListSS6PU;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Version = LibraryEditor_SpriteStudio6.Import.SSSE.KindVersion.ERROR;
					Data = new Library_SpriteStudio6.Data.Sound.Inventory();

					NameDirectory = string.Empty;
					NameFileBody = string.Empty;
					NameFileExtension = string.Empty;

					ID = -1;
					Name = string.Empty;
					TableSound = null;

					DataSoundListSS6PU.CleanUp();
					DataSoundListSS6PU.BootUp(1);	/* Always 1 */
				}
				#endregion Functions

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				public class Sound
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public string Name;
					public string PathOriginalFile;					/* Ful path */
					public float TimeTotal;							/* 1.0f: 1sec */
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public void CleanUp()
					{
						Name = string.Empty;
						PathOriginalFile = string.Empty;
						TimeTotal = 0.0f;
					}
					#endregion Functions
				}
				#endregion Classes, Structs & Interfaces
			}

			public static partial class ModeSS6PU
			{
				/* ----------------------------------------------- Functions */
				#region Functions
				public static bool ConvertSound(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
													LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
													LibraryEditor_SpriteStudio6.Import.SSSE.Information informationSSSE
												)
				{	/* Convert-SS6PU Pass-1 ... Transfer necessary data from the temporary. */
//					const string messageLogPrefix = "Convert (Sound List)";

					int countSound = informationSSSE.TableSound.Length;

					informationSSSE.Data.Name = string.Copy(informationSSSE.NameFileBody);
					informationSSSE.Data.Sound = new Library_SpriteStudio6.Data.Sound.Inventory.Fragment[countSound];

					for(int i=0; i<countSound; i++)	{
						informationSSSE.Data.Sound[i].Name = string.Copy(informationSSSE.TableSound[i].Name);
						informationSSSE.Data.Sound[i].Duration = informationSSSE.TableSound[i].TimeTotal;
					}

					return(true);

//				ConvertSound_ErrorEnd:;
//					return(false);
				}
				#endregion Functions

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
