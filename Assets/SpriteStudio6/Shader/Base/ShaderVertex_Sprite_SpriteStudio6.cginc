//
//	SpriteStudio5 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
InputPS VS_main(InputVS input)
{
	InputPS output;
	float4 temp;

	temp.xy = input.texcoord.xy;
	temp.z = floor(input.texcoord1.x);
	temp.w = 0.0f;
	output.Texture00UV = temp;

	temp = float4(1.0f, 1.0f, 1.0f, input.texcoord1.y);
	output.ColorMain = temp;

	output.ColorOverlay = input.color;

	temp = input.vertex;
	temp = UnityObjectToClipPos(temp);
	output.PositionDraw = temp;
	output.Position = temp;

	return(output);
}
