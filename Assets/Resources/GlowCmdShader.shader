﻿Shader "Hidden/GlowCmdShader" {
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata { float4 vertex : POSITION; };
			struct v2f { float4 vertex : SV_POSITION; };

			v2f vert (appdata v){
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 _GlowColor;

			fixed4 frag (v2f IN) : SV_Target {
				return _GlowColor;
			}
			ENDCG
		}
	}
}
