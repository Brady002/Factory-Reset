using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;
    private GameObject GO;
    public UnityEvent onPickup;
    private bool canPickup = true;
    [SerializeField] public bool infinite = false;

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
            if (Input.GetMouseButton(1) && player.equipLeft == null && canPickup)
            {
                GO.transform.parent = null;
                GO.transform.parent = player.attachLeft;
                GO.transform.localPosition = new Vector3(0f, 0f, 0f);
                GO.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                GO.GetComponent<BaseWeapon>().origin = player.aim.transform;

                player.equipLeft = GO;
                player.leftHand.SetInteger("Weapon", GO.GetComponent<BaseWeapon>().type);
                player.leftHand.SetBool("Equipped", true);
                StartCoroutine(player.AllowUseOfWeapon(1));

                onPickup.Invoke();
                
                StartCoroutine(Replenish());
            } else if (Input.GetMouseButton(0) && player.equipRight == null && canPickup)
            {
                GO.transform.parent = null;
                GO.transform.parent = player.attachRight;
                GO.transform.localPosition = new Vector3(0f, 0f, 0f);
                GO.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                GO.GetComponent<BaseWeapon>().origin = player.aim.transform;

                player.equipRight = GO;
                player.rightHand.SetInteger("Weapon", GO.GetComponent<BaseWeapon>().type);
                player.rightHand.SetBool("Equipped", true);
                StartCoroutine(player.AllowUseOfWeapon(0));

                onPickup.Invoke();
                StartCoroutine(Replenish());
            }

            
        }
    }

    private IEnumerator Replenish()
    {
        if(infinite)
        {
            canPickup = false;
            yield return new WaitForSeconds(1f);
            canPickup = true;
            GO = Instantiate(weaponPrefab, this.transform);
        }
        else
        {
            {
                yield return new WaitForSeconds(1f); //This needs to be set greater or equal to the AllowUseWeapon time in the player script;
                Destroy(this.gameObject);
            }
        }
    }
}
