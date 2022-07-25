//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//

struct InputVS
{
	float4 vertex : POSITION;
	float4 color : COLOR0;
	float2 texcoord : TEXCOORD0;
	float2 blend : TEXCOORD1;	/* .x: blend for Parts-Color / .y: blend for target */
	float2 power : TEXCOORD2;	/* .x: Rate Opacity / .y: (no used) */
	float2 coloParts : TEXCOORD3;	/* .x: R*255+G / .y: B*255+A */
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
	float4 MaskUV : TEXCOORD5;
	float4 PositionWorld : TEXCOORD6;
	float4 PositionDraw : TEXCOORD7;
#if defined(RESTRICT_SHADER_MODEL_3)
	float4	ParameterOverlay : TEXCOORD1;
#else
#endif
	UNITY_VERTEX_OUTPUT_STEREO
};

sampler2D _MainTex;
sampler2D _AlphaTex;
float _EnableExternalAlpha;
fixed4 _Color;
fixed4 _TextureSampleAdd;
float4 _ClipRect;
float4 _MainTex_ST;
float _UIMaskSoftnessX;
float _UIMaskSoftnessY;
// fixed4 _RendererColor
// fixed2 _Flip

