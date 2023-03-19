// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "NPR Cartoon Effect/Cartoon Low" {
	Properties {
		_MainTex ("Base", 2D) = "white" {}
		//
		
		//
		_HighlitColor ("Highlit", Color) = (0.6, 0.6, 0.6, 1.0)
		_DarkColor ("Dark", Color) = (0.4, 0.4, 0.4, 1.0)
		
		
		
		
		_OutlineColor ("Outline Color", Color) = (0, 0, 0, 0)
		_OutlineWidth ("Outline Width", Float) = 0.02
		_ExpandFactor ("Outline Factor", Float) = 1
		
		_RimColor ("Rim Color", Color) = (0.8, 0.8, 0.8, 0.6)
		_RimMin ("Rim Min", Float) = 0.5
		_RimMax ("Rim Max", Float) = 1
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
		Pass {
			CGPROGRAM
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#pragma multi_compile_fwdbase
			#pragma multi_compile _ NCE_BUMP
			#pragma multi_compile _ NCE_RAMP_TEXTURE
			#pragma multi_compile _ NCE_SPECULAR
			#pragma multi_compile _ NCE_STYLIZED_SPECULAR
			#pragma multi_compile _ NCE_STYLIZED_SHADOW
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex, _StylizedShadowTex;
			float4 _MainTex_ST, _StylizedShadowTex_ST;
			fixed4 _SpecularColor, _HighlitColor, _DarkColor, _RimColor;
			float _SpecularScale;
			float _SpecularTranslationX, _SpecularTranslationY;
			float _SpecularRotationX, _SpecularRotationY, _SpecularRotationZ;
			float _SpecularScaleX, _SpecularScaleY;
			float _SpecularSplitX, _SpecularSplitY;
			fixed _RampThreshold, _RampSmooth, _SpecPower, _SpecSmooth, _RimMin, _RimMax;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float3 tgsnor : TEXCOORD1;    // tangent space normal
				float3 tgslit : TEXCOORD2;    // tangent space light
				float3 tgsview : TEXCOORD3;   // tangent space view
				LIGHTING_COORDS(4, 5)
			};
			v2f vert (appdata_tan v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.tex.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.tex.zw = TRANSFORM_TEX(v.texcoord, _StylizedShadowTex);
				TANGENT_SPACE_ROTATION;
				o.tgsnor = mul(rotation, v.normal);
				o.tgslit = mul(rotation, ObjSpaceLightDir(v.vertex));
				o.tgsview = mul(rotation, ObjSpaceViewDir(v.vertex));
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
			
			
			float4 frag (v2f i) : SV_TARGET
			{

				float3 N = normalize(i.tgsnor);

				float3 L = normalize(i.tgslit);
				float3 V = normalize(i.tgsview);
				float3 H = normalize(V + L);

				//
				// cartoon light model
				//
				
				// ambient light from Unity render setting
				//float3 ambientColor = UNITY_LIGHTMODEL_AMBIENT.xyz;
				float3 ambientColor = float3(0,0,0);

				// rim light
				half rim = 1.0 - saturate(dot(V, N));
				rim = smoothstep(_RimMin, _RimMax, rim) * _RimColor.a;
				
				fixed4 albedo = tex2D(_MainTex, i.tex.xy);
				albedo = lerp(albedo, _RimColor, rim);
				
				// diffuse cartoon light
				float diff = saturate(dot(N, L)) * LIGHT_ATTENUATION(i);

				fixed4 darkColor = _DarkColor;
#if NCE_STYLIZED_SHADOW
				darkColor = tex2D(_StylizedShadowTex, i.tex.zw);
#endif
				

				fixed4 c = lerp(_HighlitColor, darkColor, _DarkColor.a);
								
				float4 diffuseColor = albedo ;


				fixed3 specularColor = fixed3(0, 0, 0);

				
				//return float4(ambientColor + diffuseColor.rgb + specularColor, 1.0) * _LightColor0;
				return float4(ambientColor + diffuseColor.rgb + specularColor, 1.0);
            }
			ENDCG
		}
		Pass {
			Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
			Cull Front

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag

			float4 _OutlineColor;
			float _OutlineWidth, _ExpandFactor;
			struct v2f
			{
				float4 pos : SV_POSITION;
			};
			v2f vert (appdata_base v)
			{
				float3 dir1 = normalize(v.vertex.xyz);
				float3 dir2 = v.normal;
				float3 dir = lerp(dir1, dir2, _ExpandFactor);
				dir = mul((float3x3)UNITY_MATRIX_IT_MV, dir);
				float2 offset = TransformViewToProjection(dir.xy);
				offset = normalize(offset);
				float dist = 1;//distance(mul(unity_ObjectToWorld, v.vertex), _WorldSpaceCameraPos);
			
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

#if UNITY_VERSION > 540
				o.pos.xy += offset * o.pos.z * _OutlineWidth * dist;
#else
				o.pos.xy += offset * o.pos.z * _OutlineWidth / dist;
#endif
				return o;
			}
			float4 frag (v2f i) : SV_TARGET
			{
				return _OutlineColor;
            }
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
