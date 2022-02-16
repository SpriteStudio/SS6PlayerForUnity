//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//
sampler2D _MainTex;
sampler2D _AlphaTex;
float _EnableExternalAlpha;

#define ParameterGetDiscard(_brightness_,_color_)	\
	abs(step(_brightness_, 0.0f) - max(_color_.r, max(_color_.g, _color_.b)))

fixed4 PS_main(InputPS input) : PIXELSHADER_BINDOUTPUT
{
	fixed4 output;

	float fBrightness = params0;

	if(A_TW <= 0.0f)	{
		/* Check Discarding-Pixel */
		float b = ParameterGetDiscard(fBrightness, input.ColorMain);
		PixelDiscardAlpha(b, abs(fBrightness));

		output = input.ColorMain;
		return(output);
	}

	/* Texel Sampling */
	fixed4 pixel = tex2D(_MainTex, input.Texture00UV.xy);
	PixelSynthesizeExternalAlpha(pixel.a, _AlphaTex, coord.xy, _EnableExternalAlpha);
	PixelSolvePMA(pixel, pixel.a);

	/* Check Discarding-Pixel */
	float b = ParameterGetDiscard(fBrightness, pixel);
	PixelDiscardAlpha(b, abs(fBrightness));

	/* Blending "Parts-Color" */
	/* MEMO: Need to run "PixelSynthesizePartsColor" to synthesize "Parts-Color". */
	float pixelA = pixel.a;
	PixelSynthesizePartsColor(pixel, input);
	pixel.a = pixelA;

	/* Finalize */
	output = pixel;
	return(output);
}
