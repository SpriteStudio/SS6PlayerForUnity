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
		public static partial class SSCE
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static Information Parse(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
												string nameFile,
												LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
											)
			{
				const string messageLogPrefix = "SSCE-Parse";
				Information informationSSCE = null;

				/* ".ssce" Load */
				if(false == System.IO.File.Exists(nameFile))
				{
					LogError(messageLogPrefix, "File Not Found", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				System.Xml.XmlDocument xmlSSCE = new System.Xml.XmlDocument();
				xmlSSCE.Load(nameFile);

				/* Check Version */
				System.Xml.XmlNode nodeRoot = xmlSSCE.FirstChild;
				nodeRoot = nodeRoot.NextSibling;
				KindVersion version = (KindVersion)(LibraryEditor_SpriteStudio6.Utility.XML.VersionGet(nodeRoot, "SpriteStudioCellMap", (int)KindVersion.ERROR, true));
				switch(version)
				{
					case KindVersion.ERROR:
						LogError(messageLogPrefix, "Version Invalid", nameFile, informationSSPJ);
						goto Parse_ErrorEnd;

					case KindVersion.CODE_000100:
					case KindVersion.CODE_010000:
					case KindVersion.CODE_010001:
						break;

					default:
						if(KindVersion.TARGET_EARLIEST > version)
						{
							version = KindVersion.TARGET_EARLIEST;
							if(true == setting.CheckVersion.FlagInvalidSSCE)
							{
								LogWarning(messageLogPrefix, "Version Too Early", nameFile, informationSSPJ);
							}
						}
						else
						{
							version = KindVersion.TARGET_LATEST;
							if(true == setting.CheckVersion.FlagInvalidSSCE)
							{
								LogWarning(messageLogPrefix, "Version Unknown", nameFile, informationSSPJ);
							}
						}
						break;
				}

				/* Create Information */
				informationSSCE = new Information();
				if(null == informationSSCE)
				{
					LogError(messageLogPrefix, "Not Enough Memory", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				informationSSCE.CleanUp();
				informationSSCE.Version = version;

				/* Get Base-Directories */
				LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out informationSSCE.NameDirectory, out informationSSCE.NameFileBody, out informationSSCE.NameFileExtension, nameFile);
				informationSSCE.NameDirectory += "/";

				/* Decode Tags */
				System.Xml.NameTable nodeNameSpace = new System.Xml.NameTable();
				System.Xml.XmlNamespaceManager managerNameSpace = new System.Xml.XmlNamespaceManager(nodeNameSpace);

				string valueText = "";

				/* Get Texture Path-Name */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "imagePath", managerNameSpace);
				string namePathTexture = "";
				if(true == System.IO.Path.IsPathRooted(valueText))
				{
					namePathTexture = string.Copy(valueText);
				}
				else
				{
					namePathTexture = informationSSPJ.PathGetAbsolute(valueText, LibraryEditor_SpriteStudio6.Import.KindFile.TEXTURE);
				}
				informationSSCE.IndexTexture = informationSSPJ.AddTexture(namePathTexture);

				/* Get Texture Pixel-Size */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "pixelSize", managerNameSpace);
				string[] valueTextSprit = null;
				if(true == string.IsNullOrEmpty(valueText))
				{	/* Get directly from texture */
					informationSSCE.SizePixelX = -1;
					informationSSCE.SizePixelY = -1;
				}
				else
				{	/* Synchronized with Cell size */
					valueTextSprit = valueText.Split(' ');
					informationSSCE.SizePixelX = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueTextSprit[0]);
					informationSSCE.SizePixelY = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueTextSprit[1]);
				}

				/* Get Texture Addressing */
				informationSSCE.WrapTexture = informationSSPJ.WrapTexture;
				informationSSCE.FilterTexture = informationSSPJ.FilterTexture;

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "overrideTexSettings", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
					bool valueBool = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
					if(true == valueBool)
					{
						/* Get Texture Wrap-Mode */
						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "wrapMode", managerNameSpace);
						switch(valueText)
						{
							case "repeat":
								informationSSCE.WrapTexture = Library_SpriteStudio6.Data.Texture.KindWrap.REPEAT;
								break;

							case "mirror":
#if false
								LogWarning(messageLogPrefix, "Wrap-Mode \"Mirror\" is not Suppoted. Changed \"Clamp\"", nameFile, informationSSPJ);
								goto case "clamp";
#else
								informationSSCE.WrapTexture = Library_SpriteStudio6.Data.Texture.KindWrap.MIRROR;
								break;
#endif

							case "clamp":
								informationSSCE.WrapTexture = Library_SpriteStudio6.Data.Texture.KindWrap.CLAMP;
								break;

							default:
								goto case "clamp";
						}

						/* Get Texture Filter-Mode */
						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "filterMode", managerNameSpace);
						switch(valueText)
						{
							case "nearlest":
								informationSSCE.FilterTexture = Library_SpriteStudio6.Data.Texture.KindFilter.NEAREST;
								break;

							case "linear":
								informationSSCE.FilterTexture = Library_SpriteStudio6.Data.Texture.KindFilter.LINEAR;
								break;

							case "bilinear":
								informationSSCE.FilterTexture = Library_SpriteStudio6.Data.Texture.KindFilter.BILINEAR;
								break;

							default:
								goto case "linear";
						}
					}
				}

				/* Get Cells */
				List<Information.Cell> listCell = new List<Information.Cell>();
				if(null == listCell)
				{
					LogError(messageLogPrefix, "Not Enough Memory (CellMap WorkArea)", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				listCell.Clear();

				System.Xml.XmlNodeList listNode= LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "cells/cell", managerNameSpace);
				if(null == listNode)
				{
					LogError(messageLogPrefix, "Cells-Node Not Found", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}

				double pivotNormalizeX = 0.0;
				double pivotNormalizeY = 0.0;
				Information.Cell cell = null;
				foreach(System.Xml.XmlNode nodeCell in listNode)
				{
					cell = new Information.Cell();
					if(null == cell)
					{
						LogError(messageLogPrefix, "Not Enough Memory (Cell WorkArea)", nameFile, informationSSPJ);
						goto Parse_ErrorEnd;
					}
					cell.CleanUp();

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeCell, "name", managerNameSpace);
					cell.Name = string.Copy(valueText);

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeCell, "pos", managerNameSpace);
					valueTextSprit = valueText.Split(' ');
					cell.Area.x = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueTextSprit[0]));
					cell.Area.y = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueTextSprit[1]));

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeCell, "size", managerNameSpace);
					valueTextSprit = valueText.Split(' ');
					cell.Area.width = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueTextSprit[0]));
					cell.Area.height = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueTextSprit[1]));

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeCell, "pivot", managerNameSpace);
					valueTextSprit = valueText.Split(' ');
					pivotNormalizeX = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSprit[0]);
					pivotNormalizeY = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSprit[1]);
					cell.Pivot.x = (float)((double)cell.Area.width * (pivotNormalizeX + 0.5));
					cell.Pivot.y = (float)((double)cell.Area.height * (-pivotNormalizeY + 0.5));

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeCell, "rotated", managerNameSpace);
					cell.Rotate = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);

					listCell.Add(cell);
				}
				informationSSCE.TableCell = listCell.ToArray();
				listCell.Clear();

				return(informationSSCE);

			Parse_ErrorEnd:
				if(null != informationSSCE)
				{
					informationSSCE.CleanUp();
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
				CODE_000100 = 0x00000100,	/* under-development SS5 */
				CODE_010000 = 0x00010000,	/* after SS5.0.0 */
				CODE_010001 = 0x00010200,	/* after SS5.5.0 beta-3 */

				TARGET_EARLIEST = CODE_000100,
				TARGET_LATEST = CODE_010001
			}

			private const string ExtentionFile = ".ssce";
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public class Information
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public LibraryEditor_SpriteStudio6.Import.SSCE.KindVersion Version;
				public Library_SpriteStudio6.Data.CellMap Data;

				public string NameDirectory;
				public string NameFileBody;
				public string NameFileExtension;

				public int IndexTexture;
				public int SizePixelX;	/* Temporary */
				public int SizePixelY;	/* Temporary */
				public Library_SpriteStudio6.Data.Texture.KindWrap WrapTexture;
				public Library_SpriteStudio6.Data.Texture.KindFilter FilterTexture;

				public Cell[] TableCell;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Version = LibraryEditor_SpriteStudio6.Import.SSCE.KindVersion.ERROR;
					Data = new Library_SpriteStudio6.Data.CellMap();
					Data.CleanUp();

					NameDirectory = "";
					NameFileBody = "";
					NameFileExtension = "";

					IndexTexture = -1;
					WrapTexture = (Library_SpriteStudio6.Data.Texture.KindWrap)(-1);
					FilterTexture = (Library_SpriteStudio6.Data.Texture.KindFilter)(-1);

					TableCell = null;
				}

				public string FileNameGetFullPath()
				{
					return(NameDirectory + NameFileBody + NameFileExtension);
				}

				public int IndexGetCell(string name)
				{
					if(null != TableCell)
					{
						int count = TableCell.Length;
						for(int i=0; i<count; i++)
						{
							if(name == TableCell[i].Name)
							{
								return(i);
							}
						}
					}
					return(-1);
				}

				public bool ConvertSS6PU(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
												LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
											)
				{	/* Convert-SS6PU Pass-1 ... Transfer necessary data from the temporary. */
					LibraryEditor_SpriteStudio6.Import.SSCE.Information.Texture informationTexture = null;	/* hUnityEngine.Textureh and my "Texture", class-names are conflict unless fully-qualified. */
					if(0 <= IndexTexture)
					{
						informationTexture = informationSSPJ.TableInformationTexture[IndexTexture];
						int countCell = TableCell.Length;

						Data.Name = string.Copy(NameFileBody);
						Data.SizeOriginal.x = (float)informationTexture.SizeX;
						Data.SizeOriginal.y = (float)informationTexture.SizeY;
						Data.TableCell = new Library_SpriteStudio6.Data.CellMap.Cell[countCell];

						Library_SpriteStudio6.Data.CellMap.Cell informationCell = new Library_SpriteStudio6.Data.CellMap.Cell();
						for(int i=0; i<countCell; i++)
						{
							informationCell.CleanUp();
							informationCell.Name = string.Copy(TableCell[i].Name);
							informationCell.Rectangle = TableCell[i].Area;
							informationCell.Pivot = TableCell[i].Pivot;

							Data.TableCell[i] = informationCell;
						}
						TableCell = null;	/* Purge WorkArea */
					}
					return(true);

//				ConvertSS6PU_ErrorEnd:;
//					return(false);
				}

				public bool ConvertTrimPixelSS6PU(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
													LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
												)
				{	/* Convert-SS6PU Pass-2 */
					return(false);
				}
				#endregion Functions

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				public class Cell
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public string Name;
					public Rect Area;
					public Vector2 Pivot;
					public float Rotate;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public void CleanUp()
					{
						Name = "";
						Area = Rect.zero;
						Pivot = Vector2.zero;
						Rotate = 0.0f;
					}
					#endregion Functions
				}

				public class Texture
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public string Name;

					public string NameDirectory;
					public string NameFileBody;
					public string NameFileExtension;

					public Library_SpriteStudio6.Data.Texture.KindWrap Wrap;
					public Library_SpriteStudio6.Data.Texture.KindFilter Filter;

					public int SizeX;
					public int SizeY;

					public LibraryEditor_SpriteStudio6.Import.Assets<Texture2D> PrefabTexture;
					public LibraryEditor_SpriteStudio6.Import.Assets<Material> MaterialAnimation;
					public LibraryEditor_SpriteStudio6.Import.Assets<Material> MaterialEffect;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public void CleanUp()
					{
						Name = "";

						NameDirectory = "";
						NameFileBody = "";
						NameFileExtension = "";

						Wrap = (Library_SpriteStudio6.Data.Texture.KindWrap)(-1);
						Filter = (Library_SpriteStudio6.Data.Texture.KindFilter)(-1);

						SizeX = -1;
						SizeY = -1;

						PrefabTexture.CleanUp();
						PrefabTexture.BootUp(1);	/* Always 1 */
						MaterialAnimation.CleanUp();
						MaterialAnimation.BootUp((int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR);
						MaterialEffect.CleanUp();
						MaterialEffect.BootUp((int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR);
					}

					public string FileNameGetFullPath()
					{
						return(NameDirectory + NameFileBody + NameFileExtension);
					}

					public bool AssetNameDecideTexture(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
														LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
														string nameOutputAssetFolderBase,
														Texture2D textureOverride
													)
					{
						if(null != textureOverride)
						{	/* Specified */
							PrefabTexture.TableName[0] = AssetDatabase.GetAssetPath(textureOverride);
							PrefabTexture.TableData[0] = textureOverride;
						}
						else
						{	/* Default */
							PrefabTexture.TableName[0] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.TEXTURE, nameOutputAssetFolderBase)
															+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.TEXTURE, NameFileBody, informationSSPJ.NameFileBody)
															+ NameFileExtension;
							/* MEMO: Can not detect Platform-Dependent Textures (such as DDS and PVR). */
							PrefabTexture.TableData[0] = AssetDatabase.LoadAssetAtPath<Texture2D>(PrefabTexture.TableName[0]);
						}

						return(true);
					}

					public bool AssetCreateTexture(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
													LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
												)
					{
						const string messageLogPrefix = "AssetCreate(Texture)";

						/* Copy into Asset */
						string namePathAssetNative = LibraryEditor_SpriteStudio6.Utility.File.PathGetAssetNative(PrefabTexture.TableName[0]);
						LibraryEditor_SpriteStudio6.Utility.File.FileCopyToAsset(	namePathAssetNative,
																					FileNameGetFullPath(),
																					true
																				);

						/* Set Texture-Importer */
						if(null == PrefabTexture.TableData[0])
						{
							AssetDatabase.ImportAsset(PrefabTexture.TableName[0]);
							TextureImporter importer = TextureImporter.GetAtPath(PrefabTexture.TableName[0]) as TextureImporter;
							if(null != importer)
							{
								importer.anisoLevel = 1;
								importer.borderMipmap = false;
								importer.convertToNormalmap = false;
								importer.fadeout = false;
								switch(Filter)
								{
									case Library_SpriteStudio6.Data.Texture.KindFilter.NEAREST:
										importer.filterMode = FilterMode.Point;
										break;

									case Library_SpriteStudio6.Data.Texture.KindFilter.LINEAR:
										importer.filterMode = FilterMode.Bilinear;
										break;

									default:
										/* MEMO: Errors and warnings have already been done and values have been revised. Therefore, will not come here. */
										goto AssetCreate_ErrorEnd;
								}

								/* MEMO: For 5.5.0beta & later, with "Sprite" to avoid unnecessary interpolation. */
								importer.textureShape = TextureImporterShape.Texture2D;
								importer.isReadable = false;
								importer.mipmapEnabled = false;
								importer.maxTextureSize = 4096;
								importer.alphaSource = TextureImporterAlphaSource.FromInput;
								importer.alphaIsTransparency = true;
								importer.npotScale = TextureImporterNPOTScale.None;
								importer.textureType = TextureImporterType.Sprite;

								switch(Wrap)
								{
									case Library_SpriteStudio6.Data.Texture.KindWrap.REPEAT:
										importer.wrapMode = TextureWrapMode.Repeat;
										break;

									case Library_SpriteStudio6.Data.Texture.KindWrap.CLAMP:
										importer.wrapMode = TextureWrapMode.Clamp;
										break;

									case Library_SpriteStudio6.Data.Texture.KindWrap.MIRROR:
										importer.wrapMode = TextureWrapMode.Mirror;
										break;

									default:
										/* MEMO: Errors and warnings have already been done and values have been revised. Therefore, will not come here. */
										goto AssetCreate_ErrorEnd;
								}
								AssetDatabase.ImportAsset(PrefabTexture.TableName[0], ImportAssetOptions.ForceUpdate);
							}
						}
						AssetDatabase.SaveAssets();

						PrefabTexture.TableData[0] = AssetDatabase.LoadAssetAtPath(PrefabTexture.TableName[0], typeof(Texture2D)) as Texture2D;
						if((0 >= SizeX) || (0 >= SizeY))
						{	/* Only when texture size can not be get from SSCE */
							SizeX = PrefabTexture.TableData[0].width;
							SizeY = PrefabTexture.TableData[0].height;
						}

						return(true);

					AssetCreate_ErrorEnd:;
						return(false);
					}

					public bool AssetNameDecideMaterialAnimationSS6PU(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																		LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																		string nameOutputAssetFolderBase,
																		Library_SpriteStudio6.KindOperationBlend operationTarget,
																		Material materialOverride
																	)
					{
						int indexTable = (int)operationTarget;
						if(null != materialOverride)
						{	/* Specified */
							MaterialAnimation.TableName[indexTable] = AssetDatabase.GetAssetPath(materialOverride);
							MaterialAnimation.TableData[indexTable] = materialOverride;
						}
						else
						{	/* Default */
							MaterialAnimation.TableName[indexTable] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_ANIMATION_SS6PU, nameOutputAssetFolderBase)
																		+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_ANIMATION_SS6PU, NameFileBody, informationSSPJ.NameFileBody) + "_" + operationTarget.ToString()
																		+ NameExtentionMaterial;
							MaterialAnimation.TableData[indexTable] = AssetDatabase.LoadAssetAtPath<Material>(MaterialAnimation.TableName[indexTable]);
						}

						return(true);
					}

					public bool AssetNameDecideMaterialEffectSS6PU(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																	LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																	string nameOutputAssetFolderBase,
																	Library_SpriteStudio6.KindOperationBlendEffect operationTarget,
																	Material materialOverride
																)
					{
						int indexTable = (int)operationTarget;
						if(null != materialOverride)
						{	/* Specified */
							MaterialEffect.TableName[indexTable] = AssetDatabase.GetAssetPath(materialOverride);
							MaterialEffect.TableData[indexTable] = materialOverride;
						}
						else
						{	/* Default */
							MaterialEffect.TableName[indexTable] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_EFFECT_SS6PU, nameOutputAssetFolderBase)
																	+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_EFFECT_SS6PU, NameFileBody, informationSSPJ.NameFileBody) + "_" + operationTarget.ToString()
																	+ NameExtentionMaterial;
							MaterialEffect.TableData[indexTable] = AssetDatabase.LoadAssetAtPath<Material>(MaterialEffect.TableName[indexTable]);
						}

						return(true);
					}

					public bool AssetCreateMaterialAnimationSS6PU(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																	LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																	Library_SpriteStudio6.KindOperationBlend operationTarget
																)
					{
						const string messageLogPrefix = "AssetCreate(Material-Animation)";
						int indexOperationTarget = (int)operationTarget;

						Material material = null;
						material = MaterialAnimation.TableData[indexOperationTarget];
						if(null == material)
						{
							material = new Material(Library_SpriteStudio6.Data.Shader.TableSprite[indexOperationTarget]);
							AssetDatabase.CreateAsset(material, MaterialAnimation.TableName[indexOperationTarget]);
						}

						material.mainTexture = PrefabTexture.TableData[0];
						EditorUtility.SetDirty(material);
						AssetDatabase.SaveAssets();

						return(true);

//					AssetCreateMaterialAnimationSS6PU_ErrorEnd:;
//						return(false);
					}

					public bool AssetCreateMaterialEffectSS6PU(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																Library_SpriteStudio6.KindOperationBlendEffect operationTarget
															)
					{
						const string messageLogPrefix = "AssetCreate(Material-Effect)";
						int indexOperationTarget = (int)operationTarget;

						Material material = null;
						material = MaterialEffect.TableData[indexOperationTarget];
						if(null == material)
						{
							material = new Material(Library_SpriteStudio6.Data.Shader.TableEffect[indexOperationTarget]);
							AssetDatabase.CreateAsset(material, MaterialEffect.TableName[indexOperationTarget]);
						}

						material.mainTexture = PrefabTexture.TableData[0];
						EditorUtility.SetDirty(material);
						AssetDatabase.SaveAssets();

						return(true);

//					AssetCreateMaterialEffectSS6PU_ErrorEnd:;
//						return(false);
					}

					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					private const string NameExtentionMaterial = ".mat";
					#endregion Enums & Constants
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
