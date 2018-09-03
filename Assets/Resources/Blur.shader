Shader "Hidden/Blur" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}
	CGINCLUDE //Code that is identical in several passes/subshaders can be declared with CGINCLUDE outside of their scope
	#pragma vertex vert_img //Built-in declaration of appdata and v2f, sending just position and uv (texture coordinate)
	#pragma fragment frag
	#include "UnityCG.cginc"

	float2 _BlurSize;
	sampler2D _MainTex;
	ENDCG

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass { //Horizontal
			CGPROGRAM
			fixed4 frag (v2f_img IN) : SV_Target {
				fixed4 s = tex2D(_MainTex, IN.uv) * 0.38774;
				s += tex2D(_MainTex, IN.uv + float2(_BlurSize.x * 2, 0)) * 0.06136;
				s += tex2D(_MainTex, IN.uv + float2(_BlurSize.x, 0)) * 0.24477;
				s += tex2D(_MainTex, IN.uv + float2(_BlurSize.x * -1, 0)) * 0.24477;
				s += tex2D(_MainTex, IN.uv + float2(_BlurSize.x * -2, 0)) * 0.06136;

				return s;
			}
			ENDCG
		}
		Pass { //Vertical
			CGPROGRAM
			fixed4 frag (v2f_img IN) : SV_Target {
				fixed4 s = tex2D(_MainTex, IN.uv) * 0.38774;
				s += tex2D(_MainTex, IN.uv + float2(0, _BlurSize.y * 2)) * 0.06136;
				s += tex2D(_MainTex, IN.uv + float2(0, _BlurSize.y)) * 0.24477;			
				s += tex2D(_MainTex, IN.uv + float2(0, _BlurSize.y * -1)) * 0.24477;
				s += tex2D(_MainTex, IN.uv + float2(0, _BlurSize.y * -2)) * 0.06136;

				return s;
			}
			ENDCG
		}
	}
}
