using Assets.Scripts.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private int maxCharges;

    public int MaxCharges
    {
        get { return maxCharges; }
    }

    private int currentCharge = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentCharge = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentCharge);
        if (currentCharge <= 0)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Enemy collidedEnemy = collision.gameObject.GetComponent<Enemy>();
        if (collidedEnemy != null)
        {
            Debug.Log("AAAAH");
            ScoreManager.Instance.ChangeScoreBy(collidedEnemy.ScoreValue);
            collidedEnemy.DestroyShip();

            DecreaseShieldCharge();
        }
    }

    public void IncreaseShieldCharge(int charge = 1)
    {
        if (currentCharge < maxCharges)
            currentCharge += charge;
    }

    public void DecreaseShieldCharge(int charge = 1)
    {
        currentCharge -= charge;
    }
}
