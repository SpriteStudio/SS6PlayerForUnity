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

float toGrayValue(float4 color)
{
	float fRatioR = 0.298912f;
	float fRatioG = 0.586611f;
	float fRatioB = 0.114478f;
	float3	grayScale = float3(fRatioR, fRatioG, fRatioB);

	return(dot(color.rgb, grayScale));
}

float4 toSepiaColor(float4 color)
{
	float fRatioR = 1.07f;
	float fRatioG = 0.74f;
	float fRatioB = 0.43f;
	float3 sepiaScale = float3(fRatioR, fRatioG, fRatioB);

	return(float4((color.rgb * sepiaScale), color.a));
}

fixed4 PS_main(InputPS input) : PIXELSHADER_BINDOUTPUT
{
	fixed4 output;

	float fPower = params0;

	if(A_TW <= 0.0f)	{
		output = input.ColorMain;
		return(output);
	}

	/* Texel Sampling */
	float4 Pixel;
	float4 Gray;
	float4 Sepia;

	/* MEMO: Run "PixelSynthesizeExternalAlpha", especially if you want to support ETC1's split-alpha. */
	Pixel = tex2D(_MainTex, input.Texture00UV.xy);
	PixelSynthesizeExternalAlpha(Pixel.a, _AlphaTex, input.Texture00UV.xy, _EnableExternalAlpha);
	PixelSolvePMA(Pixel, Pixel.a);

	/* Check Discarding-Pixel */
	/* MEMO: Once pixel's alpha has been determined, Need to run "PixelDiscardAlpha". */
	PixelDiscardAlpha(Pixel.a, 0.0f);

	/* Color Calculate */
	float Luminous = toGrayValue(Pixel);
	Gray = float4(Luminous, Luminous, Luminous, Pixel.a);
	Sepia = lerp(toSepiaColor(Gray), Gray, step(fPower, 0.0f));

	Pixel = lerp(Pixel, Sepia, abs(fPower));

	/* Blending Vertex-Color */
	PixelDiscardAlpha(Pixel.a, 0.0f);

	/* Blending "Parts-Color" */
	/* MEMO: Need to run "PixelSynthesizePartsColor" to synthesize "Parts-Color". */
	float pixelA = Pixel.a;
	PixelSynthesizePartsColor(Pixel, input);
	Pixel.a = pixelA;

	/* Finalize */
	output = Pixel;
	return(output);
}
