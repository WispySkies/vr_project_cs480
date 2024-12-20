Shader "Custom/ChromaKey"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _KeyColor ("Key Color", Color) = (0, 1, 0, 1) // Default to green (0,1,0) for chroma key
        _Threshold ("Threshold", Range(0, 1)) = 0.3 // Sensitivity of key color removal
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _KeyColor;
            float _Threshold;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // Calculate the distance between the pixel color and the key color
                float diff = distance(texColor.rgb, _KeyColor.rgb);

                // If the color is close enough to the key color, make it transparent
                texColor.a = smoothstep(_Threshold, _Threshold + 0.1, diff);

                return texColor;
            }
            ENDCG
        }
    }
}
