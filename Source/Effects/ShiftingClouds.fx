sampler gameTarget : register(s0);
sampler noiseTexture : register(s1);

float2 Dimensions;
float Time;

float3 SkyColor;

float timeScale = 10.0;
float cloudScale = 1.0;

float noise(float2 uv) 
{
    uv = frac(uv + 0.5) - 0.5;
    uv = uv * 0.99 + 0.5;
    return tex2D(noiseTexture, uv).r;
}

float2 rotate(float2 uv)
{
    uv = uv + noise(uv * 0.2) * 0.005;
    float rot = 345;
    
    float sinRot = sin(rot);
    float cosRot = cos(rot);
    
    float2x2 rotMat = float2x2(cosRot, -sinRot, sinRot, cosRot);
    return mul(uv, rotMat);
}

float fbm(float2 uv)
{
    int noiseOctaves = 4;

    float value = 0.0;
    float total = 0.0;

    float amplitude = 0.5;

    for (int i = 0; i < noiseOctaves; i++) 
    {
        value += amplitude * noise(uv - Time * 0.00015 * (1.0 - amplitude));
        total += amplitude;

        uv *= 3;
        uv = rotate(uv);
        amplitude *= 0.5;
    }

    return value / total;
}

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{   
    float4 baseColor = tex2D(gameTarget, uv);
    
    float2 adjUV = uv * float2(Dimensions.x * 2.0, Dimensions.y);
    float2 scaledUV = adjUV / (40000.0 * cloudScale);

    float color1 = fbm(scaledUV - 0.5 - (Time * 0.00004 * timeScale));
    float color2 = fbm(scaledUV - 5.5 - (Time * 0.00002 * timeScale));
    
    float threshold = 0.6;
    float softness = 0.2;
    float cutoff = 1.0 - threshold;

    float bright = 1.0 * (1.8 - threshold);
    
    float clouds1 = smoothstep(cutoff, min(cutoff + softness * 2.0, 1.0), color1) - 0.25;
    float clouds2 = smoothstep(cutoff, min(cutoff + softness, 1.0), color2) - 0.25;
    float allClouds = saturate(clouds1 + clouds2);

    float4 skyCol = float4(SkyColor, 1.0);

    float cloudCol = saturate(saturate(1.0 - pow(color1, 1.0) * 0.2) * bright);

    float4 clouds1Col = float4(cloudCol, cloudCol, cloudCol, 1.0);
    float4 clouds2Col = lerp(clouds1Col, skyCol, 0.5);
    float4 allCloudCol = lerp(clouds1Col, clouds2Col, saturate(clouds2 - clouds1));
    
    float luminance = dot(baseColor.rgb, float3(0.299, 0.587, 0.114));
    float mask = saturate(luminance * 50.0);

    float4 cloudResult = lerp(skyCol, allCloudCol, allClouds);
    float4 finalColor = lerp(baseColor, cloudResult, mask);

    return finalColor;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}