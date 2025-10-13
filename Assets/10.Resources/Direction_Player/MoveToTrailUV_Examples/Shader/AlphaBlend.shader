Shader "MoveToTrailUV/AlphaBlend_Builtin"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _AlphaMap("Alpha Map", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            sampler2D _BaseMap;
            float4 _BaseMap_ST;

            sampler2D _AlphaMap;
            float4 _AlphaMap_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = TRANSFORM_TEX(v.uv, _BaseMap);
                o.uv2 = TRANSFORM_TEX(v.uv, _AlphaMap);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_BaseMap, i.uv);
                fixed4 alpha = tex2D(_AlphaMap, i.uv2);
                color.a = alpha.r; // 알파맵의 빨강 채널 사용
                return color;
            }
            ENDCG
        }
    }
}
