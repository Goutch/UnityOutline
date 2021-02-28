
Shader "Custom/Outline/DrawOutlineShape"
{
    SubShader
    {
        ZWrite Off
        ZTest Always
        Lighting Off
        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex VShader
            #pragma fragment FShader
            #pragma target 4.5
            struct VertexToFragment
            {
                float4 pos:SV_POSITION;
            };

            //just get the position correct
            VertexToFragment VShader(VertexToFragment i)
            {
               VertexToFragment o;
                o.pos = UnityObjectToClipPos(i.pos);
                return o;
            }

            //return white
            half FShader():COLOR0
            {
                return 1;
            }
            ENDCG
        }
    }
}