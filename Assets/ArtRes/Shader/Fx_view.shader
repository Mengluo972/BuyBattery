Shader "Fx/view" {
    //材质面板参数
    Properties {
        [Header(Texture)]
            _viewColor  ("显示颜色",Color)=(1.0,1.0,1.0,1.0)
            _NoiseTex   ("RGB；混合用噪声图，A：透贴",2d)="gray"{}
            _MainTex    ("RGB：纹理 A：透贴", 2D) ="gray"{}
            _Cutoff    ("Alpha cutoff",Range(0,1)) = 0.5
            _Power      ("强度",Range(1,3)) = 1
    }
    SubShader {
        Tags {
            "Queue"="Transparent"  
            "RenderType"="Transparent"    // 对应改为Cutout
            "ForceNoShaderCasting"="True"       //关闭投影反射
            "IgnoreProjector"="True"            //不响应投射器
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha      // 修改混合方式
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma target 3.0
            //输入参数
            // Texture
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _NoiseTex; uniform float4 _NoiseTex_ST;
            uniform half _Cutoff;
            uniform half _Power;
            uniform half3 _viewColor;
            //输入结构
            struct VertexInput {
                float4 vertex   : POSITION;
                float2 uv0       : TEXCOORD0;    // uv
            };

            //输出结构
            struct VertexOutput {
                float4 pos    : SV_POSITION;    // 模型顶点换算来的顶点屏幕位置
                float2 uv0     : TEXCOORD0;
            };

            //输入结构>>顶点shader>>输出结构
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;             
                    o.pos = UnityObjectToClipPos( v.vertex );
                    o.uv0 = TRANSFORM_TEX(v.uv0, _MainTex);
                return o;
            }
            
            //输出结构>>>像素
            float4 frag(VertexOutput i) : COLOR {
                half4 var_NoiseTex = tex2D(_NoiseTex,i.uv0).r;
                half noise = lerp(1.0,var_NoiseTex * 2.0, _Power);
                noise = max(0.0,noise);                                 // 如果没有这一段会出现奇怪的颜色
                half opacity = var_NoiseTex.a * noise - _Cutoff;
                // 返回值
                return float4(var_NoiseTex.rgb *opacity,opacity);
                // half opacity = var_MainTex.a * _Power - _Cutoff;
                // opacity = max(0.0,opacity);
                // // 返回值
                // return half4(var_MainTex.rgb * opacity,opacity);
            }
            ENDCG
        }
    }
}
