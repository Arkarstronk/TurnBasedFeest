#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 tex = tex2D(SpriteTextureSampler,input.TextureCoordinates);
	
	int r = floor(tex.r >= 1.0 ? 255 : tex.r * 256.0);
	int g = floor(tex.g >= 1.0 ? 255 : tex.g * 256.0);
	int b = floor(tex.b >= 1.0 ? 255 : tex.b * 256.0);

	if (r == 255 && g == 255 && b == 255) 
	{
		return input.Color;
	}
    return tex;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};