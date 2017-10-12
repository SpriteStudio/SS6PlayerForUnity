/**
	SpriteStudio6 Player for Unity

	Copyright(C) Web Technology Corp. 
	All rights reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Script_SpriteStudio6_RootEffect
{
	/* ----------------------------------------------- Functions */
	#region Functions
	/* ********************************************************* */
	//! Get Material
	/*!
	@param	indexCellMap
		Serial-number of using Cell-Map
	@param	operationBlend
		Color-Blend Operation for the target
	@retval	Return-Value
		Material
	*/
	public Material MaterialGet(int indexCellMap, Library_SpriteStudio6.KindOperationBlendEffect operationBlend)
	{
		const int CountLength = (int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR;
		if(	(0 <= indexCellMap)
			&& ((null != TableMaterial) && ((TableMaterial.Length / CountLength) > indexCellMap))
			&& (Library_SpriteStudio6.KindOperationBlendEffect.NON < operationBlend) && (Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR > operationBlend)
			)
		{
			return(TableMaterial[(indexCellMap * CountLength) + (int)operationBlend]);
		}
		return(null);
	}

	/* ********************************************************* */
	//! Get Material-Table length
	/*!
	@param	countCellMap
		Number of CellMap-s<br>
		0 == Currently set Material-Table length<br>
		-1 == Original Material-Table length
	@retval	Return-Value
		Material-Table length
	*/
	public int CountGetMaterialTable(int countCellMap)
	{
		if(0 > countCellMap)
		{	/* Original */
			if((null == DataEffect) || (null == DataEffect.TableMaterial))
			{
				return(-1);
			}

			return(DataEffect.TableMaterial.Length);
		}

		if(0 == countCellMap)
		{
			if(null == TableMaterial)
			{
				return(-1);
			}

			return(TableMaterial.Length);
		}

		return(countCellMap * (int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_KIND);
	}

	/* ********************************************************* */
	//! Get Material-Table's index
	/*!
	@param	indexCellMap
		index of CellMap
	@param	blend
		Kind of Blending
	@retval	Return-Value
		index of Material-Table
	*/
	public int IndexGetMaterialTable(int indexCellMap, Library_SpriteStudio6.KindOperationBlendEffect blend)
	{
		return((indexCellMap * (int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_KIND) + (int)blend);
	}
	#endregion Functions
}
