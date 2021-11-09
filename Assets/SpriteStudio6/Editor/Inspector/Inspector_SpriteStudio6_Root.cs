/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Script_SpriteStudio6_Root))]
public class Inspector_SpriteStudio6_Root : Editor
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	private Script_SpriteStudio6_Root InstanceRoot;

	private SerializedProperty PropertyDataCellMap;
	private SerializedProperty PropertyDataAnimation;
	private SerializedProperty PropertyHideForce;
	private SerializedProperty PropertyColliderInterlockHideForce;
	private SerializedProperty PropertyFlagPlanarization;
	private SerializedProperty PropertyOrderInLayer;
	private SerializedProperty PropertyCountTrack;
	private SerializedProperty PropertyInformationPlay;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	private void OnEnable()
	{
		InstanceRoot = (Script_SpriteStudio6_Root)target;

		serializedObject.FindProperty("__DUMMY__");
		PropertyDataCellMap = serializedObject.FindProperty("DataCellMap");
		PropertyDataAnimation = serializedObject.FindProperty("DataAnimation");
		PropertyHideForce = serializedObject.FindProperty("FlagHideForce");
		PropertyColliderInterlockHideForce = serializedObject.FindProperty("FlagColliderInterlockHideForce");
		PropertyFlagPlanarization = serializedObject.FindProperty("FlagPlanarization");
		PropertyOrderInLayer = serializedObject.FindProperty("OrderInLayer");
		PropertyCountTrack = serializedObject.FindProperty("LimitTrack");
		PropertyInformationPlay = serializedObject.FindProperty("TableInformationPlay");
	}

	public override void OnInspectorGUI()
	{
		const string NameMissing = "(Data Missing)";

		serializedObject.Update();

		EditorGUILayout.LabelField("[SpriteStudio6 Animation]");
		int levelIndent = 0;
		bool flagUpdate = false;
		bool flagUpdateLimitTrack = false;

		/* Static Datas */
		EditorGUILayout.Space();
		PropertyDataAnimation.isExpanded = EditorGUILayout.Foldout(PropertyDataAnimation.isExpanded, "Static Datas");
		if(true == PropertyDataAnimation.isExpanded)
		{
			EditorGUI.indentLevel = levelIndent + 1;

			PropertyDataCellMap.objectReferenceValue = (Script_SpriteStudio6_DataCellMap)(EditorGUILayout.ObjectField("Data:CellMap", PropertyDataCellMap.objectReferenceValue, typeof(Script_SpriteStudio6_DataCellMap), true));
			PropertyDataAnimation.objectReferenceValue = (Script_SpriteStudio6_DataAnimation)(EditorGUILayout.ObjectField("Data:Animation", PropertyDataAnimation.objectReferenceValue, typeof(Script_SpriteStudio6_DataAnimation), true));
			EditorGUI.indentLevel = levelIndent;
		}

		/* Animation */
		Script_SpriteStudio6_DataAnimation dataAnimation = InstanceRoot.DataAnimation;
//		Script_SpriteStudio6_DataCellMap dataCellMap = InstanceRoot.DataCellMap;

		EditorGUILayout.Space();
		PropertyInformationPlay.isExpanded = EditorGUILayout.Foldout(PropertyInformationPlay.isExpanded, "Initial/Preview Play Setting");
		if(true == PropertyInformationPlay.isExpanded)
		{
			/* Order-In-Layer (SortingOrder) */
			PropertyOrderInLayer.intValue = EditorGUILayout.IntField("Order In Layer", PropertyOrderInLayer.intValue);
			EditorGUILayout.LabelField("(0: Default)");
			EditorGUILayout.Space();

			/* Hide */
			PropertyHideForce.boolValue = EditorGUILayout.Toggle("Hide Force", PropertyHideForce.boolValue);
//			EditorGUILayout.Space();

			/* Collider Interlock Hide */
			PropertyColliderInterlockHideForce.boolValue = EditorGUILayout.Toggle("Collider Interlock Hide", PropertyColliderInterlockHideForce.boolValue);
//			EditorGUILayout.Space();

			/* Planarization (Cancellation Rotate Sprite) */
			PropertyFlagPlanarization.boolValue = EditorGUILayout.Toggle("Planarization", PropertyFlagPlanarization.boolValue);
			EditorGUILayout.Space();

			/* Track */
			int countTrack = EditorGUILayout.IntField("Number of Track", PropertyCountTrack.intValue);
			EditorGUILayout.LabelField("(0: Default)");
			if(0 >= countTrack)
			{
				countTrack = 0;
			}
			if(PropertyCountTrack.intValue != countTrack)
			{
				PropertyCountTrack.intValue = countTrack;
				flagUpdateLimitTrack |= true;
				flagUpdate |= true;
			}
			EditorGUILayout.Space();

			if(null == dataAnimation)
			{
				EditorGUILayout.LabelField("(Animation Data Missing)");
			}
			else
			{
				/* Creation animation name table */
				int countAnimation = dataAnimation.CountGetAnimation();
				string[] tableNameAnimation = new string[countAnimation];
				for(int i=0; i<countAnimation; i++)
				{
					tableNameAnimation[i] = dataAnimation.TableAnimation[i].Name;
					if(true == string.IsNullOrEmpty(tableNameAnimation[i]))
					{
						tableNameAnimation[i] = NameMissing;
					}
				}

				SerializedProperty propertyInformationPlay = PropertyInformationPlay.GetArrayElementAtIndex(0);
				InformationPlay(ref flagUpdate, propertyInformationPlay, InstanceRoot, tableNameAnimation);
			}
		}

		serializedObject.ApplyModifiedProperties();

		/* Reset Track */
		if(true == flagUpdateLimitTrack)
		{
			InstanceRoot.TrackReboot(InstanceRoot.LimitTrack);
		}

		/* Reset Animation */
		if(true == flagUpdate)
		{
			InstanceRoot.AnimationStop(-1);
			InstanceRoot.AnimationPlayInitial();
		}
	}

	private void InformationPlay(	ref bool flagUpdate,
									SerializedProperty propertyInformationPlay,
									Script_SpriteStudio6_Root instanceRoot,
									string[] tableNameAnimation
								)
	{
		SerializedProperty propertyFlagStopInitial = propertyInformationPlay.FindPropertyRelative("FlagStopInitial");
		SerializedProperty propertyNameAnition = propertyInformationPlay.FindPropertyRelative("NameAnimation");
		SerializedProperty propertyFlagPingPong = propertyInformationPlay.FindPropertyRelative("FlagPingPong");
		SerializedProperty propertyLabelStart = propertyInformationPlay.FindPropertyRelative("LabelStart");
		SerializedProperty propertyFrameOffsetStart= propertyInformationPlay.FindPropertyRelative("FrameOffsetStart");
		SerializedProperty propertyLabelEnd = propertyInformationPlay.FindPropertyRelative("LabelEnd");
		SerializedProperty propertyFrameOffsetEnd = propertyInformationPlay.FindPropertyRelative("FrameOffsetEnd");
		SerializedProperty propertyFrame = propertyInformationPlay.FindPropertyRelative("Frame");
		SerializedProperty propertyTimesPlay = propertyInformationPlay.FindPropertyRelative("TimesPlay");
		SerializedProperty propertyRateTime = propertyInformationPlay.FindPropertyRelative("RateTime");

		/* Set "Initial Stop" */
		propertyFlagStopInitial.boolValue = EditorGUILayout.Toggle("Initial Stop", propertyFlagStopInitial.boolValue);
		EditorGUILayout.Space();

		/* "Animation" Select */
		int indexAnimation = instanceRoot.IndexGetAnimation(propertyNameAnition.stringValue);
		if((0 > indexAnimation) || (tableNameAnimation.Length <= indexAnimation))
		{
			indexAnimation = 0;
			flagUpdate |= true;
		}
		int indexNow = EditorGUILayout.Popup("Animation Name", indexAnimation, tableNameAnimation);
		if(indexNow != indexAnimation)
		{
			indexAnimation = indexNow;
			propertyNameAnition.stringValue = instanceRoot.DataAnimation.TableAnimation[indexAnimation].Name;
			flagUpdate |= true;
		}

		Library_SpriteStudio6.Data.Animation dataAnimation = instanceRoot.DataAnimation.TableAnimation[indexAnimation];
//		int frameAnimationEnd = dataAnimation.CountFrame - 1;
		int frameValidStart = dataAnimation.FrameValidStart;
		int frameValidEnd = dataAnimation.FrameValidEnd;
		int countFrameValid = dataAnimation.CountFrameValid;
		EditorGUILayout.LabelField("- Valid Frame: " + frameValidStart.ToString() + " - " + frameValidEnd.ToString() + "  (Count: " + countFrameValid.ToString() + ")");

		/* Create "Label" table */
		string[] tableNameLabel = null;
		int[] tableIndexLabel = null;
		int[] tableFrameLabel = null;

		int countLabel = dataAnimation.CountGetLabel();
		if(0 >= countLabel)
		{	/* Has no labels */
			countLabel = 2;

			tableNameLabel = new string[countLabel];
			tableIndexLabel = new int[countLabel];
			tableFrameLabel = new int[countLabel];
		}
		else
		{	/* Has labels */
			countLabel += 2;	/* +2 ... "_start" and "_end" (Reserved-Labels) */

			tableNameLabel = new string[countLabel];
			tableIndexLabel = new int[countLabel];
			tableFrameLabel = new int[countLabel];

			int indexTable;
			for(int i=0; i<(countLabel - 2); i++)
			{
				indexTable = i + 1;
				tableNameLabel[indexTable] = dataAnimation.TableLabel[i].Name;
				tableIndexLabel[indexTable] = i;
				tableFrameLabel[indexTable] = dataAnimation.TableLabel[i].Frame;
			}
		}

		tableNameLabel[0] = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START];
		tableIndexLabel[0] = (int)(Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START | Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.INDEX_RESERVED);
		tableFrameLabel[0] = frameValidStart;

		tableNameLabel[countLabel - 1] = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END];
		tableIndexLabel[countLabel - 1] = (int)(Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END | Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.INDEX_RESERVED);
		tableFrameLabel[countLabel - 1] = frameValidEnd;

		/* Get current labels */
		string nameLabelStart = propertyLabelStart.stringValue;
		string nameLabelEnd = propertyLabelEnd.stringValue;
		int indexTableLabelStart = -1;
		int indexTableLabelEnd = -1;
		for(int i=0; i<countLabel; i++)
		{
			if(tableNameLabel[i] == nameLabelStart)
			{
				indexTableLabelStart = i;
				break;
			}
		}
		for(int i=0; i<countLabel; i++)
		{
			if(tableNameLabel[i] == nameLabelEnd)
			{
				indexTableLabelEnd = i;
				break;
			}
		}
		if(0 > indexTableLabelStart)
		{
			indexTableLabelStart = 0;
		}
		if(0 > indexTableLabelEnd)
		{
			indexTableLabelEnd = countLabel - 1;
		}

		int offsetStart = propertyFrameOffsetStart.intValue;
		int offsetEnd = propertyFrameOffsetEnd.intValue;
		int frameLabelStart = tableFrameLabel[indexTableLabelStart];
		int frameLabelEnd = tableFrameLabel[indexTableLabelEnd];
		int indexTableLabelNew;
		int frameLimit;

		/* Range "Start" */
		EditorGUILayout.Space();
		EditorGUILayout.LabelField(	"Range Start: (" + frameLabelStart.ToString()
									+ " + "
									+ offsetStart.ToString()
									+ ") = "
									+ (frameLabelStart + offsetStart).ToString()
								);

		/* Start-Label Select */
		indexTableLabelNew = EditorGUILayout.Popup("Range Start Label", indexTableLabelStart, tableNameLabel);
		if(indexTableLabelNew != indexTableLabelStart)
		{
			indexTableLabelStart = indexTableLabelNew;
			propertyLabelStart.stringValue = tableNameLabel[indexTableLabelStart];
			flagUpdate |= true;
		}
		frameLabelStart = tableFrameLabel[indexTableLabelStart];

		/* Start-Offset */
		frameLimit = frameLabelEnd + offsetEnd;
		int offsetStartNew = EditorGUILayout.IntField("Range Start Offset", offsetStart);
		EditorGUILayout.LabelField(	"- Valid Value Range: Min[" + (frameValidStart - frameLabelStart).ToString() +
									"] to Max[" + ((frameLimit - frameLabelStart) - 1).ToString() + "] "	/* -1 ... End frame */
								);

		offsetStartNew = (frameLimit <= (frameLabelStart + offsetStartNew)) ? ((frameLimit - frameLabelStart) - 1) : offsetStartNew;
		offsetStartNew = (frameValidStart > (frameLabelStart + offsetStartNew)) ? (frameValidStart - frameLabelStart) : offsetStartNew;
		offsetStartNew = (frameValidEnd < (frameLabelStart + offsetStartNew)) ? (frameValidEnd - frameLabelStart) : offsetStartNew;
		if(offsetStartNew != offsetStart)
		{
			offsetStart = offsetStartNew;
			propertyFrameOffsetStart.intValue = offsetStartNew;
			flagUpdate |= true;
		}

		/* Range "End" */
		EditorGUILayout.Space();
		EditorGUILayout.LabelField(	"Range End: (" + frameLabelEnd.ToString()
									+ " + "
									+ offsetEnd.ToString()
									+ ") = "
									+ (frameLabelEnd + offsetEnd).ToString()
								);

		/* End-Label Select */
		indexTableLabelNew = EditorGUILayout.Popup("Range End Label", indexTableLabelEnd, tableNameLabel);
		if(indexTableLabelNew != indexTableLabelEnd)
		{	/* Data is valid & Changed Animation */
			indexTableLabelEnd = indexTableLabelNew;
			propertyLabelEnd.stringValue = tableNameLabel[indexTableLabelEnd];
			flagUpdate |= true;
		}
		frameLabelEnd = tableFrameLabel[indexTableLabelEnd];

		/* End-Offset */
		frameLimit = frameLabelStart + offsetStart;
		int offsetEndNew = EditorGUILayout.IntField("Range End Offset", offsetEnd);
		EditorGUILayout.LabelField(	"- Valid Value Range: Min[" + ((frameLimit - frameLabelEnd) + 1).ToString() +	/* +1 ... Start frame */
									"] to Max[" + (frameValidEnd - frameLabelEnd).ToString() + "] "
								);

		offsetEndNew = (frameLimit >= (frameLabelEnd + offsetEndNew)) ? ((frameLimit - frameLabelEnd) + 1) : offsetEndNew;
		offsetEndNew = (frameValidStart > (frameLabelEnd + offsetEndNew)) ? (frameValidStart - frameLabelEnd) : offsetEndNew;
		offsetEndNew = (frameValidEnd < (frameLabelEnd + offsetEndNew)) ? (frameValidEnd - frameLabelEnd) : offsetEndNew;
		if(offsetEndNew != offsetEnd)
		{
			offsetEnd = offsetEndNew;
			propertyFrameOffsetEnd.intValue = offsetEndNew;
			flagUpdate |= true;
		}

		/* Play Pingpong */
		EditorGUILayout.Space();
		bool flagPingPong = propertyFlagPingPong.boolValue;
		bool flagPingPongNew = EditorGUILayout.Toggle("Play-Pingpong", flagPingPong);
		if(flagPingPongNew != flagPingPong)
		{
			propertyFlagPingPong.boolValue = flagPingPongNew;
			flagUpdate |= true;
		}

		/* Rate-Time */
		EditorGUILayout.Space();
		float rateTime = propertyRateTime.floatValue;
		if(0.0f == rateTime)
		{
			rateTime = 1.0f;
		}
		float rateTimeNew = EditorGUILayout.FloatField("Rate Time-Progress", rateTime);
		EditorGUILayout.LabelField("(set Negative-Value, Play Backwards.)");
		if(rateTimeNew != rateTime)
		{
			propertyRateTime.floatValue = rateTimeNew;
			flagUpdate |= true;
		}

		/* Play-Times */
		EditorGUILayout.Space();
		int timesPlay = propertyTimesPlay.intValue;
		int timesPlayNew = EditorGUILayout.IntField("Number of Plays", timesPlay);
		EditorGUILayout.LabelField("(1: No Loop / 0: Infinite Loop)");
		if(timesPlayNew != timesPlay)
		{
			propertyTimesPlay.intValue = timesPlayNew;
			flagUpdate |= true;
		}

		/* Reset */
		EditorGUILayout.Space();
		if(true == GUILayout.Button("Reset (Reinitialize)"))
		{
			propertyNameAnition.stringValue = tableNameAnimation[0];
			propertyFrame.intValue = 0;
			propertyRateTime.floatValue = 1.0f;
			propertyTimesPlay.intValue = 0;
			propertyFlagPingPong.boolValue = false;
			propertyLabelStart.stringValue = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START];
			propertyFrameOffsetStart.intValue = 0;
			propertyLabelEnd.stringValue = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END];
			propertyFrameOffsetEnd.intValue = 0;
			propertyFlagStopInitial.boolValue = false;
			flagUpdate = true;	/* Force */
		}
	}
	#endregion Functions
}