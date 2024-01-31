using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;
    private GameObject GO;
    public UnityEvent onPickup;

    // Start is called before the first frame update
    void Start()
    {
        GO = Instantiate(weaponPrefab, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<CharacterController>(out CharacterController player))
        {
            if (Input.GetMouseButton(1) && player.equipLeft == null)
            {
                GO.transform.parent = null;
                GO.transform.parent = player.attachLeft;
                GO.transform.localPosition = new Vector3(0f, 0f, 0f);
                GO.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                GO.GetComponent<BaseWeapon>().origin = player.aim.transform;
                player.equipLeft = GO;
            }

            if (Input.GetMouseButton(0) && player.equipRight == null)
            {
                GO.transform.parent = null;
                GO.transform.parent = player.attachRight;
                GO.transform.localPosition = new Vector3(0f, 0f, 0f);
                GO.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                GO.GetComponent<BaseWeapon>().origin = player.aim.transform;
                player.equipRight = GO;
            }

            onPickup.Invoke();
        }
    }
}
