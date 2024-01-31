using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject teleportPosition;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = teleportPosition.transform.position;
    }
}
