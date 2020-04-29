Shader "Custom/RotateUV" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Scale ("Scale", Float) = 1
        _Test("TEST", Vector) =(1,1,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
       
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
 
        sampler2D _MainTex;     
        static const float _PI = 3.14159265;  
        float _Scale;  
        float3 _Center;
        float3 _DebugCenter;
        float3 _Test;
 
        struct Input {
            float2 uv_MainTex;
        };
 
        float2x2 rotate2d(float _angle){
            return float2x2(cos(_angle),-sin(_angle),
                            sin(_angle),cos(_angle));
        }
        
        float2x2 scale(float2 _scale){
            return float2x2(_scale.x,0.0,
                        0.0,_scale.y);
        }
         
        void vert (inout appdata_full v) {
            _Scale /= pow(_Scale, 2);
            //v.texcoord.xy -= float2(0.500,0.500);
            //v.texcoord.xy  = mul(rotate2d(debugNum), v.texcoord.xy);
            //v.texcoord.xy += float2(0.500,0.500);
            
            float2 center = float2 (_DebugCenter.x,_DebugCenter.z) / unity_ObjectToWorld[0][0];
            v.texcoord.xy += center;

            float2 worldScaleMatrixAdjusted = unity_ObjectToWorld[0][0] * _Scale; 

            v.texcoord.xy -= float2(0.5,0.5);
            v.texcoord.xy = mul(scale(worldScaleMatrixAdjusted), v.texcoord.xy);
            v.texcoord.xy += float2(0.5,0.5);            
        }
 
        void surf (Input IN, inout SurfaceOutput o) {  
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}