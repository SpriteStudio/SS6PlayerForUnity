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
					case KindVersion.CODE_020001:
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

				/* MEMO: SSQE has no Base-Directory specification. */
//				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/sequenceBaseDirectory", managerNameSpace);
				informationSSPJ.NameDirectoryBaseSSQE = string.Copy(informationSSPJ.NameDirectory);

				/* Get Texture-Mode-Setting */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/wrapMode", managerNameSpace);
				switch(valueText)
				{
					case "repeat":
						informationSSPJ.WrapTexture = Library_SpriteStudio6.Data.Texture.KindWrap.REPEAT;
						break;

					case "mirror":
#if UNITY_2017_1_OR_NEWER
						informationSSPJ.WrapTexture = Library_SpriteStudio6.Data.Texture.KindWrap.MIRROR;
						break;
#else
						LogWarning(messageLogPrefix, "Texture Wrap-Mode \"Mirror\" is not Suppoted. Change to \"Clamp\"", nameFile);
						goto case "clamp";
#endif
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
						LogWarning(messageLogPrefix, "Texture Filter-Mode Unknown. Change to \"Linear\"", nameFile);
						informationSSPJ.FilterTexture = Library_SpriteStudio6.Data.Texture.KindFilter.LINEAR;
						break;
				}

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/convertImageToPMA", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
				    informationSSPJ.FlagConvertImagePremultipliedAlpha = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
				}

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/blendImageAsPMA", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
				    informationSSPJ.FlagBlendImagePremultipliedAlpha = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
				}

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/vertexAnimeFloat", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
				    informationSSPJ.FlagVertexAnimeFloat = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
				}

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/signal", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
					informationSSPJ.Signal = valueText;
				}

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "settings/coordUnit", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
					switch(valueText)
					{
						case "rate":
							informationSSPJ.CoordUnit = LibraryEditor_SpriteStudio6.Import.SSPJ.KindCoordUnit.RATE;
							break;
						case "pix":
							informationSSPJ.CoordUnit = LibraryEditor_SpriteStudio6.Import.SSPJ.KindCoordUnit.PIX;
							break;
						default:
							break;
					}
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

				/* add 6.4 Sequence FileNames */
				nodeList = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "sequencepackNames/value", managerNameSpace);
				if(null == nodeList)
				{
					informationSSPJ.TableNameSSQE = new string[0];
				}
				else
				{
					foreach(System.Xml.XmlNode NodeEffect in nodeList)
					{
						nameFile = NodeEffect.InnerText;
						nameFile = informationSSPJ.PathGetAbsolute(nameFile, LibraryEditor_SpriteStudio6.Import.KindFile.SSQE);
						listNameFile.Add(nameFile);
					}
					informationSSPJ.TableNameSSQE = listNameFile.ToArray();
				}
				informationSSPJ.TableInformationSSQE = new LibraryEditor_SpriteStudio6.Import.SSQE.Information[informationSSPJ.TableNameSSQE.Length];
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
				CODE_020001 = 0x00020001,	/* after SS6.2.0 */

				TARGET_EARLIEST = CODE_020000,
				TARGET_LATEST = CODE_020001
			}

			public enum KindCoordUnit	/* Added 6.4 */
			{
				INVALID = -1,
				RATE,
				PIX,

				TERMINATOR
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
				public bool FlagConvertImagePremultipliedAlpha;
				public bool FlagBlendImagePremultipliedAlpha;
				public bool FlagVertexAnimeFloat;

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

				public string NameDirectoryBaseSSQE;
				public LibraryEditor_SpriteStudio6.Import.SSQE.Information[] TableInformationSSQE;
				public string[] TableNameSSQE;	/* Temporary */

				public LibraryEditor_SpriteStudio6.Import.Assets<Script_SpriteStudio6_DataProject> DataProjectSS6PU;
				public LibraryEditor_SpriteStudio6.Import.Assets<Script_SpriteStudio6_DataCellMap> DataCellMapSS6PU;

				/* MEMO: Before 6.4 */
				public string Signal;
				public KindCoordUnit CoordUnit;

				public LibraryEditor_SpriteStudio6.Import.SignalSettings.Information InformationSignalSetting;	/* Set externally */
				/* MEMO: (Up to this point) Before 6.4 */
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
					FlagConvertImagePremultipliedAlpha = false;
					FlagBlendImagePremultipliedAlpha = false;
					FlagVertexAnimeFloat = false;

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

					NameDirectoryBaseSSEE = "";
					TableNameSSEE = null;
					TableInformationSSEE = null;

					DataProjectSS6PU.CleanUp();
					DataProjectSS6PU.BootUp(1);

					DataCellMapSS6PU.CleanUp();
//					DataCellMapSS6PU.BootUp(1);	/* Don't boot-up here. */

					Signal = "";
					CoordUnit = KindCoordUnit.INVALID;	/* KindCoordUnit.RATE */
					InformationSignalSetting = null;
				}

				public bool InformationCreateTexture(ref LibraryEditor_SpriteStudio6.Import.Setting setting)
				{
					const string messageLogPrefix = "Fix Information(Texture)";

					/* Create Texture-Information Table */
					int countTexture = (null != ListNameTexture) ? ListNameTexture.Count : 0;
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

							case LibraryEditor_SpriteStudio6.Import.KindFile.SSQE:
								namePathNew = NameDirectoryBaseSSQE + namePath;
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

							case LibraryEditor_SpriteStudio6.Import.KindFile.SSQE:
								nameBase = NameDirectoryBaseSSQE;
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

				public int IndexGetSequence(string name)
				{
					int count = TableInformationSSQE.Length;
					for(int i=0; i<count; i++)
					{
						if(null != TableInformationSSQE[i])
						{
							if(name == TableInformationSSQE[i].NameFileBody)
							{
								return(i);
							}
						}
					}
					return(-1);
				}

				public int IndexGetTexture(string name)
				{
					int count = TableInformationTexture.Length;
					for(int i=0; i<count; i++)
					{
						if(null != TableInformationTexture[i])
						{
							if(name == TableInformationTexture[i].NameFileBody)
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
								break;	/* i-Loop */
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
					int countSSQE = informationSSPJ.TableInformationSSQE.Length;
					int countTexture = informationSSPJ.TableInformationTexture.Length;
					int countPrefabAnimation = informationSSPJ.TableInformationSSQE.Length;

					/* Get reference to original datas */
					/* MEMO: These arrays are old (will be overwritten) data sorted in order for (new) project-data. */
					Script_SpriteStudio6_DataProject dataProjectOld = null;
					Script_SpriteStudio6_DataCellMap dataCellMapOld = null;
					Script_SpriteStudio6_DataAnimation[] dataAnimationOld = new Script_SpriteStudio6_DataAnimation[countSSAE];
					Script_SpriteStudio6_DataEffect[] dataEffectOld = new Script_SpriteStudio6_DataEffect[countSSEE];
					Script_SpriteStudio6_DataSequence[] dataSequenceOld = new Script_SpriteStudio6_DataSequence[countSSQE];
					Texture2D[] textureOld = new Texture2D[countTexture];	/* Texture[] */
					Script_SpriteStudio6_Root[] prefabAnimationOld = new Script_SpriteStudio6_Root[countSSAE];
					Script_SpriteStudio6_RootEffect[] prefabEffectOld = new Script_SpriteStudio6_RootEffect[countSSEE];
					for(int i=0; i<countSSAE; i++)
					{
						dataAnimationOld[i] = null;
						prefabAnimationOld[i] = null;
					}
					for(int i=0; i<countSSEE; i++)
					{
						dataEffectOld[i] = null;
						prefabEffectOld[i] = null;
					}
					for(int i=0; i<countSSQE; i++)
					{
						dataSequenceOld[i] = null;
					}
					for(int i=0; i<countTexture; i++)
					{
						textureOld[i] = null;
					}

					/* SSPJ (Data) */
					/* MEMO: First, get it by the name of the project.                 */
					/*       When Project-Data is not found, may be reverse-identified */
					/*        from SSCE, SSAE and SSEE that are found later.           */
					if(false == LibraryEditor_SpriteStudio6.Import.SSPJ.ModeSS6PU.AssetNameDecideProject(	ref setting,
																											informationSSPJ,
																											nameOutputAssetFolderBase,
																											null
																									)
						)
					{
						goto AssetNameDecide_ErrorEnd;
					}

					dataProjectOld = informationSSPJ.DataProjectSS6PU.TableData[0];
					if(false == setting.Basic.FlagTrackAssets)
					{	/* Not Tracking */
						/* MEMO: Even when project to overwrite is found...                     */
						/*       If "Tracking Assets" setting is false, assets are not tracked. */
						/*       Then overwrite is determined from stored asset-file name.      */
						dataProjectOld = null;
					}

					if(null != dataProjectOld)
					{	/* Asset is found */
						/* SSAEs */
						if(null != dataProjectOld.DataAnimation)
						{
							int count = dataProjectOld.DataAnimation.Length;
							for(int i=0; i<count; i++)
							{
								if(null != dataProjectOld.DataAnimation[i])
								{
									string name = dataProjectOld.DataAnimation[i].Name;
									if(false == string.IsNullOrEmpty(name))
									{
										int index = informationSSPJ.IndexGetAnimation(name);
										if(0 <= index)
										{
											dataAnimationOld[index] = dataProjectOld.DataAnimation[i];
										}
									}
								}
							}
						}
						/* SSAEs (Prefabs) */
						/* MEMO: prefab is paired with DataAnimation, so key is DataAnimation that prefab has. */
						if(null != dataProjectOld.PrefabAnimation)
						{
							int count = dataProjectOld.PrefabAnimation.Length;
							for(int i=0; i<count; i++)
							{
								Script_SpriteStudio6_Root scriptRoot = dataProjectOld.PrefabAnimation[i] as Script_SpriteStudio6_Root;
								if(null != scriptRoot)
								{
									Script_SpriteStudio6_DataAnimation dataAnimation = scriptRoot.DataAnimation;
									if(null != dataAnimation)
									{
										string name = dataAnimation.Name;
										if(false == string.IsNullOrEmpty(name))
										{
											int index = informationSSPJ.IndexGetAnimation(name);
											if(0 <= index)
											{
												prefabAnimationOld[index] = scriptRoot;
											}
										}
									}
								}
							}
						}

						/* SSEEs */
						if(null != dataProjectOld.DataEffect)
						{
							int count = dataProjectOld.DataEffect.Length;
							for(int i=0; i<count; i++)
							{
								if(null != dataProjectOld.DataEffect[i])
								{
									string name = dataProjectOld.DataEffect[i].Name;
									if(false == string.IsNullOrEmpty(name))
									{
										int index = informationSSPJ.IndexGetEffect(name);
										if(0 <= index)
										{
											dataEffectOld[index] = dataProjectOld.DataEffect[i];
										}
									}
								}
							}
						}
						/* SSCEs (Prefabs) */
						/* MEMO: prefab is paired with DataEffect, so key is DataEffect that prefab has. */
						if(null != dataProjectOld.PrefabEffect)
						{
							int count = dataProjectOld.PrefabEffect.Length;
							for(int i=0; i<count; i++)
							{
								Script_SpriteStudio6_RootEffect scriptRoot = dataProjectOld.PrefabEffect[i] as Script_SpriteStudio6_RootEffect;
								if(null != scriptRoot)
								{
									Script_SpriteStudio6_DataEffect dataEffect = scriptRoot.DataEffect;
									if(null != dataEffect)
									{
										string name = dataEffect.Name;
										if(false == string.IsNullOrEmpty(name))
										{
											int index = informationSSPJ.IndexGetEffect(name);
											if(0 <= index)
											{
												prefabEffectOld[index] = scriptRoot;
											}
										}
									}
								}
							}
						}

						/* SSQEs */
						if(null != dataProjectOld.DataSequence)
						{
							int count = dataProjectOld.DataSequence.Length;
							for(int i=0; i<count; i++)
							{
								if(null != dataProjectOld.DataSequence[i])
								{
									string name = dataProjectOld.DataSequence[i].Name;
									if(false == string.IsNullOrEmpty(name))
									{
										int index = informationSSPJ.IndexGetSequence(name);
										if(0 <= index)
										{
											dataSequenceOld[index] = dataProjectOld.DataSequence[i];
										}
									}
								}
							}
						}

						/* SSCE */
						/* MEMO: In principle, CellMap-data (SSCE) is paired with project-data. */
						dataCellMapOld = dataProjectOld.DataCellMap;

						/* Textures */
						if(null != dataProjectOld.TableTexture)
						{
							int count = dataProjectOld.TableTexture.Length;
							for(int i=0; i<count; i++)
							{
								if(null != dataProjectOld.TableTexture[i])
								{
									string name = dataProjectOld.TableTexture[i].name;
									if(false == string.IsNullOrEmpty(name))
									{
										int index = informationSSPJ.IndexGetTexture(name);
										if(0 <= index)
										{
											textureOld[index] = dataProjectOld.TableTexture[i] as Texture2D;
										}
									}
								}
							}
						}
					}

					/* Determine asset-names */
					for(int i=0; i<countSSAE; i++)	/* SSAEs (Prefab) */
					{
						if(false == LibraryEditor_SpriteStudio6.Import.SSAE.ModeSS6PU.AssetNameDecidePrefab(	ref setting,
																												informationSSPJ,
																												informationSSPJ.TableInformationSSAE[i],
																												nameOutputAssetFolderBase,
																												prefabAnimationOld[i]
																											)
							)
						{
							goto AssetNameDecide_ErrorEnd;
						}
					}
					for(int i=0; i<countSSEE; i++)	/* SSEEs (Prefab) */
					{
						if(false == LibraryEditor_SpriteStudio6.Import.SSEE.ModeSS6PU.AssetNameDecidePrefab(	ref setting,
																												informationSSPJ,
																												informationSSPJ.TableInformationSSEE[i],
																												nameOutputAssetFolderBase,
																												prefabEffectOld[i]
																										)
							)
						{
							goto AssetNameDecide_ErrorEnd;
						}
					}
					if(0 < countSSCE)	/* SSCEs (CellMap data) */
					{	/* Has data */
						informationSSPJ.DataCellMapSS6PU.BootUp(1);	/* Always 1 */
						if(false == AssetNameDecideCellMap(	ref setting,
															informationSSPJ,
															nameOutputAssetFolderBase,
															dataCellMapOld
														)
							)
						{
							goto AssetNameDecide_ErrorEnd;
						}
					}
					else
					{	/* Has no data */
						informationSSPJ.DataCellMapSS6PU.CleanUp();
					}
					for(int i=0; i<countSSAE; i++)	/* SSAEs (Data) */
					{
						if(false == LibraryEditor_SpriteStudio6.Import.SSAE.ModeSS6PU.AssetNameDecideData(	ref setting,
																											informationSSPJ,
																											informationSSPJ.TableInformationSSAE[i],
																											nameOutputAssetFolderBase,
																											dataAnimationOld[i]
																										)
							)
						{
							goto AssetNameDecide_ErrorEnd;
						}
					}
					for(int i=0; i<countSSEE; i++)	/* SSEEs (Data) */
					{
						if(false == LibraryEditor_SpriteStudio6.Import.SSEE.ModeSS6PU.AssetNameDecideData(	ref setting,
																											informationSSPJ,
																											informationSSPJ.TableInformationSSEE[i],
																											nameOutputAssetFolderBase,
																											dataEffectOld[i]
																										)
							)
						{
							goto AssetNameDecide_ErrorEnd;
						}
					}
					for(int i=0; i<countSSQE; i++)	/* SSQEs (Data) */
					{
						if(false == LibraryEditor_SpriteStudio6.Import.SSQE.ModeSS6PU.AssetNameDecideData(	ref setting,
																											informationSSPJ,
																											informationSSPJ.TableInformationSSQE[i],
																											nameOutputAssetFolderBase,
																											dataSequenceOld[i]
																										)
							)
						{
							goto AssetNameDecide_ErrorEnd;
						}
					}
					for(int i=0; i<countTexture; i++)	/* Textures */
					{
						if(false == LibraryEditor_SpriteStudio6.Import.SSCE.AssetNameDecideTexture(	ref setting,
																									informationSSPJ,
																									informationSSPJ.TableInformationTexture[i],
																									nameOutputAssetFolderBase,
																									textureOld[i]
																								)
							)
						{
							goto AssetNameDecide_ErrorEnd;
						}
					}

					dataProjectOld = null;
					dataCellMapOld = null;
					dataAnimationOld = null;
					dataEffectOld = null;
					dataSequenceOld = null;
					textureOld = null;
					prefabAnimationOld = null;
					prefabEffectOld = null;

					return(true);

				AssetNameDecide_ErrorEnd:;
					dataProjectOld = null;
					dataCellMapOld = null;
					dataAnimationOld = null;
					dataEffectOld = null;
					dataSequenceOld = null;
					textureOld = null;
					prefabAnimationOld = null;
					prefabEffectOld = null;

					return(false);
				}

				private static bool AssetNameDecideProject(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
															LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
															string nameOutputAssetFolderBase,
															Script_SpriteStudio6_DataProject dataOverride
														)
				{
					if(null != dataOverride)
					{	/* Specified */
						informationSSPJ.DataProjectSS6PU.TableName[0] = AssetDatabase.GetAssetPath(dataOverride);
					}
					else
					{	/* Default */
						informationSSPJ.DataProjectSS6PU.TableName[0] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_PROJECT_SS6PU, nameOutputAssetFolderBase)
																		+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_PROJECT_SS6PU, informationSSPJ.NameFileBody, informationSSPJ.NameFileBody)
																		+ LibraryEditor_SpriteStudio6.Import.NameExtentionScriptableObject;
						dataOverride = AssetDatabase.LoadAssetAtPath<Script_SpriteStudio6_DataProject>(informationSSPJ.DataProjectSS6PU.TableName[0]);
					}

					informationSSPJ.DataProjectSS6PU.TableData[0] = dataOverride;
					informationSSPJ.DataProjectSS6PU.Version[0] = (null != dataOverride) ? (int)(dataOverride.Version) : (int)Script_SpriteStudio6_DataProject.KindVersion.SS5PU;

					return(true);

//				AssetNameDecideProject_ErrorEnd:;
//					return(false);
				}

				private static bool AssetNameDecideCellMap(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
															LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
															string nameOutputAssetFolderBase,
															Script_SpriteStudio6_DataCellMap dataOverride
														)
				{
					if(null != dataOverride)
					{	/* Specified */
						informationSSPJ.DataCellMapSS6PU.TableName[0] = AssetDatabase.GetAssetPath(dataOverride);
					}
					else
					{	/* Default */
						informationSSPJ.DataCellMapSS6PU.TableName[0] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_CELLMAP_SS6PU, nameOutputAssetFolderBase)
																		+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_CELLMAP_SS6PU, informationSSPJ.NameFileBody, informationSSPJ.NameFileBody)
																		+ LibraryEditor_SpriteStudio6.Import.NameExtentionScriptableObject;
						dataOverride = AssetDatabase.LoadAssetAtPath<Script_SpriteStudio6_DataCellMap>(informationSSPJ.DataCellMapSS6PU.TableName[0]);
					}

					informationSSPJ.DataCellMapSS6PU.TableData[0] = dataOverride;
					informationSSPJ.DataCellMapSS6PU.Version[0] = (null != dataOverride) ? (int)(dataOverride.Version) : (int)Script_SpriteStudio6_DataCellMap.KindVersion.SS5PU;

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
					dataCellMap.DataProject = informationSSPJ.DataProjectSS6PU.TableData[0];

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

				public static bool ConvertDataProject(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
														LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
													)
				{
//					const string messageLogPrefix = "Convert (Data-Project)";

					/* MEMO: Nothing to do, now. */

					return(true);

//				ConvertData_ErrorEnd:;
//					return(false);
				}

				public static bool AssetCreateDataProject(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
															LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
														)
				{
//					const string messageLogPrefix = "Create Asset(Data-Project)";

					Script_SpriteStudio6_DataProject dataProject = informationSSPJ.DataProjectSS6PU.TableData[0];
					if(null == dataProject)
					{
						dataProject = ScriptableObject.CreateInstance<Script_SpriteStudio6_DataProject>();
						AssetDatabase.CreateAsset(dataProject, informationSSPJ.DataProjectSS6PU.TableName[0]);
						informationSSPJ.DataProjectSS6PU.TableData[0] = dataProject;
					}

					dataProject.Version = Script_SpriteStudio6_DataProject.KindVersion.SUPPORT_LATEST;
					dataProject.Name = string.Copy(informationSSPJ.NameFileBody);

					int countSSAE = (null != informationSSPJ.TableInformationSSAE) ? informationSSPJ.TableInformationSSAE.Length : 0;
					int countSSCE = (null != informationSSPJ.TableInformationSSCE) ? informationSSPJ.TableInformationSSCE.Length : 0;
					int countSSEE = (null != informationSSPJ.TableInformationSSEE) ? informationSSPJ.TableInformationSSEE.Length : 0;
					int countSSQE = (null != informationSSPJ.TableInformationSSQE) ? informationSSPJ.TableInformationSSQE.Length : 0;

					/* MEMO: Each-data's reference is not set here (not been finalized).      */
					/*       Determined references are set in "AssetFixDataProject" function. */

					/* Cell-Map & Texture */
					dataProject.DataCellMap = null;
					dataProject.TableTexture = new Texture[countSSCE];

					/* Animation */
					dataProject.DataAnimation = new Script_SpriteStudio6_DataAnimation[countSSAE];
					dataProject.PrefabAnimation = new Object[countSSAE];

					/* Effect */
					dataProject.DataEffect = new Script_SpriteStudio6_DataEffect[countSSEE];
					dataProject.PrefabEffect = new Object[countSSEE];

					/* Sequence */
					dataProject.DataSequence = new Script_SpriteStudio6_DataSequence[countSSQE];

					EditorUtility.SetDirty(dataProject);
					AssetDatabase.SaveAssets();

					return(true);

//				AssetCreateData_ErrorEnd:;
//					return(false);
				}

				public static bool AssetFixDataProject(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
															LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
														)
				{
//					const string messageLogPrefix = "Create Asset(Data-Project)";

					Script_SpriteStudio6_DataProject dataProject = informationSSPJ.DataProjectSS6PU.TableData[0];
					if(null == dataProject)
					{
						goto AssetFixDataProject_ErrorEnd;
					}

					int countSSAE = (null != informationSSPJ.TableInformationSSAE) ? informationSSPJ.TableInformationSSAE.Length : 0;
					int countSSCE = (null != informationSSPJ.TableInformationSSCE) ? informationSSPJ.TableInformationSSCE.Length : 0;
					int countSSEE = (null != informationSSPJ.TableInformationSSEE) ? informationSSPJ.TableInformationSSEE.Length : 0;
					int countSSQE = (null != informationSSPJ.TableInformationSSQE) ? informationSSPJ.TableInformationSSQE.Length : 0;

					/* Cell-Map & Texture */
					dataProject.DataCellMap = (0 < countSSCE) ? informationSSPJ.DataCellMapSS6PU.TableData[0] : null;
					dataProject.TableTexture = new Texture[countSSCE];
					for(int i=0; i<countSSCE; i++)
					{
						int indexTexture = informationSSPJ.TableInformationSSCE[i].IndexTexture;
						if(0 > indexTexture)
						{
							dataProject.TableTexture[i] = null;
						}
						else
						{
							dataProject.TableTexture[i] = informationSSPJ.TableInformationTexture[indexTexture].PrefabTexture.TableData[0];
						}
					}

					/* Animation */
					for(int i=0; i<countSSAE; i++)
					{
						dataProject.DataAnimation[i] = informationSSPJ.TableInformationSSAE[i].DataAnimationSS6PU.TableData[0];
						dataProject.PrefabAnimation[i] = informationSSPJ.TableInformationSSAE[i].PrefabAnimationSS6PU.TableData[0];
					}

					/* Effect */
					for(int i=0; i<countSSEE; i++)
					{
						dataProject.DataEffect[i] = informationSSPJ.TableInformationSSEE[i].DataEffectSS6PU.TableData[0];
						dataProject.PrefabEffect[i] = informationSSPJ.TableInformationSSEE[i].PrefabEffectSS6PU.TableData[0];
					}

					/* Sequence */
					for(int i=0; i<countSSQE; i++)
					{
						dataProject.DataSequence[i] = informationSSPJ.TableInformationSSQE[i].DataSequenceSS6PU.TableData[0];
					}

					EditorUtility.SetDirty(dataProject);
					AssetDatabase.SaveAssets();

					return(true);

				AssetFixDataProject_ErrorEnd:;
					return(false);
				}
				#endregion Functions
			}

			public static partial class ModeUnityNative
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
					int countTexture = informationSSPJ.TableInformationTexture.Length;

					/* SSAEs (Prefab) */
					for(int i=0; i<countSSAE; i++)
					{
						LibraryEditor_SpriteStudio6.Import.SSAE.ModeUnityNative.AssetNameDecidePrefab(	ref setting,
																										informationSSPJ,
																										informationSSPJ.TableInformationSSAE[i],
																										nameOutputAssetFolderBase,
																										null
																									);
					}

					/* SSAEs (AnimationClip) */
					int countAnimation;
					for(int i=0; i<countSSAE; i++)
					{
						countAnimation = informationSSPJ.TableInformationSSAE[i].TableAnimation.Length;
						/* MEMO: Create asset's informations since number of animations in SSAE is finalized. */
						informationSSPJ.TableInformationSSAE[i].DataAnimationUnityNative.BootUp(countAnimation);
						for(int j=0; j<countAnimation; j++)
						{
							LibraryEditor_SpriteStudio6.Import.SSAE.ModeUnityNative.AssetNameDecideData(	ref setting,
																											informationSSPJ,
																											informationSSPJ.TableInformationSSAE[i],
																											j,
																											nameOutputAssetFolderBase,
																											null
																										);
						}
					}

					/* SSAEs (SkinnedMesh) */
					int countParts;
					for(int i=0; i<countSSAE; i++)
					{
						countParts = informationSSPJ.TableInformationSSAE[i].TableParts.Length;
						for(int j=0; j<countParts; j++)
						{
							LibraryEditor_SpriteStudio6.Import.SSAE.ModeUnityNative.AssetNameDecideDataMesh(	ref setting,
																												informationSSPJ,
																												informationSSPJ.TableInformationSSAE[i],
																												j,
																												nameOutputAssetFolderBase,
																												null
																											);
						}
					}

					/* Textures */
					for(int i=0; i<countTexture; i++)
					{
						LibraryEditor_SpriteStudio6.Import.SSCE.AssetNameDecideTexture(	ref setting,
																						informationSSPJ,
																						informationSSPJ.TableInformationTexture[i],
																						nameOutputAssetFolderBase,
																						null
																					);
					}

					return(true);

//				AssetNameDecide_ErrorEnd:;
//					return(false);
				}
				#endregion Functions
			}

			public static partial class ModeUnityUI
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
					int countTexture = informationSSPJ.TableInformationTexture.Length;

					/* SSAEs (Prefab) */
					for(int i=0; i<countSSAE; i++)
					{
						LibraryEditor_SpriteStudio6.Import.SSAE.ModeUnityUI.AssetNameDecidePrefab(	ref setting,
																									informationSSPJ,
																									informationSSPJ.TableInformationSSAE[i],
																									nameOutputAssetFolderBase,
																									null
																								);
					}

					/* SSAEs (AnimationClip) */
					int countAnimation;
					for(int i=0; i<countSSAE; i++)
					{
						countAnimation = informationSSPJ.TableInformationSSAE[i].TableAnimation.Length;
						/* MEMO: Create asset's informations since number of animations in SSAE is finalized. */
						informationSSPJ.TableInformationSSAE[i].DataAnimationUnityUI.BootUp(countAnimation);
						for(int j=0; j<countAnimation; j++)
						{
							LibraryEditor_SpriteStudio6.Import.SSAE.ModeUnityUI.AssetNameDecideData(	ref setting,
																										informationSSPJ,
																										informationSSPJ.TableInformationSSAE[i],
																										j,
																										nameOutputAssetFolderBase,
																										null
																									);
						}
					}

					/* Textures */
					for(int i=0; i<countTexture; i++)
					{
						LibraryEditor_SpriteStudio6.Import.SSCE.AssetNameDecideTexture(	ref setting,
																						informationSSPJ,
																						informationSSPJ.TableInformationTexture[i],
																						nameOutputAssetFolderBase,
																						null
																					);
					}

					return(true);

//				AssetNameDecide_ErrorEnd:;
//					return(false);
				}
				#endregion Functions
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
