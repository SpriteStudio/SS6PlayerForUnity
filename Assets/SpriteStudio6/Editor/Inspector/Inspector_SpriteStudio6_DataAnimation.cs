/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
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