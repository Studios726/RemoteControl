Shader "MyShader/PointCloud" 
{
    Properties 
    {
        _PointSize("Point Size", Range(1.0, 100.0)) = 10.0
    }
    SubShader 
    {
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float size : SIZE;
            };
 
            float _PointSize;
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.size = _PointSize;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(1.0, 1.0, 1.0, 1.0); // White point
            }
            ENDCG
        }
    }
}
