Shader "Custom/InstancedWorldPoscolor"
{
    Properties
    {
        [HDR]_Color1("Color 1", Color) = (1,1,1,1)
        [HDR]_Color2("Color 2", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Offset("World Position Offset", Vector) = (0,0,0,0)
        _Smooth("Color Smooth Variation", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        
        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        float3 _Offset;

        float4 _Color1, _Color2;
        float _Smooth;


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            
            float aboveZero = smoothstep(-_Smooth,_Smooth, IN.worldPos.y);

            o.Albedo = lerp(_Color1, _Color2, aboveZero);
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;
            o.Emission = o.Albedo;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
