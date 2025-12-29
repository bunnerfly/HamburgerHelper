sampler gameTarget : register(s0);

float2 Dimensions;

float3 Color;

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{   
    float4 baseColor = tex2D(gameTarget, uv);
    return float4(Color.rgb, 1.0) * baseColor.a;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}