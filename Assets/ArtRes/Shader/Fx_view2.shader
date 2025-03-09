Shader "Fx/view2" {
    //材质面板参数
    Properties {
            _viewColor  ("显示颜色",Color)          =(1.0,1.0,1.0,1.0)
            _NoiseTex   ("混合用噪声图",2d)           ="gray"{}
            _Opacity    ("透明度",Range(0,1))        =0.5
            _NoiseInt   ("混合强度",Range(0,5))       =0.5
            _FlowSpeed  ("噪声图流动速度",Range(0,10))   =5
    }
    SubShader {
        Tags {
            "Queue"="Transparent"               // 调整渲染顺序
            "RenderType"="Transparent"
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
            uniform sampler2D _NoiseTex; uniform float4 _NoiseTex_ST;
            uniform half3 _viewColor;
            uniform half _Opacity;
            uniform half _NoiseInt;
            uniform half _FlowSpeed;
            //输入结构
            struct VertexInput {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;    // uv
            };

            //输出结构
            struct VertexOutput {
                float4 pos    : SV_POSITION;    // 模型顶点换算来的顶点屏幕位置
                float2 uv0     : TEXCOORD0;
                float2 uv1     : TEXCOORD1;
            };

            //输入结构>>顶点shader>>输出结构
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;             
                    o.pos = UnityObjectToClipPos( v.vertex );
                    o.uv0 = v.uv;
                    o.uv1 = TRANSFORM_TEX(v.uv, _NoiseTex);             //UV1支持TilingOffset
                    o.uv1.y = o.uv1.y + frac(-_Time.x * _FlowSpeed);    //UV1 V轴扰动
                return o;
            }
            
            //输出结构>>>像素
            float4 frag(VertexOutput i) : COLOR {
                half var_NoiseTex = tex2D(_NoiseTex,i.uv1).r;
                half noise = lerp(1.0,var_NoiseTex * 2.0, _NoiseInt);
                noise = max(0.0,noise);                                 // 如果没有这一段会出现奇怪的颜色
                half opacity = _Opacity*noise;
                // 返回值
                return float4(_viewColor*opacity,opacity);
            }
            ENDCG
        }
    }
}
