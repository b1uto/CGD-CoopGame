// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// // Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Shader "CGD/SpriteHiddenClue"
// {
//     Properties
//     {
//         [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
//         _Color ("Tint", Color) = (1,1,1,1)
//         [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
//         [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
//         [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
//         [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
//         [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

        
// 	    _LightDirection("Spot Light Direction", Vector) = (0,0,1,0)
// 	    _LightPosition("Spot Light Position", Vector) = (0,0,0,0)
// 	    _LightAngle("Spot Light Angle", Range(0,180)) = 45
// 	    _StrengthScalor("Strength Scalor", Float) = 50
//     }

//     SubShader
//     {
//         Tags
//         {
//             "Queue"="Transparent"
//             "IgnoreProjector"="True"
//             "RenderType"="Transparent"
//             "PreviewType"="Plane"
//             "CanUseSpriteAtlas"="True"
//         }

//         Cull Off
//         Lighting Off
//         ZWrite Off
//         Blend One OneMinusSrcAlpha

//         Pass
//         {
//         CGPROGRAM
//             #pragma vertex SpriteVert
//             #pragma fragment SpriteFrag
//             #pragma target 2.0
//             #pragma multi_compile_instancing
//             #pragma multi_compile_local _ PIXELSNAP_ON
//             #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
//             #include "UnitySprites.cginc"

   
      
    
//         ENDCG
//         }
//     }
// }



Shader "CGD/SpriteHiddenClue"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        _SpotLightDirection("Spot Light Direction", Vector) = (0,0,1,0)
        _SpotLightIntensity("Spot Light Intensity", Float) = 10
        _SpotLightRange("Spot Light Range", Float) = 10
        _SpotLightPosition("Spot Light Position", Vector) = (0,0,0,0)
        _SpotLightAngle("Spot Light Angle", Range(0,180)) = 45
        _StrengthScalor("Strength Scalor", Float) = 50
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            fixed4 _Color;
            float4 _SpotLightDirection;
            float4 _SpotLightPosition;
            float _SpotLightAngle;
            float _SpotLightIntensity;
            float _SpotLightRange;
            float _StrengthScalor;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex).xyz;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {

                fixed4 color = tex2D(_MainTex, IN.texcoord) * _Color;
                float3 toFragment = normalize(IN.worldPos - _SpotLightPosition.xyz);
                float distanceToFragment = length(IN.worldPos - _SpotLightPosition.xyz);
                float3 lightDir = normalize(_SpotLightDirection.xyz);
                float dotProduct = dot(toFragment, lightDir);
                float cosThreshold = cos(_SpotLightAngle * 0.5 * (3.14159 / 180.0));
                float strength = (-dotProduct) - cosThreshold;

                strength = clamp(strength * _StrengthScalor, 0.0, 1.0);

               float distanceRatio = distanceToFragment / _SpotLightRange;
               float smoothAttenuation = 1.0 - smoothstep(0.0, 1.0, distanceRatio);
               float clampedIntensity = clamp(_SpotLightIntensity, 0.015, 1);

               strength *= smoothAttenuation * clampedIntensity;
               color *= strength * tex2D(_AlphaTex, IN.texcoord).r;
                
               return color;
            }
        ENDCG
        }
    }
}
