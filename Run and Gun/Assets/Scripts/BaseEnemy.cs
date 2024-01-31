using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    private EnemyState currentState;

    public float maxHealth = 25f;
    public float currentHealth;

    public ParticleSystem deathParticles;

    private GameObject player;

    private Rigidbody rb;
    private void OnEnable()
    {
        currentHealth = maxHealth;
        
    }

    private void Start()
    {
        currentState = EnemyState.Idle;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdleState();
                break;
            case EnemyState.Patrol:
                UpdatePatrolState();
                break;
            case EnemyState.Chase:
                UpdateChaseState();
                break;
            case EnemyState.Attack:
                UpdateAttackState();
                break;
            case EnemyState.Dead:
                // Handle dead state
                break;
        }
    }

    private void UpdateIdleState()
    {
        // Implement logic for the Idle state
        // For example, wait for a certain time, then transition to another state
        // e.g., currentState = EnemyState.Patrol;
        Vector3 directionToTarget = player.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        rb.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }

    private void UpdatePatrolState()
    {
        // Implement logic for the Patrol state
        // Move the enemy along a predefined path
        // Transition to Chase state if the player is detected
    }

    private void UpdateChaseState()
    {
        // Implement logic for the Chase state
        // Move towards the player
        // Transition to Attack state if the player is within attack range
        // Transition back to Patrol if the player is out of range
    }

    private void UpdateAttackState()
    {
        // Implement logic for the Attack state
        // Perform the attack action
        // Check for hits using hitboxes

        // Transition back to Chase state after attacking
    }

    private void DealDamage(GameObject target, int damage)
    {
        // Deal damage to the target (player or another enemy)
        // You can add more sophisticated logic here based on your game design
        // For simplicity, let's subtract damage directly from the target's health
        //target.GetComponent<PlayerHealth>().TakeDamage(damage);
    }

    public void TakeDamage(float Damage, Transform hitPosition)
    {
        Debug.Log("Pain");
        float damageTaken = Mathf.Clamp(Damage, 0, currentHealth);
        currentHealth -= damageTaken;
        if (damageTaken != 0)
        {

        }


        if (currentHealth == 0 && damageTaken != 0)
        {

            StartCoroutine(Die(hitPosition));
        }
    }

    private IEnumerator Die(Transform hitPosition)
    {
        // Implement logic for when the enemy dies
        currentState = EnemyState.Dead;
        rb.freezeRotation = false;
        // Additional actions such as playing death animation, dropping loot, etc.
        //rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        rb.AddTorque(-hitPosition.position, ForceMode.Impulse);
        rb.AddForceAtPosition(-rb.transform.forward, hitPosition.position, ForceMode.Impulse);
        deathParticles.Play();
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
        
    }

    public void SetState(EnemyState newState)
    {
        currentState = newState;
    }
}

