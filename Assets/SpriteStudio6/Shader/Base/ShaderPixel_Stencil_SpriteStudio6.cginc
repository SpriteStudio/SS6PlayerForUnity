//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
sampler2D _MainTex;
sampler2D _AlphaTex;
float _EnableExternalAlpha;

#if defined(SV_Target)
fixed4 PS_main(InputPS input) : SV_Target
#else
fixed4 PS_main(InputPS input) : COLOR0
#endif
{
	fixed4 output = 0;
	fixed4 pixel = tex2D(_MainTex, input.Texture00UV.xy);
#if defined(ETC1_EXTERNAL_ALPHA)
	fixed4 alpha = tex2D(_AlphaTex, input.Texture00UV.xy);
	pixel.a = lerp(pixel.a, alpha.r, _EnableExternalAlpha);
#endif
	if (input.ColorMain.a >= pixel.a)
	{
		discard;
	}

	return(output);
}
