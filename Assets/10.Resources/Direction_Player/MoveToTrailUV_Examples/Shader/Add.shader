Shader "MoveToTrailUV/Add_Builtin"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _Multiplier("Multiplier", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend One One // Additive
        ZWrite Off

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
                float2 uv : TEXCOORD0;
            };

            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            float _Multiplier;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_BaseMap, i.uv);
                col.rgb *= _Multiplier;
                col.rgb *= col.a; // 알파 보정
                col.a = 1; // Additive 블렌딩에서는 알파 무시
                return col;
            }
            ENDCG
        }
    }
}