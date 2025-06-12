using UnityEngine;
using System.Collections;

public class PostCollisionAccelerationTracker : MonoBehaviour
{
    private float bonusAmount;
    private float duration;
    private PlayerMovement player;
    private bool isActive = false;

    public void Setup(float bonus, float time, PlayerMovement movement)
    {
        bonusAmount = bonus;
        duration = time;
        player = movement;
    }

    public void OnWallCollision()
    {
        if (!isActive)
        {
            StartCoroutine(TemporaryAcceleration());
        }
    }

    private IEnumerator TemporaryAcceleration()
    {
        isActive = true;
        player.IncreaseAcceleration(bonusAmount);
        Debug.Log($"⚡ Accélération temporaire : +{bonusAmount}");

        yield return new WaitForSeconds(duration);

        player.IncreaseAcceleration(-bonusAmount); // Revenir à l’état initial
        Debug.Log("⏱️ Fin du boost d'accélération");

        isActive = false;
    }
}