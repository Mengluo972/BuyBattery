Shader "Scene/dotTex"
{
    //材质面板参数
    Properties
    {
        _BaseColor    ("基础色",color) = (1,1,1)
        _ScreenTex    ("屏幕纹理", 2D) = "black"{}
        _ScreenPower  ("屏幕纹理强度", Range(0, 1)) = 0.5
        _ShadowColor ("Shadow Color", Color) = (0.7, 0.7, 0.8)
		_ShadowRange ("Shadow Range", Range(0, 1)) = 0.5
        _ShadowSmooth("Shadow Smooth", Range(0, 1)) = 0.2
    }
    SubShader {
        Tags
        {
            "LightMode"="ForwardBase"
        }
        Pass
        {
            Name "FORWARD"
            Tags
            {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma target 3.0
            //输入参数
            half3 _BaseColor;
            uniform sampler2D _ScreenTex;float4 _ScreenTex_ST;
            half3 _ShadowColor;
            half _ShadowRange;
            half _ScreenPower;
            half _ShadowSmooth;
            //输入结构
            struct a2v
            {
                float4 vertex   : POSITION;
                float4 normal   : NORMAL;
                float2 uv : TEXCOORD0;
            };

            //输出结构
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
                float2 screenUV : TEXCOORD3;
            };

            //输入结构>>顶点shader>>输出结构
            v2f vert (a2v v)
            {
                v2f o = (v2f)0;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                float3 posVS = UnityObjectToViewPos(v.vertex).xyz;
                o.screenUV = posVS.xy / posVS.z;
                // float originDist = UnityObjectToViewPos(float3(0.0,0.0,0.0)).z;
//                o.screenUV *= originDist;
                o.screenUV *= _ScreenTex_ST.xy * 7;
                //修正摄像机距离问题：使拉近前后纹理大小一致
                
                // o.screenUV *= ndcNormal.xy;
                return o;
            }
            
            //输出结构>>>像素
            float4 frag(v2f i) : COLOR
            {
                half4 col = 1;
				half3 worldNormal = normalize(i.worldNormal);
                half3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
                half halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;
			
				half ramp = smoothstep(0, _ShadowSmooth, halfLambert - _ShadowRange);
                
                half var_ScreenTex = tex2D(_ScreenTex,i.screenUV).r;
				half3 shadowDiffuse = _ShadowColor + _BaseColor * var_ScreenTex * _ScreenPower;
				half3 diffuse = lerp(shadowDiffuse, _BaseColor, ramp);
                
                col.rgb = _LightColor0 * diffuse ;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    
}
