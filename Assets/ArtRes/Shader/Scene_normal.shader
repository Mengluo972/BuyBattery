Shader"Scene/col"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1,1,1)
		_ShadowColor1 ("Shadow Color", Color) = (0.7, 0.7, 0.8)
		_ShadowColor2 ("Shadow Color", Color) = (0.7, 0.7, 0.8)
		_ShadowRange1 ("Shadow Range", Range(0, 1)) = 0.5
		_ShadowRange2 ("Shadow Range", Range(0, 1)) = 0.2
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
            
            half3 _MainColor;
			half3 _ShadowColor1;
			half3 _ShadowColor2;
            half _ShadowRange1;
            half _ShadowRange2;

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
                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_TARGET 
			{
                half4 col = 1;
                // half4 mainTex = tex2D(_MainTex, i.uv);
				half3 worldNormal = normalize(i.worldNormal);
                half3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				
				half halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;
				// half3 diffuse = halfLambert > _ShadowRange ? _MainColor : _ShadowColor;
				//将上一行改成多层
				half3 diffuse1 = halfLambert > _ShadowRange1 ? _MainColor : _ShadowColor1;
				half3 diffuse2 = halfLambert > _ShadowRange2 ? diffuse1 : _ShadowColor2;
				half3 diffuse = diffuse2 * 1;

				col.rgb = _LightColor0 * diffuse;
                return col;
            }
            ENDCG
        }
		//此处添加描边Pass,参考outline_vertex
        
    }
}