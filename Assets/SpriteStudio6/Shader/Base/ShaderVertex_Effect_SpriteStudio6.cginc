//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//

/* Static-Datas for "Parts-Color" */
VERTEX_STATICDATA_PARTSCOLOR;

InputPS VS_main(InputVS input)
{
	InputPS output;
	float4 temp;
	float indexBlend = floor(input.texcoord.z);

	/* Set Texture00-UV */
	temp.xy = input.texcoord.xy;
	temp.z = indexBlend;
	temp.w = 0.0f;
	output.Texture00UV = temp;

	/* MEMO: "VertexSetCellAuxiliary" is a fixed process to set auxiliary data of "Cell"(UV-coordinate). */
	VertexSetCellAuxiliary(output, input);

	/* Set "Parts-Color" parameter & Argument for Vetex-Shader */
	temp = input.color;
	output.ColorMain = temp;
	output.ColorOverlay = temp;	/* Unused */

	/* MEMO: For calculate "Parts-Color" in Pixel-Shader, Need to run "VertexSetPartsColor". */
	/*       At the same time, set ÅhArgumentVs00"(uniform for Vertex-Shader).               */
	VertexSetPartsColor(output, indexBlend, input.texcoord.w);

	/* Set Draw-Position */
	temp = input.vertex;
	temp = UnityObjectToClipPos(temp);
	output.PositionDraw = temp;
	output.Position = temp;

	/* Finalize */
	return(output);
}
