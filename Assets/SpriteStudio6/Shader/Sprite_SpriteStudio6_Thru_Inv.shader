//
//	SpriteStudio5 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
Shader "Custom/SpriteStudio6/SS6PU/Sprite/Through/Inverse"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
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
				Ref 1
				Comp Always
				Pass Keep
			}

			Blend OneMinusDstColor Zero

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

			#include "UnityCG.cginc"

			#define PS_NOT_DISCARD
			#include "Base/Shader_Data_SpriteStudio6.cginc"
			#include "Base/ShaderVertex_Sprite_SpriteStudio6.cginc"
			#include "Base/ShaderPixel_Sprite_SpriteStudio6.cginc"
			ENDCG
		}
	}
	FallBack Off
}
