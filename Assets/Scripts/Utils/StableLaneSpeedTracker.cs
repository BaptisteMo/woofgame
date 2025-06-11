using UnityEngine;

public class StableLaneSpeedTracker : MonoBehaviour
{
    private float timer = 0f;
    private int lastLane;
    private float interval;
    private float bonusPercentPerInterval;
    private float accumulatedPercent = 0f;

    private PlayerMovement player;

    private const string sourceId = "StableLaneBonus";

    public void Setup(float intervalSeconds, float percentPerInterval, PlayerMovement movement)
    {
        interval = intervalSeconds;
        bonusPercentPerInterval = percentPerInterval;
        player = movement;

        lastLane = player.currentLane;
        timer = 0f;
    }

    private void Update()
    {
        if (player == null) return;

        if (player.currentLane == lastLane)
        {
            timer += Time.deltaTime;

            if (timer >= interval)
            {
                timer = 0f;
                accumulatedPercent += bonusPercentPerInterval;

                player.AddSpeedModifier(sourceId, accumulatedPercent); // 🧠 mise à jour avec le cumul
                Debug.Log($"⏱️ Bonus stabilité +{accumulatedPercent}% appliqué");
            }
        }
        else
        {
            // 🔄 changement de ligne → on reset
            player.RemoveSpeedModifier(sourceId);
            Debug.Log("🔁 Changement de ligne → bonus stabilité retiré");

            lastLane = player.currentLane;
            timer = 0f;
            accumulatedPercent = 0f;
        }
    }
}