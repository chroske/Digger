// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Red" {
    SubShader {
	    Tags { "RenderType"="Opaque" "Queue"="Geometry"}
	        ZWrite Off
	        AlphaTest Greater 0.5
	        ColorMask 0
	        ZTest Always
//        Tags { "RenderType"="Opaque" "Queue"="Geometry"}
//        ColorMask 0

        Stencil {
            Ref 2 // リファレンス値
            Comp Always  // 常にステンシルテストをパスさせます。
            //Comp Equal 
            Pass Replace    // リファレンス値をバッファに書き込みます。
        }

        Pass
        {
                CGPROGRAM
                        #pragma vertex vert
                        #pragma fragment frag

                        #include "UnityCG.cginc"

                        struct appdata_t
                        {
                                float4 vertex : POSITION;
                                float2 texcoord : TEXCOORD0;
                        };

                        struct v2f
                        {
                                float4 vertex : SV_POSITION;
                                half2 texcoord : TEXCOORD0;
                        };

                        sampler2D _MainTex;
                        float4 _MainTex_ST;

                        v2f vert (appdata_t v)
                        {
                                v2f o;
                                o.vertex = UnityObjectToClipPos(v.vertex);
                                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                                return o;
                        }

                        fixed4 frag (v2f i) : COLOR
                        {
                                fixed4 col = tex2D(_MainTex, i.texcoord);
                                if(col.a<0.1)discard;
                                return col;
                        }
                ENDCG
        }
//        Pass {
//
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            struct appdata {
//                float4 vertex : POSITION;
//            };
//            struct v2f {
//                float4 pos : SV_POSITION;
//            };
//            v2f vert(appdata v) {
//                v2f o;
//                o.pos = UnityObjectToClipPos(v.vertex);
//                return o;
//            }
//            half4 frag(v2f i) : SV_Target {
//                return half4(1,0,0,1);
//            }
//            ENDCG
//        }
    } 
}