using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDeleter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CharacterController>())
        {
            CharacterController character = other.GetComponent<CharacterController>();
            Destroy(character.equipLeft);
            character.equipLeft = null;
            Destroy(character.equipRight);
            character.equipRight = null;
            character.canUseLeft = false;
            character.canUseRight = false;
            character.rightHand.SetBool("Equipped", false);
            character.leftHand.SetBool("Equipped", false);
        }
    }
}
