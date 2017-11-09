/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
#if false
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Script_SpriteStudio6_Collider))]
public class Inspector_SpriteStudio6_Collider : Editor
{
	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField("[SpriteStudio CellMap-Data]");
		Script_SpriteStudio6_Collider Data = (Script_SpriteStudio6_Collider)target;
	}
}
#endif