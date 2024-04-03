using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class Infector : BaseEnemy
{

    public GameObject projectile;
    public Transform shootOrigin;
    public GameObject turretHead;
    public NavMeshAgent agent;
    public LayerMask HitMask;

    private float distanceToTarget;
    private Vector3 targetPosition;
    public float buffer = 10f;

    [Header("Bullet Settings")]
    [SerializeField] private int attackDamage;
    [SerializeField] private float bulletSpeed;

    private void LateUpdate()
    {
        
        distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
        targetPosition = player.transform.position;

        Vector3 directionToTarget = targetPosition - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        turretHead.transform.rotation = Quaternion.Euler(targetRotation.eulerAngles);

        if(!attacking)
        {
            if(distanceToTarget < buffer)
            {
                currentState = EnemyState.Patrol;
            }
        }

    }
    public override void UpdateIdleState(float aggroRange, GameObject player)
    {
        if(distanceToTarget < aggroRange)
        {
            currentState = EnemyState.Chase;
        }

    }

    public override void UpdatePatrolState()
    {
        if(distanceToTarget < buffer)
        {
            Vector3 randomDirection = Random.insideUnitSphere * buffer;
            randomDirection += transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, buffer, 1))
            {
                finalPosition = hit.position;
            }

            rb.position = finalPosition;
        } else if (distanceToTarget > buffer && Physics.Raycast(shootOrigin.position, shootOrigin.forward, out RaycastHit hit, attackRange, HitMask))
        {
            if (hit.collider.GetComponent<CharacterController>())
            {
                agent.SetDestination(transform.position);
                currentState = EnemyState.Attack;
            } else
            {
                currentState = EnemyState.Chase;
            }
        }


    }

    public override void UpdateChaseState(float attackRange, GameObject player)
    {
        if(distanceToTarget > buffer && Physics.Raycast(shootOrigin.position, shootOrigin.forward, out RaycastHit hit, attackRange, HitMask))
        {
            if(hit.collider.GetComponent<CharacterController>())
            {
                agent.SetDestination(transform.position);
                currentState = EnemyState.Attack;
            } else
            {
                agent.SetDestination(targetPosition);
            }
            
        } else
        {
            currentState = EnemyState.Patrol;
        }
        
        
    }

    public override void UpdateAttackState()
    {
        

        if (!attacking)
        {
            if(Physics.Raycast(shootOrigin.position, shootOrigin.forward, out RaycastHit hit, attackRange, HitMask))
            {
                if (hit.collider.GetComponent<CharacterController>())
                {
                    StartCoroutine(Attack());
                } else
                {
                    currentState = EnemyState.Patrol;
                }
                
            }
            
        }
        
    }

    private IEnumerator Attack()
    {
        
        attacking = true;
        GameObject bulletGO = Instantiate(projectile, shootOrigin);
        bulletGO.transform.parent = null;
        bulletGO.GetComponent<Hurtbox>().Attributes(attackDamage, false);
        bulletGO.GetComponent<Projectile>().Attributes(bulletSpeed, attackRange);
        yield return new WaitForSeconds(attackDelay);
        attacking = false;
    }
}
