//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//
Shader "Custom/SpriteStudio6/SS6PU/Shape"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSource("Blend Source", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDestination("Blend Destination", Float) = 0
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOperation("Blend Operation", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _CompareStencil("Compare Stencil", Float) = 0

		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOperation("Stencil Operation", Float) = 0
		/* MEMO: (Ver.2.3.0-) Separates the stencil into bit-planes, so that "Invert"("Mask"-parts affected by */
		/*       masking) and "Increment/DecrementWrap"(not affected) never disturb each other.                */
		/*       Default 255 reproduces the behavior before Ver.2.3.0. (For replaced shaders lacking this)     */
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15

		[HideInInspector] _ArgumentFs00("Argument Fs00", Vector) = (0,0,0,0)
		[HideInInspector] _ParameterFs00("Parameter Fs00", Vector) = (0,0,0,0)
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass
		{
			Cull Off
			ZTest LEqual
			ZWRITE Off
			Stencil
			{
				Ref 0
				Comp [_CompareStencil]
				Pass [_StencilOperation]
				WriteMask [_StencilWriteMask]
			}
			ColorMask [_ColorMask]
			BlendOp [_BlendOperation]
			Blend [_BlendSource] [_BlendDestination]

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

//			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "HLSLSupport.cginc"

//			#define RESTRICT_SHADER_MODEL_3
// 			#define PS_NOT_DISCARD
//			#define PS_OUTPUT_PMA
			#include "Base/Shader_Lib_SpriteStudio6.cginc"
			#include "Base/Shader_Data_SpriteStudio6.cginc"
			#include "Base/ShaderVertex_Sprite_SpriteStudio6.cginc"
			#include "Base/ShaderPixel_Shape_SpriteStudio6.cginc"
			ENDCG
		}
	}
	FallBack Off
}
