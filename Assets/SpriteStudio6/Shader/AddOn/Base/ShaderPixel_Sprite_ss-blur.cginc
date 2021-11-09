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

#define Distance 16.0f

fixed4 PS_main(InputPS input) : PIXELSHADER_BINDOUTPUT
{
	fixed4 output;

	if(A_TW == 0.0f)	{
		output = input.ColorMain;
		return(output);
	}

	/* Texel Sampling */
	float pix_w = 1.0f / A_TW;
	float pix_h = 1.0f / A_TH;

	/* MEMO: Since "params0" is between -1.0 and 1.0, only mixes up neighbor pixels at most. */
	/*       So, multiply moderately to sample distant pixels.                               */
	float defocused_x = params0 * pix_w * Distance;
	float defocused_y = params0 * pix_h * Distance;

	float2 tc = input.Texture00UV.xy;
	float4 out_color = tex2D(_MainTex, tc);
	out_color += tex2D(_MainTex, float2(tc.x + defocused_x,	tc.y + defocused_y	));
	out_color += tex2D(_MainTex, float2(tc.x + defocused_x,	tc.y				));
	out_color += tex2D(_MainTex, float2(tc.x,				tc.y + defocused_y	));
	out_color += tex2D(_MainTex, float2(tc.x - defocused_x,	tc.y - defocused_y	));
	out_color += tex2D(_MainTex, float2(tc.x + defocused_x,	tc.y - defocused_y	));
	out_color += tex2D(_MainTex, float2(tc.x - defocused_x,	tc.y + defocused_y	));
	out_color += tex2D(_MainTex, float2(tc.x - defocused_x,	tc.y				));
	out_color += tex2D(_MainTex, float2(tc.x,				tc.y - defocused_y	));
	out_color /= 9.0f;	/* Number of Sampling-point is 9 */

	/* Blending Vertex-Color & Check Discarding-Pixel */
	out_color *= input.ColorMain;
	PixelDiscardAlpha(out_color.a, 0.0f);

	/* PreMultiplied-Alpha Solving */
	PixelSolvePMA(out_color, out_color.a);

	/* Finalize */
	output = out_color;
	return(output);
}
