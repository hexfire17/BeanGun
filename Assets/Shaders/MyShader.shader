// Upgrade NOTE: replaced 'glstate.matrix.modelview[0]' with 'UNITY_MATRIX_MV'
// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'glstate.matrix.projection' with 'UNITY_MATRIX_P'

Shader "Custom/MyShader" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor ("Outline Color", Color) = (0,1,0,1)
		_Outline ("Outline Width", Range (0.02, 0.03)) = 0.1
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		UsePass "Diffuse/BASE"
		Pass
		{
			Name = "OUTLINE"
			Tags { "LightMode" = "Always" }

			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct appdata members vertex,normal)
#pragma exclude_renderers d3d11 xbox360

			struct appdata {
				float4 vertex;
				float3 normal;
			};

			struct v2f {
				float4 pos : POSITION;
				float4 color : COLOR;
				float fog : FOGC;
			};

			uniform float _Outline;
			uniform float4 _OutlineColor;

			v2f vert(appdata v){
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				float3 norm = mul ((float3x3)UNITY_MATRIX_MV, v.normal);
				norm.x *= UNITY_MATRIX_P[0][0];
				norm.y *= UNITY_MATRIX_P[1][1];
				p.pos.xy += norm.xy * o.pos.z * _OutlineColor;
				return o;
			}
			ENDCG

			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			SetTexture [_MainTex] { combine primary }
		}
	}
	FallBack "Diffuse"
}
