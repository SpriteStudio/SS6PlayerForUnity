//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//
Shader "Custom/SpriteStudio6/UnityNative/SkinnedMesh"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1, 1, 1, 1)
		[Toggle(PIXELSNAP_ON)] PixelSnap("Pixel snap", Float) = 0

		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSource("Blend Source", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDestination("Blend Destination", Float) = 0
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOperation("Blend Operation", Float) = 0
		[Toggle] _ZWrite("Write Z Buffer", Float) = 0

		[Toggle(PS_NOT_DISCARD)] _NotDiscardPixel("Not Discard Pixel", Float) = 0
		[Toggle(PS_OUTPUT_PMA)] _OutputPixelPMA("Output PreMultiplied Alpha", Float) = 0
		[Toggle(PS_INPUT_PMA)] _InputPixelPMA("Input PreMultiplied Alpha", Float) = 0

		[HideInInspector] _RendererColor("RendererColor", Color) = (1, 1, 1, 1)
		[HideInInspector] _Flip("Flip", Vector) = (1, 1, 1, 1)
		[HideInInspector] _BlendParam("BlendParam", Vector) = (0.01, 1.0, 0.0, 0.0)
		[HideInInspector] _PartsColor_LU("PartsColor_LU", Color) = (1, 1, 1, 0)
		[HideInInspector] _PartsColor_RU("PartsColor_RU", Color) = (1, 1, 1, 0)
		[HideInInspector] _PartsColor_RD("PartsColor_RD", Color) = (1, 1, 1, 0)
		[HideInInspector] _PartsColor_LD("PartsColor_LD", Color) = (1, 1, 1, 0)
		[HideInInspector] _PartsColor_Opacity("PartsColor_Opacity", Color) = (1, 1, 1, 1)
		[HideInInspector] _CellPivot_LocalScale("CellPivot_LocalScale", Vector) = (1.0, 1.0, 1.0, 1.0)
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
		}

		Pass
		{
			Cull Off
			ZTest LEqual
			ZWRITE [_ZWrite]
			BlendOp [_BlendOperation]
			Blend [_BlendSource] [_BlendDestination]

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

			#pragma target 2.0
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"

			#define RESTRICT_SHADER_MODEL_3
			#pragma multi_compile _ PS_NOT_DISCARD
			#pragma multi_compile _ PS_OUTPUT_PMA
			#pragma multi_compile _ PS_INPUT_PMA
			#include "Base/Shader_Data_UnityNative.cginc"
			#include "Base/Shader_Lib_SpriteStudio6.cginc"
			#include "Base/ShaderVertex_SkinnedMesh_UnityNative.cginc"
			#include "Base/ShaderPixel_Sprite_UnityNative.cginc"
			ENDCG
		}
	}
	FallBack Off
}
