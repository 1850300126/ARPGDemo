Shader "Anime/AnimeBodyLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightSmooth ("LightSmooth",float) = 1
        _ShadowColor ("ShadowColor", Color) = (1, 1, 1, 1)
        _BaseColor ("BaseColor", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100

        Pass
        { 
            Tags { "Queue"="Geometry" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS: TANGENT;
				float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;   
                float3 normal : TEXCOORD1;             
            };

			CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
			CBUFFER_END
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex); 


            float _LightSmooth;
            half4 _ShadowColor;
            half4 _BaseColor;

            v2f vert (appdata v)
            {
				v2f o;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
				o.pos = vertexInput.positionCS;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = TransformObjectToWorldDir(v.normalOS);
				return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.uv);

                half3 lightDir = _MainLightPosition.xyz;

                half3 worldNormal = i.normal;

                float IDot = dot(lightDir, worldNormal);

                float ISmooth = smoothstep(0, _LightSmooth, IDot);

                half4 Ilerp = lerp(_ShadowColor, _BaseColor, ISmooth);

                half4 Color = Ilerp * col;

				return Color;
            }
            ENDHLSL
        }

        // 一般在Buit-In管线里，我们只需要最后FallBack返回到系统的Diffuse Shader，管线就会去里面找到他处理阴影的Pass
        // 但是在URP中，一个Shader中的所有Pass需要有一致的CBuffer，否则便会打破SRP Batcher，影响效率。
        // 而系统默认SimpleLit的Shader中的CBuffer内容和我的写的并不一致，
        // 所以我们需要把它阴影处理的Pass复制一份，并且删掉其中引用的SimpleLitInput.hlsl（相关CBuffer的声明在这里面）
        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}
 
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]
 
            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5
 
            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
 
            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
 
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
 
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
    }
}

