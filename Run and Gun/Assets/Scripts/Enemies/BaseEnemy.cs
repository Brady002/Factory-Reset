using System.Collections;
using System.Collections.Generic;
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
    public string enemyName = "";
    public float maxHealth = 25f;
    public float currentHealth;
    public List<Collider> damageSources;

    private bool enableAI = false;

    [SerializeField] public float aggroRange;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackDelay = 0.9f;
    public bool attacking = false;

    //public ParticleSystem deathParticles;
    public int deathPointValue = 50;

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
        rb.useGravity = !OnSlope();
    }


    private bool OnSlope() //Checks to see if enemy is on slope.
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < 45 && angle != 0;
        }

        return false;
    }
    public void TakeDamage(float Damage, Vector3 hitPosition)
    {
        float damageTaken = Mathf.Clamp(Damage, 0, currentHealth);
        if(damageSources.Count != 0)
        {
            StartCoroutine(ClearDamageArray());
        }
        currentHealth -= damageTaken;

        if (currentHealth == 0 && damageTaken != 0)
        {

            StartCoroutine(Die(hitPosition));
        }
    }

    private IEnumerator ClearDamageArray()
    {
        yield return new WaitForSeconds(.5f);
        damageSources.RemoveAt(0);
    }

    private IEnumerator Die(Vector3 hitPosition)
    {
        currentState = EnemyState.Dead;
        FindObjectOfType<PointSystem>().AddPoints(deathPointValue);
        FindObjectOfType<PointSystem>().AddTextToDisplay("+ Killed " + enemyName);

        rb.freezeRotation = false;
        Destroy(gameObject.GetComponent<NavMeshAgent>());

        rb.AddTorque(-hitPosition, ForceMode.Impulse);
        rb.AddForceAtPosition(-rb.transform.forward, hitPosition, ForceMode.Impulse);
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

