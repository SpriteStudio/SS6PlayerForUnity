//
//	SpriteStudio5 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
Shader "Custom/SpriteStudio6/SS6PU/Stencil/Clear"
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
				Ref 0
				Comp Always
				Pass Replace
			}
			ColorMask 0

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

			#include "UnityCG.cginc"

			#include "Base/Shader_Data_SpriteStudio6.cginc"
//			#include "Base/ShaderVertex_Sprite_SpriteStudio6.cginc"
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

//			#include "Base/ShaderPixel_Sprite_SpriteStudio6.cginc"
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
