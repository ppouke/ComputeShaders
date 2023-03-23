using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUPos : MonoBehaviour
{
    // Start is called before the first frame update
    
    const int maxRes = 1000;
    [SerializeField, Range(10, maxRes)]
    int resolution;

    public enum ChaoticAttractor{Lorenz, Aizawa}
    [SerializeField]
    ChaoticAttractor chaoticAttractor;

    ComputeBuffer positionsBuffer;

    [SerializeField] 
    ComputeShader computeShader;

    [SerializeField]
    Material material;
    Mesh mesh;

    static readonly int positionsId = Shader.PropertyToID("_Positions"),
    resolutionId = Shader.PropertyToID("_Resolution");

    
    void OnEnable()
    {
        positionsBuffer = new ComputeBuffer(maxRes, 3 * 4);
    }

    void OnDisable(){
        positionsBuffer.Release();
        positionsBuffer = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateGPU(){

        computeShader.SetInt(resolutionId, resolution);

        computeShader.SetBuffer((int)chaoticAttractor, positionsId, positionsBuffer);
        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch((int)chaoticAttractor, groups, groups, 1);
        material.SetBuffer(positionsId, positionsBuffer);
        
        var bounds = new Bounds(Vector3.zero, Vector3.one * 2f);
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution);



    }

    
}
