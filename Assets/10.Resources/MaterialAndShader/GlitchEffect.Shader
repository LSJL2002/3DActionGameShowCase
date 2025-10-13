Shader "URP/ImageEffect/Glitch" {
    Properties {
        [Toggle] _FlipY("Grab Texture Y Flip (UI/Image Effect용)", Float) = 0
        _GlitchAmount("GlitchAmount", Range(0, 1)) = 1
        [NoScaleOffset]_GlitchTex("GlitchTex", 2D) = "white" {}
        _GlitchColor1("GlitchColor1", Color) = (1, 1, 1, 1)
        _GlitchColor2("GlitchColor2", Color) = (1, 1, 1, 1)
        _GlitchColor3("GlitchColor3", Color) = (1, 1, 1, 1)
        _GlitchCutAmountX("GlitchCutAmountX", Range(0.1, 10)) = 1 //글리치 잘리는 양.
        _GlitchCutAmountY("GlitchCutAmountY", Range(0.1, 10)) = 1 //글리치 잘리는 양.
    }

    SubShader {
        // URP 쉐이더를 위한 Tags 설정
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }
        LOD 200

        // GrabPass는 URP에서 사용하지 않습니다. URP에서는 C# 스크립트를 통해 텍스처를 전달합니다.
        
        Pass {
            Name "GlitchEffect"
            // 포스트 프로세싱을 위한 설정
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            // 버텍스/프래그먼트 쉐이더 지정
            #pragma vertex vert
            #pragma fragment frag

            // URP 쉐이더 라이브러리 포함 (유용한 함수와 구조체 포함)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // ------------------------------------
            // 변수 선언 (CBUFFER)
            // ------------------------------------
            
            // 전역 변수
            TEXTURE2D(_GrabTexture);
            SAMPLER(sampler_GrabTexture);
            TEXTURE2D(_GlitchTex);
            SAMPLER(sampler_GlitchTex);
            
            // Material Properties
            CBUFFER_START(UnityPerMaterial)
                float4 _GlitchTex_ST;
                half _GlitchAmount;
                half3 _GlitchColor1;
                half3 _GlitchColor2;
                half3 _GlitchColor3;
                half _GlitchCutAmountX;
                half _GlitchCutAmountY;
                half _FlipY;
            CBUFFER_END
            
            // ------------------------------------
            // 구조체
            // ------------------------------------

            // A2V (Application to Vertex) : 버텍스 쉐이더 입력
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0; // 화면 UV
            };

            // V2F (Vertex to Fragment) : 프래그먼트 쉐이더 입력
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0; // 화면 UV
            };

            // ------------------------------------
            // 버텍스 쉐이더
            // ------------------------------------

            Varyings vert (Attributes input)
            {
                Varyings output;
                
                // UnityClipOrScreenPos 함수를 사용하여 클립 공간 위치를 계산합니다.
                // 이는 스크린 전체를 덮는 쿼드에 사용되는 표준 버텍스 쉐이더입니다.
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                
                // UV 좌표를 그대로 전달합니다. 이 uv는 0~1 범위의 화면 좌표로 사용됩니다.
                output.uv = input.uv;
                
                return output;
            }

            // ------------------------------------
            // 프래그먼트 쉐이더
            // ------------------------------------

            half4 frag (Varyings input) : SV_Target
            {
                // 1. 화면 UV 준비
                float2 screenUV = input.uv;
                
                // _FlipY 처리 (빌트인 쉐이더의 screenPos.y = lerp(screenUV.y, 1 - screenUV.y, _FlipY); 로직 반영)
                // URP는 보통 Y축이 뒤집히지 않지만, Image Effect의 호환성을 위해 유지합니다.
                screenUV.y = lerp(screenUV.y, 1 - screenUV.y, _FlipY);

                // 2. 글리치 노이즈 값 계산 (Time 값은 _Time.y 대신 Get = _Time.y * 100을 사용하여 오버플로우 방지)
                float timeValue = _Time.y * 100.0;
                
                // _GlitchTex를 샘플링할 UV 좌표에 시간과 GlitchCutAmount 적용
                float2 glitchTexUV = float2(
                    screenUV.x * _GlitchCutAmountX + timeValue, 
                    screenUV.y * _GlitchCutAmountY + sin(timeValue)
                );
                
                // _GlitchTex에서 노이즈 값 추출
                half GlitchUV = SAMPLE_TEXTURE2D(_GlitchTex, sampler_GlitchTex, glitchTexUV).r;
                
                // 최종 UV 오프셋 양 계산
                half UV = GlitchUV * _GlitchAmount;
                
                // 글리치 색상 블렌딩을 위한 최종 Amount (0~1 범위로 clamp)
                half GlitchAmountFinal = saturate(_GlitchAmount * 10);

                // 3. 색상 분리 및 샘플링
                
                // Red 채널: X축 +UV 오프셋 적용
                // fixed3 r = SAMPLE_TEXTURE2D(_GrabTexture, sampler_GrabTexture, float2(screenUV.x + UV, screenUV.y)).r
                //          * lerp(fixed3(1, 0, 0), _GlitchColor1, GlitchAmountFinal);
                
                // Green 채널: X축 -UV 오프셋 적용
                // fixed3 g = SAMPLE_TEXTURE2D(_GrabTexture, sampler_GrabTexture, float2(screenUV.x - UV, screenUV.y)).g
                //          * lerp(fixed3(0, 1, 0), _GlitchColor2, GlitchAmountFinal);
                
                // Blue 채널: Y축 +UV 오프셋 적용
                // fixed3 b = SAMPLE_TEXTURE2D(_GrabTexture, sampler_GrabTexture, float2(screenUV.x, screenUV.y + UV)).b
                //          * lerp(fixed3(0, 0, 1), _GlitchColor3, GlitchAmountFinal);

                // 4. 최종 색상 조합
                half3 GlitchFinal = r + g + b;
                
                // 결과 반환 (투명도 없음, Alpha = 1)
                return half4(GlitchFinal, 1.0);
            }
            ENDHLSL
        }
    }
    // Fallback 제거
}