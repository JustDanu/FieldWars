Shader "Unlit/Sun effect"
{
    Properties
    {
        _Color ("Main Color", Color) = (1, 1, 0, 1)
        _Radius ("Radius", Float) = 0.9
        _Smoothness ("Smoothness", Float) = 0.3
        _FlickerSpeed ("Flicker Speed", Float) = 0.2
        _FlickerIntensity ("Flicker Intensity", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _Color;
            float _Radius;
            float _Smoothness;
            float _FlickerSpeed;
            float _FlickerIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2 - 1;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float flicker = sin(_Time.y * _FlickerSpeed) * _FlickerIntensity;
                float adjustedRadius = _Radius + flicker;

                float dist = length(i.uv);
                float glow = smoothstep(adjustedRadius, adjustedRadius + _Smoothness, dist);
                return float4(_Color.rgb, (1 - glow) * _Color.a);
            }
            ENDCG
        }
    }
}
