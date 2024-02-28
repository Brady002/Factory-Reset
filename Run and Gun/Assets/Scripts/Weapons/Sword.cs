using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Sword : BaseWeapon
{
    // Start is called before the first frame update

    public Rigidbody rb;
    private float LastUse = 0;
    public LayerMask HitMask;
    private bool attacking = false;
    public float attackTime = 0.4f;

    private void Start()
    {
        
    }
    public override void Fire(float damage, float range, float cooldown, Animator _hand)
    {
        if(rb == null)
        {
            rb = this.transform.parent.parent.parent.GetComponent<Rigidbody>();
        }

        if (Time.time > cooldown + LastUse)
        {
            _hand.SetFloat("ShootSpeed", 1 / (cooldown - 1));
            _hand.SetTrigger("Fire");
            LastUse = Time.time;
            attacking = true;
            StartCoroutine(Attacking());
            Vector3 aimDirection = origin.forward;
            

            aimDirection.Normalize();
        }
    }

    private IEnumerator Attacking()
    {
        yield return new WaitForSeconds(attackTime);
        attacking = false;
    }

    private void Update()
    {
        if(attacking)
        {
            rb.velocity = rb.transform.forward * 30;
            if (Physics.SphereCast(origin.position, range, origin.forward, out RaycastHit collision, range, HitMask) && attacking)
            {
                Collider[] enemies = Physics.OverlapSphere(collision.point, 2);
                foreach (Collider c in enemies)
                {
                    if (c.gameObject.GetComponent<Damagable>())
                    {
                        Damagable damagable = c.GetComponent<Damagable>();
                        Transform hitpoint = collision.transform;
                        damagable.SetDamage(damage, hitpoint);
                    }
                }
            }
        }
    }
}
