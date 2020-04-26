using Assets.Scripts.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerShip player;
    [SerializeField] private SceneSwitch sceneSwitcher;
    [SerializeField] private GameObject[] enemySpawers;
    [SerializeField] private Crosshair crosshair;

    // Start is called before the first frame update
    void Start()
    {
        string[] joysticks = Input.GetJoystickNames();
        for (int i = 0; i < joysticks.Length; i++)
        {
            //if (joysticks[i].Length > 0)
                //UpdatePlayerJoyStick(true);
        }
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

    void UpdatePlayerJoyStick(bool value)
    {
        crosshair.Visible = value;
        player.UsingGamepad = value;
    }
}
