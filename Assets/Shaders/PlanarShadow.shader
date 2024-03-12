Shader "PlanarShadow"
{
    HLSLINCLUDE
    
    struct appdata
    {
        float4 position : POSITION;
    };

    struct v2f
    {
        float4 positionCS : SV_POSITION;
        float4 color : TEXCOORD0;
        float4 positionWS : TEXCOORD1;
    };

    //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

    uniform float _heights[8];
    uniform float _planarTypes[8];
    uniform float4x4 _castMatrixs[8];

    float4 GetWorldPosInGround(float3 positionWS, int index)
    {
        float3 lightDir = normalize(_MainLightPosition.xyz);
        float4 shadowWS;
        shadowWS.y = min(positionWS.y, _heights[index]);
        //lightDir.y / (positionWS.y - height) = lightDir.xz / tar
        //tar = (positionWS.y - height) / lightDir.y * lightDir.xz;
        shadowWS.xz = positionWS.xz - (lightDir.xz * ((positionWS.y - _heights[index]) / lightDir.y));
        shadowWS.w = positionWS.y >= _heights[index] ? 1 : 0;
        return shadowWS;
    }

    float4 GetWorldPosInAnyPlanar(float3 positionWS, int index)
    {
        float4 shadowWS;
        float3 res = mul(_castMatrixs[index], positionWS);
        return float4(res, 1);
    }

    v2f vertBase(appdata a, int index)
    {
        v2f o;
        float3 positionWS = TransformObjectToWorld(a.position);
        float4 shadowWS;
        if(_planarTypes[index] == 1)
        {
            shadowWS = GetWorldPosInGround(positionWS, index);
        }
        else
        {
            //TODO==
            //shadowWS = GetWorldPosInAnyPlanar(positionWS, index);
        }
        o.positionWS = shadowWS;
        o.positionCS = TransformWorldToHClip(shadowWS);
        o.color = float4(0, 0, 0, shadowWS.w);
        return o;
    }

    half4 frag(v2f i):SV_Target
    {
        float4 shadowCoord = TransformWorldToShadowCoord(i.positionWS);
        ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
        half4 shadowParams = GetMainLightShadowParams();
        half shadow = SampleShadowmap(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowCoord, shadowSamplingData, shadowParams, false);

        return i.color * shadow;
    }
    
    ENDHLSL
    SubShader
    {
        Tags{ "RenderPipeline" = "UniversalPipeline" }
        LOD 100
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha

        Offset -1 , 0
        Pass
        {
            Stencil
            {
                //参考 https://docs.unity.cn/cn/2020.3/Manual/SL-Stencil.html#stencil-operation-values
                Ref  1//测试后的对比值
                Comp Equal//等于ref值即为通过
                Pass IncrSat
                Fail Keep
                ZFail keep
            }
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata a)
            {
                return vertBase(a, 0);
            }
            
            ENDHLSL
        }
        
        Pass
        {
            Stencil
            {
                //参考 https://docs.unity.cn/cn/2020.3/Manual/SL-Stencil.html#stencil-operation-values
                Ref  2//测试后的对比值
                Comp Equal//等于ref值即为通过
                Pass IncrSat
                Fail Keep
                ZFail keep
            }
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata a)
            {
                return vertBase(a, 1);
            }
            
            ENDHLSL
        }
        
        Pass
        {
            Stencil
            {
                //参考 https://docs.unity.cn/cn/2020.3/Manual/SL-Stencil.html#stencil-operation-values
                Ref  3//测试后的对比值
                Comp Equal//等于ref值即为通过
                Pass IncrSat
                Fail Keep
                ZFail keep
            }
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata a)
            {
                return vertBase(a, 2);
            }
            
            ENDHLSL
        }
        
        Pass
        {
            Stencil
            {
                //参考 https://docs.unity.cn/cn/2020.3/Manual/SL-Stencil.html#stencil-operation-values
                Ref  4//测试后的对比值
                Comp Equal//等于ref值即为通过
                Pass IncrSat
                Fail Keep
                ZFail keep
            }
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata a)
            {
                return vertBase(a, 3);
            }
            
            ENDHLSL
        }
        
        Pass
        {
            Stencil
            {
                //参考 https://docs.unity.cn/cn/2020.3/Manual/SL-Stencil.html#stencil-operation-values
                Ref  5//测试后的对比值
                Comp Equal//等于ref值即为通过
                Pass IncrSat
                Fail Keep
                ZFail keep
            }
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata a)
            {
                return vertBase(a, 4);
            }
            
            ENDHLSL
        }
        
        Pass
        {
            Stencil
            {
                //参考 https://docs.unity.cn/cn/2020.3/Manual/SL-Stencil.html#stencil-operation-values
                Ref  6//测试后的对比值
                Comp Equal//等于ref值即为通过
                Pass IncrSat
                Fail Keep
                ZFail keep
            }
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata a)
            {
                return vertBase(a, 5);
            }
            
            ENDHLSL
        }
        
        Pass
        {
            Stencil
            {
                //参考 https://docs.unity.cn/cn/2020.3/Manual/SL-Stencil.html#stencil-operation-values
                Ref  7//测试后的对比值
                Comp Equal//等于ref值即为通过
                Pass IncrSat
                Fail Keep
                ZFail keep
            }
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata a)
            {
                return vertBase(a, 6);
            }
            
            ENDHLSL
        }
        
        Pass
        {
            Stencil
            {
                //参考 https://docs.unity.cn/cn/2020.3/Manual/SL-Stencil.html#stencil-operation-values
                Ref  8//测试后的对比值
                Comp Equal//等于ref值即为通过
                Pass IncrSat
                Fail Keep
                ZFail keep
            }
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata a)
            {
                return vertBase(a, 7);
            }
            
            ENDHLSL
        }
    }
}
