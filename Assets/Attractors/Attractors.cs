using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractors : MonoBehaviour
{
    // Start is called before the first frame update

    const int maxRes = 10000;
    [SerializeField, Range(10,maxRes)]
    int resolution = 10;


    public enum Attractor{Lorenz, Aizawa, ChenLee, FourWing, Halvorsen, Thomas, Rossler, Dadras, TSUCS};

    [SerializeField]
    Attractor attractor;


    public float speed = 1;

    public float sizeFactor = 1f;

    public float startPosVariance = 0.1f;


    private Vector3[] positions;

    ComputeBuffer positionsBuffer;

    [SerializeField]
    ComputeShader computeShader;


    [SerializeField] 
    GameObject objectInstance;

    GameObject[] gOs;


    public float startDelay = 1f;



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

        gOs = new GameObject[resolution];
        for(int i = 0; i < positions.Length; i++ ){
            gOs[i] = Instantiate(objectInstance, positions[i], Quaternion.identity);
    
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > startDelay){
            ComputeAttractor();
        }

        
    }

    void ComputeAttractor(){
        float step = Time.deltaTime * speed;
        


        computeShader.SetFloat(stepId, step);
        computeShader.SetBuffer((int)attractor, positionsId, positionsBuffer);
        int groups = Mathf.CeilToInt(resolution / 64f);

    
        computeShader.Dispatch((int)attractor, groups, 1, 1);


        //var bounds = new Bounds(Vector3.zero, Vector3.one * boundSize);
        //Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution);


        positionsBuffer.GetData(positions);

        bool hitRange = false;
        for(int i = 0; i < gOs.Length; i++){
            if(positions[i].magnitude > 900){
                hitRange = true;
                positions[i] = new Vector3(Random.Range(-startPosVariance,startPosVariance),
                Random.Range(-startPosVariance,startPosVariance),
                Random.Range(-startPosVariance,startPosVariance));
            }
            
            gOs[i].transform.position = positions[i];
            
        }

        if(hitRange)
            positionsBuffer.SetData(positions);
        


    

        
    }

    


}
