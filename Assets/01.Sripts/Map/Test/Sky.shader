Shader "Skybox/PanoramicBlendSingle_URP"
{
    Properties
    {
        // Inspector에 노출되는 속성들 -----------------
        _Tex1 ("Panoramic Day", 2D) = "white" {}     // 낮 HDRI
        _Tex2 ("Panoramic Sunset", 2D) = "gray" {}   // 저녁 HDRI
        _Tex3 ("Panoramic Night", 2D) = "black" {}   // 밤 HDRI
        _Blend ("Day→Night Blend", Range(0,1)) = 0   // 0=낮, 0.5=저녁, 1=밤
        _Exposure ("Exposure", Range(0,8)) = 1.0      // 밝기
        _Rotation ("Rotation", Range(0,360)) = 0      // 수평 회전
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Background" "RenderType"="Background" }
        ZWrite Off Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // 텍스처 선언 --------------------------------
            TEXTURE2D(_Tex1); SAMPLER(sampler_Tex1); // 낮
            TEXTURE2D(_Tex2); SAMPLER(sampler_Tex2); // 저녁
            TEXTURE2D(_Tex3); SAMPLER(sampler_Tex3); // 밤
            float _Blend, _Exposure, _Rotation;

            // 정점 구조체 --------------------------------
            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings { float4 positionHCS : SV_POSITION; float3 dirWS : TEXCOORD0; };

            // 정점 셰이더 --------------------------------
            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.dirWS = IN.positionOS.xyz;
                return OUT;
            }

            // 픽셀 셰이더 --------------------------------
            half4 frag (Varyings IN) : SV_Target
            {
                float3 d = normalize(IN.dirWS);
                float theta = atan2(d.x, d.z);
                float phi = acos(d.y);

                float2 uv;
                uv.x = frac((theta / (2.0 * PI)) + 0.5 + (_Rotation / 360.0));
                uv.y = 1.0 - (phi / PI);

                // 각 텍스처 샘플
                half4 day    = SAMPLE_TEXTURE2D(_Tex1, sampler_Tex1, uv);
                half4 sunset = SAMPLE_TEXTURE2D(_Tex2, sampler_Tex2, uv);
                half4 night  = SAMPLE_TEXTURE2D(_Tex3, sampler_Tex3, uv);

                // 0~0.5 구간 → 낮→저녁, 0.5~1 구간 → 저녁→밤
                half t = saturate(_Blend * 2.0);           // 0~1 스케일 (낮→저녁)
                half t2 = saturate((_Blend - 0.5) * 2.0);  // 0~1 스케일 (저녁→밤)

                half4 dayToSunset = lerp(day, sunset, t);  // 낮→저녁
                half4 finalCol = lerp(dayToSunset, night, t2); // 저녁→밤

                return finalCol * _Exposure;
            }
            ENDHLSL
        }
    }
}
