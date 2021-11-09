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

fixed4 PS_main(InputPS input) : PIXELSHADER_BINDOUTPUT
{
	fixed4 output;

	float fThreshold = params0;
	float fStage = params1;
	float fColor = params2;

	if(A_TW <= 0.0f)	{
		output = input.ColorMain;
		return(output);
	}

	/* Texel Sampling */
	/* MEMO: Run "PixelSynthesizeExternalAlpha", especially if you want to support ETC1's split-alpha. */
	float4 Pixel = tex2D(_MainTex, input.Texture00UV.xy);
	PixelSynthesizeExternalAlpha(Pixel.a, _AlphaTex, input.Texture00UV.xy, _EnableExternalAlpha);
	PixelSolvePMA(Pixel, Pixel.a);

	float d = 1.0f + 255.0f * abs(fStage);
	float e = 1.0e-10f;
	float t = abs(fThreshold);
	float b = abs(step(fThreshold, 0.0) - max(Pixel.r, max(Pixel.g, Pixel.b)));
	float r = clamp(floor((b - t) / (t + e) * 256.0f / d ) * d / 256.0f, 0.0f, 1.0f);
	float c = step(t, b) * r;
	float4 v = float4(c, c, c, Pixel.a);
	Pixel.rgb = lerp(v.rgb, Pixel.rgb * c, fColor);

	/* Blending Vertex-Color & Check Discarding-Pixel */
	/* MEMO: Once pixel's alpha has been determined, Need to run "PixelDiscardAlpha". */
	Pixel *= input.ColorMain;
//	PixelDiscardAlpha(Pixel.a, 0.0f);

	/* Blending "Parts-Color" */
	/* MEMO: Need to run "PixelSynthesizePartsColor" to synthesize "Parts-Color". */
	float pixelA = Pixel.a;
	PixelSynthesizePartsColor(Pixel, input);
	Pixel.a = pixelA;

	/* Finalize */
	output = Pixel;
	return(output);
}
