using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public bool onlyPlayerTriggered = true;
    public UnityEvent TriggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        TriggerEntered.Invoke();
    }
}
