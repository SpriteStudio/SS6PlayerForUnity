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

float rand(float2 seed)
{
	/* MEMO: "return fract(sin(dot(p, vec2(12.9898, 78.233))) * 43758.5453);"                                        */
	/*       This formula which is often used, does not work on properly some GPUs, so replaced with an approximate. */
	/*       See follows for details... https://gist.github.com/johansten/3633917                                    */
	float a = frac(dot(seed.xy, float2(2.067390879775102f, 12.451168662908249f))) - 0.5f;
	float aa = a * a;
	float s = a * (6.182785114200511f + aa * (-38.026512460676566f + (aa * 53.392573080032137f)));
	return(frac(s * 43758.5453f));
}

fixed4 PS_main(InputPS input) : PIXELSHADER_BINDOUTPUT
{
	fixed4 output;

	float fPower = params0;
	float fRatio = params1;
	float fPhase = params2;

	/* Texel Sampling */
	if(A_TW <= 0.0f)	{
		output = input.ColorMain;
		return(output);
	}

	float PI = 3.14159265358979f;
	float2 Coord;
	float2 u;

	Coord = input.Texture00UV.xy;
	u = float2(floor(Coord.x * A_TW) / A_TW, floor(Coord.y * A_TH) / A_TH);

	float2 t = u + float2(fPhase, fPhase);
	float r = rand(t) * PI * 2.0 - PI;
	float r2 = step(rand(t * r), fRatio);

	float2 v = float2(sin(r), cos(r)) * float2(A_U1, A_V1) * 96.0f * fPower * r2;
	Coord += v;

	/* MEMO: Run "PixelSynthesizeExternalAlpha", especially if you want to support ETC1's split-alpha. */
	float4 Pixel = tex2D(_MainTex, Coord.xy);
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
