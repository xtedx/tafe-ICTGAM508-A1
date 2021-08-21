using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{
    public FixedJoint fj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fj == null)
        {
            //Debug.Log("connection broken");
        }
    }
    
    
    
}
