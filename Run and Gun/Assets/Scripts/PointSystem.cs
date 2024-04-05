using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    private int score = 0;
    public TMP_Text display;

    public List<string> messages = new List<string>();
    public List<TMP_Text> rows = new List<TMP_Text>();
    private List<float> alphaTime = new List<float>() { 3, 3, 3, 3, 3 }; 

    private void Start()
    {
       AddTextToDisplay("");
       display.text = score.ToString();
    }
    public void AddPoints(int points)
    {
        score += points;
        display.text = score.ToString();
    }

    public void SubtractPoints(int points)
    {
        int pointLoss = Mathf.Clamp(points, 0, 999999);
        score -= pointLoss;
        display.text = score.ToString();
    }

    public void AddTextToDisplay(string _message)
    {
        messages.Add(_message);
        for (int c = messages.Count - 1; c > 0; c--)
        {
            if(c != 1)
            {
                messages[c - 1] = messages[c - 2];
                alphaTime[c -1] = alphaTime[c - 2];
            } else
            {
                messages[c - 1] = messages[messages.Count - 1];
            }


        }
        messages.RemoveAt(5);

        for (int i = 0; i < rows.Count; i++)
        {
            if(_message != null || _message != string.Empty)
            {
                rows[i].text = messages[i].ToString();
                
            } else
            {
                rows[i].text = string.Empty;
            }
            alphaTime[0] = 3;
        }
    }

    private void LateUpdate()
    {

        for(int t = 0; t < alphaTime.Count; t++)
        {
            if (alphaTime[t] > 0)
            {
                alphaTime[t] -= Time.deltaTime;
                rows[t].color = new Vector4(1, 1, 1, Mathf.Lerp(0, 1, alphaTime[t]));
            }
        }
    }
}
