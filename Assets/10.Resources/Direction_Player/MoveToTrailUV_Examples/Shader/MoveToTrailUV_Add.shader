Shader "MoveToTrailUV/MoveToTrailUV_Add_Builtin"
{
    Properties
    {
        _MainTex("Main Texture (RGB)", 2D) = "white" {}
        _MainTexVFade("MainTex V Fade", Range(0,1)) = 0
        _MainTexVFadePow("MainTex V Fade Pow", Float) = 1
        _MainTexPow("Main Texture Gamma", Float) = 1
        _MainTexMultiplier("Main Texture Multiplier", Float) = 1
        _TintTex("Tint Texture (RGB)", 2D) = "white" {}
        _Multiplier("Multiplier", Float) = 1
        _MainScrollSpeedU("Main Scroll U Speed", Float) = 10
        _MainScrollSpeedV("Main Scroll V Speed", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend One One // Additive
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
                float2 uvOrigin : TEXCOORD1;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MainTexVFade;
            float _MainTexVFadePow;
            float _MainTexPow;
            float _MainTexMultiplier;

            sampler2D _TintTex;

            float _Multiplier;
            float _MainScrollSpeedU;
            float _MainScrollSpeedV;
            float _MoveToMaterialUV;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.x -= frac(_Time.x * _MainScrollSpeedU) + _MoveToMaterialUV;
                o.uv.y -= frac(_Time.x * _MainScrollSpeedV);
                o.uvOrigin = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Main texture
                fixed4 mainTex = tex2D(_MainTex, i.uv);

                // V Fade
                float vFade = 1 - abs(i.uvOrigin.y - 0.5) * 2;
                vFade = pow(abs(vFade), _MainTexVFadePow);
                vFade = lerp(1, vFade, _MainTexVFade);
                mainTex.rgb *= vFade;

                // Main texture processing
                mainTex.rgb = pow(abs(mainTex.rgb), _MainTexPow) * _MainTexMultiplier;

                // Vertex alpha * multiplier
                float intensity = _Multiplier * i.color.a;

                // Tint
                float avr = mainTex.r * 0.3333 + mainTex.g * 0.3334 + mainTex.b * 0.3333;
                avr = saturate(avr * intensity);
                fixed4 col = tex2D(_TintTex, float2(avr, 0.5));

                float intensityHigh = max(1, intensity);
                col.rgb *= intensityHigh * i.color.rgb;
                return col;
            }
            ENDCG
        }
    }
}