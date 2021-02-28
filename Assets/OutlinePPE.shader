Shader "Custom/Outline/OutlinePPE"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineShape ("Texture",2D)= "" {}
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _OutlineShape;
            float2 _MainTex_TexelSize;
            fixed4 frag(v2f i) : SV_Target
            {
                int range = 10;
                fixed4 color = tex2D(_MainTex, i.uv);
                float shapeColor = tex2D(_OutlineShape, i.uv).r;
                if (shapeColor > 0)
                {
                    return color;
                }
                float outline_intensity = 0;
                for (int offset_x = -range; offset_x <= range; offset_x++)
                {
                    for (int offset_y = -range; offset_y <= range; offset_y++)
                    {
                        float2 uv_offset = i.uv + float2(offset_x * _MainTex_TexelSize.x,
                                                         offset_y * _MainTex_TexelSize.y);
                        if (uv_offset.x >= 0 && uv_offset.x <= 1 &&
                            uv_offset.y >= 0 && uv_offset.y <= 1)
                        {
                            if (tex2D(_OutlineShape, uv_offset).r > 0)
                            {
                                outline_intensity = max(outline_intensity,
                                                        smoothstep(1, 0, -0.1+(distance(float2(offset_x, offset_y),
                                                                       float2(0, 0)) / range)));
                            }
                        }
                    }
                }
                color += float4(1, 0, 0, 1) * outline_intensity;


                return color;
            }
            ENDCG
        }
    }
}