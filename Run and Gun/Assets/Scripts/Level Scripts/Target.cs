using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    public bool activated = false;
    public UnityEvent onShot;
    public UnityEvent onDeactivate;
    public enum Mode
    {
        Single,
        Timed,
        Toggle
    }
    public Mode mode;

    [Header("Timed Variables")]
    public float duration;

    public void Shot()
    {
        if(!activated)
        {
            switch (mode)
            {
                case Mode.Single:
                    activated = true;
                    onShot.Invoke();

                    break;
                case Mode.Timed:
                    activated = true;
                    onShot.Invoke();
                    StartCoroutine(timer());

                    break;
                case Mode.Toggle:
                    if (!activated)
                    {
                        activated = true;
                        onShot.Invoke();
                    }
                    else
                    {
                        activated = false;
                        onDeactivate.Invoke();
                    }
                    break;



            }
        }
        
    }

    private IEnumerator timer()
    {
        yield return new WaitForSeconds(duration);
        activated = false;
        onDeactivate.Invoke();


    }

}
