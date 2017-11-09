/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Script_SpriteStudio6_Root
{
	/* ----------------------------------------------- Functions */
	#region Functions
	/* ******************************************************** */
	//! Get number of parts in animation
	/*!
	@param	
		(none)
	@retval	Return-Value
		Number of parts in animation
		-1 == Error / "Name" is not-found.

	Get number of parts in animation.<br>
	The range of Parts-ID is "0" to "ReturnValue-1".<br>
	*/
	public int CountGetParts()
	{
		if(null == DataAnimation)
		{
			return(-1);
		}

		return(DataAnimation.CountGetParts());
	}

	/* ******************************************************** */
	//! Get part's index(Parts-ID) from the part's-name
	/*!
	@param	Name
		Part's name
	@retval	Return-Value
		Parts-ID
		-1 == Error / "Name" is not-found.

	Get the part's-Index(Parts-ID) from the name.<br>
	The Index is the serial-number (0 origins) in the Animation-data.
	*/
	public int IDGetParts(string name)
	{
		if(null == DataAnimation)
		{
			return(-1);
		}

		return(DataAnimation.IndexGetParts(name));
	}

	/* ******************************************************** */
	//! Force-Hide Set
	/*!
	@param	idParts
		Parts-ID<br>
		0 == Hide the entire animation<br>
		-1 == Set Hide to all parts
	@param	flagSwitch
		true == Set Hide (Hide, force)<br>
		false == Hide Reset (Show. State of animation is followed.)
	@param	flagInvolveChildren
		true == Children are set same state.<br>
		false == only oneself.<br>
		default: false
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	State of "Hide" is set to parts, ignore with state of animation.<br>
	This setting is ignored when set to parts that does not have display capability such as "NULL"-parts.<br>
	Moreover, when set to "Mask"-parts, affect of mask can be erased.<br>
	This setting also affects the "Instance"-parts and "Effect"-parts, but not set to each (subordinate) animation objects.<br>
	<br>
	If set 0 or -1 to "idParts", hide entire animation.<br>
	However, behaviors differ clearly between 0 and -1.<br>
	Do not confuse both.<br>
	<br>
	idParts == 0:<br>
	Set hide state to whole animation.<br>
	(It is same behavior as checking or unchecking "Hide" on inspector)<br>
	Recommend that use this when normally set hide state of the whole animation.<br>
	This setting is separately from setting to each parts, and has priority.<br>
	<br>
	idParts == -1:<br>
	Set hide state to each all parts.<br>
	*/
	public bool HideSet(int idParts, bool flagSwitch, bool flagInvolveChildren=false)
	{
		if((null == DataAnimation) || (null == TableControlParts))
		{
			return(false);
		}

		int countParts = TableControlParts.Length;
		if(0 > idParts)
		{	/* All parts */
			for(int i=1; i<countParts; i++)
			{
				HideSetMain(i, flagSwitch, false);
			}
			return(true);
		}

		if(0 == idParts)
		{	/* "Root"-Parts */
			FlagHideForce = flagSwitch;
			return(true);
		}

		if(countParts <= idParts)
		{	/* Invalid ID */
			return(false);
		}

		HideSetMain(idParts, flagSwitch, flagInvolveChildren);

		return(true);
	}
	private void HideSetMain(int idParts, bool flagSwitch, bool flagInvolveChildren=false)
	{
		if(true == flagSwitch)
		{
			TableControlParts[idParts].Status |= Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.HIDE_FORCE;
		}
		else
		{
			TableControlParts[idParts].Status &= ~Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.HIDE_FORCE;
		}

		if(true == flagInvolveChildren)
		{
			int[] tableIDPartsChild = DataAnimation.TableParts[idParts].TableIDChild;
			int countPartsChild = tableIDPartsChild.Length;
			for(int i=0; i<countPartsChild; i++)
			{
				HideSetMain(tableIDPartsChild[i], flagSwitch, true);
			}
		}
	}

	/* ******************************************************** */
	//! Get instance of "Instance"
	/*!
	@param	idParts
		Parts-ID ("Instance"-part)
	@retval	Return-Value
		"Instance" animation's instance<br>
		null == Error / Invalid instance animation

	Get "Instance" controled by part.<br>
	<br>
	The reason why this function returns null is as follows.
	<br>
	- "idParts" is not "Instance" part<br>
	- "Instance"'s instance does not exist<br>
	- Before executing "Start()"<br>
	*/
	public Script_SpriteStudio6_Root InstanceGet(int idParts)
	{
		if((null == DataAnimation) || (null == TableControlParts))
		{
			return(null);
		}
		if((0 > idParts) || (TableControlParts.Length <= idParts))
		{
			return(null);
		}
		if(Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE != DataAnimation.TableParts[idParts].Feature)
		{
			return(null);
		}

		return(TableControlParts[idParts].InstanceRootUnderControl);
	}

	/* ******************************************************** */
	//! Change "Instance"
	/*!
	@param	idParts
		Parts-ID ("Instance"-part)
	@param	source
		Source of new "Instance" (mainly "Prefab")<br>
		null == Revert to initial data
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Set(Change) "Instance" controled by part.<br>
	<br>
	Be careful when using this function.<br>
	<br>
	This function is heavey load and consumes "Managed-Heap".<br>
	Because execute following process, basically.<br>
	<br>
	- Destroy old "Instance"<br>
	- Instantiate "source"(New "Instance")<br>
	- Rebuild parent-animation's drawing-meshes buffer<br>
	<br>
	Also, when set new "Instance", animation to be played is set animation of index 0.<br>
	(When revert, set animation-index that original-data has)<br>
	<br>
	Depending on animation data of the newly set "Instance",  may not playing-result as expected.<br>
	(Pay attention that how to be controlled from the "Instance"-part)<br>
	<br>
	Caution that not to cause infinite-nesting by replacing.<br>
	*/
	public bool InstanceChange(int idParts, GameObject source)
	{
		if((null == DataAnimation) || (null == TableControlParts))
		{
			return(false);
		}
		if((0 > idParts) || (TableControlParts.Length <= idParts))
		{
			return(false);
		}
		if(Library_SpriteStudio6.Data.Parts.Animation.KindFeature.INSTANCE != DataAnimation.TableParts[idParts].Feature)
		{
			return(false);
		}

		Script_SpriteStudio6_Root scriptRootHighest = RootGetHighest();
		if(null == scriptRootHighest)
		{
			scriptRootHighest = this;
		}

		/* Renew "Instance" */
		if(false == TableControlParts[idParts].BootUpInstance(this, idParts, true, source))
		{
			return(false);
		}

		/* Rebuild Hiest-Root's Draw buffes */
		if(false == scriptRootHighest.ClusterBootUpDraw())
		{
			return(false);
		}

		return(true);
	}

	/* ******************************************************** */
	//! Change "Instance"'s Animation (by name)
	/*!
	@param	idParts
		Parts-ID ("Instance"-part)
	@param	nameAnimation
		New "Instance"'s animation-name<br>
		"" or null == Change only "ignoreAttribute"
	@param	ignoreAttribute
		NON == Restart animation when new "Instance" attribute deecoded<br>
		NOW_ANIMATION == Ignore "Instance" attribute until new animation starts playing<br>
		PERMANENT == Continue Ignoring "Instance" attribute even if new animation starts playing
	@param	flagStartImmediate
		true == Animation is started immediate<br>
		false == Start playing according to "Instance" attribute
	@param	timesPlay
		0 == Infinite-looping<br>
		1 == Not looping<br>
		2 <= Number of Plays
	@param	rateTime
		Coefficient of time-passage of new animation.<br>
		Minus Value is given, Animation is played backwards.
	@param	style
		Library_SpriteStudio6.KindStylePlay.NOMAL == Animation is played One-Way.<br>
		Library_SpriteStudio6.KindStylePlay.PINGPONG == Animation is played round-trip.
	@param	labelRangeStart
		Label name to start playing animation.<br>
		"" or "_start" == Top frame of Animation ("_start" is reserved label-name)
	@param	frameRangeOffsetStart
		Offset frame from labelRangeStart<br>
		Start frame of animation play range is "labelRangeStart + frameRangeOffsetStart".
	@param	labelRangeEnd
		Label-name of the terminal in animation.<br>
		"" or "_end" == Last frame of Animation ("_end" is reserved label-name)
	@param	frameRangeOffsetEnd
		Offset frame from labelRangeStart<br>
		End frame of animation play range is "labelRangeEnd + frameRangeOffsetEnd".
	@retval	Return-Value
		true == Success<br>
		false == Failure (Error)

	Change "Instance"'s animation controled by part.<br>
	<br>
	To change "Instance"'s animation, use this function without calling "Instance"'s "Script_SpriteStudio6_Root.AnimationPlay".<br>
	(Cause inconsistency with control from "Instance" part)<br>
	<br>
	As a general rule, no designation to "Do not change playing  parameters" like "AnimationPlay".<br>
	The only exception is changing attribute-ignore setting(ignoreAttribute)  without changing the animation.<br>
	When set null or "" to "nameAnimation", change only attribute-ignore setting.<br>
	(The argument omission after "flagStartImmediate" is written to change only attribute-ignore setting)<br>
	<br>
	When "nameAnimation" is set normally animation name...<br>
	If "flagStartImmediate" is set to true, "Instance"'s animation will be played immediately.<br>
	In the case, "Instance" play behaves the same as when "Independent of time" is checked in the "Instance" attribute on SpriteStudio6.<br>
	Conversely, If "flagStartImmediate" is set to false, forcefully wait for next data of "Instance" attribute and start playing at decoding new data.
	*/
	public bool AnimationChangeInstance(	int idParts,
											string nameAnimation,
											Library_SpriteStudio6.KindIgnoreAttribute ignoreAttribute,
											bool flagStartImmediate = false,
											int timesPlay = 1,
											float rateTime = 1.0f,
											Library_SpriteStudio6.KindStylePlay style = Library_SpriteStudio6.KindStylePlay.NORMAL,
											string labelRangeStart = null,
											int frameRangeOffsetStart = 0,
											string labelRangeEnd = null,
											int frameRangeOffsetEnd = 0
										)
	{
		Script_SpriteStudio6_Root scriptRootInstance = InstanceGet(idParts);
		if(null == scriptRootInstance)
		{
			return(false);
		}

		int indexAnimation = -1;
		if(false == string.IsNullOrEmpty(nameAnimation))
		{
			indexAnimation = scriptRootInstance.IndexGetAnimation(nameAnimation);
			if(0 > indexAnimation)
			{
				return(false);
			}
		}

		return(AnimationChangeInstanceMain(	ref TableControlParts[idParts],
											scriptRootInstance,
											indexAnimation,
											ignoreAttribute,
											flagStartImmediate,
											timesPlay,
											rateTime,
											style,
											labelRangeStart,
											frameRangeOffsetStart,
											labelRangeEnd,
											frameRangeOffsetEnd
										)
			);
	}
	private bool AnimationChangeInstanceMain(	ref Library_SpriteStudio6.Control.Animation.Parts controlParts,
												Script_SpriteStudio6_Root scriptRootInstance,
												int indexAnimation,
												Library_SpriteStudio6.KindIgnoreAttribute ignoreAttribute,
												bool flagStartImmediate,
												int timesPlay,
												float rateTime,
												Library_SpriteStudio6.KindStylePlay style,
												string labelRangeStart,
												int frameRangeOffsetStart,
												string labelRangeEnd,
												int frameRangeOffsetEnd
											)
	{
		if(null == TableControlTrack)
		{
			return(false);
		}
		if(0 > timesPlay)
		{
			return(false);
		}
		if(true == float.IsNaN(rateTime))
		{
			return(false);
		}

		Library_SpriteStudio6.Data.Animation.Attribute.Instance dataInstance = new Library_SpriteStudio6.Data.Animation.Attribute.Instance();
		dataInstance.Flags = Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.CLEAR;
		switch(style)
		{
			case Library_SpriteStudio6.KindStylePlay.NO_CHANGE:
				return(false);

			case Library_SpriteStudio6.KindStylePlay.NORMAL:
				break;

			case Library_SpriteStudio6.KindStylePlay.PINGPONG:
				dataInstance.Flags |= Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.PINGPONG;
				break;
		}

		controlParts.Status &= ~Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.INSTANCE_IGNORE_EXCEPT_NEXTDATA;
		switch(ignoreAttribute)
		{
			case Library_SpriteStudio6.KindIgnoreAttribute.NON:
				controlParts.Status &= ~Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.INSTANCE_IGNORE_ATTRIBUTE;
				controlParts.Status &= ~Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.INSTANCE_IGNORE_NEWANIMATION;

				dataInstance.Flags &= ~Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.INDEPENDENT;
				break;

			case Library_SpriteStudio6.KindIgnoreAttribute.NOW_ANIMATION:
				controlParts.Status |= Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.INSTANCE_IGNORE_ATTRIBUTE;
				controlParts.Status &= ~Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.INSTANCE_IGNORE_NEWANIMATION;

				dataInstance.Flags |= Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.INDEPENDENT;
				break;

			case Library_SpriteStudio6.KindIgnoreAttribute.PERMANENT:
				controlParts.Status |= Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.INSTANCE_IGNORE_ATTRIBUTE;
				controlParts.Status |= Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.INSTANCE_IGNORE_NEWANIMATION;

				dataInstance.Flags |= Library_SpriteStudio6.Data.Animation.Attribute.Instance.FlagBit.INDEPENDENT;
				break;
		}

		dataInstance.PlayCount = timesPlay;
		dataInstance.RateTime = rateTime;
		dataInstance.LabelStart = labelRangeStart;
		dataInstance.OffsetStart = frameRangeOffsetStart;
		dataInstance.LabelEnd = labelRangeEnd;
		dataInstance.OffsetEnd = frameRangeOffsetEnd;

		if(0 <= indexAnimation)
		{
			if(true == flagStartImmediate)
			{
				controlParts.Status &= ~Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.INSTANCE_IGNORE_EXCEPT_NEXTDATA;
			}
			else
			{
				controlParts.Status |= Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.INSTANCE_IGNORE_EXCEPT_NEXTDATA;

				/* MEMO: When does not start immediately, necessary to always decode next data, so turn off "Independent time" status. */
				controlParts.Status &= ~Library_SpriteStudio6.Control.Animation.Parts.FlagBitStatus.INSTANCE_PLAY_INDEPENDENT;
			}

			controlParts.IndexAnimationUnderControl = indexAnimation;
			controlParts.DataInstance = dataInstance;

			int indexTrack = controlParts.IndexControlTrack;
			if(0 <= indexTrack)
			{
				return(controlParts.InstancePlayStart(this, TableControlTrack[indexTrack].StatusIsPlayingReverse));
			}
		}
		return(true);
	}

	/* ******************************************************** */
	//! Change "Instance"'s Animation (by index)
	/*!
	@param	idParts
		Parts-ID ("Instance"-part)
	@param	indexAnimation
		New "Instance"'s animation-index<br>
		-1 == Change only "ignoreAttribute"
	@param	flagStartImmediate
		true == Animation is started immediate<br>
		false == Start playing according to "Instance" attribute
	@param	ignoreAttribute
		NON == Restart animation when new "Instance" attribute deecoded<br>
		NOW_ANIMATION == Ignore "Instance" attribute until new animation starts playing<br>
		PERMANENT == Continue Ignoring "Instance" attribute even if new animation starts playing
	@param	rateTime
		Coefficient of time-passage of new animation.<br>
		Minus Value is given, Animation is played backwards.
	@param	style
		Library_SpriteStudio6.KindStylePlay.NOMAL == Animation is played One-Way.<br>
		Library_SpriteStudio6.KindStylePlay.PINGPONG == Animation is played round-trip.
	@param	labelRangeStart
		Label name to start playing animation.<br>
		"" or "_start" == Top frame of Animation ("_start" is reserved label-name)
	@param	frameRangeOffsetStart
		Offset frame from labelRangeStart<br>
		Start frame of animation play range is "labelRangeStart + frameRangeOffsetStart".
	@param	labelRangeEnd
		Label-name of the terminal in animation.<br>
		"" or "_end" == Last frame of Animation ("_end" is reserved label-name)
	@param	frameRangeOffsetEnd
		Offset frame from labelRangeStart<br>
		End frame of animation play range is "labelRangeEnd + frameRangeOffsetEnd".
	@retval	Return-Value
		"Instance" animation's instance<br>
		null == Error / Invalid instance animation

	Change "Instance"'s animation controled by part.<br>
	<br>
	This function is the same as "name designation" except that animation is "index designation".<br>
	(This function is a little bit faster than "name designation" by not searching animation names)<br>
	Can use if "Instance"'s animation indexes have been gotten in advance.<br>
	*/
	public bool AnimationChangeInstance(	int idParts,
											int indexAnimation,
											Library_SpriteStudio6.KindIgnoreAttribute ignoreAttribute,
											bool flagStartImmediate = false,
											int timesPlay = 1,
											float rateTime = 1.0f,
											Library_SpriteStudio6.KindStylePlay style = Library_SpriteStudio6.KindStylePlay.NORMAL,
											string labelRangeStart = null,
											int frameRangeOffsetStart = 0,
											string labelRangeEnd = null,
											int frameRangeOffsetEnd = 0
										)
	{
		Script_SpriteStudio6_Root scriptRootInstance = InstanceGet(idParts);
		if(null == scriptRootInstance)
		{
			return(false);
		}

		if(0 <= indexAnimation)
		{
			if(scriptRootInstance.CountGetAnimation() <= indexAnimation)
			{
				return(false);
			}
		}

		return(AnimationChangeInstanceMain(	ref TableControlParts[idParts],
											scriptRootInstance,
											indexAnimation,
											ignoreAttribute,
											flagStartImmediate,
											timesPlay,
											rateTime,
											style,
											labelRangeStart,
											frameRangeOffsetStart,
											labelRangeEnd,
											frameRangeOffsetEnd
										)
			);
	}

	/* ******************************************************** */
	//! Get instance of "Effect"
	/*!
	@param	idParts
		Parts-ID
	@retval	Return-Value
		"Effect" animation's instance<br>
		null == Error / Invalid instance animation

	Get "Effect" animation controlled by part.<br>
	<br>
	The reason why this function returns null is as follows.
	<br>
	- "idParts" is not "Effect" part<br>
	- "Effect"'s instance does not exist<br>
	- Before executing "Start()"<br>
	*/
	public Script_SpriteStudio6_RootEffect EffectGet(int idParts)
	{
		if((null == DataAnimation) || (null == TableControlParts))
		{
			return(null);
		}

		if((0 > idParts) || (TableControlParts.Length <= idParts))
		{
			return(null);
		}

		if(Library_SpriteStudio6.Data.Parts.Animation.KindFeature.EFFECT != DataAnimation.TableParts[idParts].Feature)
		{
			return(null);
		}

		return(TableControlParts[idParts].InstanceRootEffectUnderControl);
	}

//	EffectChange
	#endregion Functions
	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	public static partial class Parts
	{
		/* ----------------------------------------------- Functions */
		#region Functions
		/* ******************************************************** */
		//! Get Root-Parts
		/*!
		@param	gameObject
			GameObject of starting search
		@param	flagApplySelf
			true == Include "gameObject" as check target<br>
			false == exclude "gameObject"<br>
			default: true
		@retval	Return-Value
			Instance of "Script_SpriteStudio6_Root"<br>
			null == Not-Found / Failure	

		Get component "Script_SpriteStudio6_Root" by examining "gameObject" and direct-children.<br>
		<br>
		This function returns "Script_SpriteStudio6_Root" first found.<br>
		However, it is not necessarily in shallowest GameObject-hierarchy.<br>
		(Although guarantee up to direct-children, can not guarantee if farther than direct-children)<br>
		*/
		public static Script_SpriteStudio6_Root RootGet(GameObject gameObject, bool flagApplySelf=true)
		{
			Script_SpriteStudio6_Root scriptRoot = null;

			/* Check Origin */
			if(true == flagApplySelf)
			{
				scriptRoot = RootGetMain(gameObject);
				if(null != scriptRoot)
				{
					return(scriptRoot);
				}
			}

			/* Check Direct-Children */
			/* MEMO: Processing is wastefull, but check direct-children first so that make to find in closely-relation as much as possible. */
			int countChild = gameObject.transform.childCount;
			Transform transformChild = null;

			for(int i=0; i<countChild; i++)
			{
				transformChild = gameObject.transform.GetChild(i);
				scriptRoot = RootGetMain(transformChild.gameObject);
				if(null != scriptRoot)
				{
					return(scriptRoot);
				}
			}

			/* Check Children */
			for(int i=0; i<countChild; i++)
			{
				transformChild = gameObject.transform.GetChild(i);
				scriptRoot = RootGet(transformChild.gameObject, false);
				if(null != scriptRoot)
				{	/* Has Root-Parts */
					return(scriptRoot);
				}
			}

			return(null);
		}
		private static Script_SpriteStudio6_Root RootGetMain(GameObject gameObject)
		{
			Script_SpriteStudio6_Root scriptRoot = null;
			scriptRoot = gameObject.GetComponent<Script_SpriteStudio6_Root>();
			if(null != scriptRoot)
			{	/* Has Root-Parts */
				if(null == scriptRoot.InstanceRootParent)
				{	/* has no Parent */
					return(scriptRoot);
				}
			}

			return(null);
		}
		#endregion Functions
	}
	#endregion Classes, Structs & Interfaces
}
