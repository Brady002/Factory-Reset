using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Virus : BaseEnemy
{
    public NavMeshAgent agent;
    public float rotationSpeed;
    public float movementSpeed;
    public GameObject attackPoint;

    private float distanceToTarget;
    private Vector3 targetPosition;

    private void Update()
    {
        if(distanceToTarget > attackRange && distanceToTarget > aggroRange)
        {
            Idle();
        } else if (distanceToTarget > attackRange && distanceToTarget < aggroRange)
        {
            Chase();
        } else if (distanceToTarget < attackRange)
        {
            StartCoroutine(Attack());
        }
    }
    private void LateUpdate()
    {
        distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
        targetPosition = player.transform.position;

    }
    private void Idle()
    {
        Vector3 directionToTarget = player.transform.position - transform.position;
        if(directionToTarget.magnitude < aggroRange)
        {
            currentState = EnemyState.Chase;
        }
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        rb.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }
    private void Chase()
    {
        if (currentState != EnemyState.Dead) { agent.SetDestination(targetPosition); }
        Vector3 offset = (player.transform.position - transform.position).normalized * (attackRange - .2f); //Prevents enemy from trying to clip into target. Recommended to set around attack range.
        if (currentState != EnemyState.Dead) { agent.SetDestination(targetPosition - offset); }
    }


    private IEnumerator Attack()
    {
        //if (currentState != EnemyState.Dead) { agent.SetDestination(targetPosition); }
        attacking = true;
        attackPoint.SetActive(true);
        yield return new WaitForSeconds(attackDelay);
        attackPoint.SetActive(false);
        attacking = false;
    }
    
}
