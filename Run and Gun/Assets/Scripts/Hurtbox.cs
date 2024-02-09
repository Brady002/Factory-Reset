using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Hurtbox : MonoBehaviour
{

    [SerializeField] private bool friendly = false;
    [SerializeField] private float damageGracePeriod = 1f;
    [SerializeField] private float damage = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Attributes(damage);
    }
    public void Attributes(float _damage)
    {
        damage = _damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (friendly)
        {
            if (other.GetComponent<Damagable>())
            {
                Damagable target = other.GetComponent<Damagable>();
                target.SetDamage(damage, other.transform);
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
