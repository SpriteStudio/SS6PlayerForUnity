//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//
Shader "Custom/SpriteStudio6/SS6PU/ss-sepia"
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
		[Toggle] _ZWrite("Write Z Buffer", Float) = 0

		[Toggle(PS_NOT_DISCARD)] _NotDiscardPixel("Not Discard Pixel", Float) = 0
		[Toggle(PS_OUTPUT_PMA)] _OutputPixelPMA("Output PreMultiplied Alpha", Float) = 0

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
			ZWRITE [_ZWrite]
			Stencil
			{
				Ref 0
				Comp [_CompareStencil]
				Pass Keep
			}
			BlendOp [_BlendOperation]
			Blend [_BlendSource] [_BlendDestination]

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"

//			#define RESTRICT_SHADER_MODEL_3
			#pragma multi_compile _ PS_NOT_DISCARD
			#pragma multi_compile _ PS_OUTPUT_PMA
			#include "../Base/Shader_Lib_SpriteStudio6.cginc"
			#include "../Base/Shader_Data_SpriteStudio6.cginc"
			#include "../Base/ShaderVertex_Sprite_SpriteStudio6.cginc"
			#include "Base/Shader_Lib_SS6toSS6PU.cginc"
			#include "Base/ShaderPixel_Sprite_ss-sepia.cginc"
			ENDCG
		}
	}
	FallBack Off
}
