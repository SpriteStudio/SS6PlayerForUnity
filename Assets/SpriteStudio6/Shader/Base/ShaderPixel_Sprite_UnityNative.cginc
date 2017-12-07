//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
sampler2D _MainTex;
sampler2D _AlphaTex;

#ifdef SV_Target
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

	fixed4 color[4];
	float pixelA = pixel.a;
	float rate = input.ColorOverlay.a;
	float rateInverse = 1.0f - rate;
	color[0] = (pixel * rateInverse) + (input.ColorOverlay * rate);	/* Mix */
	color[1] = pixel + (input.ColorOverlay * rate);	/* Add */
	color[2] = pixel - (input.ColorOverlay * rate);	/* Subtract */
	color[3] = (pixel * rateInverse) + ((pixel * input.ColorOverlay) * rate);	/* Multiple */

	pixel = color[input.Texture00UV.z];
	pixel.a = pixelA;
#if !defined(PS_NOT_CLAMP_COLOR)
	pixel = saturate(pixel);
#endif
	output = pixel;

	return(output);
}
