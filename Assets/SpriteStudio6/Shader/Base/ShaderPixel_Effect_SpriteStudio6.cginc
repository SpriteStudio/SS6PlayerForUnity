//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
sampler2D _MainTex;
sampler2D _AlphaTex;
float _EnableExternalAlpha;

#ifdef SV_Target
fixed4 PS_main(InputPS Input) : SV_Target
#else
fixed4 PS_main(InputPS Input) : COLOR0
#endif
{
	fixed4 output;

	fixed4	pixel = tex2D(_MainTex, Input.Texture00UV.xy);
#if defined(ETC1_EXTERNAL_ALPHA)
	fixed4 alpha = tex2D(_AlphaTex, Input.Texture00UV.xy);
	pixel.a = lerp(pixel.a, alpha.r, _EnableExternalAlpha);
#endif
	pixel *= Input.ColorMain;
#if !defined(PS_NOT_DISCARD)
	if(0.0f >= pixel.a)
	{
		discard;
	}
#endif
#if defined(PS_OUTPUT_PMA)
	pixel.xyz *= pixel.a;
#endif

	output = pixel;

	return(output);
}
