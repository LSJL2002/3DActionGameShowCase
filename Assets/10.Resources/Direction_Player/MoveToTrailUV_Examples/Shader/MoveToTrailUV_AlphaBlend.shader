Shader "MoveToTrailUV/MoveToTrailUV_AlphaBlend_Builtin"
{
    Properties
    {
        _MainTex("Main Texture (RGBA)", 2D) = "white" {}
        _MainTexAPow("MainTex AlphaGamma", Float) = 1
        _SoftAlpha("Soft Alpha", Range(0,1)) = 1
        _TintTex("Tint Texture (RGB)", 2D) = "white" {}
        _MainScrollSpeedU("Main Scroll U Speed", Float) = 10
        _MainScrollSpeedV("Main Scroll V Speed", Float) = 0
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MainTexAPow;
            float _SoftAlpha;
            float _MainScrollSpeedU;
            float _MainScrollSpeedV;
            float _MoveToMaterialUV;

            sampler2D _TintTex;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.x -= frac(_Time.x * _MainScrollSpeedU) + _MoveToMaterialUV;
                o.uv.y -= frac(_Time.x * _MainScrollSpeedV);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 mainTex = tex2D(_MainTex, i.uv);

                // Alpha 처리
                mainTex.a = pow(abs(mainTex.a), _MainTexAPow);
                float toonAlpha = saturate((mainTex.a - (1 - i.color.a)) / _SoftAlpha);
                float alpha = mainTex.a * i.color.a;
                float alphaMix = lerp(alpha, toonAlpha, i.color.a);

                fixed4 tintCol = tex2D(_TintTex, float2(alphaMix, 0.5));

                fixed4 col;
                col.rgb = lerp(tintCol.rgb * mainTex.rgb, tintCol.rgb, i.color.a);
                col.rgb *= i.color.rgb;
                col.a = alphaMix;

                return col;
            }
            ENDCG
        }
    }
}