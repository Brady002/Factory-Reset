using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    private int score = 0;
    public TMP_Text display;

    private void Start()
    {
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
}
