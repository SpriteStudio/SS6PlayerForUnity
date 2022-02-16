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

	float fPower = params0;

	if(A_TW <= 0.0f)	{
		output = input.ColorMain;
		return(output);
	}

	/* Texel Sampling */
	float e = 1.0e-10f;
	float w = A_TW + e;
	float h = A_TH + e;
	float v = 1.0f + 96.0f * fPower;
//	float4	coord = float4((floor(input.Texture00UV.x * w / v) * v / w), (floor(input.Texture00UV.y * h / v) * v / h), 0.0f, 0.0f);
	float4	coord = float4((floor(input.Texture00UV.x * w / v) * v / w), (ceil(input.Texture00UV.y * h / v) * v / h), 0.0f, 0.0f);

	/* MEMO: Run "PixelSynthesizeExternalAlpha", especially if you want to support ETC1's split-alpha. */
	fixed4 pixel = tex2D(_MainTex, coord.xy);
	PixelSynthesizeExternalAlpha(pixel.a, _AlphaTex, coord.xy, _EnableExternalAlpha);
	PixelSolvePMA(pixel, pixel.a);

	/* Blending Vertex-Color & Check Discarding-Pixel */
	/* MEMO: Once pixel's alpha has been determined, Need to run "PixelDiscardAlpha". */
	pixel *= input.ColorMain;
	PixelDiscardAlpha(pixel.a, 0.0f);

	/* Blending "Parts-Color" */
	/* MEMO: Need to run "PixelSynthesizePartsColor" to synthesize "Parts-Color". */
	float pixelA = pixel.a;
	PixelSynthesizePartsColor(pixel, input);
	pixel.a = pixelA;

	/* Finalize */
	output = pixel;
	return(output);
}
