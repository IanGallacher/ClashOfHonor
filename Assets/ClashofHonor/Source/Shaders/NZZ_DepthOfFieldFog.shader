Shader "NZZ_Shader/NZZ_Depth" {
		SubShader {
	Tags { 
		"RenderType"="Opaque" 
	}
	ZWrite On
	ZTest LEqual 
	Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _CameraDepthTexture;

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
			};

			struct v2f {
			   float4 pos : SV_POSITION;
			   float4 scrPos:TEXCOORD1;
			   float4 color : COLOR;
			};

			//Vertex Shader
			v2f vert (appdata v){
			   v2f o;
			   o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			   o.scrPos=ComputeScreenPos(o.pos);
			   //for some reason, the y position of the depth texture comes out inverted
			   o.scrPos.y = o.scrPos.y;
			   o.color = v.color;
			   return o;
			}

			//Fragment Shader
			half4 frag (v2f i) : COLOR{
			   float depthValue = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
			   half4 depth;

			   depth.r = depthValue;
			   depth.g = depthValue;
			   depth.b = depthValue;

			   depth.a = i.color.a;
			   return i.color-((depth*i.color));
			}
			ENDCG
			}
	}
FallBack "Diffuse"
}