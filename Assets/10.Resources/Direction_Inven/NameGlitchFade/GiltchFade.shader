Shader "UI/URP/TriangleDotMatrixUI_WaveTri_AspectCorrect"
{
    Properties
    {
        _FlashColor("Flash Color", Color) = (1,1,1,1)
        _BaseColor("Base Color", Color) = (0,0,0,1)
        _Fade("Fade", Range(0,1)) = 1
        _WaveSpeed("Wave Speed", Range(0,5)) = 1
        _TrianglesX("Triangles X", Range(5,50)) = 10
        _TrianglesY("Triangles Y", Range(5,50)) = 10
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

            float4 _FlashColor;
            float4 _BaseColor;
            float _Fade;
            float _WaveSpeed;
            float _TrianglesX;
            float _TrianglesY;

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
                OUT.positionHCS = TransformObjectToHClip(float4(IN.positionOS.xyz, 1.0));
                OUT.uv = IN.uv;
                return OUT;
            }

            // 정삼각형 패턴 (화면 종횡비 보정)
            float TrianglePattern(float2 uv)
            {
                // 화면 종횡비 계산
                float aspect = _ScreenParams.x / _ScreenParams.y;

                // UV 스케일링
                float2 scaled = float2(uv.x * aspect * _TrianglesX, uv.y * _TrianglesY);

                float col = floor(scaled.x);
                float row = floor(scaled.y);

                float localX = frac(scaled.x);
                float localY = frac(scaled.y);

                // 홀수 행 오프셋
                float xOffset = fmod(row, 2.0) > 0.5 ? 0.5 : 0.0;
                localX = localX - xOffset;

                // 정삼각형 높이 = sqrt(3)/2
                float h = sqrt(3)/2;

                // 삼각형 내부 여부
                float inside = step(localY, h * (1.0 - localX)) * step(localY, h * localX);
                inside = saturate(inside);

                return inside;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // 좌→우 반복 웨이브
                float t = _Time.y;
                float wave = frac(IN.uv.x + t * _WaveSpeed);

                float tri = TrianglePattern(IN.uv);

                // 삼각형 전체가 wave 범위를 지날 때만 흰색
                float flash = smoothstep(0.45, 0.55, wave) * tri;

                half4 col = lerp(_BaseColor, _FlashColor, flash);
                col.a *= _Fade;

                return col;
            }
            ENDHLSL
        }
    }
}