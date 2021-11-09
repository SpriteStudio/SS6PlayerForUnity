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

float toOutlineValue(float2 p, float2 v, float fRatio)
{
	float4 Pixel;
	float lo;
	float hi;
	float e = 1.0e-5f;
	float2 Coord;

	lo = 0.0f;

	Coord = p + float2(-v.x, 0.0f);
	Pixel = tex2D(_MainTex, Coord);
	PixelSynthesizeExternalAlpha(Pixel.a, _AlphaTex, Coord, _EnableExternalAlpha);
	lo += step(Pixel.a, fRatio);

	Coord = p + float2(+v.x, 0.0f);
	Pixel = tex2D(_MainTex, Coord);
	PixelSynthesizeExternalAlpha(Pixel.a, _AlphaTex, Coord, _EnableExternalAlpha);
	lo += step(Pixel.a, fRatio);

	Coord = p + float2(0.0f, -v.y);
	Pixel = tex2D(_MainTex, Coord);
	PixelSynthesizeExternalAlpha(Pixel.a, _AlphaTex, Coord, _EnableExternalAlpha);
	lo += step(Pixel.a, fRatio);

	Coord = p + float2(0.0f, +v.y);
	Pixel = tex2D(_MainTex, Coord);
	PixelSynthesizeExternalAlpha(Pixel.a, _AlphaTex, Coord, _EnableExternalAlpha);
	lo += step(Pixel.a, fRatio);

	Pixel = tex2D(_MainTex, p);
	PixelSynthesizeExternalAlpha(Pixel.a, _AlphaTex, p, _EnableExternalAlpha);
	hi = step(fRatio + e, Pixel.a);

	return(min(hi * lo, 1.0f));
}

fixed4 PS_main(InputPS input) : PIXELSHADER_BINDOUTPUT
{
	fixed4 output;

	float fThreshold = params0;

	if(A_TW <= 0.0f)	{
		output = input.ColorMain;
		return(output);
	}

	/* Texel Sampling & Extract edge */
	float2 Coord;
	float fPixW;
	float fPixH;

	Coord = input.Texture00UV.xy;

	fPixW = A_U1;
	fPixH = A_V1;

	float v = toOutlineValue(Coord, float2(fPixW, fPixH), abs(fThreshold));
	fixed4 pixel = fixed4(v, v, v, v);

	/* Blending Vertex-Color & Check Discarding-Pixel */
	/* MEMO: Once pixel's alpha has been determined, Need to run "PixelDiscardAlpha". */
	pixel *= input.ColorMain;
	PixelDiscardAlpha(v, 0.0f);

	/* Blending "Parts-Color" */
	/* MEMO: Need to run "PixelSynthesizePartsColor" to synthesize "Parts-Color". */
	float pixelA = pixel.a;
	PixelSynthesizePartsColor(pixel, input);
	pixel.a = pixelA;

	/* Finalize */
	output = pixel;
	return(output);
}
