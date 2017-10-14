//
//	SpriteStudio5 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
sampler2D _MainTex;

#ifdef SV_Target
fixed4 PS_main(InputPS input) : SV_Target
#else
fixed4 PS_main(InputPS input) : COLOR0
#endif
{
	fixed4 output;

	fixed4 pixel = tex2D(_MainTex, input.Texture00UV.xy);
	pixel *= input.ColorMain;

	fixed4 color[4];
	float pixelA = pixel.a;
	float rate = input.ColorOverlay.a;
	float rateInverse = 1.0f - rate;
	color[0] = (pixel * rateInverse) + (input.ColorOverlay * rate);	/* Mix */
	color[1] = (pixel * rateInverse) + ((pixel * input.ColorOverlay) * rate);	/* Multiple */
	color[2] = pixel + (input.ColorOverlay * rate);	/* Add */
	color[3] = pixel - (input.ColorOverlay * rate);	/* Subtract */

	pixel = color[input.Texture00UV.z];
	pixel.a = pixelA;
	output = pixel;

	return(output);
}
