/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Script_SpriteStudio6_Sequence))]
public class Inspector_SpriteStudio6_Sequence : Editor
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	private Script_SpriteStudio6_Sequence InstanceSequence;

	private SerializedProperty PropertyDataProject;
	private SerializedProperty PropertyNameSequencePack;
	private SerializedProperty PropertyNameDataSequence;
	private SerializedProperty PropertyIndexStepInitial;

	private SerializedProperty PropertyHideForce;
	private SerializedProperty PropertyColliderInterlockHideForce;
	private SerializedProperty PropertyFlagPlanarization;
	private SerializedProperty PropertyOrderInLayer;

	private SerializedProperty PropertyStopInitial;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	private void OnEnable()
	{
		InstanceSequence = (Script_SpriteStudio6_Sequence)target;

		serializedObject.FindProperty("__DUMMY__");
		PropertyDataProject = serializedObject.FindProperty("DataProject");
		PropertyNameSequencePack = serializedObject.FindProperty("NameSequencePack");
		PropertyNameDataSequence = serializedObject.FindProperty("NameDataSequence");
		PropertyIndexStepInitial = serializedObject.FindProperty("IndexStepInitial");

		PropertyHideForce = serializedObject.FindProperty("FlagHideForce");
		PropertyColliderInterlockHideForce = serializedObject.FindProperty("FlagColliderInterlockHideForce");
		PropertyFlagPlanarization = serializedObject.FindProperty("FlagPlanarization");
		PropertyOrderInLayer = serializedObject.FindProperty("OrderInLayer");

		PropertyStopInitial = serializedObject.FindProperty("FlagStopInitial");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.LabelField("[SpriteStudio6 Sequence]");
		int levelIndent = 0;
		bool flagUpdate = false;

		/* Static Datas */
		EditorGUILayout.Space();
		PropertyDataProject.isExpanded = EditorGUILayout.Foldout(PropertyDataProject.isExpanded, "Static Datas");
		if(true == PropertyDataProject.isExpanded)
		{
			EditorGUI.indentLevel = levelIndent + 1;

			UnityEngine.Object dataProjectNew = EditorGUILayout.ObjectField("Data:Project", PropertyDataProject.objectReferenceValue, typeof(Script_SpriteStudio6_DataProject), true);
			if(PropertyDataProject.objectReferenceValue != dataProjectNew)
			{
				PropertyDataProject.objectReferenceValue = dataProjectNew as Script_SpriteStudio6_DataProject;
				flagUpdate |= true;
			}
			EditorGUI.indentLevel = levelIndent;
		}

		Script_SpriteStudio6_DataProject dataProject = InstanceSequence.DataProject;

		/* Play Information */
		EditorGUILayout.Space();
		PropertyNameSequencePack.isExpanded = EditorGUILayout.Foldout(PropertyNameSequencePack.isExpanded, "Initial/Preview Play Setting");
		if(true == PropertyNameSequencePack.isExpanded)
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

			/* Sequence */
			if(null == dataProject)
			{
				EditorGUILayout.LabelField("(Project Data Missing)");
				PropertyNameSequencePack.stringValue = null;	/* InstanceSequence.PackSetNoUse(); */
				PropertyNameDataSequence.stringValue = null;
				PropertyIndexStepInitial.intValue = 0;
			}
			else
			{
				/* Creation Sequence name table */
				InformationPlay(ref flagUpdate, dataProject, InstanceSequence);
			}
		}

		serializedObject.ApplyModifiedProperties();
	}
	private void InformationPlay(	ref bool flagUpdate,
									Script_SpriteStudio6_DataProject dataProject,
									Script_SpriteStudio6_Sequence instanceSequence
								)
	{
		int indexCursor;

		/* "Sequence-Pack" Select */
		string nameSequencePack = PropertyNameSequencePack.stringValue;

		int indexSequencePack;
		string[] tableNameSequence;
		ListGetSequencePack(out tableNameSequence, out indexSequencePack, dataProject, nameSequencePack);
		indexCursor = EditorGUILayout.Popup("Sequence-Pack Name", indexSequencePack, tableNameSequence);
		if(indexCursor != indexSequencePack)
		{
			flagUpdate |= true;
			indexSequencePack = indexCursor;
		}
		Script_SpriteStudio6_DataSequence dataSequencePack = dataProject.DataSequence[indexSequencePack];
		nameSequencePack = dataSequencePack.Name;

		/* "Sequence" Select */
		string nameSequenceData = PropertyNameDataSequence.stringValue;

		int indexSequenceData;
		string[] tableNameSequenceData;
		ListGetSequenceData(out tableNameSequenceData, out indexSequenceData, dataSequencePack, nameSequenceData);
		indexCursor = EditorGUILayout.Popup("Sequence-Data Name", indexSequenceData, tableNameSequenceData);
		if(indexCursor != indexSequenceData)
		{
			flagUpdate |= true;
			indexSequenceData = indexCursor;
		}
		nameSequenceData = dataSequencePack.TableSequence[indexSequenceData].Name;

		/* Initial Step */
		int indexStepInitial = EditorGUILayout.IntField("Start Step", PropertyIndexStepInitial.intValue);
		if(PropertyIndexStepInitial.intValue != indexStepInitial)
		{
			flagUpdate |= true;

			int countStep = dataSequencePack.TableSequence[indexSequenceData].TableStep.Length;
			if(0 > indexStepInitial)
			{
				indexStepInitial = 0;
			}
			if(countStep <= indexStepInitial)
			{
				indexStepInitial = countStep - 1;
			}
		}

		/* Initial Stop */
		bool flagUpdateInitialStop = false;
		bool flagCheck = EditorGUILayout.Toggle("Initial Stop", PropertyStopInitial.boolValue);
		if(flagCheck != PropertyStopInitial.boolValue)
		{
			flagUpdateInitialStop |= true;
			PropertyStopInitial.boolValue = flagCheck;
		}

		/* Reset */
		EditorGUILayout.Space();
		if(true == GUILayout.Button("Reset (Reinitialize)"))
		{
			PropertyHideForce.boolValue = false;
			PropertyColliderInterlockHideForce.boolValue = false;
			PropertyFlagPlanarization.boolValue = false;

			indexSequencePack = 0;
			dataSequencePack = dataProject.DataSequence[indexSequencePack];
			nameSequencePack = dataSequencePack.Name;

			indexSequenceData = 0;
			nameSequenceData = dataSequencePack.TableSequence[indexSequenceData].Name;

			indexStepInitial = 0;

			flagUpdate = true;	/* Force */
		}

		/* Check Update */
		if(true == flagUpdate)
		{
			/* Update Properties */
			PropertyNameSequencePack.stringValue = nameSequencePack;
			PropertyNameDataSequence.stringValue = nameSequenceData;
			PropertyIndexStepInitial.intValue = indexStepInitial;

			/* Play Start */
			instanceSequence.Stop(false, false);
			if(true == instanceSequence.PackSet(indexSequencePack))
			{
				if(true == instanceSequence.SequenceSet(indexSequenceData))
				{
					instanceSequence.Play(indexStepInitial);
				}
			}
		}
		flagUpdate |= flagUpdateInitialStop;
	}
	private bool ListGetSequencePack(	out string[] tableNameSequence,
										out int indexSequencePack,
										Script_SpriteStudio6_DataProject dataProject,
										string nameSequencePack
									)
	{
		int countSequence = dataProject.DataSequence.Length;	/* InstanceSequence.CountGetPack(); */
		bool flagNameIsValid = (false == string.IsNullOrEmpty(nameSequencePack));	/* ? true : false */
		tableNameSequence = new string[countSequence];
		indexSequencePack = 0;
		for(int i=0; i<countSequence; i++)
		{
			tableNameSequence[i] = dataProject.DataSequence[i].Name;
			if(true == string.IsNullOrEmpty(tableNameSequence[i]))
			{
				tableNameSequence[i] = NameMissing;
			}
			if((true == flagNameIsValid) && (tableNameSequence[i] == nameSequencePack))
			{
				indexSequencePack = i;
			}
		}

		return(true);
	}
	private bool ListGetSequenceData(	out string[] tableNameSequenceData,
										out int indexSequenceData,
										Script_SpriteStudio6_DataSequence dataSequencePack,
										string nameSequenceData
									)
	{
		int countSequenceData = (null == dataSequencePack) ? 0 : dataSequencePack.TableSequence.Length;
		bool flagNameIsValid = (false == string.IsNullOrEmpty(nameSequenceData));	/* ? true : false */
		tableNameSequenceData = new string[countSequenceData];
		indexSequenceData = 0;
		for(int i=0; i<countSequenceData; i++)
		{
			tableNameSequenceData[i] = dataSequencePack.TableSequence[i].Name;
			if(true == string.IsNullOrEmpty(tableNameSequenceData[i]))
			{
				tableNameSequenceData[i] = NameMissing;
			}
			if((true == flagNameIsValid) && (tableNameSequenceData[i] == nameSequenceData))
			{
				indexSequenceData = i;
			}
		}

		return(true);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	private const string NameMissing = "(Data Missing)";
	#endregion Enums & Constants
}