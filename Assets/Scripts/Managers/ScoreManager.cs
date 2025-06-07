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

    public GameObject endScreenPanel;
    public TextMeshProUGUI requiredScore;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI victoryMessage;

    public float scoreAnimationDuration = 2f;

    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI speedText;
    
    public TextMeshProUGUI distanceTextFinish;
    public TextMeshProUGUI speedTextFinish;

     [SerializeField] private PlayerMovement player;
    private float traveled;


    void Start()
    {
        
        startZ = player.transform.position.z;
    }

    void Update()
    {
        // Score distance = position actuelle - position de départ
        traveled = player.transform.position.z - startZ;
        distanceScore = Mathf.RoundToInt(traveled); // pour l'affichage uniquement
        
        if (player != null)
        {
            // Distance : arrondie à l'entier
            distanceText.text =distanceScore.ToString();

            // Vitesse actuelle : arrondie à 1 décimale
            float totalSpeed = Mathf.Min( (player.currentSpeed + player.boostSpeed), GameSession.Instance.maxSpeed);
            speedText.text = totalSpeed.ToString("F1");
        }
    }

    // Appelé quand on atteint la ligne d’arrivée
    public void CalculateFinalScore(float playerSpeed)
    {
        finalScore = Mathf.RoundToInt(traveled * playerSpeed);
        
        Debug.Log("SCORE FINAL : " + finalScore);

        bool hasWon = finalScore >= scoreToWin;
        
        ShowEndScreen(finalScore, hasWon);
        distanceTextFinish.text = distanceText.text;
        speedTextFinish.text = speedText.text;
        
        requiredScore.text = "Victoire :" + scoreToWin.ToString();

        if (hasWon)
            HandleWin();
        else
            HandleLose();
    }
    private void HandleWin()
    {
        // Ajouter l’argent au joueur
        //TODO

        // Charger la scène boutique après 1s
        StartCoroutine(LoadSceneAfterDelay("Boutique", 1f));
    }


    private void HandleLose()
    {
        GameSession.Instance.ResetSession();
        StartCoroutine(LoadSceneAfterDelay("MainMenu", 1f));
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