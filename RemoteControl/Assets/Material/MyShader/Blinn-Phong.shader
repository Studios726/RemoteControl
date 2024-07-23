// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "MyShader/Blinn-Phong with Vertex Color"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MainCol ("漫反射颜色", Color) = (0.15, 0.15, 0.15, 1.0)
        _SpecularCol("高光颜色", Color) = ( 0.2, 0.2, 0.3, 1.0)
        _SpecularPow ("高光次幂", Range(1, 90)) = 5.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        Pass
        {
            Name "FORWARD"
            Tags
            {
                "LightMode"="ForwardBase"
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Lighting.cginc"
            sampler2D _MainTex;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
                float3 normal : TEXCOORD2;
                float2 uv : TEXCOORD0;
                float3 worldPos:TEXCOORD1;
            };
            
            float4 _MainCol;
            float4 _SpecularCol;
            float _SpecularPow;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color * _MainCol; // Use vertex color multiplied by _MainCol
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = float2(0, 0); // Just set some dummy UVs for now
                o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float4 mainTexColor = tex2D(_MainTex, i.uv);
                float3 nDir = normalize(i.normal);
                float3 lDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 vDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
                float3 hDir = normalize(vDir + lDir);
                float ndotl = dot(nDir, lDir);
                float ndoth = dot(nDir, hDir);
                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
                float3 diffuse = _LightColor0.rgb * mainTexColor.rgb * max(0, ndotl);
                float3 specular = _LightColor0.rgb * _SpecularCol.rgb * pow(max(0, ndoth), _SpecularPow);
                float3 blinnPhong = ambient + diffuse + specular;
                return float4(blinnPhong * i.color.rgb, 1.0); // Multiply the final color by vertex color
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}