using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{

    [SerializeField]
    public float _MaxHealth = 25;
    [SerializeField]
    public float _Health;
    public float CurrentHealth { get => _Health; private set => _Health = value; }
    public float MaxHealth { get => _MaxHealth; private set => _MaxHealth = value; }

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;



    private void OnEnable()
    {
        CurrentHealth = MaxHealth;
    }
    public void TakeDamage(float Damage)
    {
        float damageTaken = Mathf.Clamp(Damage, 0, CurrentHealth);
        CurrentHealth -= damageTaken;
        if(damageTaken != 0)
        {
            OnTakeDamage?.Invoke(damageTaken);
            Debug.Log("Current Health = " + CurrentHealth);
        }


        if(CurrentHealth == 0 && damageTaken != 0)
        {
            
            OnDeath?.Invoke(transform.position);
        }
    }
}
