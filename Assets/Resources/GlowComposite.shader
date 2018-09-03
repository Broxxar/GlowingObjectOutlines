Shader "Hidden/GlowComposite" {
	Properties {
		_MainTex ("Texture", 2D) = "black" {}
	}
	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			CGPROGRAM
			#pragma vertex vert_img //Built-in declaration of appdata and v2f, sending just position and uv (texture coordinate)
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _GlowPrePassTex;
			sampler2D _GlowBlurredTex;
			float _Intensity;

			fixed4 frag (v2f_img IN) : SV_Target {
				fixed4 col = tex2D(_MainTex, IN.uv);

				#if !UNITY_UV_STARTS_AT_TOP //Invert Y in case of D3D?
	      			IN.uv.y = 1 - IN.uv.y; //Unnecessary? if (_MainTex_TexelSize.y < 0)
				#endif

				fixed4 glow = max(0, tex2D(_GlowBlurredTex, IN.uv) - tex2D(_GlowPrePassTex, IN.uv));
				return col + glow * _Intensity;
			}
			ENDCG
		}
	}
}
