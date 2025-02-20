// Made with  1130165192@qq.com
Shader "Custom/piDai_xsy"
{
	Properties
	{
		_A("Albedo", 2D) = "white" {}
		_N("NormalMap", 2D) = "bump" {}
		_M("Metallic OCC Smoothness", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
		[Toggle]_FlowSwitch("Flow Y OR N", Float) = 0
		_FlowSpeed("Flow Speed", Range( -1 , 1)) = -0.1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _N;
		uniform float _FlowSwitch;
		uniform float _FlowSpeed;
		uniform sampler2D _A;
		uniform sampler2D _M;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 appendResult13 = (float4(0.0 , (( _FlowSwitch )?( 0.0 ):( _FlowSpeed )) , 0.0 , 0.0));
			float2 panner9 = ( 1.0 * _Time.y * appendResult13.xy + i.uv_texcoord);
			o.Normal = UnpackNormal( tex2D( _N, panner9 ) );
			o.Albedo = tex2D( _A, panner9 ).rgb;
			float4 tex2DNode12 = tex2D( _M, panner9 );
			o.Metallic = tex2DNode12.r;
			o.Smoothness = tex2DNode12.a;
			o.Occlusion = tex2DNode12.g;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
-1920;-182;1920;1019;2132.373;663.5273;1.6;True;True
Node;AmplifyShaderEditor.RangedFloatNode;15;-1714.824,360.83;Inherit;False;Property;_FlowSpeed;FlowSpeed;1;0;Create;True;0;0;0;False;0;False;-0.1;0.4;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;16;-1403.923,363.4298;Inherit;False;Property;_FlowSwitch;FlowSwitch;0;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1273.389,208.8005;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;13;-1127.179,334.935;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;9;-910.9861,206.7853;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;10;-423.0182,-101.7174;Inherit;True;Property;_A;A;2;0;Create;True;0;0;0;False;0;False;-1;None;6f0dbe174001d214499d6a82742c16a8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-418.1555,394.2235;Inherit;True;Property;_M;M;4;0;Create;True;0;0;0;False;0;False;-1;None;355811fa49609604db39b573f4fcce1e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-419.0236,146.4808;Inherit;True;Property;_N;N;3;0;Create;True;0;0;0;False;0;False;-1;None;739ce4e802725b241ad711b64750f27a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;17;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/FlowB_lzy;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;15;0
WireConnection;13;1;16;0
WireConnection;9;0;8;0
WireConnection;9;2;13;0
WireConnection;10;1;9;0
WireConnection;12;1;9;0
WireConnection;11;1;9;0
WireConnection;17;0;10;0
WireConnection;17;1;11;0
WireConnection;17;3;12;1
WireConnection;17;4;12;4
WireConnection;17;5;12;2
ASEEND*/
//CHKSM=47C09C9CAD7FAA5F13C610B5D25D896E52FBA50F