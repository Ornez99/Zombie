Shader "Custom/4DifferentTextures"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		// Splat Map Control Texture
		_Control("Control (RGBA)", 2D) = "red" {}

		//Textures
		_Splat0("Layer 0 (R)", 2D) = "white" {}
		_Splat1("Layer 1 (G)", 2D) = "white" {}
		_Splat2("Layer 2 (B)", 2D) = "white" {}
		_Splat3("Layer 3 (A)", 2D) = "white" {}
	}

		SubShader{
			Tags {
			"SplatCount" = "4"
			"Queue" = "Geometry-100"
			"RenderType" = "Opaque"
			}
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows

			#pragma target 4.0

			sampler2D _Control;
			sampler2D _Splat0;
			sampler2D _Splat1;
			sampler2D _Splat2;
			sampler2D _Splat3;

		struct Input
		{
			float2 uv_Control : TEXCOORD0;
			float2 uv_Splat0 : TEXCOORD1;
			float2 uv_Splat1 : TEXCOORD2;
			float2 uv_Splat2 : TEXCOORD3;
			float2 uv_Splat3 : TEXCOORD4;
		};

		fixed4 _Color;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_Control, IN.uv_Control) * _Color;
			fixed3 color = fixed3(0, 0, 0);
			color += c.r * tex2D(_Splat0, IN.uv_Splat0).rgb;
			color += c.g * tex2D(_Splat1, IN.uv_Splat1).rgb;
			color += c.b * tex2D(_Splat2, IN.uv_Splat2).rgb;
			color += c.a * tex2D(_Splat3, IN.uv_Splat3).rgb;
			//clip(0.5 - c.a);
			o.Albedo = color;
			o.Alpha = c.a;
			//o.Albedo = tex2D(_Control, IN.uv_Control) * _Color;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
