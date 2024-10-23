Shader "Custom/CutoutShadowShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:clip addshadow

        sampler2D _MainTex;
        float _Cutoff;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Obtén el color del mapa de textura principal
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // Aplica el recorte en función del valor de alfa y el umbral de recorte (_Cutoff)
            clip(c.a - _Cutoff);

            // Establece el color de salida (Albedo) y el alfa
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
