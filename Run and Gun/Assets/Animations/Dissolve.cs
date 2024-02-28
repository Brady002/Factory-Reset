using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public float startPos;
    public float endPos;
    public bool destroy = true;
    private Renderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<Renderer>();
    }

    public void StartMaterialization(float _speed)
    {
        StartCoroutine(Materialize(_speed));
    }

    public void StartDematerialization(float _speed)
    {
        StartCoroutine(Dematerialize(_speed));
    }

    private IEnumerator Materialize(float speed)
    {
        float currentPos = startPos;
        while (currentPos >= endPos)
        {
            currentPos -= Time.deltaTime * speed;

            mesh.material.SetFloat("_Cutoff", currentPos);
            yield return null;
        }
    }

    private IEnumerator Dematerialize(float speed)
    {
        float currentPos = endPos;
        while (currentPos <= startPos)
        {
            currentPos += Time.deltaTime * speed;

            mesh.material.SetFloat("_Cutoff", currentPos);
            yield return null;
        }

        if(destroy)
        {
            Destroy(this.gameObject.GetComponentInChildren<Renderer>().material);
            Destroy(this.gameObject);
            
        }
        
        
    }
}
