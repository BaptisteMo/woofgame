using UnityEngine;

public class MaxSpeedNoCollisionTracker : MonoBehaviour
{
    private float timeSinceLastCollision = 0f;
    private float requiredTime;
    private float bonusPercent;

    private bool bonusApplied = false;
    private PlayerMovement player;

    private const string sourceId = "NoCollisionSpeedBonus";

    public void Setup(float requiredTime, float bonusPercent)
    {
        this.requiredTime = requiredTime;
        this.bonusPercent = bonusPercent;

        player = GetComponent<PlayerMovement>();
        timeSinceLastCollision = 0f;
    }

    void Update()
    {
        if (player == null) return;

        timeSinceLastCollision += Time.deltaTime;

        if (!bonusApplied && timeSinceLastCollision >= requiredTime)
        {
            player.AddSpeedModifier(sourceId, bonusPercent);
            bonusApplied = true;
            Debug.Log($"🛡️ Bonus sans collision actif : +{bonusPercent}% de vitesse max");
        }
    }

    public void OnWallCollision()
    {
        timeSinceLastCollision = 0f;

        if (bonusApplied)
        {
            player.RemoveSpeedModifier(sourceId);
            bonusApplied = false;
            Debug.Log("🚧 Collision détectée → bonus annulé");
        }
    }
}