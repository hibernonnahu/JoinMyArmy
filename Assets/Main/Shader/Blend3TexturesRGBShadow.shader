Shader "Custom/Blend3TexturesRGBShadow"
{
    Properties
    {
        _MainTex1 ("Texture 1", 2D) = "white" {}
        _MainTex2 ("Texture 2", 2D) = "white" {}
        _MainTex3 ("Texture 3", 2D) = "white" {}
        _ControlTex ("Control Texture", 2D) = "white" {}
        
        _Tiling1 ("Tiling Texture 1", Vector) = (1, 1, 0, 0)
        _Tiling2 ("Tiling Texture 2", Vector) = (1, 1, 0, 0)
        _Tiling3 ("Tiling Texture 3", Vector) = (1, 1, 0, 0)
        _TilingControl ("Tiling Control Texture", Vector) = (1, 1, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex1;
        sampler2D _MainTex2;
        sampler2D _MainTex3;
        sampler2D _ControlTex;

        float4 _Tiling1;
        float4 _Tiling2;
        float4 _Tiling3;
        float4 _TilingControl;

        struct Input
        {
            float2 uv_MainTex1;
            float2 uv_MainTex2;
            float2 uv_MainTex3;
            float2 uv_ControlTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Calcula las coordenadas UV con tiling y offset para cada textura
            float2 uv1 = IN.uv_MainTex1 * _Tiling1.xy + _Tiling1.zw;
            float2 uv2 = IN.uv_MainTex2 * _Tiling2.xy + _Tiling2.zw;
            float2 uv3 = IN.uv_MainTex3 * _Tiling3.xy + _Tiling3.zw;
            float2 uvControl = IN.uv_ControlTex * _TilingControl.xy + _TilingControl.zw;

            // Muestras las texturas con las coordenadas ajustadas
            float4 tex1 = tex2D(_MainTex1, uv1);
            float4 tex2 = tex2D(_MainTex2, uv2);
            float4 tex3 = tex2D(_MainTex3, uv3);
            
            // Muestra la textura de control
            float3 control = tex2D(_ControlTex, uvControl).rgb;
            
            // Normaliza los valores de control para que la mezcla sume 1
            float total = control.r + control.g + control.b;
            if (total > 0.0001)
            {
                control /= total; // Normaliza para que la suma de los pesos sea 1
            }
            else
            {
                control = float3(1.0/3.0, 1.0/3.0, 1.0/3.0); // Si la suma es muy baja, usa pesos iguales
            }
            
            // Mezcla las tres texturas seg�n los pesos de control
            float4 blendedColor = tex1 * control.r + tex2 * control.g + tex3 * control.b;

            // Asigna el color mezclado al color de salida del shader de superficie
            o.Albedo = blendedColor.rgb;
            o.Alpha = blendedColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
