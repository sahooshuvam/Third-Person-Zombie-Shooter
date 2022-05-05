using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score;
    [SerializeField] Text scoreText;

    public void ScoreCalculator(int scoreValue)
    {
        score = score + scoreValue;
        scoreText.text = score.ToString();
    }
}
