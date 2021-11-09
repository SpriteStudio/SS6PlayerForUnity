//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//
fixed4 _Color;	// Material Color.

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

	float4 temp;
	float4 temp2;
	float4 start;
	float4 end;
	float4 vertex = input.vertex;
	float4 blendParam = BlendParam;

	temp.xy = Flip;
	vertex.xy *= temp.xy;

	/* Set Mapping-UV */
	temp.xy = input.texcoord.xy;
	temp.z = floor(blendParam.x);
	temp.w = 0.0f;
	output.Texture00UV = temp;

#if defined(RESTRICT_SHADER_MODEL_3)
	/* Set Parameter-Overlay */
	output.ParameterOverlay = (2.0f > temp.z)
		? ((1.0f > temp.z) ? _OverlayParameter_Mix : _OverlayParameter_Add)
		: ((3.0f > temp.z) ? _OverlayParameter_Sub : _OverlayParameter_Mul);
#else
#endif

	/* Get vertex's rate in Rectangle */
	float4 rate;
	temp = CellPivot_LocalScale;
	temp2 = CellRectangle;

	rate.x = vertex.x + temp.x;
	rate.y = vertex.y + (temp2.w - temp.y);
	rate.xy /= temp2.zw;
	rate.zw = 1.0f;
	rate.zw -= rate.xy;

	/* Set Color-Overlay */
	temp = PartsColor_LU;
	temp2 = PartsColor_RU;
	start = (temp * rate.z) + (temp2 * rate.x);

	temp = PartsColor_LD;
	temp2 = PartsColor_RD;
	end = (temp * rate.z) + (temp2 * rate.x);

	temp = (start * rate.y) + (end * rate.w);
	output.ColorOverlay = temp;

	/* Set Color-Vertex */
	temp = PartsColor_Opacity;

	start.x = (temp.x * rate.z) + (temp.y * rate.x);
	start.y = (temp.w * rate.z) + (temp.z * rate.x);
	start.z = (start.x * rate.y) + (start.y * rate.w);

	temp2 = RendererColor;
	temp = input.color * temp2 * _Color;
	temp2.x = blendParam.y;
	temp.a *= temp2.x * start.z;
	output.ColorMain = temp;

	/* Set Coordinate */
	temp = VertexOffset_LURU;
	temp2 = VertexOffset_RDLD;

	start.xy = (temp.xy * rate.z) + (temp.zw * rate.x);
	end.xy = (temp2.zw * rate.z) + (temp2.xy * rate.x);
	temp.xy = (start.xy * rate.y) + (end.xy * rate.w);
	vertex.xy += temp.xy;

	temp = CellPivot_LocalScale;
	vertex.xy *= temp.zw;

	vertex = UnityObjectToClipPos(vertex);
	output.PositionDraw = vertex;

#if defined(PIXELSNAP_ON)
	vertex = UnityPixelSnap(vertex);
#else
#endif
	output.Position = vertex;

	return(output);
}
