using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTitle : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject cam;
    private Animator anim;
    private bool firstTime = true;
    public Dissolve titleWall;
    public Dissolve blackScreen;
    private void Start()
    {
        cam = GetComponent<GameObject>();
        anim = GetComponent<Animator>();
    }

    public void ToMain()
    {
        anim.SetTrigger("To Main");
    }
    public void ToLevelSelect()
    {
        if(firstTime)
        {
            firstTime = false;
            titleWall.StartDematerialization(15);
            Invoke(nameof(StartToLevel), 1f);
        }
        else
        {
            StartToLevel();
        }
        
    }

    private void StartToLevel()
    {
        anim.SetTrigger("To Level Select");
    }
    public void HowToPlay()
    {
        anim.SetTrigger("To Controls");
    }

    public void LoadLevel()
    {
        anim.SetTrigger("Load");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
