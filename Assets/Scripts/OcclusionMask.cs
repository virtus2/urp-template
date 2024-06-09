using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionMask : MonoBehaviour
{
    private MaterialPropertyBlock materialPropertyBlock;
    private Renderer meshRenderer;
    private Transform targetTransform;
    private readonly int positionPropertyID = Shader.PropertyToID("_Position");


    private void Awake()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        meshRenderer = GetComponent<Renderer>();

        SetTarget(FindFirstObjectByType<Character>().transform);
    }
    
    public void SetTarget(Transform transform)
    {
        targetTransform = transform;
    }

    private void Update()
    {
        if(targetTransform != null)
        {
            materialPropertyBlock.SetVector(positionPropertyID, targetTransform.position);
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }
    }

}
