Shader "Custom/URP_SpaceCrack"
{
    Properties
    {
        _Color("Base Color", Color) = (1,1,1,1) // 🔹 NUEVO: Para que Unity pueda usar material.color
        _MainTex("Base Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _CrackColor("Crack Color", Color) = (0.5, 0.8, 1, 1)
        _FresnelColor("Fresnel Color", Color) = (0.2, 0.5, 1, 1)
        _NoiseSpeed("Noise Speed", Float) = 0.5
        _Distortion("Distortion Amount", Float) = 0.1
        _FresnelPower("Fresnel Power", Float) = 3
        _Alpha("Transparency", Range(0,1)) = 0.7
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : NORMAL;
                float3 viewDirWS : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;

            float4 _Color;         // 🔹 NUEVO
            float4 _CrackColor;
            float4 _FresnelColor;
            float _NoiseSpeed;
            float _Distortion;
            float _FresnelPower;
            float _Alpha;
            float4 _EmissionColor;

            Varyings vert (Attributes v)
            {
                Varyings o;
                float3 positionWS = TransformObjectToWorld(v.positionOS.xyz);
                o.positionHCS = TransformWorldToHClip(positionWS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.viewDirWS = GetWorldSpaceViewDir(positionWS);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // Animación del ruido
                float2 noiseUV = i.uv + _Time.y * _NoiseSpeed;
                float noiseSample = tex2D(_NoiseTex, noiseUV).r;

                // Distorsión UV
                float2 distortedUV = i.uv + (noiseSample - 0.5) * _Distortion;

                // Textura base
                half4 baseCol = tex2D(_MainTex, distortedUV) * _CrackColor * _Color; // 🔹 multiplicamos por _Color

                // Fresnel
                float fresnel = pow(1.0 - saturate(dot(normalize(i.normalWS), normalize(i.viewDirWS))), _FresnelPower);
                half4 fresnelCol = _FresnelColor * fresnel;

                // Combinación
                half4 finalCol = baseCol + fresnelCol;

                // Emisión sumada
                finalCol.rgb += _EmissionColor.rgb; // 🔹 añadimos emisión

                // Transparencia por ruido
                finalCol.a = _Alpha * saturate(noiseSample * 2.0);

                return finalCol;
            }
            ENDHLSL
        }
    }
}
