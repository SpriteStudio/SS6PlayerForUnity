//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
fixed4 _Color;	// Material Color.
float2 _MainTex_ST;

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
	float4 temp2;
	float4 start;
	float4 end;
	float4 vertex = input.vertex;
	float4 tempVert = vertex;
	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

#if defined(UNITY_INSTANCING_ENABLED)
	float4 flip = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _Flip);
	vertex.xy *= flip.xy;
#else
	vertex.xy *= _Flip.xy;
#endif
	/* Set Mapping-UV */
	temp.xy = input.texcoord.xy;
#if defined(UNITY_INSTANCING_ENABLED)
	float4 blendParam = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _BlendParam);
	temp.z = floor(blendParam.x);
#else
	temp.z = floor(_BlendParam.x);
#endif
	temp.w = 0.0f;
	output.Texture00UV = temp;

#if defined(RESTRICT_SHADER_MODEL_3)
	/* Set Parameter-Overlay */
	output.ParameterOverlay = (2.0f > temp.z)
		? ((1.0f > temp.z) ? _OverlayParameter_Mix : _OverlayParameter_Add)
		: ((3.0f > temp.z) ? _OverlayParameter_Sub : _OverlayParameter_Mul);
	// #else
#endif

	/* Get vertex's rate in Rectangle */
	float4 rate;
#if defined(UNITY_INSTANCING_ENABLED)
	temp = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _CellPivot_LocalScale);
	temp2 = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _CellRectangle);
#else
	temp = _CellPivot_LocalScale;
	temp2 = _CellRectangle;
#endif
	rate.x = vertex.x + temp.x;
	rate.y = vertex.y + (temp2.w - temp.y);
	rate.xy /= temp2.zw;
	rate.zw = 1.0f;
	rate.zw -= rate.xy;

	/* Set Color-Overlay */
#if defined(UNITY_INSTANCING_ENABLED)
	temp = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _PartsColor_LU);
	temp2 = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _PartsColor_RU);
#else
	temp = _PartsColor_LU;
	temp2 = _PartsColor_RU;
#endif
	start = (temp * rate.z) + (temp2 * rate.x);
	
#if defined(UNITY_INSTANCING_ENABLED)
	temp = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _PartsColor_LD);
	temp2 = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _PartsColor_RD);
#else
	temp = _PartsColor_LD;
	temp2 = _PartsColor_RD;
#endif
	end = (temp * rate.z) + (temp2 * rate.x);

	temp = (start * rate.y) + (end * rate.w);
	output.ColorOverlay = temp;

	/* Set Color-Vertex */
#if defined(UNITY_INSTANCING_ENABLED)
	temp = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _PartsColor_Opacity);
#else
	temp = _PartsColor_Opacity;
#endif

	start.x = (temp.x * rate.z) + (temp.y * rate.x);
	start.y = (temp.w * rate.z) + (temp.z * rate.x);
	start.z = (start.x * rate.y) + (start.y * rate.w);
	
#if defined(UNITY_INSTANCING_ENABLED)
	temp2 = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _RendererColor);
#else
	temp2 = _RendererColor;
#endif
	temp = input.color * temp2 * _Color;
	
#if defined(UNITY_INSTANCING_ENABLED)
	temp2.x = blendParam.y;
#else
	temp2.x = _BlendParam.y;
#endif
	temp.a *= temp2.x * start.z;
	output.ColorMain = temp;

	/* Set Coordinate */
#if defined(UNITY_INSTANCING_ENABLED)
	temp = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _VertexOffset_LURU);
	temp2 = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _VertexOffset_RDLD);
#else
	temp = _VertexOffset_LURU;
	temp2 = _VertexOffset_RDLD;
#endif


	start.xy = (temp.xy * rate.z) + (temp.zw * rate.x);
	end.xy = (temp2.zw * rate.z) + (temp2.xy * rate.x);
	temp.xy = (start.xy * rate.y) + (end.xy * rate.w);
	vertex.xy += temp.xy;
	//tempVert = vertex;
	
#if defined(UNITY_INSTANCING_ENABLED)
	temp = UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _CellPivot_LocalScale);
#else
	temp = _CellPivot_LocalScale;
#endif
	vertex.xy *= mul(unity_ObjectToWorld, _CellPivot_LocalScale).zw;
	tempVert = vertex;

	vertex = UnityObjectToClipPos(tempVert);
	//vertex.xy *= _CellPivot_LocalScale.zw;
	output.PositionDraw = vertex;
#if defined(PIXELSNAP_ON)
	vertex = UnityPixelSnap(vertex);
#endif

	output.Position = vertex;

	return(output);
}
