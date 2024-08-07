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
    public int type; //1 - Revolver, 2 - Shotgun, 3 - Sword

    public void Use(Animator _hand)
    {
        Fire(damage, range, cooldown, _hand);
    }

    public abstract void Fire(float damage, float range, float cooldown, Animator _hand);


}
