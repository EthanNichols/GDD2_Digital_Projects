using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;
using UnityEngine.UI;

public class HighScoreStart : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Text>().text = "High Score: " + ScoreManager.Instance.GetSessionHighScore;
    }
}
