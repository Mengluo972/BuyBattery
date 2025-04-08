Shader "Scene/TexTree"
{
    Properties
    {
         _MainTex    ("RGB：基础颜色 A：透贴", 2D) ="gray"{}
        _Cutoff    ("Alpha cutoff",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags {
            "RenderType"="TransparentCutout"    // 对应改为Cutout
            "ForceNoShaderCasting"="True"       //关闭投影反射
            "IgnoreProjector"="True"            //不响应投射器
        }
        Pass
        {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase_fullshadows
            #include "UnityCG.cginc"
            #pragma target 3.0
            //输入参数
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform half _Cutoff;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o= (v2f)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 var_MainTex = tex2D(_MainTex,i.uv);
                clip(var_MainTex.a - _Cutoff);
                // 返回值
                return half4(var_MainTex.rgb,1.0);
            }
            ENDCG
        }
    }
}
