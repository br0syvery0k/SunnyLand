using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreInHell : MonoBehaviour
{
    private Text FinalScore;
    private int Score;
    void Start()
    {
        FinalScore = GameObject.FindGameObjectWithTag("FinalScoreInHell").GetComponent<Text>();
        Score = PlayerPrefs.GetInt("Score");
        FinalScore.text = Score.ToString();
        if (Score > PlayerPrefs.GetInt("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", Score);
    }
}
