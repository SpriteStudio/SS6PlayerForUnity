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

float4 PS_main(InputPS input) : PIXELSHADER_BINDOUTPUT
{
	float4 output;

	/* Texel Sampling */
	/* MEMO: Run "PixelSynthesizeExternalAlpha", especially if you want to support ETC1's split-alpha. */
	float4 pixel = tex2D(_MainTex, input.Texture00UV.xy);
	pixel = PixelSolveColorspaceInput(pixel);

	PixelSynthesizeExternalAlpha(pixel.a, _AlphaTex, input.Texture00UV.xy, _EnableExternalAlpha);

	/* MEMO: No "Part-Color" is applied to "Effect". */

	/* Blending Vertex-Color & Check Discarding-Pixel */
	/* MEMO: Once pixel's alpha has been determined, Need to run "PixelDiscardAlpha". */
	pixel *= input.ColorMain;

	/* Finalize color */
	PixelSolvePMA(pixel, pixel.a);
	PixelDiscardAlpha(pixel.a, 0.0f);
	pixel = PixelSolveColorspaceOutput(pixel);

	output = pixel;

	return(output);
}
