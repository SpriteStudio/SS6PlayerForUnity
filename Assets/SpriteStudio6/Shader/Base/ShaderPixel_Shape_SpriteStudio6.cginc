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
	/* MEMO: "Shape" part decode no textures. */
//	float4 pixel = float4(0.0f, 0.0f, 0.0f, 1.0f);

	/* Blending "Parts-Color" */
	/* MEMO: In case “Shape” parts, "Parts-Color" is used for output-color. */
//	PixelSynthesizePartsColor(pixel, input);
	float4 pixel = input.ColorOverlay;

	/* Blending Vertex-Color & Check Discarding-Pixel */
	/* MEMO: Once pixel's alpha has been determined, Need to run "PixelDiscardAlpha". */
	pixel *= input.ColorMain;
	PixelDiscardAlpha(pixel.a, 0.0f);

	/* Finalize color */
	PixelSolvePMA(pixel, pixel.a);
	pixel = PixelSolveColorspaceOutput(pixel);

	output = pixel;

	return(output);
}
