using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class MiniByter : BaseEnemy
{
    public NavMeshAgent agent;

    private float distanceToTarget;
    private Vector3 targetPosition;

    private float shootRange = 10f;

    [Header("Attacks")]
    public GameObject meleePoint;

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
}
