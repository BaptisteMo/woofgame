using UnityEngine;

public class GameSession : MonoBehaviour
{
    
    public static GameSession Instance;

    [Header("Stats")]
    public int playerMoney = 0;
    public float maxSpeed = 20f;             // Maximum forward speed
    public float baseSpeed = 5f;          // Initial speed
    public float accelerationDuration = 15f;

    [Header("Progression")]
    public int currentLevel = 1;
    public string nextSceneName = "";
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
}