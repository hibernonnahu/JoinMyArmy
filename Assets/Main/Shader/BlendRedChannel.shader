Shader "Unlit/BlendRedChannel"
{
    Properties
    {
        _MainTex1 ("Texture 1", 2D) = "white" {}
        _MainTex2 ("Texture 2", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}

        _MainTex1_ST ("Texture 1 Tiling/Offset", Vector) = (1,1,0,0)
        _MainTex2_ST ("Texture 2 Tiling/Offset", Vector) = (1,1,0,0)
        _MaskTex_ST ("Mask Tiling/Offset", Vector) = (1,1,0,0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float2 uvMask : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex1;
            sampler2D _MainTex2;
            sampler2D _MaskTex;

            float4 _MainTex1_ST;
            float4 _MainTex2_ST;
            float4 _MaskTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Aplicar tiling y offset a las coordenadas UV
                o.uv1 = TRANSFORM_TEX(v.uv, _MainTex1);
                o.uv2 = TRANSFORM_TEX(v.uv, _MainTex2);
                
                // Ajustar las UV de la máscara para que el centro esté en el centro de la textura
                float2 uvMaskCentered = v.uv - 0.5;
                uvMaskCentered = uvMaskCentered * _MaskTex_ST.xy + _MaskTex_ST.zw + 0.5;
                o.uvMask = uvMaskCentered;

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Texturas principales
                float4 tex1 = tex2D(_MainTex1, i.uv1);
                float4 tex2 = tex2D(_MainTex2, i.uv2);

                // Textura de máscara
                float4 mask = tex2D(_MaskTex, i.uvMask);

                // Mezcla de texturas basada en el canal rojo de la máscara
                float blendFactor = mask.r;
                float4 blendedColor = lerp(tex2, tex1, blendFactor);

                return blendedColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
