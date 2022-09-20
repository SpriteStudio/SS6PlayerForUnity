//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//
#if defined(RESTRICT_SHADER_MODEL_3)
/* MEMO: ".x" is not used, now. */
static const float4 _OverlayParameter_Mix = {1.0f, 1.0f, 0.0f, 1.0f};
static const float4 _OverlayParameter_Add = {1.0f, 0.0f, 0.0f, 1.0f};
static const float4 _OverlayParameter_Sub = {1.0f, 0.0f, 0.0f, -1.0f};
static const float4 _OverlayParameter_Mul = {1.0f, 1.0f, 1.0f, 1.0f};
#else
#endif

InputPS VS_main(InputVS input)
{
	InputPS output;

	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

	/* Get Positions */
	float4 vertex = input.vertex;
	float4 position = UnityObjectToClipPos(vertex);
	float2 sizePixel = position.w;
	sizePixel /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

	/* Set Texture-UVs */	
	float4 rectangleClamped = clamp(_ClipRect, -2e10, 2e10);
	float2 uvMask = (vertex.xy - rectangleClamped.xy) / (rectangleClamped.zw - rectangleClamped.xy);

	output.Texture00UV.xy = TRANSFORM_TEX(input.texcoord.xy, _MainTex);
	output.Texture00UV.zw = input.blend.xy;
	output.MaskUV = float4(vertex.xy * 2.0 - rectangleClamped.xy - rectangleClamped.zw, 0.25 / (0.25 * float2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(sizePixel.xy)));

	/* Set Colors */
	output.ColorMain = input.color;	/* _Color; */
	output.ColorMain.a *= input.power.x;
	output.ColorOverlay = float4(	floor(input.coloParts.x / 256.0) * (1.0 / 255.0),
									fmod(input.coloParts.x, 256.0) * (1.0 / 255.0),
									floor(input.coloParts.y / 256.0) * (1.0 / 255.0),
									fmod(input.coloParts.y, 256.0) * (1.0 / 255.0)
								);

#if defined(RESTRICT_SHADER_MODEL_3)
	/* Set Parameter-Overlay */
	float blend = input.blend.x;
	output.ParameterOverlay = (2.0f > blend)
		? ((1.0f > blend) ? _OverlayParameter_Mix : _OverlayParameter_Add)
		: ((3.0f > blend) ? _OverlayParameter_Sub : _OverlayParameter_Mul);
#else
#endif

	/* Set Positions */
	output.PositionWorld = vertex;
	output.Position = position;
	output.PositionDraw = position;

	return(output);
}
