/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
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
	private static bool FoldOutStaticDatas;
	private static bool FoldOutMaterialTable;
	private static bool FoldOutPlayInformation;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	public override void OnInspectorGUI()
	{
		const string NameMissing = "(Data Missing)";

		Script_SpriteStudio6_Root data = (Script_SpriteStudio6_Root)target;

		EditorGUILayout.LabelField("[SpriteStudio6 Animation]");
		int levelIndent = 0;

		/* Static Datas */
		EditorGUILayout.Space();
		FoldOutStaticDatas = EditorGUILayout.Foldout(FoldOutStaticDatas, "Static Datas");
		if(true == FoldOutStaticDatas)
		{
			EditorGUI.indentLevel = levelIndent + 1;
			data.DataCellMap = (Script_SpriteStudio6_DataCellMap)(EditorGUILayout.ObjectField("Data:CellMap", data.DataCellMap, typeof(Script_SpriteStudio6_DataCellMap), true));
			data.DataAnimation = (Script_SpriteStudio6_DataAnimation)(EditorGUILayout.ObjectField("Data:Animation", data.DataAnimation, typeof(Script_SpriteStudio6_DataAnimation), true));
			EditorGUI.indentLevel = levelIndent;
		}

		/* Table-Material */
		EditorGUILayout.Space();
		FoldOutMaterialTable = EditorGUILayout.Foldout(FoldOutMaterialTable, "Table-Material");
		if(true == FoldOutMaterialTable)
		{
			EditorGUI.indentLevel = levelIndent + 1;
			LibraryEditor_SpriteStudio6.Utility.Inspector.TableMaterialAnimation(data.TableMaterial, levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		/* Animation */
		Script_SpriteStudio6_DataAnimation dataAnimation = data.DataAnimation;
//		Script_SpriteStudio6_DataCellMap dataCellMap = data.DataCellMap;

		EditorGUILayout.Space();
		FoldOutPlayInformation = EditorGUILayout.Foldout(FoldOutPlayInformation, "Initial/Preview Play Setting");
		if(true == FoldOutPlayInformation)
		{
			bool flagUpdate = false;

			/* Set Hide */
			data.FlagHideForce = EditorGUILayout.Toggle("Hide Force", data.FlagHideForce);
			EditorGUILayout.Space();

			if(null == dataAnimation)
			{
				EditorGUILayout.LabelField("(Animation Data Missin)");
			}
			else
			{
				/* Creation animation name table */
				int countAnimation = data.CountGetAnimation();
				string[] tableNameAnimation = new string[countAnimation];
				for(int i=0; i<countAnimation; i++)
				{
					tableNameAnimation[i] = dataAnimation.TableAnimation[i].Name;
					if(true == string.IsNullOrEmpty(tableNameAnimation[i]))
					{
						tableNameAnimation[i] = NameMissing;
					}
				}

				InfromationPlay(ref flagUpdate,ref data.TableInformationPlay[0],data, tableNameAnimation);

				/* RePlay Animation */
				if(true == flagUpdate)
				{
					Undo.RecordObject(target, "SpriteStudio6 Animation");
					data.AnimationPlayInitial();
				}
			}
		}
	}

	private void InfromationPlay(	ref bool flagUpdate,
									ref Script_SpriteStudio6_Root.InformationPlay informationPlay,
									Script_SpriteStudio6_Root instanceRoot,
									string[] tableNameAnimation
								)
	{
		/* Set "Initial Stop" */
		informationPlay.FlagStopInitial = EditorGUILayout.Toggle("Initial Stop", informationPlay.FlagStopInitial);
		EditorGUILayout.Space();

		/* "Animation" Select */
		int indexAnimation = instanceRoot.IndexGetAnimation(informationPlay.NameAnimation);
		if((0 > indexAnimation) || (tableNameAnimation.Length <= indexAnimation))
		{
			indexAnimation = 0;
			flagUpdate |= true;
		}
		int indexNow = EditorGUILayout.Popup("Animation Name", indexAnimation, tableNameAnimation);
		if(indexNow != indexAnimation)
		{
			indexAnimation = indexNow;
			informationPlay.NameAnimation = instanceRoot.DataAnimation.TableAnimation[indexAnimation].Name;
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
		int indexTableLabelStart = -1;
		int indexTableLabelEnd = -1;
		for(int i=0; i<countLabel; i++)
		{
			if(tableNameLabel[i] == informationPlay.LabelStart)
			{
				indexTableLabelStart = i;
				break;
			}
		}
		for(int i=0; i<countLabel; i++)
		{
			if(tableNameLabel[i] == informationPlay.LabelEnd)
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

		int offsetStart = informationPlay.FrameOffsetStart;
		int offsetEnd = informationPlay.FrameOffsetEnd;
		int frameLabelStart = tableFrameLabel[indexTableLabelStart];
		int frameLabelEnd = tableFrameLabel[indexTableLabelEnd];
		int indexTableLabel;
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
		indexTableLabel = EditorGUILayout.Popup("Range Start Label", indexTableLabelStart, tableNameLabel);
		if(indexTableLabel != indexTableLabelStart)
		{	/* Data is valid & Changed Animation */
			indexTableLabelStart = indexTableLabel;
			flagUpdate |= true;
		}
		informationPlay.LabelStart = tableNameLabel[indexTableLabelStart];
		frameLabelStart = tableFrameLabel[indexTableLabelStart];

		/* Start-Offset */
		frameLimit = frameLabelEnd + offsetEnd;
		offsetStart = EditorGUILayout.IntField("Range Start Offset", informationPlay.FrameOffsetStart);
		EditorGUILayout.LabelField(	"- Valid Value Range: Min[" + (frameValidStart - frameLabelStart).ToString() +
									"] to Max[" + ((frameLimit - frameLabelStart) - 1).ToString() + "] "	/* -1 ... End frame */
								);

		offsetStart = (frameLimit <= (frameLabelStart + offsetStart)) ? ((frameLimit - frameLabelStart) - 1) : offsetStart;
		offsetStart = (frameValidStart > (frameLabelStart + offsetStart)) ? (frameValidStart - frameLabelStart) : offsetStart;
		offsetStart = (frameValidEnd < (frameLabelStart + offsetStart)) ? (frameValidEnd - frameLabelStart) : offsetStart;
		if(informationPlay.FrameOffsetStart != offsetStart)
		{
			informationPlay.FrameOffsetStart = offsetStart;
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
		indexTableLabel = EditorGUILayout.Popup("Range End Label", indexTableLabelEnd, tableNameLabel);
		if(indexTableLabel != indexTableLabelEnd)
		{	/* Data is valid & Changed Animation */
			indexTableLabelEnd = indexTableLabel;
			flagUpdate |= true;
		}
		informationPlay.LabelEnd = tableNameLabel[indexTableLabelEnd];
		frameLabelEnd = tableFrameLabel[indexTableLabelEnd];

		/* End-Offset */
		frameLimit = frameLabelStart + offsetStart;
		offsetEnd = EditorGUILayout.IntField("Range End Offset", informationPlay.FrameOffsetEnd);
		EditorGUILayout.LabelField(	"- Valid Value Range: Min[" + ((frameLimit - frameLabelEnd) + 1).ToString() +	/* +1 ... Start frame */
									"] to Max[" + (frameValidEnd - frameLabelEnd).ToString() + "] "
								);

		offsetEnd = (frameLimit >= (frameLabelEnd + offsetEnd)) ? ((frameLimit - frameLabelEnd) + 1) : offsetEnd;
		offsetEnd = (frameValidStart > (frameLabelEnd + offsetEnd)) ? (frameValidStart - frameLabelEnd) : offsetEnd;
		offsetEnd = (frameValidEnd < (frameLabelEnd + offsetEnd)) ? (frameValidEnd - frameLabelEnd) : offsetEnd;
		if(informationPlay.FrameOffsetEnd != offsetEnd)
		{
			informationPlay.FrameOffsetEnd = offsetEnd;
			flagUpdate |= true;
		}

		/* Play Pingpong */
		EditorGUILayout.Space();
		bool flagPingPong = EditorGUILayout.Toggle("Play-Pingpong", informationPlay.FlagPingPong);
		if(flagPingPong != informationPlay.FlagPingPong)
		{
			informationPlay.FlagPingPong = flagPingPong;
			flagUpdate |= true;
		}

		/* Rate-Time */
		EditorGUILayout.Space();
		if(0.0f == informationPlay.RateTime)
		{
			informationPlay.RateTime = 1.0f;
		}
		float rateTime = EditorGUILayout.FloatField("Rate Time-Progress", informationPlay.RateTime);
		EditorGUILayout.LabelField("(set Negative-Value, Play Backwards.)");
		if(rateTime != informationPlay.RateTime)
		{
			informationPlay.RateTime = rateTime;
			flagUpdate |= true;
		}

		/* Play-Times */
		EditorGUILayout.Space();
		int timesPlay = EditorGUILayout.IntField("Number of Plays", informationPlay.TimesPlay);
		EditorGUILayout.LabelField("(1: No Loop / 0: Infinite Loop)");
		if(timesPlay != informationPlay.TimesPlay)
		{
			informationPlay.TimesPlay = timesPlay;
			flagUpdate |= true;
		}

		/* Reset */
		EditorGUILayout.Space();
		if(true == GUILayout.Button("Reset (Reinitialize)"))
		{
			informationPlay.NameAnimation = tableNameAnimation[0];
			informationPlay.Frame = 0;
			informationPlay.RateTime = 1.0f;
			informationPlay.TimesPlay = 0;
			informationPlay.FlagPingPong = false;
			informationPlay.LabelStart = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START];
			informationPlay.FrameOffsetStart = 0;
			informationPlay.LabelEnd = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END];
			informationPlay.FrameOffsetEnd = 0;
			informationPlay.FlagStopInitial = false;
			flagUpdate = true;	/* Force */
		}
	}
	#endregion Functions
}