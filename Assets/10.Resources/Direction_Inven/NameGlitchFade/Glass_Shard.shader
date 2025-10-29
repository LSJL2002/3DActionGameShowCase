Shader "UI/URP/UIShardGlitch_BaseOnFullFade"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0.1,0.1,0.1,1)
        _ShardColor("Shard Color", Color) = (0.5,0.8,1,1)
        _Fade("Fade", Range(0,1)) = 0
        _ShardCountX("Shards X", Range(4,40)) = 8
        _ShardCountY("Shards Y", Range(4,40)) = 8
        _GlitchStrength("Glitch Strength", Range(0,0.05)) = 0.02
        _ColorOffset("Color Offset Strength", Range(0,0.05)) = 0.02
        _Speed("Animation Speed", Range(0,10)) = 3
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
            "CanvasRenderer"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _BaseColor;
            float4 _ShardColor;
            float _Fade;
            float _ShardCountX;
            float _ShardCountY;
            float _GlitchStrength;
            float _ColorOffset;
            float _Speed;

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

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            float rand(float2 co)
            {
                return frac(sin(dot(co,float2(12.9898,78.233))) * 43758.5453);
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float time = _Time.y * _Speed;

                // --- 조각 단위 ---
                float2 shardUV = float2(floor(IN.uv.x * _ShardCountX),
                                        floor(IN.uv.y * _ShardCountY));
                float n = rand(shardUV);

                // Fade에 따라 조각 등장
                float visible = step(1.0 - _Fade, n);

                // 글리치 흔들림 (Fade가 1이면 제거)
                float glitchX = (_Fade < 1.0) ? (rand(shardUV + floor(time*10)) - 0.5) * _GlitchStrength : 0.0;
                float glitchY = (_Fade < 1.0) ? (rand(shardUV + floor(time*20)) - 0.5) * _GlitchStrength : 0.0;

                float2 uvGlitch = IN.uv + float2(glitchX, glitchY);

                // 색상 결정
                half4 col;
                if (_Fade < 1.0)
                {
                    // Fade < 1 → 조각별 색상 + RGB 분리
                    float rOff = (rand(shardUV + 1.0) - 0.5) * _ColorOffset;
                    float gOff = (rand(shardUV + 2.0) - 0.5) * _ColorOffset;
                    float bOff = (rand(shardUV + 3.0) - 0.5) * _ColorOffset;

                    col.r = lerp(_BaseColor.r, _ShardColor.r, n) * (1.0 - rOff);
                    col.g = lerp(_BaseColor.g, _ShardColor.g, n) * (1.0 - gOff);
                    col.b = lerp(_BaseColor.b, _ShardColor.b, n) * (1.0 - bOff);
                    col.a = visible;
                }
                else
                {
                    // Fade = 1 → 모든 조각 BaseColor
                    col = _BaseColor;
                    col.a = 1.0;
                }

                return col;
            }
            ENDHLSL
        }
    }
}