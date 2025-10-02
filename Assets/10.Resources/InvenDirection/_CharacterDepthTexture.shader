Shader "Custom/DepthShadowColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        _HighlightColor ("Highlight Color", Color) = (1,1,0,1)
        _DepthCutoff ("Depth Cutoff", Range(0,1)) = 0.5
        _Softness ("Softness", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _ShadowColor;
            float4 _HighlightColor;
            float _DepthCutoff;
            float _Softness;
            float4 _MainTex_ST;

            sampler2D _CameraDepthTexture;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // 함수 이름 변경해서 충돌 방지
            float MyLinearDepth(float rawDepth)
            {
                return LinearEyeDepth(rawDepth) / _ProjectionParams.z; // _ProjectionParams.z = _CameraFar
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float sceneDepthRaw = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.pos.xy / _ScreenParams.xy);
                float linearDepth = MyLinearDepth(sceneDepthRaw);

                float shadowFactor = smoothstep(_DepthCutoff, _DepthCutoff + _Softness, linearDepth);

                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = lerp(_ShadowColor.rgb, _HighlightColor.rgb, shadowFactor);

                return col;
            }
            ENDCG
        }
    }
}