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
        Attributes(damage, friendly);
    }
    public void Attributes(float _damage, bool _friendly)
    {
        damage = _damage;
        friendly = _friendly;
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
