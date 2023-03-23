using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPULorenz : MonoBehaviour
{
    // Start is called before the first frame update

    const int maxRes = 1000;
    [SerializeField, Range(10,maxRes)]
    int resolution = 10;

    [SerializeField]
    Mesh mesh;

    
	[SerializeField]
	Material material;
    public float[] parameters = new float[3];

    public float speed = 1;


    public float size = 5f;

    public float startPosVariance = 0.1f;


    private Vector3[] positions;

    ComputeBuffer positionsBuffer;

    [SerializeField]
    ComputeShader computeShader;





    [SerializeField]
    public float boundSize = 2f;



    static int positionsId = Shader.PropertyToID("_Positions"),
    stepId = Shader.PropertyToID("_Step"),
    paramsId = Shader.PropertyToID("_Params");

  

    void OnEnable(){
        positionsBuffer = new ComputeBuffer(resolution , 3 * 4);
    }

    void OnDisable(){
        positionsBuffer.Release();
        positionsBuffer = null;
    }
    void Start()
    {
        positions = new Vector3[resolution];

        float minPos = -startPosVariance;
        float maxPos = startPosVariance;

        for(int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector3(Random.Range(minPos,maxPos),
            Random.Range(minPos,maxPos),
            Random.Range(minPos,maxPos)
            );
        }

        positionsBuffer.SetData(positions);
    }

    // Update is called once per frame
    void Update()
    {
        LorenzAttractor();

        
    }

    void LorenzAttractor(){
        float step = Time.deltaTime * speed;
        Vector3 m = new Vector3(parameters[0], parameters[1], parameters[2]);

        /*for(int i = 0; i < positions.Length; i++){
            positions[i].x = m.x * (positions[i].y - positions[i].x) * step;
            positions[i].y = (positions[i].x * (m.y - positions[i].z) - positions[i].y) * step;
            positions[i].z = (positions[i].x * positions[i].y - m.z * positions[i].z) * step;
        }*/

        computeShader.SetVector(paramsId, m);
        computeShader.SetFloat(stepId, step);
        computeShader.SetBuffer(0, positionsId, positionsBuffer);
        int groups = Mathf.CeilToInt(resolution / 64f);
        computeShader.Dispatch(0, groups, 1, 1);


        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, size/ resolution);

        var bounds = new Bounds(Vector3.zero, Vector3.one * boundSize);
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution);


    

        
    }

    


}
