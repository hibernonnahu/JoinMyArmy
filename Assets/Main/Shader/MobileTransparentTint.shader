﻿Shader "Unlit/MobileTransparentTint"

{
    Properties
    {
        _Color("Color",COLOR)=(0.5,0.5,0.5,1.0)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    }
 
    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        
        ZWrite Off
        Lighting Off
        Cull Back
        Fog { Mode Off }
		
        Blend SrcAlpha OneMinusSrcAlpha 

        Pass {
            Color [_Color]
            SetTexture [_MainTex] { combine texture * primary } 
        }
    }
    Fallback "Unlit/Transparent Cutout"
}
