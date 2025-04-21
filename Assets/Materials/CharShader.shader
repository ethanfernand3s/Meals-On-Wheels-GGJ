Shader "Custom/CharacterControlSurface"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Roughness("Roughness", Range(0, 1)) = 0.5
        _ShadowBrightness("Shadow Brightness", Range(0, 5)) = 5.0
        _Color("Color Tint", Color) = (1,1,1,1)
        _FresnelStrength("Fresnel Strength", Range(0, 2)) = 0.5
        _FresnelPower("Fresnel Softness", Range(0.1, 8)) = 3.0
        _FresnelColor("Fresnel Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        half _Roughness;
        half _ShadowBrightness;
        fixed4 _Color;
        half _FresnelStrength;
        half _FresnelPower;
        fixed4 _FresnelColor;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            o.Albedo = tex.rgb;
            o.Alpha = tex.a;
            o.Smoothness = 1.0 - _Roughness;
            o.Metallic = 0.0;
            o.Occlusion = _ShadowBrightness;

            // Fresnel effect with customizable power
            float fresnel = pow(1.0 - saturate(dot(normalize(IN.viewDir), o.Normal)), _FresnelPower);
            o.Emission = _FresnelColor.rgb * fresnel * _FresnelStrength;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
