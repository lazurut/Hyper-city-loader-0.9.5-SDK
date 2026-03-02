Shader "Ultimate 10+ Shaders/Ocean" {
	Properties {
		_Color ("Color", Vector) = (0,0.25,0.35,0)
		_Normal1 ("Normal Map (1)", 2D) = "white" {}
		_NormalStrength1 ("Normal Strength (1)", Range(0, 2)) = 0.17
		_FlowDirection1 ("Flow Direction (1)", Vector) = (0.05,0,0,1)
		_Normal2 ("Normal Map (2)", 2D) = "white" {}
		_NormalStrength2 ("Normal Strength (2)", Range(0, 2)) = 0.8
		_FlowDirection2 ("Flow Direction (2)", Vector) = (0,0.05,0,1)
		_Glossiness ("Smoothness", Range(0, 1)) = 0.6
		_Metallic ("Metallic", Range(0, 1)) = 0.2
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_MatrixMVP;

			struct Vertex_Stage_Input
			{
				float3 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixMVP, float4(input.pos, 1.0));
				return output;
			}

			float4 _Color;

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				return _Color; // RGBA
			}

			ENDHLSL
		}
	}
	Fallback "Diffuse"
}