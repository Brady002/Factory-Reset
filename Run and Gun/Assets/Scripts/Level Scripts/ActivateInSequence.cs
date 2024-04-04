using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateInSequence : MonoBehaviour
{

    public List<GameObject> ObjectsToActivate = new List<GameObject>();

    public float initialDelay;
    public float delay;
    private bool done = false;

    public void StartSequence()
    {
        if(!done)
        {
            done = true;
            StartCoroutine(Sequence());
        }
        
    }
    private IEnumerator Sequence()
    {
        yield return new WaitForSeconds(initialDelay);
        for (int i = 0; i < ObjectsToActivate.Count; i++)
        {
            bool on = ObjectsToActivate[i].active;
            ObjectsToActivate[i].SetActive(!on);
            yield return new WaitForSeconds(delay);
        }
        
    }
}
