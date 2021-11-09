/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
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
	private Script_SpriteStudio6_RootEffect InstanceRoot;

	private SerializedProperty PropertyDataCellMap;
	private SerializedProperty PropertyDataEffect;
	private SerializedProperty PropertyTableMaterial;
	private SerializedProperty PropertyHideForce;
	private SerializedProperty PropertyLimitParticle;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	private void OnEnable()
	{
		InstanceRoot = (Script_SpriteStudio6_RootEffect)target;

		serializedObject.FindProperty("__DUMMY__");
		PropertyDataCellMap = serializedObject.FindProperty("DataCellMap");
		PropertyDataEffect = serializedObject.FindProperty("DataEffect");
		PropertyTableMaterial = serializedObject.FindProperty("TableMaterial");
		PropertyHideForce = serializedObject.FindProperty("FlagHideForce");
		PropertyLimitParticle = serializedObject.FindProperty("LimitParticleDraw");
	}

	public override void OnInspectorGUI()
	{
		Script_SpriteStudio6_RootEffect data = (Script_SpriteStudio6_RootEffect)target;

		serializedObject.Update();

		EditorGUILayout.LabelField("[SpriteStudio6 Effect]");
		int levelIndent = 0;

		/* Static Datas */
		EditorGUILayout.Space();
		PropertyDataEffect.isExpanded = EditorGUILayout.Foldout(PropertyDataEffect.isExpanded, "Static Datas");
		if(true == PropertyDataEffect.isExpanded)
		{
			EditorGUI.indentLevel = levelIndent + 1;

			PropertyDataCellMap.objectReferenceValue = (Script_SpriteStudio6_DataCellMap)(EditorGUILayout.ObjectField("Data:CellMap", PropertyDataCellMap.objectReferenceValue, typeof(Script_SpriteStudio6_DataCellMap), true));
			PropertyDataEffect.objectReferenceValue = (Script_SpriteStudio6_DataEffect)(EditorGUILayout.ObjectField("Data:Effect", PropertyDataEffect.objectReferenceValue, typeof(Script_SpriteStudio6_DataEffect), true));
			EditorGUI.indentLevel = levelIndent;
		}

		/* Effect */
		/* MEMO: Use particle-limit's IsExpand since no opportune group. */
		EditorGUILayout.Space();
		PropertyLimitParticle.isExpanded = EditorGUILayout.Foldout(PropertyLimitParticle.isExpanded, "Initial/Preview Play Setting");
		if(true == PropertyLimitParticle.isExpanded)
		{
			EditorGUI.indentLevel = levelIndent;

			/* Hide */
			PropertyHideForce.boolValue = EditorGUILayout.Toggle("Hide Force", PropertyHideForce.boolValue);
			EditorGUILayout.Space();

			/* Limit draw particle */
			int limitParticle = PropertyLimitParticle.intValue;
			int limitParticleNew = EditorGUILayout.IntField("Count Limit Particle",limitParticle);
			EditorGUILayout.LabelField("(0: Default-Value Set)");
			if(0 > limitParticleNew)
			{
				limitParticleNew = 0;
			}
			if(limitParticleNew != limitParticle)
			{
				limitParticle = limitParticleNew;
				PropertyLimitParticle.intValue = limitParticleNew;
			}

			EditorGUI.indentLevel = levelIndent;
		}

		serializedObject.ApplyModifiedProperties();
	}
	#endregion Functions
}
