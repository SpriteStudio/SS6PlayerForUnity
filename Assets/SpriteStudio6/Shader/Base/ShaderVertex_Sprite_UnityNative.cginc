//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
fixed4 _Color;	// Material Color.
float4 _Blend_LocalScale;
float4 _PartsColor;

#if defined(RESTRICT_SHADER_MODEL_3)
/* MEMO: ".x" is not used, now.  */
static const float4 _OverlayParameter_Mix = {1.0f, 1.0f, 0.0f, 1.0f};
static const float4 _OverlayParameter_Add = {1.0f, 0.0f, 0.0f, 1.0f};
static const float4 _OverlayParameter_Sub = {1.0f, 0.0f, 0.0f, -1.0f};
static const float4 _OverlayParameter_Mul = {1.0f, 1.0f, 1.0f, 1.0f};
// #else
#endif

InputPS VS_main(InputVS input)
{
	InputPS output;
	float4 temp;
	float indexBlend = floor(_Blend_LocalScale.x);

	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

#if defined(UNITY_INSTANCING_ENABLED)
	input.vertex.xy *= _Flip.xy;
#endif

	temp.xy = input.texcoord.xy;
	temp.z = indexBlend;
	temp.w = 0.0f;
	output.Texture00UV = temp;

	temp = input.color * _RendererColor * _Color;
	temp.a *= _Blend_LocalScale.y;
	output.ColorMain = temp;

	output.ColorOverlay = _PartsColor;

#if defined(RESTRICT_SHADER_MODEL_3)
	output.ParameterOverlay = (2.0f > indexBlend)
								? ((1.0f > indexBlend) ? _OverlayParameter_Mix : _OverlayParameter_Add)
								: ((3.0f > indexBlend) ? _OverlayParameter_Sub : _OverlayParameter_Mul);
// #else
#endif

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
