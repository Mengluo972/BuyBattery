Shader "Toturial/SolidColor"
{
    Properties
    {
        [HDR]_MainColor("Main Color", Color) = (1,1,1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            uniform  float4 _MainColor;

            struct a2v
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
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
                return _MainColor;
            }

            ENDCG
        }
    }
}

