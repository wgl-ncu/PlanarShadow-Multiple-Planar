using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlanarShadowPass : ScriptableRenderPass
{
    private Material _material;
    private ProfilingSampler _sampler = new ProfilingSampler("Planar Shadow");
    private int planarHeightsId = Shader.PropertyToID("_heights");
    private int castMatrixsId = Shader.PropertyToID("_castMatrixs");
    private int planarTypesId = Shader.PropertyToID("_planarTypes");
    private List<Renderer> _renderers => PlanarShadowManager.Instance.renderers;
    private List<Transform> _planars => PlanarShadowManager.Instance.planars;

    private List<Vector3> _normals => PlanarShadowManager.Instance.normals;

    private Matrix4x4[] castMatrixs = new Matrix4x4[8];
    private float[] planarHeight = new float[8];
    public PlanarShadowPass(Shader shader)
    {
        _material = CoreUtils.CreateEngineMaterial(shader);
        renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, _sampler))
        {
            SetPlanars(context, ref renderingData, cmd);
            DoRender(context, ref renderingData, cmd);
        }
        context.ExecuteCommandBuffer(cmd);
        cmd.Release();
    }

    private void DoRender(ScriptableRenderContext context, ref RenderingData renderingData, CommandBuffer cmd)
    {
        int planarCount = _planars.Count;
        for (int i = 0; i < _renderers.Count; ++i)
        {
            for (int j = 0; j < planarCount; ++j)
            {
                cmd.DrawRenderer(_renderers[i], _material, 0, j);
                //context.ExecuteCommandBuffer(cmd);
            }
        }
    }

    private void SetPlanars(ScriptableRenderContext context, ref RenderingData renderingData, CommandBuffer cmd)
    {
        var mainLight = renderingData.lightData.visibleLights[renderingData.lightData.mainLightIndex];
        var lightDir = -mainLight.localToWorldMatrix.GetColumn(2);
        lightDir.w = 0.0f;
        float[] types = new float[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < _planars.Count; ++i)
        {
            if (IsPlanarNeedMatrix(i))
            {
                var planeTrans = _planars[i];
                Vector4 plane = GetPlane(_normals[i], planeTrans.position);
                castMatrixs[i] = CalculateCastMatrix(plane, lightDir);
            }
            else
            {
                planarHeight[i] = _planars[i].position.y;
            }

            types[i] = IsPlanarNeedMatrix(i) ? 0 : 1;
        }
        _material.SetMatrixArray(castMatrixsId, castMatrixs);
        _material.SetFloatArray(planarHeightsId, planarHeight);
        _material.SetFloatArray(planarTypesId, types);
    }

    private Vector4 GetPlane(Vector3 normal, Vector3 point)
    {
        return new Vector4(normal.x, normal.y, normal.z, -Vector3.Dot(normal, point));
    }

    public Matrix4x4 CalculateCastMatrix(Vector4 planar, Vector3 lightDir)
    {
        var normal = new Vector3(planar.x, planar.y, planar.z);
        var d = planar.w;
        var invNdotL = 1.0f /Vector3.Dot(normal, lightDir);
        var nx = normal.x;
        var ny = normal.y;
        var nz = normal.z;
        var lx = lightDir.x;
        var ly = lightDir.y;
        var lz = lightDir.z;
        Vector4 colmun0 = new Vector4(1f - nx * lx * invNdotL, -nx * ly * invNdotL, -nx * lz * invNdotL, 0);
        Vector4 column1 = new Vector4(-ny * lx * invNdotL, 1f - ny * ly * invNdotL, -ny * lz * invNdotL, 0);
        Vector4 column2 = new Vector4(-nz * lx * invNdotL, -nz * ly * invNdotL, 1f - nz * lz * invNdotL, 0);
        Vector4 column3 = new Vector4(-d * lx * invNdotL, -d * ly * invNdotL, -d * lz * invNdotL, 1f);
        return new Matrix4x4(colmun0, column1, column2, column3);
    }

    public bool IsPlanarNeedMatrix(int index)
    {
        var planarType = PlanarShadowManager.Instance.GetPlanarType(index);
        return planarType == PlanarType.Any;
    }
}
