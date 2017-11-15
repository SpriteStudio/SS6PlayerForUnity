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
		public static partial class SSPJ
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static Information Parse(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
												string nameDirectory,
												string nameFileBody,
												string nameFileExtention
											)
			{
				const string messageLogPrefix = "Parse SSPJ";
				string nameFile = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(nameDirectory + "/" + nameFileBody + nameFileExtention);
				Information informationSSPJ = null;

				/* Load ".sspj" */
				if(false == System.IO.File.Exists(nameFile))
				{
					LogError(messageLogPrefix, "File Not Found", nameFile);
					goto Parse_ErrorEnd;
				}
				System.Xml.XmlDocument xmlSSPJ = new System.Xml.XmlDocument();
				xmlSSPJ.Load(nameFile);

				/* Check Version */
				System.Xml.XmlNode nodeRoot = xmlSSPJ.FirstChild;
				nodeRoot = nodeRoot.NextSibling;
				KindVersion version = (KindVersion)(LibraryEditor_SpriteStudio6.Utility.XML.VersionGet(nodeRoot, "SpriteStudioProject", (int)KindVersion.ERROR, true));
				switch(version)
				{
					case KindVersion.ERROR:
						LogError(messageLogPrefix, "Version Invalid", nameFile);
						goto Parse_ErrorEnd;

					case KindVersion.CODE_000100:
					case KindVersion.CODE_010000:
					case KindVersion.CODE_010200:
					case KindVersion.CODE_010201:
						LogError(messageLogPrefix, "\"SpriteStudio5\"'s data can not be imported.Please re-save data in \"SpriteStudio6\" and then import.", nameFile);
						goto Parse_ErrorEnd;

					case KindVersion.CODE_020000:
						break;

					default:
						if(KindVersion.TARGET_EARLIEST > version)
						{
							version = KindVersion.TARGET_EARLIEST;
							if(true == setting.CheckVersion.FlagInvalidSSPJ)
							{
								LogWarning(messageLogPrefix, "Version Too Early", nameFile);
							}
						}
						else
						{
							version = KindVersion.TARGET_LATEST;
							if(true == setting.CheckVersion.FlagInvalidSSPJ)
							{
								LogWarning(messageLogPrefix, "Version Unknown", nameFile);
							}
						}
						break;
				}

				/* Create Information */
				informationSSPJ = new Information();
				if(null == informationSSPJ)
				{
					LogError(messageLogPrefix, "Not Enough Memory", nameFile);
					goto Parse_ErrorEnd;
				}
				informationSSPJ.CleanUp();
				informationSSPJ.Version = version;

				/* Get Base-Directories */
				LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out informationSSPJ.NameDirectory, out informationSSPJ.NameFileBody, out informationSSPJ.NameFileExtension, nameFile);

				/* Decode Tags */
				System.Xml.NameTable nodeNameSpace = new System.Xml.NameTable();
				System.Xml.XmlNamespaceManager managerNameSpace = new System.Xml.XmlNamespaceManager(nodeNameSpace);

				string valueText = "";

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/imageBaseDirectory", managerNameSpace);
				informationSSPJ.NameDirectoryBaseTexture = (true == string.IsNullOrEmpty(valueText)) ? string.Copy(informationSSPJ.NameDirectory) : LibraryEditor_SpriteStudio6.Utility.File.PathGetAbsolute(informationSSPJ.NameDirectory + valueText + "/", informationSSPJ.NameDirectory);

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/cellMapBaseDirectory", managerNameSpace);
				informationSSPJ.NameDirectoryBaseSSCE = (true == string.IsNullOrEmpty(valueText)) ? string.Copy(informationSSPJ.NameDirectory) : LibraryEditor_SpriteStudio6.Utility.File.PathGetAbsolute(informationSSPJ.NameDirectory + valueText + "/", informationSSPJ.NameDirectory);

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/animeBaseDirectory", managerNameSpace);
				informationSSPJ.NameDirectoryBaseSSAE = (true == string.IsNullOrEmpty(valueText)) ? string.Copy(informationSSPJ.NameDirectory) : LibraryEditor_SpriteStudio6.Utility.File.PathGetAbsolute(informationSSPJ.NameDirectory + valueText + "/", informationSSPJ.NameDirectory);

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/effectBaseDirectory", managerNameSpace);
				informationSSPJ.NameDirectoryBaseSSEE = (true == string.IsNullOrEmpty(valueText)) ? string.Copy(informationSSPJ.NameDirectory) : LibraryEditor_SpriteStudio6.Utility.File.PathGetAbsolute(informationSSPJ.NameDirectory + valueText + "/", informationSSPJ.NameDirectory);

				/* Get Texture-Mode-Setting */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/wrapMode", managerNameSpace);
				switch(valueText)
				{
					case "repeat":
						informationSSPJ.WrapTexture = Library_SpriteStudio6.Data.Texture.KindWrap.REPEAT;
						break;

					case "mirror":
						LibraryEditor_SpriteStudio6.Utility.Log.Warning(messageLogPrefix + "Texture Wrap-Mode \"Mirror\" is not Suppoted. Change to \"Clamp\". [" + nameFile + "]");
						goto case "clamp";

					case "clamp":
					default:
						informationSSPJ.WrapTexture = Library_SpriteStudio6.Data.Texture.KindWrap.CLAMP;
						break;
				}

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/filterMode", managerNameSpace);
				switch(valueText)
				{
					case "nearlest":
						informationSSPJ.FilterTexture = Library_SpriteStudio6.Data.Texture.KindFilter.NEAREST;
						break;

					case "linear":
						informationSSPJ.FilterTexture = Library_SpriteStudio6.Data.Texture.KindFilter.LINEAR;
						break;

					case "bilinear":
						informationSSPJ.FilterTexture = Library_SpriteStudio6.Data.Texture.KindFilter.BILINEAR;
						break;

					default:
						LibraryEditor_SpriteStudio6.Utility.Log.Warning(messageLogPrefix + "Texture Filter-Mode Unknown. Change to \"Linear\". [" + nameFile + "]");
						informationSSPJ.FilterTexture = Library_SpriteStudio6.Data.Texture.KindFilter.LINEAR;
						break;
				}

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/convertImageToPMA", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
				    informationSSPJ.flagConvertImagePremultipliedAlpha = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
				}

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/blendImageAsPMA", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
				    informationSSPJ.flagBlendImagePremultipliedAlpha = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
				}

				/* Get CellMap FileNames */
				System.Xml.XmlNodeList nodeList = null;
				List<string> listNameFile = new List<string>();
				listNameFile.Clear();

				nodeList = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "cellmapNames/value", managerNameSpace);
				if(null == nodeList)
				{
					informationSSPJ.TableNameSSCE = new string[0];
				}
				else
				{
					foreach(System.Xml.XmlNode nodeNameCellMap in nodeList)
					{
						nameFile = nodeNameCellMap.InnerText;
						nameFile = informationSSPJ.PathGetAbsolute(nameFile, LibraryEditor_SpriteStudio6.Import.KindFile.SSCE);
						listNameFile.Add(nameFile);
					}
					informationSSPJ.TableNameSSCE = listNameFile.ToArray();
				}
				informationSSPJ.TableInformationSSCE = new LibraryEditor_SpriteStudio6.Import.SSCE.Information[informationSSPJ.TableNameSSCE.Length];
				listNameFile.Clear();

				/* Get Animation FileNames */
				nodeList = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "animepackNames/value", managerNameSpace);
				if(null == nodeList)
				{
					informationSSPJ.TableNameSSAE = new string[0];
				}
				else
				{
					foreach(System.Xml.XmlNode nodeNameAnimation in nodeList)
					{
						nameFile = nodeNameAnimation.InnerText;
						nameFile = informationSSPJ.PathGetAbsolute(nameFile, LibraryEditor_SpriteStudio6.Import.KindFile.SSAE);
						listNameFile.Add(nameFile);
					}
					informationSSPJ.TableNameSSAE = listNameFile.ToArray();
				}
				informationSSPJ.TableInformationSSAE = new LibraryEditor_SpriteStudio6.Import.SSAE.Information[informationSSPJ.TableNameSSAE.Length];
				listNameFile.Clear();

				/* Get Effect FileNames */
				nodeList = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "effectFileNames/value", managerNameSpace);
				if(null == nodeList)
				{
					informationSSPJ.TableNameSSEE = new string[0];
				}
				else
				{
					foreach(System.Xml.XmlNode NodeEffect in nodeList)
					{
						nameFile = NodeEffect.InnerText;
						nameFile = informationSSPJ.PathGetAbsolute(nameFile, LibraryEditor_SpriteStudio6.Import.KindFile.SSEE);
						listNameFile.Add(nameFile);
					}
					informationSSPJ.TableNameSSEE = listNameFile.ToArray();
				}
				informationSSPJ.TableInformationSSEE = new LibraryEditor_SpriteStudio6.Import.SSEE.Information[informationSSPJ.TableNameSSEE.Length];
				listNameFile.Clear();

				return(informationSSPJ);

			Parse_ErrorEnd:
				if(null != informationSSPJ)
				{
					informationSSPJ.CleanUp();
				}
				return(null);
			}

			private static void LogError(string messagePrefix, string message, string nameFile)
			{
				LibraryEditor_SpriteStudio6.Utility.Log.Error(	messagePrefix
																+ ": " + message
															+ " [" + nameFile + "]"
															);
			}

			private static void LogWarning(string messagePrefix, string message, string nameFile)
			{
				LibraryEditor_SpriteStudio6.Utility.Log.Warning(	messagePrefix
																	+ ": " + message
																	+ " [" + nameFile + "]"
																);
			}
			#endregion Functions

			/* ----------------------------------------------- Enums & Constants */
			#region Enums & Constants
			public enum KindVersion
			{
				ERROR = 0x00000000,
				CODE_000100 = 0x00000100,	/* under-development SS5 */
				CODE_010000 = 0x00010000,
				CODE_010200 = 0x00010200,	/* after SS5.5.0 beta-3 */
				CODE_010201 = 0x00010201,	/* after SS5.7.0 beta */
				CODE_020000 = 0x00020000,	/* after SS6.0.0 */

				TARGET_EARLIEST = CODE_020000,
				TARGET_LATEST = CODE_020000
			}

			private const string ExtentionFile = ".sspj";
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public class Information
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				/* Project Setting: SSPJ */
				public LibraryEditor_SpriteStudio6.Import.SSPJ.KindVersion Version;

				public string NameDirectory;
				public string NameFileBody;
				public string NameFileExtension;

				public Library_SpriteStudio6.Data.Texture.KindWrap WrapTexture;
				public Library_SpriteStudio6.Data.Texture.KindFilter FilterTexture;
				public bool flagConvertImagePremultipliedAlpha;
				public bool flagBlendImagePremultipliedAlpha;

				public string NameDirectoryBaseTexture;
				public LibraryEditor_SpriteStudio6.Import.SSCE.Information.Texture[] TableInformationTexture;
				public List<string> ListNameTexture;	/* Temporary ... Accumulating list for determine TableNameTexture. Valid during analyzing SSxx-s. */

				public string NameDirectoryBaseSSCE;
				public LibraryEditor_SpriteStudio6.Import.SSCE.Information[] TableInformationSSCE;
				public string[] TableNameSSCE;	/* Temporary */

				public string NameDirectoryBaseSSAE;
				public LibraryEditor_SpriteStudio6.Import.SSAE.Information[] TableInformationSSAE;
				public string[] TableNameSSAE;	/* Temporary */
				public int[] QueueConvertSSAE;

				public string NameDirectoryBaseSSEE;
				public LibraryEditor_SpriteStudio6.Import.SSEE.Information[] TableInformationSSEE;
				public string[] TableNameSSEE;	/* Temporary */

				public Material[] TableMaterialAnimationSS6PU;
				public Material[] TableMaterialEffectSS6PU;
				public LibraryEditor_SpriteStudio6.Import.Assets<Script_SpriteStudio6_DataCellMap> DataCellMapSS6PU;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Version = LibraryEditor_SpriteStudio6.Import.SSPJ.KindVersion.ERROR;

					NameDirectory = "";
					NameFileBody = "";
					NameFileExtension = "";

					WrapTexture = Library_SpriteStudio6.Data.Texture.KindWrap.CLAMP;
					FilterTexture = Library_SpriteStudio6.Data.Texture.KindFilter.NEAREST;
					flagConvertImagePremultipliedAlpha = false;
					flagBlendImagePremultipliedAlpha = false;

					NameDirectoryBaseTexture = "";
					ListNameTexture = null;
					TableInformationTexture = null;

					NameDirectoryBaseSSCE = "";
					TableNameSSCE = null;
					TableInformationSSCE = null;

					NameDirectoryBaseSSAE = "";
					TableNameSSAE = null;
					TableInformationSSAE = null;
					QueueConvertSSAE = null;
;
					NameDirectoryBaseSSEE = "";
					TableNameSSEE = null;
					TableInformationSSEE = null;

					TableMaterialAnimationSS6PU = null;
					TableMaterialEffectSS6PU = null;
					DataCellMapSS6PU.CleanUp();
//					DataCellMapSS6PU.BootUp(1);	/* Don't boot-up here. */
				}

				public bool InformationCreateTexture(ref LibraryEditor_SpriteStudio6.Import.Setting setting)
				{
					const string messageLogPrefix = "Fix Information(Texture)";

					/* Create Texture-Information Table */
					int countTexture = ListNameTexture.Count;
					int countSSCE = TableInformationSSCE.Length;
					TableInformationTexture = new LibraryEditor_SpriteStudio6.Import.SSCE.Information.Texture[countTexture];
					if(null == TableInformationTexture)
					{
						LogError(messageLogPrefix, "Not Enough Memory (Information Table)", FileNameGetFullPath());
						goto InformationCreateTexture_ErrorEnd;
					}
					for(int i=0; i<countTexture; i++)
					{
						TableInformationTexture[i] = null;
					}

					/* Create Texture-Informations */
					int indexTexture;
					string namePathTexture = null;
					LibraryEditor_SpriteStudio6.Import.SSCE.Information informationSSCE = null;
					for(int i=0; i<countSSCE; i++)
					{
						informationSSCE = TableInformationSSCE[i];
						indexTexture = informationSSCE.IndexTexture;
						namePathTexture = ListNameTexture[indexTexture];
						if(null == TableInformationTexture[indexTexture])
						{	/* Not Created */
							LibraryEditor_SpriteStudio6.Import.SSCE.Information.Texture informationTexture = new LibraryEditor_SpriteStudio6.Import.SSCE.Information.Texture();
							TableInformationTexture[indexTexture] = informationTexture;

							informationTexture.CleanUp();
							informationTexture.Name = PathGetRelative(namePathTexture, LibraryEditor_SpriteStudio6.Import.KindFile.TEXTURE);
							informationTexture.Name = LibraryEditor_SpriteStudio6.Utility.Text.DataNameGetFromPath(informationTexture.Name, true);
							informationTexture.Wrap = informationSSCE.WrapTexture;
							informationTexture.Filter = informationSSCE.FilterTexture;
							informationTexture.SizeX = informationSSCE.SizePixelX;
							informationTexture.SizeY = informationSSCE.SizePixelY;
							LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out informationTexture.NameDirectory, out informationTexture.NameFileBody, out informationTexture.NameFileExtension, namePathTexture);
						}
					}
					return(true);

				InformationCreateTexture_ErrorEnd:;
					return(false);
				}

				public string FileNameGetFullPath()
				{
					return(NameDirectory + NameFileBody + NameFileExtension);
				}

				public string PathGetAbsolute(string namePath, LibraryEditor_SpriteStudio6.Import.KindFile kindFile)
				{
					string namePathNew = "";
					if(true == System.IO.Path.IsPathRooted(namePath))
					{	/* MEMO: "namePath" is "Absolute". */
						namePathNew = namePath;
					}
					else
					{	/* MEMO: "namePath" is "Relative". */
						switch(kindFile)
						{
							case LibraryEditor_SpriteStudio6.Import.KindFile.NON:
								namePathNew = namePath;
								break;

							case LibraryEditor_SpriteStudio6.Import.KindFile.TEXTURE:
								namePathNew = NameDirectoryBaseTexture + namePath;
								break;

							case LibraryEditor_SpriteStudio6.Import.KindFile.SSPJ:
								namePathNew = NameDirectory + namePath;
								break;

							case LibraryEditor_SpriteStudio6.Import.KindFile.SSCE:
								namePathNew = NameDirectoryBaseSSCE + namePath;
								break;

							case LibraryEditor_SpriteStudio6.Import.KindFile.SSAE:
								namePathNew = NameDirectoryBaseSSAE + namePath;
								break;

							case LibraryEditor_SpriteStudio6.Import.KindFile.SSEE:
								namePathNew = NameDirectoryBaseSSEE + namePath;
								break;
						}
					}

					namePathNew = System.IO.Path.GetFullPath(namePathNew);
					namePathNew = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(namePathNew);

					return(namePathNew);
				}

				public string PathGetRelative(string namePath, LibraryEditor_SpriteStudio6.Import.KindFile kindFile)
				{
					string nameBase = "";
					if(false == System.IO.Path.IsPathRooted(namePath))
					{	/* MEMO: "namePath" is "Relative". */
						nameBase = namePath;
						nameBase = nameBase.Replace("\\", "/");	/* "\" -> "/" */
						return(nameBase);
					}
					else
					{	/* MEMO: "namePath" is "Absolute". */
						switch(kindFile)
						{
							case LibraryEditor_SpriteStudio6.Import.KindFile.NON:
								return(namePath);

							case LibraryEditor_SpriteStudio6.Import.KindFile.TEXTURE:
								nameBase = NameDirectoryBaseTexture;
								break;

							case LibraryEditor_SpriteStudio6.Import.KindFile.SSPJ:
								nameBase = NameDirectory;
								break;

							case LibraryEditor_SpriteStudio6.Import.KindFile.SSCE:
								nameBase = NameDirectoryBaseSSCE;
								break;

							case LibraryEditor_SpriteStudio6.Import.KindFile.SSAE:
								nameBase = NameDirectoryBaseSSAE;
								break;

							case LibraryEditor_SpriteStudio6.Import.KindFile.SSEE:
								nameBase = NameDirectoryBaseSSEE;
								break;
						}

						nameBase = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(nameBase);
						string namePathNew = LibraryEditor_SpriteStudio6.Utility.File.PathNormalize(namePath);
						namePathNew = namePathNew.Replace(nameBase, "");
						return(namePathNew);
					}
				}

				public int AddTexture(string nameTexture)
				{
					if(null == ListNameTexture)
					{
						ListNameTexture = new List<string>();
						ListNameTexture.Clear();
					}

					int index = IndexGetFileName(ListNameTexture, nameTexture);
					if(0 > index)
					{	/* New SSAE */
						string nameFileNew = string.Copy(nameTexture);
						ListNameTexture.Add(nameFileNew);
						index = ListNameTexture.Count - 1;
					}
					return(index);
				}

				public int IndexGetFileName(List<string> listNameFile, string nameFile)
				{
					if(null != listNameFile)
					{
						for(int i=0; i<listNameFile.Count; i++)
						{
							string nameFileNow = listNameFile[i] as string;
							if(nameFile == nameFileNow)
							{
								return(i);
							}
						}
					}
					return(-1);
				}

				public int IndexGetFileName(string[] tableNameFile, string nameFile)
				{
					if(null != tableNameFile)
					{
						for(int i=0; i<tableNameFile.Length; i++)
						{
							if(nameFile == tableNameFile[i])
							{
								return(i);
							}
						}
					}
					return(-1);
				}

				public int IndexGetCellMap(string name)
				{
					int count = TableInformationSSCE.Length;
					for(int i=0; i<count; i++)
					{
						if(null != TableInformationSSCE[i].TableCell)
						{
							if(name == TableInformationSSCE[i].NameFileBody)
							{
								return(i);
							}
						}
						else
						{
							if(name == TableInformationSSCE[i].Data.Name)
							{
								return(i);
							}
						}
					}
					return(-1);
				}

				public int IndexGetAnimation(string name)
				{
					int count = TableInformationSSAE.Length;
					for(int i=0; i<count; i++)
					{
						if(null != TableInformationSSAE[i])
						{
							if(name == TableInformationSSAE[i].NameFileBody)
							{
								return(i);
							}
						}
					}
					return(-1);
				}

				public int IndexGetEffect(string name)
				{
					int count = TableInformationSSEE.Length;
					for(int i=0; i<count; i++)
					{
						if(null != TableInformationSSEE[i])
						{
							if(name == TableInformationSSEE[i].NameFileBody)
							{
								return(i);
							}
						}
					}
					return(-1);
				}

				public int[] QueueGetConvertSSAE(ref LibraryEditor_SpriteStudio6.Import.Setting setting)
				{
					const string messageLogPrefix = "Fix SSAEs Conversion-Queue";

					int countSSAE = TableInformationSSAE.Length;
					int index = 0;

					int[] tableIndexSSAE = new int[countSSAE];
					if(null == tableIndexSSAE)
					{
						LogError(messageLogPrefix, "Not Enough Memory (Order-Table)", FileNameGetFullPath());
						goto QueueGetConvertSSAE_ErrorEnd;
					}
					for(int i=0; i<countSSAE; i++)
					{
						tableIndexSSAE[i] = -1;
					}

					/* Set having no "Instance"parts */
					for(int i=0; i<countSSAE; i++)
					{
						if(0 >= CountGetInstancePartsSSAE(ref setting, TableInformationSSAE[i]))
						{
							tableIndexSSAE[index] = i;
							index++;
						}
					}

					/* Set having "Instance"parts */
					bool flagAlreadyQueued = false;
					bool flagAllInstanceExist = false;
					while(countSSAE > index)
					{
						for(int i=0; i<countSSAE; i++)
						{
							/* Check already queued */
							flagAlreadyQueued = false;
							for(int j=0; j<index; j++)
							{
								if(i == tableIndexSSAE[j])
								{	/* Already Set */
									flagAlreadyQueued = true;
									break;
								}
							}
							if(true == flagAlreadyQueued)
							{
								continue;
							}

							/* Check all calling "Instance"s are queued */
							flagAllInstanceExist = false;
							if(false == QueueCheckAllInstance(	ref flagAllInstanceExist,
																ref setting,
																TableInformationSSAE[i],
																tableIndexSSAE,
																index
															)
								)
							{	/* Error (Not Found SSAE-Name) */
								goto QueueGetConvertSSAE_ErrorEnd;
							}
							if(true == flagAllInstanceExist)
							{	/* All Queued */
								tableIndexSSAE[index] = i;
								index++;
								break;	/* Break for-Loop */
							}
						}
					}

					return(tableIndexSSAE);

				QueueGetConvertSSAE_ErrorEnd:;
					return(null);
				}
				private int CountGetInstancePartsSSAE(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
														LibraryEditor_SpriteStudio6.Import.SSAE.Information informationSSAE
													)
				{
//					const string messageLogPrefix = "Fix SSAEs Conversion-Queue";
					int countParts = informationSSAE.TableParts.Length;
					int count = 0;
					for(int i=0; i<countParts; i++)
					{
						if(Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE == informationSSAE.TableParts[i].Data.Feature)
						{
							count++;
						}
					}
					return(count);
				}
				private bool QueueCheckAllInstance(	ref bool flagAllInstanceExist,
													ref LibraryEditor_SpriteStudio6.Import.Setting setting,
													LibraryEditor_SpriteStudio6.Import.SSAE.Information informationSSAE,
													int[] tableIndexSSAE,
													int countQueued
												)
				{
					const string messageLogPrefix = "Fix SSAEs Conversion-Queue";

					int countParts = informationSSAE.TableParts.Length;
					LibraryEditor_SpriteStudio6.Import.SSAE.Information.Parts parts = null;
					int indexInstanceSSAE;
					bool flagExist;
					for(int i=0; i<countParts; i++)
					{
						parts = informationSSAE.TableParts[i];
						if(Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE == parts.Data.Feature)
						{
							indexInstanceSSAE = IndexGetAnimation(parts.NameUnderControl);
							if(-1 == indexInstanceSSAE)
							{
								LogError(messageLogPrefix, "Instance missing Parts [" + parts.Data.Name + "] in [" + informationSSAE.FileNameGetFullPath() + "]", FileNameGetFullPath());
								goto QueueCheckAllInstance_ErrorEnd;
							}
							flagExist = false;
							for(int j=0; j<countQueued; j++)
							{
								if(tableIndexSSAE[j] == indexInstanceSSAE)
								{
									flagExist = true;
									break;
								}
							}
							if(false == flagExist)
							{
								flagAllInstanceExist = false;
								return(true);
							}
						}
					}

					flagAllInstanceExist = true;
					return(true);

				QueueCheckAllInstance_ErrorEnd:;
					flagAllInstanceExist = false;
					return(false);
				}
				#endregion Functions
			}

			public static partial class ModeSS6PU
			{
				/* ----------------------------------------------- Functions */
				#region Functions
				public static bool AssetNameDecide(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
													LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
													string nameOutputAssetFolderBase
												)
				{
					int countSSAE = informationSSPJ.TableInformationSSAE.Length;
					int countSSEE = informationSSPJ.TableInformationSSEE.Length;
					int countSSCE = informationSSPJ.TableInformationSSCE.Length;

					/* MEMO: Prefab created from "ssae" and "ssee" can only be tracked by name. */
					/* SSAEs (Prefab) */
					for(int i=0; i<countSSAE; i++)
					{
						LibraryEditor_SpriteStudio6.Import.SSAE.ModeSS6PU.AssetNameDecidePrefab(	ref setting,
																									informationSSPJ,
																									informationSSPJ.TableInformationSSAE[i],
																									nameOutputAssetFolderBase,
																									null
																								);
					}

					/* SSEEs (Prefab) */
					for(int i=0; i<countSSEE; i++)
					{
						LibraryEditor_SpriteStudio6.Import.SSEE.ModeSS6PU.AssetNameDecidePrefab(	ref setting,
																									informationSSPJ,
																									informationSSPJ.TableInformationSSEE[i],
																									nameOutputAssetFolderBase,
																									null
																							);
					}

					/* Track existing assets */
					Script_SpriteStudio6_DataCellMap dataCellMapOld = null;	/* Pair with SSPJ (Always 1) */
					Material[] tableMaterialAnimationOld = null;
					Material[,,] tableMaterialAnimationtNew = new Material[countSSCE, (int)Library_SpriteStudio6.KindMasking.TERMINATOR, (int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR_TABLEMATERIAL];
					Material[] tableMaterialEffectOld = null;
					Material[,,] tableMaterialEffectNew = new Material[countSSCE, (int)Library_SpriteStudio6.KindMasking.TERMINATOR, (int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_TABLEMATERIAL];
					Texture2D[] tableTextureNew = new Texture2D[countSSCE];
					Script_SpriteStudio6_Root[] tableScriptRootOld = new Script_SpriteStudio6_Root[countSSAE];
					Script_SpriteStudio6_DataAnimation[] tableDataAnimationOld = new Script_SpriteStudio6_DataAnimation[countSSAE];
					Script_SpriteStudio6_RootEffect[] tableScriptRootEffectOld = new Script_SpriteStudio6_RootEffect[countSSEE];
					Script_SpriteStudio6_DataEffect[] tableDataEffectOld = new Script_SpriteStudio6_DataEffect[countSSEE];
					if(	(null == tableMaterialAnimationtNew) || (null == tableMaterialEffectNew) || (null == tableTextureNew)
						|| (null == tableScriptRootOld) || (null == tableDataAnimationOld)
						|| (null == tableScriptRootEffectOld) || (null == tableDataEffectOld)
						)
					{
						goto AssetNameDecide_ErrorEnd;
					}
					for(int i=0; i<countSSCE; i++)
					{
						for(int j=0; j<(int)Library_SpriteStudio6.KindMasking.TERMINATOR; j++)
						{
							for(int k=0; k<(int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR_TABLEMATERIAL; k++)
							{
								tableMaterialAnimationtNew[i, j, k] = null;
							}
							for(int k=0; k<(int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_TABLEMATERIAL; k++)
							{
								tableMaterialEffectNew[i, j, k] = null;
							}
						}
					}

					if(true == setting.Basic.FlagTrackAssets)
					{
						GameObject gameObjectPrefab;

						/* Get DataAnimation */
						/* MEMO: To get DataAnimation, Get reference directly from Animation-Prefab. */
						for(int i=0; i<countSSAE; i++)
						{
							tableScriptRootOld[i] = null;
							tableDataAnimationOld[i] = null;
							if(null != informationSSPJ.TableInformationSSAE[i].PrefabAnimationSS6PU.TableData[0])
							{
								gameObjectPrefab = (GameObject)informationSSPJ.TableInformationSSAE[i].PrefabAnimationSS6PU.TableData[0];
								if(null != gameObjectPrefab)
								{
									tableScriptRootOld[i] = gameObjectPrefab.GetComponent<Script_SpriteStudio6_Root>();
									if(null != tableScriptRootOld[i])
									{	/* Valid Animation-prefab */
										tableDataAnimationOld[i] = tableScriptRootOld[i].DataAnimation;
										if((null != tableDataAnimationOld[i]) && (null == tableMaterialAnimationOld))
										{
											/* MEMO: Do not get Root's TableMaterial. (Not imported original data) */
											tableMaterialAnimationOld = tableDataAnimationOld[i].TableMaterial;
										}

										if(null == dataCellMapOld)
										{
											dataCellMapOld = tableScriptRootOld[i].DataCellMap;
										}
									}
								}
							}
						}

						/* Get DataEffect */
						/* MEMO: To get DataEffect, Get reference directly from Effect-Prefab. */
						for(int i=0; i<countSSEE; i++)
						{
							tableScriptRootEffectOld[i] = null;
							tableDataEffectOld[i] = null;
							if(null != informationSSPJ.TableInformationSSEE[i].PrefabEffectSS6PU.TableData[0])
							{
								gameObjectPrefab = (GameObject)informationSSPJ.TableInformationSSEE[i].PrefabEffectSS6PU.TableData[0];
								if(null != gameObjectPrefab)
								{
									tableScriptRootEffectOld[i] = gameObjectPrefab.GetComponent<Script_SpriteStudio6_RootEffect>();
									if(null != tableScriptRootEffectOld[i])
									{	/* Valid Effect-prefab */
										tableDataEffectOld[i] = tableScriptRootEffectOld[i].DataEffect;
										if((null != tableDataEffectOld[i]) && (null == tableMaterialEffectOld))
										{
											/* MEMO: Do not get RootEffect's TableMaterial. (Not imported original data) */
											tableMaterialEffectOld = tableDataEffectOld[i].TableMaterial;
										}

										if(null == dataCellMapOld)
										{
											dataCellMapOld = tableScriptRootEffectOld[i].DataCellMap;
										}
									}
								}
							}
						}

						/* Get new SSCE's index */
						/* MEMO: SSCEs are collated DataCellMap's CellMap-names. */
						/* MEMO: Materials are set based on indexes of collated CellMaps. */
						/* MEMO: Textures are gotten from materials. */
						for(int i=0; i<countSSCE; i++)
						{
							tableTextureNew[i] = null;
						}
						if(null != dataCellMapOld)
						{
							int countCellMapOld = dataCellMapOld.CountGetCellMap();
							string nameCellMapOld;
							int indexCellMapNew;
							int indexMaterialOld;
							Material material;

							for(int i=0; i<countCellMapOld; i++)
							{
								nameCellMapOld = dataCellMapOld.TableCellMap[i].Name;
								if(false == string.IsNullOrEmpty(nameCellMapOld))
								{
									indexCellMapNew = informationSSPJ.IndexGetCellMap(nameCellMapOld);
									if(0 <= indexCellMapNew)
									{
										if(null != tableMaterialAnimationOld)
										{
											for(int j=0; j<(int)Library_SpriteStudio6.KindMasking.TERMINATOR; j++)
											{
												for(int k=0; k<(int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR_TABLEMATERIAL; k++)
												{
													indexMaterialOld = Script_SpriteStudio6_Root.Material.IndexGetTable(	i,
																															(Library_SpriteStudio6.KindOperationBlend)(k + (int)Library_SpriteStudio6.KindOperationBlend.INITIATOR),
																															(Library_SpriteStudio6.KindMasking)j
																													);
													if(0 <= indexMaterialOld)
													{
														material = tableMaterialAnimationOld[indexMaterialOld];
														tableMaterialAnimationtNew[indexCellMapNew, j, k] = material;
														if((null != material) && (null == tableTextureNew[indexCellMapNew]))
														{
															tableTextureNew[indexCellMapNew] = material.mainTexture as Texture2D;
														}
													}
												}
											}
										}

										if(null != tableMaterialEffectOld)
										{
											for(int j=0; j<(int)Library_SpriteStudio6.KindMasking.TERMINATOR; j++)
											{
												for(int k=0; k<(int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_TABLEMATERIAL; k++)
												{
													indexMaterialOld = Script_SpriteStudio6_RootEffect.Material.IndexGetTable(	i,
																																(Library_SpriteStudio6.KindOperationBlendEffect)(k + (int)Library_SpriteStudio6.KindOperationBlendEffect.INITIATOR),
																																(Library_SpriteStudio6.KindMasking)j
																															);
													if(0 <= indexMaterialOld)
													{
														material = tableMaterialEffectOld[indexMaterialOld];
														tableMaterialEffectNew[indexCellMapNew, j, k] = material;
														if((null != material) && (null == tableTextureNew[indexCellMapNew]))
														{
															tableTextureNew[indexCellMapNew] = material.mainTexture as Texture2D;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}

					/* SSAEs (Data) */
					for(int i=0; i<countSSAE; i++)
					{
						LibraryEditor_SpriteStudio6.Import.SSAE.ModeSS6PU.AssetNameDecideData(	ref setting,
																								informationSSPJ,
																								informationSSPJ.TableInformationSSAE[i],
																								nameOutputAssetFolderBase,
																								tableDataAnimationOld[i]
																							);
					}

					/* SSEEs (Data) */
					for(int i=0; i<countSSEE; i++)
					{
						LibraryEditor_SpriteStudio6.Import.SSEE.ModeSS6PU.AssetNameDecideData(	ref setting,
																								informationSSPJ,
																								informationSSPJ.TableInformationSSEE[i],
																								nameOutputAssetFolderBase,
																								tableDataEffectOld[i]
																							);
					}

					/* SSCEs */
					if(0 < countSSCE)
					{
						informationSSPJ.DataCellMapSS6PU.BootUp(1);	/* Always 1 */
						AssetNameDecideCellMap(ref setting, informationSSPJ, nameOutputAssetFolderBase, dataCellMapOld);
					}
					else
					{
						informationSSPJ.DataCellMapSS6PU.CleanUp();
					}

					/* Materials & Textures */
					int countTexture = informationSSPJ.TableInformationTexture.Length;
					int indexMaterialBlend;
					for(int i=0; i<countTexture; i++)
					{
						/* Materials (Animation) */
						for(int j=(int)Library_SpriteStudio6.KindOperationBlend.INITIATOR; j<(int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR; j++)
						{
							indexMaterialBlend =  j - (int)Library_SpriteStudio6.KindOperationBlend.INITIATOR;	/* - (-x) = +(x) */

							LibraryEditor_SpriteStudio6.Import.SSCE.ModeSS6PU.AssetNameDecideMaterialAnimation(	ref setting,
																												informationSSPJ,
																												informationSSPJ.TableInformationTexture[i],
																												nameOutputAssetFolderBase,
																												(Library_SpriteStudio6.KindOperationBlend)j,
																												Library_SpriteStudio6.KindMasking.THROUGH,
																												tableMaterialAnimationtNew[i, (int)Library_SpriteStudio6.KindMasking.THROUGH, indexMaterialBlend]
																											);

							LibraryEditor_SpriteStudio6.Import.SSCE.ModeSS6PU.AssetNameDecideMaterialAnimation(	ref setting,
																												informationSSPJ,
																												informationSSPJ.TableInformationTexture[i],
																												nameOutputAssetFolderBase,
																												(Library_SpriteStudio6.KindOperationBlend)j,
																												Library_SpriteStudio6.KindMasking.MASK,
																												tableMaterialAnimationtNew[i, (int)Library_SpriteStudio6.KindMasking.MASK, indexMaterialBlend]
																											);
						}

						/* Materials (Effect) */
						for(int j=(int)Library_SpriteStudio6.KindOperationBlendEffect.INITIATOR; j<(int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR; j++)
						{
							indexMaterialBlend =  j - (int)Library_SpriteStudio6.KindOperationBlendEffect.INITIATOR;	/* - (-x) = +(x) */

							LibraryEditor_SpriteStudio6.Import.SSCE.ModeSS6PU.AssetNameDecideMaterialEffect(	ref setting,
																												informationSSPJ,
																												informationSSPJ.TableInformationTexture[i],
																												nameOutputAssetFolderBase,
																												(Library_SpriteStudio6.KindOperationBlendEffect)j,
																												Library_SpriteStudio6.KindMasking.THROUGH,
																												tableMaterialEffectNew[i, (int)Library_SpriteStudio6.KindMasking.THROUGH, indexMaterialBlend]
																											);

							LibraryEditor_SpriteStudio6.Import.SSCE.ModeSS6PU.AssetNameDecideMaterialEffect(	ref setting,
																												informationSSPJ,
																												informationSSPJ.TableInformationTexture[i],
																												nameOutputAssetFolderBase,
																												(Library_SpriteStudio6.KindOperationBlendEffect)j,
																												Library_SpriteStudio6.KindMasking.MASK,
																												tableMaterialEffectNew[i, (int)Library_SpriteStudio6.KindMasking.MASK, indexMaterialBlend]
																											);
						}

						/* Texture */
						LibraryEditor_SpriteStudio6.Import.SSCE.ModeSS6PU.AssetNameDecideTexture(	ref setting,
																									informationSSPJ,
																									informationSSPJ.TableInformationTexture[i],
																									nameOutputAssetFolderBase,
																									tableTextureNew[i]
																								);
					}

					return(true);

				AssetNameDecide_ErrorEnd:;
					dataCellMapOld = null;
					tableMaterialAnimationOld = null;
					tableMaterialAnimationtNew = null;
					tableMaterialEffectOld = null;
					tableMaterialEffectNew = null;
					tableTextureNew = null;
					tableScriptRootOld = null;
					tableDataAnimationOld = null;
					tableScriptRootEffectOld = null;
					tableDataEffectOld = null;
					return(false);
				}
				private static bool AssetNameDecideCellMap(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
															LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
															string nameOutputAssetFolderBase,
															Script_SpriteStudio6_DataCellMap cellMapOverride
														)
				{
					if(null != cellMapOverride)
					{	/* Specified */
						informationSSPJ.DataCellMapSS6PU.TableName[0] = AssetDatabase.GetAssetPath(cellMapOverride);
						informationSSPJ.DataCellMapSS6PU.TableData[0] = cellMapOverride;
					}
					else
					{	/* Default */
						informationSSPJ.DataCellMapSS6PU.TableName[0] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_CELLMAP_SS6PU, nameOutputAssetFolderBase)
																		+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_CELLMAP_SS6PU, informationSSPJ.NameFileBody, informationSSPJ.NameFileBody)
																		+ LibraryEditor_SpriteStudio6.Import.NameExtentionScriptableObject;
						informationSSPJ.DataCellMapSS6PU.TableData[0] = AssetDatabase.LoadAssetAtPath<Script_SpriteStudio6_DataCellMap>(informationSSPJ.DataCellMapSS6PU.TableName[0]);
					}

					return(true);

//				AssetNameDecideCellMap_ErrorEnd:;
//					return(false);
				}

				public static bool AssetCreateCellMap(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
														LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
													)
				{
					Script_SpriteStudio6_DataCellMap dataCellMap = informationSSPJ.DataCellMapSS6PU.TableData[0];
					if(null == dataCellMap)
					{
						dataCellMap = ScriptableObject.CreateInstance<Script_SpriteStudio6_DataCellMap>();
						AssetDatabase.CreateAsset(dataCellMap, informationSSPJ.DataCellMapSS6PU.TableName[0]);
						informationSSPJ.DataCellMapSS6PU.TableData[0] = dataCellMap;
					}

					int countSSCE = informationSSPJ.TableInformationSSCE.Length;
					dataCellMap.Version = Script_SpriteStudio6_DataCellMap.KindVersion.SUPPORT_LATEST;
					dataCellMap.TableCellMap = new Library_SpriteStudio6.Data.CellMap[countSSCE];
					for(int i=0; i<countSSCE; i++)
					{
						dataCellMap.TableCellMap[i] = informationSSPJ.TableInformationSSCE[i].Data;
					}

					EditorUtility.SetDirty(dataCellMap);
					AssetDatabase.SaveAssets();

					return(true);

//				AssetCreateCellMap_ErrorEnd:;
//					return(false);
				}

				public static bool MaterialPickUp(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
														LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
													)
				{
					const string messageLogPrefix = "Pick up Materials";

					int indexTexture;
					int indexMaterial;
					int indexMaterialTexture;
					int countSSCE = informationSSPJ.TableInformationSSCE.Length;
					int countMaterial = Script_SpriteStudio6_Root.Material.CountGetTable(countSSCE);
					informationSSPJ.TableMaterialAnimationSS6PU = new Material[countMaterial];
					if(null == informationSSPJ.TableMaterialAnimationSS6PU)
					{
						LogError(messageLogPrefix, "Not Enough Memory (MaterialTable Animation)", informationSSPJ.FileNameGetFullPath());
						goto MaterialPickUp_ErrorEnd;
					}
					for(int i=0; i<countSSCE; i++)
					{
						for(int j=(int)Library_SpriteStudio6.KindOperationBlend.INITIATOR; j<(int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR; j++)
						{
							indexTexture = informationSSPJ.TableInformationSSCE[i].IndexTexture;
							indexMaterial =  Script_SpriteStudio6_Root.Material.IndexGetTable(i, (Library_SpriteStudio6.KindOperationBlend)j, Library_SpriteStudio6.KindMasking.THROUGH);
							indexMaterialTexture = Script_SpriteStudio6_Root.Material.IndexGetTable(0, (Library_SpriteStudio6.KindOperationBlend)j, Library_SpriteStudio6.KindMasking.THROUGH);
							informationSSPJ.TableMaterialAnimationSS6PU[indexMaterial] = (0 > indexTexture)
																							? null
																							: informationSSPJ.TableInformationTexture[indexTexture].MaterialAnimationSS6PU.TableData[indexMaterialTexture];

						}

						for(int j=(int)Library_SpriteStudio6.KindOperationBlend.INITIATOR; j<(int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR; j++)
						{
							indexTexture = informationSSPJ.TableInformationSSCE[i].IndexTexture;
							indexMaterial =  Script_SpriteStudio6_Root.Material.IndexGetTable(i, (Library_SpriteStudio6.KindOperationBlend)j, Library_SpriteStudio6.KindMasking.MASK);
							indexMaterialTexture = Script_SpriteStudio6_Root.Material.IndexGetTable(0, (Library_SpriteStudio6.KindOperationBlend)j, Library_SpriteStudio6.KindMasking.MASK);
							informationSSPJ.TableMaterialAnimationSS6PU[indexMaterial] = (0 > indexTexture)
																							 ? null
																							: informationSSPJ.TableInformationTexture[indexTexture].MaterialAnimationSS6PU.TableData[indexMaterialTexture];
						}
					}

					countMaterial = Script_SpriteStudio6_RootEffect.Material.CountGetTable(countSSCE);
					informationSSPJ.TableMaterialEffectSS6PU = new Material[countMaterial];
					if(null == informationSSPJ.TableMaterialEffectSS6PU)
					{
						LogError(messageLogPrefix, "Not Enough Memory (MaterialTable Effect)", informationSSPJ.FileNameGetFullPath());
						goto MaterialPickUp_ErrorEnd;
					}
					for(int i=0; i<countSSCE; i++)
					{
						for(int j=(int)Library_SpriteStudio6.KindOperationBlendEffect.INITIATOR; j<(int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR; j++)
						{
							indexTexture = informationSSPJ.TableInformationSSCE[i].IndexTexture;
							indexMaterial = Script_SpriteStudio6_RootEffect.Material.IndexGetTable(i, (Library_SpriteStudio6.KindOperationBlendEffect)j, Library_SpriteStudio6.KindMasking.THROUGH);
							indexMaterialTexture = Script_SpriteStudio6_RootEffect.Material.IndexGetTable(0, (Library_SpriteStudio6.KindOperationBlendEffect)j, Library_SpriteStudio6.KindMasking.THROUGH);
							informationSSPJ.TableMaterialEffectSS6PU[indexMaterial] = (0 > indexTexture)
																						? null
																						: informationSSPJ.TableInformationTexture[indexTexture].MaterialEffectSS6PU.TableData[indexMaterialTexture];
						}

						for(int j=(int)Library_SpriteStudio6.KindOperationBlendEffect.INITIATOR; j<(int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR; j++)
						{
							indexTexture = informationSSPJ.TableInformationSSCE[i].IndexTexture;
							indexMaterial = Script_SpriteStudio6_RootEffect.Material.IndexGetTable(i, (Library_SpriteStudio6.KindOperationBlendEffect)j, Library_SpriteStudio6.KindMasking.MASK);
							indexMaterialTexture = Script_SpriteStudio6_RootEffect.Material.IndexGetTable(0, (Library_SpriteStudio6.KindOperationBlendEffect)j, Library_SpriteStudio6.KindMasking.MASK);
							informationSSPJ.TableMaterialEffectSS6PU[indexMaterial] = (0 > indexTexture)
																						? null
																						: informationSSPJ.TableInformationTexture[indexTexture].MaterialEffectSS6PU.TableData[indexMaterialTexture];
						}
					}

					return(true);

				MaterialPickUp_ErrorEnd:;
					return(false);
				}
				#endregion Functions
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
