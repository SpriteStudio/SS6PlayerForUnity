//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//
sampler2D _MainTex;
sampler2D _AlphaTex;
// float _EnableExternalAlpha;

#if defined(SV_Target)
fixed4 PS_main(InputPS input) : SV_Target
#else
fixed4 PS_main(InputPS input) : COLOR0
#endif
{
	fixed4 output;

	float2 uv = input.Texture00UV.xy;
	fixed4 pixel = tex2D(_MainTex, uv);
#if defined(ETC1_EXTERNAL_ALPHA)
	fixed4 alpha = tex2D(_AlphaTex, uv);
	pixel.a = lerp(pixel.a, alpha.r, _EnableExternalAlpha);
#endif

	pixel *= input.ColorMain;
#if !defined(PS_NOT_DISCARD)
	if(0.0f >= pixel.a)
	{
		discard;
	}
#endif
	float pixelA = pixel.a;

#if defined(RESTRICT_SHADER_MODEL_3)
	fixed4	colorOverlay = input.ColorOverlay;
	float	colorOverlayA = colorOverlay.a;
	fixed4	overlayParameter = input.ParameterOverlay;
	fixed4	pixelCoefficientColorOvelay = (fixed4(1.0f, 1.0f, 1.0f, 1.0f) * (1.0f - overlayParameter.z)) + (pixel * overlayParameter.z);
	colorOverlay *= colorOverlayA;

	pixel = (pixel * (1.0f - (colorOverlayA * overlayParameter.y))) + (pixelCoefficientColorOvelay * colorOverlay * overlayParameter.w);
#else
	fixed4 color[4];
	float rate = input.ColorOverlay.a;
	float rateInverse = 1.0f - rate;
	color[0] = (pixel * rateInverse) + (input.ColorOverlay * rate);	/* Mix */
	color[1] = pixel + (input.ColorOverlay * rate);	/* Add */
	color[2] = pixel - (input.ColorOverlay * rate);	/* Subtract */
	color[3] = (pixel * rateInverse) + ((pixel * input.ColorOverlay) * rate);	/* Multiple */

	pixel = color[input.Texture00UV.z];
#endif

#if defined(PS_OUTPUT_PMA)
	pixel.xyz *= pixelA;
#endif
	pixel.a = pixelA;

	output = pixel;

	return(output);
}
