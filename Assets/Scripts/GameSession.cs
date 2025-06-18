using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameLevel
{
    Level_1_1,
    Level_2,
    Level_3,
    Level_4,
    Level_5,
    Level_1_V2
}


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
    [Header("Niveaux")]
    public GameLevel currentLevel = GameLevel.Level_1_1;
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
    public bool wallHitReduced = false;
    public float wallHitMalusReduced = 0.75f;
    public float wallHitMalus;
    public int wallHitCount = 0;
    public bool ignoreFirstCollision = false;
    public bool perfectRunMultiplier = false;
    [Header("Fin de run")]
    public bool doubleAccelIfFastEnabled = false;
    public float speedThresholdForDoubleAccel = 0f;

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

    private void Start()
    {
        AutoDetectCurrentLevel();
    }

    void Update()
    {
        
        // 🔁 Redémarrer la scène courante avec la touche R
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadCurrentScene();
            wallHitCount = 0;
        }
        
    }
    public int wallHitCountTrack()
    {
        wallHitCount++;
        return wallHitCount;
    }

    public void ResetSession()
    {
        playerMoney = 0;
        baseSpeed = 5f;
        maxSpeed = 20f;
        accelerationDuration = 15f;
        currentLevel = GameLevel.Level_1_1;
        hasEnded = false;
        leftLaneGoldenCrateBonus = 0f;
        leftLaneNormalCrateBonus = 0f;
        wallHitCount = 0;
        //  BoostManager.Instance.ResetBoosts(); // ✅ Reset des boosts
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
    // Définis ici ta progression linéaire
    private Dictionary<GameLevel, GameLevel> levelProgression = new Dictionary<GameLevel, GameLevel>
    {
        { GameLevel.Level_1_1, GameLevel.Level_2 },
        { GameLevel.Level_2, GameLevel.Level_3 },
        { GameLevel.Level_3, GameLevel.Level_4 },
        
        // etc...
    };

    public string GetSceneName(GameLevel level)
    {
        return level.ToString(); // suppose que la scène s’appelle pareil que l'enum (ex : "Level1")
    }

    public GameLevel GetNextLevel()
    {
        return levelProgression.ContainsKey(currentLevel) ? levelProgression[currentLevel] : currentLevel;
    }

    public void AdvanceToNextLevel()
    {
        currentLevel = GetNextLevel();
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(GetSceneName(currentLevel));
    }
    public void AutoDetectCurrentLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        foreach (GameLevel level in Enum.GetValues(typeof(GameLevel)))
        {
            if (level.ToString() == currentSceneName)
            {
                currentLevel = level;
                Debug.Log($"✅ Niveau détecté automatiquement : {currentLevel}");
                return;
            }
        }

        Debug.LogWarning($"⚠️ Aucun niveau correspondant à la scène '{currentSceneName}' n’a été trouvé dans l'enum GameLevel.");
    }

}