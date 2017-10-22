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
	public UnityEngine.Material MaterialGet(	int indexCellMap,
												Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
												Library_SpriteStudio6.KindMasking masking
										)
	{
		if(TableCellMap.Length <= indexCellMap)
		{
			return (null);
		}

		int indexMaterial = Material.IndexGetTableMaterial(indexCellMap, operationBlend, masking);
		if(0 > indexMaterial)
		{
			return (null);
		}

		return (TableMaterial[indexMaterial]);
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
	public int CountGetMaterialTable(bool flagInUse = true)
	{
		if(false == flagInUse)
		{   /* Original */
			if((null == DataEffect) || (null == DataEffect.TableMaterial))
			{
				return (-1);
			}

			return (DataEffect.TableMaterial.Length);
		}

		if(null == TableMaterial)
		{
			return (-1);
		}

		return (TableMaterial.Length);
	}
	#endregion Functions

	/* ----------------------------------------------- Classes, Structs & Interfaces */
	#region Classes, Structs & Interfaces
	public static partial class Material
	{
		/* ----------------------------------------------- Functions */
		#region Functions
		/* ********************************************************* */
		//! Get Material-Table length
		/*!
		@param	countCellMap
			Number of CellMap-s
		@retval	Return-Value
			Material-Table length

		If give positive number to "countCellMap", returns length of materials needed to store.
		*/
		public static int CountGetTableMaterial(int countCellMap)
		{
			if(0 > countCellMap)
			{
				return (-1);
			}

			return (countCellMap * ((int)Library_SpriteStudio6.KindMasking.TERMINATOR * (int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_TABLEMATERIAL));
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

		Get material's index in TableMaterial.<br>
		Caution that this function does not check upper-limit of "indexCellMap".
		*/
		public static int IndexGetTableMaterial(int indexCellMap,
													Library_SpriteStudio6.KindOperationBlendEffect operationBlend,
													Library_SpriteStudio6.KindMasking masking
											)
		{
			if((0 > indexCellMap)
				|| (Library_SpriteStudio6.KindOperationBlendEffect.INITIATOR > operationBlend) || (Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_TABLEMATERIAL <= operationBlend)
				|| (Library_SpriteStudio6.KindMasking.THROUGH > masking) || (Library_SpriteStudio6.KindMasking.TERMINATOR <= masking)
				)
			{
				return (-1);
			}

			return ((((indexCellMap * (int)Library_SpriteStudio6.KindMasking.TERMINATOR) + (int)masking) * (int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_TABLEMATERIAL) + ((int)operationBlend + (int)Library_SpriteStudio6.KindOperationBlendEffect.INITIATOR));
		}

		/* ********************************************************* */
		//! Get Texture-count in Material-table
		/*!
		@param	tableMaterial
			Material-table
		@retval	Return-Value
			Number of textures that can be stored in Material-table

		Get number of textures that can be stored in Material-table.
		*/
		public static int CountGetTexture(UnityEngine.Material[] tableMaterial)
		{
			if(null == tableMaterial)
			{
				return(-1);
			}
			return(tableMaterial.Length / ((int)Library_SpriteStudio6.KindMasking.TERMINATOR * (int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR_TABLEMATERIAL));
		}

		/* ********************************************************* */
		//! Change Texture in Material-table
		/*!
		@param	tableMaterial
			Material-table
		@param	indexCellMap
			index of CellMap
		@param	texture
			Texture
		@param	flagMaterialNew
			Whether make a new material or overwrite<br>
			true == Create new material<br>
			false == Overwrite exist material
		@retval	Return-Value
			true == Success<br>
			false == Failure(Error)

		Change materials' texture corresponding to specified CellMap in material-table.<br>
		When "flagMaterialNew" is set true, new materials are created.<br>
		When false, materials are overwritten.
		*/
		public static bool TextureChange(UnityEngine.Material[] tableMaterial, int indexCellMap, Texture2D texture, bool flagMaterialNew)
		{
			int indexTop = IndexGetTableMaterial(indexCellMap, Library_SpriteStudio6.KindOperationBlendEffect.INITIATOR, Library_SpriteStudio6.KindMasking.THROUGH);
			if(0 > indexTop)
			{
				return(false);
			}

			UnityEngine.Material material;
			int index;
			for(int i=(int)Library_SpriteStudio6.KindMasking.THROUGH; i<(int)Library_SpriteStudio6.KindMasking.TERMINATOR; i++)
			{
				for(int j=(int)Library_SpriteStudio6.KindOperationBlendEffect.INITIATOR; j<(int)Library_SpriteStudio6.KindOperationBlendEffect.TERMINATOR; j++)
				{
					index = IndexGetTableMaterial(0, (Library_SpriteStudio6.KindOperationBlendEffect)j, (Library_SpriteStudio6.KindMasking)i);
					index += indexTop;

					material = tableMaterial[i];
					if(true == flagMaterialNew)
					{	/* Create Material */
						if(null == material)
						{
							material = new UnityEngine.Material(Library_SpriteStudio6.Data.Shader.ShaderGetEffect(	(Library_SpriteStudio6.KindOperationBlendEffect)j,
																													(Library_SpriteStudio6.KindMasking)i
																												)
															);
						}
						else
						{
							material = new UnityEngine.Material(material);
						}
					}
					else
					{	/* Overwrite Material */
						if(null == material)
						{
							material = new UnityEngine.Material(Library_SpriteStudio6.Data.Shader.ShaderGetAnimation(	(Library_SpriteStudio6.KindOperationBlend)j,
																														(Library_SpriteStudio6.KindMasking)i
																													)
															);
						}
					}
					material.mainTexture = texture;
					tableMaterial[index] = material;
				}
			}

			return(true);

		}
		#endregion Functions
	}
	#endregion Classes, Structs & Interfaces
}
