//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//

// Customed by Web Technology Corp. Dec. 2017
//
// Copyright (c) 2016 Unity Technologies
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Shader "Custom/SpriteStudio6/UnityNative/Sprite/Inverse"
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

		[HideInInspector] _BlendParam("BlendParam", Vector) = (0.01, 1, 0, 0)
		[HideInInspector] _PartsColor_LU("Parts Color LU", Color) = (1, 1, 1, 0)
		[HideInInspector] _PartsColor_RU("Parts Color RU", Color) = (1, 1, 1, 0)
		[HideInInspector] _PartsColor_RD("Parts Color RD", Color) = (1, 1, 1, 0)
		[HideInInspector] _PartsColor_LD("Parts Color LD", Color) = (1, 1, 1, 0)
		[HideInInspector] _PartsColor_Opacity("Parts Color Opacity", Vector) = (1, 1, 1, 1)
		[HideInInspector] _CellPivot_LocalScale("Cell Pivot Local Scale", Vector) = (0, 0, 1, 1)
		[HideInInspector] _CellRectangle("Cell Rectangle", Vector) = (0, 0, 1, 1)
		[HideInInspector] _VertexOffset_LURU("VertexOffset LURU", Vector) = (0, 0, 0, 0)
		[HideInInspector] _VertexOffset_RDLD("VertexOffset RDLD", Vector) = (0, 0, 0, 0)
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
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
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#define UNITY_SPRITES_INCLUDED

			#include "UnityCG.cginc"

			#define RESTRICT_SHADER_MODEL_3
			#define PS_NOT_DISCARD
			#define	PS_NOT_CLAMP_COLOR
			#include "Base/Shader_Data_UnityNative.cginc"
			#include "Base/ShaderVertex_Sprite_UnityNative.cginc"
			#include "Base/ShaderPixel_Sprite_UnityNative.cginc"
			ENDCG
		}
	}
	FallBack Off
}
