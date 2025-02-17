Shader "Custom/ScrollingColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}     // Texture gradient màu
        _Speed ("Scroll Speed", Range(-5, 5)) = 1 // Tốc độ cuộn màu
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            uniform float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // Cuộn màu bằng cách di chuyển UV theo thời gian
                o.uv.x += _Time.y * _Speed;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
