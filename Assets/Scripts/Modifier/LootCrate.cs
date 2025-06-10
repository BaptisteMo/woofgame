using UnityEngine;

public class LootCrate : MonoBehaviour
{
    [Range(0f, 100f)]
    public float baseDropChance = 50f;
    public int coinReward = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float bonus = GameSession.Instance.coinDropBonusPercent;
            float finalChance = baseDropChance + bonus;
            float roll = Random.Range(0f, 100f);
            
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (roll <= finalChance)
            {
                GameSession.Instance.playerMoney += coinReward;
                Debug.Log($"💰 Caisse lootée ! ({finalChance}%)");

                if (GameSession.Instance.lootGrantsSpeed && player != null)
                {
                    float speedBonus = player.currentSpeed * (GameSession.Instance.lootSpeedBonusPercent / 100f);
                    player.BoostSpeed(speedBonus);
                    Debug.Log($"🚀 Bonus de vitesse appliqué : +{speedBonus} !");
                }
            }
            else
            {
                Debug.Log($"❌ Rien obtenu ({finalChance}%)");
            }

            Destroy(gameObject);
        }
    }
}