using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Rigidbody>())
        {
            other.GetComponent<Rigidbody>().AddForce(transform.up * 50f, ForceMode.Force);
        }
        
    }

}
