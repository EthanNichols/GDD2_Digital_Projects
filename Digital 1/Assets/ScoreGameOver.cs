using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Shared;

public class ScoreGameOver : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Text>().text = "Your Score: " + ScoreManager.Instance.GetScore + "     High Score: " + ScoreManager.Instance.GetSessionHighScore;
    }
}
