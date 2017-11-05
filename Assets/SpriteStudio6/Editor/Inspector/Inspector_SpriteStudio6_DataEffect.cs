/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
#if false
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Script_SpriteStudio6_DataEffect))]
public class Inspector_SpriteStudio6_DataEffect : Editor
{
	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField("[SpriteStudio Effect-Data]");
		Script_SpriteStudio6_DataEffect Data = (Script_SpriteStudio6_DataEffect)target;
	}
}
#endif