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

[CustomEditor(typeof(Script_SpriteStudio6_Root))]
public class Inspector_SpriteStudio6_Root : Editor
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	/* WorkArea (for Inspector) */
	private Script_SpriteStudio6_Root InstanceRoot;

	private SerializedProperty PropertyDataCellMap;
	private SerializedProperty PropertyDataAnimation;
	private SerializedProperty PropertyHolderAsset;

	private SerializedProperty PropertyHideForce;
	private SerializedProperty PropertyColliderInterlockHideForce;
	private SerializedProperty PropertyFlagPlanarization;
	private SerializedProperty PropertyOrderInLayer;
	private SerializedProperty PropertyCountTrack;
	private SerializedProperty PropertyInformationPlay;

	private string[] TableNameAnimation = null;

#if SUPPORT_PREVIEW
	/* WorkArea (for Preview) */
	private LibraryEditor_SpriteStudio6.Utility.Inspector.Preview InstancePreview;
	private Script_SpriteStudio6_Root InstanceRootPreview = null;
	private float TimeElapsedPreview = float.NaN;
	private float RateScalePreview = 1.0f;

	private bool FlagFoldOutInterfaces = false;
	private bool FlagPlayAnimationPreview = false;

	private string[] TableNameAnimationPreview = null;
#endif
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	private void CleanUp()
	{
		InstanceRoot = null;

//		PropertyDataCellMap
//		PropertyDataAnimation
//		PropertyHolderAsset

//		PropertyHideForce
//		PropertyColliderInterlockHideForce
//		PropertyFlagPlanarization
//		PropertyOrderInLayer
//		PropertyCountTrack
//		PropertyInformationPlay

		TableNameAnimation = null;

#if SUPPORT_PREVIEW
		InstancePreview = null;
		InstanceRootPreview = null;
		TimeElapsedPreview = float.NaN;
		RateScalePreview = 1.0f;

		FlagFoldOutInterfaces = false;
		FlagPlayAnimationPreview = false;

		TableNameAnimationPreview = null;
#endif
	}

	private void OnEnable()
	{
		InstanceRoot = (Script_SpriteStudio6_Root)target;

		serializedObject.FindProperty("__DUMMY__");
		PropertyDataCellMap = serializedObject.FindProperty("DataCellMap");
		PropertyDataAnimation = serializedObject.FindProperty("DataAnimation");
		PropertyHolderAsset = serializedObject.FindProperty("HolderAsset");

		PropertyHideForce = serializedObject.FindProperty("FlagHideForce");
		PropertyColliderInterlockHideForce = serializedObject.FindProperty("FlagColliderInterlockHideForce");
		PropertyFlagPlanarization = serializedObject.FindProperty("FlagPlanarization");
		PropertyOrderInLayer = serializedObject.FindProperty("OrderInLayer");
		PropertyCountTrack = serializedObject.FindProperty("LimitTrack");
		PropertyInformationPlay = serializedObject.FindProperty("TableInformationPlay");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.LabelField("[SpriteStudio6 Animation]");
		int levelIndent = 0;
		bool flagUpdate = false;
		bool flagUpdateLimitTrack = false;

		/* Static Datas */
		EditorGUILayout.Space();
		PropertyDataAnimation.isExpanded = EditorGUILayout.Foldout(PropertyDataAnimation.isExpanded, "Static Datas");
		if(true == PropertyDataAnimation.isExpanded)
		{
			EditorGUI.indentLevel = levelIndent + 1;

			PropertyDataCellMap.objectReferenceValue = (Script_SpriteStudio6_DataCellMap)(EditorGUILayout.ObjectField("Data:CellMap", PropertyDataCellMap.objectReferenceValue, typeof(Script_SpriteStudio6_DataCellMap), true));
			PropertyDataAnimation.objectReferenceValue = (Script_SpriteStudio6_DataAnimation)(EditorGUILayout.ObjectField("Data:Animation", PropertyDataAnimation.objectReferenceValue, typeof(Script_SpriteStudio6_DataAnimation), true));
			PropertyHolderAsset.objectReferenceValue = (Script_SpriteStudio6_HolderAsset)(EditorGUILayout.ObjectField("Holder:Asset", PropertyHolderAsset.objectReferenceValue, typeof(Script_SpriteStudio6_HolderAsset), true));
			EditorGUI.indentLevel = levelIndent;
		}

		/* Animation */
		Script_SpriteStudio6_DataAnimation dataAnimation = InstanceRoot.DataAnimation;
//		Script_SpriteStudio6_DataCellMap dataCellMap = InstanceRoot.DataCellMap;

		EditorGUILayout.Space();
		PropertyInformationPlay.isExpanded = EditorGUILayout.Foldout(PropertyInformationPlay.isExpanded, "Initial/Preview Play Setting");
		if(true == PropertyInformationPlay.isExpanded)
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

			/* Track */
			int countTrack = EditorGUILayout.IntField("Number of Track", PropertyCountTrack.intValue);
			EditorGUILayout.LabelField("(0: Default)");
			if(0 >= countTrack)
			{
				countTrack = 0;
			}
			if(PropertyCountTrack.intValue != countTrack)
			{
				PropertyCountTrack.intValue = countTrack;
				flagUpdateLimitTrack |= true;
				flagUpdate |= true;
			}
			EditorGUILayout.Space();

			if(null == dataAnimation)
			{
				EditorGUILayout.LabelField("(Animation Data Missing)");
			}
			else
			{
				/* Creation animation name table */
				SerializedProperty propertyInformationPlay = PropertyInformationPlay.GetArrayElementAtIndex(0);
				TableGetNameAnimation(ref TableNameAnimation, dataAnimation);
				InformationPlay(ref flagUpdate, propertyInformationPlay, InstanceRoot, TableNameAnimation);
			}
		}

		serializedObject.ApplyModifiedProperties();

		/* Reset Track */
		if(true == flagUpdateLimitTrack)
		{
			InstanceRoot.TrackReboot(InstanceRoot.LimitTrack);
		}

		/* Reset Animation */
		if(true == flagUpdate)
		{
			InstanceRoot.AnimationStop(-1);
			InstanceRoot.AnimationPlayInitial();
		}
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

	private void InformationPlay(	ref bool flagUpdate,
									SerializedProperty propertyInformationPlay,
									Script_SpriteStudio6_Root instanceRoot,
									string[] tableNameAnimation
								)
	{
		SerializedProperty propertyFlagStopInitial = propertyInformationPlay.FindPropertyRelative("FlagStopInitial");
		SerializedProperty propertyNameAnition = propertyInformationPlay.FindPropertyRelative("NameAnimation");
		SerializedProperty propertyFlagPingPong = propertyInformationPlay.FindPropertyRelative("FlagPingPong");
		SerializedProperty propertyLabelStart = propertyInformationPlay.FindPropertyRelative("LabelStart");
		SerializedProperty propertyFrameOffsetStart= propertyInformationPlay.FindPropertyRelative("FrameOffsetStart");
		SerializedProperty propertyLabelEnd = propertyInformationPlay.FindPropertyRelative("LabelEnd");
		SerializedProperty propertyFrameOffsetEnd = propertyInformationPlay.FindPropertyRelative("FrameOffsetEnd");
		SerializedProperty propertyFrame = propertyInformationPlay.FindPropertyRelative("Frame");
		SerializedProperty propertyTimesPlay = propertyInformationPlay.FindPropertyRelative("TimesPlay");
		SerializedProperty propertyRateTime = propertyInformationPlay.FindPropertyRelative("RateTime");

		/* Set "Initial Stop" */
		propertyFlagStopInitial.boolValue = EditorGUILayout.Toggle("Initial Stop", propertyFlagStopInitial.boolValue);
		EditorGUILayout.Space();

		/* "Animation" Select */
		string nameAnimation = propertyNameAnition.stringValue;
		int indexAnimation = TableSelectNameAnimation(ref flagUpdate, ref nameAnimation, instanceRoot, tableNameAnimation, "Animation Name");
		propertyNameAnition.stringValue = nameAnimation;

		Library_SpriteStudio6.Data.Animation dataAnimation = instanceRoot.DataAnimation.TableAnimation[indexAnimation];
//		int frameAnimationEnd = dataAnimation.CountFrame - 1;
		int frameValidStart = dataAnimation.FrameValidStart;
		int frameValidEnd = dataAnimation.FrameValidEnd;
		int countFrameValid = dataAnimation.CountFrameValid;
		EditorGUILayout.LabelField("- Valid Frame: " + frameValidStart.ToString() + " - " + frameValidEnd.ToString() + "  (Count: " + countFrameValid.ToString() + ")");

		/* Create "Label" table */
		string[] tableNameLabel = null;
		int[] tableIndexLabel = null;
		int[] tableFrameLabel = null;

		int countLabel = dataAnimation.CountGetLabel();
		if(0 >= countLabel)
		{	/* Has no labels */
			countLabel = 2;

			tableNameLabel = new string[countLabel];
			tableIndexLabel = new int[countLabel];
			tableFrameLabel = new int[countLabel];
		}
		else
		{	/* Has labels */
			countLabel += 2;	/* +2 ... "_start" and "_end" (Reserved-Labels) */

			tableNameLabel = new string[countLabel];
			tableIndexLabel = new int[countLabel];
			tableFrameLabel = new int[countLabel];

			int indexTable;
			for(int i=0; i<(countLabel - 2); i++)
			{
				indexTable = i + 1;
				tableNameLabel[indexTable] = dataAnimation.TableLabel[i].Name;
				tableIndexLabel[indexTable] = i;
				tableFrameLabel[indexTable] = dataAnimation.TableLabel[i].Frame;
			}
		}

		tableNameLabel[0] = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START];
		tableIndexLabel[0] = (int)(Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START | Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.INDEX_RESERVED);
		tableFrameLabel[0] = frameValidStart;

		tableNameLabel[countLabel - 1] = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END];
		tableIndexLabel[countLabel - 1] = (int)(Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END | Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.INDEX_RESERVED);
		tableFrameLabel[countLabel - 1] = frameValidEnd;

		/* Get current labels */
		string nameLabelStart = propertyLabelStart.stringValue;
		string nameLabelEnd = propertyLabelEnd.stringValue;
		int indexTableLabelStart = -1;
		int indexTableLabelEnd = -1;
		for(int i=0; i<countLabel; i++)
		{
			if(tableNameLabel[i] == nameLabelStart)
			{
				indexTableLabelStart = i;
				break;
			}
		}
		for(int i=0; i<countLabel; i++)
		{
			if(tableNameLabel[i] == nameLabelEnd)
			{
				indexTableLabelEnd = i;
				break;
			}
		}
		if(0 > indexTableLabelStart)
		{
			indexTableLabelStart = 0;
		}
		if(0 > indexTableLabelEnd)
		{
			indexTableLabelEnd = countLabel - 1;
		}

		int offsetStart = propertyFrameOffsetStart.intValue;
		int offsetEnd = propertyFrameOffsetEnd.intValue;
		int frameLabelStart = tableFrameLabel[indexTableLabelStart];
		int frameLabelEnd = tableFrameLabel[indexTableLabelEnd];
		int indexTableLabelNew;
		int frameLimit;

		/* Range "Start" */
		EditorGUILayout.Space();
		EditorGUILayout.LabelField(	"Range Start: (" + frameLabelStart.ToString()
									+ " + "
									+ offsetStart.ToString()
									+ ") = "
									+ (frameLabelStart + offsetStart).ToString()
								);

		/* Start-Label Select */
		indexTableLabelNew = EditorGUILayout.Popup("Range Start Label", indexTableLabelStart, tableNameLabel);
		if(indexTableLabelNew != indexTableLabelStart)
		{
			indexTableLabelStart = indexTableLabelNew;
			propertyLabelStart.stringValue = tableNameLabel[indexTableLabelStart];
			flagUpdate |= true;
		}
		frameLabelStart = tableFrameLabel[indexTableLabelStart];

		/* Start-Offset */
		frameLimit = frameLabelEnd + offsetEnd;
		int offsetStartNew = EditorGUILayout.IntField("Range Start Offset", offsetStart);
		EditorGUILayout.LabelField(	"- Valid Value Range: Min[" + (frameValidStart - frameLabelStart).ToString() +
									"] to Max[" + ((frameLimit - frameLabelStart) - 1).ToString() + "] "	/* -1 ... End frame */
								);

		offsetStartNew = (frameLimit <= (frameLabelStart + offsetStartNew)) ? ((frameLimit - frameLabelStart) - 1) : offsetStartNew;
		offsetStartNew = (frameValidStart > (frameLabelStart + offsetStartNew)) ? (frameValidStart - frameLabelStart) : offsetStartNew;
		offsetStartNew = (frameValidEnd < (frameLabelStart + offsetStartNew)) ? (frameValidEnd - frameLabelStart) : offsetStartNew;
		if(offsetStartNew != offsetStart)
		{
			offsetStart = offsetStartNew;
			propertyFrameOffsetStart.intValue = offsetStartNew;
			flagUpdate |= true;
		}

		/* Range "End" */
		EditorGUILayout.Space();
		EditorGUILayout.LabelField(	"Range End: (" + frameLabelEnd.ToString()
									+ " + "
									+ offsetEnd.ToString()
									+ ") = "
									+ (frameLabelEnd + offsetEnd).ToString()
								);

		/* End-Label Select */
		indexTableLabelNew = EditorGUILayout.Popup("Range End Label", indexTableLabelEnd, tableNameLabel);
		if(indexTableLabelNew != indexTableLabelEnd)
		{	/* Data is valid & Changed Animation */
			indexTableLabelEnd = indexTableLabelNew;
			propertyLabelEnd.stringValue = tableNameLabel[indexTableLabelEnd];
			flagUpdate |= true;
		}
		frameLabelEnd = tableFrameLabel[indexTableLabelEnd];

		/* End-Offset */
		frameLimit = frameLabelStart + offsetStart;
		int offsetEndNew = EditorGUILayout.IntField("Range End Offset", offsetEnd);
		EditorGUILayout.LabelField(	"- Valid Value Range: Min[" + ((frameLimit - frameLabelEnd) + 1).ToString() +	/* +1 ... Start frame */
									"] to Max[" + (frameValidEnd - frameLabelEnd).ToString() + "] "
								);

		offsetEndNew = (frameLimit >= (frameLabelEnd + offsetEndNew)) ? ((frameLimit - frameLabelEnd) + 1) : offsetEndNew;
		offsetEndNew = (frameValidStart > (frameLabelEnd + offsetEndNew)) ? (frameValidStart - frameLabelEnd) : offsetEndNew;
		offsetEndNew = (frameValidEnd < (frameLabelEnd + offsetEndNew)) ? (frameValidEnd - frameLabelEnd) : offsetEndNew;
		if(offsetEndNew != offsetEnd)
		{
			offsetEnd = offsetEndNew;
			propertyFrameOffsetEnd.intValue = offsetEndNew;
			flagUpdate |= true;
		}

		/* Play Pingpong */
		EditorGUILayout.Space();
		bool flagPingPong = propertyFlagPingPong.boolValue;
		bool flagPingPongNew = EditorGUILayout.Toggle("Play-Pingpong", flagPingPong);
		if(flagPingPongNew != flagPingPong)
		{
			propertyFlagPingPong.boolValue = flagPingPongNew;
			flagUpdate |= true;
		}

		/* Rate-Time */
		EditorGUILayout.Space();
		float rateTime = propertyRateTime.floatValue;
		if(0.0f == rateTime)
		{
			rateTime = 1.0f;
		}
		float rateTimeNew = EditorGUILayout.FloatField("Rate Time-Progress", rateTime);
		EditorGUILayout.LabelField("(set Negative-Value, Play Backwards.)");
		if(rateTimeNew != rateTime)
		{
			propertyRateTime.floatValue = rateTimeNew;
			flagUpdate |= true;
		}

		/* Play-Times */
		EditorGUILayout.Space();
		int timesPlay = propertyTimesPlay.intValue;
		int timesPlayNew = EditorGUILayout.IntField("Number of Plays", timesPlay);
		EditorGUILayout.LabelField("(1: No Loop / 0: Infinite Loop)");
		if(timesPlayNew != timesPlay)
		{
			propertyTimesPlay.intValue = timesPlayNew;
			flagUpdate |= true;
		}

		/* Reset */
		EditorGUILayout.Space();
		if(true == GUILayout.Button("Reset (Reinitialize)"))
		{
			propertyNameAnition.stringValue = tableNameAnimation[0];
			propertyFrame.intValue = 0;
			propertyRateTime.floatValue = 1.0f;
			propertyTimesPlay.intValue = 0;
			propertyFlagPingPong.boolValue = false;
			propertyLabelStart.stringValue = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.START];
			propertyFrameOffsetStart.intValue = 0;
			propertyLabelEnd.stringValue = Library_SpriteStudio6.Data.Animation.Label.TableNameLabelReserved[(int)Library_SpriteStudio6.Data.Animation.Label.KindLabelReserved.END];
			propertyFrameOffsetEnd.intValue = 0;
			propertyFlagStopInitial.boolValue = false;
			flagUpdate = true;	/* Force */
		}
	}

	private bool TableGetNameAnimation(ref string[] tableNameAnimation, Script_SpriteStudio6_DataAnimation dataAnimation)
	{
		int countAnimation = dataAnimation.CountGetAnimation();
		if((null == tableNameAnimation) || (countAnimation != tableNameAnimation.Length))
		{
			tableNameAnimation = new string[countAnimation];
		}

		for(int i=0; i<countAnimation; i++)
		{
			tableNameAnimation[i] = dataAnimation.TableAnimation[i].Name;
			if(true == string.IsNullOrEmpty(tableNameAnimation[i]))
			{
				tableNameAnimation[i] = NameMissing;
			}
		}

		return(true);
	}
	private int TableSelectNameAnimation(	ref bool flagUpdate,
											ref string nameAnimation,
											Script_SpriteStudio6_Root instanceRoot,
											string[] tableNameAnimation,
											string labelUI,
											int width=0
									)
	{
		/* "Animation" Select */
		int indexAnimation = instanceRoot.IndexGetAnimation(nameAnimation);
		if((0 > indexAnimation) || (tableNameAnimation.Length <= indexAnimation))
		{
			indexAnimation = 0;
			flagUpdate |= true;
		}

		int indexNow = -1;
		if(false == string.IsNullOrEmpty(labelUI))
		{
			if(0 >= width)
			{
				indexNow = EditorGUILayout.Popup(labelUI, indexAnimation, tableNameAnimation);
			}
			else
			{
				indexNow = EditorGUILayout.Popup(labelUI, indexAnimation, tableNameAnimation, GUILayout.Width(width));
			}
		}
		else
		{
			if(0 >= width)
			{
				indexNow = EditorGUILayout.Popup(indexAnimation, tableNameAnimation);
			}
			else
			{
				indexNow = EditorGUILayout.Popup(indexAnimation, tableNameAnimation, GUILayout.Width(width));
			}
		}

		if(indexNow != indexAnimation)
		{
			indexAnimation = indexNow;
			nameAnimation = instanceRoot.DataAnimation.TableAnimation[indexAnimation].Name;
			flagUpdate |= true;
		}

		return(indexAnimation);
	}
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
				const int indexTrackAnimation = 0;	/* force */

				/* "Play" Button */
				FlagPlayAnimationPreview = UnityEngine.GUILayout.Toggle(	FlagPlayAnimationPreview,
																			(true == FlagPlayAnimationPreview) ? EditorGUIUtility.IconContent("preAudioPlayOn") : EditorGUIUtility.IconContent("preAudioPlayOff"),
																			(UnityEngine.GUIStyle)"preButton"
																	);

				/* "Animation" Select */
				int indexAnimation = -1;
				{
					const int widthList = 100;

					bool flagUpdate = false;
					Script_SpriteStudio6_DataAnimation dataAnimation = InstanceRootPreview.DataAnimation;
					string nameAnimation = InstanceRootPreview.TableInformationPlay[0].NameAnimation;
					TableGetNameAnimation(ref TableNameAnimationPreview, dataAnimation);
					indexAnimation = TableSelectNameAnimation(ref flagUpdate, ref nameAnimation, InstanceRootPreview, TableNameAnimationPreview, null, widthList);
					if(true == flagUpdate)
					{
						/* Set Animation */
						InstanceRootPreview.AnimationPlay(-1, indexAnimation, 0);
					}

					InstanceRootPreview.AnimationPause(-1, !FlagPlayAnimationPreview);
				}

				/* "Frame" Textbox */
				{
					const int widthBox = 60;
					int frameAnimation = InstanceRootPreview.TableControlTrack[indexTrackAnimation].ArgumentContainer.Frame;

					if(false == FlagPlayAnimationPreview)
					{	/* Stopping */
						/* MEMO: Can specify Frame-No. */
						int frameAnimationNew = EditorGUILayout.DelayedIntField(frameAnimation, GUILayout.Width(widthBox));
						if(frameAnimation != frameAnimationNew)
						{
							int frameLimit = InstanceRootPreview.TableControlTrack[indexTrackAnimation].FrameStart;
							if(frameLimit > frameAnimationNew)
							{
								frameAnimationNew = frameLimit;
							}
							frameLimit = InstanceRootPreview.TableControlTrack[indexTrackAnimation].FrameEnd;
							if(frameLimit < frameAnimationNew)
							{
								frameAnimationNew = frameLimit;
							}

#if false
							/* MEMO: When animation is started by specifying frame directly, start frame may be set to -1 */
							/*         , e.g., for 1/60 second data.                                                      */
							/*       (Due to "rounding-off" error).                                                       */
							InstanceRootPreview.AnimationPlay(-1, indexAnimation, -1, frameAnimationNew);
#else
							const float adjustTimeMargin = 0.0001f;	/* 1/1000[sec] */
							float timeElapsed =	(	frameAnimationNew
													* InstanceRootPreview.TableControlTrack[indexTrackAnimation].TimePerFrame
												) + adjustTimeMargin;
							InstanceRootPreview.TableControlTrack[indexTrackAnimation].TimeSkip(timeElapsed, false, false);
#endif
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

			InstanceRootPreview = InstancePreview.GameObjectAnimation.GetComponent<Script_SpriteStudio6_Root>();
			if(null == InstanceRootPreview)
			{
				return;
			}

			/* Initialize Animation object */
			InstancePreview.ObjectBootUpAnimation(InstanceRootPreview.gameObject);

			/* Set Initial Animation */
			InstanceRootPreview.AnimationPlay(-1, 0);	/* force */

			/* Set CallBack-s */
			InstanceRootPreview.FunctionTimeElapse = FunctionTimeElapseAnimation;
			InstanceRootPreview.FunctionTimeElapseEffect = FunctionTimeElapseEffect;
			InstanceRootPreview.FunctionUnifyChildTimeElapse();

			/* Set Status */
			/* MEMO: Keep animation from running automatically. (2 updates run: scene lifecycle and manual) */
			InstanceRootPreview.FlagPlanarization = true;
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

	private float FunctionTimeElapseAnimation(Script_SpriteStudio6_Root scriptRoot)
	{
		if(false == FlagPlayAnimationPreview)
		{
			return(0.0f);
		}
		return(TimeElapsedPreview);
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
	private readonly static string NameMissing = "(Data Missing)";

#if SUPPORT_PREVIEW
	private readonly static GUIContent TitlePreview = new GUIContent("Preview [Script_SpriteStudio6_Root]");
#endif
	#endregion Enums & Constants
}
