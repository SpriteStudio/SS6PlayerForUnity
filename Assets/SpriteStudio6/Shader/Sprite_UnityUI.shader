//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//

// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
// -------------------------------------------- Copy of license.txt (Unity 2019.4.31 Builtin shaders)
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

Shader "Custom/SpriteStudio6/UnityUI/Sprite"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
//		[PerRendererData] _AlphaTex("Sprite Alpha Texture", 2D) = "white" {}
//		[PerRendererData] _EnableExternalAlpha("Enable Sprite Alpha Texture", Float) = 0
		_Color("Tint", Color) = (1, 1, 1, 1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Pass
		{
			Name "Default"

			Cull Off
			Lighting Off
			ZTest [unity_GUIZTestMode]
			ZWrite Off
			BlendOp Add
			Blend One OneMinusSrcAlpha
			ColorMask [_ColorMask]

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

			#pragma target 2.0
			#pragma multi_compile_instancing
//			#pragma multi_compile _ PIXELSNAP_ON
//			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

			#define UNITY_SPRITES_INCLUDED
			#include "UnityCG.cginc"

//			#define RESTRICT_SHADER_MODEL_3
//			#define RESTRICT_UNITY_2017_2
			#include "Base/Shader_Data_UnityUI.cginc"
			#include "Base/ShaderVertex_Sprite_UnityUI.cginc"
			#include "Base/ShaderPixel_Sprite_UnityUI.cginc"
			ENDCG
		}
	}
}
