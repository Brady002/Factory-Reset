using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class PillarControlScript : MonoBehaviour
{

    public Animator animator;
    [SerializeField] private int anim;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ChangeAnimation(anim);
    }

    public void ChangeAnimation(int _state)
    {
        switch (_state)
        {
            case 0:
                animator.SetTrigger("Spin");
                break;
            case 1:
                animator.SetTrigger("Spin Inverted");
                break;
            case 2:
                animator.SetTrigger("Spin Stilted");
                break;
            case 3:
                animator.SetTrigger("Wave");
                break;


        }
    }
}
