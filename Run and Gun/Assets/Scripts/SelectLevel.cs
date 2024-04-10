using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    private string level;
    public void DevLevel(float waitTime)
    {
        level = "Dev Level";
        StartCoroutine(LoadLevel(waitTime));

    }
    public void LevelOne(float waitTime)
    {
        level = "Level One";
        StartCoroutine(LoadLevel(waitTime));
    }

    public void LevelTwo(float waitTime)
    {
        level = "Level Two";
        StartCoroutine(LoadLevel(waitTime));
    }

    private IEnumerator LoadLevel(float waitTime)
    {
        if(waitTime == null)
        {
            waitTime = 0;
        }
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(level);
    }
    
}
