//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
Shader "Custom/SpriteStudio6/SS6PU/Stencil/PreDraw"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
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
				Pass IncrWrap
			}
			ColorMask 0

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#include "UnityCG.cginc"

//			#define RESTRICT_SHADER_MODEL_3
//			#define PS_NOT_DISCARD
			#define	PS_NOT_CLAMP_COLOR
			#include "Base/Shader_Data_SpriteStudio6.cginc"
			#include "Base/ShaderVertex_Sprite_SpriteStudio6.cginc"

			sampler2D _MainTex;

#if defined(SV_Target)
			fixed4 PS_main(InputPS input) : SV_Target
#else
			fixed4 PS_main(InputPS input) : COLOR0
#endif
			{
				fixed4 output = 0;
				fixed4 pixel = tex2D(_MainTex, input.Texture00UV.xy);
				if (input.ColorMain.a >= pixel.a)
				{
					discard;
				}

				return(output);
			}
			ENDCG
		}
	}
	FallBack Off
}
