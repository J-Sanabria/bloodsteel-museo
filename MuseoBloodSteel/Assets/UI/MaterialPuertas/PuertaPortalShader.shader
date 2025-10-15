Shader "Custom/BluePortal_URP"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _PortalColor ("Portal Color", Color) = (0.2, 0.5, 1, 1)
        _GlowColor ("Glow Color", Color) = (0.3, 0.8, 1, 1)
        _DistortionStrength ("Distortion Strength", Range(0,0.2)) = 0.05
        _GlowIntensity ("Glow Intensity", Range(0,5)) = 2
        _Speed ("Animation Speed", Range(0,5)) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "BluePortal"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 normalWS    : NORMAL;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            float4 _PortalColor;
            float4 _GlowColor;
            float _DistortionStrength;
            float _GlowIntensity;
            float _Speed;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // Distorsión con Noise animado
                float2 noiseUV = i.uv + float2(_Time.y * _Speed, _Time.y * _Speed * 0.5);
                float noiseVal = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV).r;
                float2 distortedUV = i.uv + (noiseVal - 0.5) * _DistortionStrength;

                // Color base del portal
                half4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, distortedUV) * _PortalColor;

                // Fresnel para bordes
                float fresnel = 1 - saturate(dot(normalize(i.normalWS), float3(0,0,-1)));
                fresnel = pow(fresnel, 3); // Intensidad del borde
                half3 glow = _GlowColor.rgb * fresnel * _GlowIntensity;

                // Composición final
                baseColor.rgb += glow;
                baseColor.a = 1; // Mantener portal opaco con glow
                return baseColor;
            }
            ENDHLSL
        }
    }
}
