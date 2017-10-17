//
//	SpriteStudio5 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
Shader "Custom/SpriteStudio6/SS6PU/Sprite/Inverse"
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

			Blend OneMinusDstColor Zero

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

			#include "UnityCG.cginc"

			#include "Base/ShaderVertex_Sprite_SpriteStudio6.cginc"
			#include "Base/ShaderPixel_Sprite_SpriteStudio6.cginc"
			ENDCG
		}
	}
	FallBack Off
}
