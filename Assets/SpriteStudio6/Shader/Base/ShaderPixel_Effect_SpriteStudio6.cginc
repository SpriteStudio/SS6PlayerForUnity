//
//	SpriteStudio5 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
sampler2D	_MainTex;

#ifdef SV_Target
fixed4 PS_main(InputPS Input) : SV_Target
#else
fixed4 PS_main(InputPS Input) : COLOR0
#endif
{
	fixed4 output;

	fixed4	pixel = tex2D(_MainTex, Input.Texture00UV.xy);
	pixel *= Input.ColorMain;
	if(0.0f >= pixel.a)
	{
		discard;
	}
	output = pixel;

	return(output);
}
