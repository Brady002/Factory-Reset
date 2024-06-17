using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    [Header("General Settings")]
    public float appearPosition;
    public float dissapearPosition;
    public float effectSpeed = 5.0f;

    [Header("Touch Settings")]
    public bool touchActivated = true;
    public bool oneTime = false;
    public float initialDelay = 0f;
    public float reapperanceDelay = 2.0f;
    private bool triggered = false;

    private Collider[] collision;
    private Renderer mesh;

    private void Start()
    {
        mesh = GetComponent<Renderer>();
        collision = GetComponents<Collider>();
    }
    public void StartMaterialization()
    {
        StartCoroutine(Materialize(effectSpeed));
    }

    public void StartDematerialization()
    {
        StartCoroutine(Dematerialize(effectSpeed));
    }

    private IEnumerator Materialize(float speed)
    {
        foreach(Collider box in collision)
        {
            box.enabled = true;
        }
        
        float currentPos = appearPosition;
        while (currentPos >= dissapearPosition)
        {
            currentPos -= Time.deltaTime * speed;

            mesh.material.SetFloat("_Cutoff", currentPos);
            yield return null;
        }
    }

    private IEnumerator Dematerialize(float speed)
    {
        float currentPos = dissapearPosition;
        while (currentPos <= appearPosition)
        {
            currentPos += Time.deltaTime * speed;

            mesh.material.SetFloat("_Cutoff", currentPos);
            yield return null;
        }

        foreach (Collider box in collision)
        {
            box.enabled = false;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if(touchActivated && !triggered)
        {
            triggered = true;
            StartCoroutine(TouchCycle());
        }
    }

    private IEnumerator TouchCycle()
    {
        yield return new WaitForSeconds(initialDelay);
        StartDematerialization();
        if(!oneTime)
        {
            yield return new WaitForSeconds(reapperanceDelay);
            StartMaterialization();
        }
        triggered = false;
    }
}
