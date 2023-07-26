Shader "SimpleLit/Glass"
{
    Properties
    {
        [MainTex] _BaseMap ("Base Map", 2D) = "white" { }
        [MainColor][HDR]  _Color ("Color", Color) = (1, 1, 1, 1)
        _BlurStrength("Blur Strength", Range(0, 1)) = 1
        _Gloss("Gloss", Range(0, 1)) = 1
        _Thickness("Thickness", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "Universal"
        }

        Pass
        {
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            ZTest LEqual
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            struct Attributes
            {
                float4 PositionOS : POSITION;
                float3 NormalOS : NORMAL;
                float2 UV : TEXCOORD0;
            };

            struct VertexToFragment
            {
                float4 PositionHCS : SV_POSITION;
                float3 NormalWS : NORMAL;
                float2 UV : TEXCOORD0;
                float3 PositionWS : TEXCOORD1;
                float4 PositionSS : TEXCOORD2;
            };

            float4 _CameraOpaqueTexture_TexelSize;

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _Color;
            float _BlurStrength;
            float _Gloss;
            float _Thickness;
            CBUFFER_END

            float4 ComputeScreenPos(in const float4 PositionHCS, in const float ProjectionSign)
            {
                float4 Output = PositionHCS * 0.5f;
                Output.xy = float2(Output.x, Output.y * ProjectionSign) + Output.w;
                Output.zw = PositionHCS.zw;
                return Output;
            }

            float4 GetBaseColor(in const float2 UV)
            {
                const float2 MainTexUV = TRANSFORM_TEX(UV, _BaseMap);
                const float4 MainTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, MainTexUV);
                const float4 BaseColor = MainTex * _Color;

                return BaseColor;
            }

            float GetDirectLightFalloff(in const float3 Normal, in const float3 LightDirection)
            {
                const float LightFalloff = clamp(dot(Normal, LightDirection), 0, 0.7);
                return LightFalloff;
            }

            float GetSpecularFalloff(in const float3 Normal, in const float3 PositionWS, in const float3 LightDirection)
            {
                const float3 ViewReflect = reflect(normalize(PositionWS - GetCameraPositionWS()), Normal);
                float SpecularFalloff = max(0, dot(ViewReflect, LightDirection));
                SpecularFalloff = pow(SpecularFalloff, pow(2, 10 * _Gloss));

                return SpecularFalloff;
            }

            float3 GetBlurredScreenColor(in const float2 UVSS)
            {
                #define OFFSET_X(kernel) float2(_CameraOpaqueTexture_TexelSize.x * kernel * _BlurStrength, 0)
                #define OFFSET_Y(kernel) float2(0, _CameraOpaqueTexture_TexelSize.y * kernel * _BlurStrength)

                #define BLUR_PIXEL(weight, kernel) float3(0, 0, 0) \
                    + (SampleSceneColor(UVSS + OFFSET_Y(kernel)) * weight * 0.125) \
                    + (SampleSceneColor(UVSS - OFFSET_Y(kernel)) * weight * 0.125) \
                    + (SampleSceneColor(UVSS + OFFSET_X(kernel)) * weight * 0.125) \
                    + (SampleSceneColor(UVSS - OFFSET_X(kernel)) * weight * 0.125) \
                    + (SampleSceneColor(UVSS + ((OFFSET_X(kernel) + OFFSET_Y(kernel)))) * weight * 0.125) \
                    + (SampleSceneColor(UVSS + ((OFFSET_X(kernel) - OFFSET_Y(kernel)))) * weight * 0.125) \
                    + (SampleSceneColor(UVSS - ((OFFSET_X(kernel) + OFFSET_Y(kernel)))) * weight * 0.125) \
                    + (SampleSceneColor(UVSS - ((OFFSET_X(kernel) - OFFSET_Y(kernel)))) * weight * 0.125) \

                float3 Sum = 0;

                Sum += BLUR_PIXEL(0.02, 10.0);
                Sum += BLUR_PIXEL(0.02, 9.0);
                
                Sum += BLUR_PIXEL(0.06, 8.5);
                Sum += BLUR_PIXEL(0.06, 8.0);
                Sum += BLUR_PIXEL(0.06, 7.5);
                
                Sum += BLUR_PIXEL(0.05, 7);
                Sum += BLUR_PIXEL(0.05, 6.5);
                Sum += BLUR_PIXEL(0.05, 6);
                Sum += BLUR_PIXEL(0.05, 5.5);
                
                Sum += BLUR_PIXEL(0.065, 4.5);
                Sum += BLUR_PIXEL(0.065, 4);
                Sum += BLUR_PIXEL(0.065, 3.5);
                Sum += BLUR_PIXEL(0.065, 3);
                
                Sum += BLUR_PIXEL(0.28, 2);
                
                Sum += BLUR_PIXEL(0.04, 0);

                #undef BLUR_PIXEL
                #undef OFFSET_X
                #undef OFFSET_Y

                return Sum;
            }

            float3 BlendWithBackground(in const float4 Color, in const float2 UVSS)
            {
                const float3 BlurredScreenColor = GetBlurredScreenColor(UVSS);
                const float3 MixedColor = BlurredScreenColor * Color.rgb;
                const float3 AlphaInterpolatedColor = lerp(MixedColor, Color.rgb, Color.a);

                return AlphaInterpolatedColor;
            }

            VertexToFragment Vert(const Attributes Input)
            {
                VertexToFragment Output;

                const float4 PositionHCS = TransformObjectToHClip(Input.PositionOS.xyz);

                Output.PositionHCS = PositionHCS;

                Output.NormalWS = TransformObjectToWorldNormal(Input.NormalOS);

                Output.UV = Input.UV;

                Output.PositionWS = TransformObjectToWorld(Input.PositionOS.xyz);

                const float3 RefractionAdjustedPositionOS = Input.PositionOS - (Input.NormalOS * _Thickness);
                const float4 RefractionAdjustedPositionHCS = TransformObjectToHClip(RefractionAdjustedPositionOS);
                Output.PositionSS = ComputeScreenPos(RefractionAdjustedPositionHCS, _ProjectionParams.x);

                return Output;
            }

            float4 Frag(const VertexToFragment Input) : SV_Target
            {
                const float3 SmoothNormal = normalize(Input.NormalWS);
                // const float4 ModifiedPositionSS = ComputeScreenPos(TransformWorldToHClip(Input.PositionWS - (SmoothNormal * _Thickness)), _ProjectionParams.x);
                const float4 ModifiedPositionSS = Input.PositionSS;

                float4 BaseColor = GetBaseColor(Input.UV);

                const Light MainLight = GetMainLight();

                // apply direct
                BaseColor += GetDirectLightFalloff(SmoothNormal, MainLight.direction).xxxx * float4(MainLight.color.rgb, 0);

                // apply specular
                BaseColor += GetSpecularFalloff(SmoothNormal, Input.PositionWS, MainLight.direction);

                // apply background color
                const float2 UVSS = ModifiedPositionSS.xy / ModifiedPositionSS.w;
                BaseColor = float4(BlendWithBackground(BaseColor, UVSS), 1);

                return BaseColor;
            }
            ENDHLSL
        }
    }
}