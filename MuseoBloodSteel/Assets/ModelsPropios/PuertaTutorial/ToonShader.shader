Shader "Universal Render Pipeline/ToonWithOutline_Pro"
{
    Properties
    {
        // Base
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _MainTex   ("Albedo", 2D) = "white" {}

        // Toon / Luz
        _Steps           ("Sombras (Niveles)", Range(2,8)) = 3
        _ShadowSoftness  ("Suavizado borde sombra", Range(0,0.3)) = 0.06
        _ToonShadowBias  ("Desplazamiento sombra (umbral)", Range(-0.5,0.5)) = 0.0
        _LightStrength   ("Fuerza de luz directa", Range(0,2)) = 1.0
        _BandAA          ("Anti-alias banda (fwidth)", Range(0,2)) = 0.6

        // Ambiente (con override opcional)
        _AmbientStrength ("Fuerza de ambiente", Range(0,1)) = 0.35
        _AmbientMode     ("Modo Ambiente (0=Color, 1=SH)", Range(0,1)) = 1
        _AmbientColor    ("Color Ambiente (override)", Color) = (0.22, 0.19, 0.16, 1)

        // Normal / Metal / Occlusion
        _BumpMap         ("Normal Map", 2D) = "bump" {}
        _BumpScale       ("Intensidad Normal", Range(0,2)) = 1.0

        _Metallic        ("Metallic", Range(0,1)) = 0.0
        _SpecColor       ("Color Specular Toon", Color) = (1,1,1,1)
        _SpecPower       ("Fuerza Specular", Range(0,1)) = 0.15
        _GlossSteps      ("Niveles Specular", Range(1,6)) = 2

        _OcclusionMap    ("Occlusion Map", 2D) = "white" {}
        _OcclusionStrength ("Fuerza Oclu.", Range(0,1)) = 1.0

        // Outline
        _OutlineColor    ("Color Outline", Color) = (0,0,0,1)
        _OutlineThickness("Grosor Outline (unid. mundo)", Range(0.0001,0.10)) = 0.006
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 250

        // ---------- FORWARD LIT TOON ----------
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag

            // URP keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile_fog

            #pragma target 3.0
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

            TEXTURE2D(_MainTex);      SAMPLER(sampler_MainTex);
            TEXTURE2D(_BumpMap);      SAMPLER(sampler_BumpMap);
            TEXTURE2D(_OcclusionMap); SAMPLER(sampler_OcclusionMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _MainTex_ST;

                float  _Steps;
                float  _ShadowSoftness;
                float  _ToonShadowBias;
                float  _LightStrength;
                float  _BandAA;

                float  _AmbientStrength;
                float  _AmbientMode;
                float4 _AmbientColor;

                float  _BumpScale;

                float  _Metallic;
                float4 _SpecColor;
                float  _SpecPower;
                float  _GlossSteps;

                float  _OcclusionStrength;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 tangentOS  : TANGENT;
                float2 uv0        : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD1;
                float3 normalWS   : TEXCOORD2;
                float3 tangentWS  : TEXCOORD3;
                float3 bitanWS    : TEXCOORD4;
                float2 uv         : TEXCOORD0;
                half   fogFactor  : TEXCOORD5;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs   nrmInputs = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);

                OUT.positionCS = posInputs.positionCS;
                OUT.positionWS = posInputs.positionWS;
                OUT.normalWS   = nrmInputs.normalWS;
                OUT.tangentWS  = nrmInputs.tangentWS;
                OUT.bitanWS    = nrmInputs.bitangentWS;

                OUT.uv         = IN.uv0 * _MainTex_ST.xy + _MainTex_ST.zw; // Tiling/Offset
                OUT.fogFactor  = ComputeFogFactor(posInputs.positionCS.z);
                return OUT;
            }

            // Cuantización toon con AA de banda
            float toonQuantize(float x, float steps, float softness, float aa)
            {
                float s = saturate(x + 1e-4);
                float w = max(steps - 1.0, 1.0);
                float k = floor(s * steps) / w;
                float edge = max(softness, fwidth(s) * aa);
                return smoothstep(k - edge, k + edge, s);
            }

            // Normal TS -> WS
            float3 NormalTS_to_WS(float3 nTS, float3 nWS, float3 tWS, float3 bWS)
            {
                float3x3 TBN = float3x3(normalize(tWS), normalize(bWS), normalize(nWS));
                return normalize(mul(nTS, TBN));
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Albedo y Oclu
                float4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _BaseColor;
                float  aoTex  = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, IN.uv).r;
                float  ao     = lerp(1.0, aoTex, _OcclusionStrength);

                // Normal map
                float3 nTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv), _BumpScale);
                float3 N   = NormalTS_to_WS(nTS, IN.normalWS, IN.tangentWS, IN.bitanWS);

                // Luz principal
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(IN.positionWS));
                float3 L = normalize(mainLight.direction);
                float  NdotL = dot(N, L);

                // Difuso toon
                float raw  = saturate(NdotL * _LightStrength + _ToonShadowBias);
                float diff = toonQuantize(raw, _Steps, _ShadowSoftness, _BandAA);

                // Ambiente: mezcla SH/Color según _AmbientMode
                float3 shAmb  = SampleSH(N);
                float3 colAmb = _AmbientColor.rgb;
                float3 ambMix = lerp(colAmb, shAmb, saturate(_AmbientMode));
                float3 ambient = ambMix * _AmbientStrength * ao;

                // Spec toon
                float3 V = normalize(_WorldSpaceCameraPos - IN.positionWS);
                float3 H = normalize(L + V);
                float  NdotH = saturate(dot(N, H));
                float  specRaw = pow(NdotH, lerp(16.0, 64.0, _Metallic));
                float  specQ   = toonQuantize(specRaw, _GlossSteps, 0.08, 0.4) * _SpecPower;

                float3 lit = albedo.rgb * (ambient + diff * mainLight.color) + _SpecColor.rgb * specQ;

                // Fog
                lit = MixFog(lit, IN.fogFactor);

                return float4(lit, albedo.a);
            }
            ENDHLSL
        }

        // ---------- OUTLINE (Inverted Hull) ----------
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="SRPDefaultUnlit" }
            Cull Front
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float  _OutlineThickness;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; };
            struct Varyings   { float4 positionCS : SV_POSITION; };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 nWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 pWS = TransformObjectToWorld(IN.positionOS.xyz);
                pWS += nWS * _OutlineThickness;  // grosor en unidades de mundo
                OUT.positionCS = TransformWorldToHClip(pWS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target { return _OutlineColor; }
            ENDHLSL
        }
    }

    FallBack Off
}
