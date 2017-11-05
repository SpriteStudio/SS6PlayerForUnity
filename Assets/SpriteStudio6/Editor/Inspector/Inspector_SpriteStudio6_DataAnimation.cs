/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
#if false
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Script_SpriteStudio6_DataAnimation))]
public class Inspector_SpriteStudio6_DataAnimation : Editor
{
	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField("[SpriteStudio Animation-Data]");
		Script_SpriteStudio6_DataAnimation Data = (Script_SpriteStudio6_DataAnimation)target;
	}
}
#endif