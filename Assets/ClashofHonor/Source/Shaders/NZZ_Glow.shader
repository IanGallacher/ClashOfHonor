
Shader "NZZ_Shader/NZZ_Glow" {
	Properties {
		_MyFloat ("My float", Float) = 0.5 
		_MainTex ("Texture", 2D) = "white" {}
	}
	
	Category {
		SubShader {
			Tags { "RenderType" = "Opaque" }
			Pass {
				CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
				#pragma exclude_renderers gles
				
//		        #pragma vertex vert
		        #pragma fragment frag
            	#include "UnityCG.cginc"
				//#pragma surface surf Lambert
				               
		        struct appdata_t {
		            float4 vertex : POSITION;
		            float2 texcoord: TEXCOORD0;
		        };
			    struct v2f {
		            float4 vertex : POSITION;
		            float4 uvgrab : TEXCOORD0;
		        };
				struct Input {
					float2 uv_MainTex;
				};
				sampler2D _MainTex;
		        float4 _MainTex_TexelSize;
		        float _Size;
				float _MyFloat; 
		        const float blurSize = 1.0/512.0;
//				void surf (Input IN, inout SurfaceOutput o) {
//					o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
//					o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
//				}
//				v2f vert (appdata_t v) {
//		            v2f o;
//		            o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
//		            #if UNITY_UV_STARTS_AT_TOP
//		            float scale = -1.0;
//		            #else
//		            float scale = 1.0;
//		            #endif
//		            o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
//		            o.uvgrab.zw = o.vertex.zw;
//		            return o;
//		        }
		       
		        sampler2D _GrabTexture;
		       
		        float4 frag( v2f_img  i ) : COLOR {
		//                  half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
		//                  return col;
		           
		            float4 sum = half4(0,0,0,0);
//
		            #define GRABPIXEL(weight,kernelx) tex2D( _MainTex, float2(i.uv.x + _MainTex_TexelSize.x * kernelx*2, i.uv.y) ) * _MyFloat
		            #define LOWFOCUS(weight,kernelx) tex2D( _MainTex, float2(i.uv.x + _MainTex_TexelSize.x * kernelx*2, i.uv.y) ) * weight * (_MyFloat)
		            sum += LOWFOCUS(0.05, -4.0);
		            sum += LOWFOCUS(0.09, -3.0);
		            sum += LOWFOCUS(0.12, -2.0);
		            sum += LOWFOCUS(0.15, -1.0);
		            sum += LOWFOCUS(0.18,  0.0);
		            sum += LOWFOCUS(0.15, +1.0);
		            sum += LOWFOCUS(0.12, +2.0);
		            sum += LOWFOCUS(0.09, +3.0);
		            sum += LOWFOCUS(0.05, +4.0);
//					sum = tex2D(_MainTex, i.uv);
		           
		            return sum;
		        }
				ENDCG
			}
			
		}
	} 
}
