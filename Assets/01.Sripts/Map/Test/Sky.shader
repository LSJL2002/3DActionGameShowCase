Shader "Skybox/PanoramicBlend"
{
    Properties
    {
        _Tex1 ("Panoramic 1", 2D) = "white" {}
        _Tex2 ("Panoramic 2", 2D) = "black" {}
        _Blend ("Blend", Range(0,1)) = 0.0
        _Exposure ("Exposure", Range(0, 8)) = 1.0
        _Rotation ("Rotation", Range(0,360)) = 0
        _FlipX ("Flip X", Float) = 0
        _FlipY ("Flip Y", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _Tex1;
            sampler2D _Tex2;
            float _Blend;
            float _Exposure;
            float _Rotation;
            float _FlipX;
            float _FlipY;

            struct appdata { float4 vertex : POSITION; };
            struct v2f { float4 pos : SV_POSITION; float3 dir : TEXCOORD0; };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.dir = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 d = normalize(i.dir);

                float theta = atan2(d.x, d.z); // -pi ~ pi
                float phi = acos(d.y);         // 0 ~ pi

                float2 uv;
                uv.x = (theta / (2.0 * UNITY_PI)) + 0.5 + (_Rotation / 360.0);
                uv.y = 1.0 - (phi / UNITY_PI);

                // Flip 옵션 적용
                if (_FlipX > 0.5) uv.x = 1.0 - uv.x;
                if (_FlipY > 0.5) uv.y = 1.0 - uv.y;

                uv = frac(uv); // 0~1 범위 유지

                fixed4 col1 = tex2D(_Tex1, uv);
                fixed4 col2 = tex2D(_Tex2, uv);

                return lerp(col1, col2, _Blend) * _Exposure;
            }
            ENDCG
        }
    }
}
