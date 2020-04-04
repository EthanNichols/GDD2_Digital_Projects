using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private string mainMenuPath;
    [SerializeField] private string gamePath;
    [SerializeField] private string gameOverPath;

    void Start()
    {
    }

    void Update()
    {
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene(mainMenuPath);
    }

    public void GotoGame()
    {
        SceneManager.LoadScene(gamePath);
    }

    public void GotoGameOver()
    {
        SceneManager.LoadScene(gameOverPath);
    }
}
