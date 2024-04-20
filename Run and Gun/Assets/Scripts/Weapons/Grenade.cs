using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Grenade : BaseWeapon
{
    // Start is called before the first frame update

    public Vector3 Spread;
    private float LastUse = 0;
    public LayerMask HitMask;
    public GameObject projectile;


    private void Start()
    {
    }
    public override void Fire(float damage, float range, float cooldown, Animator _hand)
    {
        if (Time.time > cooldown + LastUse)
        {
            _hand.SetFloat("ShootSpeed", 1/cooldown);
            _hand.SetTrigger("Fire");

            LastUse = Time.time;
            Vector3 aimDirection = origin.forward
                    + new Vector3(
                        Random.Range(-Spread.x, Spread.x),
                        Random.Range(-Spread.y, Spread.y),
                        Random.Range(-Spread.z, Spread.z)
                        );
            aimDirection.Normalize();
            GameObject projectileGO = Instantiate(projectile, origin);
            projectileGO.transform.parent = null;
        }
        
    }

}
