﻿using Assets.Scripts.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerShip player;
    [SerializeField] private SceneSwitch sceneSwitcher;
    [SerializeField] private GameObject[] enemySpawers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("DemoScene");
            ScoreManager.Instance.ResetScore();
        }

        if (player.IsDead)
        {
            sceneSwitcher.GotoGameOver();
        }
    }
}
