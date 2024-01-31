using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerWeaponSelector : MonoBehaviour
{
    [SerializeField]
    private WeaponType WeaponRight;
    [SerializeField]
    private WeaponType WeaponLeft;
    [SerializeField]
    private Transform WeaponParent;
    [SerializeField]
    private List<WeaponScriptableObject> Weapons;
    //[SerializeField]
    //private PlayerIK InverseKinematics;

    [Space]
    [Header("Runtime Filled")]
    public WeaponScriptableObject ActiveWeaponRight;
    public WeaponScriptableObject ActiveWeaponLeft;

    private void Start()
    {
        WeaponScriptableObject weapon1 = Weapons.Find(weapon1 => weapon1.Type == WeaponRight);
        WeaponScriptableObject weapon2 = Weapons.Find(weapon2 => weapon2.Type == WeaponLeft);
        if (weapon1 == null) 
        { 
            Debug.LogError($"No Weapon found for WeaponType: {weapon1}");
            return;
        }
        if (weapon2 == null)
        {
            Debug.LogError($"No Weapon found for WeaponType: {weapon2}");
            return;
        }

        ActiveWeaponRight = weapon2;
        ActiveWeaponLeft = weapon1;
        weapon1.Spawn(WeaponParent, this, 1);
        weapon2.Spawn(WeaponParent, this, 2);
        Debug.Log(ActiveWeaponLeft +  " " + ActiveWeaponRight);
    }
}
