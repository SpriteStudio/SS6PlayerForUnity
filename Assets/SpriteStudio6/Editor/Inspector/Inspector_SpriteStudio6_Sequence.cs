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

[CustomEditor(typeof(Script_SpriteStudio6_Sequence))]
public class Inspector_SpriteStudio6_Sequence : Editor
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	/* WorkArea (for Inspector) */
	private Script_SpriteStudio6_Sequence InstanceSequence;

	private SerializedProperty PropertyDataProject;
	private SerializedProperty PropertyNameSequencePack;
	private SerializedProperty PropertyNameDataSequence;
	private SerializedProperty PropertyIndexStepInitial;

	private SerializedProperty PropertyHideForce;
	private SerializedProperty PropertyColliderInterlockHideForce;
	private SerializedProperty PropertyFlagPlanarization;
	private SerializedProperty PropertyOrderInLayer;

	private SerializedProperty PropertyStopInitial;

	private string[] TableNameSequencePack = null;
	private string[] TableNameSequenceData = null;

#if SUPPORT_PREVIEW
	/* WorkArea (for Preview) */
	private LibraryEditor_SpriteStudio6.Utility.Inspector.Preview InstancePreview;
	private Script_SpriteStudio6_Sequence InstanceSequencePreview = null;
	private float TimeElapsedPreview = float.NaN;
	private float RateScalePreview = 1.0f;

	private bool FlagFoldOutInterfaces = false;
	private bool FlagPlayAnimationPreview = false;
	private int FramePerSecondPreview = 30;

	private string[] TableNameSequencePackPreview = null;
	private string[] TableNameSequenceDataPreview = null;
#endif
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	private void CleanUp()
	{
		InstanceSequence = null;

//		PropertyDataProject
//		PropertyNameSequencePack
//		PropertyNameDataSequence
//		PropertyIndexStepInitial

//		PropertyHideForce
//		PropertyColliderInterlockHideForce
//		PropertyFlagPlanarization
//		PropertyOrderInLayer

//		PropertyStopInitial

		TableNameSequencePack = null;
		TableNameSequenceData = null;

#if SUPPORT_PREVIEW
		InstancePreview = null;
		InstanceSequencePreview = null;
		TimeElapsedPreview = float.NaN;
		RateScalePreview = 1.0f;

		FlagFoldOutInterfaces = false;
		FlagPlayAnimationPreview = false;
		FramePerSecondPreview = 30;

		TableNameSequencePackPreview = null;
		TableNameSequenceDataPreview = null;
#endif
	}

	private void OnEnable()
	{
		InstanceSequence = (Script_SpriteStudio6_Sequence)target;

		serializedObject.FindProperty("__DUMMY__");
		PropertyDataProject = serializedObject.FindProperty("DataProject");
		PropertyNameSequencePack = serializedObject.FindProperty("NameSequencePack");
		PropertyNameDataSequence = serializedObject.FindProperty("NameDataSequence");
		PropertyIndexStepInitial = serializedObject.FindProperty("IndexStepInitial");

		PropertyHideForce = serializedObject.FindProperty("FlagHideForce");
		PropertyColliderInterlockHideForce = serializedObject.FindProperty("FlagColliderInterlockHideForce");
		PropertyFlagPlanarization = serializedObject.FindProperty("FlagPlanarization");
		PropertyOrderInLayer = serializedObject.FindProperty("OrderInLayer");

		PropertyStopInitial = serializedObject.FindProperty("FlagStopInitial");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.LabelField("[SpriteStudio6 Sequence]");
		int levelIndent = 0;
		bool flagUpdate = false;

		/* Static Datas */
		EditorGUILayout.Space();
		PropertyDataProject.isExpanded = EditorGUILayout.Foldout(PropertyDataProject.isExpanded, "Static Datas");
		if(true == PropertyDataProject.isExpanded)
		{
			EditorGUI.indentLevel = levelIndent + 1;

			UnityEngine.Object dataProjectNew = EditorGUILayout.ObjectField("Data:Project", PropertyDataProject.objectReferenceValue, typeof(Script_SpriteStudio6_DataProject), true);
			if(PropertyDataProject.objectReferenceValue != dataProjectNew)
			{
				PropertyDataProject.objectReferenceValue = dataProjectNew as Script_SpriteStudio6_DataProject;
				flagUpdate |= true;
			}
			EditorGUI.indentLevel = levelIndent;
		}

		Script_SpriteStudio6_DataProject dataProject = InstanceSequence.DataProject;

		/* Play Information */
		EditorGUILayout.Space();
		PropertyNameSequencePack.isExpanded = EditorGUILayout.Foldout(PropertyNameSequencePack.isExpanded, "Initial/Preview Play Setting");
		if(true == PropertyNameSequencePack.isExpanded)
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

			/* Sequence */
			if(null == dataProject)
			{
				EditorGUILayout.LabelField("(Project Data Missing)");
				PropertyNameSequencePack.stringValue = null;	/* InstanceSequence.PackSetNoUse(); */
				PropertyNameDataSequence.stringValue = null;
				PropertyIndexStepInitial.intValue = 0;
			}
			else
			{
				/* Creation Sequence name table */
				InformationPlay(ref flagUpdate, dataProject, InstanceSequence);
			}
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

	private void InformationPlay(	ref bool flagUpdate,
									Script_SpriteStudio6_DataProject dataProject,
									Script_SpriteStudio6_Sequence instanceSequence
								)
	{
		/* "Sequence-Pack" Select */
		Script_SpriteStudio6_DataSequence dataSequencePack = null;
		string nameSequencePack = PropertyNameSequencePack.stringValue;
		int indexSequencePack;
		TableGetSequencePack(ref TableNameSequencePack, out indexSequencePack, dataProject, nameSequencePack);
		indexSequencePack = TableSelectNameSequencePack(ref flagUpdate, ref nameSequencePack, dataProject, TableNameSequencePack, "Sequence-Pack Name");
		if(0 > indexSequencePack)
		{
			dataSequencePack = null;
//			PropertyNameSequencePack.stringValue = 
			flagUpdate = false;
		}
		else
		{
			dataSequencePack = dataProject.DataSequence[indexSequencePack];
			PropertyNameSequencePack.stringValue = nameSequencePack;
		}

		/* "Sequence" Select */
		string nameSequenceData = PropertyNameDataSequence.stringValue;
		int indexSequenceData;
		TableGetSequenceData(ref TableNameSequenceData, out indexSequenceData, dataSequencePack, nameSequenceData);
		indexSequenceData = TableSelectNameSequence(ref flagUpdate, ref nameSequenceData, dataSequencePack, TableNameSequenceData, "Sequence-Data Name");
		if(0 > indexSequenceData)
		{
//			PropertyNameDataSequence.stringValue = 
			flagUpdate = false;
		}
		else
		{
			PropertyNameDataSequence.stringValue = nameSequenceData;
		}

		/* Initial Step */
		int indexStepInitial = EditorGUILayout.IntField("Start Step", PropertyIndexStepInitial.intValue);
		if(PropertyIndexStepInitial.intValue != indexStepInitial)
		{
			flagUpdate |= true;

			int countStep = dataSequencePack.TableSequence[indexSequenceData].TableStep.Length;
			if(0 > indexStepInitial)
			{
				indexStepInitial = 0;
			}
			if(countStep <= indexStepInitial)
			{
				indexStepInitial = countStep - 1;
			}
		}

		/* Initial Stop */
		bool flagUpdateInitialStop = false;
		bool flagCheck = EditorGUILayout.Toggle("Initial Stop", PropertyStopInitial.boolValue);
		if(flagCheck != PropertyStopInitial.boolValue)
		{
			flagUpdateInitialStop |= true;
			PropertyStopInitial.boolValue = flagCheck;
		}

		/* Reset */
		EditorGUILayout.Space();
		if(true == GUILayout.Button("Reset (Reinitialize)"))
		{
			PropertyHideForce.boolValue = false;
			PropertyColliderInterlockHideForce.boolValue = false;
			PropertyFlagPlanarization.boolValue = false;

			indexSequencePack = 0;
			dataSequencePack = dataProject.DataSequence[indexSequencePack];
			nameSequencePack = dataSequencePack.Name;

			indexSequenceData = 0;
			nameSequenceData = dataSequencePack.TableSequence[indexSequenceData].Name;

			indexStepInitial = 0;

			flagUpdate = true;	/* Force */
		}

		/* Check Update */
		if(true == flagUpdate)
		{
			/* Update Properties */
			PropertyNameSequencePack.stringValue = nameSequencePack;
			PropertyNameDataSequence.stringValue = nameSequenceData;
			PropertyIndexStepInitial.intValue = indexStepInitial;

			/* Play Start */
			instanceSequence.Stop(false, false);
			if(true == instanceSequence.PackSet(indexSequencePack))
			{
				if(true == instanceSequence.SequenceSet(indexSequenceData))
				{
					instanceSequence.Play(indexStepInitial);
				}
			}
		}
		flagUpdate |= flagUpdateInitialStop;
	}

	private bool TableGetSequencePack(	ref string[] tableNameSequence,
										out int indexSequencePack,
										Script_SpriteStudio6_DataProject dataProject,
										string nameSequencePack
									)
	{
		int countSequence = dataProject.DataSequence.Length;	/* InstanceSequence.CountGetPack(); */
		bool flagNameIsValid = (false == string.IsNullOrEmpty(nameSequencePack));	/* ? true : false */

		if((null == tableNameSequence) || (countSequence != tableNameSequence.Length))
		{
			tableNameSequence = new string[countSequence];
		}

		indexSequencePack = 0;
		for(int i=0; i<countSequence; i++)
		{
			tableNameSequence[i] = dataProject.DataSequence[i].Name;
			if(true == string.IsNullOrEmpty(tableNameSequence[i]))
			{
				tableNameSequence[i] = NameMissing;
			}
			if((true == flagNameIsValid) && (tableNameSequence[i] == nameSequencePack))
			{
				indexSequencePack = i;
			}
		}

		return(true);
	}
	private bool TableGetSequenceData(	ref string[] tableNameSequenceData,
										out int indexSequenceData,
										Script_SpriteStudio6_DataSequence dataSequencePack,
										string nameSequenceData
									)
	{
		int countSequenceData = (null == dataSequencePack) ? 0 : dataSequencePack.TableSequence.Length;
		bool flagNameIsValid = (false == string.IsNullOrEmpty(nameSequenceData));	/* ? true : false */

		if((null == tableNameSequenceData) || (countSequenceData != tableNameSequenceData.Length))
		{
			tableNameSequenceData = new string[countSequenceData];
		}

		indexSequenceData = 0;
		for(int i=0; i<countSequenceData; i++)
		{
			tableNameSequenceData[i] = dataSequencePack.TableSequence[i].Name;
			if(true == string.IsNullOrEmpty(tableNameSequenceData[i]))
			{
				tableNameSequenceData[i] = NameMissing;
			}
			if((true == flagNameIsValid) && (tableNameSequenceData[i] == nameSequenceData))
			{
				indexSequenceData = i;
			}
		}

		return(true);
	}

	private int TableSelectNameSequencePack(	ref bool flagUpdate,
												ref string nameSequencePack,
												Script_SpriteStudio6_DataProject dataProject,
												string[] tableNameSequencePack,
												string labelUI,
												int width=0
											)
	{
		/* "Sequence-Pack" Select */
		if((null == tableNameSequencePack) || (0 >= tableNameSequencePack.Length))
		{
			goto TableSelectNameSequencePack_ErrorEnd;
		}

		int indexSequecePack = System.Array.IndexOf(tableNameSequencePack, nameSequencePack);
		if((0 > indexSequecePack) || (tableNameSequencePack.Length <= indexSequecePack))
		{
			indexSequecePack = 0;
			nameSequencePack = tableNameSequencePack[indexSequecePack];
			flagUpdate |= true;
		}

		int indexNow = -1;
		if(false == string.IsNullOrEmpty(labelUI))
		{
			if(0 >= width)
			{
				indexNow = EditorGUILayout.Popup(labelUI, indexSequecePack, tableNameSequencePack);
			}
			else
			{
				indexNow = EditorGUILayout.Popup(labelUI, indexSequecePack, tableNameSequencePack, GUILayout.Width(width));
			}
		}
		else
		{
			if(0 >= width)
			{
				indexNow = EditorGUILayout.Popup(indexSequecePack, tableNameSequencePack);
			}
			else
			{
				indexNow = EditorGUILayout.Popup(indexSequecePack, tableNameSequencePack, GUILayout.Width(width));
			}
		}

		if(indexNow != indexSequecePack)
		{
			indexSequecePack = indexNow;
			nameSequencePack = tableNameSequencePack[indexSequecePack];
			flagUpdate |= true;
		}

		return(indexSequecePack);

	TableSelectNameSequencePack_ErrorEnd:;
		if(false == string.IsNullOrEmpty(labelUI))
		{
			if(0 >= width)
			{
				EditorGUILayout.LabelField(labelUI, NameNoData);
			}
			else
			{
				EditorGUILayout.LabelField(labelUI, NameNoData, GUILayout.Width(width));
			}
		}
		else
		{
			if(0 >= width)
			{
				EditorGUILayout.LabelField(NameNoData);
			}
			else
			{
				EditorGUILayout.LabelField(NameNoData, GUILayout.Width(width));
			}
		}

		return(-1);
	}
	private int TableSelectNameSequence(	ref bool flagUpdate,
											ref string nameSequence,
											Script_SpriteStudio6_DataSequence dataSequence,
											string[] tableNameSequence,
											string labelUI,
											int width=0
										)
	{
		/* "Sequence" Select */
		if((null == dataSequence) || ((null == tableNameSequence) || (0 >= tableNameSequence.Length)))
		{
			goto TableSelectNameSequence_ErrorEnd;
		}

		int indexSequece = dataSequence.IndexGetSequence(nameSequence);
		if((0 > indexSequece) || (tableNameSequence.Length <= indexSequece))
		{
			indexSequece = 0;
			nameSequence = tableNameSequence[indexSequece];
			flagUpdate |= true;
		}

		int indexNow = -1;
		if(false == string.IsNullOrEmpty(labelUI))
		{
			if(0 >= width)
			{
				indexNow = EditorGUILayout.Popup(labelUI, indexSequece, tableNameSequence);
			}
			else
			{
				indexNow = EditorGUILayout.Popup(labelUI, indexSequece, tableNameSequence, GUILayout.Width(width));
			}
		}
		else
		{
			if(0 >= width)
			{
				indexNow = EditorGUILayout.Popup(indexSequece, tableNameSequence);
			}
			else
			{
				indexNow = EditorGUILayout.Popup(indexSequece, tableNameSequence, GUILayout.Width(width));
			}
		}

		if(indexNow != indexSequece)
		{
			indexSequece = indexNow;
//			nameSequence = dataSequence.TableSequence[indexSequece].Name;
			nameSequence = tableNameSequence[indexSequece];
			flagUpdate |= true;
		}

		return(indexSequece);

	TableSelectNameSequence_ErrorEnd:;
		if(false == string.IsNullOrEmpty(labelUI))
		{
			if(0 >= width)
			{
				EditorGUILayout.LabelField(labelUI, NameNoData);
			}
			else
			{
				EditorGUILayout.LabelField(labelUI, NameNoData, GUILayout.Width(width));
			}
		}
		else
		{
			if(0 >= width)
			{
				EditorGUILayout.LabelField(NameNoData);
			}
			else
			{
				EditorGUILayout.LabelField(NameNoData, GUILayout.Width(width));
			}
		}

		return(-1);
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
		if(null == InstanceSequencePreview)
		{
			return;
		}
		if(false == InstanceSequencePreview.StatusIsValid)
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
			if(null != InstanceSequencePreview)
			{
				Script_SpriteStudio6_DataProject dataProject = InstanceSequencePreview.DataProject;

				/* "Play" Button */
				FlagPlayAnimationPreview = UnityEngine.GUILayout.Toggle(	FlagPlayAnimationPreview,
																			(true == FlagPlayAnimationPreview) ? EditorGUIUtility.IconContent("preAudioPlayOn") : EditorGUIUtility.IconContent("preAudioPlayOff"),
																			(UnityEngine.GUIStyle)"preButton"
																	);

				bool flagRestartAnimation = false;
				Script_SpriteStudio6_DataSequence dataSequencePack = null;
				int indexSequencePack = -1;
				int indexSequenceData = -1;

				/* "Sequence-Pack" Select */
				if(null != dataProject)
				{
					const int widthList = 60;

					bool flagUpdate = false;
					string nameSequencePack = InstanceSequencePreview.NameSequencePack;
					TableGetSequencePack(ref TableNameSequencePackPreview, out indexSequencePack, dataProject, nameSequencePack);
					indexSequencePack = TableSelectNameSequencePack(ref flagUpdate, ref nameSequencePack, dataProject, TableNameSequencePackPreview, null, widthList);
					if(0 > indexSequencePack)
					{
//						InstanceSequencePreview.NameSequencePack = ;
						dataSequencePack = null;
					}
					else
					{
						InstanceSequencePreview.NameSequencePack = nameSequencePack;
						dataSequencePack = dataProject.DataSequence[indexSequencePack];

						if(true == flagUpdate)
						{
							InstanceSequencePreview.Stop(false, false);
							InstanceSequencePreview.PackSet(indexSequencePack);

							indexSequenceData = 0;
							flagRestartAnimation = true;
						}
					}
				}

				/* "Sequence" Select */
				if(null != dataSequencePack)
				{
					const int widthList = 60;

					string nameSequenceData = InstanceSequencePreview.NameDataSequence;
					if(0 <= indexSequenceData)
					{
						nameSequenceData = null;
					}

					bool flagUpdate = false;
					TableGetSequenceData(ref TableNameSequenceDataPreview, out indexSequenceData, dataSequencePack, nameSequenceData);
					indexSequenceData = TableSelectNameSequence(ref flagUpdate, ref nameSequenceData, dataSequencePack, TableNameSequenceDataPreview, null, widthList);
					if(0 > indexSequenceData)
					{
//						InstanceSequencePreview.NameDataSequence = 
						flagRestartAnimation = false;
					}
					else
					{
						InstanceSequencePreview.NameDataSequence = nameSequenceData;

						if(true == flagUpdate)
						{
							InstanceSequencePreview.Stop(false, false);

							InstanceSequencePreview.SequenceSet(indexSequenceData);
							flagRestartAnimation = true;
						}
					}
				}

				/* Restart Animation */
				if(true == InstanceSequencePreview.IsPlayable)
				{
					if(	(true == flagRestartAnimation)
						|| ((true == FlagPlayAnimationPreview) && (false == InstanceSequencePreview.StatusIsPlaying))
					)
					{
						InstanceSequencePreview.Play();
					}

					if(true == FlagPlayAnimationPreview)
					{
						InstanceSequencePreview.PauseSet(!FlagPlayAnimationPreview);
					}
				}

				/* "Frame" Textbox */
				{
					const int widthBox = 50;

					float timePerFrame = 1.0f / (float)FramePerSecondPreview;
					float timeSequence = InstanceSequencePreview.TimeTotalPreview;
					int frameSequence = (int)(timeSequence / timePerFrame);

					if(false == FlagPlayAnimationPreview)
					{	/* Stopping */
						int frameSequenceNew = EditorGUILayout.DelayedIntField(frameSequence, GUILayout.Width(widthBox));
						if(frameSequence != frameSequenceNew)
						{
							int frameLimit = 0;
							if(frameLimit > frameSequenceNew)
							{
								frameSequenceNew = frameLimit;
							}

							if(true == InstanceSequencePreview.IsPlayable)
							{
								float timeDuration = InstanceSequencePreview.TimeGetDurationSequence(null, true);
								frameLimit = (int)(timeDuration / timePerFrame);
								if(frameLimit < frameSequenceNew)
								{
									frameSequenceNew = frameLimit;
								}

								const float adjustTimeMargin = 0.0f;
								float timeElapsed =	(frameSequenceNew * timePerFrame) + adjustTimeMargin;
								InstanceSequencePreview.CursorSet(timeElapsed, true);
							}
						}
					}
					else
					{	/* Playing */
						/* MEMO: Just display Frame-No. */
						EditorGUILayout.LabelField(frameSequence.ToString(), GUILayout.Width(widthBox));
					}
				}

				/* FPS Select */
				{
					const int widthList = 60;

					FramePerSecondPreview = InstancePreview.FrameSelectFPS(FramePerSecondPreview, widthList);
				}

				/* Rate Select */
				if(null != InstanceSequencePreview)
				{
					const int widthList = 60;

					float rateScalePreview = RateScalePreview;
					RateScalePreview = InstancePreview.RateSelectScale(rateScalePreview, widthList);
					if((0.0f < RateScalePreview) && (RateScalePreview != rateScalePreview))
					{
						InstanceSequencePreview.transform.localScale = new Vector3(RateScalePreview, RateScalePreview, RateScalePreview);
					}
				}
			}
		}
	}

	public override void OnPreviewGUI(Rect rect, GUIStyle background)
	{
		if(null == InstanceSequence)
		{
			return;
		}

		/* Check booted-up */
		if(null == InstancePreview)
		{	/* Preview-Scene */
			InstancePreview = new LibraryEditor_SpriteStudio6.Utility.Inspector.Preview();
		}
		if(null == InstanceSequencePreview)
		{	/* Animation */
			if(null == InstancePreview.GameObjectAnimation)
			{
				InstancePreview.Create(InstanceSequence.gameObject);
			}

			InstanceSequencePreview = InstancePreview.GameObjectAnimation.GetComponent<Script_SpriteStudio6_Sequence>();
			if(null == InstanceSequencePreview)
			{
				return;
			}

			/* Initialize Animation object */
			InstancePreview.ObjectBootUpAnimation(InstanceSequencePreview.gameObject);

			/* Set Initial Animation */
			InstanceSequencePreview.Play(0, 1.0f);

			/* Set CallBack-s */
			InstanceSequencePreview.FunctionTimeElapse = FunctionTimeElapseSequence;

			/* Set Status */
			/* MEMO: Keep animation from running automatically. (2 updates run: scene lifecycle and manual) */
			InstanceSequencePreview.FlagPlanarization = true;
			InstanceSequencePreview.StatusIsControlledPreview = true;
			InstanceSequencePreview.enabled = false;

			InstanceSequencePreview.NameSequencePack = PropertyNameSequencePack.stringValue;
			InstanceSequencePreview.NameDataSequence = PropertyNameDataSequence.stringValue;
			InstanceSequencePreview.IndexStepInitial = PropertyIndexStepInitial.intValue;
			InstanceSequencePreview.FlagHideForce = false;
			InstanceSequencePreview.FlagStopInitial = true;
		}

		/* Update Scene & Objects */
		InstancePreview.Update();
		TimeElapsedPreview = InstancePreview.TimeElapsed;

		InstanceSequencePreview.UpdatePreview();

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

	public float FunctionTimeElapseSequence(Script_SpriteStudio6_Sequence scriptSequence)
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
	private readonly static string NameNoData = "(No Data)";

#if SUPPORT_PREVIEW
	private readonly static GUIContent TitlePreview = new GUIContent("Preview [Script_SpriteStudio6_Sequence]");
#endif
	#endregion Enums & Constants
}