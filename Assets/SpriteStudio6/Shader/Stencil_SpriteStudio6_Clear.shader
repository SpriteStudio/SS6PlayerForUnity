//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
Shader "Custom/SpriteStudio6/SS6PU/Stencil/Clear"
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
				Ref 0
				Comp Always
				Pass Replace
			}
			ColorMask 0

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#include "UnityCG.cginc"

//			#define RESTRICT_SHADER_MODEL_3
			#include "Base/Shader_Data_SpriteStudio6.cginc"

			InputPS VS_main(InputVS input)
			{
				InputPS output;

				output.Texture00UV = input.texcoord1.x;

				output.ColorMain = input.color;

				output.ColorOverlay = input.color;

				output.PositionDraw = input.vertex;
				output.Position = input.vertex;

				return(output);
			}

			#ifdef SV_Target
			fixed4 PS_main(InputPS input) : SV_Target
			#else
			fixed4 PS_main(InputPS input) : COLOR0
			#endif
			{
				fixed4 output = 0;
				return(output);
			}
			ENDCG
		}
	}
		FallBack Off
}
