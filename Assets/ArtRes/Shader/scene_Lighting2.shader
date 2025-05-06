Shader "scene/Lighting2"
{
    Properties
    {
        [HDR]_LightColor("lightColor",Color) = (1,1,1)
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
            
            
            half3 _LightColor;

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
            };

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                
                return half4(_LightColor,1.0);
            }

            ENDCG
        }
    }
}

