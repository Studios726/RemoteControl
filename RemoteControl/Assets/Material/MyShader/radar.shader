Shader "MyShader/radar"
{ 
    ///鼠标移动正方形
    Properties
    {
    _Center("Center",Vector)=(0,0,0,0)
    _Raduis("Radius",float)=0.1
    _AxisColor("AxisColor",Color)=(1,1,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"
        struct v2f{
            float4 vertex:SV_POSITION;
            float4 position:TEXCOORD1;
            float2 uv:TEXCOORD;
            float4 screenPos:TEXCOORD2;
        };
        v2f vert(appdata_base v){
            v2f o;
            o.vertex=UnityObjectToClipPos(v.vertex);
            o.position=v.vertex;
            o.uv=v.texcoord;
            o.screenPos=ComputeScreenPos(o.vertex);
            return o;
        }
        //中心点
        float4 _Center;
        //半径
        float _Raduis;

        fixed4 _AxisColor;
        //绘制正方形
        float rect(float2 pt,float2 size,float2 center){
            float2 p=pt-center;
            float2 halfsize=size*0.5;
            float hotz=step(-halfsize.x,p.x)-step(halfsize.x,p.x);
            float vert=step(-halfsize.y,p.y)-step(halfsize.y,p.y);
            return hotz*vert;
        }
        //普通的实心圆
        float circle(float2 pt,float2 center,float radius){
            float2 p=pt-center;
            return 1.0-step(radius,length(p));
        }
        //边缘软化圆
          float circle(float2 pt,float2 center,float radius,bool soften){
            float2 p=pt-center;
            float edge=(soften)?radius*0.05:0.0;
            return 1.0-smoothstep(radius-edge,radius+edge,length(p));
        }
         //轮廓圆
        float circle(float2 pt,float2 center,float radius,float line_width){
            float2 p=pt-center;
            float len=length(p);
            float half_line_width=line_width/2.0;

            return step(radius-half_line_width,len)-step(radius+half_line_width,len);
        }
        //画线
        float onLine(float a,float b,float line_width,float edge_thickness){
            float half_line_width=line_width*0.5;
            return smoothstep(a-half_line_width-edge_thickness,a-half_line_width,b)-smoothstep(a+half_line_width,a+half_line_width+edge_thickness,b);
        }
        //扫描线
        float sweep(float2 pt,float2 center,float radius,float line_width,float edge_thickness){
            float2 d=pt-center;
            float theta=_Time.z;
            float2 p=float2(cos(theta),-sin(theta))*radius;
            float h=clamp(dot(d,p)/dot(p,p),0.0,1.0);
            float l=length(d-p*h);
            float gradient=0.0;
            const float gradient_angle=UNITY_PI*0.5;
            if(length(d)<radius){
                float angle=fmod(theta+atan2(d.y,d.x),UNITY_TWO_PI);
                gradient=clamp(gradient_angle-angle,0,gradient_angle)/gradient_angle*0.5;
            }
            return gradient+1.0-smoothstep(line_width,line_width+edge_thickness,l);
        }
        

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pos=i.position*2;
                float2 size=0.2;
                float ouline1=onLine(i.uv.y,0.5,0.002,0.001)*_AxisColor;
                float ouline2=onLine(i.uv.x,0.5,0.002,0.001)*_AxisColor;
                
                float outlineCircle=circle(pos,_Center,_Raduis,0.01f);
                fixed3 col=ouline1+ouline2+outlineCircle;
                col+=circle(pos,_Center,_Raduis-0.1,0.01f);
                col+=circle(pos,_Center,_Raduis-0.2,0.01f);
                col+=sweep(pos,_Center,_Raduis,0.002,0.001)*_AxisColor;
                return fixed4(col,1.0);
            }
            ENDCG
        }
    }
}

