Shader "Indicators/Targeting"
 {
    Properties {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _AreaColor ("Area Color", Color) = (1, 1, 1)
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
        
 
        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };
        
        SurfaceOutput handleCircular(Input IN, inout SurfaceOutput o) {
            //const float PI = 3.14159265;    
            float dist = distance(_Center, IN.worldPos);  
            //half4 c = tex2D (_MainTex, IN.uv_MainTex);
            // radius of 40 = range of 1 
            // range of 20 = radius of 
            //float toUse = sin(_Radius/10 * PI);
            //float2 coors = (toUse, toUse);
            
           // half4 c = tex2D (
           // _MainTex,
           // // scale the texture
           // (coors * IN.uv_MainTex) - 
           // // center the texture
           // ((coors / 2) - float2(0.5, 0.5)));
             
            //o.Albedo = c.rgb;
            //o.Alpha = c.a;
            
            
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
        
        float line_distance(float2 p, float2 p1, float2 p2) {
            float2 center = (p1 + p2) * 0.5;
            float len = length(p2 - p1);
            float2 dir = (p2 - p1) / len;
            float2 rel_p = p - center;
            return dot(rel_p, float2(dir.y, -dir.x));
        }
        
        // Computes the signed distance from a line segment
        float segment_distance(float2 p, float2 p1, float2 p2) {
            float2 center = (p1 + p2) * 0.5;
            float len = length(p2 - p1);
            float2 dir = (p2 - p1) / len;
            float2 rel_p = p - center;
            float dist1 = abs(dot(rel_p, float2(dir.y, -dir.x)));
            float dist2 = abs(dot(rel_p, dir)) - 0.5*len;
            return max(dist1, dist2);
        }
        
        float arrow_triangle(float2 texcoord,
                            float body, float head, float height,
                            float linewidth, float antialias, 
                            float2 start, float2 end, float debug) {
            float w = linewidth/2.0 + antialias;
            // Head : 3 lines
            float d1 = line_distance(texcoord,
            end, end - head*float2(+1.0,-height));
            float d2 = line_distance(texcoord,
            end - head*float2(+1.0,+height), end);
            float d3 = texcoord.x - end.x + head;
            
            // REMOVING THE ARROW HEAD
            // ...rotation is messed up
            //d1 = 99;
            //d2 = 99;
            d3 = -1;
            
            // Body : 1 segment
            float d4 = segment_distance(texcoord,
            start, end - float2(linewidth,0.0));
            float d = min(max(max(d1, d2), -d3), d4);
            return d;
        }
        
        SurfaceOutput handleLinear(Input IN, inout SurfaceOutput o) {
            _Body = 15;
            _Width = 2;
            _Head = 2;
            //_Height = 0.4;
                
            float2 curPos = float2(IN.worldPos.x, IN.worldPos.z);
            float2 startPos = float2(_StartPos.x, _StartPos.z);
            float2 endPos = float2(_EndPos.x, _EndPos.z);
           
            bool test = arrow_triangle(curPos, 15,  _Head,
                                        0.4, _Width,  0.001,
                                        startPos, endPos,
                                        _Height) < 1;    
                                            
            if ( test ) {
                o.Alpha = 0.5;
                o.Albedo = _AreaColor.rgb;
            }
            
            return o;
        }
        
        void surf (Input IN, inout SurfaceOutput o) { 
            if (_IndicatorType == 0) {
                handleCircular(IN, o);
            } else if (_IndicatorType == 1) {
                 handleLinear(IN, o);
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
