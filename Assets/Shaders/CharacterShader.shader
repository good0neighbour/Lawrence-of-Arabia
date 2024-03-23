Shader "Custom/CharacterShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Cutoff("Alpha cutoff", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        cull off

        CGPROGRAM
        #pragma surface surf CharacterDoubleShade fullforwardshadows noambient alphatest:_Cutoff
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        struct CharacterSurfaceOutput
        {
            fixed3 Albedo;
            fixed3 Normal;
            fixed3 Emission;
            fixed Alpha;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout CharacterSurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        inline fixed4 LightingCharacterDoubleShade(CharacterSurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed tAtten)
        {
            fixed4 tResult;
            //tResult.rgb = s.Albedo * (tAtten * 0.5 + 0.5);
            tResult.rgb = s.Albedo;
            tResult.a = s.Alpha;
            return tResult;
        }
        ENDCG
    }
    FallBack "Transparent/Cutout/Diffuse"
}
