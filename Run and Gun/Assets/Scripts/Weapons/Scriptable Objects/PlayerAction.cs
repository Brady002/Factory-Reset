using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerWeaponSelector WeaponSelector;

    private void Update()
    {
        if (Input.GetMouseButton(0) && WeaponSelector.ActiveWeaponRight != null) {
            WeaponSelector.ActiveWeaponRight.Use(2);
        }

        if (Input.GetMouseButton(1) && WeaponSelector.ActiveWeaponLeft != null)
        {
            WeaponSelector.ActiveWeaponLeft.Use(1);
        }
    }
}
