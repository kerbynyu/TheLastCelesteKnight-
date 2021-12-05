Shader "Unlit/Vintage"
{
 Properties
    {
        _MainTex ("render texture", 2D) = "white" {}
        _ditherPattern ("dither pattern", 2D) = "gray"{}
        _threshold ("threshold", Range (-0.5, 0.5)) = 0 
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex; float4 _MainTex_TexelSize; 
            sampler2D _ditherPattern; float4 _ditherPattern_TexelSize; 
            float _threshold; 

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float3 color = float3(0.960, 0.827, 0.619);

                float2 uv = i.uv;
                float3 newcolor = tex2D(_MainTex, uv);
               

                float3 grayscaleLuminence = float3(0.674, 0.729, 0.945);
                color = dot(tex2D(_MainTex, uv), grayscaleLuminence);

                color *= (newcolor);

      

                float distFromCenter = distance(i.uv.xy, float2(0.75, 0.75));
                float vingette = 1 - distFromCenter;
                return float4(color * vingette, 1.0);
            }

           
            ENDCG


        }
    }
}
