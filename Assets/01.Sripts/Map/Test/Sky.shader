Shader "Skybox/PanoramicBlend"
{
    Properties
    {
        // Inspector에 노출되는 속성들 -----------------
        _Tex1 ("Panoramic 1", 2D) = "white" {}   // 낮 HDRI 텍스처
        _Tex2 ("Panoramic 2", 2D) = "black" {}   // 밤 HDRI 텍스처
        _Blend ("Blend", Range(0,1)) = 0.0       // 낮~밤 보간 값 (0=낮, 1=밤)
        _Exposure ("Exposure", Range(0, 8)) = 1.0 // 전체 밝기
        _Rotation ("Rotation", Range(0,360)) = 0 // 수평 회전
        _FlipX ("Flip X", Float) = 0             // 좌우 반전 (0=끄기, 1=켜기)
        _FlipY ("Flip Y", Float) = 0             // 상하 반전 (0=끄기, 1=켜기)
    }
    SubShader
    {
        // Background Queue → 씬 배경으로 렌더링
        Tags { "Queue"="Background" "RenderType"="Background" }
        Cull Off ZWrite Off // 백페이스 제거, 깊이값 기록 안 함

        Pass
        {
            CGPROGRAM
            #pragma vertex vert   // 정점 셰이더
            #pragma fragment frag // 픽셀(프래그먼트) 셰이더
            #include "UnityCG.cginc"

            // 셰이더에서 사용할 변수들 -----------------
            sampler2D _Tex1;  // 낮 HDRI 텍스처
            sampler2D _Tex2;  // 밤 HDRI 텍스처
            float _Blend;     // 낮/밤 전환 값
            float _Exposure;  // 밝기
            float _Rotation;  // 회전
            float _FlipX;     // 좌우 반전 여부
            float _FlipY;     // 상하 반전 여부

            // 정점 데이터 (모델 → GPU)
            struct appdata { float4 vertex : POSITION; };

            // 정점 셰이더에서 픽셀 셰이더로 넘길 값
            struct v2f { 
                float4 pos : SV_POSITION; // 클립 공간 좌표
                float3 dir : TEXCOORD0;   // 방향 벡터
            };

            // 정점 셰이더 --------------------------------
            v2f vert (appdata v)
            {
                v2f o;
                // 클립 좌표로 변환 (화면 위치)
                o.pos = UnityObjectToClipPos(v.vertex);
                // 원래 정점 위치를 방향 벡터로 사용 (스카이박스용)
                o.dir = v.vertex.xyz;
                return o;
            }

            // 픽셀 셰이더 --------------------------------
            fixed4 frag (v2f i) : SV_Target
            {
                // 방향 벡터 정규화
                float3 d = normalize(i.dir);

                // 방향 벡터 → 구 좌표계 변환
                float theta = atan2(d.x, d.z); // 방위각 (-pi ~ pi)
                float phi = acos(d.y);         // 고도각 (0 ~ pi)

                // 구 좌표 → UV (0~1)
                float2 uv;
                uv.x = (theta / (2.0 * UNITY_PI)) + 0.5 + (_Rotation / 360.0);
                uv.y = 1.0 - (phi / UNITY_PI);

                // Flip 옵션 적용 -----------------
                if (_FlipX > 0.5) uv.x = 1.0 - uv.x; // 좌우 반전
                if (_FlipY > 0.5) uv.y = 1.0 - uv.y; // 상하 반전

                // UV 범위 보정 (0~1 유지)
                uv = frac(uv);

                // 낮/밤 텍스처 샘플링
                fixed4 col1 = tex2D(_Tex1, uv); 
                fixed4 col2 = tex2D(_Tex2, uv);

                // Blend 값에 따라 보간 후 Exposure 곱하기
                return lerp(col1, col2, _Blend) * _Exposure;
            }
            ENDCG
        }
    }
}
