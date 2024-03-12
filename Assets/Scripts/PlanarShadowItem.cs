using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlanarShadowItem : MonoBehaviour
{
    void Start()
    {
        var renderer = GetComponent<MeshRenderer>();
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        PlanarShadowManager.Instance.AddRenderer(renderer);
    }
}
