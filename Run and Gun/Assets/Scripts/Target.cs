using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    public bool activated = false;
    public UnityEvent onShot;

    private void Update()
    {
        if(activated)
        {
            activated = false;
            onShot.Invoke();
        }
    }
    public void ActivateEvent()
    {
        print("Event");
    }

}
