//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//

Shader "Custom/SpriteStudio6/UnityNative/SkinnedMesh/Inverse"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1, 1, 1, 1)
		[HideInInspector] _Flip("Flip", Vector) = (1, 1, 1, 1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

		[HideInInspector] _BlendParam("BlendParam", Vector) = (0.01, 1, 1, 0)
		[HideInInspector] _PartsColor("Parts Color", Color) = (1, 1, 1, 0)
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
		}

		Pass
		{
			Cull Off
			ZTest LEqual
			ZWRITE Off

			Blend OneMinusDstColor Zero

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

			#pragma target 2.0
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#include "UnityCG.cginc"

			#define RESTRICT_SHADER_MODEL_3
			#define PS_NOT_DISCARD
			#define	PS_NOT_CLAMP_COLOR
			#include "Base/Shader_Data_UnityNative.cginc"
			#include "Base/ShaderVertex_SkinnedMesh_UnityNative.cginc"
			#include "Base/ShaderPixel_Sprite_UnityNative.cginc"
			ENDCG
		}
	}
	FallBack Off
}
