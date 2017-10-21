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
		Blend Operation for the target
	@retval	Return-Value
		Material

	Get specified material in TableMaterial.
	*/
	public Material MaterialGet(	int indexCellMap,
										Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
										Library_SpriteStudio6.KindMasking masking
									)
	{
		int indexMaterial = IndexGetMaterialTable(indexCellMap, operationBlend, masking);
		if(0 > indexMaterial)
		{
			return(null);
		}
		return(TableMaterial[indexMaterial]);
	}

	/* ********************************************************* */
	//! Get Material-Table length
	/*!
	@param	flagInUse
		true == TableMaterial's length of Currently in use<br>
		false == TableMaterial's length of original animation data
	@retval	Return-Value
		Material-Table length

	Get TableMaterial's length.
	*/
	public int CountGetMaterialTable(bool flagInUse=true)
	{
		if(false == flagInUse)
		{	/* Original */
			if((null == DataEffect) || (null == DataEffect.TableMaterial))
			{
				return(-1);
			}

			return(DataEffect.TableMaterial.Length);
		}

		if(null == TableMaterial)
		{
			return(-1);
		}

			return(TableMaterial.Length);
	}

	/* ********************************************************* */
	//! Get Material-Table length
	/*!
	@param	countCellMap
		Number of CellMap-s
	@retval	Return-Value
		Material-Table length

	If give positive number to "countCellMap", returns length of materials needed to store.
	*/
	public static int CountGetMaterialTable(int countCellMap)
	{
		if(0 > countCellMap)
		{
			return(-1);
		}

		return(countCellMap * ((int)Library_SpriteStudio6.KindMasking.TERMINATOR * (int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_TABLEMATERIAL));
	}

	/* ********************************************************* */
	//! Get Material-Table's index
	/*!
	@param	indexCellMap
		index of CellMap
	@param	operationBlend
		Kind of Blending
	@param	masking
		Kind of Masking
	@retval	Return-Value
		index of Material-Table

	Get material's index in TableMaterial.
	*/
	public int IndexGetMaterialTable(	int indexCellMap,
										Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
										Library_SpriteStudio6.KindMasking masking
									)
	{
		if((0 > indexCellMap) || (TableCellMap.Length <= indexCellMap)
			|| (Library_SpriteStudio6.KindOperationBlendEffect.INITIATOR_TABLEMATERIAL >= operationBlend) || (Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_TABLEMATERIAL <= operationBlend)
			|| (Library_SpriteStudio6.KindMasking.THROUGH > masking) || (Library_SpriteStudio6.KindMasking.TERMINATOR <= masking)
			)
		{
			return(-1);
		}
		return((((indexCellMap * (int)Library_SpriteStudio6.KindMasking.TERMINATOR) + (int)masking) * (int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_TABLEMATERIAL) + ((int)operationBlend + (int)Library_SpriteStudio6.KindOperationBlendEffect.INITIATOR_TABLEMATERIAL));
	}
	#endregion Functions
}
