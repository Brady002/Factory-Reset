using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillGate : MonoBehaviour
{
    public List<GameObject> EnemyList = new List<GameObject>();

    public UnityEvent allEnemiesDead;
    void Update()
    {
        for(int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == null) {
            EnemyList.RemoveAt(i);
            }
        }

        if(EnemyList.Count <= 0) {
        allEnemiesDead.Invoke();
        }
    }
}
