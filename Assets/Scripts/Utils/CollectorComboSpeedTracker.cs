using UnityEngine;

public class CollectorComboSpeedTracker : MonoBehaviour
{
    private const string boostId = "ComboBoost"; // identifiant unique pour ce boost

    private int collectedCount = 0;
    private float bonusPercentPerCombo;
    private float currentBonusPercent = 0f;

    private PlayerMovement player;

    /// <summary>
    /// Initialisation du boost
    /// </summary>
    public void Setup(float bonusPercent, PlayerMovement movement)
    {
        bonusPercentPerCombo = bonusPercent;
        player = movement;
    }

    /// <summary>
    /// À appeler lorsqu'un bonus est ramassé (caisse ou speed)
    /// </summary>
    public void OnCollectablePicked()
    {
        collectedCount++;

        if (collectedCount >= 3)
        {
            collectedCount = 0;
            currentBonusPercent += bonusPercentPerCombo;

            player.AddSpeedModifier(boostId, currentBonusPercent);

            Debug.Log($"🔥 Combo x3 → +{currentBonusPercent}% maxSpeed (source : {boostId})");
        }
    }

    /// <summary>
    /// À appeler lorsqu'on touche un mur
    /// </summary>
    public void OnObstacleHit()
    {
        collectedCount = 0;
        currentBonusPercent = 0f;

        player.RemoveSpeedModifier(boostId);

        Debug.Log("❌ Obstacle touché → Reset du bonus de combo");
    }
}