using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    private string level;

    public void Level(string _level)
    {
        level = _level;
        LoadLevel();
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(level);
        
    }
    
}
