Shader "Indicators/Triangle"
{
    Properties
    {
        _StartPos ("Start Pos", Vector) = (0,0,0)
        _EndPos ("End Pos", Vector) = (0,0,0)
        _DebugVal("Debug Val", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        
        CGPROGRAM
        #pragma surface surf NoLighting noambient
        
         fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
             return fixed4(s.Albedo, s.Alpha);
         }
 
        float3 _BaseColor;
        float3 _AreaColor;
        float _DebugVal;
        static const float _Scale = 1;
        
        struct Input {
        float3 worldPos;
        };
         
         float2x2 rotate2d(float _angle){
            return float2x2(cos(_angle),-sin(_angle),
                            sin(_angle),cos(_angle));
         }
         
         float2x2 scale(float2 _scale){
             return float2x2(_scale.x,0.0,
                         0.0,_scale.y);
         }

        void surf (Input IN, inout SurfaceOutput o) {                        
            const float PI = 3.14159265;    
            // Number of sides of your shape
            int N = 3;
            
            // Angle and radius from the current pixel
            float angle = atan2(IN.worldPos.x,IN.worldPos.z)+PI;
            float radius = (PI* 2)/float(N);
            float d = 0.0;
            
            // Shaping function that modulate the distance
            d = cos(floor(.5+angle/radius)*radius-angle)*length(IN.worldPos);
            
            d =  mul( IN.worldPos, rotate2d( sin(_DebugVal)*PI ));
            
            d /= _Scale;
            o.Albedo = d;
            //if (d > 1)
            //o.Albedo =  _BaseColor;
            //else
            //o.Albedo = _AreaColor;
        }
        ENDCG
    }
}
