using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class PlanarShadowFeature : ScriptableRendererFeature
{
    public Shader shader;
    private PlanarShadowPass _pass;
    public override void Create()
    {
        _pass = new PlanarShadowPass(shader);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_pass);
    }
}
