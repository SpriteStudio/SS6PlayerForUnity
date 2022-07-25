//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//

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

	pixel = color[(int)input.Texture00UV.z];
#endif

#if defined(UNITY_UI_CLIP_RECT)
	half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(input.mask.xy)) * input.mask.zw);
	pixelA *= m.x * m.y;
#endif

	pixel.a = pixelA;
	pixel *= input.ColorMain;

	if(0.5f > input.Texture00UV.w)
	{	/* Blend-Target : MIX */
#if defined(UNITY_UI_ALPHACLIP)
		pixel.a -= 0.001;
#endif
		if(0.0f >= pixel.a)
		{
			discard;
		}
		pixel.rgb *= pixel.a;
	}
	else
	{	/* Blend-Target : ADD */
		/* MEMO: Alpha is reflected to RGB. (When drawing, always 0.) */
		pixel.rgb *= pixel.a;
		pixel.a = 0.0;
	}

	output = pixel;

	return(output);
}
