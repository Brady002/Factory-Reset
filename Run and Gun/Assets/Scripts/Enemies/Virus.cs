using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Virus : BaseEnemy
{
    public NavMeshAgent agent;
    public float rotationSpeed;
    public float movementSpeed;
    public GameObject attackPoint;

    //private bool attacking = false;


    public override void UpdateIdleState(float aggroRange, GameObject player)
    {
        Vector3 directionToTarget = player.transform.position - transform.position;
        if(directionToTarget.magnitude < aggroRange)
        {
            currentState = EnemyState.Chase;
        }
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        rb.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }

    public override void UpdatePatrolState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateChaseState(float attackRange, GameObject player)
    {
        Vector3 targetPosition = player.transform.position;
        Vector3 offset = (player.transform.position - transform.position).normalized * attackRange; //Prevents enemy from trying to clip into target. Recommended to set around attack range.
        agent.SetDestination(targetPosition - offset);
        if((player.transform.position - transform.position).magnitude < attackRange)
        {
            currentState = EnemyState.Attack;
        }
    }

    public override void UpdateAttackState()
    {
        if ((player.transform.position - transform.position).magnitude < attackRange)
        {
            if(!attacking)
            {
                StartCoroutine(Attack());
            }
            
        } else
        {
            currentState = EnemyState.Chase;
        }
    }

    private IEnumerator Attack()
    {
        attacking = true;
        attackPoint.SetActive(true);
        yield return new WaitForSeconds(attackDelay);
        attackPoint.SetActive(false);
        attacking = false;
    }
    
}
