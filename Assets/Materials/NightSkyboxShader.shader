Shader "Custom/NightSkyboxShader"
{
    Properties
    {
        _TopColor("Top Sky Color", Color) = (0.01, 0.01, 0.05, 1)
        _BottomColor("Bottom Sky Color", Color) = (0.1, 0.05, 0.15, 1)
        _StarColor("Star Color", Color) = (1, 1, 1, 1)
        _StarDensity("Star Density", Range(0, 1000)) = 300
        _StarBrightness("Star Brightness", Range(0, 1)) = 1
        _TwinkleSpeed("Twinkle Speed", Range(0, 10)) = 2
        _Seed("Random Seed", Float) = 123.0
        _GradientSharpness("Gradient Sharpness", Range(0.1, 5)) = 1
        _GradientOffset("Gradient Offset", Range(-1, 1)) = 0
        _HorizonGlowColor("Horizon Glow Color", Color) = (0.8, 0.4, 0.2, 1)
        _HorizonGlowIntensity("Horizon Glow Intensity", Range(0, 1)) = 0.2
    }

    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Opaque" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _TopColor;
            fixed4 _BottomColor;
            fixed4 _StarColor;
            float _StarDensity;
            float _StarBrightness;
            float _TwinkleSpeed;
            float _Seed;
            float _GradientSharpness;
            float _GradientOffset;
            fixed4 _HorizonGlowColor;
            float _HorizonGlowIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 direction : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.direction = v.vertex.xyz;
                return o;
            }

            float hash(float3 p)
            {
                return frac(sin(dot(p, float3(12.9898, 78.233, 45.164))) * 43758.5453 + _Seed);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 dir = normalize(i.direction);

                // Gradient with offset + sharpness
                float t = saturate((dir.y + _GradientOffset) * 0.5 + 0.5);
                t = pow(t, _GradientSharpness);
                fixed3 skyColor = lerp(_BottomColor.rgb, _TopColor.rgb, t);

                // Horizon glow
                float glowFactor = saturate(1.0 - t);
                skyColor += _HorizonGlowColor.rgb * glowFactor * _HorizonGlowIntensity;

                // Stars
                float h = hash(floor(dir * _StarDensity));
                if (h > 0.997)
                {
                    float brightness = (h - 0.997) / 0.003;

                    // Smooth, varied star twinkle
                    float twinklePhase = h * 6.28318; // 2PI
                    float twinkle = 0.5 + 0.5 * sin(twinklePhase + _Time.y * _TwinkleSpeed);
                    brightness *= twinkle;

                    skyColor += _StarColor.rgb * brightness * _StarBrightness;
                }

                return fixed4(skyColor, 1);
            }
            ENDCG
        }
    }
}
