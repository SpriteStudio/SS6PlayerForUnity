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

//	float fPhase = params0;	/* for SS6 */
	float fPhase = -params0;	/* for Unity */
	float fDirection = params1;

	if (A_TW <= 0.0f)	{
		output = input.ColorMain;
		return(output);
	}

	/* Texel Sampling */
	float PI = 3.14159265358979f;
	float e = 1.0e-10f;
	float2 uv1 = float2(A_U1 + e, A_V1 + e);
	float2 tx = input.Texture00UV.xy / uv1;
	float2 c = float2(A_CU, A_CV) / uv1;
	float2 lt = float2(A_LU, A_TV) / uv1;
	float2 rb = float2(A_RU, A_BV) / uv1;
	float2 d = rb - lt;
	float2 v = tx - c;
	float r = min(abs(rb.x - c.x), abs(rb.y - c.y)) + e;
	float dis = length(v);

	if(dis > r)	{
		discard;
	}

	float2 nv = normalize(v);
	float uu = 1.0f - dis / r;
	float vv = (atan2(nv.y, nv.x) / PI + 1.0f) * 0.5f + fPhase;

	if(vv < 0.0f)	{
		vv += 1.0f;
	}
	if(vv > 1.0f)	{
		vv -= 1.0f;
	}

	if(fDirection <= 0.0f)	{
		vv = 1.0f - vv;
	}

	float2 st = d * float2(uu, vv) * uv1;
	float2 texUV = float2(A_LU, A_TV) + st;
	fixed4 pixel = tex2D(_MainTex, texUV);
	PixelSynthesizeExternalAlpha(pixel.a, _AlphaTex, texUV.xy, _EnableExternalAlpha);
	PixelSolvePMA(pixel, pixel.a);

	/* Blending "Parts-Color" */
	float pixelA = pixel.a;
	PixelSynthesizePartsColor(pixel, input);
	pixel.a = pixelA;

	/* Finalize */
	output = pixel;
	return(output);
}
