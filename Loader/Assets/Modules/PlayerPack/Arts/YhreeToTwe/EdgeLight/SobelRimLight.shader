Shader "Unlit/SobelRimLight"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ThicknessX("ThicknessX", Float) = 0.01
		_ThicknessY("ThicknessY", Float) = 0.01
		_MaxThickness("MaxThickness", Float) = 0.01
		_Intensity("Intensity", Range(0,1)) = 0.01
		_LerpValue("LerpValue", Range(0,1)) = 1
		_Distance("Distance", Float) = 1
		_Color("RimLightColor",Color) = (0,0,0)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"			

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 scrPos : TEXCOORD2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _ThicknessX;
			float _ThicknessY;
			float _MaxThickness;
			float _Intensity;
			float _LerpValue;
			float _Distance;
			sampler2D _CameraDepthTexture;
			float4 _CameraDepthTexture_ST;
			float4 _Color;

			static float2 sobelSamplePoints[9] = {
				float2(-1,1),float2(0,1),float2(1,1),
				float2(-1,0),float2(0,0),float2(1,0),
				float2(-1,-1),float2(0,-1),float2(1,-1),
			};

			static float sobelXMatrix[9] = {
				1,0,-1,
				2,0,-2,
				1,0,-1,
			};
			static float sobelYMatrix[9] = {
				1,2,1,
				0,0,0,
				-1,-2,-1,
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.scrPos = ComputeScreenPos(o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 sobel = 0;
				//获得深度
				float originDepth = tex2D(_CameraDepthTexture, i.uv);
				//转化为线性0-1深度，这样0.5就表示处于相机到farPlane一半的位置了
				originDepth= Linear01Depth(originDepth);
				//找到该像素离相机的真实距离，_ProjectionParams.z表示_Camera_FarPlane的距离。https://docs.unity.cn/Packages/com.unity.shadergraph@6.9/manual/Camera-Node.html				
				float depthDistance = _ProjectionParams.z*originDepth;				
				float2 adaptiveThickness = float2(_ThicknessX, _ThicknessY);
				if (depthDistance <= 0)
					adaptiveThickness = float2(_MaxThickness, _MaxThickness);
				else 
				{//根据距离对边缘光厚度进行线性缩放
					adaptiveThickness = adaptiveThickness / depthDistance;
				}
				adaptiveThickness = min(adaptiveThickness, float2(_MaxThickness, _MaxThickness));

				
				for (int id = 0; id < 9; id++) 
				{
					float2 screenPos = i.uv + sobelSamplePoints[id] * adaptiveThickness;					
					float depth = tex2D(_CameraDepthTexture, screenPos);					
					depth = Linear01Depth(depth);

					sobel += depth * float2(sobelXMatrix[id], sobelYMatrix[id]);					
				}		
				fixed4 previousColor = tex2D(_MainTex, i.uv);
				fixed4 rimLightColor = _Color * step(_Distance,length(sobel)*_ProjectionParams.z);

				//叠加到原来的颜色上
				fixed4 col = previousColor + lerp(previousColor*rimLightColor, rimLightColor, _LerpValue) * _Intensity;

				return col;
			}
			ENDCG
		}
	}
}