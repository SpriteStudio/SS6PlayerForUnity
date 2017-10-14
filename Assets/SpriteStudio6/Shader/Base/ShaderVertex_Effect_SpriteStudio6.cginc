//
//	SpriteStudio5 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
struct InputVS
{
	float4 vertex : POSITION;
	float4 color : COLOR0;
	float4 texcoord : TEXCOORD0;
};

struct InputPS
{
#ifdef SV_POSITION
	float4 Position : SV_POSITION;
#else
	float4 Position : POSITION;
#endif
	float4 ColorMain : COLOR0;
	float4 Texture00UV : TEXCOORD0;
	float4 PositionDraw : TEXCOORD7;
};

InputPS VS_main(InputVS input)
{
	InputPS	output;
	float4	temp;

	temp.xy = input.texcoord.xy;
	temp.z = 0.0f;
	temp.w = 0.0f;
	output.Texture00UV = temp;

	output.ColorMain = input.color;

	temp = input.vertex;
	temp = UnityObjectToClipPos(temp);
	output.PositionDraw = temp;
	output.Position = temp;

	return(output);
}
