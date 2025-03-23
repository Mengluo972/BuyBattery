Shader "Character/player"
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
        Tags { "RenderType"="Opaque" }

        pass
        {
           Tags {"LightMode"="ForwardBase"}
			 
            Cull Back
            
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
		    Tags {"LightMode"="ForwardBase"}
				 
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float3 OctToUnitVector(float2 oct)
            {
                float3 N = float3(oct, 1 - dot(1, abs(oct)));
				float t = max(-N.z, 0);
				N.x += N.x >= 0 ? (-t) : t;
				N.y += N.y >= 0 ? (-t) : t;
            	return normalize(N);
            }

            half _OutlineWidth;
            half4 _OutLineColor;

            struct a2v 
			{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertColor : COLOR;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            	float2 uv1 : TEXCOORD1;
            };

            struct v2f
			{
                float4 pos : SV_POSITION;
            	float2 uv0 : TEXCOORD0;
            	float3 nDirOS : TEXCOORD1;
                float3 tDirOS : TEXCOORD2;
                float3 bDirOS : TEXCOORD3;
            	float3 vertColor : COLOR;
            };
            
            v2f vert (a2v v) 
			{
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
            	//从uv1获取平滑法线
                float3 smoothNormal = OctToUnitVector(v.uv1);
                o.nDirOS = v.normal;   
                o.tDirOS = v.tangent;
                o.bDirOS = normalize(cross(o.nDirOS, o.tDirOS) * v.tangent.w);
                float3x3 tbnOS = float3x3(o.tDirOS,o.bDirOS,o.nDirOS);
                float3 smoothNormalOS = normalize(mul(smoothNormal, tbnOS));
            	
            	//修正摄像机距离问题：使拉近前后描边宽度一致
                float4 pos = UnityObjectToClipPos(v.vertex);
                float3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, smoothNormalOS);
                float3 ndcNormal = normalize(TransformViewToProjection(viewNormal.xyz)) * pos.w;
                float4 nearUpperRight = mul(unity_CameraInvProjection, float4(1, 1, UNITY_NEAR_CLIP_VALUE, _ProjectionParams.y));
                float aspect = abs(nearUpperRight.y / nearUpperRight.x);//求得屏幕宽高比
                ndcNormal.x *= aspect;
				
                pos.xy += 0.01 * _OutlineWidth * ndcNormal.xy ;
                o.pos = pos;
                return o;
            }

            half4 frag(v2f i) : SV_TARGET 
			{
                return fixed4(_OutLineColor.rgb, 0);
            }
            ENDCG
        }
    }
}