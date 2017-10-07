/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script_SpriteStudio6_DataEffect : ScriptableObject
{
	/* ----------------------------------------------- Variables & Properties */
	#region Variables & Properties
	public KindVersion Version;

	public FlagBit FlagData;
	public bool StatusIsLockSeedRandom
	{
		get
		{
			return(0 != (FlagData & FlagBit.SEEDRANDOM_LOCK));
		}
	}
	public int SeedRandom;
	public int VersionRenderer;

	public int CountMaxParticle;
	public int CountFramePerSecond;

	public Vector2 ScaleLayout;

	public Library_SpriteStudio6.Data.Parts.Effect[] TableParts;
	public Library_SpriteStudio6.Data.Effect.Emitter[] TableEmitter;
	public int[] TableIndexEmitterOrderDraw;
	#endregion Variables & Properties

	/* ----------------------------------------------- Functions */
	#region Functions
	public void CleanUp()
	{
		Version = (KindVersion)(-1);
	}

	public int CountGetParts()
	{
		return((null != TableParts) ? TableParts.Length : -1);
	}

	public int CountGetEmitter()
	{
		return((null != TableEmitter) ? TableEmitter.Length : -1);
	}
	#endregion Functions

	/* ----------------------------------------------- Enums & Constants */
	#region Enums & Constants
	public enum KindVersion
	{
		SS5PU = 0,	/* Before SS5PU *//* (Reserved) */
		CODE_010000,	/* SS6PU Ver.1.0.0 */

		SUPPORT_EARLIEST = CODE_010000,
		SUPPORT_LATEST = CODE_010000
	}

	[System.Flags]
	public enum FlagBit
	{
		SEEDRANDOM_LOCK = 0x00000001,

		CLEAR = 0x00000000
	}
	#endregion Enums & Constants
}
