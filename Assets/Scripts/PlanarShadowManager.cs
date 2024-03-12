using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum PlanarType
{
    Any,
    Ground
}
public class PlanarShadowManager : MonoBehaviour
{
    private static PlanarShadowManager instance;

    public static PlanarShadowManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = GameObject.Find("PlanarReflectionMgr").GetComponent<PlanarShadowManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    [SerializeField] private ForwardRendererData _data;
    
    public List<Renderer> renderers = new List<Renderer>();
    private short _planarTypeFlag = 0;
    public List<Transform> planars = new List<Transform>();
    public List<Vector3> normals = new List<Vector3>();

    private PlanarShadowFeature PlanarShadowFeature;
    private void Awake()
    {
        PlanarShadowFeature = _data.rendererFeatures.OfType<PlanarShadowFeature>().FirstOrDefault();
    }

    private void OnDisable()
    {
        renderers.Clear();
        planars.Clear();
        normals.Clear();
    }

    public void AddRenderer(Renderer renderer)
    {
        renderers.Add(renderer);
    }

    public int AddPlanar(Transform planar, bool isGround, Vector3 point1, Vector3 point2, Vector3 point3)
    {
        if (planars.Count < 8)
        {
            _planarTypeFlag |= (short)((isGround ? 1 : 0) << planars.Count);
            planars.Add(planar);
            var line1 = point1 - point2;
            var line2 = point1 - point3;
            normals.Add(Vector3.Cross(line1, line2).normalized);
            return planars.Count;
        }
        else
        {
            Debug.LogError("planner more than 8");
        }

        return 0;
    }

    public PlanarType GetPlanarType(int index)
    {
        var typeVal = _planarTypeFlag & (short)(1 << index);
        return typeVal > 0 ? PlanarType.Ground : PlanarType.Any;
    }
}
