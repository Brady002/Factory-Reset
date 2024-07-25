using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatusEffect : MonoBehaviour
{
    public enum StatusEffects
    {
        None,
        Slow,
        Quicken,
    }

    [SerializeField] private StatusEffects status = StatusEffects.Slow; //1 - Slow, 2 - Quicken
    [SerializeField] private bool friendly = false;
    [SerializeField] private int strength = 50; //Percentage based
    [SerializeField] private float duration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Attributes((int)status, strength, duration, friendly);
    }
    public void Attributes(int _status, int _strength, float _duration, bool _friendly)
    {
        status = (StatusEffects)Enum.GetValues(status.GetType()).GetValue(_status);
        strength = _strength;
        friendly = _friendly;
        duration = _duration;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (friendly)
        {
            if (other.GetComponent<BaseEnemy>())
            {
                BaseEnemy target = other.GetComponent<BaseEnemy>();
                NavMeshAgent agent = target.GetComponent<NavMeshAgent>();
            }
        }
        else
        {
            if (other.GetComponent<CharacterController>())
            {
                CharacterController player = other.GetComponent<CharacterController>();
                player.ActivateStatus((int)status, strength, duration);
            }
        }
        
    }
}
