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

	float fDistance = params0;
	float fDirection = params1;
	float fPower = params2;

	if(A_TW <= 0.0f)	{
		output = input.ColorMain;
		return(output);
	}

	/* Texel Sampling */
	float2 Coord;
	float fPixW;
	float fPixH;
	int iCount;
	float2 Vel;
	float4 Pixel;
	float PI = 3.14159265358979f;

	Coord = input.Texture00UV.xy;

	fPixW = A_U1;
	fPixH = A_V1;

	iCount = int(floor(abs(fDistance) * 96.0f));

//	Vel = float2(sin(fDirection * PI) * fPixW, cos(fDirection * PI) * fPixH) * sign(fDistance);
	Vel = float2(sin(fDirection * PI) * fPixW, -cos(fDirection * PI) * fPixH) * sign(fDistance);

	Coord += Vel * iCount;

	Coord -= Vel;
	Pixel = tex2D(_MainTex, Coord);
	PixelSynthesizeExternalAlpha(Pixel.a, _AlphaTex, Coord.xy, _EnableExternalAlpha);
	PixelSolvePMA(Pixel, Pixel.a);

	float4 PixelShift;
	for(int i=1; i<iCount; i++)	{
		Coord -= Vel;
		PixelShift = tex2D(_MainTex, Coord);
		PixelSynthesizeExternalAlpha(PixelShift.a, _AlphaTex, Coord.xy, _EnableExternalAlpha);
		PixelSolvePMA(PixelShift, PixelShift.a);

		Pixel = lerp(PixelShift, Pixel, 0.96f * abs(fPower));
	}

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
