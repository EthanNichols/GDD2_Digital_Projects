using Assets.Scripts.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    //If you want to edit the score, use ScoreManager.Instance
    private int points;

    [SerializeField] private string label = "Score";
    [SerializeField] private Text text;

    public string Label
    {
        get
        {
            return label;
        }
        set
        {
            label = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Pulls score from ScoreManager Singleton and updates the UI accordingly
    /// </summary>
    public void UpdateScore()
    {
        points = ScoreManager.Instance.GetScore;
        text.text = label + ": " + points;
    }
}
