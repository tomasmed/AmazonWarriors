Shader "Custom/VertexColor" {
	Properties {
		//_MainTint("Global Color Tint", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert alpha

		sampler2D _MainTex;
		//float4 _MainTint;

		struct Input {
			float2 uv_MainTex;
			float4 vertColor;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.vertColor = v.color;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb  * IN.vertColor.rgb;// * _MainTint.rgb;
			o.Alpha = c.a * IN.vertColor.a;

		}
		ENDCG
	} 
	FallBack "Diffuse"
}
