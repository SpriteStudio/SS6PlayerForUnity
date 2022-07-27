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
		public static partial class SignalSettings
		{
			/* ----------------------------------------------- Functions */
			#region Functions
			public static Information Parse(	ref LibraryEditor_SpriteStudio6.Import.Setting setting,
												string nameFile
											)
			{
				const string messageLogPrefix = "Parse SignaleSetting";
				Information informationSignalSetting = null;

				/* ".ssce" Load */
				if(false == System.IO.File.Exists(nameFile))
				{
					LogError(messageLogPrefix, "File Not Found", nameFile);
					goto Parse_ErrorEnd;
				}
				System.Xml.XmlDocument xmlSianalSetting = new System.Xml.XmlDocument();
				xmlSianalSetting.Load(nameFile);

				/* Check Version */
				System.Xml.XmlNode nodeRoot = xmlSianalSetting.FirstChild;
				nodeRoot = nodeRoot.NextSibling;
				KindVersion version = (KindVersion)(LibraryEditor_SpriteStudio6.Utility.XML.VersionGet(nodeRoot, "SpriteStudioSignal", (int)KindVersion.ERROR, true));
				switch(version)
				{
					case KindVersion.ERROR:
						LogError(messageLogPrefix, "Version Invalid", nameFile);
						goto Parse_ErrorEnd;

					case KindVersion.CODE_000100:
					case KindVersion.CODE_010000:
						break;

					default:
						if(KindVersion.TARGET_EARLIEST > version)
						{
							version = KindVersion.TARGET_EARLIEST;
							if(true == setting.CheckVersion.FlagInvalidSSCE)
							{
								LogWarning(messageLogPrefix, "Version Too Early", nameFile);
							}
						}
						else
						{
							version = KindVersion.TARGET_LATEST;
							if(true == setting.CheckVersion.FlagInvalidSSCE)
							{
								LogWarning(messageLogPrefix, "Version Unknown", nameFile);
							}
						}
						break;
				}

				/* Create Information */
				informationSignalSetting = new Information();
				if(null == informationSignalSetting)
				{
					LogError(messageLogPrefix, "Not Enough Memory", nameFile);
					goto Parse_ErrorEnd;
				}
				informationSignalSetting.BootUp();
				informationSignalSetting.Version = version;

				/* Get Base-Directories */
				LibraryEditor_SpriteStudio6.Utility.File.PathSplit(out informationSignalSetting.NameDirectory, out informationSignalSetting.NameFileBody, out informationSignalSetting.NameFileExtension, nameFile);

				/* Decode Tags */
				System.Xml.NameTable nodeNameSpace = new System.Xml.NameTable();
				System.Xml.XmlNamespaceManager managerNameSpace = new System.Xml.XmlNamespaceManager(nodeNameSpace);

				string valueText = "";

				/* Get Command Setting */
				System.Xml.XmlNodeList listNode = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "commands/value", managerNameSpace);
				if(null == listNode)
				{
					LogError(messageLogPrefix, "\"command/value\"-Node Not Found", nameFile);
					goto Parse_ErrorEnd;
				}
				Information.Command command = null;
				foreach(System.Xml.XmlNode nodeCommand in listNode)
				{
					command = new Information.Command();
					if(null == command)
					{
						LogError(messageLogPrefix, "Not Enough Memory (Command WorkArea)", nameFile);
						goto Parse_ErrorEnd;
					}
					command.BootUp();

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeCommand, "id", managerNameSpace);
					command.ID = valueText.Trim();
					if(true == string.IsNullOrEmpty(command.ID))
					{
						LogError(messageLogPrefix, "Command-ID Invelid (Empty)", nameFile);
						goto Parse_ErrorEnd;
					}

					/* MEMO: "Name" is not decoded because only needed on SS6. */

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeCommand, "valueId", managerNameSpace);
					command.IDValue = valueText.Trim();
					/* MEMO: "IDValue" allows the empty. (means "No values") */

					/* Add Command */
					informationSignalSetting.TableCommand.Add(command.ID, command);
				}

				/* Get Value Setting */
				listNode = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "values/value", managerNameSpace);
				if(null == listNode)
				{
					LogError(messageLogPrefix, "\"values/value\"-Node Not Found", nameFile);
					goto Parse_ErrorEnd;
				}
				Information.Value value = null;
				Information.Value.Parameter parameterValue = null;
				System.Xml.XmlNodeList listNodeParam = null;
				foreach(System.Xml.XmlNode nodeValue in listNode)
				{
					value = new Information.Value();
					if(null == value)
					{
						LogError(messageLogPrefix, "Not Enough Memory (Value WorkArea)", nameFile);
						goto Parse_ErrorEnd;
					}
					value.BootUp();

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeValue, "id", managerNameSpace);
					value.ID = valueText.Trim();
					if(true == string.IsNullOrEmpty(value.ID))
					{
						LogError(messageLogPrefix, "Value-ID Invelid (Empty)", nameFile);
						goto Parse_ErrorEnd;
					}

					listNodeParam = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeValue, "params/value", managerNameSpace);
					if(null == listNodeParam)
					{
						LogError(messageLogPrefix, "\"values/value/params/value\"-Node Not Found", nameFile);
						goto Parse_ErrorEnd;
					}
					foreach(System.Xml.XmlNode nodeParameterValue in listNodeParam)
					{
						parameterValue = new Information.Value.Parameter();
						if(null == parameterValue)
						{
							LogError(messageLogPrefix, "Not Enough Memory (Value's parameter WorkArea)", nameFile);
							goto Parse_ErrorEnd;
						}
						parameterValue.CleanUp();

						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParameterValue, "id", managerNameSpace);
						parameterValue.ID = valueText.Trim();
						if(true == string.IsNullOrEmpty(parameterValue.ID))
						{
							LogError(messageLogPrefix, "Value's Parameter-ID Invelid (Empty)", nameFile);
							goto Parse_ErrorEnd;
						}

						/* MEMO: "Name" is not decoded because only needed on SS6. */

						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParameterValue, "type", managerNameSpace);
						valueText = valueText.Trim();
						parameterValue.Type = TypeGetParameterValue(valueText);
						if(Library_SpriteStudio6.Data.Animation.Attribute.Signal.Command.Parameter.KindType.ERROR == parameterValue.Type)
						{
							LogError(messageLogPrefix, "Value's Param Type \"" + valueText +"\" Invelid (Enpty)", nameFile);
							goto Parse_ErrorEnd;
						}

						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeParameterValue, "listId", managerNameSpace);
						parameterValue.IDList = valueText.Trim();
						/* MEMO: "IDList" allows the empty. (means "No Index-List") */

						/* Add Parameter */
						value.TableParameter.Add(parameterValue.ID, parameterValue);
					}

					/* Add Value */
					informationSignalSetting.TableValue.Add(value.ID, value);
				}

				/* Get List Setting */
				listNode = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeRoot, "lists/value", managerNameSpace);
				if(null == listNode)
				{
					LogError(messageLogPrefix, "\"lists/value\"-Node Not Found", nameFile);
					goto Parse_ErrorEnd;
				}
				Information.ListIndex listIndex = null;
				Information.ListIndex.Item itemListIndex = null;
				System.Xml.XmlNodeList listNodeItemList = null;
				foreach(System.Xml.XmlNode nodeList in listNode)
				{
					listIndex = new Information.ListIndex();
					if(null == listIndex)
					{
						LogError(messageLogPrefix, "Not Enough Memory (ListIndex WorkArea)", nameFile);
						goto Parse_ErrorEnd;
					}
					listIndex.BootUp();

					valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeList, "id", managerNameSpace);
					listIndex.ID = valueText.Trim();
					if(true == string.IsNullOrEmpty(listIndex.ID))
					{
						LogError(messageLogPrefix, "ListIndex-ID Invelid (Empty)", nameFile);
						goto Parse_ErrorEnd;
					}

					listNodeItemList = LibraryEditor_SpriteStudio6.Utility.XML.ListGetNode(nodeList, "items/value", managerNameSpace);
					if(null == listNodeItemList)
					{
						LogError(messageLogPrefix, "\"values/value/params/value\"-Node Not Found", nameFile);
						goto Parse_ErrorEnd;
					}
					foreach(System.Xml.XmlNode nodeItemListIndex in listNodeItemList)
					{
						itemListIndex = new Information.ListIndex.Item();
						if(null == itemListIndex)
						{
							LogError(messageLogPrefix, "Not Enough Memory (ListIndex's item WorkArea)", nameFile);
							goto Parse_ErrorEnd;
						}
						itemListIndex.CleanUp();

						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeItemListIndex, "id", managerNameSpace);
						itemListIndex.ID = valueText.Trim();
						if(true == string.IsNullOrEmpty(itemListIndex.ID))
						{
							LogError(messageLogPrefix, "ListIndex Item-ID Invelid (Empty)", nameFile);
							goto Parse_ErrorEnd;
						}

						/* MEMO: Decode the name in case of "ListIndex" (just in case). */
						valueText = LibraryEditor_SpriteStudio6.Utility.XML.TextGetNode(nodeItemListIndex, "name", managerNameSpace);
						itemListIndex.Name = valueText.Trim();
						/* MEMO: "Name" allows the empty, now. */

						/* Add Parameter */
						listIndex.TableItem.Add(itemListIndex.ID, itemListIndex);
					}

					/* Add List */
					informationSignalSetting.TableListIndex.Add(listIndex.ID, listIndex);
				}

				/* Check Link(Relational)-ID */
				{
					string idCheck;

					foreach(KeyValuePair<string, Information.Command> pair in informationSignalSetting.TableCommand)
					{
						idCheck = pair.Value.IDValue;
						if(false == string.IsNullOrEmpty(idCheck))
						{
							if(false == informationSignalSetting.TableValue.ContainsKey(idCheck))
							{
								LogError(	messageLogPrefix,
											"Missing Link Value-ID(" + idCheck + ") in Command-ID(" + pair.Key + ")",
											nameFile
										);
								goto Parse_ErrorEnd;
							}
						}
					}

					foreach(KeyValuePair<string, Information.Value> pair in informationSignalSetting.TableValue)
					{
						foreach(KeyValuePair<string, Information.Value.Parameter> pairParameter in informationSignalSetting.TableValue[pair.Key].TableParameter)
						{
							idCheck = pairParameter.Value.IDList;
							if(false == string.IsNullOrEmpty(idCheck))
							{
								if(false == informationSignalSetting.TableListIndex.ContainsKey(idCheck))
								{
									LogError(	messageLogPrefix,
												"Missing Link List-ID(" + idCheck + ") in Value-ID(" + pair.Key + ", " + pairParameter.Key + ")",
												nameFile
											);
									goto Parse_ErrorEnd;
								}
							}
						}
					}
				}

				return(informationSignalSetting);

			Parse_ErrorEnd:
				if(null != informationSignalSetting)
				{
					informationSignalSetting.CleanUp();
				}
				return(null);
			}
			private static Library_SpriteStudio6.Data.Animation.Attribute.Signal.Command.Parameter.KindType TypeGetParameterValue(string text)
			{
			
				for(int i=0; i<(int)Library_SpriteStudio6.Data.Animation.Attribute.Signal.Command.Parameter.KindType.TERMINATOR; i++)
				{
					if(NameTypeParameterValue[i] == text)
					{
						return((Library_SpriteStudio6.Data.Animation.Attribute.Signal.Command.Parameter.KindType)i);
					}
				}

				return(Library_SpriteStudio6.Data.Animation.Attribute.Signal.Command.Parameter.KindType.ERROR);
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
				CODE_000100 = 0x00000100,	/* under-development (before SS6.4) */
				CODE_010000 = 0x00010000,	/* after SS6.4.0 */

				TARGET_EARLIEST = CODE_010000,
				TARGET_LATEST = CODE_010000
			}

			private const string ExtentionFile = ".ss-signalsettings";

			private static readonly string[] NameTypeParameterValue =
			{
				/* BOOL */		"bool",
				/* INDEX */		"index",
				/* INTEGER */	"integer",
				/* FLOATING */	"floating",
				/* TEXT */		"text",
			};
			#endregion Enums & Constants

			/* ----------------------------------------------- Classes, Structs & Interfaces */
			#region Classes, Structs & Interfaces
			public class Information
			{
				/* ----------------------------------------------- Variables & Properties */
				#region Variables & Properties
				public KindVersion Version;

				public string NameDirectory;
				public string NameFileBody;
				public string NameFileExtension;

				public Dictionary<string, Command> TableCommand;
				public Dictionary<string, Value> TableValue;
				public Dictionary<string, ListIndex> TableListIndex;

				public bool IsValid
				{
					get
					{
						return(KindVersion.ERROR != Version);	/* ? true : false */
					}
				}
				#endregion Variables & Properties

				/* ----------------------------------------------- Functions */
				#region Functions
				public void CleanUp()
				{
					Version = KindVersion.ERROR;

					NameDirectory = "";
					NameFileBody = "";
					NameFileExtension = "";

					TableCommand = null;
					TableValue = null;
					TableListIndex = null;
				}

				public bool BootUp()
				{
					CleanUp();

					TableCommand = new Dictionary<string, Command>();
					if(null == TableCommand)
					{
						return(false);
					}
					TableCommand.Clear();

					TableValue = new Dictionary<string, Value>();
					if(null == TableValue)
					{
						return(false);
					}
					TableValue.Clear();

					TableListIndex = new Dictionary<string, ListIndex>();
					if(null == TableListIndex)
					{
						return(false);
					}
					TableListIndex.Clear();

					return(true);
				}

				public string FileNameGetFullPath()
				{
					return (NameDirectory + NameFileBody + NameFileExtension);
				}
				#endregion Functions

				/* ----------------------------------------------- Classes, Structs & Interfaces */
				#region Classes, Structs & Interfaces
				public class Command
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public string ID;
					public string IDValue;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public Command()
					{
						CleanUp();
					}

					public void CleanUp()
					{
						ID = null;
						IDValue = null;
					}

					public bool BootUp()
					{
						CleanUp();

						ID = string.Empty;
						IDValue = string.Empty;

						return(true);
					}
					#endregion Functions

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					#endregion Classes, Structs & Interfaces
				}

				public class Value
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public string ID;
					public Dictionary<string, Parameter> TableParameter;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public Value()
					{
						CleanUp();
					}

					public void CleanUp()
					{
						ID = null;
						TableParameter = null;
					}

					public bool BootUp()
					{
						CleanUp();

						ID = string.Empty;

						TableParameter = new Dictionary<string, Parameter>();
						if(null == TableParameter)
						{
							return(false);
						}
						TableParameter.Clear();

						return(true);
					}
					#endregion Functions

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					public class Parameter
					{
						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						public string ID;
						public Library_SpriteStudio6.Data.Animation.Attribute.Signal.Command.Parameter.KindType Type;
						public string IDList;
						#endregion Variables & Properties

						/* ----------------------------------------------- Functions */
						#region Functions
						public Parameter()
						{
							CleanUp();
						}

						public void CleanUp()
						{
							ID = null;
							Type = Library_SpriteStudio6.Data.Animation.Attribute.Signal.Command.Parameter.KindType.ERROR;
							IDList = null;
						}

						public bool BootUp()
						{
							CleanUp();

							ID = string.Empty;
							IDList = string.Empty;

							return(true);
						}
						#endregion Functions

						/* ----------------------------------------------- Classes, Structs & Interfaces */
						#region Classes, Structs & Interfaces
						#endregion Classes, Structs & Interfaces
					}
					#endregion Classes, Structs & Interfaces
				}

				public class ListIndex
				{
					/* ----------------------------------------------- Variables & Properties */
					#region Variables & Properties
					public string ID;
					public Dictionary<string, Item> TableItem;
					#endregion Variables & Properties

					/* ----------------------------------------------- Functions */
					#region Functions
					public ListIndex()
					{
						CleanUp();
					}

					public void CleanUp()
					{
						ID = null;
						TableItem = null;
					}

					public bool BootUp()
					{
						CleanUp();

						ID = string.Empty;

						TableItem = new Dictionary<string, Item>();
						TableItem.Clear();

						return(true);
					}
					#endregion Functions

					/* ----------------------------------------------- Classes, Structs & Interfaces */
					#region Classes, Structs & Interfaces
					public class Item
					{
						/* ----------------------------------------------- Variables & Properties */
						#region Variables & Properties
						public string ID;
						public string Name;
						#endregion Variables & Properties

						/* ----------------------------------------------- Functions */
						#region Functions
						public void CleanUp()
						{
							ID = null;
							Name = null;
						}

						public bool BootUp()
						{
							CleanUp();

							ID = string.Empty;
							Name = string.Empty;

							return(true);
						}
						#endregion Functions

						/* ----------------------------------------------- Classes, Structs & Interfaces */
						#region Classes, Structs & Interfaces
						#endregion Classes, Structs & Interfaces
					}
					#endregion Classes, Structs & Interfaces
				}
				#endregion Classes, Structs & Interfaces
			}
			#endregion Classes, Structs & Interfaces
		}
	}
}
