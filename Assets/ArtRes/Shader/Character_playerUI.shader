Shader "Character/playerUI"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _MainColor("Main Color", Color) = (1,1,1)
		_ShadowColor ("Shadow Color", Color) = (0.7, 0.7, 0.8)
		_ShadowRange ("Shadow Range", Range(0, 1)) = 0.5

        [Space(10)]
		_OutlineWidth ("Outline Width", Range(0.01, 2)) = 0.24
        _OutLineColor ("OutLine Color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader
    {
//        Tags { "RenderType"="Opaque" }

        pass
        {
//           Tags {"LightMode"="ForwardBase"}
			 
//            Cull Back
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
		    #include "UnityCG.cginc"
		    #include "Lighting.cginc"
            #include "AutoLight.cginc"

            sampler2D _MainTex;
			float4 _MainTex_ST;
            half3 _MainColor;
			half3 _ShadowColor;
            half _ShadowRange;

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
            };


            v2f vert(a2v v)
			{
                v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_TARGET 
			{
                half4 col = 1;
                half4 mainTex = tex2D(_MainTex, i.uv);
                // half3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);//由于没有用高光所以这个不用
				half3 worldNormal = normalize(i.worldNormal);
                half3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				half halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;
				half3 diffuse = halfLambert > _ShadowRange ? _MainColor : _ShadowColor;
				diffuse *= mainTex;

				col.rgb = _LightColor0 * diffuse;
                return col;
            }
            ENDCG
        }
		//描边Pass
        Pass 
        {
            Stencil
            {
                Ref 1
                Comp NotEqual
            }

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            float _Outline;
            fixed4 _OutlineColor;

            struct a2v 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            }; 

            struct v2f 
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (a2v v) 
            {
                v2f o;

                float4 pos = mul(UNITY_MATRIX_MV, v.vertex); 
                float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);  
                normal.z = -0.5;
                pos = pos + float4(normalize(normal), 0) * _Outline;
                o.pos = mul(UNITY_MATRIX_P, pos);

                return o;
            }

            float4 frag(v2f i) : SV_Target 
            { 
                return float4(_OutlineColor.rgb, 1);               
            }

            ENDCG
        }
    }
}