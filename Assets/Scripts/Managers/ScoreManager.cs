using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization; // si tu utilises TextMeshPro

public class ScoreManager : MonoBehaviour
{
    
     // Le joueur à suivre
    public float distanceScore = 0f;
    public float finalScore = 0f;
    public int scoreToWin = 1500;

    
    
    
    private float startZ;
    private float lastZ;
    private float distanceAccumulator = 0f;

    public GameObject endScreenPanel;
    
    public TextMeshProUGUI requiredScore;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI victoryMessage;

    public float scoreAnimationDuration = 2f;

    public TextMeshProUGUI distnanceOnLevel;
    public TextMeshProUGUI speedOnLevel;
    public TextMeshProUGUI playerGold;
    public TextMeshProUGUI distanceFinishScreen;
    public TextMeshProUGUI speedFinishScreen;

    public TextMeshProUGUI goldBaseGain;
    public TextMeshProUGUI goldInterest;
    public TextMeshProUGUI goldTotalGain;

    private GameObject continueButtonObj;
    private GameObject retryButtonObj;

     [SerializeField] private PlayerMovement player;
    private float traveled;


    void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        endScreenPanel = FindInactiveObjectByName("FinishScreen");
        startZ = player.transform.position.z;
        lastZ = startZ;
        
        distnanceOnLevel = FindTextByName("DistanceOnLevel");
        speedOnLevel = FindTextByName("SpeedOnLevel");
        playerGold = FindTextByName("PlayerGold");
        
        var endScreen = transform.Find("UI_Level/FinishScreen");
        goldBaseGain = FindTextByName("GoldBaseGain");
        goldInterest = FindTextByName("GoldInterest");
        goldTotalGain = FindTextByName("GoldTotalGain");


        distanceFinishScreen = FindTextByName("DistanceFinishScreen");
        speedFinishScreen = FindTextByName("SpeedFinishScreen");
        resultText = FindTextByName("FinalResult");
        requiredScore = FindTextByName("Requis");
        victoryMessage = FindTextByName("Victorytext");

        continueButtonObj = FindInactiveObjectByName("ContinueButton");
        retryButtonObj = FindInactiveObjectByName("RetryButton");

// On s'assure qu’ils sont désactivés au lancement
        continueButtonObj.SetActive(false);
        retryButtonObj.SetActive(false);

    }

    void Update()
    {
        
        if (player != null)
        {
            float currentZ = player.transform.position.z;
            float deltaZ = currentZ - lastZ;

            if (deltaZ > 0f)
            {
                int multiplier = player.CurrentSpeedIsMax() && GameSession.Instance.doubleDistanceOnMaxSpeed ? 2 : 1;

                distanceAccumulator += deltaZ * multiplier;

                // Si on atteint au moins 1 point de distance
                if (distanceAccumulator >= 1f)
                {
                    int gained = Mathf.FloorToInt(distanceAccumulator);
                    distanceScore += gained;
                    distanceAccumulator -= gained;
                }
            }

            lastZ = currentZ;

            distnanceOnLevel.text = distanceScore.ToString();
            

            // Vitesse actuelle : arrondie à 1 décimale
            float totalSpeed = player.currentSpeed;
            
            speedOnLevel.text = totalSpeed.ToString("F1");
            playerGold.text ="Gold :" + GameSession.Instance.playerMoney.ToString();
        }
        
    }

    private TextMeshProUGUI FindTextByName(string name)
    {
        var texts = GameObject.FindObjectsOfType<TextMeshProUGUI>(true); // true = inclut les désactivés
        foreach (var txt in texts)
        {
            if (txt.name == name)
                return txt;
        }

        Debug.LogWarning($"Text UI '{name}' non trouvé dans la scène");
        return null;
    }


    // Appelé quand on atteint la ligne d’arrivée
    public void CalculateFinalScore()
    {
        float speedUsed = player.currentSpeed;
        float finalDistance = distanceScore;

        if (GameSession.Instance.perfectRunMultiplier && GameSession.Instance.wallHitCount == 0)
        {
            Debug.Log("Perfect run, aucune collision détectée");
            finalDistance *= 2f;
        }
        if (GameSession.Instance.doubleAccelIfFastEnabled &&
            speedUsed > GameSession.Instance.speedThresholdForDoubleAccel)
        {
            Debug.Log($"⚡ Boost appliqué : vitesse finale > seuil ({speedUsed:F1} > {GameSession.Instance.speedThresholdForDoubleAccel})");
            speedUsed *= 2f;
        }

        finalScore = Mathf.RoundToInt(finalDistance * speedUsed);

    
        Debug.Log("SCORE FINAL : " + finalScore);

        bool hasWon = finalScore >= scoreToWin;

        ShowEndScreen(finalScore, hasWon);
        goldBaseGain.text = 0.ToString();
        goldInterest.text = 0.ToString();
        goldTotalGain.text = 0.ToString();
        distanceFinishScreen.text = finalDistance.ToString("F1");
        speedFinishScreen.text = speedUsed.ToString("F1");

        requiredScore.text = "Victoire : " + scoreToWin;

        if (hasWon)
            HandleWin();
        else
            HandleLose();
    }

    private void HandleWin()
    {
        int baseReward = GameSession.Instance.winReward;

        int goldBefore = GameSession.Instance.playerMoney;
        int interest = Mathf.FloorToInt(goldBefore / (float)GameSession.Instance.interestDivider);
        interest = Mathf.Min(interest, GameSession.Instance.maxInterestReward);

        int totalEarned = baseReward + interest;

        GameSession.Instance.playerMoney += totalEarned;

        Debug.Log($"🏆 Réussite ! Gain : +{baseReward}g | Intérêts : +{interest}g => Total : +{totalEarned}g");

        // Affiche l’écran de victoire + animation
        ShowGoldGain(baseReward, interest, totalEarned);

        GameSession.Instance.AdvanceToNextLevel();
        
        // 🔘 Activer le bouton continuer
        continueButtonObj.SetActive(true);
        continueButtonObj.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        continueButtonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Boutique");
        });
    }
    
    private void ShowGoldGain(int baseReward, int interest, int total)
    {
        
        StartCoroutine(AnimateGoldGainSequence(baseReward, interest, total));
    }
    private IEnumerator AnimateGoldGainSequence(int baseReward, int interest, int total)
    {
        yield return StartCoroutine(AnimateGoldGain(goldBaseGain, baseReward, 1));
        if (interest > 0)
        {
         yield return StartCoroutine(AnimateGoldGain(goldInterest, interest, 1));
        }
        yield return StartCoroutine(AnimateGoldGain(goldTotalGain, total, 1));
    }


    private void HandleLose()
    {
        GameSession.Instance.ResetSession();

        // 🔘 Activer le bouton recommencer
        retryButtonObj.SetActive(true);
        retryButtonObj.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        retryButtonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        });
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    
    public void ShowEndScreen(float finalScore, bool hasWon)
    {
        endScreenPanel.SetActive(true);
        StartCoroutine(AnimateScore(finalScore, hasWon));
    }
    
    private IEnumerator AnimateScore(float finalScore, bool hasWon)
    {
        float elapsed = 0f;
        int displayedScore = 0;

        while (elapsed < scoreAnimationDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / scoreAnimationDuration);
            displayedScore = Mathf.RoundToInt(Mathf.Lerp(0, finalScore, progress));
            resultText.text =displayedScore.ToString();
            yield return null;
        }

        resultText.text = finalScore.ToString();

        yield return new WaitForSeconds(0.5f);

        victoryMessage.text = hasWon ? "Victoire !" : "Défaite...";
    }
    
    private IEnumerator AnimateGoldGain(TextMeshProUGUI label, int finalValue, int AnimDuration)
    {
        float elapsed = 0f;
        int displayedValue = 0;

        while (elapsed < AnimDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / AnimDuration);
            displayedValue = Mathf.RoundToInt(Mathf.Lerp(0, finalValue, t));
            label.text = $"+{displayedValue}g";
            yield return null;
        }

        label.text = $"+{finalValue}g";
    }

    public static GameObject FindInactiveObjectByName(string name)
    {
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.name == name && !obj.hideFlags.HasFlag(HideFlags.NotEditable) && !obj.hideFlags.HasFlag(HideFlags.HideAndDontSave))
            {
                return obj;
            }
        }

        return null;
    }


}