using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseWeapon : MonoBehaviour
{
    public float damage;
    public float range;
    public float cooldown;
    public bool canDestroy;
    public Transform origin;

    public void Use()
    {
        Fire(damage, range, cooldown);
    }

    public abstract void Fire(float damage, float range, float cooldown);


}
