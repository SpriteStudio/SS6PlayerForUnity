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
public class Script_Sample_Benchmark_SS6PlayerPlay : Script_Sample_Benchmark_SwitchSSPlayer.ControlSSPlayerBase
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
	// Instantiate animation object and start animation (SSPlayer mode)
	bool animationStart(int index)
	{
		bool retVal = false;
		GameObject go = null;
		GameObject goRoot = null;
		Script_SpriteStudio6_Root scriptRoot = null;	// Instance of the class for operating SpriteStudio6 animation-object

		// Get resource name and instantiate and start animation
		string resourceName = GetResourceName(index);
		if(string.IsNullOrEmpty(resourceName) == false)
		{
			// Instantiate animation object
			goRoot = Instantiate(Resources.Load(resourceName), Vector3.zero, Quaternion.identity) as GameObject;
			if(goRoot != null)
			{
				scriptRoot = Script_SpriteStudio6_Root.Parts.RootGet(goRoot);
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
					go.name = "SS6Anim_" + index.ToString("D3");	// Give a unique name

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

					// Set sorting-order so that parts' drawing order does not change by Unity's dynamic-batching
					//  when same depth(Z-position) as other animation objects.
					// In this case, even if Z-coordinate is slightly shifted,
					//  the same effect can be obtained.
					//  eg: go.transform.z + = (float)index; * 0.00001f;
					scriptRoot.OrderInLayer = index;

					// Add to the list of animation objects being played
					mAddAnimation(go);

					// Start animation
					scriptRoot.AnimationPlay(-1, 0, 0);

					// All Complete
					retVal = true;
				}
			}
		}

		return retVal;
	}
}

