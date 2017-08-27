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
		public static partial class SSEE
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static Information Parse(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
												string nameFile,
												LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ
											)
			{
				const string messageLogPrefix = "SSEE-Parse";
				Information informationSSEE = null;

				/* ".ssee" Load */
				if(false == System.IO.File.Exists(nameFile))
				{
					LogError(messageLogPrefix, "File Not Found", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				System.Xml.XmlDocument xmlSSEE = new System.Xml.XmlDocument();
				xmlSSEE.Load(nameFile);

				/* Check Version */
				System.Xml.XmlNode nodeRoot = xmlSSEE.FirstChild;
				nodeRoot = nodeRoot.NextSibling;
				KindVersion version = (KindVersion)(LibraryEditor_SpriteStudio6.Utility.XML.VersionGet(nodeRoot, "SpriteStudioEffect", (int)KindVersion.ERROR, true));
				switch(version)
				{
					case KindVersion.ERROR:
						LogError(messageLogPrefix, "Version Invalid", nameFile, informationSSPJ);
						goto Parse_ErrorEnd;

					case KindVersion.CODE_010000:
					case KindVersion.CODE_010001:
						LogError(messageLogPrefix, "Version Disused", nameFile, informationSSPJ);
						goto Parse_ErrorEnd;

					case KindVersion.CODE_010002:
						LogError(messageLogPrefix, "Version Unsupported (SpriteStudio5.5/5.6)", nameFile, informationSSPJ);
						goto Parse_ErrorEnd;

					case KindVersion.CODE_010100:
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
				informationSSEE = new Information();
				if(null == informationSSEE)
				{
					LogError(messageLogPrefix, "Not Enough Memory", nameFile, informationSSPJ);
					goto Parse_ErrorEnd;
				}
				informationSSEE.CleanUp();
				informationSSEE.Version = version;

				/* Get Base-Directories */
				LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out informationSSEE.NameDirectory, out informationSSEE.NameFileBody, out informationSSEE.NameFileExtension, nameFile);
				informationSSEE.NameDirectory += "/";

				/* Decode Tags */
				System.Xml.NameTable nodeNameSpace = new System.Xml.NameTable();
				System.Xml.XmlNamespaceManager managerNameSpace = new System.Xml.XmlNamespaceManager(nodeNameSpace);
				System.Xml.XmlNodeList nodeList = null;

				/* Get Base-Data */
				string valueText = "";
				switch(informationSSEE.Version)
				{
					case KindVersion.CODE_010002:
						break;

					case KindVersion.CODE_010100:
					{	/* SS5.7 */
						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "effectData/lockRandSeed", managerNameSpace);
						if(false == string.IsNullOrEmpty(valueText))
						{
							informationSSEE.Seed = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
						}

						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "effectData/isLockRandSeed", managerNameSpace);
						if(false == string.IsNullOrEmpty(valueText))
						{
							informationSSEE.FlagLockSeed = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetBool(valueText);
						}

						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "effectData/fps", managerNameSpace);
						if(false == string.IsNullOrEmpty(valueText))
						{
							informationSSEE.FramePerSecond = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
						}

						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "effectData/renderVersion", managerNameSpace);
						if(false == string.IsNullOrEmpty(valueText))
						{
							informationSSEE.VersionRenderer = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
						}

						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "effectData/layoutScaleX", managerNameSpace);
						if(false == string.IsNullOrEmpty(valueText))
						{
							informationSSEE.ScaleLayout.x = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText)) / 100.0f;
						}

						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeRoot, "effectData/layoutScaleY", managerNameSpace);
						if(false == string.IsNullOrEmpty(valueText))
						{
							informationSSEE.ScaleLayout.y = (float)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText)) / 100.0f;
						}

						/* Get Parts-Data */
						nodeList = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "effectData/nodeList/node", managerNameSpace);
						if(null == nodeList)
						{
							LogError(messageLogPrefix, "PartList-Node Not Found", nameFile, informationSSPJ);
							goto Parse_ErrorEnd;
						}
						informationSSEE.TableParts = new Information.Parts[nodeList.Count];
						if(null == informationSSEE.TableParts)
						{
							LogError(messageLogPrefix, "Not Enough Memory (Parts-Data WorkArea)", nameFile, informationSSPJ);
							goto Parse_ErrorEnd;
						}
						foreach(System.Xml.XmlNode nodeParts in nodeList)
						{
							/* Get Part-ID */
							int indexParts = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "arrayIndex", managerNameSpace));

							/* Get Part-Data */
							informationSSEE.TableParts[indexParts] = ParseParts(	ref setting,
																					informationSSPJ,
																					nodeParts,
																					managerNameSpace,
																					informationSSEE,
																					indexParts,
																					nameFile
																				);
							if(null == informationSSEE.TableParts[indexParts])
							{
								goto Parse_ErrorEnd;
							}
						}
					}
					break;
				}

				return(informationSSEE);

			Parse_ErrorEnd:;
				return(null);
			}

			private static Information.Parts ParseParts(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
															LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
															System.Xml.XmlNode nodeParts,
															System.Xml.XmlNamespaceManager managerNameSpace,
															Information informationSSEE,
															int indexParts,
															string nameFileSSEE
														)
			{
				const string messageLogPrefix = "SSEE-Parse (Parts)";

				/* Create Information */
				Information.Parts informationParts = new Information.Parts();
				if(null == informationParts)
				{
					LogError(messageLogPrefix, "Not Enough Memory (Parts WorkArea)", nameFileSSEE, informationSSPJ);
					goto ParseParts_ErrorEnd;
				}
				informationParts.CleanUp();

				/* Get Base-Datas */
				string valueText = "";

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "name", managerNameSpace);
				informationParts.Data.Name = string.Copy(valueText);

				informationParts.Data.ID = indexParts;

				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "parentIndex", managerNameSpace);
				informationParts.Data.IDParent = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
				if(0 <= informationParts.Data.IDParent)
				{
					Information.Parts informationPartsParent = informationSSEE.TableParts[informationParts.Data.IDParent];
					informationPartsParent.ListIndexPartsChild.Add(informationParts.Data.ID);
				}

				Library_SpriteStudio6.KindOperationBlendEffect operationBlendTarget = Library_SpriteStudio6.KindOperationBlendEffect.NON;
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "behavior/BlendType", managerNameSpace);
				if(false == string.IsNullOrEmpty(valueText))
				{
					switch(valueText)
					{
						case "Mix":
							operationBlendTarget = Library_SpriteStudio6.KindOperationBlendEffect.MIX;
							break;

						case "Add":
							operationBlendTarget = Library_SpriteStudio6.KindOperationBlendEffect.ADD;
							break;

						default:
							LogWarning(messageLogPrefix, "Unknown Alpha-Blend Kind \"" + valueText + "\" Parts[" + indexParts.ToString() + "]", nameFileSSEE, informationSSPJ);
							goto case "Mix";
					}
				}

				string nameCellMap = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "behavior/CellMapName", managerNameSpace);
				string nameCell = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "behavior/CellName", managerNameSpace);

				/* Get Parts-Kind */
				valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParts, "type", managerNameSpace);
				switch(valueText)
				{
					case "Root":
						if(true == string.IsNullOrEmpty(informationParts.Data.Name))
						{	/* Default-Name */
							informationParts.Data.Name = "Root";
						}
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Effect.KindFeature.ROOT;
						informationParts.DataEmitter = null;
						break;

					case "Emmiter":	/* "Emitter" */
						if(true == string.IsNullOrEmpty(informationParts.Data.Name))
						{	/* Default-Name */
							informationParts.Data.Name = "Emitter";
						}
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Effect.KindFeature.EMITTER;
						informationParts.DataEmitter = ParsePartsEmitter(	ref setting,
																			informationSSPJ,
																			nodeParts,
																			managerNameSpace,
																			informationSSEE,
																			informationParts,
																			operationBlendTarget,
																			nameCellMap,
																			nameCell,
																			indexParts,
																			nameFileSSEE
																		);
						if(null == informationParts.DataEmitter)
						{
							goto ParseParts_ErrorEnd;
						}
						break;

					case "Particle":
						if(true == string.IsNullOrEmpty(informationParts.Data.Name))
						{	/* Default-Name */
							informationParts.Data.Name = "Particle";
						}
						informationParts.Data.Feature = Library_SpriteStudio6.Data.Parts.Effect.KindFeature.PARTICLE;
						informationParts.DataEmitter = null;
						break;

					default:
						/* MEMO: Error */
						if(true == string.IsNullOrEmpty(informationParts.Data.Name))
						{	/* Default-Name */
							informationParts.Data.Name = "Error";
						}
						informationParts.Data.Feature = (Library_SpriteStudio6.Data.Parts.Effect.KindFeature)(-1);
						informationParts.DataEmitter = null;
						break;
				}

				return(informationParts);

			ParseParts_ErrorEnd:;
				return(null);
			}

			private static Information.Parts.Emitter ParsePartsEmitter(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
																		LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																		System.Xml.XmlNode nodeParts,
																		System.Xml.XmlNamespaceManager managerNameSpace,
																		Information informationSSEE,
																		Information.Parts informationParts,
																		Library_SpriteStudio6.KindOperationBlendEffect operationBlendTarget,
																		string nameCellMap,
																		string nameCell,
																		int indexParts,
																		string nameFileSSEE
																	)
			{
				const string messageLogPrefix = "SSEE-Parse (Emitter)";

				/* Create Information */
				Information.Parts.Emitter informationEmitter = new Information.Parts.Emitter();
				if(null == informationEmitter)
				{
					LogError(messageLogPrefix, "Not Enough Memory (Parts WorkArea)", nameFileSSEE, informationSSPJ);
					goto ParsePartsEmitter_ErrorEnd;
				}
				informationEmitter.CleanUp();

				/* Get Base Datas */
				informationEmitter.Data.FlagData = Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.CLEAR;
				informationEmitter.Data.OperationBlendTarget =  operationBlendTarget;
				informationEmitter.NameCellMap = nameCellMap;
				informationEmitter.NameCell = nameCell;

				string valueText = "";
				valueText = nameCellMap;
				if(false == string.IsNullOrEmpty(valueText))
				{
					valueText = informationSSPJ.PathGetAbsolute(valueText, LibraryEditor_SpriteStudio6.Import.KindFile.SSCE);
					informationEmitter.Data.IndexCellMap = informationSSPJ.IndexGetFileName(informationSSPJ.TableNameSSCE, valueText);
				}
				informationEmitter.Data.IndexCell = -1;

				System.Xml.XmlNode nodeEmitterAttributes = LibraryEditor_SpriteStudio6.Utility.XML.NodeGet(nodeParts, "behavior/list", managerNameSpace);
				System.Xml.XmlNodeList listNodeAttribute = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeEmitterAttributes, "value", managerNameSpace);
				foreach(System.Xml.XmlNode nodeAttribute in listNodeAttribute)
				{
					/* Get Attribute-Classification */
					valueText = nodeAttribute.Attributes["name"].Value;
					switch(valueText)
					{
						case "Basic":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.BASIC;

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "priority", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.PriorityParticle = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText);
								}

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "maximumParticle", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.CountParticleMax = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
								}

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "attimeCreate", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.CountParticleEmit = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
								}

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "interval", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.Interval = (int)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText));
								}

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "lifetime", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.DurationEmitter = (int)(LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText));
								}

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.Speed.Main, ref informationEmitter.Data.Speed.Sub, informationSSPJ, nodeAttribute, "speed", managerNameSpace, indexParts, nameFileSSEE);

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.DurationParticle.Main, ref informationEmitter.Data.DurationParticle.Sub, informationSSPJ, nodeAttribute, "lifespan", managerNameSpace, indexParts, nameFileSSEE);

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "angle", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.Angle.Main = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText);
								}

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "angleVariance", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.Angle.Sub = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText);
								}
							}
							break;

						case "OverWriteSeed":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.SEEDRANDOM;

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "Seed", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.SeedRandom = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
								}
							}
							break;

						case "Delay":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.DELAY;

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "DelayTime", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.Delay = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetInt(valueText);
								}
							}
							break;

						case "Gravity":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.GRAVITY_DIRECTION;

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "Gravity", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									string[] valueTextSplit = valueText.Split(' ');
									if(2 == valueTextSplit.Length)
									{
										informationEmitter.Data.GravityDirectional.x = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueTextSplit[0]);
										informationEmitter.Data.GravityDirectional.y = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueTextSplit[1]);
									}
									else
									{
										LogWarning(messageLogPrefix, "Broken Emitter-Attribute \"Gravity/Gravity\" Parts[" + indexParts.ToString() + "]", nameFileSSEE, informationSSPJ);
									}
								}
							}
							break;

						case "init_position":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.POSITION;

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.Position.Main.x, ref informationEmitter.Data.Position.Sub.x, informationSSPJ, nodeAttribute, "OffsetX", managerNameSpace, indexParts, nameFileSSEE);
								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.Position.Main.y, ref informationEmitter.Data.Position.Sub.y, informationSSPJ, nodeAttribute, "OffsetY", managerNameSpace, indexParts, nameFileSSEE);
							}
							break;

						case "trans_speed":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.SPEED_FLUCTUATION;

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.SpeedFluctuation.Main, ref informationEmitter.Data.SpeedFluctuation.Sub, informationSSPJ, nodeAttribute, "Speed", managerNameSpace, indexParts, nameFileSSEE);
							}
							break;

						case "init_rotation":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.ROTATION;

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.Rotation.Main, ref informationEmitter.Data.Rotation.Sub, informationSSPJ, nodeAttribute, "Rotation", managerNameSpace, indexParts, nameFileSSEE);

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.RotationFluctuation.Main, ref informationEmitter.Data.RotationFluctuation.Sub, informationSSPJ, nodeAttribute, "RotationAdd", managerNameSpace, indexParts, nameFileSSEE);
						}
						break;

						case "trans_rotation":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.ROTATION_FLUCTUATION;

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "RotationFactor", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.RotationFluctuationRate = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText);
								}

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "EndLifeTimePer", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									/* MEMO: Percent -> Rate */
									informationEmitter.Data.RotationFluctuationRateTime = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText) * 0.01f;
								}
							}
							break;

						case "add_tangentiala":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.TANGENTIALACCELATION;

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.RateTangentialAcceleration.Main, ref informationEmitter.Data.RateTangentialAcceleration.Sub, informationSSPJ, nodeAttribute, "Acceleration", managerNameSpace, indexParts, nameFileSSEE);
							}
							break;

						case "add_pointgravity":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.GRAVITY_POINT;

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "Position", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									string[] valueTextSplit = valueText.Split(' ');
									if(2 == valueTextSplit.Length)
									{
										informationEmitter.Data.GravityPointPosition.x = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueTextSplit[0]);
										informationEmitter.Data.GravityPointPosition.y = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueTextSplit[1]);
									}
									else
									{
										LogWarning(messageLogPrefix, "Broken Emitter-Attribute \"add_pointgravity/Position\" Parts[" + indexParts.ToString() + "]", nameFileSSEE, informationSSPJ);
									}
								}

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "Power", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.GravityPointPower = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText);
								}
							}
							break;

						case "init_vertexcolor":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.COLORVERTEX;

								ParsePartsEmitterRangeGetColor(ref informationEmitter.Data.ColorVertex.Main, ref informationEmitter.Data.ColorVertex.Sub, informationSSPJ, nodeAttribute, "Color", managerNameSpace, indexParts, nameFileSSEE);
							}
							break;

						case "trans_vertexcolor":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.COLORVERTEX_FLUCTUATION;

								ParsePartsEmitterRangeGetColor(ref informationEmitter.Data.ColorVertexFluctuation.Main, ref informationEmitter.Data.ColorVertexFluctuation.Sub, informationSSPJ, nodeAttribute, "Color", managerNameSpace, indexParts, nameFileSSEE);
							}
							break;

						case "init_size":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.SCALE_START;

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.ScaleStart.Main.x, ref informationEmitter.Data.ScaleStart.Sub.x, informationSSPJ, nodeAttribute, "SizeX", managerNameSpace, indexParts, nameFileSSEE);

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.ScaleStart.Main.y, ref informationEmitter.Data.ScaleStart.Sub.y, informationSSPJ, nodeAttribute, "SizeY", managerNameSpace, indexParts, nameFileSSEE);

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.ScaleRateStart.Main, ref informationEmitter.Data.ScaleRateStart.Sub, informationSSPJ, nodeAttribute, "ScaleFactor", managerNameSpace, indexParts, nameFileSSEE);
							}
							break;

						case "trans_size":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.SCALE_END;

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.ScaleEnd.Main.x, ref informationEmitter.Data.ScaleEnd.Sub.x, informationSSPJ, nodeAttribute, "SizeX", managerNameSpace, indexParts, nameFileSSEE);

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.ScaleEnd.Main.y, ref informationEmitter.Data.ScaleEnd.Sub.y, informationSSPJ, nodeAttribute, "SizeY", managerNameSpace, indexParts, nameFileSSEE);

								ParsePartsEmitterRangeGetFloat(ref informationEmitter.Data.ScaleRateEnd.Main, ref informationEmitter.Data.ScaleRateEnd.Sub, informationSSPJ, nodeAttribute, "ScaleFactor", managerNameSpace, indexParts, nameFileSSEE);
							}
							break;

						case "trans_colorfade":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.FADEALPHA;

								Library_SpriteStudio6.Data.Effect.Emitter.RangeFloat rangeTemp = new Library_SpriteStudio6.Data.Effect.Emitter.RangeFloat();
								ParsePartsEmitterRangeGetFloat(ref rangeTemp.Main, ref rangeTemp.Sub, informationSSPJ, nodeAttribute, "disprange", managerNameSpace, indexParts, nameFileSSEE);
								informationEmitter.Data.AlphaFadeStart = rangeTemp.Main * 0.01f;
								informationEmitter.Data.AlphaFadeEnd = rangeTemp.Sub * 0.01f;
							}
							break;

						case "TurnToDirection":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.TURNDIRECTION;

								valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeAttribute, "Rotation", managerNameSpace);
								if(false == string.IsNullOrEmpty(valueText))
								{
									informationEmitter.Data.TurnDirectionFluctuation = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText);
								}
							}
							break;

						case "InfiniteEmit":
							{
								informationEmitter.Data.FlagData |= Library_SpriteStudio6.Data.Effect.Emitter.FlagBit.EMIT_INFINITE;
							}
							break;

						default:
							LogWarning(messageLogPrefix, "Unknown Attribute \"" + valueText + "\" Parts[" + indexParts.ToString() + "]", nameFileSSEE, informationSSPJ);
							break;
					}
				}

				return(informationEmitter);

			ParsePartsEmitter_ErrorEnd:;
				return(null);
			}
			private static bool ParsePartsEmitterRangeGetFloat(	ref float outputMain,
																ref float outputSub,
																LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																System.Xml.XmlNode nodeAttribute,
																string name,
																System.Xml.XmlNamespaceManager managerNameSpace,
																int indexParts,
																string nameFileSSEE
															)
			{
				const string messageLogPrefix = "SSEE-Parse (Emitter)";

				bool flagValid = true;
				string valueText = "";
				System.Xml.XmlNode nodeNow = LibraryEditor_SpriteStudio6.Utility.XML.NodeGet(nodeAttribute, name, managerNameSpace);
				if(null != nodeNow)
				{
					valueText = nodeNow.Attributes["value"].Value;
					if(false == string.IsNullOrEmpty(valueText))
					{
						outputMain = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText);
					}
					else
					{
						flagValid = false;
					}
	
					valueText = nodeNow.Attributes["subvalue"].Value;
					if(false == string.IsNullOrEmpty(valueText))
					{
						outputSub = LibraryEditor_SpriteStudio6.Utility.Text.ValueGetFloat(valueText);
					}
					else
					{
						flagValid = false;
					}
	
					if(outputMain > outputSub)
					{
						float floatTemp = outputSub;
						outputSub = outputMain;
						outputMain = floatTemp;
					}
					outputSub -= outputMain;
				}
				if(false == flagValid)
				{
					LogWarning(messageLogPrefix, "Broken Emitter-Attribute's Parameter \"" + name + "\" Parts[" + indexParts.ToString() + "]", nameFileSSEE, informationSSPJ);
				}
				return(flagValid);
			}
			private static bool ParsePartsEmitterRangeGetColor(	ref Color outputMain,
																ref Color outputSub,
																LibraryEditor_SpriteStudio6.Import.SSPJ.Information informationSSPJ,
																System.Xml.XmlNode nodeAttribute,
																string name,
																System.Xml.XmlNamespaceManager managerNameSpace,
																int indexParts,
																string nameFileSSEE
															)
			{
				const string messageLogPrefix = "SSEE-Parse (Emitter)";

				bool flagValid = true;
				string valueText = "";
				System.Xml.XmlNode nodeNow = LibraryEditor_SpriteStudio6.Utility.XML.NodeGet(nodeAttribute, name, managerNameSpace);
				if(null != nodeNow)
				{
					uint colorTemp;
					valueText = nodeNow.Attributes["value"].Value;
					if(false == string.IsNullOrEmpty(valueText))
					{
						colorTemp = LibraryEditor_SpriteStudio6.Utility.Text.HexToUInt(valueText);
						outputMain.a = ((float)((colorTemp >> 24) & 0xff)) / 255.0f;
						outputMain.r = ((float)((colorTemp >> 16) & 0xff)) / 255.0f;
						outputMain.g = ((float)((colorTemp >> 8) & 0xff)) / 255.0f;
						outputMain.b = ((float)(colorTemp & 0xff)) / 255.0f;
					}
					else
					{
						flagValid = false;
					}
	
					valueText = nodeNow.Attributes["subvalue"].Value;
					if(false == string.IsNullOrEmpty(valueText))
					{
						colorTemp = LibraryEditor_SpriteStudio6.Utility.Text.HexToUInt(valueText);
						outputSub.a = ((float)((colorTemp >> 24) & 0xff)) / 255.0f;
						outputSub.r = ((float)((colorTemp >> 16) & 0xff)) / 255.0f;
						outputSub.g = ((float)((colorTemp >> 8) & 0xff)) / 255.0f;
						outputSub.b = ((float)(colorTemp & 0xff)) / 255.0f;
					}
					else
					{
						flagValid = false;
					}
	
					float floatTemp;
					if(outputMain.a > outputSub.a)
					{
						floatTemp = outputSub.a;
						outputSub.a = outputMain.a;
						outputMain.a = floatTemp;
					}
					if(outputMain.r > outputSub.r)
					{
						floatTemp = outputSub.r;
						outputSub.r = outputMain.r;
						outputMain.r = floatTemp;
					}
					if(outputMain.g > outputSub.g)
					{
						floatTemp = outputSub.g;
						outputSub.g = outputMain.g;
						outputMain.g = floatTemp;
					}
					if(outputMain.b > outputSub.b)
					{
						floatTemp = outputSub.b;
						outputSub.b = outputMain.b;
						outputMain.b = floatTemp;
					}
	
					outputSub.a -= outputMain.a;
					outputSub.r -= outputMain.r;
					outputSub.g -= outputMain.g;
					outputSub.b -= outputMain.b;
				}
				if(false == flagValid)
				{
					LogWarning(messageLogPrefix, "Broken Emitter-Attribute's Parameter \"" + name + "\" Parts[" + indexParts.ToString() + "]", nameFileSSEE, informationSSPJ);
				}
				return(flagValid);
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
				CODE_010000 = 0x00010000,	/* (Disuse) */
				CODE_010001 = 0x00010001,	/* (Disuse) */
				CODE_010002 = 0x00010002,	/* after ver.5.5 (Unsupported) */
				CODE_010100 = 0x00010100,	/* after ver.5.7 */

				TARGET_EARLIEST = CODE_010100,
				TARGET_LATEST = CODE_010100
			};

			private const string ExtentionFile = ".ssee";
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public class Information
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public LibraryEditor_SpriteStudio6.Import.SSEE.KindVersion Version;

				public string NameDirectory;
				public string NameFileBody;
				public string NameFileExtension;

				public int VersionRenderer;
				public Vector2 ScaleLayout;
				public int Seed;
				public bool FlagLockSeed;
				public int FramePerSecond;

				public Parts[] TableParts;
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Version = LibraryEditor_SpriteStudio6.Import.SSEE.KindVersion.ERROR;

					NameDirectory = "";
					NameFileBody = "";
					NameFileExtension = "";

					VersionRenderer = 0;
					ScaleLayout = Vector2.one;
					Seed = 0;
					FlagLockSeed = false;;
					FramePerSecond = 60;

					TableParts = null;
				}

				public string FileNameGetFullPath()
				{
					return(NameDirectory + NameFileBody + NameFileExtension);
				}
				#endregion Functions

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interface
				public class Parts
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public Library_SpriteStudio6.Data.Parts.Effect Data;

					public List<int> ListIndexPartsChild;
					public Emitter DataEmitter;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public void CleanUp()
					{
						Data.CleanUp();

						ListIndexPartsChild = new List<int>();
						ListIndexPartsChild.Clear();

						DataEmitter = new Emitter();
						DataEmitter.CleanUp();
					}
					#endregion Functions

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfac
					public class Emitter
					{
						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						public Library_SpriteStudio6.Data.Effect.Emitter Data;

						public string NameCellMap;
						public string NameCell;
						#endregion Variables & Properties

						/* ----------------------------------------------- Functions */
						#region Functions
						public void CleanUp()
						{
							Data.CleanUp();

							NameCellMap = "";
							NameCell = "";
						}
						#endregion Functions
					}
					#endregion Classes, Structs & Interface
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
