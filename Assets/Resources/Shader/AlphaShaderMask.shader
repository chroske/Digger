Shader "AlphaShaderMask" {
	Properties
	{
	        _MainTex ("Base (RGB)", 2D) = "white" {}
	}

    SubShader {
	    Tags { "RenderType"="Opaque" "Queue"="Geometry"} //ココらへんが原因？
	        ZWrite Off
	        AlphaTest Greater 0.5
	        ColorMask 0
	        ZTest Always

        Stencil {
            Ref 2 // リファレンス値
            Comp Always  // 常にステンシルテストをパスさせます。
            Pass Replace    // リファレンス値をバッファに書き込みます。
        }

        Pass {
        }
    } 
}