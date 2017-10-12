//
//	SpriteStudio5 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
Shader "Custom/SpriteStudio6/Effect/AddPA"	{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

	SubShader{
		Tags{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass{
			// MEMO: Blend "Add", "Straight-Alpha"
			Lighting Off
			Fog{ Mode off }

			Cull Off
			ZTest LEqual
			ZWRITE Off

			Blend SrcAlpha One

			CGPROGRAM
#pragma vertex VS_main
#pragma fragment PS_main

#include "UnityCG.cginc"

#include "Base/ShaderVertex_Effect_SpriteStudio6.cginc"

#include "Base/ShaderPixel_Effect_SpriteStudio6.cginc"
			ENDCG

			SetTexture[_MainTex]{
				Combine Texture, Texture
			}
		}
	}
	FallBack Off
}
