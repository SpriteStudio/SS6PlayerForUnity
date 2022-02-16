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

	float	fPower = params0;
	float	fDecay = params1;
	float	fColor = params2;

	if(A_TW <= 0.0f)	{
		output = input.ColorMain;
		return(output);
	}

	/* Texel Sampling */
	float e = 1.0e-10f;
	float p = abs(fPower);
	float d = abs(fDecay);
	float2 uv1 = float2(A_U1 + e, A_V1 + e);
	float2 c = float2(A_CU, A_CV) / uv1;
	float2 rb = float2(A_RU, A_BV) / uv1;
	float l = min(abs(rb.x - c.x), abs(rb.y - c.y)) + e;
	float2 t = input.Texture00UV.xy / uv1;
	float2 v = c - t;

	float fR = max(p / (length(v / l) + e) - p, 0.0f);
	float fL = max(p - length(v / l), 0.0f);

	fR = clamp(lerp(fR, fL, d), 0.0f, 1.0f);

	/* MEMO: Run "PixelSynthesizeExternalAlpha", especially if you want to support ETC1's split-alpha. */
	float4 pixel = tex2D(_MainTex,  input.Texture00UV.xy);
	PixelSynthesizeExternalAlpha(pixel.a, _AlphaTex, input.Texture00UV.xy, _EnableExternalAlpha);
	PixelSolvePMA(pixel, pixel.a);

	/* Calculate Spot */
	pixel.rgb = lerp(float3(1.0f, 1.0f, 1.0f), pixel.rgb, fColor);
	pixel.a = fR;

	/* Blending Vertex-Color & Check Discarding-Pixel */
	/* MEMO: Once pixel's alpha has been determined, Need to run "PixelDiscardAlpha". */
	pixel *= input.ColorMain;
//	PixelDiscardAlpha(pixel.a, 0.0f);

	/* Blending "Parts-Color" */
	/* MEMO: Need to run "PixelSynthesizePartsColor" to synthesize "Parts-Color". */
	float pixelA = pixel.a;
	PixelSynthesizePartsColor(pixel, input);
	pixel.a = pixelA;

	/* Finalize */
	output = pixel;
	return(output);
}
