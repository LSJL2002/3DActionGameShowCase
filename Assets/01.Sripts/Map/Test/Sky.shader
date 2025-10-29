Shader "Skybox/PanoramicBlendSingle_URP"
{
    Properties
    {
        _Tex1 ("Panoramic Day", 2D) = "white" {}
        _Tex2 ("Panoramic Sunset", 2D) = "gray" {}
        _Tex3 ("Panoramic Night", 2D) = "black" {}
        _Blend ("Day→Night Blend", Range(0,1)) = 0
        _Exposure ("Exposure", Range(0,8)) = 1.0
        _Rotation ("Rotation", Range(0,360)) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalRenderPipeline"
            "Queue"="Background"
            "UniversalMaterialType"="Skybox"   // ✅ Unity 6에서 필수
        }

        ZWrite Off
        Cull Off

        Pass
        {
            Name "SkyboxPass"                   // ✅ 명시적 이름도 필수적
            Tags { "LightMode" = "UniversalSkybox" } // ✅ URP가 Skybox Pass로 인식하게 함

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5
            #pragma multi_compile_instancing
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"

            TEXTURE2D(_Tex1); SAMPLER(sampler_Tex1);
            TEXTURE2D(_Tex2); SAMPLER(sampler_Tex2);
            TEXTURE2D(_Tex3); SAMPLER(sampler_Tex3);
            float _Blend, _Exposure, _Rotation;

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings { float4 positionHCS : SV_POSITION; float3 dirWS : TEXCOORD0; };

            float4 TransformObjectToHClip(float3 pos)
            {
                return mul(UNITY_MATRIX_MVP, float4(pos, 1.0));
            }

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.dirWS = IN.positionOS.xyz;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float3 d = normalize(IN.dirWS);
                float theta = atan2(d.x, d.z);
                float phi = acos(clamp(d.y, -1.0, 1.0));

                float2 uv;
                uv.x = frac((theta / (2.0 * PI)) + 0.5 + (_Rotation / 360.0));
                uv.y = 1.0 - (phi / PI);

                half4 day = SAMPLE_TEXTURE2D(_Tex1, sampler_Tex1, uv);
                half4 sunset = SAMPLE_TEXTURE2D(_Tex2, sampler_Tex2, uv);
                half4 night = SAMPLE_TEXTURE2D(_Tex3, sampler_Tex3, uv);

                half t = saturate(_Blend * 2.0);
                half t2 = saturate((_Blend - 0.5) * 2.0);

                half4 dayToSunset = lerp(day, sunset, t);
                half4 finalCol = lerp(dayToSunset, night, t2);

                return finalCol * _Exposure;
            }
            ENDHLSL
        }
    }
}
