using System.Collections;
using UnityEngine;
using TMPro;

public class StartCountdownManager : MonoBehaviour
{
    [SerializeField]private GameObject countdownPanel;
    [SerializeField]private TextMeshProUGUI countdownText;
    private PlayerMovement player;

    [Header("Réglages")]
    public float countdownDelay = 1f;
    public Vector3 popScale = new Vector3(1.5f, 1.5f, 1f);
    public float popDuration = 0.2f;

    void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        StartCoroutine(CountdownRoutine());
    }

  

    IEnumerator CountdownRoutine()
    {
        if (player != null) player.enabled = false;

        if (countdownPanel != null) countdownPanel.SetActive(true);

        string[] steps = { "3", "2", "1", "GO !" };

        foreach (var step in steps)
        {
            if (countdownText != null)
            {
                countdownText.text = step;
                StartCoroutine(PopAnimation(countdownText.transform));
            }

            yield return new WaitForSeconds(countdownDelay);
        }

        if (countdownPanel != null) countdownPanel.SetActive(false);
        if (player != null) player.enabled = true;
    }

    IEnumerator PopAnimation(Transform target)
    {
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = popScale;

        float t = 0f;

        // pop → retour
        while (t < popDuration)
        {
            t += Time.deltaTime;
            float progress = t / popDuration;
            float scaleFactor = Mathf.Lerp(1f, popScale.x, progress);
            target.localScale = Vector3.one * scaleFactor;
            yield return null;
        }

        // retour à la normale
        t = 0f;
        while (t < popDuration)
        {
            t += Time.deltaTime;
            float progress = t / popDuration;
            float scaleFactor = Mathf.Lerp(popScale.x, 1f, progress);
            target.localScale = Vector3.one * scaleFactor;
            yield return null;
        }

        target.localScale = originalScale;
    }
}
