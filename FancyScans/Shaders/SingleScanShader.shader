Shader "FancyScans/ScanShader" {
	Properties{
		//mesh color
		_MeshColor("MeshColor", Color) = (1.0,1.0, 1.0, 0)
		//scan origin
		_ScanOrigin("Scan Origin", Vector) = (0,0,0,0)
		//scan default color
		_ScanColor("Scan Color", Color) = (1.0, 1.0, 1.0, 1.0)
		//scan distance from center
		_ScanDistance("Scan Distance", float) = 0
		//scan line width
		_ScanWidth("Scan Width", float) = 10
	}
	
	SubShader{
	Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
	LOD 200
	Pass{ ZWrite On ColorMask 0 }
	
	CGPROGRAM
	#pragma surface surf Lambert vertex:vert alpha noforwardadd
	#pragma target 3.0
	//background mesh color
	fixed4 _MeshColor;
	//scan origin
	float3 _ScanOrigin;
	//scan color
	half4 _ScanColor;
	//scan distance from center
	float _ScanDistance;
	//scan line width
	float _ScanWidth;

	struct Input {
		float3 worldPos;
		float3 localPos;
		float2 uv_MainTex;
	};

	void vert(inout appdata_full v, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.localPos = v.vertex.xyz;
	}

	void surf(Input IN, inout SurfaceOutput o) {
		float3 worldPosIgnoreY = float3(IN.worldPos.x, 0, IN.worldPos.z);
		float dist = distance(worldPosIgnoreY, _ScanOrigin);
		fixed isFragOnScanLine = max(0, sign(_ScanDistance - dist)) * max(0, sign(dist - (_ScanDistance - _ScanWidth)));
		o.Albedo = isFragOnScanLine * _ScanColor.rgb + ( 1 - isFragOnScanLine ) * _MeshColor.rgb;
		o.Alpha = isFragOnScanLine * _ScanColor.a + ( 1 - isFragOnScanLine ) * _MeshColor.a;
	}
	ENDCG
	}
	FallBack "Diffuse"
}