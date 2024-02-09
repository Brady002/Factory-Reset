using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public enum colliderType { head, body, arm }
    public colliderType bodyPart;
    public BaseEnemy Controller;
    [SerializeField] private float headMultiplyer = 1.5f;
    [SerializeField] private float bodyMultiplyer = 1f;
    [SerializeField] private float armMultiplyer = 0.7f;

    public void SetDamage(float WeaponDamage, Transform hitPosition)
    {
        switch(bodyPart)
        {
            case colliderType.head:
                Controller.TakeDamage(WeaponDamage * headMultiplyer, hitPosition);
                break;

            case colliderType.body:
                Controller.TakeDamage(WeaponDamage * bodyMultiplyer, hitPosition);
                break;
            case colliderType.arm:
                Controller.TakeDamage(WeaponDamage * armMultiplyer, hitPosition);
                break;
        }

    }


}
