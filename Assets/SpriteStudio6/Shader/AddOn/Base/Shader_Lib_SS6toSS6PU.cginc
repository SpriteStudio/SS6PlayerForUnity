//
//	SpriteStudio6 Player for Unity
//
//	Copyright(C) 1997-2021 Web Technology Corp.
//	Copyright(C) CRI Middleware Co., Ltd.
//	All rights reserved.
//
/* Arguments for Pixel-Shader */
#define A_TW	PixelSettingTextureSizeX
#define A_TH	PixelSettingTextureSizeY
#define A_U1	PixelSettingPerTexelU
#define A_V1	PixelSettingPerTexelV
#define A_LU	input.Texture00UVMinMax.x
#define A_TV	input.Texture00UVMinMax.y
#define A_CU	input.Texture00UVAverage.x
#define A_CV	input.Texture00UVAverage.y
#define A_RU	input.Texture00UVMinMax.z
#define A_BV	input.Texture00UVMinMax.w
// #define A_PM	PS_OUTPUT_PMA

/* Parameters for Pixel-Shader */
#define params0	ShaderParameter0
#define params1	ShaderParameter1
#define params2	ShaderParameter2
#define params3	ShaderParameter3
