Shader "Custom/AlwaysRender" {
     Properties
     {
         _Color ("Color", Color) = (1,1,1,1)
     }
     Category
     {
         SubShader
         {
             Tags 
			 { 
				"Queue"="Overlay+1"
				"RenderType"="Transparent"
			 }
             Pass
             {
                 Blend SrcAlpha OneMinusSrcAlpha
                 ZWrite Off
                 ZTest Greater
                 Lighting Off
                 Color [_Color]
             }
             Pass
             {
                 Blend SrcAlpha OneMinusSrcAlpha
                 ZTest Less
                 Color [_Color]
             }
         }
     }
     FallBack "Diffuse"
 }