Shader "Custom/VerticalScreenFade_CenterAndEdges_Fixed"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Fade("Edge Fade Strength", Range(0,1)) = 0.2
        _CenterY("Center Position", Range(0,1)) = 0.5
        _CenterSize("Center Band Size", Range(0,1)) = 0.2
        _EdgeSize("Edge Size", Range(0,0.5)) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Fade;
            float _CenterY;
            float _CenterSize;
            float _EdgeSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 screenUV : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;

                o.screenUV = (o.vertex.xy / o.vertex.w) * 0.5 + 0.5;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

                // 중앙부 거리 계산
                float distFromCenter = abs(i.screenUV.y - _CenterY);

                // 중앙 밴드 안: 완전 투명
                float centerAlpha = step(_CenterSize * 0.5, distFromCenter); // 중앙 벗어나면 1, 안쪽이면 0

                // 끝부분 근처 페이드
                float distToEdge = min(i.screenUV.y, 1.0 - i.screenUV.y);
                float edgeAlpha = smoothstep(0, _EdgeSize, distToEdge);
                edgeAlpha = 1.0 - edgeAlpha * _Fade; // 끝부분만 약간 감소

                // 중앙 제외 + 끝부분 페이드 적용
                float finalAlpha = centerAlpha * edgeAlpha;

                col.a *= finalAlpha * i.color.a;
                return col;
            }
            ENDCG
        }
    }
}