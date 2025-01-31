Shader "Custom/URP_ElectricRay"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _Speed("Speed", Range(0,5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Speed;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.position);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.x += sin(uv.y * 20 + _Time.y * _Speed) * 0.1; // Usa _Time.y en lugar de redefinir _Time
                half4 col = tex2D(_MainTex, uv) * _Color;
                col.a *= smoothstep(0.2, 0.5, sin(_Time.y * _Speed * 3)); // Animación del destello
                return col;
            }
            ENDHLSL
        }
    }
}
