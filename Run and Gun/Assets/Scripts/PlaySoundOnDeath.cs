using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class PlaySoundOnDeath : MonoBehaviour
{
    public IDamageable Damageable;
    public GameObject sphere;
    private Rigidbody rb;
    private AudioSource source;

    private void Awake()
    {
        Damageable = GetComponent<IDamageable>();
    }
    private void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

    }

    private void OnEnable()
    {
        Damageable.OnDeath += Damageable_OnDeath;
    }

    private void Damageable_OnDeath(Vector3 Position)
    {
        if (source != null)
        {
            source.Play();
            rb.useGravity = true;
            StartCoroutine(DeleteThisObject());
        }
        
    }

    private IEnumerator DeleteThisObject()
    {
        yield return new WaitForSeconds(2f);
        Destroy(sphere);
    }
}
