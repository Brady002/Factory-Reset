using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class Byter : BaseEnemy
{
    public NavMeshAgent agent;

    private float distanceToTarget;
    private Vector3 targetPosition;

    private float shootRange = 10f;

    [Header("Attacks")]
    public GameObject meleePoint;
    public GameObject projectilePoint;
    private bool canShoot = true;
    public GameObject spitProjectile;
    public GameObject deathProjectile;
    private bool summoned = false;

    [Header("Bullet Settings")]
    private float bulletSpeed = 20f;
    private void Update()
    {
        if(distanceToTarget > attackRange && distanceToTarget > aggroRange)
        {
           // Idle();
        } else if (distanceToTarget > attackRange && distanceToTarget < aggroRange)
        {
            Chase();
        } else if (distanceToTarget < attackRange)
        {
            StartCoroutine(Attack());
        }

        if (currentState != EnemyState.Dead)
        {
            if (!Physics.Raycast(transform.position, Vector3.down, 1f * 0.5f + .1f, Ground)) //Used to turn on physics
            {
                rb.useGravity = true;
                StartCoroutine(EnableAgent(false));
            }
            else
            {
                rb.useGravity = false;
                StartCoroutine(EnableAgent(true));
            }

        } else
        {
            if (!summoned) { DeathSummons(); }
            
        }
    }

    private IEnumerator EnableAgent(bool _enable)
    {
        yield return new WaitForSeconds(.1f);
        if(currentState != EnemyState.Dead) { agent.enabled = _enable; }
        
    }
    private void LateUpdate()
    {
        distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
        targetPosition = player.transform.position;

    }
    private void Chase()
    {
        if (currentState != EnemyState.Dead) { agent.SetDestination(targetPosition); }
        if(distanceToTarget < shootRange && canShoot && distanceToTarget > attackRange)
        {
            StartCoroutine(Shoot());
        }
        Vector3 offset = (player.transform.position - transform.position).normalized * (2f - .2f); //Prevents enemy from trying to clip into target. Recommended to set around attack range.
        if (currentState != EnemyState.Dead && !attacking) { agent.SetDestination(targetPosition - offset); }
    }


    private IEnumerator Attack()
    {
        if (currentState != EnemyState.Dead) {
 
            if(!attacking)
            {
                meleePoint.SetActive(true);
                attacking = true;
            }
            
            yield return new WaitForSeconds(attackDelay);
            meleePoint.SetActive(false);
            attacking = false;

        }
        
    }

    private IEnumerator Shoot()
    {
        Vector3 directionToTarget = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        projectilePoint.transform.rotation = targetRotation;
        canShoot = false;

        GameObject bulletGO = Instantiate(spitProjectile, projectilePoint.transform);
        bulletGO.transform.parent = null;
        bulletGO.GetComponent<Projectile>().Attributes(bulletSpeed, 50f);
        bulletGO.GetComponent<StatusEffect>().Attributes(1, 50, 3f, false);

        yield return new WaitForSeconds(10f);
        canShoot = true;
        

    }
    
    private void DeathSummons()
    {
        Debug.Log("Summon!");
        summoned = true;
        for(int e = 0; e < 3; e++)
        {
            GameObject EggGO = Instantiate(deathProjectile, projectilePoint.transform);
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            EggGO.GetComponent<Rigidbody>().AddForce((Vector3.up * 15f) + (randomDirection * 5f), ForceMode.Impulse);
            EggGO.GetComponent<Rigidbody>().AddTorque(randomDirection * 10f, ForceMode.Impulse);
        }
       
    }
}
