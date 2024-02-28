using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public abstract class BaseEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public EnemyState currentState;

    public float maxHealth = 25f;
    public float currentHealth;
    private bool enableAI = false;

    [SerializeField] public float aggroRange;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackDelay = 0.9f;
    public bool attacking = false;

    public ParticleSystem deathParticles;

    public GameObject player;
    public Rigidbody rb;

    public Renderer mesh;
    private void OnEnable()
    {
        currentHealth = maxHealth;
        StartCoroutine(Materialize());
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
        if(enableAI)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    UpdateIdleState(aggroRange, player);
                    break;
                case EnemyState.Patrol:
                    UpdatePatrolState();
                    break;
                case EnemyState.Chase:
                    UpdateChaseState(attackRange, player);
                    break;
                case EnemyState.Attack:
                    UpdateAttackState();
                    break;
                case EnemyState.Dead:
                    break;
            }
        }
        
    }

    public abstract void UpdateIdleState(float aggroRange, GameObject player);
    /*{
        Vector3 directionToTarget = player.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        rb.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }*/

    public abstract void UpdatePatrolState();

    public abstract void UpdateChaseState(float attackRange, GameObject player);

    public abstract void UpdateAttackState();

    public void TakeDamage(float Damage, Transform hitPosition)
    {
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
        currentState = EnemyState.Dead;
        rb.freezeRotation = false;
        Destroy(gameObject.GetComponent<NavMeshAgent>());

        // Additional actions such as playing death animation, dropping loot, etc.
        //rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        rb.AddTorque(-hitPosition.position, ForceMode.Impulse);
        rb.AddForceAtPosition(-rb.transform.forward, hitPosition.position, ForceMode.Impulse);
        deathParticles.Play();
        yield return new WaitForSeconds(1f);
        StartCoroutine(Dematerialize());    
    }

    public void SetState(EnemyState newState)
    {
        currentState = newState;
    }

    private IEnumerator Materialize()
    {
        float max = -2f;
        float elapsedTime = 2f;
        float speed = 5;
        while(elapsedTime > max) 
        { 
            elapsedTime -= Time.deltaTime * speed;

            mesh.material.SetFloat("_Cutoff", elapsedTime);
            yield return null;
        }
        enableAI = true;
    }

    private IEnumerator Dematerialize()
    {
        float max = 2f;
        float elapsedPos = -2f;
        float speed = 5;
        while (elapsedPos < max)
        {
            elapsedPos += Time.deltaTime * speed;

            mesh.material.SetFloat("_Cutoff", elapsedPos);
            yield return null;
        }
        Destroy(this.gameObject.GetComponentInChildren<Renderer>().material);
        Destroy(this.gameObject);
    }
}

