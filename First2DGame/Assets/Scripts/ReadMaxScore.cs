using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadMaxScore : MonoBehaviour
{
    private Text MaxScore;
    void Start()
    {
        MaxScore = GameObject.FindGameObjectWithTag("MaxScore").GetComponent<Text>();
        MaxScore.text = PlayerPrefs.GetInt("MaxScore").ToString();
    }
}