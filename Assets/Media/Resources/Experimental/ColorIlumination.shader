Shader "Custom/ColorIlumination"
{
    Properties
    {
        _Glossiness ("Smoothness", Range(0,1)) = 0.2
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert

        struct Input
        {
            float3 vertexColor; // ? Aquí no se necesita ninguna semántica (como :COLOR)
        };

        half _Glossiness;
        half _Metallic;

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.vertexColor = v.color.rgb; // ? Copiamos el color de vértice
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = IN.vertexColor; // ? Se usa como color difuso
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}