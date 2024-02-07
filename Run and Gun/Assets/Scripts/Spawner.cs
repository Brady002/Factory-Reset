using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject objectToSpawn;
    private Transform spawnPoint;

    private void Start()
    {
        spawnPoint = this.transform;
    }
    public void Spawn()
    {
        Instantiate(objectToSpawn, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
    }
}
