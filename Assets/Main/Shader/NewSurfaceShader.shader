Shader "Custom/NewSurfaceShader"
{
    Properties
    {
       
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

       

        struct Input
        {
            float2 uv_MainTex;
        };

       

      
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
           
        }
        ENDCG
    }
    FallBack "Diffuse"
}
