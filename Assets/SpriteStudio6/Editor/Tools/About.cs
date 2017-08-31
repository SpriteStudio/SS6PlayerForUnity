/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using UnityEngine;
using UnityEditor;

public sealed class MenuItem_SpriteStudio6_About : EditorWindow
{
	/* ----------------------------------------------- Functions */
	#region Functions
	[MenuItem("Tools/SpriteStudio6/About")]
	static void About()
	{
		string versionText = "1.0.0";
		EditorUtility.DisplayDialog(	LibraryEditor_SpriteStudio6.NameAsset,
										LibraryEditor_SpriteStudio6.NameAsset + "\n\n"
										+ "Version: " + versionText
										+ "\n\n"
										+ "Copyright(C) " + LibraryEditor_SpriteStudio6.NameDistributor,
										"OK"
									);
	}
	#endregion Functions
}
