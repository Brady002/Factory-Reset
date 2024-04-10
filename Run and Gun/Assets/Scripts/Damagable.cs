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

    private int limbPointValue;
    private string limbHit;

    public GameObject particles;
    private bool alreadyHit = false;

    public void SetDamage(float WeaponDamage, Vector3 hitPosition)
    {
        if(Controller.currentHealth > 0)
        {
            limbPointValue = 0;

            switch (bodyPart)
            {
                case colliderType.head:
                    Controller.TakeDamage(WeaponDamage * headMultiplyer, hitPosition);
                    limbPointValue = 100;
                    limbHit = "Headshot";
                    break;

                case colliderType.body:
                    Controller.TakeDamage(WeaponDamage * bodyMultiplyer, hitPosition);
                    limbPointValue = 25;
                    limbHit = "Bodyshot";
                    break;
                case colliderType.arm:
                    Controller.TakeDamage(WeaponDamage * armMultiplyer, hitPosition);
                    limbHit = "Limb";
                    break;
            }

            if (!alreadyHit) //
            {
                Instantiate(particles, hitPosition, transform.rotation);
                StartCoroutine(ShowParticles());
                alreadyHit = true;
                if(limbPointValue > 0)
                {
                    FindObjectOfType<PointSystem>().AddPoints(limbPointValue);
                    FindObjectOfType<PointSystem>().AddTextToDisplay("+ " + limbHit);
                }
                
            }
        }
        

        
        

    }

    private IEnumerator ShowParticles()
    {
        yield return new WaitForSeconds(0.1f);
        alreadyHit = false;

    }


}
