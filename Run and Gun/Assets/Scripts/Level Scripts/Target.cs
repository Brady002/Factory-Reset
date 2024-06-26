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
            switch (mode)
            {
                case Mode.Single:
                    if(!activated)
                    {
                        activated = true;
                        onShot.Invoke();
                    }
                    

                    break;
                case Mode.Timed:
                    if(!activated)
                    {
                        activated = true;
                        onShot.Invoke();
                        StartCoroutine(timer());
                    }
                    break;
                case Mode.Toggle:
                    if (!activated)
                    {
                        Debug.Log("shot");
                        activated = true;
                        onShot.Invoke();
                    }
                    else
                    {
                        Debug.Log("Unshot");
                        activated = false;
                        onDeactivate.Invoke();
                    }
                    break;



            }
        
    }

    private IEnumerator timer()
    {
        yield return new WaitForSeconds(duration);
        activated = false;
        onDeactivate.Invoke();


    }

}
