/**
	SpriteStudio6 Player for Unity

	Copyright(C) 1997-2021 Web Technology Corp.
	Copyright(C) CRI Middleware Co., Ltd.
	All rights reserved.
*/

using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public partial class Script_SpriteStudio6_Collider : Library_SpriteStudio6.Script.Collider
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	private Collider InstanceCollider = null;

	public CapsuleCollider InstanceColliderCapsule;
	public BoxCollider InstanceColliderBox;
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
			PivotRectangle = InstanceColliderBox.center;
		}
		if(null != InstanceColliderCapsule)
		{
			Radius = InstanceColliderCapsule.radius;
		}
	}

//	void Update()
//	{
//	}

// 	void LateUpdate()
//	{
//	}

	void OnTriggerEnter(Collider pair)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderEnter))
		{
			BootUp();

			InformationEnter.Pair = pair;
			InformationEnter.Contact = null;

			InstanceRoot.FunctionColliderEnter(InstanceRoot, InstanceGamaObject, InstanceRoot.DataAnimation.TableParts[IDParts].Name, IDParts, InformationEnter);
		}
	}

	void OnTriggerExit(Collider pair)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderExit))
		{
			BootUp();

			InformationExit.Pair = pair;
			InformationExit.Contact = null;

			InstanceRoot.FunctionColliderExit(InstanceRoot, InstanceGamaObject, InstanceRoot.DataAnimation.TableParts[IDParts].Name, IDParts, InformationExit);
		}
	}

	void OnTriggerStay(Collider pair)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderStay))
		{
			BootUp();

			InformationStay.Pair = pair;
			InformationStay.Contact = null;

			InstanceRoot.FunctionColliderStay(InstanceRoot, InstanceGamaObject, InstanceRoot.DataAnimation.TableParts[IDParts].Name, IDParts, InformationStay);
		}
	}

	void OnCollisionEnter(Collision contacts)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderEnter))
		{
			BootUp();

			InformationEnter.Pair = null;
			InformationEnter.Contact = contacts;

			InstanceRoot.FunctionColliderEnter(InstanceRoot, InstanceGamaObject, InstanceRoot.DataAnimation.TableParts[IDParts].Name, IDParts, InformationEnter);
		}
	}

	void OnCollisionExit(Collision contacts)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderExit))
		{
			BootUp();

			InformationExit.Pair = null;
			InformationExit.Contact = contacts;

			InstanceRoot.FunctionColliderExit(InstanceRoot, InstanceGamaObject, InstanceRoot.DataAnimation.TableParts[IDParts].Name, IDParts, InformationExit);
		}
	}

	void OnCollisionStay(Collision contacts)
	{
		if((null != InstanceRoot) && (null != InstanceRoot.FunctionColliderStay))
		{
			BootUp();

			InformationStay.Pair = null;
			InformationStay.Contact = contacts;

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

			if(null != InstanceColliderCapsule)
			{
				InstanceCollider = InstanceColliderCapsule;
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

	internal override bool ColliderSetRectangle(ref Vector3 size, ref Vector3 pivot)
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
			InstanceColliderBox.center = pivot;
			PivotRectangle = pivot;
		}

		return(true);
	}

	internal override bool ColliderSetRadius(float radius)
	{
		BootUp();
		if(null == InstanceColliderCapsule)
		{
			return(false);
		}

		/* MEMO: Deformation of collider takes time, so update as little as possible. */
		if(Radius != radius)
		{
			InstanceColliderCapsule.radius = radius;
			Radius = radius;
		}

		return(true);
	}
	#endregion Functions
}
