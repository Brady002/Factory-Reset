using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Explosive : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    private float launchSpeed = 50f;
    private float size = 10f;
    private LayerMask mask;
    private Vector3 startPos;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
       rb.AddForce(transform.forward * launchSpeed, ForceMode.Impulse);
       rb.AddForce(transform.up, ForceMode.Impulse);
       startPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(transform.position, startPos) >= 1.5f)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            anim.SetTrigger("Explode");

            Collider[] p = Physics.OverlapSphere(transform.position, 10f);
            foreach (Collider c in p)
            {
                if (c.gameObject.GetComponent<Rigidbody>())
                {
                    c.gameObject.GetComponent<Rigidbody>().AddForce((c.transform.position - transform.position) * 15f, ForceMode.Force);
                }
            }

        }
        
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
