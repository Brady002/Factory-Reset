using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableTerrainObject : MonoBehaviour
{
    public GameObject self;

    public float maxHealth;
    public float currentHealth;
    // Start is called before the first frame update

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float Damage, bool canDestroy)
    {
        
        float damageTaken = Mathf.Clamp(Damage, 0, currentHealth);
        if (canDestroy)
        {
            currentHealth -= damageTaken;

            if (currentHealth <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        

        
    }
}
