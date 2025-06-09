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

    
    
    public string nextLevel;
    private float startZ;
    private float lastZ;
    private float distanceAccumulator = 0f;

    public GameObject endScreenPanel;
    public TextMeshProUGUI requiredScore;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI victoryMessage;

    public float scoreAnimationDuration = 2f;

    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI playerGold;
    public TextMeshProUGUI distanceTextFinish;
    public TextMeshProUGUI speedTextFinish;

     [SerializeField] private PlayerMovement player;
    private float traveled;


    void Start()
    {
        
        startZ = player.transform.position.z;
        lastZ = startZ;
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

            distanceText.text = distanceScore.ToString();
            

            // Vitesse actuelle : arrondie à 1 décimale
            float totalSpeed = player.currentSpeed;
            
            speedText.text = totalSpeed.ToString("F1");
            playerGold.text ="Gold :" + GameSession.Instance.playerMoney.ToString();
        }
        
    }

    // Appelé quand on atteint la ligne d’arrivée
    public void CalculateFinalScore()
    {
        finalScore = Mathf.RoundToInt(distanceScore * player.currentSpeed);
    
        Debug.Log("SCORE FINAL : " + finalScore);

        bool hasWon = finalScore >= scoreToWin;

        ShowEndScreen(finalScore, hasWon);
        distanceTextFinish.text = distanceText.text;
        speedTextFinish.text = player.currentSpeed.ToString("F1");

        requiredScore.text = "Victoire : " + scoreToWin;

        if (hasWon)
            HandleWin();
        else
            HandleLose();
    }

    private void HandleWin()
    {
        // Ajouter l’argent au joueur
        //TODO
        GameSession.Instance.nextSceneName = nextLevel;
        // Charger la scène boutique après 1s
        StartCoroutine(LoadSceneAfterDelay("Boutique", 3f));
    }


    private void HandleLose()
    {
        GameSession.Instance.ResetSession();
        StartCoroutine(LoadSceneAfterDelay("MainMenu", 3f));
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


}