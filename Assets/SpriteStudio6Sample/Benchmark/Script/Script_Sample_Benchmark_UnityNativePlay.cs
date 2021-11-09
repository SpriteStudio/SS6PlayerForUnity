/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public partial class Script_Sample_Benchmark_UnityNativePlay : Script_Sample_Benchmark_SwitchSSPlayer.ControlSSPlayerBase
{
	/* ----------------------------------------------- Notes */
	#region Notes
	/* This source list is the source involveed to "Script_Sample_Benchmark_SwitchSSPlayer.cs". */
	/* (Part of the sample "Benchmark Switch SSPlayer")                                         */
	/*                                                                                          */
	/* For basic notes, see the "Script_Sample_Benchmark_SwitchSSPlayer.cs".                    */
	#endregion Notes

	// --------------------------------------------------------------------------------------------------------------------------
	// Start and Update are not installed since there is no separate processing.
	//  void Start()
	//  {
	//  }
	// 
	//  void Update()
	//  {
	//  }

	// --------------------------------------------------------------------------------------------------------------------------
	// override functions
	public override bool AddAnimation()
	{
		int count = GetAnimationCount();
		return (animationStart(count));
	}

	public override bool DelAnimation()
	{
		return mDelAnimationLast();
	}

	// --------------------------------------------------------------------------------------------------------------------------
	// Instantiate animation object and start animation (Unity-Native mode)
	bool animationStart(int index)
	{
		bool retVal = false;
		GameObject go = null;
		GameObject goRoot = null;
		Script_SpriteStudio6_RootUnityNative scriptRoot = null;	// Instance of the class for operating SpriteStudio6 animation-object

		// Get resource name and instantiate and start animation
		string resourceName = GetResourceName(index);
		if(string.IsNullOrEmpty(resourceName) == false)
		{
			// Instantiate animation object
			goRoot = Instantiate(Resources.Load(resourceName), Vector3.zero, Quaternion.identity) as GameObject;
			if(goRoot != null)
			{
				scriptRoot = goRoot.GetComponent<Script_SpriteStudio6_RootUnityNative>();
				if(scriptRoot != null)
				{
					// Create GameObject to control Transform of animation object
					go = new GameObject();
					if(go == null)
					{
						Destroy(goRoot);
						goRoot = null;
						return false;
					}
					go.name = "SS6UNAnim_" + index.ToString("D3");	// Give a unique name

					goRoot.transform.parent = go.transform;

					// Get animation object's position and scaling
					Vector3 position;
					Vector3 scale;
					base.GetPositionScale(out position, out scale, GetAnimationCount());

					// Make animation object child of "go", and set position and scaling to "go"
					go.transform.parent = this.transform;
					go.transform.localPosition = position;
					go.transform.localRotation = Quaternion.identity;
					go.transform.localScale = scale;

					// Add to the list of animation objects being played
					mAddAnimation(go);

					// All Complete
					retVal = true;
				}
			}
		}

		return retVal;
	}
}
