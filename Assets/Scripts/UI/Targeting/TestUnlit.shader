Shader "Debug/TestUnlit"
 {
    Properties {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _AreaColor ("Area Color", Color) = (1, 1, 1)
        _Center ("Center", Vector) = (0,0,0,0)
        _Radius ("Radius", Range(0, 500)) = 20
        _Border ("Border", Range(0, 100)) = 5
    }
    
    SubShader {
        Tags { "RenderType"="Opaque" }
       
        CGPROGRAM
        #pragma surface surf NoLighting noambient
        
         fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
             return fixed4(s.Albedo, s.Alpha);
         }
 
        fixed4 _BaseColor;
        
        struct Input {
        fixed4 empty;
        };
         
        void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = _BaseColor.rgb;
        }
        ENDCG
        
        ZWrite Off      
        Blend DstColor Zero
        CGPROGRAM
        #pragma surface surf NoLighting noambient alpha:fade
        
         fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
             return fixed4(s.Albedo, s.Alpha);
         }
         
        fixed4 _BaseColor;
        sampler2D _MainTex;
        fixed3 _AreaColor;
        float3 _Center;
        float _Border;
        float _Radius;
 
        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };
        
        void surf (Input IN, inout SurfaceOutput o) {            
             half4 c = tex2D (_MainTex, IN.uv_MainTex);
//             float dist = distance(_Center, IN.worldPos);
//            
//            if (dist > _Radius && 
//                dist < (_Radius + _Border)) {
//                o.Albedo = _AreaColor.rgb;
//                o.Alpha = 0.5;
//            } else if (dist < _Radius) {
//                o.Albedo = _AreaColor.rgb;
//                o.Alpha = 0.25;
//            }
//            
//            else if(dist > (_Radius + _Border)){
//                o.Albedo = _BaseColor.rgb;
//                o.Alpha = 1;
//            }

            
            o.Albedo = _AreaColor.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
