/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

#define SUPPORT_PREVIEW

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
	private SerializedProperty PropertyHolderAsset;

	private SerializedProperty PropertyHideForce;
	private SerializedProperty PropertyLimitParticle;

#if SUPPORT_PREVIEW
	/* WorkArea (for Preview) */
	private LibraryEditor_SpriteStudio6.Utility.Inspector.Preview InstancePreview;
	private Script_SpriteStudio6_RootEffect InstanceRootPreview = null;
	private float TimeElapsedPreview = float.NaN;
	private float RateScalePreview = 1.0f;

	private bool FlagFoldOutInterfaces = false;
	private bool FlagPlayAnimationPreview = false;
#endif
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	private void CleanUp()
	{
		InstanceRoot = null;

//		PropertyDataCellMap
//		PropertyDataEffect
//		PropertyHolderAsset
//
//		PropertyHideForce
//		PropertyLimitParticle

#if SUPPORT_PREVIEW
		InstancePreview = null;
		InstanceRootPreview = null;
		TimeElapsedPreview = float.NaN;
		RateScalePreview = 1.0f;

		FlagFoldOutInterfaces = false;
		FlagPlayAnimationPreview = false;
#endif
	}

	private void OnEnable()
	{
		CleanUp();

		InstanceRoot = (Script_SpriteStudio6_RootEffect)target;

		serializedObject.FindProperty("__DUMMY__");
		PropertyDataCellMap = serializedObject.FindProperty("DataCellMap");
		PropertyDataEffect = serializedObject.FindProperty("DataEffect");
		PropertyHolderAsset = serializedObject.FindProperty("HolderAsset");

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
			PropertyHolderAsset.objectReferenceValue = (Script_SpriteStudio6_HolderAsset)(EditorGUILayout.ObjectField("Holder:Asset", PropertyHolderAsset.objectReferenceValue, typeof(Script_SpriteStudio6_HolderAsset), true));
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

#if SUPPORT_PREVIEW
	private void OnDisable()
	{
		if(null != InstancePreview)
		{
			InstancePreview.Dispose();
		}

		CleanUp();
	}
#endif
	#endregion Functions

#if SUPPORT_PREVIEW
	/* ----------------------------------------------- Functions-forPreview */
	#region Functions-forPreview
	public override bool HasPreviewGUI()
	{
		return(true);
	}

	public override GUIContent GetPreviewTitle()
	{
		return(TitlePreview);
	}

	public override bool RequiresConstantRepaint()
	{
		/* MEMO: Update frequently only during playing preview animation. */
		return(FlagPlayAnimationPreview);
	}

	public override void OnPreviewSettings()
	{
		if(null == InstanceRootPreview)
		{
			return;
		}
		if(false == InstanceRootPreview.StatusIsValid)
		{
			return;
		}

		/* "Fold-out" Buttoons */
		FlagFoldOutInterfaces = UnityEngine.GUILayout.Toggle(	FlagFoldOutInterfaces,
																(true == FlagFoldOutInterfaces) ? EditorGUIUtility.IconContent("ArrowNavigationLeft") : EditorGUIUtility.IconContent("ArrowNavigationRight"),
																(UnityEngine.GUIStyle)"preButton"
														);
		if(false == FlagFoldOutInterfaces)
		{	/* Show Interfaces */
			if(null != InstanceRootPreview)
			{
				/* "Play" Button */
				FlagPlayAnimationPreview = UnityEngine.GUILayout.Toggle(	FlagPlayAnimationPreview,
																			(true == FlagPlayAnimationPreview) ? EditorGUIUtility.IconContent("preAudioPlayOn") : EditorGUIUtility.IconContent("preAudioPlayOff"),
																			(UnityEngine.GUIStyle)"preButton"
																	);

				Script_SpriteStudio6_DataEffect dataEffect = InstanceRootPreview.DataEffect;
				if(null == dataEffect)
				{
					return;
				}

				/* "Frame" Textbox */
				{
					const int widthBox = 60;
					int frameAnimation = (int)InstanceRootPreview.Frame;

					if(false == FlagPlayAnimationPreview)
					{	/* Stopping */
						/* MEMO: Can specify Frame-No. */
						int frameAnimationNew = EditorGUILayout.DelayedIntField(frameAnimation, GUILayout.Width(widthBox));
						if(frameAnimation != frameAnimationNew)
						{
							int frameLimit = 0;
							if(frameLimit > frameAnimationNew)
							{
								frameAnimationNew = frameLimit;
							}
							frameLimit = (int)InstanceRootPreview.FrameRange - 1;
							if(frameLimit < frameAnimationNew)
							{
								frameAnimationNew = frameLimit;
							}

							const float adjustTimeMargin = 0.0001f;	/* 1/1000[sec] */
							float timeElapsed =	(	frameAnimationNew
													* InstanceRootPreview.TimePerFrame
												) + adjustTimeMargin;
							InstanceRootPreview.TimeSkip(timeElapsed, false);
						}
					}
					else
					{	/* Playing */
						/* MEMO: Just display Frame-No. */
						EditorGUILayout.LabelField(frameAnimation.ToString(), GUILayout.Width(widthBox));
					}
				}

				/* Rate Select */
				if(null != InstanceRootPreview)
				{
					const int widthList = 60;

					float rateScalePreview = RateScalePreview;
					RateScalePreview = InstancePreview.RateSelectScale(rateScalePreview, widthList);
					if((0.0f < RateScalePreview) && (RateScalePreview != rateScalePreview))
					{
						InstanceRootPreview.transform.localScale = new Vector3(RateScalePreview, RateScalePreview, RateScalePreview);
					}
				}
			}
		}
	}

	public override void OnPreviewGUI(Rect rect, GUIStyle background)
	{
		if(null == InstanceRoot)
		{
			return;
		}

		/* Check booted-up */
		if(null == InstancePreview)
		{	/* Preview-Scene */
			InstancePreview = new LibraryEditor_SpriteStudio6.Utility.Inspector.Preview();
		}
		if(null == InstanceRootPreview)
		{	/* Animation */
			if(null == InstancePreview.GameObjectAnimation)
			{
				InstancePreview.Create(InstanceRoot.gameObject);
			}

			InstanceRootPreview = InstancePreview.GameObjectAnimation.GetComponent<Script_SpriteStudio6_RootEffect>();
			if(null == InstanceRootPreview)
			{
				return;
			}

			/* Initialize Animation object */
			InstancePreview.ObjectBootUpAnimation(InstanceRootPreview.gameObject);

			/* Set Initial Animation */
			InstanceRootPreview.AnimationPlay();

			/* Set CallBack-s */
			InstanceRootPreview.FunctionTimeElapse = FunctionTimeElapseEffect;

			/* Set CallBack-s & Status */
			/* MEMO: Keep animation from running automatically. (2 updates run: scene lifecycle and manual) */
			InstanceRootPreview.StatusIsControlledPreview = true;
			InstanceRootPreview.enabled = false;
		}

		/* Update Scene & Objects */
		InstancePreview.Update();
		TimeElapsedPreview = InstancePreview.TimeElapsed;

		InstanceRootPreview.LateUpdatePreview();

		/* Render Preview-Scene */
		InstancePreview.Render();

		/* Update Preview-Window */
		{
			base.OnPreviewGUI(rect, background);

			UnityEngine.Texture textureTarget = InstancePreview.TextureTarget;
			if(null != textureTarget)
			{
				GUI.DrawTexture(rect, textureTarget, ScaleMode.ScaleToFit);
			}
		}
	}

	private float FunctionTimeElapseEffect(Script_SpriteStudio6_RootEffect scriptRoot)
	{
		if(false == FlagPlayAnimationPreview)
		{
			return(0.0f);
		}
		return(TimeElapsedPreview);
	}
	#endregion Functions-forPreview
#endif

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
#if SUPPORT_PREVIEW
	private readonly static GUIContent TitlePreview = new GUIContent("Preview [Script_SpriteStudio6_RootEffect]");
#endif
	#endregion Enums & Constants
}
