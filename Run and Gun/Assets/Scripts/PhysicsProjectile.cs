using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float range = 50f;
    private Vector3 startPos;
    private Rigidbody rb;
    private GameObject objectBounds;
    private bool destructable = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Attributes(speed, range); //Default stats if not instantiated elsewhere
    }
    public void Attributes(float _speed, float _range)
    {
        speed = _speed;
        range = _range;
        startPos = transform.position;
        Launch();
    }

    private void Launch()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(transform.position, startPos) >= 4f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(destructable)
        {
            Destroy(this.gameObject);
        }
        
    }

    
}
