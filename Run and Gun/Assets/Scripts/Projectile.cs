using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float range = 50f;
    private Vector3 startPos;
    private Rigidbody rb;

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
    }

    // Update is called once per frame
    void Update()
    {
        float velocity = speed * Time.deltaTime;

        rb.velocity = transform.forward * speed;

        if (Vector3.Distance(transform.position, startPos) >= range)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Vector3.Distance(transform.position, startPos) >= 1.5f)
        {
            Destroy(this.gameObject);

        }
    }
}
