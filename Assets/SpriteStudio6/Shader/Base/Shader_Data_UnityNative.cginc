//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//
#if defined(UNITY_INSTANCING_ENABLED)
	/* MEMO: Caution! If variable-name actually defined ("_...") are changed, can not receive values output by animation-clip. */
	UNITY_INSTANCING_BUFFER_START(PerDrawSprite)

	UNITY_DEFINE_INSTANCED_PROP(fixed4, _RendererColor)
	UNITY_DEFINE_INSTANCED_PROP(fixed2, _Flip)
	UNITY_DEFINE_INSTANCED_PROP(float4, _BlendParam)
	UNITY_DEFINE_INSTANCED_PROP(float4, _PartsColor_LU)
	UNITY_DEFINE_INSTANCED_PROP(float4, _PartsColor_RU)
	UNITY_DEFINE_INSTANCED_PROP(float4, _PartsColor_RD)
	UNITY_DEFINE_INSTANCED_PROP(float4, _PartsColor_LD)
	UNITY_DEFINE_INSTANCED_PROP(float4, _PartsColor_Opacity)
	UNITY_DEFINE_INSTANCED_PROP(float4, _CellPivot_LocalScale)
	UNITY_DEFINE_INSTANCED_PROP(float4, _CellRectangle)
	UNITY_DEFINE_INSTANCED_PROP(float4, _VertexOffset_LURU)
	UNITY_DEFINE_INSTANCED_PROP(float4, _VertexOffset_RDLD)

	UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

	#define RendererColor UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _RendererColor)
	#define Flip UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _Flip)
	#define BlendParam UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _BlendParam)
	#define PartsColor_LU UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _PartsColor_LU)
	#define PartsColor_RU UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _PartsColor_RU)
	#define PartsColor_RD UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _PartsColor_RD)
	#define PartsColor_LD UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _PartsColor_LD)
	#define PartsColor_Opacity UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _PartsColor_Opacity)
	#define CellPivot_LocalScale UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _CellPivot_LocalScale)
	#define CellRectangle UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _CellRectangle)
	#define VertexOffset_LURU UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _VertexOffset_LURU)[unity_InstanceID]
	#define VertexOffset_RDLD UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _VertexOffset_RDLD)

	#define PartsColor _PartsColor_LU

#else
#endif /* defined(UNITY_INSTANCING_ENABLED) */


CBUFFER_START(UnityPerDrawSprite)
#if defined(UNITY_INSTANCING_ENABLED)
#else
	fixed4 _RendererColor;
	float4 _Flip;
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

	#define RendererColor _RendererColor
	#define Flip _Flip

	#define BlendParam _BlendParam
	#define PartsColor_LU _PartsColor_LU
	#define PartsColor_RU _PartsColor_RU
	#define PartsColor_RD _PartsColor_RD
	#define PartsColor_LD _PartsColor_LD
	#define PartsColor_Opacity _PartsColor_Opacity
	#define CellPivot_LocalScale _CellPivot_LocalScale
	#define CellRectangle _CellRectangle
	#define VertexOffset_LURU _VertexOffset_LURU
	#define VertexOffset_RDLD _VertexOffset_RDLD

	#define PartsColor PartsColor_LU
#endif

	float _EnableExternalAlpha;
CBUFFER_END

struct InputVS
{
	float4 vertex : POSITION;
	float4 color : COLOR0;
	float4 texcoord : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct InputPS
{
#if defined(SV_POSITION)
	float4 Position : SV_POSITION;
#else
	float4 Position : POSITION;
#endif
	float4 ColorMain : COLOR0;
	float4 ColorOverlay : COLOR1;
	float4 Texture00UV : TEXCOORD0;
	float4 PositionDraw : TEXCOORD7;
#if defined(RESTRICT_SHADER_MODEL_3)
	float4	ParameterOverlay : TEXCOORD1;
#else
#endif
	UNITY_VERTEX_OUTPUT_STEREO
};
