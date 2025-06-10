using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    
    public static GameSession Instance;

    

    private bool hasEnded = false;
    [SerializeField] private string sceneToReset;
    
    [Header("Stats")]
    public int playerMoney;
    public float maxSpeed = 20f;             // Maximum forward speed
    public float baseSpeed = 5f;          // Initial speed
    public float accelerationDuration = 15f;
    public float lastPlayerSpeed = 20f; // mis à jour en temps réel par le player

    [Header("Progression")]
    public int currentLevel = 1;
    public string nextSceneName = "";
    public string RetryLevel = "";
    [Header("Bonus & Malus")] 
    public bool adjacentLootCollectorEnabled = false;
    public bool lootGrantsSpeed = false;
    public float lootSpeedBonusPercent = 10f;
    public float coinDropBonusPercent = 0f;
    public float boostPowerMultiplier = 1f;
    public bool doubleDistanceOnMaxSpeed = false;
    public float leftLaneNormalCrateBonus = 0f;
    public float leftLaneGoldenCrateBonus = 0f;
    public float goldenCrateSpawnerChanceBonus = 0f; 

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

    void Update()
    {
        
        // 🔁 Redémarrer la scène courante avec la touche R
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadCurrentScene();
        }
        
    }
    public void ReloadCurrentScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    public void ResetSession()
    {
        playerMoney = 0;
        baseSpeed = 5f;
        maxSpeed = 20f;
        accelerationDuration = 15f;
        currentLevel = 1;
        hasEnded = false;
        leftLaneGoldenCrateBonus = 0f;
        leftLaneNormalCrateBonus = 0f;
        BoostManager.Instance.ResetBoosts(); // ✅ Reset des boosts
    }

    public void TriggerRunEnd()
    {
        if (hasEnded) return;
        hasEnded = true;

        Debug.Log("☠️ Fin de la run !");
        ResetSession(); // ou autre logique de reset
        SceneManager.LoadScene(sceneToReset); // ou une scène de défaite spécifique
    }
    
    public void ApplyStatUpgrade(StatUpgradeData stat)
    {
        maxSpeed += stat.speedBonus;
        accelerationDuration = Mathf.Max(1f, accelerationDuration - stat.accelerationBonus);
    }


}