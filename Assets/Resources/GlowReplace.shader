﻿Shader "Hidden/GlowReplace" {
	SubShader {
		Tags {
		"RenderType"="Opaque"
		"Glowable" = "True"
		}

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata { float4 vertex : POSITION; };
			struct v2f { float4 vertex : SV_POSITION; };
			
			v2f vert (appdata v){
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex); //Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
				return o;
			}
			
			fixed4 _GlowColor;

			fixed4 frag (v2f i) : SV_Target {
				return _GlowColor;
			}
			ENDCG
		}
	}
}
