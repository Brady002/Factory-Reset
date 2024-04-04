using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class Floater : BaseEnemy
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


    private void Update()
    {
        bool inSight = false;
        turretHead.transform.LookAt(targetPosition);
        if(Physics.Raycast(shootOrigin.position, shootOrigin.forward, out RaycastHit hit, attackRange, HitMask))
        {
            if(hit.collider.GetComponent<CharacterController>())
            {
                inSight = true;
            } else
            {
                inSight = false;
            }
        }
        Debug.Log(inSight);
        //Attack: Distance > 5 and target is in sight
        if (distanceToTarget > buffer && inSight) { Invoke(nameof(Attack), .5f); }
        if(distanceToTarget < buffer && inSight) { Teleport();  }
        //Patrol: Target not in sight
        if (!inSight) { Patrol(); }
    }
    private void LateUpdate()
    {
        distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
        targetPosition = player.transform.position;

    }
    public override void UpdateIdleState(float aggroRange, GameObject player)
    {

    }

    public override void UpdatePatrolState()
    {

    }

    public override void UpdateChaseState(float attackRange, GameObject player)
    {
        
        
    }

    public override void UpdateAttackState()
    {
        
    }

    private void Patrol()
    {
        agent.SetDestination(targetPosition);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
        Vector3 directionToTarget = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        turretHead.transform.rotation = targetRotation;

        if (attacking == false)
        {
            attacking = true;
            GameObject bulletGO = Instantiate(projectile, shootOrigin);
            bulletGO.transform.parent = null;
            bulletGO.GetComponent<Hurtbox>().Attributes(attackDamage, false);
            bulletGO.GetComponent<Projectile>().Attributes(bulletSpeed, attackRange);
            Invoke(nameof(ResetAttack), 1f);
        }
        
    }

    private void ResetAttack()
    {
        attacking = false;
    }

    private void Teleport()
    {
        Vector3 randomDirection = Random.onUnitSphere * (buffer + 5);
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, buffer, 1))
        {
            finalPosition = hit.position;
        }

        rb.position = finalPosition;
    }
}
