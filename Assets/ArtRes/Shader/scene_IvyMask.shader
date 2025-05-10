Shader "scene/IvylMask"
{
    Properties
    {
        _ID ("Mask ID", Int) = 1 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry+1" }
        ColorMask 0 //RGBA、RGB、R、G、B、0
        ZWrite Off
        Stencil
        {
            Ref [_ID]
            Comp Always //默认always
            Pass Replace    //默认keep
            //Fail Keep
            //ZFail Keep
        }
        Pass
        {
                CGINCLUDE
                #include <UnityShaderUtilities.cginc>

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            half4 frag (v2f i) : SV_Target
            {
                return half4(1,1,1,1);
            }
                ENDCG
        }
    }
}
