//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//

/* Defines for Vertex-Shader */
#define VERTEX_STATICDATA_PARTSCOLOR											\
	/* MEMO: ".x" is not used, now.  */											\
	static const float4 _OverlayParameter_Mix = { 1.0f, 1.0f, 0.0f, 1.0f };		\
	static const float4 _OverlayParameter_Add = { 1.0f, 0.0f, 0.0f, 1.0f };		\
	static const float4 _OverlayParameter_Sub = { 1.0f, 0.0f, 0.0f, -1.0f };	\
	static const float4 _OverlayParameter_Mul = { 1.0f, 1.0f, 1.0f, 1.0f }

/* Defines for Pixel-Shader */
#if defined(SV_Target)
#define PIXELSHADER_BINDOUTPUT	SV_Target
#else
#define PIXELSHADER_BINDOUTPUT	COLOR0
#endif

#if defined(SV_POSITION)
#define PIXELSHADER_BINDDATA_POSITION	SV_POSITION
#else
#define PIXELSHADER_BINDDATA_POSITION	POSITION
#endif

/* ********************************************************* */
//! [for Vertex-Shader] Routine processing for "Cell"
/*!
@param	_output_
	[Out] Struct-data for Pixel-Shader input
@param	_input_
	Vertex-Shader's input struct (Vertex-Data)

@retval	Return-Value
	(none)
@retval	_output_
	Parameter for "Cell auxiliary"

Set auxiliary data for "Cell".

Specifically, Set "_input_.texcoord1" and "_input_.texcoord2" to
 "_output_.Texture00UVMinmax" and "_output_.Texture00UVAverage".
Since a fixed process, be made into a function-macro with
  intention of preventing errors in description.
*/
#define VertexSetCellAuxiliary(_output_,_input_)		\
	_output_.Texture00UVMinMax = _input_.texcoord1;		\
	_output_.Texture00UVAverage = _input_.texcoord2;

/* ********************************************************* */
//! [for Vertex-Shader] Routine processing for "Parts-Color"
/*!
@param	_output_
	[Out] Struct-data for Pixel-Shader input
@param	_indexBlend_
	Blending type of "Parts-Color" (index)

@retval	Return-Value
	(none)
@retval	_output_
	Parameter for "Parts-Color"

Set parameter for "Parts-Color".

Use in conjunction with "VERTEX_STATICDATA_PARTSCOLOR".
When "RESTRICT_SHADER_MODEL_3" is not defined, process empty
	(When not defined, all calculations will be done in Pixel-Shader).
*/
#if defined(RESTRICT_SHADER_MODEL_3)
#define VertexSetPartsColor(_output_,_indexBlend_,_rate_)							\
	{																				\
		float _ratioSrc = 1.0;														\
		float _ratioDst = _rate_;													\
		float4 _parameterOverlay;													\
		if(2.0f > _indexBlend_)	{													\
			if(1.0f > _indexBlend_)	{												\
				_parameterOverlay = _OverlayParameter_Mix;							\
				_ratioSrc -= _ratioDst;												\
			} else {																\
				_parameterOverlay = _OverlayParameter_Add;							\
			}																		\
		} else {																	\
			if(3.0f > _indexBlend_)	{												\
				_parameterOverlay = _OverlayParameter_Sub;							\
			} else {																\
				_parameterOverlay = _OverlayParameter_Mul;							\
				_ratioSrc -= _ratioDst;												\
			}																		\
		}																			\
		_output_.ArgumentVs00 = float4(	_ratioSrc,									\
										_parameterOverlay.w * _ratioDst,			\
										_parameterOverlay.y,						\
										0.0											\
									);												\
		_output_.ParameterOverlay = _parameterOverlay;								\
	}
#else
#define VertexSetPartsColor(_output_,_indexBlend_,_rate_)							\
	{																				\
		float _ratioSrc = 1.0;														\
		float _ratioDst = _rate_;													\
		float4 _parameterOverlay;													\
		if(2.0f > _indexBlend_)	{													\
			if(1.0f > _indexBlend_)	{												\
				_parameterOverlay = _OverlayParameter_Mix;							\
				_ratioSrc -= _ratioDst;												\
			} else {																\
				_parameterOverlay = _OverlayParameter_Add;							\
			}																		\
		} else {																	\
			if(3.0f > _indexBlend_)	{												\
				_parameterOverlay = _OverlayParameter_Sub;							\
			} else {																\
				_parameterOverlay = _OverlayParameter_Mul;							\
				_ratioSrc -= _ratioDst;												\
			}																		\
		}																			\
		_output_.ArgumentVs00 = float4(	_ratioSrc,									\
										_parameterOverlay.w * _ratioDst,			\
										_parameterOverlay.y,						\
										0.0											\
									);												\
	}
#endif

/* ********************************************************* */
//! [for Pixel-Shader] Routine processing for "Parts-Color"
/*!
@param	_pixel_
	[In/Out] Pixel-Color/Alpha
@param	_input_
	Pixel-Shader's input struct

@retval	Return-Value
	(none)
@param	_pixel_
	Pixel-Color combined with "Part-Color"

Set parameter for "Parts-Color".

Be sure to process the "VertexSetPartsColor" in Vertex-Shader.
*/
#if defined(RESTRICT_SHADER_MODEL_3)
#define PixelSynthesizePartsColor(_pixel_,_input_)																									\
	{																																				\
		half4	colorOverlay = _input_.ColorOverlay;																								\
		float	colorOverlayA = colorOverlay.w;																										\
		half4	overlayParameter = _input_.ParameterOverlay;																						\
		half4	pixelCoefficientColorOvelay = (half4(1.0f, 1.0f, 1.0f, 1.0f) * (1.0f - overlayParameter.z)) + (_pixel_ * overlayParameter.z);		\
		colorOverlay *= colorOverlayA;																												\
		_pixel_ = (_pixel_ * (1.0f - (colorOverlayA * overlayParameter.y))) + (pixelCoefficientColorOvelay * colorOverlay * overlayParameter.w);	\
	}
#else
#define PixelSynthesizePartsColor(_pixel_,_input_)														\
	{																									\
		half4 color[4];																					\
		float rate = _input_.ColorOverlay.w;															\
		float rateInverse = 1.0f - rate;																\
		color[0] = (_pixel_ * rateInverse) + (_input_.ColorOverlay * rate);	/* Mix */					\
		color[1] = _pixel_ + (_input_.ColorOverlay * rate);	/* Add */									\
		color[2] = _pixel_ - (_input_.ColorOverlay * rate);	/* Subtract */								\
		color[3] = (_pixel_ * rateInverse) + ((_pixel_ * _input_.ColorOverlay) * rate);	/* Multiple */	\
		_pixel_ = color[_input_.Texture00UV.z];															\
	}
#endif

/* ********************************************************* */
//! [for Pixel-Shader] Synthesize External-Alpha
/*!
@param	_pixleA_
	[In/Out] Alpha of source (float4 / fixed4)
@param	_textureExternal_
	External texture to get alpha (sampler2D)
@param	_UV_
	UV coordinates (float2 / fixed2)
@param	_rate_
	Rate to apply external alpha (float)

@retval	Return-Value
	(none)
@retval	_pixelA_
	Synthesized alpha

Synthesize pixel's alpha with external texture's pixel.
Use then recording alpha to external texture, as in ETC1.

When "ETC1_EXTERNAL_ALPHA" is not defined, process empty.
*/
#if defined(ETC1_EXTERNAL_ALPHA)
#define PixelSynthesizeExternalAlpha(_pixleA_,_textureExternal_,_UV_,_rate_)	\
	{																			\
		half4 alpha = tex2D(_textureExternal_, _UV_);							\
		_pixleA_ = lerp(_pixleA_, alpha.r, _rate_);								\
	}
#else
#define PixelSynthesizeExternalAlpha(_pixleA_,_textureAlpha,_UV_,_rate_)
#endif

/* ********************************************************* */
//! [for Pixel-Shader] Pixel discard at alpha-threshold
/*!
@param	_pixleA_
	Pixel's alpha (float)
@param	_threshould_
	Threshould (float)

@retval	Return-Value
	(none)

When "_pixelA_" is not greater than "_threshold_", discard pixel.

When "PS_NOT_DISCARD" is not defined, process empty.
*/
#if !defined(PS_NOT_DISCARD)
#define PixelDiscardAlpha(_pixelA_,_threshould_)	\
	if(_threshould_ >= _pixelA_)	{				\
		 discard;									\
	}
#else
#define PixelDiscardAlpha(_pixelA_,_threshould_)
#endif

/* ********************************************************* */
//! [for Pixel-Shader] Adjust RGB with pre-multiplied alpha
/*!
@param	_pixleA_.xyz
	[In/Out] Pixel's RGB (float4 / fixed4)
@param	_pixelA_
	Alpha (float)

@retval	Return-Value
	(none)
@retval	_pixelA_.xyz
	Adjusted RGB

Adjusts pixel's RGB to take into account the pre-multiplied alpha.

When "PS_OUTPUT_PMA" is not defined, process empty.
*/
#if defined(PS_OUTPUT_PMA)
#define PixelSolvePMA(_pixelRGB_,_pixelA_)				\
	if(0.0f >= _pixelA_)	{							\
		_pixelRGB_ = float4(0.0f, 0.0f, 0.0f, 0.0f);	\
	} else {											\
		_pixelRGB_.xyz *= _pixelA_;						\
	}
#else
#define PixelSolvePMA(_pixelRGB_,_pixelA_)
#endif
/* MEMO: (Abbreviated-)Processing for "PixelDiscardAlpha" and other cases where a non-zero Alpha guaranteed. */
#if defined(PS_OUTPUT_PMA)
#define PixelSolvePMA_ValidAlpha(_pixelRGB_,_pixelA_)	\
	_pixelRGB_.xyz *= _pixelA_;
#else
#define PixelSolvePMA_ValidAlpha(_pixelRGB_,_pixelA_)
#endif
