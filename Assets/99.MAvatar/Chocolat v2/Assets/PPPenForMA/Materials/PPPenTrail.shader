Shader "PPPen/Trail_Toon_Cutout"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("AlphaCutoff", Range(0,1)) = 0.5

        [Space(40)]
        [Toggle]
        _ZWrite ("ZWrite", float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] 
        _BlendSrc ("SrcBlend RGB", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] 
        _BlendDst ("DstBlend RGB", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="AlphaTest" "Queue"="AlphaTest" "VRCFallback"="Hidden"}
        Cull Off
        ZWrite [_ZWrite]
        LOD 100
        Blend [_BlendSrc] [_BlendDst]

        Pass
        {
            CGPROGRAM
            #pragma shader_feature_local _ _ALPHATEST_ON
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma instancing_options procedural:vertInstancingSetup
            #include "UnityCG.cginc"
            #include "UnityStandardParticleInstancing.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2g
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                half4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            struct g2f
            {
                float2 uv :TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                half4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Threshold;
            float4 _Color;

            v2g vert (appdata v)
            {
                v2g o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.color = half4(GammaToLinearSpace(v.color.rgb), v.color.a);                
#ifdef UNITY_PARTICLE_INSTANCING_ENABLED
                vertInstancingColor(o.color);
                o.color.rgb = min(1, o.color.rgb);
                vertInstancingUVs(v.uv, o.uv);
#endif
                return o;
            }

            [maxvertexcount(3)]
            void geom (triangle v2g input[3], inout TriangleStream<g2f> outStream)
            {
                for (int i=0; i<3; i++)
                {
                    g2f o;
                    o.uv = input[i].uv;
                    o.vertex = input[i].vertex;
                    o.color = input[i].color;
                    o.color.w = min(min(input[0].color.w, input[1].color.w),input[2].color.w);
                    DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(input[i], o);
                    UNITY_TRANSFER_FOG(o, o.vertex);
                    outStream.Append(o);
                }
                outStream.RestartStrip();
            }

            fixed4 frag (g2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                // sample the texture
                fixed4 albedo = tex2D(_MainTex, i.uv);
                albedo *= i.color;
                clip(albedo.w-_Threshold);
                albedo.a = 1;
                UNITY_APPLY_FOG(i.fogCoord, albedo);
                return albedo;
            }
            
            ENDCG
        }
    }
    Fallback "Fallback/Toon/Cutout"
}
