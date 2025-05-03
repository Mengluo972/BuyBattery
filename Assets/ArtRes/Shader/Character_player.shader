// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Character/player"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _MainColor("Main Color", Color) = (1,1,1)
    	_Ambiel_K("Ambiel_K(环境光系数)",Float) = 1
		_ShadowColor ("Shadow Color", Color) = (0.7, 0.7, 0.8)
		_ShadowRange ("Shadow Range", Range(0, 1)) = 0.5
    	_ShadowSmooth("Shadow Smooth", Range(0, 1)) = 0.2
    	_Diff_K("Diff_K(漫反射系数)", Float) = 1
		_DiffColor("Diff_Color(漫反射颜色)",Color) = (1,1,1,1)
    }
    SubShader
    {
		
        CGINCLUDE
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "AutoLight.cginc"
		// #pragma vertex vert
		ENDCG
        pass
        {
           //使用光照模型
			Tags{"LightingMode" = "ForwardBase"}
			//剔除背面
			Cull Back
			CGPROGRAM
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma	multi_compile_shadowcaster
			#pragma vertex vert
			
		    
            #pragma multi_compile_fwdbase
			#pragma	multi_compile_shadowcaster

            sampler2D _MainTex;
			float4 _MainTex_ST;
            half3 _MainColor;
            float _Ambiel_K;
			half3 _ShadowColor;
            half _ShadowRange;
            half _ShadowSmooth;
            float _Diff_K;
			float4 _DiffColor;

            struct a2v 
			{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
			{
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
            	SHADOW_COORDS(4)
            };


            v2f vert(a2v v)
			{
                v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            	TRANSFER_SHADOW(o);
                return o;
            }

            half4 frag(v2f i) : SV_TARGET 
			{
                half4 col = 1;
                half4 mainTex = tex2D(_MainTex, i.uv);
				//aoLight
				//环境光
				float3 Ambiel = UNITY_LIGHTMODEL_AMBIENT.xyz * _Ambiel_K * _MainColor;
				//阴影
				UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);
                // half3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);//由于没有用高光所以这个不用
				half3 worldNormal = normalize(i.worldNormal);
                half3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				// 伪厚涂
				 half halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5 ;
				 half ramp = smoothstep(0, _ShadowSmooth, halfLambert - _ShadowRange);//-1 + atten
				 half3 diffuse = lerp(_ShadowColor, _MainColor, ramp);
				// diffuse *= mainTex;
				fixed4 light = _LightColor0 * 0.5 + 0.5;
				//漫反射
				fixed3 Diff = light * mainTex * (diffuse + (1 - diffuse)*_ShadowColor) * _DiffColor * _Diff_K;
				col.rgb = Diff + Ambiel;
			
                return col;
            }
            ENDCG
        }
		pass
		{
			Tags{ "LightMode" = "ForwardAdd" } //ForwardAdd ：多灯混合//
			Blend One One//混合模式，表示该Pass计算的光照结果可以在帧缓存中与之前的光照结果进行叠加，否则会覆盖之前的光照结果
			CGPROGRAM
			#pragma fragment fragLight
			#pragma multi_compile_fwdadd//
			#pragma vertex vert

			sampler2D _MainTex;
			float4 _MainTex_ST;
            half3 _MainColor;
            float _Ambiel_K;
			half3 _ShadowColor;
            half _ShadowRange;
            half _ShadowSmooth;
            float _Diff_K;
			float4 _DiffColor;

			struct a2v {
				float4 vertex:POSITION;
				float2 uv:TEXCOORD0;
				float3 normal:NORMAL;
				float4 color:COLOR;
			};
			struct v2f {
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
				float3 worldNormal:TEXCOORD1;
				float3 worldPos:TEXCOORD2;
				float3 color:TEXCOORD3;
				//点光源需要的衰减
				LIGHTING_COORDS(6, 7)
			};
			v2f vert(a2v v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				//坐标转换到裁剪空间
				o.pos = UnityObjectToClipPos(v.vertex);
				//贴图
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.color = v.color;
				return o;
			}
			fixed4 fragLight(v2f i) :SV_Target
			{
				fixed3 N = normalize(i.worldNormal);
				#ifdef USING_DIRECTIONAL_LIGHT  //平行光下可以直接获取世界空间下的光照方向
							fixed3 L = normalize(_WorldSpaceLightPos0.xyz);
				#else  //其他光源下_WorldSpaceLightPos0代表光源的世界坐标，与顶点的世界坐标的向量相减可得到世界空间下的光照方向
							fixed3 L = normalize(_WorldSpaceLightPos0.xyz - i.worldPos.xyz);
				#endif
				//相机坐标减去顶点坐标
				fixed3 viewdir = normalize(UnityWorldSpaceViewDir(i.worldPos));


				fixed spec = dot(N, normalize(L + viewdir)) + (i.color.g - 0.5) * 2;
				fixed w = fwidth(spec)*2.0;
				//半兰伯特光照的漫反射
				fixed halfdiff = dot(L, N) + (i.color.r - 0.5 * 4);
				half ramp = smoothstep(0, _ShadowSmooth, halfdiff - _ShadowRange);
				half3 diffuse = lerp(_ShadowColor, _MainColor, ramp);
				fixed4 light = _LightColor0 * 0.5 + 0.5;


				//漫反射
				fixed3 Diff = light *(diffuse + (1 - diffuse)*_ShadowColor)*_DiffColor*_Diff_K;
				
				//点光源需要的衰减
				fixed atten;
				#ifdef USING_DIRECTIONAL_LIGHT//平行光下不存在光照衰减，恒值为1
					atten = 1.0;
				#else
					#if defined(POINT)//点光源的光照衰减计算
						//unity_WorldToLight内置矩阵，世界空间到光源空间变换矩阵。与顶点的世界坐标相乘可得到光源空间下的顶点坐标
						float3 lightcoord = mul(unity_WorldToLight, float4(i.worldPos, 1)).xyz;
						/*利用Unity内置函数tex2D对Unity内置纹理_LightTexture0进行纹理采样计算光源衰减，获取其衰减纹理，
						再通过UNITY_ATTEN_CHANNEL得到衰减纹理中衰减值所在的分量，以得到最终的衰减值*/
						atten = tex2D(_LightTexture0, dot(lightcoord, lightcoord).rr).UNITY_ATTEN_CHANNEL;
					#elif defined(SPOT)//聚光灯的光照衰减
						float4 lightcoord = mul(unity_WorldToLight, float4(i.worldPos, 1));
						/*(lightCoord.z > 0)：聚光灯的深度值小于等于0时，则光照衰减为0
						_LightTextureB0：如果该光源使用了cookie，则衰减查找纹理则为_LightTextureB0*/
						atten = (lightcoord.z > 0) * tex2D(_LightTexture0, lightcoord.xy / lightcoord.w + 0.5).w * tex2D(_LightTextureB0, dot(lightcoord, lightcoord).rr).UNITY_ATTEN_CHANNEL;
					#else
						atten = 1;
					#endif
				#endif

				fixed4 col = fixed4(Diff * atten, 1);
				return col;
			}
			ENDCG
		}
		// 即 此Pass将对象渲染为阴影投射器
        Pass
        {
            Tags{ "LightMode"="ShadowCaster"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster

            struct v2f
            {
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}