using Assets.Scripts.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private int points;
    [SerializeField] private string label = "Score";
    [SerializeField] private Text text;

    public int Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
            UpdateScore();
        }
    }

    public string Label
    {
        get
        {
            return label;
        }
        set
        {
            label = value;
            UpdateScore();
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

    public void UpdateScore()
    {
        points = ScoreManager.Instance.GetScore;
        text.text = label + ": " + points;
    }
}
