using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    
    public static GameSession Instance;
   
   // public List<BoostData> boostPool;

    public StatUpgradeData currentStatUpThisLevel; // Celle proposée pour ce niveau

    [Header("Stats")]
    public int playerMoney;
    public float maxSpeed = 20f;             // Maximum forward speed
    public float baseSpeed = 5f;          // Initial speed
    public float accelerationDuration = 15f;

    [Header("Progression")]
    public int currentLevel = 1;
    public string nextSceneName = "";
    public string RetryLevel = "";
    [Header("Bonus & Malus")]

    public float wallHitMalus;
    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetSession()
    {
        playerMoney = 0;
        baseSpeed = 5f;
        maxSpeed = 20f;
        currentLevel = 1;
    }
    

    public void ApplyStatUpgrade(StatUpgradeData stat)
    {
        maxSpeed += stat.speedBonus;
        accelerationDuration = Mathf.Max(1f, accelerationDuration - stat.accelerationBonus);
    }


}