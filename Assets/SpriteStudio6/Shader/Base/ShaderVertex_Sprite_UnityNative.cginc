//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
fixed4 _Color;	// Material Color.

float4 _BlendParam;	/* .x:Blend-Operation / .y:Opacity / .zw:(no used) */
float4 _PartsColor_LU;
float4 _PartsColor_RU;
float4 _PartsColor_RD;
float4 _PartsColor_LD;
float4 _PartsColor_Opacity;	/* .x:LU / .y:RU / .z:RD / .w:LD */
float4 _CellPivot_LocalScale;	/* .xy:Pivot.xy / .zw:LocalScale.xy */
float4 _CellRectangle;	/* .xy:CellPositionXY / .zw:CellSizeXY */
float4 _VertexOffset_LURU;	/* .xy:LU.xy / .zw:RD.xy */
float4 _VertexOffset_RDLD;	/* .xy:RD.xy / .zw:LD.xy */

#if defined(RESTRICT_SHADER_MODEL_3)
/* MEMO: ".x" is not used, now. */
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
	float4 start;
	float4 end;
	float indexBlend = floor(_BlendParam.x);
	float4 vertex = input.vertex;

	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

#if defined(UNITY_INSTANCING_ENABLED)
//	input.vertex.xy *= _Flip.xy;
	vertex.xy *= _Flip.xy;
#endif

	/* Set Mapping-UV */
	temp.xy = input.texcoord.xy;
	temp.z = indexBlend;
	temp.w = 0.0f;
	output.Texture00UV = temp;

#if defined(RESTRICT_SHADER_MODEL_3)
	/* Set Parameter-Overlay */
	output.ParameterOverlay = (2.0f > indexBlend)
								? ((1.0f > indexBlend) ? _OverlayParameter_Mix : _OverlayParameter_Add)
								: ((3.0f > indexBlend) ? _OverlayParameter_Sub : _OverlayParameter_Mul);
// #else
#endif

	/* Get vertex's rate in Rectangle */
	float4 rate;
//	rate.xy = vertex.xy + _CellPivot_LocalScale.xy;
	rate.x = vertex.x + _CellPivot_LocalScale.x;
	rate.y = vertex.y + (_CellRectangle.w - _CellPivot_LocalScale.y);
	rate.xy /= _CellRectangle.zw;
	rate.zw = 1.0f;
	rate.zw -= rate.xy;

	/* Set Color-Overlay */
	start = (_PartsColor_LU * rate.z) + (_PartsColor_RU * rate.x);
	end = (_PartsColor_LD * rate.z) + (_PartsColor_RD * rate.x);
	temp = (start * rate.y) + (end * rate.w);
	output.ColorOverlay = temp;

	/* Set Color-Vertex */
	start.x = (_PartsColor_Opacity.x * rate.z) + (_PartsColor_Opacity.y * rate.x);
	start.y = (_PartsColor_Opacity.w * rate.z) + (_PartsColor_Opacity.z * rate.x);
	start.z = (start.x * rate.y) + (start.y * rate.w);

	temp = input.color * _RendererColor * _Color;
	temp.a *= _BlendParam.y * start.z;
	output.ColorMain = temp;

	/* Set Coordinate */
	start.xy = (_VertexOffset_LURU.xy * rate.z) + (_VertexOffset_LURU.zw * rate.x);
	end.xy = (_VertexOffset_RDLD.zw * rate.z) + (_VertexOffset_RDLD.xy * rate.x);
	temp.xy = (start.xy * rate.y) + (end.xy * rate.w);
	vertex.xy += temp.xy;

	vertex.xy *= _CellPivot_LocalScale.zw;
	vertex = UnityObjectToClipPos(vertex);
	output.PositionDraw = vertex;
#if defined(PIXELSNAP_ON)
	vertex = UnityPixelSnap(vertex);
#endif
	output.Position = vertex;

	return(output);
}
