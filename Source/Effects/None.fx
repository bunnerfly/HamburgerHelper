sampler gameTarget : register(s0);

float2 Dimensions;

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{   
    return float4(1.0f, 1.0f, 1.0f, 1.0f);
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}