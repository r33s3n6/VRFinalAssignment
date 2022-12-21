// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/MobiusDome"
{
    Properties
    {
        _Orientation ("Transition orientation", Vector) = (1, 0, 0)
        _Process ("Current transition step", Float) = 0.0
        _CubemapFrom ("Current view cubemap", Cube) = "_Skybox" {}
        _CubemapTo ("Next view cubemap", Cube) = "_Skybox" {}
    }
    SubShader
    {
        Pass
        {
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _Process;
            float3 _Orientation;
            samplerCUBE _CubemapFrom;
            samplerCUBE _CubemapTo;
            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half3 worldDir : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldDir = mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 orientation = normalize(_Orientation);
                orientation.x = -orientation.x;
                fixed3 worldDir = normalize(i.worldDir);
                worldDir.x = -worldDir.x;
                fixed3 color;
                float cosine = dot(worldDir, orientation);
                fixed3 orthogonal = normalize(worldDir - cosine * orientation);
                float criterion = 3.1415926 * _Process;
                if (_Process < 0.5) {
                    if (cosine > cos(criterion) + 0.001) {
                        float sampleAngle = acos(cosine) * (0.5 * 3.1415926) / criterion;
                        half3 sampleDir = cos(sampleAngle) * orientation + sin(sampleAngle) * orthogonal;
                        color = texCUBE(_CubemapTo, sampleDir).rgb;
                    } else {
                        color = texCUBE(_CubemapFrom, worldDir);
                    }
                } else {
                    if (cosine < cos(criterion) - 0.001) {
                        float sampleAngle = 3.1415926 - (3.1415926 - acos(cosine)) * (0.5 * 3.1415926) / (3.1415926 - criterion);
                        half3 sampleDir = cos(sampleAngle) * orientation + sin(sampleAngle) * orthogonal;
                        color = texCUBE(_CubemapFrom, sampleDir).rgb;
                    } else {
                        color = texCUBE(_CubemapTo, worldDir);
                    }
                }
                color = pow(color, 2.2);
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
