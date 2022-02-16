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

float4 toHSB(float4 color)
{
	float4 K = float4(0.0f, -1.0f / 3.0f, 2.0f / 3.0f, -1.0f);
	float4 p = lerp(float4(color.bg, K.wz), float4(color.gb, K.xy), step(color.b, color.g));
	float4 q = lerp(float4(p.xyw, color.r), float4(color.r, p.yzx), step(p.x, color.r));

	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10f;

	return(float4(abs(q.z + (q.w - q.y) / (6.0f * d + e)), d / (q.x + e), q.x, color.a));
}

float4 toRGB(float4 hsb)
{
	float4 K = float4(1.0f, 2.0f / 3.0f, 1.0f / 3.0f, 3.0f);
	float3 p = abs(frac(hsb.xxx + K.xyz) * 6.0f - K.www);

	return(float4(hsb.z * lerp(K.xxx, clamp(p - K.xxx, 0.0f, 1.0f), hsb.y), hsb.w));
}

float4 shiftHSB(float4 hsb, float fRatioH, float fRatioS, float fRatioB)
{
	float4 shift = hsb;

	shift.x += fRatioH;
	shift.y = clamp(hsb.y + fRatioS, 0.0f, 1.0f);
	shift.z = clamp(hsb.z + fRatioB, 0.0f, 1.0f);

	if(shift.x < 0.0f)	{
		shift.x += 1.0f;
	}
	if(shift.x > 1.0)	{
		shift.x -= 1.0f;
	}

	return(shift);
}

float4 adjustHSB(float4 color, float fRatioH, float fRatioS, float fRatioB)
{
	float4 hsb;

	hsb = toHSB(color);
	hsb = shiftHSB(hsb, fRatioH, fRatioS, fRatioB);

	return(toRGB(hsb));
}

fixed4 PS_main(InputPS input) : PIXELSHADER_BINDOUTPUT
{
	fixed4 output;

	float	fHue = params0;
	float	fSaturation = params1;
	float	fBrightness = params2;

	if(A_TW <= 0.0f)	{
		output = adjustHSB(input.ColorMain, fHue, fSaturation, fBrightness);
		return(output);
	}

	/* Texel Sampling */
	fixed4 pixel = tex2D(_MainTex, input.Texture00UV.xy);
	PixelSynthesizeExternalAlpha(pixel.a, _AlphaTex, input.Texture00UV.xy, _EnableExternalAlpha);
	PixelSolvePMA(pixel, pixel.a);

	/* Check Discarding-Pixel */
	/* MEMO: Once pixel's alpha has been determined, Need to run "PixelDiscardAlpha". */
//	PixelDiscardAlpha((pixel.a * input.ColorMain.a), 0.0f);

	/* Color Shift */
	pixel = adjustHSB(pixel, fHue, fSaturation, fBrightness);

	/* Blending Vertex-Color */
	pixel *= input.ColorMain;

	/* Blending "Parts-Color" */
	/* MEMO: Need to run "PixelSynthesizePartsColor" to synthesize "Parts-Color". */
	float pixelA = pixel.a;
	PixelSynthesizePartsColor(pixel, input);
	pixel.a = pixelA;

	/* Finalize */
	output = pixel;
	return(output);
}
