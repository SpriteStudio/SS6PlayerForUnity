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
				const string messageLogPrefix = "Parse SSCE";
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
					else
					{
						int count = Data.TableCell.Length;
						for(int i=0; i<count; i++)
						{
							if(name == Data.TableCell[i].Name)
							{
								return(i);
							}
						}
					}
					return(-1);
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
					public LibraryEditor_SpriteStudio6.Import.Assets<Material> MaterialAnimationSS6PU;
					public LibraryEditor_SpriteStudio6.Import.Assets<Material> MaterialEffectSS6PU;
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
						MaterialAnimationSS6PU.CleanUp();
						MaterialAnimationSS6PU.BootUp((int)Library_SpriteStudio6.KindOperationBlend.TERMINATOR);
						MaterialEffectSS6PU.CleanUp();
						MaterialEffectSS6PU.BootUp((int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR);
					}

					public string FileNameGetFullPath()
					{
						return(NameDirectory + NameFileBody + NameFileExtension);
					}
					#endregion Functions
				}
				#endregion Classes, Structs & Interfaces
			}

			public static partial class ModeSS6PU
			{
				/* MEMO: Originally functions that should be defined in each information class. */
				/*       However, confusion tends to occur with mode increases.                 */
				/*       ... Compromised way.                                                   */

				/* ----------------------------------------------- Functions */
				#region Functions
				public static bool AssetNameDecideTexture(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
															LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
															Information.Texture informationTexture,
															string nameOutputAssetFolderBase,
															Texture2D textureOverride
														)
				{	/* MEMO: In each import mode, texture is shared. */
					if(null != textureOverride)
					{	/* Specified */
						informationTexture.PrefabTexture.TableName[0] = AssetDatabase.GetAssetPath(textureOverride);
						informationTexture.PrefabTexture.TableData[0] = textureOverride;
					}
					else
					{	/* Default */
						informationTexture.PrefabTexture.TableName[0] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.TEXTURE, nameOutputAssetFolderBase)
																		+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.TEXTURE, informationTexture.NameFileBody, informationSSPJ.NameFileBody)
																		+ informationTexture.NameFileExtension;
						/* MEMO: Can not detect Platform-Dependent Textures (such as DDS and PVR). */
						informationTexture.PrefabTexture.TableData[0] = AssetDatabase.LoadAssetAtPath<Texture2D>(informationTexture.PrefabTexture.TableName[0]);
					}

					return(true);

//				AssetNameDecideTexture_ErrorEnd:;
//					return(false);
				}

				public static bool AssetCreateTexture(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
														LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
														Information.Texture informationTexture
													)
				{	/* MEMO: In each import mode, texture is shared. */
					const string messageLogPrefix = "Create Asset(Texture)";

					/* Copy into Asset */
					string namePathAssetNative = LibraryEditor_SpriteStudio6.Utility.File.PathGetAssetNative(informationTexture.PrefabTexture.TableName[0]);
					LibraryEditor_SpriteStudio6.Utility.File.FileCopyToAsset(	namePathAssetNative,
																				informationTexture.FileNameGetFullPath(),
																				true
																			);

					/* Set Texture-Importer */
					if(null == informationTexture.PrefabTexture.TableData[0])
					{
						AssetDatabase.ImportAsset(informationTexture.PrefabTexture.TableName[0]);
						TextureImporter importer = TextureImporter.GetAtPath(informationTexture.PrefabTexture.TableName[0]) as TextureImporter;
						if(null != importer)
						{
							importer.anisoLevel = 1;
							importer.borderMipmap = false;
							importer.convertToNormalmap = false;
							importer.fadeout = false;
							switch(informationTexture.Filter)
							{
								case Library_SpriteStudio6.Data.Texture.KindFilter.NEAREST:
									importer.filterMode = FilterMode.Point;
									break;

								case Library_SpriteStudio6.Data.Texture.KindFilter.LINEAR:
									importer.filterMode = FilterMode.Bilinear;
									break;

								default:
									/* MEMO: Errors and warnings have already been done and values have been revised. Therefore, will not come here. */
									goto AssetCreateTexture_ErrorEnd;
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

							switch(informationTexture.Wrap)
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
									goto AssetCreateTexture_ErrorEnd;
							}
							AssetDatabase.ImportAsset(informationTexture.PrefabTexture.TableName[0], ImportAssetOptions.ForceUpdate);
						}
					}
					AssetDatabase.SaveAssets();

					informationTexture.PrefabTexture.TableData[0] = AssetDatabase.LoadAssetAtPath(informationTexture.PrefabTexture.TableName[0], typeof(Texture2D)) as Texture2D;
					if((0 >= informationTexture.SizeX) || (0 >= informationTexture.SizeY))
					{	/* Only when texture size can not be get from SSCE */
						informationTexture.SizeX = informationTexture.PrefabTexture.TableData[0].width;
						informationTexture.SizeY = informationTexture.PrefabTexture.TableData[0].height;
					}

					return(true);

				AssetCreateTexture_ErrorEnd:;
					return(false);
				}

				public static bool AssetNameDecideMaterialAnimation(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																		LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																		Information.Texture informationTexture,
																		string nameOutputAssetFolderBase,
																		Library_SpriteStudio6.KindOperationBlend operationTarget,
																		Material materialOverride
																	)
				{
					int indexTable = (int)operationTarget;
					if(null != materialOverride)
					{	/* Specified */
						informationTexture.MaterialAnimationSS6PU.TableName[indexTable] = AssetDatabase.GetAssetPath(materialOverride);
						informationTexture.MaterialAnimationSS6PU.TableData[indexTable] = materialOverride;
					}
					else
					{	/* Default */
						informationTexture.MaterialAnimationSS6PU.TableName[indexTable] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_ANIMATION_SS6PU, nameOutputAssetFolderBase)
																							+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_ANIMATION_SS6PU, informationTexture.NameFileBody, informationSSPJ.NameFileBody) + "_" + operationTarget.ToString()
																							+ LibraryEditor_SpriteStudio6.Import.NameExtentionMaterial;
						informationTexture.MaterialAnimationSS6PU.TableData[indexTable] = AssetDatabase.LoadAssetAtPath<Material>(informationTexture.MaterialAnimationSS6PU.TableName[indexTable]);
					}

					return(true);

//				AssetNameDecideMaterialAnimation_ErrorEnd:;
//					return(false);
				}

				public static bool AssetNameDecideMaterialEffect(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																	LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																	Information.Texture informationTexture,
																	string nameOutputAssetFolderBase,
																	Library_SpriteStudio6.KindOperationBlendEffect operationTarget,
																	Material materialOverride
																)
				{
					int indexTable = (int)operationTarget;
					if(null != materialOverride)
					{	/* Specified */
						informationTexture.MaterialEffectSS6PU.TableName[indexTable] = AssetDatabase.GetAssetPath(materialOverride);
						informationTexture.MaterialEffectSS6PU.TableData[indexTable] = materialOverride;
					}
					else
					{	/* Default */
						informationTexture.MaterialEffectSS6PU.TableName[indexTable] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_EFFECT_SS6PU, nameOutputAssetFolderBase)
																						+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.MATERIAL_EFFECT_SS6PU, informationTexture.NameFileBody, informationSSPJ.NameFileBody) + "_" + operationTarget.ToString()
																						+ LibraryEditor_SpriteStudio6.Import.NameExtentionMaterial;
						informationTexture.MaterialEffectSS6PU.TableData[indexTable] = AssetDatabase.LoadAssetAtPath<Material>(informationTexture.MaterialEffectSS6PU.TableName[indexTable]);
					}

					return(true);

//				AssetNameDecideMaterialEffect_ErrorEnd:;
//					return(false);
				}

				public static bool AssetCreateMaterialAnimation(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																	LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																	Information.Texture informationTexture,
																	Library_SpriteStudio6.KindOperationBlend operationTarget
																)
				{
					const string messageLogPrefix = "Create Asset(Material-Animation)";
					int indexOperationTarget = (int)operationTarget;

					Material material = null;
					material = informationTexture.MaterialAnimationSS6PU.TableData[indexOperationTarget];
					if(null == material)
					{
						material = new Material(Library_SpriteStudio6.Data.Shader.TableSprite[indexOperationTarget]);
						AssetDatabase.CreateAsset(material, informationTexture.MaterialAnimationSS6PU.TableName[indexOperationTarget]);
					}

					material.mainTexture = informationTexture.PrefabTexture.TableData[0];
					EditorUtility.SetDirty(material);
					AssetDatabase.SaveAssets();

					return(true);

//				AssetCreateMaterialAnimation_ErrorEnd:;
//					return(false);
				}

				public static bool AssetCreateMaterialEffect(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																Information.Texture informationTexture,
																Library_SpriteStudio6.KindOperationBlendEffect operationTarget
															)
				{
					const string messageLogPrefix = "Create Asset(Material-Effect)";
					int indexOperationTarget = (int)operationTarget;

					Material material = null;
					material = informationTexture.MaterialEffectSS6PU.TableData[indexOperationTarget];
					if(null == material)
					{
						material = new Material(Library_SpriteStudio6.Data.Shader.TableEffect[indexOperationTarget]);
						AssetDatabase.CreateAsset(material, informationTexture.MaterialEffectSS6PU.TableName[indexOperationTarget]);
					}

					material.mainTexture = informationTexture.PrefabTexture.TableData[0];
					EditorUtility.SetDirty(material);
					AssetDatabase.SaveAssets();

					return(true);

//				AssetCreateMaterialEffect_ErrorEnd:;
//					return(false);
				}

				public static bool ConvertCellMap(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
													LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
													LibraryEditor_SpriteStudio6.Import.SSCE.Information informationSSCE
												)
				{	/* Convert-SS6PU Pass-1 ... Transfer necessary data from the temporary. */
					LibraryEditor_SpriteStudio6.Import.SSCE.Information.Texture informationTexture = null;	/* ÅhUnityEngine.TextureÅh and my "Texture", class-names are conflict unless fully-qualified. */
					if(0 <= informationSSCE.IndexTexture)
					{
						informationTexture = informationSSPJ.TableInformationTexture[informationSSCE.IndexTexture];
						int countCell = informationSSCE.TableCell.Length;

						informationSSCE.Data.Name = string.Copy(informationSSCE.NameFileBody);
						informationSSCE.Data.SizeOriginal.x = (float)informationTexture.SizeX;
						informationSSCE.Data.SizeOriginal.y = (float)informationTexture.SizeY;
						informationSSCE.Data.TableCell = new Library_SpriteStudio6.Data.CellMap.Cell[countCell];

						Library_SpriteStudio6.Data.CellMap.Cell informationCell = new Library_SpriteStudio6.Data.CellMap.Cell();
						for(int i=0; i<countCell; i++)
						{
							informationCell.CleanUp();
							informationCell.Name = string.Copy(informationSSCE.TableCell[i].Name);
							informationCell.Rectangle = informationSSCE.TableCell[i].Area;
							informationCell.Pivot = informationSSCE.TableCell[i].Pivot;

							informationSSCE.Data.TableCell[i] = informationCell;
						}
						informationSSCE.TableCell = null;	/* Purge WorkArea */
					}
					return(true);

//				ConvertSS6PU_ErrorEnd:;
//					return(false);
				}

				public static bool ConverCellMaptPixelTrimTransparent(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																		LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																		LibraryEditor_SpriteStudio6.Import.SSCE.Information informationSSCE
																	)
				{	/* Convert-SS6PU Pass-2 */
					return(false);

//				ConverCellMaptPixelTrimTransparent_ErrorEnd:;
//					return(false);
				}
				#endregion Functions
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
