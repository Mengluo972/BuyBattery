Shader "PostEffect/Dithering"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelScale ("_PixelScale", Float) = 1
        _DitherSize ("Dihter Size", Float) = 1
    	_XOffset("ScreenX",Float) = 1
    	_YOffset("ScreenY",Float) = 1
    	_PixelPower("pixelPower",Float) = 0.5
    }
    SubShader
    {
        Pass
        {
            ZTest Always
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _PixelScale;
            float _DitherSize;
            float _XOffset;
            float _YOffset;
            float _PixelPower;
            
            struct v2f
            {
                float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
            };

            v2f vert (appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
                return o;
            }
            inline fixed get_limit(int x, int y)
	        {
                const int dither[8][8] = {
				{ 0,  32, 8,  40, 2,  34, 10, 42 }, 
				{ 48, 16, 56, 24, 50, 18, 58, 26 }, 
				{ 12, 44, 4,  36, 14, 46, 6,  38 }, 
				{ 60, 28, 52, 20, 62, 30, 54, 22 }, 
				{ 3,  35, 11, 43, 1,  33, 9,  41 }, 
				{ 51, 19, 59, 27, 49, 17, 57, 25 },
				{ 15, 47, 7,  39, 13, 45, 5,  37 },
				{ 63, 31, 55, 23, 61, 29, 53, 21 } };
				return (dither[x][y] + 1) / 64.0;
	        }
            fixed find_closest_gray(int x, int y, inout fixed col)
			{
				fixed limit = get_limit(x, y);
				return step(limit, col);
			}
            fixed4 frag (v2f i) : SV_Target
            {
            	/* uv降低采样率法 */
            	float2 interval = _PixelScale * _MainTex_TexelSize.xy;
                float2 th = i.uv / interval;    // 按interval划分中，属于第几个像素
                float2 th_int = th - frac(th);  // 去小数，让采样的第几个纹素整数化，这就是失真UV关键
                th_int *= interval;             // 再重新按第几个像素的uv坐标定位
            	/* 色阶纹理 规则抖动算法 */
                fixed4 mainCol = tex2D(_MainTex, th_int);
            	float2 xy = (i.uv * float2(_XOffset,_YOffset).xy) * _DitherSize;
				int x = int(fmod(xy.x, 8)); 
				int y = int(fmod(xy.y, 8)); 
                fixed4 lum = fixed4(0.299, 0.587, 0.114, 0);
				fixed grayscale = dot(mainCol, lum);
				fixed finalTex = saturate(find_closest_gray(x, y, grayscale));
            	fixed3 finalCol = mainCol * finalTex;
            	finalCol = lerp(mainCol, finalCol * mainCol + mainCol * _PixelPower,finalTex);//+ mainCol * 0.2;// 校色
            	// finalCol = lerp(finalCol * mainCol , mainCol,finalTex);//+ mainCol * 0.2;// 校色
                return float4(finalCol,1);
            }
            ENDCG
        }
    }
}
