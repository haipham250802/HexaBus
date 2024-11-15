Shader "DTS/Fade 3D with Horizontal Flip"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _FadeMap ("Fade Map", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness ("Outline Thickness", Range(0.001, 0.03)) = 0.01
        _OutlineBrightness ("Outline Brightness", Range(1, 5)) = 2.0
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Opaque"}
        LOD 300
        Cull Off
        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _MainTex;
        sampler2D _FadeMap;
        float _Cutoff;
        float _OutlineThickness;
        float _OutlineBrightness;
        fixed4 _Color;
        fixed4 _OutlineColor;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Flip the UVs horizontally
            float2 flippedUV = float2(1.0 - IN.uv_MainTex.x, IN.uv_MainTex.y);

            fixed4 diffuse = tex2D(_MainTex, flippedUV);
            fixed4 fadeSample = tex2D(_FadeMap, flippedUV);
            float alpha = (fadeSample.r + fadeSample.g + fadeSample.b) / 3.0;

            // Detect if we are on the edge of the cutoff region
            bool cut = alpha < _Cutoff ? true : false;

            // Calculate offset texture coordinates for edge detection
            float2 offsets[4] = { float2(_OutlineThickness, 0), float2(-_OutlineThickness, 0),
                                  float2(0, _OutlineThickness), float2(0, -_OutlineThickness) };

            bool isEdge = false;
            float edgeBrightness = 0.0;
            for (int i = 0; i < 4; i++)
            {
                float2 offsetUV = flippedUV + offsets[i];
                fixed4 offsetSample = tex2D(_FadeMap, offsetUV);
                float offsetAlpha = (offsetSample.r + offsetSample.g + offsetSample.b) / 3.0;

                if (cut && offsetAlpha >= _Cutoff)
                {
                    isEdge = true;
                    // Increase brightness based on proximity to the edge
                    edgeBrightness += 1.0 - (offsetAlpha - _Cutoff) / (1.0 - _Cutoff);
                }
            }

            // Apply brightness scaling to the outline
            fixed4 brightOutline = _OutlineColor * (1.0 + edgeBrightness * (_OutlineBrightness - 1.0));

            o.Albedo = cut ? (isEdge ? brightOutline.rgb : diffuse.rgb) : _Color.rgb;
        }
        ENDCG
    }

    FallBack "Transparent/Cutout/Diffuse"
}
