Shader "Hidden/Blur" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}
	CGINCLUDE //Code that is identical in several passes/subshaders can be declared with CGINCLUDE outside of their scope
	#pragma vertex vert_img //Built-in declaration of appdata and v2f for image effects, sending just position and uv (texture coordinate)
	#pragma fragment frag
	#include "UnityCG.cginc"

	float2 _BlurSize;
	sampler2D _MainTex;
	ENDCG

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass { //Horizontal
			CGPROGRAM
			half4 frag (v2f_img IN) : SV_Target {
				half4 s = tex2D(_MainTex, IN.uv) * 0.38774;
				fixed2 cordOffset = fixed2(_BlurSize.x, 0);
				s += tex2D(_MainTex, IN.uv + cordOffset * 2) * 0.06136;
				s += tex2D(_MainTex, IN.uv + cordOffset) * 0.24477;
				s += tex2D(_MainTex, IN.uv + cordOffset * -1) * 0.24477;
				s += tex2D(_MainTex, IN.uv + cordOffset * -2) * 0.06136;

				return s;
			}
			ENDCG
		}
		Pass { //Vertical
			CGPROGRAM
			half4 frag (v2f_img IN) : SV_Target {
				half4 s = tex2D(_MainTex, IN.uv) * 0.38774;
				fixed2 cordOffset = fixed2(0, _BlurSize.y);
				s += tex2D(_MainTex, IN.uv + cordOffset * 2) * 0.06136;
				s += tex2D(_MainTex, IN.uv + cordOffset) * 0.24477;
				s += tex2D(_MainTex, IN.uv + cordOffset * -1) * 0.24477;
				s += tex2D(_MainTex, IN.uv + cordOffset * -2) * 0.06136;

				return s;
			}
			ENDCG
		}
	}
}
