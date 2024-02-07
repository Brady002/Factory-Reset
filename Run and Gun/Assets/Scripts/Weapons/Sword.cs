using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

public class Sword : BaseWeapon
{
    // Start is called before the first frame update

    public Rigidbody rb;
    private float LastUse = 0;
    public LayerMask HitMask;
    private bool attacking = false;
    private float attackTime = 0.3f;

    private void Start()
    {
        
    }
    public override void Fire(float damage, float range, float cooldown)
    {
        if(rb == null)
        {
            rb = this.transform.parent.parent.parent.GetComponent<Rigidbody>();
        }

        if (Time.time > cooldown + LastUse)
        {
            LastUse = Time.time;
            attacking = true;
            Vector3 aimDirection = origin.forward;
            

            aimDirection.Normalize();
            if(Physics.Raycast(origin.position, aimDirection, out RaycastHit hit, range, HitMask) && attacking)
            {
                if (hit.collider.GetComponent<Damagable>())
                {
                    Damagable damagable = hit.collider.GetComponent<Damagable>();
                    Transform hitpoint = hit.transform;
                    damagable.SetDamage(damage, hitpoint);
                }
            }

        }
    }

    private IEnumerator Attacking()
    {
        yield return new WaitForSeconds(attackTime);
        attacking = false;
    }
}
