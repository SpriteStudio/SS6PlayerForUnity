//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
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
	float indexBlend = floor(input.texcoord1.x);

	temp.xy = input.texcoord.xy;
	temp.z = indexBlend;
	temp.w = 0.0f;
	output.Texture00UV = temp;

	temp = float4(1.0f, 1.0f, 1.0f, input.texcoord1.y);
	output.ColorMain = temp;

	output.ColorOverlay = input.color;

#if defined(RESTRICT_SHADER_MODEL_3)
	output.ParameterOverlay = (2.0f > indexBlend)
								? ((1.0f > indexBlend) ? _OverlayParameter_Mix : _OverlayParameter_Add)
								: ((3.0f > indexBlend) ? _OverlayParameter_Sub : _OverlayParameter_Mul);
// #else
#endif

	temp = input.vertex;
	temp = UnityObjectToClipPos(temp);
//	temp = mul(UNITY_MATRIX_VP, temp);
	output.PositionDraw = temp;
	output.Position = temp;

	return(output);
}
