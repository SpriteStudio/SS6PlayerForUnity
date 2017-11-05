/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp.
	All rights reserved.
*/
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Script_SpriteStudio6_RootEffect))]
public class Inspector_SpriteStudio6_RootEffect : Editor
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	private static bool FoldOutStaticDatas;
	private static bool FoldOutMaterialTable;
	private static bool FoldOutMaterialApplied;
	private static bool FoldOutPlayInformation;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	public override void OnInspectorGUI()
	{
		Script_SpriteStudio6_RootEffect data = (Script_SpriteStudio6_RootEffect)target;

		EditorGUILayout.LabelField("[SpriteStudio6 Effect]");
		int levelIndent = 0;

		/* Static Datas */
		EditorGUILayout.Space();
		FoldOutStaticDatas = EditorGUILayout.Foldout(FoldOutStaticDatas, "Static Datas");
		if(true == FoldOutStaticDatas)
		{
			EditorGUI.indentLevel = levelIndent + 1;
			data.DataCellMap = (Script_SpriteStudio6_DataCellMap)(EditorGUILayout.ObjectField("Data:CellMap", data.DataCellMap, typeof(Script_SpriteStudio6_DataCellMap), true));
			data.DataEffect = (Script_SpriteStudio6_DataEffect)(EditorGUILayout.ObjectField("Data:Effect", data.DataEffect, typeof(Script_SpriteStudio6_DataEffect), true));
			EditorGUI.indentLevel = levelIndent;
		}

		/* Table-Material */
		EditorGUILayout.Space();
		FoldOutMaterialTable = EditorGUILayout.Foldout(FoldOutMaterialTable, "Table-Material");
		if(true == FoldOutMaterialTable)
		{
			EditorGUI.indentLevel = levelIndent + 1;
			LibraryEditor_SpriteStudio6.Utility.Inspector.TableMaterialEffect(data.TableMaterial, levelIndent + 1);
			EditorGUI.indentLevel = levelIndent;
		}

		/* Effect */
		EditorGUILayout.Space();
		FoldOutPlayInformation = EditorGUILayout.Foldout(FoldOutPlayInformation, "Initial/Preview Play Setting");
		if(true == FoldOutPlayInformation)
		{
			EditorGUI.indentLevel = levelIndent;

			/* Set Hide */
			data.FlagHideForce = EditorGUILayout.Toggle("Hide Force", data.FlagHideForce);
			EditorGUILayout.Space();

			/* Set Limit draw */
			int limitParticle = EditorGUILayout.IntField("Count Limit Particle", data.LimitParticleDraw);
			EditorGUILayout.LabelField("(0: Default-Value Set)");
			if(0 > limitParticle)
			{
				limitParticle = 0;
			}
			if(limitParticle != data.LimitParticleDraw)
			{
				data.LimitParticleDraw = limitParticle;
			}

			EditorGUI.indentLevel = levelIndent;
		}

		if(true == GUI.changed)
		{
			Undo.RecordObject(target, "SpriteStudio6 Effect");
		}
	}
	#endregion Functions
}
