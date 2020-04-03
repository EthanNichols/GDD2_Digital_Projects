using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerShip player;
    [SerializeField] private SceneSwitch sceneSwitcher;

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
        }

        if (player.IsDead)
        {
            sceneSwitcher.GotoGameOver();
        }
    }
}
