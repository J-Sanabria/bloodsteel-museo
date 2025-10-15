Shader "Custom/CartoonWaterURP"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0, 0.5, 1, 1)
        _WaveSpeed("Wave Speed", Float) = 1
        _WaveScale("Wave Scale", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _BaseColor;
            float _WaveSpeed;
            float _WaveScale;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                
                // Animación simple en posición Y con ondas
                float wave = sin(IN.positionOS.x * 2 + _Time.y * _WaveSpeed) * _WaveScale;
                IN.positionOS.y += wave;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _BaseColor;
            }

            ENDHLSL
        }
    }
}
