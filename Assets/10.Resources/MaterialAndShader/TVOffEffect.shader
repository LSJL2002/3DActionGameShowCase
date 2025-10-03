Shader "Custom/TVOffEffect"
{
    Properties
    {
        _Color ("Tint Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _ScaleFactor ("Scale Factor", Range(0, 1)) = 1 // 1이면 원본, 0이면 꺼짐
        _LineThickness ("Line Thickness", Range(0, 0.2)) = 0.02 // 꺼질 때 남는 하얀 선 두께
        _FlashStrength ("Flash Strength", Range(0, 5)) = 2 // 잔광 플래시 강도
        _FlashThreshold ("Flash Threshold", Range(0, 0.1)) = 0.02 // 플래시 발생 구간
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 localPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _ScaleFactor;
            float _LineThickness;
            float _FlashStrength;
            float _FlashThreshold;

            v2f vert(appdata v)
        {
            v2f o;
        
            // UI 이미지 중심 기준으로 Y 스케일
            float pivotY = 0.5; // RectTransform pivot
            float3 pos = v.vertex.xyz;
        
            // v.vertex.y는 오브젝트의 로컬 좌표 (0~height)
            // 중심 기준으로 이동 후 스케일링
            pos.y = (pos.y - pivotY) * _ScaleFactor + pivotY;
        
            o.localPos = v.vertex.xy;
            v.vertex.xyz = pos;
        
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            return o;
        }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // TV가 거의 꺼질 때 → 가로줄로 압축
                if (_ScaleFactor < 0.05)
                {
                    float lineMask = smoothstep(_LineThickness, 0.0, abs(i.localPos.y));
                    col.rgb = _Color.rgb;
                    col.a = 1.0 - lineMask;

                    // 잔광 플래시 효과 (스케일이 _FlashThreshold 이하일 때 발광)
                    if (_ScaleFactor < _FlashThreshold)
                    {
                        float flash = (_FlashThreshold - _ScaleFactor) / _FlashThreshold; // 0~1
                        col.rgb *= 1.0 + flash * _FlashStrength; // RGB 밝기 상승
                    }
                }

                // 스케일이 완전히 0이면 투명
                if (_ScaleFactor <= 0.001)
                {
                    col.a = 0;
                }

                return col;
            }
            ENDCG
        }
    }
}
