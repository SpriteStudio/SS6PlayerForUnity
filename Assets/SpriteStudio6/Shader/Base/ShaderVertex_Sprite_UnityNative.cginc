//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
fixed4 _Color;	// Material Color.
float4 _Blend_LocalScale;
float4 _PartsColor;

InputPS VS_main(InputVS input)
{
	InputPS output;
	float4 temp;

	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

#if defined(UNITY_INSTANCING_ENABLED)
	input.vertex.xy *= _Flip.xy;
#endif

	temp.xy = input.texcoord.xy;
	temp.z = floor(_Blend_LocalScale.x);
	temp.w = 0.0f;
	output.Texture00UV = temp;

	temp = input.color * _RendererColor * _Color;
	temp.a *= _Blend_LocalScale.y;
	output.ColorMain = temp;

	output.ColorOverlay = _PartsColor;

	temp = input.vertex;
	temp.xy *= _Blend_LocalScale.zw;
	temp = UnityObjectToClipPos(temp);
	output.PositionDraw = temp;
#if defined(PIXELSNAP_ON)
	temp = UnityPixelSnap(temp);
#endif
	output.Position = temp;

	return(output);
}
