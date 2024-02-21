using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject teleportPosition;
    public float delay = 0f;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = teleportPosition.transform.position;
    }

    public void StartTeleport(GameObject entity)
    {
        StartCoroutine(Teleport(entity));
    }

    private IEnumerator Teleport(GameObject entity)
    {
        Debug.Log("tp");
        yield return new WaitForSeconds(delay);
        
        entity.transform.position = teleportPosition.transform.position;
    }
}
