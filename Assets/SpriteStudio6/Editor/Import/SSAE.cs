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

				bool flagHasSetup = false;
				string valueText;
				foreach(System.Xml.XmlNode nodeAnimation in listNode)
				{
					/* Check "SetUp" animation */
					/* MEMO: When judging "Setup"-animation, do not judge by name. (Be sure, use "isSetup" tag's value.)                      */
					/*       Especially, if SS5's SSAE including same name animation is migrated to SS6, "Setup"-animation's name is changed. */
					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAnimation, "isSetup", managerNameSpace);
					if(false == string.IsNullOrEmpty(valueText))
					{
						if(0 < LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText))
						{
							flagHasSetup = true;
						}
					}
				}
				int countAnimation = listNode.Count;
				if(true == flagHasSetup)
				{
					/* MEMO: If has "Setup", should not be countAnimation negative. */
					countAnimation--;
				}
				informationSSAE.TableAnimation = new Information.Animation[countAnimation];
				if(null == informationSSAE.TableAnimation)
				{
					LogError(messageLogPrefix, "Not Enough Memory (Animation-Data WorkArea)", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}

				int indexAnimation = 0;
				Information.Animation informationAnimation = null;
				foreach(System.Xml.XmlNode nodeAnimation in listNode)
				{
					/* Animation (& Parts' Key-Frames) Get */
					flagHasSetup = false;	/* recycling *//* Is animation "Setup"-animation ? */
					informationAnimation = ParseAnimation(	ref setting,
															out flagHasSetup,
															informationSSPJ,
															nodeAnimation,
															managerNameSpace,
															informationSSAE,
															indexAnimation,
															nameFile
														);
					if(null == informationAnimation)
					{
						goto Parse_ErrorEnd;
					}

					if(true == flagHasSetup)
					{	/* "Setup" animation */
						informationSSAE.AnimationSetup = informationAnimation;
					}
					else
					{	/* Normal animation */
						informationSSAE.TableAnimation[indexAnimation] = informationAnimation;
						indexAnimation++;
					}
				}

				/* Set secondary parameters */
				/* MEMO: This process is unnecessary, since "Setup" animation has only attributes' initial values. */
				for(int i=0; i<countAnimation; i++)
				{
					/* Solving Attributes */
					/* MEMO: Complement all animation-attributes' frame 0 key-data.                           */
					/*       Applying "Setup" animation and deleting useless key-datas is processed here.     */
					/*       Process after parse all animation data since "Setup" animation is not guaranteed */
					/*        defining at top of the animation-list in SSAE.                                  */
					informationSSAE.TableAnimation[i].AttributeSolve(informationSSPJ, informationSSAE, setting.Basic.FlagInvisibleToHideAll);

					/* Set Part-Status */
					/* MEMO: Analyze key-data and set each parts' usage status. */
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

					case "mask":
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK;
						break;

					case "joint":
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT;
						break;

					case "armature":
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ARMATURE;
						break;

					case "movenode":
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE;
						break;

					case "constraint":
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT;
						break;

					case "bonepoint":
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT;
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
						informationParts.Data.OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.MUL_NA;
						break;

					case "add":
						informationParts.Data.OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.ADD;
						break;

					case "sub":
						informationParts.Data.OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.SUB;
						break;

					case "mulalpha":
						/* MEMO: In SS5PU, "mul" was handled as Alpha-Multiply.                            */
						/*       From SS6PU, "mul" and "mulalpha" are devided so it is handled separately. */
						/*       ("mul": Non-Alpha-Multiply / "mulalpha": Alpha-Multiply)                  */
						informationParts.Data.OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.MUL;
						break;

					case "screen":
						informationParts.Data.OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.SCR;
						break;

					case "exclusion":
						informationParts.Data.OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.EXC;
						break;

					case "invert":
						informationParts.Data.OperationBlendTarget = Library_SpriteStudio6.KindOperationBlend.INV;
						break;

					default:
						LogWarning(messageLogPrefix, "Unknown Alpha-Blend Kind \"" + valueText + "\" Parts[" + indexParts.ToString() + "]", nameFileSSAE, informationSSPJ);
						goto case "mix";
				}

				/* Get Parts Show(Hide) */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "show", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
					informationParts.FlagHide = !(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText));	/* Show -> Hide */
				}
				else
				{	/* Legacy format */
					informationParts.FlagHide = false;
				}

				/* Get Mask-Targeting */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "maskInfluence", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
					informationParts.FlagMasking = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
				}
				else
				{	/* Legacy format */
					informationParts.FlagMasking = false;
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
							/* MEMO: CAUTION! Store only the name, because index of animation referring to can not be decided here. */
							/*       (Determined at "ModeSS6PU.ConvertData".)                                                       */
							informationParts.NameUnderControl = valueText;

							/* MEMO: Store animation names paired with references of undercontrol object("informationParts.Data.PrefabUnderControl") */
							/*        to other variable(not "informationParts.Data.NameAnimationUnderControl").                                      */
							/*       (Because I think that pair data should be stored in same place)                                                 */
							valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "refAnime", managerNameSpace);
							informationParts.NameAnimationUnderControl = (null != valueText) ? string.Copy(valueText) : "";
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
								/* MEMO: CAUTION! Store only the name, because index of animation referring to can not be decided here. */
								/*       (Determined at "ModeSS6PU.ConvertData".)                                                       */
								informationParts.NameUnderControl = string.Copy(valueText);
								informationParts.NameAnimationUnderControl = "";
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
					informationParts.Data.LabelColor.Set(Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.NON);
				}
				else
				{
					switch(valueText)
					{
						case "Red":
							informationParts.Data.LabelColor.Set(Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.RED);
							break;

						case "Orange":
							informationParts.Data.LabelColor.Set(Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.ORANGE);
							break;

						case "Yellow":
							informationParts.Data.LabelColor.Set(Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.YELLOW);
							break;

						case "Green":
							informationParts.Data.LabelColor.Set(Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.GREEN);
							break;

						case "Blue":
							informationParts.Data.LabelColor.Set(Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.BLUE);
							break;

						case "Violet":
							informationParts.Data.LabelColor.Set(Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.VIOLET);
							break;

						case "Gray":
							informationParts.Data.LabelColor.Set(Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.GRAY);
							break;

						default:
							{
								float ColorA;
								float ColorR;
								float ColorG;
								float ColorB;
								if(false == LibraryEditor_SpriteStudio6.Utility.Text.TextToColor(out ColorA, out ColorR, out ColorG, out ColorB, valueText))
								{
									LogWarning(messageLogPrefix, "Unknown Color-Label Kind \"" + valueText + "\" Parts[" + indexParts.ToString() + "]", nameFileSSAE, informationSSPJ);
									informationParts.Data.LabelColor.Set(Library_SpriteStudio6.Data.Parts.Animation.ColorLabel.KindForm.NON);
								}
								else
								{
									informationParts.Data.LabelColor.Set(new Color(ColorR, ColorG, ColorB, ColorA));
								}
							}
							break;
					}
				}

				return(informationParts);

			ParseParts_ErrorEnd:;
				return(null);
			}

			private static Information.Animation ParseAnimation(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																	out bool flagIsSetup,
																	LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																	System.Xml.XmlNode nodeAnimation,
																	System.Xml.XmlNamespaceManager managerNameSpace,
																	Information informationSSAE,
																	int indexAnimation,
																	string nameFileSSAE
																)
			{
				const string messageLogPrefix = "Parse SSAE(Animation)";

				flagIsSetup = false;

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

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAnimation, "isSetup", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
					if(0 < LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText))
					{
						flagIsSetup = true;
					}
				}

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

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAnimation, "settings/startFrame", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
					informationAnimation.Data.FrameValidStart = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
				}
				else
				{
					informationAnimation.Data.FrameValidStart = 0;
				}

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAnimation, "settings/endFrame", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
					informationAnimation.Data.FrameValidEnd = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
				}
				else
				{
					informationAnimation.Data.FrameValidEnd = informationAnimation.Data.CountFrame - 1;
				}

				informationAnimation.Data.CountFrameValid = (informationAnimation.Data.FrameValidEnd - informationAnimation.Data.FrameValidStart) + 1;

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAnimation, "settings/ik_depth", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
					informationAnimation.Data.DepthIK = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
				}
				else
				{
					informationAnimation.Data.DepthIK = 3;	/* Default */
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
					informationAnimation.TableParts[i] = new Information.Animation.Parts();
					if(null == informationAnimation.TableParts[i])
					{
						LogError(messageLogPrefix, "Not Enough Memory (Animation Attribute WorkArea) Animation-Name[" + indexAnimation.ToString() + "]", nameFileSSAE, informationSSPJ);
						goto ParseAnimation_ErrorEnd;
					}
					informationAnimation.TableParts[i].CleanUp();
					informationAnimation.TableParts[i].BootUp();
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
															flagIsSetup,
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
															bool flagIsSetup,
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
							case "LSCX":
								attributeFloat = informationAnimationParts.ScalingXLocal;
								goto case "_ValueFloat_";
							case "LSCY":
								attributeFloat = informationAnimationParts.ScalingYLocal;
								goto case "_ValueFloat_";
							case "ALPH":
								attributeFloat = informationAnimationParts.RateOpacity;
								goto case "_ValueFloat_";
							case "LALP":
								attributeFloat = informationAnimationParts.RateOpacityLocal;
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
							case "MASK":
								attributeFloat = informationAnimationParts.PowerMask;
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

							case "PCOL":
								{
									Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributePartsColor.KeyData data = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributePartsColor.KeyData();
									data.Value.CleanUp();
									data.Value.VertexColor = new Color[(int)Library_SpriteStudio6.KindVertex.TERMINATOR2];

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
											LogWarning(messageLogPrefix, "Unknown PartsColor-Operation \"" + valueText + "\" Frame[" + frame.ToString() + "] Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
											data.Value.Operation = Library_SpriteStudio6.KindOperationBlend.MIX;
											break;
									}

									float colorA = 0.0f;
									float colorR = 0.0f;
									float colorG = 0.0f;
									float colorB = 0.0f;
									valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeKey, "value/target", managerNameSpace);
									switch(valueText)
									{
										case "whole":
											{
												data.Value.Bound = Library_SpriteStudio6.KindBoundBlend.OVERALL;

												ParseAnimationAttributePartsColor(out colorA, out colorR, out colorG, out colorB, nodeKey, "value/color", managerNameSpace);
												for(int i=0; i<(int)Library_SpriteStudio6.KindVertex.TERMINATOR2; i++)
												{
													data.Value.VertexColor[i].r = colorR;
													data.Value.VertexColor[i].g = colorG;
													data.Value.VertexColor[i].b = colorB;
													data.Value.VertexColor[i].a = colorA;
												}
											}
											break;

										case "vertex":
											{
												data.Value.Bound = Library_SpriteStudio6.KindBoundBlend.VERTEX;

												ParseAnimationAttributePartsColor(out colorA, out colorR, out colorG, out colorB, nodeKey, "value/LT", managerNameSpace);
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LU].r = colorR;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LU].g = colorG;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LU].b = colorB;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LU].a = colorA;

												ParseAnimationAttributePartsColor(out colorA, out colorR, out colorG, out colorB, nodeKey, "value/RT", managerNameSpace);
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RU].r = colorR;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RU].g = colorG;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RU].b = colorB;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RU].a = colorA;

												ParseAnimationAttributePartsColor(out colorA, out colorR, out colorG, out colorB, nodeKey, "value/RB", managerNameSpace);
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RD].r = colorR;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RD].g = colorG;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RD].b = colorB;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.RD].a = colorA;

												ParseAnimationAttributePartsColor(out colorA, out colorR, out colorG, out colorB, nodeKey, "value/LB", managerNameSpace);
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LD].r = colorR;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LD].g = colorG;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LD].b = colorB;
												data.Value.VertexColor[(int)Library_SpriteStudio6.KindVertex.LD].a = colorA;
											}
											break;

										default:
											{
												LogWarning(messageLogPrefix, "Unknown PartsColor-Bound \"" + valueText + "\" Frame[" + frame.ToString() + "] Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
												data.Value.Bound = Library_SpriteStudio6.KindBoundBlend.OVERALL;
												for(int i=0; i<(int)Library_SpriteStudio6.KindVertex.TERMINATOR2; i++)
												{
													data.Value.VertexColor[i] = Library_SpriteStudio6.Data.Animation.Attribute.ColorClear;
												}
											}
											break;
									}

									/* Add Key-Data */
									informationAnimationParts.PartsColor.ListKey.Add(data);
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
								LogWarning(messageLogPrefix, "No-Longer-Used attribute \"" + tagText + "\" Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
								break;

							case "VCOL":
								LogWarning(messageLogPrefix, "Deprecated attribute \"Color Blend\" (Data is ignored. Please use \"Parts Color\") Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
								goto case "PCOL";
//								break;

							/* Unknown Attributes */
							default:
								LogWarning(messageLogPrefix, "Unknown Attribute \"" + tagText + "\" Animation-Name[" + informationAnimation.Data.Name + "]", nameFileSSAE, informationSSPJ);
								break;
						}
					}
				}

				return(true);

//			ParseAnimationAttribute_ErrorEnd:;
//				return(false);
			}
			private static void ParseAnimationAttributePartsColor(	out float colorA,
																	out float colorR,
																	out float colorG,
																	out float colorB,
																	System.Xml.XmlNode NodeKey,
																	string NameTagBase,
																	System.Xml.XmlNamespaceManager ManagerNameSpace
																)
			{
				string valueText = "";

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(NodeKey, NameTagBase + "/rgba", ManagerNameSpace);
				LibraryEditor_SpriteStudio6.Utility.Text.TextToColor(out colorA, out colorR, out colorG, out colorB, valueText);

				/* MEMO: Currently, discard "/rate" because meaningless parameter. */
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
				public Animation AnimationSetup;

				public Library_SpriteStudio6.Data.Animation.Parts.KindFormat FormatSS6PU;
				public LibraryEditor_SpriteStudio6.Import.Assets<Script_SpriteStudio6_DataAnimation> DataAnimationSS6PU;
				public LibraryEditor_SpriteStudio6.Import.Assets<Object> PrefabAnimationSS6PU;
				public string NameGameObjectAnimationSS6PU;
				public LibraryEditor_SpriteStudio6.Import.Assets<Object> PrefabControlAnimationSS6PU;
				public string NameGameObjectAnimationControlSS6PU;
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
					AnimationSetup = null;

					DataAnimationSS6PU.CleanUp();
					DataAnimationSS6PU.BootUp(1);	/* Always 1 */
					PrefabAnimationSS6PU.CleanUp();
					PrefabAnimationSS6PU.BootUp(1);	/* Always 1 */
					NameGameObjectAnimationSS6PU = "";
					PrefabControlAnimationSS6PU.CleanUp();
					PrefabControlAnimationSS6PU.BootUp(1);	/* Always 1 */
					NameGameObjectAnimationControlSS6PU = "";
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

					public bool FlagHide;
					public bool FlagMasking;

					/* MEMO: UnderControl == Instance, Effect */
					public string NameUnderControl;
					public string NameAnimationUnderControl;
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

						FlagHide = false;
						FlagMasking = false;

						NameUnderControl = "";
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

					public bool AttributeSolve(	LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
												Information informationSSAE,
												bool flagInvisibleToHideAll
											)
					{
						int countParts = TableParts.Length;
						Parts animationParts = null;
						Parts animationPartsSetup = null;
						LibraryEditor_SpriteStudio6.Import.SSAE.Information.Parts parts = null;

						for(int i=0; i<countParts; i++)
						{
							parts = informationSSAE.TableParts[i];
							animationParts = TableParts[i];

							/* Get Setup-Animation */
							if(null != informationSSAE.AnimationSetup)
							{	/* Has Setup animation */
								if(null != informationSSAE.AnimationSetup.TableParts)
								{	/* Has Animation-Parts table */
									animationPartsSetup = informationSSAE.AnimationSetup.TableParts[i];
								}
							}

							/* Adjust Top-Frame Key-Data */
							animationParts.Cell.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.Cell);

							animationParts.PositionX.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.PositionX);
							animationParts.PositionY.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.PositionY);
							animationParts.PositionZ.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.PositionZ);

							animationParts.RotationX.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.RotationX);
							animationParts.RotationY.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.RotationY);
							animationParts.RotationZ.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.RotationZ);

							animationParts.ScalingX.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.ScalingX);
							animationParts.ScalingY.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.ScalingY);

							animationParts.ScalingXLocal.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.ScalingXLocal);
							animationParts.ScalingYLocal.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.ScalingYLocal);

							animationParts.RateOpacity.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.RateOpacity);
							animationParts.RateOpacityLocal.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.RateOpacityLocal);
							animationParts.Priority.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.Priority);

							animationParts.FlipX.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.FlipX);
							animationParts.FlipY.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.FlipY);

							if((true == flagInvisibleToHideAll) && (true == parts.FlagHide))
							{	/* Parts Hide (for Editing), convert to All-Frame-Hide */
								animationParts.Hide.CleanUpKey();

								Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool.KeyData data = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool.KeyData();
								data.Formula = Library_SpriteStudio6.Utility.Interpolation.KindFormula.NON;
								data.FrameCurveStart = 0.0f;
								data.ValueCurveStart = 0.0f;
								data.FrameCurveEnd = 0.0f;
								data.ValueCurveEnd = 0.0f;
								data.Frame = 0;
								data.Value = true;
								animationParts.Hide.ListKey.Add(data);
							}
							else
							{
								animationParts.Hide.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.Hide, true, true, false);	/* "Hide" is true for the top-frames without key data.(not value of first key to appear) */
							}

							animationParts.PartsColor.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.PartsColor);

							animationParts.VertexCorrection.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.VertexCorrection);

							animationParts.PivotOffsetX.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.PivotOffsetX);
							animationParts.PivotOffsetY.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.PivotOffsetY);

							animationParts.AnchorPositionX.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.AnchorPositionX);
							animationParts.AnchorPositionY.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.AnchorPositionY);
							animationParts.SizeForceX.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.SizeForceX);
							animationParts.SizeForceY.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.SizeForceY);

							animationParts.TexturePositionX.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.TexturePositionX);
							animationParts.TexturePositionY.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.TexturePositionY);
							animationParts.TextureRotation.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.TextureRotation);
							animationParts.TextureScalingX.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.TextureScalingX);
							animationParts.TextureScalingY.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.TextureScalingY);
							animationParts.TextureFlipX.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.TextureFlipX);
							animationParts.TextureFlipY.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.TextureFlipY);

							animationParts.RadiusCollision.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.RadiusCollision);
							animationParts.PowerMask.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.PowerMask);

							/* MEMO: UserData does not complement 0 frame. */
//							animationParts.UserData.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.UserData, Library_SpriteStudio6.Data.Animation.Attribute.DefaultUseData, false, true);
							/* MEMO: Do not set at here. Set in processing for each part type. */
//							animationParts.Instance.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.Instance);
//							animationParts.Effect.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.Effect);

							/* Delete attributes that should not exist */
							animationParts.AnchorPositionX.ListKey.Clear();	/* Unsupported */
							animationParts.AnchorPositionY.ListKey.Clear();	/* Unsupported */
							switch(parts.Data.Feature)
							{
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
									animationParts.Cell.ListKey.Clear();

									animationParts.PartsColor.ListKey.Clear();
									animationParts.VertexCorrection.ListKey.Clear();

									animationParts.PivotOffsetX.ListKey.Clear();
									animationParts.PivotOffsetY.ListKey.Clear();

									animationParts.SizeForceX.ListKey.Clear();
									animationParts.SizeForceY.ListKey.Clear();

									animationParts.TexturePositionX.ListKey.Clear();
									animationParts.TexturePositionY.ListKey.Clear();
									animationParts.TextureRotation.ListKey.Clear();
									animationParts.TextureScalingX.ListKey.Clear();
									animationParts.TextureScalingY.ListKey.Clear();
									animationParts.TextureFlipX.ListKey.Clear();
									animationParts.TextureFlipY.ListKey.Clear();

									animationParts.PowerMask.ListKey.Clear();

									animationParts.Instance.ListKey.Clear();
									animationParts.Effect.ListKey.Clear();
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
									if(0 >= animationParts.VertexCorrection.CountGetKey())
									{
										parts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2;
									}
									else
									{
										parts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4;
									}

									animationParts.PowerMask.ListKey.Clear();

									animationParts.Instance.ListKey.Clear();
									animationParts.Effect.ListKey.Clear();
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
									animationParts.Cell.ListKey.Clear();

									animationParts.FlipX.ListKey.Clear();
									animationParts.FlipY.ListKey.Clear();

									animationParts.PartsColor.ListKey.Clear();
									animationParts.VertexCorrection.ListKey.Clear();

									animationParts.PivotOffsetX.ListKey.Clear();
									animationParts.PivotOffsetY.ListKey.Clear();

									animationParts.SizeForceX.ListKey.Clear();
									animationParts.SizeForceY.ListKey.Clear();

									animationParts.TexturePositionX.ListKey.Clear();
									animationParts.TexturePositionY.ListKey.Clear();
									animationParts.TextureRotation.ListKey.Clear();
									animationParts.TextureScalingX.ListKey.Clear();
									animationParts.TextureScalingY.ListKey.Clear();
									animationParts.TextureFlipX.ListKey.Clear();
									animationParts.TextureFlipY.ListKey.Clear();

									animationParts.PowerMask.ListKey.Clear();

									/* MEMO: In "Animation.StatusSetParts" function to be executed later,                               */
									/*        if all frames are hide status, judge that "Instance" are not used and erase all key data. */
									/*       Complement key-data for now.                                                               */
									animationParts.Instance.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.Instance, Library_SpriteStudio6.Data.Animation.Attribute.DefaultInstance, false, false);

									animationParts.Effect.ListKey.Clear();
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
									animationParts.Cell.ListKey.Clear();

									animationParts.FlipX.ListKey.Clear();
									animationParts.FlipY.ListKey.Clear();

									animationParts.PartsColor.ListKey.Clear();
									animationParts.VertexCorrection.ListKey.Clear();

									animationParts.PivotOffsetX.ListKey.Clear();
									animationParts.PivotOffsetY.ListKey.Clear();

									animationParts.SizeForceX.ListKey.Clear();
									animationParts.SizeForceY.ListKey.Clear();

									animationParts.TexturePositionX.ListKey.Clear();
									animationParts.TexturePositionY.ListKey.Clear();
									animationParts.TextureRotation.ListKey.Clear();
									animationParts.TextureScalingX.ListKey.Clear();
									animationParts.TextureScalingY.ListKey.Clear();
									animationParts.TextureFlipX.ListKey.Clear();
									animationParts.TextureFlipY.ListKey.Clear();

									animationParts.PowerMask.ListKey.Clear();

									/* MEMO: In "Animation.StatusSetParts" function to be executed later,                             */
									/*        if all frames are hide status, judge that "Effect" are not used and erase all key data. */
									/*       Complement key-data for now.                                                             */
									animationParts.Effect.KeyDataAdjustTopFrame((null == animationPartsSetup) ? null : animationPartsSetup.Effect, Library_SpriteStudio6.Data.Animation.Attribute.DefaultEffect, false, false);

									animationParts.Instance.ListKey.Clear();
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4:
									if(0 >= animationParts.VertexCorrection.CountGetKey())
									{
										parts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2;
									}
									else
									{
										parts.Data.Feature = Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4;
									}

									animationParts.Instance.ListKey.Clear();
									animationParts.Effect.ListKey.Clear();
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
									break;
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ARMATURE:
									break;
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
								break;
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
								break;
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
								break;

								default:
									break;
							}
						}

						return(true);
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

							/* Set Masking */
							/* MEMO: Caution that flag is inverted. (Difference between "Mask" and "No-Mask") */
							if(true == informationSSAE.TableParts[i].FlagMasking)
							{
								animationParts.StatusParts &= ~Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_MASKING;
							}
							else
							{
								animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_MASKING;
							}

							bool flagInUse = false;

							/* Check Transform */
							if((0 >= animationParts.PositionX.CountGetKey())
								&& (0 >= animationParts.PositionY.CountGetKey())
								&& (0 >= animationParts.PositionZ.CountGetKey())
								)
							{
								animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION;
							}
							else
							{
								animationParts.StatusParts &= ~Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION;
								flagInUse |= true;
							}

							if((0 >= animationParts.RotationX.CountGetKey())
								&& (0 >= animationParts.RotationY.CountGetKey())
								&& (0 >= animationParts.RotationZ.CountGetKey())
								)
							{
								animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION;
							}
							else
							{
								animationParts.StatusParts &= ~Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION;
								flagInUse |= true;
							}

							if((0 >= animationParts.ScalingX.CountGetKey())
								&& (0 >= animationParts.ScalingY.CountGetKey())
								)
							{
								animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING;
							}
							else
							{
								animationParts.StatusParts &= ~Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING;
								flagInUse |= true;
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
									break;
								}
							}
							if(true == flagHideAll)
							{
								animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.HIDE_FULL;

								/* MEMO: Set "Instance" and "Effect" to unused */
								animationParts.Instance.CleanUpKey();
								animationParts.Effect.CleanUpKey();
							}
							else
							{
								animationParts.StatusParts &= ~Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.HIDE_FULL;
								flagInUse |= true;
							}

							/* Check Texture-Transform */
							if((0 >= animationParts.TexturePositionX.CountGetKey())
								&& (0 >= animationParts.TexturePositionY.CountGetKey())
								&& (0 >= animationParts.TextureScalingX.CountGetKey())
								&& (0 >= animationParts.TextureScalingY.CountGetKey())
								&& (0 >= animationParts.TextureRotation.CountGetKey())
								&& (0 >= animationParts.TextureFlipX.CountGetKey())
								&& (0 >= animationParts.TextureFlipY.CountGetKey())
								)
							{
								animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_TRANSFORMATION_TEXTURE;
							}
							else
							{
								animationParts.StatusParts &= ~Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_TRANSFORMATION_TEXTURE;
								flagInUse |= true;
							}

							/* Check UserData */
							if(0 >= animationParts.UserData.CountGetKey())
							{
								animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_USERDATA;
							}
							else
							{
								animationParts.StatusParts &= ~Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_USERDATA;
								flagInUse |= true;
							}

							/* Check PartsColor */
							if(0 >= animationParts.PartsColor.CountGetKey())
							{
								animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_PARTSCOLOR;
							}
							else
							{
								animationParts.StatusParts &= ~Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_PARTSCOLOR;
								flagInUse |= true;
							}

							/* Other Attribute */
							flagInUse |= (0 >= animationParts.Cell.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.ScalingXLocal.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.ScalingYLocal.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.RateOpacity.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.RateOpacityLocal.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.Priority.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.FlipX.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.FlipY.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.VertexCorrection.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.PivotOffsetX.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.PivotOffsetY.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.AnchorPositionX.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.AnchorPositionY.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.SizeForceX.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.SizeForceY.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.RadiusCollision.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.PowerMask.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.Instance.CountGetKey()) ? true : false;
							flagInUse |= (0 >= animationParts.Effect.CountGetKey()) ? true : false;

							/* MEMO: Attributes for Fix format absolutely do not have data at this point. */
//							flagInUse |= (0 >= animationParts.FixIndexCellMap.CountGetKey()) ? true : false;
//							flagInUse |= (0 >= animationParts.FixCoordinate.CountGetKey()) ? true : false;
//							flagInUse |= (0 >= animationParts.FixUV.CountGetKey()) ? true : false;
//							flagInUse |= (0 >= animationParts.FixSizeCollisionX.CountGetKey()) ? true : false;
//							flagInUse |= (0 >= animationParts.FixSizeCollisionY.CountGetKey()) ? true : false;
//							flagInUse |= (0 >= animationParts.Effect.FixPivotCollisionX()) ? true : false;
//							flagInUse |= (0 >= animationParts.Effect.FixPivotCollisionY()) ? true : false;

							if(false == flagInUse)
							{	/* Not Use */
								animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED;

								/* MEMO: Just in case. Set flags when parts is not used. */
								/*       (No possibility of having key-datas)            */
								animationParts.StatusParts |= (	Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_POSITION
																| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_ROTATION
																| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_SCALING
																| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.HIDE_FULL
																| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_TRANSFORMATION_TEXTURE
																| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_USERDATA
																| Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NO_PARTSCOLOR
															);
							}
							else
							{
								animationParts.StatusParts &= ~Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED;
							}

							/* Set Valid */
							animationParts.StatusParts |= Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.VALID;
						}
						return(true);

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
						/* MEMO: "Draw" is for normal drawing.                                                   */
						/*       "PreDraw" is for drawing initial mask.                                          */
						/*        When focusing on "Mask" parts only, "Draw" and "PreDraw" are in reverse order. */
						List<int> listIndexPartsDraw = new List<int>(countParts);
						listIndexPartsDraw.Clear();
						float[][] tableDrawPriority = new float[countParts][];
						bool flagAddList = false;
						for(int i=0; i<countParts; i++)
						{
							animationParts = TableParts[i];
							parts = informationSSAE.TableParts[i];
							tableDrawPriority[i] = null;

							flagAddList = false;
							switch(parts.Data.Feature)
							{
								/* Non draw parts */
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
									/* MEMO: Create table in "Root"part so that can get first drawing part's index. (for "Draw" and "PreDraw") */
									animationParts.TableOrderDraw = new int[countFrame];
									animationParts.TableOrderPreDraw = new int[countFrame];
									for(int j=0; j<countFrame; j++)
									{
										animationParts.TableOrderDraw[j] = -1;
										animationParts.TableOrderPreDraw[j] = -1;
									}
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
									break;

								/* Draw parts */
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
									/* MEMO: Normal rendering parts only draw on "Draw". */
									/* Create Draw-Order table */
									animationParts.TableOrderDraw = new int[countFrame];
									for(int j=0; j<countFrame; j++)
									{
										animationParts.TableOrderDraw[j] = -1;
									}

									flagAddList = true;
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
									/* Create Draw-Order table */
									/* MEMO:  Since "Mask" draws twice on "Draw" and "PreDraw", both tables are necessary. */
									animationParts.TableOrderDraw = new int[countFrame];
									animationParts.TableOrderPreDraw = new int[countFrame];
									for(int j=0; j<countFrame; j++)
									{
										animationParts.TableOrderDraw[j] = -1;
										animationParts.TableOrderPreDraw[j] = -1;
									}

									flagAddList = true;
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ARMATURE:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
									break;

								default:
									/* MEMO: No reach here. */
									break;
							}
							if(true == flagAddList)
							{
								/* Calculate all frames' priority. */
								tableDrawPriority[i] = new float[countFrame];
								switch(ModeSort)
								{
									case KindModeSort.PRIORITY:
										DrawOrderCreatePriority(ref tableDrawPriority[i], informationSSPJ, informationSSAE, this, animationParts);
										break;

									case KindModeSort.POSITION_Z:
//										DrawOrderCreatePositionZ(ref tableDrawPriority[i], informationSSPJ, informationSSAE, this, animationParts);
										break;
								}

								/* Add as part to be processed */
								listIndexPartsDraw.Add(i);
							}
						}

						/* Decide Draw-Order Table */
						/* MEMO: "Root"part is excluded from target. */
						int countIndexPartsDraw = listIndexPartsDraw.Count;
						List<int> listIndexPartsSort = new List<int>(countIndexPartsDraw);
						listIndexPartsSort.Clear();
						List<int> listIndexPartsSortPreDraw = new List<int>(countIndexPartsDraw);
						listIndexPartsSortPreDraw.Clear();
						List<float> listPrioritySort = new List<float>(countIndexPartsDraw);
						listPrioritySort.Clear();
						for(int frame=0; frame<countFrame; frame++)
						{
							/* Extract draw parts (in this frame) */
							for(int i=0; i<countIndexPartsDraw; i++)
							{
								int indexParts = listIndexPartsDraw[i];
								parts = informationSSAE.TableParts[indexParts];
								animationParts = TableParts[indexParts];
								if(0 == (animationParts.StatusParts & Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.HIDE_FULL))
								{
									switch(parts.Data.Feature)
									{
										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
											/* MEMO: No reach here. */
											break;

										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
											/* MEMO: Not be added to list at when hide state. */
											if(false == animationParts.TableHide[frame])
											{
												listIndexPartsSort.Add(indexParts);
												listPrioritySort.Add(tableDrawPriority[indexParts][frame]);
											}
											break;

										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
											/* MEMO: "Instance"-parts and "Effect"-parts are always updated regardless of hide state, so unconditionally added to list. */
											listIndexPartsSort.Add(indexParts);
											listPrioritySort.Add(tableDrawPriority[indexParts][frame]);
											break;

										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2:
										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4:
										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
											/* MEMO: Not be added to list at when hide state. */
											if(false == animationParts.TableHide[frame])
											{
												listIndexPartsSort.Add(indexParts);
												listPrioritySort.Add(tableDrawPriority[indexParts][frame]);
											}
											break;

										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ARMATURE:
										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
										case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
											break;

										default:
											/* MEMO: No reach here. */
											break;
									}
								}
							}

							/* Sort (Bubble) */
							/* When the same priority, parts that has larger ID (part-index) drawed later. */
							int countIndexPartsSort = listIndexPartsSort.Count;
							for(int i=0; i<(countIndexPartsSort - 1); i++)
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

							/* Create Order for "PreDraw" */
							/* MEMO: Enough to add in reverse order  to "listIndexPartsSortPreDraw",           */
							/*        since "listIndexPartsSort" has already been sorted in the drawing order. */
							listIndexPartsSortPreDraw.Clear();
							for(int i=(countIndexPartsSort-1); i>=0 ; i--)
							{
								int indexParts = listIndexPartsSort[i];
								parts = informationSSAE.TableParts[indexParts];
								animationParts = TableParts[indexParts];
									switch(parts.Data.Feature)
								{
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
										/* MEMO: No reach here. */
										break;

									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL:
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
										break;

									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2:
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4:
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK:
										/* MEMO: Add "Mask"parts only */
										listIndexPartsSortPreDraw.Add(indexParts);
										break;

									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ARMATURE:
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
									case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
										break;

									default:
										/* MEMO: No reach here. */
										break;
								}
							}

							/* Set Order for "Draw" */
							/* MEMO: In "Root"part, first-drawing part's index is stored. */
							TableParts[0].TableOrderDraw[frame] = (0 < countIndexPartsSort) ? listIndexPartsSort[0] : -1;
							for(int i=1; i<countIndexPartsSort; i++)
							{
								TableParts[listIndexPartsSort[i - 1]].TableOrderDraw[frame] = listIndexPartsSort[i];
							}

							/* Set Order for "PreDraw" */
							countIndexPartsSort = listIndexPartsSortPreDraw.Count;
							TableParts[0].TableOrderPreDraw[frame] = (0 < countIndexPartsSort) ? listIndexPartsSortPreDraw[0] : -1;
							for(int i=1; i<countIndexPartsSort; i++)
							{
								TableParts[listIndexPartsSortPreDraw[i - 1]].TableOrderPreDraw[frame] = listIndexPartsSortPreDraw[i];
							}

							listIndexPartsSort.Clear();
							listIndexPartsSortPreDraw.Clear();
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
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat ScalingXLocal;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat ScalingYLocal;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat RateOpacity;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat RateOpacityLocal;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt Priority;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool FlipX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool FlipY;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool Hide;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributePartsColor PartsColor;
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
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat PowerMask;	/* AttributeInt */

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUserData UserData;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInstance Instance;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeEffect Effect;

						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt FixIndexCellMap;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeCoordinateFix FixCoordinate;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeUVFix FixUV;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat FixSizeCollisionX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat FixSizeCollisionY;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat FixPivotCollisionX;
						public Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat FixPivotCollisionY;

						public Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus StatusParts;
						public bool[] TableHide;	/* Expand "Hide"attribute in order to drawing state optimize. */
						public int[] TableOrderDraw;
						public int[] TableOrderPreDraw;

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
							ScalingXLocal = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							ScalingXLocal.CleanUp();
							ScalingYLocal = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							ScalingYLocal.CleanUp();

							RateOpacity = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							RateOpacity.CleanUp();
							RateOpacityLocal = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							RateOpacityLocal.CleanUp();
							Priority = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeInt();
							Priority.CleanUp();

							FlipX = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool();
							FlipX.CleanUp();
							FlipY = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool();
							FlipY.CleanUp();
							Hide = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeBool();
							Hide.CleanUp();

							PartsColor = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributePartsColor();
							PartsColor.CleanUp();
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
							PowerMask = new Library_SpriteStudio6.Data.Animation.Attribute.Importer.AttributeFloat();
							PowerMask.CleanUp();

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
							TableOrderPreDraw = null;
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
							ScalingXLocal.BootUp();
							ScalingYLocal.BootUp();

							RateOpacity.BootUp();
							RateOpacityLocal.BootUp();
							Priority.BootUp();

							FlipX.BootUp();
							FlipY.BootUp();
							Hide.BootUp();

							PartsColor.BootUp();
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
							PowerMask.BootUp();

							UserData.BootUp();

							Instance.BootUp();
							Effect.BootUp();

							FixIndexCellMap.BootUp();
							FixCoordinate.BootUp();
							FixUV.BootUp();
							FixSizeCollisionX.BootUp();
							FixSizeCollisionY.BootUp();
							FixPivotCollisionX.BootUp();
							FixPivotCollisionY.BootUp();

							StatusParts = Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.NOT_USED;
							TableHide = null;
							TableOrderDraw = null;
							TableOrderPreDraw = null;

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
							ScalingXLocal.ShutDown();
							ScalingYLocal.ShutDown();

							RateOpacity.ShutDown();
							RateOpacityLocal.ShutDown();
							Priority.ShutDown();

							FlipX.ShutDown();
							FlipY.ShutDown();
							Hide.ShutDown();

							PartsColor.ShutDown();
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
							PowerMask.ShutDown();

							UserData.ShutDown();

							Instance.ShutDown();
							Effect.ShutDown();

							FixIndexCellMap.ShutDown();
							FixCoordinate.ShutDown();
							FixUV.ShutDown();
							FixSizeCollisionX.ShutDown();
							FixSizeCollisionY.ShutDown();
							FixPivotCollisionX.ShutDown();
							FixPivotCollisionY.ShutDown();

							StatusParts = Library_SpriteStudio6.Data.Animation.Parts.FlagBitStatus.CLEAR;
							TableHide = null;
							TableOrderDraw = null;
							TableOrderPreDraw = null;
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
													Script_SpriteStudio6_Root prefabOverride,
													GameObject prefabOverrideControl
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
						informationSSAE.NameGameObjectAnimationSS6PU = string.Copy(prefabOverride.name);

						informationSSAE.PrefabAnimationSS6PU.TableName[0] = AssetDatabase.GetAssetPath(prefabOverride);
						informationSSAE.PrefabAnimationSS6PU.TableData[0] = prefabOverride;
					}
					else
					{	/* Default */
						informationSSAE.NameGameObjectAnimationSS6PU = setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_SS6PU, informationSSAE.NameFileBody, informationSSPJ.NameFileBody);

						informationSSAE.PrefabAnimationSS6PU.TableName[0] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_ANIMATION_SS6PU, nameOutputAssetFolderBase)
																			+ informationSSAE.NameGameObjectAnimationSS6PU
																			+ LibraryEditor_SpriteStudio6.Import.NameExtensionPrefab;
						informationSSAE.PrefabAnimationSS6PU.TableData[0] = AssetDatabase.LoadAssetAtPath<GameObject>(informationSSAE.PrefabAnimationSS6PU.TableName[0]);
					}

					if(null != prefabOverrideControl)
					{	/* Specified */
						informationSSAE.NameGameObjectAnimationSS6PU = string.Copy(prefabOverrideControl.name);

						informationSSAE.PrefabControlAnimationSS6PU.TableName[0] = AssetDatabase.GetAssetPath(prefabOverrideControl);
						informationSSAE.PrefabControlAnimationSS6PU.TableData[0] = prefabOverrideControl;
					}
					else
					{	/* Default */
						informationSSAE.NameGameObjectAnimationControlSS6PU = setting.RuleNameAsset.NameGetAsset(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_CONTROL_ANIMATION_SS6PU, informationSSAE.NameFileBody, informationSSPJ.NameFileBody);

						informationSSAE.PrefabControlAnimationSS6PU.TableName[0] = setting.RuleNameAssetFolder.NameGetAssetFolder(LibraryEditor_SpriteStudio6.Import.Setting.KindAsset.PREFAB_CONTROL_ANIMATION_SS6PU, nameOutputAssetFolderBase)
																					+ informationSSAE.NameGameObjectAnimationControlSS6PU
																					+ LibraryEditor_SpriteStudio6.Import.NameExtensionPrefab;
						informationSSAE.PrefabControlAnimationSS6PU.TableData[0] = AssetDatabase.LoadAssetAtPath<GameObject>(informationSSAE.PrefabControlAnimationSS6PU.TableName[0]);
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

					dataAnimation.TableMaterial = informationSSPJ.TableMaterialAnimationSS6PU;	/* Back up original */

					EditorUtility.SetDirty(dataAnimation);
					AssetDatabase.SaveAssets();

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

					GameObject gameObjectRoot = null;
					Script_SpriteStudio6_Root scriptRoot = null;
					Script_SpriteStudio6_Root.InformationPlay[] informationPlayRoot = null;
					string[] nameAnimation = null;
					bool flagHideForce = false;
					int limitTrack = 0;
					int indexAnimation;

					/* Create? Update? */
					if(null == informationSSAE.PrefabAnimationSS6PU.TableData[0])
					{	/* New */
						informationSSAE.PrefabAnimationSS6PU.TableData[0] = PrefabUtility.CreateEmptyPrefab(informationSSAE.PrefabAnimationSS6PU.TableName[0]);
						if(null == informationSSAE.PrefabAnimationSS6PU.TableData[0])
						{
							LogError(messageLogPrefix, "Failure to create Prefab", informationSSAE.FileNameGetFullPath(), informationSSPJ);
							goto AssetCreatePrefab_ErrorEnd;
						}
					}
					else
					{	/* Exist */
						/* MEMO: Do not instantiate old prefabs. Instantiates up to objects under control, and mixed in updated prefab. */
						gameObjectRoot = (GameObject)informationSSAE.PrefabAnimationSS6PU.TableData[0];
						scriptRoot = gameObjectRoot.GetComponent<Script_SpriteStudio6_Root>();
						if(null != scriptRoot)
						{
							flagHideForce = scriptRoot.FlagHideForce;
							limitTrack = scriptRoot.LimitTrack;

							if(null != scriptRoot.TableInformationPlay)
							{
								int countInformationPlay = scriptRoot.TableInformationPlay.Length;
								if(0 < countInformationPlay)
								{
									informationPlayRoot = new Script_SpriteStudio6_Root.InformationPlay[countInformationPlay];
									nameAnimation = new string[countInformationPlay];
									if((null == informationPlayRoot) || (null == nameAnimation))
									{
										LogError(messageLogPrefix, "Not Enough Memory (Play-Information BackUp)", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto AssetCreatePrefab_ErrorEnd;
									}

									for(int i=0; i<countInformationPlay; i++)
									{
										informationPlayRoot[i].FlagSetInitial = scriptRoot.TableInformationPlay[i].FlagSetInitial;
										informationPlayRoot[i].FlagStopInitial = scriptRoot.TableInformationPlay[i].FlagStopInitial;

										nameAnimation[i] = scriptRoot.TableInformationPlay[i].NameAnimation;
										informationPlayRoot[i].NameAnimation = "";
										informationPlayRoot[i].FlagPingPong = scriptRoot.TableInformationPlay[i].FlagPingPong;
										informationPlayRoot[i].LabelStart = (false == string.IsNullOrEmpty(scriptRoot.TableInformationPlay[i].LabelStart)) ? string.Copy(scriptRoot.TableInformationPlay[i].LabelStart) : "";
										informationPlayRoot[i].FrameOffsetStart = scriptRoot.TableInformationPlay[i].FrameOffsetStart;
										informationPlayRoot[i].LabelEnd = (false == string.IsNullOrEmpty(scriptRoot.TableInformationPlay[i].LabelEnd)) ? string.Copy(scriptRoot.TableInformationPlay[i].LabelEnd) : "";
										informationPlayRoot[i].FrameOffsetEnd = scriptRoot.TableInformationPlay[i].FrameOffsetEnd;
										informationPlayRoot[i].Frame = scriptRoot.TableInformationPlay[i].Frame;
										informationPlayRoot[i].TimesPlay = scriptRoot.TableInformationPlay[i].TimesPlay;
										informationPlayRoot[i].RateTime = scriptRoot.TableInformationPlay[i].RateTime;
									}
								}
							}
						}

						gameObjectRoot = null;
						scriptRoot = null;
					}
					if(null == informationPlayRoot)
					{
						informationPlayRoot = new Script_SpriteStudio6_Root.InformationPlay[1];
						nameAnimation = new string[1];
						if(null == informationPlayRoot)
						{
							LogError(messageLogPrefix, "Not Enough Memory (Play-Information BackUp)", informationSSAE.FileNameGetFullPath(), informationSSPJ);
							goto AssetCreatePrefab_ErrorEnd;
						}

						nameAnimation[0] = "";
						informationPlayRoot[0].CleanUp();
						informationPlayRoot[0].FlagSetInitial = true;
					}

					/* Create new GameObject (Root) */
					gameObjectRoot = Library_SpriteStudio6.Utility.Asset.GameObjectCreate(informationSSAE.NameGameObjectAnimationSS6PU, false, null);	/* informationSSAE.NameFileBody */
					if(null == gameObjectRoot)
					{
						LogError(messageLogPrefix, "Failure to get Temporary-GameObject", informationSSAE.FileNameGetFullPath(), informationSSPJ);
						goto AssetCreatePrefab_ErrorEnd;
					}
//					gameObjectRoot.name = informationSSAE.NameGameObjectAnimationSS6PU;	/* informationSSAE.NameFileBody; */	/* Give Root same name as SSAE */
					scriptRoot = gameObjectRoot.AddComponent<Script_SpriteStudio6_Root>();
					if(null == scriptRoot)
					{
						LogError(messageLogPrefix, "Failure to add component\"Script_SpriteStudio6_Root\"", informationSSAE.FileNameGetFullPath(), informationSSPJ);
						goto AssetCreatePrefab_ErrorEnd;
					}

					/* Make GameObject & Parts controllers */
					/* MEMO: Should be stored keeping permutation of parent and child. */
					int countParts = informationSSAE.TableParts.Length;
					Library_SpriteStudio6.Control.Animation.Parts[] tableControlParts = new Library_SpriteStudio6.Control.Animation.Parts[countParts];
					tableControlParts[0].InstanceGameObject = gameObjectRoot;

					GameObject gameObjectParent = null;
					GameObject gameObjectParts = null;
					Script_SpriteStudio6_Collider scriptCollider = null;
					int indexPartsParent;
					bool flagAttachCollider;
					for(int i=0; i<countParts; i++)
					{
						if(0 >= i)
						{	/* "Root" */
							indexPartsParent = -1;
							gameObjectParent = null;
							gameObjectParts = gameObjectRoot;
//							gameObjectParts.name = 
//							tableControlParts[0].InstanceGameObject = gameObjectParts;
						}
						else
						{	/* Not "Root" */
							indexPartsParent = informationSSAE.TableParts[i].Data.IDParent;
							gameObjectParent = (0 <= indexPartsParent) ? tableControlParts[indexPartsParent].InstanceGameObject : null;
							gameObjectParts = Library_SpriteStudio6.Utility.Asset.GameObjectCreate(informationSSAE.TableParts[i].Data.Name, true, gameObjectParent);
							gameObjectParts.name = informationSSAE.TableParts[i].Data.Name;
							tableControlParts[i].InstanceGameObject = gameObjectParts;
						}

						scriptCollider = null;
						flagAttachCollider = false;
						if(null != gameObjectParts)
						{
							switch(informationSSAE.TableParts[i].Data.ShapeCollision)
							{
								case Library_SpriteStudio6.Data.Parts.Animation.KindCollision.NON:
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindCollision.SQUARE:
									/* Attach Script */
									scriptCollider = gameObjectParts.AddComponent<Script_SpriteStudio6_Collider>();
									tableControlParts[i].InstanceScriptCollider = scriptCollider;
									if(null != scriptCollider)
									{
										scriptCollider.InstanceRoot = scriptRoot;
										scriptCollider.IDParts = i;

										BoxCollider collider = gameObjectParts.AddComponent<BoxCollider>();
										if(null != collider)
										{
											collider.enabled = true;
											collider.size = new Vector3(1.0f, 1.0f, setting.Collider.SizeZ);
											collider.center = Vector2.zero;
											collider.isTrigger = false;

											scriptCollider.InstanceColliderBox = collider;

											flagAttachCollider = true;
										}
									}
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindCollision.AABB:
									/* MEMO: Not Supported */
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindCollision.CIRCLE:
									scriptCollider = gameObjectParts.AddComponent<Script_SpriteStudio6_Collider>();
									tableControlParts[i].InstanceScriptCollider = scriptCollider;
									if(null != scriptCollider)
									{
										scriptCollider.InstanceRoot = scriptRoot;
										scriptCollider.IDParts = i;

										CapsuleCollider collider = gameObjectParts.AddComponent<CapsuleCollider>();
										if(null != collider)
										{
											collider.enabled = true;
											collider.radius = 1.0f;
											collider.height = setting.Collider.SizeZ;
											collider.direction = 2;
											collider.isTrigger = false;

											scriptCollider.InstanceColliderCapsule = collider;

											flagAttachCollider = true;
										}
									}
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindCollision.CIRCLE_SCALEMINIMUM:
									/* MEMO: Not Supported */
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindCollision.CIRCLE_SCALEMAXIMUM:
									/* MEMO: Not Supported */
									break;
							}
							if(true == flagAttachCollider)
							{
								if(true == setting.Collider.FlagAttachRigidBody)
								{
									/* Attach Rigid-Body */
									Rigidbody rigidbody = gameObjectParts.AddComponent<Rigidbody>();
									rigidbody.isKinematic = false;
									rigidbody.useGravity = false;
								}
							}
							else
							{
								if(null != scriptCollider)
								{
									/* Remove Script */
									UnityEngine.Object.Destroy(scriptCollider);
								}
							}
						}
					}

					/* Datas Set */
					scriptRoot.DataCellMap = informationSSPJ.DataCellMapSS6PU.TableData[0];
					scriptRoot.DataAnimation = informationSSAE.DataAnimationSS6PU.TableData[0];
					scriptRoot.TableMaterial = informationSSPJ.TableMaterialAnimationSS6PU;
					scriptRoot.LimitTrack = limitTrack;
					scriptRoot.TableControlParts = tableControlParts;
					tableControlParts = null;

					scriptRoot.FlagHideForce = flagHideForce;

					int countLimitTrack = scriptRoot.LimitGetTrack();
					scriptRoot.TableInformationPlay = new Script_SpriteStudio6_Root.InformationPlay[countLimitTrack];
					if(null == scriptRoot.TableInformationPlay)
					{
						LogError(messageLogPrefix, "Not Enough Memory (Play-Information)", informationSSAE.FileNameGetFullPath(), informationSSPJ);
						goto AssetCreatePrefab_ErrorEnd;
					}
					for(int i=0; i<countLimitTrack; i++)
					{
						scriptRoot.TableInformationPlay[i].CleanUp();
					}
					countLimitTrack = (countLimitTrack > informationPlayRoot.Length) ? informationPlayRoot.Length : countLimitTrack;
					bool flagClearAnimation;
					for(int i=0; i<countLimitTrack; i++)
					{
						scriptRoot.TableInformationPlay[i].FlagSetInitial = informationPlayRoot[i].FlagSetInitial;
						scriptRoot.TableInformationPlay[i].FlagStopInitial = informationPlayRoot[i].FlagStopInitial;

						flagClearAnimation = false;
						if(false == string.IsNullOrEmpty(nameAnimation[i]))
						{
							indexAnimation = scriptRoot.DataAnimation.IndexGetAnimation(nameAnimation[i]);
							if(0 > indexAnimation)
							{
								flagClearAnimation = true;
							}
							informationPlayRoot[i].NameAnimation = string.Copy(nameAnimation[i]);
						}
						else
						{
							flagClearAnimation = true;
						}
						if(true == flagClearAnimation)
						{
							informationPlayRoot[i].NameAnimation = string.Copy(scriptRoot.DataAnimation.TableAnimation[0].Name);
							informationPlayRoot[i].LabelStart = "";
							informationPlayRoot[i].FrameOffsetStart = 0;
							informationPlayRoot[i].LabelEnd = "";
							informationPlayRoot[i].FrameOffsetEnd = 0;
						}

						scriptRoot.TableInformationPlay[i].NameAnimation = informationPlayRoot[i].NameAnimation;
						scriptRoot.TableInformationPlay[i].FlagPingPong = informationPlayRoot[i].FlagPingPong;
						scriptRoot.TableInformationPlay[i].LabelStart = (false == string.IsNullOrEmpty(informationPlayRoot[i].LabelStart)) ? informationPlayRoot[i].LabelStart : "";
						scriptRoot.TableInformationPlay[i].FrameOffsetStart = informationPlayRoot[i].FrameOffsetStart;
						scriptRoot.TableInformationPlay[i].LabelEnd = (false == string.IsNullOrEmpty(informationPlayRoot[i].LabelEnd)) ? informationPlayRoot[i].LabelEnd : "";
						scriptRoot.TableInformationPlay[i].FrameOffsetEnd = informationPlayRoot[i].FrameOffsetEnd;
						scriptRoot.TableInformationPlay[i].Frame = informationPlayRoot[i].Frame;
						scriptRoot.TableInformationPlay[i].TimesPlay = informationPlayRoot[i].TimesPlay;
						scriptRoot.TableInformationPlay[i].RateTime = informationPlayRoot[i].RateTime;
					}

					gameObjectRoot.SetActive(true);

					/* Fixing Prefab */
					informationSSAE.PrefabAnimationSS6PU.TableData[0] = PrefabUtility.ReplacePrefab(	gameObjectRoot,
																										informationSSAE.PrefabAnimationSS6PU.TableData[0],
																										LibraryEditor_SpriteStudio6.Import.OptionPrefabReplace
																									);
					AssetDatabase.SaveAssets();

					/* Destroy Temporary */
					UnityEngine.Object.DestroyImmediate(gameObjectRoot);
					gameObjectRoot = null;

					/* Create Control-Object */
					if(true == setting.Basic.FlagCreateControlGameObject)
					{
						/* Control-Object Create */
						GameObject gameObjectControl = Library_SpriteStudio6.Utility.Asset.GameObjectCreate(informationSSAE.NameGameObjectAnimationControlSS6PU, false, null);
						if(null == gameObjectControl)
						{
							LogError(messageLogPrefix, "Failure to get Temporary-GameObject (Control)", informationSSAE.FileNameGetFullPath(), informationSSPJ);
							goto AssetCreatePrefab_ErrorEnd;
						}

						/* Attach Script & Link Prefab */
						Script_SpriteStudio6_ControlPrefab scriptControlPrefab = gameObjectControl.AddComponent<Script_SpriteStudio6_ControlPrefab>();
						scriptControlPrefab.PrefabAnimation = informationSSAE.PrefabAnimationSS6PU.TableData[0];

						/* Create Prefab */
						gameObjectControl.SetActive(true);

						UnityEngine.Object prefabControl = PrefabUtility.CreateEmptyPrefab(informationSSAE.PrefabControlAnimationSS6PU.TableName[0]);
						PrefabUtility.ReplacePrefab(	gameObjectControl,
														prefabControl,
														LibraryEditor_SpriteStudio6.Import.OptionPrefabReplace
													);
						AssetDatabase.SaveAssets();

						/* Destroy Temporary */
						UnityEngine.Object.DestroyImmediate(gameObjectControl);
						gameObjectControl = null;
					}

					return(true);

				AssetCreatePrefab_ErrorEnd:;
					if(null != gameObjectRoot)
					{
						UnityEngine.Object.DestroyImmediate(gameObjectRoot);
					}
					return(false);
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

					/* Convert Parts (Pick up underControl Objects) */
					/* MEMO: Since order of SSAE conversion is determined in "informationSSPJ.QueueGetConvertSSAE", */
					/*        prefabs referred to in this animation has already been fixed.                         */
					LibraryEditor_SpriteStudio6.Import.SSAE.Information.Parts informationParts = null;
					for(int i=0; i<countParts; i++)
					{
						int indexUnderControl;
						informationParts = informationSSAE.TableParts[i];
						switch(informationParts.Data.Feature)
						{
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
								break;

							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
								informationParts.Data.PrefabUnderControl = null;
								informationParts.Data.NameAnimationUnderControl = "";
								if(false == string.IsNullOrEmpty(informationParts.NameUnderControl))
								{
									indexUnderControl = informationSSPJ.IndexGetAnimation(informationParts.NameUnderControl);
									if(0 <= indexUnderControl)
									{
										informationParts.Data.PrefabUnderControl = informationSSPJ.TableInformationSSAE[indexUnderControl].PrefabAnimationSS6PU.TableData[0];
										informationParts.Data.NameAnimationUnderControl = informationParts.NameAnimationUnderControl;
									}
								}
								break;

							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
								informationParts.Data.PrefabUnderControl = null;
								informationParts.Data.NameAnimationUnderControl = "";
								if(false == string.IsNullOrEmpty(informationParts.NameUnderControl))
								{
									indexUnderControl = informationSSPJ.IndexGetEffect(informationParts.NameUnderControl);
									if(0 <= indexUnderControl)
									{
										informationParts.Data.PrefabUnderControl = informationSSPJ.TableInformationSSEE[indexUnderControl].PrefabEffectSS6PU.TableData[0];
									}
								}
								break;

							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4:
								break;

							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ARMATURE:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
							case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
								break;

							default:
								break;
						}
					}

					/* Convert Animations */
					LibraryEditor_SpriteStudio6.Import.SSAE.Information.Animation informationAnimation = null;
					LibraryEditor_SpriteStudio6.Import.SSAE.Information.Animation.Parts informationAnimationParts = null;
					Library_SpriteStudio6.Data.Animation dataAnimation = null;
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
							informationParts = informationSSAE.TableParts[j];
							informationAnimationParts = informationAnimation.TableParts[j];
							dataAnimation.TableParts[j].StatusParts = informationAnimationParts.StatusParts;
							dataAnimation.TableParts[j].Format = informationSSAE.FormatSS6PU;

							dataAnimation.TableParts[j].Status = PackAttribute.FactoryStatus(setting.PackAttributeAnimation.Status);
							if(false == dataAnimation.TableParts[j].Status.Function.Pack(	dataAnimation.TableParts[j].Status,
																							Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeStatus,
																							countFrame,
																							informationAnimationParts.StatusParts,
																							informationAnimationParts.TableOrderDraw,
																							informationAnimationParts.TableOrderPreDraw,
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

							dataAnimation.TableParts[j].Position = PackAttribute.FactoryVector3(setting.PackAttributeAnimation.Position);
							if(false == dataAnimation.TableParts[j].Position.Function.Pack(	dataAnimation.TableParts[j].Position,
																							Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePosition,
																							countFrame,
																							informationAnimationParts.StatusParts,
																							informationAnimationParts.TableOrderDraw,
																							informationAnimationParts.TableOrderPreDraw,
																							informationAnimationParts.PositionX,
																							informationAnimationParts.PositionY,
																							informationAnimationParts.PositionZ
																						)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"Position\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}
							dataAnimation.TableParts[j].Rotation = PackAttribute.FactoryVector3(setting.PackAttributeAnimation.Rotation);
							if(false == dataAnimation.TableParts[j].Rotation.Function.Pack(	dataAnimation.TableParts[j].Rotation,
																							Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeRotation,
																							countFrame,
																							informationAnimationParts.StatusParts,
																							informationAnimationParts.TableOrderDraw,
																							informationAnimationParts.TableOrderPreDraw,
																							informationAnimationParts.RotationX,
																							informationAnimationParts.RotationY,
																							informationAnimationParts.RotationZ
																						)
								)
							{
								goto ConvertData_ErrorEnd;
							}
							dataAnimation.TableParts[j].Scaling = PackAttribute.FactoryVector2(setting.PackAttributeAnimation.Scaling);
							if(false == dataAnimation.TableParts[j].Scaling.Function.Pack(	dataAnimation.TableParts[j].Scaling,
																							Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeScaling,
																							countFrame,
																							informationAnimationParts.StatusParts,
																							informationAnimationParts.TableOrderDraw,
																							informationAnimationParts.TableOrderPreDraw,
																							informationAnimationParts.ScalingX,
																							informationAnimationParts.ScalingY
																						)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"Rotation\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}

							dataAnimation.TableParts[j].ScalingLocal = PackAttribute.FactoryVector2(setting.PackAttributeAnimation.ScalingLocal);
							if(false == dataAnimation.TableParts[j].ScalingLocal.Function.Pack(	dataAnimation.TableParts[j].ScalingLocal,
																								Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeScalingLocal,
																								countFrame,
																								informationAnimationParts.StatusParts,
																								informationAnimationParts.TableOrderDraw,
																								informationAnimationParts.TableOrderPreDraw,
																								informationAnimationParts.ScalingXLocal,
																								informationAnimationParts.ScalingYLocal
																							)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"Rotation\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}

							/* MEMO: "RateOpacity" and "RateOpacityLocal" never work in parallel. (always "RateOpacityLocal" takes precedence)                 */
							/*       Also, for "Mask"parts, "RateOpacity" and "RateOpacityLocal" does not work. Instead, "PowerMask" works.                    */
							/*       However, "RateOpacity" is always valid as an inheritance parameter for child-parts.                                       */
							/*       For above reasons, "RateOpacity" is used as a common storage for "RateOpacity", "RateOpacityLocal" and "PowerMask".       */
							/*                                                                                                                                 */
							/*       The point to note is that the value-range of "RateOpacity" is "0.0 to 1.0", and the equivalent "PowerMask" is "255 to 0". */
							/*       This conversion is processed by judging processing-attribute's name in ("StandardUncompress"'s) "Funtion.Pack" function.  */
							switch(informationParts.Data.Feature)
							{
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ROOT:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NULL:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE2:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.NORMAL_TRIANGLE4:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT:
									if(0 >= informationAnimationParts.RateOpacityLocal.CountGetKey())
									{	/* RateOpacity */
										dataAnimation.TableParts[j].RateOpacity = PackAttribute.FactoryFloat(setting.PackAttributeAnimation.RateOpacity);
										if(false == dataAnimation.TableParts[j].RateOpacity.Function.Pack(	dataAnimation.TableParts[j].RateOpacity,
																											Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeRateOpacity,
																											countFrame,
																											informationAnimationParts.StatusParts,
																											informationAnimationParts.TableOrderDraw,
																											informationAnimationParts.TableOrderPreDraw,
																											informationAnimationParts.RateOpacity
																										)
											)
										{
											LogError(messageLogPrefix, "Failure Packing Attribute \"RateOpacity\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
											goto ConvertData_ErrorEnd;
										}
									}
									else
									{	/* RateOpacity-Local */
										dataAnimation.TableParts[j].RateOpacity = PackAttribute.FactoryFloat(setting.PackAttributeAnimation.RateOpacity);
										if(false == dataAnimation.TableParts[j].RateOpacity.Function.Pack(	dataAnimation.TableParts[j].RateOpacity,
																											Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeRateOpacityLocal,
																											countFrame,
																											informationAnimationParts.StatusParts,
																											informationAnimationParts.TableOrderDraw,
																											informationAnimationParts.TableOrderPreDraw,
																											informationAnimationParts.RateOpacityLocal
																										)
											)
										{
											LogError(messageLogPrefix, "Failure Packing Attribute \"RateOpacityLocal\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
											goto ConvertData_ErrorEnd;
										}
									}
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE2:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MASK_TRIANGLE4:
									dataAnimation.TableParts[j].RateOpacity = PackAttribute.FactoryFloat(setting.PackAttributeAnimation.RateOpacity);
									if(false == dataAnimation.TableParts[j].RateOpacity.Function.Pack(	dataAnimation.TableParts[j].RateOpacity,
																										Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePowerMask,
																										countFrame,
																										informationAnimationParts.StatusParts,
																										informationAnimationParts.TableOrderDraw,
																										informationAnimationParts.TableOrderPreDraw,
																										informationAnimationParts.PowerMask
																									)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"PowerMask\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}
									break;

								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.JOINT:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.ARMATURE:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.MOVENODE:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.CONSTRAINT:
								case Library_SpriteStudio6.Data.Parts.Animation.KindFeature.BONEPOINT:
									break;

								default:
									break;
							}

							dataAnimation.TableParts[j].PartsColor = PackAttribute.FactoryPartsColor(setting.PackAttributeAnimation.PartsColor);
							if(false == dataAnimation.TableParts[j].PartsColor.Function.Pack(	dataAnimation.TableParts[j].PartsColor,
																								Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePartsColor,
																								countFrame,
																								informationAnimationParts.StatusParts,
																								informationAnimationParts.TableOrderDraw,
																								informationAnimationParts.TableOrderPreDraw,
																								informationAnimationParts.PartsColor
																							)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"PartsColor\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}

							dataAnimation.TableParts[j].PositionAnchor = PackAttribute.FactoryVector2(setting.PackAttributeAnimation.PositionAnchor);
							if(false == dataAnimation.TableParts[j].PositionAnchor.Function.Pack(	dataAnimation.TableParts[j].PositionAnchor,
																									Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePositionAnchor,
																									countFrame,
																									informationAnimationParts.StatusParts,
																									informationAnimationParts.TableOrderDraw,
																									informationAnimationParts.TableOrderPreDraw,
																									informationAnimationParts.AnchorPositionX,
																									informationAnimationParts.AnchorPositionY
																								)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"PositionAnchor\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}

							dataAnimation.TableParts[j].RadiusCollision = PackAttribute.FactoryFloat(setting.PackAttributeAnimation.RadiusCollision);
							if(false == dataAnimation.TableParts[j].RadiusCollision.Function.Pack(	dataAnimation.TableParts[j].RadiusCollision,
																									Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeRadiusCollision,
																									countFrame,
																									informationAnimationParts.StatusParts,
																									informationAnimationParts.TableOrderDraw,
																									informationAnimationParts.TableOrderPreDraw,
																									informationAnimationParts.RadiusCollision
																								)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"RadiusCollision\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}

							dataAnimation.TableParts[j].UserData = PackAttribute.FactoryUserData(setting.PackAttributeAnimation.UserData);
							if(false == dataAnimation.TableParts[j].UserData.Function.Pack(	dataAnimation.TableParts[j].UserData,
																							Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeUserData,
																							countFrame,
																							informationAnimationParts.StatusParts,
																							informationAnimationParts.TableOrderDraw,
																							informationAnimationParts.TableOrderPreDraw,
																							informationAnimationParts.UserData
																						)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"UserData\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}
							dataAnimation.TableParts[j].Instance = PackAttribute.FactoryInstance(setting.PackAttributeAnimation.Instance);
							if(false == dataAnimation.TableParts[j].Instance.Function.Pack(	dataAnimation.TableParts[j].Instance,
																							Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeInstance,
																							countFrame,
																							informationAnimationParts.StatusParts,
																							informationAnimationParts.TableOrderDraw,
																							informationAnimationParts.TableOrderPreDraw,
																							informationAnimationParts.Instance
																						)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"Instance\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}
							dataAnimation.TableParts[j].Effect = PackAttribute.FactoryEffect(setting.PackAttributeAnimation.Effect);
							if(false == dataAnimation.TableParts[j].Effect.Function.Pack(	dataAnimation.TableParts[j].Effect,
																							Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeEffect,
																							countFrame,
																							informationAnimationParts.StatusParts,
																							informationAnimationParts.TableOrderDraw,
																							informationAnimationParts.TableOrderPreDraw,
																							informationAnimationParts.Effect
																					)
								)
							{
								LogError(messageLogPrefix, "Failure Packing Attribute \"Effect\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
								goto ConvertData_ErrorEnd;
							}

							/* MEMO: Just create, even if do not use.                            */
							/*       (Because pack format at instantiate becomes inappropriate.) */
							dataAnimation.TableParts[j].Plain.Cell = PackAttribute.FactoryCell(setting.PackAttributeAnimation.PlainCell);
							dataAnimation.TableParts[j].Plain.VertexCorrection = PackAttribute.FactoryVertexCorrection(setting.PackAttributeAnimation.PlainVertexCorrection);
							dataAnimation.TableParts[j].Plain.OffsetPivot = PackAttribute.FactoryVector2(setting.PackAttributeAnimation.PlainOffsetPivot);
							dataAnimation.TableParts[j].Plain.PositionTexture = PackAttribute.FactoryVector2(setting.PackAttributeAnimation.PlainPositionTexture);
							dataAnimation.TableParts[j].Plain.ScalingTexture = PackAttribute.FactoryVector2(setting.PackAttributeAnimation.PlainScalingTexture);
							dataAnimation.TableParts[j].Plain.RotationTexture = PackAttribute.FactoryFloat(setting.PackAttributeAnimation.PlainRotationTexture);

							dataAnimation.TableParts[j].Fix.IndexCellMap = PackAttribute.FactoryInt(setting.PackAttributeAnimation.FixIndexCellMap);
							dataAnimation.TableParts[j].Fix.Coordinate = PackAttribute.FactoryCoordinateFix(setting.PackAttributeAnimation.FixCoordinate);
							dataAnimation.TableParts[j].Fix.UV0 = PackAttribute.FactoryUVFix(setting.PackAttributeAnimation.FixUV0);
							dataAnimation.TableParts[j].Fix.SizeCollision = PackAttribute.FactoryVector2(setting.PackAttributeAnimation.FixSizeCollision);
							dataAnimation.TableParts[j].Fix.PivotCollision = PackAttribute.FactoryVector2(setting.PackAttributeAnimation.FixPivotCollision);

							switch(informationSSAE.FormatSS6PU)
							{
								case Library_SpriteStudio6.Data.Animation.Parts.KindFormat.PLAIN:
									dataAnimation.TableParts[j].Plain.SizeForce = PackAttribute.FactoryVector2(setting.PackAttributeAnimation.PlainSizeForce);
									if(false == dataAnimation.TableParts[j].Plain.SizeForce.Function.Pack(	dataAnimation.TableParts[j].Plain.SizeForce,
																											Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainSizeForce,
																											countFrame,
																											informationAnimationParts.StatusParts,
																											informationAnimationParts.TableOrderDraw,
																											informationAnimationParts.TableOrderPreDraw,
																											informationAnimationParts.SizeForceX,
																											informationAnimationParts.SizeForceY
																										)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"SizeForce\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									if(false ==  dataAnimation.TableParts[j].Plain.Cell.Function.Pack(	dataAnimation.TableParts[j].Plain.Cell,
																										Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainCell,
																										countFrame,
																										informationAnimationParts.StatusParts,
																										informationAnimationParts.TableOrderDraw,
																										informationAnimationParts.TableOrderPreDraw,
																										informationAnimationParts.Cell
																									)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.Cell\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									if(false == dataAnimation.TableParts[j].Plain.VertexCorrection.Function.Pack(	dataAnimation.TableParts[j].Plain.VertexCorrection,
																													Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainVertexCorrection,
																													countFrame,
																													informationAnimationParts.StatusParts,
																													informationAnimationParts.TableOrderDraw,
																													informationAnimationParts.TableOrderPreDraw,
																													informationAnimationParts.VertexCorrection
																											)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.VertexCorrection\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									if(false == dataAnimation.TableParts[j].Plain.OffsetPivot.Function.Pack(	dataAnimation.TableParts[j].Plain.OffsetPivot,
																												Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainOffsetPivot,
																												countFrame,
																												informationAnimationParts.StatusParts,
																												informationAnimationParts.TableOrderDraw,
																												informationAnimationParts.TableOrderPreDraw,
																												informationAnimationParts.PivotOffsetX,
																												informationAnimationParts.PivotOffsetY
																										)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.OffsetPivot\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									if(false == dataAnimation.TableParts[j].Plain.PositionTexture.Function.Pack(	dataAnimation.TableParts[j].Plain.PositionTexture,
																													Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainPositionTexture,
																													countFrame,
																													informationAnimationParts.StatusParts,
																													informationAnimationParts.TableOrderDraw,
																													informationAnimationParts.TableOrderPreDraw,
																													informationAnimationParts.TexturePositionX,
																													informationAnimationParts.TexturePositionY
																											)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.PositionTexture\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									if(false == dataAnimation.TableParts[j].Plain.ScalingTexture.Function.Pack(	dataAnimation.TableParts[j].Plain.ScalingTexture,
																												Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainScalingTexture,
																												countFrame,
																												informationAnimationParts.StatusParts,
																												informationAnimationParts.TableOrderDraw,
																												informationAnimationParts.TableOrderPreDraw,
																												informationAnimationParts.TextureScalingX,
																												informationAnimationParts.TextureScalingY
																											)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.ScalingTexture\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									if(false == dataAnimation.TableParts[j].Plain.RotationTexture.Function.Pack(	dataAnimation.TableParts[j].Plain.RotationTexture,
																													Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributePlainRotationTexture,
																													countFrame,
																													informationAnimationParts.StatusParts,
																													informationAnimationParts.TableOrderDraw,
																													informationAnimationParts.TableOrderPreDraw,
																													informationAnimationParts.TextureRotation
																											)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Plain.RotationTexture\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}
									break;

								case Library_SpriteStudio6.Data.Animation.Parts.KindFormat.FIX:
									if(false == dataAnimation.TableParts[j].Fix.IndexCellMap.Function.Pack(	dataAnimation.TableParts[j].Fix.IndexCellMap,
																											Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixIndexCellMap,
																											countFrame,
																											informationAnimationParts.StatusParts,
																											informationAnimationParts.TableOrderDraw,
																											informationAnimationParts.TableOrderPreDraw,
																											informationAnimationParts.FixIndexCellMap
																										)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Fix.Coordinate\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									if(false == dataAnimation.TableParts[j].Fix.Coordinate.Function.Pack(	dataAnimation.TableParts[j].Fix.Coordinate,
																											Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixCoordinate,
																											countFrame,
																											informationAnimationParts.StatusParts,
																											informationAnimationParts.TableOrderDraw,
																											informationAnimationParts.TableOrderPreDraw,
																											informationAnimationParts.FixCoordinate
																									)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Fix.Coordinate\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									if(false == dataAnimation.TableParts[j].Fix.UV0.Function.Pack(	dataAnimation.TableParts[j].Fix.UV0,
																									Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixUV0,
																									countFrame,
																									informationAnimationParts.StatusParts,
																									informationAnimationParts.TableOrderDraw,
																									informationAnimationParts.TableOrderPreDraw,
																									informationAnimationParts.FixUV
																								)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Fix.UV0\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									if(false == dataAnimation.TableParts[j].Fix.SizeCollision.Function.Pack(	dataAnimation.TableParts[j].Fix.SizeCollision,
																												Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixSizeCollision,
																												countFrame,
																												informationAnimationParts.StatusParts,
																												informationAnimationParts.TableOrderDraw,
																												informationAnimationParts.TableOrderPreDraw,
																												informationAnimationParts.FixSizeCollisionX,
																												informationAnimationParts.FixSizeCollisionY
																										)
										)
									{
										LogError(messageLogPrefix, "Failure Packing Attribute \"Fix.SizeCollision\" Animation-Name[" + informationAnimation.Data.Name + "]", informationSSAE.FileNameGetFullPath(), informationSSPJ);
										goto ConvertData_ErrorEnd;
									}

									if(false == dataAnimation.TableParts[j].Fix.PivotCollision.Function.Pack(	dataAnimation.TableParts[j].Fix.PivotCollision,
																												Library_SpriteStudio6.Data.Animation.Attribute.Importer.NameAttributeFixPivotCollision,
																												countFrame,
																												informationAnimationParts.StatusParts,
																												informationAnimationParts.TableOrderDraw,
																												informationAnimationParts.TableOrderPreDraw,
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
				#endregion Functions

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				public static partial class PackAttribute
				{
					/* ----------------------------------------------- Functions */
					#region Functions
					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt FactoryInt(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInt();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionInt(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat FactoryFloat(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerFloat();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionFloat(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 FactoryVector2(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2 container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector2();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector2(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 FactoryVector3(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3 container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVector3();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVector3(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus FactoryStatus(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerStatus();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionStatus(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell FactoryCell(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCell();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionCell(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor FactoryPartsColor(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerPartsColor();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionPartsColor(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection FactoryVertexCorrection(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerVertexCorrection();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionVertexCorrection(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData FactoryUserData(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUserData();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionUserData(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance FactoryInstance(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerInstance();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionInstance(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect FactoryEffect(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerEffect();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionEffect(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix FactoryCoordinateFix(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerCoordinateFix();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionCoordinateFix(container);
						return(container);
					}

					public static Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix FactoryUVFix(Library_SpriteStudio6.Data.Animation.PackAttribute.KindTypePack pack)
					{
						Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix container = new Library_SpriteStudio6.Data.Animation.PackAttribute.ContainerUVFix();
						container.TypePack = pack;
						Library_SpriteStudio6.Data.Animation.PackAttribute.BootUpFunctionUVFix(container);
						return(container);
					}
					#endregion Functions
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
