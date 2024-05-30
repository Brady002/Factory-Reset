using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public bool onlyPlayerTriggered = true;
    public bool oneTime = false;
    private bool triggered = false;
    public UnityEvent TriggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if(!oneTime)
        {
            TriggerEntered.Invoke();
        } else if (oneTime && !triggered)
        {
            triggered = true;
            TriggerEntered.Invoke();
        }
        
    }
}
