/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Script_SpriteStudio6_Replicate))]
public class Inspector_SpriteStudio6_Replicate : Editor
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	private Script_SpriteStudio6_Replicate InstanceReplicate;

	private SerializedProperty PropertyRootOriginal;
	private SerializedProperty PropertyRootEffectOriginal;
	private SerializedProperty PropertySequenceOriginal;
	private SerializedProperty PropertyHideForce;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	private void OnEnable()
	{
		InstanceReplicate = (Script_SpriteStudio6_Replicate)target;

		serializedObject.FindProperty("__DUMMY__");

		PropertyRootOriginal = serializedObject.FindProperty("InstanceRootOriginal");
		PropertyRootEffectOriginal = serializedObject.FindProperty("InstanceRootEffectOriginal");
		PropertySequenceOriginal = serializedObject.FindProperty("InstanceSequenceOriginal");
		PropertyHideForce = serializedObject.FindProperty("FlagHideForce");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.LabelField("[SpriteStudio6 Replicate Animation]");
		int levelIndent = 0;
		bool flagUpdate = false;

		EditorGUI.indentLevel = levelIndent + 1;

		/* Original Data */
		Script_SpriteStudio6_Root instanceRoot = PropertyRootOriginal.objectReferenceValue as Script_SpriteStudio6_Root;
		Script_SpriteStudio6_RootEffect instanceRootEffect = PropertyRootEffectOriginal.objectReferenceValue as Script_SpriteStudio6_RootEffect;
		Script_SpriteStudio6_Sequence instanceSequence = PropertySequenceOriginal.objectReferenceValue as Script_SpriteStudio6_Sequence;

		UnityEngine.GameObject gameObjectNow = null;
		if(null != instanceRoot)
		{
			gameObjectNow = instanceRoot.gameObject;
		}
		else if(null != instanceRootEffect)
		{
			gameObjectNow = instanceRootEffect.gameObject;
		}
		else if(null != instanceSequence)
		{
			gameObjectNow = instanceSequence.gameObject;
		}

		EditorGUILayout.Space();
		UnityEngine.GameObject gameObjectNew = EditorGUILayout.ObjectField("Original Animation", gameObjectNow, typeof(UnityEngine.GameObject), true) as UnityEngine.GameObject;
		EditorGUILayout.LabelField("GameObject that has following components");
		EditorGUILayout.LabelField("  can be set to \"Original Animation\".");
		EditorGUILayout.LabelField("- Script_SpriteStudio6_Root");
		EditorGUILayout.LabelField("- Script_SpriteStudio6_RootEffect");
		EditorGUILayout.LabelField("- Script_SpriteStudio6_Sequence");
		EditorGUILayout.Space();

		if(gameObjectNow != gameObjectNew)
		{
			Script_SpriteStudio6_Root instanceRootNew = null;
			Script_SpriteStudio6_RootEffect instanceRootEffectNew = null;
			Script_SpriteStudio6_Sequence instanceSequenceNew = null;
			if(null == gameObjectNew)
			{
				InstanceReplicate.Stop();

//				instanceRootNew = null;
//				instanceRootEffectNew = null;
//				instanceSequenceNew = null;
			}
			else
			{
				instanceRootNew = gameObjectNew.GetComponent<Script_SpriteStudio6_Root>();
				instanceRootEffectNew = gameObjectNew.GetComponent<Script_SpriteStudio6_RootEffect>();
				instanceSequenceNew = gameObjectNew.GetComponent<Script_SpriteStudio6_Sequence>();

				if(null != instanceRootNew)
				{
					InstanceReplicate.OriginalSet(instanceRootNew);
//					instanceRootNew = null;
					instanceRootEffectNew = null;
					instanceSequenceNew = null;
				}
				else if(null != instanceRootEffectNew)
				{
					InstanceReplicate.OriginalSet(instanceRootEffectNew);
					instanceRootNew = null;
//					instanceRootEffectNew = null;
					instanceSequenceNew = null;
				}
				else if(null != instanceSequenceNew)
				{
					InstanceReplicate.OriginalSet(instanceSequenceNew);
					instanceRootNew = null;
					instanceRootEffectNew = null;
//					instanceSequenceNew = null;
				}
//				else
//				{
//					PropertyRootOriginal.objectReferenceValue = null;
//					PropertyRootEffectOriginal.objectReferenceValue = null;
//					PropertySequenceOriginal.objectReferenceValue = null;
//				}
			}

			instanceRoot = instanceRootNew;
			instanceRootEffect = instanceRootEffectNew;
			instanceSequence = instanceSequenceNew;

			flagUpdate |= true;
		}

		/* Hide */
		PropertyHideForce.boolValue = EditorGUILayout.Toggle("Hide Force", PropertyHideForce.boolValue);
//		EditorGUILayout.Space();

		EditorGUI.indentLevel = levelIndent;

		if(true == flagUpdate)
		{
			PropertyRootOriginal.objectReferenceValue = instanceRoot;
			PropertyRootEffectOriginal.objectReferenceValue = instanceRootEffect;
			PropertySequenceOriginal.objectReferenceValue = instanceSequence;
		}

		serializedObject.ApplyModifiedProperties();
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	private const string NameMissing = "(Data Missing)";
	#endregion Enums & Constants
}