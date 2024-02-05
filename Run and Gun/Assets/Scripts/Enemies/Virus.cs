using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Virus : BaseEnemy
{
    public NavMeshAgent agent;
    public float rotatioSpeed;
    public float movementSpeed;
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
        //Quaternion targetRotation = Quaternion.LookRotation(targetPosition, Vector3.up);
        //Quaternion desiredRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        //rb.rotation = Quaternion.Slerp(rb.rotation, desiredRotation, Time.deltaTime * rotatioSpeed);
        //rb.velocity = transform.forward * movementSpeed;
        agent.SetDestination(targetPosition);
        if((player.transform.position - transform.position).magnitude < attackRange)
        {
            currentState = EnemyState.Attack;
        }
    }

    public override void UpdateAttackState()
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

    
}
