/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
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

	public GameObject GameObjectActiveSS6P;		// for activating run of "SS6Player" mode.
	public GameObject GameObjectActiveSS6PUN;	// for activating run of "Unity-Natime" mode.

	// Frame-Rate Setting
	public enum FrameRateKind
	{
		FPS_60 = 0,
		FPS_30,
	}
	public FrameRateKind FrameRate;

	// Setting items (Inspector)
	public int AnimationLineCount;		// Number of animation objects in a row
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
	private bool m_SkipAddTime;
	private float m_FrameTime;

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
		int frameRate = 60;
		switch(FrameRate)
		{
			case FrameRateKind.FPS_60:
				frameRate = 60;
				break;

			case FrameRateKind.FPS_30:
				frameRate = 30;
				break;

			default:
				break;
		}
		Application.targetFrameRate = frameRate;
		m_FrameTime = 1.0f / (float)frameRate;

		// Initialize measurement history
		m_TimeHistory.Initialize(frameRate / 2);	// 0.5 sec. per measurement
		m_SkipAddTime = true;

		// Initialize playing mode
		m_playerNo = PlayerNoKind.SS6P;
		SwitchSSPlayer();
	}

	// --------------------------------------------------------------------------------------------------------------------------
	// 
	void Update()
	{
		// Get elapsed time
		bool addAnimation = false;
		if(m_SkipAddTime == false)
			addAnimation = m_TimeHistory.AddTime(Time.deltaTime);
		m_SkipAddTime = false;

		// Change playing mode
		if(Input.GetKeyDown(KeyCode.Space) == true)
		{
			// Clear Animation objects
			m_ControlSSP.ClearAnimation();	// When you comment out this line, you can switch playing modes while leaving animation objects.

			// Switch GameObject for running of "SS6Player" and "Unity-Native"
			m_playerNo = (PlayerNoKind)(((int)m_playerNo + 1) % (int)PlayerNoKind.MAX);
			SwitchSSPlayer();

			// Clear measurement history
			m_TimeHistory.Clear();
		}

		// Measurement per time range
		if(addAnimation == true)
		{
			if(m_TimeHistory.CheckTime(m_FrameTime) == true)
				m_ControlSSP.AddAnimation();	// in frame time (increase by 1 animation-object)
			else
				m_ControlSSP.DelAnimation();	// out of frame time (decreases by 1 animation-object)

			// 計測ワークを再初期化
			m_TimeHistory.Clear();

			// Since there is overhead for instantiate and destroy,
			//  frames with increased or decreased (animation-object) are not included in the measurement.
			m_SkipAddTime = true;
		}
	}

	// --------------------------------------------------------------------------------------------------------------------------
	// 
	void OnGUI()
	{
		const string textSS6P = "SS6Player for Unity";
		const string textSS6PUN = "SS6Player for Unity (UnityNative)";

		// Now Playing-Mode
		switch(m_playerNo)
		{
			case PlayerNoKind.SS6P:
				GUI.Label(new Rect(20, 20, 300, 50), textSS6P);
				break;

			case PlayerNoKind.SS6P_UN:
				GUI.Label(new Rect(20, 20, 300, 50), textSS6PUN);
				break;

			default:
				break;
		}

		// Frame-Rate
		float time = m_TimeHistory.GetTime();
		float fps = 0.0f;
		if(time > 0.0f)
			fps = 1.0f / time;
		GUI.Label(new Rect(500, 20, 300, 50), "FPS: " + fps.ToString("F2"));

		// Number of Animation-Object
		if(m_ControlSSP != null)
		{
			GUI.Label(new Rect(300, 20, 300, 50), "Count: " + (m_ControlSSP.GetAnimationCount()).ToString());
		}
	}

	// --------------------------------------------------------------------------------------------------------------------------
	// Switch Playing-Mode
	private void SwitchSSPlayer()
	{
		switch(m_playerNo)
		{
			// SS6Player - Active
			case PlayerNoKind.SS6P:
				GameObjectActiveSS6P.SetActive(true);
				GameObjectActiveSS6PUN.SetActive(false);
				m_ControlSSP = m_SS6P;
				break;
			// Unity Native - Active
			case PlayerNoKind.SS6P_UN:
				GameObjectActiveSS6P.SetActive(false);
				GameObjectActiveSS6PUN.SetActive(true);
				m_ControlSSP = m_SS6PUN;
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
		private float[] m_Time;
		private int m_Index;
		private int m_IndexMax;
		private const float m_TimeRag = 0.0003f;	// margin of about 0.2ms in 1 frame

		public bool Initialize(int indexMax)
		{
			m_IndexMax = indexMax;

			if((m_Time == null) || (m_Time.Length != m_IndexMax))
			{
				m_Time = new float[m_IndexMax];
				if(m_Time == null)
				{
					return false;
				}
			}
			Clear();

			return true;
		}

		// Clear History
		public void Clear()
		{
			for(int i = 0; i < m_IndexMax; i++)
			{
				m_Time[i] = -1.0f;
			}
			m_Index = 0;
		}

		// Add to History
		public bool AddTime(float time)
		{
			m_Time[m_Index] = time;
			++m_Index;
			m_Index %= m_IndexMax;

			// Returns true if the history is full
			return (m_Index == 0);
		}

		// Get time based on history
		public float GetTime()
		{
			if(m_Index <= 0)
				return 0.0f;

			float sum = 0.0f;

			for(int i = 0; i < m_Index; i++)
			{
				sum += m_Time[i];
			}
			return (sum / m_Index);
		}

		// Scan the history and measure whether display is over within the specified time
		public bool CheckTime(float time)
		{
			int count = 0;
			float timeLimit = time + m_TimeRag;

			// Count frames within the time
			for(int i = 0; i < m_IndexMax; i++)
			{
				if(m_Time[i] <= timeLimit)
					++count;
			}

			// Since the time per frame shakes, do not judge by average.
			// Judge to pass when frames within the time exceeds the majority.
			return (count > (m_IndexMax / 2));
		}
	}

	// --------------------------------------------------------------------------------------------------------------------------
	// Base-Class of Animation-object control
	public class ControlSSPlayerBase : MonoBehaviour
	{
		// Subpath in "Resources" of animation-prefab
		[SerializeField]
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

			position.x = m_SettingData.GetParam(AnimationSettingDataType.StartX) + (m_SettingData.GetParam(AnimationSettingDataType.AligmentX) * (float)indexX);
			position.y = m_SettingData.GetParam(AnimationSettingDataType.StartY) - (m_SettingData.GetParam(AnimationSettingDataType.AligmentY) * (float)indexY);
			position.z = 0.0f;

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