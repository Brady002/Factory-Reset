using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour
{

    public float force = 25f;
    public Vector3 forceDirection = Vector3.up;
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Rigidbody>())
        {
            other.GetComponent<Rigidbody>().AddForce(forceDirection * force, ForceMode.Force);
        }
        
    }

}
