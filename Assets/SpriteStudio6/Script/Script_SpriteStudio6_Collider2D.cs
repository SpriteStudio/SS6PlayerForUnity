/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public partial class Script_SpriteStudio6_Collider2D : Library_SpriteStudio6.Script.Collider
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	private Collider2D InstanceCollider = null;

	public CircleCollider2D InstanceColliderCircle;
	public BoxCollider2D InstanceColliderBox;
	#endregion Variables & Properties

	/* ----------------------------------------------- MonoBehaviour-Functions */
	#region MonoBehaviour-Functions
//	void Awake()
//	{
//	}

	void Start()
	{
		if(null != InstanceColliderBox)
		{
			SizeRectangle = InstanceColliderBox.size;
			PivotRectangle = InstanceColliderBox.offset;
		}
		if(null != InstanceColliderCircle)
		{
			Radius = InstanceColliderCircle.radius;
		}
	}

//	void Update()
//	{
//	}

// 	void LateUpdate()
//	{
//	}

	void OnTriggerEnter2D(Collider2D pair)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderEnter))
		{
			BootUp();

			InformationEnter.Pair2D = pair;
			InformationEnter.Contact2D = null;

			InstanceRoot.FunctionColliderEnter(InstanceRoot, InstanceGamaObject, InstanceRoot.DataAnimation.TableParts[IDParts].Name, IDParts, InformationEnter);
		}
	}

	void OnTriggerExit2D(Collider2D pair)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderExit))
		{
			BootUp();

			InformationExit.Pair2D = pair;
			InformationExit.Contact2D = null;

			InstanceRoot.FunctionColliderExit(InstanceRoot, InstanceGamaObject, InstanceRoot.DataAnimation.TableParts[IDParts].Name, IDParts, InformationExit);
		}
	}

	void OnTriggerStay2D(Collider2D pair)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderStay))
		{
			BootUp();

			InformationStay.Pair2D = pair;
			InformationStay.Contact2D = null;

			InstanceRoot.FunctionColliderStay(InstanceRoot, InstanceGamaObject, InstanceRoot.DataAnimation.TableParts[IDParts].Name, IDParts, InformationEnter);
		}
	}

	void OnCollisionEnter2D(Collision2D contacts)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderEnter))
		{
			BootUp();

			InformationEnter.Pair2D = null;
			InformationEnter.Contact2D = contacts;

			InstanceRoot.FunctionColliderEnter(InstanceRoot, InstanceGamaObject, InstanceRoot.DataAnimation.TableParts[IDParts].Name, IDParts, InformationEnter);
		}
	}

	void OnCollisionExit2D(Collision2D contacts)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderExit))
		{
			BootUp();

			InformationExit.Pair2D = null;
			InformationExit.Contact2D = contacts;

			InstanceRoot.FunctionColliderExit(InstanceRoot, InstanceGamaObject, InstanceRoot.DataAnimation.TableParts[IDParts].Name, IDParts, InformationExit);
		}
	}

	void OnCollisionStay2D(Collision2D contacts)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderStay))
		{
			BootUp();

			InformationStay.Pair2D = null;
			InformationStay.Contact2D = contacts;

			InstanceRoot.FunctionColliderStay(InstanceRoot, InstanceGamaObject, InstanceRoot.DataAnimation.TableParts[IDParts].Name, IDParts, InformationStay);
		}
	}
	#endregion MonoBehaviour-Functions

	/* ----------------------------------------------- Functions */
	#region Functions
	protected override void BootUp()
	{
		if(null == InstanceGamaObject)
		{
			InstanceGamaObject = gameObject;
		}

		if(null == InstanceCollider)
		{
			if(null != InstanceColliderBox)
			{
				InstanceCollider = InstanceColliderBox;
				goto BootUp_ColliderConfirmed;
			}

			if(null != InstanceColliderCircle)
			{
				InstanceCollider = InstanceColliderCircle;
				goto BootUp_ColliderConfirmed;
			}
		}

		return;

	BootUp_ColliderConfirmed:;
		InformationEnter.BootUp(InstanceCollider);
		InformationExit.BootUp(InstanceCollider);
		InformationStay.BootUp(InstanceCollider);
//		return;
	}

	internal override bool ColliderSetEnable(bool flagSwitch)
	{
		BootUp();
		if(null == InstanceCollider)
		{
			return(false);
		}

		if(InstanceCollider.enabled != flagSwitch)
		{
			InstanceCollider.enabled = flagSwitch;
		}

		return(true);
	}

	internal override  bool ColliderSetRectangle(ref Vector3 size, ref Vector3 pivot)
	{
		BootUp();
		if(null == InstanceColliderBox)
		{
			return(false);
		}

		/* MEMO: Deformation of collider takes time, so update as little as possible. */
		if(SizeRectangle != size)
		{
			InstanceColliderBox.size = size;
			SizeRectangle = size;
		}
		if(PivotRectangle != pivot)
		{
			InstanceColliderBox.offset = pivot;
			PivotRectangle = pivot;
		}

		return(true);
	}

	internal override  bool ColliderSetRadius(float radius)
	{
		BootUp();
		if(null == InstanceColliderCircle)
		{
			return(false);
		}

		/* MEMO: Deformation of collider takes time, so update as little as possible. */
		if(Radius != radius)
		{
			InstanceColliderCircle.radius = radius;
			Radius = radius;
		}

		return(true);
	}
	#endregion Functions
}
