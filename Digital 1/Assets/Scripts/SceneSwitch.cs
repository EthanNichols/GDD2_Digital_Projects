using Assets.Scripts.Shared;
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
        ScoreManager.Instance.ResetScore();
    }

    public void GotoGameOver()
    {
		GameObject[] actObj = FindObjectsOfType<GameObject>();
		if (actObj != null)
		{
			for (int i = 0; i < actObj.Length; i++)
			{
				if (actObj[i].name != "Score" && actObj[i].name != "HUD Canvas" && actObj[i].GetComponentInChildren<ScoreManager>() != null && actObj[i].GetComponentInChildren<ScoreManager>() != ScoreManager.Instance)
					Destroy(actObj[i]);
			}
		}
		ShieldPowerUp[] leftOverShields = FindObjectsOfType<ShieldPowerUp>();
		SuperChargePowerup[] leftOverCharges = FindObjectsOfType<SuperChargePowerup>();
		TwinFirePowerup[] leftOverTwins = FindObjectsOfType<TwinFirePowerup>();
		if (leftOverCharges != null)
		{
			for (int i = 0; i < leftOverCharges.Length; i++)
			{
				Destroy(leftOverCharges[i].gameObject);
			}
		}
		if (leftOverShields != null)
		{
			for (int i = 0; i < leftOverShields.Length; i++)
			{
				Destroy(leftOverShields[i].gameObject);
			}
		}
		if (leftOverTwins != null)
		{
			for (int i = 0; i < leftOverTwins.Length; i++)
			{
				Destroy(leftOverTwins[i].gameObject);
			}
		}
		SceneManager.LoadScene(gameOverPath);
    }
}
