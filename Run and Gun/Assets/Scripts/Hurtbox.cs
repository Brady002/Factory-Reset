using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Hurtbox : MonoBehaviour
{

    [SerializeField] private bool friendly = false;
    [SerializeField] private float damageGracePeriod = 1f;
    [SerializeField] private int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        Attributes(damage, friendly);
    }
    public void Attributes(int _damage, bool _friendly)
    {
        damage = _damage;
        friendly = _friendly;
    }

    private void OnTriggerStay(Collider other)
    {
        if (friendly)
        {
            if (other.GetComponent<BaseEnemy>())
            {
                BaseEnemy target = other.GetComponent<BaseEnemy>();
                if(!target.damageSources.Contains(this.GetComponent<Collider>()))
                {
                    target.damageSources.Add(this.GetComponent<Collider>());
                    target.TakeDamage(damage, other.transform.position);
                    
                }
                
            }
        }
        else
        {
            if (other.GetComponent<CharacterController>())
            {
                CharacterController player = other.GetComponent<CharacterController>();
                player.TakeDamage(damage, damageGracePeriod);
            }
        }
    }
}
