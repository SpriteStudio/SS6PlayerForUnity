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

	float fWidth = params0;
	float fHeight = params1;
	float fPhase = params2;

	if(A_TW <= 0.0f)	{
		output = input.ColorMain;
		return(output);
	}

	/* Texel Sampling */
	float2 Coord;
	float fPixW;
//	float fPixH;
	float4 Pixel;
	float l = 384.0f * fHeight * 0.5f;
	float s;
	float PI = 3.14159265358979f;

	Coord = input.Texture00UV.xy;

	fPixW = A_U1;
//	fPixH = A_V1;

//	s = l * sin(Coord.y * 384.0f * fWidth * 0.5f + PI * 2.0f * fPhase ) * fPixW;
	s = l * sin((1.0f - Coord.y) * 384.0f * fWidth * 0.5f + PI * 2.0f * fPhase ) * fPixW;
	Coord.x += s;

	/* MEMO: Run "PixelSynthesizeExternalAlpha", especially if you want to support ETC1's split-alpha. */
	Pixel = tex2D(_MainTex, Coord.xy);
	PixelSynthesizeExternalAlpha(Pixel.a, _AlphaTex, Coord.xy, _EnableExternalAlpha);
	PixelSolvePMA(Pixel, Pixel.a);

	/* Blending Vertex-Color & Check Discarding-Pixel */
	/* MEMO: Once pixel's alpha has been determined, Need to run "PixelDiscardAlpha". */
	Pixel *= input.ColorMain;
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
