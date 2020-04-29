Shader "Indicators/Targeting"
 {
    Properties {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _AreaColor ("Area Color", Color) = (1, 1, 1)
        _DebugEndPos ("Debug End Pos", Vector) = (1,1,1)
        _DebugVal ("Debug Val", Float) = 1
    }
    
    SubShader {
        Tags { "RenderType"="Transparent" }
       
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
        fixed3 _AreaColor;
        
        float _IndicatorType;
        
        float3 _Center;
        float3 _Offset;
        float _Border;
        float _Radius;
        
        float3 _StartPos;
        float3 _EndPos;
        float _Width;
        float _Body;
        float _Head;
        float _Height;
        
        float3 _DebugEndPos;
        float _DebugVal;
 
        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        float2x2 rotate2d(float _angle){
            return float2x2(cos(_angle),-sin(_angle),
                            sin(_angle),cos(_angle));
        }
        
        SurfaceOutput handleCircular(Input IN, inout SurfaceOutput o) {
            float dist = distance(_Center, IN.worldPos); 
            if (dist > _Radius && 
                dist < (_Radius + _Border)) {
                o.Albedo = _AreaColor.rgb;
                o.Alpha = 0.5;
            } else if (dist < _Radius) {
                o.Albedo = _AreaColor.rgb;
                o.Alpha = 0.25;
            }
            
            else if(dist > (_Radius + _Border)){
                o.Albedo = _BaseColor.rgb;
                o.Alpha = 1;
            }
            return o;
        }
        
        // Computes the signed distance from a line segment
        float segment_distance(float2 p, float2 start, float2 end) {
            float2 center = (start + end) * 0.5;
            float len = length(end - start);
            float2 dir = (end - start) / len;
            float2 rel_p = p - center;
            float dist1 = abs(dot(rel_p, float2(dir.y, -dir.x)));
            float dist2 = abs(dot(rel_p, dir)) - 0.5*len;
            return max(dist1, dist2);
        }
        
        // Computes the signed distance from a line
        float line_distance(float2 p, float2 p1, float2 p2) {
            float2 center = (p1 + p2) * 0.5;
            float len = length(p2 - p1);
            float2 dir = (p2 - p1) / len;
            float2 rel_p = p - center;
            return dot(rel_p, float2(dir.y, -dir.x));
        }
        
        // Computes the centers of a circle with
        // given radius passing through p1 & p2
        float4 inscribed_circle(float2 p1, float2 p2, float radius)
        {
            float q = length(p2-p1);
            float2 m = (p1+p2)/2.0;
            float2 d = float2( sqrt(radius*radius - (q*q/4.0)) * (p1.y-p2.y)/q,
            sqrt(radius*radius - (q*q/4.0)) * (p2.x-p1.x)/q);
            return float4(m+d, m-d);
        }

        float arrow(float2 texcoord, float2 startPos, float2 endPos, float _DebugVal){
            // will need to do the head later
            return segment_distance(texcoord, startPos,endPos);
        }
        
        SurfaceOutput handleLinear(Input IN, inout SurfaceOutput o) {                
            float2 texcoor = float2(IN.worldPos.x, IN.worldPos.z);
            float2 startPos = float2(_StartPos.x, _StartPos.z);
            float2 endPos = float2(_EndPos.x, _EndPos.z);
           
                                        
            bool shouldRender = arrow(texcoor, startPos, endPos, _DebugVal) < 1;
                                                         
            if ( shouldRender ) {
                o.Alpha = 0.5;
                o.Albedo = _AreaColor.rgb;
            }
            
            return o;
        }
        
        // Plot a line on Y using a value between 0.0-1.0
        float plot(float2 st, float pct){
        float thickness = 0.5; // _DebugVal; // 0.02
          return  smoothstep( pct - thickness, pct, st.y) -
                  smoothstep( pct, pct + thickness, st.y);
        }
        
        void surf (Input IN, inout SurfaceOutput o) { 
            if (_IndicatorType == 1) {
                handleCircular(IN, o);
            } else if (_IndicatorType == 2) {
                handleLinear(IN, o);
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
