Shader "Custom/TownShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf TownShade fullforwardshadows noambient
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        struct TownSurfaceOutput
        {
            fixed3 Albedo;
            fixed3 Normal;
            fixed3 Emission;
            fixed Alpha;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout TownSurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        inline fixed4 LightingTownShade(TownSurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed tAtten)
        {
            fixed4 tResult;
            tResult.rgb = s.Albedo * ((tAtten * saturate(dot(lightDir, s.Normal))) * 0.5 + 0.5);
            tResult.a = s.Alpha;
            return tResult;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
