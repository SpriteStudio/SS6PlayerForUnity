//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//

/* Defines for Vertex-Shader */
/* MEMO: ".x" is used for alphablending-method. (.x = Target Blending Type) */
#define VERTEX_STATICDATA_PARTSCOLOR											\
static const float4 _OverlayParameter_Mix = {0.0f, 1.0f, 0.0f, 1.0f};			\
static const float4 _OverlayParameter_Add = {0.0f, 0.0f, 0.0f, 1.0f};			\
static const float4 _OverlayParameter_Sub = {0.0f, 0.0f, 0.0f, -1.0f};			\
static const float4 _OverlayParameter_Mul = {0.0f, 1.0f, 1.0f, 1.0f}
// #define VERTEX_STATICDATA_PARTSCOLOR											\
// 	/* MEMO: ".x" is not used, now.  */											\
// 	static const float4 _OverlayParameter_Mix = { 1.0f, 1.0f, 0.0f, 1.0f };		\
// 	static const float4 _OverlayParameter_Add = { 1.0f, 0.0f, 0.0f, 1.0f };		\
// 	static const float4 _OverlayParameter_Sub = { 1.0f, 0.0f, 0.0f, -1.0f };	\
// 	static const float4 _OverlayParameter_Mul = { 1.0f, 1.0f, 1.0f, 1.0f }

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
// #if defined(RESTRICT_SHADER_MODEL_3)
// #define VertexSetPartsColor(_output_,_indexBlend_,_rate_)							\
// 	{																				\
// 		float _ratioSrc = 1.0;														\
// 		float _ratioDst = _rate_;													\
// 		float4 _parameterOverlay;													\
// 		if(2.0f > _indexBlend_)	{													\
// 			if(1.0f > _indexBlend_)	{												\
// 				_parameterOverlay = _OverlayParameter_Mix;							\
// 				_ratioSrc -= _ratioDst;												\
// 			} else {																\
// 				_parameterOverlay = _OverlayParameter_Add;							\
// 			}																		\
// 		} else {																	\
// 			if(3.0f > _indexBlend_)	{												\
// 				_parameterOverlay = _OverlayParameter_Sub;							\
// 			} else {																\
// 				_parameterOverlay = _OverlayParameter_Mul;							\
// 				_ratioSrc -= _ratioDst;												\
// 			}																		\
// 		}																			\
// 		_output_.ArgumentVs00 = float4(	_ratioSrc,									\
// 										_parameterOverlay.w * _ratioDst,			\
// 										_parameterOverlay.y,						\
// 										0.0											\
// 									);												\
// 		_output_.ParameterOverlay = _parameterOverlay;								\
// 	}
// #else
// #define VertexSetPartsColor(_output_,_indexBlend_,_rate_)							\
// 	{																				\
// 		float _ratioSrc = 1.0;														\
// 		float _ratioDst = _rate_;													\
// 		float4 _parameterOverlay;													\
// 		if(2.0f > _indexBlend_)	{													\
// 			if(1.0f > _indexBlend_)	{												\
// 				_parameterOverlay = _OverlayParameter_Mix;							\
// 				_ratioSrc -= _ratioDst;												\
// 			} else {																\
// 				_parameterOverlay = _OverlayParameter_Add;							\
// 			}																		\
// 		} else {																	\
// 			if(3.0f > _indexBlend_)	{												\
// 				_parameterOverlay = _OverlayParameter_Sub;							\
// 			} else {																\
// 				_parameterOverlay = _OverlayParameter_Mul;							\
// 				_ratioSrc -= _ratioDst;												\
// 			}																		\
// 		}																			\
// 		_output_.ArgumentVs00 = float4(	_ratioSrc,									\
// 										_parameterOverlay.w * _ratioDst,			\
// 										_parameterOverlay.y,						\
// 										0.0											\
// 									);												\
// 	}
// #endif

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
//#define PixelSynthesizePartsColor(_pixel_,_input_)																										\
//	{																																					\
//		half4	colorOverlay = _input_.ColorOverlay;																									\
//		float	colorOverlayA = colorOverlay.w;																											\
//		half4	parameterOverlay = _input_.ParameterOverlay;																							\
//		half4	white = half4(1.0, 1.0, 1.0, 1.0);																										\
//		half4	pixelCoefficientColorOvelay = (white * (1.0f - parameterOverlay.z)) + (_pixel_ * parameterOverlay.z);									\
//		colorOverlay *= colorOverlayA;																													\
//																																						\
//		float	methodAlphaBlend = parameterOverlay.x;																									\
//		if(11.0 <= methodAlphaBlend)	{		/* Ovl2 */																								\
//			if(((_pixel_.r + _pixel_.g + _pixel_.b) / 3) < 0.5)	{																						\
//				_pixel_ = 2.0 * (_pixel_  * colorOverlay);																								\
//			} else {																																	\
//				_pixel_ = white - (2.0 * (white - _pixel_) * (white - colorOverlay));																	\
//			}																																			\
//		} else if(10.0 <= methodAlphaBlend)	{	/* Scr2 */																								\
//			half4	white = half4(1.0, 1.0, 1.0, 1.0);																									\
//			_pixel_ = white - (white - _pixel_) * (white - colorOverlay);																				\
//		} else if(9.0 <= methodAlphaBlend)	{	/* Div2 */																								\
//			_pixel_ = colorOverlay / _pixel_;																											\
//		} else if(8.0 <= methodAlphaBlend)	{	/* Mul2 */																								\
//			_pixel_ = colorOverlay * _pixel_;																											\
//		} else {								/* MIX - INV */																							\
//			_pixel_ = (_pixel_ * (1.0f - (colorOverlayA * parameterOverlay.y))) + (pixelCoefficientColorOvelay * colorOverlay * parameterOverlay.w);	\
//		}																																				\
//	}
#define PixelSynthesizePartsColor(_pixel_,_input_)																									\
	{																																				\
		half4	colorOverlay = _input_.ColorOverlay;																								\
		float	colorOverlayA = colorOverlay.w;																										\
		half4	overlayParameter = _input_.ParameterOverlay;																						\
		half4	pixelCoefficientColorOvelay = (half4(1.0f, 1.0f, 1.0f, 1.0f) * (1.0f - overlayParameter.z)) + (_pixel_ * overlayParameter.z);		\
		colorOverlay *= colorOverlayA;																												\
		_pixel_ = (_pixel_ * (1.0f - (colorOverlayA * overlayParameter.y))) + (pixelCoefficientColorOvelay * colorOverlay * overlayParameter.w);	\
	}

// #if defined(RESTRICT_SHADER_MODEL_3)
// #define PixelSynthesizePartsColor(_pixel_,_input_)																									\
// 	{																																				\
// 		half4	colorOverlay = _input_.ColorOverlay;																								\
// 		float	colorOverlayA = colorOverlay.w;																										\
// 		half4	overlayParameter = _input_.ParameterOverlay;																						\
// 		half4	pixelCoefficientColorOvelay = (half4(1.0f, 1.0f, 1.0f, 1.0f) * (1.0f - overlayParameter.z)) + (_pixel_ * overlayParameter.z);		\
// 		colorOverlay *= colorOverlayA;																												\
// 		_pixel_ = (_pixel_ * (1.0f - (colorOverlayA * overlayParameter.y))) + (pixelCoefficientColorOvelay * colorOverlay * overlayParameter.w);	\
// 	}
// #else
// #define PixelSynthesizePartsColor(_pixel_,_input_)														\
// 	{																									\
// 		half4 color[4];																					\
// 		float rate = _input_.ColorOverlay.w;															\
// 		float rateInverse = 1.0f - rate;																\
// 		color[0] = (_pixel_ * rateInverse) + (_input_.ColorOverlay * rate);	/* Mix */					\
// 		color[1] = _pixel_ + (_input_.ColorOverlay * rate);	/* Add */									\
// 		color[2] = _pixel_ - (_input_.ColorOverlay * rate);	/* Subtract */								\
// 		color[3] = (_pixel_ * rateInverse) + ((_pixel_ * _input_.ColorOverlay) * rate);	/* Multiple */	\
// 		_pixel_ = color[_input_.Texture00UV.z];															\
// 	}
// #endif

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
	if(0.0 >= _pixelA_)	{								\
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

/* ********************************************************* */
//! [for Vertex/Pixel-Shader] Solve ColorSpace Linear to Gamma
/*!
@param	_pixleA_
	[In/Out] Pixel's RGBA (float4 / fixed4)

@retval	Return-Value
	Adjusted RGB (float4)

Solves differences in Color-Space settings.
*/
float4 PixelSolveColorspaceInput(float4 color)
{
#if defined(UNITY_COLORSPACE_GAMMA)
	return(color);
#else
#if defined(PS_INPUT_PMA)
	if((1.0 / 255.0) > color.a)	{
//		color = float4(0.0, 0.0, 0.0, 0.0);
		color = color;
	} else {
		color.rgb = color.rgb / color.a;
	}
#endif
// //	color.rgb = color.rgb;
//	color.rgb = sqrt(color.rgb);
	color.rgb = pow(color.rgb, 1.0/2.2);
//	color.rgb = (max(1.055h * pow(max(color, float4(0.0, 0.0, 0.0, 0.0)), 0.416666667) - 0.055, 0.0)).rgb;

	return(color);
#endif
}

/* ********************************************************* */
//! [for Vertex/Pixel-Shader] Solve ColorSpace Gamma to Linear
/*!
@param	_pixleA_
	[In/Out] Pixel's RGBA (float4 / fixed4)

@retval	Return-Value
	Adjusted RGB (float4)

Solves differences in Color-Space settings.
*/
float4 PixelSolveColorspaceOutput(float4 color)
{
#if defined(UNITY_COLORSPACE_GAMMA)
	return(color);
#else
// //	color.rgb = color.rgb;
//	color.rgb = (color * color).rgb;
    color.rgb = (pow(color, 2.2)).rgb;
//	color.rgb = (color * (color * (color * 0.305306011h + 0.682171111h) + 0.012522878h)).rgb;

    return (color);
#endif
}

/* ********************************************************* */
//! [for Pixel-Shader] Target Blending (Sampling FrameBuffer)
/*!
@param	_output_
	[Out] Result
@param	_source_
	[In] Pixel's RGBA (float4 / fixed4)
@param	_destination_
	[In] FrameBuffer's RGBA

@retval	_output_
	Blended color
*/
#if COMPILEOPTION_FRAMEBUFFER_FETCH
#define alphaSource	(_source_.a)
#define alphaDestination	(_destination_.a)
#define alphaOneMinusSourceAlpha	((1.0 - alphaSource) * alphaDestination)
#define colorWhite	float4(1.0, 1.0, 1.0, 1.0)
#define PixelBlendTarget(_output_,_source_,_destination_)																							\
	{																																				\
		float	methodAlphaBlend = parameterOverlay.x;																								\
		if(6.0 <= methodAlphaBlend)	{																												\
			if(9.0 <= methodAlphaBlend)	{																											\
				if(10.0 <= methodAlphaBlend)	{																									\
					if(11.0 <= methodAlphaBlend)	{																			/* OVL2 : 11 */		\
						if(((_pixel_.r + _pixel_.g + _pixel_.b) / 3) < 0.5)	{																		\
							_output_.xyz = 2.0 * (_source_.xyz  * _destination_.xyz);																\
						} else {																													\
							_output_.xyz = colorWhite.xyz - (2.0 * (colorWhite.xyz - _source_.xyz) * (colorWhite.xyz - _destination_.xyz));			\
						}																															\
					} else {																									/* SCR2 : 10 */		\
						_output_.xyz = colorWhite.xyz - (colorWhite.xyz - _source_.xyz) * (colorWhite.xyz - _destination_.xyz);						\
					}																																\
				} else {																										/* DIV2 : 9 */		\
					_output_.xyz = _destination_.xyz / _source_.xyz;																				\
				}																																	\
			} else {																																\
				if(7.0 <= methodAlphaBlend)	{																										\
					if(8.0 <= methodAlphaBlend)	{																				/* MUL2 : 8 */		\
						_output_.xyz = _source_.xyz * _destination_.xyz;																			\
					} else {																									/* INV : 7 */		\
						_output_.xyz = (_source_.xyz * (colorWhite.xyz - _destination_.xyz));														\
					}																																\
				} else {																										/* EXC : 6 */		\
					_output_.xyz = (_source_.xyz * (colorWhite.xyz - _destination_.xyz)) + (_destination_.xyz * alphaOneMinusSourceAlpha);			\
				}																																	\
			}																																		\
		} else {																																	\
			if(3.0 <= methodAlphaBlend)	{																											\
				if(4.0 <= methodAlphaBlend)	{																										\
					if(5.0 <= methodAlphaBlend)	{																				/* SCR : 5 */		\
						_output_.xyz = (_source_.xyz * (colorWhite.xyz - _destination_.xyz)) + _destination_.xyz;									\
					} else {																									/* MUL_NA : 4 */	\
						_output_.xyz = _source_.xyz * _destination_.xyz;																			\
					}																																\
				} else {																										/* MUL : 3 */		\
					_output_.xyz = (_source_.xyz * _destination_.xyz) + (_destination_.xyz * alphaOneMinusSourceAlpha);								\
				}																																	\
			} else {																																\
				if(1.0 <= methodAlphaBlend)	{																										\
					if(2.0 <= methodAlphaBlend)	{																				/* SUB : 2 */		\
						_output_.xyz = _destination_.xyz - (_source_.xyz * alphaSource);															\
					} else {																									/* ADD : 1 */		\
						_output_.xyz = (_source_.xyz * alphaSource) + _destination_.xyz;															\
					}																																\
				} else {																										/* MIX : 0 */		\
					_output_.xyz = _source_.xyz + (_destination_.xyz * alphaOneMinusSourceAlpha);													\
				}																																	\
			}																																		\
		}																																			\
																																					\
		_output_.w = _source_.w + (_destination_.w * alphaOneMinusSourceAlpha);																		\
	}

//	{																																					\
//		half4	colorOverlay = _input_.ColorOverlay;																									\
//		float	colorOverlayA = colorOverlay.w;																											\
//		half4	parameterOverlay = _input_.ParameterOverlay;																							\
//		half4	white = half4(1.0, 1.0, 1.0, 1.0);																										\
//		half4	pixelCoefficientColorOvelay = (white * (1.0f - parameterOverlay.z)) + (_pixel_ * parameterOverlay.z);									\
//		colorOverlay *= colorOverlayA;																													\
//																																						\
//		float	methodAlphaBlend = parameterOverlay.x;																									\
//		if(11.0 <= methodAlphaBlend)	{		/* Ovl2 */																								\
//			if(((_pixel_.r + _pixel_.g + _pixel_.b) / 3) < 0.5)	{																						\
//				_pixel_ = 2.0 * (_pixel_  * colorOverlay);																								\
//			} else {																																	\
//				_pixel_ = white - (2.0 * (white - _pixel_) * (white - colorOverlay));																	\
//			}																																			\
//		} else if(10.0 <= methodAlphaBlend)	{	/* Scr2 */																								\
//			half4	white = half4(1.0, 1.0, 1.0, 1.0);																									\
//			_pixel_ = white - (white - _pixel_) * (white - colorOverlay);																				\
//		} else if(9.0 <= methodAlphaBlend)	{	/* Div2 */																								\
//			_pixel_ = colorOverlay / _pixel_;																											\
//		} else if(8.0 <= methodAlphaBlend)	{	/* Mul2 */																								\
//			_pixel_ = colorOverlay * _pixel_;																											\
//		} else {								/* MIX - INV */																							\
//			_pixel_ = (_pixel_ * (1.0f - (colorOverlayA * parameterOverlay.y))) + (pixelCoefficientColorOvelay * colorOverlay * parameterOverlay.w);	\
//		}																																				\
//	}
#else
#endif
