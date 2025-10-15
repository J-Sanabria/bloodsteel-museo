Shader "Universal Render Pipeline/ToonWithOutline"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _MainTex   ("Albedo", 2D) = "white" {}
        _Steps     ("Sombras (Niveles)", Range(1,8)) = 3
        _ShadowSoftness ("Suavizado borde sombra", Range(0,1)) = 0.1
        _SpecColor ("Color Specular Toon", Color) = (1,1,1,1)
        _SpecPower ("Fuerza Specular", Range(0,1)) = 0.15
        _GlossSteps ("Niveles Specular", Range(1,6)) = 2

        // Outline
        _OutlineColor ("Color Outline", Color) = (0,0,0,1)
        _OutlineThickness ("Grosor Outline (unidades mundo)", Range(0.0005,0.02)) = 0.005
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }
        LOD 200

        // ---------- Pass principal (Lit toon) ----------
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag

            // URP keywords comunes (main light, sombras, etc.)
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile_fog

            // SRP Batcher friendly
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Steps;
                float _ShadowSoftness;
                float4 _SpecColor;
                float _SpecPower;
                float _GlossSteps;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD1;
                float3 normalWS   : TEXCOORD2;
                float2 uv         : TEXCOORD0;
                half   fogFactor  : TEXCOORD3;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs   nrmInputs = GetVertexNormalInputs(IN.normalOS);

                OUT.positionCS = posInputs.positionCS;
                OUT.positionWS = posInputs.positionWS;
                OUT.normalWS   = nrmInputs.normalWS;
                OUT.uv         = IN.uv;
                OUT.fogFactor  = ComputeFogFactor(posInputs.positionCS.z);
                return OUT;
            }

            // Cuantización simple de luz para efecto toon
            float toonStep(float x, float steps, float softness)
            {
                // x en [0,1]; cuantiza a 'steps' niveles con banda suave
                float s = saturate(x);
                float k = floor(s * steps) / max(steps - 1.0, 1.0);
                return smoothstep(k - softness, k + softness, s);
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Textura base
                float4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _BaseColor;

                // Luz principal
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(IN.positionWS));
                float NdotL = saturate(dot(normalize(IN.normalWS), mainLight.direction));

                // Cuantización de difuso
                float diff = toonStep(NdotL, _Steps, _ShadowSoftness);

                // Ambiente
                float3 ambient = SampleSH(IN.normalWS);

                // Specular toon cuantizado
                float3 viewDir = normalize(_WorldSpaceCameraPos - IN.positionWS);
                float3 halfDir = normalize(mainLight.direction + viewDir);
                float NdotH = saturate(dot(normalize(IN.normalWS), halfDir));
                float specRaw = pow(NdotH, 48.0); // lóbulos apretados
                float spec = toonStep(specRaw, _GlossSteps, 0.1) * _SpecPower;

                float3 litColor =
                    albedo.rgb * (ambient + diff * mainLight.color) +
                    _SpecColor.rgb * spec;

                // Fog URP
                litColor = MixFog(litColor, IN.fogFactor);

                return float4(litColor, albedo.a);
            }
            ENDHLSL
        }

        // ---------- Pass de Outline (inverted hull) ----------
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="SRPDefaultUnlit" }
            Cull Front           // dibuja caras traseras para “engordar” la malla
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float  _OutlineThickness; // en unidades de mundo
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                // Extrusión en espacio objeto según normal, compensando escala
                float3 nWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 pWS = TransformObjectToWorld(IN.positionOS.xyz);

                pWS += nWS * _OutlineThickness;

                OUT.positionCS = TransformWorldToHClip(pWS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }

    FallBack Off
}
