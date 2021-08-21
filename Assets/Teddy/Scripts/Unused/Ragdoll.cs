using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    //[SerializeField] 
    public float Force = 50f;
    //components to use in code
    public Rigidbody RBody;
    // Start is called before the first frame update
    void Start()
    {
        RBody = GetComponent<Rigidbody>();
        if (null != RBody) {Launch(RBody, Vector3.up);}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch(Rigidbody rbody, Vector3 direction)
    {
        rbody.AddForce(direction * Force);
    }
}
