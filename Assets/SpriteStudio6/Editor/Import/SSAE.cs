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
		public static partial class SSAE
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static Information Parse(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
												string nameFile,
												LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
											)
			{
				const string messageLogPrefix = "Parse SSAE";
				Information informationSSAE = null;

				/* ".ssce" Load */
				if(false == System.IO.File.Exists(nameFile))
				{
					LogError(messageLogPrefix, "File Not Found", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				System.Xml.XmlDocument xmlSSAE = new System.Xml.XmlDocument();
				xmlSSAE.Load(nameFile);

				/* Check Version */
				System.Xml.XmlNode nodeRoot = xmlSSAE.FirstChild;
				nodeRoot = nodeRoot.NextSibling;
				KindVersion version = (KindVersion)(LibraryEditor_SpriteStudio6.Utility.XML.VersionGet(nodeRoot, "SpriteStudioAnimePack", (int)KindVersion.ERROR, true));
				switch(version)
				{
					case KindVersion.ERROR:
						LogError(messageLogPrefix, "Version Invalid", nameFile, informationSSPJ);
						goto Parse_ErrorEnd;

					case KindVersion.CODE_000100:
					case KindVersion.CODE_010000:
					case KindVersion.CODE_010001:
					case KindVersion.CODE_010002:
					case KindVersion.CODE_010200:
					case KindVersion.CODE_010201:
					case KindVersion.CODE_010202:
						break;

					default:
						if(KindVersion.TARGET_EARLIEST > version)
						{
							version = KindVersion.TARGET_EARLIEST;
							if(true == setting.CheckVersion.FlagInvalidSSAE)
							{
								LogWarning(messageLogPrefix, "Version Too Early", nameFile, informationSSPJ);
							}
						}
						else
						{
							version = KindVersion.TARGET_LATEST;
							if(true == setting.CheckVersion.FlagInvalidSSAE)
							{
								LogWarning(messageLogPrefix, "Version Unknown", nameFile, informationSSPJ);
							}
						}
						break;
				}

				/* Create Information */
				informationSSAE = new Information();
				if(null == informationSSAE)
				{
					LogError(messageLogPrefix, "Not Enough Memory", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				informationSSAE.CleanUp();
				informationSSAE.Version = version;
				informationSSAE.FormatSS6PU = Library_SpriteStudio6.Data.Animation.Parts.KindFormat.PLAIN;

				/* Get Base-Directories */
				LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out informationSSAE.NameDirectory, out informationSSAE.NameFileBody, out informationSSAE.NameFileExtension, nameFile);
				informationSSAE.NameDirectory += "/";

				/* Decode Tags */
				System.Xml.NameTable nodeNameSpace = new System.Xml.NameTable();
				System.Xml.XmlNamespaceManager managerNameSpace = new System.Xml.XmlNamespaceManager(nodeNameSpace);
				System.Xml.XmlNodeList listNode= null;

				/* Parts-Data Get */
				listNode = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "Model/partList/value", managerNameSpace);
				if(null == listNode)
				{
					LogError(messageLogPrefix, "PartList-Node Not Found", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				int countParts = listNode.Count;
				informationSSAE.TableParts = new Information.Parts[countParts];
				if(null == informationSSAE.TableParts)
				{
					LogError(messageLogPrefix, "Not Enough Memory (Parts-Data WorkArea)", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				foreach(System.Xml.XmlNode nodeAnimation in listNode)
				{
					/* Part-Data Get */
					int indexParts = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAnimation, "arrayIndex", managerNameSpace));
					informationSSAE.TableParts[indexParts] = ParseParts(	ref setting,
																			informationSSPJ,
																			nodeAnimation,
																			managerNameSpace,
																			informationSSAE,
																			indexParts,
																			nameFile
																		);
					if(null == informationSSAE.TableParts[indexParts])
					{
						goto Parse_ErrorEnd;
					}
				}
				for(int i=0; i<countParts; i++)
				{
					/* Fix child-parts' index table */
					informationSSAE.TableParts[i].Data.TableIDChild = informationSSAE.TableParts[i].ListIndexPartsChild.ToArray();
					informationSSAE.TableParts[i].ListIndexPartsChild.Clear();
					informationSSAE.TableParts[i].ListIndexPartsChild = null;

					/* Fix inheritance kind */
					/* MEMO: Parent-part is always fixed earlier. */
					int indexPartsParent = -1;
					switch(informationSSAE.TableParts[i].Inheritance)
					{
						case Information.Parts.KindInheritance.PARENT:
							indexPartsParent = informationSSAE.TableParts[i].Data.IDParent;
							if(0 <= indexPartsParent)
							{
								informationSSAE.TableParts[i].Inheritance = Information.Parts.KindInheritance.SELF;
								informationSSAE.TableParts[i].FlagInheritance = informationSSAE.TableParts[indexPartsParent].FlagInheritance;
							}
							break;
						case Information.Parts.KindInheritance.SELF:
							break;

						default:
							goto case Information.Parts.KindInheritance.PARENT;
					}
				}

				/* Solve Referenced-CellMaps' index */
				listNode = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "cellmapNames/value", managerNameSpace);
				if(null == listNode)
				{
					informationSSAE.TableIndexCellMap = null;
				}
				else
				{
					int countCellMap = listNode.Count;
					int indexCellMap = 0;
					string nameCellMap = "";

					informationSSAE.TableIndexCellMap = new int[countCellMap];
					for(int i=0; i<countCellMap; i++)
					{
						informationSSAE.TableIndexCellMap[i] = -1;
					}
					foreach(System.Xml.XmlNode nodeCellMapName in listNode)
					{
						nameCellMap = nodeCellMapName.InnerText;
						nameCellMap = informationSSPJ.PathGetAbsolute(nameCellMap, LibraryEditor_SpriteStudio6.Import.KindFile.SSCE);

						informationSSAE.TableIndexCellMap[indexCellMap] = informationSSPJ.IndexGetFileName(informationSSPJ.TableNameSSCE, nameCellMap);
						if(-1 == informationSSAE.TableIndexCellMap[indexCellMap])
						{
							LogError(messageLogPrefix, "CellMap Not Found", nameFile, informationSSPJ);
							goto Parse_ErrorEnd;
						}

						indexCellMap++;
					}
				}

				/* Animations (& Parts' Key-Frames) Get */
				listNode = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "animeList/anime", managerNameSpace);
				if(null == listNode)
				{
					LogError(messageLogPrefix, "AnimationList-Node Not Found", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				informationSSAE.TableAnimation = new Information.Animation[listNode.Count];
				if(null == informationSSAE.TableAnimation)
				{
					LogError(messageLogPrefix, "Not Enough Memory (Animation-Data WorkArea)", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}

				int indexAnimation = 0;
				foreach(System.Xml.XmlNode nodeAnimation in listNode)
				{
					/* Animation (& Parts' Key-Frames) Get */
					informationSSAE.TableAnimation[indexAnimation] = ParseAnimation(	ref setting,
																						informationSSPJ,
																						nodeAnimation,
																						managerNameSpace,
																						informationSSAE,
																						indexAnimation,
																						nameFile
																					);
					if(null == informationSSAE.TableAnimation[indexAnimation])
					{
						goto Parse_ErrorEnd;
					}

					indexAnimation++;
				}

				/* Set secondary parameters */
				int countAnimation = informationSSAE.TableAnimation.Length;
				for(int i=0; i<countAnimation; i++)
				{
					/* Set Part-Status */
					/* MEMO: Execute before "DrawOrderCreate". */
					informationSSAE.TableAnimation[i].StatusSetParts(informationSSPJ, informationSSAE);

					/* Set Draw-Order */
					informationSSAE.TableAnimation[i].DrawOrderCreate(informationSSPJ, informationSSAE);
				}

				return(informationSSAE);

			Parse_ErrorEnd:;
				return(null);
			}

			private static Information.Parts ParseParts(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
															LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
															System.Xml.XmlNode nodeParts,
															System.Xml.XmlNamespaceManager managerNameSpace,
															Information informationSSAE,
															int indexParts,
															string nameFileSSAE
														)
			{
				const string messageLogPrefix = "Parse SSAE(Parts)";

				Information.Parts informationParts = new Information.Parts();
				if(null == informationParts)
				{
					LogError(messageLogPrefix, "Not Enough Memory (Parts WorkArea) Parts[" + indexParts.ToString() + "]", nameFileSSAE, informationSSPJ);
					goto ParseParts_ErrorEnd;
				}
				informationParts.CleanUp();

				/* Get Base Datas */
				string valueText = "";

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "name", managerNameSpace);
				informationParts.Data.Name = string.Copy(valueText);

				informationParts.Data.ID = indexParts;

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "parentIndex", managerNameSpace);
				informationParts.Data.IDParent = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
				if(0 <= informationParts.Data.IDParent)
				{
					Information.Parts informationPartsParent = informationSSAE.TableParts[informationParts.Data.IDParent];
					informationPartsParent.ListIndexPartsChild.Add(informationParts.Data.ID);
				}

				/* Get Parts-Type */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "type", managerNameSpace);
				switch(valueText)
				{
					case "null":
						informationParts.Data.Feature = (0 == informationParts.Data.ID) ? Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT : Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL;
						break;

					case "normal":
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL;
						break;

					case "instance":
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE;
						break;

					case "effect":
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT;
						break;

					default:
						LogWarning(messageLogPrefix, "Unknown Parts-Type \"" + valueText + "\" Parts[" + indexParts.ToString() + "]", nameFileSSAE, informationSSPJ);
						goto case "null";
				}

				/* Get "Collision" Datas */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "boundsType", managerNameSpace);
				switch(valueText)
				{
					case "none":
						informationParts.Data.ShapeCollision = Library_SpriteStudio6.Data.Parts.Animation.KindCollision.NON;
						informationParts.Data.SizeCollisionZ = 0.0f;
						break;

					case "quad":
						informationParts.Data.ShapeCollision = Library_SpriteStudio6.Data.Parts.Animation.KindCollision.SQUARE;
						informationParts.Data.SizeCollisionZ = setting.Collider.SizeZ;
						break;

					case "aabb":
						informationParts.Data.ShapeCollision = Library_SpriteStudio6.Data.Parts.Animation.KindCollision.AABB;
						informationParts.Data.SizeCollisionZ = setting.Collider.SizeZ;
						break;

					case "circle":
						informationParts.Data.ShapeCollision = Library_SpriteStudio6.Data.Parts.Animation.KindCollision.CIRCLE;
						informationParts.Data.SizeCollisionZ = setting.Collider.SizeZ;
						break;

					case "circle_smin":
						informationParts.Data.ShapeCollision = Library_SpriteStudio6.Data.Parts.Animation.KindCollision.CIRCLE_SCALEMINIMUM;
						informationParts.Data.SizeCollisionZ = setting.Collider.SizeZ;
						break;

					case "circle_smax":
						informationParts.Data.ShapeCollision = Library_SpriteStudio6.Data.Parts.Animation.KindCollision.CIRCLE_SCALEMAXIMUM;
						informationParts.Data.SizeCollisionZ = setting.Collider.SizeZ;
						break;

					default:
						LogWarning(messageLogPrefix, "Unknown Collision Kind \"" + valueText + "\" Parts[" + indexParts.ToString() + "]", nameFileSSAE, informationSSPJ);
						goto case "none";
				}

				/* Get "Inheritance" Datas */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "inheritType", managerNameSpace);
				switch(valueText)
				{
					case "parent":
						{
							switch(informationSSAE.Version)
							{
								case KindVersion.CODE_010000:
								case KindVersion.CODE_010001:
								case KindVersion.CODE_010002:
								case KindVersion.CODE_010200:
								case KindVersion.CODE_010201:
								case KindVersion.CODE_010202:	/* EffectPartsCheck? */
									if(0 == informationParts.Data.ID)
									{
										informationParts.Inheritance = Information.Parts.KindInheritance.SELF;
										informationParts.FlagInheritance = Information.Parts.FlagBitInheritance.PRESET;
									}
									else
									{
										informationParts.Inheritance = Information.Parts.KindInheritance.PARENT;
										informationParts.FlagInheritance = Information.Parts.FlagBitInheritance.CLEAR;
									}
									break;
							}
						}
						break;

					case "self":
						{
							switch(informationSSAE.Version)
							{
								case KindVersion.CODE_010000:
								case KindVersion.CODE_010001:
									{
										/* MEMO: Default-Value: 0(true) */
										/*       Attributes'-Tag exists when Value is 0(false). */
										informationParts.Inheritance = Information.Parts.KindInheritance.SELF;
										informationParts.FlagInheritance = Information.Parts.FlagBitInheritance.CLEAR;
	
										System.Xml.XmlNode nodeAttribute = null;
										nodeAttribute = LibraryEditor_SpriteStudio6.Utility.XML.NodeGet(nodeParts, "ineheritRates/ALPH", managerNameSpace);
										if(null == nodeAttribute)
										{
											informationParts.FlagInheritance |= Information.Parts.FlagBitInheritance.OPACITY_RATE;
										}
	
										nodeAttribute = LibraryEditor_SpriteStudio6.Utility.XML.NodeGet(nodeParts, "ineheritRates/FLPH", managerNameSpace);
										if(null == nodeAttribute)
										{
											informationParts.FlagInheritance |= Information.Parts.FlagBitInheritance.FLIP_X;
										}
	
										nodeAttribute = LibraryEditor_SpriteStudio6.Utility.XML.NodeGet(nodeParts, "ineheritRates/FLPV", managerNameSpace);
										if(null == nodeAttribute)
										{
											informationParts.FlagInheritance |= Information.Parts.FlagBitInheritance.FLIP_Y;
										}
	
										nodeAttribute = LibraryEditor_SpriteStudio6.Utility.XML.NodeGet(nodeParts, "ineheritRates/HIDE", managerNameSpace);
										if(null == nodeAttribute)
										{
											informationParts.FlagInheritance |= Information.Parts.FlagBitInheritance.SHOW_HIDE;
										}
									}
									break;

								case KindVersion.CODE_010002:
								case KindVersion.CODE_010200:
								case KindVersion.CODE_010201:
								case KindVersion.CODE_010202:
									{
										/* MEMO: Attributes'-Tag always exists. */
										bool valueBool = false;
	
										informationParts.Inheritance = Information.Parts.KindInheritance.SELF;
										informationParts.FlagInheritance = Information.Parts.FlagBitInheritance.PRESET;
										informationParts.FlagInheritance |= Information.Parts.FlagBitInheritance.FLIP_X;
										informationParts.FlagInheritance |= Information.Parts.FlagBitInheritance.FLIP_Y;
										informationParts.FlagInheritance |= Information.Parts.FlagBitInheritance.SHOW_HIDE;

										valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "ineheritRates/ALPH", managerNameSpace);
										if(null != valueText)
										{
											valueBool = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
											if(false == valueBool)
											{
												informationParts.FlagInheritance &= ~Information.Parts.FlagBitInheritance.OPACITY_RATE;
											}
										}
	
										valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "ineheritRates/FLPH", managerNameSpace);
										if(null != valueText)
										{
											valueBool = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
											if(false == valueBool)
											{
												informationParts.FlagInheritance &= ~Information.Parts.FlagBitInheritance.FLIP_X;
											}
										}
	
										valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "ineheritRates/FLPV", managerNameSpace);
										if(null != valueText)
										{
											valueBool = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
											if(false == valueBool)
											{
												informationParts.FlagInheritance &= ~Information.Parts.FlagBitInheritance.FLIP_Y;
											}
										}
	
										valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "ineheritRates/HIDE", managerNameSpace);
										if(null != valueText)
										{
											valueBool = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
											if(false == valueBool)
											{
												informationParts.FlagInheritance &= ~Information.Parts.FlagBitInheritance.SHOW_HIDE;
											}
										}
									}
									break;
							}
						}
						break;

					default:
						LogWarning(messageLogPrefix, "Unknown Inheritance Kind \"" + valueText + "\" Parts[" + indexParts.ToString() + "]", nameFileSSAE, informationSSPJ);
						goto case "parent";
				}

				/* Get Target-Blending */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "alphaBlendType", managerNameSpace);
				switch(valueText)
				{
					case "mix":
						informationParts.Data.OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.MIX;
						break;

					case "mul":
						informationParts.Data.OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.MUL;
						break;

					case "add":
						informationParts.Data.OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.ADD;
						break;

					case "sub":
						informationParts.Data.OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.SUB;
						break;

					default:
						LogWarning(messageLogPrefix, "Unknown Alpha-Blend Kind \"" + valueText + "\" Parts[" + indexParts.ToString() + "]", nameFileSSAE, informationSSPJ);
						goto case "mix";
				}

				/* UnderControl Data Get */
				/* MEMO: Type of data under control is determined uniquely according to Part-Type. (Mutually exclusive) */
				switch(informationParts.Data.Feature)
				{
					case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
						/* Instance-Animation Datas Get */
						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "refAnimePack", managerNameSpace);
						if(null != valueText)
						{
							informationParts.NameUnderControl = valueText;

							valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "refAnime", managerNameSpace);
							informationParts.Data.NameAnimationUnderControl = (null != valueText) ? string.Copy(valueText) : "";
						}
						break;

					case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
						/* Get Effect Datas */
						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "refEffectName", managerNameSpace);
						if(null != valueText)
						{
							/* MEMO: Tag present but value may be empty. */
							if(false == string.IsNullOrEmpty(valueText))
							{
								informationParts.NameUnderControl = string.Copy(valueText);
							}
						}
						break;

					default:
						break;
				}

				/* Get Color-Label */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "colorLabel", managerNameSpace);
				if(null == valueText)
				{
					informationParts.Data.ColorLabel = Library_SpriteStudio6.Data.Parts.Animation.KindColorLabel.NON;
				}
				else
				{
					switch(valueText)
					{
						case "Red":
							informationParts.Data.ColorLabel = Library_SpriteStudio6.Data.Parts.Animation.KindColorLabel.RED;
							break;

						case "Orange":
							informationParts.Data.ColorLabel = Library_SpriteStudio6.Data.Parts.Animation.KindColorLabel.ORANGE;
							break;

						case "Yellow":
							informationParts.Data.ColorLabel = Library_SpriteStudio6.Data.Parts.Animation.KindColorLabel.YELLOW;
							break;

						case "Green":
							informationParts.Data.ColorLabel = Library_SpriteStudio6.Data.Parts.Animation.KindColorLabel.GREEN;
							break;

						case "Blue":
							informationParts.Data.ColorLabel = Library_SpriteStudio6.Data.Parts.Animation.KindColorLabel.BLUE;
							break;

						case "Violet":
							informationParts.Data.ColorLabel = Library_SpriteStudio6.Data.Parts.Animation.KindColorLabel.RED;
							break;

						case "Gray":
							informationParts.Data.ColorLabel = Library_SpriteStudio6.Data.Parts.Animation.KindColorLabel.GRAY;
							break;

						default:
							LogWarning(messageLogPrefix, "Unknown Color-Label Kind \"" + valueText + "\" Parts[" + indexParts.ToString() + "]", nameFileSSAE, informationSSPJ);
							informationParts.Data.ColorLabel = Library_SpriteStudio6.Data.Parts.Animation.KindColorLabel.NON;
							break;
					}
				}

				return(informationParts);

			ParseParts_ErrorEnd:;
				return(null);
			}

			private static Information.Animation ParseAnimation(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																	LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																	System.Xml.XmlNode nodeAnimation,
																	System.Xml.XmlNamespaceManager managerNameSpace,
																	Information informationSSAE,
																	int indexAnimation,
																	string nameFileSSAE
																)
			{
				const string messageLogPrefix = "Parse SSAE(Animation)";

				Information.Animation informationAnimation = new Information.Animation();
				if(null == informationAnimation)
				{
					LogError(messageLogPrefix, "Not Enough Memory (Parts WorkArea) Animation[" + indexAnimation.ToString() + "]", nameFileSSAE, informationSSPJ);
					goto ParseAnimation_ErrorEnd;
				}
				informationAnimation.CleanUp();

				/* Get Base Datas */
				string valueText;
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAnimation, "name", managerNameSpace);
				informationAnimation.Data.Name = string.Copy(valueText);

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAnimation, "settings/fps", managerNameSpace);
				informationAnimation.Data.FramePerSecond = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAnimation, "settings/frameCount", managerNameSpace);
				informationAnimation.Data.CountFrame = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAnimation, "settings/sortMode", managerNameSpace);
				switch(valueText)
				{
					case "prio":
						informationAnimation.ModeSort = Information.Animation.KindModeSort.PRIORITY;
						break;
					case "z":
						informationAnimation.ModeSort = Information.Animation.KindModeSort.POSITION_Z;
						break;
					default:
						goto case "prio";
				}

				/* Get Labels */
				List<Library_SpriteStudio6.Data.Animation.Label> listLabel = new List<Library_SpriteStudio6.Data.Animation.Label>();
				if(null == listLabel)
				{
					LogError(messageLogPrefix, "Not Enough Memory (Animation-Label WorkArea) Animation[" + indexAnimation.ToString() + "]", nameFileSSAE, informationSSPJ);
					goto ParseAnimation_ErrorEnd;
				}
				listLabel.Clear();

				Library_SpriteStudio6.Data.Animation.Label label = new Library_SpriteStudio6.Data.Animation.Label();
				System.Xml.XmlNodeList nodeListLabel = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeAnimation, "labels/value", managerNameSpace);
				foreach(System.Xml.XmlNode nodeLabel in nodeListLabel)
				{
					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeLabel, "name", managerNameSpace);
					if(0 > Library_SpriteStudio6.Data.Animation.Label.NameCheckReserved(valueText))
					{
						label.CleanUp();
						label.Name = string.Copy(valueText);

						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeLabel, "time", managerNameSpace);
						label.Frame = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);

						listLabel.Add(label);
					}
					else
					{
						LogWarning(messageLogPrefix, "Used reserved Label-Name \"" + valueText + "\" Animation[" + indexAnimation.ToString() + "]", nameFileSSAE, informationSSPJ);
					}
				}
				informationAnimation.Data.TableLabel = listLabel.ToArray();
				listLabel.Clear();
				listLabel = null;

				/* Get Key-Frames */
				int countParts = informationSSAE.TableParts.Length;
				informationAnimation.TableParts = new Information.Animation.Parts[countParts];
				if(null == informationAnimation.TableParts)
				{
					LogError(messageLogPrefix, "Not Enough Memory (Animation Part Data) Animation[" + indexAnimation.ToString() + "]", nameFileSSAE, informationSSPJ);
					goto ParseAnimation_ErrorEnd;
				}
				/* MEMO: All animation part information is created here. Because parts-animation that has no key-data are not recorded in SSAE. */
				for(int i=0; i<countParts; i++)
				{
					informationAnimation.TableParts[i] =new Information.Animation.Parts();
					if(null == informationAnimation.TableParts[i])
					{
						LogError(messageLogPrefix, "Not Enough Memory (Animation Attribute WorkArea) Animation-Name[" + indexAnimation.ToString() + "]", nameFileSSAE, informationSSPJ);
						goto ParseAnimation_ErrorEnd;
					}
					informationAnimation.TableParts[i].CleanUp();
					informationAnimation.TableParts[i].BootUp();
					informationAnimation.TableParts[i].StatusParts = Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED;
				}

				System.Xml.XmlNodeList nodeListAnimationParts = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeAnimation, "partAnimes/partAnime", managerNameSpace);
				if(null == nodeListAnimationParts)
				{
					LogError(messageLogPrefix, "PartAnimation Node Not Found Animation[" + indexAnimation.ToString() + "]", nameFileSSAE, informationSSPJ);
					goto ParseAnimation_ErrorEnd;
				}
				int indexParts = -1;
				foreach(System.Xml.XmlNode nodeAnimationPart in nodeListAnimationParts)
				{
					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAnimationPart, "partName", managerNameSpace);
					indexParts = informationSSAE.IndexGetParts(valueText);
					if(-1 == indexParts)
					{
						LogError(messageLogPrefix, "Part's Name Not Found \"" + valueText + "\" Animation[" + indexAnimation.ToString() + "]", nameFileSSAE, informationSSPJ);
						goto ParseAnimation_ErrorEnd;
					}

					System.Xml.XmlNode nodeAnimationAttributes = LibraryEditor_SpriteStudio6.Utility.XML.NodeGet(nodeAnimationPart, "attributes", managerNameSpace);
					if(false ==  ParseAnimationAttribute(	ref setting,
															informationSSPJ,
															nodeAnimationAttributes,
															managerNameSpace,
															informationSSAE,
															informationAnimation,
															indexParts,
															nameFileSSAE
														)
						)
					{
						goto ParseAnimation_ErrorEnd;
					}
				}

				return(informationAnimation);

			ParseAnimation_ErrorEnd:;
				return(null);
			}

			private static bool ParseAnimationAttribute(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
															LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
															System.Xml.XmlNode nodeAnimationAttributes,
															System.Xml.XmlNamespaceManager managerNameSpace,
															Information informationSSAE,
															Information.Animation informationAnimation,
															int indexParts,
															string nameFileSSAE
														)
			{
				const string messageLogPrefix = "Parse SSAE(Attributes)";

				Information.Animation.Parts informationAnimationParts = informationAnimation.TableParts[indexParts];

				/* Set Part's Status */
				/* MEMO: Set here at least "Not Used" flag. When this function is called, key-data exists in this part. */
				/*       Other flags are set in "Information.Animation.StatusSetParts".                                 */
				informationAnimationParts.StatusParts &= ~Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED;

				/* Set Inheritance */
				Information.Parts parts = informationSSAE.TableParts[indexParts];
				int indexPartsParent = parts.Data.IDParent;
				if(0 <= indexPartsParent)
				{
					Information.Animation.Parts informationAnimationPartsParent = informationAnimation.TableParts[indexPartsParent];
					if(0 != (parts.FlagInheritance & Information.Parts.FlagBitInheritance.OPACITY_RATE))
					{
						informationAnimationParts.RateOpacity.Parent = informationAnimationPartsParent.RateOpacity;
					}
					if(0 != (parts.FlagInheritance & Information.Parts.FlagBitInheritance.SHOW_HIDE))
					{
						informationAnimationParts.Hide.Parent = informationAnimationPartsParent.Hide;
					}
					if(0 != (parts.FlagInheritance & Information.Parts.FlagBitInheritance.FLIP_X))
					{
						informationAnimationParts.FlipX.Parent = informationAnimationPartsParent.FlipX;
					}
					if(0 != (parts.FlagInheritance & Information.Parts.FlagBitInheritance.FLIP_Y))
					{
						informationAnimationParts.FlipY.Parent = informationAnimationPartsParent.FlipY;
					}
				}

				/* Get KeyFrame List */
				string tagText;
				string valueText;
				System.Xml.XmlNodeList listNodeAttribute = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeAnimationAttributes, "attribute", managerNameSpace);
				foreach(System.Xml.XmlNode nodeAttribute in listNodeAttribute)
				{
					/* Get Attribute-Tag */
					tagText = nodeAttribute.Attributes["tag"].Value;

					/* Get Key-Data List */
					System.Xml.XmlNodeList listNodeKey = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeAttribute, "key", managerNameSpace);
					if(null == listNodeKey)
					{
						LogWarning(messageLogPrefix, "Attribute \"" + tagText + "\" has no Key-Frame Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
						continue;
					}

					/* Get Key-Data */
					System.Xml.XmlNode nodeInterpolation = null;
					int frame = -1;
					Library_SpriteStudio6.Utility.Interpolation.KindFormula formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.NON;
					bool flagHasParameterCurve = false;
					float frameCurveStart = 0.0f;
					float valueCurveStart = 0.0f;
					float frameCurveEnd = 0.0f;
					float valueCurveEnd = 0.0f;
					string[] valueTextSplit = null;

					Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool attributeBool = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt attributeInt = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat attributeFloat = null;
					foreach(System.Xml.XmlNode nodeKey in listNodeKey)
					{
						/* Get Interpolation(Curve) Parameters */
						frame = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(nodeKey.Attributes["time"].Value);
						formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.NON;
						flagHasParameterCurve = false;
						frameCurveStart = 0.0f;
						valueCurveStart = 0.0f;
						frameCurveEnd = 0.0f;
						valueCurveEnd = 0.0f;
						nodeInterpolation = nodeKey.Attributes["ipType"];
						if(null != nodeInterpolation)
						{
							valueText = string.Copy(nodeInterpolation.Value);
							switch(valueText)
							{
								case "linear":
									formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.LINEAR;
									flagHasParameterCurve = false;
									break;

								case "hermite":
									formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.HERMITE;
									flagHasParameterCurve = true;
									break;

								case "bezier":
									formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.BEZIER;
									flagHasParameterCurve = true;
									break;

								case "acceleration":
									formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.ACCELERATE;
									flagHasParameterCurve = false;
									break;

								case "deceleration":
									formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.DECELERATE;
									flagHasParameterCurve = false;
									break;

								default:
									LogWarning(messageLogPrefix, "Unknown Interpolation \"" + valueText + "\" Frame[" + frame.ToString() + "] Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
									formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.NON;
									flagHasParameterCurve = false;
									break;
							}
							if(true == flagHasParameterCurve)
							{
								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "curve", managerNameSpace);
								if(null == valueText)
								{
									LogWarning(messageLogPrefix, "Interpolation \"" + valueText + "\" Parameter Missing Frame[" + frame.ToString() + "] Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
									formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.NON;
									flagHasParameterCurve = false;
									frameCurveStart = 0.0f;
									valueCurveStart = 0.0f;
									frameCurveEnd = 0.0f;
									valueCurveEnd = 0.0f;
								}
								else
								{
									valueTextSplit = valueText.Split(' ');
									frameCurveStart = (float)LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[0]);
									valueCurveStart = (float)LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[1]);
									frameCurveEnd = (float)LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[2]);
									valueCurveEnd = (float)LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[3]);
								}
							}
						}

						/* Get Attribute Data */
						switch(tagText)
						{
							/* Bool-Value Attributes */
							case "HIDE":
								attributeBool = informationAnimationParts.Hide;
								goto case "_ValueBool_";
							case "FLPH":
								attributeBool = informationAnimationParts.FlipX;
								goto case "_ValueBool_";
							case "FLPV":
								attributeBool = informationAnimationParts.FlipY;
								goto case "_ValueBool_";
							case "IFLH":
								attributeBool = informationAnimationParts.TextureFlipX;
								goto case "_ValueBool_";
							case "IFLV":
								attributeBool = informationAnimationParts.TextureFlipY;
								goto case "_ValueBool_";

							case "_ValueBool_":
								{
									Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool.KeyData data = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool.KeyData();

									/* Set Interpolation-Data */
									/* MEMO: Bool-Value can't have interpolation */
									data.Formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.NON;
									data.FrameCurveStart = 0.0f;
									data.ValueCurveStart = 0.0f;
									data.FrameCurveEnd = 0.0f;
									data.ValueCurveEnd = 0.0f;

									/* Set Body-Data */
									data.Frame = frame;

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value", managerNameSpace);
									data.Value = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);

									/* Add Key-Data */
									attributeBool.ListKey.Add(data);
								}
								break;

							/* Int-Value Attributes */
							case "PRIO":
								attributeInt = informationAnimationParts.Priority;
								goto case "_ValueInt_";

							case "_ValueInt_":
								{
									Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt.KeyData data = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt.KeyData();

									/* Set Interpolation-Data */
									data.Formula = formula;
									data.FrameCurveStart = frameCurveStart;
									data.ValueCurveStart = valueCurveStart;
									data.FrameCurveEnd = frameCurveEnd;
									data.ValueCurveEnd = valueCurveEnd;

									/* Set Body-Data */
									data.Frame = frame;

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value", managerNameSpace);
									data.Value = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);

									/* Add Key-Data */
									attributeInt.ListKey.Add(data);
								}
								break;

							/* Float-Value Attributes */
							case "POSX":
								attributeFloat = informationAnimationParts.PositionX;
								goto case "_ValueFloat_";
							case "POSY":
								attributeFloat = informationAnimationParts.PositionY;
								goto case "_ValueFloat_";
							case "POSZ":
								attributeFloat = informationAnimationParts.PositionZ;
								goto case "_ValueFloat_";
							case "ROTX":
								attributeFloat = informationAnimationParts.RotationX;
								goto case "_ValueFloat_";
							case "ROTY":
								attributeFloat = informationAnimationParts.RotationY;
								goto case "_ValueFloat_";
							case "ROTZ":
								attributeFloat = informationAnimationParts.RotationZ;
								goto case "_ValueFloat_";
							case "SCLX":
								attributeFloat = informationAnimationParts.ScalingX;
								goto case "_ValueFloat_";
							case "SCLY":
								attributeFloat = informationAnimationParts.ScalingY;
								goto case "_ValueFloat_";
							case "ALPH":
								attributeFloat = informationAnimationParts.RateOpacity;
								goto case "_ValueFloat_";
							case "PVTX":
								attributeFloat = informationAnimationParts.PivotOffsetX;
								goto case "_ValueFloat_";
							case "PVTY":
								attributeFloat = informationAnimationParts.PivotOffsetY;
								goto case "_ValueFloat_";
							case "ANCX":
								attributeFloat = informationAnimationParts.AnchorPositionX;
								goto case "_ValueFloat_";
							case "ANCY":
								attributeFloat = informationAnimationParts.AnchorPositionY;
								goto case "_ValueFloat_";
							case "SIZX":
								attributeFloat = informationAnimationParts.SizeForceX;
								goto case "_ValueFloat_";
							case "SIZY":
								attributeFloat = informationAnimationParts.SizeForceY;
								goto case "_ValueFloat_";
							case "UVTX":
								attributeFloat = informationAnimationParts.TexturePositionX;
								goto case "_ValueFloat_";
							case "UVTY":
								attributeFloat = informationAnimationParts.TexturePositionY;
								goto case "_ValueFloat_";
							case "UVRZ":
								attributeFloat = informationAnimationParts.TextureRotation;
								goto case "_ValueFloat_";
							case "UVSX":
								attributeFloat = informationAnimationParts.TextureScalingX;
								goto case "_ValueFloat_";
							case "UVSY":
								attributeFloat = informationAnimationParts.TextureScalingY;
								goto case "_ValueFloat_";
							case "BNDR":
								attributeFloat = informationAnimationParts.RadiusCollision;
								goto case "_ValueFloat_";

							case "_ValueFloat_":
								{
									Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat.KeyData data = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat.KeyData();

									/* Set Interpolation-Data */
									data.Formula = formula;
									data.FrameCurveStart = frameCurveStart;
									data.ValueCurveStart = valueCurveStart;
									data.FrameCurveEnd = frameCurveEnd;
									data.ValueCurveEnd = valueCurveEnd;

									/* Set Body-Data */
									data.Frame = frame;

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value", managerNameSpace);
									data.Value = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText);

									/* Add Key-Data */
									attributeFloat.ListKey.Add(data);
								}
								break;

							/* Uniquet-Value Attributes */
							case "CELL":
								{
									Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell.KeyData data = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell.KeyData();
									data.Value.CleanUp();

									/* Set Interpolation-Data */
									data.Formula = formula;
									data.FrameCurveStart = frameCurveStart;
									data.ValueCurveStart = valueCurveStart;
									data.FrameCurveEnd = frameCurveEnd;
									data.ValueCurveEnd = valueCurveEnd;

									/* Set Body-Data */
									data.Frame = frame;

									bool flagValidCell = false;
									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/mapId", managerNameSpace);
									if(null == valueText)
									{
										data.Value.IndexCellMap = -1;
										data.Value.IndexCell = -1;
									}
									else
									{
										int indexCellMap = informationSSAE.TableIndexCellMap[LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText)];
										data.Value.IndexCellMap = indexCellMap;

										valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/name", managerNameSpace);
										if(null == valueText)
										{
											data.Value.IndexCell = -1;
										}
										else
										{
											int indexCell = informationSSPJ.TableInformationSSCE[indexCellMap].IndexGetCell(valueText);
											data.Value.IndexCell = indexCell;
											if(0 <= indexCell)
											{
												flagValidCell = true;
											}
										}
									}
									if(false == flagValidCell)
									{
										LogWarning(messageLogPrefix, "Cell-Data Not Found Frame[" + frame.ToString() + "] Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
									}

									/* Add Key-Data */
									informationAnimationParts.Cell.ListKey.Add(data);
								}
								break;

							case "VCOL":
								{
									Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlend.KeyData data = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlend.KeyData();
									data.Value.CleanUp();
									data.Value.VertexColor = new Color[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2];
									data.Value.RatePixelAlpha = new float[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2];

									/* Set Interpolation-Data */
									data.Formula = formula;
									data.FrameCurveStart = frameCurveStart;
									data.ValueCurveStart = valueCurveStart;
									data.FrameCurveEnd = frameCurveEnd;
									data.ValueCurveEnd = valueCurveEnd;

									/* Set Body-Data */
									data.Frame = frame;

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/blendType", managerNameSpace);
									switch(valueText)
									{
										case "mix":
											data.Value.Operation = Library_SpriteStudio6.KindOperationBlend.MIX;
											break;

										case "mul":
											data.Value.Operation = Library_SpriteStudio6.KindOperationBlend.MUL;
											break;

										case "add":
											data.Value.Operation = Library_SpriteStudio6.KindOperationBlend.ADD;
											break;

										case "sub":
											data.Value.Operation = Library_SpriteStudio6.KindOperationBlend.SUB;
											break;

										default:
										LogWarning(messageLogPrefix, "Unknown ColorBlend-Operation \"" + valueText + "\" Frame[" + frame.ToString() + "] Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
											data.Value.Operation = Library_SpriteStudio6.KindOperationBlend.NON;
											break;
									}

									float colorA = 0.0f;
									float colorR = 0.0f;
									float colorG = 0.0f;
									float colorB = 0.0f;
									float ratePixel = 0.0f;
									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/target", managerNameSpace);
									switch(valueText)
									{
										case "whole":
											{
												data.Value.Bound = Library_SpriteStudio6.KindBoundBlend.OVERALL;

												ParseAnimationAttributeColorBlend(out colorA, out colorR, out colorG, out colorB, out ratePixel, nodeKey, "value/color", managerNameSpace);
												for(int i=0; i<(int)Library_SpriteStudio6.KindVertex.TERMINATOR2; i++)
												{
													data.Value.VertexColor[i].r = colorR;
													data.Value.VertexColor[i].g = colorG;
													data.Value.VertexColor[i].b = colorB;
													data.Value.VertexColor[i].a = colorA;
													data.Value.RatePixelAlpha[i] = ratePixel;
												}
											}
											break;

										case "vertex":
											{
												data.Value.Bound = Library_SpriteStudio6.KindBoundBlend.VERTEX;

												ParseAnimationAttributeColorBlend(out colorA, out colorR, out colorG, out colorB, out ratePixel, nodeKey, "value/LT", managerNameSpace);
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LU].r = colorR;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LU].g = colorG;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LU].b = colorB;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LU].a = colorA;
												data.Value.RatePixelAlpha[(int)Library_SpriteStudio6.KindVertex.LU] = ratePixel;

												ParseAnimationAttributeColorBlend(out colorA, out colorR, out colorG, out colorB, out ratePixel, nodeKey, "value/RT", managerNameSpace);
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RU].r = colorR;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RU].g = colorG;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RU].b = colorB;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RU].a = colorA;
												data.Value.RatePixelAlpha[(int)Library_SpriteStudio6.KindVertex.RU] = ratePixel;

												ParseAnimationAttributeColorBlend(out colorA, out colorR, out colorG, out colorB, out ratePixel, nodeKey, "value/RB", managerNameSpace);
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RD].r = colorR;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RD].g = colorG;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RD].b = colorB;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RD].a = colorA;
												data.Value.RatePixelAlpha[(int)Library_SpriteStudio6.KindVertex.RD] = ratePixel;

												ParseAnimationAttributeColorBlend(out colorA, out colorR, out colorG, out colorB, out ratePixel, nodeKey, "value/LB", managerNameSpace);
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LD].r = colorR;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LD].g = colorG;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LD].b = colorB;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LD].a = colorA;
												data.Value.RatePixelAlpha[(int)Library_SpriteStudio6.KindVertex.LD] = ratePixel;
											}
											break;

										default:
											{
												LogWarning(messageLogPrefix, "Unknown ColorBlend-Bound \"" + valueText + "\" Frame[" + frame.ToString() + "] Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
												data.Value.Bound = Library_SpriteStudio6.KindBoundBlend.OVERALL;
												for(int i=0; i<(int)Library_SpriteStudio6.KindVertex.TERMINATOR2; i++)
												{
													data.Value.VertexColor[i].r = 0.0f;
													data.Value.VertexColor[i].g = 0.0f;
													data.Value.VertexColor[i].b = 0.0f;
													data.Value.VertexColor[i].a = 0.0f;
													data.Value.RatePixelAlpha[i] = 1.0f;
												}
											}
											break;
									}

									/* Add Key-Data */
									informationAnimationParts.ColorBlend.ListKey.Add(data);
								}
								break;

							case "VERT":
								{
									Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection.KeyData data = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection.KeyData();
									data.Value.CleanUp();
									data.Value.Coordinate = new Vector2[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2];

									/* Set Interpolation-Data */
									data.Formula = formula;
									data.FrameCurveStart = frameCurveStart;
									data.ValueCurveStart = valueCurveStart;
									data.FrameCurveEnd = frameCurveEnd;
									data.ValueCurveEnd = valueCurveEnd;

									/* Set Body-Data */
									data.Frame = frame;

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/LT", managerNameSpace);
									valueTextSplit = valueText.Split(' ');
									data.Value.Coordinate[(int)Library_SpriteStudio6.KindVertex.LU].x = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[0]));
									data.Value.Coordinate[(int)Library_SpriteStudio6.KindVertex.LU].y = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[1]));

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/RT", managerNameSpace);
									valueTextSplit = valueText.Split(' ');
									data.Value.Coordinate[(int)Library_SpriteStudio6.KindVertex.RU].x = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[0]));
									data.Value.Coordinate[(int)Library_SpriteStudio6.KindVertex.RU].y = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[1]));

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/RB", managerNameSpace);
									valueTextSplit = valueText.Split(' ');
									data.Value.Coordinate[(int)Library_SpriteStudio6.KindVertex.RD].x = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[0]));
									data.Value.Coordinate[(int)Library_SpriteStudio6.KindVertex.RD].y = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[1]));

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/LB", managerNameSpace);
									valueTextSplit = valueText.Split(' ');
									data.Value.Coordinate[(int)Library_SpriteStudio6.KindVertex.LD].x = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[0]));
									data.Value.Coordinate[(int)Library_SpriteStudio6.KindVertex.LD].y = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[1]));

									/* Add Key-Data */
									informationAnimationParts.VertexCorrection.ListKey.Add(data);
								}
								break;

							case "USER":
								{
									Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUserData.KeyData data = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUserData.KeyData();
									data.Value.CleanUp();

									/* Set Interpolation-Data */
									/* MEMO: User-Data can't have interpolation */
									data.Formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.NON;
									data.FrameCurveStart = 0.0f;
									data.ValueCurveStart = 0.0f;
									data.FrameCurveEnd = 0.0f;
									data.ValueCurveEnd = 0.0f;

									/* Set Body-Data */
									data.Frame = frame;

									data.Value.Flags = Library_SpriteStudio6.Data.Animation.Attribute.UserData.FlagBit.CLEAR;
									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/integer", managerNameSpace);
									if(null != valueText)
									{
										data.Value.Flags |= Library_SpriteStudio6.Data.Animation.Attribute.UserData.FlagBit.NUMBER;
										if(false == int.TryParse(valueText, out data.Value.NumberInt))
										{
											uint valueUint = 0;
											if(false == uint.TryParse(valueText, out valueUint))
											{
												LogWarning(messageLogPrefix, "Broken UserData-Integer \"" + valueText + "\" Frame[" + frame.ToString() + "] Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
												data.Value.Flags &= ~Library_SpriteStudio6.Data.Animation.Attribute.UserData.FlagBit.NUMBER;
												data.Value.NumberInt = 0;
											}
											else
											{
												data.Value.NumberInt = (int)valueUint;
											}
										}
									}
									else
									{
										data.Value.Flags &= ~Library_SpriteStudio6.Data.Animation.Attribute.UserData.FlagBit.NUMBER;
										data.Value.NumberInt = 0;
									}

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/rect", managerNameSpace);
									if(null != valueText)
									{
										valueTextSplit = valueText.Split(' ');
										data.Value.Rectangle.xMin = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[0]));
										data.Value.Rectangle.yMin = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[1]));
										data.Value.Rectangle.xMax = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[2]));
										data.Value.Rectangle.yMax = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[3]));
										data.Value.Flags |= Library_SpriteStudio6.Data.Animation.Attribute.UserData.FlagBit.RECTANGLE;
									}
									else
									{
										data.Value.Rectangle.xMin = 0.0f;
										data.Value.Rectangle.yMin = 0.0f;
										data.Value.Rectangle.xMax = 0.0f;
										data.Value.Rectangle.yMax = 0.0f;
										data.Value.Flags &= ~Library_SpriteStudio6.Data.Animation.Attribute.UserData.FlagBit.RECTANGLE;
									}

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/point", managerNameSpace);
									if(null != valueText)
									{
										valueTextSplit = valueText.Split(' ');
										data.Value.Coordinate.x = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[0]));
										data.Value.Coordinate.y = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueTextSplit[1]));
										data.Value.Flags |= Library_SpriteStudio6.Data.Animation.Attribute.UserData.FlagBit.COORDINATE;
									}
									else
									{
										data.Value.Coordinate.x = 0.0f;
										data.Value.Coordinate.y = 0.0f;
										data.Value.Flags &= ~Library_SpriteStudio6.Data.Animation.Attribute.UserData.FlagBit.COORDINATE;
									}

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/string", managerNameSpace);
									if(null != valueText)
									{
										data.Value.Text = string.Copy(valueText);
										data.Value.Flags |= Library_SpriteStudio6.Data.Animation.Attribute.UserData.FlagBit.TEXT;
									}
									else
									{
										data.Value.Text = null;
										data.Value.Flags &= ~Library_SpriteStudio6.Data.Animation.Attribute.UserData.FlagBit.TEXT;
									}

									/* Add Key-Data */
									informationAnimationParts.UserData.ListKey.Add(data);
								}
								break;

							case "IPRM":
								{
									Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInstance.KeyData data = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInstance.KeyData();
									data.Value.CleanUp();

									/* Set Interpolation-Data */
									/* MEMO: Instance can't have interpolation */
									data.Formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.NON;
									data.FrameCurveStart = 0.0f;
									data.ValueCurveStart = 0.0f;
									data.FrameCurveEnd = 0.0f;
									data.ValueCurveEnd = 0.0f;

									/* Set Body-Data */
									data.Frame = frame;

									data.Value.PlayCount = -1;
									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/infinity", managerNameSpace);
									if(null != valueText)
									{
										if(true == LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText))
										{	/* Check */
											data.Value.PlayCount = 0;
										}
									}
									if(-1 == data.Value.PlayCount)
									{	/* Loop-Limited */
										valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/loopNum", managerNameSpace);
										data.Value.PlayCount = (null == valueText) ? 1 : LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
									}

									float SignRateSpeed = 1.0f;
									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/reverse", managerNameSpace);
									if(null != valueText)
									{
										SignRateSpeed = (true == LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText)) ? -1.0f : 1.0f;
									}

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/pingpong", managerNameSpace);
									if(null == valueText)
									{
										data.Value.Flags &= ~Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.PINGPONG;
									}
									else
									{
										data.Value.Flags = (true == LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText)) ?
																(data.Value.Flags | Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.PINGPONG)
																: (data.Value.Flags & ~Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.PINGPONG);
									}

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/independent", managerNameSpace);
									if(null == valueText)
									{
										data.Value.Flags &= ~Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.INDEPENDENT;
									}
									else
									{
										data.Value.Flags = (true == LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText)) ?
																(data.Value.Flags | Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.INDEPENDENT)
																: (data.Value.Flags & ~Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.INDEPENDENT);
									}

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/startLabel", managerNameSpace);
									data.Value.LabelStart = (null == valueText) ? string.Copy(Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START]) : string.Copy(valueText);

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/startOffset", managerNameSpace);
									data.Value.OffsetStart = (null == valueText) ? 0 : LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/endLabel", managerNameSpace);
									data.Value.LabelEnd = (null == valueText) ? string.Copy(Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END]) : string.Copy(valueText);

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/endOffset", managerNameSpace);
									data.Value.OffsetEnd = (null == valueText) ? 0 : LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/speed", managerNameSpace);
									data.Value.RateTime = (null == valueText) ? 1.0f : (float)LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueText);
									data.Value.RateTime *= SignRateSpeed;

									/* Add Key-Data */
									informationAnimationParts.Instance.ListKey.Add(data);
								}
								break;

							case "EFCT":
								{
									Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeEffect.KeyData data = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeEffect.KeyData();
									data.Value.CleanUp();

									/* Set Interpolation-Data */
									/* MEMO: Instance can't have interpolation */
									data.Formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.NON;
									data.FrameCurveStart = 0.0f;
									data.ValueCurveStart = 0.0f;
									data.FrameCurveEnd = 0.0f;
									data.ValueCurveEnd = 0.0f;

									/* Set Body-Data */
									data.Frame = frame;

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/startTime", managerNameSpace);
									data.Value.FrameStart = (null == valueText) ? 0 : LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/speed", managerNameSpace);
									data.Value.RateTime = (null == valueText) ? 1.0f : LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText);

									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/independent", managerNameSpace);
									if(null == valueText)
									{
										data.Value.Flags &= ~Library_SpriteStudio6.Data.Animation.Attribute.Effect.FlagBit.INDEPENDENT;
									}
									else
									{
										data.Value.Flags = (true == LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText)) ?
																(data.Value.Flags | Library_SpriteStudio6.Data.Animation.Attribute.Effect.FlagBit.INDEPENDENT)
																: (data.Value.Flags & ~Library_SpriteStudio6.Data.Animation.Attribute.Effect.FlagBit.INDEPENDENT);
									}

									/* Add Key-Data */
									informationAnimationParts.Effect.ListKey.Add(data);
								}
								break;

							/* Disused(Legacy) Attributes */
							case "IMGX":
							case "IMGY":
							case "IMGW":
							case "IMGH":
							case "ORFX":
							case "ORFY":
								LogWarning(messageLogPrefix, "No-Longer-Used Attribute \"" + tagText + "\" Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
								break;

							/* Unknown Attributes */
							default:
								LogWarning(messageLogPrefix, "Unknown Attribute \"" + tagText + "\" Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
								break;
						}
					}
				}

				/* Solve Attributes */
				if(false == ParseAnimationAttributeSolve(	ref setting,
															informationSSPJ,
															informationSSAE,
															informationAnimation,
															informationAnimationParts,
															indexParts,
															nameFileSSAE
														)
					)
				{
					goto ParseAnimationAttribute_ErrorEnd;
				}
				return(true);

			ParseAnimationAttribute_ErrorEnd:;
				return(false);
			}
			private static void ParseAnimationAttributeColorBlend(	out float colorA,
																	out float colorR,
																	out float colorG,
																	out float colorB,
																	out float ratePixel,
																	System.Xml.XmlNode NodeKey,
																	string NameTagBase,
																	System.Xml.XmlNamespaceManager ManagerNameSpace
																)
			{
				string valueText = "";

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(NodeKey, NameTagBase + "/rgba", ManagerNameSpace);
				uint ARGB = LibraryEditor_SpriteStudio6.Utility.Text.HexToUInt(valueText);
				ratePixel = (float)((ARGB >> 24) & 0xff) / 255.0f;
				colorR = (float)((ARGB >> 16) & 0xff) / 255.0f;
				colorG = (float)((ARGB >> 8) & 0xff) / 255.0f;
				colorB = (float)(ARGB & 0xff) / 255.0f;

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(NodeKey, NameTagBase + "/rate", ManagerNameSpace);
				colorA = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetDouble(valueText));
			}

			private static bool ParseAnimationAttributeSolve(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																Information informationSSAE,
																Information.Animation informationAnimation,
																Information.Animation.Parts informationAnimationParts,
																int indexParts,
																string nameFileSSAE
															)
			{
				const string messageLogPrefix = "Parse SSAE(Attributes)";

				/* Adjust Top-Frame Key-Data */
				informationAnimationParts.Cell.KeyDataAdjustTopFrame();

				informationAnimationParts.PositionX.KeyDataAdjustTopFrame();
				informationAnimationParts.PositionY.KeyDataAdjustTopFrame();
				informationAnimationParts.PositionZ.KeyDataAdjustTopFrame();
				informationAnimationParts.RotationX.KeyDataAdjustTopFrame();
				informationAnimationParts.RotationY.KeyDataAdjustTopFrame();
				informationAnimationParts.RotationZ.KeyDataAdjustTopFrame();
				informationAnimationParts.ScalingX.KeyDataAdjustTopFrame();
				informationAnimationParts.ScalingY.KeyDataAdjustTopFrame();

				informationAnimationParts.RateOpacity.KeyDataAdjustTopFrame();
				informationAnimationParts.Priority.KeyDataAdjustTopFrame();

				informationAnimationParts.FlipX.KeyDataAdjustTopFrame();
				informationAnimationParts.FlipY.KeyDataAdjustTopFrame();
				informationAnimationParts.Hide.KeyDataAdjustTopFrame(true);	/* "Hide" is true for the top-frames without key data.(not value of first key to appear) */

				informationAnimationParts.ColorBlend.KeyDataAdjustTopFrame();
				informationAnimationParts.VertexCorrection.KeyDataAdjustTopFrame();

				informationAnimationParts.PivotOffsetX.KeyDataAdjustTopFrame();
				informationAnimationParts.PivotOffsetY.KeyDataAdjustTopFrame();

				informationAnimationParts.AnchorPositionX.KeyDataAdjustTopFrame();
				informationAnimationParts.AnchorPositionY.KeyDataAdjustTopFrame();
				informationAnimationParts.SizeForceX.KeyDataAdjustTopFrame();
				informationAnimationParts.SizeForceY.KeyDataAdjustTopFrame();

				informationAnimationParts.TexturePositionX.KeyDataAdjustTopFrame();
				informationAnimationParts.TexturePositionY.KeyDataAdjustTopFrame();
				informationAnimationParts.TextureRotation.KeyDataAdjustTopFrame();
				informationAnimationParts.TextureScalingX.KeyDataAdjustTopFrame();
				informationAnimationParts.TextureScalingY.KeyDataAdjustTopFrame();
				informationAnimationParts.TextureFlipX.KeyDataAdjustTopFrame();
				informationAnimationParts.TextureFlipY.KeyDataAdjustTopFrame();

				informationAnimationParts.RadiusCollision.KeyDataAdjustTopFrame();

/* 				informationAnimationParts.UserData.KeyDataAdjustTopFrame(); *//* Not Adjust */
				informationAnimationParts.Instance.KeyDataAdjustTopFrame();
				informationAnimationParts.Effect.KeyDataAdjustTopFrame();

				/* Delete attributes that should not exist */
				informationAnimationParts.AnchorPositionX.ListKey.Clear();	/* Unsupported */
				informationAnimationParts.AnchorPositionY.ListKey.Clear();	/* Unsupported */
				switch(informationSSAE.TableParts[indexParts].Data.Feature)
				{
					case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
					case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
						informationAnimationParts.Cell.ListKey.Clear();

						informationAnimationParts.ColorBlend.ListKey.Clear();
						informationAnimationParts.VertexCorrection.ListKey.Clear();

						informationAnimationParts.PivotOffsetX.ListKey.Clear();
						informationAnimationParts.PivotOffsetY.ListKey.Clear();

						informationAnimationParts.SizeForceX.ListKey.Clear();
						informationAnimationParts.SizeForceY.ListKey.Clear();

						informationAnimationParts.TexturePositionX.ListKey.Clear();
						informationAnimationParts.TexturePositionY.ListKey.Clear();
						informationAnimationParts.TextureRotation.ListKey.Clear();
						informationAnimationParts.TextureScalingX.ListKey.Clear();
						informationAnimationParts.TextureScalingY.ListKey.Clear();
						informationAnimationParts.TextureFlipX.ListKey.Clear();
						informationAnimationParts.TextureFlipY.ListKey.Clear();

						informationAnimationParts.Instance.ListKey.Clear();
						informationAnimationParts.Effect.ListKey.Clear();
						break;

					case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
					case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
					case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
						if(null == informationAnimationParts.VertexCorrection)
						{
							informationSSAE.TableParts[indexParts].Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2;
						}
						else
						{
							informationSSAE.TableParts[indexParts].Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4;
						}

						informationAnimationParts.Instance.ListKey.Clear();
						informationAnimationParts.Effect.ListKey.Clear();
						break;

					case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
						informationAnimationParts.Cell.ListKey.Clear();

						informationAnimationParts.FlipX.ListKey.Clear();
						informationAnimationParts.FlipY.ListKey.Clear();

						informationAnimationParts.ColorBlend.ListKey.Clear();
						informationAnimationParts.VertexCorrection.ListKey.Clear();

						informationAnimationParts.PivotOffsetX.ListKey.Clear();
						informationAnimationParts.PivotOffsetY.ListKey.Clear();

						informationAnimationParts.SizeForceX.ListKey.Clear();
						informationAnimationParts.SizeForceY.ListKey.Clear();

						informationAnimationParts.TexturePositionX.ListKey.Clear();
						informationAnimationParts.TexturePositionY.ListKey.Clear();
						informationAnimationParts.TextureRotation.ListKey.Clear();
						informationAnimationParts.TextureScalingX.ListKey.Clear();
						informationAnimationParts.TextureScalingY.ListKey.Clear();
						informationAnimationParts.TextureFlipX.ListKey.Clear();
						informationAnimationParts.TextureFlipY.ListKey.Clear();

						informationAnimationParts.Effect.ListKey.Clear();
						break;

					case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
						informationAnimationParts.Cell.ListKey.Clear();

						informationAnimationParts.FlipX.ListKey.Clear();
						informationAnimationParts.FlipY.ListKey.Clear();

						informationAnimationParts.ColorBlend.ListKey.Clear();
						informationAnimationParts.VertexCorrection.ListKey.Clear();

						informationAnimationParts.PivotOffsetX.ListKey.Clear();
						informationAnimationParts.PivotOffsetY.ListKey.Clear();

						informationAnimationParts.SizeForceX.ListKey.Clear();
						informationAnimationParts.SizeForceY.ListKey.Clear();

						informationAnimationParts.TexturePositionX.ListKey.Clear();
						informationAnimationParts.TexturePositionY.ListKey.Clear();
						informationAnimationParts.TextureRotation.ListKey.Clear();
						informationAnimationParts.TextureScalingX.ListKey.Clear();
						informationAnimationParts.TextureScalingY.ListKey.Clear();
						informationAnimationParts.TextureFlipX.ListKey.Clear();
						informationAnimationParts.TextureFlipY.ListKey.Clear();

						informationAnimationParts.Instance.ListKey.Clear();
						break;

					default:
						break;
				}

				return(true);
			}

			private static void LogError(string messagePrefix, string message, string nameFile, LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ)
			{
				LibraryEditor_SpriteStudio6.Utility.Log.Error(	messagePrefix
																+ ": " + message
																+ " [" + nameFile + "]"
																+ " in \"" + informationSSPJ.FileNameGetFullPath() + "\""
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
				CODE_010001 = 0x00010001,
				CODE_010002 = 0x00010002,	/* after SS5.3.5 */
				CODE_010200 = 0x00010200,	/* after SS5.5.0 beta-3 */
				CODE_010201 = 0x00010201,	/* after SS5.7.0 beta-1 */
				CODE_010202 = 0x00010202,	/* after SS5.7.0 beta-2 */

				TARGET_EARLIEST = CODE_000100,
				TARGET_LATEST = CODE_010202
			};

			private const string ExtentionFile = ".ssae";
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public class Information
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public LibraryEditor_SpriteStudio6.Import.SSAE.KindVersion Version;

				public string NameDirectory;
				public string NameFileBody;
				public string NameFileExtension;

				public Parts[] TableParts;
				public int[] TableIndexCellMap;
				public Animation[] TableAnimation;

				public Library_SpriteStudio6.Data.Animation.Parts.KindFormat FormatSS6PU;
				public LibraryEditor_SpriteStudio6.Import.Assets<Script_SpriteStudio6_DataAnimation> DataAnimationSS6PU;
				public LibraryEditor_SpriteStudio6.Import.Assets<Object> PrefabAnimationSS6PU;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Version = LibraryEditor_SpriteStudio6.Import.SSAE.KindVersion.ERROR;

					NameDirectory = "";
					NameFileBody = "";
					NameFileExtension = "";

					TableParts = null;
					TableIndexCellMap = null;
					TableAnimation = null;

					DataAnimationSS6PU.CleanUp();
					DataAnimationSS6PU.BootUp(1);	/* Always 1 */
					PrefabAnimationSS6PU.CleanUp();
					PrefabAnimationSS6PU.BootUp(1);	/* Always 1 */
				}

				public string FileNameGetFullPath()
				{
					return(NameDirectory + NameFileBody + NameFileExtension);
				}

				public int IndexGetParts(string name)
				{
					if(null != TableParts)
					{
						for(int i=0; i<TableParts.Length; i++)
						{
							if(name == TableParts[i].Data.Name)
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
				public class Parts
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Library_SpriteStudio6.Data.Parts.Animation Data;

					public KindInheritance Inheritance;
					public FlagBitInheritance FlagInheritance;

					public List<int> ListIndexPartsChild;

					/* MEMO: UnderControl == Instance, Effect */
					public string NameUnderControl;
					public int IndexUnderControl;
					public int IndexAnimationUnderControl;	/* only Instance */
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public void CleanUp()
					{
						Data.CleanUp();

						Inheritance = KindInheritance.PARENT;
						FlagInheritance = FlagBitInheritance.CLEAR;

						ListIndexPartsChild = new List<int>();
						ListIndexPartsChild.Clear();

						NameUnderControl = "";
						IndexUnderControl = -1;
						IndexAnimationUnderControl = -1;
					}
					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					public enum KindInheritance
					{
						PARENT = 0,
						SELF
					}

					public enum FlagBitInheritance
					{
						OPACITY_RATE = 0x000000001,
						SHOW_HIDE = 0x000000002,
						FLIP_X = 0x000000010,
						FLIP_Y = 0x000000020,

						CLEAR = 0x00000000,
						ALL = OPACITY_RATE
							| SHOW_HIDE
							| FLIP_X
							| FLIP_Y,
						PRESET = OPACITY_RATE
					}
					#endregion Enums & Constants
				}

				public class Animation
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Library_SpriteStudio6.Data.Animation Data;

					public KindModeSort ModeSort;

					public Parts[] TableParts;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public void CleanUp()
					{
						Data = new Library_SpriteStudio6.Data.Animation();	/* class */

						ModeSort = KindModeSort.PRIORITY;

						TableParts = null;
					}

					public bool StatusSetParts(	LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
												LibraryEditor_SpriteStudio6.Import.SSAE.Information informationSSAE
											)
					{
						int countFrame = Data.CountFrame;
						int countParts = TableParts.Length;
						Parts animationParts = null;

						for(int i=0; i<countParts; i++)
						{
							animationParts = TableParts[i];

							if(0 != (animationParts.StatusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED))
							{	/* Not Use */
								animationParts.StatusParts |= (Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION
																| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION
																| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING
																| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.HIDE_FULL);
							}
							else
							{
								/* Check Transform */
								if((0 >= animationParts.PositionX.CountGetKey())
									&& (0 >= animationParts.PositionY.CountGetKey())
									&& (0 >= animationParts.PositionZ.CountGetKey())
									)
								{
									animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION;
								}
								if((0 >= animationParts.RotationX.CountGetKey())
									&& (0 >= animationParts.RotationY.CountGetKey())
									&& (0 >= animationParts.RotationZ.CountGetKey())
									)
								{
									animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION;
								}
								if((0 >= animationParts.ScalingX.CountGetKey())
									&& (0 >= animationParts.ScalingY.CountGetKey())
									)
								{
									animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING;
								}

								/* Check Hidden */
								bool flagHideAll = true;
								animationParts.TableHide = new bool[countFrame];
								for(int j=0; j<countFrame; j++)
								{
									Library_SpriteStudio6.Data.Animation.Attribute.Importer.Inheritance.ValueGetBoolOR(	out animationParts.TableHide[j],
																														animationParts.Hide,
																														j,
																														true
																													);
									if(false == animationParts.TableHide[j])
									{
										flagHideAll = false;
									}
								}
								if(true == flagHideAll)
								{
									animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.HIDE_FULL;
								}
							}

							/* Set Valid */
							animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.VALID;
						}
						return(false);

//					PartsStatusSet_ErrorEnd:;
//						return(false);
					}

					public bool DrawOrderCreate(	LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
													LibraryEditor_SpriteStudio6.Import.SSAE.Information informationSSAE
											)
					{
						int countFrame = Data.CountFrame;
						int countParts = TableParts.Length;
						Parts animationParts = null;
						LibraryEditor_SpriteStudio6.Import.SSAE.Information.Parts parts = null;

						/* Prepare parts to process */
						List<int> listIndexPartsDraw = new List<int>(countParts);
						listIndexPartsDraw.Clear();
						float[][] tableDrawPriority = new float[countParts][];
						for(int i=0; i<countParts; i++)
						{
							animationParts = TableParts[i];
							parts = informationSSAE.TableParts[i];
							tableDrawPriority[i] = null;

							switch(parts.Data.Feature)
							{
								/* Non draw parts */
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
									/* MEMO: Create table in "Root"part so that can get first drawing part's index. */
									animationParts.TableOrderDraw = new int[countFrame];
									for(int j=0; j<countFrame; j++)
									{
										animationParts.TableOrderDraw[j] = -1;
									}
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
									break;

								/* Draw parts */
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
									/* Create Draw-Order table */
									animationParts.TableOrderDraw = new int[countFrame];
									for(int j=0; j<countFrame; j++)
									{
										animationParts.TableOrderDraw[j] = -1;
									}

									/* Calculate all frames' priority. */
									tableDrawPriority[i] = new float[countFrame];
									switch(ModeSort)
									{
										case KindModeSort.PRIORITY:
											DrawOrderCreatePriority(ref tableDrawPriority[i], informationSSPJ, informationSSAE, this, animationParts);
											break;

										case KindModeSort.POSITION_Z:
//											DrawOrderCreatePositionZ(ref tableDrawPriority[i], informationSSPJ, informationSSAE, this, animationParts);
											break;
									}

									/* Add as part to be processed */
									listIndexPartsDraw.Add(i);
									break;

								default:
									/* MEMO: No reach here. */
									break;
							}
						}

						/* Decide Draw-Order Table */
						/* MEMO: "Root"part is excluded from target. */
						int countIndexPartsDraw = listIndexPartsDraw.Count;
						List<int> listIndexPartsSort = new List<int>(countIndexPartsDraw);
						listIndexPartsSort.Clear();
						List<float> listPrioritySort = new List<float>(countIndexPartsDraw);
						listPrioritySort.Clear();
						for(int frame=0; frame<countFrame; frame++)
						{
							/* Extract draw parts (in this frame) */
							for(int i=0; i<countIndexPartsDraw; i++)
							{
								int indexParts = listIndexPartsDraw[i];
								animationParts = TableParts[indexParts];
								if(0 == (animationParts.StatusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.HIDE_FULL))
								{
									if(false == animationParts.TableHide[frame])
									{
										listIndexPartsSort.Add(indexParts);
										listPrioritySort.Add(tableDrawPriority[indexParts][frame]);
									}
								}
							}

							/* Sort (Bubble) */
							/* When the same priority, parts that has larger ID (part-index) drawed later. */
							int countIndexPartsSort = listIndexPartsSort.Count;
							for(int i=0; i<countIndexPartsSort; i++)
							{
								for(int j=(countIndexPartsSort - 1); j>i; j--)
								{
									int k = j - 1;
									if((listPrioritySort[j] < listPrioritySort[k])
										|| ((listPrioritySort[j] == listPrioritySort[k]) && (listIndexPartsSort[j] < listIndexPartsSort[k]))
										)
									{
										float tempFloat = listPrioritySort[j];
										int tempInt = listIndexPartsSort[j];

										listPrioritySort[j] = listPrioritySort[k];
										listIndexPartsSort[j] = listIndexPartsSort[k];

										listPrioritySort[k] = tempFloat;
										listIndexPartsSort[k] = tempInt;
									}
								}
							}
							listPrioritySort.Clear();

							/* Set Order */
							/* MEMO: In "Root"part, first-drawing part's index is stored. */
							TableParts[0].TableOrderDraw[frame] = (0 < countIndexPartsSort) ? listIndexPartsSort[0] : -1;
							for(int i=1; i<countIndexPartsSort; i++)
							{
								TableParts[listIndexPartsSort[i - 1]].TableOrderDraw[frame] = listIndexPartsSort[i];
							}
							listIndexPartsSort.Clear();
						}

						return(true);
					}
					private void DrawOrderCreatePriority(	ref float[] tableValue,
															LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
															LibraryEditor_SpriteStudio6.Import.SSAE.Information informationSSAE,
															LibraryEditor_SpriteStudio6.Import.SSAE.Information.Animation informationAnimation,
															LibraryEditor_SpriteStudio6.Import.SSAE.Information.Animation.Parts informationAnimationParts
													)
					{
						int valueInt;
						int countFrame = tableValue.Length;
						for(int j=0; j<countFrame; j++)
						{
							if(false == informationAnimationParts.Priority.ValueGet(out valueInt, j))
							{
								tableValue[j] = 0.0f;
							}
							else
							{
								tableValue[j] = (float)valueInt;
							}
						}
					}

					#endregion Functions

					/* ----------------------------------------------- Enums & Constants */
					#region Enums & Constants
					public enum KindModeSort
					{
						PRIORITY = 0,
						POSITION_Z,
					}
					#endregion Enums & Constants

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					public class Parts
					{
						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell Cell;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat PositionX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat PositionY;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat PositionZ;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat RotationX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat RotationY;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat RotationZ;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat ScalingX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat ScalingY;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat RateOpacity;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt Priority;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool FlipX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool FlipY;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool Hide;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlend ColorBlend;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection VertexCorrection;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat PivotOffsetX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat PivotOffsetY;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat AnchorPositionX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat AnchorPositionY;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat SizeForceX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat SizeForceY;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat TexturePositionX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat TexturePositionY;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat TextureRotation;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat TextureScalingX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat TextureScalingY;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool TextureFlipX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool TextureFlipY;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat RadiusCollision;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUserData UserData;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInstance Instance;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeEffect Effect;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt FixIndexCellMap;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCoordinateFix FixCoordinate;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlendFix FixColorBlend;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUVFix FixUV;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat FixSizeCollisionX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat FixSizeCollisionY;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat FixPivotCollisionX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat FixPivotCollisionY;

						public Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus StatusParts;
						public bool[] TableHide;	/* Expand "Hide"attribute in order to drawing state optimize. */
						public int[] TableOrderDraw;

						#endregion Variables & Properties

						/* ----------------------------------------------- Functions */
						#region Functions
						public void CleanUp()
						{
							Cell = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCell();
							Cell.CleanUp();

							PositionX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							PositionX.CleanUp();
							PositionY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							PositionY.CleanUp();
							PositionZ = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							PositionZ.CleanUp();
							RotationX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							RotationX.CleanUp();
							RotationY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							RotationY.CleanUp();
							RotationZ = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							RotationZ.CleanUp();
							ScalingX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							ScalingX.CleanUp();
							ScalingY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							ScalingY.CleanUp();

							RateOpacity = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							RateOpacity.CleanUp();
							Priority = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt();
							Priority.CleanUp();

							FlipX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool();
							FlipX.CleanUp();
							FlipY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool();
							FlipY.CleanUp();
							Hide = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool();
							Hide.CleanUp();

							ColorBlend = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlend();
							ColorBlend.CleanUp();
							VertexCorrection = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeVertexCorrection();
							VertexCorrection.CleanUp();

							PivotOffsetX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							PivotOffsetX.CleanUp();
							PivotOffsetY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							PivotOffsetY.CleanUp();

							AnchorPositionX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							AnchorPositionX.CleanUp();
							AnchorPositionY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							AnchorPositionY.CleanUp();
							SizeForceX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							SizeForceX.CleanUp();
							SizeForceY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							SizeForceY.CleanUp();

							TexturePositionX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							TexturePositionX.CleanUp();
							TexturePositionY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							TexturePositionY.CleanUp();
							TextureRotation = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							TextureRotation.CleanUp();
							TextureScalingX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							TextureScalingX.CleanUp();
							TextureScalingY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							TextureScalingY.CleanUp();
							TextureFlipX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool();
							TextureFlipX.CleanUp();
							TextureFlipY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool();
							TextureFlipY.CleanUp();

							RadiusCollision = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							RadiusCollision.CleanUp();

							UserData = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUserData();
							UserData.CleanUp();

							Instance = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInstance();
							Instance.CleanUp();
							Effect = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeEffect();
							Effect.CleanUp();

							FixIndexCellMap = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt();
							FixIndexCellMap.CleanUp();
							FixCoordinate = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCoordinateFix();
							FixCoordinate.CleanUp();
							FixColorBlend = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeColorBlendFix();
							FixColorBlend.CleanUp();
							FixUV = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUVFix();
							FixUV.CleanUp();
							FixSizeCollisionX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							FixSizeCollisionX.CleanUp();
							FixSizeCollisionY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							FixSizeCollisionY.CleanUp();
							FixPivotCollisionX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							FixPivotCollisionX.CleanUp();
							FixPivotCollisionY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							FixPivotCollisionY.CleanUp();

							StatusParts = Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED;
							TableHide = null;
							TableOrderDraw = null;
						}

						public bool BootUp()
						{
							Cell.BootUp();

							PositionX.BootUp();
							PositionY.BootUp();
							PositionZ.BootUp();
							RotationX.BootUp();
							RotationY.BootUp();
							RotationZ.BootUp();
							ScalingX.BootUp();
							ScalingY.BootUp();

							RateOpacity.BootUp();
							Priority.BootUp();

							FlipX.BootUp();
							FlipY.BootUp();
							Hide.BootUp();

							ColorBlend.BootUp();
							VertexCorrection.BootUp();

							PivotOffsetX.BootUp();
							PivotOffsetY.BootUp();

							AnchorPositionX.BootUp();
							AnchorPositionY.BootUp();
							SizeForceX.BootUp();
							SizeForceY.BootUp();

							TexturePositionX.BootUp();
							TexturePositionY.BootUp();
							TextureRotation.BootUp();
							TextureScalingX.BootUp();
							TextureScalingY.BootUp();
							TextureFlipX.BootUp();
							TextureFlipY.BootUp();

							RadiusCollision.BootUp();

							UserData.BootUp();

							Instance.BootUp();
							Effect.BootUp();

							FixIndexCellMap.BootUp();
							FixCoordinate.BootUp();
							FixColorBlend.BootUp();
							FixUV.BootUp();
							FixSizeCollisionX.BootUp();
							FixSizeCollisionY.BootUp();
							FixPivotCollisionX.BootUp();
							FixPivotCollisionY.BootUp();

							StatusParts = Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.CLEAR;
							TableHide = null;
							TableOrderDraw = null;

							return(true);
						}

						public void ShutDown()
						{
							Cell.ShutDown();

							PositionX.ShutDown();
							PositionY.ShutDown();
							PositionZ.ShutDown();
							RotationX.ShutDown();
							RotationY.ShutDown();
							RotationZ.ShutDown();
							ScalingX.ShutDown();
							ScalingY.ShutDown();

							RateOpacity.ShutDown();
							Priority.ShutDown();

							FlipX.ShutDown();
							FlipY.ShutDown();
							Hide.ShutDown();

							ColorBlend.ShutDown();
							VertexCorrection.ShutDown();

							PivotOffsetX.ShutDown();
							PivotOffsetY.ShutDown();

							AnchorPositionX.ShutDown();
							AnchorPositionY.ShutDown();
							SizeForceX.ShutDown();
							SizeForceY.ShutDown();

							TexturePositionX.ShutDown();
							TexturePositionY.ShutDown();
							TextureRotation.ShutDown();
							TextureScalingX.ShutDown();
							TextureScalingY.ShutDown();
							TextureFlipX.ShutDown();
							TextureFlipY.ShutDown();

							RadiusCollision.ShutDown();

							UserData.ShutDown();

							Instance.ShutDown();
							Effect.ShutDown();

							FixIndexCellMap.ShutDown();
							FixCoordinate.ShutDown();
							FixColorBlend.ShutDown();
							FixUV.ShutDown();
							FixSizeCollisionX.ShutDown();
							FixSizeCollisionY.ShutDown();
							FixPivotCollisionX.ShutDown();
							FixPivotCollisionY.ShutDown();

							StatusParts = Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.CLEAR;
							TableHide = null;
							TableOrderDraw = null;
						}
						#endregion Functions
					}
					#endregion Classes, Structs & Interfaces
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
				public static bool AssetNameDecide(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
													LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
													LibraryEditor_SpriteStudio6.Import.SSAE.Information informationSSAE,
													string nameOutputAssetFolderBase,
													Script_SpriteStudio6_DataAnimation dataOverride,
													Script_SpriteStudio6_Root prefabOverride
												)
				{
					if(null != dataOverride)
					{	/* Specified */
						informationSSAE.DataAnimationSS6PU.TableName[0] = AssetDatabase.GetAssetPath(dataOverride);
						informationSSAE.DataAnimationSS6PU.TableData[0] = dataOverride;
					}
					else
					{	/* Default */
						informationSSAE.DataAnimationSS6PU.TableName[0] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_SS6PU, nameOutputAssetFolderBase)
																		+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.DATA_ANIMATION_SS6PU, informationSSAE.NameFileBody, informationSSPJ.NameFileBody)
																		+ LibraryEditor_SpriteStudio6.Import.NameExtentionScriptableObject;
						informationSSAE.DataAnimationSS6PU.TableData[0] = AssetDatabase.LoadAssetAtPath<Script_SpriteStudio6_DataAnimation>(informationSSAE.DataAnimationSS6PU.TableName[0]);
					}

					if(null != prefabOverride)
					{	/* Specified */
						informationSSAE.PrefabAnimationSS6PU.TableName[0] = AssetDatabase.GetAssetPath(prefabOverride);
						informationSSAE.PrefabAnimationSS6PU.TableData[0] = prefabOverride;
					}
					else
					{	/* Default */
						informationSSAE.PrefabAnimationSS6PU.TableName[0] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_SS6PU, nameOutputAssetFolderBase)
																		+ setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_SS6PU, informationSSAE.NameFileBody, informationSSPJ.NameFileBody)
																		+ LibraryEditor_SpriteStudio6.Import.NameExtensionPrefab;
						informationSSAE.PrefabAnimationSS6PU.TableData[0] = AssetDatabase.LoadAssetAtPath<GameObject>(informationSSAE.PrefabAnimationSS6PU.TableName[0]);
					}

					return(true);

//				AssetNameDecideData_ErroeEnd:;
//					return(false);
				}

				public static bool AssetCreateData(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
													LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
													LibraryEditor_SpriteStudio6.Import.SSAE.Information informationSSAE
												)
				{
					const string messageLogPrefix = "Create Asset(Data-Animation)";

					Script_SpriteStudio6_DataAnimation dataAnimation = informationSSAE.DataAnimationSS6PU.TableData[0];
					if(null == dataAnimation)
					{
						dataAnimation = ScriptableObject.CreateInstance<Script_SpriteStudio6_DataAnimation>();
						AssetDatabase.CreateAsset(dataAnimation, informationSSAE.DataAnimationSS6PU.TableName[0]);
						informationSSAE.DataAnimationSS6PU.TableData[0] = dataAnimation;
					}

					dataAnimation.Version = Script_SpriteStudio6_DataAnimation.KindVersion.SUPPORT_LATEST;

					int countParts = informationSSAE.TableParts.Length;
					Library_SpriteStudio6.Data.Parts.Animation[] tablePartsRuntime = new Library_SpriteStudio6.Data.Parts.Animation[countParts];
					for(int i=0; i<countParts; i++)
					{
						tablePartsRuntime[i] = informationSSAE.TableParts[i].Data;
					}
					dataAnimation.TableParts = tablePartsRuntime;

					int countAnimation = informationSSAE.TableAnimation.Length;
					Library_SpriteStudio6.Data.Animation[] tableAnimationRuntime = new Library_SpriteStudio6.Data.Animation[countAnimation];
					for(int i=0; i<countAnimation; i++)
					{
						tableAnimationRuntime[i] = informationSSAE.TableAnimation[i].Data;
					}
					dataAnimation.TableAnimation = tableAnimationRuntime;

					EditorUtility.SetDirty(dataAnimation);
					AssetDatabase.SaveAssets();

					return(true);

//				AssetCreateData_ErrorEnd:;
//					return(false);
				}

				public static bool ConvertFixMesh(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
													LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
													LibraryEditor_SpriteStudio6.Import.SSAE.Information informationSSAE
												)
				{
					const string messageLogPrefix = "Convert Fixing-Mesh (Data-Animation)";
					informationSSAE.FormatSS6PU = Library_SpriteStudio6.Data.Animation.Parts.KindFormat.FIX;
					return(true);

//				ConvertDataFixMesh_ErrorEnd:;
//					return(false);
				}

				public static bool ConvertData(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
												LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
												LibraryEditor_SpriteStudio6.Import.SSAE.Information informationSSAE
											)
				{
					const string messageLogPrefix = "Convert (Data-Animation)";
					int countParts = informationSSAE.TableParts.Length;
					int countAnimation = informationSSAE.TableAnimation.Length;

					/* Convert Animations */
					LibraryEditor_SpriteStudio6.Import.SSAE.Information.Animation informationAnimation = null;
					LibraryEditor_SpriteStudio6.Import.SSAE.Information.Animation.Parts informationAnimationParts = null;
					Library_SpriteStudio6.Data.Animation dataAnimation = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerBool packArgumentBool = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerInt packArgumentInt = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerFloat packArgumentFloat = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerCell packArgumentCell = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerColorBlend packArgumentColorBlend = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerVertexCorrection packArgumentVertexCorrection = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerUserData packArgumentUserData = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerInstance packArgumentInstance = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerEffect packArgumentEffect = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerCoordinateFix packArgumentCoordinateFix = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerColorBlendFix packArgumentColorBlendFix = null;
					Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerUVFix packArgumentUVFix = null;
					Library_SpriteStudio6.Data.Animation.PackAttribute.ArgumentContainer argumentContainer;
					int countFrame;
					for(int i=0; i<countAnimation; i++)
					{
						informationAnimation = informationSSAE.TableAnimation[i];
						dataAnimation = informationAnimation.Data;
						dataAnimation.TableParts = new Library_SpriteStudio6.Data.Animation.Parts[countParts];
						if(null == dataAnimation.TableParts)
						{
							LogError(messageLogPrefix, "Not Enough Memory (Data Animation Parts-Table) Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
							goto ConvertData_ErrorEnd;
						}
						countFrame = informationAnimation.Data.CountFrame;

						for(int j=0; j<countParts; j++)
						{
							argumentContainer.InstanceDataAnimation = null;	/* Unuse */
							argumentContainer.IndexAnimation = i;
							argumentContainer.IndexParts = j;
							argumentContainer.Frame = -1;	/* Unuse */
							argumentContainer.FrameKeyPrevious = -1;	/* Unuse */

							informationAnimationParts = informationAnimation.TableParts[j];
							dataAnimation.TableParts[j].StatusParts = informationAnimationParts.StatusParts;
							dataAnimation.TableParts[j].Format = informationSSAE.FormatSS6PU;

							PackAttribute.FactoryStatus(ref dataAnimation.TableParts[j].Status, ref packArgumentBool, setting.PackAttributeAnimation.Status);
							if(false == packArgumentBool.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeStatus,
																informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																informationAnimationParts.Hide,
																informationAnimationParts.FlipX,
																informationAnimationParts.FlipY,
																informationAnimationParts.TextureFlipX,
																informationAnimationParts.TextureFlipY
															)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"Status\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}

							PackAttribute.FactoryVector3(ref dataAnimation.TableParts[j].Position, ref packArgumentFloat, setting.PackAttributeAnimation.Position);
							if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePosition,
																informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																informationAnimationParts.PositionX,
																informationAnimationParts.PositionY,
																informationAnimationParts.PositionZ
															)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"Position\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}
							PackAttribute.FactoryVector3(ref dataAnimation.TableParts[j].Rotation, ref packArgumentFloat, setting.PackAttributeAnimation.Rotation);
							if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeRotation,
																informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																informationAnimationParts.RotationX,
																informationAnimationParts.RotationY,
																informationAnimationParts.RotationZ
															)
								)
							{
								goto ConvertData_ErrorEnd;
							}
							PackAttribute.FactoryVector2(ref dataAnimation.TableParts[j].Scaling, ref packArgumentFloat, setting.PackAttributeAnimation.Scaling);
							if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeScaling,
																informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																informationAnimationParts.ScalingX,
																informationAnimationParts.ScalingY
															)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"Rotation\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}


							PackAttribute.FactoryFloat(ref dataAnimation.TableParts[j].RateOpacity, ref packArgumentFloat, setting.PackAttributeAnimation.RateOpacity);
							if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeRateOpacity,
																informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																informationAnimationParts.RateOpacity
															)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"Scaling\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}

							PackAttribute.FactoryVector2(ref dataAnimation.TableParts[j].PositionAnchor, ref packArgumentFloat, setting.PackAttributeAnimation.PositionAnchor);
							if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePositionAnchor,
																informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																informationAnimationParts.AnchorPositionX,
																informationAnimationParts.AnchorPositionY
															)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"PositionAnchor\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}
							PackAttribute.FactoryVector2(ref dataAnimation.TableParts[j].SizeForce, ref packArgumentFloat, setting.PackAttributeAnimation.SizeForce);
							if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeSizeForce,
																informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																informationAnimationParts.SizeForceX,
																informationAnimationParts.SizeForceY
															)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"SizeForce\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}

							PackAttribute.FactoryUserData(ref dataAnimation.TableParts[j].UserData, ref packArgumentUserData, setting.PackAttributeAnimation.UserData);
							if(false == packArgumentUserData.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeUserData,
																	informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																	informationAnimationParts.UserData
															)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"UserData\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}
							PackAttribute.FactoryInstance(ref dataAnimation.TableParts[j].Instance, ref packArgumentInstance, setting.PackAttributeAnimation.Instance);
							if(false == packArgumentInstance.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeInstance,
																	informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																	informationAnimationParts.Instance
															)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"Instance\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}
							PackAttribute.FactoryEffect(ref dataAnimation.TableParts[j].Effect, ref packArgumentEffect, setting.PackAttributeAnimation.Effect);
							if(false == packArgumentEffect.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeEffect,
																	informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																	informationAnimationParts.Effect
															)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"Effect\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}

							PackAttribute.FactoryFloat(ref dataAnimation.TableParts[j].RadiusCollision, ref packArgumentFloat, setting.PackAttributeAnimation.RadiusCollision);
							if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeRadiusCollision,
																informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																informationAnimationParts.RadiusCollision
															)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"RadiusCollision\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}

							switch(informationSSAE.FormatSS6PU)
							{
								case Library_SpriteStudio6.Data.Animation.Parts.KindFormat.PLAIN:
									PackAttribute.FactoryCell(ref dataAnimation.TableParts[j].Plain.Cell, ref packArgumentCell, setting.PackAttributeAnimation.PlainCell);
									if(false == packArgumentCell.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainCell,
																		informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																		informationAnimationParts.Cell
																	)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.Cell\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									PackAttribute.FactoryColorBlend(ref dataAnimation.TableParts[j].Plain.ColorBlend, ref packArgumentColorBlend, setting.PackAttributeAnimation.PlainColorBlend);
									if(false == packArgumentColorBlend.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainColorBlend,
																				informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																				informationAnimationParts.ColorBlend
																		)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.ColorBlend\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									PackAttribute.FactoryVertexCorrection(ref dataAnimation.TableParts[j].Plain.VertexCorrection, ref packArgumentVertexCorrection, setting.PackAttributeAnimation.PlainVertexCorrection);
									if(false == packArgumentVertexCorrection.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainVertexCorrection,
																					informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																					informationAnimationParts.VertexCorrection
																				)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.VertexCorrection\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									PackAttribute.FactoryVector2(ref dataAnimation.TableParts[j].Plain.OffsetPivot, ref packArgumentFloat, setting.PackAttributeAnimation.PlainOffsetPivot);
									if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainOffsetPivot,
																		informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																		informationAnimationParts.PivotOffsetX,
																		informationAnimationParts.PivotOffsetY
																	)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.OffsetPivot\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									PackAttribute.FactoryVector2(ref dataAnimation.TableParts[j].Plain.PositionTexture, ref packArgumentFloat, setting.PackAttributeAnimation.PlainPositionTexture);
									if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainPositionTexture,
																		informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																		informationAnimationParts.TexturePositionX,
																		informationAnimationParts.TexturePositionY
																	)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.PositionTexture\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									PackAttribute.FactoryVector2(ref dataAnimation.TableParts[j].Plain.ScalingTexture, ref packArgumentFloat, setting.PackAttributeAnimation.PlainScalingTexture);
									if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainScalingTexture,
																		informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																		informationAnimationParts.TextureScalingX,
																		informationAnimationParts.TextureScalingY
																	)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.ScalingTexture\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									PackAttribute.FactoryFloat(ref dataAnimation.TableParts[j].Plain.RotationTexture, ref packArgumentFloat, setting.PackAttributeAnimation.PlainRotationTexture);
									if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainRotationTexture,
																		informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																		informationAnimationParts.TextureRotation
																	)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.RotationTexture\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}
									break;

								case Library_SpriteStudio6.Data.Animation.Parts.KindFormat.FIX:
									PackAttribute.FactoryInt(ref dataAnimation.TableParts[j].Fix.IndexCellMap, ref packArgumentInt, setting.PackAttributeAnimation.FixIndexCellMap);
									if(false == packArgumentInt.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixIndexCellMap,
																		informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																		informationAnimationParts.FixIndexCellMap
																	)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Fix.Coordinate\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									PackAttribute.FactoryCoordinateFix(ref dataAnimation.TableParts[j].Fix.Coordinate, ref packArgumentCoordinateFix, setting.PackAttributeAnimation.FixCoordinate);
									if(false == packArgumentCoordinateFix.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixCoordinate,
																				informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																				informationAnimationParts.FixCoordinate
																			)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Fix.Coordinate\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									PackAttribute.FactoryColorBlendFix(ref dataAnimation.TableParts[j].Fix.ColorBlend, ref packArgumentColorBlendFix, setting.PackAttributeAnimation.FixColorBlend);
									if(false == packArgumentColorBlendFix.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixColorBlend,
																				informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																				informationAnimationParts.FixColorBlend
																			)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Fix.ColorBlend\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									PackAttribute.FactoryUVFix(ref dataAnimation.TableParts[j].Fix.UV0, ref packArgumentUVFix, setting.PackAttributeAnimation.FixUV0);
									if(false == packArgumentUVFix.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixUV0,
																		informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																		informationAnimationParts.FixUV
																	)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Fix.UV0\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									PackAttribute.FactoryVector2(ref dataAnimation.TableParts[j].Fix.SizeCollision, ref packArgumentFloat, setting.PackAttributeAnimation.FixSizeCollision);
									if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixSizeCollision,
																		informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																		informationAnimationParts.FixSizeCollisionX,
																		informationAnimationParts.FixSizeCollisionY
																	)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Fix.SizeCollision\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									PackAttribute.FactoryVector2(ref dataAnimation.TableParts[j].Fix.PivotCollision, ref packArgumentFloat, setting.PackAttributeAnimation.FixPivotCollision);
									if(false == packArgumentFloat.Pack(	ref argumentContainer, countFrame, Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixPivotCollision,
																		informationAnimationParts.StatusParts, informationAnimationParts.TableOrderDraw,
																		informationAnimationParts.FixPivotCollisionX,
																		informationAnimationParts.FixPivotCollisionY
																	)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Fix.PivotCollision\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									break;

								default:
									/* MEMO: Impossible to reach. */
									break;
							}
						}
					}

					return(true);

				ConvertData_ErrorEnd:;
					return(false);
				}

				public static bool AssetPrecreatePrefab(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
															LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
															LibraryEditor_SpriteStudio6.Import.SSAE.Information informationSSAE
														)
				{
					const string messageLogPrefix = "Create Asset(Prefab-Animation)";

					return(true);

//				AssetCreateData_ErrorEnd:;
//					return(false);
				}

				public static bool AssetCreatePrefab(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
														LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
														LibraryEditor_SpriteStudio6.Import.SSAE.Information informationSSAE
													)
				{
					const string messageLogPrefix = "Create Asset(Prefab-Animation)";

					AssetDatabase.SaveAssets();

					return(true);

//				AssetCreateData_ErrorEnd:;
//					return(false);
				}
				#endregion Functions

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				public static partial class PackAttribute
				{
					/* ----------------------------------------------- Functions */
					#region Functions
					public static bool FactoryInt(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt container,
													ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerInt containerImport,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
												)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeInt attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeInt();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeInt attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeInt();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryFloat(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat container,
														ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerFloat containerImport,
														Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
													)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeFloat attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeFloat();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeFloat attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeFloat();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryVector2(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 container,
														ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerFloat containerImport,
														Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
												)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVector2 attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVector2();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeVector2 attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeVector2();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryVector3(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 container,
														ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerFloat containerImport,
														Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
												)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVector3 attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVector3();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeVector3 attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeVector3();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryStatus(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus container,
														ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerBool containerImport,
														Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
												)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeStatus attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeStatus();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeStatus attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeStatus();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryCell(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell container,
													ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerCell containerImport,
													Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
												)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeCell attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeCell();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeCell attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeCell();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryColorBlend(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerColorBlend container,
															ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerColorBlend containerImport,
															Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
														)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeColorBlend attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeColorBlend();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeColorBlend attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeColorBlend();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryVertexCorrection(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection container,
																ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerVertexCorrection containerImport,
																Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
															)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVertexCorrection attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeVertexCorrection();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeVertexCorrection attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeVertexCorrection();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryUserData(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData container,
														ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerUserData containerImport,
														Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
													)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								/* Not Support */
								return(false);

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeUserData attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeUserData();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryInstance(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance container,
														ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerInstance containerImport,
														Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
													)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								/* Not Support */
								return(false);

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeInstance attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeInstance();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryEffect(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect container,
														ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerEffect containerImport,
														Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
													)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								/* Not Support */
								return(false);

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeEffect attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeEffect();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryCoordinateFix(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix container,
																ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerCoordinateFix containerImport,
																Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
															)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeCoordinateFix attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeCoordinateFix();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeCoordinateFix attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeCoordinateFix();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryColorBlendFix(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerColorBlendFix container,
																ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerColorBlendFix containerImport,
																Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
															)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeColorBlendFix attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeColorBlendFix();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeColorBlendFix attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeColorBlendFix();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}

					public static bool FactoryUVFix(	ref Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix container,
														ref Library_SpriteStudio6.Data.Animation.Attribute.Importer.PackAttribute.ContainerUVFix containerImport,
														Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack pack
													)
					{
						switch(pack)
						{
							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_UNCOMPRESSED:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeUVFix attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardUncompressed.PackAttributeUVFix();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.STANDARD_CPE:
								{
									Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeUVFix attribute = new Library_SpriteStudio6.Data.Animation.PackAttribute.StandardCPE.PackAttributeUVFix();
									attribute.CleanUp();

									container = attribute;
									containerImport = attribute;
								}
								break;

							case Library_SpriteStudio6.Data.Animation.PackAttribute.KindPack.CPE_FLYWEIGHT:
								break;

							default:
								return(false);
						}
						return(true);
					}
					#endregion Functions
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
