using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{

    private Animator anim;

    public bool automatic;
    public float extendTime = 1f;
    public float retractTime = 1f;
    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        if (automatic)
        {
            Invoke(nameof(Initiate), offset);
        }
    }

    private void Initiate()
    {
        StartCoroutine(RunMachine());
    }

    private IEnumerator RunMachine()
    {
        yield return new WaitForSeconds(retractTime);
        anim.SetTrigger("Extend");
        yield return new WaitForSeconds(extendTime);
        anim.SetTrigger("Retract");
        StartCoroutine(RunMachine());
    }

    private void StopMachine()
    {
        StopCoroutine(RunMachine());
        anim.SetTrigger("Retract");
    }
}
