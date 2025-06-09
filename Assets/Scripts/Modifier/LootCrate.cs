using UnityEngine;

public class LootCrate : MonoBehaviour
{
    [Range(0f, 100f)]
    public float dropChance = 50f; // Pourcentage de chance d’obtenir des pièces
    public int coinReward = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tirage aléatoire
            float roll = Random.Range(0f, 100f);
            if (roll <= dropChance)
            {
                GameSession.Instance.playerMoney += coinReward;
                Debug.Log($"💰 Le joueur a obtenu {coinReward} pièces !");
            }
            else
            {
                Debug.Log("❌ Pas de loot cette fois !");
            }

            Destroy(gameObject); // Détruit la caisse après interaction
        }
    }
}