//
//	SpriteStudio5 Player for Unity
//
//	Copyright(C) Web Technology Corp.
//	All rights reserved.
//
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
