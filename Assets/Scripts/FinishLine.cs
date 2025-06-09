using System;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null && scoreManager != null)
            {
                float playerSpeed = Mathf.Min(player.currentSpeed, GameSession.Instance.maxSpeed);
                scoreManager.CalculateFinalScore();
                player.isFinished = true;

                // Tu peux ici désactiver les contrôles et afficher un écran de fin
                Debug.Log("Niveau terminé !");
            }
        }
    }
}