Shader "Custom/Line"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
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
        sampler2D _MainTex;
        
        struct Input {
        float3 worldPos;
        };
        
         float2x2 rotate2d(float _angle){
            return float2x2(cos(_angle),-sin(_angle),
                            sin(_angle),cos(_angle));
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
        
        float arrow_triangle(float2 worldPos,
                            float body, float head, float height,
                            float linewidth, float antialias, 
                            float2 start, float2 end, float debug) {
            float w = linewidth/2.0 + antialias;
            
            // Body : 1 segment
            float d4 = segment_distance(worldPos,
            start, end - float2(linewidth,0.0));
            
            // Head : 3 lines
            const float PI = 3.14159265;    
            // Number of sides of your shape
            int N = 3;
            
            // Angle and radius from the current pixel
            float2 relEnd = worldPos - end;
            float angle = atan2(relEnd.x,relEnd.y)+PI;
            float radius = (PI* 2)/float(N);
            float d5 = 0.0;
            
            // Shaping function that modulate the distance
            float2 shape = cos(floor(.5+angle/radius)*radius-angle);
            
            worldPos -= float2(0.500,0.500);
            worldPos  = mul(rotate2d(angle), worldPos);
            worldPos += float2(0.500,0.500);
            
            float2 loc = length(relEnd);
            d5 = shape*loc;
            mul(shape+loc, 
            rotate2d( (atan2(relEnd.x,relEnd.y)+ PI )+ _Time * debug ));
            
            //d5 =  mul( relEnd, rotate2d( sin(debug) * PI ));
            
            // REMOVING THE ARROW HEAD
            // ...rotation is messed up
            //d1 = 99;
            //d2 = 99;
            //d3 = -1;
            
            float d = min(d5, d4);
            return d4;
        }
        

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
