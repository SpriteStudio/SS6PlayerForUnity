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
		public static partial class SSQE
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static int ValueTextToInt(string valueText)
			{
				if(false == string.IsNullOrEmpty(valueText))
				{
					return(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText));
				}

				return(0);
			}

			public static bool ValueTextToBool(string valueText)
			{
				if(false == string.IsNullOrEmpty(valueText))
				{
					return(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText));
				}

				return(false);
			}

			public static Library_SpriteStudio6.Data.Sequence.Type ValueTextToSequenceType(string valueText)
			{
				if(false == string.IsNullOrEmpty(valueText))
				{

					switch(valueText)
					{
						case "LAST":
							return(Library_SpriteStudio6.Data.Sequence.Type.LAST);
						case "KEEP":
							return(Library_SpriteStudio6.Data.Sequence.Type.KEEP);
						case "TOP":
							return(Library_SpriteStudio6.Data.Sequence.Type.TOP);
					}
				}

				return(Library_SpriteStudio6.Data.Sequence.Type.INVALID);
			}

			public static Information Parse(ref LibraryEditor_SpriteStudio6.Import.Setting setting,
												string nameFile,
												LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
											)
			{
				const string messageLogPrefix = "Parse SSQE";
				Information informationSSQE = null;

				/* ".ssee" Load */
				if(false == System.IO.File.Exists(nameFile))
				{
					LogError(messageLogPrefix, "File Not Found", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				System.Xml.XmlDocument xmlSSQE = new System.Xml.XmlDocument();
				xmlSSQE.Load(nameFile);

				/* Check Version */
				System.Xml.XmlNode nodeRoot = xmlSSQE.FirstChild;
				nodeRoot = nodeRoot.NextSibling;
				KindVersion version = (KindVersion)(LibraryEditor_SpriteStudio6.Utility.XML.VersionGet(nodeRoot, "SpriteStudioSequencePack", (int)KindVersion.ERROR, true));

				/* MEMO: Loose version check                                                       */
				/*       If you check strictly, there are a lot of datas that can not be imported. */
				switch(version)
				{
					case KindVersion.ERROR:
						LogError(messageLogPrefix, "Version Invalid", nameFile, informationSSPJ);
						goto Parse_ErrorEnd;

					case KindVersion.CODE_010000:
						/* MEMO: Read all as Ver.1.01.00. */
						version = KindVersion.CODE_010000;
						break;
					default:
						if(KindVersion.TARGET_EARLIEST > version)
						{
							version = KindVersion.TARGET_EARLIEST;
							if(true == setting.CheckVersion.FlagInvalidSSQE)
							{
								LogWarning(messageLogPrefix, "Version Too Early", nameFile, informationSSPJ);
							}
						}
						else
						{
							version = KindVersion.TARGET_LATEST;
							if(true == setting.CheckVersion.FlagInvalidSSQE)
							{
								LogWarning(messageLogPrefix, "Version Unknown", nameFile, informationSSPJ);
							}
						}
						break;
				}

				/* Create Information */
				informationSSQE = new Information();
				if(null == informationSSQE)
				{
					LogError(messageLogPrefix, "Not Enough Memory", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				informationSSQE.CleanUp();
				informationSSQE.Version = version;

				/* Get Base-Directories */
				LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out informationSSQE.NameDirectory, out informationSSQE.NameFileBody, out informationSSQE.NameFileExtension, nameFile);

				/* Decode Tags */
				System.Xml.NameTable nodeNameSpace = new System.Xml.NameTable();
				System.Xml.XmlNamespaceManager managerNameSpace = new System.Xml.XmlNamespaceManager(nodeNameSpace);

				//string valueText = "";
				informationSSQE.Name = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "name", managerNameSpace);
				informationSSQE.ExportPath = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "exportPath", managerNameSpace);

				System.Xml.XmlNodeList nodeList = null;
				nodeList = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "sequenceList/sequence", managerNameSpace);
				if(null == nodeList)
				{
					informationSSPJ.TableNameSSQE = new string[0];
				}
				else
				{
					/* MEMO: Nothing to do, now. */
				}

				List<Library_SpriteStudio6.Data.Sequence.Data> listSequence = new List<Library_SpriteStudio6.Data.Sequence.Data>();

				listSequence.Clear();
				foreach(System.Xml.XmlNode nodeSequence in nodeList)
				{
					Library_SpriteStudio6.Data.Sequence.Data sequence = new Library_SpriteStudio6.Data.Sequence.Data();
					sequence.Name = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeSequence, "name", managerNameSpace);
					sequence.Index = ValueTextToInt(LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeSequence, "index", managerNameSpace));
					sequence.Type = ValueTextToSequenceType(LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeSequence, "type", managerNameSpace));

					System.Xml.XmlNodeList nodeListValueSequence = null;
					nodeListValueSequence = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeSequence, "list/value", managerNameSpace);


					List<Library_SpriteStudio6.Data.Sequence.Data.Step> listDetail = new List<Library_SpriteStudio6.Data.Sequence.Data.Step>();
					listDetail.Clear();

					foreach(System.Xml.XmlNode nodeValue in nodeListValueSequence)
					{
						Library_SpriteStudio6.Data.Sequence.Data.Step dataStep = new Library_SpriteStudio6.Data.Sequence.Data.Step();
						dataStep.NamePackAnimation = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeValue, "refAnimePack", managerNameSpace);
						dataStep.NameAnimation = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeValue, "refAnime", managerNameSpace);
						dataStep.PlayCount = ValueTextToInt(LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeValue, "repeatCount", managerNameSpace));
						listDetail.Add(dataStep);
					}
					sequence.TableStep = listDetail.ToArray();
					listSequence.Add(sequence);
				}

				informationSSQE.SequenceList = listSequence.ToArray();

				return(informationSSQE);

			Parse_ErrorEnd:;
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
				CODE_010000 = 0x00010000,					/* after SS6.4 */
				CODE_020000 = 0x00020000,					/* after SS6.5 */

				TARGET_EARLIEST = CODE_010000,
				TARGET_LATEST = CODE_020000
			};

			private const string ExtentionFile = ".sssq";
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public class Information
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public LibraryEditor_SpriteStudio6.Import.SSQE.KindVersion Version;

				public string NameDirectory;
				public string NameFileBody;
				public string NameFileExtension;

				public string Name;
				public string ExportPath;
				public Library_SpriteStudio6.Data.Sequence.Data[] SequenceList;

				public LibraryEditor_SpriteStudio6.Import.Assets<Script_SpriteStudio6_DataSequence> DataSequenceSS6PU;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Version = LibraryEditor_SpriteStudio6.Import.SSQE.KindVersion.ERROR;

					NameDirectory = "";
					NameFileBody = "";
					NameFileExtension = "";

					Name = "";
					ExportPath = "";

					DataSequenceSS6PU.CleanUp();
					DataSequenceSS6PU.BootUp(1);	/* Always 1 */
				}

				public string FileNameGetFullPath()
				{
					return(NameDirectory + NameFileBody + NameFileExtension);
				}
				#endregion Functions

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				#endregion Classes, Structs & Interfaces
			}

			public static partial class ModeSS6PU
			{
				/* MEMO: Originally functions that should be defined in each information class. */
				/*       However, confusion tends to occur with mode increases.                 */
				/*       ... Compromised way.                                                   */

				/* ----------------------------------------------- Functions */
				#region Functions
				public static bool AssetNameDecideData(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
														LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
														LibraryEditor_SpriteStudio6.Import.SSQE.Information informationSSQE,
														string nameOutputAssetFolderBase,
														Script_SpriteStudio6_DataSequence dataOverride
													)
				{
					if(null != dataOverride)
					{	/* Specified */
						informationSSQE.DataSequenceSS6PU.TableName[0] = AssetDatabase.GetAssetPath(dataOverride);
					}
					else
					{	/* Default */
						informationSSQE.DataSequenceSS6PU.TableName[0] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_SEQUENCE_SS6PU, nameOutputAssetFolderBase)
																		+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_SEQUENCE_SS6PU, informationSSQE.NameFileBody, informationSSPJ.NameFileBody)
																		+ LibraryEditor_SpriteStudio6.Import.NameExtentionScriptableObject;
						dataOverride = AssetDatabase.LoadAssetAtPath<Script_SpriteStudio6_DataSequence>(informationSSQE.DataSequenceSS6PU.TableName[0]);
					}

					informationSSQE.DataSequenceSS6PU.TableData[0] = dataOverride;
					informationSSQE.DataSequenceSS6PU.Version[0] = (null != dataOverride) ? (int)(dataOverride.Version) : (int)Script_SpriteStudio6_DataEffect.KindVersion.SS5PU;

					return(true);

//				AssetNameDecide_ErroeEnd:;
//					return(false);
				}

				public static bool AssetCreateData(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
													LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
													LibraryEditor_SpriteStudio6.Import.SSQE.Information informationSSQE
												)
				{
//					const string messageLogPrefix = "Create Asset(Data-Sequence)";

					Script_SpriteStudio6_DataSequence dataSequence = informationSSQE.DataSequenceSS6PU.TableData[0];
					if(null == dataSequence)
					{
						dataSequence = ScriptableObject.CreateInstance<Script_SpriteStudio6_DataSequence>();
						AssetDatabase.CreateAsset(dataSequence, informationSSQE.DataSequenceSS6PU.TableName[0]);
						informationSSQE.DataSequenceSS6PU.TableData[0] = dataSequence;
					}

					dataSequence.Version = Script_SpriteStudio6_DataSequence.KindVersion.SUPPORT_LATEST;
					dataSequence.Name = string.Copy(informationSSQE.NameFileBody);
					dataSequence.DataProject = informationSSPJ.DataProjectSS6PU.TableData[0];

					if(null == informationSSQE.SequenceList)
					{
						dataSequence.TableSequence = new Library_SpriteStudio6.Data.Sequence.Data[0];
					}
					else
					{
						dataSequence.TableSequence = informationSSQE.SequenceList;
					}

					EditorUtility.SetDirty(dataSequence);
					AssetDatabase.SaveAssets();

					return(true);

//				AssetCreateData_ErrorEnd:;
//					return(false);
				}

				public static bool ConvertData(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
												LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
												LibraryEditor_SpriteStudio6.Import.SSQE.Information informationSSQE
											)
				{
//					const string messageLogPrefix = "Convert (Data-Sequence)";

					/* MEMO: Nothing to do, now.                                                                                          */
					/*       Conversion was finished at parsing, since Sequence datas is almost same as SSQE-datas. (in Function-"Parse") */

					return(true);

//				ConvertSS6PU_ErroeEnd:;
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
