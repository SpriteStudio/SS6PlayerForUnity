/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/
#define ADD_USERDATA_FUNCTION_UNITYNATIVE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Sample_Benchmark_SwitchSSPlayer : MonoBehaviour
{
	/* ----------------------------------------------- Notes */
	#region Notes
	/* -------------------------- Sample "Benchmark Switch SSPlayer" [Expert] */
	/* The points of this sample are as follows.                              */
	/*                                                                        */
	/* - Tips to simultaneously handling 2 modes "SS6P" and "Uniy-Native"     */
	/* - How to load Animation data from "Resources" folder                   */
	/* - How to receive user data callbacks in "Unity-Native" mode            */
	/* - Test-program for measuring the number of animation objects           */
	/*    that can be displayed per frame.                                    */
	#endregion Notes

	public UnityEngine.UI.Text UITextMode;
	public UnityEngine.UI.Text UITextCount;
	public UnityEngine.UI.Text UITextFPS;

	public GameObject GameObjectActiveSS6P;		// for activating run of "SS6Player" mode.
	public GameObject GameObjectActiveSS6PUN;	// for activating run of "Unity-Natime" mode.

	// Frame-Rate Setting
	public enum FrameRateKind
	{
		FPS_60 = 0,
		FPS_30,
	}
	public FrameRateKind FrameRate;
    public int StartAnimationCount;   // 開始時に一気に表示する数  

	// Steps to add Animation-Object
	private enum AddAnimationStep
	{
		Start = 0,
		Update,
		End
	}
	private AddAnimationStep m_AAStep = AddAnimationStep.Start;

	// Setting items (Inspector)
	public int AnimationLineCount;		// Number of animation objects in a row
	public int AnimationLineMaximum;	// Maximun number of columns
	public int AnimationStartX;			// Display Start Position X
	public int AnimationStartY;			//                        Y
	public int AnimationAlignmentX;		// Alignment of Position X
	public int AnimationAlignmentY;		//                       Y 
	public float AnimationScaleX;		// Animation object's Scaling X
	public float AnimationScaleY;		//                            Y

	private AnimationSettingData m_AnimetionSettingData;

	// Playing-Mode
	private enum PlayerNoKind
	{
		SS6P = 0,
		SS6P_UN,
		MAX,
	}
	private PlayerNoKind m_playerNo = PlayerNoKind.SS6P;	// Currently playing mode
	private Script_Sample_Benchmark_SS6PlayerPlay m_SS6P;
	private Script_Sample_Benchmark_UnityNativePlay m_SS6PUN;
	private ControlSSPlayerBase m_ControlSSP;

	// Frame rate measurement
	private TimeHistory m_TimeHistory;
	private int m_FrameRate;				// Frame-Rate
	private int m_SkipAddDelCount;			// Counter to add/del the number of Animation-Objects displayed

	// --------------------------------------------------------------------------------------------------------------------------
	// 
	void Start()
	{
		// Set Setting
		m_AnimetionSettingData.Initialize();
		m_AnimetionSettingData.SetParam(AnimationSettingDataType.LineCount, (float)AnimationLineCount);
		m_AnimetionSettingData.SetParam(AnimationSettingDataType.StartX, (float)AnimationStartX);
		m_AnimetionSettingData.SetParam(AnimationSettingDataType.StartY, (float)AnimationStartY);
		m_AnimetionSettingData.SetParam(AnimationSettingDataType.AligmentX, (float)AnimationAlignmentX);
		m_AnimetionSettingData.SetParam(AnimationSettingDataType.AligmentY, (float)AnimationAlignmentY);
		m_AnimetionSettingData.SetParam(AnimationSettingDataType.ScaleX, (float)AnimationScaleX);
		m_AnimetionSettingData.SetParam(AnimationSettingDataType.ScaleY, (float)AnimationScaleY);

		// Get Animation-Control Components
		// SS6Player for Unity
		if(GameObjectActiveSS6P != null)
		{
			m_SS6P = GameObjectActiveSS6P.GetComponent<Script_Sample_Benchmark_SS6PlayerPlay>();
			m_SS6P.Initialize(m_AnimetionSettingData);
		}
		else
		{
			m_SS6P = null;
		}
		// SS6Player for Unity (Unity Native)
		if(GameObjectActiveSS6PUN != null)
		{
			m_SS6PUN = GameObjectActiveSS6PUN.GetComponent<Script_Sample_Benchmark_UnityNativePlay>();
			m_SS6PUN.Initialize(m_AnimetionSettingData);
		}
		else
		{
			m_SS6PUN = null;
		}

		// Set Application's frame rate
		switch(FrameRate)
		{
			case FrameRateKind.FPS_60:
				m_FrameRate = 60;
				break;

			case FrameRateKind.FPS_30:
				m_FrameRate = 30;
				break;

			default:
				break;
		}
		Application.targetFrameRate = m_FrameRate;

		// Initialize measurement history
		m_TimeHistory.FPSInit();
		m_SkipAddDelCount = 0;

		// Initialize (Animation-Object) Add-Step 
		m_AAStep = AddAnimationStep.Start;

		// Initialize playing mode
		m_playerNo = PlayerNoKind.SS6P;
		SwitchSSPlayer();
	}

	// --------------------------------------------------------------------------------------------------------------------------
	// 
	void Update()
	{
		// Change playing mode
		if(Input.GetKeyDown(KeyCode.Space) == true)
		{
			// Clear Animation objects
			m_ControlSSP.ClearAnimation();	// When you comment out this line, you can switch playing modes while leaving animation objects.

			// Switch GameObject for running of "SS6Player" and "Unity-Native"
			m_playerNo = (PlayerNoKind)(((int)m_playerNo + 1) % (int)PlayerNoKind.MAX);
			SwitchSSPlayer();

			// Clear measurement history
			m_TimeHistory.FPSInit();

			// Initialize (Animation-Object) Add-Step 
			m_AAStep = AddAnimationStep.Start;
		}

		// Measurement per time range
		{
			m_TimeHistory.FrameCount();

			m_SkipAddDelCount--;
			if(m_SkipAddDelCount <= 0)
			{
				if(true == m_TimeHistory.IsFPSCheck(m_FrameRate))
				{
					switch(m_AAStep)
					{
						case AddAnimationStep.Start:
							for(int i = 0; i < StartAnimationCount; i++)
							{
								m_ControlSSP.AddAnimation();	// Number of animation objects, displayed first.
							}

							m_SkipAddDelCount = m_FrameRate * 3;	// Keep 3 seconds after initial display.
							m_AAStep = AddAnimationStep.Update;
							break;

						case AddAnimationStep.Update:
							m_ControlSSP.AddAnimation();	// Add 1 Animation-Object
							m_SkipAddDelCount = m_FrameRate;	// Keep 1 seconds
							break;

						default:
							break;
					}
				}
				else
				{
					float fps = (float)m_FrameRate - ((float)m_FrameRate / 20);

					// Remove animation-object when frame rate is deteriorating for more than 0.5 seconds.
					if(m_TimeHistory.GetFPS() < fps)
					{
						m_ControlSSP.DelAnimation();	// Remove 1 Animation-Object
						m_SkipAddDelCount = m_FrameRate;	// Keep 1 seconds
					}
				}
			}
		}

		// Display Status
		{
			// Frame-Rate
			float fps = m_TimeHistory.GetFPS();
			if(UITextFPS != null)
			{
				UITextFPS.text = "FPS: " + fps.ToString("F2");
			}

			// Number of Animation-Object
			if(UITextCount != null)
			{
				if(m_ControlSSP != null)
				{
					UITextCount.text = "Count: " + (m_ControlSSP.GetAnimationCount()).ToString();
				}
			}
		}
	}

	// --------------------------------------------------------------------------------------------------------------------------
	// Switch Playing-Mode
	private void SwitchSSPlayer()
	{
		const string textSS6P = "SS6Player for Unity";
		const string textSS6PUN = "SS6Player for Unity (UnityNative)";

		switch(m_playerNo)
		{
			// SS6Player - Active
			case PlayerNoKind.SS6P:
				GameObjectActiveSS6P.SetActive(true);
				GameObjectActiveSS6PUN.SetActive(false);
				m_ControlSSP = m_SS6P;

				if(UITextMode != null)
				{
					UITextMode.text = textSS6P;
				}
				break;
			// Unity Native - Active
			case PlayerNoKind.SS6P_UN:
				GameObjectActiveSS6P.SetActive(false);
				GameObjectActiveSS6PUN.SetActive(true);
				m_ControlSSP = m_SS6PUN;

				if(UITextMode != null)
				{
					UITextMode.text = textSS6PUN;
				}
				break;

			default:
				break;
		}
	}

	// --------------------------------------------------------------------------------------------------------------------------
	// Animation-Object management
	public enum AnimationSettingDataType
	{
		LineCount = 0,	// Number of animation objects in a row
		MaxLine,		// Maximun number of columns
		StartX,			// Display Start Position X
		StartY,			//                        Y
		AligmentX,		// Alignment of Position X
		AligmentY,		//                       Y 
		ScaleX,			// Animation object's Scaling X
		ScaleY,			//                            Y
		Count
	}

	public struct AnimationSettingData
	{
		private float[] m_param;

		// Initialize (Clean up)
		public bool Initialize()
		{
			if(m_param == null)
			{
				m_param = new float[(int)AnimationSettingDataType.Count];
				if(m_param == null)
				{
					return false;
				}
			}
			for(AnimationSettingDataType type = AnimationSettingDataType.LineCount; type < AnimationSettingDataType.Count; type++)
			{
				m_param[(int)type] = 0.0f;
			}

			return true;
		}

		// Initialize
		public bool Initialize(AnimationSettingData settingData)
		{
			if(m_param == null)
			{
				m_param = new float[(int)AnimationSettingDataType.Count];
				if(m_param == null)
				{
					return false;
				}
			}
			for(AnimationSettingDataType type = AnimationSettingDataType.LineCount; type < AnimationSettingDataType.Count; type++)
			{
				m_param[(int)type] = settingData.GetParam(type);
			}

			return true;
		}

		public void SetParam(AnimationSettingDataType type, float param)
		{
			if(m_param != null)
				m_param[(int)type] = param;
		}

		public float GetParam(AnimationSettingDataType type)
		{
			if(m_param == null)
				return 0.0f;
			return m_param[(int)type];
		}
	}

	// --------------------------------------------------------------------------------------------------------------------------
	// Measurement-History management
	public struct TimeHistory
	{
		private float m_updateInterval;
		private int m_frameCount;
		private float m_prevTime;
		private float m_fps;

		public void FPSInit()
		{
			m_updateInterval = 0.5f;	// 0.5 sec. per measurement
			m_frameCount = 0;
			m_prevTime = Time.realtimeSinceStartup;
			m_fps = 60.0f;
		}

		// FPS Count
		public void FrameCount()
		{
			++m_frameCount;
			float timeNow = Time.realtimeSinceStartup;

			if(timeNow > (m_updateInterval + m_prevTime)) {
				m_fps = ((float)m_frameCount / (timeNow - m_prevTime));
				m_frameCount = 0;
				m_prevTime = timeNow;
			}
		}

		// Check Frame-Count
		public bool IsFPSCheck(int iFrame)
		{
			bool retval = true;
			int fps = (int)m_fps + 1;
			if(iFrame > fps)
				retval = false;
			return retval;
		}

		// FPS取得  
		public float GetFPS()
		{
			return m_fps;
		}
	}

	// --------------------------------------------------------------------------------------------------------------------------
	// Base-Class of Animation-object control
	public class ControlSSPlayerBase : MonoBehaviour
	{
		// Subpath in "Resources" of animation-prefab
		public string[] FileNameList;

		// List of the currently displayed animation object
		protected List<GameObject> m_listAnimatonObject = null;

		// Setting for animation object
		private AnimationSettingData m_SettingData;

		// Initialize (Clean up)
		public bool Initialize(AnimationSettingData SettingData)
		{
			if(m_listAnimatonObject == null)
			{
				m_listAnimatonObject = new List<GameObject>();
				if(m_listAnimatonObject == null)
				{
					return false;
				}

				m_listAnimatonObject.Clear();
			}

			m_SettingData.Initialize(SettingData);

			return (ClearAnimation());
		}

		// Get number of animation objects
		public int GetAnimationCount()
		{
			if(m_listAnimatonObject == null)
			{
				return -1;
			}
			return m_listAnimatonObject.Count;
		}

		// Create animation object name corresponding to index
		public string GetResourceName(int index)
		{
			int listLength = FileNameList.Length;
			if(listLength < 0)
				return null;

			if(index < 0)
				index = 0;

			string resourceName = FileNameList[index % listLength];
			if(string.IsNullOrEmpty(resourceName) == true)
				return null;

			return resourceName;
		}

		// Delete all managed animation objects
		public virtual bool ClearAnimation()
		{
			if(m_listAnimatonObject == null)
			{
				return false;
			}

			while(mDelAnimationLast() == true) ;

			return true;
		}

		// Add 1 animation object (Implemented in derived classes)
		public virtual bool AddAnimation()
		{
			return false;
		}

		// Delete 1 animation object (Implemented in derived classes)
		public virtual bool DelAnimation()
		{
			return false;
		}

		// Add animation object's GameObject to the list
		protected bool mAddAnimation(GameObject go)
		{
			if((m_listAnimatonObject == null) || (go == null))
			{
				return false;
			}

			m_listAnimatonObject.Add(go);
			return true;
		}

		// Delete animation object's GameObject at end of the list
		protected bool mDelAnimationLast()
		{
			if(m_listAnimatonObject == null)
				return false;

			int length = m_listAnimatonObject.Count;
			if(length <= 0)
				return false;

			int index = length - 1;
			GameObject go = m_listAnimatonObject[index];
			if(go == null)
				return false;

			Destroy(go);
			m_listAnimatonObject.RemoveAt(index);
			return true;
		}

		// Calculate position and scale corresponding to index
		protected void GetPositionScale(out Vector3 position, out Vector3 scale, int index)
		{
			int indexX = index % (int)m_SettingData.GetParam(AnimationSettingDataType.LineCount);
			int indexY = index / (int)m_SettingData.GetParam(AnimationSettingDataType.LineCount);
			int maxLine = (int)m_SettingData.GetParam(AnimationSettingDataType.MaxLine);
			if(0 >= maxLine)
			{
				maxLine = 8;
			}
			int pages = indexY / maxLine;

			position.x = m_SettingData.GetParam(AnimationSettingDataType.StartX) + (m_SettingData.GetParam(AnimationSettingDataType.AligmentX) * (float)indexX);
			if(pages < 1)
			{
				position.y = m_SettingData.GetParam(AnimationSettingDataType.StartY) - (m_SettingData.GetParam(AnimationSettingDataType.AligmentY) * (float)indexY);
				position.z = (0.1f * 100.0f) - (0.1f * (float)indexY) - 0.001f * indexX;
			}
			else
			{
				indexY %= maxLine;
				float alignmentX = m_SettingData.GetParam(AnimationSettingDataType.AligmentX);
				float rateOffset = (float)(pages % 3) / 3.0f;
				float rateOffset2 = 0.0f;
				if(pages >= 3)	{
					rateOffset2 = alignmentX * 0.5f;
				}
				position.y = m_SettingData.GetParam(AnimationSettingDataType.StartY) - (m_SettingData.GetParam(AnimationSettingDataType.AligmentY) * (float)indexY);
				position.x += (alignmentX * rateOffset);
				position.x += rateOffset2;
				position.y -= (m_SettingData.GetParam(AnimationSettingDataType.AligmentY) * rateOffset);
				position.z = (0.1f * 100.0f) - (0.1f * (float)indexY) - 0.03f - 0.001f * indexX;
			}

			scale.x = m_SettingData.GetParam(AnimationSettingDataType.ScaleX);
			scale.y = m_SettingData.GetParam(AnimationSettingDataType.ScaleY);
			scale.z = 1.0f;
		}
	}
}

// --------------------------------------------------------------------------------------------------------------------------
// Partial for adding event callback function to class "Script_SpriteStudio6_RootUnityNative"
#if ADD_USERDATA_FUNCTION_UNITYNATIVE
public partial class Script_SpriteStudio6_RootUnityNative
{
	// Functions with a fixed name necessary to receive an animation event.
	// Mandatory for receiving "User Data" with "Unity-Native" mode imported data.
	public void FunctionEventInt(int value)
	{
	}

	public void FunctionEventText(string value)
	{
	}
}
#endif