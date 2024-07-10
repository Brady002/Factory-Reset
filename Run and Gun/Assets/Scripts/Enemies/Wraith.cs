using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wraith : BaseEnemy
{
    private void Update()
    {
        if(currentState != EnemyState.Dead)
        {
            Vector3 directionToTarget = player.transform.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            rb.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }
        
    }
    
}
