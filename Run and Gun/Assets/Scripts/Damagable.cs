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

    [SerializeField] private int limbPointValue;

    public GameObject particles;
    private bool alreadyHit = false;

    public void SetDamage(float WeaponDamage, Vector3 hitPosition)
    {
        if(Controller.currentHealth > 0)
        {
            if (!alreadyHit) //
            {
                Instantiate(particles, hitPosition, transform.rotation);
                StartCoroutine(ShowParticles());
                alreadyHit = true;
            }

            switch (bodyPart)
            {
                case colliderType.head:
                    Controller.TakeDamage(WeaponDamage * headMultiplyer, hitPosition);
                    FindObjectOfType<PointSystem>().AddPoints(100);
                    break;

                case colliderType.body:
                    Controller.TakeDamage(WeaponDamage * bodyMultiplyer, hitPosition);
                    FindObjectOfType<PointSystem>().AddPoints(25);
                    break;
                case colliderType.arm:
                    Controller.TakeDamage(WeaponDamage * armMultiplyer, hitPosition);
                    FindObjectOfType<PointSystem>().AddPoints(10);
                    break;
            }
        }
        

        
        

    }

    private IEnumerator ShowParticles()
    {
        yield return new WaitForSeconds(0.1f);
        alreadyHit = false;

    }


}
