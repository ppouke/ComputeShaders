using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{

    [SerializeField]
    Transform rotationAxis;

    [SerializeField]
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.RotateAround(rotationAxis.position, Vector3.up, speed * Time.deltaTime);

        transform.LookAt(rotationAxis);
        
    }
}
