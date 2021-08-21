using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollScoring : MonoBehaviour
{
    private Joint[] joints;
    [SerializeField] private float minForceToAdd = 50f;
    
    public int currentScore = 0;
    private void OnEnable()
    {
        joints = GetComponentsInChildren<Joint>();
    }

    private void FixedUpdate()
    {
        currentScore += ScoreRagdoll();
    }

    public int ScoreRagdoll()
    {
        float totalForce = 0;
        foreach (Joint joint in joints)
        {
            if (joint.currentForce.magnitude > minForceToAdd)
            {
                totalForce += joint.currentForce.magnitude * 0.01f;
            }
        }

        return (int)totalForce;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}