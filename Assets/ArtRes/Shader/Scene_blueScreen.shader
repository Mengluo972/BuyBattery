Shader "scene/blueScreen"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _LightingPower("LightingPower",Range(0,2)) = 1
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
		    #include "AutoLight.cginc"
            
            sampler2D _MainTex;float4 _MainTex_ST;
            half _LightingPower;

            struct a2v
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                half3 mainTex = tex2D(_MainTex, i.uv) * _LightingPower;
                return half4(mainTex,1.0);
            }

            ENDCG
        }
    }
}

