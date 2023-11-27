Shader "Anime/FaceShadowLight"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.CompareFunction)]_ZTest("ZTest",int) = 0
        _MainTex ("Texture", 2D) = "white" {}
        _ShaderTex ("ShaderTex", 2D) = "white" {}
        _SkinColor ("SkinColor", Color) = (0.8784314, 0.7333333, 0.6941177, 1)
        _TestFloat ("TestFloat", float) = 1
        _Cutoff("Cutoff",float)=0.5
    }
    SubShader
    { 
        Tags {"RenderPipeline" = "UniversalPipeline" "RenderType"="Opaque"}

        LOD 100

        Pass
        {   
			Tags { "LightMode"="UniversalForward" }
			
			Cull Back
            ZWrite On

			HLSLPROGRAM
		    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#pragma vertex vert
			#pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 shadow : TEXCOORD1;
                half3 worldPos : TEXCOORD2;
                float3 normal : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _ShaderTex;
            float4 _ShaderTex_ST;
            half4 _SkinColor;

            float _Cutoff;

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.shadow = TRANSFORM_TEX(v.uv, _ShaderTex);
                o.normal = TransformObjectToWorldDir(v.normalOS);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 col = tex2D(_MainTex, i.uv);

                half4 ShaderTex = tex2D(_ShaderTex, i.shadow);
    
                float3 _HeadForward = normalize(TransformObjectToWorldDir(float3(0.0,0.0,1.0)));//拿到模型的向前方向

                float3 _HeadRight = normalize(TransformObjectToWorldDir(float3(-1.0,0.0,0.0)));//拿到模型的向右方向
                
                half3 worldLight = _MainLightPosition.xyz;

                float dotF = dot(normalize(_HeadForward.xz), worldLight.xz);
                float dotR = dot(normalize(_HeadRight.xz), worldLight.xz);

                float dotFStep = step(0, dotF);

                float dotRAcos = (acos(dotR) / PI) * 2;

                float dotRAcosDir = (dotR < 0) ? (dotRAcos - 1) : (1 - dotRAcos);

                float texShadowDir = (dotR < 0) ? ShaderTex.g : ShaderTex.r;

                float shadowDir = step(dotRAcosDir, texShadowDir) * dotFStep;

                half4 endValue = lerp(_SkinColor, float4(1, 1, 1, 1), shadowDir);

                half4 endShadow = col * endValue;

                return endShadow;
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
