using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationToRagdoll : MonoBehaviour
{
    [SerializeField] private Collider wholeCollider;
    [SerializeField] private float respawnTime = 30f;
    private Rigidbody[] rigidbodies;
    private bool IsRagdoll = false;
    
    private Joint[] joints;
    [SerializeField] private float minForceToAdd = 50f;
    public int currentScore = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        wholeCollider = GetComponent<BoxCollider>();
        ToggleRagdoll(true);
        joints = GetComponentsInChildren<Joint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleRagdoll(!IsRagdoll);
            Debug.Log("spcebar presed");
        }
    }

    private void FixedUpdate()
    {
        if (IsRagdoll)
        {
            currentScore += ScoreRagdoll();
        }
    }

    //if ragdoll is animating, the loops through all the bones and disable physics
    //else enable the physics and stop animation.
    private void ToggleRagdoll(bool isAnimating)
    {
        IsRagdoll = !isAnimating;
        wholeCollider.enabled = isAnimating;
        foreach (Rigidbody ragdollBone in rigidbodies)
        {
            ragdollBone.isKinematic = isAnimating;
        }
        GetComponent<Animator>().enabled = isAnimating;
        if (isAnimating) PlayAnimation();
    }

    //if collision occurs, then turn on the ragdoll
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsRagdoll /*&& collision.gameObject.tag == "Projectile"*/)
        {
            ToggleRagdoll(false);
            StartCoroutine(GetBackUp());
        }
    }

    //get back up after a certain time for a replay
    private IEnumerator GetBackUp()
    {
        yield return new WaitForSeconds(respawnTime);
        ToggleRagdoll(true);
        currentScore = 0;
    }

    void PlayAnimation()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Move");
    }
    
    //loops through all joints and add up the force applied.
    //need to divide by 100 because the number is too big for a score.
    //only calculate the force if the impact is bigger than a threshold
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
}
