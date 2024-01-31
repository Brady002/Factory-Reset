using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Sword : BaseWeapon
{
    // Start is called before the first frame update

    public Rigidbody rb;
    private float LastUse = 0;
    public LayerMask HitMask;

    private void Start()
    {
        //TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
    }
    public override void Fire(float damage, float range, float cooldown)
    {
        if (Time.time > cooldown + LastUse)
        {
            LastUse = Time.time;
            rb.GetComponent<CharacterController>().dashing = true;
        }
    }
}
