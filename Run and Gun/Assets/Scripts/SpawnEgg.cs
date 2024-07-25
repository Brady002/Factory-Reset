using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEgg : MonoBehaviour
{

    public GameObject objectToSpawn;

    private void OnEnable()
    {
        Invoke(nameof(Summon), .7f);
    }
    
    private void Summon()
    {
        GameObject SummonGO = Instantiate(objectToSpawn, transform);
        SummonGO.transform.parent = null;
        Destroy(this.gameObject);
    }
}
