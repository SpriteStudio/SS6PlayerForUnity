//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
#if defined(UNITY_INSTANCING_ENABLED)
UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
// SpriteRenderer.Color while Non-Batched/Instanced.
UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
// this could be smaller but that's how bit each entry is regardless of type
UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)

UNITY_DEFINE_INSTANCED_PROP(float4, ss6pu_BlendParam)
UNITY_DEFINE_INSTANCED_PROP(float4, ss6pu_PartsColor_LU)
UNITY_DEFINE_INSTANCED_PROP(float4, ss6pu_PartsColor_RU)
UNITY_DEFINE_INSTANCED_PROP(float4, ss6pu_PartsColor_RD)
UNITY_DEFINE_INSTANCED_PROP(float4, ss6pu_PartsColor_LD)
UNITY_DEFINE_INSTANCED_PROP(float4, ss6pu_PartsColor_Opacity)
UNITY_DEFINE_INSTANCED_PROP(float4, ss6pu_CellPivot_LocalScale)
UNITY_DEFINE_INSTANCED_PROP(float4, ss6pu_CellRectangle)
UNITY_DEFINE_INSTANCED_PROP(float4, ss6pu_VertexOffset_LURU)
UNITY_DEFINE_INSTANCED_PROP(float4, ss6pu_VertexOffset_RDLD)
UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

#define _RendererColor unity_SpriteRendererColorArray[unity_InstanceID]
#define _Flip unity_SpriteFlipArray[unity_InstanceID]

#define _BlendParam ss6pu_BlendParam[unity_InstanceID]
#define _PartsColor_LU ss6pu_PartsColor_LU[unity_InstanceID]
#define _PartsColor_RU ss6pu_PartsColor_RU[unity_InstanceID]
#define _PartsColor_RD ss6pu_PartsColor_RD[unity_InstanceID]
#define _PartsColor_LD ss6pu_PartsColor_LD[unity_InstanceID]
#define _PartsColor_Opacity ss6pu_PartsColor_Opacity[unity_InstanceID]
#define _CellPivot_LocalScale ss6pu_CellPivot_LocalScale[unity_InstanceID]
#define _CellRectangle ss6pu_CellRectangle[unity_InstanceID]
#define _VertexOffset_LURU ss6pu_VertexOffset_LURU[unity_InstanceID]
#define _VertexOffset_RDLD ss6pu_VertexOffset_RDLD[unity_InstanceID]

#define _PartsColor _PartsColor_LU
#endif

CBUFFER_START(UnityPerDrawSprite)
#if !defined(UNITY_INSTANCING_ENABLED)
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

#define _PartsColor _PartsColor_LU
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
// #else
#endif
	UNITY_VERTEX_OUTPUT_STEREO
};
