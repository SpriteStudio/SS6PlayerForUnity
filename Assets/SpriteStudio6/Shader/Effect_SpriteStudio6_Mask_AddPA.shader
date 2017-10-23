//
//	SpriteStudio5 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
Shader "Custom/SpriteStudio6/SS6PU/Effect/Mask/AddPA"
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
			// MEMO: Blend "AddPA", "PreMultiplied-Alpha"
			Cull Off
			ZTest LEqual
			ZWRITE Off
			Stencil
			{
				Ref 1
				ReadMask 1
				Comp Greater
				Pass Keep
			}

			Blend SrcAlpha One

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

			#include "UnityCG.cginc"

			#include "Base/Shader_Data_SpriteStudio6.cginc"
			#include "Base/ShaderVertex_Effect_SpriteStudio6.cginc"
//			#include "Base/ShaderPixel_Effect_SpriteStudio6.cginc"
			sampler2D	_MainTex;

#ifdef SV_Target
			fixed4 PS_main(InputPS Input) : SV_Target
#else
			fixed4 PS_main(InputPS Input) : COLOR0
#endif
			{
				fixed4 output;

				fixed4	pixel = tex2D(_MainTex, Input.Texture00UV.xy);
				pixel *= Input.ColorMain;
				pixel *= pixel.a;
				output = pixel;

				return(output);
			}
			ENDCG
		}
	}
		FallBack Off
}
