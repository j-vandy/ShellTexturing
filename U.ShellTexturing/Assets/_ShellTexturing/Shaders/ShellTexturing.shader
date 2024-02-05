Shader "Custom/ShellTexturing"
{
    Properties
    {
        _HeightMapTex("HeightMapTex", 2D) = "white" {}
        _HeightMapTexScale ("HeightMapTexScale", Float) = 1.0
        _TipColor ("TipColor", Color) = (0.0, 1.0, 0.0, 1.0)
        _BaseColor ("BaseColor", Color) = (0.0, 1.0, 0.0, 1.0)
        _MeshScale ("MeshScale", Float) = 1.0
        _Density ("Density", Integer) = 1
        _Height ("Height", Float) = 0.0
        _ShellHeight ("ShellHeight", Range(0.0, 1.0)) = 0.0
        _Thickness ("Thickness", Range(0.0, 1.0)) = 1.0
        _WindDirection ("WindDirection", Vector) = (0, 0, 0)
        _WindFrequency ("WindFrequency", Float) = 0.0
        _WindAmplitude ("WindAmplitude", Float) = 0.0
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
            #include "util.cginc"

            sampler2D _HeightMapTex;
            float _HeightMapTexScale;
            float4 _TipColor;
            float4 _BaseColor;
            float _MeshScale;
            int _Density;
            float _Height;
            float _ShellHeight;
            float _Thickness;
            float3 _WindDirection;
            float _WindFrequency;
            float _WindAmplitude;
            float3 _PlayerPosition;
            float _PlayerDisplacementRadius;

            struct MeshData
            {
                float4 vertex : POSITION; // vertex pos
                float3 normal : NORMAL; // normal vec
                float2 uv : TEXCOORD0; // uv tex_coords
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION; // in clip space
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;

                // displace the vertex w/ a height along its normal
                v.vertex *= _MeshScale;
                float4 displacement = float4( v.normal * _Height, 0 );
                v.vertex += displacement; 

                // wind displacement
                float wind_strength = sin(_Time.y * _WindFrequency) * _WindAmplitude;
                float3 wind = normalize(_WindDirection) * wind_strength * _ShellHeight;
                v.vertex += float4(wind, 0);

                o.vertex = UnityObjectToClipPos(v.vertex); // transform world to clip
                o.normal = UnityObjectToWorldNormal(v.normal); // model to world
                o.uv = v.uv;
                o.worldPos = mul(UNITY_MATRIX_M, v.vertex); // object to world

                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                float2 topDownProjection = i.worldPos.xz;

                // diffuse lighting
                float3 N = normalize(i.normal);
                float3 L = _WorldSpaceLightPos0.xyz;
                float half_lambert = dot(N, L) * 0.5 + 0.5;

                // calculate blade_height @ uv
                float blade_height = tex2D(_HeightMapTex, topDownProjection * _HeightMapTexScale).x;

                // interpolate _ShellHeight: [0,1] -> [0,blade_height]
                float t = blade_height == 0 ? 1 : _ShellHeight / blade_height;
                float4 c = lerp( _BaseColor, _TipColor, t );
                c = c * half_lambert;

                if (_ShellHeight == 0)
                    return c;

                // convert each cell to have local uv coords
                i.uv *= _Density;
                i.uv = float2( frac(i.uv.x), frac(i.uv.y) );
                i.uv *= 2;
                i.uv -= 1;

                // calculate magnitude of uv vector (radius)
                float mag = length(i.uv);

                if (mag > _Thickness * (1 - t * t))
                    discard;

                return c;
            }
            ENDCG
        }
    }
}
