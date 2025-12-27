sampler gameTarget : register(s0);

float2 Dimensions;
float Time;
float3 Color1;
float3 Color2;

static const float3x3 kCONEtoLMS = float3x3(
     .4121656120, .2118591070, .0883097947,
     .5362752080, .6807189584, .2818474174,
     .0514575653, .1074065790, .6302613616);

static const float3x3 kLMStoCONE = float3x3(
     4.0767245293, -1.2681437731, -.0041119885,
    -3.3072168827,  2.6093323231, -.7034763098,
     .2307590544,  -.3411344290,  1.7068625689);

float3 toOklab(float3 rgb)
{
    return pow(mul(kCONEtoLMS, rgb), .33333);
}

float3 toRGB(float3 oklab)
{
    return mul(kLMStoCONE, pow(oklab, 3));
}

float4 oklabLerp(float4 colA, float4 colB, float h)
{
    float3 lmsA = toOklab(colA.rgb);
    float3 lmsB = toOklab(colB.rgb);
    float3 lms = lerp(lmsA, lmsB, h);
    return float4(toRGB(lms), lerp(colA.a, colB.a, h));
}

float tri(float x)
{
    return abs(frac(x) * 2.0 - 1.0);
}

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
    float4 texColor = tex2D(gameTarget, uv);

    float scrollSpeed = 0.2;
    float h = tri(uv.x + Time * scrollSpeed);

    float4 colorA = float4(Color1, 1.0);
    float4 colorB = float4(Color2, 1.0);

    float4 col = oklabLerp(colorA, colorB, h);
    return float4(saturate(col.rgb), 1.0) * texColor.a;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}

// shoutout zenthemod for sending me the oklab shader stuff, my savior