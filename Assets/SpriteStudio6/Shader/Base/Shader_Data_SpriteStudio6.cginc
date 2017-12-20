//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
struct InputVS
{
	float4 vertex : POSITION;
	float4 color : COLOR0;
	float4 texcoord : TEXCOORD0;
	float4 texcoord1 : TEXCOORD1;
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
};
