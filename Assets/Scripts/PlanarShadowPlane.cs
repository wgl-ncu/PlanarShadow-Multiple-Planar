using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanarShadowPlane : MonoBehaviour
{
    public bool isGround;
    public List<Transform> planePoints;
    private int refId = Shader.PropertyToID("_StencilRef");
    void Start()
    {
        int refvalue = 0;
        if (planePoints.Count >= 3)
        {
            refvalue = PlanarShadowManager.Instance.AddPlanar(transform, isGround, planePoints[0].position, planePoints[1].position, planePoints[2].position);
        }

        var mats = GetComponent<Renderer>().materials;
        foreach (var mat in mats)
        {
            mat.SetInt(refId, refvalue);
        }
    }
}
