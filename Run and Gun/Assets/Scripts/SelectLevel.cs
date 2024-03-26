using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{

    public void DevLevel()
    {
        SceneManager.LoadScene("Level One");

    }
    public void LevelOne()
    {
        SceneManager.LoadScene("Level One");
    }

    public void LevelTwo()
    {
        SceneManager.LoadScene("Level Two");
    }

    
}
