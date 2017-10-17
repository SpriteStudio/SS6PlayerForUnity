//
//	SpriteStudio5 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
Shader "Custom/SpriteStudio6/SS6PU/Sprite/Through/Multiple"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
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

			Blend Zero SrcColor

			CGPROGRAM
			#pragma vertex VS_main
			#pragma fragment PS_main

			#include "UnityCG.cginc"

			#include "Base/Shader_Data_SpriteStudio6.cginc"
			#include "Base/ShaderVertex_Sprite_SpriteStudio6.cginc"
//			#include "Base/ShaderPixel_Sprite_SpriteStudio6.cginc"
			sampler2D _MainTex;

#ifdef SV_Target
			fixed4 PS_main(InputPS input) : SV_Target
#else
			fixed4 PS_main(InputPS input) : COLOR0
#endif
			{
				fixed4 output;

				fixed4 pixel = tex2D(_MainTex, input.Texture00UV.xy);
				pixel *= input.ColorMain;
				if(0.0f >= pixel.a)
				{
					discard;
				}

				fixed4 color[4];
				float pixelA = pixel.a;
				float rate = input.ColorOverlay.a;
				float rateInverse = 1.0f - rate;
				color[0] = (pixel * rateInverse) + (input.ColorOverlay * rate);	/* Mix */
				color[1] = (pixel * rateInverse) + ((pixel * input.ColorOverlay) * rate);	/* Multiple */
				color[2] = pixel + (input.ColorOverlay * rate);	/* Add */
				color[3] = pixel - (input.ColorOverlay * rate);	/* Subtract */

				pixel = color[input.Texture00UV.z];
				pixel *= pixelA;											/* Blend-Multiple Only. */
				pixel += fixed4(1.0f, 1.0f, 1.0f, 1.0f) * (1.0 - pixelA);	/* Blend-Multiple Only. */
				pixel.a = 1.0f;												/* Blend-Multiple Only. */
				output = pixel;

				return(output);
			}
			ENDCG
		}
	}
	FallBack Off
}
